using System.Xml.Linq;
using Crosscutting.CustomException;

namespace Crosscutting.Invoice
{
    public class InvoiceXmlParserResolver
    {
        private readonly IEnumerable<IInvoiceXmlParser> _parsers;

        public InvoiceXmlParserResolver(IEnumerable<IInvoiceXmlParser> parsers)
        {
            _parsers = parsers;
        }

        public IInvoiceXmlParser Resolve(XDocument document)
        {
            IInvoiceXmlParser? parser = _parsers.FirstOrDefault(p => p.CanParse(document));
            if (parser is null)
                throw new CustomBusinessException("Ops... O XML não é um documento fiscal suportado para importação.");

            return parser;
        }
    }
}
