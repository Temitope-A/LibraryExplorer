using LibraryExplorer.Models;
using System.Collections.Generic;

namespace LibraryExplorer.Services
{
    public interface ILibraryPackagesExplorer
    {
        IEnumerable<LibraryPackage> GetLibraryPackages();
    }
}
