using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.UI.MainForm
{
    static class SizeInfoManager
    {
        private static readonly int DEFAULT_HEIGHT = 250;
        private static readonly int DEFAULT_WIDTH = 250;
        public static Size Load(string userSettingPath)
        {
            var xml = XElement.Load(userSettingPath);
            XElement sizeInfo;
            string heightStr;
            string widthStr;
            try
            {
                sizeInfo = xml.Elements("MainFormSize").Select(b => b).Single();
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
        public static void Save(int height, int width, string useSettingPath)
        {
            var xml = XElement.Load(useSettingPath);
            XElement datas = new XElement("MainFormSize",
                            new XElement("height", height.ToString()),
                            new XElement("width", width.ToString()));
            xml.Add(datas);

            xml.Save(useSettingPath);
        }

    }
}
