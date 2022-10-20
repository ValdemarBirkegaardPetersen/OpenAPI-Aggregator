using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class OpenApiAggregator
    {
        List<OpenApiDocument> DocumentList;
        Dictionary<string, int> DuplicateLookUpTable = new Dictionary<string, int>();

        Dictionary<string, OpenApiPathItem> OpenApiPaths = new Dictionary<string, OpenApiPathItem>();
        IList<OpenApiTag> OpenApiTags = new List<OpenApiTag>();
        IDictionary<string, OpenApiCallback> OpenApiCallbacks = new Dictionary<string, OpenApiCallback>();
        IDictionary<string, OpenApiExample> OpenApiExamples = new Dictionary<string, OpenApiExample>();
        IDictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension> OpenApiExtensions = new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
        IDictionary<string, OpenApiHeader> OpenApiHeaders = new Dictionary<string, OpenApiHeader>();
        IDictionary<string, OpenApiLink> OpenApiLinks = new Dictionary<string, OpenApiLink>();
        IDictionary<string, OpenApiParameter> OpenApiParameters = new Dictionary<string, OpenApiParameter>();
        IDictionary<string, OpenApiResponse> OpenApiResponses = new Dictionary<string, OpenApiResponse>();
        IDictionary<string, OpenApiSchema> OpenApiSchemas = new Dictionary<string, OpenApiSchema>();
        IDictionary<string, OpenApiRequestBody> OpenApiRequestBody = new Dictionary<string, OpenApiRequestBody>();
        IDictionary<string, OpenApiSecurityScheme> OpenApiSecruitySchemes = new Dictionary<string, OpenApiSecurityScheme>();




        public OpenApiAggregator (List<OpenApiDocument> InputList)
        {
            DocumentList = InputList;

        }

        public void GetOpenApiPaths()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                var CurrentDocKey = DocumentList[i].Paths.Keys.ToArray();
                var CurrentDocPath = DocumentList[i].Paths.Values.ToArray();

                for (int y = 0; y < CurrentDocKey.Count(); y++)
                {
                    OpenApiPaths.Add(CurrentDocKey[y], CurrentDocPath[y]);
                }
            }
        }

        public void GetOpenApiTags()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var tag in DocumentList[i].Tags)
                {
                    OpenApiTags.Add(tag);
                }
            }
        }

        // Components --> Callbacks
        public void GetOpenApiCallbacks ()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var callback_iter in DocumentList[i].Components.Callbacks)
                {
                    CheckForDuplicate<OpenApiCallback>(callback_iter, OpenApiCallbacks, callback_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Examples
        public void GetOpenApiExamples()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var example_iter in DocumentList[i].Components.Examples)
                {
                    CheckForDuplicate<OpenApiExample>(example_iter, OpenApiExamples, example_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Extensions
        public void GetOpenApiExtensions()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var extensions_iter in DocumentList[i].Components.Extensions)
                {
                    if (OpenApiExtensions.ContainsKey(extensions_iter.Key))
                    {
                        continue;
                    }
                    else
                    {
                        OpenApiExtensions.Add(extensions_iter.Key, extensions_iter.Value);
                    }
                }
            }
        }

        // Components --> Headers
        public void GetOpenApiHeaders()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var header_iter in DocumentList[i].Components.Headers)
                {
                    CheckForDuplicate<OpenApiHeader>(header_iter, OpenApiHeaders, header_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Links
        public void GetOpenApiLinks()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var link_iter in DocumentList[i].Components.Links)
                {
                    CheckForDuplicate<OpenApiLink>(link_iter, OpenApiLinks, link_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Parameters
        public void GetOpenApiParameters()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var parameter_iter in DocumentList[i].Components.Parameters)
                {
                    CheckForDuplicate<OpenApiParameter>(parameter_iter, OpenApiParameters, parameter_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Responses
        public void GetOpenApiResponses()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var response_iter in DocumentList[i].Components.Responses)
                {
                    CheckForDuplicate<OpenApiResponse>(response_iter, OpenApiResponses, response_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Schemas
        public void GetOpenApiSchemas()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var schema_iter in DocumentList[i].Components.Schemas)
                {
                    CheckForDuplicate<OpenApiSchema>(schema_iter, OpenApiSchemas, schema_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Request Bodies
        public void GetOpenApiRequestBodies()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var requestbody_iter in DocumentList[i].Components.RequestBodies)
                {
                    CheckForDuplicate<OpenApiRequestBody>(requestbody_iter, OpenApiRequestBody, requestbody_iter.Value.Reference.Id);
                }
            }
        }

        // Components --> Secruity Schemes
        public void GetOpenApiSecruitySchemes()
        {
            for (int i = 0; i < DocumentList.Count; i++)
            {
                foreach (var secruityscheme_iter in DocumentList[i].Components.SecuritySchemes)
                {
                    CheckForDuplicate<OpenApiSecurityScheme>(secruityscheme_iter, OpenApiSecruitySchemes, secruityscheme_iter.Value.Reference.Id);
                }
            }
        }

        bool CheckForDuplicate<T>(KeyValuePair<string, T> input, IDictionary<string, T> output, string ReferenceID)
        {
            bool DuplicateExist = false;

            if (output.ContainsKey(input.Key)) {
                DuplicateExist = true;

                if(DuplicateLookUpTable.ContainsKey(input.Key))
                {
                    DuplicateLookUpTable[input.Key] += 1;
                } 
                else
                {
                    DuplicateLookUpTable.Add(input.Key, 2);
                }

                ReferenceID = input.Key + DuplicateLookUpTable[input.Key];
                output.Add(input.Key + DuplicateLookUpTable[input.Key], input.Value);
            }
            else
            {
                DuplicateExist = false;
                output.Add(input.Key, input.Value);
            }

            return DuplicateExist;
        }

        // This function allows the user to call a single command instead of having to use a function for each component type
        public void AggregateEverything()
        {
            GetOpenApiPaths();
            GetOpenApiTags();
            GetOpenApiCallbacks();
            GetOpenApiExamples();
            GetOpenApiExtensions();
            GetOpenApiHeaders();
            GetOpenApiLinks();
            GetOpenApiParameters();
            GetOpenApiResponses();
            GetOpenApiSchemas();
            GetOpenApiRequestBodies();
            GetOpenApiRequestBodies();
        }

        
        public OpenApiDocument MergeDocuments()
        {
            var OutputDocument = CreateNewDocument();

            foreach(var paths in OpenApiPaths)
            {
                OutputDocument.Paths.TryAdd(paths.Key, paths.Value);
            }

            OutputDocument.Tags = OpenApiTags;
            OutputDocument.Components.Callbacks = OpenApiCallbacks;
            OutputDocument.Components.Examples = OpenApiExamples;
            OutputDocument.Components.Extensions = OpenApiExtensions;
            OutputDocument.Components.Headers = OpenApiHeaders;
            OutputDocument.Components.Links = OpenApiLinks;
            OutputDocument.Components.Parameters = OpenApiParameters;
            OutputDocument.Components.Responses = OpenApiResponses;
            OutputDocument.Components.Schemas = OpenApiSchemas;
            OutputDocument.Components.RequestBodies = OpenApiRequestBody;
            OutputDocument.Components.SecuritySchemes = OpenApiSecruitySchemes;

            return OutputDocument;
        }

        public OpenApiDocument CreateNewDocument()
        {
            var OutputDocument = new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Version = "1.0.0",
                    Title = "e-Boks OpenAPI Aggregator",
                },
                Servers = new List<OpenApiServer>
            {
                new OpenApiServer {Url = "http://fictional.eboks.dk/api"}
            },
                Paths = new OpenApiPaths
                {
                },
                Components = new OpenApiComponents()
                {
                    Responses = new Dictionary<string, OpenApiResponse>(),
                    Schemas = new Dictionary<string, OpenApiSchema>()
                },
            };

            return OutputDocument;
        }


    }
}
