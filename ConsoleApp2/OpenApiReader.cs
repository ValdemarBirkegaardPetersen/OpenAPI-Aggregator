using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class OpenApiReader
    {

        public OpenApiReader ()
        {

        }

        public IEnumerable<string> FindDocumentPaths()
        {
            string CurrentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string[] FileExtensions = { ".yaml", ".yml" };
            IEnumerable<string> DocumentFilePaths = Directory.EnumerateFiles(CurrentDirectory, ".", SearchOption.AllDirectories)
                .Where(x => FileExtensions.Any(x.EndsWith));

            return DocumentFilePaths;
        }

        public List<OpenApiDocument> ReadDocumentFiles ()
        {
            List<OpenApiDocument> ListOfOpenApiDocuments = new List<OpenApiDocument>();
            foreach (string DocumentFile in FindDocumentPaths())
            {
                StreamReader CurrentStream = new StreamReader(DocumentFile);
                OpenApiDocument CurrentOpenApiDocument = new OpenApiStreamReader().Read(CurrentStream.BaseStream, out var diagnostic);

                ListOfOpenApiDocuments.Add(CurrentOpenApiDocument);
                CurrentStream.Dispose();
            }
            
            return ListOfOpenApiDocuments;
        }
    }
}
