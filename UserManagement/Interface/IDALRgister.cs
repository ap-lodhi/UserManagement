using UserManagement.Models;

namespace UserManagement.Interface
{
    public interface IDALRgister
    {
        ResponseModel Register(RegisterModel reg);
    }
}
