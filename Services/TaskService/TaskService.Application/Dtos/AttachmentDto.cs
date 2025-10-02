using TaskService.Domain.Entities;

namespace TaskService.Application.Dtos;

public record AttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    //public string FilePath { get; set; } = default!;
    public Guid TaskId { get; set; }
}
