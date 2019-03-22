using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GetMeX.ViewModels.Exceptions;

namespace GetMeX.ViewModels.Services.Tests
{
	[TestClass()]
	public class GetWeatherServiceTests
	{
        // Verify test result with https://openweathermap.org

		[TestMethod()]
		public void GetWeatherInfo_WithCityName_ReturnsCorrectInfo()
		{
			GetWeatherService service = new GetWeatherService(null, "santa ana,us");
			var result = service.GetWeatherInfo().Result;
			Assert.AreEqual("Santa Ana (US)", result.Location);
			Assert.AreEqual(11.02f, result.Temp, 0.1);
			Assert.AreEqual(8.7f, result.TempMin, 0.1);
			Assert.AreEqual(13.3f, result.TempMax, 0.1);
			Assert.AreEqual(1.45f, result.Wind, 0.1);
            Assert.AreEqual(96, result.Humidity);
        }

		[TestMethod()]
		public void GetWeatherInfo_WithZipCode_ReturnsCorrectInfo()
		{
			GetWeatherService service = new GetWeatherService(null, "92704,us");
			var result = service.GetWeatherInfo().Result;
            Assert.AreEqual("Santa Ana (US)", result.Location);
            Assert.AreEqual(11.02f, result.Temp, 0.1);
            Assert.AreEqual(8.7f, result.TempMin, 0.1);
            Assert.AreEqual(13.3f, result.TempMax, 0.1);
            Assert.AreEqual(1.45, result.Wind, 0.1);
            Assert.AreEqual(89, result.Humidity);
        }

        [TestMethod()]
        public void GetWeatherInfo_NoLocation_ReturnsCorrectInfo()
        {
            GetWeatherService service = new GetWeatherService(null, null);
            var result = service.GetWeatherInfo().Result;
            Assert.AreEqual("Paularino (US)", result.Location);
            Assert.AreEqual(10.96f, result.Temp, 0.1);
            Assert.AreEqual(8.7f, result.TempMin, 0.1);
            Assert.AreEqual(13.3f, result.TempMax, 0.1);
            Assert.AreEqual(4.07f, result.Wind, 0.1);
            Assert.AreEqual(96, result.Humidity);
        }

        [TestMethod()]
        public void GetWeatherInfo_WrongApiKey_ThrowArgumentException()
        {
            try
            {
                GetWeatherService service = new GetWeatherService("qwerty", "92704, us");
                var _ = service.GetWeatherInfo().Result;
                Assert.Fail();
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(
                    typeof(ArgumentException), e.InnerException.GetType(),
                    "Api Key is incorrect");
            }
        }

        [TestMethod()]
        public void GetWeatherInfo_InvalidLocation_ThrowArgumentException()
        {
            try
            {
                GetWeatherService service = new GetWeatherService(null, "heaven");
                var _ = service.GetWeatherInfo().Result;
                Assert.Fail();
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(
                    typeof(ArgumentException), e.InnerException.GetType(),
                    "Provided location is not found");
            }
        }

        /// No location is provided and not able to figure out location (no permission...) 
        [TestMethod()]
        public void GetWeatherInfo_NoLocation_ThrowArgumentException()
        {
            try
            {
                GetWeatherService service = new GetWeatherService(null, "");
                var _ = service.GetWeatherInfo().Result;
                Assert.Fail();
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(
                    typeof(InsufficientDataException), e.InnerException.GetType(),
                    "Unable to retrieve location information");
            }
        }
    }
}