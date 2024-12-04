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

        public async Task<List<FullTrack>> SearchTrack(string input)
        {
            var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);
            
            if (spotify == null)
            {
                throw new Exception("Failed to initialize Spotify client");
            }

            // Search for tracks that match the input
            var requestTrack = new SearchRequest(SearchRequest.Types.Track, input);
            var searchResult = await spotify.Search.Item(requestTrack);

            // Collect up to 10 track objects in a list
            var tracks = new List<FullTrack>();

            // Ensure searchResult has tracks and limit to 10 items
            if (searchResult.Tracks?.Items != null)
            {
                tracks = searchResult.Tracks.Items.Take(10).ToList();
            }

            return tracks;
        }
        public async Task<List<FullPlaylist>> SearchPlaylist(string input)
        {
            var spotify = await ApiVerified.InitializeSpotifyClient(Globals.jetroId, Globals.jetroPass);
            
            if (spotify == null)
            {
                throw new Exception("Failed to initialize Spotify client");
            }

            // Search for tracks that match the input
            var requestPlaylist = new SearchRequest(SearchRequest.Types.Playlist, input);
            var searchResult = await spotify.Search.Item(requestPlaylist);

            // Collect up to 10 track objects in a list
            var playlists = new List<FullPlaylist>();

            // Ensure searchResult has tracks and limit to 10 items
            if (searchResult.Playlists?.Items != null)
            {
                playlists = searchResult.Playlists.Items.Take(10).ToList();
            }

            return playlists;
        }
    }
}
