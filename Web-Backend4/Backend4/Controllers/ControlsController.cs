using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models.Controls;
using Microsoft.AspNetCore.Mvc;

namespace Backend4.Controllers
{
    public class ControlsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult TextBox()
        {
            return this.View(new TextBoxViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TextBox(TextBoxViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                return this.View("TextBoxResult", model);
            }

            return this.View(model);
        }

        public IActionResult TextArea()
        {
            return this.View(new TextAreaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TextArea(TextAreaViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                return this.View("TextAreaResult", model);
            }

            return this.View(model);
        }

        public IActionResult CheckBox()
        {
            return this.View(new CheckBoxViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckBox(CheckBoxViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                return this.View("CheckBoxResult", model);
            }

            return this.View(model);
        }

        public IActionResult Radio()
        {
            this.ViewBag.AllMonths = this.GetAllMonths();
            return this.View(new RadioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Radio(RadioViewModel model)
        {
            this.ViewBag.AllMonths = this.GetAllMonths();

            if (this.ModelState.IsValid)
            {
                return this.View("RadioResult", model);
            }

            return this.View(model);
        }

        public IActionResult DropDownList()
        {
            this.ViewBag.AllMonths = this.GetAllMonths();
            return this.View(new DropDownListViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DropDownList(DropDownListViewModel model)
        {
            this.ViewBag.AllMonths = this.GetAllMonths();

            if (this.ModelState.IsValid)
            {
                return this.View("DropDownListResult", model);
            }

            return this.View(model);
        }

        public IActionResult ListBox()
        {
            this.ViewBag.AllMonths = this.GetAllMonths();
            return this.View(new ListBoxViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ListBox(ListBoxViewModel model)
        {
            this.ViewBag.AllMonths = this.GetAllMonths();

            if (this.ModelState.IsValid)
            {
                return this.View("ListBoxResult", model);
            }

            return this.View(model);
        }

        private Month[] GetAllMonths()
        {
            return CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Select((x, i) => new Month { Id = i + 1, Name = x })
                .Where(x => !String.IsNullOrEmpty(x.Name))
                .ToArray();
        }
    }
}
