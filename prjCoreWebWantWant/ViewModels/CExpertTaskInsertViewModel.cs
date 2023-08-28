namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertTaskInsertViewModel
    {

        public string 委託人 { get; set; }
        public int 委託人ID { get; set; }
        public string 被委託人 { get; set; }
        public int 被委託人ID { get; set; }
        public string 委託內容 { get; set; }

        public int 委託期限方式 { get; set; }
        public string 委託時間起 { get; set; }
        public string 委託時間訖 { get; set; }
        public int 委託價格 { get; set; }

        public string 委託工作地點 { get; set; }
        public string? 指定委託地點 { get; set; }
    

        

       
    }
}
