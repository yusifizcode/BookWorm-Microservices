using AutoMapper;
using Catalog.API.DTOs;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MassTransit;
using MongoDB.Driver;
using SharedLibrary.DTOs;
using SharedLibrary.Messages;

namespace Catalog.API.Services;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<Category> _categories;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductService(IMapper mapper,
                         IDatabaseSettings databaseSettings,
                         IPublishEndpoint publishEndpoint)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _products = database.GetCollection<Product>(databaseSettings.CourseCollectionName);
        _categories = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SharedLibrary.DTOs.Response<List<ProductDto>>> GetAllAsync()
    {
        var products = await _products.Find(course => true).ToListAsync();

        if (products.Any())
        {
            foreach (var product in products)
            {
                product.Category = await _categories.Find<Category>(x => x.Id == product.CategoryId).FirstAsync();
            }
        }
        else
        {
            products = new List<Product>();
        }

        return SharedLibrary.DTOs.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<ProductDto>> GetByIdAsync(string id)
    {
        var product = await _products.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (product == null)
            return SharedLibrary.DTOs.Response<ProductDto>.Fail("Product is not found!", 404);

        product.Category = await _categories.Find(x => x.Id == product.CategoryId).FirstAsync();

        return SharedLibrary.DTOs.Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<List<ProductDto>>> GetAllByUserIdAsync(string userId)
    {
        var products = await _products.Find<Product>(x => x.UserId == userId).ToListAsync();

        if (products.Any())
        {
            foreach (var product in products)
            {
                product.Category = await _categories.Find<Category>(x => x.Id == product.CategoryId).FirstAsync();
            }
        }
        else
        {
            products = new List<Product>();
        }

        return SharedLibrary.DTOs.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<ProductDto>> CreateAsync(ProductCreateDto productDto)
    {
        var newProduct = _mapper.Map<Product>(productDto);

        newProduct.CreatedTime = DateTime.Now;
        await _products.InsertOneAsync(newProduct);

        return SharedLibrary.DTOs.Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);
    }

    public async Task<SharedLibrary.DTOs.Response<NoContent>> UpdateAsync(ProductUpdateDto updateDto)
    {
        var updateProduct = _mapper.Map<Product>(updateDto);

        var result = await _products.FindOneAndReplaceAsync(x => x.Id == updateDto.Id, updateProduct);

        if (result == null)
            return SharedLibrary.DTOs.Response<NoContent>.Fail("Product is not found!", 404);

        await _publishEndpoint.Publish<ProductNameChangedEvent>
                               (new ProductNameChangedEvent { ProductId = updateProduct.Id, UpdatedName = updateDto.Name });

        return SharedLibrary.DTOs.Response<NoContent>.Success(204);
    }

    public async Task<SharedLibrary.DTOs.Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _products.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
            return SharedLibrary.DTOs.Response<NoContent>.Success(204);
        else
            return SharedLibrary.DTOs.Response<NoContent>.Fail("Product is not found!", 404);
    }
}
