using AMS.Persistence.Shared;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AMS.Application.Branches.Queries;

public record GetAllBranchesQuery : IRequest<List<GetBranchVM>>;

public class GetAllBranchesQueryHandler(SharedDbContext sharedDbContext, IMapper mapper) : IRequestHandler<GetAllBranchesQuery, List<GetBranchVM>>
{
    public async Task<List<GetBranchVM>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
    {
        return await sharedDbContext.Branches
            .ProjectTo<GetBranchVM>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}