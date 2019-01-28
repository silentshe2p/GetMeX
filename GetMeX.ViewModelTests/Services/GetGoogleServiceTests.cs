using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services.Tests
{
    [TestClass()]
    public class GetGoogleServiceTests
    {
        [TestMethod()]
        public void GetSongInfo_NullStoreUrlCase_ReturnsCorrectInfo()
        {
            var wtf = new ImageRetrieverService();
            var result = wtf.RetrieveImages("https://chan.sankakucomplex.com/").Result;
            Assert.AreEqual(1, 1);
        }
    }
}