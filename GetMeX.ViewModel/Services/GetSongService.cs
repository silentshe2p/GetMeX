using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services
{
	public class GetSongService
	{
		private string[] j1Param = { "player", "player_j1x", "player_j1a", "player_j1g" };
		private static string J1OnAirSong = "https://j1fm.com/{0}/en/onair.php";
		private static string J1OnAirCover = "https://j1fm.com/{0}/en/cover.php";
		private static string cdjapanUri = "http://www.cdjapan.co.jp/product/";

		private J1Channels Channel { get; set; }

		public GetSongService(string channel)
		{
			switch(channel)
			{
				case "Xtra":
					Channel = J1Channels.Xtra;
					break;
				case "AChan":
					Channel = J1Channels.AChan;
					break;
				case "Gold":
					Channel = J1Channels.Gold;
					break;
				case "Hits":
				default:
					Channel = J1Channels.Hits;
					break;
			}
		}

		public async Task<SongInfo> GetSongInfo()
		{
			SongInfo result = new SongInfo();
			using (var client = new HttpClient())
			{
				var songUri = string.Format(J1OnAirSong, j1Param[(int)Channel]);
				var coverUri = string.Format(J1OnAirCover, j1Param[(int)Channel]);
				var songResponse = await client.GetStringAsync(songUri);
				var coverResponse = await client.GetStringAsync(coverUri);


				// HTML structure of https://j1fm.com/{0}/en/onair.php
				/*...<table...>
					<tr>
						<td...>Song name (in ENG)</td> -> first match
					</tr>
					<tr>
						<td...>Artist (in ENG)</td> -> second match
					</tr>
					<tr>
						<td...>Song name (in JAP)</td>
					</tr>
					<tr>
						<td...>Artist (in JAP)</td>
					</tr>
				</table>...*/

				var songPattern = @"<td[\s\S]*?/td>";
				var bracketPattern = @"<[\s\S]*?>";
				Match first = Regex.Match(songResponse, songPattern);
				result.Title = Regex.Replace(first.Value, bracketPattern, string.Empty);

				Match second = first.NextMatch();
				result.Artist = Regex.Replace(second.Value, bracketPattern, string.Empty);


				// HTML structure of https://j1fm.com/{0}/en/cover.php
				/* ...<a ... href=storeUrl> - nullable attribute
					<img ... src=coverUrl>
				</a>...*/

				var storeUrlPattern = @"href[\s\S]*?\>";
				var coverPattern = @"src[\s\S]*?\s+";
				Match sUrl = Regex.Match(coverResponse, storeUrlPattern); // href={storeUrl}>
				if (!sUrl.Success)
				{
					result.StoreUrl = null;
				}
				else
				{
					var storeUrl = sUrl.Value.Split('=')[1];
					storeUrl = storeUrl.Remove(storeUrl.Length - 1);
					var splited = storeUrl.Split('?');
					if (splited.Length < 2) // No "?" before the product parameter 
					{
						splited = splited[0].Split('/');
					}
					storeUrl = cdjapanUri + splited[splited.Length - 1];
					result.StoreUrl = storeUrl;
				}

				Match cUrl = Regex.Match(coverResponse, coverPattern); // src={cover}
				result.CoverUrl = cUrl.Value.Trim().Split('=')?[1];
			}
			return result;
		}
	}
}
