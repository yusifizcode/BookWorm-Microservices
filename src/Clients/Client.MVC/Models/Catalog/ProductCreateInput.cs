﻿using System.ComponentModel.DataAnnotations;

namespace Client.MVC.Models.Catalog;

public class ProductCreateInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? UserId { get; set; }
    public string? Picture { get; set; }
    public int Count { get; set; }
    [Display(Name = "Category")]
    public string CategoryId { get; set; }
    public IFormFile PhotoFormFile { get; set; }
}
