using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LinearOrdering.Parser
{
    class FileManager
    {
        string path;
        StreamReader sr;

        public FileManager(string filename)
        {
            path = AppDomain.CurrentDomain.BaseDirectory;
            sr = new StreamReader(path + filename);            
        }

        public List<string> ReadFileLine()
        {
            string line = "";
            List<string> lr = new List<string>();

            if (!sr.EndOfStream)
            {
                line = sr.ReadLine().Trim();
                lr.AddRange(line.Split(' '));
            }

            return lr;
        }

        public void SaveFile(object o)
        {
            //StreamWriter sw = new StreamWriter(path + "GLPK");
            //sw.Write(o);
            //sw.WriteLine();
            //sw.Close();
        }

    }
}
