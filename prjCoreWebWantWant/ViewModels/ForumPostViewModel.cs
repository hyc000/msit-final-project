
using prjCoreWebWantWant.Models;

namespace prjCoreWantMember.ViewModels
{
    public class ForumPostViewModel
    {
        public ForumPost MainPost { get; set; }
        public List<ForumPost> Replies { get; set; }
        public List<ForumPostComment> MainComments { get; set; }
        public List<List<ForumPostComment>> SecondCommentsList { get; set; }
        public MemberAccount? Member{ get; set; }
    }
}
