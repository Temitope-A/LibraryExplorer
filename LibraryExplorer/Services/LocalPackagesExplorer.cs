using System.Collections.Generic;
using LibraryExplorer.Models;
using System.IO;
using Newtonsoft.Json;

namespace LibraryExplorer.Services
{
    public class LibraryPackagesExplorer : ILibraryPackagesExplorer
    {
        private readonly DirectoryInfo _libraryFolder;

        public LibraryPackagesExplorer(string directoryPath)
        {
            _libraryFolder = new DirectoryInfo(directoryPath);
        }

        public IEnumerable<LibraryPackage> GetLibraryPackages()
        {
            foreach (var item in _libraryFolder.EnumerateFiles())
            {
                if (item.Extension == ".json")
                {
                    yield return JsonConvert.DeserializeObject<LibraryPackage>(item.OpenText().ReadToEnd());
                }
            }
        }
    }
}
