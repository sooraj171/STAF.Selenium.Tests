using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;


namespace STAFTests.Tests
{
    public static class AiElementGet
    {
        public static string FindElementLocatorUsingOllama(string html, string searchText)
        {
            string ollamaResponse = CallOllama(html, searchText);
            ollamaResponse = ExtractXPath(ollamaResponse);
            return ollamaResponse; // Assume Ollama returns an XPath or CSS selector
        }

        public static string CallOllama(string extractedHtml, string searchText)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11434"); // Ollama's local API

                var requestBody = new
                {
                    model = "mistral", // or any model you are using
                    //prompt = $"Find the XPath for the input element next to '{searchText}' in this HTML:\n{extractedHtml}",
                    prompt = $"Extract the XPath of the first actionable element (input, select, or textarea) next to it or under the text '{searchText}' in this HTML:{extractedHtml}. Do not include any explanations, formatting, or additional text—only return the XPath.",
                    stream = false
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("/api/generate", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static string ExtractRelevantHtml(string html, string searchText, string elementType = "*")
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Find the node containing the display text
            var textNode = doc.DocumentNode.SelectSingleNode($"//p[b[text()='{searchText}']]");

            if (textNode != null)
            {
                // If elementType is provided, use it to filter the actionable element
                string elementFilter = (elementType == "*" || string.IsNullOrEmpty(elementType))
                    ? "input|select|textarea"
                    : elementType;

                // Find the first actionable element inside the next container (div/span/etc.)
                var nextElement = textNode.SelectSingleNode($"following-sibling::*[1]//*[self::{elementFilter}]") ??
                                  textNode.SelectSingleNode($"following-sibling::*[1]");

                if (nextElement != null)
                {
                    return nextElement.OuterHtml; // Extract only this portion of HTML
                }
            }

            return "";
        }

        public static string ExtractXPath(string jsonResponse)
        {
            var json = JObject.Parse(jsonResponse);
            return json["response"]?.ToString().Trim();
        }

    }
}
