using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace ZippingEasy.UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private MainViewModel vm;
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            try
            {
                vm = Caliburn.Micro.IoC.Get<MainViewModel>();
                vm._folderDialog = new Func<string>(OpenFolderBrosweDialog);
                vm._messageBox = new Action<string, string>(ShowMessageBox);
                this.DataContext = vm;
            }
            catch (Exception ex)
            {
                StringBuilder log = new StringBuilder();
                log.Append("Exception occured in MainView.cs");
                log.Append(ex.Message);
                if (ex.InnerException != null)
                    log.Append(ex.InnerException.Message);

                LogManager.GetLogger(typeof(MainView)).Error(log);
            }

            base.OnInitialized(e);
        }

        private void ShowMessageBox(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string OpenFolderBrosweDialog()
        {
            var browserDialog = new FolderBrowserDialog();

            var result = browserDialog.ShowDialog();
            if (result == DialogResult.OK)
                return browserDialog.SelectedPath;

            LogManager.GetLogger(typeof (MainView)).Warn("no directory selected. MainView.xaml.cs. Line 55. Method OpenFolderBrowseDialog");
            return null;
        }
    }
}