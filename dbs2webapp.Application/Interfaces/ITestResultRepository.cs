using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITestResultRepository : IBaseRepository<TestResult>
    {
        Task<Test?> GetTestWithQuestionsAsync(int testId);
        Task SaveTestResultAsync(TestResult result);
    }
}
