using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using Microsoft.EntityFrameworkCore;

namespace BoschCartaoDigitalBackEnd.Repository.AreaAdministrativa
{
    public class AreaAdministrativaRepository
    {
        private readonly ProjetoBoschContext _db;

        public AreaAdministrativaRepository(ProjetoBoschContext db)
        {
            _db = db;
        }
        public async Task<List<Evento>> BuscarTodosEventosAsync(PaginacaoRequest paginacao)
        {
            return (paginacao.Tamanho == 0) ? await _db.Evento.ToListAsync() : 
            await _db.Evento.Skip(paginacao.Tamanho * (paginacao.Pagina -1)).Take(paginacao.Tamanho).ToListAsync();
        }
        public async Task<Evento> BuscarEventoPorIdAsync(int id)
        {
            return await _db.Evento.Where(e => e.Id == id).FirstOrDefaultAsync();
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