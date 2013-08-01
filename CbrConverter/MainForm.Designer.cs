namespace CbrConverter
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_SourceFolder = new System.Windows.Forms.CheckBox();
            this.tbox_OuputFolder = new System.Windows.Forms.TextBox();
            this.lbl_about = new System.Windows.Forms.Label();
            this.tbox_SourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StartStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk_JoinImages = new System.Windows.Forms.CheckBox();
            this.chk_NbPages = new System.Windows.Forms.CheckBox();
            this.chk_pdf2cbr = new System.Windows.Forms.CheckBox();
            this.chk_cbr2pdf = new System.Windows.Forms.CheckBox();
            this.chk_ReduceSize = new System.Windows.Forms.CheckBox();
            this.pbar_TotalProgress = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_ProcessingFile = new System.Windows.Forms.Label();
            this.pbar_ActualFile = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.btn_showlog = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_SourceFolder);
            this.groupBox1.Controls.Add(this.tbox_OuputFolder);
            this.groupBox1.Controls.Add(this.lbl_about);
            this.groupBox1.Controls.Add(this.tbox_SourceFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chk_SourceFolder
            // 
            this.chk_SourceFolder.AutoSize = true;
            this.chk_SourceFolder.Checked = true;
            this.chk_SourceFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_SourceFolder.Location = new System.Drawing.Point(8, 55);
            this.chk_SourceFolder.Name = "chk_SourceFolder";
            this.chk_SourceFolder.Size = new System.Drawing.Size(126, 17);
            this.chk_SourceFolder.TabIndex = 7;
            this.chk_SourceFolder.Text = "Copy to source folder";
            this.chk_SourceFolder.UseVisualStyleBackColor = true;
            this.chk_SourceFolder.CheckedChanged += new System.EventHandler(this.chk_SourceFolder_CheckedChanged);
            // 
            // tbox_OuputFolder
            // 
            this.tbox_OuputFolder.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tbox_OuputFolder.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbox_OuputFolder.Enabled = false;
            this.tbox_OuputFolder.Location = new System.Drawing.Point(5, 74);
            this.tbox_OuputFolder.Name = "tbox_OuputFolder";
            this.tbox_OuputFolder.ReadOnly = true;
            this.tbox_OuputFolder.Size = new System.Drawing.Size(475, 20);
            this.tbox_OuputFolder.TabIndex = 6;
            this.tbox_OuputFolder.Text = "Click here to select an ouput folder";
            this.tbox_OuputFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbox_OuputFolder.Click += new System.EventHandler(this.tbox_OuputFolder_Click);
            // 
            // lbl_about
            // 
            this.lbl_about.AutoSize = true;
            this.lbl_about.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_about.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_about.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_about.Location = new System.Drawing.Point(446, 16);
            this.lbl_about.Name = "lbl_about";
            this.lbl_about.Size = new System.Drawing.Size(35, 13);
            this.lbl_about.TabIndex = 5;
            this.lbl_about.Text = "About";
            this.lbl_about.Click += new System.EventHandler(this.lbl_about_Click);
            // 
            // tbox_SourceFile
            // 
            this.tbox_SourceFile.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tbox_SourceFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbox_SourceFile.Location = new System.Drawing.Point(6, 32);
            this.tbox_SourceFile.Name = "tbox_SourceFile";
            this.tbox_SourceFile.ReadOnly = true;
            this.tbox_SourceFile.Size = new System.Drawing.Size(475, 20);
            this.tbox_SourceFile.TabIndex = 5;
            this.tbox_SourceFile.Text = "Click here to select a file or folder";
            this.tbox_SourceFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbox_SourceFile.Click += new System.EventHandler(this.tbox_SourceFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select File or Folder (files will be process recursively)";
            // 
            // btn_StartStop
            // 
            this.btn_StartStop.Location = new System.Drawing.Point(5, 17);
            this.btn_StartStop.Name = "btn_StartStop";
            this.btn_StartStop.Size = new System.Drawing.Size(67, 115);
            this.btn_StartStop.TabIndex = 2;
            this.btn_StartStop.Text = "START";
            this.btn_StartStop.UseVisualStyleBackColor = true;
            this.btn_StartStop.Click += new System.EventHandler(this.btn_StartStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk_JoinImages);
            this.groupBox2.Controls.Add(this.chk_NbPages);
            this.groupBox2.Controls.Add(this.chk_pdf2cbr);
            this.groupBox2.Controls.Add(this.chk_cbr2pdf);
            this.groupBox2.Controls.Add(this.chk_ReduceSize);
            this.groupBox2.Controls.Add(this.pbar_TotalProgress);
            this.groupBox2.Controls.Add(this.btn_StartStop);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lbl_ProcessingFile);
            this.groupBox2.Controls.Add(this.pbar_ActualFile);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(10, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(486, 139);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // chk_JoinImages
            // 
            this.chk_JoinImages.AutoSize = true;
            this.chk_JoinImages.Checked = true;
            this.chk_JoinImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_JoinImages.Location = new System.Drawing.Point(203, 40);
            this.chk_JoinImages.Name = "chk_JoinImages";
            this.chk_JoinImages.Size = new System.Drawing.Size(189, 17);
            this.chk_JoinImages.TabIndex = 9;
            this.chk_JoinImages.Text = "Merge images from same Pdf page";
            this.chk_JoinImages.UseVisualStyleBackColor = true;
            // 
            // chk_NbPages
            // 
            this.chk_NbPages.AutoSize = true;
            this.chk_NbPages.Checked = true;
            this.chk_NbPages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_NbPages.Location = new System.Drawing.Point(425, 17);
            this.chk_NbPages.Name = "chk_NbPages";
            this.chk_NbPages.Size = new System.Drawing.Size(55, 17);
            this.chk_NbPages.TabIndex = 8;
            this.chk_NbPages.Text = "P/Img";
            this.chk_NbPages.UseVisualStyleBackColor = true;
            this.chk_NbPages.Visible = false;
            // 
            // chk_pdf2cbr
            // 
            this.chk_pdf2cbr.AutoSize = true;
            this.chk_pdf2cbr.Checked = true;
            this.chk_pdf2cbr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_pdf2cbr.Location = new System.Drawing.Point(78, 38);
            this.chk_pdf2cbr.Name = "chk_pdf2cbr";
            this.chk_pdf2cbr.Size = new System.Drawing.Size(80, 17);
            this.chk_pdf2cbr.TabIndex = 7;
            this.chk_pdf2cbr.Text = "PDF to Cbz";
            this.chk_pdf2cbr.UseVisualStyleBackColor = true;
            // 
            // chk_cbr2pdf
            // 
            this.chk_cbr2pdf.AutoSize = true;
            this.chk_cbr2pdf.Checked = true;
            this.chk_cbr2pdf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_cbr2pdf.Location = new System.Drawing.Point(78, 17);
            this.chk_cbr2pdf.Name = "chk_cbr2pdf";
            this.chk_cbr2pdf.Size = new System.Drawing.Size(101, 17);
            this.chk_cbr2pdf.TabIndex = 6;
            this.chk_cbr2pdf.Text = "Cbr/Cbz to PDF";
            this.chk_cbr2pdf.UseVisualStyleBackColor = true;
            // 
            // chk_ReduceSize
            // 
            this.chk_ReduceSize.AutoSize = true;
            this.chk_ReduceSize.Location = new System.Drawing.Point(203, 17);
            this.chk_ReduceSize.Name = "chk_ReduceSize";
            this.chk_ReduceSize.Size = new System.Drawing.Size(193, 17);
            this.chk_ReduceSize.TabIndex = 5;
            this.chk_ReduceSize.Text = "Compress images to reduce file size";
            this.chk_ReduceSize.UseVisualStyleBackColor = true;
            // 
            // pbar_TotalProgress
            // 
            this.pbar_TotalProgress.Location = new System.Drawing.Point(78, 117);
            this.pbar_TotalProgress.Name = "pbar_TotalProgress";
            this.pbar_TotalProgress.Size = new System.Drawing.Size(402, 16);
            this.pbar_TotalProgress.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Total:";
            // 
            // lbl_ProcessingFile
            // 
            this.lbl_ProcessingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ProcessingFile.Location = new System.Drawing.Point(143, 67);
            this.lbl_ProcessingFile.Name = "lbl_ProcessingFile";
            this.lbl_ProcessingFile.Size = new System.Drawing.Size(337, 13);
            this.lbl_ProcessingFile.TabIndex = 2;
            // 
            // pbar_ActualFile
            // 
            this.pbar_ActualFile.Location = new System.Drawing.Point(78, 83);
            this.pbar_ActualFile.Name = "pbar_ActualFile";
            this.pbar_ActualFile.Size = new System.Drawing.Size(402, 15);
            this.pbar_ActualFile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Processing:";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(4, 289);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(503, 313);
            this.textBoxLog.TabIndex = 5;
            // 
            // btn_showlog
            // 
            this.btn_showlog.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_showlog.FlatAppearance.BorderSize = 0;
            this.btn_showlog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_showlog.Image = global::CbrConverter.Properties.Resources.arrow_double_down;
            this.btn_showlog.Location = new System.Drawing.Point(235, 260);
            this.btn_showlog.Name = "btn_showlog";
            this.btn_showlog.Size = new System.Drawing.Size(39, 28);
            this.btn_showlog.TabIndex = 6;
            this.btn_showlog.UseVisualStyleBackColor = true;
            this.btn_showlog.Click += new System.EventHandler(this.btn_showlog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 287);
            this.Controls.Add(this.btn_showlog);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CBR Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbox_SourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_StartStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar pbar_TotalProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_ProcessingFile;
        private System.Windows.Forms.ProgressBar pbar_ActualFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_ReduceSize;
        private System.Windows.Forms.Label lbl_about;
        private System.Windows.Forms.CheckBox chk_NbPages;
        private System.Windows.Forms.CheckBox chk_pdf2cbr;
        private System.Windows.Forms.CheckBox chk_cbr2pdf;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.CheckBox chk_JoinImages;
        private System.Windows.Forms.Button btn_showlog;
        private System.Windows.Forms.CheckBox chk_SourceFolder;
        private System.Windows.Forms.TextBox tbox_OuputFolder;
    }
}

