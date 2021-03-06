﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class Project
    {
        private readonly string _name;

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

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(Project));
            xml.Value = _name;
            return xml;
        }

        internal static Project FromXml(XElement w, int version)
        {
            if (version < 5)
            {
                if (w.Element("ProjectElement") != null)
                {
                    return new Project(w.Element("ProjectElement").Value);
                }
                return new Project(string.Empty);
            }
            return new Project(w.Element(nameof(Project)).Value);
        }
    }
}