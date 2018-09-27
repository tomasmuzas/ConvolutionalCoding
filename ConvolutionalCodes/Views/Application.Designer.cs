namespace ConvolutionalCodes
{
    partial class Application
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
            this.enterTextLabel = new System.Windows.Forms.Label();
            this.TextInput = new System.Windows.Forms.TextBox();
            this.selectImageLabel = new System.Windows.Forms.Label();
            this.uploadImageButton = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.TextSubmit = new System.Windows.Forms.Button();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.channelNoiseLabel = new System.Windows.Forms.Label();
            this.ChannelNoiseInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.encodingResultPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // enterTextLabel
            // 
            this.enterTextLabel.AutoSize = true;
            this.enterTextLabel.Location = new System.Drawing.Point(12, 113);
            this.enterTextLabel.Name = "enterTextLabel";
            this.enterTextLabel.Size = new System.Drawing.Size(139, 17);
            this.enterTextLabel.TabIndex = 0;
            this.enterTextLabel.Text = "Enter text to encode:";
            // 
            // TextInput
            // 
            this.TextInput.Location = new System.Drawing.Point(15, 134);
            this.TextInput.Name = "TextInput";
            this.TextInput.Size = new System.Drawing.Size(197, 22);
            this.TextInput.TabIndex = 1;
            // 
            // selectImageLabel
            // 
            this.selectImageLabel.AutoSize = true;
            this.selectImageLabel.Location = new System.Drawing.Point(12, 215);
            this.selectImageLabel.Name = "selectImageLabel";
            this.selectImageLabel.Size = new System.Drawing.Size(160, 17);
            this.selectImageLabel.TabIndex = 2;
            this.selectImageLabel.Text = "Select Image to encode:";
            // 
            // uploadImageButton
            // 
            this.uploadImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uploadImageButton.Location = new System.Drawing.Point(12, 236);
            this.uploadImageButton.Name = "uploadImageButton";
            this.uploadImageButton.Size = new System.Drawing.Size(123, 35);
            this.uploadImageButton.TabIndex = 3;
            this.uploadImageButton.Text = "Upload Image";
            this.uploadImageButton.UseVisualStyleBackColor = true;
            this.uploadImageButton.Click += new System.EventHandler(this.uploadImageButton_Click);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(293, 22);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(52, 17);
            this.resultLabel.TabIndex = 4;
            this.resultLabel.Text = "Result:";
            // 
            // TextSubmit
            // 
            this.TextSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TextSubmit.Location = new System.Drawing.Point(15, 163);
            this.TextSubmit.Name = "TextSubmit";
            this.TextSubmit.Size = new System.Drawing.Size(75, 35);
            this.TextSubmit.TabIndex = 5;
            this.TextSubmit.Text = "Submit";
            this.TextSubmit.UseVisualStyleBackColor = true;
            this.TextSubmit.Click += new System.EventHandler(this.TextSubmit_Click);
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLabel.Location = new System.Drawing.Point(12, 22);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(67, 17);
            this.settingsLabel.TabIndex = 6;
            this.settingsLabel.Text = "Settings";
            // 
            // channelNoiseLabel
            // 
            this.channelNoiseLabel.AutoSize = true;
            this.channelNoiseLabel.Location = new System.Drawing.Point(15, 43);
            this.channelNoiseLabel.Name = "channelNoiseLabel";
            this.channelNoiseLabel.Size = new System.Drawing.Size(104, 17);
            this.channelNoiseLabel.TabIndex = 7;
            this.channelNoiseLabel.Text = "Channel Noise:";
            // 
            // ChannelNoiseInput
            // 
            this.ChannelNoiseInput.Location = new System.Drawing.Point(126, 43);
            this.ChannelNoiseInput.Name = "ChannelNoiseInput";
            this.ChannelNoiseInput.Size = new System.Drawing.Size(86, 22);
            this.ChannelNoiseInput.TabIndex = 8;
            this.ChannelNoiseInput.Text = "0.01";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Actions";
            // 
            // encodingResultPanel
            // 
            this.encodingResultPanel.AutoScroll = true;
            this.encodingResultPanel.AutoScrollMargin = new System.Drawing.Size(0, 450);
            this.encodingResultPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.encodingResultPanel.Location = new System.Drawing.Point(296, 56);
            this.encodingResultPanel.Name = "encodingResultPanel";
            this.encodingResultPanel.Size = new System.Drawing.Size(820, 450);
            this.encodingResultPanel.TabIndex = 10;
            this.encodingResultPanel.WrapContents = false;
            // 
            // Application
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 527);
            this.Controls.Add(this.encodingResultPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChannelNoiseInput);
            this.Controls.Add(this.channelNoiseLabel);
            this.Controls.Add(this.settingsLabel);
            this.Controls.Add(this.TextSubmit);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.uploadImageButton);
            this.Controls.Add(this.selectImageLabel);
            this.Controls.Add(this.TextInput);
            this.Controls.Add(this.enterTextLabel);
            this.Name = "Application";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Convolutional Encoding";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label enterTextLabel;
        private System.Windows.Forms.TextBox TextInput;
        private System.Windows.Forms.Label selectImageLabel;
        private System.Windows.Forms.Button uploadImageButton;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Button TextSubmit;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label channelNoiseLabel;
        private System.Windows.Forms.TextBox ChannelNoiseInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel encodingResultPanel;
    }
}

