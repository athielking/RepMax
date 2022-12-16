using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.ViewModels
{
    public interface IViewModelBase : IQueryAttributable
    {
        public bool IsBusy { get; }
        public bool IsInitialized { get; set; }

        Task InitializeAsync();
    }
}
