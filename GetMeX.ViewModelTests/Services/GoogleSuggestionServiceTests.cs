using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services.Tests
{
    [TestClass()]
    public class GoogleSuggestionServiceTests
    {
        [TestMethod()]
        public void GetSuggestions_ValidQuery_ReturnsCorrectInfo()
        {
            var service = new GoogleSuggestionService("matsuri", "en");
            var results = service.GetSuggestions().Result;
            Assert.AreEqual(10, results.Count);
            GoogleSuggestion first = results[0];
            GoogleSuggestion second = results[1];
            GoogleSuggestion third = results[2];
            Assert.AreEqual("matsuri", first.ToString());
            Assert.AreEqual("matsuri osaka", second.ToString());
            Assert.AreEqual("matsuri festival", third.ToString());
        }

        [TestMethod()]
        public void GetSuggestions_DifferentLang_ReturnsDifferentResults()
        {
            var serviceEnglish = new GoogleSuggestionService("matsuri", "en");
            var serviceJapanese = new GoogleSuggestionService("matsuri", "jp");
            var resultsEnglish = serviceEnglish.GetSuggestions().Result;
            var resultsJapanese = serviceJapanese.GetSuggestions().Result;
            CollectionAssert.AreNotEqual(resultsEnglish, resultsJapanese);
        }

        [TestMethod()]
        public void GetSuggestions_InvalidQuery_ReturnsEmptyList()
        {
            var service = new GoogleSuggestionService("qqppqqqppp", "");
            var results = service.GetSuggestions().Result;
            Assert.AreEqual(0, results.Count);
        }
    }
}