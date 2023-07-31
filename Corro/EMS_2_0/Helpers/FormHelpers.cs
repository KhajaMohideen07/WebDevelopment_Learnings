using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_2_0.Helpers
{
    public class FormHelpers
    {
        public static void WriteLogs(string Message)
        {
            string fileFullPath = "C:\\temp\\EMS_LOG_" + DateTime.Now.ToShortDateString().Replace('/','_') + ".txt";

            var filePath = Path.GetDirectoryName(fileFullPath);
            var fileName=Path.GetFileName(fileFullPath);

            if(!Directory.Exists(filePath))
              Directory.CreateDirectory(filePath);

            if(!File.Exists(fileName))
                File.Create(fileName);

            using(StreamWriter sw=File.AppendText(fileFullPath))
            {
                sw.WriteLine(Message + "_" + DateTime.Now.ToString());
            }

        }
    }
}
