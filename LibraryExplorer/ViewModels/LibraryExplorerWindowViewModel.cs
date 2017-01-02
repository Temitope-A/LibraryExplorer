using LibraryExplorer.Models;
using LibraryExplorer.Services;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.ObjectModel;
using System;
using Microsoft.VisualStudio;
using System.Windows;
using System.Collections.Generic;
using LibraryExplorer.Helpers;

namespace LibraryExplorer.ViewModels
{
    public class LibraryExplorerWindowViewModel
    {
        #region private fields
        private readonly ILibraryExplorer _LibraryPackagesExplorer;
        private readonly IVsSolution _solutionService;
        private readonly IVsActivityLog _activityLogService;
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
        /// <param name="explorerService"></param>
        public LibraryExplorerWindowViewModel(ILibraryExplorer explorerService, IVsSolution solutionService, IVsActivityLog activityLogService)
        {
            _LibraryPackagesExplorer = explorerService;
            _solutionService = solutionService;
            _activityLogService = activityLogService;

            LibraryPackages = new ObservableCollection<LibraryPackage>();
            SolutionPackages = new ObservableCollection<LibraryPackage>();

            Refresh();
        }
        #endregion

        /// <summary>
        /// Copy the source code of a package in the library and add it to the current solution as a new project
        /// </summary>
        public void AddOrUpdatePackageInSolution()
        {
            if (SelectedLibraryPackage == null)
            {
                return;
            }

            string solutionDirectoryPath, solutionFilePath, solutionUserOptionsFilePath;
            var solutionInfo = _solutionService.GetSolutionInfo(out solutionDirectoryPath, out solutionFilePath, out solutionUserOptionsFilePath);

            if (solutionFilePath == null)
            {
                MessageBox.Show("No solution found. First open a solution or create a new one");
            }
            else
            {
                IntPtr proj;
                Guid projectType;
                Guid iidProject = Guid.Empty;
                string projectLocation = _LibraryPackagesExplorer.GetProjectLocation(SelectedLibraryPackage.Name);

                _solutionService.GetProjectTypeGuid(0, projectLocation, out projectType);
                int result = _solutionService.CreateProject(projectType, projectLocation, null, null, (uint)__VSCREATEPROJFLAGS.CPF_OPENFILE, ref iidProject, out proj);

                ErrorHandler.ThrowOnFailure(result);
            }

            Refresh();
        }

        /// <summary>
        /// Refresh all lists
        /// </summary>
        public void Refresh()
        {
            LibraryPackages.Clear();
            SolutionPackages.Clear();

            var solutionProjectGuids = GetProjectNamesInSolution();

            foreach (var item in _LibraryPackagesExplorer.GetPackages())
            {
                if (solutionProjectGuids.Contains(item.Name))
                {
                    SolutionPackages.Add(item);
                }
                else
                {
                    LibraryPackages.Add(item);
                }
            }
        }

        private List<string> GetProjectNamesInSolution()
        {
            var projectHierarchies = HierarchyHelper.GetProjects(_solutionService);
            var result = new List<string>();

            foreach (var proj in projectHierarchies)
            {
                result.Add(proj.Name);
            }

            return result;
        }
    }
}
