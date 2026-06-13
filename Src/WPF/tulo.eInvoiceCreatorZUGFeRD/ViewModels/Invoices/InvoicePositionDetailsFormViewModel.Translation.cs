namespace tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;

public partial class InvoicePositionDetailsFormViewModel
{
    #region Labels&Contents
    public string LabelSaveRequestMessage { get; set; } = string.Empty;
    public string LabelInvoicePositionGroupBox { get; set; } = string.Empty;
    public string ContentButtonSaveSaveRequestMessage { get; set; } = string.Empty;
    public string ContentButtonNoSaveRequestMessage { get; set; } = string.Empty;
    public string ContentButtonReturnSaveRequestMessage { get; set; } = string.Empty;
    public string LabelCreateAsGroupItem { get; set; } = string.Empty;

    private void FillAllInvoicePositionDetailsFormLabels()
    {
        LabelSaveRequestMessage = _translatorUiProvider.Translate("LabelSaveRequestMessage");
        LabelInvoicePositionGroupBox = _translatorUiProvider.Translate("LabelInvoicePositionGroupBox");
        ContentButtonSaveSaveRequestMessage = _translatorUiProvider.Translate("ContentButtonSaveSaveRequestMessage");
        ContentButtonNoSaveRequestMessage = _translatorUiProvider.Translate("ContentButtonNoSaveRequestMessage");
        ContentButtonReturnSaveRequestMessage = _translatorUiProvider.Translate("ContentButtonReturnSaveRequestMessage");
        LabelCreateAsGroupItem = _translatorUiProvider.Translate("LabelCreateAsGroupItem");
    }

    #endregion

    #region ToolTips
    public string ToolTipYesButtonSaveRequestMessage { get; set; } = string.Empty;
    public string ToolTipNoButtonSaveRequestMessage { get; set; } = string.Empty;
    public string ToolTipReturnButtonSaveRequestMessage { get; set; } = string.Empty;

    public string ToolTipVatCategory_S { get; set; } = string.Empty;
    public string ToolTipVatCategory_Z { get; set; } = string.Empty;
    public string ToolTipVatCategory_E { get; set; } = string.Empty;
    public string ToolTipVatCategory_AE { get; set; } = string.Empty;
    public string ToolTipVatCategory_K { get; set; } = string.Empty;
    public string ToolTipVatCategory_G { get; set; } = string.Empty;

    public string ToolTipInvoicePositionNr { get; set; } = string.Empty;

    public string ToolTipInvoicePositionDescription { get; set; } = string.Empty;
    public string ToolTipInvoicePositionProductDescription { get; set; } = string.Empty;

    public string ToolTipInvoicePositionItemNr { get; set; } = string.Empty;
    public string ToolTipInvoicePositionEan { get; set; } = string.Empty;
    public string ToolTipInvoicePositionQuantity { get; set; } = string.Empty;
    public string ToolTipInvoicePostionUnit { get; set; } = string.Empty;
    public string ToolTipInvoicePositionUnitPrice { get; set; } = string.Empty;

    public string ToolTipInvoicePositionSelectedVat { get; set; } = string.Empty;
    public string ToolTipInvoicePositionVatRate { get; set; } = string.Empty;
    public string ToolTipInvoicePositionSelectedVatCategory { get; set; } = string.Empty;
    public string ToolTipInvoicePositionDiscountReason { get; set; } = string.Empty;
    public string ToolTipInvoicePositionDiscountNetAmount { get; set; } = string.Empty;
    public string ToolTipInvoicePositionNetAmount { get; set; } = string.Empty;
    public string ToolTipInvoicePositionGrossAmount { get; set; } = string.Empty;
    public string ToolTipInvoicePositionNetAmountAfterDiscount { get; set; } = string.Empty;
    public string ToolTipCalculateTotalsAtInvoicePositionCommand { get; set; } = string.Empty;

    public string ToolTipInvoicePositionOrderDate { get; set; } = string.Empty;
    public string ToolTipInvoicePositionOrderId { get; set; } = string.Empty;

    public string ToolTipInvoicePositionDeliveryNoteDate { get; set; } = string.Empty;
    public string ToolTipInvoicePositionDeliveryNoteId { get; set; } = string.Empty;
    public string ToolTipInvoicePositionDeliveryNoteLineId { get; set; } = string.Empty;

    public string ToolTipInvoicePositionRefDocId { get; set; } = string.Empty;
    public string ToolTipInvoicePositionRefDocType { get; set; } = string.Empty;
    public string ToolTipInvoicePositionRefDocRefType { get; set; } = string.Empty;


    public string ToolTipAdditionalInfosExpander { get; set; } = string.Empty;

    private void FillAllInvoicePositionDetailsFormToolTips()
    {
        ToolTipYesButtonSaveRequestMessage = _translatorUiProvider.Translate("ToolTipYesButtonSaveRequestMessage");
        ToolTipNoButtonSaveRequestMessage = _translatorUiProvider.Translate("ToolTipNoButtonSaveRequestMessage");
        ToolTipReturnButtonSaveRequestMessage = _translatorUiProvider.Translate("ToolTipReturnButtonSaveRequestMessage");

        ToolTipVatCategory_S = _translatorUiProvider.Translate("ToolTipVatCategory_S");
        ToolTipVatCategory_Z = _translatorUiProvider.Translate("ToolTipVatCategory_Z");
        ToolTipVatCategory_E = _translatorUiProvider.Translate("ToolTipVatCategory_E");
        ToolTipVatCategory_AE = _translatorUiProvider.Translate("ToolTipVatCategory_AE");
        ToolTipVatCategory_K = _translatorUiProvider.Translate("ToolTipVatCategory_K");
        ToolTipVatCategory_G = _translatorUiProvider.Translate("ToolTipVatCategory_G");

        ToolTipInvoicePositionNr = _translatorUiProvider.Translate("ToolTipInvoicePositionNr");

        ToolTipInvoicePositionDescription = _translatorUiProvider.Translate("ToolTipInvoicePositionDescription");
        ToolTipInvoicePositionProductDescription = _translatorUiProvider.Translate("ToolTipInvoicePositionProductDescription");

        ToolTipInvoicePositionItemNr = _translatorUiProvider.Translate("ToolTipInvoicePositionItemNr");
        ToolTipInvoicePositionEan = _translatorUiProvider.Translate("ToolTipInvoicePositionEan");
        ToolTipInvoicePositionQuantity = _translatorUiProvider.Translate("ToolTipInvoicePositionQuantity");
        ToolTipInvoicePostionUnit = _translatorUiProvider.Translate("ToolTipInvoicePostionUnit");
        ToolTipInvoicePositionUnitPrice = _translatorUiProvider.Translate("ToolTipInvoicePositionUnitPrice");

        ToolTipInvoicePositionSelectedVat = _translatorUiProvider.Translate("ToolTipInvoicePositionSelectedVat");
        ToolTipInvoicePositionVatRate = _translatorUiProvider.Translate("ToolTipInvoicePositionVatRate");
        ToolTipInvoicePositionSelectedVatCategory = _translatorUiProvider.Translate("ToolTipInvoicePositionSelectedVatCategory");

        ToolTipInvoicePositionDiscountReason = _translatorUiProvider.Translate("ToolTipInvoicePositionDiscountReason");
        ToolTipInvoicePositionDiscountNetAmount = _translatorUiProvider.Translate("ToolTipInvoicePositionDiscountNetAmount");

        ToolTipInvoicePositionNetAmount = _translatorUiProvider.Translate("ToolTipInvoicePositionNetAmount");
        ToolTipInvoicePositionGrossAmount = _translatorUiProvider.Translate("ToolTipInvoicePositionGrossAmount");
        ToolTipInvoicePositionNetAmountAfterDiscount = _translatorUiProvider.Translate("ToolTipInvoicePositionNetAmountAfterDiscount");

        ToolTipCalculateTotalsAtInvoicePositionCommand = _translatorUiProvider.Translate("ToolTipCalculateTotalsAtInvoicePositionCommand");

        ToolTipInvoicePositionOrderDate = _translatorUiProvider.Translate("ToolTipInvoicePositionOrderDate");
        ToolTipInvoicePositionOrderId = _translatorUiProvider.Translate("ToolTipInvoicePositionOrderId");

        ToolTipInvoicePositionDeliveryNoteDate = _translatorUiProvider.Translate("ToolTipInvoicePositionDeliveryNoteDate");
        ToolTipInvoicePositionDeliveryNoteId = _translatorUiProvider.Translate("ToolTipInvoicePositionDeliveryNoteId");
        ToolTipInvoicePositionDeliveryNoteLineId = _translatorUiProvider.Translate("ToolTipInvoicePositionDeliveryNoteLineId");

        ToolTipInvoicePositionRefDocId = _translatorUiProvider.Translate("ToolTipInvoicePositionRefDocId");
        ToolTipInvoicePositionRefDocType = _translatorUiProvider.Translate("ToolTipInvoicePositionRefDocType");
        ToolTipInvoicePositionRefDocRefType = _translatorUiProvider.Translate("ToolTipInvoicePositionRefDocRefType");

        ToolTipAdditionalInfosExpander = _translatorUiProvider.Translate("ToolTipAdditionalInfosExpander");
    }

    #endregion

    #region Placeholder
    public string PlaceholderInvoicePositionDescription { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionProductDescription { get; set; } = string.Empty;

    public string PlaceholderInvoicePositionItemNr { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionEan { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionQuantity { get; set; } = string.Empty;
    public string PlaceholderInvoicePostionUnit { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionUnitPrice { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionVatRate { get; set; } = string.Empty;

    public string PlaceholderInvoicePositionDiscountReason { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionDiscountNetAmount { get; set; } = string.Empty;

    public string PlaceholderInvoicePositionNetAmount { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionGrossAmount { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionNetAmountAfterDiscount { get; set; } = string.Empty;

    public string PlaceholderInvoicePositionOrderId { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionDeliveryNoteId { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionDeliveryNoteLineId { get; set; } = string.Empty;

    public string PlaceholderInvoicePositionRefDocId { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionRefDocType { get; set; } = string.Empty;
    public string PlaceholderInvoicePositionRefDocRefType { get; set; } = string.Empty;

    private void FillAllInvoicePositionDetailsFormPlaceholders()
    {
        PlaceholderInvoicePositionDescription = _translatorUiProvider.Translate("PlaceholderInvoicePositionDescription");
        PlaceholderInvoicePositionProductDescription = _translatorUiProvider.Translate("PlaceholderInvoicePositionProductDescription");

        PlaceholderInvoicePositionItemNr = _translatorUiProvider.Translate("PlaceholderInvoicePositionItemNr");
        PlaceholderInvoicePositionEan = _translatorUiProvider.Translate("PlaceholderInvoicePositionEan");
        PlaceholderInvoicePositionQuantity = _translatorUiProvider.Translate("PlaceholderInvoicePositionQuantity");
        PlaceholderInvoicePostionUnit = _translatorUiProvider.Translate("PlaceholderInvoicePostionUnit");
        PlaceholderInvoicePositionUnitPrice = _translatorUiProvider.Translate("PlaceholderInvoicePositionUnitPrice");
        PlaceholderInvoicePositionVatRate = _translatorUiProvider.Translate("PlaceholderInvoicePositionVatRate");

        PlaceholderInvoicePositionDiscountReason = _translatorUiProvider.Translate("PlaceholderInvoicePositionDiscountReason");
        PlaceholderInvoicePositionDiscountNetAmount = _translatorUiProvider.Translate("PlaceholderInvoicePositionDiscountNetAmount");

        PlaceholderInvoicePositionNetAmount = _translatorUiProvider.Translate("PlaceholderInvoicePositionNetAmount");
        PlaceholderInvoicePositionGrossAmount = _translatorUiProvider.Translate("PlaceholderInvoicePositionGrossAmount");
        PlaceholderInvoicePositionNetAmountAfterDiscount = _translatorUiProvider.Translate("PlaceholderInvoicePositionNetAmountAfterDiscount");

        PlaceholderInvoicePositionOrderId = _translatorUiProvider.Translate("PlaceholderInvoicePositionOrderId");
        PlaceholderInvoicePositionDeliveryNoteId = _translatorUiProvider.Translate("PlaceholderInvoicePositionDeliveryNoteId");
        PlaceholderInvoicePositionDeliveryNoteLineId = _translatorUiProvider.Translate("PlaceholderInvoicePositionDeliveryNoteLineId");

        PlaceholderInvoicePositionRefDocId = _translatorUiProvider.Translate("PlaceholderInvoicePositionRefDocId");
        PlaceholderInvoicePositionRefDocType = _translatorUiProvider.Translate("PlaceholderInvoicePositionRefDocType");
        PlaceholderInvoicePositionRefDocRefType = _translatorUiProvider.Translate("PlaceholderInvoicePositionRefDocRefType");

    }
    #endregion

    #region Datepicker Tags
    public string TagInvoicePositionOrderDate { get; set; } = string.Empty;
    public string TagInvoicePositionDeliveryNoteDate { get; set; } = string.Empty;

    private void FillAllInvoicePositionDetailsFormTags()
    {
        TagInvoicePositionOrderDate = _translatorUiProvider.Translate("TagInvoicePositionOrderDate");
        TagInvoicePositionDeliveryNoteDate = _translatorUiProvider.Translate("TagInvoicePositionDeliveryNoteDate");
    }
    #endregion
}
