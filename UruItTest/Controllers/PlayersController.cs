using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UruItTest.Models;

namespace UruItTest.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PlayersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return Ok(await _context.Players.ToListAsync());
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(player);
        }

        // POST: api/Players
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Player>> Post(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }


        // GET: api/Players/StartGame
        [HttpGet("StartGame")]
        public async Task<ActionResult<Player>> StartPlayerGame(string playerANickname, string playerBNickname)
        {
            var selectedPlayerA = _context.Players.Where(e => e.Nickname == playerANickname).ToList();
            var selectedPlayerB = _context.Players.Where(e => e.Nickname == playerBNickname).ToList();
            Player createdPlayerA;
            Player createdPlayerB;
            if (selectedPlayerA.Count < 1)
            {
                var player = new Player(playerANickname, 0);
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                createdPlayerA = player;
                selectedPlayerA.Add(createdPlayerA);
            };

            if (selectedPlayerB.Count < 1)
            {
                var player = new Player(playerBNickname, 0);
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                createdPlayerB = player;
                selectedPlayerB.Add(createdPlayerB);
            };
            var opts = new Dictionary<string, List<Player>>();
            opts.Add("playerA", selectedPlayerA);
            opts.Add("playerB", selectedPlayerB);

            return Ok(opts);
        }


        // GET: api/Players/GetScoreBoard
        [HttpGet("GetScoreBoard")]
        public ActionResult<Player> GetScoreBoard()
        {
            return Ok(_context.Players.OrderByDescending(o => o.Score).ToList().Take(10));
;
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }


    }
}
