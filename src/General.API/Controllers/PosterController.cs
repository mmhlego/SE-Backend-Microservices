using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models.Requests;
using General.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharedModels;
using System.Security.Claims;
using General.API.Services;

namespace General.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PosterController : ControllerBase
    {
        PosterService _poster;
        public PosterController(PosterService poster)
        {
            _poster = poster;
        }

        [HttpGet]
        [Route("posters/")]
        public ActionResult<List<Poster>> GetPosters(PosterTypes? posterType)
        {
            List<Poster> posters = _poster.GetPosters();

            if (posterType.HasValue)
            {
                posters = posters.Where(p => p.Type == posterType).ToList();
            }
            return Ok(posters);
        }

        [HttpPost]
        [Route("posters/")]
        [Authorize(Roles = "Admin")]
        public IActionResult PostPosters(PosterPost request)
        {
            _poster.AddPoster(request.Title, request.Type, request.ImageUrl, request.TargetUrl);
            return Ok();
        }

        [HttpPut]
        [Route("posters/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult PutPoster(Guid posterId, [FromBody] PosterPost request)
        {
            Poster? poster = _poster.GetPosterById(posterId);
            if (poster == null)
                return StatusCode(StatusCodes.Status404NotFound);

            poster.Id = posterId;
            poster.Title = request.Title;
            poster.ImageUrl = request.ImageUrl;
            poster.TargetUrl = request.TargetUrl;
            poster.Type = request.Type;

            _poster.UpdatePoster(poster);
            return Ok();
        }

        [HttpDelete]
        [Route("posters/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePoster(Guid posterId)
        {
            Poster? poster = _poster.GetPosterById(posterId);

            if (poster == null)
            {
                return BadRequest();
            }
            _poster.DeletePoster(posterId);
            return Ok(poster);
        }
    }
}