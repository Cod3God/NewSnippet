using MySnippetService.Models;
using System.Collections.Generic;

namespace MySnippetService.Services
{
    public interface ISnippetService
    {
        List<SnippetResult> GetSnippets(string searchTerm);
    }
}
