Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrFAFromStock_TrFAFromStock
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    
    Private Function GetStringHd(ByVal FgExpendable As String) As String
        Return "Select * From V_GLFAFromStockHd WHERE FgExpendable = " + QuotedStr(FgExpendable)
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
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnFASubGroup" Then
                    tbFASubGroup.Text = Session("Result")(0).ToString
                    tbFASubGroupName.Text = Session("Result")(1).ToString
                    ddlFAProcess.Text = Session("Result")(2).ToString
                    tbLifeMonth.Text = Session("Result")(3).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProduct.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString + " " + TrimStr(Session("Result")(2).ToString)
                    tbUnit.Text = Session("Result")(3).ToString
                End If
                If ViewState("Sender") = "btnFALoc" Then
                    tbFALocCode.Text = Session("Result")(0).ToString
                    tbFALocName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnSubled" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    BindToText(tbSubledName, Session("Result")(1).ToString)
                End If
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
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            ViewState("SetLocation") = False
            FillCombo(ddlWrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCostCenterDt, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            FillCombo(ddlFAStatus, "EXEC S_GetFAStatus", True, "FAStatusCode", "FAStatusName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLifeMonth.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformat();")
            'tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformat();")
            tbLifeMonth.Attributes.Add("ReadOnly", "True")
            tbAmountForexDt.Attributes.Add("OnBlur", "setformat();")
            tbRateDt.Attributes.Add("OnBlur", "setformat();")
            tbQty.Attributes.Add("OnBlur", "setformat();")
            tbLifeMonth.Attributes.Add("OnBlur", "setformat();")

            If Request.QueryString("ContainerId").ToString = "TrEAFromStockId" Then
                ViewState("FgExpendable") = "Y"
                lbTitle.Text = "Expendable Asset From Stock"
            Else
                ViewState("FgExpendable") = "N"
                lbTitle.Text = "Fixed Asset From Stock"
            End If
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
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd(ViewState("FgExpendable").ToString), StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLFAFromStockDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                    Result = ExecSPCommandGo(ActionValue, "S_GLFAFromStock", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlWrhs.Enabled = State
            tbSubled.Enabled = State And tbFgSubled.Text.Trim <> "N"
            btnSubled.Visible = tbSubled.Enabled
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
            'GridDt.HeaderRow.Cells(13).Text = "Amount (" + ViewState("Currency") + ")"
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFgSubled.Text = "N"
            tbSubled.Text = ""
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            tbSubledName.Text = ""
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
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbUnit.Text = ""
            'ddlLocation.SelectedIndex = 0
            ddlFAStatus.SelectedIndex = 0
            ddlCostCenterDt.SelectedIndex = 0
            tbFALocCode.Text = ""
            tbFALocName.Text = ""
            ddlCurrDt.SelectedValue = ViewState("Currency")
            ddlFAProcess.SelectedIndex = 0
            tbQty.Text = "0"
            tbRateDt.Text = "0"
            tbLifeMonth.Text = "0"
            tbAmountForexDt.Text = "0"
            ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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
            If tbSubled.Text.Trim = "" And tbFgSubled.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubled.Focus()
                Return False
            End If
            If ddlWrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlWrhs.Focus()
                Return False
            End If
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
                If Dr("FACode").ToString = "" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Must Have Value")
                    Return False
                End If
                If Dr("FAName").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Name Must Have Value")
                    Return False
                End If
                If Dr("Product").ToString = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Location").ToString = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    Return False
                End If
                If Dr("FAStatus").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Status Must Have Value")
                    Return False
                End If
                If Dr("FASubGroup").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA SubGroup Must Have Value")
                    Return False
                End If
                If Dr("FALocationCode").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    Return False
                End If
                If Dr("CostCtr").ToString = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                    Return False
                End If

                If Dr("Currency").ToString = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If CFloat(Dr("LifeMonth").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Life Month Must Have Value")
                    Return False
                End If
                If CFloat(Dr("AmountForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Must Have Value")
                    Return False
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
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If ddlLocation.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    ddlLocation.Focus()
                    Return False
                End If
                If ddlFAStatus.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("FA Status Must Have Value")
                    ddlFAStatus.Focus()
                    Return False
                End If
                If tbFASubGroup.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA SubGroup Must Have Value")
                    tbFASubGroup.Focus()
                    Return False
                End If
                If tbFALocCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    tbFALocCode.Focus()
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
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbLifeMonth.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Life Month Must Have Value")
                    tbLifeMonth.Focus()
                    Return False
                End If
                If CFloat(tbAmountForexDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Must Have Value")
                    tbAmountForexDt.Focus()
                    Return False
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
            Dt = BindDataTransaction(GetStringHd(ViewState("FgExpendable").ToString), "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlWrhs, Dt.Rows(0)("Wrhs_Code").ToString)
            BindToText(tbSubled, Dt.Rows(0)("Subled").ToString)
            BindToText(tbSubledName, Dt.Rows(0)("Subled_Name").ToString)
            BindToText(tbFgSubled, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal FA As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FACode = " + QuotedStr(FA))
            If Dr.Length > 0 Then
                BindToText(tbFA, Dr(0)("FACode").ToString)
                BindToText(tbFAName, Dr(0)("FAName").ToString)
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                BindToDropList(ddlFAStatus, Dr(0)("FAStatus").ToString)
                BindToDropList(ddlFAOwner, Dr(0)("FAOwner").ToString)
                BindToText(tbFASubGroup, Dr(0)("FASubGroup").ToString)
                BindToText(tbFASubGroupName, Dr(0)("FASubGroupName").ToString)
                BindToDropList(ddlFALocType, Dr(0)("FALocationType").ToString)
                BindToText(tbFALocCode, Dr(0)("FALocationCode").ToString)
                BindToText(tbFALocName, Dr(0)("FA_Location_Name").ToString)
                BindToDropList(ddlFAProcess, Dr(0)("FgProcess").ToString)
                BindToDropList(ddlCostCenterDt, Dr(0)("CostCtr").ToString)
                BindToText(tbFALocCode, Dr(0)("FALocationCode").ToString)
                BindToText(tbFALocName, Dr(0)("FA_Location_Name").ToString)
                BindToDropList(ddlCurrDt, Dr(0)("Currency").ToString)
                BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbLifeMonth, Dr(0)("LifeMonth").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbAmountForexDt, Dr(0)("AmountForex").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("FACode = " + QuotedStr(tbFA.Text))(0)
                Row.BeginEdit()
                Row("FACode") = tbFA.Text
                Row("FAName") = tbFAName.Text
                Row("Product") = tbProduct.Text
                Row("Product_Name") = tbProductName.Text
                Row("Unit") = tbUnit.Text
                Row("Location") = ddlLocation.SelectedValue
                Row("Location_Name") = ddlLocation.SelectedItem.Text
                Row("Qty") = tbQty.Text
                Row("FAStatus") = ddlFAStatus.SelectedValue
                Row("FAStatusName") = ddlFAStatus.Text
                Row("FAOwner") = ddlFAOwner.SelectedValue
                Row("FASubGroup") = tbFASubGroup.Text
                Row("FASubGroupName") = tbFASubGroupName.Text
                Row("FALocationType") = ddlFALocType.Text
                Row("FALocationCode") = tbFALocCode.Text
                Row("FA_Location_Name") = tbFALocName.Text
                Row("Currency") = ddlCurrDt.SelectedValue
                Row("FgProcess") = ddlFAProcess.SelectedValue
                Row("ForexRate") = tbRateDt.Text
                Row("CostCtr") = ddlCostCenterDt.SelectedValue
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                End If
                Row("CostCtrName") = ddlCostCenterDt.SelectedItem.Text
                Row("LifeMonth") = tbLifeMonth.Text
                Row("AmountForex") = tbAmountForexDt.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FACode") = tbFA.Text
                dr("FAName") = tbFAName.Text
                dr("Product") = tbProduct.Text
                dr("Product_Name") = tbProductName.Text
                dr("Unit") = tbUnit.Text
                dr("Location") = ddlLocation.SelectedValue
                dr("Location_Name") = ddlLocation.SelectedItem.Text
                dr("Qty") = tbQty.Text
                dr("FAStatus") = ddlFAStatus.SelectedValue
                dr("FAStatusName") = ddlFAStatus.Text
                dr("FAOwner") = ddlFAOwner.SelectedValue
                dr("FASubGroup") = tbFASubGroup.Text
                dr("FASubGroupName") = tbFASubGroupName.Text
                dr("FALocationType") = ddlFALocType.Text
                dr("FALocationCode") = tbFALocCode.Text
                dr("FA_Location_Name") = tbFALocName.Text
                dr("Currency") = ddlCurrDt.SelectedValue
                dr("FgProcess") = ddlFAProcess.SelectedValue
                dr("ForexRate") = tbRateDt.Text
                dr("CostCtr") = ddlCostCenterDt.SelectedValue
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                End If
                dr("CostCtrName") = ddlCostCenterDt.SelectedItem.Text
                dr("LifeMonth") = tbLifeMonth.Text
                dr("AmountForex") = tbAmountForexDt.Text

                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
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
            If PnlDt.Visible = False Then
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
                If ViewState("FgExpendable").ToString = "Y" Then
                    tbCode.Text = GetAutoNmbr("EAW", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                Else
                    tbCode.Text = GetAutoNmbr("FAW", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                End If
                SQLString = "INSERT INTO GLFAFromStockHd (TransNmbr, TransDate, STATUS, FgReport, Warehouse, FgSubled, Subled, " + _
                "Operator, Remark, FgExpendable, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', 'Y', " + _
                QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbFgSubled.Text) + ", " + QuotedStr(tbSubled.Text) + ", " + QuotedStr(tbOperator.Text) + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("FgExpendable").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFAFromStockHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFAFromStockHd SET Operator =" + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", Warehouse = " + QuotedStr(ddlWrhs.SelectedValue) + ", FgSubled = " + QuotedStr(tbFgSubled.Text) + ", Subled = " + QuotedStr(tbSubled.Text) + _
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FACode, FAName, Product, Unit, Location, Qty, FAStatus, FAOwner, FASubGroup, FALOcationType, FALocationCode, LifeMonth, Currency, ForexRate, AmountForex, FgProcess, CostCtr  FROM GLFAFromStockDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFAFromStockDt")

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
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            ddlWrhs.SelectedIndex = 0
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
            If ViewState("SetLocation") Then
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            PnlDt.Visible = True
            BindDataDt("")
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
            FilterName = "Reference, Warehouse, Operator, Remark"
            FilterValue = "TransNmbr, Wrhs_Name, Operator, Remark"
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
                    tbSubled.Enabled = tbFgSubled.Text <> "N"
                    btnSubled.Visible = tbSubled.Enabled
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
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
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = False
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurrDt.SelectedValue, ViewState("DBConnection").ToString)
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("SelectCommand") = "EXEC S_GLFormFAFromStock '" + GVR.Cells(2).Text + "'"
                        Session("ReportFile") = ".../../../Rpt/FormFAFromStock.frx"
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
            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If GetCountRecord(ViewState("Dt")) = 0 Then
                    Exit Sub
                Else
                    MultiView1.ActiveViewIndex = 1
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("FACode = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            If ViewState("SetLocation") Then
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFASubGroup.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsFAGroupSub WHERE Fg_Expendable = " + QuotedStr(ViewState("FgExpendable").ToString)
            ResultField = "FA_SubGrp_Code, FA_SubGrp_Name, Fg_Process, LifeMonth"
            ViewState("Sender") = "btnFASubGroup"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search FASubGroup Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnFALoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFALoc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsFALocationAll where FA_Location_Type=" + QuotedStr(ddlFALocType.SelectedValue)
            ResultField = "FA_Location_Code, FA_Location_Name"
            ViewState("Sender") = "btnFALoc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
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
            dt = SQLExecuteQuery("select FA_SubGrp_Code, FA_SubGrp_Name, LifeMonth, Fg_Process from VMsFAGroupSub WHERE Fg_Expendable = " + QuotedStr(ViewState("FgExpendable").ToString) + " AND FA_SubGrp_Code = " + QuotedStr(tbFASubGroup.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            Dr = FindMaster("FAGroupSub", tbFASubGroup.Text, ViewState("DBConnection").ToString)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbFASubGroup.Text = Dr("FA_SubGrp_Code").ToString
                tbFASubGroupName.Text = Dr("FA_SubGrp_Name").ToString
                tbLifeMonth.Text = Dr("LifeMonth").ToString
                ddlFAProcess.SelectedValue = Dr("Fg_Process").ToString
            Else
                tbFASubGroup.Text = ""
                tbFASubGroupName.Text = ""
                tbLifeMonth.Text = "0"
                ddlFAProcess.SelectedValue = "Y"
            End If
            tbFASubGroup.Focus()
        Catch ex As Exception
            Throw New Exception("tb FAGroupSub Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbFALocCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFALocCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("FALocationAll", tbFALocCode.Text + "|" + ddlFALocType.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
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

    Protected Sub ddlWrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", ddlWrhs.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubled.Text = Dr("FgSubLed")
                tbSubled.Text = ""
                tbSubledName.Text = ""
            Else
                tbFgSubled.Text = "N"
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
            ViewState("SetLocation") = True
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            ddlWrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct "
            ResultField = "Product_Code, Product_Name, Specification, Unit"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbProduct.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProduct.Text = Dr("Product_Code").ToString
                tbProductName.Text = Dr("Product_Name").ToString + " " + TrimStr(Dr("Specification").ToString)
                tbUnit.Text = Dr("Unit").ToString
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbUnit.Text = ""
            End If
            ddlLocation.Focus()
        Catch ex As Exception
            Throw New Exception("tb FALocation Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgActive = 'Y' AND FgSubled = " + QuotedStr(tbFgSubled.Text)
            ResultField = "Subled_No, Subled_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSubled_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubled.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgActive = 'Y' AND FgSubled = " + QuotedStr(tbFgSubled.Text) + " AND Subled_No = " + QuotedStr(tbSubled.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbSubled.Text = Dr("Subled_No")
                tbSubledName.Text = Dr("Subled_Name")
            Else
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubled_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
