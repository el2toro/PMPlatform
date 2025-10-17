using Core.Exceptions;

namespace TaskService.Domain.Exceptions;

public class TaskNotFoundException : NotFoundException
{
    public TaskNotFoundException(string id) : base("Task", id)
    {
    }
}
