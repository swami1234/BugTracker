using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helper
{
    public class ImageUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                        ImageFormat.Png.Equals(img.RawFormat) ||
                         ImageFormat.Icon.Equals(img.RawFormat) ||
                          ImageFormat.Bmp.Equals(img.RawFormat) ||
                        ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }

        }

        public static string GetIcon(string fileName)

        {

            var icon = "";

            var extension = Path.GetExtension(fileName);



            switch (extension)

            {

                case ".pdf":

                    icon = "fa fa-file-pdf-o";

                    break;

                case ".doc":

                case ".docx":

                    icon = "fa fa-file-word-o";

                    break;

                case ".xls":

                case ".xlsx":

                    icon = "fa fa-file-excel-o";

                    break;

                case ".txt":

                    icon = "fa fa-file-text-o";

                    break;

                case ".jpg":

                case ".jpeg":

                case ".png":

                case ".gif":

                    icon = "fa fa-file-image-o";

                    break;

                case ".mp3":

                    icon = "fa fa-file-sound-o";

                    break;

                case ".zip":

                    icon = "fa fa-file-zip-o";

                    break;

                default:

                    icon = "fa fa-file-code-o";

                    break;



            }



            return icon;



        }

        public static bool IsValidAttachment(HttpPostedFileBase file)
        {
            // var file = FilePath;
            try
            {

                if (file == null)
                    return false;

                if (file.ContentLength > 5 * 1024 * 1024 || file.ContentLength < 1024)
                    return false;

                var extValid = false;
                foreach (var ext in WebConfigurationManager.AppSettings["AllowedAttachmentExtensions"].Split(','))
                {
                    if (Path.GetExtension(file.FileName) == ext)
                    {
                        extValid = true;
                        break;
                    }
                }
                return IsWebFriendlyImage(file) || extValid;
            }
            catch
            {
                return false;
            }
        }
    }
}