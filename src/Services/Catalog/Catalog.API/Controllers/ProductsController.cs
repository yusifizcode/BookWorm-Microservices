using Catalog.API.DTOs;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : CustomBaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
        => _productService = productService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _productService.GetAllAsync();
        return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _productService.GetByIdAsync(id);
        return CreateActionResultInstance(response);
    }

    [HttpGet]
    [Route("/api/[controller]/GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _productService.GetAllByUserIdAsync(userId);
        return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto createDto)
    {
        var response = await _productService.CreateAsync(createDto);
        return CreateActionResultInstance(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(ProductUpdateDto updateDto)
    {
        var response = await _productService.UpdateAsync(updateDto);
        return CreateActionResultInstance(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _productService.DeleteAsync(id);
        return CreateActionResultInstance(response);
    }
}
