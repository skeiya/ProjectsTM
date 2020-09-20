using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

namespace ProjectsTM.UI.MainForm
{
    static class SizeInfoManager
    {
        private static readonly int DEFAULT_HEIGHT = 250;
        private static readonly int DEFAULT_WIDTH = 250;
        public static Size Load(string sizeInfoPath)
        {
            XDocument xDocument;
            if (!File.Exists(sizeInfoPath))
                {
                    xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "true"),
                        new XElement("MainFormSize",
                            new XElement("height", DEFAULT_WIDTH.ToString()),
                            new XElement("width", DEFAULT_HEIGHT.ToString())
                            ));

                    xDocument.Save(sizeInfoPath);
                }
            var xml = XElement.Load(sizeInfoPath);
            string heightStr;
            string widthStr;
            try
            {
                heightStr = xml.Element("height").Value;
                widthStr = xml.Element("width").Value;
            }
            catch
            {
                return new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            }
            
            if (Int32.TryParse(widthStr, out int width) && Int32.TryParse(heightStr, out int height))
            {
                return new Size(width, height);
            }
            return new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }
        public static void Save(int height, int width, string sizeInfoPath)
        {
            var xml = XElement.Load(sizeInfoPath);

            xml.Element("height").Value = height.ToString();
            xml.Element("width").Value = width.ToString();

            xml.Save(sizeInfoPath);
        }

    }
}
