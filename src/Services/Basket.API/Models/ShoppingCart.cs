namespace Basket.API.Models;

public record ShoppingCart(Guid Id, List<ShoppingCartItem> Items)
{
    public decimal TotalPrice => Items.Sum(i => i.PriceAtAddition * i.Quantity);
}