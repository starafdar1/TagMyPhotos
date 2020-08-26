using System.Threading.Tasks;

namespace TagMyPhotos
{
    interface ITaggingService
    {
        Task TagImages(string folderPath, string groupId);
    }
}
