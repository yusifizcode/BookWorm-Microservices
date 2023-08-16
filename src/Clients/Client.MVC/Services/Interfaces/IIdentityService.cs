using Client.MVC.Models;
using IdentityModel.Client;
using SharedLibrary.DTOs;

namespace Client.MVC.Services.Interfaces;

public interface IIdentityService
{
    Task<Response<bool>> SignUp(SignUpInput signUpInput);
    Task<Response<bool>> SignIn(SignInInput signInInput);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();
}
