using Client.MVC.Models;
using Microsoft.Extensions.Options;

namespace Client.MVC.Helpers;

public class PhotoHelper
{
    private readonly ServiceApiSettings _serviceApiSettings;

    public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        => _serviceApiSettings = serviceApiSettings.Value;

    public string GetPhotoStockUrl(string photoUrl)
        => $"{_serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
}
