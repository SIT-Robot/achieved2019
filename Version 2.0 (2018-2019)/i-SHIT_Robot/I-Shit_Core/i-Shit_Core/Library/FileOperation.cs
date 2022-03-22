using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Library
{
    public class FileOperation
    {
        public static string DataPath = @"..\Data\";
        public static string ReadTXT(string filename)
        {
            string strReadFilePath = @"..\Data\" + filename;
            StreamReader srReadFile = new StreamReader(strReadFilePath);
            string content = "";
            while (!srReadFile.EndOfStream)
            {
                string strReadLine = srReadFile.ReadLine();
                content = content + strReadLine + Environment.NewLine;
            }
            srReadFile.Close();

            return content.TrimEnd('\n');
        }

        public static void WriteTXT(string filename, string content)
        {
            string strWriteFilePath = @"..\Data\ " + filename;
            StreamWriter swWriteFile = File.CreateText(strWriteFilePath);
            swWriteFile.Write(content);
            swWriteFile.Close();
        }
    }
}