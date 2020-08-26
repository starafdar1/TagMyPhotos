using CLAP;
using System;

namespace TagMyPhotos
{
    class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser<TagMyPhotosApp>();
            parser.Register.EmptyHelpHandler(help => Console.WriteLine(help));

            var app = new TagMyPhotosApp();
            parser.Run(args, app);
        }
    }
}
