using Client.MVC.Models.Catalog;

namespace Client.MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<List<CourseViewModel>> GetAllCourseAsync();
    Task<List<CategoryViewModel>> GetAllCategoryAsync();
    Task<CourseViewModel> GetByCourseId(string courseId);
    Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

    Task<bool> DeleteCourseAsync(string courseId);
    Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
    Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
}
