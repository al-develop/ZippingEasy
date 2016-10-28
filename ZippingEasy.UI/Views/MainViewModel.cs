﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Mvvm;
using SevenZip;
using ZippingEasy.Common.Entity;
using ZippingEasy.Common.Utils;
using ZippingEasy.Logic;
using Action = System.Action;
using LogManager = log4net.LogManager;

namespace ZippingEasy.UI.Views
{
    public class MainViewModel : Screen
    {
        private IZipLogic _logic;
        private IWindowManager _windowManager;
        public Func<string> _folderDialog;
        public Action<string, string> _messageBox;

        private string _sourcePath;
        private string _destinationPath;
        private CompressionLevel _compressionLevel;
        private ObservableCollection<SelectedFile> _selectedFolders;

        #region Properties
        public ICommand SelectSourceCommand { get; set; }
        public ICommand SelectDestinationCommand { get; set; }
        public ICommand BeginCompressionCommand { get; set; }
        public ICommand AboutCommand { get; set; }


        public ObservableCollection<SelectedFile> SelectedFolders
        {
            get { return _selectedFolders; }
            set
            {
                _selectedFolders = value;
                this.NotifyOfPropertyChange(() => this.SelectedFolders);
            }
        }
        public CompressionLevel CompressionLevel
        {
            get { return _compressionLevel; }
            set
            {
                _compressionLevel = value;
                this.NotifyOfPropertyChange(() => CompressionLevel);
            }
        }
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                _destinationPath = value;
                this.NotifyOfPropertyChange(() => DestinationPath);
            }
        }
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
                this.NotifyOfPropertyChange(() => SourcePath);
                if(!String.IsNullOrEmpty(this.SourcePath))
                    LoadSelectedPaths();
            }
        }
        #endregion Properties

        #region Constructor
        public MainViewModel()
        {
            _windowManager = new WindowManager();
            Initiliaztion();
        }

        public MainViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            Initiliaztion();
        }

        private void Initiliaztion()
        {
            _logic = Factory.GetZipLogic();
            CommandInit();
        }
        #endregion Constructor

        #region Methods
        private void CommandInit()
        {
            SelectSourceCommand = new DelegateCommand(SelectSource);
            SelectDestinationCommand = new DelegateCommand(SelectDestination);
            BeginCompressionCommand = new AsyncCommand(BeginCompression);
            AboutCommand = new DelegateCommand(About);
        }

        private void About()
        {
            var about = IoC.Get<AboutViewModel>();
            _windowManager.ShowDialog(about);
        }

        private async Task<Result> BeginCompression()
        {
            var result = await _logic.Zip(this.SourcePath, this.DestinationPath, this.CompressionLevel);
            if (result.Status != ResultStatus.Success)
            {
                string message = $"An error occured: {Environment.NewLine}{result.Message}{Environment.NewLine}Check log file for more information";
                _messageBox.Invoke(message, "Error occured");
            }
            return result;
        }

        private void SelectDestination()
        {
            var selectedPath = _folderDialog.Invoke();
            if (!String.IsNullOrEmpty(selectedPath))
                this.DestinationPath = selectedPath;
        }

        private void SelectSource()
        {
            var selectedPath = _folderDialog.Invoke();
            if (!String.IsNullOrEmpty(selectedPath))
                this.SourcePath = selectedPath;
        }

        private void LoadSelectedPaths()
        {
            if (this.SelectedFolders == null)
                this.SelectedFolders = new ObservableCollection<SelectedFile>();

            if (String.IsNullOrEmpty(this.SourcePath))
            {
                LogManager.GetLogger(typeof (MainViewModel)).Warn("couldn't load subfolders because the source path was null or empty");
            }

            IEnumerable<string> subfolders = this._logic.GetSubfolders(this.SourcePath);
            foreach (var folder in subfolders)
            {
                this.SelectedFolders.Add(new SelectedFile()
                {
                    FilePath = folder,
                    ZipProgress = 0
                });
            }
        }

        #endregion Methods
    }
}