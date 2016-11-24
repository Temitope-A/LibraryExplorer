namespace LibraryExplorer.Models
{
    /// <summary>
    /// Version information for a library package
    /// </summary>
    public class LibraryPackageVersion
    {
        /// <summary>
        /// Version number
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Description of the change set
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Location of the nuget package
        /// </summary>
        public string NugetPackageLocation { get; set; }
    }
}
