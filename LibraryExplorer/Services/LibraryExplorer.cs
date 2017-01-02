using System.Collections.Generic;
using LibraryExplorer.Models;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace LibraryExplorer.Services
{
    /// <summary>
    /// Library explorer service
    /// </summary>
    public class LibraryExplorer : ILibraryExplorer
    {
        private readonly DirectoryInfo _libraryFolder;

        /// <summary>
        /// Contructor
        /// </summary>
        public LibraryExplorer(string libraryPath)
        {
            _libraryFolder = new DirectoryInfo(libraryPath);
            if (!_libraryFolder.Exists)
            {
                throw new System.Exception("There is no package at the configured location");
            }
        }

        /// <summary>
        /// Retrieve all packages metadata in the library
        /// </summary>
        public IEnumerable<LibraryPackage> GetPackages()
        {
            foreach (var packageFolder in _libraryFolder.EnumerateDirectories())
            {
                var items = packageFolder.GetFiles("package.json");
                if (items != null && items.Length == 1)
                {
                    var item = items[0];
                    LibraryPackage package = null;
                    try
                    {
                        package = JsonConvert.DeserializeObject<LibraryPackage>(item.OpenText().ReadToEnd());
                    }
                    catch(Exception e)
                    {
                        //TODO: print to output
                        Debug.Print($"Could not find a valid package json fle in {item.FullName}: {e.Message}");
                    }
                    if (package != null)
                    {
                        yield return package;
                    }
                }
            }
        }
    }
}