using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var keyVault = builder.AddAzureKeyVault("key-vault");

var apiKey = builder.AddParameter("ApiKeySecret", secret: true);

var secret = keyVault.AddSecret("ApiKey", apiKey);

// Projects
var basket = builder.AddProject<Projects.Basket_API>("basket")
    .WithReference(keyVault)
    .WaitFor(keyVault);

// Reverse Proxy
var gateway = builder.AddYarp("api-gateway-mobile")
    .WithConfiguration(yarp =>
    {
        var basketcluster = yarp.AddCluster(basket);

        yarp.AddRoute("/basket/{**catch-all}", basketcluster)
            .WithTransformPathRemovePrefix("/basket")
            .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
            .WithTransformResponseHeader("X-Powered-By", "YARP");
    })
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();