using Application.Interfaces;
using dbs2webapp.Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TestResultRepository : BaseRepository<TestResult>, ITestResultRepository
    {
        private readonly AppDbContext _context;

        public TestResultRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Test?> GetTestWithQuestionsAsync(int testId)
        {
            return await _context.Tests
                .Include(t => t.Questions)!
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == testId);
        }

        public async Task SaveTestResultAsync(TestResult result)
        {
            await _context.TestResults.AddAsync(result);
            await _context.SaveChangesAsync();
        }
    }
}
