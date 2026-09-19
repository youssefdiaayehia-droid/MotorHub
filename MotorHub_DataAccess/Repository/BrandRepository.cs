using MotorHub_DataAccess.Data;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorHub_DataAccess.Repository
{
    public class BrandRepository :Repository<Brand> , IBrandRepository
    {
        private ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        { }

    }
}
