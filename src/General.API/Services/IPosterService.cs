using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Models;

namespace General.API.Services {
    public interface IPosterService
    {
        IEnumerable<Poster> GetPosters();
        void AddPoster(string title, PosterTypes type, string imageUrl, string targetUrl);
        void UpdatePoster(Poster poster);
    }
}