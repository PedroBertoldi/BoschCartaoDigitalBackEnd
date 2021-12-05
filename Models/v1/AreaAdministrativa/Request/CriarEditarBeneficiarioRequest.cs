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
        public int? UnidadeOrganizacionalId { get; set; }

        /// <summary>
        /// EDV do beneficiário.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(10)]
        public string EDV { get; set; }

        /* 
        criar um modelo de direitos resumido
        */
        public List<CriarEditarBeneficiarioDireitoRequestResumido> beneficios { get; set; }
    }
}