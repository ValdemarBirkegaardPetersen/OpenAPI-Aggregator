using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using System;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            OpenApiReader reader = new OpenApiReader();
            reader.FindDocumentPaths();
            var DocumentFiles = reader.ReadDocumentFiles();

            OpenApiAggregator aggregator = new OpenApiAggregator(DocumentFiles);
            aggregator.AggregateEverything();
            var output = aggregator.MergeDocuments();

            OpenApiWriter writer = new OpenApiWriter();
            //writer.WriteOutputToConsole(output);
            //writer.WriteOutputToFile(output);
            var output_string = writer.SaveOutputAsString(output);
            Console.WriteLine(output_string);
        }
    }
}
