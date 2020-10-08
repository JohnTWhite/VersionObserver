using System;
using Xunit;

namespace VersionObserver.Tests
{
    public class ObserverService_IT
    {
        ObserverService testObject;
        public ObserverService_IT()
        {
            testObject = new ObserverService();
        }

        [Fact]
        public void CanGetXMLFile()
        {
            //arrange

            //act
            var result = testObject.GetCSProjBlobAsync("https://dev.azure.com/ClearentTFS/fbb16616-6a11-45a0-bb94-efb708615cb8/_apis/git/repositories/4ff15bff-7955-4935-8b62-07825231608f/blobs/22bcf9480d65dceccbdee30547a8566eb3afdd8d");

            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GetReposGetsRepos()
        {
            //arrange

            //act
            var result = testObject.GetRepos("https://dev.azure.com/ClearentTFS/Clearent/_apis/git/repositories");

            //assert
            Assert.NotNull(result);
        }
    }
}
