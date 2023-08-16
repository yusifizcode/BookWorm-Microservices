using Client.MVC.Models;
using Client.MVC.Services.Interfaces;

namespace Client.MVC.Services;

public class UserService : IUserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
        => _client = client;

    public async Task<UserViewModel> GetUser()
        => await _client.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
}
