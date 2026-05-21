using System.Windows.Controls;

namespace tulo.eInvoiceCreatorZUGFeRD.Views.Invoices.Controls
{
    /// <summary>
    /// Interaction logic for NewInvocieDesign.xaml
    /// </summary>
    public partial class NewInvocieDesign : UserControl
    {
        public NewInvocieDesign()
        {
            InitializeComponent();
        }
        private void LoadSampleData()
        {
            var items = new List<InvoiceItem>
        {
            new InvoiceItem
            {
                Position    = 1,
                Description = "Druckerpapier A4, 80g/m², 500 Blatt",
                Quantity    = 10,
                Unit        = "Pkg.",
                UnitPrice   = "8,90 €",
                VatRate     = "19 %",
                NetTotal    = "89,00 €"
            },
            new InvoiceItem
            {
                Position    = 2,
                Description = "Kugelschreiber blau, 10er-Pack",
                Quantity    = 5,
                Unit        = "Pkg.",
                UnitPrice   = "4,50 €",
                VatRate     = "19 %",
                NetTotal    = "22,50 €"
            },
            new InvoiceItem
            {
                Position    = 3,
                Description = "Aktenordner A4, breit, schwarz",
                Quantity    = 20,
                Unit        = "Stk.",
                UnitPrice   = "2,80 €",
                VatRate     = "19 %",
                NetTotal    = "56,00 €"
            },
            new InvoiceItem
            {
                Position    = 4,
                Description = "Beratungsleistung Software-Integration",
                Quantity    = 3,
                Unit        = "Std.",
                UnitPrice   = "95,00 €",
                VatRate     = "19 %",
                NetTotal    = "285,00 €"
            }
        };

            InvoiceItemsControl.ItemsSource = items;
        }
    }
}
