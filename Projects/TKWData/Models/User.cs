namespace TKWData.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
}
