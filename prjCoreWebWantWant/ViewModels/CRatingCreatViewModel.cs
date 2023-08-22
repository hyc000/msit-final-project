namespace prjCoreWebWantWant.ViewModels
{
    public class CRatingCreatViewModel
    {
        public string taskprincipal { get; set; }//委託者:
        public string taskexperter { get; set; }//接案者:

        public string taskname { get {
                string name = $"委託者{taskprincipal}跟接案者{taskexperter}的委託單";
                return name; } }
        public int starscore { get; set; }

        public string ratingcontent { get; set; }
     

    }
}
