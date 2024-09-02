using System.Data;

using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Helper;
using UserManagement.Interface;
using UserManagement.Models;
namespace UserManagement.DAL
{
    public class DalRegister : IDALRgister
    {
        private readonly IConfiguration config;

        public DalRegister(IConfiguration configuration)
        {
            config = configuration;
        }


        #region User  Register 
        public ResponseModel Register(RegisterModel reg)
        {
            // Access the connection string from customData
            var constring = config["customData:connectionString"];

            ResponseModel result = new ResponseModel();
            using (SqlConnection con = new SqlConnection(constring))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", reg.Name);
                        cmd.Parameters.AddWithValue("@Email", reg.Email);
                        cmd.Parameters.AddWithValue("@MobileNumber", reg.MobileNumber);
                        cmd.Parameters.AddWithValue("@Address", reg.Address);
                        string encryptedPassword = EncryptPassword(reg.Password);
                        cmd.Parameters.AddWithValue("@Password", encryptedPassword);

                        var id = cmd.ExecuteNonQuery();

                        if (id > 0)
                        {
                            result.status = true;
                            result.message = "Register  Successfully";
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Please Check...Something Went wrong...!!!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;
                    CommonHelper.WriteLog("ex" + ex);
                    Console.WriteLine("Error is:" + ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }
        #endregion
        private string EncryptPassword(string password)
        {
            string encryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

}
