using LibraryExplorer.Models;
using System.Collections.Generic;

namespace LibraryExplorer.Services
{
    /// <summary>
    /// Library explorer service
    /// </summary>
    public interface ILibraryExplorer
    {
        /// <summary>
        /// Retrieve metadata for all packages in the library
        /// </summary>
        IEnumerable<LibraryPackage> GetPackages();

        /// <summary>
        /// Get the path to the project in the source code
        /// </summary>
        /// <param name="packageName">package name</param>
        /// <returns>Path to the .csproj file</returns>
        string GetProjectLocation(string packageName);
    }
}
