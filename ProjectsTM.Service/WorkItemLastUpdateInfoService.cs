using ProjectsTM.Model;
using System.Xml;
using System.Xml.Linq;

namespace ProjectsTM.Service
{
    public class WorkItemLastUpdateInfoService
    {
        private string _filePath;
        private XElement _xml;

        public void Load(string filePath)
        {
            _filePath = filePath;
            _xml = XElement.Load(filePath, LoadOptions.SetLineInfo).Element(nameof(WorkItems));
        }

        public string GetInfo(WorkItem workItem)
        {
            if (_xml == null) return string.Empty;
            foreach (var m in _xml.Elements("WorkItemsOfEachMember"))
            {
                foreach (var w in m.Element(nameof(MembersWorkItems))
                    .Elements(nameof(WorkItem)))
                {
                    if (w.Element("HashCode") == null) continue;
                    if (!workItem.GetMd5Code().Equals(w.Element("HashCode").Value)) continue;
                    return "履歴：" + GitRepositoryService.GetLastUpdateInfo(_filePath, ((IXmlLineInfo)w.Element("HashCode")).LineNumber);
                }
            }
            return string.Empty;
        }
    }
}
