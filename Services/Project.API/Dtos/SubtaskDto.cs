namespace Project.API.Dtos;

public record SubtaskDto(Guid Id, Guid TaskId, string Title, bool IsCompleted);
