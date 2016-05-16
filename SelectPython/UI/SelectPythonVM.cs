using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SelectPython.Core;


namespace SelectPython.UI
{
    [Serializable]
    public class SelectPythonVM: ViewModel
    {
        private ObservableCollection<PythonVersionVM> pythons;

        public SelectPythonVM()
        {
            pythons = new ObservableCollection<PythonVersionVM>();
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
    }
}
