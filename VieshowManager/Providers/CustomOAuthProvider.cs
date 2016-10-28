using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using VieshowManager.Services;
using VieshowManager.Models.Mongo;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace VieshowManager.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", context.UserName);
            var userDoc = await MongoService.users.Find(filter).FirstOrDefaultAsync();
            var user = BsonSerializer.Deserialize<User>(userDoc);

            if (!context.Password.Equals(user.password))
            {
                context.Rejected();
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

            context.Validated(identity);
            context.Request.Context.Authentication.SignIn(identity);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
    }
}