using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PearaPrintAPI.Controllers.API
{
    public class PrinterController : ApiController
    {
        [System.Web.Http.HttpGet]
        public IHttpActionResult PrintReceipt()
        {
            var printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = "RONGTA 80mm Series Printer";
            //printDocument.PrinterSettings.PrinterName = "HPB0227A4E5353(HP Laser MFP 131 133 135-138)";
            printDocument.PrintPage += PrintReceipt;
            printDocument.PrintController = new StandardPrintController();
            printDocument.Print();
            //return Content("Printing Started...");
            return Ok();
        }
        private void PrintReceipt(object sender, PrintPageEventArgs e)
        {
            var graphics = e.Graphics;
            var font = new Font("Arial", 12);
            var brush = Brushes.Black;
            var startX = 10;
            var startY = 10;
            var lineHeight = font.GetHeight();

            // DRAW THE HEADER
            var headerText = "Singel e-World Ltd";
            var addressText = "Block J, Baridhara,Dhaka-1213, Bangladesh.";
            var phoneText = "Phone: 123-456-7890";
            graphics.DrawString(headerText, font, brush, startX, startY);
            graphics.DrawString(addressText, font, brush, startX, startY + lineHeight);
            graphics.DrawString(phoneText, font, brush, startX, startY + 2 * lineHeight);

            // DRAW THE DATE AND RECEIPT DETAILS
            var dateText = "Date: 2023-06-26 14:30:00";
            var receiptNoText = "Receipt No: 00123";
            var cashierText = "Cashier: Ariful Islam Akash";
            graphics.DrawString(dateText, font, brush, startX, startY + 4 * lineHeight);
            graphics.DrawString(receiptNoText, font, brush, startX, startY + 5 * lineHeight);
            graphics.DrawString(cashierText, font, brush, startX, startY + 6 * lineHeight);

            // DRAW THE ITEM DETAILS
            var itemHeader = "Item";
            var quantityHeader = "Quantity";
            var priceHeader = "Price";
            var separator = new string('-', 30);
            var itemsStartY = startY + 8 * lineHeight;
            graphics.DrawString(itemHeader, font, brush, startX, itemsStartY);
            graphics.DrawString(quantityHeader, font, brush, startX + 150, itemsStartY);
            graphics.DrawString(priceHeader, font, brush, startX + 230, itemsStartY);
            graphics.DrawString(separator, font, brush, startX, itemsStartY + lineHeight);

            var items = new[]
            {
                new { Name = "Full Frid Rice 1", Quantity = 2, Price = 10.00m },
                new { Name = "Product 2", Quantity = 1, Price = 15.50m },
                new { Name = "Product 3", Quantity = 3, Price = 5.25m }
            };

            var currentY = itemsStartY + 2 * lineHeight;
            foreach (var item in items)
            {
                string itemName = item.Name;
                if (itemName.Length > 20)
                {
                    string firstLine = itemName.Substring(0, 20);
                    string remainingText = itemName.Substring(20);
                    graphics.DrawString(firstLine, font, brush, startX, currentY);
                    graphics.DrawString(item.Quantity.ToString(), font, brush, startX + 150, currentY);
                    graphics.DrawString(item.Price.ToString("C2"), font, brush, startX + 230, currentY);
                    currentY += lineHeight;
                    graphics.DrawString(remainingText, font, brush, startX, currentY);
                }
                else
                {
                    graphics.DrawString(itemName, font, brush, startX, currentY);
                    graphics.DrawString(item.Quantity.ToString(), font, brush, startX + 150, currentY);
                    graphics.DrawString(item.Price.ToString("C2"), font, brush, startX + 230, currentY);
                }
                currentY += lineHeight;
            }
            graphics.DrawString(separator, font, brush, startX, currentY);
            //DRAW THE SUBTOTAL, TAX, AND TOTAL DUE
            var subtotalText = "Subtotal:";
            var taxText = "Tax (7%):";
            var totalDueText = "Total Due:";
            var subtotalValue = 51.75m;
            var taxValue = 3.62m;
            var totalDueValue = 55.37m;
            graphics.DrawString(subtotalText, font, brush, startX, currentY + lineHeight);
            graphics.DrawString(subtotalValue.ToString("C2"), font, brush, startX + 230, currentY + lineHeight);
            graphics.DrawString(taxText, font, brush, startX, currentY + 2 * lineHeight);
            graphics.DrawString(taxValue.ToString("C2"), font, brush, startX + 230, currentY + 2 * lineHeight);
            graphics.DrawString(separator, font, brush, startX, currentY + 3 * lineHeight);
            graphics.DrawString(totalDueText, font, brush, startX, currentY + 4 * lineHeight);
            graphics.DrawString(totalDueValue.ToString("C2"), font, brush, startX + 230, currentY + 4 * lineHeight);
            // DRAW THE FOOTER
            var thankYouText = "THANK YOU FOR SHOPPING WITH US!";
            graphics.DrawString(thankYouText, font, brush, startX, currentY + 6 * lineHeight);
            e.HasMorePages = false;
        }
        [System.Web.Http.HttpGet]
        public IHttpActionResult MultiplePrinter()
        {
            var printer1 = "HPB0227A4E5353(HP Laser MFP 131 133 135-138)";
            var printer2 = "RONGTA 80mm Series Printer";

            var printDocument1 = new PrintDocument();
            printDocument1.PrinterSettings.PrinterName = printer1;
            printDocument1.PrintPage += (sender, e) => PrintReceiptPrinter1(sender, e, printer1);
            printDocument1.PrintController = new StandardPrintController();
            printDocument1.Print();
            //=========================================================//
            var printDocument2 = new PrintDocument();
            printDocument2.PrinterSettings.PrinterName = printer2;
            printDocument2.PrintPage += (sender, e) => PrintReceiptPrinter2(sender, e, printer2);
            printDocument1.PrintController = new StandardPrintController();
            printDocument2.Print();
            return Ok();
        }
        private void PrintReceiptPrinter1(object sender, PrintPageEventArgs e, string printerName)
        {
            var graphics = e.Graphics;
            var font = new Font("Arial", 12);
            var brush = Brushes.Black;
            var startX = 10;
            var startY = 10;
            var lineHeight = font.GetHeight();

            // DRAW THE HEADER
            var headerText = "Formate 1";
            var addressText = "Block J, Baridhara,Dhaka-1213, Bangladesh.";
            var phoneText = "Phone: 123-456-7890";
            graphics.DrawString(headerText, font, brush, startX, startY);
            graphics.DrawString(addressText, font, brush, startX, startY + lineHeight);
            graphics.DrawString(phoneText, font, brush, startX, startY + 2 * lineHeight);

            // DRAW THE DATE AND RECEIPT DETAILS
            var dateText = "Date: 2023-06-26 14:30:00";
            var receiptNoText = "Receipt No: 00123";
            var cashierText = "Cashier: Ariful Islam Akash";
            graphics.DrawString(dateText, font, brush, startX, startY + 4 * lineHeight);
            graphics.DrawString(receiptNoText, font, brush, startX, startY + 5 * lineHeight);
            graphics.DrawString(cashierText, font, brush, startX, startY + 6 * lineHeight);

            // DRAW THE ITEM DETAILS
            var itemHeader = "Item";
            var quantityHeader = "Quantity";
            var priceHeader = "Price";
            var separator = new string('-', 30);
            var itemsStartY = startY + 8 * lineHeight;
            graphics.DrawString(itemHeader, font, brush, startX, itemsStartY);
            graphics.DrawString(quantityHeader, font, brush, startX + 150, itemsStartY);
            graphics.DrawString(priceHeader, font, brush, startX + 230, itemsStartY);
            graphics.DrawString(separator, font, brush, startX, itemsStartY + lineHeight);

            var items = new[]
            {
                new { Name = "Product 1", Quantity = 2, Price = 10.00m },
                new { Name = "Product 2", Quantity = 1, Price = 15.50m },
                new { Name = "Product 3", Quantity = 3, Price = 5.25m }
            };

            var currentY = itemsStartY + 2 * lineHeight;
            foreach (var item in items)
            {
                graphics.DrawString(item.Name, font, brush, startX, currentY);
                graphics.DrawString(item.Quantity.ToString(), font, brush, startX + 150, currentY);
                graphics.DrawString(item.Price.ToString("C2"), font, brush, startX + 230, currentY);
                currentY += lineHeight;
            }

            graphics.DrawString(separator, font, brush, startX, currentY);

            // DRAW THE SUBTOTAL, TAX, AND TOTAL DUE
            var subtotalText = "Subtotal:";
            var taxText = "Tax (7%):";
            var totalDueText = "Total Due:";
            var subtotalValue = 51.75m;
            var taxValue = 3.62m;
            var totalDueValue = 55.37m;
            graphics.DrawString(subtotalText, font, brush, startX, currentY + lineHeight);
            graphics.DrawString(subtotalValue.ToString("C2"), font, brush, startX + 230, currentY + lineHeight);
            graphics.DrawString(taxText, font, brush, startX, currentY + 2 * lineHeight);
            graphics.DrawString(taxValue.ToString("C2"), font, brush, startX + 230, currentY + 2 * lineHeight);
            graphics.DrawString(separator, font, brush, startX, currentY + 3 * lineHeight);
            graphics.DrawString(totalDueText, font, brush, startX, currentY + 4 * lineHeight);
            graphics.DrawString(totalDueValue.ToString("C2"), font, brush, startX + 230, currentY + 4 * lineHeight);

            // DRAW THE FOOTER
            var thankYouText = "THANK YOU FOR SHOPPING WITH US!";
            graphics.DrawString(thankYouText, font, brush, startX, currentY + 6 * lineHeight);
            e.HasMorePages = false;
        }
        private void PrintReceiptPrinter2(object sender, PrintPageEventArgs e, string printerName)
        {
            var graphics = e.Graphics;
            var font = new Font("Arial", 12);
            var brush = Brushes.Black;
            var startX = 10;
            var startY = 10;
            var lineHeight = font.GetHeight();

            // DRAW THE HEADER
            var headerText = "Formate 2";
            var addressText = "Block J, Baridhara,Dhaka-1213, Bangladesh.";
            var phoneText = "Phone: 123-456-7890";
            graphics.DrawString(headerText, font, brush, startX, startY);
            graphics.DrawString(addressText, font, brush, startX, startY + lineHeight);
            graphics.DrawString(phoneText, font, brush, startX, startY + 2 * lineHeight);

            // DRAW THE DATE AND RECEIPT DETAILS
            var dateText = "Date: 2023-06-26 14:30:00";
            var receiptNoText = "Receipt No: 00123";
            var cashierText = "Cashier: Ariful Islam Akash";
            graphics.DrawString(dateText, font, brush, startX, startY + 4 * lineHeight);
            graphics.DrawString(receiptNoText, font, brush, startX, startY + 5 * lineHeight);
            graphics.DrawString(cashierText, font, brush, startX, startY + 6 * lineHeight);

            // DRAW THE ITEM DETAILS
            var itemHeader = "Item";
            var quantityHeader = "Quantity";
            var priceHeader = "Price";
            var separator = new string('-', 30);
            var itemsStartY = startY + 8 * lineHeight;
            graphics.DrawString(itemHeader, font, brush, startX, itemsStartY);
            graphics.DrawString(quantityHeader, font, brush, startX + 150, itemsStartY);
            graphics.DrawString(priceHeader, font, brush, startX + 230, itemsStartY);
            graphics.DrawString(separator, font, brush, startX, itemsStartY + lineHeight);

            var items = new[]
            {
                new { Name = "Full Frid Rice 1", Quantity = 2, Price = 10.00m },
                new { Name = "Product 2", Quantity = 1, Price = 15.50m },
                new { Name = "Product 3", Quantity = 3, Price = 5.25m }
            };

            var currentY = itemsStartY + 2 * lineHeight;
            foreach (var item in items)
            {
                graphics.DrawString(item.Name, font, brush, startX, currentY);
                graphics.DrawString(item.Quantity.ToString(), font, brush, startX + 150, currentY);
                graphics.DrawString(item.Price.ToString("C2"), font, brush, startX + 230, currentY);
                currentY += lineHeight;
            }

            graphics.DrawString(separator, font, brush, startX, currentY);

            // DRAW THE SUBTOTAL, TAX, AND TOTAL DUE
            var subtotalText = "Subtotal:";
            var taxText = "Tax (7%):";
            var totalDueText = "Total Due:";
            var subtotalValue = 51.75m;
            var taxValue = 3.62m;
            var totalDueValue = 55.37m;
            graphics.DrawString(subtotalText, font, brush, startX, currentY + lineHeight);
            graphics.DrawString(subtotalValue.ToString("C2"), font, brush, startX + 230, currentY + lineHeight);
            graphics.DrawString(taxText, font, brush, startX, currentY + 2 * lineHeight);
            graphics.DrawString(taxValue.ToString("C2"), font, brush, startX + 230, currentY + 2 * lineHeight);
            graphics.DrawString(separator, font, brush, startX, currentY + 3 * lineHeight);
            graphics.DrawString(totalDueText, font, brush, startX, currentY + 4 * lineHeight);
            graphics.DrawString(totalDueValue.ToString("C2"), font, brush, startX + 230, currentY + 4 * lineHeight);

            // DRAW THE FOOTER
            var thankYouText = "THANK YOU FOR SHOPPING WITH US!";
            graphics.DrawString(thankYouText, font, brush, startX, currentY + 6 * lineHeight);
            e.HasMorePages = false;
        }

    }
}
