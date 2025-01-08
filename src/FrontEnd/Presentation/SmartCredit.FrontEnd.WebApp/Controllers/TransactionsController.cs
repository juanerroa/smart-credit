using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Razor.Templating.Core;
using SmartCredit.FrontEnd.WebApp.Contracts;
using SmartCredit.FrontEnd.WebApp.Filters;
using SmartCredit.FrontEnd.WebApp.Helpers;
using SmartCredit.FrontEnd.WebApp.Validators;
using SmartCredit.FrontEnd.WebApp.ViewModels;
using System.Reflection;
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

        public async Task<IActionResult> GetMovementsExcel(int year, int month)
        {
            var cardId = GetCardIdSelected();
            var card = await _uow.CreditCardRepository.GetById(new Guid(cardId));
            var model = new CreditCardStatementViewModel();
            model.Transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), year, month, null);
            model.CardNumber = card.CardNumber;
            model.Year = year;
            model.Month = month;

            // Configurar EPPlus para trabajar con licencias no comerciales
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Crear hoja de Excel
                var worksheet = package.Workbook.Worksheets.Add("Transacciones");

                // Encabezados principales
                worksheet.Cells["A1"].Value = "Transacciones de tarjeta";
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["A2"].Value = $"Año: {model.Year}, Mes: {model.Month}";
                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Agregar color de fondo al título
                worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6393F"));
                worksheet.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                // Información del titular
                worksheet.Cells["A3"].Value = "Titular:";
                worksheet.Cells["A3"].Style.Font.Bold = true;
                worksheet.Cells["B3"].Value = model.HolderName;
                worksheet.Cells["A4"].Value = "Número de tarjeta:";
                worksheet.Cells["A4"].Style.Font.Bold = true;
                worksheet.Cells["B4"].Value = model.CardNumber;

                // Encabezados de transacciones
                worksheet.Cells["A6"].Value = "Fecha";
                worksheet.Cells["B6"].Value = "Descripción";
                worksheet.Cells["C6"].Value = "Pago";
                worksheet.Cells["D6"].Value = "Compra";
                worksheet.Cells["A6:D6"].Style.Font.Bold = true;

                // Establecer color de fondo y color de fuente para el encabezado de transacciones
                var headerRange = worksheet.Cells["A6:D6"];
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6393F"));
                headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);

                // Aplicar bordes al encabezado
                headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                // Cuerpo de las transacciones
                int row = 7; // Comenzar la tabla inmediatamente después del encabezado
                foreach (var transaction in model.Transactions)
                {
                    worksheet.Cells[row, 1].Value = transaction.Date.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt");
                    worksheet.Cells[row, 2].Value = transaction.Description;
                    worksheet.Cells[row, 3].Value = transaction.Type == 2 ? transaction.Amount.ToString("C") : "-";
                    worksheet.Cells[row, 4].Value = transaction.Type == 1 ? transaction.Amount.ToString("C") : "-";

                    // Aplicar bordes al cuerpo
                    var bodyRange = worksheet.Cells[row, 1, row, 4];
                    bodyRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    bodyRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    bodyRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    bodyRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    row++;
                }

                // Totales
                worksheet.Cells[row, 2].Value = "Total:";
                worksheet.Cells[row, 3].Value = model.Transactions.Where(x => x.Type == 2).Sum(x => x.Amount).ToString("C");
                worksheet.Cells[row, 4].Value = model.Transactions.Where(x => x.Type == 1).Sum(x => x.Amount).ToString("C");
                worksheet.Cells[row, 2, row, 4].Style.Font.Bold = true;

                // Aplicar bordes al total
                var totalRange = worksheet.Cells[row, 2, row, 4];
                totalRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                totalRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                totalRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                totalRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                // Ajustar columnas con un ancho adicional
                worksheet.Column(1).Width = 40; // Fecha
                worksheet.Column(2).Width = 40; // Descripción
                worksheet.Column(3).Width = 15; // Pago
                worksheet.Column(4).Width = 40; // Compra

                // Retornar el archivo Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                var fileName = $"Transacciones_{model.Year}_{model.Month}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }


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
