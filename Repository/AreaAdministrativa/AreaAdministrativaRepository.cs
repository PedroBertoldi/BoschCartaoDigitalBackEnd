using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Extentions;
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
        public async Task<List<Beneficio>> ListaBeneficioIdEventoAsync(int eventoId)
        {
            Evento con = await _db.Evento.Where(e => e.Id == eventoId)
                .Include( d => d.BeneficioEvento )
                .ThenInclude( d => d.Beneficio )
                .AsSplitQuery().FirstOrDefaultAsync();
            return ((con==null) ? null : con.BeneficioEvento.Select(c => c.Beneficio ).ToList());
        }
        public async Task<BeneficioEvento> BuscarBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            return await _db.BeneficioEvento.Where(e => e.BeneficioId == beneficioId).FirstOrDefaultAsync();
        }
        public async Task<List<Direito>> BuscarDireitoIdBeneficioAsync(int beneficioId)
        {
            return await _db.Direito.Where(e => e.BeneficioId == beneficioId).ToListAsync();
        }
        public async Task<List<BeneficioEvento>> BuscarBeneficioEventoIdEventoAsync(int eventoId)
        {
            return await _db.BeneficioEvento.Where(e => e.EventoId == eventoId).ToListAsync();
        }
        public async Task<Evento> AdicionarEventoAsync(Evento evento)
        {
            await _db.Evento.AddAsync(evento);
            await _db.SaveChangesAsync();
            return evento;
        }
        public async Task<Evento> EditarEventoAsync(Evento evento)
        {
            _db.Attach(evento).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return evento;
        }
        public async Task<Beneficio> BuscarBeneficioPorDescricaoAsync(string descricao)
        {
            var descricaoLimpa = descricao.NormalizarString();
            return await _db.Beneficio.Where(b => b.DescricaoNormalizada == descricaoLimpa).FirstOrDefaultAsync();
        }
        public async Task<Beneficio> CriarBeneficioAsync(Beneficio beneficio)
        {
            await _db.Beneficio.AddAsync(beneficio);
            await _db.SaveChangesAsync();
            return beneficio;
        }
        public async Task<Beneficio> BuscarBeneficioPorIdAsync(int id)
        {
            return await _db.Beneficio.Where(b => b.Id == id).FirstOrDefaultAsync();
        }
        public async Task<BeneficioEvento> CriarRelacaoBeneficioEventoAsync(BeneficioEvento beneficioEvento)
        {
            await _db.BeneficioEvento.AddAsync(beneficioEvento);
            await _db.SaveChangesAsync();
            return beneficioEvento;
        }
        public async Task<Beneficio> EditarBeneficioAsync(Beneficio beneficio)
        {
            _db.Attach(beneficio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return beneficio;
        }
        public async Task<BeneficioEvento> ExcluirBeneficioEventoAsync(BeneficioEvento beneficioEvento)
        {
            _db.Entry(beneficioEvento).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return beneficioEvento;
        }
        public async Task<Beneficio> ExcluirBeneficioAsync(Beneficio beneficio)
        {
            _db.Entry(beneficio).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return beneficio;
        }
        public async Task<Direito> ExcluirDireitoAsync(Direito direito)
        {
            _db.Entry(direito).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return direito;
        }
        public async Task<List<Beneficio>> BuscarTodosBeneficiosAsync()
        {
            return await _db.Beneficio.ToListAsync();
        }
    }
}