using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models;
using Microsoft.EntityFrameworkCore;

namespace General.API.Services {
    public class PosterService : IPosterService
    {
        private readonly GeneralContext _context;

        public PosterService(GeneralContext context)
        {
            _context = context;
        }

        public IEnumerable<Poster> GetPosters()
        {
            return _context.Posters.ToList();
        }

        public void AddPoster(string title, PosterTypes type, string imageUrl, string targetUrl)
        {
            var poster = new Poster
            {
                Title = title,
                Type = type,
                ImageUrl = imageUrl,
                TargetUrl = targetUrl
            };
            _context.Posters.Add(poster);
            _context.SaveChanges();
        }

        public void UpdatePoster(Poster poster)
        {
           var P = _context.Posters.FirstOrDefault(P => P.Id == poster.Id);
            P.ImageUrl = poster.ImageUrl;
            P.TargetUrl = poster.TargetUrl;
            P.Title = poster.Title;
            P.Type = poster.Type;
            _context.Posters.Update(P);
            _context.SaveChanges();
        }
    }
}