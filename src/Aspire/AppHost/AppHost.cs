using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

// Projects
var basket = builder.AddProject<Projects.Basket_API>("basket");

// Reverse Proxy
var gateway = builder.AddYarp("api-gateway-mobile")
    .WithHostPort(5004)
    .WithConfiguration(yarp =>
    {
        var basketcluster = yarp.AddCluster(basket);

        yarp.AddRoute("/basket/{**catch-all}", basketcluster)
            .WithTransformPathRemovePrefix("/basket")
            .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
            .WithTransformResponseHeader("X-Powered-By", "YARP");
    });

await builder.Build().RunAsync();