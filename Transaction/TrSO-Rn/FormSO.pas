unit FormSO;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  Db, DBClient, ExtCtrls, QuickRpt, Qrctrls, qrprntr, jpeg, PsQrCtrls, ClientShare;

type
  TFmFormSO = class(TForm)
    cdsForm: TClientDataSet;
    QuickRep1: TQuickRep;
    QRBand1: TQRBand;
    QRDBText1: TQRDBText;
    QRDBText6: TQRDBText;
    PageFooterBand1: TQRBand;
    PsQRShape10: TPsQRShape;
    PsQRShape14: TPsQRShape;
    QrlNo: TPsQRLabel;
    QRDBText7: TQRDBText;
    PsQRShape15: TPsQRShape;
    QRDBText3: TQRDBText;
    QRLabel12: TQRLabel;
    QRDBText5: TQRDBText;
    PsQRShape13: TPsQRShape;
    QRLabel29: TQRLabel;
    QRDBText8: TQRDBText;
    PsQRShape16: TPsQRShape;
    QRLabel30: TQRLabel;
    QRDBText9: TQRDBText;
    PsQRShape17: TPsQRShape;
    QRLabel31: TQRLabel;
    QRDBText17: TQRDBText;
    PsQRShape18: TPsQRShape;
    QRLabel32: TQRLabel;
    QRDBText18: TQRDBText;
    QRLabel33: TQRLabel;
    QRDBText19: TQRDBText;
    QRTotal: TQRLabel;
    PsQRShape21: TPsQRShape;
    PsQRShape20: TPsQRShape;
    QRLabel34: TQRLabel;
    QRLabel35: TQRLabel;
    QRLabel36: TQRLabel;
    QRLabel37: TQRLabel;
    PsQRShape23: TPsQRShape;
    PsQRShape24: TPsQRShape;
    QRLabel10: TQRLabel;
    QRLabel11: TQRLabel;
    QRLabel38: TQRLabel;
    QRLabel49: TQRLabel;
    QRLabel51: TQRLabel;
    PsQRShape11: TPsQRShape;
    PsQRShape19: TPsQRShape;
    PsQRShape22: TPsQRShape;
    PsQRShape28: TPsQRShape;
    PsQRShape9: TPsQRShape;
    PageHeaderBand1: TQRBand;
    QRLabel19: TQRLabel;
    PsQRShape2: TPsQRShape;
    PsQRShape4: TPsQRShape;
    QRLabel16: TQRLabel;
    QRImageCompany: TPsQRImage;
    QrlCompany: TPsQRLabel;
    QrlCompLineA: TQRLabel;
    QrlCompLineB: TQRLabel;
    QRLabel23: TQRLabel;
    QRDBText14: TQRDBText;
    QRLabel26: TQRLabel;
    QRLabel27: TQRLabel;
    QRLabel28: TQRLabel;
    QRLabel14: TQRLabel;
    PsQRShape25: TPsQRShape;
    QRLabel15: TQRLabel;
    QRLabel17: TQRLabel;
    QRDBText10: TQRDBText;
    QRDBText11: TQRDBText;
    QRLabel18: TQRLabel;
    QRLabel21: TQRLabel;
    QRLabel22: TQRLabel;
    QRLabel24: TQRLabel;
    QRLabel39: TQRLabel;
    QRLabel43: TQRLabel;
    QRLabel44: TQRLabel;
    QRLabel45: TQRLabel;
    QRLabel47: TQRLabel;
    QRLabel48: TQRLabel;
    QRLabel3: TQRLabel;
    QRLabel4: TQRLabel;
    QRLabel5: TQRLabel;
    QRLabel6: TQRLabel;
    QRLabel8: TQRLabel;
    PsQRShape3: TPsQRShape;
    QRShape2: TQRShape;
    PsQRShape6: TPsQRShape;
    PsQRShape26: TPsQRShape;
    PsQRShape27: TPsQRShape;
    QRShape3: TQRShape;
    QRShape1: TQRShape;
    QRShape4: TQRShape;
    QRLabel41: TQRLabel;
    QRDBText4: TQRDBText;
    lbPKP: TQRLabel;
    lbNPKP: TQRLabel;
    QRDBText13: TQRDBText;
    QRDBText15: TQRDBText;
    QRDBText16: TQRDBText;
    QRDBText20: TQRDBText;
    QRDBText22: TQRDBText;
    QRDBText2: TQRDBText;
    QRDBText12: TQRDBText;
    QRDBText21: TQRDBText;
    QRLabel1: TQRLabel;
    QRDBText23: TQRDBText;
    PsQRShape1: TPsQRShape;
    PsQRShape5: TPsQRShape;
    QRDBText24: TQRDBText;
    QRDBText25: TQRDBText;
    QRDBText26: TQRDBText;
    QRDBText27: TQRDBText;
    QRDBText28: TQRDBText;
    QRLabel2: TQRLabel;
    cdsFormTransNmbr: TStringField;
    cdsFormTransDate: TDateTimeField;
    cdsFormCustomer: TStringField;
    cdsFormAttn: TStringField;
    cdsFormTelp: TStringField;
    cdsFormAlamat: TStringField;
    cdsFormFgPPN: TStringField;
    cdsFormNPWP: TStringField;
    cdsFormCustPONo: TStringField;
    cdsFormCustPODate: TDateTimeField;
    cdsFormDeliveryDate: TDateTimeField;
    cdsFormDeliveryAddr: TStringField;
    cdsFormTermPayment: TStringField;
    cdsFormTerm: TStringField;
    cdsFormRemark: TStringField;
    cdsFormBaseForex: TFloatField;
    cdsFormDiscForex: TFloatField;
    cdsFormPPNForex: TFloatField;
    cdsFormTotalForex: TFloatField;
    cdsFormProduct: TStringField;
    cdsFormQty: TFloatField;
    cdsFormUnit: TStringField;
    cdsFormPrice: TFloatField;
    cdsFormAmount: TFloatField;
    cdsFormCity: TStringField;
    cdsFormCurrency: TStringField;
    cdsFormPPN: TFloatField;
    cdsFormBaseDiscForex: TFloatField;
    QRSysData1: TQRSysData;
    QRLPage: TQRLabel;
    QRDBSales: TQRDBText;
    cdsFormSales: TStringField;
    procedure QRBand1BeforePrint(Sender: TQRCustomBand;
      var PrintBand: Boolean);
    procedure QuickRep1BeforePrint(Sender: TCustomQuickRep;
      var PrintReport: Boolean);
    procedure QuickRep1Preview(Sender: TObject);
  private
    { Private declarations }
    Nomor : Integer;
  public
    { Public declarations }
    Currency, CurrCode : String;
    SecurityRpt : TSecurityRec;
  end;

var
  FmFormSO: TFmFormSO;

implementation

uses USO, Converter, PreviewRpt;

{$R *.DFM}

procedure TFmFormSO.QRBand1BeforePrint(Sender: TQRCustomBand;
  var PrintBand: Boolean);
begin
   QrlNo.Caption := INTTostr(nomor);
   Nomor := Nomor + 1;
end;

procedure TFmFormSO.QuickRep1BeforePrint(Sender: TCustomQuickRep;
  var PrintReport: Boolean);
begin
   nomor := 1;
   IF cdsFormCurrency.AsString = CurrCode Then
     QRTotal.Caption := RupiahToWord(cdsFormTotalForex.AsFloat) + ' ' + Currency
   else
     QRTotal.Caption := DollarToWord(CdsFormTotalForex.AsFloat) + ' ' + Currency;
   lbPKP.Enabled := cdsFormFgPPN.ASString ='Y';
   lbNPkp.Enabled := Not(lbPKP.Enabled);
end;

procedure TFmFormSO.QuickRep1Preview(Sender: TObject);
begin
  Application.CreateForm(TfmPreviewRpt, fmPreviewRpt);
  with fmPreviewRpt do begin
    Caption := QuickRep1.ReportTitle;
    SecurityPrv := SecurityRpt;
    pQuickReport := QuickRep1;
    QRPreview1.QRPrinter := TQRPrinter(Sender);
    Show;
  end;
end;

end.
