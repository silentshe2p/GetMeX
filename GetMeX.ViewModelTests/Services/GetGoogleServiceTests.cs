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
            GetGoogleService gs = new GetGoogleService("dig");
            var result = gs.GetGoogleSearches().Result;
            var wtf = result[0].Header + result[0].Link + result[0].Description;
            Assert.AreEqual(wtf, "Invader Invader");;
        }
    }
}