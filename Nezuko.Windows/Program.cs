namespace Nezuko.Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nezuko = new NezukoApp())
            {
                nezuko.ExtractedChunksRootDir = @"L:\Sync\MHW Mods\chunks_v0\chunk";
                nezuko.Run();
            }
        }
    }
}
