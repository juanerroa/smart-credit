using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartCredit.FrontEnd.WebApp.Contracts;
using SmartCredit.FrontEnd.WebApp.Enums;
using SmartCredit.FrontEnd.WebApp.Helpers;
using SmartCredit.FrontEnd.WebApp.Validators;
using SmartCredit.FrontEnd.WebApp.ViewModels;
using Razor.Templating.Core;

namespace SmartCredit.FrontEnd.WebApp.Controllers
{
    public class CreditCardsController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private PdfService _pdfService;
        public CreditCardsController(IUnitOfWork uow, DinkToPdf.Contracts.IConverter converter)
        {
            _uow = uow;
            _pdfService = new PdfService(converter);
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _uow.CreditCardRepository.GetAll();
            return View(cards);
        }

        public IActionResult Statement()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new CreditCardViewModel
            {
                CreditCardTypeOptions = Enum.GetValues(typeof(CreditCardType))
                        .Cast<CreditCardType>()
                        .Select(e => new KeyValuePair<int, string>((int)e, e.GetDisplayName()))
                        .ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> GetStamentPeriodPDF(int year, int month)
        {
            var cardId = GetCardIdSelected();
            var statement = await _uow.CreditCardRepository.GetCreditCardStatement(new Guid(cardId), year, month);
            statement.Transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), year, month, null);
            statement.Year = year;
            statement.Month = month;
            string htmlString = await RazorTemplateEngine.RenderAsync("~/Views/CreditCards/_Partials/_StatementReport.cshtml", statement);

            var pdfBytes = _pdfService.GeneratePdf(htmlString);
            return File(pdfBytes, "application/pdf", $"estado-tarjeta-{year.ToString()}-{month.ToString()}.pdf");
        }

        [HttpPost]
        public async Task<ActionResult> GetHTMLStamentPeriod([FromBody] CreditCardStatementViewModel request)
        {
            var cardId = GetCardIdSelected();
            var statement = await _uow.CreditCardRepository.GetCreditCardStatement(new Guid(cardId), request.Year, request.Month);
            statement.Transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), request.Year, request.Month, null);

            return PartialView("~/Views/CreditCards/_Partials/_StatementByPeriod.cshtml", statement);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreditCardViewModel model)
        {
            var validator = new CreditCardRequestValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                // Agregar errores manualmente al ModelState para que se muestren en la vista
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                model.CreditCardTypeOptions = Enum.GetValues(typeof(CreditCardType))
                        .Cast<CreditCardType>()
                        .Select(e => new KeyValuePair<int, string>((int)e, e.GetDisplayName()))
                        .ToList();

                return View(model);
            }
            await _uow.CreditCardRepository.AddUserAndCreditCardAsync(model);
            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}