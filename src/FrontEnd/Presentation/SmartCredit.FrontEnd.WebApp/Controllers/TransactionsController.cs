using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razor.Templating.Core;
using SmartCredit.FrontEnd.WebApp.Contracts;
using SmartCredit.FrontEnd.WebApp.Filters;
using SmartCredit.FrontEnd.WebApp.Helpers;
using SmartCredit.FrontEnd.WebApp.Validators;
using SmartCredit.FrontEnd.WebApp.ViewModels;
using System.Transactions;

namespace SmartCredit.FrontEnd.WebApp.Controllers
{
    [ServiceFilter(typeof(CustomAuthorize))]
    public class TransactionsController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private PdfService _pdfService;

        public TransactionsController(IUnitOfWork uow, DinkToPdf.Contracts.IConverter converter)
        {
            _uow = uow;
            _pdfService = new PdfService(converter);
        }

        public async Task<IActionResult> Index()
        {
            var card = await _uow.CreditCardRepository.GetById(new Guid(GetCardIdSelected()));
            return View(card);
        }

        public async Task<IActionResult> NewPayment(TransactionsViewModel? model)
        {
            var transactionModel = await GetTransactionModel(2, model);
            return View("~/Views/Transactions/_Partials/_Form.cshtml", transactionModel);
        }

        public async Task<IActionResult> NewPurchase(TransactionsViewModel? model)
        {
            var transactionModel = await GetTransactionModel(1, model);
            return View("~/Views/Transactions/_Partials/_Form.cshtml", transactionModel);
        }

        public async Task<IActionResult> GetMovementsPDF(int year, int month)
        {
            var cardId = GetCardIdSelected();
            var card = await _uow.CreditCardRepository.GetById(new Guid(cardId));
            var statement = new CreditCardStatementViewModel();
            statement.Transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), year, month, null);
            statement.CardNumber = card.CardNumber;
            statement.Year = year;
            statement.Month = month;
            string htmlString = await RazorTemplateEngine.RenderAsync("~/Views/Transactions/_Partials/_MovementsReport.cshtml", statement);

            var pdfBytes = _pdfService.GeneratePdf(htmlString);
            return File(pdfBytes, "application/pdf", $"estado-tarjeta-{year.ToString()}-{month.ToString()}.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> Validate(TransactionsViewModel model)
        {
            var validator = new TransactionsRequestValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                // Agregar errores manualmente al ModelState para que se muestren en la vista
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                TempData["ValidationErrors"] = JsonConvert.SerializeObject(result.Errors);

                if (model.Type == 2)
                    return RedirectToAction(nameof(NewPayment));
                else
                    return RedirectToAction(nameof(NewPurchase));
            }

            // Convertir la fecha local ingresada por el usuario a UTC
            model.Date = model.Date.ToUniversalTime();
            var cardId = GetCardIdSelected();
            
            if(model.Type == 1)
                await _uow.TransactionsRepository.AddPurchase(new Guid(cardId), model.Description, model.Amount, model.Date);
            else
                await _uow.TransactionsRepository.AddPayment(new Guid(cardId), model.Amount, model.Date);

            return RedirectToAction(nameof(Index), model);
        }

        [HttpPost]
        public async Task<ActionResult> GetHtmlMovementsTable([FromBody] TransactionsViewModel request)
        {
            var cardId = GetCardIdSelected();
            var transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), request.Year, request.Month, null);
            return PartialView("~/Views/Transactions/_Partials/_MovementsTable.cshtml", transactions);
        }

        private async Task<TransactionsViewModel> GetTransactionModel(int type, TransactionsViewModel? model)
        {
            var card = await _uow.CreditCardRepository.GetById(new Guid(GetCardIdSelected()));

            if(model == null || model.CreditCardId == Guid.Empty)
                model = new TransactionsViewModel
                {
                    CreditCardId = card.Id,
                    Type = type,
                    Date = DateTime.Now
                };

            if (TempData["ValidationErrors"] != null)
            {
                var errors = JsonConvert.DeserializeObject<IEnumerable<ValidationFailure>>(TempData["ValidationErrors"].ToString());
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return model;
        }

    }
}
