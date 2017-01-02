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
        private readonly DirectoryInfo _sourceFolder;
        private readonly string _dotnetPlatform;

        /// <summary>
        /// Contructor
        /// </summary>
        public LibraryExplorer(string libraryPath, string sourceCodeFolderPath, string dotnetPlatform)
        {
            _libraryFolder = new DirectoryInfo(libraryPath);
            _sourceFolder = new DirectoryInfo(sourceCodeFolderPath);
            _dotnetPlatform = dotnetPlatform;

            if (!_libraryFolder.Exists)
            {
                throw new Exception("There is no package at the configured location");
            }
        }

        /// <summary>
        /// Retrieve all packages metadata in the library
        /// </summary>
        public IEnumerable<LibraryPackage> GetPackages()
        {
            foreach (var packageFolder in _libraryFolder.EnumerateDirectories())
            {
                var platformSpecificFolder = packageFolder.GetDirectories(_dotnetPlatform);

                if (platformSpecificFolder != null && platformSpecificFolder.Length != 0)
                {
                    var items = platformSpecificFolder[0].GetFiles("project.json");

                    if (items != null && items.Length == 1)
                    {
                        var item = items[0];
                        LibraryPackage package = null;
                        try
                        {
                            package = JsonConvert.DeserializeObject<LibraryPackage>(item.OpenText().ReadToEnd());
                        }
                        catch (Exception e)
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


        /// <summary>
        /// Get the path to the project in the source code
        /// </summary>
        /// <param name="packageName">package name</param>
        /// <returns>Path to the .csproj file</returns>
        public string GetProjectLocation(string packageName)
        {
            var candidateSolutionDirs = _sourceFolder.GetDirectories(packageName);

            if (candidateSolutionDirs.Length == 0)
            {
                throw new FileNotFoundException($"Could not find the source code for the package {packageName}");
            }
            else if (candidateSolutionDirs.Length > 1)
            {
                throw new FileNotFoundException($"Multiple source code packages matched the package {packageName}");
            }

            var projDir = new DirectoryInfo(Path.Combine(candidateSolutionDirs[0].FullName, "src", packageName));
            return projDir.GetFiles(packageName+".*proj")[0].FullName;
        }
    }
}