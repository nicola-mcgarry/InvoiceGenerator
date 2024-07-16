using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using InvoiceGenerator.Models;
using iText.Kernel.Pdf.Canvas.Draw;

namespace InvoiceGenerator.Services
{
    public class PdfGeneratorService
    {
        public byte[] GenerateInvoicePdf(Request request)
        {
            using (var memoryStream = new MemoryStream())
            {
                var pdfWriter = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                var document = new iText.Layout.Document(pdfDocument);

                AddHeader(document);
                AddClientDetails(document, request);
                AddInvoiceDetails(document, request);
                AddInvoiceTable(document, request.items);
                AddSummarySection(document, request);
                AddFooter(document);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private static void AddHeader(Document document)
        {
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "logo.png");
            if (File.Exists(logoPath))
            {
                var logo = ImageDataFactory.Create(logoPath);
                var img = new Image(logo).ScaleAbsolute(60, 60);
                document.Add(img);
            }
            document.Add(new Paragraph("Invoice").SetFontSize(20).SetBold().SetTextAlignment(TextAlignment.RIGHT));
            document.Add(new LineSeparator(new SolidLine()));
        }

        private static void AddClientDetails(Document document, Request request)
        {          

            var clientAddress = new Paragraph()
               .Add($"{request.sales_person}\n")
               .Add($"{request.currency_name}\n")
               .Add($"{request.invoice_address1}\n")       
               .Add($"{request.invoice_city}\n")
               .Add($"{request.invoice_state_county}\n")
               .Add($"{request.invoice_post_code}\n")
               .Add("\n");            

            document.Add(clientAddress);
        }

        private static void AddInvoiceDetails(iText.Layout.Document document, Request request)
        {
            var invoiceDate = request.order_confirmed_date.ToShortDateString();
            var dueDate = request.order_confirmed_date.AddDays(30).ToShortDateString();

            var invoiceDetails = new Paragraph()
                .Add($"Invoice number: {request.order_id}\n")
                .Add($"Invoice date: {invoiceDate}\n")
                .Add($"Due date: {dueDate}\n")
                .Add($"Customer number: {request.order_id}\n")
                .SetTextAlignment(TextAlignment.RIGHT);

            document.Add(invoiceDetails);
            document.Add(new LineSeparator(new SolidLine()));
        }

        private static void AddInvoiceTable(Document document, List<Item> items)
        {
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 2, 2, 2, 2, 2, 2 })).UseAllAvailableWidth();
            table.AddHeaderCell("Order Id");
            table.AddHeaderCell("Product Name");
            table.AddHeaderCell("Purchase Order");
            table.AddHeaderCell("Item");
            table.AddHeaderCell("month");
            table.AddHeaderCell("Year");
            table.AddHeaderCell("Gross Price");
            table.AddHeaderCell("Net Price");


            foreach (var item in items)
            {
                table.AddCell(new Cell().Add(new Paragraph(item.order_item_id.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(item.product_name)));
                table.AddCell(new Cell().Add(new Paragraph(item.purchase_order)));
                table.AddCell(new Cell().Add(new Paragraph(item.item)));
                table.AddCell(new Cell().Add(new Paragraph(item.month_name)));
                table.AddCell(new Cell().Add(new Paragraph($"item.year")));
                table.AddCell(new Cell().Add(new Paragraph(item.gross_price.ToString("C2"))));
                table.AddCell(new Cell().Add(new Paragraph(item.net_price.ToString("C2"))));
            }

            document.Add(table);
            document.Add(new LineSeparator(new SolidLine()));
        }

        private static void AddSummarySection(Document document, Request request)
        {
            decimal netPrice = 0m;
            decimal totalVat = 0m;
            decimal totalDue = 0m;

            foreach (var item in request.items)
            {
                netPrice += (decimal)item.net_price;
                decimal itemVat = (decimal)item.net_price * 0.20m;
                totalVat += itemVat;
                totalDue += (decimal)item.net_price + itemVat;
            }

            var summary = new Paragraph()
                  .Add($"Total excl. VAT: {netPrice:C2}\n")
                  .Add($"VAT 20%: {totalVat:C2}\n")
                  .Add($"Total amount due: {totalDue:C2}\n")
                  .SetTextAlignment(TextAlignment.RIGHT);

            document.Add(summary);
        }

        private static void AddFooter(Document document)
        {
            document.Add(new Paragraph("Thank you for your business!").SetTextAlignment(TextAlignment.CENTER));
            document.Add(new LineSeparator(new SolidLine()));
        }
    }
}
