using BlazorFullStackCourse.Shared;

namespace BlazorFullStackCourse.Client.Services.SuperHeroService
{
    public interface ISuperHeroService
    {
        List<SuperHero> Heroes { get; set; }
        List<Comic> Comics { get; set; }
        Task GetComics();
        Task<SuperHero> GetSingleSuperHero(int id);
        Task GetAllSuperHeroes();
        Task CreateHero(SuperHero hero);
        Task DeleteHero(int id);
        Task UpdateHero(SuperHero hero);
    }
}
