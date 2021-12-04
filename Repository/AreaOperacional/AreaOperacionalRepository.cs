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
        public async Task<List<Direito>> BuscarDireitosPorColaboradorIdAsync(int id)
        {
            return await _db.Direito.Where(d => d.ColaboradorId == id)
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Colaborador).AsSplitQuery()
                .ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitosIndicadosPorColaboradorIdAsync(int id)
        {
            return await _db.Direito.Where(d => d.IndicadoId == id)
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Colaborador).AsSplitQuery()
                .ToListAsync();
        }
    }
}