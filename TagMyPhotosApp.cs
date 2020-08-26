using CLAP;
using CLAP.Validation;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;

namespace TagMyPhotos
{
    public enum OperationModes
    {
        Tag,
        Train
    }

    public enum TagModes
    {
        CosoleOutput,
        RenameFiles
    }

    public class TagMyPhotosApp
    {
        [Verb(IsDefault = true)]
        public static void TagMyPhotos
            (
            [Required]
            [Description("The Subscription Key for a Microsoft Face AI Service.")]
            string subscriptionKey,

            [Required]
            [Description("The endpoint for a Microsoft Face AI Service.")]
            string endPoint,

            [Required]
            [Description("An Id for the group of faces to be identified. Use the same Id during training and tagging mode.")]
            [RegexMatches(@"[0-9a-z_]+")]
            string photosGroupId,

            [Required]
            [Description("Path to the folder containing the sample images of persons to be identified. This folder should contain one subfolder for each person to be identified." 
            + " The subfolders should contain sample images for that person. These images will be used for training the face recognition engine.")]
            string samplePhotosPath,

            [Required]
            [Description("Path to the folder containing the images to be tagged.")]
            string tagPhotosPath, 
            
            [DefaultValue(OperationModes.Tag)]
            [Description("Indicates whether we are training the engine, or using the engine to tag images.")]
            OperationModes operationMode,

            [DefaultValue(TagModes.CosoleOutput)]
            [Description("Indicates what the result of the tagging should be. ConsoleOutput will show the names of the persons found in each photo in the console. RenameFiles will rename the original files by appending the names of the persons found in the photo to the filename.")]
            TagModes tagMode
            )
        {
            var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(subscriptionKey)) { Endpoint = endPoint };

            if (operationMode == OperationModes.Train)
            {
                var groupConfigurationService = new FaceGroupConfigurationService(faceClient);
                groupConfigurationService.ConfigureGroups(samplePhotosPath, photosGroupId).GetAwaiter().GetResult();
            }
            else if (operationMode == OperationModes.Tag)
            {
                var imageTaggingService = TaggingServiceFactory.GetTaggingService(tagMode, faceClient);
                imageTaggingService.TagImages(tagPhotosPath, photosGroupId).GetAwaiter().GetResult();
            }
                
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
