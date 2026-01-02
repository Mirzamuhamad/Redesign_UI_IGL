Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrFATransfer_TrFATransfer
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_GLFATransferHd"

    Private Function GetStringHd(ByVal Type As String, ByVal FgExpendable As String) As String
        Return "SELECT DISTINCT TransNmbr, Nmbr, TransDate, Trans_Date, Status, Operator, Remark, TransferType, FgExpendable FROM V_GLFATransferHd WHERE TransferType = " + QuotedStr(Type) + " AND FgExpendable = " + QuotedStr(FgExpendable)
    End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLFATransferDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLFATransferDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

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
                If ViewState("Sender") = "btnFASubGroup" Then
                    tbFASubGroup.Text = Session("Result")(0).ToString
                    tbFASubGroupName.Text = Session("Result")(1).ToString
                    tbLifeMonth.Text = Session("Result")(2).ToString.Replace("&nbsp;", "0")
                    tbFAMax.Text = SQLExecuteScalar("S_GetMaxFA " + QuotedStr(tbFASubGroup.Text), ViewState("DBConnection"))
                End If
                If ViewState("Sender") = "btnFALoc" Then
                    tbFALocCode.Text = Session("Result")(0).ToString
                    tbFALocName.Text = Session("Result")(1).ToString
                End If
                'Dim drow As DataRow()
                'drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
                'If drow.Length > 0 Then
                '    BindGridDt(drow.CopyToDataTable, GridDt2)
                '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                'Else
                '    Dim DtTemp As New DataTable
                '    DtTemp = ViewState("Dt2").Clone
                '    DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                '    GridDt2.DataSource = DtTemp
                '    GridDt2.DataBind()
                '    GridDt2.Columns(0).Visible = False
                'End If

                ' CountTotalDt()
                If ViewState("Sender") = "btnRRNo" Then
                    'ResultField = "RR_No, RR_Date, Currency, ForexRate, Product, Product_Name, Unit, PriceForex"
                    tbRRNo.Text = Session("Result")(0).ToString
                    tbBuyDate.SelectedDate = Session("Result")(1).ToString
                    ddlCurrDt.SelectedValue = Session("Result")(2).ToString
                    tbRateDt.Text = Session("Result")(3).ToString
                    tbProductCode.Text = Session("Result")(4).ToString
                    tbProductName.Text = Session("Result")(5).ToString
                    tbFAName.Text = Session("Result")(5).ToString
                    tbQty.Text = FormatNumber(Session("Result")(6).ToString, ViewState("DigitCurr"))
                    ddlUnit.SelectedValue = Session("Result")(7).ToString
                    tbPriceForex.Text = Session("Result")(8).ToString
                    'tbSpecFA.Text = Session("Result")(9).ToString.Replace("amp;", "").Replace("&nbsp;", "")
                    ViewState("DigitCurr") = GetCurrDigit(ddlCurrDt.SelectedValue, ViewState("DBConnection").ToString)
                    tbAmountForexDt.Text = FormatNumber(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
                    tbAmountHomeDt.Text = FormatNumber(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text) * CFloat(tbRateDt.Text), ViewState("DigitHome"))
                    If (CFloat(tbLifeMonth.Text) <> 0) And (CFloat(tbLifeProcessDepr.Text) <> 0) Then
                        tbAmountProcessDepr.Text = FormatNumber(CFloat(tbAmountHomeDt.Text) * CFloat(tbLifeProcessDepr.Text) / CFloat(tbLifeMonth.Text), ViewState("DigitHome"))
                    End If
                End If
                'StatusButtonSave(True)
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        'Dim index As Integer
        'Try
        '    index = Int32.Parse(e.Item.Value)
        '    MultiView1.ActiveViewIndex = index
        '    btnSaveTrans.Focus()
        'Catch ex As Exception
        '    lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        'End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCostCenterDt, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            FillCombo(ddlFAStatus, "EXEC S_GetFAStatus", True, "FAStatusCode", "FAStatusName", ViewState("DBConnection"))
            FillCombo(ddlDepyear, "EXEC S_Getyear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlDepMonth, "EXEC S_GetPeriod", True, "Period", "Description", ViewState("DBConnection"))
            FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))

            If Request.QueryString("ContainerId").ToString = "TrFATransferID" Then
                ViewState("TransferType") = "Transfer"
                'lblTitle.Text = "Fixed Asset TransFer"
                lblTitle.Text = "Fixed Asset Registration"
                ViewState("FgExpendable") = "N"
                'lbUnit.Visible = False
                lbUnit.Visible = True
                ddlUnit.Enabled = True
                tbPriceForex.Enabled = True
                tbBuyDate.Enabled = True
                ddlCurrDt.Enabled = True
                'tbRateDt.Enabled = True
                lbProcessDepr.Visible = True
                lbProcessDeprOpr.Visible = True
                tbLifeProcessDepr.Visible = True
                tbAmountProcessDepr.Visible = True
                tbFAName.Enabled = True
                '------------Add by Chris-------------------'
                PnlRRNoProduct.Visible = False  'Add by Chris
                pnlDepresiasi.Visible = True  'Add by Chris
                pnllblDepresiasi.Visible = True  'Add by Chris
                '---------------------------------------------'
            ElseIf Request.QueryString("ContainerId").ToString = "TrEATransferID" Then
                ViewState("TransferType") = "Transfer"
                lblTitle.Text = "Expendable Asset TransFer"
                ViewState("FgExpendable") = "Y"
                'lbUnit.Visible = False
                lbUnit.Visible = True
                ddlUnit.Enabled = True
                tbPriceForex.Enabled = True
                tbBuyDate.Enabled = True
                ddlCurrDt.Enabled = True
                'tbRateDt.Enabled = True
                lbProcessDepr.Visible = True
                lbProcessDeprOpr.Visible = True
                tbLifeProcessDepr.Visible = True
                tbAmountProcessDepr.Visible = True
                tbFAName.Enabled = False
            ElseIf Request.QueryString("ContainerId").ToString = "TrEARegistraionID" Then
                ViewState("TransferType") = "Registration"
                lblTitle.Text = "Expendable Asset Registration"
                ViewState("FgExpendable") = "Y"
                lbUnit.Visible = True
                ddlUnit.Enabled = True  'False
                tbQty.Enabled = True  'Add by Chris
                tbPriceForex.Enabled = True  'False
                tbBuyDate.Enabled = False
                ddlCurrDt.Enabled = False
                tbRateDt.Enabled = False
                lbProcessDepr.Visible = False
                lbProcessDeprOpr.Visible = False
                tbLifeProcessDepr.Visible = True
                tbAmountProcessDepr.Visible = True
                tbFAName.Enabled = True  'False
            Else
                ViewState("TransferType") = "Registration"
                lblTitle.Text = "Fixed Asset TransFer"
                'lblTitle.Text = "Fixed Asset Registration"
                ViewState("FgExpendable") = "N"
                lbUnit.Visible = True
                ddlUnit.Enabled = True  'False
                tbQty.Enabled = True  'Add by Chris
                tbPriceForex.Enabled = True  'False
                tbBuyDate.Enabled = False
                ddlCurrDt.Enabled = False
                tbRateDt.Enabled = False
                lbProcessDepr.Visible = True
                lbProcessDeprOpr.Visible = True
                tbLifeProcessDepr.Visible = True
                tbAmountProcessDepr.Visible = True
                tbFAName.Enabled = True  'False
                '------------Add by Chris-------------------'
                PnlRRNoProduct.Visible = True  'Add by Chris
                pnlDepresiasi.Visible = False  'Add by Chris
                pnllblDepresiasi.Visible = False  'Add by Chris
                '---------------------------------------------'
            End If

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbLifeMonth.Attributes.Add("ReadOnly", "True")
            tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            'tbQty.Attributes.Add("ReadOnly", "True")  'Revision by Chris
            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLifeMonth.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Untuk Registrasi
            tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")

            'tbLifeMonth.Attributes.Add("OnBlur", "setformatdt();")

            tbLifeProcessDepr.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountProcessDepr.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLifeProcessDepr.Attributes.Add("OnBlur", "setformatdt();")

            tbPriceForex.Attributes.Add("OnBlur", "setformatdt();")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd(ViewState("TransferType").ToString, ViewState("FgExpendable").ToString), StrFilter, ViewState("DBConnection").ToString)
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate ASC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
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
                    Result = ExecSPCommandGo(ActionValue, "S_GLFATransfer", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        'lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        lbStatus.Text = lbStatus.Text + MessageDlg(Result) + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
            GridDt.HeaderRow.Cells(19).Text = "Amount (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(20).Text = "Amount Process Depr (" + ViewState("Currency") + ")"
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
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

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbOperator.Text = ViewState("UserId").ToString
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbFA.Text = ""
            tbFAName.Text = ""
            ddlFAStatus.SelectedIndex = 0
            ddlFAOwner.SelectedIndex = 0
            tbFASubGroup.Text = ""
            tbFASubGroupName.Text = ""
            ddlCostCenterDt.SelectedIndex = 0
            tbBuyDate.SelectedValue = Now
            ddlCurrDt.SelectedValue = ViewState("Currency")
            tbRateDt.Text = "0"
            tbQty.Text = "0"
            tbLifeMonth.Text = "0"
            tbLifeProcessDepr.Text = "0"
            tbAmountForexDt.Text = "0"
            tbAmountHomeDt.Text = "0"
            tbAmountProcessDepr.Text = "0"
            tbRRNo.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbPriceForex.Text = "0"
            ddlDepyear.SelectedValue = Year(Now)
            ddlDepMonth.SelectedValue = Month(Now)
            ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ddlFAOwner.Enabled = ViewState("FgExpendable") = "Y"
            tbSpecFA.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            ddlFALocType.SelectedIndex = 0
            tbFALocCode.Text = ""
            tbFALocName.Text = ""
            tbQtyDt2.Text = "0"
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("FixedAsset").ToString = "" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Must Have Value")
                    tbFA.Focus()
                    Return False
                End If
                If Dr("FAName").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Name Must Have Value")
                    tbFAName.Focus()
                    Return False
                End If
                If Dr("FAStatus").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Status Must Have Value")
                    ddlFAStatus.Focus()
                    Return False
                End If
                If Dr("FAOwner").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Owner Must Have Value")
                    ddlFAOwner.Focus()
                    Return False
                End If
                If Dr("FASubGroup").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA SubGroup Must Have Value")
                    tbFASubGroup.Focus()
                    Return False
                End If
                If Dr("Currency").ToString = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrDt.Focus()
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    tbRateDt.Focus()
                    Return False
                End If
                
                If CFloat(Dr("StartYearDepr")) < CFloat(Year(Dr("BuyingDate").ToString)) Then
                    lbStatus.Text = MessageDlg("Start Depr Year greater than Buying Date")
                    ddlDepyear.Focus()
                    Return False
                End If
                
                If CFloat(Dr("StartYearDepr").ToString) = CFloat(Year(Dr("BuyingDate").ToString)) And CFloat(Dr("StartMonthDepr").ToString) < CFloat(Month(Dr("BuyingDate").ToString)) Then
                    lbStatus.Text = MessageDlg("Start Depr Month greater than Buying Date")
                    ddlDepMonth.Focus()
                    Return False
                End If
                If CFloat(Dr("LifeMonth").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Life Month Must Have Value")
                    tbLifeMonth.Focus()
                    Return False
                End If
                If CFloat(Dr("LifeProcessDepr").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Life Process Depr Must Have Value")
                    tbLifeProcessDepr.Focus()
                    Return False
                End If
                'If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    Return False
                'End If
                If CFloat(Dr("AmountForex").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
                    tbAmountForexDt.Focus()
                    Return False
                End If
                If CFloat(Dr("AmountHome").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Home Must Have Value")
                    tbAmountHomeDt.Focus()
                    Return False
                End If
                If CFloat(Dr("AmountProcessDepr").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Process Depr Must Have Value")
                    tbAmountProcessDepr.Focus()
                    Return False
                End If
                'Untuk Registrasi
                If Request.QueryString("ContainerId").ToString = "TrFARegistrationID" Then
                    If Dr("RRNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("RRNo Must Have Value")
                        tbRRNo.Focus()
                        Return False
                    End If
                End If
            Else
                If tbFA.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Must Have Value")
                    tbFA.Focus()
                    Return False
                End If
                If tbFAName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA Name Must Have Value")
                    tbFAName.Focus()
                    Return False
                End If
                If ddlFAStatus.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("FA Status Must Have Value")
                    ddlFAStatus.Focus()
                    Return False
                End If
                If ddlFAOwner.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("FA Owner Must Have Value")
                    ddlFAOwner.Focus()
                    Return False
                End If
                If tbFASubGroup.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA SubGroup Must Have Value")
                    tbFASubGroup.Focus()
                    Return False
                End If
                If ddlCostCenterDt.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                    ddlCostCenterDt.Focus()
                    Return False
                End If
                If ddlCurrDt.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrDt.Focus()
                    Return False
                End If
                If CFloat(tbRateDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    tbRateDt.Focus()
                    Return False
                End If
                If CFloat(ddlDepyear.SelectedValue) < CFloat(Year(tbBuyDate.SelectedDate)) Then
                    lbStatus.Text = MessageDlg("Start Depr Year greater than Buying Date")
                    ddlDepyear.Focus()
                    Return False
                End If

                If CFloat(ddlDepyear.SelectedValue) = CFloat(Year(tbBuyDate.SelectedDate)) And CFloat(ddlDepMonth.SelectedValue) < CFloat(Month(tbBuyDate.SelectedDate)) Then
                    lbStatus.Text = MessageDlg("Start Depr Month greater than Buying Date")
                    ddlDepMonth.Focus()
                    Return False
                End If

                If CFloat(tbLifeMonth.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Life Month Must Have Value")
                    tbLifeMonth.Focus()
                    Return False
                End If
                If CFloat(tbLifeProcessDepr.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Life Process Depr Must Have Value")
                    tbLifeProcessDepr.Focus()
                    Return False
                End If
                'If tbQty.Text = "0" Or tbQty.Text = "" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    tbQty.Focus()
                '    Return False
                'End If
                If CFloat(tbAmountForexDt.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
                    tbAmountForexDt.Focus()
                    Return False
                End If
                If CFloat(tbAmountHomeDt.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Home Must Have Value")
                    tbAmountHomeDt.Focus()
                    Return False
                End If
                If CFloat(tbAmountProcessDepr.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Amount Process Depr Must Have Value")
                    tbAmountProcessDepr.Focus()
                    Return False
                End If
                'Untuk Registrasi
                If Request.QueryString("ContainerId").ToString = "TrFARegistrationID" Then
                    If Dr("RRNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("RRNo Must Have Value")
                        tbRRNo.Focus()
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("FALocationCode").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If

            Else
                If tbFALocCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    tbFALocCode.Focus()
                    Return False
                End If
                If tbQtyDt2.Text = "0" Or tbQty.Text = "" Then
                    'lbStatus.Text = "Qty Must Have Value"
                    'Exit Function
                    'MessageDlg("Qty Must Have Value")
                    'tbQtyDt2.Focus()
                    'Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd(ViewState("TransferType").ToString, ViewState("FgExpendable").ToString), "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal FA As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FixedAsset = " + QuotedStr(FA))
            If Dr.Length > 0 Then
                BindToText(tbFA, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FAName").ToString)
                BindToDropList(ddlFAStatus, Dr(0)("FAStatus").ToString)
                BindToDropList(ddlFAOwner, Dr(0)("FAOwner").ToString)
                BindToText(tbFASubGroup, Dr(0)("FASubGroup").ToString)
                BindToText(tbFASubGroupName, Dr(0)("FA_SubGrp_Name").ToString)
                tbFAMax.Text = SQLExecuteScalar("S_GetMaxFA " + QuotedStr(tbFASubGroup.Text), ViewState("DBConnection"))
                BindToDate(tbBuyDate, Dr(0)("BuyingDate").ToString)
                BindToDropList(ddlCostCenterDt, Dr(0)("CostCtr").ToString)
                BindToDropList(ddlCurrDt, Dr(0)("Currency").ToString)
                BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbQty, FormatNumber(Dr(0)("Qty").ToString), 0)
                BindToText(tbLifeMonth, Dr(0)("LifeMonth").ToString)
                BindToText(tbLifeProcessDepr, Dr(0)("LifeProcessDepr").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForexDt, Dr(0)("AmountForex").ToString)
                BindToText(tbAmountHomeDt, Dr(0)("AmountHome").ToString)
                BindToText(tbAmountProcessDepr, Dr(0)("AmountProcessDepr").ToString)
                BindToDropList(ddlDepyear, Dr(0)("StartYearDepr").ToString)
                BindToDropList(ddlDepMonth, Dr(0)("StartMonthDepr").ToString)
                ViewState("DigitCurr") = GetCurrDigit(Dr(0)("Currency").ToString, ViewState("DBConnection").ToString)
                'If Request.QueryString("ContainerId").ToString = "TrFARegistrationID" Then
                BindToText(tbRRNo, Dr(0)("RRNo").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                'End If
                If Trim(Dr(0)("StartYearDepr").ToString) = "" Then
                    ddlDepyear.SelectedValue = Year(Dr(0)("BuyingDate").ToString)
                    ddlDepMonth.SelectedValue = Month(Dr(0)("BuyingDate").ToString)
                End If
                ddlFAOwner.Enabled = ViewState("FgExpendable") = "Y"
                BindToText(tbSpecFA, Dr(0)("Specification").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal FALoc As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("FixedAsset+'|'+FALocationType+'|'+FALocationCode = " + QuotedStr(FALoc))
            If Dr.Length > 0 Then
                BindToDropList(ddlFALocType, Dr(0)("FALocationType").ToString)
                BindToText(tbFALocCode, Dr(0)("FALocationCode").ToString)
                BindToText(tbFALocName, Dr(0)("FA_Location_Name").ToString)
                BindToText(tbQtyDt2, FormatNumber(Dr(0)("Qty").ToString), 0)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim tbTotal As Double
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If Left(tbRRNo.Text, 7) <> "IGL/LAP" Then

                If tbPriceForex.Text.Replace(",", "") = 0 Then
                    lbStatus.Text = MessageDlg(" Price Forex must have value")
                    tbPriceForex.Focus()
                    Exit Sub
                End If

            End If

            If Request.QueryString("ContainerId").ToString = "TrFATransferID" Then

                If CFloat(tbLifeProcessDepr.Text) > CFloat(tbLifeMonth.Text) Then
                    lbStatus.Text = MessageDlg(" Life Depreciation cannot greater than Life Total")
                    tbLifeProcessDepr.Focus()
                    Exit Sub
                End If

                If CFloat(tbAmountProcessDepr.Text) > CFloat(tbAmountHomeDt.Text) Then
                    lbStatus.Text = MessageDlg(" Amount Depreciation cannot greater than Amount Total")
                    tbAmountProcessDepr.Focus()
                    Exit Sub
                End If

                tbTotal = CFloat(tbLifeMonth.Text) - CFloat(tbLifeProcessDepr.Text)

                If tbTotal = 0 Then
                    If CFloat(tbAmountProcessDepr.Text) < CFloat(tbAmountHomeDt.Text) Then
                        lbStatus.Text = MessageDlg(" Amount Depreciation cannot smaller than Amount Total")
                        tbAmountProcessDepr.Focus()
                        Exit Sub
                    End If
                End If

            End If
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("FixedAsset = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("FixedAsset") = tbFA.Text
                Row("FAName") = tbFAName.Text
                Row("FAStatus") = ddlFAStatus.SelectedValue
                Row("FAStatusName") = ddlFAStatus.Text
                Row("FAOwner") = ddlFAOwner.SelectedValue
                Row("FASubGroup") = tbFASubGroup.Text
                Row("FA_SubGrp_Name") = tbFASubGroupName.Text
                Row("Currency") = ddlCurrDt.SelectedValue
                Row("ForexRate") = tbRateDt.Text
                Row("CostCtr") = ddlCostCenterDt.SelectedValue
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                End If
                Row("Cost_Ctr_Name") = ddlCostCenterDt.SelectedItem.Text
                Row("BuyingDate") = tbBuyDate.SelectedValue
                Row("LifeMonth") = tbLifeMonth.Text.Replace(",", "")
                Row("LifeProcessDepr") = tbLifeProcessDepr.Text.Replace(",", "")
                Row("Qty") = tbQty.Text 'FormatNumber(tbQty.Text, ViewState("DigitQty"))
                Row("PriceForex") = FormatNumber(tbPriceForex.Text, ViewState("DigitCurr"))
                Row("AmountProcessDepr") = FormatNumber(tbAmountProcessDepr.Text, ViewState("DigitHome"))
                Row("AmountForex") = FormatNumber(tbAmountForexDt.Text, ViewState("DigitCurr"))
                Row("AmountHome") = FormatNumber(tbAmountHomeDt.Text, ViewState("DigitHome"))
                Row("Product") = tbProductCode.Text
                Row("RRNo") = tbRRNo.Text
                Row("StartYearDepr") = ddlDepyear.SelectedValue
                Row("StartMonthDepr") = ddlDepMonth.SelectedValue
                Row("Specification") = tbSpecFA.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFA.Text
                dr("FAName") = tbFAName.Text
                dr("FAStatus") = ddlFAStatus.SelectedValue
                dr("FAStatusName") = ddlFAStatus.Text
                dr("FAOwner") = ddlFAOwner.SelectedValue
                dr("FASubGroup") = tbFASubGroup.Text
                dr("FA_SubGrp_Name") = tbFASubGroupName.Text
                dr("Currency") = ddlCurrDt.SelectedValue
                dr("ForexRate") = tbRateDt.Text
                dr("CostCtr") = ddlCostCenterDt.SelectedValue
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                End If
                dr("Cost_Ctr_Name") = ddlCostCenterDt.SelectedItem.Text
                dr("BuyingDate") = tbBuyDate.SelectedValue
                dr("LifeMonth") = tbLifeMonth.Text.Replace(",", "")
                dr("LifeProcessDepr") = tbLifeProcessDepr.Text.Replace(",", "")
                dr("Qty") = tbQty.Text 'FormatNumber(tbQty.Text, ViewState("DigitQty"))
                dr("PriceForex") = FormatNumber(tbPriceForex.Text, ViewState("DigitCurr"))
                dr("AmountProcessDepr") = FormatNumber(tbAmountProcessDepr.Text, ViewState("DigitHome"))
                dr("AmountForex") = FormatNumber(tbAmountForexDt.Text, ViewState("DigitCurr"))
                dr("AmountHome") = FormatNumber(tbAmountHomeDt.Text, ViewState("DigitHome"))
                dr("RRNo") = tbRRNo.Text
                dr("Product") = tbProductCode.Text
                dr("StartYearDepr") = ddlDepyear.SelectedValue
                dr("StartMonthDepr") = ddlDepMonth.SelectedValue
                dr("Specification") = tbSpecFA.Text
                dr("Unit") = ddlUnit.SelectedValue
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("FixedAsset+'|'+FALocationType+'|'+FALocationCode = " + QuotedStr(lbFADt2.Text + "|" + ddlFALocType.SelectedValue + "|" + tbFALocCode.Text))(0)
                Row.BeginEdit()
                Row("FixedAsset") = lbFADt2.Text
                Row("FALocationType") = ddlFALocType.SelectedValue
                Row("FALocationCode") = tbFALocCode.Text
                Row("FA_Location_Name") = tbFALocName.Text
                Row("Qty") = tbQtyDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("FixedAsset") = lbFADt2.Text
                dr("FALocationType") = ddlFALocType.SelectedValue
                dr("FALocationCode") = tbFALocCode.Text
                dr("FA_Location_Name") = tbFALocName.Text
                dr("Qty") = tbQtyDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            CountTotalDt()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub CountTotalDt()
        Dim QtyTotal As Double
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim havedetail As Boolean
        Try
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            QtyTotal = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
                    End If
                Next
            End If
            Dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))(0)
            'price = CFloat(Dr("Price").ToString)
            'lbStatus.Text = Dr("PriceForex").ToString + "   " + ViewState("DigitCurr").ToString
            'Exit Sub
            Dr.BeginEdit()
            Dr("Qty") = QtyTotal 'FormatNumber(QtyTotal, ViewState("DigitQty"))
            Dr("AmountForex") = FormatNumber(QtyTotal * Dr("PriceForex"), ViewState("DigitCurr"))
            Dr("AmountHome") = FormatNumber(QtyTotal * Dr("PriceForex") * Dr("ForexRate"), ViewState("DigitHome"))
            'Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
            If CFloat(Dr("LifeMonth")) <> 0 Then
                Dr("AmountProcessDepr") = FormatNumber(QtyTotal * Dr("PriceForex") * Dr("ForexRate") * CFloat(Dr("LifeProcessDepr")) / CFloat(Dr("LifeMonth")), ViewState("DigitHome"))
            End If
            Dr.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)
            'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If ViewState("TransferType").ToString = "Transfer" And ViewState("FgExpendable") = "N" Then
                    tbCode.Text = GetAutoNmbr("FTR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                    'tbCode.Text = GetAutoNmbr("FRG", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                ElseIf ViewState("TransferType").ToString = "Transfer" And ViewState("FgExpendable") = "Y" Then
                    tbCode.Text = GetAutoNmbr("ETR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                ElseIf ViewState("TransferType").ToString = "Registration" And ViewState("FgExpendable") = "Y" Then
                    tbCode.Text = GetAutoNmbr("ERG", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                Else
                    tbCode.Text = GetAutoNmbr("FRG", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                    'tbCode.Text = GetAutoNmbr("FTR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                End If

                SQLString = "INSERT INTO GLFATransferHd (TransNmbr, TransDate, Status, " + _
                "Operator, Remark, UserPrep, DatePrep, TransferType, FgExpendable) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbOperator.Text) + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()" + ", " + QuotedStr(ViewState("TransferType").ToString) + ", " + QuotedStr(ViewState("FgExpendable").ToString)
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM GLFATransferHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFATransferHd SET Operator =" + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate() WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FixedAsset, FAName, FAStatus, FAOwner, FASubGroup, BuyingDate, LifeMonth, LifeProcessDepr, Qty, Currency, ForexRate, AmountForex, AmountHome, AmountProcessDepr, CostCtr, AccFA, FgFA, AccCapital, FgCapital, RRNo, Product, PriceForex, StartYearDepr, StartMonthDepr, Specification, Unit FROM GLFATransferDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFATransferDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, FixedAsset, FALocationType, FALocationCode, Qty, Remark FROM GLFATransferDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("GLFATransferDt2")

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
        Dim cekQty As Boolean

        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                Exit Sub
            End If

            cekQty = checkQty()
            If cekQty = False Then
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Dim cekQty As Boolean

        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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

            cekQty = checkQty()
            If cekQty = False Then
                Exit Sub
            End If

            SaveAll()
            newTrans()
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Private Function checkQty() As Boolean
        Dim drow() As DataRow
        Dim dt As New DataTable
        Dim GVR As GridViewRow
        Dim baris As Integer
        Dim i As Integer
        Dim fa As String
        Dim qty, QtyTotal As Integer
        Dim havedetail As Boolean

        Try
            If GetCountRecord(ViewState("Dt")) <> 0 Then
                dt = ViewState("Dt")
                baris = dt.Rows.Count
                For i = 0 To GetCountRecord(ViewState("Dt")) - 1 'For i = 0 To baris - 1
                    GVR = GridDt.Rows(i)
                    fa = GVR.Cells(2).Text
                    qty = CInt(GVR.Cells(14).Text)
                    QtyTotal = 0

                    drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(fa))

                    If drow.Length > 0 Then
                        For Each Dr In drow.CopyToDataTable.Rows
                            If Not Dr.RowState = DataRowState.Deleted Then
                                QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
                            End If
                        Next
                    End If

                    If qty <> QtyTotal Then
                        lbStatus.Text = MessageDlg(GVR.Cells(3).Text + " Qty must be equall with Qty Total in Detail 2")
                        Return False
                    End If
                Next
            End If

            Return True
        Catch ex As Exception
            lbStatus.Text = "checkQty error : " + ex.ToString
        End Try
    End Function

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True

            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbQtyDt2.Text = lblQty.Text
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Status, Operator, Remark"
            FilterValue = "TransNmbr, Status, Operator, Remark"
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
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
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
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBank '" + GVR.Cells(2).Text + "','CASH OUT'"
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                        Session("DBConnection") = ViewState("DBConnection")
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If

            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                Dim lbFA As Label

                lbFA = GVR.FindControl("lbFa")

                lbFADt2.Text = GVR.Cells(2).Text
                lbFANameDt2.Text = GVR.Cells(3).Text
                lbStatusFA.Text = GVR.Cells(5).Text
                lbFAOwner.Text = GVR.Cells(6).Text
                lbFASubGroup.Text = GVR.Cells(7).Text
                lbBuyingDate.Text = GVR.Cells(9).Text
                lbCurr.Text = GVR.Cells(16).Text
                lbCostCtr.Text = GVR.Cells(21).Text
                lblQty.Text = FormatNumber(GVR.Cells(14).Text, 0)
                lblUnit.Text = GVR.Cells(15).Text
                MultiView1.ActiveViewIndex = 1

                ViewState("DigitCurr") = GetCurrDigit(lbCurr.Text, ViewState("DBConnection").ToString)
                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                End If
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text))) '+ " AND FAName = " + QuotedStr(TrimStr(lbFANameDt2.Text)) + " AND FAStatus = " + QuotedStr(TrimStr(lbStatusFA.Text)))
                '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = False
                End If
            End If
            'If e.CommandName = "View" Then
            '    Dim GVR As GridViewRow
            '    GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
            '    If GetCountRecord(ViewState("Dt")) = 0 Then
            '        Exit Sub
            '    Else
            '        lbFADt2.Text = GVR.Cells(2).Text
            '        lbFANameDt2.Text = GVR.Cells(3).Text
            '        MultiView1.ActiveViewIndex = 1
            '        BindDataDt2(ViewState("TransNmbr"))
            '        If ViewState("StateHd") = "View" Then
            '            GridDt2.Columns(0).Visible = False
            '        Else : GridDt2.Columns(0).Visible = True
            '        End If

            '    End If
            'End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt.Rows(e.RowIndex)

            'Delete Detail 2------------------------------------
            'dr = ViewState("Dt2").select("FALocationType+'|'+FALocationCode = " + QuotedStr(ddlFALocType.SelectedValue + "|" + tbFALocCode.Text))
            dr = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()

            'Delete Detail 1------------------------------------
            dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)

            'If dr.Length > 0 Then
            '    lbStatus.Text = " Data Detail exist"
            '    Exit Sub

            '    'If GetCountRecord(ViewState("Dt2")) <> 0 Then
            '    '    lbStatus.Text = " Data Detail exist"
            '    '    Exit Sub
            'Else
            '    GVR = GridDt.Rows(e.RowIndex)
            '    dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(2).Text))
            '    dr(0).Delete()
            '    BindGridDt(ViewState("Dt"), GridDt)
            '    EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalQty As Decimal = 0
    'Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
    '    Try
    '        Dim SQLString As String = ""
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "FixedAsset")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                TotalQty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                SQLString = "UPDATE GLFATransferDt SET Qty =" + TotalQty.ToString + _
    '                             " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) + " AND FixedAsset = " + QuotedStr(lbFADt2.Text)
    '                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    '    'Dim SQLString As String = ""
    '    'Try
    '    '    If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "FixedAsset")) Then
    '    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '    '            TotalQty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
    '    '        ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '    '            SQLString = "UPDATE GLFATransferDt SET Qty =" + TotalQty.ToString + _
    '    '            " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) + " AND FixedAsset = " + QuotedStr(lbFADt2.Text)
    '    '            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    '    '            'tbTotalPayment.Text = FormatNumber(TotalPayment, ViewState("DigitHome"))
    '    '        End If
    '    '    End If
    '    'Catch ex As Exception
    '    '    lbStatus.Text = "Grid Dt2 Row Data Bound Error : " + ex.ToString
    '    'End Try
    'End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("FixedAsset+'|'+FALocationType+'|'+FALocationCode = " + QuotedStr(lbFADt2.Text + "|" + GVR.Cells(1).Text + "|" + GVR.Cells(2).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            CountTotalDt()

            'BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDt(GVR.Cells(2).Text)
            
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)

            
            'If ViewState("InputCurrency") = "Y" Then
            'ddlCurrDt.DataBind()
            'ViewState("InputCurrency") = Nothing
            'End If
            'ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbFADt2.Text + "|" + GVR.Cells(1).Text + "|" + GVR.Cells(2).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRRNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRRNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT RR_No, RR_Date, Currency, dbo.FormatFloat(ForexRate," + ViewState("DigitRate").ToString + ") AS ForexRate, " + _
            "Product, Product_Name, Specification, Qty,Unit, PriceForex " + _
            "FROM V_GetRRCIP WHERE FgExpendable = " + QuotedStr(ViewState("FgExpendable").ToString)  ''N'
            ResultField = "RR_No, RR_Date, Currency, ForexRate, Product, Product_Name, Qty, Unit, PriceForex, Specification"
            ViewState("Sender") = "btnRRNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search RRNo Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFASubGroup.Click
        Dim ResultField As String
        Try

            Session("filter") = "SELECT * FROM VMsFAGroupSub WHERE Fg_Expendable = " + QuotedStr(ViewState("FgExpendable").ToString)
            ResultField = "FA_SubGrp_Code, FA_SubGrp_Name, LifeMonth"
            ViewState("Sender") = "btnFASubGroup"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search FASubGroup Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnFALoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFALoc.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsFALocationAll WHERE FA_Location_Type=" + QuotedStr(ddlFALocType.SelectedValue)
            ResultField = "FA_Location_Code, FA_Location_Name"
            ViewState("Sender") = "btnFALoc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search FALoc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlFALocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFALocType.SelectedIndexChanged
        tbFALocCode.Text = ""
        tbFALocName.Text = ""
    End Sub

    Protected Sub ddlCurrDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDt.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            ddlCurrDt.DataBind()
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        tbRateDt.Focus()
    End Sub

    Protected Sub tbFASubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFASubGroup.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("SELECT FA_SubGrp_Code, FA_SubGrp_Name, LifeMonth FROM VMsFAGroupSub WHERE Fg_Expendable = " + QuotedStr(ViewState("FgExpendable").ToString) + " AND FA_SubGrp_Code = " + QuotedStr(tbFASubGroup.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            Dr = FindMaster("FAGroupSub", tbFASubGroup.Text, ViewState("DBConnection").ToString)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbFASubGroup.Text = Dr("FA_SubGrp_Code").ToString
                tbFASubGroupName.Text = Dr("FA_SubGrp_Name").ToString
                tbLifeMonth.Text = Dr("LifeMonth").ToString
                tbFAMax.Text = SQLExecuteScalar("S_GetMaxFA " + QuotedStr(tbFASubGroup.Text), ViewState("DBConnection"))
            Else
                tbFASubGroup.Text = ""
                tbFASubGroupName.Text = ""
                tbLifeMonth.Text = "0"
                tbFAMax.Text = ""
            End If
            tbFASubGroup.Focus()
        Catch ex As Exception
            Throw New Exception("tb FAGroupSub Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbFALocCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFALocCode.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Try
            Dt = SQLExecuteQuery("SELECT * FROM VMsFALocationAll WHERE FA_Location_Type = " + QuotedStr(ddlFALocType.SelectedValue) + " AND FA_Location_Code = " + QuotedStr(tbFALocCode.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbFALocCode.Text = Dr("FA_Location_Code").ToString
                tbFALocName.Text = Dr("FA_Location_Name").ToString
            Else
                tbFALocCode.Text = ""
                tbFALocName.Text = ""
            End If
            tbFASubGroup.Focus()
        Catch ex As Exception
            Throw New Exception("tb FALocation Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBackDt2ke1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2ke1.Click, btnBackDt2ke2.Click
        Try
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPriceForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForex.TextChanged
        tbPriceForex.Text = FormatNumber(tbPriceForex.Text, ViewState("DigitCurr"))
        tbAmountForexDt.Text = FormatNumber(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
        'tbAmountForexDt.Text = FormatNumber(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
        tbAmountHomeDt.Text = FormatNumber(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text) * CFloat(tbRateDt.Text), ViewState("DigitHome"))
        If (CFloat(tbLifeMonth.Text) <> 0) And (CFloat(tbLifeProcessDepr.Text) <> 0) Then
            tbAmountProcessDepr.Text = FormatNumber(CFloat(tbAmountHomeDt.Text) * CFloat(tbLifeProcessDepr.Text) / CFloat(tbLifeMonth.Text), ViewState("DigitHome"))
        End If
    End Sub

    Protected Sub tbBuyDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBuyDate.SelectionChanged
        Try
            ddlDepMonth.SelectedValue = Month(tbBuyDate.SelectedDate)
            ddlDepyear.SelectedValue = Year(tbBuyDate.SelectedDate)
        Catch ex As Exception
            lbStatus.Text = "tbBuyDate_SelectionChanged error : " + ex.ToString
        End Try
    End Sub
End Class
