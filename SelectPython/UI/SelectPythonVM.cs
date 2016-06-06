using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SelectPython.Core;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;


namespace SelectPython.UI
{
    [Serializable]
    public class SelectPythonVM: ViewModel
    {
        private ObservableCollection<PythonVersionVM> pythons;
        [NonSerialized]
        private PythonVersionVM selectedPython;
        [NonSerialized]
        private ICommand addPythonCommand;
        [NonSerialized]
        private ICommand deletePythonCommand;
        [NonSerialized]
        private ICommand applyPythonCommand;
        [NonSerialized]
        private ICommand detectPythonCommand;

        public SelectPythonVM()
        {
            pythons = new ObservableCollection<PythonVersionVM>();
            UpdateCommands();
        }

        public ObservableCollection<PythonVersionVM> Pythons
        {
            get { return pythons; }
            set
            {
                pythons = value;
                OnPropertyChanged("Pythons");
            }
        }

        public PythonVersionVM SelectedPython
        {
            get { return selectedPython; }
            set
            {
                selectedPython = value;
                OnPropertyChanged("SelectedPythons");
                UpdateCanExecute();
            }
        }

        public ICommand AddPythonCommand
        {
            get { return addPythonCommand; }
            private set
            {
                addPythonCommand = value;
                OnPropertyChanged("AddPythonCommand");
            }
        }

        public ICommand DeletePythonCommand
        {
            get { return deletePythonCommand; }
            private set
            {
                deletePythonCommand = value;
                OnPropertyChanged("DeletePythonCommand");
            }
        }

        public ICommand ApplyPythonCommand
        {
            get { return applyPythonCommand; }
            private set
            {
                applyPythonCommand = value;
                OnPropertyChanged("ApplyPythonCommand");
            }
        }

        public ICommand DetectPythonCommand
        {
            get { return detectPythonCommand; }
            private set
            {
                detectPythonCommand = value;
                OnPropertyChanged("DetectPythonCommand");
            }
        }

        override protected void UpdateCommands()
        {
            if (AddPythonCommand == null)
            {
                AddPythonCommand = new RelayCommand(OnAddPython);
            }
            if (DeletePythonCommand == null)
            {
                DeletePythonCommand = new RelayCommand(OnDeletePython, CanPython);
            }
            if (ApplyPythonCommand == null)
            {
                ApplyPythonCommand = new RelayCommand(OnApplyPython, CanPython);
            }
            if (DetectPythonCommand == null)
            {
                DetectPythonCommand = new RelayCommand(OnDetectPython);
            }
        }

        private void UpdateCanExecute()
        {
            ((RelayCommand)AddPythonCommand).OnCanExecutedChanged();
            ((RelayCommand)DeletePythonCommand).OnCanExecutedChanged();
            ((RelayCommand)ApplyPythonCommand).OnCanExecutedChanged();
            ((RelayCommand)DetectPythonCommand).OnCanExecutedChanged();
        }

        private Boolean CanPython(Object o)
        {
            return SelectedPython != null;
        }

        private void OnAddPython(Object o)
        {
            var dlg = new OpenFileDialog();

            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Executable files (*.exe)|*.exe";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                var filePath = dlg.FileName;
                if (filePath.Contains("python.exe"))
                {
                    var pvm = new PythonVersionVM();
                    Pythons.Add(pvm);
                    pvm.PythonPath = Path.GetDirectoryName(filePath);
                }
            }
        }

        private void OnDeletePython(Object o)
        {
            Pythons.Remove(SelectedPython);
        }

        private void OnApplyPython(Object o)
        {
            foreach(var p in Pythons)
            {
                p.VersionApplied = false;
            }
            SelectedPython.VersionApplied = true;

            var target = EnvironmentVariableTarget.User;

            var name = "PATH";
            var value = Environment.GetEnvironmentVariable(name, target);

            var variables = value.Split(';');

            var newVariables = new List<String> { SelectedPython.PythonPath, SelectedPython.PyqtPath, SelectedPython.VtkPath, SelectedPython.ScriptsPath};
            foreach (var v in variables)
            {
                var lv = v.ToLower();
                if (!lv.Contains("win") && !lv.Contains("python"))
                {
                    newVariables.Add(v);
                }
            }

            var newValue = String.Join(";", newVariables);
            Environment.SetEnvironmentVariable(name, newValue, target);
        }

        private void OnDetectPython(Object o)
        {
            var foundPythons = PythonDetector.Find();
            if (foundPythons.Count > 0)
            {
                Pythons.Clear();
            }
            foreach(var p in foundPythons)
            {
                Pythons.Add(new PythonVersionVM { PythonPath = p });
            }
        }
    }
}
