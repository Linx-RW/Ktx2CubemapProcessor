namespace CubemapAssemblerForm
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            equirectangularToolStripMenuItem = new ToolStripMenuItem();
            cubemapToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exportHeightMapMenuStrip = new ToolStripMenuItem();
            exportNormalMapMenuStrip = new ToolStripMenuItem();
            exportColorMapMenuStrip = new ToolStripMenuItem();
            exportBiomeIdMapMenuStrip = new ToolStripMenuItem();
            exportBiomeControlManMenuStrip = new ToolStripMenuItem();
            orientationToolStripMenuItem = new ToolStripMenuItem();
            equirectImportOptionsToolStripMenuItem = new ToolStripMenuItem();
            nearestNeighbourFilteringToolStripMenuItem = new ToolStripMenuItem();
            sourceOrientationToolStripMenuItem = new ToolStripMenuItem();
            orientationSourceVulkanCheckbox = new ToolStripMenuItem();
            orientationSourceCcfCheckbox = new ToolStripMenuItem();
            destinationOrientationToolStripMenuItem = new ToolStripMenuItem();
            orientationDestVulkanCheckbox = new ToolStripMenuItem();
            orientationDestCcfCheckbox = new ToolStripMenuItem();
            destinationResolutionToolStripMenuItem = new ToolStripMenuItem();
            destFaceRes1024 = new ToolStripMenuItem();
            destFaceRes2048 = new ToolStripMenuItem();
            destFaceRes4096 = new ToolStripMenuItem();
            destFaceRes8192 = new ToolStripMenuItem();
            destFaceRes16384 = new ToolStripMenuItem();
            pictureBoxNegX = new PictureBox();
            pictureBoxPosZ = new PictureBox();
            pictureBoxPosX = new PictureBox();
            pictureBoxNegZ = new PictureBox();
            pictureBoxPosY = new PictureBox();
            pictureBoxNegY = new PictureBox();
            mainPanel = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            bottomPanel = new Panel();
            exportProgressBar = new ProgressBar();
            openMeshToolstripMenu = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegY).BeginInit();
            mainPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, orientationToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1307, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { equirectangularToolStripMenuItem, cubemapToolStripMenuItem, openMeshToolstripMenu });
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(224, 26);
            openToolStripMenuItem.Text = "Open";
            // 
            // equirectangularToolStripMenuItem
            // 
            equirectangularToolStripMenuItem.Name = "equirectangularToolStripMenuItem";
            equirectangularToolStripMenuItem.Size = new Size(228, 26);
            equirectangularToolStripMenuItem.Text = "Equirectangular";
            equirectangularToolStripMenuItem.Click += equirectangularToolStripMenuItem_Click;
            // 
            // cubemapToolStripMenuItem
            // 
            cubemapToolStripMenuItem.Name = "cubemapToolStripMenuItem";
            cubemapToolStripMenuItem.Size = new Size(228, 26);
            cubemapToolStripMenuItem.Text = "Cubemap";
            cubemapToolStripMenuItem.Click += cubemapToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportHeightMapMenuStrip, exportNormalMapMenuStrip, exportColorMapMenuStrip, exportBiomeIdMapMenuStrip, exportBiomeControlManMenuStrip });
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(224, 26);
            saveToolStripMenuItem.Text = "Export";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // exportHeightMapMenuStrip
            // 
            exportHeightMapMenuStrip.Name = "exportHeightMapMenuStrip";
            exportHeightMapMenuStrip.Size = new Size(222, 26);
            exportHeightMapMenuStrip.Text = "Height Map";
            exportHeightMapMenuStrip.Click += exportHeightMapMenuStrip_Click;
            // 
            // exportNormalMapMenuStrip
            // 
            exportNormalMapMenuStrip.Name = "exportNormalMapMenuStrip";
            exportNormalMapMenuStrip.Size = new Size(222, 26);
            exportNormalMapMenuStrip.Text = "Normal Map";
            exportNormalMapMenuStrip.Click += exportNormalMapMenuStrip_Click;
            // 
            // exportColorMapMenuStrip
            // 
            exportColorMapMenuStrip.Name = "exportColorMapMenuStrip";
            exportColorMapMenuStrip.Size = new Size(222, 26);
            exportColorMapMenuStrip.Text = "Color Map";
            exportColorMapMenuStrip.Click += exportColorMapMenuStrip_Click;
            // 
            // exportBiomeIdMapMenuStrip
            // 
            exportBiomeIdMapMenuStrip.Name = "exportBiomeIdMapMenuStrip";
            exportBiomeIdMapMenuStrip.Size = new Size(222, 26);
            exportBiomeIdMapMenuStrip.Text = "Biome ID Map";
            exportBiomeIdMapMenuStrip.Click += exportBiomeIdMapMenuStrip_Click;
            // 
            // exportBiomeControlManMenuStrip
            // 
            exportBiomeControlManMenuStrip.Name = "exportBiomeControlManMenuStrip";
            exportBiomeControlManMenuStrip.Size = new Size(222, 26);
            exportBiomeControlManMenuStrip.Text = "Biome Control Map";
            exportBiomeControlManMenuStrip.Click += exportBiomeControlManMenuStrip_Click;
            // 
            // orientationToolStripMenuItem
            // 
            orientationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { equirectImportOptionsToolStripMenuItem, sourceOrientationToolStripMenuItem, destinationOrientationToolStripMenuItem, destinationResolutionToolStripMenuItem });
            orientationToolStripMenuItem.Name = "orientationToolStripMenuItem";
            orientationToolStripMenuItem.Size = new Size(75, 24);
            orientationToolStripMenuItem.Text = "Options";
            // 
            // equirectImportOptionsToolStripMenuItem
            // 
            equirectImportOptionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nearestNeighbourFilteringToolStripMenuItem });
            equirectImportOptionsToolStripMenuItem.Name = "equirectImportOptionsToolStripMenuItem";
            equirectImportOptionsToolStripMenuItem.Size = new Size(254, 26);
            equirectImportOptionsToolStripMenuItem.Text = "Equirect. Import Options";
            // 
            // nearestNeighbourFilteringToolStripMenuItem
            // 
            nearestNeighbourFilteringToolStripMenuItem.Name = "nearestNeighbourFilteringToolStripMenuItem";
            nearestNeighbourFilteringToolStripMenuItem.Size = new Size(276, 26);
            nearestNeighbourFilteringToolStripMenuItem.Text = "Nearest Neighbour Filtering";
            nearestNeighbourFilteringToolStripMenuItem.Click += nearestNeighbourFilteringToolStripMenuItem_Click;
            // 
            // sourceOrientationToolStripMenuItem
            // 
            sourceOrientationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { orientationSourceVulkanCheckbox, orientationSourceCcfCheckbox });
            sourceOrientationToolStripMenuItem.Name = "sourceOrientationToolStripMenuItem";
            sourceOrientationToolStripMenuItem.Size = new Size(254, 26);
            sourceOrientationToolStripMenuItem.Text = "Source Orientation";
            // 
            // orientationSourceVulkanCheckbox
            // 
            orientationSourceVulkanCheckbox.Checked = true;
            orientationSourceVulkanCheckbox.CheckState = CheckState.Checked;
            orientationSourceVulkanCheckbox.Name = "orientationSourceVulkanCheckbox";
            orientationSourceVulkanCheckbox.Size = new Size(203, 26);
            orientationSourceVulkanCheckbox.Text = "Vulkan / OpenGL";
            orientationSourceVulkanCheckbox.Click += orientationSourceVulkanCheckbox_Click;
            // 
            // orientationSourceCcfCheckbox
            // 
            orientationSourceCcfCheckbox.Name = "orientationSourceCcfCheckbox";
            orientationSourceCcfCheckbox.Size = new Size(203, 26);
            orientationSourceCcfCheckbox.Text = "KSA CCF";
            orientationSourceCcfCheckbox.Click += orientationSourceCcfCheckbox_Click;
            // 
            // destinationOrientationToolStripMenuItem
            // 
            destinationOrientationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { orientationDestVulkanCheckbox, orientationDestCcfCheckbox });
            destinationOrientationToolStripMenuItem.Name = "destinationOrientationToolStripMenuItem";
            destinationOrientationToolStripMenuItem.Size = new Size(254, 26);
            destinationOrientationToolStripMenuItem.Text = "Destination Orientation";
            // 
            // orientationDestVulkanCheckbox
            // 
            orientationDestVulkanCheckbox.Name = "orientationDestVulkanCheckbox";
            orientationDestVulkanCheckbox.Size = new Size(203, 26);
            orientationDestVulkanCheckbox.Text = "Vulkan / OpenGL";
            orientationDestVulkanCheckbox.Click += orientationDestVulkanCheckbox_Click;
            // 
            // orientationDestCcfCheckbox
            // 
            orientationDestCcfCheckbox.Checked = true;
            orientationDestCcfCheckbox.CheckState = CheckState.Checked;
            orientationDestCcfCheckbox.Name = "orientationDestCcfCheckbox";
            orientationDestCcfCheckbox.Size = new Size(203, 26);
            orientationDestCcfCheckbox.Text = "KSA CCF";
            orientationDestCcfCheckbox.Click += orientationDestCcfCheckbox_Click;
            // 
            // destinationResolutionToolStripMenuItem
            // 
            destinationResolutionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { destFaceRes1024, destFaceRes2048, destFaceRes4096, destFaceRes8192, destFaceRes16384 });
            destinationResolutionToolStripMenuItem.Name = "destinationResolutionToolStripMenuItem";
            destinationResolutionToolStripMenuItem.Size = new Size(254, 26);
            destinationResolutionToolStripMenuItem.Text = "Destination Resolution";
            // 
            // destFaceRes1024
            // 
            destFaceRes1024.Name = "destFaceRes1024";
            destFaceRes1024.Size = new Size(132, 26);
            destFaceRes1024.Text = "1024";
            destFaceRes1024.Click += destFaceRes1024_Click;
            // 
            // destFaceRes2048
            // 
            destFaceRes2048.Name = "destFaceRes2048";
            destFaceRes2048.Size = new Size(132, 26);
            destFaceRes2048.Text = "2048";
            destFaceRes2048.Click += destFaceRes2048_Click;
            // 
            // destFaceRes4096
            // 
            destFaceRes4096.Checked = true;
            destFaceRes4096.CheckState = CheckState.Checked;
            destFaceRes4096.Name = "destFaceRes4096";
            destFaceRes4096.Size = new Size(132, 26);
            destFaceRes4096.Text = "4096";
            destFaceRes4096.Click += destFaceRes4096_Click;
            // 
            // destFaceRes8192
            // 
            destFaceRes8192.Name = "destFaceRes8192";
            destFaceRes8192.Size = new Size(132, 26);
            destFaceRes8192.Text = "8192";
            destFaceRes8192.Click += destFaceRes8192_Click;
            // 
            // destFaceRes16384
            // 
            destFaceRes16384.Name = "destFaceRes16384";
            destFaceRes16384.Size = new Size(132, 26);
            destFaceRes16384.Text = "16384";
            destFaceRes16384.Click += destFaceRes16384_Click;
            // 
            // pictureBoxNegX
            // 
            pictureBoxNegX.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxNegX.Dock = DockStyle.Fill;
            pictureBoxNegX.Location = new Point(0, 243);
            pictureBoxNegX.Margin = new Padding(0);
            pictureBoxNegX.Name = "pictureBoxNegX";
            pictureBoxNegX.Size = new Size(317, 243);
            pictureBoxNegX.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxNegX.TabIndex = 1;
            pictureBoxNegX.TabStop = false;
            // 
            // pictureBoxPosZ
            // 
            pictureBoxPosZ.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxPosZ.Dock = DockStyle.Fill;
            pictureBoxPosZ.Location = new Point(317, 243);
            pictureBoxPosZ.Margin = new Padding(0);
            pictureBoxPosZ.Name = "pictureBoxPosZ";
            pictureBoxPosZ.Size = new Size(317, 243);
            pictureBoxPosZ.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPosZ.TabIndex = 2;
            pictureBoxPosZ.TabStop = false;
            // 
            // pictureBoxPosX
            // 
            pictureBoxPosX.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxPosX.Dock = DockStyle.Fill;
            pictureBoxPosX.Location = new Point(634, 243);
            pictureBoxPosX.Margin = new Padding(0);
            pictureBoxPosX.Name = "pictureBoxPosX";
            pictureBoxPosX.Size = new Size(317, 243);
            pictureBoxPosX.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPosX.TabIndex = 3;
            pictureBoxPosX.TabStop = false;
            // 
            // pictureBoxNegZ
            // 
            pictureBoxNegZ.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxNegZ.Dock = DockStyle.Fill;
            pictureBoxNegZ.Location = new Point(951, 243);
            pictureBoxNegZ.Margin = new Padding(0);
            pictureBoxNegZ.Name = "pictureBoxNegZ";
            pictureBoxNegZ.Size = new Size(320, 243);
            pictureBoxNegZ.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxNegZ.TabIndex = 4;
            pictureBoxNegZ.TabStop = false;
            // 
            // pictureBoxPosY
            // 
            pictureBoxPosY.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxPosY.Dock = DockStyle.Fill;
            pictureBoxPosY.Location = new Point(317, 0);
            pictureBoxPosY.Margin = new Padding(0);
            pictureBoxPosY.Name = "pictureBoxPosY";
            pictureBoxPosY.Size = new Size(317, 243);
            pictureBoxPosY.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPosY.TabIndex = 5;
            pictureBoxPosY.TabStop = false;
            // 
            // pictureBoxNegY
            // 
            pictureBoxNegY.BackColor = Color.FromArgb(45, 45, 45);
            pictureBoxNegY.Dock = DockStyle.Fill;
            pictureBoxNegY.Location = new Point(317, 486);
            pictureBoxNegY.Margin = new Padding(0);
            pictureBoxNegY.Name = "pictureBoxNegY";
            pictureBoxNegY.Size = new Size(317, 246);
            pictureBoxNegY.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxNegY.TabIndex = 6;
            pictureBoxNegY.TabStop = false;
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(tableLayoutPanel1);
            mainPanel.Location = new Point(24, 44);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1271, 732);
            mainPanel.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(pictureBoxPosY, 1, 0);
            tableLayoutPanel1.Controls.Add(pictureBoxNegX, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxPosZ, 1, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxPosX, 2, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxNegZ, 3, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxNegY, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
            tableLayoutPanel1.Size = new Size(1271, 732);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(31, 31, 31);
            bottomPanel.Controls.Add(exportProgressBar);
            bottomPanel.ForeColor = SystemColors.ActiveBorder;
            bottomPanel.Location = new Point(24, 783);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(1271, 28);
            bottomPanel.TabIndex = 8;
            // 
            // exportProgressBar
            // 
            exportProgressBar.Dock = DockStyle.Fill;
            exportProgressBar.Location = new Point(0, 0);
            exportProgressBar.Name = "exportProgressBar";
            exportProgressBar.Size = new Size(1271, 28);
            exportProgressBar.TabIndex = 0;
            exportProgressBar.Click += exportProgressBar_Click;
            // 
            // openMeshToolstripMenu
            // 
            openMeshToolstripMenu.Name = "openMeshToolstripMenu";
            openMeshToolstripMenu.Size = new Size(228, 26);
            openMeshToolstripMenu.Text = "Mesh (Experimental)";
            openMeshToolstripMenu.Click += openMeshToolstripMenu_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 31, 31);
            ClientSize = new Size(1307, 817);
            Controls.Add(bottomPanel);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            Text = "Cubemap Assembler";
            Load += Form1_Load;
            Resize += MainWindow_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegX).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosX).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPosY).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNegY).EndInit();
            mainPanel.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem orientationToolStripMenuItem;
        private ToolStripMenuItem sourceOrientationToolStripMenuItem;
        private ToolStripMenuItem orientationSourceVulkanCheckbox;
        private ToolStripMenuItem orientationSourceCcfCheckbox;
        private ToolStripMenuItem destinationOrientationToolStripMenuItem;
        private ToolStripMenuItem orientationDestVulkanCheckbox;
        private ToolStripMenuItem orientationDestCcfCheckbox;
        private ToolStripMenuItem exportHeightMapMenuStrip;
        private ToolStripMenuItem exportNormalMapMenuStrip;
        private ToolStripMenuItem exportColorMapMenuStrip;
        private ToolStripMenuItem exportBiomeIdMapMenuStrip;
        private ToolStripMenuItem exportBiomeControlManMenuStrip;
        private PictureBox pictureBoxNegX;
        private PictureBox pictureBoxPosZ;
        private PictureBox pictureBoxPosX;
        private PictureBox pictureBoxNegZ;
        private PictureBox pictureBoxPosY;
        private PictureBox pictureBoxNegY;
        private Panel mainPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel bottomPanel;
        private ProgressBar exportProgressBar;
        private ToolStripMenuItem equirectangularToolStripMenuItem;
        private ToolStripMenuItem cubemapToolStripMenuItem;
        private ToolStripMenuItem equirectImportOptionsToolStripMenuItem;
        private ToolStripMenuItem nearestNeighbourFilteringToolStripMenuItem;
        private ToolStripMenuItem destinationResolutionToolStripMenuItem;
        private ToolStripMenuItem destFaceRes1024;
        private ToolStripMenuItem destFaceRes2048;
        private ToolStripMenuItem destFaceRes4096;
        private ToolStripMenuItem destFaceRes8192;
        private ToolStripMenuItem destFaceRes16384;
        private ToolStripMenuItem openMeshToolstripMenu;
    }
}
