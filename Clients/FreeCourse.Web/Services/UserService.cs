using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class UserService : IUserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
        => _client = client;

    public async Task<UserViewModel> GetUser()
        => await _client.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
}
