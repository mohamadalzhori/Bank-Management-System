using System.Net;
using TMS.Common.Exceptions;

namespace TMS.Domain.Branches;

public class BranchNotFound(Guid branchId)
    : CustomException($"Branch with id {branchId} was not found.", HttpStatusCode.NotFound);