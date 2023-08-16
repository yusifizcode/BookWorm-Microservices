namespace Catalog.API.DTOs;

public class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string UserId { get; set; }
    public string Picture { get; set; }
    public DateTime CreatedTime { get; set; }
    public int Count { get; set; }
    public string CategoryId { get; set; }
    public CategoryDto Category { get; set; }
}
