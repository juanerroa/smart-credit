using DinkToPdf.Contracts;
using DinkToPdf;

namespace SmartCredit.FrontEnd.WebApp.Helpers
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            try
            {
                var globalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.Letter,
                    Orientation = Orientation.Portrait,

                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,

                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings },
                };

                return _converter.Convert(pdf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new byte[0];
        }
    }
}
