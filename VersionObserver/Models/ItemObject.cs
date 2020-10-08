using System;
namespace VersionObserver.Models
{
    public class ItemObject
    {
        public ItemObject()
        {
        }
    }

    public class ItemRootobject
    {
        public string objectId { get; set; }
        public string gitObjectType { get; set; }
        public string commitId { get; set; }
        public string path { get; set; }
        public bool isFolder { get; set; }
        public string url { get; set; }
        public _Links _links { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
        public Repository repository { get; set; }
        public Tree tree { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Repository
    {
        public string href { get; set; }
    }

    public class Tree
    {
        public string href { get; set; }
    }

}
