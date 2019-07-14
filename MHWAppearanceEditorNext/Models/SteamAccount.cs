namespace MHWAppearanceEditorNext.Models
{
    public class SteamAccount
    {
        public long SteamId64 { get; set; }
        public string AccountName { get; set; }
        public string PersonaName { get; set; }
        public bool RememberPassword { get; set; }
        public bool MostRecent { get; set; }
        public long Timestamp { get; set; }

        public long SteamId3
        {
            get
            {
                const long magic = 76561197960265728;
                return SteamId64 - magic;
            }
        }
    }
}
