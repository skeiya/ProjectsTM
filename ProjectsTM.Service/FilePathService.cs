using System.IO;

namespace ProjectsTM.Service
{
    public static class FilePathService
    {
        public static string GetPatternHistoryPath(string filePath)
        {
            return Path.Combine(Path.GetDirectoryName(filePath), "PatternHistory.xml");
        }
    }
}
