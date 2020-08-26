using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace TagMyPhotos
{
    public static class TaggingServiceFactory
    {
        public static ITaggingService GetTaggingService(TagModes tagMode, IFaceClient faceClient)
        {
            switch (tagMode)
            {
                case TagModes.CosoleOutput:
                    return new ConsoleOutputTaggingService(faceClient);
                    
                case TagModes.RenameFiles:
                    return new RenameFileTaggingService(faceClient);

                default:
                    return new ConsoleOutputTaggingService(faceClient);
            }
        }
    }
}
