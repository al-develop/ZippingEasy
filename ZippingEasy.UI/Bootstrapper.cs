using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using ZippingEasy.UI.Views;
using log4net;
using log4net.Config;
using ZippingEasy.Common.Utils;
using LogManager = log4net.LogManager;

namespace ZippingEasy.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;
        private SimpleContainer _simpleContainer;
        public Bootstrapper()
        {
            this.Initialize();
            ConfigurateLogging();
        }

        private void ConfigurateLogging()
        {
            Uri configUri = new Uri($"{AppDomain.CurrentDomain.BaseDirectory}config/logconfig.xml");
            if (!File.Exists(configUri.OriginalString))
                throw new ConfigNotExistsException($"configuration file not found at: {configUri.AbsolutePath}");

            XmlConfigurator.Configure(configUri);
            LogManager.GetLogger(typeof(Bootstrapper)).Info("Logging configurated in Bootstrapper");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<MainViewModel>();
        }

        protected override void Configure()
        {
            _container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));
            CompositionBatch batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue(_container);
            
            _container.Compose(batch);
        }
    }
}