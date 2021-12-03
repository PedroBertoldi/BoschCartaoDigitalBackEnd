using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Repository.AreaAdmin
{
    public class AreaAdminRepository
    {
        private readonly ProjetoBoschContext _db;

        public AreaAdminRepository(ProjetoBoschContext db)
        {
            _db = db;
        }

        public async Task<List<Beneficio>> ListaBeneficioIdEventoAsync(int? eventoId)
        {
            Evento con = await _db.Evento.Where(e => e.Id == (int)eventoId)
                .Include( d => d.BeneficioEvento )
                .ThenInclude( d => d.Beneficio )
                .AsSplitQuery().FirstOrDefaultAsync();
            return ((con==null) ? null : con.BeneficioEvento.Select(c => c.Beneficio ).ToList());
        }
    }
}