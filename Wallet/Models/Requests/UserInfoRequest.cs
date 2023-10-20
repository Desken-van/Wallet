namespace Wallet.Models.Requests
{
    public class UserInfoRequest
    {
        public int UserId { get; set; }

        public double CurrentBalance { get; set; }
    }

    public class UserInfoUpdateRequest : UserInfoRequest
    {
        public int Id { get; set; }
    }
}
