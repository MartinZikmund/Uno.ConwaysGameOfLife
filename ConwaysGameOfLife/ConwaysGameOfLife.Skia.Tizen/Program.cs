﻿using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace ConwaysGameOfLife.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new ConwaysGameOfLife.App(), args);
            host.Run();
        }
    }
}
