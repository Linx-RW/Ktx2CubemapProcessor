using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Numerics;
using Image = SixLabors.ImageSharp.Image;

namespace CubemapAssemblerForm
{
    public partial class MainWindow
    {
        public void ConvertEquirectangularToCubemap(Image image, bool useNearestNeighbor = false)
        {
            if (image.Width != 2 * image.Height)
            {
                throw new ArgumentException("Input image must have 2:1 aspect ratio (equirectangular)");
            }

            // Calculate cubemap face size (typical convention uses face height = 1/4 of equirect height)
            int faceSize = image.Height / 2;

            // Dispose existing
            for (int i = 0; i < cubemapFaces.Length; i++)
            {
                cubemapFaces[i]?.Dispose();
                cubemapFaces[i] = null;
            }

            if (image is Image<L16> l16Image)
            {
                ProcessEquirectWithFormat<L16>(l16Image, faceSize, useNearestNeighbor);
            }
            else if (image is Image<Rgba32> rgbaImage)
            {
                ProcessEquirectWithFormat<Rgba32>(rgbaImage, faceSize, useNearestNeighbor);
            }
            else if (image is Image<Rgb24> rgb24Image)
            {
                ProcessEquirectWithFormat<Rgb24>(rgb24Image, faceSize, useNearestNeighbor);
            }
            else if (image is Image<L8> l8Image)
            {
                ProcessEquirectWithFormat<L8>(l8Image, faceSize, useNearestNeighbor);
            }
            else if (image is Image<La16> la16Image)
            {
                ProcessEquirectWithFormat<La16>(la16Image, faceSize, useNearestNeighbor);
            }
            else if (image is Image<La32> la32Image)
            {
                ProcessEquirectWithFormat<La32>(la32Image, faceSize, useNearestNeighbor);
            }
            else
            {
                // Unsupported, fall back to RGBA32
                using var converted = image.CloneAs<Rgba32>();
                ProcessEquirectWithFormat<Rgba32>(converted, faceSize, useNearestNeighbor);
            }
        }

        private int GetUniqueColourCount<TPixel>(Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel>
        {
            var uniqueColours = new HashSet<TPixel>();
            for (int y = 0; y < image.Height; y++)
            {
                Span<TPixel> rowSpan = image.DangerousGetPixelRowMemory(y).Span;
                for (int x = 0; x < image.Width; x++)
                {
                    uniqueColours.Add(rowSpan[x]);
                }
            }
            return uniqueColours.Count;
        }

        private void ProcessEquirectWithFormat<TPixel>(Image<TPixel> equirectImage, int faceSize, bool useNearestNeighbor)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // Get the pixel type info for debugging
            var pixelType = equirectImage.PixelType;

            // Process each face with the exact same pixel format
            Parallel.For(0, (int)CubemapFace.Num, i =>
            {
                cubemapFaces[i] = ProcessCubemapFaceWithFormat<TPixel>(
                    equirectImage, (CubemapFace)i, faceSize, useNearestNeighbor);
            });
        }

        private Image ProcessCubemapFaceWithFormat<TPixel>(
            Image<TPixel> equirectImage,
            CubemapFace face,
            int faceSize,
            bool useNearestNeighbor)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var faceImage = new Image<TPixel>(faceSize, faceSize);

            Parallel.For(0, faceSize, y =>
            {
                Span<TPixel> rowSpan = faceImage.DangerousGetPixelRowMemory(y).Span;

                for (int x = 0; x < faceSize; x++)
                {
                    Vector3 direction = GetDirectionFromCubeMap(face, x, y, faceSize);
                    (float u, float v) = DirectionToEquirectUV(direction);

                    // Apply orientation correction
                    if (face != CubemapFace.Up && face != CubemapFace.Down)
                    {
                        u = 1f - u;
                        u += 0.25f;
                    }
                    else
                    {
                        u += 0.25f;
                    }

                    TPixel color = SampleEquirectangular(equirectImage, u, v, useNearestNeighbor);
                    rowSpan[x] = color;
                }
            });

            return faceImage;
        }

        private Vector3 GetDirectionFromCubeMap(CubemapFace face, int x, int y, int faceSize)
        {
            float u = (2.0f * (x + 0.5f) / faceSize) - 1.0f;
            float v = (2.0f * (y + 0.5f) / faceSize) - 1.0f;

            switch (face)
            {
                case CubemapFace.Right:
                    return Vector3.Normalize(new Vector3(1.0f, -v, -u));
                case CubemapFace.Left:
                    return Vector3.Normalize(new Vector3(-1.0f, -v, u));
                case CubemapFace.Up:
                    return Vector3.Normalize(new Vector3(u, 1.0f, -v));
                case CubemapFace.Down:
                    return Vector3.Normalize(new Vector3(u, -1.0f, v));
                case CubemapFace.Front:
                    return Vector3.Normalize(new Vector3(u, -v, 1.0f));
                case CubemapFace.Back:
                    return Vector3.Normalize(new Vector3(-u, -v, -1.0f));
                default:
                    throw new ArgumentException("Invalid cubemap face");
            }
        }

        private (float u, float v) DirectionToEquirectUV(Vector3 direction)
        {
            direction = Vector3.Normalize(direction);

            float longitude = MathF.Atan2(direction.Z, direction.X);
            float u = (longitude + MathF.PI) / (2.0f * MathF.PI);

            float latitude = MathF.Asin(direction.Y);
            float v = 0.5f - latitude / MathF.PI;

            return (u, v);
        }

        private TPixel SampleEquirectangular<TPixel>(Image<TPixel> image, float u, float v, bool useNearestNeighbor)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // [0, 1]
            u = u % 1.0f;
            if (u < 0) u += 1.0f;
            v = Math.Clamp(v, 0.0f, 1.0f);

            // Convert to pixel coordinates
            float px = u * image.Width;
            float py = v * image.Height;

            if (useNearestNeighbor)
            {
                int ix = (int)MathF.Floor(px);
                int iy = (int)MathF.Floor(py);

                ix = ix % image.Width;
                if (ix < 0) ix += image.Width;

                iy = Math.Clamp(iy, 0, image.Height - 1);

                return image[ix, iy];
            }
            else
            {
                // Bilinear interpolation
                float x = px - 0.5f;
                float y = py - 0.5f;

                int x0 = (int)MathF.Floor(x);
                int y0 = (int)MathF.Floor(y);
                int x1 = x0 + 1;
                int y1 = y0 + 1;

                float fx = x - x0;
                float fy = y - y0;

                x0 = x0 % image.Width;
                if (x0 < 0) x0 += image.Width;
                x1 = x1 % image.Width;
                if (x1 < 0) x1 += image.Width;

                y0 = Math.Clamp(y0, 0, image.Height - 1);
                y1 = Math.Clamp(y1, 0, image.Height - 1);

                // Get the four neighboring pixels
                TPixel c00 = image[x0, y0];
                TPixel c10 = image[x1, y0];
                TPixel c01 = image[x0, y1];
                TPixel c11 = image[x1, y1];

                // Use generic bilinear interpolation
                return Bilinear(c00, c10, c01, c11, fx, fy);
            }
        }

        static TPixel Bilinear<TPixel>(
            TPixel c00, TPixel c10, TPixel c01, TPixel c11,
            float fx, float fy)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            Vector4 v00 = c00.ToVector4();
            Vector4 v10 = c10.ToVector4();
            Vector4 v01 = c01.ToVector4();
            Vector4 v11 = c11.ToVector4();

            Vector4 vx0 = Vector4.Lerp(v00, v10, fx);
            Vector4 vx1 = Vector4.Lerp(v01, v11, fx);
            Vector4 vxy = Vector4.Lerp(vx0, vx1, fy);

            TPixel result = default;
            result.FromVector4(vxy);
            return result;
        }

        private Rgba32 InterpolatePixels(Rgba32 c00, Rgba32 c10, Rgba32 c01, Rgba32 c11, float fx, float fy)
        {
            byte r = (byte)(c00.R * (1 - fx) * (1 - fy) + c10.R * fx * (1 - fy) +
                           c01.R * (1 - fx) * fy + c11.R * fx * fy);
            byte g = (byte)(c00.G * (1 - fx) * (1 - fy) + c10.G * fx * (1 - fy) +
                           c01.G * (1 - fx) * fy + c11.G * fx * fy);
            byte b = (byte)(c00.B * (1 - fx) * (1 - fy) + c10.B * fx * (1 - fy) +
                           c01.B * (1 - fx) * fy + c11.B * fx * fy);
            byte a = (byte)(c00.A * (1 - fx) * (1 - fy) + c10.A * fx * (1 - fy) +
                           c01.A * (1 - fx) * fy + c11.A * fx * fy);

            return new Rgba32(r, g, b, a);
        }

        private L16 InterpolatePixels(L16 c00, L16 c10, L16 c01, L16 c11, float fx, float fy)
        {
            float p00 = c00.PackedValue / 65535.0f;
            float p10 = c10.PackedValue / 65535.0f;
            float p01 = c01.PackedValue / 65535.0f;
            float p11 = c11.PackedValue / 65535.0f;

            float result = p00 * (1 - fx) * (1 - fy) + p10 * fx * (1 - fy) +
                          p01 * (1 - fx) * fy + p11 * fx * fy;

            return new L16((ushort)(result * 65535.0f));
        }

        private TPixel InterpolatePixels<TPixel>(TPixel c00, TPixel c10, TPixel c01, TPixel c11, float fx, float fy)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // Generic fallback using Vector4
            Vector4 v00 = c00.ToVector4();
            Vector4 v10 = c10.ToVector4();
            Vector4 v01 = c01.ToVector4();
            Vector4 v11 = c11.ToVector4();

            Vector4 result = v00 * (1 - fx) * (1 - fy) + v10 * fx * (1 - fy) +
                            v01 * (1 - fx) * fy + v11 * fx * fy;

            TPixel pixel = default;
            pixel.FromVector4(result);
            return pixel;
        }
    }
}