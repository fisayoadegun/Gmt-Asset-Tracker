using AutoMapper;
using EmailService;
using Gmt_Asset_Tracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Gmt_Asset_Tracker.Controllers;
using System.Linq;

namespace Gmt_Asset_Tracker.Controllers
{
	public class AccountController : Controller
	{
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IEmailSender _emailSender;

		public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
		{
			_mapper = mapper;
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UserRegistrationModel userModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userModel);
			}

			var user = _mapper.Map<User>(userModel);

			var result = await _userManager.CreateAsync(user, userModel.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return View(userModel);
			}

			//await _userManager.AddToRoleAsync(user);

			return RedirectToAction(nameof(AssetController.Index), "Asset");
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
		{
			if (!ModelState.IsValid)
			{
				return View(userModel);
			}

			var signedUser = await _userManager.FindByEmailAsync(userModel.Email);
			var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);
			if (result.Succeeded)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);

				var roles = await _userManager.GetRolesAsync(signedUser);
				return RedirectToAction(nameof(AssetController.Index), "Asset");
			}
			else
			{
				ModelState.AddModelError("", "Invalid UserName or Password");
				return View();
			}
			//return RedirectToAction(nameof(ContactsController.Index), "Contacts");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(AssetController.Index), "Asset");
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			if (!ModelState.IsValid)
				return View(forgotPasswordModel);

			var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
			if (user == null)
				return RedirectToAction(nameof(ForgotPasswordConfirmation));

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

			var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
			await _emailSender.SendEmailAsync(message);

			return RedirectToAction(nameof(ForgotPasswordConfirmation));
		}

		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string token, string email)
		{
			var model = new ResetPasswordModel { Token = token, Email = email };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			if (!ModelState.IsValid)
				return View(resetPasswordModel);

			var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
			if (user == null)
				RedirectToAction(nameof(ResetPasswordConfirmation));

			var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
			if (!resetPassResult.Succeeded)
			{
				foreach (var error in resetPassResult.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return View();
			}

			return RedirectToAction(nameof(ResetPasswordConfirmation));
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);
			else
				return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Home");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}