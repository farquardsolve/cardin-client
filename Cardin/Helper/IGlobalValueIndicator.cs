using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Helper
{
    public class IGlobalValueIndicator: INotifyPropertyChanged
    {
        bool _IIndicator;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IIndicator
        {
            get
            {
                return _IIndicator;
            }
            set
            {
                if (value != _IIndicator)
                {
                    _IIndicator = value;
                    NotifyPropertyChanged("IIndicator");
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            } 
        }
        
    }
}
