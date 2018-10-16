using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.ViewModel.Services.Tests
{
	[TestClass()]
	public class GetWeatherServiceTests
	{
		[TestMethod()]
		public void GetWeatherInfo_WithLocationProvide_ReturnsCorrectInfo()
		{
			GetWeatherService service = new GetWeatherService(null, "los angeles");
			var result = service.GetWeatherInfo().Result;
			Assert.AreEqual("Los Angeles (US)", result.Location);
			Assert.AreEqual(299.52f, result.Temp, 0.1);
			Assert.AreEqual(296.15f, result.TempMin, 0.1);
			Assert.AreEqual(301.15f, result.TempMax, 0.1);
			Assert.AreEqual(3.6f, result.Wind, 0.1);
		}

		[TestMethod()]
		public void GetWeatherInfo_WithNoLocationProvide_ReturnsCorrectInfo()
		{
			GetWeatherService service = new GetWeatherService(null, null);
			var result = service.GetWeatherInfo().Result;
			Assert.AreEqual("Los Angeles (US)", result.Location);
			Assert.AreEqual(299.52f, result.Temp, 0.1);
			Assert.AreEqual(296.15f, result.TempMin, 0.1);
			Assert.AreEqual(301.15f, result.TempMax, 0.1);
			Assert.AreEqual(3.6f, result.Wind, 0.1);
		}
	}
}