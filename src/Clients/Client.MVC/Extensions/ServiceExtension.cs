﻿using Client.MVC.Handlers;
using Client.MVC.Models;
using Client.MVC.Services;
using Client.MVC.Services.Interfaces;

namespace Client.MVC.Extensions;

public static class ServiceExtension
{
    public static void AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceApiSettings = configuration
                                        .GetSection("ServiceApiSettings")
                                        .Get<ServiceApiSettings>();

        services.AddHttpClient<IIdentityService, IdentityService>();
        services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

        services.AddHttpClient<IUserService, UserService>(opt =>
        {
            opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        //services.AddHttpClient<IUserService, UserService>(opt =>
        //{
        //    opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
        //}).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IBasketService, BasketService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<IDiscountService, DiscountService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<IPaymentService, PaymentService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Payment.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<IOrderService, OrderService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<ICatalogService, CatalogService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
    }
}
