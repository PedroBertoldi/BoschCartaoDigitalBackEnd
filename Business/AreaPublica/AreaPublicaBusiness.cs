using BoschCartaoDigitalBackEnd.Database.Context;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaPublica;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica;
using Microsoft.AspNetCore.Mvc;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Business.Commom;

namespace BoschCartaoDigitalBackEnd.Business.AreaPublica
{
    
    public class AreaPublicaBusiness : BaseBussiness
    {
        private readonly AreaPublicaRepository _repository;
        public AreaPublicaBusiness(AreaPublicaRepository repository) : base()
        {
            _repository = repository;
        }

        /// <summary>
        /// Faz uma busca nos direitos e retorna os direitos disponíveis para o usuário.
        /// </summary>
        public async Task<List<Direito>> BuscarMeusDireitosAsync(MeusBeneficiosRequest request){
            if (request == null) throw new ArgumentException($"{nameof(request)} é um parametro obrigatório");
            var evento = (request.EventoID == null) ? await _repository.BuscarEventoAtivoAsync() 
                : await _repository.BuscarEventoPorIdAsync((int)request.EventoID);
            
            if (evento == null)
            {
                _errors.Add(new ErrorModel{
                    FieldName = nameof(request.EventoID),
                    Message = (request.EventoID == null) ? "Atualmente não existe nem um evento ativo" 
                        : $"Não foi encontrado nem um evento com o id: {request.EventoID}",
                });
                return null;
            }
            var colaborador = await _repository.BuscarColaboradorAsync(request.Cpf, request.DataNascimento.Value);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel{
                    FieldName = nameof(request.Cpf),
                    Message = "Usuário não encontrado"
                });
                return null;
            }
            
            return await _repository.BuscarDireitosAsync(evento.Id, colaborador.Id);
        }
    }
}