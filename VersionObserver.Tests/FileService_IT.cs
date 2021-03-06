﻿using System;
using System.IO;
using VersionObserver.Services;
using Xunit;


namespace VersionObserver.Tests
{
    public class FileService_IT
    {
        FileService testObject;
        public FileService_IT()
        {
            testObject = new FileService();
        }

        [Fact(Skip = "Integration Test")]
        public void GetFiles()
        {
            //arrange
            //Uses it's own csproj file, because why not?
            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();

            //act
            var result = testObject.GetCSProjFiles(filePath);

            //assert
            Assert.NotNull(result);
        }

        [Fact(Skip = "Integration Test")]
        public void CanDetermineDependencies()
        {
            //arrange
            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
            var projFiles = testObject.GetCSProjFiles(filePath);

            //act
            var result = testObject.GetDependenciesFromProjectFiles(projFiles);

            //assert
            Assert.NotNull(result);
        }

        [Fact(Skip = "Integration Test")]
        public void SaveDependencies()
        {
            //arrange
            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.ToString();
            var projFiles = testObject.GetCSProjFiles(filePath);
            var dependencies = testObject.GetDependenciesFromProjectFiles(projFiles);

            //act
            testObject.SaveDependencies(dependencies);

            //assert
        }
    }
}
