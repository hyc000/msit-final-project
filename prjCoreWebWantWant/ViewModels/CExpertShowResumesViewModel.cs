namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertShowResumesViewModel
    {
        public int 專家ID { get; set; }
        public int 專家履歷ID { get; set; }
        public string 專家姓名 { get; set; }
        public string 服務地區 { get; set; }
     
        public string 專長1 { get; set; }
      
        public string 證照1 { get; set; }
     
        public string 專長2 { get; set; }
       
        public string 證照2 { get; set; }

      
        public string 專長3 { get; set; }
      
        public string 證照3 { get; set; }
        public byte[] 履歷照片 { get; set; }
     
        public string 履歷標題 { get; set; } //給本人自己選的

        public string 專家介紹 { get; set; }
        public string? 個人網站 { get; set; }
        public string 聯絡方式 { get; set; }
        public string 提供服務 { get; set; }
        public string 收款方式 { get; set; }
        public string 常見問題 { get; set; }
        public string 基本價格 { get; set; }

        public string 點閱次數 { get; set; }


    }
}
