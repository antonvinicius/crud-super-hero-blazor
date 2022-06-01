using BlazorFullStackCourse.Server.Data;
using BlazorFullStackCourse.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorFullStackCourse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext context;

        public SuperHeroController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await context.SuperHeroes
                .Include(h => h.Comic)
                .ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await context.Comics.ToListAsync();
            return Ok(comics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHero(int id)
        {
            var hero = await context.SuperHeroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);
            
            if (hero == null) return NotFound("Hero not found");

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;

            context.SuperHeroes.Add(hero);

            await context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> EditSuperHero(int id, SuperHero hero)
        {
            var dbHero = await context.SuperHeroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (dbHero == null)
            {
                return NotFound("Hero not found!");
            }

            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await context.SuperHeroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (dbHero == null)
            {
                return NotFound("Hero not found!");
            }

            context.SuperHeroes.Remove(dbHero);

            await context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await context.SuperHeroes.Include(h => h.Comic).ToListAsync();
        }
    }
}
