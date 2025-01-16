using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagement.Write.Api.Helpers;

public class RestaurantOwnerAuthorizeAttribute() : AuthorizeAttribute(Constants.RestaurantOwnerPolicy);
