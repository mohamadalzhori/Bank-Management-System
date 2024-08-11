using System.Net;
using BMS.Common.Exceptions;

namespace AMS.Domain.Customers;

public class CustomerNotFound(Guid customerId)
    : CustomException($"Customer with id {customerId} was not found.", HttpStatusCode.NotFound);