namespace TextComparer
{
    partial class TextComparer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextComparer));
            this.textBoxUncorrected = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSourceNoWords = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelDestinationNoWords = new System.Windows.Forms.Label();
            this.textBoxCorrected = new System.Windows.Forms.TextBox();
            this.labelNumberOfDifferences = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonDiffEngine = new System.Windows.Forms.RadioButton();
            this.radioButtonWordSeqAligner = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxUncorrected
            // 
            this.textBoxUncorrected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUncorrected.Location = new System.Drawing.Point(6, 19);
            this.textBoxUncorrected.Multiline = true;
            this.textBoxUncorrected.Name = "textBoxUncorrected";
            this.textBoxUncorrected.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUncorrected.Size = new System.Drawing.Size(749, 182);
            this.textBoxUncorrected.TabIndex = 0;
            this.textBoxUncorrected.TextChanged += new System.EventHandler(this.textBoxSource_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelSourceNoWords);
            this.groupBox1.Controls.Add(this.textBoxUncorrected);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 224);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paste uncorrected text here:";
            // 
            // labelSourceNoWords
            // 
            this.labelSourceNoWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSourceNoWords.AutoSize = true;
            this.labelSourceNoWords.Location = new System.Drawing.Point(6, 204);
            this.labelSourceNoWords.Name = "labelSourceNoWords";
            this.labelSourceNoWords.Size = new System.Drawing.Size(50, 13);
            this.labelSourceNoWords.TabIndex = 1;
            this.labelSourceNoWords.Text = "Words: 0";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.labelDestinationNoWords);
            this.groupBox2.Controls.Add(this.textBoxCorrected);
            this.groupBox2.Location = new System.Drawing.Point(12, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(761, 256);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Paste corrected (100% accurate) text here:";
            // 
            // labelDestinationNoWords
            // 
            this.labelDestinationNoWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDestinationNoWords.AutoSize = true;
            this.labelDestinationNoWords.Location = new System.Drawing.Point(6, 236);
            this.labelDestinationNoWords.Name = "labelDestinationNoWords";
            this.labelDestinationNoWords.Size = new System.Drawing.Size(50, 13);
            this.labelDestinationNoWords.TabIndex = 1;
            this.labelDestinationNoWords.Text = "Words: 0";
            // 
            // textBoxCorrected
            // 
            this.textBoxCorrected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCorrected.Location = new System.Drawing.Point(9, 19);
            this.textBoxCorrected.Multiline = true;
            this.textBoxCorrected.Name = "textBoxCorrected";
            this.textBoxCorrected.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCorrected.Size = new System.Drawing.Size(746, 214);
            this.textBoxCorrected.TabIndex = 0;
            this.textBoxCorrected.TextChanged += new System.EventHandler(this.textBoxDestination_TextChanged);
            // 
            // labelNumberOfDifferences
            // 
            this.labelNumberOfDifferences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNumberOfDifferences.AutoSize = true;
            this.labelNumberOfDifferences.Location = new System.Drawing.Point(93, 504);
            this.labelNumberOfDifferences.Name = "labelNumberOfDifferences";
            this.labelNumberOfDifferences.Size = new System.Drawing.Size(236, 39);
            this.labelNumberOfDifferences.TabIndex = 3;
            this.labelNumberOfDifferences.Text = "Number of deletions: , additions: , replacements: \r\nPercentage of new words:\r\nPro" +
    "cessing took n/a s\r\n";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 504);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Compare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonCompare_Click);
            // 
            // radioButtonDiffEngine
            // 
            this.radioButtonDiffEngine.AutoSize = true;
            this.radioButtonDiffEngine.Checked = true;
            this.radioButtonDiffEngine.Location = new System.Drawing.Point(658, 507);
            this.radioButtonDiffEngine.Name = "radioButtonDiffEngine";
            this.radioButtonDiffEngine.Size = new System.Drawing.Size(109, 17);
            this.radioButtonDiffEngine.TabIndex = 5;
            this.radioButtonDiffEngine.TabStop = true;
            this.radioButtonDiffEngine.Text = "Diff Engine (John)";
            this.radioButtonDiffEngine.UseVisualStyleBackColor = true;
            // 
            // radioButtonWordSeqAligner
            // 
            this.radioButtonWordSeqAligner.AutoSize = true;
            this.radioButtonWordSeqAligner.Location = new System.Drawing.Point(658, 526);
            this.radioButtonWordSeqAligner.Name = "radioButtonWordSeqAligner";
            this.radioButtonWordSeqAligner.Size = new System.Drawing.Size(95, 17);
            this.radioButtonWordSeqAligner.TabIndex = 6;
            this.radioButtonWordSeqAligner.Text = "NIST algorithm";
            this.radioButtonWordSeqAligner.UseVisualStyleBackColor = true;
            // 
            // TextComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.radioButtonWordSeqAligner);
            this.Controls.Add(this.radioButtonDiffEngine);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelNumberOfDifferences);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TextComparer";
            this.Text = "Text comparison tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxUncorrected;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSourceNoWords;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelDestinationNoWords;
        private System.Windows.Forms.TextBox textBoxCorrected;
        private System.Windows.Forms.Label labelNumberOfDifferences;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonDiffEngine;
        private System.Windows.Forms.RadioButton radioButtonWordSeqAligner;
    }
}

