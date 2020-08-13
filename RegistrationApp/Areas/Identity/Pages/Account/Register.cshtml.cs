using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RegistrationApp.Services;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IEnumConverterService _enumConverterService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IEnumConverterService enumConverterService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _enumConverterService = enumConverterService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Bekræft Email")]
            [Compare("Email", ErrorMessage = "The email and confirmation email do not match")]
            public string ConfirmEmail { get; set; }

            [Required]
            [Display(Name = "Fuldt Navn")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Telefonnummer")]
            public string TelephoneNumber { get; set; }

            [Required]
            [Display(Name = "Dansekøn")]
            public string DanceGender { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public List<string> Levels { get; set; } = new List<string>();

        public string LevelErrorMessage { get; set; }

        public void CheckboxClicked(string level, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!Levels.Contains(level))
                {
                    Levels.Add(level);
                }
            }
            else
            {
                if (Levels.Contains(level))
                {
                    Levels.Remove(level);
                }
            }
        }

        public List<string> GetAllLevelsInDanish()
        {
            return (from level in (Level.GetAllLevels()) select _enumConverterService.ConvertLevelToDanishString(level)).ToList();
        }

        public List<string> GetAllGendersInDanish()
        {
            return (from gender in (DanceGender.GetAllDanceGenders()) select _enumConverterService.ConvertGenderToDanishString(gender)).ToList();
        }

        private string GetLevelFromDanishString(string level)
        {
            return _enumConverterService.ConvertDanishStringToLevel(level);
        }

        private string GetGenderFromDanishString(string gender)
        {
            return _enumConverterService.ConvertDanishStringToGender(gender);
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public bool ValidLevels()
        {
            if (Levels.Count == 1)
            {
                return true;
            }

            if (Levels.Count == 2 && Levels.All(x =>
                x == _enumConverterService.ConvertLevelToDanishString(Level.Advanced) ||
                x == _enumConverterService.ConvertLevelToDanishString(Level.Theme)))
            {
                return true;
            }

            LevelErrorMessage = "Ugyldig holdvalg";
            return false;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid && ValidLevels())
            {
                var levels = new List<string>();

                Levels.ForEach(x => levels.Add(GetLevelFromDanishString(x)));

                var user = new ApplicationUser(GetGenderFromDanishString(Input.DanceGender))
                {
                    UserName = Input.Email, 
                    Email = Input.Email, 
                    PhoneNumber = Input.TelephoneNumber, 
                    NormalizedUserName = Input.Name,
                    Levels = levels
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
