using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using VieshowManager.Models.Mongo;
using VieshowManager.Models.UserApi.RequestBody;
using VieshowManager.Models.UserApi.ResponseBody;
using VieshowManager.Services;
using VieshowManager.Services.Helpers;

namespace VieshowManager.Controllers
{
    [RoutePrefix("api/user")]
    public class UserApiController : ApiController
    {
        //POST api/user/register
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<RegisterResBody> register(RegisterReqBody reqBody)
        {
            var registerResBody = new RegisterResBody();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage);

                registerResBody.error = errors.First();
                return registerResBody;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("username", reqBody.username);
            var existUserDoc = await MongoService.users.Find(filter).FirstOrDefaultAsync();
            if (existUserDoc != null)
            {
                registerResBody.error = "使用者名稱重複。";
                return registerResBody;
            }

            var user = new User()
            {
                username = reqBody.username,
                password = reqBody.password
            };

            await MongoService.users.InsertOneAsync(user.toDoc());

            var authToken = await getAuthToken(reqBody.username, reqBody.password);

            if (authToken == null)
            {
                registerResBody.error = "註冊成功，請重新登入。";
                return registerResBody;
            }

            registerResBody.token = authToken;

            return registerResBody;
        }

        //POST api/user/login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<LoginResBody> login(LoginReqBody reqBody)
        {
            var loginResBody = new LoginResBody();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage);

                loginResBody.error = errors.First();
                return loginResBody;
            }

            var authToken = await getAuthToken(reqBody.username, reqBody.password);

            if (authToken == null)
            {
                loginResBody.error = "認證錯誤。";
                return loginResBody;
            }

            loginResBody.token = authToken;

            return loginResBody;
        }

        private async Task<string> getAuthToken(string username, string password)
        {
            var client = new HttpClient();

            var requestParams = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await client.PostAsync(ApplicationHelper.getAppBaseUrl() + "/token", requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();

            var responseData = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(responseString);

            return responseData["access_token"];
        }
    }
}
