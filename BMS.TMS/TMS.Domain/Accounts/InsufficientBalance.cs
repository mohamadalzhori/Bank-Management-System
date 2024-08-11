using System.Net;
using TMS.Common.Exceptions;

namespace TMS.Domain.Accounts;

public class InsufficientBalance(decimal amount)
    : CustomException($"Insufficient balance to withdraw {amount}", HttpStatusCode.BadRequest);