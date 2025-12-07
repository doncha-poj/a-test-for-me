using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Fall2025_Final_Humbuckers.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<FavoriteTrack> FavoriteTracks { get; set; } = new List<FavoriteTrack>();
    }
}
