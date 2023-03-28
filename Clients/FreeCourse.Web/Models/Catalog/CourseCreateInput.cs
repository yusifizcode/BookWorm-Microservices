﻿using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog;

public class CourseCreateInput
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    public string? UserId { get; set; }
    public string? Picture { get; set; }
    public FeatureViewModel Feature { get; set; }
    [Display(Name = "Category")]
    [Required]
    public string CategoryId { get; set; }
    public IFormFile PhotoFormFile { get; set; }
}
