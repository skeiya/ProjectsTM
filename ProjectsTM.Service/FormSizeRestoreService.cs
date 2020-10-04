using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Service
{
    public static class FormSizeRestoreService
    {
        private static readonly int DEFAULT_HEIGHT = 250;
        private static readonly int DEFAULT_WIDTH = 500;
        private static string AppConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectsTM");
        private static string SizeInfoPath => Path.Combine(AppConfigDir, "FormSizeInfo.xml");

        public static Size Load( string form)
        {
            try
            {
                var xml = XElement.Load(SizeInfoPath);
                var sizeInfo = xml.Elements(form).Single();
                var heightStr = sizeInfo.Element("height").Value;
                var widthStr = sizeInfo.Element("width").Value;
                if (Int32.TryParse(widthStr, out int width) && Int32.TryParse(heightStr, out int height))
                {
                    return new Size(width, height);
                }
            }
            catch
            {
            }
            return new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }
        public static void Save(int height, int width, string form)
        {
            var xml = XElement.Load(SizeInfoPath);
            var sizeInfo = xml.Elements(form).Single();

            sizeInfo.Element("height").Value = height.ToString();
            sizeInfo.Element("width").Value = width.ToString();

            xml.Save(SizeInfoPath);
        }
    }
}
