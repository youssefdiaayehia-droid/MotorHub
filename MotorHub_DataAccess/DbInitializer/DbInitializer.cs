using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorHub_DataAccess.Data;
using MotorHub_Models;
using MotorHub_Utility;

namespace MotorHub_DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // 1. Migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            // 2. Roles
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

            }

            // 3. تعيين Admin Role للمستخدم admin@motorhub.com
            var adminUser = _userManager.FindByEmailAsync("admin@motorhub.com").GetAwaiter().GetResult();

            if (adminUser != null)
            {
                var isAdmin = _userManager.IsInRoleAsync(adminUser, SD.Role_Admin).GetAwaiter().GetResult();
                if (!isAdmin)
                {
                    _userManager.AddToRoleAsync(adminUser, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}