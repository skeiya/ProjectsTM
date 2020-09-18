using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectsTM.UI.MainForm
{
    class SizeInfoManager
    {
        private readonly int DEFAULT_HEIGHT = 250;
        private readonly int DEFAULT_WIDTH = 250;
        private readonly string _dirPath = @".\Resource";
        private readonly string _filePath = @".\Resource\sizeInfo.txt";
        public void CheckSourceFile()
        {
            if (!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
            }
            if (!File.Exists(_filePath))
            {
                using (StreamWriter writer = File.CreateText(_filePath))
                {
                    writer.WriteLine(DEFAULT_HEIGHT.ToString());
                    writer.WriteLine(DEFAULT_WIDTH.ToString());
                }
            }
        }
        public void SaveSizeInfo(int height, int width)
        {
            using (var writer = new StreamWriter(_filePath))
            {
                writer.WriteLine(height.ToString());
                writer.WriteLine(width.ToString());
            }
        }
        public (int height, int width) LoadSizeInfo()
        {
            var info = new List<string>();
            using (var reader = new StreamReader(_filePath))
            {
                while (reader.Peek() != -1)
                {
                    info.Add(reader.ReadLine());
                }
            }
            return (Convert.ToInt32(info[0]), Convert.ToInt32(info[1]));
        }
    }
}
