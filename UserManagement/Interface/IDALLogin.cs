using UserManagement.Models;

namespace UserManagement.Interface
{
    public interface IDALLogin
    {
        ResponseModel loginUser(RegisterModel log);
    }
}
