//------------------------------------------------------------------------------
// <copyright file="LibraryExplorerWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryExplorer
{
    using Models;
    using Services;
    using System.Windows.Controls;
    using ViewModels;

    /// <summary>
    /// Interaction logic for LibraryExplorerWindowControl.
    /// </summary>
    public partial class LibraryExplorerWindowControl : UserControl
    {
        /// <summary>
        /// The viewmodel
        /// </summary>
        private LibraryExplorerWindowViewModel _viewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryExplorerWindowControl"/> class.
        /// </summary>
        public LibraryExplorerWindowControl()
        {
            InitializeComponent();

            ILibraryPackagesExplorer explorer = new LibraryPackagesExplorer(@"C:\LibraryPackages");
            _viewModel = new LibraryExplorerWindowViewModel(explorer);
            DataContext = _viewModel;
        }

        /// <summary>
        /// Refresh all lists
        /// </summary>
        public void Refresh()
        {
            _viewModel.Refresh();
        }

        /// <summary>
        /// Get selected library package, returns null if no package is selected
        /// </summary>
        /// <returns></returns>
        public LibraryPackage GetSelectedLibraryPackage()
        {
            return _viewModel.SelectedLibraryPackage;
        }
    }
}