using AutoMapper;
using AutoMapper.QueryableExtensions;
using BMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMS.Application.Branches.Queries;

public record GetAllBranchesQuery : IRequest<List<GetBranchVM>>;

public class GetAllBranchesQueryHandler(SharedDbContext sharedDbContext, IMapper mapper) : IRequestHandler<GetAllBranchesQuery, List<GetBranchVM>>
{
    public async Task<List<GetBranchVM>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
    {
        return await sharedDbContext.Branches
            .ProjectTo<GetBranchVM>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken); 
    }
}
