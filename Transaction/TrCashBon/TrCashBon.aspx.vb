Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrCashBon_TrCashBon
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FNCashBonHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                If ViewState("Sender") = "btnAccount" Then
                    tbAccount.Text = Session("Result")(0).ToString
                    tbAccountName.Text = Session("Result")(1).ToString
                    tbFgSubled.Text = Session("Result")(2).ToString
                    tbFgCostCtr.Text = Session("Result")(3).ToString
                    tbSubled.Text = ""
                    tbSubledName.Text = ""

                    tbSubled.Enabled = tbFgSubled.Text <> "N"
                    btnSubled.Visible = tbSubled.Enabled
                    If tbFgSubled.Text = "N" Then
                        tbSubled.Text = ""
                        tbSubledName.Text = ""
                    End If
                    ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
                    If tbFgCostCtr.Text = "N" Then
                        ddlCostCenter.SelectedIndex = 0
                    End If
                ElseIf ViewState("Sender") = "btnSubled" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnPettyNo" Then
                    BindToText(tbPettyNo, Session("Result")(0).ToString)
                    BindToText(tbAccCodeHd, Session("Result")(1).ToString)
                    BindToText(tbAccNameHd, Session("Result")(2).ToString)
                    BindToText(tbFgSubledHd, Session("Result")(3).ToString)
                    BindToText(tbSubledCodeHd, Session("Result")(4).ToString)
                    BindToText(tbSubledNameHd, Session("Result")(5).ToString)
                    BindToDropList(ddlCurr, Session("Result")(6).ToString)
                    BindToText(tbRate, Session("Result")(7).ToString)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
                    tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
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

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlCostCenter, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbAmountForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")
            tbTotalForex.Attributes.Add("OnBlur", "setformatdt();")
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
        Return "SELECT * From V_FNCashBonDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNo"
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
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
                Dim QCNo As String

                Pertamax = True
                Result = ""
                QCNo = ""

                'For Each GVR In GridView1.Rows
                '    CB = GVR.FindControl("cbSelect")
                '    If CB.Checked Then
                '        If GVR.Cells(3).Text = "P" Then
                '            ListSelectNmbr = GVR.Cells(2).Text
                '            If Pertamax Then
                '                Result = "'''" + ListSelectNmbr + "''"
                '                QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                '                Pertamax = False
                '            Else
                '                Result = Result + ",''" + ListSelectNmbr + "''"
                '            End If
                '        End If
                '    End If
                'Next
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
                Session("DBConnection") = ViewState("DBConnection")
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_FNCashBonForm" + Result + "," + QuotedStr(ViewState("UserId").ToString)
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormFNCashBon.frx"

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
                        Result = ExecSPCommandGo(ActionValue, "S_FNCashBon", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
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
            btnPettyNo.Visible = State
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
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
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
            tbPettyNo.Text = ""
            tbAccCodeHd.Text = ""
            tbAccNameHd.Text = ""
            tbFgSubledHd.Text = ""
            tbSubledCodeHd.Text = ""
            tbSubledNameHd.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            ddlCurr_SelectedIndexChanged(Nothing, Nothing)
            tbRemark.Text = ""
            tbTotalForex.Text = FormatNumber(0, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbRemarkDt.Text = ""
            tbFgSubled.Text = "N"
            tbAccount.Text = ""
            tbAccountName.Text = ""
            tbAmountForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbFgSubled.Text = "N"
            tbFgCostCtr.Text = "N"

            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            If tbFgSubled.Text = "N" Then
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If

            ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
            If tbFgCostCtr.Text = "N" Then
                ddlCostCenter.SelectedIndex = 0
            End If

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
            If tbPettyNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Petty No Must Have Value")
                tbPettyNo.Focus()
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
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then

            Else
                If tbAccount.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    tbAccount.Focus()
                    Return False
                End If
                If CFloat(tbAmountForex.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Must Have Value")
                    tbAmountForex.Focus()
                    Return False
                End If
                If tbFgSubled.Text <> "N" Then
                    If tbSubled.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Subled must have value")
                        tbSubled.Focus()
                        Return False
                    End If
                End If
                If tbFgCostCtr.Text <> "N" Then
                    If ddlCostCenter.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("Cost Center must have value")
                        ddlCostCenter.Focus()
                        Return False
                    End If
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
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbPettyNo, Dt.Rows(0)("PettyNo").ToString)
            BindToText(tbAccCodeHd, Dt.Rows(0)("Account").ToString)
            BindToText(tbAccNameHd, Dt.Rows(0)("Account_Name").ToString)
            BindToText(tbFgSubledHd, Dt.Rows(0)("FgSubled").ToString)
            BindToText(tbSubledCodeHd, Dt.Rows(0)("Subled").ToString)
            BindToText(tbSubledNameHd, Dt.Rows(0)("SubledName").ToString)
            'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            tbRate.Enabled = ddlCurr.SelectedValue <> ViewState("Currency")
            If ddlCurr.SelectedValue = ViewState("Currency") Then
                ViewState("DigitCurr") = ViewState("DigitHome")
            Else
                ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            End If
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo
                BindToText(tbFgSubled, Dr(0)("FgSubled").ToString)
                BindToText(tbAccount, Dr(0)("Account").ToString)
                BindToText(tbAccountName, Dr(0)("AccountName").ToString)
                BindToText(tbSubled, Dr(0)("Subled").ToString)
                BindToText(tbSubledName, Dr(0)("SubledName").ToString)
                BindToDropList(ddlCostCenter, Dr(0)("CostCtr").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString, ViewState("DigitCurr"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbFgCostCtr, Dr(0)("FgCostCtr").ToString)
                tbSubled.Enabled = tbFgSubled.Text <> "N"
                btnSubled.Visible = tbSubled.Enabled
                ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("Account") = tbAccount.Text
                Row("AccountName") = tbAccountName.Text
                Row("FgSubled") = tbFgSubled.Text
                Row("Subled") = tbSubled.Text
                Row("SubledName") = tbSubledName.Text
                Row("AmountForex") = tbAmountForex.Text
                Row("CostCtr") = ddlCostCenter.SelectedValue
                Row("CostCtrName") = ddlCostCenter.SelectedItem.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgCostCtr") = tbFgCostCtr.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("Account") = tbAccount.Text
                dr("AccountName") = tbAccountName.Text
                dr("FgSubled") = tbFgSubled.Text
                dr("Subled") = tbSubled.Text
                dr("SubledName") = tbSubledName.Text
                dr("AmountForex") = tbAmountForex.Text
                dr("CostCtr") = ddlCostCenter.SelectedValue
                dr("CostCtrName") = ddlCostCenter.SelectedItem.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgCostCtr") = tbFgCostCtr.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            countDt()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Private Sub countDt()
        Dim dr As DataRow
        Dim hasil As Double
        Try
            hasil = 0

            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    hasil = hasil + CFloat(dr("AmountForex").ToString)
                End If
            Next
            tbTotalForex.Text = FormatNumber(hasil, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("Count Dt Error : " + ex.ToString)
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
                tbCode.Text = GetAutoNmbr("PCB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINCashBonHd (TransNmbr, transDate, STATUS, " + _
                "PettyNo, Account, FgSubled, Subled, " + _
                "Currency, ForexRate, TotalForex, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbPettyNo.Text) + "," + QuotedStr(tbAccCodeHd.Text) + "," + _
                QuotedStr(tbFgSubledHd.Text) + "," + QuotedStr(tbSubledCodeHd.Text) + "," + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + _
                ", " + tbTotalForex.Text.Replace(",", "") + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINCashBonHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINCashBonHd SET PettyNo =" + QuotedStr(tbPettyNo.Text) + _
                ", Account =" + QuotedStr(tbAccCodeHd.Text) + ", FgSubled =" + QuotedStr(tbFgSubledHd.Text) + _
                ", Subled =" + QuotedStr(tbFgSubledHd.Text) + ", Currency =" + QuotedStr(ddlCurr.SelectedValue) + _
                ", ForexRate =" + tbRate.Text.Replace(",", "") + _
                ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark =" + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Account, FgSubled, Subled, CostCtr, AmountForex, Remark FROM FINCashBonDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE FINCashBonDt SET Account = @Account, FgSubled = @FgSubled, Subled = @Subled, CostCtr = @CostCtr, AmountForex = @AmountForex, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 20, "Account")
            Update_Command.Parameters.Add("@FgSubled", SqlDbType.VarChar, 15, "FgSubled")
            Update_Command.Parameters.Add("@Subled", SqlDbType.VarChar, 80, "Subled")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 80, "CostCtr")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 18, "AmountForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINCashBonDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINCashBonDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)            
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            EnableHd(False)
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            PnlDt.Visible = True
            BindDataDt("")
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
            FilterName = "Reference, Date, Status,  Currency, Petty No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Currency, PettyNo, Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        tbRate.Enabled = ddlCurr.SelectedValue <> ViewState("Currency")
                        btnHome.Visible = False
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNCashBonForm ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormFNCashBon.frx"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            countDt()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("Product") = GVR.Cells(1).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRate.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccount.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsAccountDt WHERE TransType = 'PCB' AND (( Currency = " + QuotedStr(ddlCurr.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL'))" '" OR Currency = " + QuotedStr(ViewState("Currency").ToString) + 
            ResultField = "Account, Description, FgSubled, FgCostCtr"
            ViewState("Sender") = "btnAccount"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Account Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccount.TextChanged
        Dim Dr As DataRow
        Try
            If tbAccount.Text.Trim = "" Then
                tbAccount.Text = ""
                tbAccountName.Text = ""
                tbFgSubled.Text = "N"
                tbSubled.Text = ""
                tbSubledName.Text = ""
                Exit Sub
            End If
            Dr = FindMaster("Account", tbAccount.Text + "|CBR", ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                If (TrimStr(Dr("Currency").ToString) = ddlCurr.SelectedValue And TrimStr(Dr("FgType").ToString) = "BS") Or (TrimStr(Dr("FgType").ToString) = "PL") Then 'Or TrimStr(Dr("Currency").ToString) = ViewState("Currency").ToString 
                    tbAccount.Text = TrimStr(Dr("Account").ToString)
                    tbAccountName.Text = TrimStr(Dr("AccountName").ToString)
                    tbFgSubled.Text = TrimStr(Dr("FgSubled").ToString)
                    tbFgCostCtr.Text = TrimStr(Dr("FgCostCtr").ToString)
                Else
                    tbAccount.Text = ""
                    tbAccountName.Text = ""
                    tbFgSubled.Text = ""
                    tbFgCostCtr.Text = ""
                End If
            Else
                tbAccount.Text = ""
                tbAccountName.Text = ""
                tbFgSubled.Text = ""
                tbFgCostCtr.Text = ""
            End If

            tbSubled.Text = ""
            tbSubledName.Text = ""

            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            If tbFgSubled.Text = "N" Then
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If

            ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
            If tbFgCostCtr.Text = "N" Then
                ddlCostCenter.SelectedIndex = 0
            End If

            ChangeFgSubLed(tbFgSubled, tbSubled, btnSubled)
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            If tbFgSubled.Text = "N" Then
                Exit Sub
            End If
            Session("filter") = "SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgSubled =" + QuotedStr(tbFgSubled.Text)
            ResultField = "Subled_No, Subled_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Subled Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbSubled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubled.TextChanged
        Dim Dr As DataRow
        Try
            If tbSubled.Text.Trim = "" Or tbFgSubled.Text = "N" Then
                tbSubled.Text = ""
                tbSubledName.Text = ""
                Exit Sub
            End If
            Dr = FindMaster("Subled", tbFgSubled.Text + "|" + tbSubled.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubled.Text = Dr("SubLed_No")
                tbSubledName.Text = Dr("SubLed_Name")
            Else
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Subled change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPettyNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPettyNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_FNPettyForCashBon"
            ResultField = "Reference, Account, Account_Name, FgSubLed, SubLed, SubLed_Name, Currency, Rate, Amount_Forex"
            ViewState("Sender") = "btnPettyNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn PettyNo Click Error : " + ex.ToString
        End Try
    End Sub
End Class
