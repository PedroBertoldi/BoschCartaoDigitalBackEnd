using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Repository.AreaAdmin
{
    public class AreaAdminRepository
    {
        private readonly ProjetoBoschContext _db;

        public AreaAdminRepository(ProjetoBoschContext db)
        {
            _db = db;
        }
    }
}