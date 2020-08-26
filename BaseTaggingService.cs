using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TagMyPhotos
{
    public abstract class BaseTaggingService : ITaggingService
    {
        private IFaceClient faceClient;
        public BaseTaggingService(IFaceClient faceClient)
        {
            this.faceClient = faceClient;
        }

        protected abstract void ApplyTag(string filePath, List<string> personNames);

        public async Task TagImages(string imageFolderPath, string groupId)
        {
            var filePaths = Directory.GetFiles(imageFolderPath);

            foreach (var filePath in filePaths)
            {
                await TagImage(groupId, filePath);
            }
        }

        private async Task TagImage(string groupId, string filePath)
        {
            using (var imageStream = File.OpenRead(filePath))
            {
                IList<DetectedFace> faces = await faceClient.Face.DetectWithStreamAsync(imageStream);
                IEnumerable<Guid?> faceIds = faces.Select(f => f.FaceId);

                IList<IdentifyResult> results = await faceClient.Face.IdentifyAsync(faceIds.ToList(), groupId);

                List<string> personNames = new List<string>();

                foreach (IdentifyResult result in results)
                {
                    if (result.Candidates.Any())
                    {
                        IdentifyCandidate topCandidate = result.Candidates.First();
                        Person person = await faceClient.PersonGroupPerson.GetAsync(groupId, topCandidate.PersonId);
                        personNames.Add(person.Name);
                    }
                }
                ApplyTag(filePath, personNames);
            }
        }
    }
}
