using Client.MVC.Models.PhotoStocks;
using Client.MVC.Services.Interfaces;
using SharedLibrary.DTOs;

namespace Client.MVC.Services;

public class PhotoStockService : IPhotoStockService
{
    private readonly HttpClient _client;

    public PhotoStockService(HttpClient client)
        => _client = client;

    public async Task<bool> DeletePhoto(string photoUrl)
    {
        var response = await _client.DeleteAsync($"photos?photoUrl={photoUrl}");
        return response.IsSuccessStatusCode;
    }

    public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
    {
        if (photo == null || photo.Length <= 0) return null;

        // example: 332542345234523453252345.jpg
        var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

        using var memoryStream = new MemoryStream();
        await photo.CopyToAsync(memoryStream);

        var multiPartContent = new MultipartFormDataContent();
        multiPartContent.Add(new ByteArrayContent(memoryStream.ToArray()), "photo", randomFileName);

        var response = await _client.PostAsync("photos", multiPartContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

        return responseSuccess.Data;
    }
}
