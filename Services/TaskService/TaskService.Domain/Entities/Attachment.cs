namespace TaskService.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    //public string FilePath { get; set; } = default!;

    //Navigation
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = default!;
}
