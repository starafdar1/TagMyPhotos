using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagMyPhotos
{
    public class RenameFileTaggingService : BaseTaggingService
    {
        public RenameFileTaggingService(IFaceClient faceClient) : base(faceClient)
        {
        }

        protected override void ApplyTag(string filePath, List<string> personNames)
        {
            var filePrefixToAdd = string.Join("_", personNames);
            var originalFileNameNoExt = Path.GetFileNameWithoutExtension(filePath);
            var newFileName = $"{originalFileNameNoExt}__{filePrefixToAdd}{Path.GetExtension(filePath)}";

            var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
            File.Move(filePath, newFilePath);
            
            Console.WriteLine($"Renamed {filePath} to {newFileName}");
        }
    }
}
