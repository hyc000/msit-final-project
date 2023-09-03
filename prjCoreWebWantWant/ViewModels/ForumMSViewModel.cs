namespace prjCoreWebWantWant.ViewModels
{
    public class ForumMSViewModel
    {
        public int PostId { get; set; }

        public int AccountId { get; set; }

        public int? ParentId { get; set; }

        public string? Title { get; set; }

        public string PostContent { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public byte Status { get; set; }

        public int? ViewCount { get; set; }

        public int? LikeCount { get; set; }

    }
}
