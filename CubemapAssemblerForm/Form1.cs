using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.Windows.Forms;
using Image = SixLabors.ImageSharp.Image;
using Point = SixLabors.ImageSharp.Point;
using Rectangle = SixLabors.ImageSharp.Rectangle;
using Size = SixLabors.ImageSharp.Size;

namespace CubemapAssemblerForm
{
    public partial class MainWindow : Form
    {
        public enum CubemapFace
        {
            PosX = 0,
            Right = 0,
            NegX = 1,
            Left = 1,
            PosY = 2,
            Up = 2,
            NegY = 3,
            Down = 3,
            PosZ = 4,
            Front = 4,
            NegZ = 5,
            Back = 5,
            Num = 6
        }

        public enum CubemapOrientation
        {
            Vulkan,
            Ccf
        }

        public enum MapType
        {
            Height,
            Normal,
            Color,
            BiomeId,
            BiomeControl
        }

        public enum FaceResolution
        {
            Res1024 = 1024,
            Res2048 = 2048,
            Res4096 = 4096,
            Res8192 = 8192,
            Res16384 = 16384
        }
        enum EdgeRotation
        {
            None,
            Rotate90CW,
            Rotate90CCW,
            Rotate180,
            FlipX,
            FlipY
        }
        struct FaceEdge
        {
            public CubemapFace Face;
            public EdgeRotation Rotation;
        }

        static readonly FaceEdge[,] FaceAdjacency = new FaceEdge[6, 4]
        {
            // Face 0 = +X
            {
                new FaceEdge{ Face = CubemapFace.PosZ, Rotation = EdgeRotation.None },        // Left
                new FaceEdge{ Face = CubemapFace.NegZ, Rotation = EdgeRotation.None },        // Right
                new FaceEdge{ Face = CubemapFace.PosY, Rotation = EdgeRotation.Rotate90CW },  // Top
                new FaceEdge{ Face = CubemapFace.NegY, Rotation = EdgeRotation.Rotate90CCW }, // Bottom
            },
            // Face 1 = -X
            {
                new FaceEdge{ Face = CubemapFace.NegZ, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.PosZ, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.PosY, Rotation = EdgeRotation.Rotate90CCW },
                new FaceEdge{ Face = CubemapFace.NegY, Rotation = EdgeRotation.Rotate90CW },
            },
            // Face 2 = +Y
            {
                new FaceEdge{ Face = CubemapFace.NegX, Rotation = EdgeRotation.Rotate90CW },
                new FaceEdge{ Face = CubemapFace.PosX, Rotation = EdgeRotation.Rotate90CCW },
                new FaceEdge{ Face = CubemapFace.NegZ, Rotation = EdgeRotation.Rotate180 },
                new FaceEdge{ Face = CubemapFace.PosZ, Rotation = EdgeRotation.None },
            },
            // Face 3 = -Y
            {
                new FaceEdge{ Face = CubemapFace.NegX, Rotation = EdgeRotation.Rotate90CCW },
                new FaceEdge{ Face = CubemapFace.PosX, Rotation = EdgeRotation.Rotate90CW },
                new FaceEdge{ Face = CubemapFace.PosZ, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.NegZ, Rotation = EdgeRotation.Rotate180 },
            },
            // Face 4 = +Z
            {
                new FaceEdge{ Face = CubemapFace.NegX, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.PosX, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.PosY, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.NegY, Rotation = EdgeRotation.None },
            },
            // Face 5 = -Z
            {
                new FaceEdge{ Face = CubemapFace.PosX, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.NegX, Rotation = EdgeRotation.None },
                new FaceEdge{ Face = CubemapFace.PosY, Rotation = EdgeRotation.Rotate180 },
                new FaceEdge{ Face = CubemapFace.NegY, Rotation = EdgeRotation.Rotate180 },
            },
        };


        Image?[] cubemapFaces = new Image[(int)CubemapFace.Num];
        Image?[] outCubemapFaces = new Image[(int)CubemapFace.Num];
        public CubemapOrientation sourceOrientation = CubemapOrientation.Vulkan;
        public CubemapOrientation destOrientation = CubemapOrientation.Ccf;
        public FaceResolution destFaceResolution = FaceResolution.Res4096;

        // Only used for equirect import atm
        public bool nearestNeighbourFiltering = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public Image? TryLoadImage(string path, out CubemapFace face)
        {
            if (path.ToLower().Contains("posx") || path.ToLower().Contains("right"))
            {
                face = CubemapFace.PosX;
                return Image.Load(path);
            }
            else if (path.ToLower().Contains("negx") || path.ToLower().Contains("left"))
            {
                face = CubemapFace.NegX;
                return Image.Load(path);
            }
            else if (path.ToLower().Contains("posy") || path.ToLower().Contains("up"))
            {
                face = CubemapFace.PosY;
                return Image.Load(path);
            }
            else if (path.ToLower().Contains("negy") || path.ToLower().Contains("down"))
            {
                face = CubemapFace.NegY;
                return Image.Load(path);
            }
            else if (path.ToLower().Contains("posz") || path.ToLower().Contains("front"))
            {
                face = CubemapFace.PosZ;
                return Image.Load(path);
            }
            else if (path.ToLower().Contains("negz") || path.ToLower().Contains("back"))
            {
                face = CubemapFace.NegZ;
                return Image.Load(path);
            }
            else
            {
                MessageBox.Show($"Could not determine cubemap face for file: {path}. Please ensure the face orientation is included in the file name.\nValid names are:" +
                    $"\nPosX, NegX, PosY, NegY, PosZ, NegZ\nRight, Left, Up, Down, Front, Back");
            }
            face = CubemapFace.Num;
            return null;
        }

        private void equirectangularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEquirectangular();
        }

        private void cubemapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCubemap();
        }

        private void openMeshToolstripMenu_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Object Files|*.obj";
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select Mesh File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Dispose existing
                    for (int i = 0; i < cubemapFaces.Length; i++)
                    {
                        cubemapFaces[i]?.Dispose();
                        cubemapFaces[i] = null;
                    }

                    string selectedFile = openFileDialog.FileName;

                    Image<L16> image = GenerateHeightmapParallel(selectedFile, 16384, 8192);
                    ConvertEquirectangularToCubemap(image, false);
                    PopulateImageBoxes();
                }
            }
        }

        private void OpenCubemap()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tif";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Image Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Dispose existing
                    for (int i = 0; i < cubemapFaces.Length; i++)
                    {
                        cubemapFaces[i]?.Dispose();
                        cubemapFaces[i] = null;
                    }

                    string[] selectedFiles = openFileDialog.FileNames;
                    Parallel.For(0, selectedFiles.Length, i =>
                    {
                        Image? image = TryLoadImage(selectedFiles[i], out var face);
                        if (image != null && face != CubemapFace.Num)
                        {
                            cubemapFaces[(int)face] = image;
                        }
                    });

                    if (!cubemapFaces.Any(img => img == null))
                    {
                        ConvertToBasisOrientation();
                    }

                    PopulateImageBoxes();
                }
            }
        }

        private void OpenEquirectangular()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tif";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Image Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Dispose existing
                    for (int i = 0; i < cubemapFaces.Length; i++)
                    {
                        cubemapFaces[i]?.Dispose();
                        cubemapFaces[i] = null;
                    }

                    string selectedFile = openFileDialog.FileName;
                    Image? image = Image.Load(selectedFile);

                    if (image.Width != image.Height * 2)
                    {
                        MessageBox.Show("Equirectangular image must have a 2:1 aspect ratio.");
                        return;
                    }

                    //if (image.PixelType)

                    MessageBox.Show("Pixel format: " + image.PixelType.ToString() + ", BPP: " + image.PixelType.BitsPerPixel);

                    ConvertEquirectangularToCubemap(image, nearestNeighbourFiltering);

                    PopulateImageBoxes();
                }
            }
        }

        // Helper method to convert ImageSharp Image to System.Drawing.Bitmap
        private static System.Drawing.Bitmap? ImageSharpToBitmapThumbnail(
            Image? image,
            int width,
            int height)
        {
            if (image == null)
                return null;

            // Clone and resize without touching the original
            using Image clone = image.Clone(ctx =>
                ctx.Resize(new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(width, height),
                    Mode = ResizeMode.Max,   // keeps aspect ratio
                    Sampler = KnownResamplers.Bicubic
                })
            );

            using var ms = new MemoryStream();
            clone.SaveAsPng(ms);
            ms.Position = 0;

            return new System.Drawing.Bitmap(ms);
        }

        public void PopulateImageBoxes()
        {
            pictureBoxPosX.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.PosX],
                pictureBoxPosX.Width,
                pictureBoxPosX.Height);

            pictureBoxNegX.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.NegX],
                pictureBoxNegX.Width,
                pictureBoxNegX.Height);

            pictureBoxPosY.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.PosY],
                pictureBoxPosY.Width,
                pictureBoxPosY.Height);

            pictureBoxNegY.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.NegY],
                pictureBoxNegY.Width,
                pictureBoxNegY.Height);

            pictureBoxPosZ.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.PosZ],
                pictureBoxPosZ.Width,
                pictureBoxPosZ.Height);

            pictureBoxNegZ.Image = ImageSharpToBitmapThumbnail(
                cubemapFaces[(int)CubemapFace.NegZ],
                pictureBoxNegZ.Width,
                pictureBoxNegZ.Height);
        }

        public void ConvertToBasisOrientation()
        {
            // Basis orientation is Vulkan
            if (sourceOrientation == CubemapOrientation.Vulkan)
                return;

            if (sourceOrientation == CubemapOrientation.Ccf)
            {
                Image?[] newFaces = new Image[(int)CubemapFace.Num];

                newFaces[(int)CubemapFace.PosZ] = Rotate(cubemapFaces[(int)CubemapFace.PosX]!, 90);
                newFaces[(int)CubemapFace.NegZ] = Rotate(cubemapFaces[(int)CubemapFace.NegX]!, 270);
                newFaces[(int)CubemapFace.PosY] = Rotate(cubemapFaces[(int)CubemapFace.PosZ]!, 90);
                newFaces[(int)CubemapFace.NegY] = Rotate(cubemapFaces[(int)CubemapFace.NegZ]!, 90);
                newFaces[(int)CubemapFace.PosX] = Rotate(cubemapFaces[(int)CubemapFace.PosY]!, 180);
                newFaces[(int)CubemapFace.NegX] = Rotate(cubemapFaces[(int)CubemapFace.NegY]!, 0);

                // Dispose old images
                for (int i = 0; i < cubemapFaces.Length; i++)
                    cubemapFaces[i]?.Dispose();

                cubemapFaces = newFaces;
            }
        }

        public void ConvertBasisToDestOrientation()
        {
            if (destOrientation == CubemapOrientation.Vulkan)
            {
                return;
            }

            if (destOrientation == CubemapOrientation.Ccf)
            {
                Image?[] newFaces = new Image[(int)CubemapFace.Num];

                newFaces[(int)CubemapFace.PosX] = Rotate(cubemapFaces[(int)CubemapFace.PosZ]!, 270);
                newFaces[(int)CubemapFace.NegX] = Rotate(cubemapFaces[(int)CubemapFace.NegZ]!, 90);
                newFaces[(int)CubemapFace.PosZ] = Rotate(cubemapFaces[(int)CubemapFace.PosY]!, 270);
                newFaces[(int)CubemapFace.NegZ] = Rotate(cubemapFaces[(int)CubemapFace.NegY]!, 270);
                newFaces[(int)CubemapFace.PosY] = Rotate(cubemapFaces[(int)CubemapFace.PosX]!, 180);
                newFaces[(int)CubemapFace.NegY] = Rotate(cubemapFaces[(int)CubemapFace.NegX]!, 0);

                outCubemapFaces = newFaces;
            }
        }
        private Image Rotate(Image src, float degrees)
        {
            return src.Clone(ctx => ctx.Rotate(degrees));
        }

        private void orientationSourceVulkanCheckbox_Click(object sender, EventArgs e)
        {
            sourceOrientation = CubemapOrientation.Vulkan;
            orientationSourceCcfCheckbox.Checked = false;
            orientationSourceVulkanCheckbox.Checked = true;
        }

        private void orientationSourceCcfCheckbox_Click(object sender, EventArgs e)
        {
            sourceOrientation = CubemapOrientation.Ccf;
            orientationSourceVulkanCheckbox.Checked = false;
            orientationSourceCcfCheckbox.Checked = true;
        }

        private void orientationDestVulkanCheckbox_Click(object sender, EventArgs e)
        {
            destOrientation = CubemapOrientation.Vulkan;
            orientationDestCcfCheckbox.Checked = false;
            orientationDestVulkanCheckbox.Checked = true;
        }

        private void orientationDestCcfCheckbox_Click(object sender, EventArgs e)
        {
            destOrientation = CubemapOrientation.Ccf;
            orientationDestVulkanCheckbox.Checked = false;
            orientationDestCcfCheckbox.Checked = true;
        }

        private void exportHeightMapMenuStrip_Click(object sender, EventArgs e)
        {
            ExportMap(MapType.Height);
        }

        private void exportNormalMapMenuStrip_Click(object sender, EventArgs e)
        {
            ExportMap(MapType.Normal);
        }

        private void exportColorMapMenuStrip_Click(object sender, EventArgs e)
        {
            ExportMap(MapType.Color);
        }

        private void exportBiomeIdMapMenuStrip_Click(object sender, EventArgs e)
        {
            ExportMap(MapType.BiomeId);
        }

        private void exportBiomeControlManMenuStrip_Click(object sender, EventArgs e)
        {
            ExportMap(MapType.BiomeControl);
        }

        public void ExportMap(MapType mapType)
        {
            // Dispose old images
            for (int i = 0; i < outCubemapFaces.Length; i++)
                outCubemapFaces[i]?.Dispose();

            string outputKtxPath;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Khronos KTX2|*.ktx2";
                saveFileDialog.Title = "Export Cubemap KTX2";
                saveFileDialog.FileName = mapType + "_Cubemap.ktx2";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                outputKtxPath = saveFileDialog.FileName;
            }

            // Temp directory
            string tempDir = Path.Combine(Path.GetTempPath(), "CubemapAssembler", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            ConvertBasisToDestOrientation();

            try
            {
                // Save faces to temp
                string prefix = Path.GetFileNameWithoutExtension(outputKtxPath);

                string[] facePaths = new string[(int)CubemapFace.Num];

                Parallel.For(0, (int)CubemapFace.Num, i =>
                {
                    string faceName = ((CubemapFace)i).ToString();
                    string facePath = Path.Combine(tempDir, $"{prefix}_{faceName}.png");

                    Image? imageToSave = outCubemapFaces[i];
                    imageToSave?.Save(facePath);

                    facePaths[i] = facePath;
                });

                foreach (var path in facePaths)
                {
                    if (!File.Exists(path))
                        throw new FileNotFoundException("Missing cubemap face", path);
                }

                string tempKtxPath = Path.Combine(tempDir, "intermediate.ktx2");
                RunKtxCreate(tempKtxPath, facePaths, mapType);

                if (mapType == MapType.Height || mapType == MapType.BiomeId || mapType == MapType.BiomeControl)
                {
                    // Height maps are final output already
                    File.Copy(tempKtxPath, outputKtxPath, true);
                }
                else
                {
                    RunKtxTranscode(tempKtxPath, outputKtxPath, mapType);
                }
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);

                    // Dispose out cubemap faces
                    for (int i = 0; i < outCubemapFaces.Length; i++)
                    {
                        outCubemapFaces[i]?.Dispose();
                        outCubemapFaces[i] = null;
                    }
                }
                catch
                {
                }
            }
        }

        public void RunKtxCreate(string tempKtxPath, string[] facePaths, MapType mapType)
        {
            string faceArgs = string.Join(" ", facePaths.Select(p => $"\"{p}\""));
            string args;

            if (mapType == MapType.Height)
            {
                args =
                    "create --cubemap --generate-mipmap --mipmap-wrap clamp " +
                    "--format R16_UNORM " +
                    "--assign-tf linear " +
                    faceArgs + " " +
                    $"\"{tempKtxPath}\"";
            }
            else if (mapType == MapType.Normal)
            {
                args =
                    "create --cubemap --generate-mipmap --mipmap-wrap clamp " +
                    "--format R8G8B8A8_UNORM " +
                    "--encode uastc --uastc-quality 3 --uastc-rdo " +
                    "--normal-mode " +
                    "--assign-tf linear " +
                    faceArgs + " " +
                    $"\"{tempKtxPath}\"";
            }
            else if (mapType == MapType.Color)
            {
                args =
                    "create --cubemap --generate-mipmap --mipmap-wrap clamp " +
                    "--format R8G8B8_SRGB " +
                    "--encode uastc --uastc-quality 3 --uastc-rdo " +
                    "--assign-tf srgb --assign-primaries srgb " +
                    faceArgs + " " +
                    $"\"{tempKtxPath}\"";
            }
            else if (mapType == MapType.BiomeId || mapType == MapType.BiomeControl)
            {
                args =
                    "--t2 --cubemap " +
                    "--encode none --target_type RGBA " +
                    "--assign_oetf linear " + (mapType == MapType.BiomeControl ? "--genmipmap " : "")
                    + $"\"{tempKtxPath}\"" + " " + faceArgs;
            }
            else
            {
                // BC7:
                //args =
                //    "create --cubemap --generate-mipmap --mipmap-wrap clamp " +
                //    "--format R8G8B8A8_UNORM " +
                //    "--encode uastc --uastc-quality 3 --uastc-rdo " +
                //    "--assign-tf linear " +
                //    faceArgs + " " +
                //    $"\"{tempKtxPath}\"";

                args =
                    "create --cubemap --generate-mipmap --mipmap-wrap clamp " +
                    "--format R8G8B8A8_UNORM " +
                    "--assign-tf linear " +
                    faceArgs + " " +
                    $"\"{tempKtxPath}\"";
            }

            if (mapType == MapType.BiomeControl || mapType == MapType.BiomeId)
            {
                RunProcess("toktx", args, "toktx create failed", Path.GetDirectoryName(tempKtxPath)!);
            }
            else
            {
                RunProcess("ktx", args, "ktx create failed", Path.GetDirectoryName(tempKtxPath)!);
            }
        }

        public void RunKtxTranscode(string tempKtxPath, string outputKtxPath, MapType mapType)
        {
            string targetFormat =
                mapType == MapType.Normal ? "bc5" : "bc7";

            string args =
                $"transcode --target {targetFormat} --zstd 20 " +
                $"\"{tempKtxPath}\" \"{outputKtxPath}\"";

            RunProcess("ktx", args, "ktx transcode failed", Path.GetDirectoryName(tempKtxPath)!);

            MessageBox.Show("Export completed.");
        }

        private void RunProcess(string exe, string arguments, string errorTitle, string workingDir)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                WorkingDirectory = workingDir,   // REQUIRED
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo)!;

            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                MessageBox.Show($"{errorTitle}:\n{stderr}");
            }
        }

        public void RunToktx(string outputKtxPath, string[] facePaths, MapType mapType)
        {
            string toktxExe = "toktx";

            string encodeArgs;

            if (mapType == MapType.Normal)
            {
                encodeArgs =
                    "--t2 --cubemap --genmipmap " +
                    "--encode uastc --normal_mode " +
                    "--assign_oetf linear --assign_primaries none ";
            }
            else if (mapType == MapType.Height)
            {
                encodeArgs =
                    "--t2 --cubemap --genmipmap " +
                    "--encode none --target_type R " +
                    "--assign_oetf linear ";
            }
            else if (mapType == MapType.BiomeId)
            {
                encodeArgs =
                    "--t2 --cubemap " +
                    "--encode none --target_type RGBA " +
                    "--assign_oetf linear ";
            }
            else if (mapType == MapType.BiomeControl)
            {
                encodeArgs =
                    "--t2 --cubemap --genmipmap " +
                    "--encode none --target_type RGBA " +
                    "--assign_oetf linear ";
            }
            else // Color
            {
                encodeArgs =
                    "--t2 --cubemap --genmipmap " +
                    "--encode uastc " +
                    "--assign_oetf srgb --assign_primaries srgb ";
            }

            string arguments = encodeArgs + $"\"{outputKtxPath}\" " + string.Join(" ", facePaths.Select(p => $"\"{p}\""));

            var startInfo = new ProcessStartInfo
            {
                FileName = toktxExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo)!;

            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                MessageBox.Show("Toktx command failed:\n" + stderr);
            }
            else
            {
                MessageBox.Show("Export completed.");
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            // Available area (inside the form)
            int availableWidth = ClientSize.Width;
            int availableHeight = ClientSize.Height - menuStrip1.Height - bottomPanel.Height;

            // Determine maximum size while keeping 4:3 aspect ratio
            int widthBasedOnHeight = availableHeight * 4 / 3;
            int heightBasedOnWidth = availableWidth * 3 / 4;

            int panelWidth, panelHeight;

            if (widthBasedOnHeight <= availableWidth)
            {
                // Height-limited
                panelWidth = widthBasedOnHeight;
                panelHeight = availableHeight;
            }
            else
            {
                // Width-limited
                panelWidth = availableWidth;
                panelHeight = heightBasedOnWidth;
            }

            mainPanel.Width = panelWidth;
            mainPanel.Height = panelHeight;

            // Center the panel in available area
            mainPanel.Left = (availableWidth - panelWidth) / 2;
            mainPanel.Top = menuStrip1.Bottom + (availableHeight - panelHeight) / 2;

            bottomPanel.Top = ClientSize.Height - bottomPanel.Height;
            bottomPanel.Width = panelWidth;
            bottomPanel.Left = mainPanel.Left;
        }

        private void exportProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void nearestNeighbourFilteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nearestNeighbourFiltering = !nearestNeighbourFiltering;
            nearestNeighbourFilteringToolStripMenuItem.Checked = nearestNeighbourFiltering;
        }

        private void destFaceRes1024_Click(object sender, EventArgs e)
        {
            destFaceResolution = FaceResolution.Res1024;
            destFaceRes1024.Checked = true;

            // Uncheck others
            destFaceRes2048.Checked = false;
            destFaceRes4096.Checked = false;
            destFaceRes8192.Checked = false;
            destFaceRes16384.Checked = false;
        }

        private void destFaceRes2048_Click(object sender, EventArgs e)
        {
            destFaceResolution = FaceResolution.Res2048;
            destFaceRes2048.Checked = true;

            // Uncheck others
            destFaceRes1024.Checked = false;
            destFaceRes4096.Checked = false;
            destFaceRes8192.Checked = false;
            destFaceRes16384.Checked = false;
        }

        private void destFaceRes4096_Click(object sender, EventArgs e)
        {
            destFaceResolution = FaceResolution.Res4096;
            destFaceRes4096.Checked = true;

            // Uncheck others
            destFaceRes1024.Checked = false;
            destFaceRes2048.Checked = false;
            destFaceRes8192.Checked = false;
            destFaceRes16384.Checked = false;
        }

        private void destFaceRes8192_Click(object sender, EventArgs e)
        {
            destFaceResolution = FaceResolution.Res8192;
            destFaceRes8192.Checked = true;

            // Uncheck others
            destFaceRes1024.Checked = false;
            destFaceRes2048.Checked = false;
            destFaceRes4096.Checked = false;
            destFaceRes16384.Checked = false;
        }

        private void destFaceRes16384_Click(object sender, EventArgs e)
        {
            destFaceResolution = FaceResolution.Res16384;
            destFaceRes16384.Checked = true;

            // Uncheck others
            destFaceRes1024.Checked = false;
            destFaceRes2048.Checked = false;
            destFaceRes4096.Checked = false;
            destFaceRes8192.Checked = false;
        }

        
    }
}
