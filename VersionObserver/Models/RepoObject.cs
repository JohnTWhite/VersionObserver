using System;
namespace VersionObserver.Models
{
    public class RepoObject
    {
        public RepoObject()
        {
        }
    }
    public class Rootobject
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public string remoteUrl { get; set; }
        public string sshUrl { get; set; }
        public string webUrl { get; set; }
        public string defaultBranch { get; set; }
        public int size { get; set; }
        public bool isFork { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }
}
