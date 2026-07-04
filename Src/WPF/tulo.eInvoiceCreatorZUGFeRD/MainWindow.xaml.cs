using System.Windows;

namespace Tulo.eInvoiceCreatorZUGFeRD;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(object dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;

        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
    }
}