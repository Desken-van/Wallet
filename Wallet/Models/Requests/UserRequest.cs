namespace Wallet.Models.Requests
{
    public class UserAuthRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class UserRequest : UserAuthRequest
    {
        public string Role { get; set; }
    }

    public class UserUpdateRequest : UserRequest
    {
        public int Id { get; set; }
        public string Role { get; set; }
    }
}
