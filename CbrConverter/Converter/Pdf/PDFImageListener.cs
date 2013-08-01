using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace CbrConverter
{
	internal class PDFImageListener : IRenderListener
	{
		/** the byte array of the extracted images */
        public Dictionary<PageImageIndex, Image> ImagesList
        {
            get;
            set;
        }

		public int PageIndex { get; set; }
        private string OutputPath { get; set; }
		// ---------------------------------------------------------------------------
		/**
		 * Creates a RenderListener that will look for images.
		 */
		public PDFImageListener(string outputPath)
		{
            ImagesList = new Dictionary<PageImageIndex, Image>();
            this.OutputPath = outputPath;
			//_images = new List<Image>();
		}
		// ---------------------------------------------------------------------------
		/**
		 * @see com.itextpdf.text.pdf.parser.RenderListener#beginTextBlock()
		 */
		public void BeginTextBlock() { }
		// ---------------------------------------------------------------------------     
		/**
		 * @see com.itextpdf.text.pdf.parser.RenderListener#endTextBlock()
		 */
		public void EndTextBlock() { }
		// ---------------------------------------------------------------------------     
		/**
		 * @see com.itextpdf.text.pdf.parser.RenderListener#renderImage(
		 *     com.itextpdf.text.pdf.parser.ImageRenderInfo)
		 */
        public void RenderImage(ImageRenderInfo renderInfo)
        {
            PdfImageObject image = renderInfo.GetImage();
            //PdfName filter = (PdfName)image.Get(PdfName.FILTER);

            string imageName = string.Format("{0:0000}_{1:0000}.{2}", PageIndex, ImagesList.Count, image.GetImageBytesType().FileExtension);
            var pageImageIndex = new PageImageIndex
            {
                ImageName = imageName,
                ImageIndex = ImagesList.Count,
                PageIndex = PageIndex
            };
            var imageType = image.GetImageBytesType();
            //if (imageType != PdfImageObject.ImageBytesType.JBIG2)
            //{
            //var bmp = image.GetDrawingImage();                

            // Write image to file
            string pathToSave = string.Format(@"{0}\{1}", OutputPath, imageName);
            //bmp.Save(string.Format(pathToSave));
            // bmp.Dispose();

            // Sometime gdi+ error happen. We must write byte directly to disk
            if (!Directory.Exists(OutputPath))
                Directory.CreateDirectory(OutputPath);
            var bytes = image.GetImageAsBytes();
            File.WriteAllBytes(pathToSave, bytes);

            ImagesList.Add(pageImageIndex, null);
            //}
        }
		// ---------------------------------------------------------------------------     
		/**
		  * @see com.itextpdf.text.pdf.parser.RenderListener#renderText(
		  *     com.itextpdf.text.pdf.parser.TextRenderInfo)
		  */
		public void RenderText(TextRenderInfo renderInfo) { }

	}
}
