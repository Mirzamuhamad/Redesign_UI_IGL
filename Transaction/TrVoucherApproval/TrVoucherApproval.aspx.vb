Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class ApprovalVoucher
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_FINAPApprovalHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlExpenseType, "SELECT Payment_Code, Payment_Name FROM V_MsPayType WHERE FgMode = 'E'", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
                'FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                'FillCombo(ddlCostCenter, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                'FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                'FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                'FillCombo(ddlPType, "EXEC S_GetProductType", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT COUNT(Invoice_No) FROM V_GetInvPosting WHERE TotalAmount>=0 ", ViewState("DBConnection").ToString)
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToText(tbSuppType, Session("Result")(2).ToString)
                    'BindToDropList(ddlTerm, Session("Result")(4).ToString)
                    'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString)
                    'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                End If

                If ViewState("Sender") = "btnExpense" Then
                    tbExpense.Text = Session("Result")(0).ToString
                    tbExpenseName.Text = Session("Result")(1).ToString

                End If

                If ViewState("Sender") = "btnGetInv" Then
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        If CekExistData(ViewState("Dt"), "InvoiceNo", drResult("Invoice_No")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = MaxItem
                            dr("InvoiceNo") = drResult("Invoice_No")
                            dr("InvoiceDate") = drResult("Invoice_Date")
                            dr("PoNo") = drResult("Rfference_No")
                            dr("Invoice") = drResult("Invoice")
                            dr("Potongan") = drResult("Potongan")
                            dr("Dpp") = drResult("Dpp")
                            dr("Ppn") = drResult("Ppn")
                            dr("PpnInvoice") = drResult("PPn_Invoice")
                            dr("Pph") = drResult("Pph")
                            dr("PphInvoice") = drResult("Pph_Invoice")
                            dr("Remark") = drResult("Remark")
                            dr("InvoiceType") = drResult("InvoiceType")
                            dr("TotalAmount") = drResult("TotalAmount")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    CountTotalDt()
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    Dim DataRow As DataRow()
                    Dim FirstTime As Boolean = True

                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        If FirstTime Then
                            BindToText(tbSuppCode, drResult("Supplier").ToString)
                            BindToText(tbSuppName, drResult("SupplierName").ToString)
                            BindToText(tbSuppType, drResult("SupplierType").ToString)
                        End If

                        If CekExistData(ViewState("Dt"), "InvoiceNo", drResult("Invoice_No")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            DataRow = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbRef.Text))
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = MaxItem
                            dr("InvoiceNo") = drResult("Invoice_No")
                            dr("InvoiceDate") = drResult("Invoice_Date")
                            dr("PoNo") = drResult("Rfference_No")
                            dr("Invoice") = drResult("Invoice")
                            dr("Potongan") = drResult("Potongan")
                            dr("Dpp") = drResult("DPP")
                            dr("Ppn") = drResult("PPn")
                            dr("PpnInvoice") = drResult("PPn_Invoice")
                            dr("Pph") = drResult("PPh")
                            dr("PphInvoice") = drResult("PPh_Invoice")
                            dr("Remark") = drResult("Remark")
                            dr("InvoiceType") = drResult("InvoiceType")
                            dr("TotalAmount") = drResult("TotalAmount")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    CountTotalDt()
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
        ViewState("PPN") = SQLExecuteScalar("Select Max(PPN) FROM MsPPN ", ViewState("DBConnection"))
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 2
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Me.tbdpp.Attributes.Add("ReadOnly", "False")
        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbTotalPotongan.Attributes.Add("ReadOnly", "True")
        Me.tbTotalDpp.Attributes.Add("ReadOnly", "True")
        Me.TbtotalPPN.Attributes.Add("ReadOnly", "True")
        Me.TbTotalPPH.Attributes.Add("ReadOnly", "True")
        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.TbTotalInvoiceHd.Attributes.Add("ReadOnly", "True")
        Me.tbPotongan.Attributes.Add("ReadOnly", "False")
        ' Me.tbpph.Attributes.Add("ReadOnly", "True")
        Me.tbPphValue.Attributes.Add("ReadOnly", "True")
        Me.tbppnValue.Attributes.Add("ReadOnly", "True")

        Me.tbInvoice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPotongan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbdpp.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbppn.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPphValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbpph.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbppnValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbTotalAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbInvoice.Attributes.Add("OnBlur", "setformatfordt();")
        tbPotongan.Attributes.Add("OnBlur", "setformatfordt();")
        tbppn.Attributes.Add("OnBlur", "setformatfordt();")
        tbppnValue.Attributes.Add("OnBlur", "setformatfordt();")
        tbdpp.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformatfordt();")
        tbpph.Attributes.Add("OnBlur", "setformatfordt();")
        tbPphValue.Attributes.Add("OnBlur", "setformatfordt();")

        'Me.tbBaseForex.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        'Me.tbDisc.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'%'); setformat();")
        'Me.tbDiscForex.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        'Me.tbPPN.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")

        'Me.tbQty.Attributes.Add("OnBlur", "kali(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbAmountForex.ClientID + "); setformatdt();")
        'Me.tbPrice.Attributes.Add("OnBlur", "kali(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbAmountForex.ClientID + "); setformatdt();")
        'Me.tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")
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
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FINAPApprovalDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
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

            If ActionValue = "Print" Then
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

                If Result.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Data Selected")
                    Exit Sub
                End If

                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_FNFormApprovalVoucher " + Result + ", " + QuotedStr(ViewState("UserId").ToString) + " "
                Session("ReportFile") = ".../../../Rpt/FormApprovalVoucher.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNApprovalVoucher", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbDate.Enabled = State
            btnSupp.Visible = State
            btnGetInv.Visible = State
            tbSuppCode.Enabled = State
            tbExpenseName.Enabled = False
            'tbAttn.Enabled = State
            ddlInvoiceType.Enabled = State
            ddlExpenseType.Enabled = State
            btnExpand.Enabled = State
            'tbNamaPenerima.Enabled = State
            'tbBankPenerima.Enabled = State
            'tbAccount.Enabled = State
            ' tbRemark.Enabled = State
            tbTotalInvoice.Enabled = False
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableDt(ByVal State As Boolean)
        Try
            'tbInvoice.Enabled = State
            tbInvoiceDate.Enabled = State
            tbReference.Enabled = State
            tbPotongan.Enabled = State
            'tbdpp.Enabled = State
            tbppn.Enabled = State
            tbppnValue.Enabled = State
            tbpph.Enabled = State
            tbPphValue.Enabled = State
            tbTotalAmount.Enabled = State
            tbRemarkDt.Enabled = State
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
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
                If ViewState("DtValue") <> lbItemNo.Text Then
                    If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
                        lbStatus.Text = "Item No " + lbItemNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("InvoiceNo") = tbInvoiceNo.Text
                Row("InvoiceDate") = tbInvoiceDate.SelectedDate
                Row("PoNo") = tbReference.Text
                Row("Invoice") = tbInvoice.Text
                Row("Potongan") = tbPotongan.Text
                Row("Dpp") = tbdpp.Text
                Row("Ppn") = tbppn.Text
                Row("PPnInvoice") = Math.Round(CFloat(tbdpp.Text) * CFloat(tbppn.Text) / 100, 2)
                Row("Pph") = tbpph.Text
                Row("PPhInvoice") = Math.Round(CFloat(tbdpp.Text) * CFloat(tbpph.Text) / 100, 2) 'tbPphValue.Text
                Row("TotalAmount") = (CFloat(tbdpp.Text) + Row("PPnInvoice").ToString) - Row("PPhInvoice").ToString 'tbTotalAmount.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else


                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) = True Then
                    lbStatus.Text = "Item No " + lbItemNo.Text + " has already been exist"
                    Exit Sub
                End If


                If ddlInvoiceType.SelectedValue = "EXPENSE" Or ddlInvoiceType.SelectedValue = "PB" Then
                    If GetCountRecord(ViewState("Dt")) >= 1 Then
                        lbStatus.Text = MessageDlg("Cannot insert more than one record.!!")
                        MovePanel(pnlEditDt, pnlDt)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        BindGridDt(ViewState("Dt"), GridDt)
                        Exit Sub
                    End If
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("InvoiceNo") = tbInvoiceNo.Text
                dr("InvoiceDate") = tbInvoiceDate.SelectedDate
                dr("PoNo") = tbReference.Text
                dr("Invoice") = tbInvoice.Text
                dr("Potongan") = tbPotongan.Text
                dr("Dpp") = tbdpp.Text
                dr("Ppn") = tbppn.Text
                dr("PPnInvoice") = tbppnValue.Text
                dr("Pph") = tbpph.Text
                dr("PPhInvoice") = tbPphValue.Text
                dr("TotalAmount") = tbTotalAmount.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            CountTotalDt()
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
        Dim CekMenu As String
        Try
            'System.Threading.Thread.Sleep(7000)

             CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

            If pnlDt.Visible = False Then
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
                If ddlInvoiceType.SelectedValue = "PB" Then
                    tbRef.Text = GetAutoNmbr("VPB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                Else
                    tbRef.Text = GetAutoNmbr("VKD", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                End If

                SQLString = "INSERT INTO FINAPApprovalHd (TransNmbr,Status, TransDate, SuppCode, Attn,SupplierType, " + _
                "InvoiceType, ExpenseType, BankPenerima, NamaPenerima, AccNmbr, TotalAmountHd, TotalPotongan, TotalDpp, TotalPPN, TotalPPH, TotalInvoice,PerkiraanBayar, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + "," + QuotedStr(tbSuppType.Text) + ", " + QuotedStr(ddlInvoiceType.SelectedValue) + ", " + _
                QuotedStr(tbExpense.Text) + ", " + QuotedStr(tbBankPenerima.Text) + ",  " + _
                QuotedStr(tbNamaPenerima.Text) + ", " + QuotedStr(tbAccount.Text) + "," + _
                QuotedStr(TbTotalInvoiceHd.Text.Replace(",", "")) + ", " + QuotedStr(tbTotalPotongan.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbTotalDpp.Text.Replace(",", "")) + ", " + QuotedStr(TbtotalPPN.Text.Replace(",", "")) + "," + QuotedStr(TbTotalPPH.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbTotalInvoice.Text.Replace(",", "")) + ",'" + Format(tbPerkiraanBayar.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINAPApprovalHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                CountTotalDt()
                SQLString = "UPDATE FINAPApprovalHd SET SuppCode = " + QuotedStr(tbSuppCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", InvoiceType = " + QuotedStr(ddlInvoiceType.SelectedValue) + _
                ", ExpenseType = " + QuotedStr(tbExpense.Text) + _
                ", BankPenerima = " + QuotedStr(tbBankPenerima.Text) + _
                ", SupplierType = " + QuotedStr(tbSuppType.Text) + _
                ", NamaPenerima = " + QuotedStr(tbNamaPenerima.Text) + _
                ", AccNmbr = " + QuotedStr(tbAccount.Text) + _
                ", TotalAmountHd = " + QuotedStr(TbTotalInvoiceHd.Text.Replace(",", "")) + _
                ", TotalPotongan = " + QuotedStr(tbTotalPotongan.Text.Replace(",", "")) + _
                ", TotalDpp = " + QuotedStr(tbTotalDpp.Text.Replace(",", "")) + _
                ", TotalPPN = " + QuotedStr(TbtotalPPN.Text.Replace(",", "")) + _
                ", TotalPPH = " + QuotedStr(TbTotalPPH.Text.Replace(",", "")) + _
                ", TotalInvoice = " + QuotedStr(tbTotalInvoice.Text.Replace(",", "")) + _
                ", PerkiraanBayar = '" + Format(tbPerkiraanBayar.SelectedDate, "yyyy-MM-dd") + _
                "', Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + " "
            End If
            SQLString = ChangeQuoteNull(SQLString)
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, InvoiceNo, InvoiceDate, PONo, Invoice, Potongan, DPP, PPn, PPnInvoice,PPh, PPhInvoice , TotalAmount,InvoiceType, Remark FROM FINAPApprovalDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE FINAPApprovalDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
            '"PONo = @PONo, Invoice = @Invoice, Potongan = @Potongan, InvoiceDate = @InvoiceDate, " + _
            '"DPP = @DPP, PPn = @PPn, PPnInvoice = @PPnInvoice, PPh = @PPh, " + _
            '"PPhInvoice = @PPhInvoice, TotalAmount = @TotalAmount, " + _
            '"Remark = @Remark " + _
            '"WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            '' Define output parameters.
            'Update_Command.Parameters.Add("@ItemNo", SqlDbType.VarChar, 5, "ItemNo")
            'Update_Command.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 12, "InvoiceNo")
            'Update_Command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime, "InvoiceDate")
            'Update_Command.Parameters.Add("@PONo", SqlDbType.VarChar, 30, "PONo")
            'Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 5, "CostCtr")
            'Update_Command.Parameters.Add("@Invoice", SqlDbType.Float, 22, "Invoice")
            'Update_Command.Parameters.Add("@Potongan", SqlDbType.Float, 22, "Potongan")
            'Update_Command.Parameters.Add("@DPP", SqlDbType.Float, 22, "DPP")
            'Update_Command.Parameters.Add("@PPn", SqlDbType.Float, 22, "PPn")
            'Update_Command.Parameters.Add("@PPnInvoice", SqlDbType.Float, 22, "PPnInvoice")
            'Update_Command.Parameters.Add("@PPh", SqlDbType.Float, 22, "PPh")
            'Update_Command.Parameters.Add("@PPhInvoice", SqlDbType.Float, 22, "PPhInvoice")
            'Update_Command.Parameters.Add("@TotalAmount", SqlDbType.Float, 22, "TotalAmount")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            ' '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM FINAPApprovalDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINAPApprovalDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
            If ddlInvoiceType.SelectedValue = "Invoice" Then
                ddlExpenseType.Enabled = "False"
                btnExpense.Enabled = "False"
                btnAddDt.Visible = "False"
                btnAddDt2.Visible = "False"
            End If

            'ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            'If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
            '    tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            'Else
            '    tbPpnRate.Text = FormatNumber(1, ViewState("DigitCurr"))
            'End If
            btnHome.Visible = False
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
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPerkiraanBayar.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbAttn.Text = ""
            ddlInvoiceType.SelectedIndex = 0
            ddlExpenseType.SelectedIndex = 0
            tbExpense.Text = ""
            tbExpenseName.Text = ""
            tbBankPenerima.Text = ""
            tbNamaPenerima.Text = ""
            tbAccount.Text = ""
            tbSuppType.Text = ""
            TbTotalInvoiceHd.Text = 0
            tbTotalPotongan.Text = 0
            tbTotalDpp.Text = 0
            TbtotalPPN.Text = 0
            TbTotalPPH.Text = 0
            tbTotalInvoice.Text = 0
            tbRemark.Text = ""
            EnableHd(True)
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbInvoice.Text = ""
            tbInvoiceDate.SelectedDate = ViewState("ServerDate")
            tbReference.Text = ""
            tbInvoice.Text = 0
            tbPotongan.Text = 0
            tbdpp.Text = 0
            tbppn.Text = ViewState("PPN")
            tbppnValue.Text = 0
            tbpph.Text = 0
            tbPphValue.Text = 0
            tbTotalAmount.Text = 0
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_GetSupplier"
            ResultField = "Supplier_Code, Supplier_Name, Supplier_Type"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchGrid();", Page, Me.GetType())
            AttachScript("OpenPopupSupplier();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetInvt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetInv.Click
        Dim ResultField, ResultSame As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            'Session("Filter") = "SELECT * FROM V_GetInvPosting WHERE Supplier+'|'+SupplierType= " + QuotedStr(tbSuppCode.Text) + "+'|'+" + QuotedStr(tbSuppType.Text)
            Session("Filter") = "SELECT * FROM V_GetInvPosting WHERE TotalAmount>=0 AND Supplier= " + QuotedStr(tbSuppCode.Text)
            ResultField = "Invoice_No,Invoicetype,Invoice_Date,Supplier,SupplierName,Rfference_No,Invoice,Potongan,DPP,PPn,PPn_Invoice,PPh,PPh_Invoice,TotalAmount,SupplierType,Remark"
            Session("Column") = ResultField.Split(",")
            ResultSame = "Supplier, SupplierType, Invoicetype"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetInv"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, ResultSame As String 'ResultSame 
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
                Exit Sub
            End If

            Session("Result") = Nothing
            Session("Filter") = "SELECT * FROM V_GetInvPosting WHERE TotalAmount>=0 "
            'Session("Filter") = "EXEC S_FINGetInvPosting"
            ResultField = "Invoice_No,InvoiceType,Invoice_Date,Supplier,SupplierName,Rfference_No,Invoice,Potongan,DPP,PPn,PPn_Invoice,PPh,PPh_Invoice,TotalAmount,SupplierType,Remark"
            Session("Column") = ResultField.Split(",")
            ResultSame = "Supplier, SupplierType, InvoiceType "
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                tbSuppType.Text = Dr("Supplier_Type")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbSuppType.Text = ""
                tbAttn.Text = ""
            End If
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlInvoiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInvoiceType.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            If ddlInvoiceType.SelectedValue = "Invoice" Then
                ddlExpenseType.Enabled = False
                btnExpense.Enabled = False
                btnGetInv.Visible = True
                btnAddDt.Visible = False
                btnAddDt2.Visible = False
                tbExpense.Text = ""
                tbExpenseName.Text = ""
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbSuppType.Text = ""
                tbAttn.Text = ""
                btnSupp.Visible = True
                lblExpense.Text = "Expense Type"

            Else
                ddlExpenseType.Enabled = True
                btnExpense.Enabled = True
                btnGetInv.Visible = False
                btnAddDt.Visible = True
                btnAddDt2.Visible = True
                btnSupp.Visible = False

                If ddlInvoiceType.SelectedValue = "Expense" Then
                    tbExpense.Text = ""
                    tbExpenseName.Text = ""
                    lblExpense.Text = "Expense Type"
                    tbSuppCode.Text = "G0100001"
                    Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)

                    tbSuppCode.Text = Dr("Supplier_Code")
                    tbSuppName.Text = Dr("Supplier_Name")
                    tbSuppType.Text = Dr("Supplier_Type")
                    btnExpense_Click(Nothing, Nothing)
                ElseIf ddlInvoiceType.SelectedValue = "PB" Then
                    tbExpense.Text = ""
                    tbExpenseName.Text = ""
                    lblExpense.Text = "PB Type"
                    tbSuppCode.Text = "IGLPBG000001"
                    Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)

                    tbSuppCode.Text = Dr("Supplier_Code")
                    tbSuppName.Text = Dr("Supplier_Name")
                    tbSuppType.Text = Dr("Supplier_Type")
                    btnExpense_Click(Nothing, Nothing)
                End If

            End If

        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CountTotalDt()
        Dim TotalAmount, TotalInvoice, TotalDpp, TotalPotongan, TotalPPN, TotalPPH As Double
        Dim Dr As DataRow
        Try
            TotalInvoice = 0
            TotalDpp = 0
            TotalPotongan = 0
            TotalPPN = 0
            TotalPPH = 0
            TotalAmount = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotalInvoice = TotalInvoice + CFloat(Dr("Invoice").ToString)
                    TotalDpp = TotalDpp + CFloat(Dr("Dpp").ToString)
                    TotalPotongan = TotalPotongan + CFloat(Dr("Potongan").ToString)
                    TotalPPN = TotalPPN + CFloat(Dr("PpnInvoice").ToString)
                    TotalPPH = TotalPPH + CFloat(Dr("PPhInvoice").ToString)
                    TotalAmount = TotalAmount + CFloat(Dr("TotalAmount").ToString)
                End If
            Next
            'BindToText(TbTotalInvoiceHd, Session("Result")(7).ToString, ViewState("DigitHome"))
            'BindToText(tbPotongan, Session("Result")(7).ToString, ViewState("DigitHome"))
            'BindToText(tbdpp, Session("Result")(8).ToString, ViewState("DigitHome"))
            'BindToText(tbppnValue, Session("Result")(10).ToString, ViewState("DigitHome"))
            'BindToText(tbPphValue, Session("Result")(12).ToString, ViewState("DigitHome"))
            'BindToText(tbTotalInvoice, Session("Result")(13).ToString, ViewState("DigitHome"))

            TbTotalInvoiceHd.Text = FormatNumber(TotalInvoice, ViewState("DigitHome"))
            tbTotalPotongan.Text = FormatNumber(TotalPotongan, ViewState("DigitHome"))
            tbTotalDpp.Text = FormatNumber(TotalDpp, ViewState("DigitHome"))
            TbtotalPPN.Text = FormatNumber(TotalPPN, ViewState("DigitHome"))
            TbTotalPPH.Text = FormatNumber(TotalPPH, ViewState("DigitHome"))
            tbTotalInvoice.Text = FormatNumber(TotalAmount, ViewState("DigitHome"))

            tbTotalAmount.Text = FormatNumber(CFloat(tbTotalDpp.Text) + CFloat(TbtotalPPN.Text) - CFloat(TbTotalPPH.Text), ViewState("DigitHome"))

            'tbTotalInvoice.Text = FormatNumber(CFloat(tbTotalAmount.Text), ViewState("DigitHome"))
            'TbTotalInvoiceHd.Text = FormatNumber(CFloat(tbInvoice.Text), ViewState("DigitHome"))
            'tbTotalPotongan.Text = FormatNumber(CFloat(tbPotongan.Text), ViewState("DigitHome"))
            'tbTotalDpp.Text = FormatNumber(CFloat(tbdpp.Text), ViewState("DigitHome"))
            'TbtotalPPN.Text = FormatNumber(CFloat(tbppnValue.Text), ViewState("DigitHome"))
            'TbTotalPPH.Text = FormatNumber(CFloat(tbPphValue.Text), ViewState("DigitHome"))

        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            'btnAccount.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Transaction Date must have value")
                tbDate.Focus()
                Return False
            End If

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If

            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnSupp.Focus()
                Return False
            End If
            If ddlInvoiceType.SelectedValue = "Expense" Then
                If tbExpense.Text = "" Then
                    lbStatus.Text = MessageDlg("Expense Type must have value")
                    btnExpense.Focus()
                    Return False
                End If

            End If

            If ddlInvoiceType.SelectedValue = "PB" Then
                If tbExpense.Text = "" Then
                    lbStatus.Text = MessageDlg("PB Type must have value")
                    btnExpense.Focus()
                    Return False
                End If

            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("Account").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Account Must Have Value")
                '    Return False
                'End If
                'If Dr("FgSubled").ToString <> "N" And TrimStr(Dr("SubLed").ToString) = "" Then
                '    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                '    Return False
                'End If
                'If Dr("FgCostCtr").ToString = "Y" And Dr("CostCtr").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                '    Return False
                'End If
                'If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    Return False
                'End If
                'If Dr("PriceForex").ToString = "0" Or Dr("PriceForex").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                '    Return False
                'End If
                'If Dr("AmountForex").ToString = "0" Or Dr("AmountForex").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                '    Return False
                'End If
            Else
                If tbInvoice.Text = "0" Or tbInvoice.Text = "" Then
                    lbStatus.Text = MessageDlg("Invoice Must Have Value")
                    tbInvoice.Focus()
                    Return False
                End If
                If tbPotongan.Text = "" Then
                    lbStatus.Text = MessageDlg("Potongan Must Have Value")
                    tbPotongan.Focus()
                    Return False
                End If
                If tbdpp.Text = "" Then
                    lbStatus.Text = MessageDlg("DPP Must Have Value")
                    tbdpp.Focus()
                    Return False
                End If
                If tbppn.Text = "" Then
                    lbStatus.Text = MessageDlg("PPN Persen Must Have Value")
                    tbppn.Focus()
                    Return False
                End If
                If tbppnValue.Text = "" Then
                    lbStatus.Text = MessageDlg("PPN Must Have Value")
                    tbppnValue.Focus()
                    Return False
                End If
                If tbpph.Text = "" Then
                    lbStatus.Text = MessageDlg("PPH Persen Must Have Value")
                    tbpph.Focus()
                    Return False
                End If

                If tbPphValue.Text = "" Then
                    lbStatus.Text = MessageDlg("PPH Must Have Value")
                    tbPphValue.Focus()
                    Return False
                End If

                If tbTotalAmount.Text = "0" Or tbTotalAmount.Text = "" Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    tbTotalAmount.Focus()
                    Return False
                End If

                If ddlInvoiceType.SelectedValue = "Invoice" Then
                    Dim cekInvoice As String
                    cekInvoice = SQLExecuteScalar("Select Invoice FROM V_GetInvPosting WHERE Invoice_No = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
                    'lbStatus.Text = cekInvoice
                    'Exit Function
                    If CFloat(tbInvoice.Text.Replace(",", "")) > CFloat(cekInvoice) Then
                        lbStatus.Text = MessageDlg("Invoice Approval cannot be greater than Total Invoice ")
                        Exit Function
                    End If

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
            FDateName = "Date, Invoice Date"
            FDateValue = "TransDate, SuppInvDate"
            FilterName = "Reference, Date, Status, Supplier Code, Supplier Name,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, SuppCode, SuppName, Remark"
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
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    'ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        EnableHd(True)

                        If ddlInvoiceType.SelectedValue = "Invoice" Then
                            ddlInvoiceType.Enabled = False
                            btnExpense.Enabled = False
                            btnAddDt.Visible = False
                            btnAddDt2.Visible = False
                        Else
                            ddlInvoiceType.Enabled = True
                            btnExpense.Enabled = True
                            btnAddDt.Visible = True
                            btnAddDt2.Visible = True
                        End If
                       
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("SelectCommand") = "EXEC S_FNFormApprovalVoucher '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormApprovalVoucher.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)

                        'Dim URL As String
                        'URL = ResolveUrl("../../Rpt/PrintForm.Aspx")
                        'Dim s As String = "window.open('" & URL & "', '_blank');"
                        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)
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
            End If
            'CountTotalDt()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            CountTotalDt()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        'Dim Dr As DataRow
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)

            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = lbItemNo.Text
            If ddlInvoiceType.SelectedValue = "Invoice" Then
                EnableDt(False)
                tbRemarkDt.Enabled = True
            Else
                EnableDt(True)
            End If
            StatusButtonSave(False)
            'CountTotalDt()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
    '    Try
    '        ViewState("InputCurrency") = "Y"
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Currency Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lblExpense_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblExpense.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsPayType')();", Page, Me.GetType())
            'lbStatus.Text = MessageDlg("Tes 1 " + "\n" + "Test 2")
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub

    'Dim BaseForex As Decimal = 0

    '' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                'AttachScript("BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '        TotalHd()
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("SuppCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("SuppName").ToString)
            BindToText(tbSuppType, Dt.Rows(0)("SupplierType").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlInvoiceType, Dt.Rows(0)("InvoiceType").ToString)
            BindToText(tbExpense, Dt.Rows(0)("ExpenseType").ToString)
            BindToText(tbExpenseName, Dt.Rows(0)("ExpenseName").ToString)
            BindToText(tbBankPenerima, Dt.Rows(0)("BankPenerima").ToString)
            BindToText(tbNamaPenerima, Dt.Rows(0)("NamaPenerima").ToString)
            BindToText(tbAccount, Dt.Rows(0)("AccNmbr").ToString)
            BindToDate(tbPerkiraanBayar, Dt.Rows(0)("PerkiraanBayar").ToString)
            BindToText(TbTotalInvoiceHd, Dt.Rows(0)("TotalAmountHD").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalPotongan, Dt.Rows(0)("TotalPotongan").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalDpp, Dt.Rows(0)("TotalDpp").ToString, ViewState("DigitCurr"))
            BindToText(TbtotalPPN, Dt.Rows(0)("TotalPpn").ToString, ViewState("DigitCurr"))
            BindToText(TbTotalPPH, Dt.Rows(0)("TotalPph").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalInvoice, Dt.Rows(0)("TotalInvoice").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

            If ddlInvoiceType.SelectedValue = "Expense" Then
                lblExpense.Text = "Expense Type"
            Else
                lblExpense.Text = "PB"
            End If

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                BindToText(tbInvoiceNo, Dr(0)("invoiceNo").ToString)
                BindToDate(tbInvoiceDate, Dr(0)("InvoiceDate").ToString)
                BindToText(tbReference, Dr(0)("PoNo").ToString)
                BindToText(tbInvoiceNo, Dr(0)("invoiceNo").ToString)
                BindToText(tbInvoice, Dr(0)("Invoice").ToString, ViewState("DigitHome"))
                BindToText(tbPotongan, Dr(0)("Potongan").ToString, ViewState("DigitHome"))
                BindToText(tbdpp, Dr(0)("Dpp").ToString, ViewState("DigitHome"))
                BindToText(tbppn, Dr(0)("PPn").ToString, ViewState("DigitHome"))
                BindToText(tbppnValue, Dr(0)("PPNInvoice").ToString, ViewState("DigitHome"))
                BindToText(tbpph, Dr(0)("PPh").ToString, ViewState("DigitHome"))
                BindToText(tbPphValue, Dr(0)("PphInvoice").ToString, ViewState("DigitHome"))
                BindToText(tbTotalAmount, Dr(0)("TotalAmount").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub


    Protected Sub btnExpense_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpense.Click
        Dim ResultField As String
        Try
            If ddlInvoiceType.SelectedValue = "Expense" Then
                Session("filter") = "SELECT Payment_Code,Payment_Name FROM V_MsPayType WHERE FgMode='E' "
            ElseIf ddlInvoiceType.SelectedValue = "PB" Then
                Session("filter") = "SELECT Payment_Code,Payment_Name FROM V_MsPayType WHERE FgMode='P' "
            End If

            ResultField = "Payment_Code,Payment_Name"
            ViewState("Sender") = "btnExpense"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


End Class
