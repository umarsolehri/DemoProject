using DemoProjectBOL.Models;
using DemoProjectDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectBLL
{
    public class DevTestService
    {
        private readonly DemoProjectContext db;
        public DevTestService() : this(new DemoProjectContext()) { }
        public DevTestService(DemoProjectContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<DevTest>> ListDevs()
        {
            return await db.DevTest.ToListAsync();
        }
        public async Task<DevTest> DevsDetails(int? id)
        {
            return await db.DevTest.Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddDevs(DevTest test)
        {
            db.DevTest.Add(test);
            await Save();
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
        public async Task<DevTest> FindbyAffiliation(string name)
        {
            return await db.DevTest.Where(i => i.AffiliateName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
        public async Task<DevTest> FindById(int? id)
        {
            return await db.DevTest.Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task Update(DevTest test)
        {
            db.Entry(test).State = EntityState.Modified;
            await Save();
        }

        public async Task DeleteDev(int? id)
        {
            var find = await FindById(id);
            db.DevTest.Remove(find);
            await Save();
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                db.Dispose();
            }

            _disposed = true;
        }
    }
}
