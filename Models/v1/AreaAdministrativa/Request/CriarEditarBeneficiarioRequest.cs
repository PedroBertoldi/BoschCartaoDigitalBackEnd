using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class CriarEditarBeneficiarioRequest
    {
        /// <summary>
        /// Cpf do beneficiário.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(11)]
        public string Cpf { get; set; }

        /// <summary>
        /// Nome do beneficiário.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// Data de Nascimento do beneficiário.
        /// </summary>
        [Required]
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Id da Unidade Organizacional do beneficiário.
        /// </summary>
        [Required]
        public int? UnidadeOrganizacionalID { get; set; }

        /// <summary>
        /// EDV do beneficiário.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(10)]
        public string EDV { get; set; }

        /* 
        criar um modelo de direitos resumido
        */
        public List<DireitoRequestResumido> beneficios { get; set; }
    }

    /* 
    -------------------------------------------EXEMPLO--------------------------------------------------------
    {
        "cpf":"2222222",                 -- VAI PARA A TABELA "colaborador"
        "nomeCompleto":"Meu nome",       -- VAI PARA A TABELA "colaborador"
        "dataNascimento":"19980206",     -- VAI PARA A TABELA "colaborador"
        "unidadeOrganizacionalID":1,     -- VAI PARA A TABELA "colaborador"
        "EDV":"999999",                  -- VAI PARA A TABELA "colaborador"
        "beneficios":[
            {
                "eventoID":3,            -- VAI PARA A TABELA "direito"
                "beneficioID":5,         -- VAI PARA A TABELA "direito"
                "qtdBeneficio":4         -- VAI SER USADO PARA FAZER MULTIPLOS INSERTS
            },
            {
                "eventoID":3,            -- VAI PARA A TABELA "direito"
                "beneficioID":9,         -- VAI PARA A TABELA "direito"
                "qtdBeneficio":1         -- VAI SER USADO PARA FAZER MULTIPLOS INSERTS
            },
        ]
    }

    CREATE TABLE projetoBosch.dbo.colaborador (
        id int identity(1,1),
        cpf varchar(11) not null,
        nomeCompleto nvarchar(255),
        dataNascimento date,
        unidadeOrganizacionalID int,
        EDV varchar(10),
        Senha varchar(15),
        constraint pk_colaborador_id primary key(id),
        constraint fk_colaborador_unidadeOrganizacionalID foreign key(unidadeOrganizacionalID) references unidadeOrganizacional(id)
    );

    CREATE TABLE projetoBosch.dbo.direito(
        id bigint identity(1,1),
        colaboradorID int not null,
        eventoID int not null,
        beneficioID int,
        indicadoID int,
        retiradoID int,
        dataRetirada datetime,
        constraint pk_direito_id primary key(id),
        constraint fk_direito_colaboradorID foreign key(colaboradorID) references colaborador(id),
        constraint fk_direito_eventoID foreign key(eventoID) references evento(id),
        constraint fk_direito_beneficioID foreign key(beneficioID) references beneficio(id),
        constraint fk_direito_indicadoID foreign key(indicadoID) references colaborador(id),
        constraint fk_direito_retiradoID foreign key(retiradoID) references colaborador(id)
    );
    -------------------------------------------EXEMPLO--------------------------------------------------------
    */
}