using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using BrandonSimpleBlog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    public class AccountInfoModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMediaStorageService _mediaStorageService;
        private readonly IConfiguration _configuration;

        public AccountInfoModel(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMediaStorageService mediaStorageService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mediaStorageService = mediaStorageService;
            _configuration=configuration;
        }

        public string ProfileImageURL { get; set; }
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name ="First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (user.HasAvatarImage)
            {
                ProfileImageURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/profile/" + user.Id + ".jpg";
            }
            else
            {
                ProfileImageURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/profile/default.jpg";
            }
            
            Username = userName;

            Input = new InputModel
            {
                Email = email,

                PhoneNumber = phoneNumber,
                FirstName=user.FirstName,
                LastName=user.LastName,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                var firstNameEditResult = await _userManager.UpdateAsync(user);
                if (!firstNameEditResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting first name for user with ID '{userId}'.");
                }
            }

            if (Input.LastName!=user.LastName)
            {
                user.LastName = Input.LastName;
                var lastNameEditResult = await _userManager.UpdateAsync(user);
                if (!lastNameEditResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting last name for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadProfileImages(IFormFile profileImage, IFormFile avatarImage)
        {
            bool avatarUploadSuccess=false;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (profileImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    profileImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    await _mediaStorageService.SaveProfileImage(fileBytes, profileImage.FileName, user.Id);
                }
            }

            if (avatarImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    avatarImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    avatarUploadSuccess= await _mediaStorageService.SaveAvatarImage(fileBytes, avatarImage.FileName, user.Id);
                }
            }
            user.HasAvatarImage = avatarUploadSuccess;
            var avatarUpdateResult = await _userManager.UpdateAsync(user);
            if (avatarUpdateResult.Succeeded)
            {
                StatusMessage = "Avatar and profile images updated.";
            }
            return RedirectToPage();
        }
    }
}