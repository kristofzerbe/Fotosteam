using System.Linq;
using System.Web;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Models
{
    public class FotoMetaModel
    {
        public FotoMetaModel(Photo photo)
        {
            if (photo == null)
                return;

            MemberPlainName = photo.Member.PlainName;
            PhotoCaption = (photo.Title.Length != 0) ? photo.Title : photo.Name;
            PhotoUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            
            var link = photo.DirectLinks.FirstOrDefault(x => x.Size == 640);
            if (link != null)
                PhotoUrl640 = link.Url;
        }

        public string MemberPlainName { get; set; }
        public string PhotoCaption { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoUrl640 { get; set; }
    }

    public class StoryMetaModel
    {
        public StoryMetaModel(Story story)
        {
            MemberPlainName = story.Member.PlainName;
            StoryName = story.Name;
            StoryUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (story.HeaderPhoto == null || story.HeaderPhoto.DirectLinks == null) return;

            var link = story.HeaderPhoto.DirectLinks.FirstOrDefault(x => x.Size == 640);
            if (link != null)
                PhotoUrl640 = link.Url;
        }

        public string MemberPlainName { get; set; }
        public string StoryName { get; set; }
        public string StoryUrl { get; set; }
        public string PhotoUrl640 { get; set; }

    }
}