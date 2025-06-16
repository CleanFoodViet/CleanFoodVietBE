using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Role
    {
        [Key]
        public Ulid RoleId { get; set; }
        public string Name{ get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
