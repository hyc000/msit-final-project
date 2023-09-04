namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertKeyword
    {
        //關鍵字搜尋
        public string? txtKeyword { get; set; }
        //證照篩選
        public string? SelectedCertificate { get; set; }
        //專長篩選
        public string? SelectedSkill { get; set; }
        //最高價篩選
        public decimal? SelectedMaxPrice { get; set; }
    }
}
