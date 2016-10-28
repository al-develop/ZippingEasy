using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Mvvm;

namespace ZippingEasy.UI.Views
{
    public class AboutViewModel : Screen
    {
        public AboutViewModel()
        {
            this.DisplayName = "About";
            CloseCommand = new DelegateCommand(Close);
        }

        public ICommand CloseCommand { get; set; }

        private void Close()
        {
            this.TryClose();
        }
    }
}