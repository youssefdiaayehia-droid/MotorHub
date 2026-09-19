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
    public class CarRepository : Repository<Car> , ICarRepository
    {
        private readonly ApplicationDbContext _db;
        public CarRepository(ApplicationDbContext db) : base(db)
        {        }
    }
}
