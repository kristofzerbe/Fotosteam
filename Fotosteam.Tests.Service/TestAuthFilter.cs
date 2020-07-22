using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Fotosteam.Tests.Service
{
    /// <summary>
    ///     Der Filter wird benötigt, damit der Claim für den Benutzer an den Controller übergeben werden kann
    /// </summary>
    public class TestAuthFilter : IAuthenticationFilter
    {
        static TestAuthFilter()
        {
            TestUserId = "TestDomain\\TestUser";
        }

        public static bool UseAuthenticatedUser { get; set; }
        private static ClaimsIdentity _currentIdentity;
        private static object _lock = new object();
        internal  static IIdentity Identity
        {
            get
            {
                lock (_lock)
                {
                    if (_currentIdentity == null)                    
                    {
                        var currentClaim = new Claim(ClaimTypes.NameIdentifier, UseAuthenticatedUser ? "114664602030178284885" : "-114664602030178284885", "Value", "Google");                        
                        _currentIdentity = new ClaimsIdentity( new[] {currentClaim},
                            UseAuthenticatedUser ? "Authenticated" : null);
                        _currentIdentity.AddClaim(new Claim(ClaimTypes.Email, "bobbakos@gmail.com"));
                        _currentIdentity.AddClaim(new Claim(ClaimTypes.Name, "bobbakos@gmail.com"));                 
                    }
                    return _currentIdentity;
                }
            }
        }

        public static string TestUserId { get; set; }
        public bool AllowMultiple { get; private set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            lock (_lock )
            {
                if (UseAuthenticatedUser !=Identity.IsAuthenticated)
                    _currentIdentity =null;

                context.Principal = new ClaimsPrincipal(Identity);
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            
        }
    }
}