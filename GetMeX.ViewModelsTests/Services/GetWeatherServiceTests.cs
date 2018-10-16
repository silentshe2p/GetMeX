using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.ViewModels.Services.Tests
{
	[TestClass()]
	public class GetWeatherServiceTests
	{
		[TestMethod()]
		public void GetWeatherInfo_WithLocationProvide_ReturnsCorrectInfo()
		{
			GetWeatherService service = new GetWeatherService(null, "los angeles");

			Assert.Fail();
			
		}
	}
}