using measurement_generator.Models.Erp;
using measurement_generator.Repository;
using Microsoft.EntityFrameworkCore;

namespace measurement_generator.Services
{
    public class ErpsService
    {
        private readonly AppDBContext _db;

        public ErpsService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<Erp>> GetAllErp()
        {
            var erps = await _db.Erps.ToListAsync();
            return erps;
        }

        public async Task<List<Erp>> GetErpsWithHaveFile()
        {
            var erps = await _db.Erps.Where(e => e.HaveFile == true).ToListAsync();
            return erps;
        }

    }
}
