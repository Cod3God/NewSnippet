using Microsoft.AspNetCore.Mvc;
using MySnippetService.Models;
using MySnippetService.Models;
using MySnippetService.Services;
using System.Collections.Generic;

namespace MySnippetService.Controllers
{
    //Endpoint til at hente uddrag af søgeresultater baseret på søgeord!

    //Controller nåes via api/snippets
        [Route("api/[controller]")]
        [ApiController]
        public class SnippetsController : ControllerBase
        {
            private readonly ISnippetService _snippetService;
        //modtager instans af ISnippetService
            public SnippetsController(ISnippetService snippetService)
            {
                _snippetService = snippetService;
            }

        //Nåes via api/snippets/ Searchterm
            [HttpGet("{searchTerm}")]
            public ActionResult<List<SnippetResult>> GetSnippets(string searchTerm)
            {
                Console.WriteLine($"Received request for snippets with search term: {searchTerm}");
                var snippets = _snippetService.GetSnippets(searchTerm);
                if (snippets == null || snippets.Count == 0)
                {
                    Console.WriteLine("No snippets found.");
                    return NotFound();
                }
                Console.WriteLine($"Returning {snippets.Count} snippets.");
                return Ok(snippets);
            }
        }
    }
