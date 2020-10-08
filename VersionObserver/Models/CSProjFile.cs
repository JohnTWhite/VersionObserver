using System;
using System.Xml;

namespace VersionObserver.Models
{
    public class CSProjFile
    {
        public string ProjectName { get; set; }
        public XmlDocument xmlDocument { get; set; }
    }
}
