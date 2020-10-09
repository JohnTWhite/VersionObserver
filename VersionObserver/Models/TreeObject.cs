using System;
namespace VersionObserver.Models
{
    public class TreeObject
    {

        public class Rootobject
        {
            public string objectId { get; set; }
            public string url { get; set; }
            public Treeentry1[] treeEntries { get; set; }
            public int size { get; set; }
            public _Links _links { get; set; }
        }

        public class _Links
        {
            public Self self { get; set; }
            public Repository repository { get; set; }
            public Treeentry[] treeEntries { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Repository
        {
            public string href { get; set; }
        }

        public class Treeentry
        {
            public string href { get; set; }
        }

        public class Treeentry1
        {
            public string objectId { get; set; }
            public string relativePath { get; set; }
            public string mode { get; set; }
            public string gitObjectType { get; set; }
            public string url { get; set; }
            public int size { get; set; }
        }

    }
}
