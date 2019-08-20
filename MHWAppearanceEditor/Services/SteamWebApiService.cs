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

        private int ErrorCount = 0;
        private readonly string ApiKey;
        private readonly HttpClient Client;

        public SteamWebApiService(string apiKey)
        {
            ApiKey = apiKey;
            Client = new HttpClient();
        }

        public async Task<string> GetPersonaName(string steamId)
        {
            if (ErrorCount < 3)
            {
                // Yeah it sucks but who cares
                try
                {
                    var res = await Client.GetAsync("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + ApiKey + "&steamids=" + steamId);
                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<SteamApiResponse<GetPlayerSummariesResponse>>(json);
                        return obj.Response.Players.FirstOrDefault()?.PersonaName ?? "(no account found)";
                    }
                    else
                    {
                        ErrorCount++;
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    CtxLog.Error(ex, $"{ex.Message}. ErrorCount: {ErrorCount}/3");
                }
            }

            return "(steam api error)";
        }
    }
}
