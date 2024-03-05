using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Models
{
    public class BlogImages : BaseEntity
    {
        [Required]
        [DisplayName("Blog Title")]
        public string BlogTitle { get; set; }

        [DisplayName("Image Url")]
        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Sub Category")]
        public int SubCategoryId { get; set; }

        [ValidateNever]
        public SubCategory subCategory { get; set; }
    }
}
