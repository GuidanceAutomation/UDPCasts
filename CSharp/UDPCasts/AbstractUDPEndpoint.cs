using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UDPCasts
{
    public class AbstractUDPEndpoint : INotifyPropertyChanged
    {
        public AbstractUDPEndpoint()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}