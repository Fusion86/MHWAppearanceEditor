using System;
using Xenko.Engine;

namespace Nezuko
{
    public class NezukoApp : Game
    {
        public string ExtractedChunksRootDir { get; set; }

        protected override void OnWindowCreated()
        {
            Window.AllowUserResizing = true;
            base.OnWindowCreated();
        }

        protected override void BeginRun()
        {
            base.BeginRun();

            if (ExtractedChunksRootDir == null)
                throw new Exception("ExtractedChunksRootDir has not been set.");
        }
    }
}
