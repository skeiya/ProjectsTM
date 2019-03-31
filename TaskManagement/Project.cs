namespace TaskManagement
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

        public override string ToString()
        {
            return _name;
        }
    }
}