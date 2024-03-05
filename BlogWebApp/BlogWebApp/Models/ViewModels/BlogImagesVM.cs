using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWebApp.Models.ViewModels
{
    public class BlogImagesVM
    {
        public BlogImages BlogImages { get; set; }
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }

        [ValidateNever]
        public List<IFormFile>? Files { get; set; }
    }
}
