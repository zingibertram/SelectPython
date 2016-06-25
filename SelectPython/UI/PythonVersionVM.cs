using System;
using System.IO;
using System.Diagnostics;
using SelectPython.Core;
using System.Windows;

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
            var info = PythonInfo.Get(PythonPath);
            if (info != null)
            {
                PythonVersion = info[1];
                Platform = info[2];
                PyqtVersion = info[3];
            }
        }
    }
}
