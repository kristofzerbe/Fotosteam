using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Repository;
using Fotosteam.Tests.Service.Fake;

namespace Fotosteam.Tests.Service
{
    /// <summary>
    /// Die Klasse wird verwendet, um die Controller mit einem speziellen Repository für die Tests versehn zu können.
    /// Sie könnte auch zum Testen der Webanwendung verwendet werden
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
            AuthRepository.Dispose();
        }

        private static readonly object Lock = new object();
        private static readonly FakeFotosteamDbContext Context = new FakeFotosteamDbContext();
        private static readonly IDataRepository Repository = new DataRepository(Context);
        private static readonly IAuthRepository AuthRepository = new FakeAuthRepository();



        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <returns>
        /// The retrieved service.
        /// </returns>
        /// <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            lock (Lock)
            {
                if (serviceType == typeof(DataController))
                    return new DataController(Repository, AuthRepository);

                if (serviceType == typeof(AccountController))
                    return new AccountController(Repository, AuthRepository);

                if (serviceType == typeof(CommunicationController))
                {
                    return new CommunicationController(Repository, AuthRepository);
                }
                if (serviceType == typeof(AuthorizeController))
                {
                    return new AuthorizeController(Repository, AuthRepository);
                }
                if (serviceType == typeof(RoKrController))
                {
                    return new RoKrController(Repository, AuthRepository);
                }
                if (serviceType == typeof(SynchController))
                {
                    return new SynchController(Repository);
                }
                return null;
            }
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            //if (serviceType == typeof(DataController))
            //    return new List<object>() { new DataController(new FotosteamRepository(new FakeFotosteamDbContext())) };

            //if (serviceType == typeof(AccountController))
            //    return new List<object>() { new AccountController(new FotosteamRepository(new FakeFotosteamDbContext())) };

            return new List<object>(); ;
        }

        /// <summary>
        /// Starts a resolution scope. 
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}