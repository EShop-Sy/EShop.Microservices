using Basket.API.Models;
using Basket.API.Services;

namespace Basket.API.Endpoints;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("").RequireAuthorization();

        // GET by id
        group.MapGet("/{id:guid}", async (Guid id, BasketService service) =>
            {
                var cart = await service.GetBasket(id);
                return cart is null ? Results.NotFound() : Results.Ok(cart);
            })
            .WithName("GetBasketById")
            .Produces<ShoppingCart>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}