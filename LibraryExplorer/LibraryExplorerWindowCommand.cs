//------------------------------------------------------------------------------
// <copyright file="LibraryExplorerWindowCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows;
using Microsoft.VisualStudio;

namespace LibraryExplorer
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class LibraryExplorerWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("8727d3a8-20ed-4925-b344-f911057ae865");
        public static readonly Guid TransferCommandSet = new Guid("33C85F04-749D-4C47-AD77-66919C1E6938");
        public static readonly Guid SearchCommandSet = new Guid("2FE8A97A-8006-4F2E-880C-933CE68CA34C");

        public const int ToolbarID = 0x1000;

        public const int cmdidRefresh = 0x301;
        public const int cmdidDownload = 0x201;
        public const int cmdidUpload = 0x202;

        /// <summary>
        /// Tool Window
        /// </summary>
        private LibraryExplorerWindow _window;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryExplorerWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private LibraryExplorerWindowCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this._package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static LibraryExplorerWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this._package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new LibraryExplorerWindowCommand(package);
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            
            _window = (LibraryExplorerWindow)_package.FindToolWindow(typeof(LibraryExplorerWindow), 0, true);
            if ((null == _window) || (null == _window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)_window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());

            CreateCommands();
        }

        /// <summary>
        /// Create and wire tool window commands to the backend
        /// </summary>
        private void CreateCommands()
        {
            var menuCommandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            var refreshCommandID = new CommandID(SearchCommandSet, cmdidRefresh);
            var downloadCommandID = new CommandID(TransferCommandSet, cmdidDownload);
            var uploadCommandID = new CommandID(TransferCommandSet, cmdidUpload);

            var refreshCommand = new MenuCommand(new EventHandler(RefreshHandler), refreshCommandID);
            var downloadCommand = new MenuCommand(new EventHandler(AddOrUpdatePackageInSolution), downloadCommandID);
            var uploadCommand = new MenuCommand(new EventHandler(AddOrUpdatePackageInLibrary), uploadCommandID);

            menuCommandService.AddCommand(refreshCommand);
            menuCommandService.AddCommand(downloadCommand);
            menuCommandService.AddCommand(uploadCommand);
        }

        /// <summary>
        /// Refresh all lists in view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private void RefreshHandler(object sender, EventArgs arguments)
        {
            _window.control.Refresh();
        }

        /// <summary>
        /// Copy the source code of a package in the library and add it to the current solution as a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private void AddOrUpdatePackageInSolution(object sender, EventArgs arguments)
        {
            IVsSolution solutionService = ServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            var selectedPackage = _window.control.GetSelectedLibraryPackage();

            if (selectedPackage == null)
            {
                return;
            }

            string solutionDirectoryPath, solutionFilePath, solutionUserOptionsFilePath;
            var solutionInfo = solutionService.GetSolutionInfo(out solutionDirectoryPath, out solutionFilePath, out solutionUserOptionsFilePath);

            if (solutionFilePath == null)
            {
                MessageBox.Show("No solution found. First open a solution or create a new one");
            }
            else
            {
                IntPtr proj;
                Guid projectType;
                Guid iidProject = Guid.Empty;

                solutionService.GetProjectTypeGuid(0, selectedPackage.ProjectLocation, out projectType);
                int result = solutionService.CreateProject(projectType, selectedPackage.ProjectLocation, null, null, (uint)__VSCREATEPROJFLAGS.CPF_OPENFILE, ref iidProject, out proj);

                ErrorHandler.ThrowOnFailure(result);
            }
        }

        //TODO: Upload a project to the library
        private void AddOrUpdatePackageInLibrary(object sender, EventArgs arguments)
        {
            MessageBox.Show("Upload not implemented");
        }
    }
}
