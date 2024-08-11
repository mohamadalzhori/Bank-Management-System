using System.Net;
using TMS.Common.Exceptions;

namespace TMS.Domain.Accounts;

public class AccountNotFound (Guid accountId)
    : CustomException($"Account with id {accountId} not found", HttpStatusCode.NotFound);