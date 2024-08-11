using AutoMapper;
using BMS.Application.Branches;
using BMS.Domain.Branches;

namespace BMS.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Branch, GetBranchVM>();
    } 
}