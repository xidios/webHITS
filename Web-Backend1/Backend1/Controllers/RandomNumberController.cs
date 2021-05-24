using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers
{
    public class RandomNumberController : Controller
    {
        private readonly Services.IRandomNumberService randomNumberService;
        private readonly Services.ICalculationSevice calculationSevice;
        public RandomNumberController(Services.IRandomNumberService randomNumberService, Services.ICalculationSevice calculationSevice)
        {
            this.randomNumberService = randomNumberService;
            this.calculationSevice = calculationSevice;
        }       
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PassUsingModel()
        {
          
            int a = randomNumberService.GenerateNumber();
            int b = randomNumberService.GenerateNumber();
            var model = new CalculatingRadnomNumbersModel
            {
                GeneratedNumber1 = a,
                GeneratedNumber2 = b,
                Addition = calculationSevice.Addition(a, b),
                Multiplication = calculationSevice.Multiplication(a,b),
                Subtraction= calculationSevice.Subtraction(a,b)

                
            };          
            try
            {               
                model.Division = calculationSevice.Division(a, b);                
            }
            catch (DivideByZeroException e)
            {                
                this.ModelState.AddModelError(nameof(CalculatingRadnomNumbersModel.Division),e.Message);//поле / сообщение об ошибке
            }
            
            return View(model);
        }
        public IActionResult PassUsingViewBag()
        {
            ViewBag.GeneratedNumber1 = randomNumberService.GenerateNumber(); 
            ViewBag.GeneratedNumber2 = randomNumberService.GenerateNumber(); 
            ViewBag.Addition = calculationSevice.Addition(ViewBag.GeneratedNumber1, ViewBag.GeneratedNumber2);
            ViewBag.Subtraction = calculationSevice.Subtraction(ViewBag.GeneratedNumber1, ViewBag.GeneratedNumber2);
            ViewBag.Multiplication = calculationSevice.Multiplication(ViewBag.GeneratedNumber1, ViewBag.GeneratedNumber2);
            try
            {
                ViewBag.Division = calculationSevice.Division(ViewBag.GeneratedNumber1, ViewBag.GeneratedNumber2);
            }
            catch (DivideByZeroException e)
            {
                ViewBag.Error = e.Message;//поле / сообщение об ошибке
            }
            return View();
        }
        public IActionResult PassUsingViewData()
        {
            
           
            int r1 = randomNumberService.GenerateNumber();
            ViewData["GeneratedNumber1"] = r1;
            int r2 = randomNumberService.GenerateNumber();
            ViewData["GeneratedNumber2"] = r2;
            ViewData["Addition"] = calculationSevice.Addition(r1, r2);
            ViewData["Subtraction"] = calculationSevice.Subtraction(r1, r2);
            ViewData["Multiplication"] = calculationSevice.Multiplication(r1, r2);
            try
            {
                ViewData["Division"] = calculationSevice.Division(r1, r2);
            }
            catch (DivideByZeroException e)
            {
                ViewData["Error"] = e.Message;//поле / сообщение об ошибке
            }
            return View();
        }
        public IActionResult AccessServiceDirectly()
        {
            return View();
        }
    }
}
