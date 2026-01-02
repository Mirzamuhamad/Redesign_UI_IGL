Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrDPCustReceipt
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Protected GetStringHd As String = "Select * From V_FNDPCustHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
                    tbDPList.Text = ""
                    tbSONo.Text = ""
                End If
                If ViewState("Sender") = "btnDPList" Then
                    tbDPList.Text = Session("Result")(0).ToString
                    BindToDropList(ddlCurr, Session("Result")(1).ToString)
                    BindToText(tbPPN, Session("Result")(2).ToString)
                    BindToText(tbSONo, Session("Result")(3).ToString)
                    BindToText(tbCustCode, Session("Result")(4).ToString)
                    BindToText(tbCustName, Session("Result")(5).ToString)
                    BindToText(tbCustName, Session("Result")(5).ToString)
                    BindToText(tbBaseForex, Session("Result")(7).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
                    ViewState("InputDPListNo") = Session("Result")(0).ToString
                    ViewState("InputAmount") = CFloat(Session("Result")(6).ToString.Replace(",", ""))
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
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
        Dim dt As DataTable
        dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
        lbTitle.Text = dt.Rows(0)("MenuName").ToString

        ViewState("PayType") = ""
        ViewState("SortExpression") = Nothing
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        FillCombo(ddlCurr, "S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlCurr2, "S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlChargeCurr, "S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

        GridDt.Columns(12).HeaderText = "Receipt (" + ViewState("Currency") + ")"
        GridDt.Columns(13).HeaderText = "Charge (" + ViewState("Currency") + ")"
        GridDt.Columns(14).HeaderText = "DP (" + ViewState("Currency") + ")"

        Me.tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbPPNRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")

        Me.tbBaseForex.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        Me.tbPPN.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbChargeHome.Attributes.Add("ReadOnly", "True")
        tbReceiptHome.Attributes.Add("ReadOnly", "True")
        tbDPForex.Attributes.Add("ReadOnly", "True")
        tbDPHome.Attributes.Add("ReadOnly", "True")
        tbRate2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbChargeRate.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbReceiptForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbChargeForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbDPHome.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbRate2.Attributes.Add("OnBlur", "kali(" + Me.tbRate2.ClientID + "," + Me.tbReceiptForex.ClientID + "," + Me.tbReceiptHome.ClientID + "); setformatdt();")
        tbReceiptForex.Attributes.Add("OnBlur", "kali(" + Me.tbRate2.ClientID + "," + Me.tbReceiptForex.ClientID + "," + Me.tbReceiptHome.ClientID + "); setformatdt();")

        tbChargeRate.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRate.ClientID + "," + Me.tbChargeForex.ClientID + "," + Me.tbChargeHome.ClientID + "); setformatdt();")
        tbChargeForex.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRate.ClientID + "," + Me.tbChargeForex.ClientID + "," + Me.tbChargeHome.ClientID + "); setformatdt();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbFilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
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
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "Select TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, Currency, Forexrate, ReceiptForex, ReceiptHome, DPForex, DPHome, Remark, FgMode, BankGiro, DueDate, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome, ReceiptTypeName, BankGiroName from V_FNDPCustDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function


    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCustCode.Enabled = State
            btnCust.Visible = State
            tbDPList.Enabled = State
            btnDPList.Enabled = State
            'tbTransDate.Enabled = State
            'ddlCurr.Enabled = State // tidak boleh diubah. harus sesuai Dp Cust List No
            tbRate.Enabled = State
            'tbPPNRate.Enabled = State
            'tbBaseForex.Enabled = State
            tbPPN.Enabled = State
            'ddlReport.Enabled = State
            'btnGetDt.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            Dim dr As DataRow
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
            For Each dr In dt.Rows
                If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Then
                    ViewState("PayType") = dt.Rows(0)("ReceiptType").ToString
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub SaveAll()
        Dim SQLString As String
        Dim tgl As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            'Save Hd
            If tbPPNDate.IsNull Then
                tgl = "NULL"
            Else
                tgl = QuotedStr(Format(tbPPNDate.SelectedDate, "yyyy-MM-dd"))
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'insert      
                'ddlReport.SelectedValue
                tbTransNo.Text = GetAutoNmbr("DP", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "Insert INTO FINDPCustHd (TransNmbr, Status, TransDate, FgReport, " + _
                "SONo, Customer, DPListNo, Attn, Currency, ForexRate, PPNNo, PPNDate, " + _
                "PPNRate, BaseForex, PPN, PPNForex, TotalForex, Remark, UserPrep, DatePrep, BalanceBase, BalancePPN) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr("Y") + "," + QuotedStr(tbSONo.Text) + "," + _
                QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbDPList.Text) + "," + QuotedStr(tbAttn.Text) + "," + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(tbPPNNo.Text) + "," + _
                tgl + "," + tbPPNRate.Text.Replace(",", "") + "," + _
                tbBaseForex.Text.Replace(",", "") + "," + tbPPN.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate(),0,0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDPCustHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINDPCustHd SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", FgReport=" + QuotedStr("Y") + ", SONo=" + QuotedStr(tbSONo.Text) + ", Customer=" + QuotedStr(tbCustCode.Text) + _
                ", Attn=" + QuotedStr(tbAttn.Text) + ", DPListNo=" + QuotedStr(tbDPList.Text) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate=" + tbRate.Text.Replace(",", "") + ", PPNNo=" + QuotedStr(tbPPNNo.Text) + _
                ", PPNDate=" + tgl + ", PPNRate=" + tbPPNRate.Text.Replace(",", "") + _
                ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + ", PPN=" + tbPPN.Text.Replace(",", "") + _
                ", PPNForex= " + tbPPNForex.Text.Replace(",", "") + ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark=" + QuotedStr(tbRemark.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If

            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("Select TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, Currency, ForexRate, ReceiptForex, ReceiptHome, DPForex, DPHome, FgMode, BankGiro, DueDate, Remark, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome from FINDPCustDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("{FINDPCustDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPPNDate.SelectedDate = ViewState("ServerDate") 'Today
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbSONo.Text = ""
            tbDPList.Text = ""
            tbAttn.Text = ""
            tbPPNNo.Text = ""
            tbPPN.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPPNRate.Text = "0"
            tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRate.Text = "1"
            tbRate.Enabled = False
            'ddlReport.SelectedValue = "Y"
            tbRemark.Text = ""
            FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptDP" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            lbItemNo.Text = ""
            ddlReceiptType.SelectedValue = Nothing
            tbDocNo.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""
            tbReceiptDate.SelectedDate = tbTransDate.SelectedDate
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlCurr2.SelectedValue = ddlCurr.SelectedValue
            ChangeCurrency(ddlCurr2, tbTransDate, tbRate2, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            ChangePaymentType(ddlReceiptType.SelectedValue, tbFgMode, tbTransDate, tbDueDate, ddlBankGiro, ddlCurr2, ddlChargeCurr, tbRate2, tbChargeRate, tbChargeForex, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            If Not ViewState("InputDPListNo") Is Nothing Then
                If ViewState("InputDPListNo") = tbDPList.Text Then
                    tbReceiptForex.Text = FormatFloat(ViewState("InputAmount"), ViewState("DigitCurrDt2"))
                Else
                    tbReceiptForex.Text = "0"
                End If
            Else
                tbReceiptForex.Text = "0"
            End If
            tbRate2.Text = "1"
            tbReceiptHome.Text = tbReceiptForex.Text
            tbChargeForex.Text = "0"
            tbChargeHome.Text = "0"
            ddlChargeCurr.SelectedValue = ddlCurr.SelectedValue
            tbChargeForex.Text = "0"
            tbChargeHome.Text = "0"
            tbChargeRate.Text = "0"

            tbDPForex.Text = tbReceiptForex.Text
            tbDPHome.Text = tbReceiptForex.Text
            tbRemarkDt.Text = tbRemark.Text
            
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbDate.IsNull Then
            '    lbStatus.Text = MessageDlg("Date must have value")
            '    tbDate.Focus()
            '    Return False
            'End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbTransDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbTransDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If ddlReqType.SelectedValue.Trim = "" Then
            '    lbStatus.Text = "Request Type must have value"
            '    ddlReqType.Focus()
            '    Return False
            'End If
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustCode.Focus()
                Return False
            End If
            If tbCustName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustName.Focus()
                Return False
            End If
            If tbDPList.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("DP Inv No must have value")
                tbDPList.Focus()
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
            'If CFloat(tbPPN.Text) <= 0 And ddlReport.SelectedValue = "Y" Then
            '    lbStatus.Text = "PPN must have value"
            '    tbPPN.Focus()
            '    Return False
            'End If

            'If CFloat(tbPPNRate.Text) <= 0 And ddlReport.SelectedValue = "Y" Then
            '    lbStatus.Text = "PPN Rate must have value"
            '    tbPPNRate.Focus()
            '    Return False
            'End If
            'If CFloat(tbBaseForex.Text) <= 0 Then
            '    lbStatus.Text = "Base Forex must have value"
            '    tbBaseForex.Focus()
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ReceiptType").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Receipt Code Must Have Value")
                    Return False
                End If
                If Dr("FgMode").ToString = "B" Or Dr("FgMode").ToString = "K" Then
                    If Dr("Reference").ToString = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        Return False
                    End If
                End If
                If Dr("ReceiptDate").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Receipt Date Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Forex Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ReceiptForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Receipt Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("DPForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("DP Forex Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ChargeForex").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Charge Forex Must Have Value")
                    Return False
                End If
            Else
                If ddlReceiptType.SelectedValue = Nothing Then
                    lbStatus.Text = MessageDlg("Receipt Must Have Value")
                    ddlReceiptType.Focus()
                    Return False
                End If
                If tbReceiptDate.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Receipt Date Have Value")
                    tbReceiptDate.Focus()
                    Return False
                End If
                If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
                    If tbVoucherNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        tbVoucherNo.Focus()
                        Return False
                    End If
                End If
                If CFloat(tbRate2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Forex Rate Must Have Value")
                    tbRate2.Focus()
                    Return False
                End If
                If CFloat(tbReceiptForex.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Receipt Forex Must Have Value")
                    tbReceiptForex.Focus()
                    Return False
                End If
                If CFloat(tbDPForex.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("DP Forex Must Have Value")
                    tbDPForex.Focus()
                    Return False
                End If
                If CFloat(tbChargeForex.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Charge Forex Must Have Value")
                    tbChargeForex.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Customer, DP List No, Attention, SO No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Customer_Name, DPListNo, Attn, SONo, Remark"
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
        Dim CekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    tbTransDate.Enabled = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        tbTransDate.Enabled = True
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf (DDL.SelectedValue = "Print") Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','DPCRC'," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"

                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    Dim SqlString As String

                    SqlString = "Declare @A VarChar(255) EXEC S_FNDPCustReceiptDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "

                    SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
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
            lbStatus.Text = "Grid View 1 sorting Error : " + ex.ToString
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAdddt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        totalingDt()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select DigitDecimal FROM MsCurrency WHERE CurrCode = " + QuotedStr(ddlCurr.SelectedValue)))
            'pnlEditDt.Visible = True
            'pnlDt.Visible = False
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            'ChangePaymentType(ddlReceiptType.SelectedValue, tbFgMode, tbReceiptDate, tbDueDate, ddlBankGiro, ddlCurr2, ddlChargeCurr, tbRate2, tbChargeRate, tbChargeForex, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            ViewState("StateDt") = "Edit"
            ddlReceiptType.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbDPList, Dt.Rows(0)("DPListNo").ToString)
            BindToText(tbSONo, Dt.Rows(0)("SONo").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbPPNNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPNDate, Dt.Rows(0)("PPnDate").ToString)
            BindToText(tbPPNRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptDP" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToDropList(ddlReceiptType, Dr(0)("ReceiptType").ToString)
                tbReceiptDate.SelectedDate = tbTransDate.SelectedDate
                'BindToDate(tbReceiptDate, Dr(0)("ReceiptDate").ToString)
                BindToText(tbDocNo, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToDropList(ddlCurr2, Dr(0)("Currency").ToString)
                BindToText(tbRate2, Dr(0)("ForexRate").ToString)
                BindToText(tbReceiptForex, Dr(0)("ReceiptForex").ToString)
                BindToText(tbReceiptHome, Dr(0)("ReceiptHome").ToString)
                BindToText(tbChargeForex, Dr(0)("DPForex").ToString)
                BindToText(tbChargeHome, Dr(0)("DPHome").ToString)
                FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                BindToDropList(ddlBankGiro, Dr(0)("BankGiro").ToString)
                BindToDate(tbDueDate, Dr(0)("DueDate").ToString)
                BindToText(tbFgMode, Dr(0)("FgMode").ToString)
                BindToDropList(ddlChargeCurr, Dr(0)("ChargeCurrency").ToString)
                BindToText(tbChargeRate, Dr(0)("ChargeRate").ToString)
                BindToText(tbChargeForex, Dr(0)("ChargeForex").ToString)
                BindToText(tbChargeHome, Dr(0)("ChargeHome").ToString)
                BindToText(tbDPForex, Dr(0)("DPForex").ToString)
                BindToText(tbDPHome, Dr(0)("DPHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

                'If tbChargeCurr.Text = "" Then
                '    tbChargeForex.Text = "0"
                '    tbChargeHome.Text = "0"
                'End If
                'tbChargeForex.Enabled = tbChargeCurr.Text <> ""
                tbRate2.Enabled = ddlCurr2.SelectedValue <> Session("Currency")
                ddlBankGiro.Enabled = tbFgMode.Text = "G"
                tbDueDate.Enabled = tbFgMode.Text = "G"
                ddlChargeCurr.Enabled = tbFgMode.Text = "B"
                tbChargeForex.Enabled = tbFgMode.Text = "B"
                tbChargeRate.Enabled = ddlChargeCurr.SelectedValue <> Session("Currency") And tbFgMode.Text = "B"
                tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                ddlCurr.SelectedValue = Dr("Currency")
                ddlCurr_SelectedIndexChanged(Nothing, Nothing)
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                ddlCurr.SelectedValue = ""
                ddlCurr_SelectedIndexChanged(Nothing, Nothing)
            End If
            tbDPList.Text = ""
            tbSONo.Text = ""
            tbCustCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb CustCode Code ERror : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency');", Page, Me.GetType)
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMsCurrency();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlCurr2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr2.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr2, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr2.SelectedValue), ViewState("DBConnection"))
            ChangeCurrency(ddlCurr2, tbTransDate, tbRate2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            'ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr2.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr2 ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlReceiptType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReceiptType.SelectedIndexChanged
        Try
            Dim dr As DataRow
            Dim VoucherNo As String
            dr = FindMaster("PayType", ddlReceiptType.SelectedValue, ViewState("DBConnection"))
            If Not dr Is Nothing Then
                BindToText(tbFgMode, dr("FgMode"))
                BindToDropList(ddlCurr2, dr("Currency"))
            Else
                tbFgMode.Text = "B"
            End If
            ChangePaymentType(ddlReceiptType.SelectedValue, tbFgMode, tbTransDate, tbDueDate, ddlBankGiro, ddlCurr2, ddlChargeCurr, tbRate2, tbChargeRate, tbChargeForex, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            ddlChargeCurr_SelectedIndexChanged(Nothing, Nothing)
            ddlCurr2_SelectedIndexChanged(Nothing, Nothing)
            FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
            AttachScript("kali(" + Me.tbRate2.ClientID + "," + Me.tbReceiptForex.ClientID + "," + Me.tbReceiptHome.ClientID + "); kali(" + Me.tbChargeRate.ClientID + "," + Me.tbChargeForex.ClientID + "," + Me.tbChargeHome.ClientID + "); setformatdt2();", Page, Me.GetType())
            VoucherNo = ""
            If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbTransDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlReceiptType.SelectedValue) + ", 'IN', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If
            tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Receipt Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub
    Private Sub totalingDt()
        Dim TotalPayment As Decimal = 0
        Dim TotalCharge As Decimal = 0
        Dim TotalForex As Decimal = 0
        Dim BaseForex As Decimal = 0

        Dim dr As DataRow
        Try
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    TotalPayment = TotalPayment + CFloat(dr("ReceiptHome").ToString)
                    TotalCharge = TotalCharge + CFloat(dr("ChargeHome").ToString)
                End If
            Next
            TotalForex = (TotalPayment - TotalCharge) / CFloat(tbRate.Text)
            BaseForex = TotalForex * 100 / (100 + CFloat(tbPPN.Text))
            tbBaseForex.Text = FormatFloat(BaseForex, ViewState("DigitCurr"))
            'lbStatus.Text = ViewState("DigitCurr")
            'Exit Sub
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbChargeForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbChargeForex.TextChanged
        tbDPForex.Text = FormatNumber((CFloat(tbReceiptHome.Text) + CFloat(tbChargeHome.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
        tbDPHome.Text = FormatNumber((CFloat(tbReceiptHome.Text) + CFloat(tbChargeHome.Text)), ViewState("DigitHome"))
        'ChangeCurrency(ddlCurrDt2, tbTransDate, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
        totalingDt()
    End Sub

    Protected Sub tbReceiptForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReceiptForex.TextChanged
        tbDPForex.Text = FormatNumber((CFloat(tbReceiptHome.Text) + CFloat(tbChargeHome.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
        tbDPHome.Text = FormatNumber((CFloat(tbReceiptHome.Text) + CFloat(tbChargeHome.Text)), ViewState("DigitHome"))
        totalingDt()
        tbChargeForex.Focus()
        'AttachScript("setformatDt();", Page, Me.GetType())
    End Sub

    Protected Sub ddlChargeCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurr.SelectedIndexChanged
        Try
            ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
            ChangeCurrency(ddlChargeCurr, tbTransDate, tbChargeRate, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
            If ddlChargeCurr.SelectedValue = "" Then
                tbChargeForex.Text = "0"
                tbChargeHome.Text = "0"
            End If
            tbChargeForex.Enabled = ddlChargeCurr.SelectedValue <> ""
            tbChargeRate.Focus()
            '            AttachScript("setformatDt();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    '    FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankGiro, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnHome.Visible = False
            EnableHd(True)
            tbTransDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Customer_Code, Customer_Name, Currency, Term FROM VMsCustomer "
            ResultField = "Customer_Code, Customer_Name, Currency, Term"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
            btnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        'Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            'MovePanel(pnlInput, PnlHd)
            'CurrFilter = tbFilter.Text
            'Value = ddlField.SelectedValue
            'tbFilter.Text = tbTransNo.Text
            'ddlField.SelectedValue = "TransNmbr"
            'btnSearch_Click(Nothing, Nothing)
            'tbFilter.Text = CurrFilter
            'ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "Save All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
        '    'BindToDropList(ddlReceiptType, ViewState("PayType").ToString.Trim)
        'End If
        ddlReceiptType.Focus()
    End Sub

    Protected Sub btnDPList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDPList.Click
        Dim ResultField As String
        Try
            If tbCustCode.Text.Trim = "" Then
                Session("filter") = "select DP_No, DP_Date, SO_NO, Customer_Code, Customer_Name, Currency, Base_Forex, PPN, PPN_Forex, Total_Forex, Total_Receipt, Total_Balance FROM V_FNDPCustListPending Where Customer_Code IS NOT NULL " 'AND Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
            Else
                Session("filter") = "select DP_No, DP_Date, SO_NO, Customer_Code, Customer_Name, Currency, Base_Forex, PPN, PPN_Forex, Total_Forex, Total_Receipt, Total_Balance FROM V_FNDPCustListPending Where Customer_Code = " + QuotedStr(tbCustCode.Text) '+ " AND Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
            End If
            ResultField = "DP_No, Currency, PPN, SO_No, Customer_Code, Customer_Name, Total_Balance,Base_Forex,PPN_Forex,Total_Forex"
            ViewState("Sender") = "btnDPList"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search PO Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
            If GetCountRecord(ViewState("Dt")) >= 1 Then
                If tbFgMode.Text = "K" Or tbFgMode.Text = "B" Or tbFgMode.Text = "G" Or tbFgMode.Text = "D" Then
                    If ViewState("StateDt") <> "Edit" Then
                        If ddlReceiptType.SelectedValue <> ViewState("PayType").ToString Then
                            lbStatus.Text = "Cannot input more than one Receipt type"
                            ddlReceiptType.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            End If
            'End If
            If tbFgMode.Text = "G" Then
                If CekExistGiroIn(tbDocNo.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocNo.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt") = "Edit" Then
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("ReceiptType") = ddlReceiptType.SelectedValue
                Row("ReceiptTypeName") = ddlReceiptType.SelectedItem.Text
                Row("ReceiptDate") = Format(tbReceiptDate.SelectedDate, "dd MMMM yyyy")
                Row("DocumentNo") = tbDocNo.Text
                Row("Reference") = tbVoucherNo.Text
                If tbDueDate.Enabled Then
                    Row("DueDate") = Format(tbDueDate.SelectedDate, "dd MMMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                End If
                If ddlBankGiro.SelectedIndex = 0 Then
                    Row("BankGiro") = ""
                    Row("BankGiroName") = ""
                Else
                    Row("BankGiro") = ddlBankGiro.SelectedValue
                    Row("BankGiroName") = ddlBankGiro.SelectedItem.Text
                End If

                Row("FgMode") = tbFgMode.Text
                Row("Currency") = ddlCurr2.SelectedValue
                Row("ForexRate") = tbRate2.Text
                Row("ReceiptForex") = tbReceiptForex.Text
                Row("ReceiptHome") = tbReceiptHome.Text
                Row("Remark") = tbRemarkDt.Text
                Row("ChargeCurrency") = ddlChargeCurr.SelectedValue
                Row("ChargeRate") = tbChargeRate.Text
                Row("ChargeForex") = tbChargeForex.Text
                Row("ChargeHome") = tbChargeHome.Text
                Row("DPHome") = tbDPHome.Text 'CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)
                Row("DPForex") = tbDPForex.Text '(CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text)

                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
                    lbStatus.Text = "Item No. '" + lbItemNo.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("ReceiptType") = ddlReceiptType.SelectedValue
                dr("ReceiptTypeName") = ddlReceiptType.SelectedItem.Text
                dr("ReceiptDate") = Format(tbReceiptDate.SelectedDate, "dd MMMM yyyy")
                dr("DocumentNo") = tbDocNo.Text
                dr("Reference") = tbVoucherNo.Text
                If tbDueDate.Enabled Then
                    dr("DueDate") = Format(tbDueDate.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
                If ddlBankGiro.SelectedIndex = 0 Then
                    dr("BankGiro") = ""
                    dr("BankGiroName") = ""
                Else
                    dr("BankGiro") = ddlBankGiro.SelectedValue
                    dr("BankGiroName") = ddlBankGiro.SelectedItem.Text
                End If
                dr("FgMode") = tbFgMode.Text
                dr("Currency") = ddlCurr2.SelectedValue
                dr("ForexRate") = tbRate2.Text
                dr("ReceiptForex") = tbReceiptForex.Text
                dr("ReceiptHome") = tbReceiptHome.Text
                dr("Remark") = tbRemarkDt.Text
                dr("ChargeCurrency") = ddlChargeCurr.Text
                dr("ChargeRate") = tbChargeRate.Text
                dr("ChargeForex") = tbChargeForex.Text
                dr("ChargeHome") = tbChargeHome.Text
                dr("Remark") = tbRemarkDt.Text
                dr("DPHome") = tbDPHome.Text 'CFloat(tbReceiptHome.Text) - CFloat(tbChargeHome.Text)
                dr("DPForex") = tbDPForex.Text '(CFloat(tbReceiptForex.Text) - CFloat(tbChargeForex.Text)) / CFloat(tbRate.Text)
                ViewState("Dt").Rows.Add(dr)
            End If
            If tbFgMode.Text = "K" Or tbFgMode.Text = "B" Or tbFgMode.Text = "G" Or tbFgMode.Text = "D" Then
                ViewState("PayType") = ddlReceiptType.SelectedValue
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            totalingDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            'If ViewState("PayType").ToString = "" Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
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
            If (ActionValue = "Print") Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next
                Result = Result + "'"

                Session("SelectCommand") = "EXEC S_FNFormVoucher " + Result + ",'DPCRC'," + QuotedStr(ViewState("UserId").ToString)
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"

                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_FNDPCustReceipt", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("TransNmbr in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTransDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTransDate.SelectionChanged
        Try
            tbPPNDate.SelectedDate = tbTransDate.SelectedDate
        Catch ex As Exception
            lbStatus.Text = "tbTransDate_SelectionChanged : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbFilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
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
End Class
