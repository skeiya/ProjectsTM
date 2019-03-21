using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    class Projects
    {
        private List<Project> _projects = new List<Project>();

        internal void Add(string name)
        {
            _projects.Add(new Project(name));
        }

        internal Project Get(string name)
        {
            return _projects.Find((p) => { return p.ToString().Equals(name); });
        }
    }
}
