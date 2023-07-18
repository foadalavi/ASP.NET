using Application.Services.Identity;
using Application.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Ui.Pages.Account
{
    [Authorize]
    public class AddUserModel : PageModel
    {
        private IAuthService _authService;

        [BindProperty]
        public LoginUser UserCredential { get; set; }

        public AddUserModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.RegisterUser(UserCredential))
            {
                return RedirectToPage("/Account/Login");
            }
            return BadRequest();
        }
    }
}
