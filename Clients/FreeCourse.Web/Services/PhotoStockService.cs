using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class PhotoStockService : IPhotoStockService
{
    private readonly HttpClient _client;
    public Task<bool> DeletePhoto(string photoUrl)
    {
        throw new NotImplementedException();
    }

    public Task<PhotoViewModel> UploadPhoto(IFormFile photo)
    {
        throw new NotImplementedException();
    }
}
