using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Dapper;
using VersionObserver.Models;

namespace VersionObserver.Services
{
    public class FileService
    {
        public FileService()
        {
        }

        public IEnumerable<CSProjFile> GetCSProjFiles(string folderPath)
        {
            List<CSProjFile> result = new List<CSProjFile>();
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(file);
                result.Add(new CSProjFile() { xmlDocument = xml, ProjectName = xml.BaseURI.Split("/").Last() });
            }
            return result;
        }
        
        public void SaveDependencies(IEnumerable<DependencyInformation> dependencies)
        {
            var _connectionString = "Data Source=chqdev04;Initial Catalog=Packages;Integrated Security=true;Connection Timeout=25;";
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var transaction = conn.BeginTransaction();

                try
                {
                    var sql = "INSERT INTO[dbo].[CSProjPackages]([Project],[Package],[Version],[ObservationDate])"+
                                "VALUES (@Project, @Package, @Version, GETDATE())";

                    var result = conn.Execute(sql, dependencies, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public IEnumerable<DependencyInformation> GetDependenciesFromProjectFiles(IEnumerable<CSProjFile> csprojFiles)
        {
            IList<DependencyInformation> results = new List<DependencyInformation>();
            foreach (var cprojFile in csprojFiles)
            {
                var projFile = cprojFile.xmlDocument;
                if (projFile != null)
                {
                    XmlNodeList projectGroup = projFile.GetElementsByTagName("Project");
                    XmlNodeList itemGroup = projFile.GetElementsByTagName("PackageReference");

                    foreach (XmlNode packageItem in itemGroup)
                    {
                        if (projectGroup[0] != null
                            && projectGroup[0].BaseURI != null
                            && packageItem.Attributes.Count > 1)
                        {
                            results.Add(new DependencyInformation()
                            {
                                Project = cprojFile.ProjectName,
                                Package = packageItem.Attributes[0].InnerText,
                                Version = packageItem.Attributes[1].InnerText
                            });
                        }
                    };
                }
            };
            return results;
        }
    }
}
