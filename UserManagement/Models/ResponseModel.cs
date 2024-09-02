namespace UserManagement.Models
{
    public class ResponseModel
    {
        public int UserID {  get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? message { get; set; }
        public Boolean status { get; set; }
    }
}
