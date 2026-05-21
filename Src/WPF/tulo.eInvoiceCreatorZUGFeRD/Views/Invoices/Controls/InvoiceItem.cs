namespace tulo.eInvoiceCreatorZUGFeRD.Views.Invoices.Controls
{
    public class InvoiceItem
    {
        public int Position { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string UnitPrice { get; set; } = string.Empty;
        public string VatRate { get; set; } = string.Empty;
        public string NetTotal { get; set; } = string.Empty;
    }
}
