Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrFARevaluation_TrFARevaluation
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFARevaluationHd"

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
                If ViewState("Sender") = "btnFA" Then
                    '"FA_Code, FA_Name, LifeCurrent, Total_Current, Total_NDA"
                    tbFACode.Text = Session("Result")(0).ToString
                    tbFAName.Text = Session("Result")(1).ToString
                    BindToText(tbLifeBal, Session("Result")(2).ToString)
                    BindToText(tbLifeNew, Session("Result")(2).ToString)
                    'BindToText(tbAmountBal, Session("Result")(3).ToString)
                    'BindToText(tbAmountNew, Session("Result")(3).ToString)
                    BindToText(tbAmountBalNDA, Session("Result")(4).ToString)
                    BindToText(tbAmountNewNDA, Session("Result")(4).ToString)
                    BindToText(tbAmountBalDA, FormatNumber(CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString), ViewState("DigitHome")))
                    BindToText(tbAmountNewDA, FormatNumber(CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString), ViewState("DigitHome")))
                    AttachScript("setformatdt();", Page, Me.GetType())
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
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbLifeBal.Attributes.Add("ReadOnly", "True")
            tbLifeRev.Attributes.Add("ReadOnly", "True")
            tbAmountBal.Attributes.Add("ReadOnly", "True")
            tbAmountBalNDA.Attributes.Add("ReadOnly", "True")
            tbAmountBalDA.Attributes.Add("ReadOnly", "True")
            tbAmountRev.Attributes.Add("ReadOnly", "True")
            tbAmountNew.Attributes.Add("ReadOnly", "True")

            tbLifeNew.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbAmountNew.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountNewNDA.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountNewDA.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbLifeBal.Attributes.Add("OnBlur", "setformatdt();")
            tbLifeNew.Attributes.Add("OnBlur", "setformatdt();")
            tbLifeRev.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountBal.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountBalNDA.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountBalDA.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountNew.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountNewNDA.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountNewDA.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountRev.Attributes.Add("OnBlur", "setformatdt();")

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
        Return "SELECT * From V_GLFARevaluationDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                    Result = ExecSPCommandGo(ActionValue, "S_GLFARevaluation", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnFA.Enabled = State
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
            ddlFgRemove.SelectedIndex = 1
            tbFACode.Text = ""
            tbFAName.Text = ""
            tbLifeBal.Text = "0"
            tbLifeNew.Text = "0"
            tbLifeRev.Text = "0"
            tbAmountBal.Text = "0"
            tbAmountBalNDA.Text = "0"
            tbAmountBalDA.Text = "0"
            tbAmountNew.Text = "0"
            tbAmountNewNDA.Text = "0"
            tbAmountNewDA.Text = "0"
            tbAmountRev.Text = "0"
            tbRemarkDt.Text = ""
            tbLifeNew.Enabled = True
            tbAmountNew.Enabled = True
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
                If Dr("FA_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Must Have Value")
                    Return False
                End If

                If Dr("BalanceLife").ToString = "" Then
                    lbStatus.Text = MessageDlg("Life Balance Must Have Value")
                    Return False
                End If
                If Dr("NewLife").ToString = "" Then
                    lbStatus.Text = MessageDlg("Life New Must Have Value")
                    Return False
                End If
                If Dr("RevLife").ToString = "" Then
                    lbStatus.Text = MessageDlg("Life Revaluation Must Have Value")
                    Return False
                End If

                If Dr("AmountBal").ToString = "" Then
                    lbStatus.Text = MessageDlg("Amount Balance Must Have Value")
                    Return False
                End If
                If Dr("AmountNew").ToString = "" Then
                    lbStatus.Text = MessageDlg("Amount New Must Have Value")
                    Return False
                End If
                If Dr("AmountRev").ToString = "" Then
                    lbStatus.Text = MessageDlg("Amount Revaluation Must Have Value")
                    Return False
                End If
            Else

                If tbFACode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA Must Have Value")
                    tbFACode.Focus()
                    Return False
                End If

                If tbLifeBal.Text = "" Then
                    lbStatus.Text = MessageDlg("Life Balance Must Have Value")
                    tbLifeBal.Focus()
                    Return False
                End If
                If tbLifeNew.Text = "" Then
                    lbStatus.Text = MessageDlg("Life New Must Have Value")
                    tbLifeNew.Focus()
                    Return False
                End If
                If tbLifeRev.Text = "" Then
                    lbStatus.Text = MessageDlg("Life Revaluation Must Have Value")
                    tbLifeRev.Focus()
                    Return False
                End If

                'If tbAmountBalNDA.Text = "" Then
                '    lbStatus.Text = MessageDlg("Amount Balance NDA Must Have Value")
                '    tbAmountBalNDA.Focus()
                '    Return False
                'End If

                'If tbAmountBalDA.Text = "" Then
                '    lbStatus.Text = MessageDlg("Amount Balance DA Must Have Value")
                '    tbAmountBalDA.Focus()
                '    Return False
                'End If

                If tbAmountBal.Text = "" Then
                    lbStatus.Text = MessageDlg("Amount Balance Must Have Value")
                    tbAmountBal.Focus()
                    Return False
                End If

                If tbAmountNew.Text = "" Then
                    lbStatus.Text = MessageDlg("Amount New Must Have Value")
                    tbAmountNew.Focus()
                    Return False
                End If

                'If tbAmountNewNDA.Text = "" Then
                '    lbStatus.Text = MessageDlg("Amount New NDA Must Have Value")
                '    tbAmountNewNDA.Focus()
                '    Return False
                'End If
                'If tbAmountNewDA.Text = "" Then
                '    lbStatus.Text = MessageDlg("Amount New DA Must Have Value")
                '    tbAmountNewDA.Focus()
                '    Return False
                'End If

                If tbAmountRev.Text = "" Then
                    lbStatus.Text = MessageDlg("Amount Revaluation Must Have Value")
                    tbAmountRev.Focus()
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
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
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
                BindToDropList(ddlFgRemove, Dr(0)("FgRemove").ToString)
                BindToText(tbFACode, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FA_Name").ToString)
                BindToText(tbLifeBal, Dr(0)("BalanceLife").ToString)
                BindToText(tbLifeNew, Dr(0)("NewLife").ToString)
                BindToText(tbLifeRev, Dr(0)("RevLife").ToString)
                BindToText(tbAmountBal, Dr(0)("BalanceAmount").ToString)
                BindToText(tbAmountNew, Dr(0)("NewAmount").ToString)
                BindToText(tbAmountRev, Dr(0)("RevAmount").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbAmountBalNDA, Dr(0)("BalanceNDA").ToString)
                BindToText(tbAmountBalDA, Dr(0)("BalanceDA").ToString)
                BindToText(tbAmountNewNDA, Dr(0)("NewAmountNDA").ToString)
                BindToText(tbAmountNewDA, Dr(0)("NewAmountDA").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Private Function AllowedRecordDt() As Integer
        Try
            If ViewState("FixedAsset") = tbFACode.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            Dim Row As DataRow
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("FixedAsset = " + QuotedStr(tbFACode.Text))

            'Dim Row As DataRow
            If ExistRow.Length > AllowedRecordDt() Then
                lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Row = ViewState("Dt").Select("FixedAsset = " + QuotedStr(ViewState("FixedAsset")))(0)
                Row("FixedAsset") = tbFACode.Text
                Row("FA_Name") = tbFAName.Text
                Row("FgRemove") = ddlFgRemove.SelectedValue
                Row("BalanceLife") = tbLifeBal.Text
                Row("BalanceAmount") = tbAmountBal.Text
                Row("NewLife") = tbLifeNew.Text
                Row("NewAmount") = tbAmountNew.Text
                Row("RevLife") = tbLifeRev.Text
                Row("RevAmount") = tbAmountRev.Text
                Row("Remark") = tbRemarkDt.Text
                Row("BalanceNDA") = tbAmountBalNDA.Text
                Row("BalanceDA") = tbAmountBalDA.Text
                Row("NewAmountNDA") = tbAmountNewNDA.Text
                Row("NewAmountDA") = tbAmountNewDA.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFACode.Text
                dr("FA_Name") = tbFAName.Text
                dr("FgRemove") = ddlFgRemove.SelectedValue
                dr("BalanceLife") = tbLifeBal.Text
                dr("BalanceAmount") = tbAmountBal.Text
                dr("NewLife") = tbLifeNew.Text
                dr("NewAmount") = tbAmountNew.Text
                dr("RevLife") = tbLifeRev.Text
                dr("RevAmount") = tbAmountRev.Text
                dr("Remark") = tbRemarkDt.Text
                dr("BalanceNDA") = tbAmountBalNDA.Text
                dr("BalanceDA") = tbAmountBalDA.Text
                dr("NewAmountNDA") = tbAmountNewNDA.Text
                dr("NewAmountDA") = tbAmountNewDA.Text
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("FAR", ddlFgReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO GLFARevaluationHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "Operator, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlFgReport.SelectedValue) + "," + _
                QuotedStr(tbOperator.Text) + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFARevaluationHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFARevaluationHd SET Operator =" + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FixedAsset, BalanceLife, BalanceAmount, FgRemove, NewLife, NewAmount, RevLife, RevAmount, Remark, BalanceNDA, BalanceDA, NewAmountNDA, NewAmountDA FROM GLFARevaluationDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFARevaluationDt")

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
            FilterName = "Reference, Operator, Remark"
            FilterValue = "TransNmbr, Operator, Remark"
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = False
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
                        Session("SelectCommand") = "EXEC S_GLFormFARevaluation " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormFARevaluation.frx"
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

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As DataRow = ViewState("Dt").Rows(e.RowIndex)
            dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR("FixedAsset").ToString))
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
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            'ddlFgRemove_SelectedIndexChanged(Nothing, Nothing)
            ViewState("StateDt") = "Edit"
            ViewState("FixedAsset") = GVR.Cells(1).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnFA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFA.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT FA_Code, FA_Name, FA_Sub_Group, FA_Sub_Group_Name, LifeCurrent, Total_Current, Total_NDA FROM VMsFixedAsset WHERE FgActive = 'Y' AND FgExpendable ='N'"
            ResultField = "FA_Code, FA_Name, LifeCurrent, Total_Current, Total_NDA"
            ViewState("Sender") = "btnFA"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search FA Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFACode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFACode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim DA As Double
        Try
            'Dr = FindMaster("FA", tbFACode.Text, ViewState("DBConnection").ToString)
            Dt = SQLExecuteQuery("SELECT FA_Code, FA_Name, FA_Sub_Group, FA_Sub_Group_Name, LifeCurrent, Total_Current, Total_NDA FROM VMsFixedAsset WHERE FgActive = 'Y' AND FgExpendable ='N' AND FA_Code = " + QuotedStr(tbFACode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbFACode.Text = Dr("FA_Code")
                tbFAName.Text = Dr("FA_Name")
                tbLifeBal.Text = Dr("LifeCurrent")
                tbLifeNew.Text = Dr("LifeCurrent")
                tbAmountBal.Text = Dr("Total_Current")
                tbAmountBalNDA.Text = Dr("Total_NDA")
                'tbAmountBalDA.Text = Dr("Total_DA")
                DA = Dr("Total_Current") - Dr("Total_NDA")
                tbAmountBalDA.Text = FormatFloat(DA, 2)
                tbAmountNew.Text = Dr("Total_Current")
                tbAmountNewNDA.Text = Dr("Total_NDA")
                'tbAmountNewDA.Text = Dr("Total_DA")
                tbAmountNewDA.Text = FormatFloat(DA, 2)
            Else
                tbFACode.Text = ""
                tbFAName.Text = ""
                tbLifeNew.Text = "0"
                'tbAmountNew.Text = "0"
                tbAmountBalNDA.Text = "0"
                tbAmountBalDA.Text = "0"
                tbAmountBal.Text = "0"
                tbAmountNewNDA.Text = "0"
                tbAmountNewDA.Text = "0"
            End If
            tbFACode.Focus()
        Catch ex As Exception
            Throw New Exception("tb FAGroupSub Error : " + ex.ToString)
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

    Protected Sub ddlFgRemove_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgRemove.SelectedIndexChanged
        tbLifeNew.Enabled = ddlFgRemove.SelectedValue = "N"
        'tbAmountNew.Enabled = ddlFgRemove.SelectedValue = "N"
        tbFACode_TextChanged(Nothing, Nothing)
        If ddlFgRemove.SelectedValue = "Y" Then
            tbLifeNew.Text = "0"
            'tbAmountNew.Text = "0"
            tbAmountNewNDA.Text = "0"
            tbAmountNewDA.Text = "0"
        End If
        tbAmountNew.Text = CFloat(tbAmountNewNDA.Text) + CFloat(tbAmountNewDA.Text)
        tbAmountRev.Text = CFloat(tbAmountNew.Text) - CFloat(tbAmountBal.Text)
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
End Class
