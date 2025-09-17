using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId, Guid TenantId) : IRequest<GetProjectByIdResult>;
public record GetProjectByIdResult(ProjectDetailsDto Project);

public class GetProjectByIdHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetProjectByIdQuery, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectDetailsAsync(request.ProjectId, request.TenantId, cancellationToken);
        return new GetProjectByIdResult(project);
    }
}
