namespace Grosbin.Games.KlondikeSolitaire
{
    partial class SeedDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            uxMainPanel = new FlowLayoutPanel();
            uxSeed = new NumericUpDown();
            uxOK = new Button();
            uxSeedLabel = new Label();
            uxMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uxSeed).BeginInit();
            SuspendLayout();
            // 
            // uxMainPanel
            // 
            uxMainPanel.AutoSize = true;
            uxMainPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            uxMainPanel.Controls.Add(uxSeedLabel);
            uxMainPanel.Controls.Add(uxSeed);
            uxMainPanel.Controls.Add(uxOK);
            uxMainPanel.FlowDirection = FlowDirection.TopDown;
            uxMainPanel.Location = new Point(12, 12);
            uxMainPanel.Name = "uxMainPanel";
            uxMainPanel.Size = new Size(169, 109);
            uxMainPanel.TabIndex = 0;
            uxMainPanel.WrapContents = false;
            // 
            // uxSeed
            // 
            uxSeed.Location = new Point(3, 28);
            uxSeed.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            uxSeed.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            uxSeed.Name = "uxSeed";
            uxSeed.Size = new Size(163, 31);
            uxSeed.TabIndex = 1;
            uxSeed.ThousandsSeparator = true;
            uxSeed.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // uxOK
            // 
            uxOK.DialogResult = DialogResult.OK;
            uxOK.Location = new Point(3, 65);
            uxOK.Name = "uxOK";
            uxOK.Size = new Size(163, 41);
            uxOK.TabIndex = 1;
            uxOK.Text = "OK";
            uxOK.UseVisualStyleBackColor = true;
            // 
            // uxSeedLabel
            // 
            uxSeedLabel.AutoSize = true;
            uxSeedLabel.Location = new Point(3, 0);
            uxSeedLabel.Name = "uxSeedLabel";
            uxSeedLabel.Size = new Size(55, 25);
            uxSeedLabel.TabIndex = 2;
            uxSeedLabel.Text = "Seed:";
            // 
            // SeedDialog
            // 
            AcceptButton = uxOK;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Controls.Add(uxMainPanel);
            MaximizeBox = false;
            Name = "SeedDialog";
            uxMainPanel.ResumeLayout(false);
            uxMainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)uxSeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel uxMainPanel;
        private NumericUpDown uxSeed;
        private Button uxOK;
        private Label uxSeedLabel;
    }
}