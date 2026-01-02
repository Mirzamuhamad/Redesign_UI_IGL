Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class SuppInv
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select DISTINCT TransNmbr, Nmbr, Status, TransDate, FgReport, Supplier_Code, Supplier_Name, Supplier, Attn, PONo, Term, Term_Name, DueDate, SuppInvNo, ContraBonNo, ContraBonDate, PPnNo, PPnDate, PriceIncludePPn, PPnRate, Remark, Currency,ForexRate, BaseForex, DiscForex, PPn, PPnForex, PPhForex, OtherForex, TotalForex, DPForex, PPNHome From V_FNSuppInvPKSHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                Dim dt As DataTable
                
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString

                ddlRow.SelectedValue = "20"
                lbCount.Text = SQLExecuteScalar("SELECT COUNT(PO_No) FROM V_FNSuppInvGetRRPks ", ViewState("DBConnection").ToString)
                If Not Request.QueryString("transid") Is Nothing Then
                    If Request.QueryString("transid").ToString.Length > 1 Then
                        ddlRange.SelectedValue = "0"
                        CurrFilter = tbFilter.Text
                        Value = ddlField.SelectedValue
                        tbFilter.Text = Request.QueryString("transid").ToString
                        ddlField.SelectedValue = "TransNmbr"
                        btnSearch_Click(Nothing, Nothing)
                        tbFilter.Text = CurrFilter
                        ddlField.SelectedValue = Value
                    End If
                End If

            End If

            'lbPriceSup.Visible = True
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then


                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    tbCostCtr.Text = Session("Result")(2).ToString
                End If


                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(3).ToString)
                    BindToDropList(ddlTerm, Session("Result")(4).ToString)
                    ' If rbAgingDate.SelectedIndex = 0 Then
                    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                    ' Else
                    'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbCBDate.SelectedDate, ViewState("DBConnection").ToString)
                    ' End If
                    'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ' rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'If Session("Result")(5).ToString = "Y" Then
                    '    tbPPN.Text = "10"
                    'Else
                    '    tbPPN.Text = "0"
                    'End If


                End If

                If ViewState("Sender") = "btnDPNo" Then
                    'ResultField = "DP_No, Currency, ForexRate, PPnRate, PPn, BaseForex, PPNForex, TotalForex, BasePaid, PPNPaid, TotalPaid "
                    tbDPNo.Text = Session("Result")(0).ToString
                    tbDPCurrency.Text = Session("Result")(1).ToString
                    ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(tbDPCurrency.Text), ViewState("DBConnection"))
                    tbDPRate.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitRate"))
                    tbDPPPnRate.Text = FormatFloat(Session("Result")(3).ToString, ViewState("DigitRate"))
                    tbDPPPnPercent.Text = FormatFloat(Session("Result")(4).ToString, ViewState("DigitPercent"))
                    tbDPBase.Text = FormatFloat(Session("Result")(5).ToString, ViewState("DigitCurrDt2"))
                    tbDPPPn.Text = FormatFloat(Session("Result")(6).ToString, ViewState("DigitCurrDt2"))
                    tbDPTotal.Text = FormatFloat(Session("Result")(7).ToString, ViewState("DigitCurrDt2"))
                    tbPaidBase.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitCurrDt2"))
                    tbPaidPPN.Text = FormatFloat(Session("Result")(9).ToString, ViewState("DigitCurrDt2"))
                    tbPaidTotal.Text = FormatFloat(Session("Result")(10).ToString, ViewState("DigitCurrDt2"))
                    tbDPBaseForex.Text = FormatFloat(CFloat(tbDPBase.Text) - CFloat(tbPaidBase.Text), ViewState("DigitCurrDt2"))
                    tbDPPPnForex.Text = FormatFloat(CFloat(tbDPPPn.Text) - CFloat(tbPaidPPN.Text), ViewState("DigitCurrDt2"))
                    tbDPTotalForex.Text = FormatFloat(CFloat(tbDPBaseForex.Text) + CFloat(tbDPPPnForex.Text), ViewState("DigitCurrDt2"))
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    If TbEditPrice.Text > 0 Then
                        GridDt_RowDeleting2()
                    End If

                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            'BindToDropList(ddlReport, drResult("Report")) ddlReport.SelectedValue
                            'ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) // FgReport sudah tidak ada pengaruh
                            BindToText(tbSuppCode, drResult("Supplier_Code"))
                            BindToText(tbSuppName, drResult("Supplier_Name"))
                            BindToText(tbAttn, drResult("Attn"))
                            BindToText(tbPONo, drResult("RR_Date"))
                            BindToDropList(ddlTerm, drResult("Term"))
                            BindToDropList(ddlCurr, drResult("Currency"))
                            BindToDropList(ddlFgPriceInclude, drResult("FgPriceIncludePPN"))
                            BindToText(tbPPN, drResult("PPn"))
                            'If rbAgingDate.SelectedIndex = 0 Then
                            'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                            'Else
                            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                            'End If
                            BindToText(tbRemark, drResult("RemarkHd"))

                        End If

                        If CekExistData(ViewState("Dt"), "ReffType,ReffNmbr,Product,CostCtr", drResult("RR_Type") + "|" + drResult("RR_No") + "|" + drResult("Product_Code") + "|" + drResult("Cost_Ctr")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ReffType") = drResult("RR_Type")
                            dr("ReffNmbr") = drResult("RR_No")
                            ViewState("FgReport") = drResult("Report")
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("CostCtr") = drResult("Cost_Ctr")
                            dr("UnitOrder") = drResult("Unit_Order")
                            dr("Unit") = drResult("Unit")
                            dr("QtyOrder") = drResult("Qty_Order")
                            dr("Qty") = drResult("Qty")
                            If TbEditPrice.Text = 0 Then
                                dr("PriceForex") = drResult("Price_Forex")
                                dr("BrutoForex") = drResult("Bruto_Forex")
                                dr("Disc") = drResult("Disc")
                                dr("DiscForex") = drResult("Disc_Forex")
                                dr("NettoForex") = dr("BrutoForex")
                                dr("PPh") = drResult("PPh")
                                dr("PPhForex") = drResult("PPh_Forex")
                            Else
                                dr("PriceForex") = TbEditPrice.Text
                                dr("BrutoForex") = FormatFloat(CFloat(drResult("Qty_Order") * dr("PriceForex")), ViewState("DigitCurrDt2"))
                                dr("Disc") = drResult("Disc")
                                dr("DiscForex") = drResult("Disc_Forex")
                                dr("NettoForex") = FormatFloat(CFloat(dr("BrutoForex") - dr("DiscForex")), ViewState("DigitCurrDt2"))
                                dr("PPh") = drResult("PPh")
                                dr("PPhForex") = FormatFloat(CFloat(dr("NettoForex") * dr("PPh")) / 100, ViewState("DigitCurrDt2"))
                                'FormatFloat(CFloat(tbDPBaseForex.Text) + CFloat(tbDPPPnForex.Text), ViewState("DigitCurrDt2"))
                            End If
                            'dr("PriceForex") = drResult("Price_Forex")

                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                        'ViewState("DiscHd") = FormatNumber(drResult("Disc_Hd").ToString, ViewState("DigitPercent"))
                    Next
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'ChangeReport("Add", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
                    tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
                    tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
                    tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
                    If Not (tbPPndate.IsNull) Then
                        If ddlCurr.SelectedValue <> Session("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                            tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
                        End If
                    End If
                    tbPPnNo.Enabled = False
                    tbPPndate.Enabled = False
                    BindGridDt(ViewState("Dt"), GridDt)
                    GenerateDP()
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
                    'tbDisc.Text = ViewState("DiscHd")
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'If drResult("Product_Code") = "" Then
                        '    'lbPriceSup.Visible = True
                        '    lbStatus.Text = " Product & Price with supplier , " + drResult("Supplier_Code") + " Please insert "
                        '    Exit Sub
                        'End If
                        'insert
                        If FirstTime Then
                            BindToText(tbSuppCode, drResult("Supplier_Code").ToString)
                            BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                            tbSuppCode_TextChanged(Nothing, Nothing)
                            BindToText(tbPONo, drResult("RR_date").ToString)
                            BindToDropList(ddlFgPriceInclude, drResult("FgPriceIncludePPN"))
                            BindToText(tbPPN, drResult("PPn"))
                            BindToText(tbRemark, drResult("RemarkHd"))

                        End If

                        If CekExistData(ViewState("Dt"), "ReffType,ReffNmbr,Product,CostCtr", drResult("RR_Type") + "|" + drResult("RR_No") + "|" + drResult("Product_Code") + "|" + drResult("Cost_Ctr")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ReffType") = drResult("RR_Type")
                            dr("ReffNmbr") = drResult("RR_No")
                            ViewState("FgReport") = drResult("Report")
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("CostCtr") = drResult("Cost_Ctr")
                            dr("UnitOrder") = drResult("Unit_Order")
                            dr("Unit") = drResult("Unit")
                            dr("QtyOrder") = drResult("Qty_Order")
                            dr("Qty") = drResult("Qty")
                            dr("PriceForex") = drResult("Price_Forex")
                            dr("BrutoForex") = drResult("Bruto_Forex")
                            dr("Disc") = drResult("Disc")
                            dr("DiscForex") = drResult("Disc_Forex")
                            dr("NettoForex") = drResult("Netto_Forex")
                            dr("PPh") = drResult("PPh")
                            dr("PPhForex") = drResult("PPh_Forex")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'ChangeReport("Add", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
                    tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
                    tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
                    tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
                    If Not (tbPPndate.IsNull) Then
                        If ddlCurr.SelectedValue <> Session("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                            tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
                        End If
                    End If
                    tbPPnNo.Enabled = False
                    tbPPndate.Enabled = False
                    'BindGridDt(ViewState("Dt"), GridDt)
                    GenerateDP()
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing

                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
                'Session("CriteriaField") = Nothing                
            End If

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub InitProperty()
        ViewState("DBConnection") = Session(Request.QueryString("KeyId"))("DBConnection")
        ViewState("UserId") = Session(Request.QueryString("KeyId"))("UserId")
        ViewState("UserName") = Session(Request.QueryString("KeyId"))("UserName")
        ViewState("FgAdmin") = Session(Request.QueryString("KeyId"))("FgAdmin")
        ViewState("Currency") = Session(Request.QueryString("KeyId"))("Currency")
        ViewState("GLYear") = Session(Request.QueryString("KeyId"))("Year")
        ViewState("GLPeriod") = Session(Request.QueryString("KeyId"))("Period")
        ViewState("GLPeriodName") = Session(Request.QueryString("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Request.QueryString("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Request.QueryString("KeyId"))("Address1")
        ViewState("Address2") = Session(Request.QueryString("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Request.QueryString("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Request.QueryString("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Request.QueryString("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub
    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("FgReport") = "N"
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        Me.tbBrutoForex.Attributes.Add("ReadOnly", "True")
        Me.tbNettoForex.Attributes.Add("ReadOnly", "True")

        'Me.tbPrice.Attributes.Add("ReadOnly", "True") //Wayan minta bukain biar bisa input data January (20150422)
        Me.tbDiscDt.Attributes.Add("ReadOnly", "True")
        Me.tbDiscDtForex.Attributes.Add("ReadOnly", "True")
        Me.tbPPhDtForex.Attributes.Add("ReadOnly", "True")
        ' Me.tbQtyWrhs.Attributes.Add("ReadOnly", "True")

        Me.tbBaseForex.Attributes.Add("ReadOnly", "True")
        Me.tbDiscForex.Attributes.Add("ReadOnly", "True")
        'Me.tbPPN.Attributes.Add("ReadOnly", "True")
        Me.tbPPNForex.Attributes.Add("ReadOnly", "True")
        Me.tbPPhForex.Attributes.Add("ReadOnly", "True")
        Me.tbTotalForex.Attributes.Add("ReadOnly", "True")
        Me.tbDPForex.Attributes.Add("ReadOnly", "True")
        Me.tbPPN.Attributes.Add("ReadOnly", "True")

        Me.tbDPTotalForex.Attributes.Add("ReadOnly", "True")

        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbOtherForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Me.TbEditPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscDtForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPhDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDPBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDPPPnForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Me.tbBaseForex.Attributes.Add("OnBlur", "BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        Me.tbDiscForex.Attributes.Add("OnBlur", "BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'%'); setformat();")
        Me.tbDiscForex.Attributes.Add("OnBlur", "BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        Me.tbPPN.Attributes.Add("OnBlur", "BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        Me.tbOtherForex.Attributes.Add("OnBlur", "BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")

        'tbBaseForex.Attributes.Add("OnBlur", "setformat();")
        'tbDiscForex.Attributes.Add("OnBlur", "setformat();")
        'tbPPN.Attributes.Add("OnBlur", "setformat();")
        'tbPPNForex.Attributes.Add("OnBlur", "setformat();")
        'tbDPForex.Attributes.Add("OnBlur", "setformat();")
        'tbPPhForex.Attributes.Add("OnBlur", "setformat();")
        'tbTotalForex.Attributes.Add("OnBlur", "setformat();")

        Me.tbPrice.Attributes.Add("OnBlur", "QtyPriceBrutoDiscNettoPPhGrossUp(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbBrutoForex.ClientID + "," + tbDiscDt.ClientID + "," + tbDiscDtForex.ClientID + "," + tbNettoForex.ClientID + "," + tbPPhDt.ClientID + "," + tbPPhDtForex.ClientID + ",'-'); setformatdt();")
        Me.tbDiscDt.Attributes.Add("OnBlur", "QtyPriceBrutoDiscNettoPPhGrossUp(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbBrutoForex.ClientID + "," + tbDiscDt.ClientID + "," + tbDiscDtForex.ClientID + "," + tbNettoForex.ClientID + "," + tbPPhDt.ClientID + "," + tbPPhDtForex.ClientID + ",'%'); setformatdt();")
        Me.tbDiscDtForex.Attributes.Add("OnBlur", "QtyPriceBrutoDiscNettoPPhGrossUp(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbBrutoForex.ClientID + "," + tbDiscDt.ClientID + "," + tbDiscDtForex.ClientID + "," + tbNettoForex.ClientID + "," + tbPPhDt.ClientID + "," + tbPPhDtForex.ClientID + ",'-'); setformatdt();")
        Me.tbPPhDt.Attributes.Add("OnBlur", "QtyPriceBrutoDiscNettoPPhGrossUp(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbBrutoForex.ClientID + "," + tbDiscDt.ClientID + "," + tbDiscDtForex.ClientID + "," + tbNettoForex.ClientID + "," + tbPPhDt.ClientID + "," + tbPPhDtForex.ClientID + ",'-'); setformatdt();")

        Me.tbDPBaseForex.Attributes.Add("OnBlur", "setformatdt2();")
        Me.tbDPPPnForex.Attributes.Add("OnBlur", "setformatdt2();")
    End Sub

    Protected Sub GenerateDP()
        Try
            ' hapus semuanya 
            Dim drDP As DataRow()
            Dim StringRR As String
            drDP = ViewState("Dt2").Select("")
            For j As Integer = 0 To (drDP.Count - 1)
                drDP(j).Delete()
            Next
            ' insert ulang yg baru
            StringRR = ""
            For Each dr In ViewState("Dt").Rows
                If Not (dr.RowState = DataRowState.Deleted) Then
                    StringRR = StringRR + ", " + QuotedStr(dr("ReffNmbr").ToString)
                End If
            Next
            If StringRR.Length >= 4 Then
                Dim P As Integer
                P = StringRR.Length
                StringRR = StringRR.Substring(2, P - 2)
                Dim dt As DataTable
                dt = SQLExecuteQuery("EXEC S_FNSuppInvGenerateDP " + QuotedStr(tbPONo.Text) + ", " + QuotedStr(StringRR) + "," + QuotedStr(tbSuppCode.Text) + "," + tbPPN.Text.Replace(",", ""), ViewState("DBConnection").ToString).Tables(0)
                For Each drresult In dt.Rows
                    Dim drNew As DataRow
                    drNew = ViewState("Dt2").NewRow
                    drNew("DPNo") = drresult("DP_No")
                    drNew("Currency") = drresult("Currency")
                    ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(drresult("Currency")), ViewState("DBConnection"))
                    drNew("ForexRate") = FormatFloat(drresult("ForexRate"), ViewState("DigitCurr"))
                    drNew("PPn") = FormatFloat(drresult("PPn"), ViewState("DigitPercent"))
                    drNew("PPnRate") = FormatFloat(drresult("PPnRate"), ViewState("DigitCurr"))
                    drNew("Remark") = ""
                    drNew("DPBase") = FormatFloat(CFloat(drresult("BaseForex")), ViewState("DigitCurrDt2"))
                    drNew("DPPPn") = FormatFloat(CFloat(drresult("PPnForex")), ViewState("DigitCurrDt2"))
                    drNew("DPTotal") = FormatFloat(CFloat(drresult("BaseForex")) + CFloat(drresult("PPnForex")), ViewState("DigitCurrDt2"))
                    drNew("PaidBase") = FormatFloat(CFloat(drresult("BasePaid")), ViewState("DigitCurrDt2"))
                    drNew("PaidPPn") = FormatFloat(CFloat(drresult("PPnPaid")), ViewState("DigitCurrDt2"))
                    drNew("PaidTotal") = FormatFloat(CFloat(drresult("BasePaid")) + CFloat(drresult("PPnPaid")), ViewState("DigitCurrDt2"))
                    drNew("BaseForex") = FormatFloat(CFloat(drresult("BaseToPaid")), ViewState("DigitCurrDt2"))
                    drNew("PPnForex") = FormatFloat(CFloat(drresult("PPnToPaid")), ViewState("DigitCurrDt2"))
                    drNew("TotalForex") = FormatFloat(CFloat(drresult("BaseToPaid")) + CFloat(drresult("PPnToPaid")), ViewState("DigitCurrDt2"))
                    ViewState("Dt2").Rows.Add(drNew)
                Next
            End If
            BindGridDt(ViewState("Dt2"), GridDt2)
        Catch ex As Exception
            Throw New Exception("GenerateDP Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                'BtnGo.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
            If DT.Rows.Count > 0 Then
                GridView1.HeaderRow.Cells(19).Text = "PPn (" + ViewState("Currency") + ")"
            End If

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNSuppInvPKSDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDP(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNSuppInvPKSDP WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNSuppInvPKS", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next

            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'ddlReport.Enabled = State
            'tbRef.Enabled = State
            tbSuppCode.Enabled = State
            ddlReport.Enabled = State
            btnSupp.Visible = State
            ddlFgPriceInclude.Enabled = State
            'ddlGrossUpPPh.Enabled = State
            'tbPONo.Enabled = State
            ddlCurr.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDP(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDP(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
            ClearHd()
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbRRType.Text + "|" + tbRRNo.Text + "|" + tbProductCode.Text + "|" + tbCostCtr.Text Then
                    If CekExistData(ViewState("Dt"), "ReffType,ReffNmbr,Product,CostCtr", tbRRType.Text + "|" + tbRRNo.Text + "|" + tbProductCode.Text + "|" + tbCostCtr.Text) Then
                        lbStatus.Text = "RR No " + tbRRNo.Text + " Product " + tbProductName.Text + " Cost Ctr " + tbCostCtr.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ReffType+'|'+ReffNmbr+'|'+Product+'|'+CostCtr = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ReffType") = tbRRType.Text
                Row("ReffNmbr") = tbRRNo.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("CostCtr") = tbCostCtr.Text
                Row("QtyOrder") = tbQty.Text
                Row("UnitOrder") = tbUnit.Text
                If Row("UnitOrder") = "" Then
                    Row("UnitOrder") = DBNull.Value
                End If
                Row("Qty") = tbQtyWrhs.Text
                Row("Unit") = tbUnitWrhs.Text
                Row("Remark") = tbRemarkDt.Text
                Row("PriceForex") = tbPrice.Text
                Row("BrutoForex") = tbBrutoForex.Text
                Row("Disc") = tbDiscdt.Text
                Row("DiscForex") = tbdiscdtForex.Text
                Row("NettoForex") = tbNettoForex.Text
                Row("PPh") = tbPPhDt.Text
                Row("PPhForex") = tbPPhDtForex.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ReffType,ReffNmbr,Product,CostCtr", tbRRType.Text + "|" + tbRRNo.Text + "|" + tbProductCode.Text + "|" + tbCostCtr.Text) = True Then
                    lbStatus.Text = "RR No " + tbRRNo.Text + " Product " + tbProductName.Text + " Cost Ctr " + tbCostCtr.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ReffType") = tbRRType.Text
                dr("ReffNmbr") = tbRRNo.Text
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("CostCtr") = tbCostCtr.Text
                dr("QtyOrder") = tbQty.Text
                dr("UnitOrder") = tbUnit.Text
                If dr("UnitOrder") = "" Then
                    dr("UnitOrder") = DBNull.Value
                End If
                dr("Qty") = tbQtyWrhs.Text
                dr("Unit") = tbUnitWrhs.Text
                dr("Remark") = tbRemarkDt.Text
                dr("PriceForex") = tbPrice.Text
                dr("BrutoForex") = tbBrutoForex.Text
                dr("Disc") = tbDiscdt.Text
                dr("DiscForex") = tbdiscdtForex.Text
                dr("NettoForex") = tbNettoForex.Text
                dr("PPh") = tbPPhDt.Text
                dr("PPhForex") = tbPPhDtForex.Text
                ViewState("Dt").Rows.Add(dr)
                'Dt = ViewState("Dt")
                'a = Dt.Compute("Sum(AmountForex)", "")                
                'tbBaseForex.Text = a.ToString
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer

        Try
            If pnlEditDt.Visible = True Or pnlEditDt2.Visible = True Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If

            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbRef.Text = GetAutoNmbr("SIPKS", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINSuppInvPKSHd (TransNmbr, FgReport, Status, TransDate, Supplier, Attn, PONo, " + _
                "Term, DueDate, SuppInvNo, FgPriceIncludeTax, PPnNo, PPnDate, PPnRate, Currency, ForexRate, BaseForex, DiscForex, PPn, PPnForex, PPhForex, OtherForex, TotalForex, DPForex, " + _
                "FgAgingStartDate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + QuotedStr(ViewState("FgReport").ToString) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbPONo.Text) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbSuppInvNo.Text) + "," + QuotedStr(ddlFgPriceInclude.SelectedValue) + ", " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + tbDiscForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + _
                tbPPNForex.Text.Replace(",", "") + ", " + tbPPhForex.Text.Replace(",", "") + ", " + tbOtherForex.Text.Replace(",", "") + ", " + tbTotalForex.Text.Replace(",", "") + ", " + tbDPForex.Text.Replace(",", "") + ", 0" + _
                ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINSuppInvPKSHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINSuppInvPKSHd SET FgReport = " + QuotedStr(ViewState("FgReport").ToString) + ", Supplier = " + QuotedStr(tbSuppCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", Term = " + QuotedStr(ddlTerm.SelectedValue) + ", PONo = " + QuotedStr(tbPONo.Text) + ", SuppInvNo = " + QuotedStr(tbSuppInvNo.Text) + _
                ", FgPriceIncludeTax = " + QuotedStr(ddlFgPriceInclude.SelectedValue) + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + _
                ", PPnDate = '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + ", DiscForex = " + tbDiscForex.Text.Replace(",", "") + _
                ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", PPhForex = " + tbPPhForex.Text.Replace(",", "") + ", OtherForex = " + tbOtherForex.Text.Replace(",", "") + ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + ", DPForex = " + tbDPForex.Text.Replace(",", "") + _
                ", FgAgingStartDate = 0 , Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If

            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace(", ,", ", NULL ,")
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()
            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Transnmbr, Product, ReffType, ReffNmbr, CostCtr, QtyOrder, UnitOrder, Qty, Unit, PriceForex, BrutoForex, Disc, DiscForex, NettoForex, PPh, PPhForex, Remark FROM FINSuppInvPKSDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINSuppInvPKSDt SET ReffType = @ReffType, ReffNmbr = @ReffNmbr, " + _
            "Product = @Product, CostCtr = @CostCtr, Qty = @Qty, Unit = @Unit, " + _
            "QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Remark = @Remark, " + _
            "PriceForex = @PriceForex, BrutoForex = @BrutoForex, Disc = @Disc, " + _
            "DiscForex = @DiscForex, NettoForex = @NettoForex, " + _
            "PPh = @PPh, PPhForex = @PPhForex " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ReffType = @OldReffType AND ReffNmbr = @OldReffNmbr AND CostCtr = @OldCostCtr AND Product = @OldProduct", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ReffType", SqlDbType.VarChar, 12, "ReffType")
            Update_Command.Parameters.Add("@ReffNmbr", SqlDbType.VarChar, 20, "ReffNmbr")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 10, "CostCtr")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 22, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 22, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 22, "PriceForex")
            Update_Command.Parameters.Add("@BrutoForex", SqlDbType.Float, 22, "BrutoForex")
            Update_Command.Parameters.Add("@Disc", SqlDbType.Float, 18, "Disc")
            Update_Command.Parameters.Add("@DiscForex", SqlDbType.Float, 22, "DiscForex")
            Update_Command.Parameters.Add("@NettoForex", SqlDbType.Float, 22, "NettoForex")
            Update_Command.Parameters.Add("@PPh", SqlDbType.Float, 18, "PPh")
            Update_Command.Parameters.Add("@PPhForex", SqlDbType.Float, 22, "PPhForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldReffType", SqlDbType.VarChar, 12, "ReffType")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldReffNmbr", SqlDbType.VarChar, 20, "ReffNmbr")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldCostCtr", SqlDbType.VarChar, 10, "CostCtr")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINSuppInvPKSDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ReffType = @ReffType AND ReffNmbr = @ReffNmbr AND CostCtr = @CostCtr AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ReffType", SqlDbType.VarChar, 12, "ReffType")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@ReffNmbr", SqlDbType.VarChar, 20, "ReffNmbr")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 10, "CostCtr")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINSuppInvPKSDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            Dim cmdSql2 As New SqlCommand("SELECT  TransNmbr, DPNo, Currency, ForexRate, PPn, PPnRate, DPBase, DPPPn, DPTotal, PaidBase, PaidPPN, PaidTotal, BaseForex, PPnForex, TotalForex, Remark FROM FINSuppInvPKSDP WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql2)
            Dim dbcommandBuilder2 As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder2.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder2.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder2.GetUpdateCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
            "UPDATE FINSuppInvPKSDP SET DPNo = @DPNo, Currency = @Currency, " + _
            "ForexRate = @ForexRate, PPn = @PPn, PPnRate = @PPnRate, " + _
            "DPBase = @DPBase, DPPPn = @DPPPn, DPTotal = @DPTotal, " + _
            "PaidBase = @PaidBase, PaidPPN = @PaidPPN, PaidTotal = @PaidTotal, " + _
            "BaseForex = @BaseForex, PPnForex = @PPnForex, TotalForex = @TotalForex, Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND DPNo = @OldDPNo", con) '"DPInvoice = @DPInvoice " + _

            ' Define output parameters.
            Update_Command2.Parameters.Add("@DPNo", SqlDbType.VarChar, 20, "DPNo")
            Update_Command2.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command2.Parameters.Add("@ForexRate", SqlDbType.Float, 18, "ForexRate")
            Update_Command2.Parameters.Add("@PPn", SqlDbType.Float, 18, "PPn")
            Update_Command2.Parameters.Add("@PPnRate", SqlDbType.Float, 18, "PPnRate")
            Update_Command2.Parameters.Add("@DPBase", SqlDbType.Float, 22, "DPBase")
            Update_Command2.Parameters.Add("@DPPPn", SqlDbType.Float, 22, "DPPPn")
            Update_Command2.Parameters.Add("@DPTotal", SqlDbType.Float, 22, "DPTotal")
            Update_Command2.Parameters.Add("@PaidBase", SqlDbType.Float, 22, "PaidBase")
            Update_Command2.Parameters.Add("@PaidPPN", SqlDbType.Float, 22, "PaidPPN")
            Update_Command2.Parameters.Add("@PaidTotal", SqlDbType.Float, 22, "PaidTotal")
            Update_Command2.Parameters.Add("@BaseForex", SqlDbType.Float, 22, "BaseForex")
            Update_Command2.Parameters.Add("@PPnForex", SqlDbType.Float, 22, "PPnForex")
            Update_Command2.Parameters.Add("@TotalForex", SqlDbType.Float, 22, "TotalForex")
            Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            'Update_Command2.Parameters.Add("@DPInvoice", SqlDbType.Float, 22, "DPInvoice")

            '' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldDPNo", SqlDbType.VarChar, 20, "DPNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM FINSuppInvPKSDP WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND DPNo = @DPNo ", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@DPNo", SqlDbType.VarChar, 20, "DPNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("FINSuppInvPKSDP")
            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            SaveAll()

            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            'ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
            tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
            tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            btnHome.Visible = False
            tbPPnNo.Enabled = False
            tbPPndate.Enabled = False
            TbEditPrice.Enabled = True
            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
            'ddlReport.Focus()            
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            ClearDP()
            btnSupp.Visible = True
            tbSuppCode.Enabled = True
            'ddlCurr.SelectedValue = Session("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, Session("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDP("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            'ddlReport.SelectedValue = "Y"
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            'tbCBDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbAttn.Text = ""
            ddlTerm.SelectedIndex = 0
            'ddlGrossUpPPh.SelectedValue = "N"
            tbPONo.Text = ""
            tbSuppInvNo.Text = ""
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
            tbPpnRate.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, Session("DBConnection").ToString)
            tbRate.Text = "0"
            tbBaseForex.Text = "0"
            'tbDisc.Text = "0"
            tbDiscForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbOtherForex.Text = "0"
            tbTotalForex.Text = "0"
            tbDPForex.Text = "0"
            tbRemark.Text = ""
            TbEditPrice.Text = "0"
            
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbCostCtr.Text = ""
            tbUnitWrhs.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            tbQtyWrhs.Text = "0"
            tbPrice.Text = "0"
            tbBrutoForex.Text = "0"
            tbDiscDt.Text = "0"
            tbDiscDtForex.Text = "0"
            tbNettoForex.Text = "0"
            tbPPhDt.Text = "0"
            tbPPhDtForex.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearDP()
        Try
            tbDPNo.Text = ""
            tbDPCurrency.Text = ViewState("Currency")
            tbDPRate.Text = FormatFloat("1", ViewState("DigitRate"))
            tbDPPPnRate.Text = FormatFloat("1", ViewState("DigitRate"))
            tbDPPPnPercent.Text = FormatFloat("0", ViewState("DigitPercent"))
            tbDPBase.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPPPn.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPTotal.Text = FormatFloat("0", ViewState("DigitHome"))
            tbPaidBase.Text = FormatFloat("0", ViewState("DigitHome"))
            tbPaidPPN.Text = FormatFloat("0", ViewState("DigitHome"))
            tbPaidTotal.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPBaseForex.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPPPnForex.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPTotalForex.Text = FormatFloat("0", ViewState("DigitHome"))
            tbDPRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If tbPONo.Text.Trim = "" Then
                lbStatus.Text = "PO No must have value"
                tbPONo.Focus()
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ViewState("StateHd") = "Insert"
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "select * from VMsSupplier"
            ResultField = "Supplier_Code, Supplier_Name, Currency, Contact_Person, Term, PPN"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                BindToDropList(ddlCurr, Dr("Currency"))
                BindToText(tbAttn, Dr("Contact_Person"))
                BindToDropList(ddlTerm, Dr("Term"))
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)

                'If Dr("Reported") = "Y" Then
                '    tbPPN.Text = "10"
                'Else
                '    tbPPN.Text = "0"
                'End If

            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                ddlCurr.SelectedValue = Session("Currency")
                tbAttn.Text = ""
                tbPPN.Text = "0"
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            AttachScript("setformat();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, Session("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
        tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
        tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
        tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
        If Not (tbPPndate.IsNull) Then
            If tbPPndate.SelectedValue.ToString <> "" Then
                tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            End If
        End If

        AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbProductCode.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = "RR No must have value"
            '    tbRef.Focus()
            '    Return False
            'End If
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("RR Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbSuppInvNo.Text.Trim = "" Then
                lbStatus.Text = "Supplier Invoice must have value"
                tbSuppInvNo.Focus()
                Return False
            End If

            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Exit Function
            'End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnSupp.Focus()
                Return False
            End If
            If tbPONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PO No must have value")
                btnGetData.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ReffType").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("RR Type Must Have Value")
                    Return False
                End If
                If Dr("ReffNmbr").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("RR No Must Have Value")
                    Return False
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("CostCtr").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Ctr Must Have Value")
                    Return False
                End If
                If Dr("UnitOrder").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyOrder").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    Return False
                End If
            Else
                If tbRRType.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("RR Type Must Have Value")
                    tbRRType.Focus()
                    Return False
                End If
                If tbRRNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("RR No Must Have Value")
                    tbRRType.Focus()
                    Return False
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If tbCostCtr.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Ctr Must Have Value")
                    tbCostCtr.Focus()
                    Return False
                End If
                If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    tbUnit.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtyWrhs.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQtyWrhs.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            FDateName = "TT Date, Invoice Date, Due Date"
            FDateValue = "TransDate, SuppInvDate, DueDate"
            FilterName = "Reference, Date, Supplier, PO No, Supplier Invoice, RR No, SJ Supplier No, Currency, Term, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Supplier, PONo, SuppInvNo, RR_No, SJ_Supp_No, Currency, Term_Name, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim cekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    'ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDP(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    TbEditPrice.Enabled = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        cekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDP(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        tbRate.Enabled = Not ddlCurr.SelectedValue = ViewState("Currency")
                        btnHome.Visible = False
                        tbPPnNo.Enabled = False
                        tbPPndate.Enabled = False
                        TbEditPrice.Enabled = True
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, Session("DBConnection").ToString)
                        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        cekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormSuppInvPKS " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormSuppInvPKS.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting2()
        Dim dr As DataRow()
        Dim r As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(Nothing)
        dr = ViewState("Dt").Select("ReffType+'|'+Product = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(3).Text))
        For Each r In dr
            r.Delete()
        Next
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        If GetCountRecord(ViewState("Dt")) = 0 Then
            tbPONo.Text = ""
        End If

    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr As DataRow()
        Dim r As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ReffType+'|'+ReffNmbr = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text))
        For Each r In dr
            r.Delete()
        Next
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        If GetCountRecord(ViewState("Dt")) = 0 Then
            tbPONo.Text = ""
        End If
        GenerateDP()
    End Sub
 

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + TrimStr(GVR.Cells(3).Text) + "|" + TrimStr(GVR.Cells(5).Text)
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbSuppCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTerm')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Term Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbPriceSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPriceSup.Click
        Try
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenTransaction('TrPriceListSupplier','TrPriceListSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Term Error : " + ex.ToString
        End Try
    End Sub



    Dim BaseForex As Decimal = 0
    Dim DiscForex As Decimal = 0
    Dim PPhForex As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BrutoForex"))
                    DiscForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DiscForex"))
                    PPhForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PPhForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'If ddlCurr.SelectedValue = ViewState("Currency") Then
                    '    BaseForex = Math.Floor(BaseForex)
                    '    DiscForex = Math.Floor(DiscForex)
                    'End If
                    If ddlFgPriceInclude.SelectedValue = "N" Then
                        tbBaseForex.Text = BaseForex.ToString 'FormatNumber(BaseForex, CInt(ViewState("DigitCurr")))
                    Else
                        '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
                        tbBaseForex.Text = ((BaseForex - DiscForex) * 100 / (100 + CFloat(tbPPN.Text)) + DiscForex).ToString 'FormatNumber(((BaseForex - DiscForex) * 100 / (100 + CFloat(tbPPN.Text))) + DiscForex, CInt(ViewState("DigitCurr")))
                    End If
                    tbDiscForex.Text = DiscForex.ToString  'FormatNumber(DiscForex, CInt(ViewState("DigitCurr")))
                    tbPPhForex.Text = PPhForex.ToString 'FormatNumber(PPhForex, CInt(ViewState("DigitCurr")))
                    'AttachScript("BaseDiscPPnPPhOtherTotalHd(" + Me.tbBaseForex.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbPPhForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'%'); ", Page, Me.GetType())
                    AttachScript("setformat();", Page, Me.GetType())
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier_Code").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitCurr"))
            BindToText(tbSuppInvNo, Dt.Rows(0)("SuppInvNo").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlFgPriceInclude, Dt.Rows(0)("PriceIncludePPn").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitCurr"))
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            'BindToText(tbDisc, Dt.Rows(0)("Disc").ToString, ViewState("DigitPercent"))
            BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPhForex, Dt.Rows(0)("PPhForex").ToString, ViewState("DigitCurr"))
            BindToText(tbOtherForex, Dt.Rows(0)("OtherForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbDPForex, Dt.Rows(0)("DPForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ReffType+'|'+ReffNmbr+'|'+Product+'|'+CostCtr = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbRRType, Dr(0)("ReffType").ToString)
                BindToText(tbRRNo, Dr(0)("ReffNmbr").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbCostCtr, Dr(0)("CostCtr").ToString)
                BindToText(tbQty, Dr(0)("QtyOrder").ToString)
                BindToText(tbUnit, Dr(0)("UnitOrder").ToString)
                BindToText(tbQtyWrhs, Dr(0)("Qty").ToString)
                BindToText(tbUnitWrhs, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbPrice, Dr(0)("PriceForex").ToString)
                BindToText(tbBrutoForex, Dr(0)("BrutoForex").ToString)
                BindToText(tbDiscDt, Dr(0)("Disc").ToString)
                BindToText(tbDiscDtForex, Dr(0)("DiscForex").ToString)
                BindToText(tbNettoForex, Dr(0)("NettoForex").ToString)
                BindToText(tbPPhDt, Dr(0)("PPh").ToString)
                BindToText(tbPPhDtForex, Dr(0)("PPhForex").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDP(ByVal DPNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("DPNo = " + QuotedStr(DPNo))
            If Dr.Length > 0 Then
                BindToText(tbDPNo, Dr(0)("DPNo").ToString)
                BindToText(tbDPCurrency, Dr(0)("Currency").ToString)
                ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(tbDPCurrency.Text), ViewState("DBConnection"))
                BindToText(tbDPRate, Dr(0)("ForexRate").ToString)
                BindToText(tbDPPPnRate, Dr(0)("PPnRate").ToString)
                BindToText(tbDPPPnPercent, Dr(0)("PPn").ToString)
                BindToText(tbDPBase, Dr(0)("DPBase").ToString)
                BindToText(tbDPPPn, Dr(0)("DPPPn").ToString)
                BindToText(tbDPTotal, Dr(0)("DPTotal").ToString)
                BindToText(tbPaidBase, Dr(0)("PaidBase").ToString)
                BindToText(tbPaidPPN, Dr(0)("PaidPPn").ToString)
                BindToText(tbPaidTotal, Dr(0)("PaidTotal").ToString)
                BindToText(tbDPBaseForex, Dr(0)("BaseForex").ToString)
                BindToText(tbDPPPnForex, Dr(0)("PPnForex").ToString)
                BindToText(tbDPTotalForex, Dr(0)("TotalForex").ToString)
                BindToText(tbDPRemarkDt, Dr(0)("Remark").ToString)
                ViewState("DtDPNo") = tbDPNo.Text
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
        btnGetData.Visible = Bool
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
    'End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        ' S_FNSuppInvGetRR()
        Dim ResultField, ResultSame, Filter As String
        Dim CriteriaField As String
        Try
            Session("Result") = Nothing
            Filter = ""
            If tbSuppCode.Text.Trim <> "" Then
                Filter = Filter + " AND Supplier_Code = " + QuotedStr(tbSuppCode.Text)
            End If
            If tbPONo.Text.Trim <> "" Then
                Filter = Filter + " AND RR_Date = " + QuotedStr(tbPONo.Text)
            End If
            'If ViewState("StateHd") <> "Insert" Then
            '    Filter = Filter + " AND Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
            'End If

            Session("Filter") = "EXEC S_FNSuppInvGetRRPKS " + QuotedStr(Filter)
            ResultField = "RR_Type, RR_No,IKP,IKPName, RR_Date, Report, Supplier_Code, Supplier_Name, Attn, PO_No, Term, Currency, Product_Code, Product_Name, Cost_Ctr, Qty_Order, Unit_Order, Qty, Unit, Price_Forex, Bruto_Forex, Disc, Disc_Forex, PPn, PPn_Forex, PPh, PPh_Forex, Netto_Forex, FgPriceIncludePPN, RemarkHd"
            CriteriaField = "RR_Type, RR_No,IKP,IKPName, RR_Date, Report, Supplier_Code, Supplier_Name,	Attn,	PO_No,	Term, Currency, Supplier_SJ_No,	Supplier_SJ_Date "

            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "IKP,Supplier_Name,RR_Date"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)

    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged

        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
        
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDP2.Click, btnAddDPKe2.Click
        Try
            ClearDP()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbDPNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add DP error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            'If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text.Trim) Then

            If ViewState("StateDt2") = "Edit" Then
                If ViewState("DtDPNo") <> tbDPNo.Text Then
                    If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text) Then
                        lbStatus.Text = "DP No " + tbDPNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("DPNo = " + QuotedStr(tbDPNo.Text))(0)
                Row.BeginEdit()
                Row("Currency") = tbDPCurrency.Text
                Row("ForexRate") = FormatFloat(tbDPRate.Text, ViewState("DigitCurr"))
                Row("PPnRate") = FormatFloat(tbDPPPnRate.Text, ViewState("DigitCurr"))
                Row("PPn") = FormatFloat(tbDPPPnPercent.Text, ViewState("DigitPercent"))
                Row("DPBase") = FormatFloat(tbDPBase.Text, ViewState("DigitCurrDt2"))
                Row("DPPPN") = FormatFloat(tbDPPPn.Text, ViewState("DigitCurrDt2"))
                Row("DPTotal") = FormatFloat(tbDPTotal.Text, ViewState("DigitCurrDt2"))
                Row("PaidBase") = FormatFloat(tbPaidBase.Text, ViewState("DigitCurrDt2"))
                Row("PaidPPN") = FormatFloat(tbPaidPPN.Text, ViewState("DigitCurrDt2"))
                Row("PaidTotal") = FormatFloat(tbPaidTotal.Text, ViewState("DigitCurrDt2"))
                Row("BaseForex") = FormatFloat(tbDPBaseForex.Text, ViewState("DigitCurrDt2"))
                Row("PPNForex") = FormatFloat(tbDPPPnForex.Text, ViewState("DigitCurrDt2"))
                Row("TotalForex") = FormatFloat(tbDPTotalForex.Text, ViewState("DigitCurrDt2"))
                Row("Remark") = tbDPRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text) Then
                    lbStatus.Text = "DP No " + tbDPNo.Text + " has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("DPNo") = tbDPNo.Text
                dr("Currency") = tbDPCurrency.Text
                dr("ForexRate") = tbDPRate.Text
                dr("PPnRate") = tbDPPPnRate.Text
                dr("PPn") = FormatFloat(tbDPPPnPercent.Text, ViewState("DigitPercent"))
                dr("DPBase") = FormatFloat(tbDPBase.Text, ViewState("DigitCurrDt2"))
                dr("DPPPN") = FormatFloat(tbDPPPn.Text, ViewState("DigitCurrDt2"))
                dr("DPTotal") = FormatFloat(tbDPTotal.Text, ViewState("DigitCurrDt2"))
                dr("PaidBase") = FormatFloat(tbPaidBase.Text, ViewState("DigitCurrDt2"))
                dr("PaidPPN") = FormatFloat(tbPaidPPN.Text, ViewState("DigitCurrDt2"))
                dr("PaidTotal") = FormatFloat(tbPaidTotal.Text, ViewState("DigitCurrDt2"))
                dr("BaseForex") = FormatFloat(tbDPBaseForex.Text, ViewState("DigitCurrDt2"))
                dr("PPNForex") = FormatFloat(tbDPPPnForex.Text, ViewState("DigitCurrDt2"))
                dr("TotalForex") = FormatFloat(tbDPTotalForex.Text, ViewState("DigitCurrDt2"))
                dr("Remark") = tbDPRemarkDt.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalDP As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DPNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalDP += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BaseForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbDPForex.Text = FormatNumber(TotalDP, ViewState("DigitCurrDt2"))
                End If
                AttachScript("setformat();", Page, Me.GetType())
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("DPNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDP(GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDPNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDPNo.Click
        Dim ResultField As String
        Try

            If lblTitle.Text = "Supplier - Note PKS" Then
                Session("filter") = "select DP_No,dbo.FormatDate(DP_Date) AS DP_Date,Supplier,Supplier_Name,Attn, Supp_Invoice, PONo, Currency, dbo.FormatFloat(ForexRate,dbo.DigitCurrRate()) AS ForexRate, dbo.FormatFloat(PPnRate,dbo.DigitCurrRate()) AS PPnRate, " + _
                "dbo.FormatFloat(BaseForex, dbo.DigitCurrForex(Currency)) AS BaseForex, dbo.FormatFloat(PPN,dbo.DigitPercent()) AS PPn, " + _
                "dbo.FormatFloat(PPnForex, dbo.DigitCurrForex(Currency)) AS PPnForex, dbo.FormatFloat(TotalForex, dbo.DigitCurrForex(Currency)) AS TotalForex," + _
                "dbo.FormatFloat(BasePaid, dbo.DigitCurrForex(Currency)) AS BasePaid, dbo.FormatFloat(PPnPaid, dbo.DigitCurrForex(Currency)) AS PPnPaid, dbo.FormatFloat(BasePaid+PPnPaid, dbo.DigitCurrForex(Currency)) AS TotalPaid " + _
                "from V_FNDPSuppForSI WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + "" ' and ( COALESCE(PONo,'') = '' AND Currency = " + QuotedStr(ddlCurr.SelectedValue) + " AND PPN = " + tbPPN.Text
            Else
                Session("filter") = "select DP_No,dbo.FormatDate(DP_Date) AS DP_Date,Supplier,Supplier_Name,Attn, Supp_Invoice, PONo, Currency, dbo.FormatFloat(ForexRate,dbo.DigitCurrRate()) AS ForexRate, dbo.FormatFloat(PPnRate,dbo.DigitCurrRate()) AS PPnRate, " + _
              "dbo.FormatFloat(BaseForex, dbo.DigitCurrForex(Currency)) AS BaseForex, dbo.FormatFloat(PPN,dbo.DigitPercent()) AS PPn, " + _
              "dbo.FormatFloat(PPnForex, dbo.DigitCurrForex(Currency)) AS PPnForex, dbo.FormatFloat(TotalForex, dbo.DigitCurrForex(Currency)) AS TotalForex," + _
              "dbo.FormatFloat(BasePaid, dbo.DigitCurrForex(Currency)) AS BasePaid, dbo.FormatFloat(PPnPaid, dbo.DigitCurrForex(Currency)) AS PPnPaid, dbo.FormatFloat(BasePaid+PPnPaid, dbo.DigitCurrForex(Currency)) AS TotalPaid " + _
              "from V_FNDPSuppForSI WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + " and ( COALESCE(PONo,'') = '' OR PONo = " + QuotedStr(tbPONo.Text) + " ) AND Currency = " + QuotedStr(ddlCurr.SelectedValue) + " AND PPN = " + tbPPN.Text
            End If

          
            ResultField = "DP_No, Currency, ForexRate, PPnRate, PPn, BaseForex, PPNForex, TotalForex, BasePaid, PPNPaid, TotalPaid "
            ViewState("Sender") = "btnDPNo"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search DP Error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub tbPPndate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPndate.SelectionChanged
        Try
            tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
            tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
            tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
            If Not (tbPPndate.IsNull) Then
                tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            End If
        Catch ex As Exception
            lbStatus.Text = "tbPPndate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPPN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPN.TextChanged
        Try
            tbPPnNo.Enabled = CFloat(tbPPN.Text) > 0
            tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
            tbPpnRate.Enabled = CFloat(tbPPN.Text) > 0 And (ddlCurr.SelectedValue <> ViewState("Currency"))
        Catch ex As Exception
            lbStatus.Text = "tbPPN_TextChanged Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbEditPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbEditPrice.TextChanged
        Try

            If TbEditPrice.Text = "" Then
                TbEditPrice.Text = 0
            End If

            btnGetData_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "tbPPN_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, ResultSame, Filter As String
        Dim CriteriaField As String
        Try
            Session("Result") = Nothing
            Filter = ""
            'If tbSuppCode.Text.Trim <> "" Then
            '    Filter = Filter + " AND Supplier_Code = " + QuotedStr(tbSuppCode.Text)
            'End If
            'If tbPONo.Text.Trim <> "" Then
            '    Filter = Filter + " AND PO_No = " + QuotedStr(tbPONo.Text)
            'End If
            'If ViewState("StateHd") <> "Insert" Then
            '    Filter = Filter + " AND Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
            'End If

            Session("Filter") = "EXEC S_FNSuppInvGetRRPKS " + QuotedStr(Filter)
            ResultField = "RR_Type, RR_No,IKP,IKPName,RR_Date, Report, Supplier_Code, Supplier_Name, Attn, PO_No, Term, Currency, Product_Code, Product_Name, Cost_Ctr, Qty_Order, Unit_Order, Qty, Unit, Price_Forex, Bruto_Forex, Disc, Disc_Forex, PPn, PPn_Forex, PPh, PPh_Forex, Netto_Forex, FgPriceIncludePPN, RemarkHd"
            CriteriaField = "RR_Type, RR_No,IKP,IKPName, RR_Date, Report, Supplier_Code, Supplier_Name,	Attn,	PO_No,	Term, Currency, Supplier_SJ_No,	Supplier_SJ_Date "
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "IKPName,Supplier_Name,RR_Date"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField, Resultsame As String
        Dim criteriaField As String
        Try
            Session("Filter") = "SELECT Product_Code, Product_Name, CostCtr FROM VMsProduct"
            ResultField = "Product_Code, Product_Name, CostCtr "
            criteriaField = "Product_Code, Product_Name "
            Session("Column") = ResultField.Split(",")
            Session("criteriaField") = criteriaField.Split(",")
            Resultsame = "Product_Code, Product_Name"
            Session("Resultsame") = Resultsame.Split(",")
            ViewState("Sender") = "btnProduct"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "Button Product Error : " + ex.ToString
        End Try
    End Sub

End Class
