namespace TaskManagement
{
    public class Project
    {
        private string _name;

        public Project(string name)
        {
            this._name = name;
        }
        
        public override string ToString()
        {
            return _name;
        }
    }
}