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
            XDocument xDocument;
            if (!File.Exists(SizeInfoPath))
            {
                xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "true"),
                        new XElement("FormSize",
                        new XElement("MainFormSize",
                        new XElement("height", DEFAULT_HEIGHT.ToString()),
                        new XElement("width", DEFAULT_WIDTH.ToString())),
                        new XElement("TaskListFormSize",
                        new XElement("height", DEFAULT_HEIGHT.ToString()),
                        new XElement("width", DEFAULT_WIDTH.ToString()))
                        ));

                xDocument.Save(SizeInfoPath);
            }
            var xml = XElement.Load(SizeInfoPath);
            var sizeInfo = xml.Elements(form).Select(b => b).Single();

            string heightStr;
            string widthStr;
            try
            {
                heightStr = sizeInfo.Element("height").Value;
                widthStr = sizeInfo.Element("width").Value;
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
        public static void Save(int height, int width, string form)
        {
            var xml = XElement.Load(SizeInfoPath);
            var sizeInfo = xml.Elements(form).Select(b => b).Single();

            sizeInfo.Element("height").Value = height.ToString();
            sizeInfo.Element("width").Value = width.ToString();

            xml.Save(SizeInfoPath);
        }
    }
}
