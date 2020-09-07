using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegistrationApp.Messaging.Commands.UpdateUser;
using RegistrationApp.Messaging.Models;
using RegistrationApp.Services;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEnumConverterService _enumConverterService;
        private readonly IMediator _mediator;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IEnumConverterService enumConverterService,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _enumConverterService = enumConverterService;
            _mediator = mediator;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Dansekøn")]
            public string DanceGender { get; set; }

            private bool _beginnerLevel;

            [Display(Name = "Begynder")]
            public bool BeginnerLevel
            {
                get => _beginnerLevel;
                set
                {
                    CheckBoxChanged(Constants.BeginnerDanish, value);
                    _beginnerLevel = value;
                }
            }

            private bool _noviceLevel;


            [Display(Name = "Fortsætter")]
            public bool NoviceLevel
            {
                get => _noviceLevel;
                set
                {
                    CheckBoxChanged(Constants.NoviceDanish, value);
                    _noviceLevel = value;
                }
            }

            private bool _advancedLevel;
            [Display(Name = "Videregående")]
            public bool AdvancedLevel
            {
                get => _advancedLevel;
                set
                {
                    CheckBoxChanged(Constants.AdvancedDanish, value);
                    _advancedLevel = value;
                }
            }

            private bool _themeLevel;

            [Display(Name = "Tema")]
            public bool ThemeLevel
            {
                get => _themeLevel;
                set
                {
                    CheckBoxChanged(Constants.ThemeDanish, value);
                    _themeLevel = value;
                }
            }
        }

        public static List<string> Levels { get; set; } = new List<string>();

        public static void CheckBoxChanged(string level, bool checkedValue)
        {
            if (checkedValue)
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

        public List<string> GetAllGendersInDanish()
        {
            return _enumConverterService.GetAllGendersInDanish();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Levels = user.Levels.Select(_enumConverterService.ConvertLevelToDanishString).ToList();

            Input = new InputModel
            {
                BeginnerLevel = user.Levels.Contains(Level.Beginner),
                NoviceLevel = user.Levels.Contains(Level.Novice),
                AdvancedLevel = user.Levels.Contains(Level.Advanced),
                ThemeLevel = user.Levels.Contains(Level.Theme),
                DanceGender = _enumConverterService.ConvertGenderToDanishString(user.Gender),
                PhoneNumber = phoneNumber
            };


        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public string LevelErrorMessage { get; set; }

        private bool ValidLevels()
        {

            switch (Levels.Count)
            {
                case 1:
                case 2 when Levels.All(x =>
                    x == _enumConverterService.ConvertLevelToDanishString(Level.Advanced) ||
                    x == _enumConverterService.ConvertLevelToDanishString(Level.Theme)):
                    return true;
                default:
                    LevelErrorMessage = "Ugyldig holdvalg";
                    return false;
            }
        }

        private string GetLevelFromDanishString(string level)
        {
            return _enumConverterService.ConvertDanishStringToLevel(level);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid || !ValidLevels())
            {
                await LoadAsync(user);
                return Page();
            }

            var levels = new List<string>();

            Levels.ForEach(x => levels.Add(GetLevelFromDanishString(x)));

            await _mediator.Send(new UpdateUserDetailsCommand(user.Id, levels, _enumConverterService.ConvertDanishStringToGender(Input.DanceGender)), cancellationToken);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
