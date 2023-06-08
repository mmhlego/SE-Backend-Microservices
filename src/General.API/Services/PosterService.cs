using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models;
using Microsoft.EntityFrameworkCore;

namespace General.API.Services {
    public class PosterService : IPosterService {
        private readonly GeneralContext _context;

        public PosterService(GeneralContext context) {
            _context = context;
        }

        public List<Poster> GetPosters() {
            return _context.Posters.ToList();
        }

        public Poster? GetPosterById(Guid posterId) {
            return _context.Posters.FirstOrDefault(c => c.Id == posterId);
        }
        public void AddPoster(string title, PosterTypes type, string imageUrl, string targetUrl) {
            var poster = new Poster {
                Title = title,
                Type = type,
                ImageUrl = imageUrl,
                TargetUrl = targetUrl
            };
            _context.Posters.Add(poster);
            _context.SaveChanges();
        }

        public void UpdatePoster(Poster poster) {
            var p = _context.Posters.FirstOrDefault(P => P.Id == poster.Id);
            if (p == null)
                return;

            p.ImageUrl = poster.ImageUrl;
            p.TargetUrl = poster.TargetUrl;
            p.Title = poster.Title;
            p.Type = poster.Type;
            _context.Posters.Update(p);
            _context.SaveChanges();
        }
        public void DeletePoster(Guid id) {
            var poster = _context.Posters.Where(c => c.Id == id).FirstOrDefault();

            if (poster == null) {
                // throw new Exception("No comment with the specified id was found");
                return;
            }

            _context.Posters.Remove(poster);
            _context.SaveChanges();
        }
    }
}