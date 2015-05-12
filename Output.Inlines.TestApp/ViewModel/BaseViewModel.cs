using System;
using System.ComponentModel;

namespace Markout.Output.Inlines.TestApp.ViewModel {

    public abstract class BaseViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler h = PropertyChanged;
            if(h != null) {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}