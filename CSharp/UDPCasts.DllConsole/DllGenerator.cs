using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UDPCasts.DllConsole
{
    public class DllGenerator
    {
        private readonly object lockObject = new object();

        private List<string> projects = new List<string>();

        public DllGenerator()
        {
        }

        public void Generate(bool isRelease = false, string outputDir = null, string sourceDir = null)
        {
            if (!isRelease)
            {
                MessageBox.Show("Warning! Building in Debug mode!", "WARNING", MessageBoxButtons.OK);
                Console.WriteLine("Building in Debug mode!");
            }

            if (outputDir == null)
            {
                outputDir = Path.GetTempPath() + "\\" + Guid.NewGuid().ToString();
            }

            if (!Directory.Exists(outputDir))
            {
                DirectoryInfo info = Directory.CreateDirectory(outputDir);
            }

            if (sourceDir == null)
            {
                sourceDir = Directory.GetCurrentDirectory();
            }

            Console.WriteLine("[DllGenerator] sourceDir: {0}", sourceDir);
            Console.WriteLine("[DllGenerator] outputDir: {0}", outputDir);

            lock (lockObject)
            {
                Console.WriteLine("Generating .dll's:");

                foreach (string project in projects)
                {
                    Console.WriteLine("\t" + project);
                }

                bool success = true;

                foreach (string project in projects)
                {
                    string fileName = sourceDir + "\\" + project;
                    string outputName = outputDir + "\\" + project;

                    if (File.Exists(fileName))
                    {
                        try
                        {
                            File.Copy(fileName, outputName, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("FATAL: {0}", ex.Message);
                            success = false;
                        }
                    }
                    else
                    {
                        string error = "[DLLGenerator] File does not exist: \n\t" + fileName;
                        Console.WriteLine(error);
                        return;
                    }
                }

                if (success)
                {
                    Process.Start(outputDir);
                }
                else
                {
                    Console.WriteLine(".dll generation failure", "[DllGenerator]");
                }
            }
        }

        public void AddProjects(IEnumerable<string> projects)
        {
            lock (lockObject)
            {
                this.projects.AddRange(ValidateStrings(projects));
            }
        }

        private string ValidateString(string project)
        {
            return project = project + ".dll";
        }

        private List<String> ValidateStrings(IEnumerable<string> projects)
        {
            List<string> validated = new List<string>();

            foreach (string project in projects)
            {
                validated.Add(ValidateString(project));
            }

            return validated;
        }

        public void AddProject(string project)
        {
            AddProjects(new string[] { project });
        }

        public IEnumerable<string> Projects { get { return projects.ToList(); } }
    }
}
