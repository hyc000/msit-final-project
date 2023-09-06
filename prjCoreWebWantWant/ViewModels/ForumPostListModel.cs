using prjCoreWebWantWant.Models;
using X.PagedList;

namespace prjCoreWantMember.ViewModels
{
    public class ForumPostListModel
    {
        public IPagedList<ForumPost> PagedPosts { get; set; }
        public Dictionary<int, int> ReplyCounts { get; set; }
        public List<LatestReplyViewModel>? LatestReplies { get; set; }

        public class LatestReplyViewModel
        {
            public DateTime LatestReplyTime { get; set; }
            public string? UserName { get; set; }
        }
    }


}
