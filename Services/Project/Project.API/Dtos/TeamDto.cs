namespace Project.API.Dtos;

public record TeamDto(IEnumerable<UserDto> Users, int Count);

