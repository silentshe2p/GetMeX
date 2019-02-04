using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services.Tests
{
    [TestClass()]
    public class GetGoogleServiceTests
    {
        [TestMethod()]
        public void GetGoogleSearches_ValidQuery_ReturnsCorrectInfo()
        {
            var query = "dog";
            var lang = "en";
            var service = new GetGoogleService(query, lang);
            List<SearchResult> results = service.GetGoogleSearches().Result;
            Assert.AreNotEqual(0, results.Count);
            SearchResult first = results[0];
            StringAssert.Contains(first.Header.ToLower(), query);
            StringAssert.Contains(first.Link, query);
            StringAssert.Contains(first.Link, "http");
            StringAssert.Contains(first.Description.ToLower(), query);

            // Test next page results (not empty, different from first page, valid result)
            var nextPageService = new GetGoogleService(query, lang, start: 10);
            List<SearchResult> nextPageResults = nextPageService.GetGoogleSearches().Result;
            Assert.AreNotEqual(0, nextPageResults.Count);
            CollectionAssert.AreNotEqual(results, nextPageResults);
            SearchResult nextFirst = results[0];
            StringAssert.Contains(nextFirst.Header.ToLower(), query);
            StringAssert.Contains(nextFirst.Link, query);
            StringAssert.Contains(nextFirst.Link, "http");
            StringAssert.Contains(nextFirst.Description.ToLower(), query);
        }

        [TestMethod()]
        public void GetGoogleSearches_DifferentLanguage_ReturnsDifferentResults()
        {
            var query = "dog";
            var langEng = "en";
            var serviceEng = new GetGoogleService(query, langEng);
            List<SearchResult> resultsEng = serviceEng.GetGoogleSearches().Result;

            var langJap = "jp";
            var serviceJap = new GetGoogleService(query, langJap);
            List<SearchResult> resultsJap = serviceJap.GetGoogleSearches().Result;

            CollectionAssert.AreNotEqual(resultsEng, resultsJap);
        }

        [TestMethod()]
        public void GetGoogleSearches_VideoQuery_ReturnsCorrectInfo()
        {
            var query = "aimer youtube";
            var service = new GetGoogleService(query, "auto");
            List<SearchResult> results = service.GetGoogleSearches().Result;
            // Test at least one result is a youtube video
            foreach (var res in results)
            {
                if (res.Video != null)
                {
                    StringAssert.Contains(res.Video.Link, "youtube");
                    StringAssert.Contains(res.Video.Link, "embed");
                    return;
                }
            }
            Assert.Fail();
        }
    }
}