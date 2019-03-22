using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GetMeX.ViewModels.Services.Tests
{
	[TestClass()]
	public class GetSongTests
	{
		// Get current title and artist @https://j1fm.com/player/en/onair.php
		// Get current cover and store url @https://j1fm.com/player/en/cover.php

		[TestMethod()]
		public void GetSongInfo_NullStoreUrlCase_ReturnsCorrectInfo()
		{
			GetSongService gs = new GetSongService("Hits");
			var result = gs.GetSongInfo().Result;
			Assert.AreEqual(result.Title, "Invader Invader");
			Assert.AreEqual(result.Artist, "Kyarypamyupamyu");
			Assert.IsNull(result.StoreUrl);
			Assert.AreEqual(result.CoverUrl, "https://j1fm.com/images/no_cd.jpg");
		}

		[TestMethod()]
		public void GetSongInfo_NotNullStoreUrlCase_ReturnsCorrectInfo()
		{
			GetSongService gs = new GetSongService("Hits");
			var result = gs.GetSongInfo().Result;
			Assert.AreEqual(result.Title, "Angel Enjite 20nen");
			Assert.AreEqual(result.Artist, "Up Up Girls (2)");
			Assert.AreEqual(result.StoreUrl, "https://a.j1fm.com/?TPRC-205");
			Assert.AreEqual(result.CoverUrl, "https://j1fm.tokyo/c/T/TPRC-205.jpg");
		}
	}
}