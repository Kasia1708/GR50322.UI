using Microsoft.AspNetCore.Mvc;

namespace GR50322.UI.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke ()
        {
            return View();
        }
    }
}
