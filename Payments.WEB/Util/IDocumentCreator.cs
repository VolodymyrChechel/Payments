using System.IO;
using Payments.WEB.Models;

namespace Payments.WEB.Util
{
    public interface IDocumentCreator
    {
        MemoryStream CreateDocument(PaymentViewModel payment);
    }
}