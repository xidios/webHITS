using System;
using Backend2.Models;
using Backend2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend2.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            this.calculatorService = calculatorService;
        }

        public ActionResult Manual()
        {
            if (this.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                string snumber1 = this.Request.Form["Number1"];
                string snumber2 = this.Request.Form["Number2"];
                String operation = this.Request.Form["Operation"];
                var resultModel = new CalculatorResultViewModel();


                if (String.IsNullOrEmpty(snumber1))
                {
                    ViewBag.Number1Null = "First operand is requared";
                }

                if (String.IsNullOrWhiteSpace(snumber2))
                {
                    ViewBag.Number2Null = "Second operand is requared";
                }

                if (snumber2 == "0" && operation == "/")
                {
                    ViewBag.DividedByZero = "Division by zero is forbiden";
                }
                if (String.IsNullOrEmpty(snumber1) ||
                    String.IsNullOrEmpty(snumber2) ||
                    (snumber2 == "0" && operation == "/"))
                {
                    return this.View();
                }


                int number1, number2;
                if(!int.TryParse(snumber1, out number1))
                {
                    ViewBag.ErrorParse1 = "Parse error";
                }

                if (!int.TryParse(snumber2, out number2))
                {
                    ViewBag.ErrorParse2 = "Parse error";                   
                }
                if(!int.TryParse(snumber1, out number1)
                    || !int.TryParse(snumber2, out number2))
                {
                    return this.View();
                }
                var result = this.calculatorService.Calculating(number1, number2, operation);
                resultModel.Result = result;

                return this.View(resultModel);
            }

            return this.View();
        }

        public ActionResult ManualWithSeparateHandlers()
        {
            return this.View();
        }

        [HttpPost, ActionName("ManualWithSeparateHandlers")]
        [ValidateAntiForgeryToken]
        public ActionResult ManualWithSeparateHandlersConfirm()
        {
            string snumber1 = this.Request.Form["Number1"];
            string snumber2 = this.Request.Form["Number2"];
            String operation = this.Request.Form["Operation"];
            var resultModel = new CalculatorResultViewModel();


            if (String.IsNullOrEmpty(snumber1))
            {
                ViewBag.Number1Null = "First operand is requared";
            }

            if (String.IsNullOrEmpty(snumber2))
            {
                ViewBag.Number2Null = "Second operand is requared";

            }

            if (snumber2 == "0" && operation == "/")
            {
                ViewBag.DividedByZero = "Division by zero is forbiden";
            }
            if (String.IsNullOrEmpty(snumber1) || String.IsNullOrEmpty(snumber2) || (snumber2 == "0" && operation == "/")) return this.View();

            int number1, number2;
            if (!int.TryParse(snumber1, out number1))
            {
                ViewBag.ErrorParse1 = "Parse error";
            }

            if (!int.TryParse(snumber2, out number2))
            {
                ViewBag.ErrorParse2 = "Parse error";
            }
            if (!int.TryParse(snumber1, out number1)
                || !int.TryParse(snumber2, out number2))
            {
                return this.View();
            }

            var result = this.calculatorService.Calculating(number1, number2, operation);
            resultModel.Result = result;

            return this.View(resultModel);
        }
        public ActionResult ModelBindingInParameters()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInParameters(int? snumber1, int? snumber2, String operation)//int?
        {
            var resultModel = new CalculatorResultViewModel();


            if (snumber1==null)
            {
                ViewBag.Number1Null = "First operand is requared";
            }

            if (snumber2==null)
            {
                ViewBag.Number2Null = "Second operand is requared";
            }

            if (snumber2 == 0 && operation == "/")
            {
                ViewBag.DividedByZero = "Division by zero is forbiden";
            }
            if (snumber1==null || snumber2==null || (snumber2 == 0 && operation == "/")) return this.View();

            

            var result = this.calculatorService.Calculating(snumber1.Value, snumber2.Value, operation);
            resultModel.Result = result;

            return this.View(resultModel);
        }
        public ActionResult ModelBindingInSeparateModel()
        {
            return this.View(new CalculatorViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInSeparateModel(CalculatorViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.Number1==null)
                {
                    this.ModelState.AddModelError("Number1", "First operand is requared");
                }

                if (model.Number2==null)
                {
                    this.ModelState.AddModelError("Number2", "Second operand is requared");
                }

                if (model.Number2 == 0 && model.Operation == "/")
                {
                    this.ModelState.AddModelError("Number2", "Division by zero is forbiden");
                }
                if (model.Number1 == null || model.Number2 == null || (model.Number2 == 0 && model.Operation == "/")) return this.View(model);

                

                model.Result = this.calculatorService.Calculating(model.Number1.Value, model.Number2.Value, model.Operation);

                return this.View(model);
            }
            return this.View(model);
        }
    }
}
