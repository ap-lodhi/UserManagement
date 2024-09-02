using UserManagement.Models;

namespace UserManagement.Interface
{
    public interface IDALDashbordcs
    {
        ResponseModel UpdateUser(RegisterModel obj);
        RegisterModel GetUserDetails(int Id);
    }
}
