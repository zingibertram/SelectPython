using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SelectPython.Core
{
    [Serializable]
    public class ViewModel: INotifyPropertyChanged
    {
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        virtual protected  void OnPropertyChanged(String propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        virtual protected void UpdateCommands()
        {
        }

        virtual protected void UpdateData()
        {
        }

        [OnDeserialized]
        protected void OnDeserializedMethod(StreamingContext context)
        {
            UpdateCommands();
        }
    }
}
