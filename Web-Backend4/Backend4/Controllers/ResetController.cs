using System;
using Backend4.Models;
using Backend4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend4.Controllers
{
    public class ResetController : Controller
    {
        private readonly IPasswordResetService passwordResetService;

        public ResetController(IPasswordResetService passwordResetService)
        {
            this.passwordResetService = passwordResetService;
        }

        public IActionResult Index()
        {
            var model = new ResetViewModel();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Boolean sendCode, ResetViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (sendCode)
            {
                this.passwordResetService.SendResetCode(model.Email);
            }
            
            return this.View("CodeVerification", new ResetCodeVerificationViewModel
            {
                Email = model.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CodeVerification(ResetCodeVerificationViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!this.passwordResetService.VerifyResetCode(model.Email, model.Code))
            {
                this.ModelState.AddModelError("Code", "Invalid Code");
                return this.View(model);
            }

            return this.View("PasswordReset", new ResetPasswordViewModel
            {
                Email = model.Email,
                Code = model.Code
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordReset(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!this.passwordResetService.ApplyResetCode(model.Email, model.Code, model.Password))
            {
                this.ModelState.AddModelError("", "Invalid Code");
                return this.View(model);
            }

            return this.View("Succeed", model);
        }
    }
}
