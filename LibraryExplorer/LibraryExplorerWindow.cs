//------------------------------------------------------------------------------
// <copyright file="LibraryExplorerWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace LibraryExplorer
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.ComponentModel.Design;
    using Microsoft.VisualStudio.Shell.Interop;
    using ViewModels;
    using Services;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("f7bba007-67ab-4aa1-a7c9-d8aaf7d3c6b7")]
    public class LibraryExplorerWindow : ToolWindowPane
    {

        public LibraryExplorerWindowControl control;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryExplorerWindow"/> class.
        /// </summary>
        public LibraryExplorerWindow() : base(null)
        {
            this.Caption = "Library Explorer";

            //Visual Studio services
            IVsActivityLog activityLogService = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsActivityLog)) as IVsActivityLog;
            IVsSolution solutionService = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;

            //Own services
            ILibraryExplorer libraryExplorerService = new LibraryExplorer(@"C:\LibraryPackages");

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var viewModel = new LibraryExplorerWindowViewModel(libraryExplorerService, solutionService, activityLogService);
            control = new LibraryExplorerWindowControl(viewModel);
            this.Content = control;

            this.ToolBar = new CommandID(LibraryExplorerWindowCommand.CommandSet, LibraryExplorerWindowCommand.ToolbarID);
            this.ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;
        }
    }
}
