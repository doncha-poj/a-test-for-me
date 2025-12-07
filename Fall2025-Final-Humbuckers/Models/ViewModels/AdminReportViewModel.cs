namespace Fall2025_Final_Humbuckers.Models.ViewModels
{
    public class AdminReportViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalFavoriteTracks { get; set; }
        public string TopArtist { get; set; }
        public int AverageTracksPerUser { get; set; }
        
        public List<GlobalTrack> TopRankedTracks { get; set; }
    }
}