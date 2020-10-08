using System;
namespace VersionObserver.Models
{
    public class ItemsObject
    {
        public ItemsObject()
        {
        }
    }

    public class ItemsRootobject
    {
        public int count { get; set; }
        public Value[] value { get; set; }
    }

    public class ItemsValue
    {
        public string objectId { get; set; }
        public string gitObjectType { get; set; }
        public string commitId { get; set; }
        public string path { get; set; }
        public bool isFolder { get; set; }
        public string url { get; set; }
    }

}
