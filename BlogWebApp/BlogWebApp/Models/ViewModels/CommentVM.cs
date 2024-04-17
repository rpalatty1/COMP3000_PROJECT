using Microsoft.Build.Framework;

namespace BlogWebApp.Models.ViewModels
{
    public class CommentVM
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
