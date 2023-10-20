using System;

namespace Wallet.Core.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashCode { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
