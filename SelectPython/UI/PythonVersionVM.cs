using System;
using System.Collections.Generic;
using System.IO;
using SelectPython.Core;

namespace SelectPython.UI
{
    [Serializable]
    public enum Platform
    {
        x32, x64
    }

    [Serializable]
    public enum PyqtVersion
    {
        Qt4, Qt5
    }

    [Serializable]
    public class PythonVersionVM: ViewModel
    {
        private String pythonVersion;
        private String pythonPath;
        private PyqtVersion pyqtVersion;
        private String pyqtLib;
        private Platform platform;
        private Boolean editable;
        private String saveEdit;
        private Boolean versionApplied;

        public PythonVersionVM()
        {
            editable = true;
            saveEdit = "Save";
        }

        public String PythonVersion
        {
            get { return pythonVersion; }
            set
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
            }
        }

        public PyqtVersion PyqtVersion
        {
            get { return pyqtVersion; }
            set
            {
                pyqtVersion = value;
                switch(value)
                {
                    case PyqtVersion.Qt4:
                        PyqtLib = "PyQt4";
                        break;
                    case PyqtVersion.Qt5:
                        PyqtLib = "PyQt5";
                        break;
                }
                OnPropertyChanged("PyqtVersion");
            }
        }

        public String PyqtLib
        {
            get { return pyqtLib; }
            set
            {
                pyqtLib = value;
                OnPropertyChanged("PyqtLib");
            }
        }

        public Platform Platform
        {
            get { return platform; }
            set
            {
                platform = value;
                OnPropertyChanged("Platform");
            }
        }

        public Boolean Editable
        {
            get { return editable; }
            set
            {
                editable = value;
                if (value)
                {
                    SaveEdit = "Save";
                }
                else
                {
                    SaveEdit = "Edit";
                }
                OnPropertyChanged("Editable");
            }
        }

        public String SaveEdit
        {
            get { return saveEdit; }
            set
            {
                saveEdit = value;
                OnPropertyChanged("SaveEdit");
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

        public void Apply(object o)
        {
            var pyqtPath = Path.Combine(new string[] { PythonPath, "Lib", "site-packaged", PyqtLib });
            var vtkPath = Path.Combine(new string[] { PythonPath, "Lib", "site-packaged", PyqtLib });
            var scriptsPath = Path.Combine(new string[] { PythonPath, "Scripts" });

            var target = EnvironmentVariableTarget.User;

            var name = "PATH";
            var value = Environment.GetEnvironmentVariable(name, target);

            var variables = value.Split(';');

            var newVariables = new List<String>();
            foreach(var v in variables)
            {
                var lv = v.ToLower();
                if (!lv.Contains("win") && !lv.Contains("python"))
                {
                    newVariables.Add(v);
                }
            }
            var newValue = String.Join(";", newVariables);
            Environment.SetEnvironmentVariable(newValue, value);

            VersionApplied = true;
        }
    }
}
