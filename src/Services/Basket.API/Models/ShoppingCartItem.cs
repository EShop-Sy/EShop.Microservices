namespace Basket.API.Models;

public record ShoppingCartItem(Guid ProductId, int Quantity, decimal PriceAtAddition);