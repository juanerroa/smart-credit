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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Reflection;

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

        public async Task<IActionResult> GetStamentPeriodExcel(int year, int month)
        {
            var cardId = GetCardIdSelected();
            var model = await _uow.CreditCardRepository.GetCreditCardStatement(new Guid(cardId), year, month);
            model.Transactions = await _uow.TransactionsRepository.GetByPeriod(new Guid(cardId), year, month, null);
            model.Year = year;
            model.Month = month;

            // Configurar EPPlus para trabajar con licencias no comerciales
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Crear hoja de Excel
                var worksheet = package.Workbook.Worksheets.Add("Estado de Cuenta");

                // Encabezados principales
                worksheet.Cells["A1"].Value = "Estado de cuenta de tarjeta";
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
                worksheet.Cells["A4"].Value = "Titular:";
                worksheet.Cells["B4"].Value = model.HolderName;
                worksheet.Cells["A5"].Value = "Número de tarjeta:";
                worksheet.Cells["B5"].Value = model.CardNumber;
                worksheet.Cells["A6"].Value = "Saldo actual:";
                worksheet.Cells["B6"].Value = model.Balance.ToString("C");
                worksheet.Cells["A7"].Value = "Límite de crédito:";
                worksheet.Cells["B7"].Value = model.CreditLimit.ToString("C");
                worksheet.Cells["A8"].Value = "Saldo disponible:";
                worksheet.Cells["B8"].Value = model.AvailableBalance.ToString("C");

                // Resumen financiero
                worksheet.Cells["C4"].Value = "Total Compras (Periodo):";
                worksheet.Cells["D4"].Value = model.TotalPurchaseSelectedPeriod.ToString("C");
                worksheet.Cells["C5"].Value = "Total Pagos (Periodo):";
                worksheet.Cells["D5"].Value = model.TotalPaymentsSelectedPeriod.ToString("C");
                worksheet.Cells["C6"].Value = "Total Compras (Periodo anterior):";
                worksheet.Cells["D6"].Value = model.TotalPurchaseLastPeriod.ToString("C");
                worksheet.Cells["C7"].Value = "Total Pagos (Periodo anterior):";
                worksheet.Cells["D7"].Value = model.TotalPaymentsLastPeriod.ToString("C");

                // Encabezados de transacciones
                worksheet.Cells["A10"].Value = "Fecha";
                worksheet.Cells["B10"].Value = "Descripción";
                worksheet.Cells["C10"].Value = "Pago";
                worksheet.Cells["D10"].Value = "Compra";
                worksheet.Cells["A10:D10"].Style.Font.Bold = true;

                // Aplicar bordes al encabezado
                var headerRange = worksheet.Cells["A10:D10"];
                headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                // Aplicar bordes al encabezado
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6393F"));
                headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);

                // Cuerpo de las transacciones
                int row = 11;
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
                worksheet.Column(1).Width = 35; // Fecha
                worksheet.Column(2).Width = 35; // Descripción
                worksheet.Column(3).Width = 35; // Pago
                worksheet.Column(4).Width = 35; // Compra

                // Retornar el archivo Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                var fileName = $"EstadoDeCuenta_{model.Year}_{model.Month}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }


            using (var package = new ExcelPackage())
            {
                // Crear hoja de Excel
                var worksheet = package.Workbook.Worksheets.Add("Estado de Cuenta");

                // Encabezados principales
                worksheet.Cells["A1"].Value = "Estado de cuenta de tarjeta";
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["A2"].Value = $"Año: {model.Year}, Mes: {model.Month}";
                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                var fileName = $"EstadoDeCuenta_{model.Year}_{model.Month}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }


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