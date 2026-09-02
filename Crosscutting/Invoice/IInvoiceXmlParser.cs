using System.Xml.Linq;
using Crosscutting.DTO.NfeImport;

namespace Crosscutting.Invoice
{
    public interface IInvoiceXmlParser
    {
        string DocumentType { get; }
        bool CanParse(XDocument document);
        ParsedInvoiceDTO Parse(XDocument document);
    }
}
