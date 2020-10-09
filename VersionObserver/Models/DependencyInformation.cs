using System;
namespace VersionObserver.Models
{
    public class DependencyInformation
    {
        public string Project { get; set; }
        public string Package { get; set; }
        public string Version { get; set; }
    }
}
