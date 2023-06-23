using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using SpekTube.Models;
using System.Data;
using System.Data.SqlTypes;


namespace SpekTube.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : Controller {
        private readonly SpekTubeDbContext _dbContext;
        private string connectionString = "Server=.\\SQLExpress;Database=youtubee;Trusted_Connection=True;Encrypt=False;";

        public VideosController(SpekTubeDbContext dbContext)
        {
            _dbContext = dbContext;

        }


        [HttpGet("id")]
        public IActionResult getById(int id)
        {
            var obj = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
            if (obj != null)
                return NotFound();

            return Ok(obj);
        }


        [HttpPost("uploadVideo")]
        public IActionResult uploadVideo(int id)
        {
            var file = Request.Form.Files[0];
            if (file != null && file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string newFileName = GetUniqueFileName(fileExtension);
                string filePath = Path.Combine("C://Users/csp/SpektraGroupProject/YouTube/src/assets/videos", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(stream);

                var obj = new Video
                {
                    UserID_FK = id,
                    Video_File_Name = file.FileName,
                    Video_Url = newFileName

                };
                _dbContext.Videos.Add(obj);
                _dbContext.SaveChanges();

                return Ok(new { obj.Id, newFileName });
            }
            else
            {
                return BadRequest("No file was uploaded.");
            }
        }



        [HttpPost("uploadVideoThumbnail")]
        public IActionResult UploadThumbnail(int id)
        {
            var file = Request.Form.Files[0];
            if (file != null && file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string newFileName = GetUniqueFileName(fileExtension);
                string filePath = Path.Combine("C://Users/csp/SpektraGroupProject/YouTube/src/assets/thumbnails", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(stream);

                var obj = new VideoThumbnail
                {
                    VideoID_FK = id,
                    Thumbnail_File = file.FileName,
                    Thumbnail_Url = newFileName

                };
                _dbContext.VideosThumbnail.Add(obj);
                _dbContext.SaveChanges();

                return Ok(new { newFileName });
            }
            else
            {
                return BadRequest("No file was uploaded.");
            }
        }

        private string GetUniqueFileName(string fileExtension)
        {
            string newFileName = Guid.NewGuid().ToString("N");
            if (newFileName.Length > 40)
                newFileName = newFileName.Substring(0, 40);
            newFileName += fileExtension;
            return newFileName;
        }


        [HttpGet("getVideoDataByAllCategories")]
        public IActionResult GetVideoDataByCategoryAll()
        {
            List<VideoData> videoDataList = new List<VideoData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetVideoDataByCategoryAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VideoData videoData = new VideoData()
                        {
                            VideoID = Convert.ToInt32(reader["videoID_FK"]),
                            VideoTitle = reader["video_title"].ToString(),
                            VideoDescription = reader["video_description"].ToString(),
                            DaysAgoFormatted = reader["days_ago_formatted"].ToString(),
                            UserID = Convert.ToInt32(reader["userID_FK"]),
                            VideoUrl = reader["video_url"].ToString(),
                            ThumbnailUrl = reader["thumbnail_url"].ToString(),
                            ChannelName = reader["channel_name"].ToString(),
                            ChannelLogoUrl = reader["channel_logo_url"].ToString(),
                            Views = Convert.ToInt32(reader["views"])
                        };

                        videoDataList.Add(videoData);
                    }
                }
            }

            // Process the retrieved videoDataList as per your requirements

            return Ok(videoDataList);
        }

        // FormatDaysAgo method
        private string FormatDaysAgo(int daysAgo)
        {
            if (daysAgo == 0)
            {
                return "Today";
            }
            else if (daysAgo == 1)
            {
                return "Yesterday";
            }
            else
            {
                return daysAgo + " days ago";
            }
        }


        [HttpGet("getVideoDataUrl")]
        public IActionResult GetVideoDataByUrl(string videoUrl)
        {
            VideoData2 videoData = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetVideoDataByVideoUrl", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter and set its value
                command.Parameters.Add("@videoUrl", SqlDbType.VarChar).Value = videoUrl;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        videoData = new VideoData2()
                        {
                            VideoID = Convert.ToInt32(reader["videoID_FK"]),
                            VideoTitle = reader["video_title"].ToString(),
                            VideoDescription = reader["video_description"].ToString(),
                            DaysAgo = Convert.ToInt32(reader["days_ago"]),
                            UserID = Convert.ToInt32(reader["userID_FK"]),
                            VideoUrl = reader["video_url"].ToString(),
                            ThumbnailUrl = reader["thumbnail_url"].ToString(),
                            ChannelID = Convert.ToInt32(reader["channelID"]),
                            ChannelName = reader["channel_name"].ToString(),
                            ChannelLogoUrl = reader["channel_logo_url"].ToString(),
                            Views = Convert.ToInt32(reader["views"])


                        };

                        var objj = _dbContext.VideosWatchHistory.FirstOrDefault(v => v.UserID_FK == videoData.UserID && v.VideoID_FK == videoData.VideoID);


                        if (objj == null)
                        {
                            VideoWatchHistory obj2 = new VideoWatchHistory
                            {
                                UserID_FK = videoData.UserID,
                                VideoID_FK = videoData.VideoID
                            };
                            _dbContext.VideosWatchHistory.Add(obj2);
                            _dbContext.SaveChanges();

                        }

                    }




                }
            }
            return Ok(videoData);
        }


        [HttpGet("getSubscribersAndLikes")]
        public IActionResult GetSubscribersAndLikes(int channelID, int videoID)
        {
            int subscriberCount, likeCount, dislikeCount;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("CountSubscribersAndLikes", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add input parameters
                command.Parameters.AddWithValue("@channelID_FK", channelID);
                command.Parameters.AddWithValue("@videoID_FK", videoID);

                // Add output parameters
                command.Parameters.Add("@subscriberCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@likeCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@dislikeCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();

                // Retrieve output parameter values and handle DBNull
                subscriberCount = command.Parameters["@subscriberCount"].Value != DBNull.Value ? Convert.ToInt32(command.Parameters["@subscriberCount"].Value) : 0;
                likeCount = command.Parameters["@likeCount"].Value != DBNull.Value ? Convert.ToInt32(command.Parameters["@likeCount"].Value) : 0;
                dislikeCount = command.Parameters["@dislikeCount"].Value != DBNull.Value ? Convert.ToInt32(command.Parameters["@dislikeCount"].Value) : 0;
            }


            VideoData3 videoData = new VideoData3
            {
                VideoID = videoID,
                SubscriberCount = subscriberCount,
                LikeCount = likeCount,
                DislikeCount = dislikeCount
            };

            return Ok(videoData);
        }

        [HttpGet("checkSubscriberAndIsLikeDislike")]

        public IActionResult check(int channelID, int videID, int userID)
        {
            Boolean isSubscribe;
            int isLikeDislike;
            var obj = _dbContext.Subscribers.FirstOrDefault(s => s.ChannelID_FK == channelID && s.UserID_FK == userID);
            if (obj == null)
                isSubscribe = false;
            else
                isSubscribe = true;

            var obj2 = _dbContext.Video_Like.FirstOrDefault(v => v.VideoID_FK == videID && v.UserID_FK == userID);
            if (obj2 == null)
                isLikeDislike = 0;
            else
                isLikeDislike = obj2.IsLikeDislike;


            var obj3 = new subscribeLikeDislike
            {
                isSubscribed = isSubscribe,
                isLikeDislike = isLikeDislike
            };



            return Ok(obj3);
        }

        [HttpGet("getUserUploadedVideos")]
        public IActionResult GetUploadedVideoDetails(int userID_FK)
        {
            try
            {
                List<UserVideoData> videos = _dbContext.UsersVideoData
                    .FromSqlRaw("EXEC getUploadedVideoDetails @userID_FK", new SqlParameter("userID_FK", userID_FK))
                    .ToList();

                return Ok(videos);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getAllUserUploadedVideos")]
        public IActionResult GetallUploadedVideoDetails()
        {
            try
            {
                List<UserVideoData> videos = _dbContext.UsersVideoData
                    .FromSqlRaw("EXEC getUploadedAllVideoDetails")
                    .ToList();

                return Ok(videos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        [HttpGet("getPlaylistInfo")]
        public IActionResult GetUploadedVideoDetailss(int userID_FK)
        {
            try
            {
                List<PlaylistInfo> playlists = _dbContext.PlaylistInfo
                    .FromSqlRaw("EXEC FetchPlaylistInfo @userID", new SqlParameter("userID", userID_FK))
                    .ToList();

                return Ok(playlists);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
      



    }
    public class VideoFileUploadModel
    {
        public int Id { get; set; }
        public int UserID_FK { get; set; }
        public string? Video_Url { get; set; }
        public string? Video_File_Name { get; set; }
    }
    public class VideoData2
    {
        public int VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public int DaysAgo { get; set; }
        public int UserID { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public string ChannelLogoUrl { get; set; }
        public int Views { get; set; }
    }
    public class VideoData3
    {
        public int VideoID { get; set; }
        public int SubscriberCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }


    public class subscribeLikeDislike {

        public Boolean isSubscribed { get; set; }
        public int isLikeDislike { get; set; }

    }
  

}
