using Client.MVC.Models;

namespace Client.MVC.Services.Interfaces;

public interface IUserService
{
    Task<UserViewModel> GetUser();
}
