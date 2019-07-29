namespace Nezuko.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nezuko = new NezukoApp())
            {
                nezuko.Run();
            }
        }
    }
}
