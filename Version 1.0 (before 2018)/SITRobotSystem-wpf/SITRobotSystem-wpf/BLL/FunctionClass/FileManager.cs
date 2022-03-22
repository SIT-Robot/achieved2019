using System;
using System.IO;
using System.Windows.Forms;

namespace SITRobotSystem_wpf.BLL.FunctionClass
{
    /// <summary>
    /// 文件管理类
    /// </summary>
    public static class FileManager
    {

        public static int getFileSize(string filePath)
        {
            FileInfo filtem = new FileInfo(@filePath);
            int filesize = Convert.ToInt32(filtem.Length)/1000;
            return filesize;
        }

        public static string OpenImgFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "jpg文件|*.jpg|png文件|*.png|所有文件|*.*";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "jpg";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return "";
            }
            String filepath = openFileDialog.FileName;
            return filepath;
        }

        public static bool isFileExist(string path)
        {
            return File.Exists(path) ;
        }

        public static void deleteFile(string path)
        {
            if (File.Exists(@path))
                //如果存在则删除
                File.Delete(@path);           
        }
    }
}
