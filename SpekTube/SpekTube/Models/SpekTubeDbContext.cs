using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace SpekTube.Models
{
    public class SpekTubeDbContext:DbContext
    {
        public SpekTubeDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Channels> Channel { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlayList> Playlists { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoThumbnail> VideosThumbnail { get; set; }
        public DbSet<VideoDescription> VideosDescription { get; set; }

        public DbSet<VideoWatchHistory> VideosWatchHistory { get; set; }
        public DbSet<VideoLike> Video_Like { get; set; }
        public DbSet<VideoReport> VideosReport { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<VideoComment> Video_Comments { get; set; }
        public DbSet<UserVideoData> UsersVideoData { get; set; }


        public DbSet<PlaylistInfo> PlaylistInfo { get; set; }

        public DbSet<VideoCommentModel> VideoCommentModel { get; set; }

        public DbSet<GraphData> GraphData { get; set; }





    }
}
