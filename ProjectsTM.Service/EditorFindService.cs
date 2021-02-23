using ProjectsTM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public class EditorFindService
    {
        private string _filePath;
        private readonly Dictionary<WorkItem, string> _foundDictionary = new Dictionary<WorkItem, string>();
        private readonly List<WorkItem> _searchingList = new List<WorkItem>();

        public void Load(string filePath)
        {
            _filePath = filePath;
            _foundDictionary.Clear();
        }

        public async Task<string> Find(WorkItem workItem)
        {
            _searchingList.Add(workItem);
            return await Task<string>.Run(() =>
            {
                if (_foundDictionary.TryGetValue(workItem, out var result)) return result;
                result = GitRepositoryService.GetLastEditorName(_filePath, workItem.LineStart, workItem.LineEnd);
                _foundDictionary.Add(workItem, result);
                _searchingList.Remove(workItem);
                return result;
            }).ConfigureAwait(true);
        }

        internal bool TryFind(WorkItem wi, out string result)
        {
            if (_searchingList.Contains(wi))
            {
                result = string.Empty;
                return true;
            }
            return _foundDictionary.TryGetValue(wi, out result);
        }
    }
}
