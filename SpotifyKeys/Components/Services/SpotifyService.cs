using SpotifyAPI.Web;
namespace SpotifyKeys.Components.Services
{
    public class SpotifyService //Define the class that you call when you need these methods
    {
        private readonly ApiVerified _apiVerified; //Creating the ApiVerified object for error checks

        public SpotifyService(ApiVerified apiVerified)
        {
            _apiVerified = apiVerified; //Defining ApiVerified object
        }

        public async Task<string> Track()
        {
            try //Try the operation below if it succeeds, carry on.
            {
                //We create a new object that contains our API access token from the ApiVerified Service
                var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);
                if (spotify == null)
                {
                    //If for some reason getting the access token fails we throw an error
                    throw new Exception("Failed to initialize Spotify client");
                }

                //We use our spotify object to call the Get function of Tracks class from the SpotifyAPI.Web library
                //We give the function a song ID. From this ID it will return the entire song object and save it into "track".
                var track = await spotify.Tracks.Get("1VC3Ut39SEZK48pij3ILBa");
                if (track == null)
                {
                    //If getting the song object fails we throw an error
                    throw new Exception("Track not found");
                }
                //We take the track object which contains all the song information and only return the Name field.
                return track.Name;
            }
            catch (APIException ex) //Catch errors and prepare error messages
            {
                // Handle errors fetching spotify API permissions
                throw new Exception($"Spotify API error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle errors fetching track information
                throw new Exception($"Error fetching track: {ex.Message}", ex);
            }
        }
    }
}