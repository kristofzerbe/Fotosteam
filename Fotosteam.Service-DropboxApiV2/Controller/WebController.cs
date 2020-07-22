using System;
using System.Data.Entity;
using System.Linq;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Der Controller wird verwendet um MVC-Views bei der Generierung auf dem Server mit Daten zu versorgen
    /// </summary>
    public class WebController : ControllerBase
    {
        /// <summary>
        ///     Standardkonstruktor, der ein Datenrepository initalisiert
        /// </summary>
        public WebController()
        {
            DataRepository = new DataRepository();
            AuthenticationRepository = new AuthRepository();
        }

        /// <summary>
        ///     Überladener Konstruktor, der das Einfügen eines Datenrepositories erfordert
        /// </summary>
        /// <param name="repository"></param>
        public WebController(IDataRepository repository)
        {
            DataRepository = repository;
        }

        /// <summary>
        /// Gibt die Daten für ein einzelnes Foto 
        /// </summary>
        /// <param name="alias">Alias des Benutzers zu dem das Foto gehört</param>
        /// <param name="name">Name des Fotos</param>
        /// <returns></returns>
        public Photo GetPhoto(string alias, string name)
        {
            try
            {
                var includePrivate = DoesAliasMatchAuthenticatedUser(alias);
                var member = DataRepository.Queryable<Member>()
                    .FirstOrDefault(x => x.Alias.ToLower().Equals(alias.ToLower()));

                var query =
                    DataRepository.Queryable<Photo>().Include(p => p.DirectLinks).Where(p => p.MemberId == member.Id);

                if (!includePrivate)
                {
                    query = query.Where(p => p.IsPrivate == false);
                }

                var photo = query.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                ClearSensitiveData(member);
                if (photo != null)
                {
                    photo.Member = member;
                    return photo;
                }
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
            }

            return null;
        }

        /// <summary>
        /// Gibt die Daten für eine einzelne Geschichte
        /// </summary>
        /// <param name="alias">Alias des Benutzers zu dem das Foto gehört</param>
        /// <param name="name">Name des Fotos</param>
        /// <returns></returns>
        public Story GetStory(string alias, string name)
        {
            try
            {
                var includePrivate = DoesAliasMatchAuthenticatedUser(alias);

                var member = DataRepository.Queryable<Member>()
                    .FirstOrDefault(x => x.Alias.ToLower().Equals(alias.ToLower()));

                var query =DataRepository.Queryable<Story>().Where(s => s.MemberId == member.Id);

                if (!includePrivate)
                {
                    query = query.Where(p => p.IsPrivate == false);
                }

                var idsAndNames = query.Select(s => new {  s.Id, s.Name }).ToList();

                var currentStory = idsAndNames.FirstOrDefault(s => s.Name.MakeUrlSafe().ToLower().Equals(name.ToLower()));
                if (currentStory != null)
                {
                    var storyId = currentStory.Id;

                    var story =DataRepository.Queryable<Story>().Include(s => s.HeaderPhoto).FirstOrDefault(s => s.Id == storyId);
                    if (story == null)
                        return null;

                    if (story.HeaderPhoto != null && story.IsPrivate && !includePrivate)
                        story.HeaderPhoto = null;

                    ClearSensitiveData(member);
                    story.Member = member;
                    return story;
                }
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
            }

            return null;
        }
    }
}
