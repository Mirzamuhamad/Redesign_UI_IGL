inherited fmSO: TfmSO
  Left = 213
  Top = 84
  Caption = 'Sales Order'
  ClientHeight = 536
  ClientWidth = 756
  OldCreateOrder = True
  PixelsPerInch = 96
  TextHeight = 13
  inherited pnlHeader: TPanel
    Width = 756
    inherited FrameHeader1: TFrameHeader
      Width = 756
      inherited pnlKanan: TPanel
        Width = 520
        inherited Label1: TLabel
          Left = 345
        end
        inherited lblCurrency: TLabel
          Left = 433
        end
        inherited Label6: TLabel
          Left = 345
        end
        inherited lblRate: TLabel
          Left = 433
        end
      end
      inherited pnlTop: TPanel
        Width = 756
        inherited lblCompany: TLabel
          Width = 756
        end
      end
    end
  end
  inherited pnlBottom: TPanel
    Top = 499
    Width = 756
    inherited pnlButtons: TPanel
      Left = 250
    end
  end
  inherited PcForm: TPageControl
    Width = 756
    Height = 446
    inherited tsEntry: TTabSheet
      inherited pnlHd: TPanel
        Width = 748
        Height = 275
        inherited dbtStatus: TDBText
          Left = 608
          DataField = 'FgStatus'
        end
        object Label7: TLabel [1]
          Left = 2
          Top = 5
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Date :'
        end
        object Label13: TLabel [2]
          Left = 270
          Top = 5
          Width = 100
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Report :'
        end
        object Label14: TLabel [3]
          Left = 2
          Top = 26
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Customer :'
        end
        object Label22: TLabel [4]
          Left = 443
          Top = 24
          Width = 4
          Height = 13
          Caption = '*'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clRed
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
        end
        object Label2: TLabel [5]
          Left = 2
          Top = 47
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Customer PO No :'
        end
        object Label32: TLabel [6]
          Left = 251
          Top = 47
          Width = 71
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'PO Date :'
        end
        object Label30: TLabel [7]
          Left = 2
          Top = 154
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Remark :'
        end
        object Label40: TLabel [8]
          Left = 2
          Top = 89
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Attn :'
        end
        object Label41: TLabel [9]
          Left = 2
          Top = 68
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Project :'
        end
        object Label42: TLabel [10]
          Left = 2
          Top = 110
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Product Group :'
        end
        object Label36: TLabel [11]
          Left = 203
          Top = 4
          Width = 4
          Height = 13
          Caption = '*'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clRed
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
        end
        object Label37: TLabel [12]
          Left = 312
          Top = 109
          Width = 4
          Height = 13
          Caption = '*'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clRed
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
        end
        object Label17: TLabel [13]
          Left = 2
          Top = 132
          Width = 94
          Height = 13
          Alignment = taRightJustify
          AutoSize = False
          Caption = 'Sales :'
        end
        inherited btnGetItem: TBitBtn
          Left = 682
          Top = 26
          Enabled = False
          TabOrder = 16
        end
        object dbeDate: TDBDateEdit
          Tag = 2
          Left = 100
          Top = 1
          Width = 100
          Height = 21
          DataField = 'TransDate'
          DataSource = dsHd
          NumGlyphs = 2
          TabOrder = 0
          OnKeyDown = FormKeyDown
        end
        object cbxFgReport: TDBComboBox
          Tag = 2
          Left = 375
          Top = 1
          Width = 51
          Height = 19
          Style = csOwnerDrawFixed
          DataField = 'FgReport'
          DataSource = dsHd
          ItemHeight = 13
          Items.Strings = (
            'Y'
            'N')
          TabOrder = 1
          OnKeyDown = FormKeyDown
        end
        object dbeCustCode: TDBEdit
          Tag = 2
          Left = 100
          Top = 22
          Width = 100
          Height = 21
          DataField = 'CustCode'
          DataSource = dsHd
          TabOrder = 2
          OnKeyDown = dbeCustCodeKeyDown
        end
        object dbeCustName: TDBEdit
          Tag = 2
          Left = 200
          Top = 22
          Width = 226
          Height = 21
          Color = cl3DLight
          DataField = 'CustName'
          DataSource = dsHd
          Enabled = False
          TabOrder = 3
          OnKeyDown = FormKeyDown
        end
        object dbeCustPONo: TDBEdit
          Tag = 2
          Left = 100
          Top = 43
          Width = 133
          Height = 21
          DataField = 'CustPONo'
          DataSource = dsHd
          TabOrder = 5
          OnKeyDown = FormKeyDown
        end
        object PcHdInfo: TPageControl
          Left = 0
          Top = 173
          Width = 748
          Height = 102
          ActivePage = tsNominal
          Align = alBottom
          TabOrder = 17
          object tsShipment: TTabSheet
            Caption = 'Shipment'
            OnExit = tsShipmentExit
            object Label33: TLabel
              Left = -14
              Top = 6
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Delivery To :'
            end
            object Label34: TLabel
              Left = 450
              Top = 54
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Delivery Date :'
            end
            object Label25: TLabel
              Left = -14
              Top = 30
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Delivery Addr 1'
            end
            object Label39: TLabel
              Left = 435
              Top = 7
              Width = 108
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Delivery Type :'
            end
            object Label44: TLabel
              Left = 548
              Top = 29
              Width = 158
              Height = 13
              Caption = 'Loco = Diambil,  Franco = Diantar'
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clRed
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentFont = False
            end
            object Label45: TLabel
              Left = -14
              Top = 54
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Delivery Addr 2'
            end
            object DBEdit9: TDBEdit
              Tag = 2
              Left = 84
              Top = 2
              Width = 100
              Height = 21
              DataField = 'DeliveryTo'
              DataSource = dsHd
              ReadOnly = True
              TabOrder = 1
              OnKeyDown = DBEdit9KeyDown
            end
            object DBEdit17: TDBEdit
              Tag = 2
              Left = 184
              Top = 2
              Width = 264
              Height = 21
              Color = cl3DLight
              DataField = 'DeliveryName'
              DataSource = dsHd
              Enabled = False
              TabOrder = 2
              OnKeyDown = FormKeyDown
            end
            object DBDateEdit3: TDBDateEdit
              Tag = 2
              Left = 547
              Top = 50
              Width = 102
              Height = 21
              DataField = 'DeliveryDate'
              DataSource = dsHd
              NumGlyphs = 2
              TabOrder = 5
              OnKeyDown = FormKeyDown
            end
            object BitBtn1: TBitBtn
              Tag = 2
              Left = 449
              Top = 3
              Width = 14
              Height = 20
              Enabled = False
              TabOrder = 3
              OnClick = BitBtn1Click
              Glyph.Data = {
                CE000000424DCE000000000000007600000028000000090000000B0000000100
                0400000000005800000000000000000000001000000000000000000000000000
                BF0000BF000000BFBF00BF000000BF00BF00BFBF0000C0C0C000808080000000
                FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00777777777300
                0000700000007300000077777777780000007777077773000000777000777000
                0000770000077000000070000000730000007770007778000000777000777000
                000077700077770000007777777770000000}
            end
            object DBEdit2: TDBEdit
              Tag = 2
              Left = 84
              Top = 25
              Width = 364
              Height = 21
              DataField = 'DeliveryAddr1'
              DataSource = dsHd
              TabOrder = 4
              OnKeyDown = FormKeyDown
            end
            object DBComboBox1: TDBComboBox
              Tag = 2
              Left = 547
              Top = 3
              Width = 103
              Height = 21
              Style = csDropDownList
              DataField = 'DeliveryType'
              DataSource = dsHd
              ItemHeight = 13
              Items.Strings = (
                'DIAMBIL'
                'DIANTAR-BP'
                'DIANTAR-BC')
              TabOrder = 0
              OnKeyDown = FormKeyDown
            end
            object DBEdit6: TDBEdit
              Tag = 2
              Left = 84
              Top = 49
              Width = 364
              Height = 21
              DataField = 'DeliveryAddr2'
              DataSource = dsHd
              TabOrder = 6
              OnKeyDown = FormKeyDown
            end
          end
          object tsBillTo: TTabSheet
            Caption = 'Bill To'
            ImageIndex = 2
            OnExit = tsBillToExit
            object Label31: TLabel
              Left = 2
              Top = 7
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Bill To :'
            end
            object Label15: TLabel
              Left = 402
              Top = 30
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Term :'
            end
            object Label3: TLabel
              Left = 402
              Top = 53
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Due Date :'
            end
            object Label8: TLabel
              Left = 1
              Top = 51
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Term Condition :'
            end
            object Label35: TLabel
              Left = 1
              Top = 29
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'CBD :'
            end
            object dbeBillTo: TDBEdit
              Tag = 2
              Left = 100
              Top = 3
              Width = 100
              Height = 21
              DataField = 'BillTo'
              DataSource = dsHd
              TabOrder = 0
              OnKeyDown = dbeBillToKeyDown
            end
            object DBEdit16: TDBEdit
              Tag = 2
              Left = 200
              Top = 3
              Width = 226
              Height = 21
              Color = cl3DLight
              DataField = 'Collect_Name'
              DataSource = dsHd
              Enabled = False
              TabOrder = 1
              OnKeyDown = FormKeyDown
            end
            object dblTerm: TDBLookupComboBox
              Tag = 2
              Left = 500
              Top = 26
              Width = 168
              Height = 21
              Color = cl3DLight
              DataField = 'LTerm'
              DataSource = dsHd
              Enabled = False
              TabOrder = 5
              OnKeyDown = FormKeyDown
            end
            object DBDateEdit1: TDBDateEdit
              Tag = 2
              Left = 500
              Top = 49
              Width = 100
              Height = 21
              DataField = 'DueDate'
              DataSource = dsHd
              Color = cl3DLight
              Enabled = False
              NumGlyphs = 2
              TabOrder = 6
              OnKeyDown = FormKeyDown
            end
            object BitBtn2: TBitBtn
              Tag = 2
              Left = 427
              Top = 4
              Width = 14
              Height = 23
              Enabled = False
              TabOrder = 2
              OnClick = BitBtn2Click
              Glyph.Data = {
                CE000000424DCE000000000000007600000028000000090000000B0000000100
                0400000000005800000000000000000000001000000000000000000000000000
                BF0000BF000000BFBF00BF000000BF00BF00BFBF0000C0C0C000808080000000
                FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00777777777300
                0000700000007300000077777777780000007777077773000000777000777000
                0000770000077000000070000000730000007770007778000000777000777000
                000077700077770000007777777770000000}
            end
            object DBEdit1: TDBEdit
              Tag = 2
              Left = 100
              Top = 47
              Width = 213
              Height = 21
              DataField = 'TermPayment'
              DataSource = dsHd
              TabOrder = 4
              OnKeyDown = FormKeyDown
            end
            object DBComboBox2: TDBComboBox
              Tag = 2
              Left = 100
              Top = 25
              Width = 52
              Height = 22
              Style = csOwnerDrawFixed
              DataField = 'FgCBD'
              DataSource = dsHd
              ItemHeight = 16
              Items.Strings = (
                'Y'
                'N')
              TabOrder = 3
              OnChange = DBComboBox2Change
              OnKeyDown = FormKeyDown
            end
          end
          object tsNominal: TTabSheet
            Caption = 'SO Nominal'
            ImageIndex = 3
            object Shape7: TShape
              Left = 312
              Top = 38
              Width = 100
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Shape6: TShape
              Left = 554
              Top = 38
              Width = 100
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Shape2: TShape
              Left = 161
              Top = 38
              Width = 100
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Label18: TLabel
              Left = 10
              Top = 60
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Amount :'
            end
            object Label19: TLabel
              Left = 165
              Top = 41
              Width = 92
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'Base Forex'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label21: TLabel
              Left = 556
              Top = 41
              Width = 92
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'Total Forex'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label27: TLabel
              Left = 314
              Top = 41
              Width = 92
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'Discount Forex'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Shape8: TShape
              Left = 108
              Top = 38
              Width = 53
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Label28: TLabel
              Left = 112
              Top = 41
              Width = 48
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'Curr.'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Shape4: TShape
              Left = 554
              Top = 0
              Width = 100
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Shape3: TShape
              Left = 506
              Top = 0
              Width = 50
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Shape1: TShape
              Left = 452
              Top = 0
              Width = 54
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Label20: TLabel
              Left = 456
              Top = 3
              Width = 46
              Height = 12
              Alignment = taCenter
              AutoSize = False
              Caption = 'DP'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label23: TLabel
              Left = 511
              Top = 3
              Width = 41
              Height = 12
              Alignment = taCenter
              AutoSize = False
              Caption = '% DP'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label29: TLabel
              Left = 556
              Top = 3
              Width = 92
              Height = 12
              Alignment = taCenter
              AutoSize = False
              Caption = 'DP Forex'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Shape5: TShape
              Left = 453
              Top = 38
              Width = 100
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Shape9: TShape
              Left = 413
              Top = 38
              Width = 41
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Label24: TLabel
              Left = 414
              Top = 41
              Width = 38
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'PPN %'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label26: TLabel
              Left = 455
              Top = 41
              Width = 92
              Height = 11
              Alignment = taCenter
              AutoSize = False
              Caption = 'PPN Forex'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Shape12: TShape
              Left = 262
              Top = 38
              Width = 50
              Height = 16
              Brush.Color = clTeal
              Pen.Color = clMaroon
            end
            object Label38: TLabel
              Left = 267
              Top = 41
              Width = 41
              Height = 12
              Alignment = taCenter
              AutoSize = False
              Caption = 'Disc%'
              Color = clTeal
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clWhite
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentColor = False
              ParentFont = False
            end
            object Label16: TLabel
              Left = 10
              Top = 19
              Width = 94
              Height = 13
              Alignment = taRightJustify
              AutoSize = False
              Caption = 'Currency / Rate :'
            end
            object Label43: TLabel
              Left = 282
              Top = 17
              Width = 4
              Height = 13
              Caption = '*'
              Font.Charset = DEFAULT_CHARSET
              Font.Color = clRed
              Font.Height = -11
              Font.Name = 'MS Sans Serif'
              Font.Style = []
              ParentFont = False
            end
            object DBEdit8: TDBEdit
              Tag = 2
              Left = 161
              Top = 54
              Width = 100
              Height = 21
              Color = cl3DLight
              DataField = 'BaseForex'
              DataSource = dsHd
              Enabled = False
              TabOrder = 6
              OnKeyDown = FormKeyDown
            end
            object DBEdit10: TDBEdit
              Tag = 2
              Left = 312
              Top = 54
              Width = 100
              Height = 21
              Color = clWhite
              DataField = 'DiscForex'
              DataSource = dsHd
              TabOrder = 8
              OnKeyDown = FormKeyDown
            end
            object DBEdit11: TDBEdit
              Tag = 2
              Left = 554
              Top = 54
              Width = 100
              Height = 21
              Color = cl3DLight
              DataField = 'TotalForex'
              DataSource = dsHd
              Enabled = False
              TabOrder = 11
              OnKeyDown = FormKeyDown
            end
            object DBEdit5: TDBEdit
              Tag = 2
              Left = 108
              Top = 54
              Width = 52
              Height = 21
              Color = cl3DLight
              DataField = 'CurrCode'
              DataSource = dsHd
              Enabled = False
              TabOrder = 5
              OnKeyDown = FormKeyDown
            end
            object dbDPForex: TDBEdit
              Tag = 2
              Left = 554
              Top = 15
              Width = 100
              Height = 21
              Color = clWhite
              DataField = 'DPForex'
              DataSource = dsHd
              TabOrder = 4
              OnKeyDown = FormKeyDown
            end
            object dbePPN: TDBEdit
              Tag = 2
              Left = 413
              Top = 54
              Width = 40
              Height = 21
              Color = clWhite
              DataField = 'PPN'
              DataSource = dsHd
              TabOrder = 9
              OnKeyDown = FormKeyDown
            end
            object DBEdit3: TDBEdit
              Tag = 2
              Left = 453
              Top = 54
              Width = 100
              Height = 21
              Color = cl3DLight
              DataField = 'PPNForex'
              DataSource = dsHd
              Enabled = False
              TabOrder = 10
              OnKeyDown = FormKeyDown
            end
            object dbfgDP: TDBComboBox
              Tag = 2
              Left = 453
              Top = 15
              Width = 52
              Height = 22
              Style = csOwnerDrawFixed
              DataField = 'FgDP'
              DataSource = dsHd
              ItemHeight = 16
              Items.Strings = (
                'Y'
                'N')
              TabOrder = 2
              OnClick = dbfgDPClick
              OnKeyDown = FormKeyDown
            end
            object dbDP: TDBEdit
              Tag = 2
              Left = 504
              Top = 15
              Width = 50
              Height = 21
              DataField = 'DP'
              DataSource = dsHd
              TabOrder = 3
              OnKeyDown = FormKeyDown
            end
            object DBEdit14: TDBEdit
              Tag = 2
              Left = 260
              Top = 54
              Width = 50
              Height = 21
              DataField = 'Disc'
              DataSource = dsHd
              TabOrder = 7
              OnKeyDown = FormKeyDown
            end
            object dblCurrency: TDBLookupComboBox
              Tag = 2
              Left = 108
              Top = 15
              Width = 76
              Height = 21
              DataField = 'LCurrency'
              DataSource = dsHd
              TabOrder = 0
              OnKeyDown = FormKeyDown
            end
            object dbeRate: TDBEdit
              Tag = 2
              Left = 183
              Top = 15
              Width = 96
              Height = 21
              DataField = 'ForexRate'
              DataSource = dsHd
              TabOrder = 1
              OnKeyDown = FormKeyDown
            end
          end
        end
        object DBEdit4: TDBEdit
          Tag = 2
          Left = 100
          Top = 150
          Width = 341
          Height = 21
          DataField = 'Remark'
          DataSource = dsHd
          TabOrder = 15
          OnKeyDown = FormKeyDown
        end
        object dbeAttn: TDBEdit
          Tag = 2
          Left = 100
          Top = 85
          Width = 210
          Height = 21
          DataField = 'Attn'
          DataSource = dsHd
          TabOrder = 10
          OnKeyDown = FormKeyDown
        end
        object dbeCustPODate: TDBDateEdit
          Tag = 2
          Left = 326
          Top = 43
          Width = 100
          Height = 21
          DataField = 'CustPODate'
          DataSource = dsHd
          NumGlyphs = 2
          TabOrder = 6
          OnKeyDown = FormKeyDown
        end
        object dbeProjectCode: TDBEdit
          Left = 100
          Top = 64
          Width = 165
          Height = 21
          DataField = 'ProjectCode'
          DataSource = dsHd
          ReadOnly = True
          TabOrder = 7
          OnKeyDown = dbeProjectCodeKeyDown
        end
        object dblProductGroup: TDBLookupComboBox
          Tag = 2
          Left = 100
          Top = 106
          Width = 210
          Height = 21
          DataField = 'LProductGroup'
          DataSource = dsHd
          TabOrder = 11
          OnKeyDown = FormKeyDown
        end
        object dbeProjectName: TDBEdit
          Tag = 2
          Left = 266
          Top = 64
          Width = 257
          Height = 21
          Color = cl3DLight
          DataField = 'ProjectName'
          DataSource = dsHd
          Enabled = False
          TabOrder = 8
          OnKeyDown = FormKeyDown
        end
        object btnProject: TBitBtn
          Left = 525
          Top = 65
          Width = 14
          Height = 20
          Enabled = False
          TabOrder = 9
          OnClick = btnProjectClick
          Glyph.Data = {
            CE000000424DCE000000000000007600000028000000090000000B0000000100
            0400000000005800000000000000000000001000000000000000000000000000
            BF0000BF000000BFBF00BF000000BF00BF00BFBF0000C0C0C000808080000000
            FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00777777777300
            0000700000007300000077777777780000007777077773000000777000777000
            0000770000077000000070000000730000007770007778000000777000777000
            000077700077770000007777777770000000}
        end
        object btnCust: TBitBtn
          Tag = 2
          Left = 427
          Top = 22
          Width = 14
          Height = 20
          Enabled = False
          TabOrder = 4
          OnClick = btnCustClick
          Glyph.Data = {
            CE000000424DCE000000000000007600000028000000090000000B0000000100
            0400000000005800000000000000000000001000000000000000000000000000
            BF0000BF000000BFBF00BF000000BF00BF00BFBF0000C0C0C000808080000000
            FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00777777777300
            0000700000007300000077777777780000007777077773000000777000777000
            0000770000077000000070000000730000007770007778000000777000777000
            000077700077770000007777777770000000}
        end
        object btnCloseItem: TBitBtn
          Left = 665
          Top = 153
          Width = 82
          Height = 25
          Anchors = [akTop, akRight]
          Caption = 'Closing Item'
          TabOrder = 18
          Visible = False
          OnClick = btnCloseItemClick
          Glyph.Data = {
            42020000424D4202000000000000420000002800000010000000100000000100
            1000030000000002000000000000000000000000000000000000007C0000E003
            00001F000000F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E003CF75EF75EF75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E003C003CF75EF75EF75E
            F75EF75E003CF75EF75EF75EF75EF75EF75EF75EF75E003C003C003CF75EF75E
            F75E003CF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E003C003CF75EF75E
            003C003CF75E00000000000000000000000000000000000000000000003C003C
            0F3CF75EF75E0000F75EF75EF75EF75EF75EF75EF75EF75EF75E0F3C003C003C
            F75EF75EF75E0000F75E00000000F75E00000000F75E0000F75E003C0F3C003C
            003CF75EF75E0000F75EF75EF75EF75EF75EF75EF75EF75E003C003CF75EF75E
            003C003C0F3C00000F000F000F000F000F000F000F3C003C0F000F000F000F00
            F75E0F3C003CF75E0F00FF7FFF7FFF7FFF7FFF7F003C0F3CFF7FFF7FFF7F0F00
            F75EF75EF75EF75E0F00FF7F00000000FF7F00000000FF7F00000000FF7F0F00
            F75EF75EF75EF75E0F00FF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7F0F00
            F75EF75EF75EF75E0F000F000F000F000F000F000F000F000F000F000F000F00
            F75EF75EF75EF75E0F00F75E0F000F00F75E0F000F00F75E0F000F00F75E0F00
            F75EF75EF75EF75E0F000F000F000F000F000F000F000F000F000F000F000F00
            F75EF75EF75E}
          Spacing = 1
        end
        object dbSales: TDBEdit
          Tag = 2
          Left = 100
          Top = 128
          Width = 101
          Height = 21
          DataField = 'Sales'
          DataSource = dsHd
          ReadOnly = True
          TabOrder = 12
          OnKeyDown = dbSalesKeyDown
        end
        object dbSalesName: TDBEdit
          Tag = 2
          Left = 200
          Top = 128
          Width = 226
          Height = 21
          Color = cl3DLight
          DataField = 'Sales_Name'
          DataSource = dsHd
          Enabled = False
          TabOrder = 13
          OnKeyDown = FormKeyDown
        end
        object btnSales: TBitBtn
          Tag = 2
          Left = 427
          Top = 128
          Width = 14
          Height = 20
          Enabled = False
          TabOrder = 14
          OnClick = btnSalesClick
          Glyph.Data = {
            CE000000424DCE000000000000007600000028000000090000000B0000000100
            0400000000005800000000000000000000001000000000000000000000000000
            BF0000BF000000BFBF00BF000000BF00BF00BFBF0000C0C0C000808080000000
            FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00777777777300
            0000700000007300000077777777780000007777077773000000777000777000
            0000770000077000000070000000730000007770007778000000777000777000
            000077700077770000007777777770000000}
        end
        object pnlInfo: TPanel
          Left = 445
          Top = 90
          Width = 193
          Height = 49
          Color = clSilver
          TabOrder = 19
          Visible = False
          object Label48: TLabel
            Left = 7
            Top = 4
            Width = 98
            Height = 12
            Alignment = taRightJustify
            AutoSize = False
            Caption = 'Qty On Book :'
            Transparent = True
          end
          object DBText1: TDBText
            Left = 107
            Top = 4
            Width = 42
            Height = 13
            AutoSize = True
            DataField = 'OnBook'
            DataSource = dsGetInfoStock
            Font.Charset = DEFAULT_CHARSET
            Font.Color = clBlue
            Font.Height = -11
            Font.Name = 'MS Sans Serif'
            Font.Style = []
            ParentFont = False
            Transparent = True
          end
          object Label50: TLabel
            Left = 7
            Top = 32
            Width = 98
            Height = 12
            Alignment = taRightJustify
            AutoSize = False
            Caption = 'Qty On Production :'
            Transparent = True
          end
          object DBText3: TDBText
            Left = 107
            Top = 32
            Width = 42
            Height = 13
            AutoSize = True
            DataField = 'OnProduction'
            DataSource = dsGetInfoStock
            Font.Charset = DEFAULT_CHARSET
            Font.Color = clBlue
            Font.Height = -11
            Font.Name = 'MS Sans Serif'
            Font.Style = []
            ParentFont = False
            Transparent = True
          end
          object Label51: TLabel
            Left = 7
            Top = 18
            Width = 98
            Height = 12
            Alignment = taRightJustify
            AutoSize = False
            Caption = 'Qty WIP + Wrhs :'
            Transparent = True
          end
          object lbWIP: TLabel
            Left = 107
            Top = 18
            Width = 83
            Height = 13
            Caption = 'Qty WIP + Wrhs :'
            Font.Charset = DEFAULT_CHARSET
            Font.Color = clBlue
            Font.Height = -11
            Font.Name = 'MS Sans Serif'
            Font.Style = []
            ParentFont = False
            Transparent = True
          end
        end
      end
      inherited Panel3: TPanel
        Width = 748
        inherited Bevel1: TBevel
          Width = 748
        end
        inherited BtnInsert: TBitBtn
          Left = 4
        end
        inherited btnGetAppr: TBitBtn
          Left = 495
          TabOrder = 6
        end
        inherited btnPost: TBitBtn
          Left = 562
          TabOrder = 7
        end
        inherited btnUnPost: TBitBtn
          Left = 624
          TabOrder = 8
        end
        inherited btnFind: TBitBtn
          Left = 686
          TabOrder = 9
        end
        inherited dbeCode: TEdit
          Enabled = False
        end
        object CbxRevisi: TComboBox
          Left = 348
          Top = 2
          Width = 48
          Height = 21
          Style = csDropDownList
          ItemHeight = 13
          TabOrder = 4
          OnChange = CbxRevisiChange
          Items.Strings = (
            '0')
        end
        object btnCreateRevisi: TBitBtn
          Left = 417
          Top = 2
          Width = 78
          Height = 21
          Hint = 'Get item data from reference'
          Anchors = [akTop, akRight]
          Caption = '&New Revisi'
          Enabled = False
          ParentShowHint = False
          ShowHint = True
          TabOrder = 5
          OnClick = btnCreateRevisiClick
          Glyph.Data = {
            42020000424D4202000000000000420000002800000010000000100000000100
            1000030000000002000000000000000000000000000000000000007C0000E003
            00001F000000F75EF75EF75EF75EF75EF75EF75EF75E0000F75EF75EF75EF75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E00000000F75EF75EF75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E0000E07F0000F75EF75E
            F75EF75EF75EF75EF75EF75EF75E00000000000000000000E07FE07F0000F75E
            F75EF75EF75EF75EF75EF75EF75E0000E07FE07FE07FE07FE07FE07FE07F0000
            F75EF75EF75EF75EF75EF75EF75E0000E07FE07FE07FE07FE07FE07FE07FE07F
            0000F75EF75EF75EF75EF75EF75E0000E07FE07FE07FE07FE07FE07FE07F0000
            F75EF75EF75EF75EF75EF75EF75E00000000000000000000E07FE07F0000F75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E0000E07F0000F75EF75E
            F75EF75EF75EF75EF75EF75EF75EF75EF75EF75EF75E00000000F75EF75EF75E
            F75EF75EF75EF75EF75EF75EF75EEF3D0F00F75EF75E0000F75EF75EF75EF75E
            F75EF75EF75EF75EF75EF75EEF3D0F00F75EF75EF75EF75E0F000F000F000F00
            F75EF75EF75EF75EF75EF75E0F00F75EF75EF75EF75EF75EF75E0F000F000F00
            F75EF75EF75EF75EF75EF75E0F00F75EF75EF75EF75EF75EF75E0F000F000F00
            F75EF75EF75EF75EF75EF75EEF3D0F00F75EF75EF75EEF3D0F00F75EF75E0F00
            F75EF75EF75EF75EF75EF75EF75EEF3D0F000F000F000F00F75EF75EF75EF75E
            F75EF75EF75E}
          Spacing = 1
        end
      end
      inherited PcFormDt: TPageControl
        Top = 303
        Width = 748
        Height = 87
        inherited tsDt: TTabSheet
          Caption = 'Detail 1'
          inherited DBGridDt: TSMDBGrid
            Width = 740
            Height = 59
            FixedColor = clTeal
            Font.Color = clBlack
            TitleFont.Color = clBlack
            OnEditButtonClick = DBGridDtEditButtonClick
            Bands.Strings = (
              'Order'
              'Warehouse')
            GridStyle.OddColor = clWhite
            ExOptions = [eoStandardPopup, eoDrawBands, eoShowFilterBar, eoAnyKeyFilter, eoFilterAutoApply]
            ColCount = 12
            Columns = <
              item
                ButtonStyle = cbsEllipsis
                Expanded = False
                FieldName = 'ProductCode'
                PickList.Strings = ()
                Title.Caption = 'Product Code*'
                Width = 115
                Visible = True
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'ProductName'
                PickList.Strings = ()
                ReadOnly = True
                Title.Caption = 'Product Name'
                Width = 220
                Visible = True
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Specification'
                PickList.Strings = ()
                Width = 180
                Visible = True
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'QtyOrder'
                PickList.Strings = ()
                Title.Caption = 'Qty*'
                Width = 60
                Visible = True
                BandIndex = 0
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'LUnitOrder'
                PickList.Strings = ()
                Title.Caption = 'Unit*'
                Width = 60
                Visible = True
                BandIndex = 0
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Price'
                PickList.Strings = ()
                Width = 66
                Visible = True
                BandIndex = 0
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'PriceList'
                PickList.Strings = ()
                ReadOnly = True
                Title.Caption = 'Price List'
                Visible = True
                BandIndex = 0
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Amount'
                PickList.Strings = ()
                ReadOnly = True
                Width = 80
                Visible = True
                BandIndex = 0
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Qty'
                PickList.Strings = ()
                ReadOnly = True
                Title.Caption = 'Qty*'
                Width = 60
                Visible = True
                BandIndex = 1
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Unit'
                PickList.Strings = ()
                ReadOnly = True
                Title.Caption = 'Unit*'
                Width = 48
                Visible = True
                BandIndex = 1
                FilterList.Strings = ()
              end
              item
                Expanded = False
                FieldName = 'Remark'
                PickList.Strings = ()
                Width = 200
                Visible = True
                FilterList.Strings = ()
              end>
          end
        end
      end
      inherited pnlBawah: TPanel
        Top = 390
        Width = 748
        inherited dbtUserPrep: TDBText
          Width = 210
          DataField = 'UserPrep'
        end
        inherited lblUserAppr: TLabel
          Left = 420
        end
        inherited dbtUserAppr: TDBText
          Left = 515
          Width = 166
          DataField = 'UserAppr'
        end
        inherited dbtDatePrep: TDBText
          Width = 201
          DataField = 'DatePrep'
        end
        inherited lblDateAppr: TLabel
          Left = 420
        end
        inherited dbtDateAppr: TDBText
          Left = 515
          Width = 163
          DataField = 'DateAppr'
        end
        inherited BtnSave: TBitBtn
          Left = 6
        end
      end
    end
    inherited tsSearch: TTabSheet
      inherited pnlSearchTop: TPanel
        Width = 748
        inherited lblTitle2: TLabel
          Width = 553
        end
        inherited lblTitle1: TLabel
          Width = 553
        end
        inherited btnSearch: TBitBtn
          TabOrder = 11
        end
        inherited btnBack: TBitBtn
          TabOrder = 12
        end
        inherited CbxPeriod: TCheckBox
          TabOrder = 0
        end
        inherited Panel1: TPanel
          Width = 748
          TabOrder = 13
        end
        inherited dtpStart: TDateTimePicker
          TabOrder = 1
        end
        inherited dtpEnd: TDateTimePicker
          TabOrder = 2
        end
        inherited dbeEditNo: TEdit
          TabOrder = 3
        end
      end
      inherited pnlSearchGrid: TPanel
        Width = 748
        Height = 249
        inherited dgrSearch: TSMDBGrid
          Width = 748
          Height = 249
          FixedColor = clTeal
          ExOptions = [eoENTERlikeTAB, eoBLOBEditor, eoShowFilterBar, eoAnyKeyFilter, eoFilterAutoApply]
          ColCount = 32
          Columns = <
            item
              Expanded = False
              FieldName = 'Order_No'
              PickList.Strings = ()
              Title.Caption = 'Order No'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Revisi'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Status'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Order_Date'
              PickList.Strings = ()
              Title.Caption = 'Order Date'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'FgReport'
              PickList.Strings = ()
              Title.Caption = 'Report'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Customer_Code'
              PickList.Strings = ()
              Title.Caption = 'Customer Code'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Customer_Name'
              PickList.Strings = ()
              Title.Caption = 'Customer Name'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Attn'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'BillTo'
              PickList.Strings = ()
              Title.Caption = 'Bill To'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Project_Code'
              PickList.Strings = ()
              Title.Caption = 'Project Code'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Term'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Due_Date'
              PickList.Strings = ()
              Title.Caption = 'Due Date'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Product_Group'
              PickList.Strings = ()
              Title.Caption = 'Product Group'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Product_Group_Name'
              PickList.Strings = ()
              Title.Caption = 'Product Group Name'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'FgCBD'
              PickList.Strings = ()
              Title.Caption = 'Fg CBD'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Cust_PO_No'
              PickList.Strings = ()
              Title.Caption = 'Cust PO No'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Cust_PO_Date'
              PickList.Strings = ()
              Title.Caption = 'Cust PO Date'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'DeliveryTo'
              PickList.Strings = ()
              Title.Caption = 'Delivery To'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Delivery_Date'
              PickList.Strings = ()
              Title.Caption = 'Delivery Date'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Term_Payment'
              PickList.Strings = ()
              Title.Caption = 'Term Payment'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Currency'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Forex_Rate'
              PickList.Strings = ()
              Title.Caption = 'Forex Rate'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Base_Forex'
              PickList.Strings = ()
              Title.Caption = 'Base Forex'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'PPN'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'PPN_Forex'
              PickList.Strings = ()
              Title.Caption = 'PPN Forex'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'FgDP'
              PickList.Strings = ()
              Title.Caption = 'Fg DP'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'DP'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'DP_Forex'
              PickList.Strings = ()
              Title.Caption = 'DP Forex'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'Remark'
              PickList.Strings = ()
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'FgActive'
              PickList.Strings = ()
              Title.Caption = 'Active'
              Visible = True
              FilterList.Strings = ()
            end
            item
              Expanded = False
              FieldName = 'SO_Type'
              PickList.Strings = ()
              Title.Caption = 'SO Type'
              Visible = True
              FilterList.Strings = ()
            end>
        end
      end
    end
    inherited tsReff: TTabSheet
      inherited pnlDtReff: TPanel [0]
        Top = 143
        Width = 748
        Height = 275
        inherited DbGridReff: TSMDBGrid
          Width = 748
          Height = 275
          Options = [dgEditing, dgTitles, dgIndicator, dgColumnResize, dgColLines, dgRowLines, dgConfirmDelete, dgCancelOnExit]
        end
      end
      inherited pnlHdReff: TPanel [1]
        Width = 748
        Height = 143
        inherited Bevel5: TBevel
          Left = 591
          Width = 155
        end
        inherited lblTitleReffB: TLabel
          Width = 553
        end
        inherited lblTitleReffA: TLabel
          Width = 553
        end
        inherited lblDefault: TLabel
          Left = 607
        end
        inherited pnlRefHeader: TPanel
          Left = -58
          Top = 28
          Width = 500
          Height = 56
          Visible = True
        end
        inherited pnlRefCriteria: TPanel [5]
          Visible = True
        end
        inherited Panel2: TPanel [6]
          Top = 122
          Width = 748
        end
        inherited btnGenerate: TBitBtn
          Left = 602
          Width = 71
        end
        inherited cbInput: TComboBox
          Left = 678
        end
        inherited btnReffRefresh: TBitBtn
          Left = 720
        end
        inherited btnHome: TBitBtn
          Left = 675
        end
      end
    end
  end
  object PNLcust: TPanel [3]
    Left = -504
    Top = 24
    Width = 513
    Height = 233
    Color = clSilver
    TabOrder = 3
    Visible = False
    object Panel5: TPanel
      Left = 1
      Top = 1
      Width = 511
      Height = 25
      Align = alTop
      Caption = 'DELIVERY TO'
      Color = clBlue
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWhite
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 0
      object Label46: TLabel
        Left = 11
        Top = 8
        Width = 138
        Height = 13
        Alignment = taRightJustify
        Caption = '*Double Click to choose'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWhite
        Font.Height = -11
        Font.Name = 'MS Sans Serif'
        Font.Style = [fsBold]
        ParentFont = False
        Transparent = True
      end
      object Label47: TLabel
        Left = 448
        Top = 8
        Width = 52
        Height = 13
        Alignment = taRightJustify
        Caption = '[X] Close'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWhite
        Font.Height = -11
        Font.Name = 'MS Sans Serif'
        Font.Style = [fsBold]
        ParentFont = False
        Transparent = True
        OnClick = Label47Click
      end
    end
    object DBGrid1: TSMDBGrid
      Left = 1
      Top = 26
      Width = 511
      Height = 206
      Align = alClient
      DataSource = dsmain
      FixedColor = clInactiveCaptionText
      Options = [dgEditing, dgTitles, dgIndicator, dgColumnResize, dgColLines, dgRowLines, dgTabs, dgConfirmDelete, dgCancelOnExit]
      TabOrder = 1
      TitleFont.Charset = DEFAULT_CHARSET
      TitleFont.Color = clWindowText
      TitleFont.Height = -11
      TitleFont.Name = 'MS Sans Serif'
      TitleFont.Style = []
      OnDblClick = DBGrid1DblClick
      Flat = False
      BandsFont.Charset = DEFAULT_CHARSET
      BandsFont.Color = clWindowText
      BandsFont.Height = -11
      BandsFont.Name = 'MS Sans Serif'
      BandsFont.Style = []
      Groupings = <>
      GridStyle.Style = gsCustom
      GridStyle.OddColor = clWindow
      GridStyle.EvenColor = cl3DLight
      TitleHeight.PixelCount = 24
      FooterColor = clBtnFace
      ExOptions = [eoENTERlikeTAB, eoKeepSelection, eoBLOBEditor, eoShowFilterBar, eoAnyKeyFilter, eoFilterAutoApply]
      RegistryKey = 'Software\Scalabium'
      RegistrySection = 'SMDBGrid'
      WidthOfIndicator = 11
      DefaultRowHeight = 17
      ScrollBars = ssHorizontal
      ColCount = 9
      RowCount = 2
      Columns = <
        item
          Expanded = False
          FieldName = 'DeliveryCode'
          Title.Caption = 'Delivery Code'
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'DeliveryName'
          Title.Caption = 'Delivery Name'
          Width = 200
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'DeliveryAddr1'
          Title.Caption = 'Delivery Addr 1'
          Width = 200
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'DeliveryAddr2'
          Title.Caption = 'Delivery Addr 2'
          Width = 200
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'LCountry'
          Title.Caption = 'Country'
          Width = 60
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'ZipCode'
          Title.Caption = 'Zip Code'
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'PhoneNo'
          Title.Caption = 'Phone No'
          Visible = True
        end
        item
          Expanded = False
          FieldName = 'Fax'
          Visible = True
        end>
    end
  end
  inherited DComData: TDCOMConnection
    ServerGUID = '{05E4D10A-50E1-4480-BDCF-E7FDC5E05AC8}'
    ServerName = 'MTrSO.TrSO'
    Left = 29
    Top = 0
  end
  inherited CdsHd: TClientDataSet
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end
      item
        DataType = ftString
        Name = '@Nmbr'
        ParamType = ptInput
      end
      item
        DataType = ftInteger
        Name = '@Revisi'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@SOType'
        ParamType = ptInput
      end>
    ProviderName = 'dpSONmbr'
    RemoteServer = DComData
    Left = 72
    Top = 0
    object CdsHdTransNmbr: TStringField
      FieldName = 'TransNmbr'
      FixedChar = True
    end
    object CdsHdRevisi: TIntegerField
      FieldName = 'Revisi'
    end
    object CdsHdStatus: TStringField
      FieldName = 'Status'
      FixedChar = True
      Size = 1
    end
    object CdsHdTransDate: TDateTimeField
      FieldName = 'TransDate'
    end
    object CdsHdFgReport: TStringField
      FieldName = 'FgReport'
      OnChange = CdsHdFgReportChange
      FixedChar = True
      Size = 1
    end
    object CdsHdCustCode: TStringField
      FieldName = 'CustCode'
      OnChange = CdsHdCustCodeChange
      FixedChar = True
      Size = 12
    end
    object CdsHdAttn: TStringField
      FieldName = 'Attn'
      FixedChar = True
      Size = 40
    end
    object CdsHdBillTo: TStringField
      FieldName = 'BillTo'
      OnChange = CdsHdBillToChange
      FixedChar = True
      Size = 12
    end
    object CdsHdTerm: TStringField
      FieldName = 'Term'
      OnChange = CdsHdTransDateChange
      FixedChar = True
      Size = 3
    end
    object CdsHdDueDate: TDateTimeField
      FieldName = 'DueDate'
    end
    object CdsHdProductGroup: TStringField
      FieldName = 'ProductGroup'
      FixedChar = True
      Size = 3
    end
    object CdsHdFgCBD: TStringField
      FieldName = 'FgCBD'
      OnChange = CdsHdFgCBDChange
      FixedChar = True
      Size = 1
    end
    object CdsHdCustPONo: TStringField
      FieldName = 'CustPONo'
      FixedChar = True
      Size = 30
    end
    object CdsHdCustPODate: TDateTimeField
      FieldName = 'CustPODate'
    end
    object CdsHdDeliveryDate: TDateTimeField
      FieldName = 'DeliveryDate'
    end
    object CdsHdCurrCode: TStringField
      FieldName = 'CurrCode'
      OnChange = CdsHdCurrCodeChange
      FixedChar = True
      Size = 5
    end
    object CdsHdForexRate: TFloatField
      Tag = 8
      FieldName = 'ForexRate'
    end
    object CdsHdBaseForex: TFloatField
      Tag = 8
      FieldName = 'BaseForex'
      OnChange = CdsHdBaseForexChange
      DisplayFormat = '#,##0.##'
    end
    object CdsHdDisc: TFloatField
      FieldName = 'Disc'
      OnChange = CdsHdDiscChange
      DisplayFormat = '###,##0.##'
      EditFormat = '###.##'
    end
    object CdsHdDiscForex: TFloatField
      Tag = 8
      FieldName = 'DiscForex'
      OnChange = CdsHdDiscForexChange
      DisplayFormat = '#,##0.##'
    end
    object CdsHdPPN: TFloatField
      FieldName = 'PPN'
      OnChange = CdsHdBaseForexChange
      DisplayFormat = '#,##0.##'
      EditFormat = '###.##'
    end
    object CdsHdPPNForex: TFloatField
      Tag = 8
      FieldName = 'PPNForex'
      OnChange = CdsHdPPNForexChange
      DisplayFormat = '#,##0.##'
    end
    object CdsHdTotalForex: TFloatField
      Tag = 8
      FieldName = 'TotalForex'
      OnChange = CdsHdTotalForexChange
      DisplayFormat = '#,##0.##'
    end
    object CdsHdFgDP: TStringField
      FieldName = 'FgDP'
      OnChange = CdsHdFgDPChange
      FixedChar = True
      Size = 1
    end
    object CdsHdDP: TFloatField
      Tag = 8
      FieldName = 'DP'
      OnChange = CdsHdDPChange
    end
    object CdsHdDPForex: TFloatField
      Tag = 8
      FieldName = 'DPForex'
      OnChange = CdsHdDPForexChange
    end
    object CdsHdRemark: TStringField
      FieldName = 'Remark'
      FixedChar = True
      Size = 60
    end
    object CdsHdUserPrep: TStringField
      FieldName = 'UserPrep'
      FixedChar = True
      Size = 30
    end
    object CdsHdDatePrep: TDateTimeField
      FieldName = 'DatePrep'
    end
    object CdsHdUserAppr: TStringField
      FieldName = 'UserAppr'
      FixedChar = True
      Size = 30
    end
    object CdsHdDateAppr: TDateTimeField
      FieldName = 'DateAppr'
    end
    object CdsHdUserComplete: TStringField
      FieldName = 'UserComplete'
      FixedChar = True
      Size = 30
    end
    object CdsHdDateComplete: TDateTimeField
      FieldName = 'DateComplete'
    end
    object CdsHdCollect_Name: TStringField
      FieldName = 'Collect_Name'
      FixedChar = True
      Size = 60
    end
    object CdsHdFgActive: TStringField
      FieldName = 'FgActive'
      FixedChar = True
      Size = 1
    end
    object CdsHdProjectCode: TStringField
      FieldName = 'ProjectCode'
      FixedChar = True
    end
    object CdsHdSOType: TStringField
      FieldName = 'SOType'
      FixedChar = True
      Size = 8
    end
    object CdsHdDoneDP: TStringField
      FieldName = 'DoneDP'
      FixedChar = True
      Size = 1
    end
    object CdsHdDonePFI: TStringField
      FieldName = 'DonePFI'
      FixedChar = True
      Size = 1
    end
    object CdsHdCustName: TStringField
      FieldName = 'CustName'
      FixedChar = True
      Size = 60
    end
    object CdsHdFgStatus: TStringField
      FieldName = 'FgStatus'
      OnChange = CdsHdFgStatusChange
      FixedChar = True
      Size = 12
    end
    object CdsHdLCurrency: TStringField
      FieldKind = fkLookup
      FieldName = 'LCurrency'
      LookupDataSet = CdsGetCurrency
      LookupKeyFields = 'CurrCode'
      LookupResultField = 'CurrCode'
      KeyFields = 'CurrCode'
      Lookup = True
    end
    object CdsHdLCurrDigit: TStringField
      FieldKind = fkLookup
      FieldName = 'LCurrDigit'
      LookupDataSet = CdsGetCurrency
      LookupKeyFields = 'CurrCode'
      LookupResultField = 'DigitDecimal'
      KeyFields = 'CurrCode'
      Lookup = True
    end
    object CdsHdLTerm: TStringField
      FieldKind = fkLookup
      FieldName = 'LTerm'
      LookupDataSet = cdsGetTerm
      LookupKeyFields = 'TermCode'
      LookupResultField = 'TermName'
      KeyFields = 'Term'
      Lookup = True
    end
    object CdsHdLTermX: TIntegerField
      FieldKind = fkLookup
      FieldName = 'LTermX'
      LookupDataSet = cdsGetTerm
      LookupKeyFields = 'TermCode'
      LookupResultField = 'XRange'
      KeyFields = 'Term'
      Lookup = True
    end
    object CdsHdLTermRange: TStringField
      FieldKind = fkLookup
      FieldName = 'LTermRange'
      LookupDataSet = cdsGetTerm
      LookupKeyFields = 'TermCode'
      LookupResultField = 'TypeRange'
      KeyFields = 'Term'
      Lookup = True
    end
    object CdsHdLProductGroup: TStringField
      FieldKind = fkLookup
      FieldName = 'LProductGroup'
      LookupDataSet = cdsGetProductGroup
      LookupKeyFields = 'ProductGrpCode'
      LookupResultField = 'ProductGrpName'
      KeyFields = 'ProductGroup'
      Lookup = True
    end
    object CdsHdDeliveryName: TStringField
      FieldName = 'DeliveryName'
      FixedChar = True
      Size = 60
    end
    object CdsHdProjectName: TStringField
      FieldName = 'ProjectName'
      FixedChar = True
      Size = 60
    end
    object CdsHdLCurrName: TStringField
      FieldKind = fkLookup
      FieldName = 'LCurrName'
      LookupDataSet = CdsGetCurrency
      LookupKeyFields = 'CurrCode'
      LookupResultField = 'CurrName'
      KeyFields = 'CurrCode'
      Lookup = True
    end
    object CdsHdSales: TStringField
      FieldName = 'Sales'
      OnChange = CdsHdSalesChange
      FixedChar = True
      Size = 12
    end
    object CdsHdDoneInvoice: TStringField
      FieldName = 'DoneInvoice'
      FixedChar = True
      Size = 1
    end
    object CdsHdSales_Name: TStringField
      FieldName = 'Sales_Name'
      FixedChar = True
      Size = 60
    end
    object CdsHdFgPPN: TStringField
      FieldName = 'FgPPN'
      OnChange = CdsHdFgPPNChange
      FixedChar = True
      Size = 1
    end
    object CdsHdDeliveryTo: TStringField
      FieldName = 'DeliveryTo'
      FixedChar = True
      Size = 12
    end
    object CdsHdDeliveryType: TStringField
      FieldName = 'DeliveryType'
      FixedChar = True
      Size = 10
    end
    object CdsHdDeliveryAddr1: TStringField
      FieldName = 'DeliveryAddr1'
      FixedChar = True
      Size = 60
    end
    object CdsHdDeliveryAddr2: TStringField
      FieldName = 'DeliveryAddr2'
      FixedChar = True
      Size = 60
    end
    object CdsHdAmountDPList: TFloatField
      FieldName = 'AmountDPList'
    end
    object CdsHdTermCust: TStringField
      FieldName = 'TermCust'
      FixedChar = True
      Size = 3
    end
    object CdsHdTermPayment: TStringField
      FieldName = 'TermPayment'
      FixedChar = True
      Size = 60
    end
  end
  inherited CdsDt: TClientDataSet
    AggregatesActive = True
    IndexFieldNames = 'ProductCode'
    Params = <
      item
        DataType = ftString
        Name = 'Nmbr'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = 'Revisi'
        ParamType = ptInput
      end>
    ProviderName = 'dpSODt'
    RemoteServer = DComData
    AfterDelete = CdsDtAfterDelete
    Left = 104
    Top = 0
    object CdsDtTransNmbr: TStringField
      FieldName = 'TransNmbr'
      FixedChar = True
    end
    object CdsDtRevisi: TIntegerField
      FieldName = 'Revisi'
    end
    object CdsDtProductCode: TStringField
      FieldName = 'ProductCode'
      OnChange = CdsDtProductCodeChange
      FixedChar = True
    end
    object CdsDtSpecification: TStringField
      FieldName = 'Specification'
      FixedChar = True
      Size = 100
    end
    object CdsDtQtyOrder: TFloatField
      FieldName = 'QtyOrder'
      OnChange = CdsDtQtyOrderChange
      DisplayFormat = '###,##0.####'
      EditFormat = '##0.####'
    end
    object CdsDtUnitOrder: TStringField
      FieldName = 'UnitOrder'
      OnChange = CdsDtQtyOrderChange
      FixedChar = True
      Size = 5
    end
    object CdsDtPrice: TFloatField
      FieldName = 'Price'
      OnChange = CdsDtQtyChange
      DisplayFormat = '###,##0.##'
      EditFormat = '###.##'
    end
    object CdsDtAmount: TFloatField
      Tag = 8
      FieldName = 'Amount'
      OnChange = CdsDtAmountChange
      DisplayFormat = '###,##0.##'
      EditFormat = '###.##'
    end
    object CdsDtQty: TFloatField
      FieldName = 'Qty'
      OnChange = CdsDtQtyChange
      DisplayFormat = '#,##0.####'
      EditFormat = '#,##0.##'
    end
    object CdsDtUnit: TStringField
      FieldName = 'Unit'
      FixedChar = True
      Size = 5
    end
    object CdsDtRemark: TStringField
      FieldName = 'Remark'
      FixedChar = True
      Size = 60
    end
    object CdsDtDoneClosing: TStringField
      FieldName = 'DoneClosing'
      FixedChar = True
      Size = 1
    end
    object CdsDtUserClose: TStringField
      FieldName = 'UserClose'
      FixedChar = True
      Size = 30
    end
    object CdsDtDateClose: TDateTimeField
      FieldName = 'DateClose'
    end
    object CdsDtQtyClose: TFloatField
      FieldName = 'QtyClose'
      DisplayFormat = '###,##0.##'
    end
    object CdsDtRemarkClose: TStringField
      FieldName = 'RemarkClose'
      FixedChar = True
      Size = 60
    end
    object CdsDtQtyDO: TFloatField
      Tag = 8
      FieldName = 'QtyDO'
    end
    object CdsDtProductName: TStringField
      FieldName = 'ProductName'
      FixedChar = True
      Size = 60
    end
    object CdsDtLUnitOrder: TStringField
      FieldKind = fkLookup
      FieldName = 'LUnitOrder'
      LookupDataSet = cdsGetUnit
      LookupKeyFields = 'UnitCode'
      LookupResultField = 'UnitCode'
      KeyFields = 'UnitOrder'
      Lookup = True
    end
    object CdsDtDonePDO: TStringField
      FieldName = 'DonePDO'
      FixedChar = True
      Size = 1
    end
    object CdsDtPriceList: TFloatField
      FieldName = 'PriceList'
      DisplayFormat = '###,##0.##'
      EditFormat = '###.##'
    end
    object CdsDtCAmount: TAggregateField
      FieldName = 'CAmount'
      Active = True
      Expression = 'SUM(Amount)'
    end
    object CdsDtCQtyOrder: TAggregateField
      FieldName = 'CQtyOrder'
      Active = True
      Expression = 'SUM(QtyOrder)'
    end
  end
  inherited dsHd: TDataSource
    Left = 72
    Top = 17
  end
  inherited dsDt: TDataSource
    OnStateChange = dsDtStateChange
    OnDataChange = dsDtDataChange
    Left = 104
    Top = 16
  end
  object cdsFindCust: TClientDataSet [9]
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@Code'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Type'
        ParamType = ptInput
      end
      item
        DataType = ftDateTime
        Name = '@Date'
        ParamType = ptInput
      end>
    ProviderName = 'dpFindCust'
    RemoteServer = DComData
    Left = 644
    Top = 169
  end
  inherited CdsAutoNmbr: TClientDataSet
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftInteger
        Name = '@Year'
        ParamType = ptInput
      end
      item
        DataType = ftInteger
        Name = '@Period'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Modul'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@AddParam'
        ParamType = ptInput
      end>
    Left = 668
    Top = 65529
  end
  inherited CdsSearchTrans: TClientDataSet
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end
      item
        DataType = ftString
        Name = '@Str1'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOSearch'
    Left = 168
    Top = 0
    object CdsSearchTransOrder_No: TStringField
      FieldName = 'Order_No'
      FixedChar = True
    end
    object CdsSearchTransRevisi: TIntegerField
      FieldName = 'Revisi'
    end
    object CdsSearchTransStatus: TStringField
      FieldName = 'Status'
      FixedChar = True
      Size = 1
    end
    object CdsSearchTransOrder_Date: TDateTimeField
      FieldName = 'Order_Date'
    end
    object CdsSearchTransFgReport: TStringField
      FieldName = 'FgReport'
      FixedChar = True
      Size = 1
    end
    object CdsSearchTransCustomer_Code: TStringField
      FieldName = 'Customer_Code'
      FixedChar = True
      Size = 12
    end
    object CdsSearchTransCustomer_Name: TStringField
      FieldName = 'Customer_Name'
      FixedChar = True
      Size = 60
    end
    object CdsSearchTransAttn: TStringField
      FieldName = 'Attn'
      FixedChar = True
      Size = 40
    end
    object CdsSearchTransBillTo: TStringField
      FieldName = 'BillTo'
      FixedChar = True
      Size = 12
    end
    object CdsSearchTransProject_Code: TStringField
      FieldName = 'Project_Code'
      FixedChar = True
      Size = 10
    end
    object CdsSearchTransTerm: TStringField
      FieldName = 'Term'
      FixedChar = True
      Size = 3
    end
    object CdsSearchTransDue_Date: TDateTimeField
      FieldName = 'Due_Date'
    end
    object CdsSearchTransProduct_Group: TStringField
      FieldName = 'Product_Group'
      FixedChar = True
      Size = 3
    end
    object CdsSearchTransProduct_Group_Name: TStringField
      FieldName = 'Product_Group_Name'
      FixedChar = True
      Size = 50
    end
    object CdsSearchTransFgCBD: TStringField
      FieldName = 'FgCBD'
      FixedChar = True
      Size = 1
    end
    object CdsSearchTransCust_PO_No: TStringField
      FieldName = 'Cust_PO_No'
      FixedChar = True
      Size = 30
    end
    object CdsSearchTransCust_PO_Date: TDateTimeField
      FieldName = 'Cust_PO_Date'
    end
    object CdsSearchTransDeliveryTo: TStringField
      FieldName = 'DeliveryTo'
      FixedChar = True
      Size = 5
    end
    object CdsSearchTransDelivery_Date: TDateTimeField
      FieldName = 'Delivery_Date'
    end
    object CdsSearchTransTerm_Payment: TStringField
      FieldName = 'Term_Payment'
      FixedChar = True
      Size = 30
    end
    object CdsSearchTransCurrency: TStringField
      FieldName = 'Currency'
      FixedChar = True
      Size = 5
    end
    object CdsSearchTransForex_Rate: TFloatField
      FieldName = 'Forex_Rate'
    end
    object CdsSearchTransBase_Forex: TFloatField
      FieldName = 'Base_Forex'
    end
    object CdsSearchTransPPN: TFloatField
      FieldName = 'PPN'
    end
    object CdsSearchTransPPN_Forex: TFloatField
      FieldName = 'PPN_Forex'
    end
    object CdsSearchTransFgDP: TStringField
      FieldName = 'FgDP'
      FixedChar = True
      Size = 1
    end
    object CdsSearchTransDP: TFloatField
      FieldName = 'DP'
    end
    object CdsSearchTransDP_Forex: TFloatField
      FieldName = 'DP_Forex'
    end
    object CdsSearchTransRemark: TStringField
      FieldName = 'Remark'
      FixedChar = True
      Size = 60
    end
    object CdsSearchTransFgActive: TStringField
      FieldName = 'FgActive'
      FixedChar = True
      Size = 1
    end
    object CdsSearchTransSO_Type: TStringField
      FieldName = 'SO_Type'
      FixedChar = True
      Size = 8
    end
  end
  inherited dsSearchTrans: TDataSource
    Left = 168
    Top = 16
  end
  inherited CdsPosting: TClientDataSet
    Left = 576
    Top = 16
  end
  inherited CdsFindMnemonic: TClientDataSet
    Left = 700
    Top = 33
  end
  inherited CdsReff: TClientDataSet
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end
      item
        DataType = ftString
        Name = '@Str1'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Str2'
        ParamType = ptInput
      end>
    Left = 202
    Top = 65534
  end
  inherited dsReff: TDataSource
    Left = 202
    Top = 18
  end
  inherited CdsGetRate: TClientDataSet
    Left = 692
    Top = 9
  end
  inherited CdsDt2: TClientDataSet
    Params = <
      item
        DataType = ftString
        Name = 'Nmbr'
        ParamType = ptInput
      end>
    RemoteServer = DComData
    Left = 136
    Top = 1
  end
  inherited dsDt2: TDataSource
    Left = 135
    Top = 17
  end
  object CdsGetCurrency: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end>
    ProviderName = 'dpGetCurrency'
    RemoteServer = DComData
    Left = 644
    Top = 137
  end
  object cdsGetTerm: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end>
    ProviderName = 'dpGetTerm'
    RemoteServer = DComData
    Left = 644
    Top = 201
    object cdsGetTermTermCode: TStringField
      FieldName = 'TermCode'
      FixedChar = True
      Size = 3
    end
    object cdsGetTermTermName: TStringField
      FieldName = 'TermName'
      FixedChar = True
      Size = 50
    end
    object cdsGetTermTypeRange: TStringField
      FieldName = 'TypeRange'
      FixedChar = True
      Size = 10
    end
    object cdsGetTermXRange: TIntegerField
      FieldName = 'XRange'
    end
    object cdsGetTermFgCBD: TStringField
      FieldName = 'FgCBD'
      FixedChar = True
      Size = 1
    end
  end
  object cdsGetUnit: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end>
    ProviderName = 'dpGetUnit'
    RemoteServer = DComData
    Left = 644
    Top = 89
  end
  object cdsGetProductGroup: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end>
    ProviderName = 'dpGetProductGroup'
    RemoteServer = DComData
    Left = 644
    Top = 233
    object cdsGetProductGroupProductGrpCode: TStringField
      FieldName = 'ProductGrpCode'
      FixedChar = True
      Size = 3
    end
    object cdsGetProductGroupProductGrpName: TStringField
      FieldName = 'ProductGrpName'
      FixedChar = True
      Size = 50
    end
  end
  object cdsGetConvertion: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end
      item
        DataType = ftString
        Name = '@Product'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@UnitFrom'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@UnitTo'
        ParamType = ptInput
      end>
    ProviderName = 'dpGetConvertion'
    RemoteServer = DComData
    Left = 644
    Top = 265
    object cdsGetConvertionRate: TFloatField
      FieldName = 'Rate'
    end
  end
  object CdsCekProduct: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftDateTime
        Name = '@Date'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Currency'
        ParamType = ptInput
      end
      item
        DataType = ftFloat
        Name = '@Rate'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@ProjectCode'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@ProductGrp'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Product'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOGetProduct'
    RemoteServer = DComData
    Left = 676
    Top = 169
  end
  object CdsGetRevisi: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@SONo'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOGetRevisi'
    RemoteServer = DComData
    Left = 676
    Top = 201
  end
  object CdsCreateRevisi: TClientDataSet
    Aggregates = <>
    Params = <>
    ProviderName = 'dpSOCreateRevisi'
    RemoteServer = DComData
    Left = 676
    Top = 233
  end
  object cdsClosing: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@Nmbr'
        ParamType = ptInput
      end
      item
        DataType = ftInteger
        Name = '@Revisi'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Product'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Remark'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@User'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@EMessage'
        ParamType = ptOutput
      end>
    ProviderName = 'dpMKSOClosing'
    RemoteServer = DComData
    Left = 608
    Top = 208
  end
  object cdsFindSales: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@Code'
        ParamType = ptInput
      end>
    ProviderName = 'dpFindSales'
    RemoteServer = DComData
    Left = 704
    Top = 72
    object cdsFindSalesSales_Code: TStringField
      FieldName = 'Sales_Code'
      FixedChar = True
      Size = 12
    end
    object cdsFindSalesSales_Name: TStringField
      FieldName = 'Sales_Name'
      FixedChar = True
      Size = 60
    end
  end
  object cdsCekSJ: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@SONo'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOCheckSJ'
    RemoteServer = DComData
    Left = 560
    Top = 160
    object cdsCekSJTransNmbr: TStringField
      FieldName = 'TransNmbr'
      FixedChar = True
    end
    object cdsCekSJStatus: TStringField
      FieldName = 'Status'
      FixedChar = True
      Size = 1
    end
  end
  object cdsFindBillTo: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@Customer'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Code'
        ParamType = ptInput
      end
      item
        DataType = ftDateTime
        Name = '@Date'
        ParamType = ptInput
      end>
    ProviderName = 'dpFindBillTo'
    RemoteServer = DComData
    Left = 608
    Top = 120
    object cdsFindBillToBill_To: TStringField
      FieldName = 'Bill_To'
      FixedChar = True
      Size = 12
    end
    object cdsFindBillToBill_To_Name: TStringField
      FieldName = 'Bill_To_Name'
      FixedChar = True
      Size = 60
    end
    object cdsFindBillToCurrency: TStringField
      FieldName = 'Currency'
      FixedChar = True
      Size = 5
    end
  end
  object cdsCekDO: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@SONo'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOCheckDO'
    RemoteServer = DComData
    Left = 608
    Top = 240
    object cdsCekDOStatus: TStringField
      FieldName = 'Status'
      FixedChar = True
      Size = 1
    end
  end
  object CdsMain: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftString
        Name = 'Nmbr'
        ParamType = ptInput
      end>
    ProviderName = 'dpCustAddress'
    RemoteServer = DComData
    BeforePost = CdsMainBeforePost
    AfterPost = CdsMainAfterPost
    AfterDelete = CdsMainAfterDelete
    OnNewRecord = CdsMainNewRecord
    OnReconcileError = CdsHdReconcileError
    Left = 244
    object CdsMainCustCode: TStringField
      FieldName = 'CustCode'
      FixedChar = True
      Size = 12
    end
    object CdsMainDeliveryCode: TStringField
      FieldName = 'DeliveryCode'
      FixedChar = True
      Size = 12
    end
    object CdsMainDeliveryName: TStringField
      FieldName = 'DeliveryName'
      FixedChar = True
      Size = 60
    end
    object CdsMainDeliveryAddr1: TStringField
      FieldName = 'DeliveryAddr1'
      FixedChar = True
      Size = 60
    end
    object CdsMainDeliveryAddr2: TStringField
      FieldName = 'DeliveryAddr2'
      FixedChar = True
      Size = 60
    end
    object CdsMainCountry: TStringField
      FieldName = 'Country'
      FixedChar = True
      Size = 3
    end
    object CdsMainZipCode: TStringField
      FieldName = 'ZipCode'
      FixedChar = True
      Size = 10
    end
    object CdsMainUserId: TStringField
      FieldName = 'UserId'
      FixedChar = True
      Size = 30
    end
    object CdsMainUserDate: TDateTimeField
      FieldName = 'UserDate'
    end
    object CdsMainPhoneNo: TStringField
      FieldName = 'PhoneNo'
      FixedChar = True
    end
    object CdsMainFax: TStringField
      FieldName = 'Fax'
      FixedChar = True
    end
    object CdsMainLCountry: TStringField
      FieldKind = fkLookup
      FieldName = 'LCountry'
      LookupDataSet = CdsGetCountry
      LookupKeyFields = 'CountryCode'
      LookupResultField = 'CountryName'
      KeyFields = 'Country'
      Lookup = True
    end
  end
  object dsmain: TDataSource
    DataSet = CdsMain
    Left = 243
    Top = 19
  end
  object CdsGetCountry: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end>
    ProviderName = 'dpGetCountr'
    RemoteServer = DComData
    Left = 608
    Top = 272
    object CdsGetCountryCountryCode: TStringField
      FieldName = 'CountryCode'
      FixedChar = True
      Size = 3
    end
    object CdsGetCountryCountryName: TStringField
      FieldName = 'CountryName'
      FixedChar = True
      Size = 40
    end
  end
  object cdsCekCBD: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
        Value = 0
      end>
    ProviderName = 'dpTermCekCBD'
    RemoteServer = DComData
    Left = 676
    Top = 265
    object cdsCekCBDFgCBD: TStringField
      FieldName = 'FgCBD'
      FixedChar = True
      Size = 1
    end
    object cdsCekCBDTermCode: TStringField
      FieldName = 'TermCode'
      FixedChar = True
      Size = 3
    end
  end
  object cdsGetInfoStock: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftDateTime
        Name = '@Date'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Product'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOGetInfoStock'
    RemoteServer = DComData
    Left = 468
    Top = 113
    object cdsGetInfoStockOnHand: TFloatField
      FieldName = 'OnHand'
      DisplayFormat = '###,##0.##'
    end
    object cdsGetInfoStockOnBook: TFloatField
      FieldName = 'OnBook'
      DisplayFormat = '###,##0.##'
    end
    object cdsGetInfoStockOnProduction: TFloatField
      FieldName = 'OnProduction'
      DisplayFormat = '###,##0.##'
    end
    object cdsGetInfoStockOnWrhs: TFloatField
      FieldName = 'OnWrhs'
    end
    object cdsGetInfoStockOnWIP: TFloatField
      FieldName = 'OnWIP'
    end
  end
  object dsGetInfoStock: TDataSource
    DataSet = cdsGetInfoStock
    Left = 491
    Top = 107
  end
  object CdsCekExists: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@SONo'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@ProductCode'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOCekExists'
    RemoteServer = DComData
    Left = 544
    Top = 112
  end
  object CdsCekSJDt: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@SONo'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@Product'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOCheckSJDt'
    RemoteServer = DComData
    Left = 600
    Top = 160
  end
  object CdsCekItemSJ: TClientDataSet
    Aggregates = <>
    Params = <
      item
        DataType = ftInteger
        Name = 'Result'
        ParamType = ptResult
      end
      item
        DataType = ftString
        Name = '@Nmbr'
        ParamType = ptInput
      end
      item
        DataType = ftString
        Name = '@ProductCode'
        ParamType = ptInput
      end>
    ProviderName = 'dpSOCekItemSJ'
    RemoteServer = DComData
    Left = 384
    Top = 32
  end
end
