using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelectPython.UI
{
    public partial class SelectPythonWindow
    {
        public static readonly DependencyProperty SelectPythonVMProperty =
            DependencyProperty.Register(
                "SelectPythonVM",
                typeof(SelectPythonVM),
                typeof(SelectPythonWindow),
                new PropertyMetadata(new SelectPythonVM()));

        public SelectPythonWindow()
        {
            InitializeComponent();

            var vm = LoadVM();
            if (vm != null)
            {
                SelectPythonVM = vm;
            }
        }

        public SelectPythonVM SelectPythonVM
        {
            get { return (SelectPythonVM)GetValue(SelectPythonVMProperty); }
            set { SetValue(SelectPythonVMProperty, value); }
        }

        private void AddPythonButton_Click(object sender, RoutedEventArgs e)
        {
            SelectPythonVM.Pythons.Add(new PythonVersionVM());
        }

        private void SaveVM()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, SelectPythonVM);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default.SelectPythonVM = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }

        private SelectPythonVM LoadVM()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.SelectPythonVM)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (ms.Length == 0)
                {
                    return null;
                }
                else
                {
                    return (SelectPythonVM)bf.Deserialize(ms);
                }
            }
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveVM();
        }
    }
}
