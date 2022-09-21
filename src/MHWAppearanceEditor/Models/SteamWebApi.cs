using System;
using System.Collections.Generic;

namespace MHWAppearanceEditor.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class SteamApiResponse<T>
    {
        public T Response { get; set; }
    }

    public class GetPlayerSummariesResponse
    {
        public List<SteamApiPlayer> Players { get; set; }
    }

    public class SteamApiPlayer
    {
        public string SteamId { get; set; }
        public long CommunityVisibilityState { get; set; }
        public long ProfileState { get; set; }
        public string PersonaName { get; set; }
        public long LastLogoff { get; set; }
        public Uri ProfileUrl { get; set; }
        public Uri Avatar { get; set; }
        public Uri AvatarMedium { get; set; }
        public Uri AvatarFull { get; set; }
        public long PersonaState { get; set; }
        public string RealName { get; set; }
        public string PrimaryClanId { get; set; }
        public long TimeCreated { get; set; }
        public long PersonaStateFlags { get; set; }
        public string LocCountryCode { get; set; }
        public string LocStateCode { get; set; }
        public long LocCityId { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
