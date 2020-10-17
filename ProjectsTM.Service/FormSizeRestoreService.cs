using System;
using System.Collections.Generic;
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

        public static Size LoadFormSize(string form)
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

        public static int[] LoadColWidths(string form)
        {
            try
            {
                var result = new List<int>();
                var xml = XElement.Load(SizeInfoPath);
                var colWidthsElement = xml.Elements(form).Single().Elements("colwidths");
                for (var idx = 0; idx < colWidthsElement.Elements().Count(); idx++)
                {
                    var col = colWidthsElement.Single().Elements("col" + idx.ToString());
                    if (!col.Any()) continue;
                    if (!int.TryParse(col.Single().Value, out var w)) continue;
                    result.Add(w);
                }
                return result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static void SaveColWidths(int[] colWidths, string form)
        {
            var root = GetRootElement(SizeInfoPath);
            var formElement = GetSubElement(root, form);
            var colWidthsElement = GetSubElement(formElement, "colwidths");
            var idx = 0;
            foreach (var colWidth in colWidths)
            {
                var colWidthElement = GetSubElement(colWidthsElement, "col" + idx.ToString());
                colWidthElement.Value = colWidth.ToString();
                idx++;
            }
            root.Save(SizeInfoPath);
        }

        public static void SaveFormSize(int height, int width, string form)
        {
            var root = GetRootElement(SizeInfoPath);
            var formElement = GetSubElement(root, form);
            var heightElement = GetSubElement(formElement, "height");
            var widthElement = GetSubElement(formElement, "width");
            heightElement.Value = height.ToString();
            widthElement.Value = width.ToString();
            root.Save(SizeInfoPath);
        }

        private static XElement GetSubElement(XElement parent, string name)
        {
            var child = parent.Element(name);
            if (child != null) return child;
            child = new XElement(name);
            parent.Add(child);
            return child;
        }

        private static XElement GetRootElement(string sizeInfoPath)
        {
            try
            {
                return XElement.Load(sizeInfoPath);
            }
            catch
            {
                return new XElement("root");
            }
        }
    }
}
