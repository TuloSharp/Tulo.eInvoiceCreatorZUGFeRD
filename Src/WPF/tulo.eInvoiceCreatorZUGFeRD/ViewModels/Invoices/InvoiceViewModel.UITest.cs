using Tulo.eInvoiceCreatorZUGFeRD.DTOs;

namespace Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;

#if DEBUG
public partial class InvoiceViewModel
{
    #region Test Main Positions
    private void SeedTestSellerData()
    {
        InvoiceNumber = "6063636771001";
        Currency = "EUR";
        DocumentName = "RECHNUNG";
        DocumentTypeCode = "380";

        CompanyBuyerParty = "Musterkunde GmbH & Co Name 2 Musterkunde";
        FiscalIdBuyerParty = "77777/01234";
        VatIdBuyerParty = "DE2012129398";
        ErpCustomerNumberBuyerParty = "9900880077";
        LeitwegIdBuyerParty = "04011000-1234512345-35";
        PersonBuyerParty = "Herr Test Monteur";

        StreetBuyerParty = "Musterstrasse";
        HouseNumberBuyerParty = "44";
        PostalCodeBuyerParty = "40789";
        CityBuyerParty = "Musterstadt";
        CountryCodeBuyerParty = "DE";
        PhoneBuyerParty = "02173 9364";
        EmailAddressBuyerParty = "mike.maier@lieferant.com";

        PaymentMeansCode = "58";
        PaymentReference = "Kundennummer:. 9900880077 Rechnungsnummer:. 6063636771001";
        PaymentTerms = "Zahlbar innerhalb von 14 Tagen ohne Abzug.";
        PaymentDueDate = new DateOnly(2026, 9, 16);
        PaymentDueDateText = "16.09.2026";
    }

    private void SeedTestInvoicePositions()
    {
        _invoicePositionService.AddInvoicePositionAsync(new InvoicePositionDetailsDTO
        {
            InvoicePositionNr = 1,
            InvoicePositionDescription = "GWDSTG-DIN976-A-4.8-(A2K)-M10X1000",
            InvoicePositionProductDescription = "Gewindestange",
            InvoicePositionItemNr = "0595810 25",
            InvoicePositionEan = "7711231873598",
            InvoicePositionQuantity = 25m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 2.06m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
            InvoicePositionOrderDate = new DateOnly(2026, 8, 27),
            InvoicePositionOrderId = "Abholung 1",
            InvoicePositionDeliveryNoteDate = new DateOnly(2026, 8, 27),
            InvoicePositionDeliveryNoteId = "8408230045",
            InvoicePositionDeliveryNoteLineId = "000010",
            InvoicePositionRefDocId = "2156307416",
            InvoicePositionRefDocType = "130",
            InvoicePositionRefDocRefType = "VN"
        }).GetAwaiter().GetResult();

        _invoicePositionService.AddInvoicePositionAsync(new InvoicePositionDetailsDTO
        {
            InvoicePositionNr = 2,
            InvoicePositionDescription = "MUELLSACK-EXTRASTARK-BLAU-700X1100X0,07",
            InvoicePositionProductDescription = "Müllsack, -beutel",
            InvoicePositionItemNr = "05899800555 150",
            InvoicePositionEan = "7748539263943",
            InvoicePositionQuantity = 150m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 49.29m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
            InvoicePositionOrderDate = new DateOnly(2026, 8, 27),
            InvoicePositionOrderId = "Abholung 1",
            InvoicePositionDeliveryNoteDate = new DateOnly(2026, 8, 27),
            InvoicePositionDeliveryNoteId = "8408230045",
            InvoicePositionDeliveryNoteLineId = "000020",
            InvoicePositionRefDocId = "2156307416",
            InvoicePositionRefDocType = "130",
            InvoicePositionRefDocRefType = "VN"
        }).GetAwaiter().GetResult();

        _invoicePositionService.AddInvoicePositionAsync(new InvoicePositionDetailsDTO
        {
            InvoicePositionNr = 3,
            InvoicePositionDescription = "SHR-AW30-(A2K)-7,5X152",
            InvoicePositionProductDescription = "Abstandsmontageschraube Rahmen",
            InvoicePositionItemNr = "05234830152 200",
            InvoicePositionEan = "7738898142591",
            InvoicePositionQuantity = 400m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 32.76m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
            InvoicePositionOrderDate = new DateOnly(2026, 8, 27),
            InvoicePositionOrderId = "Abholung 2",
            InvoicePositionDeliveryNoteDate = new DateOnly(2026, 8, 27),
            InvoicePositionDeliveryNoteId = "8408230046",
            InvoicePositionDeliveryNoteLineId = "000010",
            InvoicePositionRefDocId = "2156307417",
            InvoicePositionRefDocType = "130",
            InvoicePositionRefDocRefType = "VN"
        }).GetAwaiter().GetResult();
    }
    #endregion

    #region Test Main and Sub Positions
    private void SeedTestSellerDataSublines()
    {
        InvoiceNumber = "6063636771001";
        Currency = "EUR";
        DocumentName = "RECHNUNG";
        DocumentTypeCode = "380";

        // Buyer Party
        CompanyBuyerParty = "Musterkunde GmbH & Co Name 2 Musterkunde";
        FiscalIdBuyerParty = "77777/01234";
        VatIdBuyerParty = "DE2012129398";
        ErpCustomerNumberBuyerParty = "9900880077";
        LeitwegIdBuyerParty = "04011000-1234512345-35";
        PersonBuyerParty = "Herr Test Monteur";

        StreetBuyerParty = "Musterstrasse";
        HouseNumberBuyerParty = "44";
        PostalCodeBuyerParty = "40789";
        CityBuyerParty = "Musterstadt";
        CountryCodeBuyerParty = "DE";
        PhoneBuyerParty = "02173 9364";
        EmailAddressBuyerParty = "mike.maier@lieferant.com";

        // Payment
        PaymentMeansCode = "58";
        PaymentReference = "Kundennummer:. 9900880077 Rechnungsnummer:. 6063636771001";
        PaymentDueDate = new DateOnly(2026, 6, 13);
        PaymentDueDateText = "13.06.2026";

        // Payment Terms mit Skonto
        HasDiscount = true;
        DiscountPercent = 2m;
        DiscountDays = "7";
        DiscountBasisDate = new DateOnly(2026, 5, 30);
        DiscountBasisDateText = "30.05.2026";
        PaymentDueDateRange = new DateOnly(2026, 6, 13);
        PaymentDueDateRangeText = "13.06.2026";
    }

    private void SeedTestMainAndSubInvoicePositions()
    {
        // ── Pos 01 — GROUP mit Kindern (0101, 0102, 0103) ──────────────────────
        var result01 = _invoicePositionService.AddInvoicePositionAsync(new InvoicePositionDetailsDTO
        {
            InvoicePositionNr = 1,
            InvoicePositionDescription = "Subtotal hardware",
            InvoicePositionProductDescription = "Hardware Gesamt",
            InvoicePositionItemNr = "345678912",
            InvoicePositionEan = "6666656349852",
            InvoicePositionQuantity = 1m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 1000m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
            LineStatusReasonCode = "GROUP"
        }).GetAwaiter().GetResult();

        var parentId01 = result01.Data;

        // ── Sub 0101 — Laser printer B/W (2 x 300 = 600) ───────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId01, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "Laser printer B/W",
            InvoicePositionProductDescription = "Schwarzweiß Laserdrucker",
            InvoicePositionItemNr = "123456789",
            InvoicePositionEan = "88888886349852",
            InvoicePositionQuantity = 2m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 300m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

        // ── Sub 0102 — Ink printer color (3 x 150 = 450) ───────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId01, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "Ink printer color",
            InvoicePositionProductDescription = "Farbdrucker",
            InvoicePositionItemNr = "2345678910",
            InvoicePositionEan = "77777776349852",
            InvoicePositionQuantity = 3m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 150m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

        // ── Sub 0103 — Abschlag (-1 x 50 = -50) ────────────────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId01, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "Allowance",
            InvoicePositionProductDescription = "Abschlagsposition",
            InvoicePositionItemNr = "99992345678910",
            InvoicePositionEan = "0000006349852",
            InvoicePositionQuantity = -1m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 50m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

        // ── Pos 02 — GROUP mit Kindern (0201, 0202, 0203) ──────────────────────
        var result02 = _invoicePositionService.AddInvoicePositionAsync(new InvoicePositionDetailsDTO
        {
            InvoicePositionNr = 2,
            InvoicePositionDescription = "Subtotal Accessories",
            InvoicePositionProductDescription = "Zubehör Gesamt",
            InvoicePositionItemNr = "9345678912",
            InvoicePositionEan = "2222256349852",
            InvoicePositionQuantity = 1m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 405m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
            LineStatusReasonCode = "GROUP"
        }).GetAwaiter().GetResult();

        var parentId02 = result02.Data;

        // ── Sub 0201 — Toner (3 x 120 = 360) ───────────────────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId02, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "Toner",
            InvoicePositionProductDescription = "Toner",
            InvoicePositionItemNr = "456789123",
            InvoicePositionEan = "55555556349852",
            InvoicePositionQuantity = 3m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 120m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

        // ── Sub 0202 — Kopierpapier (10 x 9 = 90) ──────────────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId02, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "PAPER",
            InvoicePositionProductDescription = "Kopierpapier",
            InvoicePositionItemNr = "567891234",
            InvoicePositionEan = "5555556349852",
            InvoicePositionQuantity = 10m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 9m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

        // ── Sub 0203 — Abschlag (-1 x 45 = -45) ────────────────────────────────
        _invoicePositionService.AddSubInvoicePositionAsync(parentId02, new InvoicePositionDetailsDTO
        {
            InvoicePositionDescription = "Allowance",
            InvoicePositionProductDescription = "Abschlagsposition 10% von 450,-",
            InvoicePositionItemNr = "99992345678910",
            InvoicePositionEan = "0000006349852",
            InvoicePositionQuantity = -1m,
            InvoicePostionUnit = "H87",
            InvoicePositionUnitPrice = 45m,
            InvoicePositionVatRate = 19,
            InvoicePositionVatCategoryCode = "S",
            InvoicePositionNetAmount = 0m,
            InvoicePositionGrossAmount = 0m,
            InvoicePositionDiscountReason = string.Empty,
            InvoicePositionDiscountNetAmount = 0m,
            InvoicePositionNetAmountAfterDiscount = null,
        }).GetAwaiter().GetResult();

    }
    #endregion
}
#endif