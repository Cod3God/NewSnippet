using Microsoft.Data.Sqlite;
using MySnippetService.Models;
using MySnippetService.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MySnippetService.Services
{

    //Forbindelse til DB
        public class SnippetService : ISnippetService
        {
            private readonly string _connectionString;

            public SnippetService(string connectionString)
            {
                _connectionString = connectionString;
            }

        //SQL forspørgsel til at finde ID for søgeterm
            public List<SnippetResult> GetSnippets(string searchTerm)
            {
                var snippets = new List<SnippetResult>();

                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened.");

                    // Step 1: Find wordId for search term
                    var wordIdCommand = connection.CreateCommand();
                    wordIdCommand.CommandText = @"
                SELECT id FROM word WHERE name = $term";
                    wordIdCommand.Parameters.AddWithValue("$term", searchTerm);

                    var wordId = wordIdCommand.ExecuteScalar();
                    Console.WriteLine($"Word ID for '{searchTerm}': {wordId}");

                    if (wordId != null)
                    {
                        // Step 2: Find dokument ID'er der indeholder wordId
                        var docIdCommand = connection.CreateCommand();
                        docIdCommand.CommandText = @"
                    SELECT docId FROM occ WHERE wordId = $wordId";
                        docIdCommand.Parameters.AddWithValue("$wordId", wordId);

                        var documentIds = new List<int>();
                        using (var reader = docIdCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                documentIds.Add(reader.GetInt32(0));
                            }
                        }
                        Console.WriteLine($"Document IDs containing word ID {wordId}: {string.Join(", ", documentIds)}");

                        if (documentIds.Count > 0)
                        {
                            foreach (var docId in documentIds)
                            {
                                // Step 3: Få dokument indhold
                                var docCommand = connection.CreateCommand();
                                docCommand.CommandText = @"
                            SELECT url FROM document WHERE id = $docId";
                                docCommand.Parameters.AddWithValue("$docId", docId);

                                var url = docCommand.ExecuteScalar()?.ToString();
                                Console.WriteLine($"Document ID {docId} URL: {url}");

                                if (!string.IsNullOrEmpty(url))
                                {
                                    var content = System.IO.File.ReadAllText(url); // Læser indhold fra filen
                                    Console.WriteLine($"Content read from {url}. Length: {content.Length}");

                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        // Step 4: Extract snippets
                                        var snippetRegex = new Regex(@".{0,30}" + Regex.Escape(searchTerm) + @".{0,30}", RegexOptions.IgnoreCase);
                                        var matches = snippetRegex.Matches(content);

                                        foreach (Match match in matches)
                                        {
                                            Console.WriteLine($"Snippet found: {match.Value}");
                                            snippets.Add(new SnippetResult { Snippet = match.Value });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Total snippets found: {snippets.Count}");
                return snippets;
            }
        }
    }
