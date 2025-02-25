namespace sa_doc_encrypt.UI.Win
{
    partial class FrmMain
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
            label1 = new Label();
            txtSrcFile = new TextBox();
            btnFindFile = new Button();
            btnProcess = new Button();
            cmbOperation = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            txtOut = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 14);
            label1.Name = "label1";
            label1.Size = new Size(110, 15);
            label1.TabIndex = 0;
            label1.Text = "Archivo de entrada:";
            // 
            // txtSrcFile
            // 
            txtSrcFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSrcFile.Location = new Point(16, 36);
            txtSrcFile.Name = "txtSrcFile";
            txtSrcFile.Size = new Size(704, 23);
            txtSrcFile.TabIndex = 1;
            // 
            // btnFindFile
            // 
            btnFindFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFindFile.Location = new Point(726, 34);
            btnFindFile.Name = "btnFindFile";
            btnFindFile.Size = new Size(39, 26);
            btnFindFile.TabIndex = 2;
            btnFindFile.Text = "...";
            btnFindFile.UseVisualStyleBackColor = true;
            btnFindFile.Click += BtnFindFile_Click;
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(205, 82);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(76, 25);
            btnProcess.TabIndex = 3;
            btnProcess.Text = "Procesar";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += BtnProcess_Click;
            // 
            // cmbOperation
            // 
            cmbOperation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOperation.FormattingEnabled = true;
            cmbOperation.Location = new Point(78, 83);
            cmbOperation.Name = "cmbOperation";
            cmbOperation.Size = new Size(121, 23);
            cmbOperation.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 85);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 5;
            label2.Text = "Operación:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 128);
            label3.Name = "label3";
            label3.Size = new Size(141, 15);
            label3.TabIndex = 6;
            label3.Text = "Ruta de archivo de salida:";
            // 
            // txtOut
            // 
            txtOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOut.Location = new Point(14, 151);
            txtOut.Multiline = true;
            txtOut.Name = "txtOut";
            txtOut.ScrollBars = ScrollBars.Both;
            txtOut.Size = new Size(751, 62);
            txtOut.TabIndex = 7;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(776, 233);
            Controls.Add(txtOut);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cmbOperation);
            Controls.Add(btnProcess);
            Controls.Add(btnFindFile);
            Controls.Add(txtSrcFile);
            Controls.Add(label1);
            Name = "FrmMain";
            Text = "Utilitario de cifrado";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSrcFile;
        private Button btnFindFile;
        private Button btnProcess;
        private ComboBox cmbOperation;
        private Label label2;
        private Label label3;
        private TextBox txtOut;
    }
}
