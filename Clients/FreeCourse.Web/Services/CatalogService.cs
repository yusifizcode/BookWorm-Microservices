using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;
    public CatalogService(HttpClient client)
        => _client = client;

    public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
    {
        var response = await _client.PostAsJsonAsync<CourseCreateInput>("Courses", courseCreateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        var response = await _client.DeleteAsync($"Courses/{courseId}");

        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
    {
        // http://localhost:5000/services/catalog/categories
        var response = await _client.GetAsync("Categories");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<List<CourseViewModel>> GetAllCourseAsync()
    {
        // http://localhost:5000/services/catalog/courses
        var response = await _client.GetAsync("Courses");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
    {
        // http://localhost:5000/services/catalog/courses/GetAllByUserId/{userId}
        var response = await _client.GetAsync($"Courses/GetAllByUserId/{userId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<CourseViewModel> GetByCourseId(string courseId)
    {
        // http://localhost:5000/services/catalog/courses/{courseId}
        var response = await _client.GetAsync($"Courses/{courseId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
        return responseSuccess.Data;
    }

    public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
    {
        var response = await _client.PutAsJsonAsync<CourseUpdateInput>("Courses", courseUpdateInput);

        return response.IsSuccessStatusCode;
    }
}
