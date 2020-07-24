using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class Project
    {
        private string _name;

        public Project(string name)
        {
            this._name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is Project project &&
                   _name == project._name;
        }

        public override int GetHashCode()
        {
            return -1125283371 + EqualityComparer<string>.Default.GetHashCode(_name);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}