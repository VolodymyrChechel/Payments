using System;
using System.IO;
using Payments.WEB.Models;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Payments.Common.NLog;

namespace Payments.WEB.Util
{
    public class PdfCreator : IDocumentCreator
    {
        public MemoryStream CreateDocument(PaymentViewModel payment)
        {
            NLog.LogTrace(this.GetType(), "Create method execution");

            MemoryStream stream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime time = DateTime.Now;

            // filename
            string pdfFileName = string.Format("Payment" + time.ToString("yyyyMMMMdd ") + ".pdf");
            Document document = new Document();
            document.SetMargins(25f, 25f, 25f, 25f);

            PdfWriter.GetInstance(document, stream).CloseStream = false;
            document.Open();

            Chunk c1 = new Chunk("++bank: cheque");
            Chunk line = new Chunk("--------------------");
            Paragraph paragraph = new Paragraph(" ");
            Chunk c2 = new Chunk("payment: " + payment.Id);
            Chunk c3 = new Chunk("sum of payment: " + payment.PaymentSum);
            Chunk c4 = new Chunk("status: " + payment.PaymentStatus);
            Chunk c5 = new Chunk("recipient: " + payment.Recipient);
            Chunk c6 = new Chunk("date: " + payment.PaymentDate);

            document.Add(c1);
            document.Add(paragraph);
            document.Add(line);
            document.Add(paragraph);
            document.Add(c2);
            document.Add(paragraph);
            document.Add(c3);
            document.Add(paragraph);
            document.Add(c4);
            document.Add(paragraph);
            document.Add(c5);
            document.Add(paragraph);
            document.Add(c6);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

    }
}