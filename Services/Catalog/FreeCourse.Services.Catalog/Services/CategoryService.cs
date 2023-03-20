using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Entities;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Category> _categories;
    public CategoryService(IMapper mapper,
                           IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _categories = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categories
                                      .Find(c => true)
                                      .ToListAsync();

        return Response<List<CategoryDto>>
              .Success(_mapper.Map<List<CategoryDto>>(categories), 200);
    }

    public async Task<Response<CategoryDto>> CreateAsync(Category category)
    {
        await _categories
                     .InsertOneAsync(category);

        return Response<CategoryDto>
              .Success(_mapper.Map<CategoryDto>(category), 200);
    }

    public async Task<Response<CategoryDto>> GetByIdAsync(string id)
    {
        var category = await _categories
                                    .Find<Category>(c => c.Id == id)
                                    .FirstOrDefaultAsync();

        if (category == null)
            return Response<CategoryDto>.Fail("Category is not found!", 404);

        return Response<CategoryDto>
              .Success(_mapper.Map<CategoryDto>(category), 200);
    }
}
