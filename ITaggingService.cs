using System.Threading.Tasks;

namespace TagMyPhotos
{
    public interface ITaggingService
    {
        Task TagImages(string folderPath, string groupId);
    }
}
