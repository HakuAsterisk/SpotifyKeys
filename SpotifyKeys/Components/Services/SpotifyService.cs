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

        public async Task<(string, string, string)> GetTrack(string inputSong)
        {
            var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);

            if (spotify == null)
            {
                throw new Exception("Failed to initialize Spotify client");
            }

            // Search for the track using the input song name
            var requestTrack = new SearchRequest(SearchRequest.Types.Track, inputSong);
            var searchResult = await spotify.Search.Item(requestTrack);

            if (searchResult.Tracks.Items.Count > 0)
            {
                // Get the first track from the search results
                var firstTrack = searchResult.Tracks.Items[0];
                var albumImageUrl = firstTrack.Album.Images.Count > 0 ? firstTrack.Album.Images[0].Url : "No image available";
                var songUrl = firstTrack.ExternalUrls["spotify"];

                return ($"Track: {firstTrack.Name}, Artist: {firstTrack.Artists[0].Name}", albumImageUrl, songUrl);
            }
            else
            {
                return ("No tracks found.", null, null);
            }
        }


        public async Task<List<string>> SearchTrack(string input)
        {
            var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);
            
            if (spotify == null)
            {
            throw new Exception("Failed to initialize Spotify client");
            }

            // Search for tracks that match the input
            var requestTrack = new SearchRequest(SearchRequest.Types.Track, input);
            var searchResult = await spotify.Search.Item(requestTrack);

            // Collect the track names in a list, limiting to 10 items
            var trackNames = searchResult.Tracks.Items.Take(10).Select(track => track.Name).ToList();

            return trackNames;
        }

        public async Task<SearchResponse> Search(string query)
        {
            try
            {
                var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);
                if (spotify == null)
                {
                    throw new Exception("Failed to initialize Spotify client");
                }

                //We use our spotify object to call the Search function of SearchItems class from the SpotifyAPI.Web library
                //We give the function a search query. From this query it will return a list of song objects and save it into "search".
                var search = await spotify.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
                if (search == null)
                {
                    throw new Exception("Search not found");
                }
                //We take the first song object from the search list and only return the Name field.
                return search;
            }
            catch (APIException ex)
            {
                throw new Exception($"Spotify API error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching search: {ex.Message}", ex);
            }
        }
    }
}
