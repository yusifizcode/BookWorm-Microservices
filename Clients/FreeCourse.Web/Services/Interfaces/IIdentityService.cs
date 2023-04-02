using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using IdentityModel.Client;

namespace FreeCourse.Web.Services.Interfaces;

public interface IIdentityService
{
    Task<Response<bool>> SignUp(SignUpInput signUpInput);
    Task<Response<bool>> SignIn(SignInInput signInInput);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();
}
