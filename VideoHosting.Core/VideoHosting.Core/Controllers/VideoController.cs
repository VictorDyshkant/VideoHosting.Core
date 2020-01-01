using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Services.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MyWebProject.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private IVideoService VideoService;
        public VideoController(IVideoService service)
        {
            VideoService = service;
        }

        [HttpPost]
        [Route("video")]
        public async Task<ActionResult> AddVideo(VideoDTO model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = User.Identity.Name;
                await VideoService.AddVideo(model);
                return Ok("You added video ");
            }
            return BadRequest("Invalid data: Name,Photo,Video");
        }

        [HttpPost]
        [Route("videofiles")]
        public async Task<ActionResult> LoadVideoFiles()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count == 2)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "UsersContent");
                int count = new DirectoryInfo(path + "/UsersVideos").GetFiles().Length + 1;
                List<string> str = new List<string>();

                for (int i = 0; i < 2; i++)
                {
                    string save = "";
                    if (files[i].FileName.Contains(".JPG") || files[i].FileName.Contains(".jpg"))
                    {
                        save = User.Identity.Name + "Photo" + count + ".JPG";
                        using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", save)))
                        {
                            await files[i].CopyToAsync(stream);
                        }
                    }
                    if (files[i].FileName.Contains(".png"))
                    {
                        save = User.Identity.Name + "Photo" + count + ".png";
                        using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", save)))
                        {
                            await files[i].CopyToAsync(stream);
                        }
                    }
                    if (files[i].FileName.Contains(".mp4"))
                    {
                        save = User.Identity.Name + "Video" + count + ".mp4";
                        using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", save)))
                        {
                            await files[i].CopyToAsync(stream);
                        }
                    }
                    str.Add(save);
                }
                if (str[0].Contains("mp4"))
                {
                    str.Reverse();
                }

                return Ok(str);
            }
            throw new Exception("Here should be 2 files: photo and video");
        }

        [HttpDelete]
        [Route("video/{id}")]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            VideoDTO videoDTO = await VideoService.GetVideoById(id, User.Identity.Name);
            if (videoDTO.UserId == User.Identity.Name || User.IsInRole("Admin"))
            {
                await VideoService.RemoveVideo(id);

                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/VideosPhotos/"+ videoDTO.PhotoPath));
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/UsersVideos/" + videoDTO.VideoPath));

                return Ok("This video was deleted");
            }
            else
            {
                throw new Exception("You do not have permission");
            }
        }

        [HttpGet]
        [Route("video/{id}")]
        public async Task<ActionResult> GetVideoById(int id)
        {
            VideoDTO videoDTO = await VideoService.GetVideoById(id, User.Identity.Name);
            await VideoService.AddView(id);
            return Ok(videoDTO);
        }

        [HttpGet]
        [Route("videosuser/{id}")]
        public async Task<ActionResult> GetVideosOfUser(string id)
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetVideosOfUser(id);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videos")]
        public async Task<ActionResult> GetVideosSubscribers()
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetVideosOfSubscripters(User.Identity.Name);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videosliked/{id}")]
        public async Task<ActionResult> GetLikedVideos(string id)
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetLikedVideos(id);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videosdisliked/{id}")]
        public async Task<ActionResult> GetDislikedVideos(string id)
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetDisLikedVideos(id);
            return Ok(videos);
        }

        [HttpGet]
        [Route("allvideos")]
        public async Task<ActionResult> GetVideo()
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetVideos(User.Identity.Name);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videos/{name}")]
        public async Task<ActionResult> GetVideosName(string name)
        {
            IEnumerable<VideoDTO> videos = await VideoService.GetVideosByName(name, User.Identity.Name);
            return Ok(videos);
        }


        [HttpPut]
        [Route("like/{id}")]
        public async Task<ActionResult> PutLike(int id)
        {
            await VideoService.PutLike(id, User.Identity.Name);
            return Ok();
        }

        [HttpPut]
        [Route("dislike/{id}")]
        public async Task<ActionResult> PutDislike(int id)
        {
            await VideoService.PutDislike(id, User.Identity.Name);
            return Ok();
        }
    }
}
