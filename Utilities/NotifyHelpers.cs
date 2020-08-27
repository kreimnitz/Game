using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utilities
{
    public interface IAutoNotifyPropertyChanged : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }

    public static class NotifyHelpers
    {
        public static bool SetProperty<T>(IAutoNotifyPropertyChanged owner, ref T storage, T value, string[] dependentPropertyNames = null, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            owner.RaisePropertyChanged(propertyName);

            if (dependentPropertyNames != null)
            {
                foreach (var dependentPropertyName in dependentPropertyNames)
                {
                    owner.RaisePropertyChanged(dependentPropertyName);
                }
            }

            return true;
        }
    }
}
