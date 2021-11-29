using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using Microsoft.EntityFrameworkCore;

namespace BoschCartaoDigitalBackEnd.Repository.AreaPublica
{
    public class AreaPublicaRepository
    {
        private readonly ProjetoBoschContext _db;

        public AreaPublicaRepository(ProjetoBoschContext db)
        {
            _db = db;
        }

        public async Task<Evento> BuscarEventoAtivoAsync()
        {
            return await _db.Evento.Where(e => e.DataFim.Value.Date >= DateTime.Now && e.DataInicio.Value.Date <= DateTime.Now).FirstOrDefaultAsync();
        }

        public async Task<Evento> BuscarEventoPorIdAsync(int id)
        {
            return await _db.Evento.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> BuscarColaboradorAsync(string cpf, DateTime dataNascimento)
        {
            return await _db.Colaborador.Where(c => c.Cpf == cpf && c.DataNascimento.Value.Date == dataNascimento.Date).FirstOrDefaultAsync();
        }

        public async Task<List<Direito>> BuscarDireitosAsync(int eventoId, int colaboradorId)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.ColaboradorId == colaboradorId)
                .Include(d => d.Beneficio).AsSplitQuery().ToListAsync();
        }
    }
}