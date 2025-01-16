using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagement.Read.Api.Helpers;

public class RestaurantOwnerAuthorizeAttribute() : AuthorizeAttribute(Constants.RestaurantOwnerPolicy);
