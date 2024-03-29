using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel;

namespace BlogWebApp.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [DisplayName ("Category Name")]
        public string CategoryName { get; set; }

        [Required]
        [DisplayName("Category Slug")]
        public string CategorySlug { get; set; }

        [ValidateNever]
        public string DefaultImageUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

    }
}
