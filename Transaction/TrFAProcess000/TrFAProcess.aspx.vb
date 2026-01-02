Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class Transaction_TrFAProcess_TrFAProcess
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFAProcessHd"

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
                If ViewState("Sender") = "btnGetDt" Then
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
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
        FillCombo(ddlYear, "EXEC S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
        FillCombo(ddlPeriod, "EXEC S_GetPeriod", True, "Period", "Description", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        tbLife.Attributes.Add("ReadOnly", "True")
        tbBalanceAmount.Attributes.Add("ReadOnly", "True")
        tbProcess.Attributes.Add("ReadOnly", "True")
        tbTotal.Attributes.Add("ReadOnly", "True")
        Me.tbAdjust.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDebitForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbCreditForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbRate.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        'Me.tbDebitForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        'Me.tbCreditForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
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
            DV.Sort = "Nmbr DESC " 'ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Year As String, ByVal Month As String) As String
        '" AND Period = 'JE' + ""
        Return "SELECT * FROM V_GLFAProcessDt WHERE Year = " + Year + " AND Period = " + Month + _
        " ORDER BY FixedAsset "
    End Function

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
        'Dim Status, msg As String
        'Dim Result, ListSelectNmbr, ActionValue As String
        'Dim Nmbr(100) As String

        'Dim j As Integer
        'Try
        '    If sender.ID.ToString = "BtnGo" Then
        '        ActionValue = ddlCommand.SelectedValue
        '    Else
        '        ActionValue = ddlCommand2.SelectedValue
        '    End If
        '    If ActionValue = "Print" Then
        '        Dim GVR As GridViewRow
        '        Dim CB As CheckBox
        '        Dim Pertamax As Boolean

        '        Pertamax = True
        '        Result = ""

        '        For Each GVR In GridView1.Rows
        '            CB = GVR.FindControl("cbSelect")
        '            If CB.Checked Then
        '                If GVR.Cells(4).Text = "P" Then
        '                    ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
        '                    If Pertamax Then
        '                        Result = "'''" + ListSelectNmbr + "''"
        '                        Pertamax = False
        '                    Else
        '                        Result = Result + ",''" + ListSelectNmbr + "''"
        '                    End If
        '                End If
        '            End If
        '        Next
        '        Result = Result + "'"

        '        Session("SelectCommand") = "EXEC S_PRFormPO " + Result
        '        Session("ReportFile") = ".../../../Rpt/FormPO.frx"
        '        AttachScript("openprintdlg();", Page, Me.GetType)
        '    Else
        '        Status = CekStatus(ActionValue)

        '        ListSelectNmbr = ""
        '        msg = ""
        '        '3 = status, 2 & 3 = key, 
        '        GetListCommand(Status, GridView1, "6,3,4", ListSelectNmbr, Nmbr, msg)
        '        If ListSelectNmbr = "" Then Exit Sub
        '        For j = 0 To (Nmbr.Length - 1)                   
        '            'Result = ExecSPCommandGo(ActionValue, "S_GLFAProcess", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        '            Result = ExecSPCommandGo(ActionValue, "S_GLFAProcess", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

        '            If Trim(Result) <> "" Then
        '                lbStatus.Text = lbStatus.Text + Result + " <br/>"
        '            End If
        '        Next
        '        BindData("Year+'|'+Month in (" + ListSelectNmbr + ")")
        '        If msg.Trim <> "" Then
        '            lbStatus.Text = MessageDlg(msg)
        '        End If
        '    End If
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

            GetListCommand(Status, GridView1, "6,3,4", ListSelectNmbr, Nmbr, lbStatus.Text)

            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_GLFAProcess", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("Nmbr2 in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            btnGetDt.Visible = State
            'tbRef.Enabled = State
            'ddlYear.Enabled = State
            'ddlPeriod.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Year As String, ByVal Month As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Year, Month), ViewState("DBConnection").ToString).Tables(0)
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
                Row = ViewState("Dt").Select("FixedAsset = " + QuotedStr(tbFACode.Text))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()

                Row("FixedAsset") = tbFACode.Text
                Row("FixedAssetName") = tbFAName.Text
                Row("CostCtr") = "" 'ddlCostCtr.SelectedValue
                Row("BalanceAmount") = tbBalanceAmount.Text
                Row("AdjustDepr") = tbAdjust.Text
                Row("TotalDepr") = tbTotal.Text
                Row("AmountDepr") = tbProcess.Text
                Row("BalanceLife") = tbLife.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFACode.Text
                dr("FixedAssetName") = tbFAName.Text
                dr("CostCtr") = ""
                dr("BalanceAmount") = tbBalanceAmount.Text
                dr("AdjustDepr") = tbAdjust.Text
                dr("TotalDepr") = tbTotal.Text
                dr("AmountDepr") = tbProcess.Text
                dr("BalanceLife") = tbLife.Text
                ViewState("Dt").Rows.Add(dr)
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
                'tbRef.Text = GetAutoNmbr("JE", "N", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO GLFAProcessHd (Year, Period, Status, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + ddlYear.SelectedValue + "', '" + ddlPeriod.SelectedValue + "', 'H', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                SQLString = "UPDATE GLFAProcessHD SET Remark = '" + tbRemark.Text + "'," + _
                "Year = " + ddlYear.SelectedValue + ", Period = " + ddlPeriod.SelectedValue + ", " + _
                "DateAppr = getDate()" + _
                " WHERE year = " + ViewState("Year") + " AND Period =  " + ViewState("Month")
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Reference IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Reference") = ddlYear.SelectedValue + ddlPeriod.SelectedValue
                Row(I)("Year") = ddlYear.SelectedValue
                Row(I)("Period") = ddlPeriod.SelectedValue
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()

            Dim cmdSql As New SqlCommand("SELECT Year, Period, FixedAsset, CostCtr, AmountDepr, AdjustDepr, TotalDepr, BalanceAmount, BalanceLife FROM GLFAProcessDt WHERE year = " + ddlYear.SelectedValue + " AND Period = " + ddlPeriod.SelectedValue.ToString, con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE GLFAProcessDt SET FixedAsset = @FixedAsset, AmountDepr = @AmountDepr, AdjustDepr = @AdjustDepr, TotalDepr = @TotalDepr, CostCtr = @CostCtr, BalanceAmount = @BalanceAmount, BalanceLife = @BalanceLife WHERE Year = '" & ViewState("Year") & "' AND Period = '" & ViewState("Month") & "' And FixedAsset = @FA", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@FixedAsset", SqlDbType.VarChar, 20, "FixedAsset")
            Update_Command.Parameters.Add("@AmountDepr", SqlDbType.Float, 18, "AmountDepr")
            Update_Command.Parameters.Add("@AdjustDepr", SqlDbType.Float, 18, "AdjustDepr")
            Update_Command.Parameters.Add("@TotalDepr", SqlDbType.Float, 18, "TotalDepr")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 10, "CostCtr")
            Update_Command.Parameters.Add("@BalanceAmount", SqlDbType.Float, 18, "BalanceAmount")
            Update_Command.Parameters.Add("@BalanceLife", SqlDbType.Int, 4, "BalanceLife")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@FA", SqlDbType.VarChar, 20, "FixedAsset")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM GLFAProcessDt WHERE [Year] = '" & ViewState("Year") & "' AND Period = '" & ViewState("Month") & "' And FixedAsset = @FA", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@FA", SqlDbType.VarChar, 20, "FixedAsset")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("GLFAProcessDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'Diremark karena membuat data yang d simpan tidak masuk semua
            'SQLString = "DELETE GLFAProcessDt WHERE Year =" + ddlYear.SelectedValue + " And Period = " + ddlPeriod.SelectedValue + " And Coalesce(FixedAsset,'') Not In " + _
            '            "(Select X.FixedAsset From GLFAProcessDt X WHERE X.Year ='2015' And X.Period = '1' And Coalesce(X.FixedAsset,'') " + _
            '            "Not In (Select Coalesce(Y.FACode,'') From MsFixedAsset Y Where COALESCE(Y.FgActive,'N') = 'N' AND COALESCE(Y.FgProcess,'N') = 'N'))"

            ''SQLString = "DELETE GLFAProcessDt WHERE Year = " + ddlYear.SelectedValue + " And Period = " + ddlPeriod.SelectedValue + " And Not Exists (Select 1 From MsFixedAsset X Where X.FACode = GLFAProcessDt.FixedAsset AND COALESCE(FgActive,'N') = 'N' AND COALESCE(FgProcess,'N') = 'N')"
            'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            ddlField.SelectedValue = "Year"
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
            btnHome.Visible = False
            ddlYear.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt(0, 0)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("S_GLFAProcessGetPeriod", ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Last Process Depreciation must be posted first")
                MovePanel(pnlInput, PnlHd)
                ModifyInput2(False, pnlInput, pnlDt, GridDt)
                Exit Sub
            Else
                ddlYear.SelectedValue = dt.Rows(0)("Year").ToString
                ddlPeriod.SelectedValue = dt.Rows(0)("Period").ToString
                ddlYear.Enabled = dt.Rows(0)("FgFirst").ToString = "Y"
                ddlPeriod.Enabled = dt.Rows(0)("FgFirst").ToString = "Y"
            End If
            tbRef.Text = ddlYear.SelectedValue
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            'ddlCostCtr.SelectedIndex = 0
            tbProcess.Text = "0"
            tbBalanceAmount.Text = "0"
            tbLife.Text = "0"
            tbAdjust.Text = "0"
            tbTotal.Text = "0"
            tbSpecification.Text = ""
            'ddlCurr.SelectedValue = ViewState("Currency")
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = "Reference must have value"
            '    tbRef.Focus()
            '    Return False
            'End If
            If ddlYear.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Year must have value")
                ddlYear.Focus()
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
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("FixedAsset").ToString.Trim = "" Then
                '    lbStatus.Text = "FixedAsset Must Have Value"
                '    Return False
                'End If
                'If Dr("FgSubled").ToString <> "N" And Dr("Subled").ToString.Trim = "" Then
                '    lbStatus.Text = "Subled Must Be Filled for FgSubled other than N"
                '    Return False
                'End If
                'If Dr("Currency").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Currency must have value")
                '    Return False
                'End If
                'If CFloat(Dr("ForexRate")) = 0 Then
                '    lbStatus.Text = "Rate must have value"
                '    Return False
                'End If
                'If CFloat(Dr("DebitForex")) = 0 And CFloat(Dr("CreditForex")) = 0 Then
                '    lbStatus.Text = "Debit or Credit Forex must have value"
                '    Return False
                'End If
            Else
                'If tbAccCode.Text.Trim = "" Then
                '    lbStatus.Text = "Account Must Have Value"
                '    tbAccCode.Focus()
                '    Return False
                'End If
                'If tbfgSubled.Text.Trim <> "N" And tbSubled.Text.Trim = "" Then
                '    lbStatus.Text = "Subled Must Be Filled for FgSubled other than N"
                '    tbSubled.Focus()
                '    Return False
                'End If
                'If ddlCurr.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Currency must have value")
                '    ddlCurr.Focus()
                '    Return False
                'End If
                'If CFloat(tbRate.Text) = 0 Then
                '    lbStatus.Text = "Rate must have value"
                '    tbRate.Focus()
                '    Return False
                'End If
                'If CFloat(tbDebitForex.Text) = 0 And CFloat(tbCreditForex.Text) = 0 Then
                '    lbStatus.Text = "Debit or Credit Forex must have value"
                '    tbDebitForex.Focus()
                '    Return False
                'End If
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Year, Period, Status, Remark"
            FilterValue = "Year, Period, Status, Remark"
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
            If Not e.CommandName = "Sort" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then

                    MovePanel(PnlHd, pnlInput)
                    'ViewState("Reference") = GVR.Cells(2).Text + GVR.Cells(3).Text
                    ViewState("Year") = GVR.Cells(3).Text
                    ViewState("Month") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Year"), ViewState("Month"))
                    BindDataDt(ViewState("Year"), ViewState("Month"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(6).Text = "H" Or GVR.Cells(6).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        'ViewState("Reference") = GVR.Cells(2).Text
                        ViewState("Year") = GVR.Cells(3).Text
                        ViewState("Month") = GVR.Cells(4).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Year"), ViewState("Month"))
                        FillTextBoxHd(ViewState("Year"), ViewState("Month"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetDt.Visible = True
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
                        Session("SelectCommand") = "EXEC S_GLFormFAProcess " + QuotedStr(GVR.Cells(3).Text + GVR.Cells(4).Text)
                        Session("ReportFile") = ".../../../Rpt/FormFAProcess.frx"
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
            ViewState("SortExpression") = e.SortExpression
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
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
            ViewState("StateDt") = "Edit"
            tbAdjust.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Dim AmountDepr As Decimal = 0
    Dim AdjustDepr As Decimal = 0
    Dim TotalDepr As Decimal = 0

    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "FixedAsset")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    AmountDepr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountDepr"))
                    AdjustDepr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AdjustDepr"))
                    TotalDepr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalDepr"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(5).Text = "Total:"
                    ' for the Footer, display the running totals
                    e.Row.Cells(6).Text = FormatNumber(AmountDepr, ViewState("DigitHome"))
                    e.Row.Cells(7).Text = FormatNumber(AdjustDepr, ViewState("DigitHome"))
                    e.Row.Cells(8).Text = FormatNumber(TotalDepr, ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim JEDS As DataSet
        Dim NewRow As DataRow
        Dim DR As DataRow
        Dim CekTrans As String
        Dim drow() As DataRow
        Dim dt As New DataTable
        Dim GVR As GridViewRow
        Dim baris As Integer
        Dim i As Integer
        Dim SqlString As String

        Try
            'If GetCountRecord(ViewState("Dt")) > 0 Then
            '    lbStatus.Text = MessageDlg("Data not empty")
            '    Exit Sub
            'End If

            If ViewState("StateHd") = "Edit" Then
                'BindDataDt(0, 0)
                ''''
                dt = ViewState("Dt")
                baris = dt.Rows.Count
                For i = 0 To baris - 1
                    GVR = GridDt.Rows(i)
                    drow = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(1).Text))
                    drow(0).Delete()
                Next
                ''''
                'BindGridDt(ViewState("Dt"), GridDt)
                'Exit Sub
                
            End If

            If ViewState("StateHd") = "Insert" Then
                CekTrans = SQLExecuteScalar("SELECT COUNT(CONVERT(VARCHAR(4),Year)+CONVERT(VARCHAR(2),Period)) FROM GLFAProcessHd WHERE YEAR = " + ddlYear.SelectedValue + " AND Period = " + ddlPeriod.SelectedValue, ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Process for " + ddlPeriod.SelectedItem.Text + " " + ddlYear.SelectedValue + " exist, cannot save data")
                    Exit Sub
                End If
            End If
            Dim Msg As String
            Msg = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_GLFAProcessCekDupp " + ddlYear.SelectedValue + ", " + ddlPeriod.SelectedValue + ", @A Out ", ViewState("DBConnection").ToString)
            If Msg.Length > 5 Then
                lbStatus.Text = Msg
                Exit Sub
                'End If
                'DataDt = SQLExecuteQuery("EXEC S_GLFAProcessCekDupp " + ddlYear.SelectedValue + ", " + ddlPeriod.SelectedValue, ViewState("DBConnection")).Tables(0)
                'If DataDt.Rows.Count = 0 Then
                ' lbStatus.Text = MessageDlg("No Data Exist")
                ' Exit Sub
            Else

                JEDS = SQLExecuteQuery("EXEC S_GLFAProcessFormula '" + ddlYear.SelectedValue + "', '" + ddlPeriod.SelectedValue + "'", ViewState("DBConnection").ToString)
                For Each DR In JEDS.Tables(0).Rows
                    NewRow = ViewState("Dt").NewRow
                    NewRow("FixedAsset") = DR("FACode")
                    NewRow("FixedAssetName") = DR("FAName")
                    NewRow("BalanceAmount") = DR("BalanceAmount")
                    NewRow("BalanceLife") = DR("BalanceLife")
                    NewRow("AdjustDepr") = 0
                    NewRow("AmountDepr") = DR("Amount")
                    NewRow("TotalDepr") = DR("Amount")
                    NewRow("Specification") = DR("Specification")
                    'NewRow("CreditHome") = 0
                    ViewState("Dt").Rows.Add(NewRow)
                Next
            End If
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Taon As String, ByVal bulan As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Year = " + Taon + " AND Period = " + bulan, ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Taon
            ddlYear.SelectedValue = Taon
            ddlPeriod.SelectedValue = bulan
            BindToDropList(ddlYear, Dt.Rows(0)("Year").ToString)
            BindToDropList(ddlPeriod, Dt.Rows(0)("Period").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FixedAsset = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbFACode, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FixedAssetName").ToString)
                BindToText(tbProcess, Dr(0)("AmountDepr").ToString)
                BindToText(tbBalanceAmount, Dr(0)("BalanceAmount").ToString)
                BindToText(tbTotal, Dr(0)("TotalDepr").ToString)
                BindToText(tbAdjust, Dr(0)("AdjustDepr").ToString)
                BindToText(tbLife, Dr(0)("BalanceLife").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
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

    Protected Sub tbAdjust_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAdjust.TextChanged
        tbTotal.Text = Val(tbProcess.Text.Replace(",", "")) + Val(tbAdjust.Text.Replace(",", ""))
        tbTotal.Text = FormatNumber(tbTotal.Text, ViewState("DigitHome"))
        tbAdjust.Text = FormatNumber(tbAdjust.Text, ViewState("DigitHome"))
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        tbRef.Text = ddlYear.SelectedValue
    End Sub
End Class
