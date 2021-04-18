using System;
using System.Linq;
using System.IO;
using System.Threading;

using System.Drawing.Imaging;
using System.Drawing;
using Ionic.Zip;
using System.ComponentModel;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace CbrConverter
{
    public class Extract
    {
        string temporaryDir;
        public event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();
        public event UpdateTotalBar evnt_UpdatTotBar;
        public delegate void UpdateTotalBar(Extract m, EventArgs e);
        public event UpdateFileName evnt_UpdateFileName;
        public delegate void UpdateFileName(Extract m, EventArgs e);
        public event ErrorNotify evnt_ErrorNotify;
        public delegate void ErrorNotify(Extract m, string e);
        public EventArgs e = null;
        double CurOneStep;
        bool IsSingleFile; //to link the 2 progress bar
        bool _Cbr2Pdf, _Pdf2Cbz, _ReduceSize, _JoinImages, _CheckImagesPages;


        /// <summary>
        /// Start the extraction thread according to selected file or folder
        /// </summary>
        public void BeginExtraction(bool cbr2pdf, bool pdf2cbz, bool reduceSize, bool deleteOriginal, bool joinImages, bool checkImagesPages)
        {
            _Cbr2Pdf = cbr2pdf;
            _JoinImages = joinImages;
            _CheckImagesPages = checkImagesPages;
            _ReduceSize = reduceSize;
            _Pdf2Cbz = pdf2cbz;

            //checking if is a directory or a file
            if (File.Exists(DataAccess.Instance.g_WorkingDir))
            {
                DataAccess.Instance.g_WorkingFile = DataAccess.Instance.g_WorkingDir;
                IsSingleFile = true;
                //it's a file so i start the single extraction thread
                //check if cbr
                string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile).ToLower();
                if ((string.Compare(ext, ".zip") == 0 || string.Compare(ext, ".rar") == 0 || string.Compare(ext, ".cbr") == 0 || string.Compare(ext, ".cbz") == 0) && _Cbr2Pdf)
                {

                    Thread extract = new Thread(ConvertCbrToPdf);
                    extract.Start();
                }
                else if ((string.Compare(ext, ".pdf") == 0) && (_Pdf2Cbz))
                {
                    int nbFichiers = 1;
                    //calculate the value for the progression bar
                    double singval = (double)100 / nbFichiers;

                    var bw = new BackgroundWorker();

                    // define the event handlers
                    bw.DoWork += (sender, args) =>
                    {
                        // do your lengthy stuff here -- this will happen in a separate thread
                        ExtractMultipleFiles(DataAccess.Instance.g_WorkingDir, singval);
                    };

                    bw.RunWorkerCompleted += (sender, args) =>
                    {
                        //finished, update the ui
                        DataAccess.Instance.g_Processing = false;
                        DataAccess.Instance.g_totProgress = 0;
                        DataAccess.Instance.g_curProgress = 0;
                        DataAccess.Instance.g_WorkingFile = string.Empty;
                        evnt_UpdateFileName(this, e);
                        evnt_UpdatTotBar(this, e);
                        evnt_UpdateCurBar();
                    };

                    bw.RunWorkerAsync(); // starts the background worker
                }

            }
            else //it's a directory
            {
                IsSingleFile = false;

                int nbFichiers = CountFiles(DataAccess.Instance.g_WorkingDir);
                //calculate the value for the progression bar
                double singval = (double)100 / nbFichiers;

                //Thread ext = new Thread(() => ExtractMultipleFiles(DataAccess.Instance.g_WorkingDir, singval));
                //ext.Start();
                var bw = new BackgroundWorker();

                // define the event handlers
                bw.DoWork += (sender, args) =>
                {
                    // do your lengthy stuff here -- this will happen in a separate thread
                    ExtractMultipleFiles(DataAccess.Instance.g_WorkingDir, singval);
                };

                bw.RunWorkerCompleted += (sender, args) =>
                {
                    //finished, update the ui
                    DataAccess.Instance.g_Processing = false;
                    DataAccess.Instance.g_totProgress = 0;
                    DataAccess.Instance.g_curProgress = 0;
                    DataAccess.Instance.g_WorkingFile = string.Empty;
                    evnt_UpdateFileName(this, e);
                    evnt_UpdatTotBar(this, e);
                    evnt_UpdateCurBar();
                };

                bw.RunWorkerAsync(); // starts the background worker
            }
        }

        /// <summary>
        /// Extract the file and call the Compress Image and the Generate PDF
        /// </summary>
        private void ConvertCbrToPdf()
        {
            try
            {
                //check if cbr
                /*string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile);
                if ((string.Compare(ext, ".cbr") != 0)&&(string.Compare(ext, ".cbz") != 0))
                    return;*/

                //write the name of the file on the UI
                evnt_UpdateFileName(this, e);

                //creating directory for extraction      
                string tempFileName = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                temporaryDir = DataAccess.Instance.g_WorkingFile;
                temporaryDir = temporaryDir.Replace(tempFileName, Path.GetFileNameWithoutExtension(DataAccess.Instance.g_WorkingFile));

                // replace with new output
                var SourceFolder = DataAccess.Instance.g_WorkingDir.Replace(Path.GetFileName(DataAccess.Instance.g_WorkingDir), "");
                temporaryDir = temporaryDir.Replace(SourceFolder, DataAccess.Instance.g_Output_dir + "\\");

                //if the directory already exist, delete it
                if (Directory.Exists(temporaryDir))
                    Directory.Delete(temporaryDir, true);
                Directory.CreateDirectory(temporaryDir);


                //inizio test
                var archive = ArchiveFactory.Open(DataAccess.Instance.g_WorkingFile);
                //calculating file for pregress bar
                double CurOneStep = archive.Entries.Count();
                int divider;
                if (_ReduceSize)
                    divider = 33;
                else
                    divider = 50;
                CurOneStep = divider / CurOneStep;

                var options = new ExtractionOptions
                {
                    Overwrite = true,
                    PreserveAttributes = false,
                    PreserveFileTime = true
                };

                //extract the file into the folder
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        if (DataAccess.Instance.g_Processing) //this is to stop the thread if stop button is pressed
                        {

                            string path = Path.Combine(temporaryDir, Path.GetFileName(entry.Key));
                            //entry.WriteToDirectory(@"C:\temp", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            entry.WriteToFile(path, options);

                            DataAccess.Instance.g_curProgress += CurOneStep;
                            evnt_UpdateCurBar();

                            if (IsSingleFile)
                            {
                                DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                                evnt_UpdatTotBar(this, e);
                            }
                        }
                    }
                }


                if (DataAccess.Instance.g_Processing)
                {
                    if (DataAccess.Instance.g_ReduceSize)
                        CompressImage();
                    GeneratePdf();
                }
                //forcing garbage collector to clean to avoid lock file error caused by pdfsharp
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //deleting temp dir
                Directory.Delete(temporaryDir, true);

                //update progress bar
                DataAccess.Instance.g_curProgress = 0;
                evnt_UpdateCurBar();

                if (IsSingleFile)
                {
                    DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                    evnt_UpdatTotBar(this, e);
                }

                //if we are converting a single file and not a directory we are done, so i reset values and clean the UI
                if (IsSingleFile)
                {
                    DataAccess.Instance.g_Processing = false;
                    DataAccess.Instance.g_WorkingFile = string.Empty;
                    evnt_UpdateFileName(this, e);
                }
            }
            catch (Exception ex) //too lazy to catch specific exceptions, TODO in future!!
            {
                DataAccess.Instance.g_Processing = false; //stopping the process
                evnt_ErrorNotify(this, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Extract images from PDF and compress into a cbr archive
        /// </summary>
        private void ConvertPdfToCbr(string currentFile)
        {
            evnt_UpdateFileName(this, e);
            //creating directory for extraction      
            string tempFileName = Path.GetFileName(currentFile);
            temporaryDir = currentFile;
            temporaryDir = temporaryDir.Replace(tempFileName, Path.GetFileNameWithoutExtension(currentFile));

            // replace with new output
            var SourceFolder = DataAccess.Instance.g_WorkingDir.Replace(Path.GetFileName(DataAccess.Instance.g_WorkingDir), "");
            temporaryDir = temporaryDir.Replace(SourceFolder, DataAccess.Instance.g_Output_dir + "\\");

            //if the directory already exist, delete it
            if (Directory.Exists(temporaryDir))
                Directory.Delete(temporaryDir, true);
            Directory.CreateDirectory(temporaryDir);


            int divider;
            if (_ReduceSize)
                divider = 50;
            else
                divider = 80;


            var result = PdfFunctions.PDF_ExportImage(currentFile, temporaryDir, divider, _CheckImagesPages, _JoinImages);

            if (result.ImagesAfterMerge != result.Pages)
                evnt_ErrorNotify(this, string.Format("{0} : {1} images for {2} pages!", currentFile, result.ImagesAfterMerge, result.Pages));

            if (DataAccess.Instance.g_Processing)
            {
                //compress files in a zip  
                if (_ReduceSize)
                    CompressImage();

                string savedfile = temporaryDir + ".cbz";


                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(temporaryDir);
                    // zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                    zip.Save(savedfile);
                }

                //waiting the new sharpcompress release to fix it
                /*  using (var archive = ZipArchive.Create())
                  {
                      archive.AddAllFromDirectory(temporaryDir);                       
                      archive.SaveTo(savedfile, CompressionType.None);    
                  }*/
            }


            //delete the temp dir
            if (Directory.Exists(temporaryDir))
                Directory.Delete(temporaryDir, true);



            //if we are converting a single file and not a directory we are done, so i reset values and clean the UI
            if (IsSingleFile)
            {
                DataAccess.Instance.g_Processing = false;
                DataAccess.Instance.g_WorkingFile = string.Empty;
                evnt_UpdateFileName(this, e);
            }
        }

        private int CountFiles(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.pdf", SearchOption.AllDirectories);
            return files.Length;
        }

        /// <summary>
        /// when a directory is selected this method  is launched, it just read all the files inside the directory and call ExtractSingleFile()
        /// </summary>
        private void ExtractMultipleFiles(string currentDir, double singval)
        {
            string[] files = { currentDir };

            if ((File.GetAttributes(currentDir) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                files = Directory.GetFiles(currentDir);
                string[] directories = Directory.GetDirectories(currentDir);
                //int count = files.Count();

                foreach (string dir in directories)
                {
                    ExtractMultipleFiles(dir, singval);
                }
            }

            //call the ExtractSingleFile for each file
            foreach (string file in files)
            {
                if (DataAccess.Instance.g_Processing) //this is to stop the thread if stop button is pressed
                {
                    DataAccess.Instance.g_WorkingFile = file;
                    try
                    {
                        string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile).ToLower();
                        if (((string.Compare(ext, ".cbr") == 0) || (string.Compare(ext, ".rar") == 0) || (string.Compare(ext, ".cbz") == 0) || (string.Compare(ext, ".zip") == 0)) && (_Cbr2Pdf))
                        {
                            ConvertCbrToPdf();
                        }
                        else if ((string.Compare(ext, ".pdf") == 0)  && (_Pdf2Cbz))
                        {
                            ConvertPdfToCbr(file);
                        }

                        //updating the total progression bar
                        DataAccess.Instance.g_totProgress += singval;
                        evnt_UpdatTotBar(this, e);
                    }
                    catch (Exception a)
                    {
                        evnt_ErrorNotify(this, file + " : " + a.Message.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Generate the pdf from the images
        /// </summary>
        private void GeneratePdf()
        {
            // Create a new PDF document
            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

            string filename = temporaryDir + ".pdf";
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.Open();

            var imageFiles = Directory.GetFiles(temporaryDir).ToList();
            imageFiles.Sort();

            //count for progression bar
            CurOneStep = imageFiles.Count();
            int divider;
            if (DataAccess.Instance.g_ReduceSize)
                divider = 33;
            else
                divider = 50;
            CurOneStep = divider / CurOneStep;


            //importing the images in the pdf
            foreach (string imageFile in imageFiles)
            {
                if (DataAccess.Instance.g_Processing)
                {
                    try
                    {
                        //updating progress bar
                        DataAccess.Instance.g_curProgress += CurOneStep;
                        evnt_UpdateCurBar();

                        if (IsSingleFile)
                        {
                            DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                            evnt_UpdatTotBar(this, e);
                        }

                        //checking file extension
                        string ext = Path.GetExtension(imageFile).ToLower();
                        if ((string.Compare(ext, ".tif") == 0) || (string.Compare(ext, ".jpg") == 0) || (string.Compare(ext, ".jpeg") == 0) || (string.Compare(ext, ".png") == 0) || (string.Compare(ext, ".bmp") == 0) || (string.Compare(ext, ".new") == 0))
                        {
                            //var size = iTextSharp.text.PageSize.A4;                                                    
                            var img = iTextSharp.text.Image.GetInstance(imageFile);
                            //img.ScaleToFit(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height);
                            document.SetPageSize(new iTextSharp.text.Rectangle(img.Width, img.Height));
                            // Create an empty page
                            document.NewPage();
                            img.SetAbsolutePosition(0, 0);
                            writer.DirectContent.AddImage(img);
                            
                        }
                    }
                    catch (Exception e)
                    {
                        evnt_ErrorNotify(this, imageFile + " : " + e.Message.ToString());
                    }
                }

            }


            //saving file
            document.Close();
        }

        /// <summary>
        /// Compress the image and reduce the quality
        /// </summary>
        private void CompressImage()
        {
            int NEW_IMG_QUALITY = 40; //i found that 30 is not too bad and the filesize is about the half

            string[] imageFiles = Directory.GetFiles(temporaryDir);

            //count for progress bar
            CurOneStep = imageFiles.Count();
            int divider;
            if (DataAccess.Instance.g_ReduceSize)
                divider = 33;
            else
                divider = 50;
            CurOneStep = divider / CurOneStep;

            //compress every image
            foreach (string imageFile in imageFiles)
            {
                if (DataAccess.Instance.g_Processing) //this is to stop the thread if the user press the stop button
                {

                    string NewImg;
                    //checking file extension
                    string ext = Path.GetExtension(imageFile);
                    if ((string.Compare(ext, ".jpg") != 0) && (string.Compare(ext, ".jpeg") != 0) && (string.Compare(ext, ".bmp") != 0)
                        && (string.Compare(ext, ".JPG") != 0) && (string.Compare(ext, ".JPEG") != 0) && (string.Compare(ext, ".BMP") != 0))
                        break;

                    //compressing
                    using (Image OldImg = Image.FromFile(imageFile))
                    {
                        NewImg = imageFile + ".new";
                        SaveJpeg(NewImg, OldImg, NEW_IMG_QUALITY);
                    }

                    //updating progress bar
                    DataAccess.Instance.g_curProgress += CurOneStep;
                    evnt_UpdateCurBar();

                    if (IsSingleFile)
                    {
                        DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                        evnt_UpdatTotBar(this, e);
                    }

                    //delete the old image
                    File.Delete(imageFile);

                    //remove the .new
                    File.Move(NewImg, imageFile);
                }
            }

        }

        /// <summary>
        /// Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="quality">An integer from 0 to 100, with 100 being the highest quality</param>
        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            // to avoid gdi+ error save image to stream instead directly on file system
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, jpegCodec, encoderParams);
                ms.ToArray();
                File.WriteAllBytes(path, ms.ToArray());
                //img.Save(path, jpegCodec, encoderParams);
            }
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
    }
}
