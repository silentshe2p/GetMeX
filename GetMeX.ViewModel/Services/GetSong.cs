using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.ViewModel.Services
{
	public class GetSong
	{
		public string GetSongInfo()
		{
			string result = "";
			using (var client = new HttpClient())
			{
				var responseString = client.GetStringAsync("http://www.google.com");
				result = responseString.Result;
			}
			Console.WriteLine(result);
			return result;
		}

	}
}
