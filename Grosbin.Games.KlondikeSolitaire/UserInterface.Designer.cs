namespace Grosbin.Games.KlondikeSolitaire
{
    partial class UserInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
            uxMenuBar = new ToolStrip();
            uxNew = new ToolStripButton();
            uxGetSeed = new ToolStripButton();
            uxSeed = new ToolStripTextBox();
            uxBoard = new FlowLayoutPanel();
            uxStockFoundationPanel = new FlowLayoutPanel();
            uxTableauPanel = new FlowLayoutPanel();
            uxMenuBar.SuspendLayout();
            uxBoard.SuspendLayout();
            SuspendLayout();
            // 
            // uxMenuBar
            // 
            uxMenuBar.ImageScalingSize = new Size(24, 24);
            uxMenuBar.Items.AddRange(new ToolStripItem[] { uxNew, uxGetSeed, uxSeed });
            uxMenuBar.Location = new Point(0, 0);
            uxMenuBar.Name = "uxMenuBar";
            uxMenuBar.Size = new Size(800, 34);
            uxMenuBar.TabIndex = 0;
            uxMenuBar.Text = "toolStrip1";
            // 
            // uxNew
            // 
            uxNew.DisplayStyle = ToolStripItemDisplayStyle.Text;
            uxNew.Image = (Image)resources.GetObject("uxNew.Image");
            uxNew.ImageTransparentColor = Color.Magenta;
            uxNew.Name = "uxNew";
            uxNew.Size = new Size(102, 29);
            uxNew.Text = "New Game";
            uxNew.Click += NewClick;
            // 
            // uxGetSeed
            // 
            uxGetSeed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            uxGetSeed.Image = (Image)resources.GetObject("uxGetSeed.Image");
            uxGetSeed.ImageTransparentColor = Color.Magenta;
            uxGetSeed.Name = "uxGetSeed";
            uxGetSeed.Size = new Size(59, 29);
            uxGetSeed.Text = "Seed:";
            uxGetSeed.Click += GetSeedClick;
            // 
            // uxSeed
            // 
            uxSeed.Name = "uxSeed";
            uxSeed.ReadOnly = true;
            uxSeed.Size = new Size(100, 34);
            uxSeed.Text = "-1";
            // 
            // uxBoard
            // 
            uxBoard.AutoSize = true;
            uxBoard.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            uxBoard.Controls.Add(uxStockFoundationPanel);
            uxBoard.Controls.Add(uxTableauPanel);
            uxBoard.FlowDirection = FlowDirection.TopDown;
            uxBoard.Location = new Point(12, 37);
            uxBoard.Name = "uxBoard";
            uxBoard.Size = new Size(81, 162);
            uxBoard.TabIndex = 1;
            uxBoard.WrapContents = false;
            // 
            // uxStockFoundationPanel
            // 
            uxStockFoundationPanel.AutoSize = true;
            uxStockFoundationPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            uxStockFoundationPanel.Location = new Point(3, 3);
            uxStockFoundationPanel.MinimumSize = new Size(75, 75);
            uxStockFoundationPanel.Name = "uxStockFoundationPanel";
            uxStockFoundationPanel.Size = new Size(75, 75);
            uxStockFoundationPanel.TabIndex = 2;
            uxStockFoundationPanel.WrapContents = false;
            // 
            // uxTableauPanel
            // 
            uxTableauPanel.AutoSize = true;
            uxTableauPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            uxTableauPanel.Location = new Point(3, 84);
            uxTableauPanel.MinimumSize = new Size(75, 75);
            uxTableauPanel.Name = "uxTableauPanel";
            uxTableauPanel.Size = new Size(75, 75);
            uxTableauPanel.TabIndex = 3;
            uxTableauPanel.WrapContents = false;
            // 
            // UserInterface
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DarkGreen;
            ClientSize = new Size(800, 450);
            Controls.Add(uxBoard);
            Controls.Add(uxMenuBar);
            MaximizeBox = false;
            Name = "UserInterface";
            Text = "Klondike Solitaire";
            uxMenuBar.ResumeLayout(false);
            uxMenuBar.PerformLayout();
            uxBoard.ResumeLayout(false);
            uxBoard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip uxMenuBar;
        private ToolStripButton uxNew;
        private ToolStripButton uxGetSeed;
        private ToolStripTextBox uxSeed;
        private FlowLayoutPanel uxBoard;
        private FlowLayoutPanel uxStockFoundationPanel;
        private FlowLayoutPanel uxTableauPanel;
    }
}
