using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using Microsoft.EntityFrameworkCore;

namespace BoschCartaoDigitalBackEnd.Repository.AreaOperacional
{
    public class AreaOperacionalRepository
    {
        private readonly ProjetoBoschContext _db;

        public AreaOperacionalRepository(ProjetoBoschContext db)
        {
            _db = db;
        }

        public async Task<Colaborador> BuscarColaboradorPorCPFAsync(string cpf)
        {
            return await _db.Colaborador.Where(c => c.Cpf == cpf).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> BuscarColaboradorPorEdvAsync(string edv)
        {
            return await _db.Colaborador.Where(c => c.Edv == edv).FirstOrDefaultAsync();
        }
        public async Task<List<Direito>> BuscarDireitosPorColaboradorIdAsync(int id, int eventoId)
        {
            return await _db.Direito.Where(d => d.ColaboradorId == id && d.EventoId == eventoId)
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Colaborador).AsSplitQuery()
                .ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitosIndicadosPorColaboradorIdAsync(int id, int eventoId)
        {
            return await _db.Direito.Where(d => d.IndicadoId == id && d.EventoId == eventoId)
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Colaborador).AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Evento> BuscarProximoEventoAsync()
        {
            var evento = await _db.Evento.Where(e => e.DataFim.Value.Date >= DateTime.Today && e.DataInicio <= DateTime.Today).FirstOrDefaultAsync();
            if (evento == null)
                evento = await _db.Evento.Where(e => e.DataInicio >= DateTime.Today).OrderBy(e => e.DataInicio.Value.Date).FirstOrDefaultAsync();
            return evento;
        }

        public async Task<Evento> BuscarEventoPorIdAsync(int id)
        {
            return await _db.Evento.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Direito>> DefinirDireitoComoRecebidoAsync(List<long> direitos, int eventoId, int? idRecebedor)
        {
            var temp = await _db.Direito.Where(d => direitos.Contains(d.Id) && d.EventoId == eventoId).ToListAsync();
            foreach (var item in temp)
            {
                item.DataRetirada = DateTime.Now;
                item.RetiradoId = idRecebedor;
                _db.Attach(item).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
            return temp;
        }

        public async Task<Colaborador> BuscarColaboradorPorIDAsync(int id)
        {
            return await _db.Colaborador.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Beneficio>> BuscarTodosBeneficiosEmEvento(int idEvento)
        {
            return await _db.BeneficioEvento.Where(c => c.EventoId == idEvento).Include(d => d.Beneficio).AsSplitQuery().Select(d=> d.Beneficio).ToListAsync();

        }

        public async Task<List<Colaborador>> BuscarTodosColaboradoresBosch()
        {
            //Retorna apenas os colaboradores que trabalham na Bosch
            return await _db.Colaborador.Where(d => d.Edv != null).ToListAsync();
        }

        public async Task<List<Direito>> BuscarBeneficiosEspecificosEmEventoPorIdColaborador(int colaboradorID, int eventoId, int beneficioID)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.ColaboradorId==colaboradorID && d.BeneficioId==beneficioID && d.Retirado==null).ToListAsync();
        }

    }
}