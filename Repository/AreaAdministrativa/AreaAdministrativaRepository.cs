using System;
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
            await _db.Evento.Skip(paginacao.Tamanho * (paginacao.Pagina - 1)).Take(paginacao.Tamanho).ToListAsync();
        }
        public async Task<Evento> BuscarEventoPorIdAsync(int id)
        {
            return await _db.Evento.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Beneficio>> ListaBeneficioIdEventoAsync(int eventoId)
        {
            Evento con = await _db.Evento.Where(e => e.Id == eventoId)
                .Include(d => d.BeneficioEvento)
                .ThenInclude(d => d.Beneficio)
                .AsSplitQuery().FirstOrDefaultAsync();
            return ((con == null) ? null : con.BeneficioEvento.Select(c => c.Beneficio).ToList());
        }
        public async Task<List<Beneficio>> ListaBeneficioEventoAtivoAsync()
        {
            Evento con = await _db.Evento.Where(e => e.DataInicio.Value.Date <= DateTime.Today && e.DataFim.Value.Date >= DateTime.Today)
                .Include(d => d.BeneficioEvento)
                .ThenInclude(d => d.Beneficio)
                .AsSplitQuery().FirstOrDefaultAsync();
            return ((con == null) ? null : con.BeneficioEvento.Select(c => c.Beneficio).ToList());
        }
        public async Task<List<Beneficio>> ListaBeneficioProximoEventoAsync()
        {
            Evento con = await _db.Evento.Where(e => e.DataInicio.Value.Date >= DateTime.Today)
                .OrderBy(e => e.DataInicio.Value.Date)
                .Include(d => d.BeneficioEvento)
                .ThenInclude(d => d.Beneficio)
                .AsSplitQuery().FirstOrDefaultAsync();
            return ((con == null) ? null : con.BeneficioEvento.Select(c => c.Beneficio).ToList());
        }
        public async Task RemoverDireitoDeEventoAsync(int idEvento, int idBeneficio)
        {
            var evento = await _db.Evento.Where(e => e.Id == idEvento)
                .Include(e => e.BeneficioEvento).AsSplitQuery()
                .Include(e => e.Direito).AsSplitQuery()
                .FirstOrDefaultAsync();
            var paraRemover = evento.Direito.Where(d => d.BeneficioId == idBeneficio && d.DataRetirada == null).ToList();
            evento.BeneficioEvento = evento.BeneficioEvento.Where(b => b.BeneficioId != idBeneficio).ToList();
            _db.Attach(evento).Collection(e => e.BeneficioEvento).IsModified = true;
            _db.RemoveRange(paraRemover);
            await _db.SaveChangesAsync();
        }
        public async Task<List<BeneficioEvento>> BuscarBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            return await _db.BeneficioEvento.Where(e => e.BeneficioId == beneficioId).ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitoIdBeneficioAsync(int beneficioId)
        {
            return await _db.Direito.Where(e => e.BeneficioId == beneficioId).ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitoIdEventoAsync(int eventoId)
        {
            return await _db.Direito.Where(e => e.EventoId == eventoId).ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitoIdColaboradorIdEventoAsync(int colaboradorId, int eventoId)
        {
            return await _db.Direito.Where(e => e.ColaboradorId == colaboradorId && e.EventoId == eventoId).ToListAsync();
        }
        public async Task<List<Direito>> BuscarDireitoIdColaboradorIdEventoIdBeneficioAsync(int colaboradorId, int eventoId, int beneficioId)
        {
            return await _db.Direito.Where(e => e.ColaboradorId == colaboradorId && e.EventoId == eventoId && e.BeneficioId == beneficioId).ToListAsync();
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
        public async Task ExcluirDireitosAsync(List<Direito> direitos)
        {
            _db.RemoveRange(direitos);
            await _db.SaveChangesAsync();
        }
        public async Task<Evento> ExcluirEventoAsync(Evento evento)
        {
            _db.Entry(evento).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return evento;
        }
        public async Task<List<Beneficio>> BuscarTodosBeneficiosAsync()
        {
            return await _db.Beneficio.ToListAsync();
        }
        public async Task<Colaborador> BuscarColaboradorPorIdAsync(int id)
        {
            return await _db.Colaborador.FindAsync(id);
        }
        public async Task<Colaborador> BuscarColaboradorComDireitosAsync(int id)
        {
            return await _db.Colaborador.Where(c => c.Id == id)
                .Include(c => c.DireitoColaborador).AsSplitQuery().FirstAsync();
        }
        public async Task<Colaborador> BuscarColaboradorCpfEdvDataNascimentoAsync(string Cpf, string Edv, DateTime DataNascimento)
        {
            return await _db.Colaborador.Where(e => e.Cpf == Cpf && e.Edv == Edv && e.DataNascimento == DataNascimento).FirstOrDefaultAsync();
        }

        public async Task<List<Direito>> BuscarDireitosPorIdColaboradorAsync(int eventoId, int colaboradorId)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.ColaboradorId == colaboradorId)
                .Include(d => d.Indicado).AsSplitQuery()
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Retirado).AsSplitQuery()
                .Include(d => d.Colaborador.UnidadeOrganizacional).AsSplitQuery()
                .ToListAsync();

        }
        public async Task<List<Colaborador>> BuscarTodosColaboradoresBosch()
        {
            //Retorna apenas os colaboradores que trabalham na Bosch
            return await _db.Colaborador.Where(d => d.Edv != null).ToListAsync();
        }

        public async Task<Colaborador> BuscarIndicado(int idEvento, int idColaborador)
        { //Assume que um colaborador ou sempre ter?? o mesmo indicado para seus benef??cios, ou n??o ter?? nenhum
            var d = await _db.Direito.Where(d => d.EventoId == idEvento && d.ColaboradorId == idColaborador && d.Indicado != null).FirstOrDefaultAsync();
            if (d == null) { return null; }
            return d.Indicado;

        }

        public async Task<List<UnidadeOrganizacional>> listarUnidadeOrganizacional()
        {
            return await _db.UnidadeOrganizacional.ToListAsync();
        }
        public async Task<Colaborador> CadastrarNovoColaborador(Colaborador colaborador)
        {
            await _db.Colaborador.AddAsync(colaborador);
            await _db.SaveChangesAsync();
            return colaborador;
        }

        public async Task<Colaborador> EditarColaborador(Colaborador colaborador)
        {
            _db.Attach(colaborador).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return colaborador;
        }

        public async Task<Direito> CadastrarDireito(Direito direito)
        {
            await _db.Direito.AddAsync(direito);
            await _db.SaveChangesAsync();
            return direito;
        }

        public async Task<UnidadeOrganizacional> BuscarUnidadeOrganizacionalIdAsync(int id)
        {
            return await _db.UnidadeOrganizacional.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Colaborador> BuscarColaboradorPorEDV(string edv)
        {
            return await _db.Colaborador.Where(c => c.Edv == edv)
                .Include(c => c.UnidadeOrganizacional).AsSplitQuery()
                .FirstOrDefaultAsync();
        }
        public async Task<List<Direito>> BuscarDireitosPorEDVColaboradorAsync(int eventoId, string edv)
        {
            return await _db.Direito.Where(d => d.EventoId == eventoId && d.Colaborador.Edv == edv)
                .Include(d => d.Indicado).AsSplitQuery()
                .Include(d => d.Beneficio).AsSplitQuery()
                .Include(d => d.Retirado).AsSplitQuery()
                .Include(d => d.Colaborador).ThenInclude(c => c.UnidadeOrganizacional).AsSplitQuery()
                .ToListAsync();

        }
    }
}