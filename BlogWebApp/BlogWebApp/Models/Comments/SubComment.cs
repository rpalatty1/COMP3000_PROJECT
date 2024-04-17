namespace BlogWebApp.Models.Comments
{
    public class SubComment : Comment
    {
        public int MainCommentId { get; set; }
    }
}
