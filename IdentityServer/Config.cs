using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients => new Client[]

            {
                new Client
                { 
                    ClientId ="movieClient",
                    ClientName="Movie API",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // it is used by clients to obtain an access token outside of the context of a user
                    ClientSecrets =
                    { 
                        new Secret("secret".Sha256())  //Client aware of this value
                    },
                    AllowedScopes = {"movieapi.read","movieapi.write" },
                    
                },
                 new Client
                {
                    ClientId ="movieclient2",
                    ClientName="movieApi2",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // it is used by clients to obtain an access token outside of the context of a user
                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())  //Client aware of this value
                    },
                    AllowedScopes = {
                         "movieapi.read",
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile
                     },
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false
                }
            };
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
            {
                new ApiScope("movieapi.read"),
                new ApiScope("movieapi.write"),
            };
        public static IEnumerable<ApiResource> ApiResource => new ApiResource[]  //This is what we try to protect
            {
                new ApiResource("movieapi")
                { 
                    Scopes = new List<string>{ "movieapi.read", "movieapi.write"},
                    ApiSecrets = new List<Secret>{ new Secret ("ScopeSecret") }, //Client aware of this value
                    UserClaims = new List<string>{ "role" }
                }
            };
    }
}
