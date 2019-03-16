using System.Collections.Generic;

namespace MHWAppearanceEditor.Models
{
    public class PalicoAppearanceValues
    {
        public List<KeyValuePair<string, byte>> PatternTypes { get; }
        public List<KeyValuePair<string, byte>> EyeTypes { get; }
        public List<KeyValuePair<string, byte>> EarTypes { get; }
        public List<KeyValuePair<string, byte>> TailTypes { get; }
        public List<KeyValuePair<string, byte>> VoiceTypes { get; }
        public List<KeyValuePair<string, byte>> VoicePitches { get; }

        public PalicoAppearanceValues()
        {
            PatternTypes = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Pattern Type 1", 0),
                new KeyValuePair<string, byte>("Pattern Type 2", 1),
                new KeyValuePair<string, byte>("Pattern Type 3", 2),
                new KeyValuePair<string, byte>("Pattern Type 4", 3),
            };

            EyeTypes = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Eye Type 1", 0),
                new KeyValuePair<string, byte>("Eye Type 2", 1),
                new KeyValuePair<string, byte>("Eye Type 3", 2),
                new KeyValuePair<string, byte>("Eye Type 4", 3),
                new KeyValuePair<string, byte>("Eye Type 5", 4),
                new KeyValuePair<string, byte>("Eye Type 6", 5),
            };

            EarTypes = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Ear Type 1", 0),
                new KeyValuePair<string, byte>("Ear Type 2", 1),
                new KeyValuePair<string, byte>("Ear Type 3", 2),
                new KeyValuePair<string, byte>("Ear Type 4", 3),
                new KeyValuePair<string, byte>("Ear Type 5", 4),
            };

            TailTypes = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Tail Type 1", 0),
                new KeyValuePair<string, byte>("Tail Type 2", 1),
                new KeyValuePair<string, byte>("Tail Type 3", 2),
                new KeyValuePair<string, byte>("Tail Type 4", 3),
            };

            VoiceTypes = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Voice Type 1", 0),
                new KeyValuePair<string, byte>("Voice Type 2", 1),
                new KeyValuePair<string, byte>("Voice Type 3", 2),
            };

            VoicePitches = new List<KeyValuePair<string, byte>>
            {
                new KeyValuePair<string, byte>("Medium Pitch", 0),
                new KeyValuePair<string, byte>("Low Pitch", 1),
                new KeyValuePair<string, byte>("High Pitch", 2),
            };
        }
    }
}
