using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.ViewModel.Services.Tests
{
	[TestClass()]
	public class GetSongTests
	{
		[TestMethod()]
		public void GetSongInfoTest()
		{
			GetSong gs = new GetSong();
			string result = gs.GetSongInfo();
			Assert.IsNotNull(result);
		}
	}
}