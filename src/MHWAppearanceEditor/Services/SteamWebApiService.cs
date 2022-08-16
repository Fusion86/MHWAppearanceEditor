using MHWAppearanceEditor.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.Services
{
    // HACK:
    // I tried using the babelshift/SteamWebAPI2 library but I didn't understand anything
    // so we'll just do our own stuff
    public class SteamWebApiService
    {
        private static readonly ILogger CtxLog = Log.ForContext<SteamWebApiService>();

        private int errorCount = 0;
        private readonly string apiKey;
        private readonly HttpClient client;

        public SteamWebApiService(string apiKey)
        {
            this.apiKey = apiKey;
            client = new HttpClient();
        }

        public async Task<string> GetPersonaName(string steamId)
        {
            if (apiKey.Length == 0)
            {
                CtxLog.Warning("Steam API key is not set. Can't get persona name.");
                return "(steam api key not set)";
            }

            if (errorCount < 3)
            {
                // Yeah it sucks but who cares
                try
                {
                    var res = await client.GetAsync("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + apiKey + "&steamids=" + steamId);
                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<SteamApiResponse<GetPlayerSummariesResponse>>(json)!;
                        return obj.Response.Players.FirstOrDefault()?.PersonaName ?? "(no account found)";
                    }
                    else
                    {
                        errorCount++;
                        throw new SteamWebApiException(res.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    CtxLog.Error(ex, $"{ex.Message}. ErrorCount: {errorCount}/3");
                }
            }

            return "(steam api error)";
        }
    }

    internal class SteamWebApiException : Exception
    {
        public SteamWebApiException()
        {
        }

        public SteamWebApiException(string message) : base(message)
        {
        }

        public SteamWebApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
