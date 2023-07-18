using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Ui.Pages
{
    //[Authorize]
    [Authorize(Roles ="Admin")]
    //[Authorize(Roles ="admin1")]
    public class OnlyAdminPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
