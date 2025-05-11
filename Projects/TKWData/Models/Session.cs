using TKWData.Models;

public class Session
{
    public Guid Id { get; set; }
    public User User { get; set; } = null!;
}