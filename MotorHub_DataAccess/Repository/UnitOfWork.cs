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
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICarRepository Car { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        Car = new CarRepository(_db);
        Brand = new BrandRepository(_db);
        ShoppingCart = new ShoppingCartRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
