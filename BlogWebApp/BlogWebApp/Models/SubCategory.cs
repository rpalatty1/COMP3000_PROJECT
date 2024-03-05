using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel;

namespace BlogWebApp.Models
{
    public class SubCategory : BaseEntity
    {

        [Required]
        [DisplayName("Sub Category Name")]
        public string SubCategoryName { get; set; }
        [Required]
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
