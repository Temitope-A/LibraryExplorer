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
    }
}
