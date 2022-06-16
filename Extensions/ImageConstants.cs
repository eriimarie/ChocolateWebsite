using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Extensions
{
    public static class ImageConstants
    {
        public static string thumbnail = "thumb";
        public static string detail = "detail";

        public static string GetThumbnaill(int Id, IWebHostEnvironment _environment)
        {

            string wwwRootPath = _environment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/product/", Id.ToString());
            if (System.IO.Directory.Exists(path))
            {
                var files = System.IO.Directory.GetFiles(path);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var displayPath = "/product/" + Id + "/" + fileInfo.Name;
                    if (file.Contains(ImageConstants.thumbnail))
                    {
                        return displayPath;
                    }

                }
            }
            return string.Empty;
        }
    }
}
