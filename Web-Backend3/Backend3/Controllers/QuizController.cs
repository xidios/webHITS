using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend3.Services;
using Backend3.Models;

namespace Backend3.Controllers
{
    public class QuizController : Controller
    {

        private readonly ICalculationSevice calculationSevice;
        private readonly IRandomNumberService randomNumberService;
        private readonly IRandomOperationService randomOperationService;
        public QuizController(ICalculationSevice calculationSevice, IRandomNumberService randomNumberService, IRandomOperationService randomOperationService)
        {
            this.randomNumberService = randomNumberService;
            this.calculationSevice = calculationSevice;
            this.randomOperationService = randomOperationService;
        }
        public IActionResult Index()
        {
            return this.View();
        }
        public IActionResult Quiz()
        {
            var a = randomNumberService.GenerateNumber();
            var b = randomNumberService.GenerateNumber();
            var oper = randomOperationService.RandomOperation();
            var model = new QuizViewModel()
            {
                Number1 = a,
                Number2 = b,
                Operation = randomOperationService.CheckOperation(oper),
                Result = calculationSevice.Calculation(a, b, oper)
            };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Quiz(QuizAction action, QuizViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var answer = model.Answer;
            var result = model.Result;
            if (answer == result)
            {
                ResultInfo ri = new ResultInfo()
                {
                    Number1 = model.Number1,
                    Number2 = model.Number2,
                    Operation = model.Operation,
                    Answer = answer,
                    Equal = true
                };
                model.ListOfResults.Add(ri);
            }
            else
            {
                ResultInfo ri = new ResultInfo()
                {
                    Number1 = model.Number1,
                    Number2 = model.Number2,
                    Operation = model.Operation,
                    Answer = answer,
                    Equal = false
                };
                model.ListOfResults.Add(ri);
            }

            model.Number1 = randomNumberService.GenerateNumber();
            this.ModelState.Remove("Number1");
            model.Number2 = randomNumberService.GenerateNumber();
            this.ModelState.Remove("Number2");
            var operation = randomOperationService.RandomOperation();
            model.Operation = randomOperationService.CheckOperation(operation);
            this.ModelState.Remove("Operation");
            model.Result = calculationSevice.Calculation(model.Number1, model.Number2, operation);
            this.ModelState.Remove("Result");


            switch (action)
            {
                case QuizAction.Submit:
                    return this.View(model);
                case QuizAction.Finish:
                    return this.View("QuizResult", model);
            }
            return this.View(model);
        }
    }
}
