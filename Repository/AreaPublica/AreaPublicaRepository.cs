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
        public async Task<Evento> BuscarProximoEventoAsync()
        {
            return await _db.Evento.Where(e => e.DataInicio >= DateTime.Now).OrderBy(e => e.DataInicio.Value.Date).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> BuscarColaboradorPorCpfAsync(string cpf)
        {
            return await _db.Colaborador.Where(c => c.Cpf == cpf).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> BuscarColaboradorPorEdvAsync(string edv)
        {
            return await _db.Colaborador.Where(c => c.Edv == edv).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> BuscarColaboradorAsync(string cpf, DateTime dataNascimento)
        {
            return await _db.Colaborador.Where(c => c.Cpf == cpf && c.DataNascimento.Value.Date == dataNascimento.Date).FirstOrDefaultAsync();
        }
        public async Task<Colaborador> CadastrarNovoColaborador(string cpf, string nomeCompleto)
        {
            var temp = new Colaborador
            {
                Cpf = cpf.Trim(),
                NomeCompleto = nomeCompleto.Trim(),
            };
            await _db.Colaborador.AddAsync(temp);
            await _db.SaveChangesAsync();
            return temp;
        }
        public async Task<Colaborador> BuscarColaboradorPorIdAsync(int id)
        {
            return await _db.Colaborador.FindAsync(id);
        }
        public async Task<List<Direito>> BuscarDireitosAsync(int eventoId, int colaboradorId)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.ColaboradorId == colaboradorId)
                .Include(d => d.Indicado).AsSplitQuery()
                .Include(d => d.Colaborador).AsSplitQuery()
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Retirado).AsSplitQuery()
                .ToListAsync();
        }

        public async Task CadastrarIndicadoEmDireitosAsync(int colaboradorId, int eventoId, int indicadoId, List<long> direitos)
        {
            var items = (direitos.Count != 0) ? await _db.Direito.Where(d => d.ColaboradorId == colaboradorId && d.EventoId == eventoId && direitos.Contains(d.Id) && d.DataRetirada == null).ToListAsync()
                : await _db.Direito.Where(d => d.ColaboradorId == colaboradorId && d.EventoId == eventoId && d.DataRetirada == null).ToListAsync();

            foreach (var item in items)
            {
                item.IndicadoId = indicadoId;
            }
            
            await _db.SaveChangesAsync();
        }
        public async Task RemoverIndicacoesEmDireitosAsync(int colaboradorId, int eventoId, List<long> direitos)
        {
            var items = (direitos.Count > 0) ? await _db.Direito.Where(d => d.ColaboradorId == colaboradorId && d.EventoId == eventoId && direitos.Contains(d.Id) && d.DataRetirada == null).ToListAsync()
                : await _db.Direito.Where(d => d.ColaboradorId == colaboradorId && d.EventoId == eventoId && d.DataRetirada == null).ToListAsync();
            foreach (var item in items)
            {
                item.IndicadoId = null;
            }
            await _db.SaveChangesAsync();
        }
        public async Task<List<Direito>> BuscarDireitosIndicadosAsync(int eventoId, int userid)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.IndicadoId == userid)
            .Include(d => d.Indicado).AsSplitQuery()
            .Include(d => d.Colaborador).AsSplitQuery()
            .Include(d => d.Beneficio).AsSplitQuery()
            .Include(d => d.Retirado).AsSplitQuery()
            .ToListAsync();
        }

        //Falta criar a Business
        public async Task<Colaborador> BuscarIndicadoPorIdColaborador(int idTitular, int idEvento, int idBeneficio)
        {
            var x = await _db.Direito.Where(d => d.EventoId == idEvento && d.ColaboradorId==idTitular && d.BeneficioId==idBeneficio).Include(d => d.Indicado)
            .AsSplitQuery().FirstOrDefaultAsync();
            //tratar o valor aqui

            return (x!=null)? x.Indicado:null;

        }

        
    }
}