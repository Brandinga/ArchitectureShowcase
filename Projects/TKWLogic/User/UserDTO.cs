using TKWData.Models;

// The DTO that excludes the password (we don't want that exposed to clients)
public class UserDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

public static class UserMappingExtensions
{
    public static UserDTO AsUserDTO(this User user)
    {
        return new()
        {
            Id = user.Id,
            Name = user.Name,
        };
    }
}