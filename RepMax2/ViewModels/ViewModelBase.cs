using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private readonly SemaphoreSlim _isBusyLock = new(1, 1);
        private bool _disposedValue;

        private bool _isInitialized;
        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        private bool _isBusy;
        public bool IsBusy 
        { 
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public ViewModelBase() 
        { 
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query) 
        {
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task IsBusyFor(Func<Task> unitOfWork)
        {
            await _isBusyLock.WaitAsync();

            try
            {
                IsBusy = true;
                await unitOfWork();
            }
            finally
            {
                IsBusy = false;
                _isBusyLock.Release();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _isBusyLock?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
