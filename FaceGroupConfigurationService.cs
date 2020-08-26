using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TagMyPhotos
{
    public class FaceGroupConfigurationService
    {
        private IFaceClient faceClient;

        public FaceGroupConfigurationService(IFaceClient faceClient)
        {
            this.faceClient = faceClient;
        }

        public async Task ConfigureGroups(string groupPath, string groupId)
        {
            await CreateGroupIfRequired(groupId);
            await AddFacesToGroup(groupPath, groupId);
            await TrainGroups(groupId);
        }

        private async Task CreateGroupIfRequired(string groupId)
        {
            Console.WriteLine("Creating Groups.");
            try
            {
                await faceClient.PersonGroup.GetAsync(groupId);
            }
            catch (APIErrorException apiEx)
            {
                if (apiEx.Body.Error.Code == "PersonGroupNotFound")
                {
                    await faceClient.PersonGroup.CreateAsync(groupId, groupId);
                }
            }
            Console.WriteLine("Done Creating Groups.");
        }

        private async Task AddFacesToGroup(string groupPath, string groupId)
        {
            Console.WriteLine("Adding faces to group.");

            IEnumerable<string> folders = Directory.EnumerateDirectories(groupPath);
            foreach (var folderPath in folders)
            {
                await Task.Delay(250);

                var personName = Path.GetFileName(folderPath).ToLower();
                Person person = await faceClient.PersonGroupPerson.CreateAsync(groupId, personName);

                var filePaths = Directory.GetFiles(folderPath);

                foreach (var filePath in filePaths)
                {
                    using (var imageStream = File.OpenRead(filePath))
                    {
                        await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(groupId, person.PersonId, imageStream);
                    }
                }
            }
            Console.WriteLine("Done adding faces to group.");
        }

        public async Task TrainGroups(string groupId)
        {
            Console.WriteLine("Training Groups.");
            await faceClient.PersonGroup.TrainAsync(groupId);

            while (true)
            {
                await Task.Delay(1000);
                TrainingStatus traiiningStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(groupId);

                if (traiiningStatus.Status == TrainingStatusType.Succeeded)
                    break;
                else if (traiiningStatus.Status == TrainingStatusType.Failed)
                    throw new Exception("Group Training Failed.");

                Console.Write(".");
            }
            Console.WriteLine("Done Training Groups.");
        }
    }
}
