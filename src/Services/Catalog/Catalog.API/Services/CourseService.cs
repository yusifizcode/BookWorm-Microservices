using AutoMapper;
using Catalog.API.DTOs;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MassTransit;
using MongoDB.Driver;
using SharedLibrary.DTOs;
using SharedLibrary.Messages;

namespace Catalog.API.Services;

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

    public async Task<SharedLibrary.DTOs.Response<List<CourseDto>>> GetAllAsync()
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

        return SharedLibrary.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courses.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null)
            return SharedLibrary.DTOs.Response<CourseDto>.Fail("Course is not found!", 404);

        course.Category = await _categories.Find(x => x.Id == course.CategoryId).FirstAsync();

        return SharedLibrary.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
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

        return SharedLibrary.DTOs.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<CourseDto>> CreateAsync(CourseCreateDto createDto)
    {
        var newCourse = _mapper.Map<Course>(createDto);

        newCourse.CreatedTime = DateTime.Now;
        await _courses.InsertOneAsync(newCourse);

        return SharedLibrary.DTOs.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<NoContent>> UpdateAsync(CourseUpdateDto updateDto)
    {
        var updateCourse = _mapper.Map<Course>(updateDto);

        var result = await _courses.FindOneAndReplaceAsync(x => x.Id == updateDto.Id, updateCourse);

        if (result == null)
            return SharedLibrary.DTOs.Response<NoContent>.Fail("Course is not found!", 404);

        await _publishEndpoint.Publish<CourseNameChangedEvent>
                               (new CourseNameChangedEvent { CourseId = updateCourse.Id, UpdatedName = updateDto.Name });

        return SharedLibrary.DTOs.Response<NoContent>.Success(204);
    }

    public async Task<SharedLibrary.DTOs.Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courses.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
            return SharedLibrary.DTOs.Response<NoContent>.Success(204);
        else
            return SharedLibrary.DTOs.Response<NoContent>.Fail("Course is not found!", 404);
    }
}
