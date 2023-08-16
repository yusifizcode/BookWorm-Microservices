using Client.MVC.Models;
using Client.MVC.Services.Interfaces;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.MVC.Services;

public class ClientCredentialTokenService : IClientCredentialTokenService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _clientSettings;
    private readonly ServiceApiSettings _serviceApiSettings;
    private readonly IClientAccessTokenCache _clientAccessTokenCache;

    public ClientCredentialTokenService(HttpClient httpClient,
                                        IOptions<ClientSettings> clientSettings,
                                        IOptions<ServiceApiSettings> serviceApiSettings,
                                        IClientAccessTokenCache clientAccessTokenCache)
    {
        _httpClient = httpClient;
        _clientSettings = clientSettings.Value;
        _serviceApiSettings = serviceApiSettings.Value;
        _clientAccessTokenCache = clientAccessTokenCache;
    }

    public async Task<string> GetToken()
    {
        var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", null);

        if (currentToken != null)
            return currentToken.AccessToken;

        var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (discovery.IsError)
            throw discovery.Exception;

        var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = _clientSettings.WebClient.ClientId,
            ClientSecret = _clientSettings.WebClient.ClientSecret,
            Address = discovery.TokenEndpoint
        };

        var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

        if (newToken.IsError)
            throw newToken.Exception;

        await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, null);

        return newToken.AccessToken;
    }
}
