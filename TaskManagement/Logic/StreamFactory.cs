using System;
using System.IO;
using System.Text;

namespace TaskManagement.Logic
{
    public class StreamFactory
    {
        public static StreamWriter CreateWriter(string path)
        {
            return new StreamWriter(path, false, Encoding.UTF8);
        }

        public static StreamWriter CreateWriter(Stream s)
        {
            return new StreamWriter(s, Encoding.UTF8);
        }

        public static StreamReader CreateReader(string path)
        {
            return new StreamReader(path, Encoding.UTF8);
        }

        public static StreamReader CreateReader(Stream s)
        {
            return new StreamReader(s, Encoding.UTF8);
        }
    }
}
