using System.Collections.Generic;

namespace TaskManagement
{
    class Projects
    {
        private Dictionary<string, Project> _map = new Dictionary<string, Project>();

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
    }
}
