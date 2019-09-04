namespace FactWeb.Mvc.Models
{
    public class UpdatePasswordModel
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}