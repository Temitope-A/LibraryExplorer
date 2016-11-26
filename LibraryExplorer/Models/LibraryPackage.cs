using System.Collections.Generic;

namespace LibraryExplorer.Models
{
    /// <summary>
    /// A library package, contains metadata about the package and its versions
    /// </summary>
    public class LibraryPackage
    {
        /// <summary>
        /// Name of the package
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Title of the package (for display)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type of the package
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Header line
        /// </summary>
        public string Headerline { get; set; }

        /// <summary>
        /// Authors of the package
        /// </summary>
        public List<string> Authors { get; set; }

        /// <summary>
        /// Current editor of the package. Processes bug reports and update requests
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Description of the functionality of classes, methods and services offered
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Versions of the package
        /// </summary>
        public IEnumerable<LibraryPackageVersion> Versions {get; set;}

        /// <summary>
        /// Location of the root directory for this package
        /// </summary>
        public string ProjectLocation { get; set; }
    }
}
