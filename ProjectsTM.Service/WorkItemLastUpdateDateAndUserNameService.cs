using ProjectsTM.Model;
using System.Collections.Generic;

namespace ProjectsTM.Service
{
    public class WorkItemLastUpdateDateAndUserNameService
    {
        private string _filePath;
        private readonly Dictionary<WorkItem, string> _xmlLines = new Dictionary<WorkItem, string>();

        public void Load(string filePath)
        {
            _filePath = filePath;
            _xmlLines.Clear();
        }

        public string GetDateAndUserName(WorkItem workItem)
        {
            if (_xmlLines.TryGetValue(workItem, out var result)) return result;
            result = GitRepositoryService.GetLastUpdateDateAndUserName(_filePath, workItem.LineNumber, workItem.LineNumber + workItem.LinePosition);
            _xmlLines.Add(workItem, result);
            return result;
        }
    }
}
