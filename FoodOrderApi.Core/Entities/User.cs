using FoodOrderApi.Core.Enums;

namespace FoodOrderApi.Core.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Customer;

        public virtual ICollection<AuthenticationLog> AuthenticationLogs { get; set; } = new List<AuthenticationLog>();
    }
}
