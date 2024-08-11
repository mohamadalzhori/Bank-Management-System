using System.Net;
using BMS.Common.Exceptions;

namespace BMS.Domain.Branches;

public class DuplicateBranchNameException(string branchName)
    : CustomException($"Branch with name '{branchName}' already exists.", HttpStatusCode.Conflict);