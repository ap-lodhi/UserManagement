using System.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using UserManagement.Models;
using UserManagement.Helper;
using UserManagement.Interface;

namespace UserManagement.DAL
{
    public class DALDashbord: IDALDashbordcs
    {

        private readonly IConfiguration config;

        public DALDashbord(IConfiguration configuration)
        {
            config = configuration;
        }

        public ResponseModel UpdateUser(RegisterModel obj)
        {
            var constring = config["customData:connectionString"];
            ResponseModel result = new ResponseModel();


            using (SqlConnection con = new SqlConnection(constring))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUserDetails", con))

                    {
                        cmd.Parameters.AddWithValue("@Email", obj.Email);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", obj.Name);
                        cmd.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        cmd.Parameters.AddWithValue("@Address", obj.Address);


                        var Id = cmd.ExecuteNonQuery();



                        if (Id > 0)
                        {
                            result.status = true;
                            result.message = "User Info Updated Successfully";


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
                    con.Dispose();

                }
            }
            return result;

        }




        public RegisterModel GetUserDetails(int Id)

        {
            var constring = config["customData:connectionString"];
            ResponseModel res = new ResponseModel();

            RegisterModel User = new RegisterModel();

            using (SqlConnection con = new SqlConnection(constring))

            {

                con.Open();
                
                try

                {

                    using (SqlCommand cmd = new SqlCommand("sp_GetUserDetails", con))

                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", Id);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {
                                 User.Name = string.IsNullOrWhiteSpace(reader["Name"].ToString()) ? "" : reader["Name"].ToString();
                                 User.Email = string.IsNullOrWhiteSpace(reader["Email"].ToString()) ? "" : reader["Email"].ToString();
                                 User.MobileNumber = string.IsNullOrWhiteSpace(reader["MobileNumber"].ToString()) ? "" : reader["MobileNumber"].ToString();
                                 User.Address = string.IsNullOrWhiteSpace(reader["Address"].ToString()) ? "" : reader["Address"].ToString();

                              


                            }

                        }

                    }

                }

                catch (Exception ex)

                {

                    res.status = false;

                    res.message = "Errorr!!!";

                }

                finally

                {

                    con.Close();

                    con.Dispose();

                }


            }

            return User;

        }

    }
}
