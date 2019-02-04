using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services.Tests
{
    [TestClass()]
    public class ImageRetrieverServiceTests
    {
        [TestMethod()]
        public void RetrieveImages_KnownSiteDefaultLimit_ReturnsCorrectInfo()
        {
            var testUrl = "https://www.instagram.com/aimyon36/";
            var service = new ImageRetrieverService();
            List<OnlineImageResult> results = service.RetrieveImages(testUrl).Result;
            Assert.AreEqual(2, results.Count);
            OnlineImageResult first = results[0];
            StringAssert.Contains(first.Link, "jpg");
        }

        [TestMethod()]
        public void RetrieveImages_KnownSiteDifferentLimit_ReturnsCorrectInfo()
        {
            var testUrl = "https://www.instagram.com/aimyon36/";
            var service = new ImageRetrieverService(limit: 3);
            List<OnlineImageResult> results = service.RetrieveImages(testUrl).Result;
            Assert.AreEqual(3, results.Count);
            OnlineImageResult third = results[2];
            StringAssert.Contains(third.Link, "jpg");
        }

        [TestMethod()]
        public void RetrieveImages_UnknownSite_ReturnsEmptyList()
        {
            var testUrl = "https://www.google.com";
            var service = new ImageRetrieverService();
            List<OnlineImageResult> results = service.RetrieveImages(testUrl).Result;
            Assert.AreEqual(0, results.Count);
        }
    }
}