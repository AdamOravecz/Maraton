using Microsoft.EntityFrameworkCore;
using System.Linq;
using Maraton_webAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maraton_webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutokController : ControllerBase
    {

        private readonly MaratonContext _db;

        public FutokController(MaratonContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Futok>>> GetFutok()
        {
            var list = await _db.Futoks
                .AsNoTracking()
                .OrderBy(f => f.Fid)
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{fid:int}/eredmenyek")]
        public async Task<ActionResult<IEnumerable<Eredmenyek>>> GetFutoEredmenyek(int fid)
        {
            var exists = await _db.Futoks.AnyAsync(f => f.Fid == fid);
            if (!exists) return NotFound("Nincs ilyen futó (fid).");

            var list = await _db.Eredmenyeks
                .AsNoTracking()
                .Where(e => e.Futo == fid)
                .OrderBy(e => e.Kor)
                .ToListAsync();

            return Ok(list);
        }

        public class UpdateEredmenyDto
        {
            public int Ido { get; set; }
        }

        [HttpPut("{fid:int}/eredmenyek/{kor:int}")]
        public async Task<ActionResult> UpdateEredmeny(int fid, int kor, [FromBody] UpdateEredmenyDto dto)
        {
            if (dto is null) return BadRequest("Hiányzó body.");
            if (dto.Ido <= 0) return BadRequest("Az időnek pozitívnak kell lennie.");

            var row = await _db.Eredmenyeks.FindAsync(fid, kor); 
            if (row is null) return NotFound("Nincs ilyen eredmény sor (futo+kor).");

            row.Ido = dto.Ido;
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{fid:int}")]
        public async Task<ActionResult> DeleteFuto(int fid)
        {
            var futo = await _db.Futoks.FindAsync(fid);
            if (futo is null) return NotFound("Nincs ilyen futó (fid).");

            _db.Futoks.Remove(futo);
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("{fid:int}/potol/{kor:int}/{ido:int}")]
        public async Task<ActionResult> PotolEredmeny(int fid, int kor, int ido)
        {
            if (ido <= 0) return BadRequest("Az időnek pozitívnak kell lennie.");

            var futoExists = await _db.Futoks.AnyAsync(f => f.Fid == fid);
            if (!futoExists) return NotFound("Nincs ilyen futó (fid).");

            var row = await _db.Eredmenyeks.FindAsync(fid, kor);
            if (row is null)
            {
                _db.Eredmenyeks.Add(new Eredmenyek
                {
                    Futo = fid,
                    Kor = kor,
                    Ido = ido
                });

                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFutoEredmenyek), new { fid }, null);
            }

            row.Ido = ido;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("nok")]
        public async Task<ActionResult<IEnumerable<object>>> GetNoiFutok()
        {


            var list = await _db.Futoks
                .AsNoTracking()
                .Where(f => !f.Ffi)   
                .OrderBy(f => f.Fnev)
                .Select(f => new { f.Fnev, f.Szulev })
                .ToListAsync();

            return Ok(list);
        }

    }
}
