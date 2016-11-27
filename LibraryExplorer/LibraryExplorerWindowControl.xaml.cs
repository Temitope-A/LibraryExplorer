//------------------------------------------------------------------------------
// <copyright file="LibraryExplorerWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryExplorer
{
    using Models;
    using Services;
    using System;
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
        public LibraryExplorerWindowControl(LibraryExplorerWindowViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        /// <summary>
        /// Copy the source code of a package in the library and add it to the current solution as a new project
        /// </summary>
        public void AddOrUpdatePackageInSolution(object sender, EventArgs arguments)
        {
            _viewModel.AddOrUpdatePackageInSolution();
        }

        //TODO: Upload a project to the library
        public void AddOrUpdatePackageInLibrary(object sender, EventArgs arguments)
        { }

        /// <summary>
        /// Refresh all lists
        /// </summary>
        public void Refresh(object sender, EventArgs arguments)
        {
            _viewModel.Refresh();
        }
    }
}