using System.Net;
using BMS.Common.Exceptions;

namespace AMS.Domain.Customers;

public class NumberOfAccountsExceeded(int maxNumberOfAccounts)
    : CustomException($"The maximum allowed number of accounts ({maxNumberOfAccounts}) has been reached.", HttpStatusCode.Conflict);

