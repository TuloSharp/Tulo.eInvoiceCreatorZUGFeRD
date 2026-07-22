using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using tulo.CommonMVVM.Collector;
using tulo.CommonMVVM.Commands;
using tulo.CommonMVVM.GlobalProperties;
using tulo.CommonMVVM.ViewModels;
using tulo.CoreLib.Translators;
using Tulo.eInvoiceCreatorZUGFeRD.Commands;
using Tulo.eInvoiceCreatorZUGFeRD.Commands.Invoices;
using Tulo.Application.DTOs;
using Tulo.eInvoiceCreatorZUGFeRD.Options;
using Tulo.eInvoiceCreatorZUGFeRD.Services;
using Tulo.eInvoiceCreatorZUGFeRD.Stores.Invoices;
using Tulo.eInvoiceXmlGeneratorCii.Models;
using tulo.LoadingSpinnerControl.ViewModels;
using tulo.ResourcesWpfLib.Commands;

namespace Tulo.eInvoiceCreatorZUGFeRD.ViewModels.Invoices;

public partial class InvoiceViewModel : BaseViewModel
{
    #region Services / Stores filled via CollectorCollection
    private readonly IGlobalPropsUiManage _globalPropsUiManage;
    private readonly IInvoicePositionServiceByStore _invoicePositionService;
    private readonly ISelectedInvoicePositionStore _selectedInvoicePositionStore;
    private readonly ICollectorCollection _collectorCollection;
    //private readonly IRenavigationService _renavServiceEmployeCardList;
    private readonly IAppOptions _appOptions;
    private readonly ITranslatorUiProvider _translatorUiProvider;
    #endregion

    #region Date constants
    private static readonly CultureInfo _de = CultureInfo.GetCultureInfo("de-DE");
    private static readonly DateTime _minDate = new(1900, 1, 1);
    private static readonly DateTime _maxDate = new(2099, 12, 31);
    private const string DateFormat = "dd.MM.yyyy";
    #endregion

    #region Common Vars
    public Invoice Invoice { get; private set; } = new Invoice();
    public double? NormalWidthBeforePreview { get; set; } = AppConstants.Ui.WidthBeforePreview;
    #endregion

    #region InvoicePositions
    private readonly ObservableCollection<InvoicePositionCardItemViewModel> _invoicePositionCardListItemViewModel;
    public ICollectionView InvoicePositionCardListItemCollectionView { get; set; }

    private InvoicePositionCardItemViewModel? _selectedInvoicePositionCardItemViewModel;
    public InvoicePositionCardItemViewModel? SelectedInvoicePositionCardListItemViewModel
    {
        get => _selectedInvoicePositionCardItemViewModel;
        set
        {
            if (_selectedInvoicePositionCardItemViewModel == value)
                return;

            _selectedInvoicePositionCardItemViewModel = value;

            _selectedInvoicePositionStore.SelectedInvoicePositionId = value?.InvoicePositionId;
            _selectedInvoicePositionStore.SelectedInvoicePosition = value?.InvoicePositionDetails!;

            HasSelectedInvoicePosition = value != null;

            OnPropertyChanged(nameof(SelectedInvoicePositionCardListItemViewModel));
            OnPropertyChanged(nameof(HasSelectedInvoicePosition));
        }
    }
    #endregion

    #region Invoice Header
    private string _invoiceNumber = string.Empty;
    public string InvoiceNumber
    {
        get => _invoiceNumber;
        set => SetField(ref _invoiceNumber, value);
    }

    private string _currency = string.Empty;
    public string Currency
    {
        get => _currency;
        set => SetField(ref _currency, value);
    }

    private string _documentName = string.Empty;
    public string DocumentName
    {
        get => _documentName;
        set => SetField(ref _documentName, value);
    }

    private string _documentTypeCode = string.Empty;
    public string DocumentTypeCode
    {
        get => _documentTypeCode;
        set
        {
            if (SetField(ref _documentTypeCode, value))
            {
                // Code -> SelectedDocumentType sync
                var documentType = string.IsNullOrWhiteSpace(value)
                    ? null
                    : DocumentTypeCodesObservableCollection.FirstOrDefault(d => d.Code == value);

                if (!ReferenceEquals(_selectedDocumentTypeItem, documentType))
                {
                    _selectedDocumentTypeItem = documentType;
                    OnPropertyChanged(nameof(SelectedDocumentTypeItem));
                    OnPropertyChanged(nameof(SelectedDocumentTypeTooltip));
                }
            }
        }
    }

    public sealed class DocumentTypeItem
    {
        public string Code { get; }
        public string TextKey { get; }
        public string DisplayText { get; }

        public DocumentTypeItem(string code, string textKey, string displayText)
        {
            Code = code;
            TextKey = textKey;
            DisplayText = displayText;
        }
    }

    public ObservableCollection<DocumentTypeItem> DocumentTypeCodesObservableCollection { get; } = new();

    private void LoadDocumentTypeCodesList()
    {
        DocumentTypeCodesObservableCollection.Clear();
        void Add(string code, string key) =>
            DocumentTypeCodesObservableCollection.Add(
                new DocumentTypeItem(code, key, _translatorUiProvider.Translate(key)));

        Add("380", "DocumentTypeCode_380");
        Add("381", "DocumentTypeCode_381");
        Add("383", "DocumentTypeCode_383");
    }

    private DocumentTypeItem? _selectedDocumentTypeItem;
    public DocumentTypeItem? SelectedDocumentTypeItem
    {
        get => _selectedDocumentTypeItem;
        set
        {
            if (!SetField(ref _selectedDocumentTypeItem, value))
                return;

            OnPropertyChanged(nameof(SelectedDocumentTypeTooltip));

            if (!string.Equals(DocumentTypeCode, value?.Code ?? string.Empty, StringComparison.Ordinal))
                DocumentTypeCode = value?.Code ?? string.Empty;
        }
    }

    public string SelectedDocumentTypeTooltip => SelectedDocumentTypeItem is null ? string.Empty : _translatorUiProvider.Translate($"ToolTip{SelectedDocumentTypeItem.TextKey}");
    #endregion

    #region Buyer Party

    private string _companyBuyerParty = string.Empty;
    public string CompanyBuyerParty
    {
        get => _companyBuyerParty;
        set => SetField(ref _companyBuyerParty, value);
    }

    private string _fiscalIdBuyerParty = string.Empty;
    public string FiscalIdBuyerParty
    {
        get => _fiscalIdBuyerParty;
        set => SetField(ref _fiscalIdBuyerParty, value);
    }

    private string _vatIdBuyerParty = string.Empty;
    public string VatIdBuyerParty
    {
        get => _vatIdBuyerParty;
        set => SetField(ref _vatIdBuyerParty, value);
    }

    private string _erpCustomerNumberBuyerParty = string.Empty;
    public string ErpCustomerNumberBuyerParty
    {
        get => _erpCustomerNumberBuyerParty;
        set => SetField(ref _erpCustomerNumberBuyerParty, value);
    }

    private string _leitwegIdBuyerParty = string.Empty;
    public string LeitwegIdBuyerParty
    {
        get => _leitwegIdBuyerParty;
        set => SetField(ref _leitwegIdBuyerParty, value);
    }

    private string _personBuyerParty = string.Empty;
    public string PersonBuyerParty
    {
        get => _personBuyerParty;
        set => SetField(ref _personBuyerParty, value);
    }

    private string _streetBuyerParty = string.Empty;
    public string StreetBuyerParty
    {
        get => _streetBuyerParty;
        set => SetField(ref _streetBuyerParty, value);
    }

    private string _houseNumberBuyerParty = string.Empty;
    public string HouseNumberBuyerParty
    {
        get => _houseNumberBuyerParty;
        set => SetField(ref _houseNumberBuyerParty, value);
    }

    private string _postalCodeBuyerParty = string.Empty;
    public string PostalCodeBuyerParty
    {
        get => _postalCodeBuyerParty;
        set => SetField(ref _postalCodeBuyerParty, value);
    }

    private string _cityBuyerParty = string.Empty;
    public string CityBuyerParty
    {
        get => _cityBuyerParty;
        set => SetField(ref _cityBuyerParty, value);
    }

    private string _countryCodeBuyerParty = string.Empty;
    public string CountryCodeBuyerParty
    {
        get => _countryCodeBuyerParty;
        set => SetField(ref _countryCodeBuyerParty, value);
    }

    private string _phoneBuyerParty = string.Empty;
    public string PhoneBuyerParty
    {
        get => _phoneBuyerParty;
        set => SetField(ref _phoneBuyerParty, value);
    }

    private string _emailAddressBuyerParty = string.Empty;
    public string EmailAddressBuyerParty
    {
        get => _emailAddressBuyerParty;
        set => SetField(ref _emailAddressBuyerParty, value);
    }
    #endregion

    #region Payment Infos
    private string _paymentReference = string.Empty;
    public string PaymentReference
    {
        get => _paymentReference;
        set => SetField(ref _paymentReference, value);
    }

    private string _paymentTerms = string.Empty;
    public string PaymentTerms
    {
        get => _paymentTerms;
        set => SetField(ref _paymentTerms, value);
    }

    private DateOnly? _paymentDueDate;
    public DateOnly? PaymentDueDate
    {
        get => _paymentDueDate;
        set => SetField(ref _paymentDueDate, value);
    }

    private string _paymentDueDateText = string.Empty;
    public string PaymentDueDateText
    {
        get => _paymentDueDateText;
        set => SetDateText(ref _paymentDueDateText, value, nameof(PaymentDueDateText), v => PaymentDueDate = v, v => HasDatePickerError = v, v => DatePickerErrorMessage = v);
    }

    private bool _hasDatePickerError;
    public bool HasDatePickerError
    {
        get => _hasDatePickerError;
        set => SetField(ref _hasDatePickerError, value);
    }

    private string _datePikerErrorMessage = string.Empty;
    public string DatePickerErrorMessage
    {
        get => _datePikerErrorMessage;
        set => SetField(ref _datePikerErrorMessage, value);
    }

    private string _paymentMeansCode = string.Empty;
    public string PaymentMeansCode
    {
        get => _paymentMeansCode;
        set
        {
            if (SetField(ref _paymentMeansCode, value))
            {
                // Code -> SelectedPaymentMeans sync
                var paymentMeans = string.IsNullOrWhiteSpace(value)
                    ? null
                    : PaymentMeansCodesObservableCollection.FirstOrDefault(p => p.Code == value);

                if (!ReferenceEquals(_selectedPaymentMeansItem, paymentMeans))
                {
                    _selectedPaymentMeansItem = paymentMeans;
                    OnPropertyChanged(nameof(SelectedPaymentMeansItem));
                    OnPropertyChanged(nameof(ToolTipPaymentMeansCode));
                }
            }
        }
    }

    public sealed class PaymentMeansItem
    {
        public string Code { get; }
        public string TextKey { get; }
        public string DisplayText { get; }

        public PaymentMeansItem(string code, string textKey, string displayText)
        {
            Code = code;
            TextKey = textKey;
            DisplayText = displayText;
        }
    }

    public ObservableCollection<PaymentMeansItem> PaymentMeansCodesObservableCollection { get; } = new();

    private void LoadPaymentMeansCodesList()
    {
        PaymentMeansCodesObservableCollection.Clear();
        void Add(string code, string key) => PaymentMeansCodesObservableCollection.Add(new PaymentMeansItem(code, key, _translatorUiProvider.Translate(key)));

        Add("58", "PaymentMeansCode_58");
        Add("59", "PaymentMeansCode_59");
        Add("49", "PaymentMeansCode_49");
        Add("10", "PaymentMeansCode_10");
        Add("48", "PaymentMeansCode_48");
    }

    private PaymentMeansItem? _selectedPaymentMeansItem;
    public PaymentMeansItem? SelectedPaymentMeansItem
    {
        get => _selectedPaymentMeansItem;
        set
        {
            if (!SetField(ref _selectedPaymentMeansItem, value))
                return;

            OnPropertyChanged(nameof(ToolTipPaymentMeansCode));

            if (!string.Equals(PaymentMeansCode, value?.Code ?? string.Empty, StringComparison.Ordinal))
                PaymentMeansCode = value?.Code ?? string.Empty;
        }
    }

    public string ToolTipPaymentMeansCode => SelectedPaymentMeansItem is null ? string.Empty : _translatorUiProvider.Translate($"ToolTip{SelectedPaymentMeansItem.TextKey}");
    #endregion

    #region Payment Infos - Terms
    private bool _hasDiscount;
    public bool HasDiscount
    {
        get => _hasDiscount;
        set => SetField(ref _hasDiscount, value);
    }

    private string _discountPreviewText = string.Empty;
    public string DiscountPreviewText
    {
        get => _discountPreviewText;
        set => SetField(ref _discountPreviewText, value);
    }

    private void UpdateDiscountPreviewText()
    {
        DateOnly? previewDate = null;

        if (DiscountBasisDate.HasValue && int.TryParse(DiscountDays, out int days))
            previewDate = DiscountBasisDate.Value.AddDays(days);
        else
            previewDate = DiscountBasisDate;

        string dateText = previewDate?.ToString(DateFormat) ?? DateFormat;
        string percentText = DiscountPercent.ToString() ?? "...";

        DiscountPreviewText = PlaceholderDiscountPreviewText.Replace(DateFormat, dateText).Replace("...", percentText);
    }

    private decimal _discountPercent;
    public decimal DiscountPercent
    {
        get => _discountPercent;
        set
        {
            if (SetField(ref _discountPercent, value))
                UpdateDiscountPreviewText();
        }
    }

    private string _discountDays = string.Empty;
    public string DiscountDays
    {
        get => _discountDays;
        set => SetField(ref _discountDays, value);
    }

    private DateOnly? _discountBasisDate;
    public DateOnly? DiscountBasisDate
    {
        get => _discountBasisDate;
        set
        {
            if (SetField(ref _discountBasisDate, value))
                UpdateDiscountPreviewText();
        }
    }

    private string _discountBasisDateText = string.Empty;
    public string DiscountBasisDateText
    {
        get => _discountBasisDateText;
        set => SetDateText(ref _discountBasisDateText, value, nameof(DiscountBasisDateText), v => { DiscountBasisDate = v; UpdateDiscountPreviewText(); }, v => HasDiscountBasisDateError = v, v => DiscountBasisDateErrorMessage = v);
    }

    private bool _hasDiscountBasisDateError;
    public bool HasDiscountBasisDateError
    {
        get => _hasDiscountBasisDateError;
        set => SetField(ref _hasDiscountBasisDateError, value);
    }

    private string _discountBasisDateErrorMessage = string.Empty;
    public string DiscountBasisDateErrorMessage
    {
        get => _discountBasisDateErrorMessage;
        set => SetField(ref _discountBasisDateErrorMessage, value);
    }

    private DateOnly? _paymentDueDateRange;
    public DateOnly? PaymentDueDateRange
    {
        get => _paymentDueDateRange;
        set
        {
            if (SetField(ref _paymentDueDateRange, value))
                UpdateNoDiscountPreviewText();
        }
    }

    private string _paymentDueDateRangeText = string.Empty;
    public string PaymentDueDateRangeText
    {
        get => _paymentDueDateRangeText;
        set => SetDateText(ref _paymentDueDateRangeText, value, nameof(PaymentDueDateRangeText), v => { PaymentDueDateRange = v; UpdateNoDiscountPreviewText(); }, v => HasPaymentDueDateRangeError = v, v => PaymentDueDateRangeErrorMessage = v);
    }

    private bool _hasPaymentDueDateRangeError;
    public bool HasPaymentDueDateRangeError
    {
        get => _hasPaymentDueDateRangeError;
        set => SetField(ref _hasPaymentDueDateRangeError, value);
    }

    private string _paymentDueDateRangeErrorMessage = string.Empty;
    public string PaymentDueDateRangeErrorMessage
    {
        get => _paymentDueDateRangeErrorMessage;
        set => SetField(ref _paymentDueDateRangeErrorMessage, value);
    }

    private string _noDiscountPreviewText = string.Empty;
    public string NoDiscountPreviewText
    {
        get => _noDiscountPreviewText;
        set => SetField(ref _noDiscountPreviewText, value);
    }

    private void UpdateNoDiscountPreviewText()
    {
        string dateText = PaymentDueDateRange?.ToString(DateFormat) ?? DateFormat;

        NoDiscountPreviewText = PlaceholderNoDiscountPreviewText.Replace(DateFormat, dateText);
    }
    #endregion

    #region Local Properties
    private bool _hasSelectedInvoicePosition;
    public bool HasSelectedInvoicePosition
    {
        get => _hasSelectedInvoicePosition;
        set => SetField(ref _hasSelectedInvoicePosition, value);
    }
    #endregion

    #region Pdf Preview
    private bool _isPreviewEnabled;
    public bool IsPreviewEnabled
    {
        get => _isPreviewEnabled;
        set => SetField(ref _isPreviewEnabled, value);
    }

    private string _documentSource = string.Empty;
    public string DocumentSource
    {
        get => _documentSource;
        set => SetField(ref _documentSource, value);
    }

    private bool _resetSlideButton;
    public bool ResetSlideButton
    {
        get => _resetSlideButton;
        set => SetField(ref _resetSlideButton, value);
    }
    #endregion

    #region UI Control Properties + IUiControlPropsViewModel
    private bool _isEnabledSaveRequestInUI;
    public bool IsEnabledSaveRequestInUI
    {
        get => _isEnabledSaveRequestInUI;
        set => SetField(ref _isEnabledSaveRequestInUI, value);
    }

    private bool _isDuplicate;
    public bool IsDuplicate
    {
        get => _isDuplicate;
        set => SetField(ref _isDuplicate, value);
    }

    public MessageViewModel StatusMessageViewModel { get; }
    public string StatusMessage
    {
        set => StatusMessageViewModel.Message = value;
    }

    public static string SelectedViewModel => nameof(InvoiceViewModel);
    public string CurrentViewModelName => SelectedViewModel;
    #endregion

    #region Commands
    public ICommand UpdatePreviewInvoicePdfCommand { get; }
    public ICommand OpenAddInvoicePositionViewCommand { get; }
    public ICommand OpenEditInvoicePositionViewCommand { get; }
    public ICommand LoadInvoicePositionsCommand { get; }
    public ICommand CreateElectronicInvoiceComponentsCommand { get; }

    #endregion

    #region Common Commands
    public ICommand CloseSpinnerMessageCommand { get; }
    public ICommand OpenSpinnerMessageCommand { get; }
    public ICommand RequestBringIntoViewCommand { get; }
    public ICommand SaveCustomerDataCommand { get; }
    public ICommand LoadCustomerDataCommand { get; }
    public ICommand ClearAllInvoiceViewCommand { get; }
    #endregion

    #region Datepicker error message 
    private void SetDateText(ref string backingField, string? value, string propertyName, Action<DateOnly?> setDateValue, Action<bool> setHasError, Action<string> setErrorMessage)
    {
        if (backingField == value)
            return;

        backingField = value ?? string.Empty;
        OnPropertyChanged(propertyName);

        //reset error state
        setHasError(false);
        setErrorMessage(string.Empty);

        if (string.IsNullOrWhiteSpace(value))
        {
            setDateValue(null);
            return;
        }

        if (value.Length == 10 && DateTime.TryParseExact(value, DateFormat, _de, DateTimeStyles.None, out var dt))
        {
            if (dt.Date < _minDate || dt.Date > _maxDate)
            {
                setDateValue(null);
                setHasError(true);
                setErrorMessage($"{ContentDateMustBeBetween} {_minDate.ToString(DateFormat, _de)} & {_maxDate.ToString(DateFormat, _de)}.");
                return;
            }

            setDateValue(DateOnly.FromDateTime(dt));
            return;
        }

        setHasError(true);
        setErrorMessage(ContentDateInvalid);
    }
    #endregion 

    public InvoiceViewModel(ICollectorCollection collectorCollection)
    {
        #region Get Services / Stores from CollectorCollection
        _collectorCollection = collectorCollection;
        _selectedInvoicePositionStore = collectorCollection.GetService<ISelectedInvoicePositionStore>();
        _invoicePositionService = collectorCollection.GetService<IInvoicePositionServiceByStore>();
        _globalPropsUiManage = collectorCollection.GetService<IGlobalPropsUiManage>();
        //_renavServiceEmployeCardList = collectorCollection.GetService<IRenavigationService<EmployeeCardListViewModel>>();
        _appOptions = collectorCollection.GetService<IAppOptions>();
        _translatorUiProvider = collectorCollection.GetService<ITranslatorUiProvider>();
        #endregion

        _invoicePositionCardListItemViewModel = [];
        InvoicePositionCardListItemCollectionView = CollectionViewSource.GetDefaultView(_invoicePositionCardListItemViewModel);
        InvoicePositionCardListItemCollectionView.Filter = FilterInvoicePositions;
        InvoicePositionCardListItemCollectionView.SortDescriptions.Add(new SortDescription(nameof(InvoicePositionCardItemViewModel.LineId), ListSortDirection.Ascending));

        StatusMessageViewModel = new MessageViewModel();

        #region ViewModel Commands
        UpdatePreviewInvoicePdfCommand = new UpdatePreviewInvoicePdfCommand(this, _collectorCollection);
        OpenAddInvoicePositionViewCommand = new OpenModalStackCommand(collectorCollection, () => new AddInvoicePositionViewModel(_collectorCollection), typeof(AddInvoicePositionViewModel));
        OpenEditInvoicePositionViewCommand = new OpenModalStackCommand(collectorCollection, () => new EditInvoicePositionViewModel(_collectorCollection), typeof(EditInvoicePositionViewModel));
        SaveCustomerDataCommand = new SaveCustomerDataCommand(this, _collectorCollection);
        LoadCustomerDataCommand = new LoadCustomerDataCommand(this, _collectorCollection);
        ClearAllInvoiceViewCommand = new ClearAllInvoiceViewCommand(this, _collectorCollection);
        LoadInvoicePositionsCommand = new LoadInvoicePositionsCommand(this, _collectorCollection);
        CreateElectronicInvoiceComponentsCommand = new CreateElectronicInvoiceComponentsCommand(this, _collectorCollection);
        #endregion

        #region Common Commands
        OpenSpinnerMessageCommand = new OpenModalStackCommand(collectorCollection, () => new SpinnerMessageViewModel(), typeof(SpinnerMessageViewModel));
        CloseSpinnerMessageCommand = new CloseModalStackCommand(collectorCollection, typeof(SpinnerMessageViewModel));
        RequestBringIntoViewCommand = new RequestBringIntoViewCommand();
        #endregion

        #region Events
        _selectedInvoicePositionStore!.SelectedInvoicePositionChanged += OnSelectedInvoicePositionChanged;
        _invoicePositionCardListItemViewModel.CollectionChanged += OnInvoicePositionCollectionChanged;

        _invoicePositionService.InvoicePositionCreated += OnInvoicePositionCreated;
        _invoicePositionService.InvoicePositionUpdated += OnInvoicePositionUpdated;
        _invoicePositionService.InvoicePositionDeleted += OnInvoicePositionDeleted;
        _invoicePositionService.InvoicePositionsLoaded += OnInvoicePositionsLoaded;
        #endregion

        LoadDocumentTypeCodesList();
        LoadPaymentMeansCodesList();

        #region Partial Class Translation
        FillAllInvoiceToolTips();
        FillAllInvoicePlaceholders();
        FillAllInvoiceLabelsAndContents();
        #endregion

#if DEBUG
        //Only for UI Tests
        //SeedTestSellerData();
        //SeedTestInvoicePositions();
        SeedTestSellerDataSublines();
        SeedTestMainAndSubInvoicePositions();
#endif
    }

    private void OnInvoicePositionCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_selectedInvoicePositionCardItemViewModel != null && !_invoicePositionCardListItemViewModel.Contains(_selectedInvoicePositionCardItemViewModel))
            SelectedInvoicePositionCardListItemViewModel = null;
    }

    private void OnSelectedInvoicePositionChanged()
    {
        var storeId = _selectedInvoicePositionStore.SelectedInvoicePositionId;

        if (!storeId.HasValue)
            _selectedInvoicePositionCardItemViewModel = null;
        else
            _selectedInvoicePositionCardItemViewModel = _invoicePositionCardListItemViewModel.FirstOrDefault(invPos => invPos.InvoicePositionId == storeId.Value);

        HasSelectedInvoicePosition = _selectedInvoicePositionCardItemViewModel != null;

        HasSelectedInvoicePosition = _selectedInvoicePositionStore.SelectedInvoicePosition != null;
        OnPropertyChanged(nameof(SelectedInvoicePositionCardListItemViewModel));
    }

    private void AddInvoicePostion(InvoicePositionDetailsDTO invPosDetailsDTO, ICollectorCollection collectorCollection)
    {
        InvoicePositionCardItemViewModel invoicePositionCardItemViewModel = new(invPosDetailsDTO, collectorCollection);
        _invoicePositionCardListItemViewModel.Add(invoicePositionCardItemViewModel);
    }

    // smart update instead of clear + rebuild
    // - preserves existing card VM instances (no flicker, no selection loss)
    // - updates InvoicePositionNr and all amounts on existing cards
    // - adds new cards, removes deleted ones
    // - syncs the ObservableCollection order with the store order
    private void OnInvoicePositionsLoaded(List<InvoicePositionDetailsDTO> invoicePositions)
    {
        // 1. Update existing cards / add new ones
        foreach (var dto in invoicePositions)
        {
            var existing = _invoicePositionCardListItemViewModel.FirstOrDefault(vm => vm.InvoicePositionId == dto.Id);

            if (existing is not null)
                existing.Update(dto);
            else
                AddInvoicePostion(dto, _collectorCollection);
        }

        // 2. Remove cards that no longer exist in the store
        var activeIds = invoicePositions.Select(d => d.Id).ToHashSet();
        var toRemove = _invoicePositionCardListItemViewModel.Where(vm => !activeIds.Contains(vm.InvoicePositionId)).ToList();
        foreach (var vm in toRemove)
            _invoicePositionCardListItemViewModel.Remove(vm);

        // 3. Sync ObservableCollection order with store order
        for (var i = 0; i < invoicePositions.Count; i++)
        {
            var currentIndex = _invoicePositionCardListItemViewModel.IndexOf(_invoicePositionCardListItemViewModel.First(vm => vm.InvoicePositionId == invoicePositions[i].Id));

            if (currentIndex != i)
                _invoicePositionCardListItemViewModel.Move(currentIndex, i);
        }

        InvoicePositionCardListItemCollectionView.Refresh();

        if (_invoicePositionCardListItemViewModel.Count > 0)
            SelectedInvoicePositionCardListItemViewModel = _invoicePositionCardListItemViewModel[0];
    }

    private void OnInvoicePositionCreated(InvoicePositionDetailsDTO invoicePositionDetailsDTO)
    {
        var invPosViewModel = new InvoicePositionCardItemViewModel(invoicePositionDetailsDTO, _collectorCollection);

        _invoicePositionCardListItemViewModel.Add(invPosViewModel);
    }

    private void OnInvoicePositionUpdated(InvoicePositionDetailsDTO invoicePositionDetailsDTO)
    {
        var existingItemViewModel = _invoicePositionCardListItemViewModel.FirstOrDefault(invPos => invPos.InvoicePositionId == invoicePositionDetailsDTO.Id);

        if (existingItemViewModel == null) return;

        existingItemViewModel.Update(invoicePositionDetailsDTO);
        InvoicePositionCardListItemCollectionView.Refresh();
    }

    private void OnInvoicePositionDeleted(Guid id)
    {
        RemoveInvoicePositionFromUi(id);
    }

    public void RemoveInvoicePositionFromUi(Guid id)
    {
        var existingItemViewModel = _invoicePositionCardListItemViewModel
           .FirstOrDefault(x => x.InvoicePositionId == id);

        if (existingItemViewModel != null)
            _invoicePositionCardListItemViewModel.Remove(existingItemViewModel);

        // if deleted item was selected -> clear selection
        if (_selectedInvoicePositionStore.SelectedInvoicePositionId == id)
        {
            SelectedInvoicePositionCardListItemViewModel = null!;
            OnSelectedInvoicePositionChanged();
        }

        InvoicePositionCardListItemCollectionView.Refresh();
    }

    public static InvoiceViewModel LoadInvoiceViewModel(ICollectorCollection collectorCollection)
    {
        InvoiceViewModel invoiceViewModel = new(collectorCollection);
        invoiceViewModel.LoadInvoicePositionsCommand.Execute(null);
        return invoiceViewModel;
    }

    #region Search / Filter
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetField(ref _searchText, value))
                InvoicePositionCardListItemCollectionView.Refresh();
        }
    }

    private bool FilterInvoicePositions(object item)
    {
        if (item is not InvoicePositionCardItemViewModel vm)
            return false;

        if (string.IsNullOrWhiteSpace(SearchText))
            return true;

        var search = SearchText.Trim();

        return vm.InvoicePositionDescription.Contains(search, StringComparison.OrdinalIgnoreCase)
            || vm.InvoicePositionProductDescription.Contains(search, StringComparison.OrdinalIgnoreCase)
            || vm.InvoicePositionEan.Contains(search, StringComparison.OrdinalIgnoreCase);
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        _selectedInvoicePositionStore.SelectedInvoicePositionChanged -= OnSelectedInvoicePositionChanged;
        _invoicePositionCardListItemViewModel.CollectionChanged -= OnInvoicePositionCollectionChanged;

        _invoicePositionService.InvoicePositionCreated -= OnInvoicePositionCreated;
        _invoicePositionService.InvoicePositionUpdated -= OnInvoicePositionUpdated;
        _invoicePositionService.InvoicePositionDeleted -= OnInvoicePositionDeleted;
        _invoicePositionService.InvoicePositionsLoaded -= OnInvoicePositionsLoaded;

        base.Dispose();
    }
    #endregion
}