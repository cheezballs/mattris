using System;

namespace Mattris
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MattrisGame game = new MattrisGame())
            {
                game.Run();
            }
        }
    }
#endif
}

