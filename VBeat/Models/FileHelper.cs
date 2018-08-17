using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class FileHelper
    {
        public static string SaveFile(IFormFile formFile, string dir, string filename)
        {
            dir = dir.Trim('.', '~', '/');
            string guid = System.Guid.NewGuid().ToString();
            string htmlFilename = "/" + dir + "/" + guid + "-" + filename;
            using (Stream stream = formFile.OpenReadStream())
            {
                filename = "wwwroot/" + dir + "/" + guid + "-" + filename;
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    byte[] buffer = new byte[2048];
                    int bRead = 0;
                    while ((bRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        writer.BaseStream.Write(buffer, 0, bRead);
                    }
                }
            }

            return htmlFilename;
        }
    }
}
