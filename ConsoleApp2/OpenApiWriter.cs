using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class OpenApiWriter
    {
        public OpenApiWriter()
        {

        }

        public void WriteOutputToFile(OpenApiDocument InputDocument)
        {
            var SerialzedDocument = InputDocument.SerializeAsYaml(OpenApiSpecVersion.OpenApi3_0);
            string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            using (StreamWriter sw = File.CreateText(ProjectDirectory + @"\output.yml"))
            {
                sw.Write(SerialzedDocument);
                Console.WriteLine("Created new file in project directory named 'output.yml'");
            }
        }

        public void WriteOutputToConsole(OpenApiDocument InputDocument)
        {
            var SerialzedDocument = InputDocument.SerializeAsYaml(OpenApiSpecVersion.OpenApi3_0);
            Console.WriteLine(SerialzedDocument);
        }

        public string SaveOutputAsString(OpenApiDocument InputDocument)
        {
            var SerialzedDocument = InputDocument.SerializeAsYaml(OpenApiSpecVersion.OpenApi3_0);
            return SerialzedDocument;
        }
    }
}
