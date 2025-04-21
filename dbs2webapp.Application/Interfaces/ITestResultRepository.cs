using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITestResultRepository : IBaseRepository<TestResult>
    {
        Task<Test?> GetTestWithQuestionsAsync(int testId);
        Task SaveTestResultAsync(TestResult result);
    }
}
