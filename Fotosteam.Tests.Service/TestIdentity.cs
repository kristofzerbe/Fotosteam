using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Fotosteam.Tests.Service
{
    /// <summary>
    /// Die Identit�t wird f�r die Test gebraucht, um eine zwischen authentifizierten und nicht authentifizierten Benutzern unterscheiden zu k�nnen.
    /// Das funktioniert nur mit direktem Test von den Controllern. F�r ein Test �ber owin muss eine Claims Identity angelegt werden
    /// <code>
    /// <![CDATA[
    /// var claimsIdentity = new ClaimsIdentity(new[] { claim },  "Authenticated" );
    /// ]]>
    /// </code>
    /// </summary>
    [Serializable]
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(string name, bool isAuthenticated) 
        {
            _isAuthenticated = isAuthenticated;
            base.AddClaim(new Claim(ClaimTypes.NameIdentifier, "114664602030178284885", "Value", "google"));
            base.AddClaim(new Claim(ClaimTypes.Email, name, "Value", "google"));
            base.AddClaim(new Claim(ClaimTypes.Name, name, "Value", "google"));
            
        }

        private readonly bool _isAuthenticated;
        public override bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }

    }
}