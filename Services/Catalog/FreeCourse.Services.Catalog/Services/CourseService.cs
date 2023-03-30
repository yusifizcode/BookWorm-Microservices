using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Entities;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CourseService : ICourseService
{
    private readonly IMongoCollection<Course> _courses;
    private readonly IMongoCollection<Category> _categories;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CourseService(IMapper mapper,
                         IDatabaseSettings databaseSettings,
                         IPublishEndpoint publishEndpoint)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _courses = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
        _categories = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Shared.DTOs.Response<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courses.Find(course => true).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categories.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return Shared.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<Shared.DTOs.Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courses.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null)
            return Shared.DTOs.Response<CourseDto>.Fail("Course is not found!", 404);

        course.Category = await _categories.Find(x => x.Id == course.CategoryId).FirstAsync();

        return Shared.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }

    public async Task<Shared.DTOs.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courses.Find<Course>(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categories.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return Shared.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<Shared.DTOs.Response<CourseDto>> CreateAsync(CourseCreateDto createDto)
    {
        var newCourse = _mapper.Map<Course>(createDto);

        newCourse.CreatedTime = DateTime.Now;
        await _courses.InsertOneAsync(newCourse);

        return Shared.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
    }

    public async Task<Shared.DTOs.Response<NoContent>> UpdateAsync(CourseUpdateDto updateDto)
    {
        var updateCourse = _mapper.Map<Course>(updateDto);

        var result = await _courses.FindOneAndReplaceAsync(x => x.Id == updateDto.Id, updateCourse);

        if (result == null)
            return Shared.DTOs.Response<NoContent>.Fail("Course is not found!", 404);

        await _publishEndpoint.Publish<CourseNameChangedEvent>
                               (new CourseNameChangedEvent { CourseId = updateCourse.Id, UpdatedName = updateDto.Name });

        return Shared.DTOs.Response<NoContent>.Success(204);
    }

    public async Task<Shared.DTOs.Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courses.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
            return Shared.DTOs.Response<NoContent>.Success(204);
        else
            return Shared.DTOs.Response<NoContent>.Fail("Course is not found!", 404);
    }
}
