namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Xamarin.Forms;

    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusyValue = false;

        private string baseTitle = string.Empty;

        public INavigation Navigation { get; }

        public bool IsBusy
        {
            get { return this.isBusyValue; }
            set { this.SetProperty(ref this.isBusyValue, value); }
        }

        public string BaseTitle
        {
            get { return this.baseTitle; }
            set { this.SetProperty(ref this.baseTitle, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
