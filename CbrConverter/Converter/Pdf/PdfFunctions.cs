using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using iTextSharp.text.pdf.parser;
using System.Drawing.Imaging;
using iTextSharp.text.pdf;

namespace CbrConverter
{
    public class PdfFunctions
    {
        //static string _ExtractionDir;
        public static event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();


        public static PdfExtractionResult PDF_ExportImage(string filename, string dirForExtractions, int divider, bool checkResult, bool joinImages)
        {
            var details = new PdfExtractionResult();

            DataAccess.Instance.g_curProgress = 0;
            evnt_UpdateCurBar();

            var imagesList = new Dictionary<PageImageIndex, Image>();          

            // Ask itextsharp to extract image
            var pdfReader = new PdfReader(filename);
            var pdfParser = new PdfReaderContentParser(pdfReader);
            var pdfListener = new PDFImageListener(dirForExtractions);

            double tem0 = divider;
            double pgc = pdfReader.NumberOfPages;
            double CurOneStep = (double)(tem0 / pgc);

            details.Pages = (int)pgc;

            for (int i = 1; i <= pgc; i++)
            {
                pdfListener.PageIndex = i;
                // itextsharp send response to listener
                pdfParser.ProcessContent(i, pdfListener);

                DataAccess.Instance.g_curProgress += CurOneStep;
                evnt_UpdateCurBar();
            }

            imagesList = pdfListener.ImagesList; 
            details.ImagesBeforeMerge = pdfListener.ImagesList.Count;
            details.ImagesAfterMerge = details.ImagesBeforeMerge;

            if (checkResult && pdfReader.NumberOfPages != details.ImagesBeforeMerge)
            {
                if (joinImages)
                {
                    ImageJoiner cp = new ImageJoiner();
                    imagesList = cp.Merge(pdfListener.ImagesList, dirForExtractions);
                }

                details.ImagesAfterMerge = imagesList.Count;

                if(pdfReader.NumberOfPages != imagesList.Count)
                {                    
                    //Directory.Delete(dirForExtractions, true);
                    //throw new Exception(string.Format("Error extracting {0} : {1} images for {2} pages", Path.GetFileName(filename), pdfListener.ImagesList.Count, pdfReader.NumberOfPages));
                }
            }

            if (pdfReader != null)
                pdfReader.Close();

            // Write images to disk (because of memory problem write directly to file now)
            //WriteImages(dirForExtractions, imagesList);

            return details;
        }

//////////////////////////////////////////////// OLD METHOD (bug, sometime find same images for each pages)
        //private static void WriteImages(string outputFolder, Dictionary<PageImageIndex, Image> imageList)
        //{
        //    foreach (var imageInfo in imageList)
        //    {
        //        var imageData = imageInfo.Value;

        //        string tmp = String.Format(@"{0}\{1}", outputFolder, imageInfo.Key.ImageName);
        //        if (imageData == null)
        //            imageData = new Bitmap(tmp);

        //        imageData.Save(tmp);
        //        imageInfo.Value.Dispose();
        //    }

        //    imageList.Clear();
        //}


        //public static bool PDF_ExportImage(string filename, string dirForExtractions, int divider)
        //{
        //    bool isFileValid = true;
        //    iTextSharp.text.pdf.PdfReader.unethicalreading = true;
        //    iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(filename);

        //    DataAccess.Instance.g_curProgress = 0;
            
        //    _ExtractionDir = dirForExtractions;

        //    int imageCount = 0;
        //    double tem0 = divider;
        //    double pgc = pdfReader.NumberOfPages;
        //    double CurOneStep = (double)(tem0 / pgc);


        //    for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
        //    {
        //        PdfReader pdf = pdfReader;
        //        PdfDictionary pg = pdf.GetPageN(pageNumber);
        //        PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
        //        PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
        //        if (xobj != null)
        //        {
        //            var renderInfoList = new List<ImageRenderInfo>();
        //            foreach (iTextSharp.text.pdf.PdfName name in xobj.Keys)
        //            {
        //                iTextSharp.text.pdf.PdfObject obj = xobj.Get(name);
        //                if (obj.IsIndirect())
        //                {
        //                    PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
        //                    if (tg != null)
        //                    {
        //                        //PdfName type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
        //                        //if (PdfName.IMAGE.Equals(type))
        //                        try
        //                        {
        //                            string width = tg.Get(iTextSharp.text.pdf.PdfName.WIDTH).ToString();
        //                            string height = tg.Get(iTextSharp.text.pdf.PdfName.HEIGHT).ToString();
        //                            ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new Matrix(float.Parse(width), float.Parse(height)), (iTextSharp.text.pdf.PRIndirectReference)obj, tg);
        //                            renderInfoList.Add(imgRI);
        //                            RenderImage(renderInfoList, ref imageCount);
        //                            renderInfoList.Clear();
        //                        }
        //                        catch
        //                        {
        //                            isFileValid = false;
        //                        }
        //                    }
        //                }
        //                DataAccess.Instance.g_curProgress += CurOneStep;
        //                evnt_UpdateCurBar();
        //            }
        //            // Images are found from bottom to top
        //            renderInfoList.Reverse();
        //            //RenderImage(renderInfoList, ref imageCount);
        //        }
        //    }

        //    return isFileValid;
        //}

        //private static void RenderImage(List<ImageRenderInfo> renderInfoList, ref int count)
        //{
        //    if (renderInfoList.Count > 1)
        //    {
        //        // merge image
        //        var listImages = renderInfoList.Select(ri => ri.GetImage().GetDrawingImage()).Where(i => i != null).ToList();
        //        var combinedImage = CombineImages(listImages);
        //        // and save new
        //        string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
        //        combinedImage.Save(tmp);
        //    }
        //    else 
        //    {
        //        var renderInfo = renderInfoList.First();
        //        PdfImageObject image = renderInfo.GetImage();
        //        using (var dotnetImg = image.GetDrawingImage())
        //        {
        //            if (dotnetImg != null)
        //            {
        //                string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
        //                dotnetImg.Save(tmp);

        //            }
        //        }
        //    }
            
        //}

        //private static Image CombineImages(List<Image> imagesList)
        //{       
        //    int nIndex = 0;
        //    int height = 0;

        //    // List all width
        //    List<int> imageWidth = new List<int>();
        //    foreach (var img in imagesList)
        //    {
        //        imageWidth.Add(img.Width);
        //        height += img.Height;
        //    }
        //    imageWidth.Sort();

        //    // Take max width
        //    int width = imageWidth[imageWidth.Count - 1];

        //    // Create new image with max width and sum of all height
        //    Bitmap img3 = new Bitmap(width, height);
        //    Graphics g = Graphics.FromImage(img3);

        //    // merge all images
        //    g.Clear(SystemColors.AppWorkspace);
        //    foreach (var img in imagesList)
        //    {
        //        if (nIndex == 0)
        //        {
        //            g.DrawImage(img, new Point(0, 0));
        //            nIndex++;
        //            height = img.Height;
        //        }
        //        else
        //        {
        //            g.DrawImage(img, new Point(0, height));
        //            height += img.Height;
        //        }
        //        img.Dispose();
        //    }

        //    g.Dispose();
        //    imagesList.Clear();

        //    return img3;            
        //}
    }
}
