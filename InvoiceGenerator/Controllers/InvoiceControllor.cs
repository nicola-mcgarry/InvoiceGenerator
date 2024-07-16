using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using InvoiceGenerator.Models;
using InvoiceGenerator.Services;
using System.IO;
using System.Linq;

namespace InvoiceGenerator.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly PdfGeneratorService _pdfGeneratorService;

        public InvoicesController(PdfGeneratorService pdfGeneratorService)
        {
            _pdfGeneratorService = pdfGeneratorService;
        }

        public IActionResult Index()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "orders.json");
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var invoiceRequestData = JsonConvert.DeserializeObject<InvoiceRequest>(jsonContent);

            var requests = invoiceRequestData?.request ?? new List<Request>();

            return View(requests);
        }

        public IActionResult Details(int id)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "orders.json");
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var invoiceRequestData = JsonConvert.DeserializeObject<InvoiceRequest>(jsonContent);

            var order = invoiceRequestData?.request.FirstOrDefault(r => r.order_id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult GeneratePdf(int id)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "orders.json");
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var invoiceRequestData = JsonConvert.DeserializeObject<InvoiceRequest>(jsonContent);

            var order = invoiceRequestData?.request.FirstOrDefault(r => r.order_id == id);
            if (order == null)
            {
                return NotFound();
            }

            var pdfBytes = _pdfGeneratorService.GenerateInvoicePdf(order);
            return File(pdfBytes, "application/pdf", $"invoice_{id}.pdf");
        }
    }
}
