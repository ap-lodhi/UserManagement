namespace UserManagement.Helper
{
    public class CommonHelper
    {
        public static void WriteLog(string message)

        {
            string ErrorLogDir = @"C:\Users\win\source\repos\UserManagement\UserManagement\ErrorLog\";
           
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\error1" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sr = new StreamWriter(ErrorLogDir, true))
            {
                sr.WriteLine(DateTime.Now.ToString("DD-MM-yyyy HH-mm-ss") + message);
            }

        }
    }
}
