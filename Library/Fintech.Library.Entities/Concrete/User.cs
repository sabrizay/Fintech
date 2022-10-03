
using Fintech.Library.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintech.Library.Entities.Concrete;

[Table("Users")]
public class User : BaseEntity, IEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }

    public Decimal UserPrice { get; set; }

    public List<CurrencyHistory> currencyHistories { get; set; }
}
