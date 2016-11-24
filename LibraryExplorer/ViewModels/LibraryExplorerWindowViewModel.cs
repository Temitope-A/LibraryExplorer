using LibraryExplorer.Models;
using LibraryExplorer.Services;
using System.Collections.ObjectModel;

namespace LibraryExplorer.ViewModels
{
    public class LibraryExplorerWindowViewModel
    {
        #region private fields
        private readonly ILibraryPackagesExplorer _LibraryPackagesExplorer;
        #endregion

        #region properties
        /// <summary>
        /// Packages available in the library
        /// </summary>
        public ObservableCollection<LibraryPackage> LibraryPackages { get; set; }

        /// <summary>
        /// Packages imported into the solution
        /// </summary>
        public ObservableCollection<LibraryPackage> SolutionPackages { get; set; }

        /// <summary>
        /// Currently selected library package
        /// </summary>
        public LibraryPackage SelectedLibraryPackage { get; set; }

        /// <summary>
        /// Currently selected solution package
        /// </summary>
        public LibraryPackage SelectedSolutionPackage { get; set; }

        #endregion

        #region life cycle
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="explorer"></param>
        public LibraryExplorerWindowViewModel(ILibraryPackagesExplorer explorer)
        {
            _LibraryPackagesExplorer = explorer;

            LibraryPackages = new ObservableCollection<LibraryPackage>();
            SolutionPackages = new ObservableCollection<LibraryPackage>();

            Refresh();
        }
        #endregion

        /// <summary>
        /// Refresh all lists
        /// </summary>
        public void Refresh()
        {
            RefreshLibrary();
            RefreshSolution();
        }

        /// <summary>
        /// Refresh library lists
        /// </summary>
        private void RefreshLibrary()
        {
            LibraryPackages.Clear();

            foreach (var item in _LibraryPackagesExplorer.GetLibraryPackages())
            {
                LibraryPackages.Add(item);
            }
        }

        /// <summary>
        /// Refresh solution lists
        /// </summary>
        private void RefreshSolution()
        {
            SolutionPackages.Clear();
        }
    }
}
