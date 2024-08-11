using AMS.Application.Branches;
using AMS.Application.Customers.Queries;
using AMS.Domain.Branches;
using AMS.Domain.Customers;
using AutoMapper;

namespace AMS.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Customer, GetCustomerVM>();
        
        CreateMap<Branch, GetBranchVM>();
    } 
}