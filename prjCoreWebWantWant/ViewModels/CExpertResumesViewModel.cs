namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertResumesViewModel
    {
      //  public int? 服務地區代碼 { get; set; }
        public string 服務地區 { get; set; }
      // public int 專長代碼1 { get; set; }
        public string 專長大項1{ get; set; }
        public string 專長細項1 { get; set; }
       // public int 證照代碼1 { get; set; }
        public string 證照大項1 { get; set; }
        public string 證照細項1 { get; set; }

     //   public int 專長代碼2 { get; set; }
        public string 專長大項2 { get; set; }
        public string 專長細項2 { get; set; }
     //   public int 證照代碼2 { get; set; }
        public string 證照大項2 { get; set; }
        public string 證照細項2 { get; set; }

     //   public int 專長代碼3 { get; set; }
        public string 專長大項3 { get; set; }
        public string 專長細項3 { get; set; }
      //  public int 證照代碼3 { get; set; }
        public string 證照大項3 { get; set; }
        public string 證照細項3 { get; set; }
        public byte[] 履歷照片 { get; set; }
        /*public int 履歷狀態 { get; set; } */ //22履歷移除//23顯示履歷
        public string 履歷標題 { get; set; } //給本人自己選的

        public string 專家介紹 { get; set; }
        public string? 個人網站 { get; set; }
        public string 聯絡方式 { get; set; }
        public string 提供服務 { get; set; }
        public string 收款方式 { get; set; }
        public string 常見問題 { get; set; }
        public decimal 基本價格 { get; set; }

        public string 作品名 { get; set; }
        public byte[] 作品圖片 { get; set; }






    }
}
