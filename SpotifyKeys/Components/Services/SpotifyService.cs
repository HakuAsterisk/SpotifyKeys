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

            // Filter playlists for valid entries
            if (searchResult.Playlists?.Items != null)
            {
                playlists = searchResult.Playlists.Items
                    .Where(p => p != null // Ensure the playlist object is not null
                                && !string.IsNullOrWhiteSpace(p.Name) // Playlist must have a name
                                && p.Images != null && p.Images.Any() // Playlist must have at least one image
                                && !string.IsNullOrWhiteSpace(p.Owner?.DisplayName)) // Playlist must have an owner with a display name
                    .Take(10)
                    .ToList();
            }

            Console.WriteLine(playlists.Count);

            return playlists;
        }
    }
}
