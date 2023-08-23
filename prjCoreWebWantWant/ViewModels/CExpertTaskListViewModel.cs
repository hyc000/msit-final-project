namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertTaskListViewModel
    {
        public IEnumerable<CExpertTaskViewModel> mytasking { get; set; }
        public IEnumerable<CExpertTaskViewModel> mytasked { get; set; }

        public IEnumerable<CExpertTaskViewModel> taskfromotherno { get; set; }

        public IEnumerable<CExpertTaskViewModel> taskingfromother { get; set; }

        public IEnumerable<CExpertTaskViewModel> taskedfromother { get; set; }



    }
}
