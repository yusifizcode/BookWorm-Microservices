namespace Client.MVC.Services.Interfaces;

public interface IClientCredentialTokenService
{
    Task<String> GetToken();
}
