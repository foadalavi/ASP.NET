using Application.Services.Model;
using Application.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Ui.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginUser UserCredential { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _authService.Login(UserCredential))
            {
                await _authService.GenerateCookieAuthentication(UserCredential.Username);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
