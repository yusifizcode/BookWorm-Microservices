using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Services.Interfaces;

public interface ICatalogService
{
    Task<List<CourseViewModel>> GetAllCourseAsync();
    Task<CourseViewModel> GetByCourseId(string courseId);
    Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

    Task<bool> DeleteCourseAsync(string courseId);
    Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
    Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
}
