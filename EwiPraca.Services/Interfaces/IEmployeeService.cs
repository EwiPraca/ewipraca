using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IEmployeeService : IService<Employee>
    {
        Employee GetByPESEL(string PESEL);
    }
}