using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ReP.Web.Pages {
    public class FirstRunModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;

        public FirstRunModel(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string UserName { get; set; }

            [Required, MaxLength(50), EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

        }

        public IActionResult OnGet() {
            if (this.userManager.Users.Any()) return this.NotFound();
            this.Input.Password = SecurityHelper.GenerateRandomPassword();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (this.userManager.Users.Any()) return this.NotFound();
            if (!this.ModelState.IsValid) return this.Page();

            // Create user
            var user = new ApplicationUser {
                UserName = this.Input.UserName,
                Email = this.Input.Email,
                EmailConfirmed = true,
                Language = CultureInfo.CurrentUICulture.Name,
                Enabled = true
            };
            if (!this.IsIdentitySuccess(await this.userManager.CreateAsync(user, this.Input.Password))) return this.Page();

            // Assign Administrator role
            if (!this.IsIdentitySuccess(await this.userManager.AddToRoleAsync(user, ApplicationRole.Administrator))) return this.Page();

            // Redirect to home page
            return this.RedirectToPage("Index");
        }

    }
}
