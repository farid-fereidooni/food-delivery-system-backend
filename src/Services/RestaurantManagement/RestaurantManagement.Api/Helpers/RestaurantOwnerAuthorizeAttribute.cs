using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagement.Api.Helpers;

public class RestaurantOwnerAuthorizeAttribute() : AuthorizeAttribute(Constants.RestaurantOwnerPolicy);
