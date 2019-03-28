using System.Collections;
using System.Collections.Generic;

namespace TaskManagement
{
    class Projects : IEnumerable<KeyValuePair<string, Project>>
    {
        private Dictionary<string, Project> _map = new Dictionary<string, Project>();

        public IEnumerator<KeyValuePair<string, Project>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        internal void Add(Project pro)
        {
            if (!_map.TryGetValue(pro.ToString(), out Project project))
            {
                _map.Add(pro.ToString(), pro);
            }
        }

        internal Project Get(string name)
        {
            if (!_map.TryGetValue(name, out Project project)) return null;
            return project;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}
