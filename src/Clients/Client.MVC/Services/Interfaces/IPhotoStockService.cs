using Client.MVC.Models.PhotoStocks;

namespace Client.MVC.Services.Interfaces;

public interface IPhotoStockService
{
    Task<PhotoViewModel> UploadPhoto(IFormFile photo);
    Task<bool> DeletePhoto(string photoUrl);
}
