namespace DatingApp.API.DTOs
{
    public class UserLoginDTO
    {
        //Username and Password are being checked against the database, so no validation necessary here
        public string Username { get; set; }
        public string Password { get; set; }
    }
}