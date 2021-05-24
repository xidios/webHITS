using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend3.Controllers
{
    public class CounterController : Controller
    {
        public IActionResult Index()
        {
            var model = new CounterViewModel();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CounterAction action, CounterViewModel model)
        {
            this.ValidateCounter(model);
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            switch (action)
            {
                case CounterAction.Increase:
                    model.Actions.Add(action);
                    model.CurrentCount++;
                    this.ModelState.Remove("CurrentCount");
                    return this.View(model);
                case CounterAction.Decrease:
                    model.Actions.Add(action);
                    model.CurrentCount--;
                    this.ModelState.Remove("CurrentCount");
                    return this.View(model);
                case CounterAction.Finish:
                    return this.View("Result", model);
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private void ValidateCounter(CounterViewModel model)
        {
            var expectedCount = model.CurrentCount;
            var actualCount = model.Actions.Count(x => x == CounterAction.Increase) - model.Actions.Count(x => x == CounterAction.Decrease);
            if (expectedCount != actualCount)
            {
                this.ModelState.AddModelError("", "Counter state is invalid");
            }
        }
    }
}
