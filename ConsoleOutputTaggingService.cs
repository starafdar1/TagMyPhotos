using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;
using System.Collections.Generic;

namespace TagMyPhotos
{
    public class ConsoleOutputTaggingService : BaseTaggingService
    {
        public ConsoleOutputTaggingService(IFaceClient faceClient) : base(faceClient)
        {
        }

        protected override void ApplyTag(string filePath, List<string> personNames)
        {
            Console.WriteLine($"Image: {filePath} : {string.Join(", ", personNames)}");
        }

    }
}
