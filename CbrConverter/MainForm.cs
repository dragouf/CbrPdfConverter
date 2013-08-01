using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CbrConverter
{
    public partial class MainForm : Form
    {
        bool _fileSelected = false;
        bool _outputFolderSelected = false;

        public MainForm()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(this.btn_showlog, "Show/Hide Logs");
        }

       

        private void btn_StartStop_Click(object sender, EventArgs e)
        {

            if (DataAccess.Instance.g_Processing)
            {
                DataAccess.Instance.g_Processing = false;
                btn_StartStop.Text = "START";
                lbl_ProcessingFile.Text = string.Empty;
            }
            else
            {
                if (((chk_cbr2pdf.Checked) || (chk_pdf2cbr.Checked))&&(_fileSelected))
                {
                    btn_StartStop.Text = "STOP";
                    DataAccess.Instance.g_ReduceSize = chk_ReduceSize.Checked;
                    DataAccess.Instance.g_Processing = true;
                    Extract ex = new Extract();
                    this.Subscribe(ex);
                    ex.BeginExtraction(chk_cbr2pdf.Checked, chk_pdf2cbr.Checked, chk_ReduceSize.Checked, chk_NbPages.Checked, chk_JoinImages.Checked, chk_NbPages.Checked);
                }
            }
        }

        

        public void Subscribe(Extract m)
        {
            m.evnt_UpdateCurBar += new Extract.UpdateCurrentBar(UpdateCurrBar);
            m.evnt_UpdatTotBar += new Extract.UpdateTotalBar(UpdateTotaBar);
            m.evnt_UpdateFileName += new Extract.UpdateFileName(UpdateFileName);
            m.evnt_ErrorNotify += new Extract.ErrorNotify(ErrorShowPopup);
            PdfFunctions.evnt_UpdateCurBar += new PdfFunctions.UpdateCurrentBar(UpdateCurrBar);
        }
        private void UpdateCurrBar()
        {           
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_curProgress > 100)
                    DataAccess.Instance.g_curProgress = 100;
                pbar_ActualFile.Value = (int)DataAccess.Instance.g_curProgress;
            });
        }

        private void UpdateFileName(Extract m, EventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                lbl_ProcessingFile.Text = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                if (lbl_ProcessingFile.Text == string.Empty)//finished
                {
                    btn_StartStop.Text = "START";
                   
                }
            });
        }

        private void UpdateTotaBar(Extract m, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_totProgress > 100)
                    DataAccess.Instance.g_totProgress = 100;
                pbar_TotalProgress.Value = (int)DataAccess.Instance.g_totProgress;
            });
        }

        private void ErrorShowPopup(Extract m, string e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //btn_StartStop.Text = "START";
                //pbar_TotalProgress.Value = 0;
                //pbar_ActualFile.Value = 0;
                //lbl_ProcessingFile.Text =string.Empty;
                listViewLog.Items.Add(new ListViewItem(new List<string>{ (listViewLog.Items.Count + 1).ToString(), e}.ToArray()));
                //this.textBoxLog.Text += e + Environment.NewLine;
                ShowLog();
            });
            //MessageBox.Show(e, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        /// <summary>
        /// User clicked on textbox, opendialog to select file or folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_SourceFile_Click(object sender, EventArgs e)
        {
            var SelectFolderDlg = new FolderBrowserDialogEx();
            SelectFolderDlg.Description = "Select a file or folder:"; //message
            SelectFolderDlg.ShowNewFolderButton = true;
            SelectFolderDlg.ShowEditBox = false;                     //editbox 
            SelectFolderDlg.ShowBothFilesAndFolders = true;          //show files and folders
            SelectFolderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer; //start from computer

            DialogResult result = SelectFolderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DataAccess.Instance.g_WorkingDir = SelectFolderDlg.SelectedPath;
                tbox_SourceFile.Text = SelectFolderDlg.SelectedPath;

                //check if file or folder
                if (File.Exists(DataAccess.Instance.g_WorkingDir)) //is a file
                {
                    //check the extension
                    var ext = Path.GetExtension(SelectFolderDlg.SelectedPath).ToLower();
                    if (ext == ".pdf")
                    {
                        chk_cbr2pdf.Checked = false;
                        chk_cbr2pdf.Enabled = false;
                        chk_pdf2cbr.Checked = true;
                        chk_pdf2cbr.Enabled = true;
                        chk_JoinImages.Enabled = true;

                    }
                    else if (ext == ".cbr" || ext == ".cbz")
                    {
                        chk_cbr2pdf.Checked = true;
                        chk_cbr2pdf.Enabled = true;
                        chk_pdf2cbr.Checked = false;
                        chk_pdf2cbr.Enabled = false;
                        chk_JoinImages.Enabled = false;
                    }
                }
                else //is a folder
                {
                    chk_cbr2pdf.Checked = true;
                    chk_cbr2pdf.Enabled = true;
                    chk_pdf2cbr.Checked = true;
                    chk_pdf2cbr.Enabled = true;
                }

                _fileSelected = true;

                if (this.chk_SourceFolder.Checked)
                {
                    _outputFolderSelected = true;
                    DataAccess.Instance.g_Output_dir = Path.GetDirectoryName(SelectFolderDlg.SelectedPath);
                    this.tbox_OuputFolder.Text = this.tbox_SourceFile.Text;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbox_SourceFile.SelectionStart = 0;
        }

        private void lbl_about_Click(object sender, EventArgs e)
        {
            AboutorForm aboutform = new AboutorForm();
            aboutform.ShowDialog();
        }


        private int LogTopPosition
        {
            get
            {
                Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                int titleHeight = screenRectangle.Top - this.Top;
                return this.listViewLog.Top + titleHeight - 1;
            }
        }

        private int LogBottomPosition
        {
            get
            {
                Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                int titleHeight = screenRectangle.Top - this.Top;
                return this.listViewLog.Bottom + titleHeight + 10;
            }
        }

        private void btn_showlog_Click(object sender, EventArgs e)
        {
            ToggleLog();
        }

        private void ToggleLog()
        {
            if (this.Height >= this.LogBottomPosition)
            {
                HideLog();
            }
            else
            {
                ShowLog();
            }
        }

        private void ShowLog()
        {            
            var timerSlide = new System.Windows.Forms.Timer();
            timerSlide.Interval = 3;
            timerSlide.Tick += delegate(object sender, EventArgs e)
            {
                var timer = (System.Windows.Forms.Timer)sender;
                if (this.Height >= this.LogBottomPosition)
                {
                    timer.Enabled = false;
                    this.Height = this.LogBottomPosition;
                    this.btn_showlog.Image = global::CbrConverter.Properties.Resources.arrow_double_up;
                }
                else
                {
                    this.Height = this.Size.Height + 10;
                }
                    
            };
            timerSlide.Start();
        }

        private void HideLog()
        {
            // just a slide effect
            var timerSlide = new System.Windows.Forms.Timer();
            timerSlide.Interval = 3;
            timerSlide.Tick += delegate(object sender, EventArgs e)
            {
                var timer = (System.Windows.Forms.Timer)sender;
                if (this.Height <= this.LogTopPosition)
                {
                    timer.Enabled = false;
                    this.Height = this.LogTopPosition;
                    this.btn_showlog.Image = global::CbrConverter.Properties.Resources.arrow_double_down;
                }
                else
                {
                    this.Height = this.Height - 10;
                }

            };
            timerSlide.Start();     
        }

        private void tbox_OuputFolder_Click(object sender, EventArgs e)
        {
            var SelectFolderDlg = new FolderBrowserDialogEx();
            SelectFolderDlg.Description = "Select a folder:"; //message
            SelectFolderDlg.ShowNewFolderButton = true;
            SelectFolderDlg.ShowEditBox = false;                     //editbox 
            SelectFolderDlg.ShowBothFilesAndFolders = false;          //show files and folders
            SelectFolderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer; //start from computer

            DialogResult result = SelectFolderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DataAccess.Instance.g_Output_dir = SelectFolderDlg.SelectedPath;
                tbox_OuputFolder.Text = SelectFolderDlg.SelectedPath;

                _outputFolderSelected = true;

                if (this.chk_SourceFolder.Checked)
                {
                    this.tbox_OuputFolder.Text = this.tbox_SourceFile.Text;
                }
            }
        }

        private void chk_SourceFolder_CheckedChanged(object sender, EventArgs e)
        {           
            this.tbox_OuputFolder.Enabled = !this.chk_SourceFolder.Checked;
            if (this.chk_SourceFolder.Checked)
            {
                DataAccess.Instance.g_Output_dir = Path.GetDirectoryName(DataAccess.Instance.g_WorkingDir);
                this.tbox_OuputFolder.Text = DataAccess.Instance.g_Output_dir;
            }
        }
    }
}
