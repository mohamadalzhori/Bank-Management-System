using System.Net;
using BMS.Common.Exceptions;

namespace AMS.Domain.Branches;

public class BranchNotFound(Guid branchId)
    : CustomException($"Branch with id {branchId} was not found.", HttpStatusCode.NotFound);