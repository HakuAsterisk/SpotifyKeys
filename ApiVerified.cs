using SpotifyAPI.Web;
using Newtonsoft.Json.Linq;
namespace SpotifyKeys.Components.Services
{
    public class ApiVerified //Define the ^location and name of our ApiVerified service
    {
        public string accessToken; //The token we can use to prove to the API that we are allowed to fetch data
        public DateTime expirationTime; //A timestamp of when an accessToken expires
        public static ApiVerified authenticator; //Make an object to contain the authentication information
        public static SpotifyClient spotify; //Make the final object to contain the accessToken

        public ApiVerified() //No need to define any class definitions
        {
        }

        public static async Task<SpotifyClient> InitializeSpotifyClient(string id, string pass)
        { //We get the API ID and Secret when the method is called, we return the Spotify accessToken information
            authenticator = new ApiVerified(); //New Object using this very class.
            string accessToken = await authenticator.GetAccessToken(id, pass); //Call the GetAccessToken method below within the new object
            spotify = new SpotifyClient(accessToken); //After we get the token we build a new spotify client with all the access information
            return spotify; //And return it
        }

        public async Task<string> GetAccessToken(string id, string pass)
        {
            if (string.IsNullOrEmpty(accessToken) || DateTime.UtcNow >= expirationTime) //Check if current time is older than the expiration
            {
                await RefreshAccessToken(id, pass); //If so, authentication has expired and we have to make a new one by calling the
                //Method below
            }
            return accessToken; //If not then authentication is still good and we can just return current token.
        }

        public async Task RefreshAccessToken(string id, string pass)
        {
            using (var client = new HttpClient()) //We have to create a Json requestbody to request a new Access Token from Spotify
            {
                var requestBody = new FormUrlEncodedContent(new[]
                { //We match the needed information in the fields spotify expects from us and turn it into an object
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", id),
                new KeyValuePair<string, string>("client_secret", pass)
            });

                var response = await client.PostAsync("https://accounts.spotify.com/api/token", requestBody); //We send that object to spotify
                var responseContent = await response.Content.ReadAsStringAsync(); //We save the response we get from them
                var tokenResponse = JObject.Parse(responseContent); //We turn that response into a new object so we can handle it better

                accessToken = tokenResponse["access_token"].Value<string>(); //We grab the accessToken (a randomized string value)
                var expiresIn = tokenResponse["expires_in"].Value<int>(); //We grab the time until accessToken expires
                expirationTime = DateTime.UtcNow.AddSeconds(expiresIn); //We add time until expiration to our current timestamp and use it to
                //compare if our token has expired later
            }
        }
    }
}