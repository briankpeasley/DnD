using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapReveal
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetProperty(object value, [CallerMemberName] string caller = "")
        {
            _properties[caller] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public Type GetProperty<Type>([CallerMemberName] string caller = "")
        {
            if (_properties.ContainsKey(caller))
            {
                return (Type)_properties[caller];
            }

            return default(Type);
        }

        protected void Invoke(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
