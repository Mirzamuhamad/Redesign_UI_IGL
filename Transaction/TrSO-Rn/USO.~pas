unit USO;
//MTrSO.TrSO
interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  UTransEtr, Db, DBClient, MConnect, Grids, DBGrids, SMDBGrid, ComCtrls,
  StdCtrls, Buttons, HeaderFrame, ExtCtrls, ClientShare, Mask, DBCtrls,
  ToolEdit, RXDBCtrl, Utilities, Menus;

type
  TfmSO = class(TfmTransEtr)
    cdsFindCust: TClientDataSet;
    CdsGetCurrency: TClientDataSet;
    Label7: TLabel;
    Label13: TLabel;
    Label14: TLabel;
    Label22: TLabel;
    Label2: TLabel;
    dbeDate: TDBDateEdit;
    cbxFgReport: TDBComboBox;
    dbeCustCode: TDBEdit;
    dbeCustName: TDBEdit;
    dbeCustPONo: TDBEdit;
    cdsGetTerm: TClientDataSet;
    cdsGetTermTermCode: TStringField;
    cdsGetTermTermName: TStringField;
    cdsGetTermTypeRange: TStringField;
    cdsGetTermXRange: TIntegerField;
    Label32: TLabel;
    PcHdInfo: TPageControl;
    tsShipment: TTabSheet;
    Label30: TLabel;
    DBEdit4: TDBEdit;
    CdsHdLCurrency: TStringField;
    CdsHdLCurrDigit: TStringField;
    CdsHdLTerm: TStringField;
    CdsHdLTermRange: TStringField;
    CdsDtCAmount: TAggregateField;
    cdsGetUnit: TClientDataSet;
    CdsSearchTransOrder_No: TStringField;
    CdsSearchTransRevisi: TIntegerField;
    CdsSearchTransStatus: TStringField;
    CdsSearchTransOrder_Date: TDateTimeField;
    CdsSearchTransFgReport: TStringField;
    CdsSearchTransCustomer_Code: TStringField;
    CdsSearchTransCustomer_Name: TStringField;
    CdsSearchTransAttn: TStringField;
    CdsSearchTransBillTo: TStringField;
    CdsSearchTransProject_Code: TStringField;
    CdsSearchTransTerm: TStringField;
    CdsSearchTransDue_Date: TDateTimeField;
    CdsSearchTransProduct_Group: TStringField;
    CdsSearchTransProduct_Group_Name: TStringField;
    CdsSearchTransFgCBD: TStringField;
    CdsSearchTransCust_PO_No: TStringField;
    CdsSearchTransCust_PO_Date: TDateTimeField;
    CdsSearchTransDeliveryTo: TStringField;
    CdsSearchTransDelivery_Date: TDateTimeField;
    CdsSearchTransTerm_Payment: TStringField;
    CdsSearchTransCurrency: TStringField;
    CdsSearchTransForex_Rate: TFloatField;
    CdsSearchTransBase_Forex: TFloatField;
    CdsSearchTransPPN: TFloatField;
    CdsSearchTransPPN_Forex: TFloatField;
    CdsSearchTransFgDP: TStringField;
    CdsSearchTransDP: TFloatField;
    CdsSearchTransDP_Forex: TFloatField;
    CdsSearchTransRemark: TStringField;
    CdsSearchTransFgActive: TStringField;
    CdsSearchTransSO_Type: TStringField;
    CdsDtTransNmbr: TStringField;
    CdsDtRevisi: TIntegerField;
    CdsDtProductCode: TStringField;
    CdsDtSpecification: TStringField;
    CdsDtQtyOrder: TFloatField;
    CdsDtUnitOrder: TStringField;
    CdsDtPrice: TFloatField;
    CdsDtAmount: TFloatField;
    CdsDtQty: TFloatField;
    CdsDtUnit: TStringField;
    CdsDtRemark: TStringField;
    CdsDtDoneClosing: TStringField;
    CdsDtUserClose: TStringField;
    CdsDtDateClose: TDateTimeField;
    CdsDtQtyClose: TFloatField;
    CdsDtRemarkClose: TStringField;
    CdsDtQtyDO: TFloatField;
    CdsDtProductName: TStringField;
    CdsHdTransNmbr: TStringField;
    CdsHdRevisi: TIntegerField;
    CdsHdStatus: TStringField;
    CdsHdTransDate: TDateTimeField;
    CdsHdFgReport: TStringField;
    CdsHdCustCode: TStringField;
    CdsHdAttn: TStringField;
    CdsHdBillTo: TStringField;
    CdsHdTerm: TStringField;
    CdsHdDueDate: TDateTimeField;
    CdsHdProductGroup: TStringField;
    CdsHdFgCBD: TStringField;
    CdsHdCustPONo: TStringField;
    CdsHdCustPODate: TDateTimeField;
    CdsHdDeliveryDate: TDateTimeField;
    CdsHdCurrCode: TStringField;
    CdsHdForexRate: TFloatField;
    CdsHdBaseForex: TFloatField;
    CdsHdDisc: TFloatField;
    CdsHdDiscForex: TFloatField;
    CdsHdPPN: TFloatField;
    CdsHdPPNForex: TFloatField;
    CdsHdTotalForex: TFloatField;
    CdsHdFgDP: TStringField;
    CdsHdDP: TFloatField;
    CdsHdDPForex: TFloatField;
    CdsHdRemark: TStringField;
    CdsHdUserPrep: TStringField;
    CdsHdDatePrep: TDateTimeField;
    CdsHdUserAppr: TStringField;
    CdsHdDateAppr: TDateTimeField;
    CdsHdUserComplete: TStringField;
    CdsHdDateComplete: TDateTimeField;
    CdsHdFgActive: TStringField;
    CdsHdSOType: TStringField;
    CdsHdDoneDP: TStringField;
    CdsHdDonePFI: TStringField;
    CdsHdCustName: TStringField;
    CdsHdFgStatus: TStringField;
    CdsHdCollect_Name: TStringField;
    Label40: TLabel;
    dbeAttn: TDBEdit;
    dbeCustPODate: TDBDateEdit;
    Label41: TLabel;
    dbeProjectCode: TDBEdit;
    cdsGetProductGroup: TClientDataSet;
    cdsGetProductGroupProductGrpCode: TStringField;
    cdsGetProductGroupProductGrpName: TStringField;
    Label42: TLabel;
    dblProductGroup: TDBLookupComboBox;
    CdsHdLProductGroup: TStringField;
    Label33: TLabel;
    DBEdit9: TDBEdit;
    DBEdit17: TDBEdit;
    Label34: TLabel;
    DBDateEdit3: TDBDateEdit;
    CdsHdDeliveryName: TStringField;
    CdsDtLUnitOrder: TStringField;
    cdsGetConvertion: TClientDataSet;
    cdsGetConvertionRate: TFloatField;
    tsBillTo: TTabSheet;
    Label31: TLabel;
    dbeBillTo: TDBEdit;
    DBEdit16: TDBEdit;
    Label15: TLabel;
    dblTerm: TDBLookupComboBox;
    DBDateEdit1: TDBDateEdit;
    dbeProjectName: TDBEdit;
    Label3: TLabel;
    CdsHdProjectName: TStringField;
    CbxRevisi: TComboBox;
    CdsCekProduct: TClientDataSet;
    CdsGetRevisi: TClientDataSet;
    btnCreateRevisi: TBitBtn;
    CdsCreateRevisi: TClientDataSet;
    tsNominal: TTabSheet;
    Shape7: TShape;
    Shape6: TShape;
    Shape2: TShape;
    Label18: TLabel;
    Label19: TLabel;
    Label21: TLabel;
    Label27: TLabel;
    Shape8: TShape;
    Label28: TLabel;
    Shape4: TShape;
    Shape3: TShape;
    Shape1: TShape;
    Label20: TLabel;
    Label23: TLabel;
    Label29: TLabel;
    Shape5: TShape;
    Shape9: TShape;
    Label24: TLabel;
    Label26: TLabel;
    Shape12: TShape;
    Label38: TLabel;
    Label16: TLabel;
    DBEdit8: TDBEdit;
    DBEdit10: TDBEdit;
    DBEdit11: TDBEdit;
    DBEdit5: TDBEdit;
    dbDPForex: TDBEdit;
    dbePPN: TDBEdit;
    DBEdit3: TDBEdit;
    dbfgDP: TDBComboBox;
    dbDP: TDBEdit;
    DBEdit14: TDBEdit;
    dblCurrency: TDBLookupComboBox;
    dbeRate: TDBEdit;
    Label36: TLabel;
    Label37: TLabel;
    Label43: TLabel;
    btnProject: TBitBtn;
    btnCust: TBitBtn;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    CdsDtDonePDO: TStringField;
    cdsClosing: TClientDataSet;
    btnCloseItem: TBitBtn;
    CdsHdLCurrName: TStringField;
    CdsHdSales: TStringField;
    CdsHdDoneInvoice: TStringField;
    CdsHdSales_Name: TStringField;
    cdsFindSales: TClientDataSet;
    Label17: TLabel;
    dbSales: TDBEdit;
    dbSalesName: TDBEdit;
    btnSales: TBitBtn;
    cdsFindSalesSales_Code: TStringField;
    cdsFindSalesSales_Name: TStringField;
    cdsCekSJ: TClientDataSet;
    cdsCekSJTransNmbr: TStringField;
    cdsCekSJStatus: TStringField;
    cdsFindBillTo: TClientDataSet;
    cdsFindBillToBill_To: TStringField;
    cdsFindBillToBill_To_Name: TStringField;
    cdsFindBillToCurrency: TStringField;
    cdsCekDO: TClientDataSet;
    cdsCekDOStatus: TStringField;
    CdsHdFgPPN: TStringField;
    CdsHdDeliveryTo: TStringField;
    CdsHdDeliveryType: TStringField;
    Label25: TLabel;
    DBEdit2: TDBEdit;
    Label39: TLabel;
    DBComboBox1: TDBComboBox;
    Label44: TLabel;
    CdsDtPriceList: TFloatField;
    CdsHdDeliveryAddr1: TStringField;
    CdsHdDeliveryAddr2: TStringField;
    Label8: TLabel;
    Label35: TLabel;
    DBEdit1: TDBEdit;
    DBComboBox2: TDBComboBox;
    Label45: TLabel;
    DBEdit6: TDBEdit;
    CdsHdAmountDPList: TFloatField;
    CdsHdTermCust: TStringField;
    CdsDtCQtyOrder: TAggregateField;
    CdsMain: TClientDataSet;
    dsmain: TDataSource;
    CdsGetCountry: TClientDataSet;
    CdsGetCountryCountryCode: TStringField;
    CdsGetCountryCountryName: TStringField;
    CdsMainCustCode: TStringField;
    CdsMainDeliveryCode: TStringField;
    CdsMainDeliveryName: TStringField;
    CdsMainDeliveryAddr1: TStringField;
    CdsMainDeliveryAddr2: TStringField;
    CdsMainCountry: TStringField;
    CdsMainZipCode: TStringField;
    CdsMainUserId: TStringField;
    CdsMainUserDate: TDateTimeField;
    CdsMainPhoneNo: TStringField;
    CdsMainFax: TStringField;
    PNLcust: TPanel;
    Panel5: TPanel;
    DBGrid1: TSMDBGrid;
    CdsMainLCountry: TStringField;
    Label46: TLabel;
    Label47: TLabel;
    cdsCekCBD: TClientDataSet;
    cdsCekCBDFgCBD: TStringField;
    cdsCekCBDTermCode: TStringField;
    cdsGetTermFgCBD: TStringField;
    CdsHdProjectCode: TStringField;
    CdsHdTermPayment: TStringField;
    cdsGetInfoStock: TClientDataSet;
    cdsGetInfoStockOnHand: TFloatField;
    cdsGetInfoStockOnBook: TFloatField;
    cdsGetInfoStockOnProduction: TFloatField;
    pnlInfo: TPanel;
    Label48: TLabel;
    DBText1: TDBText;
    dsGetInfoStock: TDataSource;
    Label50: TLabel;
    DBText3: TDBText;
    cdsGetInfoStockOnWrhs: TFloatField;
    cdsGetInfoStockOnWIP: TFloatField;
    Label51: TLabel;
    lbWIP: TLabel;
    CdsCekExists: TClientDataSet;
    CdsHdLTermX: TIntegerField;
    CdsCekSJDt: TClientDataSet;
    CdsCekItemSJ: TClientDataSet;
    procedure BtnInsertClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure BtnSaveClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure dsDtDataChange(Sender: TObject; Field: TField);
    procedure btnPrintClick(Sender: TObject);
    procedure CdsHdNewRecord(DataSet: TDataSet);
    procedure CdsHdTransDateChange(Sender: TField);
    procedure dbeCustCodeKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure FormDestroy(Sender: TObject);
    procedure btnGetApprClick(Sender: TObject);
    procedure btnPostClick(Sender: TObject);
    procedure btnUnPostClick(Sender: TObject);
    procedure CdsHdCurrCodeChange(Sender: TField);
    procedure CdsHdBaseForexChange(Sender: TField);
    procedure CdsHdPPNForexChange(Sender: TField);
    procedure CdsHdFgDPChange(Sender: TField);
    procedure CdsHdDPChange(Sender: TField);
    procedure CdsHdFgReportChange(Sender: TField);
    procedure dgrSearchDblClick(Sender: TObject);
    procedure CdsDtAfterPost(DataSet: TDataSet);
    procedure CdsHdAfterOpen(DataSet: TDataSet);
    procedure CdsHdDiscForexChange(Sender: TField);
    procedure CdsHdDiscChange(Sender: TField);
    procedure dbeBillToKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure DBEdit9KeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure CdsHdFgCBDChange(Sender: TField);
    procedure CdsDtQtyChange(Sender: TField);
    procedure dbeProjectCodeKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure CdsHdCustCodeChange(Sender: TField);
    procedure DBGridDtEditButtonClick(Sender: TObject);
    procedure dsDtStateChange(Sender: TObject);
    procedure CdsDtNewRecord(DataSet: TDataSet);
    procedure CdsHdBeforePost(DataSet: TDataSet);
    procedure CdsDtQtyOrderChange(Sender: TField);
    procedure dsHdDataChange(Sender: TObject; Field: TField);
    procedure dsHdStateChange(Sender: TObject);
    procedure CdsDtProductCodeChange(Sender: TField);
    procedure btnCreateRevisiClick(Sender: TObject);
    procedure CbxRevisiChange(Sender: TObject);
    procedure CdsHdAfterPost(DataSet: TDataSet);
    procedure CdsDtBeforePost(DataSet: TDataSet);
    procedure tsShipmentExit(Sender: TObject);
    procedure tsBillToExit(Sender: TObject);
    procedure btnSearchClick(Sender: TObject);
    procedure dbeCodeExit(Sender: TObject);
    procedure btnCustClick(Sender: TObject);
    procedure btnProjectClick(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure BitBtn2Click(Sender: TObject);
    procedure CdsDtAfterScroll(DataSet: TDataSet);
    procedure CdsHdFgStatusChange(Sender: TField);
    procedure btnCloseItemClick(Sender: TObject);
    procedure CdsHdDPForexChange(Sender: TField);
    procedure dbfgDPClick(Sender: TObject);
    procedure dbSalesKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure CdsHdSalesChange(Sender: TField);
    procedure btnSalesClick(Sender: TObject);
    procedure CdsDtBeforeDelete(DataSet: TDataSet);
    procedure CdsHdBillToChange(Sender: TField);
    procedure CdsHdFgPPNChange(Sender: TField);
    procedure CdsHdTotalForexChange(Sender: TField);
    procedure DBComboBox2Change(Sender: TObject);
    procedure CdsMainNewRecord(DataSet: TDataSet);
    procedure CdsMainAfterPost(DataSet: TDataSet);
    procedure CdsMainAfterDelete(DataSet: TDataSet);
    procedure DBGrid1DblClick(Sender: TObject);
    procedure CdsMainBeforePost(DataSet: TDataSet);
    procedure Label47Click(Sender: TObject);
    procedure CdsDtAmountChange(Sender: TField);
    procedure CdsDtBeforeEdit(DataSet: TDataSet);
    procedure CdsDtAfterDelete(DataSet: TDataSet);
  private
    { Private declarations }
    SOType, StrFilterCust, StatusSJ, StatusDO : String;
    procedure CekDataHd;
    procedure ShowInfoStock;
    function PostingPO ( DataProvider : String ) : Boolean;
  public
    GetItem :Boolean;
    { Public declarations }

  end;

var
  fmSO: TfmSO;
  function show_menu(Handle: THandle; ASecurityRec: TSecurityRec): Longint;

implementation

uses FormSO;

{$R *.DFM}

function show_menu(Handle: THandle; ASecurityRec: TSecurityRec) : Longint;
var
  fmSO: TfmSO;
begin
  Application.Handle := Handle;
  Application.CreateForm(TfmSO, fmSO);
  with fmSO do
  try
    SecurityRec := ASecurityRec;
    SetRegistry;
    ConnectDCOM;
    SOType := SecurityRec.AddParam1;
    Caption := 'Sales Order - '+SOType;
    lblTitle1.Caption := 'Search Sales Order - '+SOType;
//    lblTitleReffA.Caption := 'Outstanding PR';
    SetCriteria(['Order_No', 'Revisi', 'FgStatus', 'Order_Date', 'FgReport', 'Customer_Code', 'Customer_Name', 'Attn', 'BillTo', 'Project_Code', 'Term', 'Due_Date', 'Product_Code', 'Product_Name', 'Product_Group', 'Product_Group_Name', 'FgCBD', 'Cust_PO_No', 'Cust_PO_Date', 'DeliveryTo', 'Delivery_Date', 'Term_Payment', 'Currency', 'Forex_Rate', 'Base_Forex', 'PPN', 'PPN_Forex', 'FgDP', 'DP', 'DP_Forex', 'Remark', 'FgActive'],['Order No', 'Revisi', 'Status', 'Order Date', 'Report', 'Customer Code', 'Customer Name', 'Attn', 'BillTo', 'Project Code', 'Term', 'Due Date', 'Product Code', 'Product Name', 'Product Group', 'Product Group Name', 'FgCBD', 'Cust PO No', 'Cust PO Date', 'DeliveryTo', 'Delivery Date', 'Term Payment', 'Currency', 'Forex Rate', 'Base Forex', 'PPN', 'PPN Forex', 'FgDP', 'DP', 'DP Forex', 'Remark', 'Active']);
    //SetCriteriaReff (['PR_No', 'PR_Date', 'Product_Code', 'Product_Name', 'Specification', 'Qty', 'Unit', 'Supplier_Code', 'Supplier_Name', 'Currency', 'Price'],['PR No', 'PR Date', 'Product Code', 'Product Name', 'Specification', 'Qty', 'Unit', 'Supplier Code', 'Supplier Name', 'Currency', 'Price']);
    //Caption := SecurityRec.ProgramName;
    ShowModal;
    Result := LongInt(fmSO);
  finally
    DisConnectDCOM;
    Application.ProcessMessages;
    Application.ProcessMessages;
    Free;
  end;
end;

procedure TfmSO.BtnInsertClick(Sender: TObject);
begin
  inherited;
  dbeCode.Text := TransNmbr;
  CbxRevisi.ItemIndex := 0;
  Cdshd.Insert;
  if dbeDate.CanFocus then dbeDate.SetFocus;

    StatusSJ := 'H';
    StatusDO := 'H';
    dbeCustCode.Enabled := True;
    IF SOType ='Project' then dbeProjectCode.Enabled := True;
    dbGridDt.Columns[0].ReadOnly := False;
    dbGridDt.Columns[3].ReadOnly := False;
    dbGridDt.Columns[4].ReadOnly := False;
    dbGridDt.Columns[5].ReadOnly := False;
    dbeCustCode.Enabled := True;
    dbeCustCode.Color := clWhite;
    IF SOType ='Project' then dbeProjectCode.Color := clWhite;

end;

procedure TfmSO.FormShow(Sender: TObject);
begin
  FieldSearchNmbr := 'Order_No';
  FieldSeacrhDate := 'Order_Date';
  CbxRevisi.ItemIndex := 0;
  TsDt.TabVisible := False;
  PcFormDt.ActivePage := tsDt;
  dbeProjectCode.Enabled := (SOType = 'Project');
  btnProject.Enabled := dbeProjectCode.Enabled;
  dbSales.Enabled := (SOType <> 'Project');
  if dbSales.Enabled then dbSales.Color := clWhite else dbsales.Color := cl3DLight;
  if dbeProjectCode.Enabled then begin
    dbeProjectCode.Color := clWindow;
  end else begin
    dbeProjectCode.Color := cl3DLight;
  end;
  cdsGetTerm.Active := False;
  cdsGetTerm.Data := Null;
  if not SetServerSettings then Exit;
  try
    cdsGetTerm.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  CdsGetCurrency.Active := False;
  CdsGetCurrency.Data := Null;
  if not SetServerSettings then Exit;
  try
    CdsGetCurrency.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  CdsGetCountry.Active := False;
  CdsGetCountry.Data := Null;
  if not SetServerSettings then Exit;
  try
    CdsGetCountry.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  cdsGetProductGroup.Active := False;
  cdsGetProductGroup.Data := Null;
  if not SetServerSettings then Exit;
  try
    cdsGetProductGroup.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  cdsGetUnit.Active := False;
  cdsGetUnit.Data := Null;
  if not SetServerSettings then Exit;
  try
    cdsGetUnit.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  If SOType = 'Project' then begin
    StrFilterCust := '';
  end else if SOType = 'Export' then begin
    StrFilterCust := ' And Group_Type = ''EXPORT'' ';
  end else begin
    StrFilterCust := ' And Group_Type = ''LOCAL'' ';
  end;
  inherited;
  dbeCode.Text := '';

end;

procedure TfmSO.BtnSaveClick(Sender: TObject);
begin
  StrAddParam := '';
  If SOType = 'Project' then begin
    StrTransType :='SOP';
  end else if SOType ='Export' then begin
    StrTransType :='SOE';
  end else begin
    STrTransType :='SOL';
  end;
  FlagPPn := CdsHdFgReport.AsString;
 if CdsHd.State = dsEdit then begin
  CdsCekExists.Active := False;
  CdsCekExists.Data := Null;
  if not SetServerSettings then Exit;
  CdsCekExists.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
  CdsCekExists.Params.ParamByName('@ProductCode').AsString := CdsDtProductCode.AsString;
  try
    CdsCekExists.Active := True;
        if CdsCekExists.Fields.FieldByName('QtyReceive').AsFloat > CdsDtQtyOrder.AsFloat then begin
          MessageDlg('Qty Order is more than Qty Receive (' + FloatToStr(CdsCekExists.Fields.FieldByName('QtyReceive').AsFloat) +')', mtConfirmation, [mbok], 0);
          CdsDt.Edit;
          DBGridDt.Fields[3].FocusControl;
          Exit;
        end;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
 end;
 inherited;

  pnlInfo.Visible := false;
end;

procedure TfmSO.btnEditClick(Sender: TObject);
begin
  inherited;
  cdsCekSJDt.Active := False;
  cdsCekSJDt.Data := Null;
  try
    cdsCekSJDt.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
    cdsCekSJDt.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
    if not SetServerSettings then Exit;
    cdsCekSJDt.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
 
  //IF not CdsCekSJ.IsEmpty then IF cdsCekSJ.Locate('Status','P',[]) then StatusSJ := 'P' else StatusSJ := 'H';

  cdsCekDO.Active := False;
  cdsCekDO.Data := Null;
  try
    cdsCekDO.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
    if not SetServerSettings then Exit;
    cdsCekDO.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;

  IF not cdsCekDO.IsEmpty then begin
    IF cdsCekDO.Locate('Status','P',[]) then StatusDO := 'P' else StatusDO := 'H';
    dbeCustCode.Enabled := False;
    IF SOType ='Project' then dbeProjectCode.Enabled := False;
    {dbGridDt.Columns[0].ReadOnly := StatusDO = 'P';
    dbGridDt.Columns[3].ReadOnly := StatusDO = 'P';
    dbGridDt.Columns[4].ReadOnly := StatusDO = 'P';
    dbGridDt.Columns[5].ReadOnly := StatusDO = 'P';}
  end else begin
    StatusDO := 'H';
    dbeCustCode.Enabled := true;
    IF SOType ='Project' then dbeProjectCode.Enabled := True;
    dbGridDt.Columns[0].ReadOnly := False;
    dbGridDt.Columns[3].ReadOnly := False;
    dbGridDt.Columns[4].ReadOnly := False;
    dbGridDt.Columns[5].ReadOnly := False;
  end;

  if dbeCustCode.Enabled then dbeCustCode.Color := clWhite else dbeCustCode.Color := cl3DLight;
  if dbeProjectCode.Enabled then dbeProjectCode.Color := clWhite else dbeProjectCode.Color := cl3DLight;

  cdshd.Edit;
  CekDataHd;
  if dbeDate.CanFocus then dbeDate.SetFocus;
  ShowInfoStock();
end;

procedure TfmSO.dsDtDataChange(Sender: TObject; Field: TField);
begin
  inherited;
  CekDataHd;
end;

procedure TfmSO.CekDataHd;
begin
  dbeDate.Enabled := (cdsdt.RecordCount = 0) and (CdsDt.State = dsbrowse);
  cbxFgReport.Enabled := (cdsdt.RecordCount = 0) and (CdsDt.State = dsbrowse);
  dbeProjectCode.Enabled := (cdsdt.RecordCount = 0) and (SOType = 'Project') and (CdsDt.State = dsbrowse);
  dbeCustCode.Enabled := (cdsdt.RecordCount = 0) and (CdsDt.State = dsbrowse);
  dblProductGroup.Enabled := (cdsdt.RecordCount = 0) and (CdsDt.State = dsbrowse);
  dblCurrency.Enabled := (cdsdt.RecordCount = 0) and (CdsDt.State = dsbrowse) and (CdsHdRevisi.AsInteger = 0);
  dbeRate.Enabled := (CdsHdCurrCode.AsString <> SecurityRec.Currency) AND (CdsHdRevisi.AsInteger = 0);
  btnCust.Enabled := dbeCustCode.Enabled;
  btnProject.Enabled := dbeProjectCode.Enabled;
  if dbeDate.Enabled then dbeDate.Color := clWindow else dbeDate.Color := cl3DLight;
  if cbxFgReport.Enabled then cbxFgReport.Color := clWindow else cbxFgReport.Color := cl3DLight;
  if dbeProjectCode.Enabled then dbeProjectCode.Color := clWindow else dbeProjectCode.Color := cl3DLight;
  if dbeCustCode.Enabled then dbeCustCode.Color := clWindow else dbeCustCode.Color := cl3DLight;
  if dblProductGroup.Enabled then dblProductGroup.Color := clWindow else dblProductGroup.Color := cl3DLight;
  if dblCurrency.Enabled then dblCurrency.Color := clWindow else dblCurrency.Color := cl3DLight;
end;

procedure TfmSO.btnPrintClick(Sender: TObject);
var fmFormSO : TfmFormSO;
begin
  inherited;
  if not Assigned(fmFormSO) then
    fmFormSO := TfmFormSO.Create(Nil);
  with fmFormSO do begin
    try
      qrlCompany.Caption := SecurityRec.CompanyName;
      QrlCompLineA.Caption := SecurityRec.CompanyLine1;
      QrlCompLineB.Caption := SecurityRec.CompanyLine2;
      Currency := CdsHdLCurrName.AsString;
      CurrCode := SecurityRec.Currency;
      QRDBText6.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      QRDBText5.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      QRDBText8.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      QRDBText9.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      QRDBText17.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      QRDBText18.Mask := GetFormatMoney(CdsHdLCurrDigit.AsInteger);
      try
        QRImageCompany.Picture.LoadFromFile('logo.jpg');
      except
        QRImageCompany.Picture.Metafile.Clear;
      end;
      if (SOType = 'Project') then begin
        QRDBSales.Top := 234;
      end else begin
        QRDBSales.Top := 248;
      end;
      If CdsHdFgReport.AsString ='N' Then Begin
        QRImageCompany.Enabled := False;
        qrlCompany.Enabled := False;
        QrlCompLineA.Enabled := False;
        QrlCompLineB.Enabled := False;
      end;
      cdsForm.Active := False;
      cdsForm.Data := null;
      cdsForm.Params.ParamByName('@Nmbr').AsString := CdsHdTransNmbr.asstring;
      CdsForm.Params.ParamByName('@Revisi').asInteger := CdsHdRevisi.AsInteger;
      if not SetServerSettings then exit;
      cdsForm.Active := true;
      QuickRep1.Prepare;
      QRSysData1.Enabled := QuickRep1.PageNumber > 1;
      QRLPage.Enabled := QuickRep1.PageNumber > 1;
      QRLPage.Caption := ' of '+IntToStr(QuickRep1.PageNumber);
      QuickRep1.Preview;
      Application.ProcessMessages;
      Application.ProcessMessages;
    finally
      cdsForm.Active := false;
      cdsForm.Data := null;
      FreeAndNil(fmFormSO);
    end;
  end;
end;

procedure TfmSO.CdsHdNewRecord(DataSet: TDataSet);
begin
  inherited;
  CdsHdFgReport.AsString := 'Y';
  CdsHdDeliveryType.AsString := 'DIAMBIL';
  CdsHdFgDP.AsString := 'N';
  CdsHdBaseForex.AsFloat := 0;
  CdsHdDiscForex.AsFloat := 0;
  CdsHdPPNForex.ASFloat := 0;
  CdsHdRevisi.AsInteger := 0;
  CdsHdPPN.AsFloat := 10;
  CdsHdDisc.AsFloat := 0;
  CdsHdFgActive.asString := 'Y';
  CdsHdFgCBD.AsString :='N';
  CdsHdSOType.AsString := SOType;
  CdsHdDoneDP.ASString :='N';
  CdsHdDonePFI.AsString :='N';
  CdsHdTransDate.AsDateTime := Date;
  CdsHdDP.AsFloat := 0;
  CdsHdDPForex.AsFloat := 0;
  CdsHdDeliveryDate.AsDateTime := Date;
end;

procedure TfmSO.CdsHdTransDateChange(Sender: TField);
begin
  inherited;
 if not (CdsHdTransDate.IsNull) and not (CdsHdTerm.IsNull) then begin
    CdsHdDueDate.AsDateTime := GetdueDate(CdsHdTransDate.AsDateTime, CdsHdLTermRange.AsString, CdsHdLTermX.AsInteger);
  end;
end;

procedure TfmSO.dbeCustCodeKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  inherited;
  if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if GetSearchDlg('Select Customer_Code, Customer_Name, Cust_Detail from V_MsCustomer Where Fg_Active = ''Y'' '+StrFilterCust, 'Customer_Code', 'Search Customer', 'Customer_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then CdsHdCustCode.Asstring := StrSearch[0];
    end;
  end;
  if (Key = vk_F1) then begin
    StrMnemonic := GetMnemonic(dbeCustCode.Text);
    if Length(StrMnemonic) > 0 then
      CdsHdCustCode.AsString := StrMnemonic;
  end;
end;

procedure TfmSO.FormDestroy(Sender: TObject);
begin
  inherited;
  cdsGetTerm.Filter := '';
  CdsGetTerm.Filtered := False;

  CdsGetCurrency.Active := False;
  CdsGetCurrency.Data := Null;
  cdsGetTerm.Active := False;
  cdsGetTerm.Data := Null;
  cdsGetUnit.Active := False;
  cdsGetUnit.Data := Null;
  cdsGetProductGroup.Active := False;
  cdsGetProductGroup.Data := Null;
  CdsGetCountry.Active := False;
  CdsGetCountry.Data := Null;
  CdsMain.Active := False;
  CdsMain.Data := Null;
  DBGrid1.DataSource := nil;
  DBGrid1.Free;
  CdsCekItemSJ.Active := False;
  CdsCekItemSJ.Data := Null;
end;

procedure TfmSO.btnGetApprClick(Sender: TObject);
begin
  StrProviderName := 'dpSOGetAppr';
//  inherited;
  if CdsHd.Active = False then begin
    MessageDlg('Data not open', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if CdsHd.IsEmpty then begin
    MessageDlg('No data transaction', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if not (CdsHd.FieldByName('FgStatus').AsString = 'Hold') then begin
    MessageDlg('Status data must be ''Hold''', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if PostingPO(StrProviderName) = True then begin
    CdsHd.Edit;
    CdsHd.FieldByName('FgStatus').AsString := 'Get Approval';
    CdsHd.Post;
    btnGetAppr.Enabled := False;
  end;                         
end;

procedure TfmSO.btnPostClick(Sender: TObject);
begin
  StrProviderName := 'dpSOPost';
//  inherited;
  if CdsHd.Active = False then begin
    MessageDlg('Data not open', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if CdsHd.IsEmpty then begin
    MessageDlg('No data transaction', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if not (CdsHd.FieldByName('FgStatus').AsString = 'Get Approval') then begin
    MessageDlg('Status data must be ''Get Approval''', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if PostingPO(StrProviderName) = True then begin
    CdsHd.Edit;
    CdsHd.FieldByName('FgStatus').AsString := 'Posted';
    CdsHd.FieldByName('UserAppr').AsString := SecurityRec.UserId;
    CdsHd.FieldByName('DateAppr').AsDateTime := Now;
    CdsHd.Post;
    btnPost.Enabled := False;
  end;
end;

procedure TfmSO.btnUnPostClick(Sender: TObject);
begin
  StrProviderName := 'dpSOUnPost';
//  inherited;
  if CdsHd.Active = False then begin
    MessageDlg('Data not open', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if CdsHd.IsEmpty then begin
    MessageDlg('No data transaction', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if not (CdsHd.FieldByName('FgStatus').AsString = 'Posted') then begin
    MessageDlg('Status data must be ''Posted''', mtConfirmation, [mbok], 0);
    Exit;
  end;
  if PostingPO(StrProviderName) = True then begin
    CdsHd.Edit;
    CdsHd.FieldByName('FgStatus').AsString := 'Get Approval';
    CdsHd.FieldByName('UserAppr').Clear;
    CdsHd.FieldByName('DateAppr').Clear;
    CdsHd.Post;
    btnUnPost.Enabled := False;
  end;
  loaddata(dbeCode.Text);
end;

procedure TfmSO.CdsHdCurrCodeChange(Sender: TField);
begin
  inherited;
  if CdsHdCurrCode.AsString = SecurityRec.Currency then begin
    CdsHdForexRate.AsFloat := 1;
  end else begin
    CdsHdForexRate.AsFloat := GetCurrRate(CdsHdTransDate.AsDateTime, CdsHdCurrCode.AsString);
  end;
  dbeRate.Enabled := (CdsHdCurrCode.AsString <> SecurityRec.Currency) AND (CdsHdRevisi.AsInteger = 0);
  if dbeRate.Enabled then dbeRate.Color := clWindow else dbeRate.Color := cl3DLight;
  SetFormatCurrency(CdsHdLCurrDigit.AsString, cdsHd);
  SetFormatCurrency(CdsHdLCurrDigit.AsString, cdsDt);
  DBGridDt.Columns[5].Title.Caption := 'Price ('+CdsHdCurrCode.AsString+')';
end;

procedure TfmSO.CdsHdBaseForexChange(Sender: TField);
begin
  inherited;
  CdsHdPPNForex.AsFloat := ((CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat ) * CdsHdPPN.AsFloat) / 100;
  If CdsHdDisc.Asfloat <> 0 then CdsHdDiscForex.AsFloat := (CdsHdBaseForex.AsFloat * CdsHdDisc.AsFloat) / 100;
  {if (CdsHdFgDP.AsString = 'Y') then begin
    IF CdsHdDPForex.AsFloat = 0 then begin
      If CdsHdBaseForex.Asfloat - CdsHdDiscForex.Asfloat = 0 then
        CdsHdDP.aSFloat := 0
        else
        CdsHdDP.AsFloat := (100 * CdsHdDPForex.AsFloat) / (CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat);
    end else begin
      CdsHdDPForex.AsFloat := (CdsHdDP.AsFloat * (CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat))/100;
    end;
  end;}
end;

procedure TfmSO.CdsHdPPNForexChange(Sender: TField);
begin
  inherited;
  if CdsHdLCurrDigit.AsString <> '' Then Begin
    CdsHdPPNForex.onchange := nil;
    if CdsHdCurrCode.asString = SecurityRec.Currency then CdsHdPPNForex.asfloat := trunc(CdsHdPPNForex.asfloat)
    else CdsHdPPNForex.asfloat := RoundDecimal(CdsHdPPNForex.asfloat, CdsHdLCurrDigit.ASInteger);
    CdsHdPPNForex.onchange := CdsHdPPNForexChange;
  end;
  CdsHdTotalForex.AsFloat := CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat + CdsHdPPNForex.AsFloat ;
end;

procedure TfmSO.CdsHdFgDPChange(Sender: TField);
begin
  inherited;
  If CdsHdFgDP.AsString = 'N' then begin
     CdsHdDP.AsFloat := 0;
     CdsHdDPForex.AsFloat := 0;
     dbDP.Enabled := False;
     dbdp.Color := cl3DLight;
     dbDPForex.Enabled := False;
     dbDPForex.Color := cl3dLight;
  end else begin
     dbDP.Enabled := True;
     dbdp.Color := clWhite;
     dbDPForex.Enabled := True;
     dbDPForex.Color := ClWhite;
  end;
  if dbDP.CanFocus then dbdp.SetFocus;

end;

procedure TfmSO.CdsHdDPChange(Sender: TField);
begin
  inherited;
  CdsHdDPForex.OnChange := nil;
  //CdsHdDPForex.AsFloat := (CdsHdDP.AsFloat * (CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat))/100;
  CdsHdDPForex.AsFloat := (CdsHdDP.AsFloat * CdsHdTotalForex.AsFloat)/100;
  CdsHdDPForex.OnChange := CdsHdDPForexChange;
end;

procedure TfmSO.CdsHdFgReportChange(Sender: TField);
begin
  inherited;
  If CdsHdFgReport.AsString ='Y' then begin
    CdsHdPPN.AsFloat := 10;
    dbePPN.Enabled := True;
    dbePPN.Color := clWhite;
  end else begin
    CdsHdPPN.AsFloat := 0;
    dbePPN.Enabled := False;
    dbePPN.Color := cl3DLight;
  end;
end;

procedure TfmSO.dgrSearchDblClick(Sender: TObject);
begin
  CdsHd.Params.ParamByName('@Revisi').AsInteger := CdsSearchTransRevisi.AsInteger;
  CdsHd.Params.ParamByName('@SOType').ASSTring := SOType;
  CdsDt.Params.ParamByName('Revisi').AsInteger := CdsSearchTransRevisi.AsInteger;
  inherited;
end;

procedure TfmSO.CdsDtAfterPost(DataSet: TDataSet);
begin
  inherited;
 if CdsDt.IsEmpty then exit;
 If (CdsHd.State = dsbrowse) then exit;
  if CdsDtAmount.AsFloat <= 0 then begin
   CdsHdBaseForex.Value := 0;
  end else begin
   CdsHdBaseForex.Value := CdsDtCAmount.Value;
  end;
end;

procedure TfmSO.CdsHdAfterOpen(DataSet: TDataSet);
begin
  inherited;
  btnCreateRevisi.Enabled := CdsHdFgStatus.AsString = 'Posted';
  If CdsHdFgCBD.Asstring ='Y' Then Begin
    dblTerm.Enabled := FAlse;
    dblTerm.Color := cl3DLight;
  end else begin
    dblTerm.Enabled := True;
    dblTerm.Color := clWindow;
  end;
  SetFormatCurrency(CdsHdLCurrDigit.AsString, cdsHd);
  SetFormatCurrency(CdsHdLCurrDigit.AsString, cdsDt);
  If CdsHdFgReport.AsString ='Y' then begin
    dbePPN.Enabled := True;
    dbePPN.Color := clWhite;
  end else begin
    dbePPN.Enabled := False;
    dbePPN.Color := cl3DLight;
  end;
  DBGridDt.Columns[5].Title.Caption := 'Price ('+CdsHdCurrCode.AsString+')';
  if not cdshd.IsEmpty then begin
    CdsGetRevisi.Active := False;
    CdsGetRevisi.Data := null;
    try
      CdsGetRevisi.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
      if not SetServerSettings then Exit;
      CdsGetRevisi.Active := True;
    except
      on E:Exception do begin
        MessageDlg(E.Message, mtError, [mbok], 0);
        Exit;
      end;
    end;
    CbxRevisi.Clear;
    CdsGetRevisi.First;
    While not CdsGetRevisi.Eof do begin
      CbxRevisi.Items.Add(CdsGetRevisi.Fields[0].AsString);
      CdsGetRevisi.Next;
    end;
    CbxRevisi.ItemIndex := CbxRevisi.Items.IndexOf(CdsHdRevisi.AsString);
  end;
end;

procedure TfmSO.CdsHdDiscForexChange(Sender: TField);
begin
  inherited;
  IF CdsHdLCurrDigit.AsString <> '' Then Begin
    CdsHdDiscForex.OnChange := nil;
    CdsHdDiscForex.AsFloat := RoundDecimal(CdsHdDiscForex.AsFloat, CdsHdLCurrDigit.AsInteger);
    CdsHdDiscForex.OnChange := CdsHdDiscForexChange;
  end;
  CdsHdPPNForex.AsFloat := ((CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat ) * CdsHdPPN.AsFloat) / 100;
  //CdsHdDisc.AsFloat := 0;
  If CdsHdDiscForex.AsFloat <> 0 then Begin
    CdsHdDisc.OnChange := nil;
    CdsHdDisc.AsFloat := (CdsHdDiscForex.AsFloat / CdsHdBaseForex.AsFloat) * 100;
    CdsHdDisc.OnChange := CdsHdDiscChange;
  end;
end;

procedure TfmSO.CdsHdDiscChange(Sender: TField);
begin
  inherited;
  If CdsHdDisc.Asfloat <> 0 then begin
    CdsHdDiscForex.OnChange := nil;
    CdsHdDiscForex.AsFloat := (CdsHdBaseForex.AsFloat * CdsHdDisc.AsFloat) / 100;
    CdsHdPPNForex.AsFloat := ((CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat ) * CdsHdPPN.AsFloat) / 100;
    //CdsHdDPChange(nil);
    CdsHdDiscForex.OnChange := CdsHdDiscForexChange;
  end;
end;


procedure TfmSO.dbeBillToKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  inherited;
  {if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (CdsHdCustName.AsString) = '' then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if GetSearchDlg('Select Collect_Code, Collect_Name from  Where CustCode = ''' + CdsHdCustCode.AsString + ''' ', 'Collect_Code', 'Search Bill To', 'Collect_Code', 'Collect_Name', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdBillTo.Asstring := StrSearch[0];
         CdsHdCollect_Name.AsString := StrSearch[1];
       end;
    end;
  end;}

  if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if GetSearchDlg('Select Customer_Code, Customer_Name from V_MsCustomer Where Fg_Active = ''Y'' '+StrFilterCust, 'Customer_Code', 'Search Customer', 'Customer_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then CdsHdBillTo.Asstring := StrSearch[0];
    end;
  end;
  if (Key = vk_F1) then begin
    StrMnemonic := GetMnemonic(dbeCustCode.Text);
    if Length(StrMnemonic) > 0 then
      CdsHdBillTo.AsString := StrMnemonic;
  end;

end;

procedure TfmSO.DBEdit9KeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  inherited;
  if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if Trim(CdsHdCustName.AsString) = '' then Exit;
    {if GetSearchDlg('Select DeliveryCode, DeliveryName, DeliveryAddr1, DeliveryAddr2 from V_MsCustShipTo Where CustCode = '''+CdsHdCustCode.AsString+''' ',
        'DeliveryCode', 'Search Delivery To', 'DeliveryCode', 'DeliveryName', 'DeliveryAddr1', 'DeliveryAddr2', '') then begin
       if length(StrSearch[1]) > 1 then begin
         CdsHdDeliveryTo.Asstring := StrSearch[0];
         CdsHdDeliveryName.Asstring := StrSearch[1];
         CdsHdDeliveryAddr1.ASString := StrSearch[2];
         CdsHdDeliveryAddr2.AsString := StrSearch[3];
       end;
    end;}
    CdsMain.Active := False;
    CdsMain.Data := Null;
    CdsMain.Params.ParamByName('Nmbr').AsString := CdsHdCustCode.AsString;
    if not SetServerSettings then Exit;
    try
      CdsMain.Active := True;
    except
      on E:Exception do begin
        MessageDlg(E.Message, mtError, [mbok], 0);
        Exit;
      end;
    end;

    pnlcust.top := 144;
    pnlcust.left := 144;
    PNLcust.Visible := True;
    PcForm.Enabled := false;
  end;
end;

procedure TfmSO.CdsHdFgCBDChange(Sender: TField);
begin
  inherited;
  CdsHdFgCBD.OnChange := nil;
  cdsGetTerm.Filter := 'FgCBD =''' + CdsHdFgCBD.AsString + '''';
  CdsGetTerm.Filtered := True;
  If CdsHdFgCBD.AsString = 'Y' Then Begin
    CdsHdFgDP.AsString := 'Y';
    CdsHdDP.AsFloat := 100;
    dbDP.Enabled := False;
    dbDPForex.Enabled := False;
    dbFgDP.Enabled := False;
    dbFgDP.Color := cl3DLight;
    dbDP.Color := cl3DLight;
    dbDPForex.Color := cl3DLight;
  end else begin
    dbFgDP.Enabled := True;
    dbFgDP.Color := clWhite;
    CdsHdFgDP.AsString := 'N';
    CdsHdDP.AsFloat := 0;
    CdsHdDPForex.AsFloat := 0;
    CdsHdTerm.Clear;
  end;
   CdsHdFgCBD.OnChange := CdsHdFgCBDChange;

end;

procedure TfmSO.CdsDtQtyChange(Sender: TField);
begin
  inherited;
  CdsDtAmount.AsFloat := CdsDtQtyOrder.ASFloat * CdsDtPrice.AsFloat;
end;

procedure TfmSO.dbeProjectCodeKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  inherited;
  if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (CdsHdCustName.AsString) = '' then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if GetSearchDlg('Select Project_Code, Project_Date, Project_Name, Sales_Group, SalesGroup_Name, Remark from V_MKProjectRegForSO Where CustCode = ''' + CdsHdCustCode.AsString + ''' ',
                'Project_Code', 'Search Project', 'Project_Code', 'Project_Name', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdProjectCode.Asstring := StrSearch[0];
         CdsHdProjectName.Asstring := StrSearch[1];
       end;
    end;
  end;
end;

procedure TfmSO.CdsHdCustCodeChange(Sender: TField);
begin
  inherited;
 { if CdsHdCustCode.AsString = '' then begin
    CdsHdCustName.Clear;
    CdsHdAttn.Clear;
    CdsHdCurrCode.Clear;
    CdsHdTerm.Clear;
    CdsHdBillTo.Clear;
    CdsHdCollect_Name.Clear;
    CdsHdProjectCode.Clear;
    CdsHdProjectName.Clear;
    CdsHdFgPPN.Clear;
    ShowMessage('a');
    Exit;
  end; }
  cdsFindCust.Active := False;
  cdsFindCust.Data := Null;
  cdsFindCust.Params.ParamByName('@Code').AsString := CdsHdCustCode.AsString;
  if SOType = 'Project' then begin
    cdsFindCust.Params.ParamByName('@Type').AsString := '';
  end else begin
    cdsFindCust.Params.ParamByName('@Type').AsString := UpperCase(SOType);
  end;
  cdsFindCust.Params.ParamByName('@Date').AsDate := CdsHdTransDate.AsDateTime;
  if not SetServerSettings then Exit;
  try
    cdsFindCust.Active := True;
    //CdsHdFgPPN.OnChange := nil;
  //  if not cdsFindCust.IsEmpty then begin
      CdsHdCustCode.OnChange := nil;
      CdsHdCustCode.AsString := cdsFindCust.fieldByName('CustCode').AsString;
      CdsHdCustCode.OnChange := CdsHdCustCodeChange;
      CdsHdCustName.AsString := cdsFindCust.fieldByName('CustName').AsString;
      CdsHdAttn.AsString := cdsFindCust.FieldByName('Attn').AsString;
      CdsHdCurrCode.AsString := cdsFindCust.FieldByName('CurrCode').AsString;
      CdsHdTerm.ASString := cdsFindCust.FieldByName('Term').AsString;
      CdsHdBillTo.OnChange := nil;
      CdsHdBillTo.asstring := CdsHdCustCode.asstring;
      CdsHdBillTo.OnChange := CdsHdBillToChange;
      CdsHdCollect_Name.AsString := CdsHdCustName.AsString;
      CdsHdFgPPN.ASString := CdsFindCust.FieldByName('FgPPN').AsString;
      CdsHdTermCust.AsString := CdsFindCust.FieldByName('Term').AsString;
   {       ShowMessage('c');
    end else begin
      CdsHdCustName.Clear;
      CdsHdAttn.Clear;
      CdsHdCurrCode.Clear;
      CdsHdTerm.Clear;
      CdsHdBillTo.Clear;
      CdsHdCollect_Name.Clear;
      CdsHdFgPPN.Clear;
          ShowMessage('d');
    end; }
    //CdsHdFgPPN.OnChange := CdsHdFgPPNChange;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  CdsHdProjectCode.Clear;
  CdsHdProjectName.Clear;
end;

procedure TfmSO.DBGridDtEditButtonClick(Sender: TObject);
begin
  inherited;
  if not (CdsDt.State in dseditmodes) then Exit;
  if DBGridDt.SelectedField = CdsDtProductCode then begin
    if SOType ='Project' then begin
      if GetSearchDlg('Select Product_Code, Product_Name, Qty, Unit, Unit_Order, Price, Remark From V_MKSOGetProductProject Where ProjectCode = '''+CdsHdProjectCode.AsString+''' and Product_Group = '''+CdsHdProductGroup.AsString+''' ',
                      'Product_Code', 'Search Product', 'Product_Code', 'Price', 'Qty', 'Product_Name', 'Unit') then begin
        if length(StrSearch[0]) > 1 then begin
          CdsDtProductCode.OnChange := nil;
          CdsDtProductCode.Asstring := StrSearch[0];
          CdsDtPrice.AsFloat := StrToFloat(StrSearch[1]);
          CdsDtProductName.Asstring := StrSearch[3];
          CdsDtUnitOrder.AsString := StrSearch[4];
          CdsDtUnit.asString := StrSearch[4];
          //CdsDtQty.AsInteger := StrToInt(StrSearch[2]);
          CdsDtQtyOrder.AsInteger := StrToInt(StrSearch[2]);
          CdsDtPriceList.AsFloat := CdsDtPrice.AsFloat;
          CdsDtProductCode.OnChange := CdsDtProductCodeChange;
        end;
      end;
    end else begin
      if GetSearchDlg('Select * From V_MsProductDt where FgActive =''Y'' AND Product_Group =''' + CdsHdProductGroup.AsString + ''' AND TransType =''SO'' ', 'Product_Code', 'Search Product', 'Product_Code', '', '', '', '') then begin
        if length(StrSearch[0]) > 1 then begin
          CdsDtProductCode.Asstring := StrSearch[0];
        end;
      end;
    end;
  end;
end;

procedure TfmSO.dsDtStateChange(Sender: TObject);
begin
  inherited;
  Case cdshd.State of
    dsbrowse : begin
                dbgriddt.Columns[4].FieldName := 'UnitOrder';
               end;
    dsEdit   : begin
                dbgriddt.Columns[4].FieldName := 'LUnitOrder';
               end;
    dsInsert : begin
                dbgriddt.Columns[4].FieldName := 'LUnitOrder';
                end;
  End;
end;

procedure TfmSO.CdsDtNewRecord(DataSet: TDataSet);
//VAR Total : Double;
begin
  inherited;
  CdsDtRevisi.AsInteger := CdsHdRevisi.AsInteger;
  CdsDtQtyOrder.ASFloat := 0;
  CdsDtPrice.AsFloat := 0;
  CdsDtDonePDO.AsString := 'N';
end;

procedure TfmSO.CdsHdBeforePost(DataSet: TDataSet);
begin
  if CdsHdTransDate.IsNull then begin
    MessageDlg('Date must have value', mtConfirmation, [mbok], 0);
    if dbeDate.CanFocus then dbeDate.SetFocus;
    Abort;
  end;
  if Trim(CdsHdCustName.AsString) = '' then begin
    MessageDlg('Customer must have value', mtConfirmation, [mbok], 0);
    if dbeCustCode.CanFocus then dbeCustCode.SetFocus;
    Abort;
  end;
  if (Trim(CdsHdProjectName.AsString) = '') and (UpperCase(CdsHdSOType.AsString) = 'PROJECT') then begin
    MessageDlg('Project Code must have value', mtConfirmation, [mbok], 0);
    if dbeProjectCode.CanFocus then dbeProjectCode.SetFocus;
    Abort;
  end;
  if Trim(CdsHdLProductGroup.AsSTring) = '' then begin
    MessageDlg('Product Group must have value', mtConfirmation, [mbok], 0);
    if dblProductGroup.CanFocus then dblProductGroup.SetFocus;
    Abort;
  end;
  IF (CdsHdDeliveryType.AsString <> 'DIAMBIL') AND (TRIM(CdsHdDeliveryName.ASstring) = '') Then begin
    MessageDlg('Delivery To must have value', mtConfirmation, [mbok], 0);
    pcHdInfo.ActivePage := tsShipment;
    if DBEdit9.CanFocus then DBEdit9.SetFocus;
    Abort;
  end;
  if (Trim(CdsHdSales_Name.AsSTring) = '') AND (dbSales.Enabled = True) then begin
    MessageDlg('Sales must have value', mtConfirmation, [mbok], 0);
    if dbSales.CanFocus then dbSales.SetFocus;
    Abort;
  end;
  if Trim(CdsHdCurrCode.AsString) = '' then begin
    MessageDlg('Currency must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if dblCurrency.CanFocus then dblCurrency.SetFocus;
    Abort;
  end;
  if (CdsHdForexRate.AsFloat = 0) then begin
    MessageDlg('Currency Rate must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if dbeRate.CanFocus then dbeRate.SetFocus;
    Abort;
  end;
  if (CdsHdForexRate.ASFloat  <= 0) AND (CdsHdFgReport.AsString ='Y') then begin
    MessageDlg('Currency Rate must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if dbeRate.CanFocus then dbeRate.SetFocus;
    Abort;
  end;
  if (CdsHdDisc.ASFloat  < 0) then begin
    MessageDlg('Discount must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if DBEdit14.CanFocus then DBEdit14.SetFocus;
    Abort;
  end;
  {Pif (CdsHdPPN.ASFloat  <= 0) AND (CdsHdFgPPN.AsString ='Y') then begin
    MessageDlg('PPN must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if dbePPN.CanFocus then dbePPN.SetFocus;
    Abort;
  end;}
  if (CdsHdFgDP.AsString = 'Y') and (CdsHdDP.AsFloat <= 0) then begin
    MessageDlg('DP % must have value', mtConfirmation, [mbok], 0);
    PcHdInfo.ActivePage := tsNominal;
    if dbDP.CanFocus then dbDP.SetFocus;
    Abort;
  end;
  inherited;
end;

function TfmSO.PostingPO(DataProvider: String): Boolean;
var EMsg : OleVariant;
begin
  Result := False;
  CdsPosting.ProviderName := DataProvider;
  CdsPosting.Active := False;
  CdsPosting.Data := Null;

  CdsPosting.FetchParams;
  CdsPosting.Params.ParamByName('@Nmbr').AsString := dbeCode.Text;
  CdsPosting.Params.ParamByName('@Revisi').AsInteger := CdsHdRevisi.AsInteger;
  CdsPosting.Params.ParamByName('@User').AsString := SecurityRec.UserId;
  CdsPosting.Params.ParamByName('@Year').AsInteger := SecurityRec.Year;
  CdsPosting.Params.ParamByName('@Period').AsInteger := SecurityRec.Period;
  if not SetServerSettings then Exit;
  try
     CdsPosting.Execute;
    EMsg := CdsPosting.Params.paramByName('@EMessage').AsString;
    if Trim(EMsg) <> '' then begin
      MessageDlg(EMsg, mtConfirmation, [mbok],0);
      Exit;
    end;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  Result := True;
end;

procedure TfmSO.CdsDtQtyOrderChange(Sender: TField);
begin
  inherited;
  if (CdsDtQtyOrder.AsFloat > 0) and not (CdsDtUnitOrder.IsNull) and not (CdsDtUnit.IsNull) then begin
    cdsGetConvertion.Active := False;
    cdsGetConvertion.Data := Null;
      cdsGetConvertion.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
      cdsGetConvertion.Params.ParamByName('@UnitFrom').AsString := CdsDtUnitOrder.AsString;
      cdsGetConvertion.Params.ParamByName('@UnitTo').AsString := CdsDtUnit.AsString;
    try
      if not SetServerSettings then Exit;
      cdsGetConvertion.Active := True;
    except
      on E : Exception do begin
       MessageDlg(E.Message,mtError,[mbok],0);
       Exit;
      end;
    end;
    CdsDtQty.Asfloat := RoundDecimal(cdsGetConvertionRate.ASFloat * CdsDtQtyOrder.ASFloat, 4);
  end else begin
    CdsDtQty.AsFloat := 0;
  end;
end;

procedure TfmSO.dsHdDataChange(Sender: TObject; Field: TField);
begin
  inherited;
  if CdsHd.State in dseditmodes then begin
    DBGridDt.ReadOnly := (not GetCanEditDt(CdsHd, ['TransDate','CustCode', 'CurrCode', 'ForexRate','ProductGroup']));
  end;
end;

procedure TfmSO.dsHdStateChange(Sender: TObject);
begin
  inherited;
  case CdsHd.State of
    dsbrowse : begin
                 CbxRevisi.Enabled := True;
                 btnCreateRevisi.Enabled := CdsHdFgStatus.AsString = 'Posted';
               end;
    dsEdit   : begin
                 CbxRevisi.Enabled := False;
                 btnCreateRevisi.Enabled := False;
                 btnSales.Enabled := dbSales.Enabled;
               end;
    dsInsert : begin
                 CbxRevisi.Enabled := False;
                 btnCreateRevisi.Enabled := False;
                 btnSales.Enabled := dbSales.Enabled;
               end;
  end;
end;

procedure TfmSO.CdsDtProductCodeChange(Sender: TField);
begin
  inherited;
  if CdsDtProductCode.AsString = '' then begin
    CdsDtProductName.Clear;
    CdsDtQtyOrder.AsFloat := 0;
    CdsDtUnit.Clear;
    CdsDtUnitOrder.Clear;
    CdsDtPrice.AsFloat := 0;
    CdsDtPriceList.AsFloat := 0;
    Exit;
  end;
  CdsCekProduct.Active := False;
  CdsCekProduct.Data := Null;
  CdsCekProduct.Params.ParamByName('@Currency').AsString := CdsHdCurrCode.AsString;
  CdsCekProduct.Params.ParamByName('@Date').AsDate := CdsHdTransDate.AsDateTime;
  CdsCekProduct.Params.ParamByName('@Rate').AsFloat := CdsHdForexRate.AsFloat;
  CdsCekProduct.Params.ParamByName('@ProjectCode').AsString := CdsHdProjectCode.AsString;
  CdsCekProduct.Params.ParamByName('@ProductGrp').AsString := CdsHdProductGroup.AsString;
  CdsCekProduct.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
  if not SetServerSettings then Exit;
  try
     CdsCekProduct.Active := true;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  if not CdsCekProduct.IsEmpty then begin
    if CdsCekProduct.FieldByName('Price').AsFloat = 0 then begin
      MessageDlg('Price for product ''' + CdsCekProduct.FieldByname('ProductName').AsString + ''' not exist on Price List', mtConfirmation, [mbok], 0);
      DBGridDt.Fields[0].FocusControl;
      CdsDtProductCode.OnChange := nil;
      CdsDtProductCode.Clear;
      CdsDtProductName.Clear;
      CdsDtQtyOrder.AsFloat := 0;
      CdsDtUnit.Clear;
      CdsDtUnitOrder.Clear;
      CdsDtPrice.AsFloat := 0;
      CdsDtPriceList.AsFloat := CdsDtPrice.AsFloat;
      CdsDtProductCode.OnChange := CdsDtProductCodeChange;
    end else begin
      CdsDtProductName.AsString := CdsCekProduct.FieldByName('ProductName').AsString;
      CdsDtQtyOrder.AsFloat := CdsCekProduct.FieldByName('Qty').AsFloat;
      CdsDtUnit.AsString := CdsCekProduct.FieldByName('Unit').AsString;
      CdsDtUnitOrder.AsString := CdsCekProduct.FieldByName('UnitOrder').AsString;
      CdsDtPrice.AsFloat := CdsCekProduct.FieldByName('Price').AsFloat;
      CdsDtPriceList.AsFloat := CdsDtPrice.AsFloat;
    end;
    ShowInfoStock();
  end else Begin
      MessageDlg('Price for product ''' + CdsCekProduct.FieldByname('ProductName').AsString + ''' not exist on Price List', mtConfirmation, [mbok], 0);
      DBGridDt.Fields[0].FocusControl;
      CdsDtProductCode.OnChange := nil;
      CdsDtProductCode.Clear;
      CdsDtProductCode.OnChange := CdsDtProductCodeChange;
      pnlInfo.visible := False;
      Exit;
  end;
end;

procedure TfmSO.btnCreateRevisiClick(Sender: TObject);
var EMessage : String;
begin
  inherited;
  If CdsHd.IsEmpty then exit;
  if MessageDlg('Sure to create new revisi for SO '''+CdsHdTransNmbr.AsString+''' ?', mtConfirmation, [mbyes,mbno],0) = mrno then begin
    Exit;
  end;
  CdsCreateRevisi.Active := False;
  CdsCreateRevisi.Data := NULL;
  try
    CdsCreateRevisi.FetchParams;
    CdsCreateRevisi.Params.ParamByName('@Nmbr').AsString := CdsHdTransNmbr.AsString;
    CdsCreateRevisi.Params.ParamByName('@User').AsString := SecurityRec.UserId;
    if not SetServerSettings then Exit;
    CdsCreateRevisi.Execute;
    EMessage := CdsCreateRevisi.Params.ParamByName('@EMessage').AsString;
    if Length(Trim(EMessage)) > 5 then begin
      MessageDlg(EMessage,mtError,[mbok],0);
      Exit;
    end;
  except
    on E : Exception do begin
      MessageDlg(E.Message,mtError,[mbok],0);
      Exit;
    end;
  end;
  CdsHd.Params.ParamByName('@Revisi').ASinteger := StrToInt(EMessage);
  CdsHd.Params.ParamByName('@SOType').AsString := SOType;
  CdsDt.Params.Parambyname('Revisi').AsInteger := StrToInt(EMessage);
  LoadData(CdsHdTransNmbr.AsString);
  btnEditClick(nil);
end;

procedure TfmSO.CbxRevisiChange(Sender: TObject);
begin
  inherited;
  CdsHd.Params.ParamByName('@Revisi').ASinteger := StrToInt(CbxRevisi.Text);
  CdsHd.Params.ParamByName('@SOType').AsString := SOType;
  CdsDt.Params.Parambyname('Revisi').AsInteger := StrToInt(CbxRevisi.Text);
  LoadData(dbeCode.Text);
end;

procedure TfmSO.CdsHdAfterPost(DataSet: TDataSet);
begin
  inherited;
  btnCreateRevisi.Enabled := CdsHdFgStatus.AsString = 'Posted';
end;

procedure TfmSO.CdsDtBeforePost(DataSet: TDataSet);
var Index : Integer;
begin
  if Trim(CdsDtProductName.AsString) = '' then begin
    MessageDlg('Product must have value',mtError,[mbok],0);
    DBGridDt.Fields[0].FocusControl;
    abort;
  end;
  if CdsDtQtyOrder.AsFloat = 0 then begin
    MessageDlg('Qty Order must have value', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[3].FocusControl;
    Abort;
  end;
  if CdsDtUnitOrder.AsString = '' then begin
    MessageDlg('Unit Order must have value', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[4].FocusControl;
    Abort;
  end;
  if CdsDtPrice.AsFloat = 0 then begin
    MessageDlg('Price must have value', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[5].FocusControl;
    Abort;
  end;
  if Cdsdtqty.AsFloat = 0 then begin
    MessageDlg('Qty Warehouse must have value', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[3].FocusControl;
    Abort;
  end;
{  IF CdsDtQty.AsFloat < CdsDtQtyDO.AsFloat Then Begin
    MessageDlg('Qty Warehouse is lower than Qty DO (' + FloatToStr(CdsDtQtyDo.AsFloat) +')', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[7].FocusControl;
    Abort;
  end;}
  if Trim(CdsDtUnit.AsString) = '' then begin
    MessageDlg('Unit Warehouse must have value', mtConfirmation, [mbok], 0);
    DBGridDt.Fields[8].FocusControl;
    Abort;
  end;
  if ListItem.Find(CdsDtProductCode.AsString, Index) then begin
    MessageDlg('Product '''+CdsDtProductName.AsString+''' already exists',mtError,[mbok],0);
    DBGridDt.Fields[0].FocusControl;
    abort;
  end;
  inherited;
end;

procedure TfmSO.tsShipmentExit(Sender: TObject);
begin
  inherited;
  {PcHdInfo.ActivePage := tsBillTo;
  if dbeBillTo.CanFocus then dbeBillTo.SetFocus;}
end;

procedure TfmSO.tsBillToExit(Sender: TObject);
begin
  inherited;
  {PcHdInfo.ActivePage := tsNominal;
  if dblCurrency.CanFocus then dblCurrency.SetFocus;}
end;

procedure TfmSO.btnSearchClick(Sender: TObject);
begin
  StrFindAdditional := ' SO_Type = '''+SOType+''' ';
  inherited;
end;

procedure TfmSO.dbeCodeExit(Sender: TObject);
begin
  CdsHd.Params.ParamByName('@Revisi').ASinteger := StrToInt(CbxRevisi.Text);
  CdsHd.Params.ParamByName('@SOType').AsString := SOType;
  CdsDt.Params.Parambyname('Revisi').AsInteger := StrToInt(CbxRevisi.Text);
  inherited;
end;

procedure TfmSO.btnCustClick(Sender: TObject);
begin
  inherited;
  if cdshd.state = dsbrowse then Exit;
    if GetSearchDlg('Select Customer_Code, Customer_Name, Cust_Detail from V_MsCustomer Where Fg_Active = ''Y'' '+StrFilterCust, 'Customer_Code', 'Search Customer', 'Customer_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdCustCode.Asstring := StrSearch[0];
       end;
    end;
end;

procedure TfmSO.btnProjectClick(Sender: TObject);
begin
  inherited;
  if cdshd.state = dsbrowse then Exit;
  if (CdsHdCustName.AsString) = '' then Exit;
    if GetSearchDlg('Select Project_Code, Project_Date, Project_Name, Sales_Group, SalesGroup_Name, Remark from V_MKProjectRegForSO Where CustCode = ''' + CdsHdCustCode.AsString + ''' ',
                'Project_Code', 'Search Project', 'Project_Code', 'Project_Name', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdProjectCode.Asstring := StrSearch[0];
         CdsHdProjectName.Asstring := StrSearch[1];
       end;
    end;
end;

procedure TfmSO.BitBtn1Click(Sender: TObject);
begin
  inherited;
  if cdshd.state = dsbrowse then Exit;
    if Trim(CdsHdCustName.AsString) = '' then Exit;
    {if GetSearchDlg('Select DeliveryCode, DeliveryName, DeliveryAddr1, DeliveryAddr2 from V_MsCustShipTo Where CustCode = '''+CdsHdCustCode.AsString+''' ',
        'DeliveryCode', 'Search Delivery To', 'DeliveryCode', 'DeliveryName', 'DeliveryAddr1', 'DeliveryAddr2', '') then begin
       if length(StrSearch[1]) > 1 then begin
         CdsHdDeliveryTo.Asstring := StrSearch[0];
         CdsHdDeliveryName.Asstring := StrSearch[1];
         CdsHdDeliveryAddr1.ASString := StrSearch[2];
         CdsHdDeliveryAddr2.AsString := StrSearch[3];
       end;
    end;}
    CdsMain.Active := False;
    CdsMain.Data := Null;
    CdsMain.Params.ParamByName('Nmbr').AsString := CdsHdCustCode.AsString;
    if not SetServerSettings then Exit;
    try
      CdsMain.Active := True;
    except
      on E:Exception do begin
        MessageDlg(E.Message, mtError, [mbok], 0);
        Exit;
      end;
    end;
    pnlcust.top := 144;
    pnlcust.left := 144;
    PNLcust.Visible := True;
    PcForm.Enabled := false;
end;

procedure TfmSO.BitBtn2Click(Sender: TObject);
begin
  inherited;
  if cdshd.state = dsbrowse then Exit;
    if GetSearchDlg('Select Customer_Code, Customer_Name from V_MsCustomer Where Fg_Active = ''Y'' '+StrFilterCust, 'Customer_Code', 'Search Customer', 'Customer_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then CdsHdBillTo.Asstring := StrSearch[0];
    end;
end;

procedure TfmSO.CdsDtAfterScroll(DataSet: TDataSet);
begin
  inherited;
  btnCloseItem.Visible := ((CdsDtQty.AsFloat > CdsDtQtyDO.AsFloat) AND (CdsDtDoneClosing.ASString <>'Y') AND (CdsHdFgStatus.AsString ='Posted') AND (CdsHdFgActive.AsString ='Y'));
  if cdshd.State in dsEditModes then ShowInfoStock();
  CdsCekItemSJ.Active := False;
  CdsCekItemSJ.Data := NULL;
  try
    CdsCekItemSJ.FetchParams;
    CdsCekItemSJ.Params.ParamByName('@Nmbr').AsString := CdsHdTransNmbr.AsString;
    CdsCekItemSJ.Params.ParamByName('@ProductCode').AsString := CdsDtProductCode.AsString;
    if not SetServerSettings then Exit;
    CdsCekItemSJ.active:=True;
    if not CdsCekItemSJ.IsEmpty then
      dbgriddt.Columns[5].ReadOnly:=True
    else dbgriddt.Columns[5].ReadOnly:=False;
  except
    on E : Exception do begin
      MessageDlg(E.Message,mtError,[mbok],0);
      Exit;
    end;
  end;
end;

procedure TfmSO.CdsHdFgStatusChange(Sender: TField);
begin
  inherited;
  btnCloseItem.Visible := ((CdsDtQty.AsFloat > CdsDtQtyDO.AsFloat) AND (CdsDtDoneClosing.ASString <>'Y') AND (CdsHdFgStatus.AsString ='Posted') AND (CdsHdFgActive.AsString ='Y'));
end;

procedure TfmSO.btnCloseItemClick(Sender: TObject);
var EMsg  : Variant;
    RemarkClose : String;
    ClickOK : Boolean;
begin
  inherited;
  if cdsdt.IsEmpty then Exit;
  //RemarkClose := InputBox('Closing Product','Reason Closing :','');
  ClickOK := InputQuery('Closing Product','Reason Closing :',RemarkClose);
  IF ClickOK then begin
    CdsClosing.Active := False;
    CdsClosing.Data := Null;
    try
      CdsClosing.FetchParams;
      CdsClosing.Params.ParamByName('@Nmbr').AsString := CdsHdTransNmbr.AsString;
      CdsClosing.Params.ParamByName('@Revisi').AsInteger := CdsHdRevisi.AsInteger;
      CdsClosing.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
      CdsClosing.Params.ParamByName('@Remark').AsString := RemarkClose;
      CdsClosing.Params.ParamByName('@User').AsString := SecurityRec.UserId;
      if not SetServerSettings then Exit;
      CdsClosing.Execute;
      EMsg := CdsClosing.Params.ParamByName('@EMessage').AsString;
    except
      on E:Exception do begin
        MessageDlg(E.Message, mtError, [mbok], 0);
        Exit;
      end;
    end;
    if Trim(EMsg) <> '' then begin
      MessageDlg(EMsg, mtError, [mbok], 0);
      Exit;
    end;
    LoadData(CdsHdTransNmbr.AsString);
  end;
end;

procedure TfmSO.CdsHdDPForexChange(Sender: TField);
begin
  inherited;
  IF CdsHdBaseForex.AsFloat <> 0 then begin
    CdsHdDP.OnChange := nil;
    IF CdsHdLCurrDigit.AsString <> '' Then Begin
      CdsHdDPForex.OnChange := nil;
      CdsHdDPForex.AsFloat := RoundDecimal(CdsHdDPForex.AsFloat, CdsHdLCurrDigit.AsInteger);
      CdsHdDPForex.OnChange := CdsHdDPForexChange;
    end;
    //CdsHdDP.AsFloat := (100 * CdsHdDPForex.AsFloat) / (CdsHdBaseForex.AsFloat - CdsHdDiscForex.AsFloat);
    CdsHDDP.AsFloat := (100 * CdsHdDPForex.AsFloat) / CdsHdTotalForex.AsFloat;
    CdsHdDP.OnChange := CdsHdDPChange;
  end;
end;

procedure TfmSO.dbfgDPClick(Sender: TObject);
begin
  inherited;
  If dbfgDP.Text = 'N' then begin
     CdsHdFgDP.AsString := 'N';
     CdsHdDP.AsFloat := 0;
     dbDP.Enabled := False;
     dbdp.Color := cl3DLight;
     dbDPForex.Enabled := False;
     dbDPForex.Color := cl3DLight;
  end else begin
     CdsHdFgDP.AsString := 'Y';
     dbDP.Enabled := True;
     dbdp.Color := clWhite;
     dbDPForex.Enabled := True;
     dbDPForex.Color := clWhite;     
  end;
  if dbDP.CanFocus then dbdp.SetFocus;
end;

procedure TfmSO.dbSalesKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  inherited;
  if (Key = vk_return) then PostMessage(Handle,WM_NEXTDLGCTL,0,0);
  if cdshd.state = dsbrowse then Exit;
  if (Shift = [ssCtrl]) and (Key=vk_return) then begin
    if GetSearchDlg('Select DISTINCT Sales_Code, Sales_Name From V_MsSalesman',
                'Sales_Code', 'Search Sales', 'Sales_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdSales.Asstring := StrSearch[0];
       end;
    end;
  end;
end;

procedure TfmSO.CdsHdSalesChange(Sender: TField);
begin
  inherited;
  cdsFindSales.Active := False;
  cdsFindSales.Data := Null;
  cdsFindSales.Params.ParamByName('@Code').AsString := CdsHdSales.AsString;
  try
    if not SetServerSettings then Exit;
    cdsFindSales.Active := True;
    if not cdsFindSales.IsEmpty then begin
      CdsHdSales_Name.AsString := cdsFindSalesSales_Name.AsString;
    end else begin
      CdsHdSales_Name.Clear;
    end;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;

end;

procedure TfmSO.btnSalesClick(Sender: TObject);
begin
  inherited;
  if cdshd.state = dsbrowse then Exit;
    if GetSearchDlg('Select DISTINCT Sales_Code, Sales_Name From V_MsSalesman',
                'Sales_Code', 'Search Sales', 'Sales_Code', '', '', '', '') then begin
       if length(StrSearch[0]) > 1 then begin
         CdsHdSales.Asstring := StrSearch[0];
       end;
    end;
end;

procedure TfmSO.CdsDtBeforeDelete(DataSet: TDataSet);
begin
 { IF StatusSJ ='P' Then Begin
    MessageDlg('SJ For Current SO is Posted, Cannot Delete data', mtConfirmation, [mbok],0);
    abort;
  end;}
  //LEPAS PENJAGAAN JNG CEK KE SJ
 //If CdsHdTransNmbr.AsString <> '' then begin
 If dbeCode.Text <> 'NEW' then begin
  cdsCekSJDt.Active := False;
  cdsCekSJDt.Data := Null;
  try
    cdsCekSJDt.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
    cdsCekSJDt.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
    if not SetServerSettings then Exit;
    cdsCekSJDt.Active := True;

  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
  if cdsCekSJDt.Fields.FieldByName('Qty').AsFloat > 0 then begin
    MessageDlg('SJ For Current SO is Posted or DO For Current SO is Posted, Cannot Delete data ', mtConfirmation, [mbok], 0);
    abort;
  end else begin
  end;
end;
{  IF StatusDO ='P' Then Begin
    MessageDlg('DO For Current SO is Posted, Cannot Delete data', mtConfirmation, [mbok],0);
    abort;
  end;}
    inherited;
end;

procedure TfmSO.CdsHdBillToChange(Sender: TField);
begin
  inherited;
  IF Trim(CdsHdBillTo.ASString) = '' then begin
    CdsHdCollect_Name.Clear;
    exit;
  end;

  {cdsFindBillTo.Active := False;
  cdsFindBillTo.Data := Null;
  cdsFindBillTo.Params.ParamByName('@Customer').ASString := CdsHdCustCode.Asstring;
  cdsFindBillTo.Params.ParamByName('@Code').ASString := CdsHdBillTo.AsString;
  cdsFindBillTo.Params.ParamByName('@Date').ASDateTime := CdsHdTransDate.AsDateTime;
  if not SetServerSettings then Exit;
  try
    cdsFindBillTo.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;

  CdsHdCollect_Name.Asstring := cdsFindBillToBill_To_Name.AsString;}


  cdsFindCust.Active := False;
  cdsFindCust.Data := Null;
  cdsFindCust.Params.ParamByName('@Code').AsString := CdsHdBillTo.AsString;
  if SOType = 'Project' then begin
    cdsFindCust.Params.ParamByName('@Type').AsString := '';
  end else begin
    cdsFindCust.Params.ParamByName('@Type').AsString := UpperCase(SOType);
  end;
  cdsFindCust.Params.ParamByName('@Date').AsDate := CdsHdTransDate.AsDateTime;
  if not SetServerSettings then Exit;
  try
    cdsFindCust.Active := True;
    if not cdsFindCust.IsEmpty then begin
      CdsHdCollect_Name.AsString := cdsFindCust.fieldByName('CustName').AsString;
    end else begin
      CdsHdCollect_Name.Clear;
    end;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
end;

procedure TfmSO.CdsHdFgPPNChange(Sender: TField);
begin
  inherited;
  {If CdsHdFgPPN.AsString ='Y' then begin
    CdsHdPPN.AsFloat := 10;
    dbePPN.Enabled := True;
    dbePPN.Color := clWhite;
  end else begin
    CdsHdPPN.AsFloat := 0;
    dbePPN.Enabled := False;
    dbePPN.Color := cl3DLight;
  end;}
end;

procedure TfmSO.CdsHdTotalForexChange(Sender: TField);
begin
  inherited;
  IF CdsHdLCurrDigit.AsString <> '' Then Begin
    CdsHdTotalForex.OnChange := nil;
    CdsHdTotalForex.AsFloat := RoundDecimal(CdsHdTotalForex.AsFloat, CdsHdLCurrDigit.AsInteger);
    CdsHdTotalForex.OnChange := CdsHdTotalForexChange;
  end;
  IF CdsHdFgDP.AsString ='Y' Then Begin
    CdsHdDPChange(nil);
  end;
end;

procedure TfmSO.DBComboBox2Change(Sender: TObject);
begin
  inherited;
  DBComboBox2.OnChange := nil;
  CdsHdFgCBD.AsString := DBComboBox2.Text;
  DBComboBox2.OnChange := DBComboBox2.OnChange;
  If DBComboBox2.Text ='Y' Then Begin
    CdsHdTerm.Clear;
    CdsHdDueDate.Clear;
    dblTerm.Enabled := FAlse;
    dblTerm.Color := cl3DLight;
    CdsGetTerm.Filter := '';
    CdsGetTerm.Filtered := False;
  end else begin
    dblTerm.Enabled := True;
    dblTerm.Color := clWindow;
  end;

  IF CdsHdFgCBD.AsString = 'Y' Then Begin
    cdsCekCBD.Active := False;
    cdsCekCBD.Data := Null;
    if not SetServerSettings then Exit;
    try
      cdsCekCBD.Active := True;
    except
      on E:Exception do begin
        MessageDlg(E.Message, mtError, [mbok], 0);
        Exit;
      end;
    end;
     IF not CdsCekCbd.Isempty Then
       CdsHdTerm.AsString := cdsCekCBDTermCode.AsString;
  end;
  
end;

procedure TfmSO.CdsMainNewRecord(DataSet: TDataSet);
begin
  inherited;
  CdsMainCustCode.AsString := CdsHdCustCode.AsString;
  CdsMainUserDate.ASDateTime := Date;
  CdsMainUserId.AsString := SecurityRec.Currency;
end;

procedure TfmSO.CdsMainAfterPost(DataSet: TDataSet);
begin
  inherited;
  if not SetServerSettings then Exit;
  if TClientDataSet(DataSet).ApplyUpdates(1) > 0 then Exit;
    TClientDataSet(DataSet).MergeChangeLog;
end;

procedure TfmSO.CdsMainAfterDelete(DataSet: TDataSet);
begin
  inherited;
  if not SetServerSettings then Exit;
  if TClientDataSet(DataSet).ApplyUpdates(1) > 0 then Exit;
    TClientDataSet(DataSet).MergeChangeLog;
end;

procedure TfmSO.DBGrid1DblClick(Sender: TObject);
begin
  inherited;
    CdsHdDeliveryTo.Asstring := CdsMainDeliveryCode.AsString;
    CdsHdDeliveryName.AsString := CdsMainDeliveryName.AsString;
    CdsHdDeliveryAddr1.AsString := CdsMainDeliveryAddr1.AsString;
    CdsHdDeliveryAddr2.AsString := CdsMainDeliveryAddr2.AsString;
    PNLcust.Visible := False;
    PcForm.Enabled := True;
end;

procedure TfmSO.CdsMainBeforePost(DataSet: TDataSet);
begin
  inherited;
  if Trim(CdsMainDeliveryCode.AsString) = '' then begin
    MessageDlg('Delivery must have value', mtConfirmation, [mbok], 0);
    DBGrid1.Fields[0].FocusControl;
    Abort;
  end;
  if Trim(CdsMainDeliveryName.AsString) = '' then begin
    MessageDlg('Delivery NAme have value', mtConfirmation, [mbok], 0);
    DBGrid1.Fields[0].FocusControl;
    Abort;
  end;
end;

procedure TfmSO.Label47Click(Sender: TObject);
begin
  inherited;
    PNLcust.Visible := False;
    PcForm.Enabled := True;
end;

procedure TfmSO.CdsDtAmountChange(Sender: TField);
begin
  inherited;
  IF CdsHdLCurrDigit.AsString <> '' Then Begin
    CdsDtAmount.OnChange := nil;
    CdsDtAmount.AsFloat := RoundDecimal(CdsDtAmount.AsFloat, CdsHdLCurrDigit.AsInteger);
    CdsDtAmount.OnChange := CdsDtAmountChange;
  end;

end;

procedure TfmSO.ShowInfoStock;
begin
  cdsGetInfoStock.Active := False;
  cdsGetInfoStock.Data := Null;
  cdsGetInfoStock.Params.ParamByName('@Date').AsDateTime := CdsHdTransDate.AsDateTime;
  cdsGetInfoStock.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
  if not SetServerSettings then Exit;
  try
    cdsGetInfoStock.Active := True;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;

  PNLinfo.Visible := True;
  lbwip.Caption := FormatFloat('###,##0.##', cdsGetInfoStockOnWIP.AsFloat) + ' + ' + FormatFloat('###,##0.##', cdsGetInfoStockOnWrhs.AsFloat);
end;

procedure TfmSO.CdsDtBeforeEdit(DataSet: TDataSet);
begin
  inherited;
 //LEPAS PENJAGAAN JNG CEK KE SJ
 {If CdsHdTransNmbr.AsString <> '' then begin
  cdsCekSJDt.Active := False;
  cdsCekSJDt.Data := Null;
  try
    cdsCekSJDt.Params.ParamByName('@SONo').AsString := CdsHdTransNmbr.AsString;
    cdsCekSJDt.Params.ParamByName('@Product').AsString := CdsDtProductCode.AsString;
    if not SetServerSettings then Exit;
    cdsCekSJDt.Active := True;
      if cdsCekSJDt.Fields.FieldByName('Qty').AsFloat > 0 then begin
        MessageDlg('SJ For Current SO is Posted, Cannot Delete data ', mtConfirmation, [mbok], 0);
        abort;
      end;
  except
    on E:Exception do begin
      MessageDlg(E.Message, mtError, [mbok], 0);
      Exit;
    end;
  end;
 end;  }
end;

procedure TfmSO.CdsDtAfterDelete(DataSet: TDataSet);
begin
  inherited;
     if CdsDtAmount.AsFloat <= 0 then begin
     CdsHdBaseForex.Value := 0;
    end else begin
     CdsHdBaseForex.Value := CdsDtCAmount.Value;
    end;

end;

end.
