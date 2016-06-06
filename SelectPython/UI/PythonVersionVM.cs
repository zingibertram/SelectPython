using System;
using System.IO;
using System.Diagnostics;
using SelectPython.Core;

namespace SelectPython.UI
{

    [Serializable]
    public class PythonVersionVM: ViewModel
    {
        private String pythonVersion;
        private String pythonPath;
        private String pyqtVersion;
        private String platform;
        private Boolean versionApplied;
        private String pyqtPath;
        private String vtkPath;
        private String scriptsPath;

        public PythonVersionVM()
        {
            UpdateCommands();
        }

        public String PythonVersion
        {
            get { return pythonVersion; }
            private set
            {
                pythonVersion = value;
                OnPropertyChanged("PythonVersion");
            }
        }

        public String PythonPath
        {
            get { return pythonPath; }
            set
            {
                pythonPath = value;
                OnPropertyChanged("PythonPath");

                UpdatePythonVersion();
                UpdateLibPath();
            }
        }

        public String PyqtVersion
        {
            get { return pyqtVersion; }
            private set
            {
                pyqtVersion = value;
                OnPropertyChanged("PyqtVersion");
            }
        }

        public String Platform
        {
            get { return platform; }
            private set
            {
                platform = value;
                OnPropertyChanged("Platform");
            }
        }

        public Boolean VersionApplied
        {
            get { return versionApplied; }
            set
            {
                versionApplied = value;
                OnPropertyChanged("VersionApplied");
            }
        }

        public String PyqtPath
        {
            get { return pyqtPath; }
        }

        public String ScriptsPath
        {
            get { return scriptsPath; }
        }

        public String VtkPath
        {
            get { return vtkPath; }
        }

        private void UpdateLibPath()
        {
            pyqtPath = Path.Combine(new string[] { PythonPath, "Lib", "site-packages", PyqtVersion });
            vtkPath = Path.Combine(new string[] { PythonPath, "Lib", "site-packages", "vtk" });
            scriptsPath = Path.Combine(new string[] { PythonPath, "Scripts" });
            OnPropertyChanged("PyqtPath");
            OnPropertyChanged("ScriptsPath");
            OnPropertyChanged("VtkPath");
        }

        private void UpdatePythonVersion()
        {
            var pyPath = Path.Combine(PythonPath, "python.exe");
            if (File.Exists(pyPath))
            {
                var verScriptPath = Path.Combine(new String[] { AppDomain.CurrentDomain.BaseDirectory, "Core", "Python", "PythonVersion.py" });

                var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = pyPath;
                process.StartInfo.Arguments = verScriptPath;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                String verInfo = process.StandardOutput.ReadToEnd();
                verInfo = verInfo.Replace("\r\n", "");
                verInfo = verInfo.Replace("(", "");
                verInfo = verInfo.Replace(")", "");
                var infos = verInfo.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                PythonVersion = infos[0];
                Platform = infos[1];
                PyqtVersion = infos[2];
            }
        }
    }
}
