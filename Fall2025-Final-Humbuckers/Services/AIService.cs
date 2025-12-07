using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc.Abstractions;
using OpenAI.Chat;
using System.ClientModel;
using Fall2025_Final_Humbuckers.Models;


namespace Fall2025_Final_Humbuckers.Services
{
    public class AIService : IAIService
    {
        private readonly Uri _endpoint;
        private readonly ApiKeyCredential _apiCredential;
        private readonly string _deploymentName;

        public AIService(IConfiguration configuration)
        {
            _endpoint = new Uri(configuration["AzureOpenAI:Endpoint"]);
            _apiCredential = new ApiKeyCredential(configuration["AzureOpenAI:ApiKey"]);
            _deploymentName = configuration["AzureOpenAI:DeploymentName"];
        }

        public async Task<List<string>> GenerateMusicRecommendations(List<string> favoriteTracks, int count = 5)
        {
            ChatClient client = new AzureOpenAIClient(_endpoint, _apiCredential).GetChatClient(_deploymentName);

            // Get favorite tracks as a single string
            string tracksString = string.Join(", ", favoriteTracks);

            var messages = new ChatMessage[]
            {
                new SystemChatMessage($"You are a music recommendation expert. Based on the user's favorite tracks, recommend {count} similar songs they would enjoy. For each recommendation, provide the song title and artist in the format: 'Song Title by Artist Name'. Separate each recommendation with a '|' character. Do not number them or add any other text."),
                new UserChatMessage($"Based on these favorite tracks: {tracksString}, recommend {count} songs I would like.")
            };

            ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages);
            string response = result.Value.Content[0].Text;

            // Split by | and clean up
            var recommendations = response
                .Split('|')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Take(count)
                .ToList();

            return recommendations;
        }

        // Returns recs in RecTrack model form
        public async Task<List<RecTrack>> GenerateRecTracks(List<string> favoriteTracks, int count = 5)
        {
            var client = new AzureOpenAIClient(_endpoint, _apiCredential).GetChatClient(_deploymentName);

            string tracksString = string.Join(", ", favoriteTracks);

            var messages = new ChatMessage[]
            {
                new SystemChatMessage(
                    $"You are a music recommendation expert. Based on the user's favorite tracks, recommend {count} similar songs. " +
                    "Return each recommendation in the format 'Song Title by Artist Name', separated by |. Do not number or add extra text."
                ),
                new UserChatMessage($"User's favorite tracks: {tracksString}")
            };

            var result = await client.CompleteChatAsync(messages);
            string response = result.Value.Content[0].Text;

            // Split AI output
            var recommendations = response
                .Split('|')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Take(count)
                .ToList();

            // Map each recommendation to a RecTrack
            var structured = recommendations.Select(r =>
            {
                var parts = r.Split(" by ", StringSplitOptions.RemoveEmptyEntries);
                string title = parts.Length > 0 ? parts[0].Trim() : r;
                string artist = parts.Length > 1 ? parts[1].Trim() : "";

                return new RecTrack
                {
                    Title = title,
                    Artist = artist,
                    Album = "",
                    SpotifyUrl = "",
                    AlbumArtUrl = ""
                };
            }).ToList();

            return structured;
        }
    }
}
