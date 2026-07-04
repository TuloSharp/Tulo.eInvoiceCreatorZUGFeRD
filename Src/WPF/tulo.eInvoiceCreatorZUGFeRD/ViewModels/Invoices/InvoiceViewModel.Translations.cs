namespace Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;

public partial class InvoiceViewModel
{
    #region Tooltips
    public string ToolTipCompanyBuyerParty { get; private set; } = string.Empty;
    public string ToolTipFiscalIdBuyerParty { get; private set; } = string.Empty;
    public string ToolTipVatIdBuyerParty { get; private set; } = string.Empty;
    public string ToolTipErpCustomerNumberBuyerParty { get; private set; } = string.Empty;
    public string ToolTipLeitwegIdBuyerParty { get; private set; } = string.Empty;
    public string ToolTipPersonBuyerParty { get; private set; } = string.Empty;
    public string ToolTipStreetBuyerParty { get; private set; } = string.Empty;
    public string ToolTipHouseNumberBuyerParty { get; private set; } = string.Empty;
    public string ToolTipPostalCodeBuyerParty { get; private set; } = string.Empty;
    public string ToolTipCityBuyerParty { get; private set; } = string.Empty;
    public string ToolTipCountryCodeBuyerParty { get; private set; } = string.Empty;
    public string ToolTipPhoneBuyerParty { get; private set; } = string.Empty;
    public string ToolTipEmailAddressBuyerParty { get; private set; } = string.Empty;

    public string ToolTipPaymentDueDateText { get; private set; } = string.Empty;
    public string ToolTipPaymentReference { get; private set; } = string.Empty;
    public string ToolTipPaymentTerms { get; private set; } = string.Empty;

    //header
    public string ToolTipInvoiceNumber { get; private set; } = string.Empty;
    public string ToolTipCurrency { get; private set; } = string.Empty;
    public string ToolTipDocumentName { get; private set; } = string.Empty;
    public string ToolTipDocumentTypeCode { get; private set; } = string.Empty;

    public string ToolTipCreatePreviewElectronicInvoice { get; private set; } = string.Empty;
    public string ToolTipClearAllInvoiceView { get; private set; } = string.Empty;
    public string ToolTipCreateElectronicInvoice { get; private set; } = string.Empty;
    public string ToolTipSaveCustomerData { get; private set; } = string.Empty;
    public string ToolTipLoadCustomerData { get; private set; } = string.Empty;


    public string ToolTipDiscountPercent { get; private set; } = string.Empty;
    public string ToolTipDiscountDays { get; private set; } = string.Empty;
    public string ToolTipDiscountBasisDate { get; private set; } = string.Empty;
    public string ToolTipDiscountPreviewText { get; private set; } = string.Empty;
    public string ToolTipPaymentDueDateRangeText { get; private set; } = string.Empty;
    public string ToolTipNoDiscountPreviewText { get; private set; } = string.Empty;

    //Search
    public string ToolTipSearchText { get; private set; } = string.Empty;
    public string ToolTipCreateInvoicePosition { get; private set; } = string.Empty;

    private void FillAllInvoiceToolTips()
    {
        // Buyer Party tooltips
        ToolTipCompanyBuyerParty = _translatorUiProvider.Translate("ToolTipCompanyBuyerParty");
        ToolTipFiscalIdBuyerParty = _translatorUiProvider.Translate("ToolTipFiscalIdBuyerParty");
        ToolTipVatIdBuyerParty = _translatorUiProvider.Translate("ToolTipVatIdBuyerParty");
        ToolTipErpCustomerNumberBuyerParty = _translatorUiProvider.Translate("ToolTipErpCustomerNumberBuyerParty");
        ToolTipLeitwegIdBuyerParty = _translatorUiProvider.Translate("ToolTipLeitwegIdBuyerParty");
        ToolTipPersonBuyerParty = _translatorUiProvider.Translate("ToolTipPersonBuyerParty");
        ToolTipStreetBuyerParty = _translatorUiProvider.Translate("ToolTipStreetBuyerParty");
        ToolTipHouseNumberBuyerParty = _translatorUiProvider.Translate("ToolTipHouseNumberBuyerParty");
        ToolTipPostalCodeBuyerParty = _translatorUiProvider.Translate("ToolTipPostalCodeBuyerParty");
        ToolTipCityBuyerParty = _translatorUiProvider.Translate("ToolTipCityBuyerParty");
        ToolTipCountryCodeBuyerParty = _translatorUiProvider.Translate("ToolTipCountryCodeBuyerParty");
        ToolTipPhoneBuyerParty = _translatorUiProvider.Translate("ToolTipPhoneBuyerParty");
        ToolTipEmailAddressBuyerParty = _translatorUiProvider.Translate("ToolTipEmailAddressBuyerParty");

        // Payment tooltips
        ToolTipPaymentDueDateText = _translatorUiProvider.Translate("ToolTipPaymentDueDateText");
        ToolTipPaymentReference = _translatorUiProvider.Translate("ToolTipPaymentReference");
        ToolTipPaymentTerms = _translatorUiProvider.Translate("ToolTipPaymentTerms");

        // Header tooltips
        ToolTipInvoiceNumber = _translatorUiProvider.Translate("ToolTipInvoiceNumber");
        ToolTipCurrency = _translatorUiProvider.Translate("ToolTipCurrency");
        ToolTipDocumentName = _translatorUiProvider.Translate("ToolTipDocumentName");
        ToolTipDocumentTypeCode = _translatorUiProvider.Translate("ToolTipDocumentTypeCode");

        ToolTipCreatePreviewElectronicInvoice = _translatorUiProvider.Translate("ToolTipCreatePreviewElectronicInvoice");
        ToolTipClearAllInvoiceView = _translatorUiProvider.Translate("ToolTipClearAllInvoiceView");
        ToolTipCreateElectronicInvoice = _translatorUiProvider.Translate("ToolTipCreateElectronicInvoice");
        ToolTipSaveCustomerData = _translatorUiProvider.Translate("ToolTipSaveCustomerData");
        ToolTipLoadCustomerData = _translatorUiProvider.Translate("ToolTipLoadCustomerData");

        //payment terms
        ToolTipDiscountPercent = _translatorUiProvider.Translate("ToolTipDiscountPercent");
        ToolTipDiscountDays = _translatorUiProvider.Translate("ToolTipDiscountDays");
        ToolTipDiscountBasisDate = _translatorUiProvider.Translate("ToolTipDiscountBasisDate");
        ToolTipDiscountPreviewText = _translatorUiProvider.Translate("ToolTipDiscountPreviewText");
        ToolTipPaymentDueDateRangeText = _translatorUiProvider.Translate("ToolTipPaymentDueDateRangeText");
        ToolTipNoDiscountPreviewText = _translatorUiProvider.Translate("ToolTipNoDiscountPreviewText");

        //search
        ToolTipSearchText = _translatorUiProvider.Translate("ToolTipSearch");
        ToolTipCreateInvoicePosition = _translatorUiProvider.Translate("ToolTipCreateInvoicePosition");
    }
    #endregion

    #region Placeholders
    public string PlaceholderCompanyBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderFiscalIdBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderVatIdBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderErpCustomerNumberBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderLeitwegIdBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderPersonBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderStreetBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderHouseNumberBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderPostalCodeBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderCityBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderCountryCodeBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderPhoneBuyerParty { get; private set; } = string.Empty;
    public string PlaceholderEmailAddressBuyerParty { get; private set; } = string.Empty;

    public string PlaceholderPaymentReference { get; private set; } = string.Empty;
    public string PlaceholderPaymentTerms { get; private set; } = string.Empty;

    public string PlaceholderInvoiceNumber { get; private set; } = string.Empty;
    public string PlaceholderCurrency { get; private set; } = string.Empty;
    public string PlaceholderDocumentName { get; private set; } = string.Empty;
    public string PlaceholderDocumentTypeCode { get; private set; } = string.Empty;

    public string PlaceholderDiscountPercent { get; private set; } = string.Empty;
    public string PlaceholderDiscountDays { get; private set; } = string.Empty;
    public string PlaceholderDiscountPreviewText { get; private set; } = string.Empty;
    public string PlaceholderNoDiscountPreviewText { get; private set; } = string.Empty;

    private void FillAllInvoicePlaceholders()
    {
        // Buyer Party placeholders
        PlaceholderCompanyBuyerParty = _translatorUiProvider.Translate("PlaceholderCompanyBuyerParty");
        PlaceholderFiscalIdBuyerParty = _translatorUiProvider.Translate("PlaceholderFiscalIdBuyerParty");
        PlaceholderVatIdBuyerParty = _translatorUiProvider.Translate("PlaceholderVatIdBuyerParty");
        PlaceholderErpCustomerNumberBuyerParty = _translatorUiProvider.Translate("PlaceholderErpCustomerNumberBuyerParty");
        PlaceholderLeitwegIdBuyerParty = _translatorUiProvider.Translate("PlaceholderLeitwegIdBuyerParty");
        PlaceholderPersonBuyerParty = _translatorUiProvider.Translate("PlaceholderPersonBuyerParty");
        PlaceholderStreetBuyerParty = _translatorUiProvider.Translate("PlaceholderStreetBuyerParty");
        PlaceholderHouseNumberBuyerParty = _translatorUiProvider.Translate("PlaceholderHouseNumberBuyerParty");
        PlaceholderPostalCodeBuyerParty = _translatorUiProvider.Translate("PlaceholderPostalCodeBuyerParty");
        PlaceholderCityBuyerParty = _translatorUiProvider.Translate("PlaceholderCityBuyerParty");
        PlaceholderCountryCodeBuyerParty = _translatorUiProvider.Translate("PlaceholderCountryCodeBuyerParty");
        PlaceholderPhoneBuyerParty = _translatorUiProvider.Translate("PlaceholderPhoneBuyerParty");
        PlaceholderEmailAddressBuyerParty = _translatorUiProvider.Translate("PlaceholderEmailAddressBuyerParty");

        // Payment placeholders
        PlaceholderPaymentReference = _translatorUiProvider.Translate("PlaceholderPaymentReference");
        PlaceholderPaymentTerms = _translatorUiProvider.Translate("PlaceholderPaymentTerms");

        // Header placeholders
        PlaceholderInvoiceNumber = _translatorUiProvider.Translate("PlaceholderInvoiceNumber");
        PlaceholderCurrency = _translatorUiProvider.Translate("PlaceholderCurrency");
        PlaceholderDocumentName = _translatorUiProvider.Translate("PlaceholderDocumentName");
        PlaceholderDocumentTypeCode = _translatorUiProvider.Translate("PlaceholderDocumentTypeCode");

        //payment terms
        PlaceholderDiscountPercent = _translatorUiProvider.Translate("PlaceholderDiscountPercent");
        PlaceholderDiscountDays = _translatorUiProvider.Translate("PlaceholderDiscountDays");
        PlaceholderDiscountPreviewText = _translatorUiProvider.Translate("PlaceholderDiscountPreviewText");
        PlaceholderNoDiscountPreviewText = _translatorUiProvider.Translate("PlaceholderNoDiscountPreviewText");

    }
    #endregion

    #region Labels, Tags & Contents
    public string ContentDateInvalid { get; private set; } = string.Empty;
    public string ContentDateMustBeBetween { get; private set; } = string.Empty;

    public string LabelPaymentDueDate { get; private set; } = string.Empty;
    public string LabelPaymentMeansCode { get; private set; } = string.Empty;

    public string LabelCurrency { get; private set; } = string.Empty;
    public string LabelDocumentTypeCode { get; private set; } = string.Empty;

    public string LabelContentInvoiceView { get; private set; } = string.Empty;
    public string LabelContentPreview { get; private set; } = string.Empty;
    public string ContentSlideText { get; private set; } = string.Empty;
    public string ContentSlideConfirmedText { get; private set; } = string.Empty;
    public string LabelContenBuyerInformation { get; private set; } = string.Empty;
    public string LabelContentHeader { get; private set; } = string.Empty;
    public string LabelContentPaymentInformation { get; private set; } = string.Empty;
    public string LabelContentPositionsList { get; private set; } = string.Empty;

    public string TagDiscountBasisDate { get; private set; } = string.Empty;
    public string TagPaymentDueDateRangeText { get; private set; } = string.Empty;
    public string LabelContentPaymentTerms { get; private set; } = string.Empty;

    public string LabelApplyDiscount { get; private set; } = string.Empty;

    public string ContentButtonSave { get; private set; } = string.Empty;
    public string ContentButtonLoad { get; private set; } = string.Empty;
    public string ContentButtonClearAll { get; private set; } = string.Empty;
    public string ContentButtonCreateInvoicePosition { get; private set; } = string.Empty;

    private void FillAllInvoiceLabelsAndContents()
    {
        LabelPaymentDueDate = _translatorUiProvider.Translate("LabelPaymentDueDate");
        LabelPaymentMeansCode = _translatorUiProvider.Translate("LabelPaymentMeansCode");

        LabelCurrency = _translatorUiProvider.Translate("LabelCurrency");
        LabelDocumentTypeCode = _translatorUiProvider.Translate("LabelDocumentTypeCode");

        LabelContentInvoiceView = _translatorUiProvider.Translate("LabelContentInvoiceView");
        LabelContentPreview = _translatorUiProvider.Translate("LabelContentPreview");
        ContentSlideText = _translatorUiProvider.Translate("ContentSlideText");
        ContentSlideConfirmedText = _translatorUiProvider.Translate("ContentSlideConfirmedText");
        LabelContenBuyerInformation = _translatorUiProvider.Translate("LabelContenBuyerInformation");
        LabelContentHeader = _translatorUiProvider.Translate("LabelContentHeader");
        LabelContentPaymentInformation = _translatorUiProvider.Translate("LabelContentPaymentInformation");
        LabelContentPositionsList = _translatorUiProvider.Translate("LabelContentPositionsList");

        TagDiscountBasisDate = _translatorUiProvider.Translate("TagDiscountBasisDate");
        TagPaymentDueDateRangeText = _translatorUiProvider.Translate("TagPaymentDueDateRangeText");
        LabelContentPaymentTerms = _translatorUiProvider.Translate("LabelContentPaymentTerms");
        LabelApplyDiscount = _translatorUiProvider.Translate("LabelApplyDiscount");

        ContentDateInvalid = _translatorUiProvider.Translate("ContentDateInvalid");
        ContentDateMustBeBetween = _translatorUiProvider.Translate("ContentDateMustBeBetween");

        ContentButtonSave = _translatorUiProvider.Translate("ContentButtonSave");
        ContentButtonLoad = _translatorUiProvider.Translate("ContentButtonLoad");
        ContentButtonClearAll = _translatorUiProvider.Translate("ContentButtonClearAll");
        ContentButtonCreateInvoicePosition = _translatorUiProvider.Translate("ContentButtonCreateInvoicePosition");
    }
    #endregion
}
