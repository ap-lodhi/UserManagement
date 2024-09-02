using System.Data.SqlClient;
using System.Data;
using UserManagement.Models;
using UserManagement.Helper;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Xml.Linq;
using UserManagement.Interface;

namespace UserManagement.DAL
{
    public class DALLogin: IDALLogin
    {
        private readonly IConfiguration config;

        public DALLogin(IConfiguration configuration)
        {
            config = configuration;
        }
        public ResponseModel loginUser(RegisterModel log)
        {
            var constring = config["customData:connectionString"];
            ResponseModel result = new ResponseModel();

            using (SqlConnection con = new SqlConnection(constring))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.AddWithValue("@Email", log.Email);
                        string encryptedPassword = EncryptPassword(log.Password);
                        cmd.Parameters.AddWithValue("@Password", encryptedPassword);

                        SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                        SqlParameter mobileParam = new SqlParameter("@MobileNumber", SqlDbType.NVarChar, 15) { Direction = ParameterDirection.Output };
                        SqlParameter addressParam = new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(userIdParam);
                        cmd.Parameters.Add(nameParam);
                        cmd.Parameters.Add(mobileParam);
                        cmd.Parameters.Add(addressParam);


                        // cmd.ExecuteReader();
                        cmd.ExecuteNonQuery();

                        int UserId = (int)userIdParam.Value;
                        string Name = nameParam.Value.ToString();
                        string MobileNumber = mobileParam.Value.ToString();
                        string Address = addressParam.Value.ToString();

                        if (UserId !=0)
                        {
                            result.status = true;
                            result.message = "Login successful.";
                            result.UserID = UserId;
                            result.Email = log.Email;
                            result.Name = Name;
                            result.mobile = MobileNumber;
                            result.Address = Address;
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Invalid email or password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;
                    result.message = "An error occurred. Please try again later.";
                    CommonHelper.WriteLog("ex" + ex);
                    Console.WriteLine("Error is:" + ex);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }

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
