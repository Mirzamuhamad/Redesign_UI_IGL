Imports System.Data
Imports BasicFrame.WebControls

Partial Class Transaction_TrBeginCIPPN_TrBeginCIPPN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If ViewState("DigitCurr") Is Nothing Then
                ViewState("DigitCurr") = 0
            End If
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            'Hasil dari Advance Filter
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            'Hasil dari Search Dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    tbBillCode.Text = tbCustCode.Text
                    tbBillName.Text = tbCustName.Text
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(4).ToString)
                    ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)

                    'ddlCurr.Text = Session("Result")(2).ToString
                    'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    'Dim Dt As DataTable
                    'Dim Dr1 As DataRow
                    'Dim SQLString As String
                    'Try
                    '    SQLString = "S_FindBillToDefault " + QuotedStr(Session("Result")(0).ToString)
                    '    Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
                    '    'Dr = FindMaster("CustBillTo", tbCustCode.Text, tbBillTo.Text, ViewState("DBConnection").ToString)
                    '    If Dt.Rows.Count > 0 Then
                    '        Dr1 = Dt.Rows(0)
                    '    Else
                    '        Dr1 = Nothing
                    '    End If
                    '    If Not Dr1 Is Nothing Then
                    '        BindToText(tbBillCode, Dr1("Bill_To").ToString)
                    '        BindToText(tbBillName, Dr1("Bill_To_Name").ToString)
                    '    Else
                    '        tbBillCode.Text = ""
                    '        tbBillName.Text = ""
                    '    End If
                    '    'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    '    'AttachScript("setformat();", Page, Me.GetType())
                    '    tbBillCode.Focus()
                    'Catch ex As Exception
                    '    Throw New Exception("tb BillTo Error : " + ex.ToString)
                    'End Try
                ElseIf ViewState("Sender") = "btnBillTo" Then
                    tbBillCode.Text = Session("Result")(0).ToString
                    BindToText(tbBillName, Session("Result")(1).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error :" + ex.ToString
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
        Try
            ViewState("SortExpression") = Nothing
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "SELECT CostCtrCode, CostCtrName FROM MsCostCtr", True, "CostCtrCode", "CostCtrName", ViewState("DBConnection"))
            'FillCombo(ddlPClass, "EXEC S_GetProductClass", False, "Product_Class_Code", "Product_Class_Name", ViewState("DBConnection"))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            Me.tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbPPNRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'btntemp.Attributes.Add("Visible", "false")
            tbPPNForex.Attributes.Add("ReadOnly", "True")
            tbTotalForex.Attributes.Add("ReadOnly", "True")

            'tbBaseForex.Attributes.Add("OnBlur", "setformat();")
            'tbPPN.Attributes.Add("OnBlur", "setformat();")

            'Me.tbBaseForex.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
            'Me.tbPPN.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
            'Me.tbPPNForex.Attributes.Add("OnChange", "setformat();")
            'Me.tbTotalForex.Attributes.Add("OnChange", "setformat();")
            'Me.ddlCurr.Attributes.Add("OnChange", "setformat();")            
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            End If
            DT = BindDataTransaction("Select * From V_FNBeginCIPPN", StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "InvoiceDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput(True, pnlInput)
            newTrans()
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ChangeReport("Add", "Y", True, tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnHome.Visible = False
            tbInvoiceNo.Enabled = True
            tbInvoiceNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbInvoiceNo.Text = ""
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            'tbReceiptDate.SelectedDate = ViewState("ServerDate") 'Today

            'ddlReport.SelectedIndex = 0
            tbcustPONo.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbAttn.Text = ""
            tbBillCode.Text = ""
            tbBillName.Text = ""

            tbTerm.Text = "0"
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today

            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "1"
            tbRate.Enabled = False

            ddlType.SelectedValue = "CI"

            tbPPNNo.Text = ""
            tbPPNDate.Clear()
            tbPPNRate.Text = "0"
            tbBaseForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            ddlCostCtr.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
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
                    Result = ExecSPCommandGo(ActionValue, "S_FNCIBeginPPN", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("InvoiceNo in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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
                    btnHome.Visible = True
                    MovePanel(PnlHd, pnlInput)
                    FillTextBox(GVR)
                    ' change report harus diatas modifyinput dan dibawah filTextBox
                    ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
                    ModifyInput(False, pnlInput)
                    ViewState("StateHd") = "View"
                ElseIf DDL.SelectedValue = "Edit" Then
                    Dim lbStatusTemp As String
                    lbStatusTemp = GVR.Cells(3).Text
                    If lbStatusTemp = "H" Or lbStatusTemp = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("InvoiceNo") = GVR.Cells(2).Text                        
                        ModifyInput(True, pnlInput)
                        FillTextBox(GVR)
                        ViewState("StateHd") = "Edit"
                        btnHome.Visible = False
                        tbInvoiceNo.Enabled = False
                        ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Print
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Row Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillTextBox(ByVal e As GridViewRow)
        Dim Dt As DataTable
        Dim Nmbr As String
        Try
            Nmbr = e.Cells(2).Text
            Dt = BindDataTransaction("Select * From V_FNBeginCIPPN", "InvoiceNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            newTrans()
            tbInvoiceNo.Text = Nmbr
            BindToDate(tbTransDate, Dt.Rows(0)("InvoiceDate").ToString)
            'BindToDate(tbReceiptDate, Dt.Rows(0)("ReceiptInvDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbcustPONo, Dt.Rows(0)("CustPONo").ToString)
            BindToText(tbcustPONo, Dt.Rows(0)("CustPONo").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("CustCode").ToString)
            BindToText(tbCustName, Dt.Rows(0)("CustName").ToString)
            BindToText(tbBillCode, Dt.Rows(0)("BillTo").ToString)
            BindToText(tbBillName, Dt.Rows(0)("Bill_To_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbTerm, Dt.Rows(0)("Terms").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select COALESCE(DigitDecimal,0) FROM MsCurrency WHERE CurrCode = " + QuotedStr(e.Cells(11).Text)))
            BindToText(tbPPNNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPNDate, Dt.Rows(0)("PPnDate").ToString)
            BindToText(tbPPNRate, Dt.Rows(0)("PPnRate").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString)
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlType, Dt.Rows(0)("Type").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            'BindToDropList(ddlPClass, Dt.Rows(0)("ProductClass").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CurrFilter, Value As String
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbInvoiceNo.Text
            ddlField.SelectedValue = "InvoiceNo"
            BtnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save New Error : " + ex.ToString
        End Try
    End Sub

    Function cekhd() As Boolean
        Dim Infois As String
        Try
            If tbInvoiceNo.Text.Trim.Length <= 0 Then
                lbStatus.Text = MessageDlg("Invoice No Cannot Empty.")
                tbInvoiceNo.Focus()
                Return False
            End If
            If tbCustName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Customer Cannot Empty.")
                tbCustCode.Focus()
                Return False
            End If
            If tbBillName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Bill To Cannot Empty.")
                tbBillName.Focus()
                Return False
            End If
            If tbTerm.Text.Length = 0 Or Not (IsNumeric(tbTerm.Text.Replace(",", ""))) Then
                lbStatus.Text = MessageDlg("Term Cannot Empty.")
                tbTerm.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Forex Rate must be input values")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If CFloat(tbBaseForex.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Base Forex must be input values")
                tbBaseForex.Focus()
                Return False
            End If
            If ddlCostCtr.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Cost Center must be input values")
                ddlCostCtr.Focus()
                Return False
            End If
            If tbPPN.Text.Length <= 0 Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN No Cannot Empty.")
                tbPPN.Focus()
                Return False
            End If
            'If tbPPNForex.Text.Length <= 0 Or Not (IsNumeric(tbPPNForex.Text.Replace(",", ""))) Then
            '    lbStatus.Text = "PPN Forex Cannot Empty."
            '    tbPPNForex.Focus()
            '    Return False
            'End If
            If CFloat(tbTotalForex.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Total Forex must be input values")
                tbTotalForex.Focus()
                Return False
            End If
            If ViewState("StateHd") = "Insert" Then
                Infois = SQLExecuteScalar("SELECT COALESCE(InvoiceNo, '') FROM FINBeginCIPPN WHERE InvoiceNo = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
                If Infois.Length > 0 Then
                    lbStatus.Text = MessageDlg("Invoice No Exist, Cannot Save Data")
                    tbInvoiceNo.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub SaveData()
        Dim SQLString As String
        Dim tgl As String
        Try
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
                SQLString = "Insert INTO FINBeginCIPPN (InvoiceNo, Status, InvoiceDate, FgReport, " + _
                "CustPONo, Customer, BillTo, Attn, Terms, DueDate, Currency, ForexRate, PPNNo, PPNDate, " + _
                "PPNRate, BaseForex, PPN, PPNForex, TotalForex, Remark, UserPrep, DatePrep, " + _
                "Year, Period, Type, CostCtr) " + _
                "SELECT " + QuotedStr(tbInvoiceNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr("Y") + "," + QuotedStr(tbcustPONo.Text) + "," + _
                QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbBillCode.Text) + "," + QuotedStr(tbAttn.Text) + "," + tbTerm.Text.Replace(",", "") + "," + _
                QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                tbRate.Text.Replace(",", "") + "," + QuotedStr(tbPPNNo.Text) + "," + _
                tgl + "," + tbPPNRate.Text.Replace(",", "") + "," + _
                tbBaseForex.Text.Replace(",", "") + "," + tbPPN.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()," + _
                Session(Request.QueryString("KeyId"))("Year").ToString + "," + Session(Request.QueryString("KeyId"))("Period").ToString + "," + QuotedStr(ddlType.SelectedValue) + "," + QuotedStr(ddlCostCtr.SelectedValue)

            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINBeginCIPPN WHERE InvoiceNo = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINBeginCIPPN SET InvoiceDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", FgReport=" + QuotedStr("Y") + ", CustPONo=" + QuotedStr(tbcustPONo.Text) + _
                ", Customer=" + QuotedStr(tbCustCode.Text) + ", Terms=" + tbTerm.Text.Replace(",", "") + _
                ", BillTo=" + QuotedStr(tbBillCode.Text) + ", Attn=" + QuotedStr(tbAttn.Text) + _
                ", DueDate=" + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + _
                ", ForexRate=" + tbRate.Text.Replace(",", "") + ", PPNNo=" + QuotedStr(tbPPNNo.Text) + _
                ", PPNDate=" + tgl + ", PPNRate=" + tbPPNRate.Text.Replace(",", "") + _
                ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + ", PPN=" + tbPPN.Text.Replace(",", "") + _
                ", PPNForex= " + tbPPNForex.Text.Replace(",", "") + ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Type= " + QuotedStr(ddlType.SelectedValue) + ", Remark=" + QuotedStr(tbRemark.Text) + ", CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
                " WHERE InvoiceNo = " + QuotedStr(tbInvoiceNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Save Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If

            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTerm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTerm.TextChanged, tbTransDate.SelectionChanged
        Try
            If Not IsNumeric(tbTerm.Text) Then
                tbTerm.Text = "0"
            End If
            tbDueDate.SelectedDate = tbTransDate.SelectedDate.AddDays(CInt(tbTerm.Text))
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))

        Catch ex As Exception
            lbStatus.Text = "tb Term text Changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    'End Sub

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

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        'Dim Dt As DataTable
        'Dim Dr1 As DataRow
        'Dim SQLString As String
        Try
            Dr = FindMaster("CustomerBegin", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                If Dr("Contact_Person").ToString <> "" Then
                    BindToText(tbAttn, Dr("Contact_Person"))
                End If

                'tbAttn.Text = Dr("Contact_Person")
                BindToDropList(ddlCurr, Dr("Currency"))
                tbBillCode.Text = tbCustCode.Text
                tbBillName.Text = tbCustName.Text
                tbBillCode.Focus()
                ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)

            Else
                tbCustName.Text = ""
                tbCustCode.Text = ""
                tbBillName.Text = ""
                tbBillCode.Text = ""
                tbAttn.Text = ""
                tbCustCode.Focus()
            End If
            
        Catch ex As Exception
            lbStatus.Text = "tb CustCode Code ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lkbAdvanceSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbAdvanceSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Due Date"
            FDateValue = "InvoiceDate, DueDate"
            FilterName = "Reference, Status, Report, Product Class, Customer, Bill To, Currency, CostCtrName, Remark"
            FilterValue = "InvoiceNo, Status, FgReport, Product_Class_Name, Customer, Bill_To, Currency, Cost Center, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
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

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub tbInvoiceNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbInvoiceNo.TextChanged

    End Sub

    Protected Sub tbcustPONo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcustPONo.TextChanged

    End Sub

    Protected Sub tbBillCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBillCode.TextChanged
        Dim Dr As DataRow
        Try
            'Dr = FindMaster("CustBillTo", tbCustCode.Text + "|" + tbBillCode.Text, ViewState("DBConnection").ToString)
            Dr = FindMaster("CustomerBegin", tbBillCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbBillCode.Text = Dr("Customer_Code")
                tbBillName.Text = Dr("Customer_Name")
            Else
                tbBillCode.Text = ""
                tbBillName.Text = ""
            End If
            tbBillCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb CustCode Code ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBillTo.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Customer_Code, Customer_Name FROM VMsCustomer WHERE fgActive = 'Y' "
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnBillTo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Bill To Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * FROM VMsCustomer Where FgActive = 'Y' "
            ResultField = "Customer_Code, Customer_Name, Currency, Term,Contact_Person"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub tbBaseForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBaseForex.TextChanged
        Try
            tbPPNForex.Text = (tbBaseForex.Text * tbPPN.Text) / 100
            tbTotalForex.Text = CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text)
            tbBaseForex.Text = FormatNumber(tbBaseForex.Text, ViewState("DigitCurr"))
            tbPPNForex.Text = FormatNumber(tbPPNForex.Text, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatNumber(tbTotalForex.Text, ViewState("DigitCurr"))
            tbPPN.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbBaseForex_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPPN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPN.TextChanged
        Try
            tbPPNForex.Text = (tbBaseForex.Text * tbPPN.Text) / 100
            tbTotalForex.Text = CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text)
            tbTotalForex.Text = FormatNumber(tbTotalForex.Text, ViewState("DigitCurr"))
            tbBaseForex.Text = FormatNumber(tbBaseForex.Text, ViewState("DigitCurr"))
            tbPPNForex.Text = FormatNumber(tbPPNForex.Text, ViewState("DigitCurr"))
            tbRemark.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbPPN_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPPNDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPNDate.SelectionChanged
        Try
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")                
                tbPPNRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPNDate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitRate"))
                tbPPNRate.Enabled = True
            Else
                tbPPNRate.Text = FormatNumber(1, ViewState("DigitRate"))
                tbPPNRate.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("tbPPndate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
