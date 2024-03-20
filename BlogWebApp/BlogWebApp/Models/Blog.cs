using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;


namespace BlogWebApp.Models
{
    public class Blog : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string Tags { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [ValidateNever]
        [DisplayName("Title Image Url")]
        public string TitleImageUrl { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [ValidateNever]
        public SubCategory SubCategory { get; set; }

        [Required]
        public bool IsActive { get; set; }


    }
}
