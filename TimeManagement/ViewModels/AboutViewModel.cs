using TimeManagement.Services;

namespace TimeManagement.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Username => _pageService.ReturnUsername().Result;
      
        private readonly PageService _pageService;
        public AboutViewModel()
        {
            _pageService = new PageService();
        }
    }
}