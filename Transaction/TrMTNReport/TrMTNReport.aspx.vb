Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrMTNReport_TrMTNReport
    Inherits System.Web.UI.Page
    'TransNmbr, TransDate, PettyType, PettyName, Currency, ForexRate, TotalForex, PayTo, Remark
    'ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, CostCtrName, AmountForex, Remark
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select Distinct TransNmbr, TransDate, Status, ReportDate, Remark  From V_MTNReportHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                ddlRow.SelectedValue = "20"
            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnJob" Then
                    tbJob.Text = Session("Result")(0).ToString
                    tbJobName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnWONO" Then
                    'TransNmbr, ItemNo, Job, JobName, MaintenanceItem, MaintenanceItemName, Problem, PIC
                    tbWONO.Text = Session("Result")(0).ToString
                    lbItemNo.Text = Session("Result")(1).ToString
                    tbJob.Text = Session("Result")(2).ToString
                    tbJobName.Text = Session("Result")(3).ToString
                    tbMtnItem.Text = Session("Result")(4).ToString
                    tbMtnItemName.Text = Session("Result")(5).ToString
                    tbProblem.Text = Session("Result")(6).ToString
                    tbPIC.Text = Session("Result")(7).ToString
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("WONo+'|'+ItemNo = " + QuotedStr(drResult("TransNmbr") + "|" + drResult("ItemNo")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("WONo") = drResult("TransNmbr")
                            dr("ItemNo") = drResult("ItemNo")
                            dr("Job") = drResult("Job")
                            dr("JobName") = drResult("JobName")
                            dr("MTNItem") = drResult("MaintenanceItem")
                            dr("MTNItemName") = drResult("MaintenanceItemName")
                            dr("Problem") = drResult("Problem")
                            dr("PIC") = drResult("PIC")
                            dr("FgMTNItemWorkNormal") = "Y"
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
        Try
            BtnGo.Visible = False
            FillRange(ddlRange)
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            'tbAmountForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")

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
        Return "SELECT * From V_MTNReportDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
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
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_MTNReportForm " + Result + ""
                Session("ReportFile") = ".../../../Rpt/FormMTNReport.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_MTNReport", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlMTNItem.Enabled = State
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
            tbReportDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbRemark.Text = ""
            Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbJob.Text = ""
            tbJobName.Text = ""
            tbWONO.Text = ""
            tbMtnItem.Text = ""
            tbMtnItemName.Text = ""
            lbItemNo.Text = "0"
            tbProblem.Text = ""
            tbCause.Text = ""
            tbRecommendation.Text = ""
            tbPIC.Text = ""
            ddlFgNormal.SelectedValue = "Y"
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
                If Dr("WONO").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("WO No Must Have Value")
                    Return False
                End If
                If Dr("ItemNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item No Must Have Value")
                    Return False
                End If
                If Dr("PIC").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("PIC Must Have Value")
                    Return False
                End If
            Else
                If tbWONO.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("WO NO Must Have Value")
                    btnWONO.Focus()
                    Return False
                End If
                If lbItemNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item No Must Have Value")
                    btnWONO.Focus()
                    Return False
                End If
                If tbPIC.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("PIC Must Have Value")
                    tbPIC.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Friend Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbReportDate, Dt.Rows(0)("ReportDate").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WONo+'|'+ItemNo = " + QuotedStr(ItemNo))

            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo
                BindToText(tbWONO, Dr(0)("WONo").ToString)
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbJob, Dr(0)("Job").ToString)
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbMtnItem, Dr(0)("MTNItem").ToString)
                BindToText(tbMtnItemName, Dr(0)("MTNItemName").ToString)
                BindToDropList(ddlFgNormal, Dr(0)("FgMTNItemWorkNormal").ToString)
                BindToText(tbProblem, Dr(0)("Problem").ToString)
                BindToText(tbCause, Dr(0)("Cause").ToString)
                BindToText(tbRecommendation, Dr(0)("Recommendation").ToString)
                BindToText(tbPIC, Dr(0)("PIC").ToString)
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
            'ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, CostCtrName, AmountForex, Remark
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                
                If ViewState("DtValue") <> tbWONO.Text + "|" + lbItemNo.Text Then
                    If CekExistData(ViewState("Dt"), "WONO,ItemNo", tbWONO.Text + "|" + lbItemNo.Text) Then
                        lbStatus.Text = "WO No '" + tbWONO.Text + "' Item No '" + lbItemNo.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("WONO+'|'+ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                'Row = ViewState("Dt").Select("WONO = " + QuotedStr(tbWONO.Text))(0)
                Row("WONo") = tbWONO.Text
                Row("ItemNo") = CInt(lbItemNo.Text)
                Row("Job") = tbJob.Text
                Row("JobName") = tbJobName.Text
                Row("MTNItem") = tbMtnItem.Text
                Row("MTNItemName") = tbMtnItemName.Text
                Row("PIC") = tbPIC.Text
                Row("Problem") = tbProblem.Text
                Row("Recommendation") = tbRecommendation.Text
                Row("Cause") = tbCause.Text
                Row("FgMTNItemWorkNormal") = ddlFgNormal.SelectedValue

                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "WONo,ItemNo", tbWONO.Text + "|" + lbItemNo.Text) Then
                    lbStatus.Text = "WO No '" + tbWONO.Text + "' Item No '" + lbItemNo.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WONo") = tbWONO.Text
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("Job") = tbJob.Text
                dr("JobName") = tbJobName.Text
                dr("MTNItem") = tbMtnItem.Text
                dr("MTNItemName") = tbMtnItemName.Text
                dr("PIC") = tbPIC.Text
                dr("Problem") = tbProblem.Text
                dr("Recommendation") = tbRecommendation.Text
                dr("Cause") = tbCause.Text
                dr("FgMTNItemWorkNormal") = ddlFgNormal.SelectedValue
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            'countDt()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    'Private Sub countDt()
    '    Dim dr As DataRow
    '    Dim hasil As Double
    '    Try
    '        hasil = 0

    '        For Each dr In ViewState("Dt").Rows
    '            If Not dr.RowState = DataRowState.Deleted Then
    '                hasil = hasil + CFloat(dr("AmountForex").ToString)
    '            End If
    '        Next
    '        tbTotalForex.Text = FormatNumber(hasil, ViewState("DigitCurr"))
    '    Catch ex As Exception
    '        Throw New Exception("Count Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

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
                tbCode.Text = GetAutoNmbr("LHM", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO MTNReportHd (TransNmbr, transDate, STATUS, ReportDate, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(Format(tbReportDate.SelectedValue, "yyyy-MM-dd")) + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MTNReportHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNReportHd SET ReportDate=" + QuotedStr(Format(tbReportDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate(), TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, WONo, MTNItem, Job, JobName, Problem, Cause, Recommendation, FgMTNItemWorkNormal, PIC FROM MTNReportDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE MTNReportDt SET WONo = @WONo, MTNItem = @MTNItem, Job = @Job, JobName = @JobName, Problem = @Problem, Cause = @Cause, Recommendation = @Recommendation, FgMTNItemWorkNormal = @FgMTNItemWorkNormal, PIC = @PIC WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND WONo = @WONo AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            Update_Command.Parameters.Add("@MTNItem", SqlDbType.VarChar, 10, "MTNItem")
            Update_Command.Parameters.Add("@Job", SqlDbType.VarChar, 5, "Job")
            Update_Command.Parameters.Add("@JobName", SqlDbType.VarChar, 60, "JobName")
            Update_Command.Parameters.Add("@Problem", SqlDbType.VarChar, 255, "Problem")
            Update_Command.Parameters.Add("@Cause", SqlDbType.VarChar, 255, "Cause")
            Update_Command.Parameters.Add("@Recommendation", SqlDbType.VarChar, 255, "Recommendation")
            Update_Command.Parameters.Add("@FgMTNItemWorkNormal", SqlDbType.VarChar, 1, "FgMTNItemWorkNormal")
            Update_Command.Parameters.Add("@PIC", SqlDbType.VarChar, 60, "PIC")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldWONo", SqlDbType.VarChar, 20, "WONo")
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MTNReportDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'  AND WONo = @WONo AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MTNReportDt")

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
                lbStatus.Text = MessageDlg("Detail Work Order must have at least 1 record")
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
            tbWONO.Focus()
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
            'TransNmbr, TransDate, PettyType, PettyName, Currency, ForexRate, TotalForex, PayTo, Remark
            FilterName = "Petty No, Petty Date, Status,  Petty Type, Petty Name, Currency, Forex Rate, Total Forex, Pay To, Report, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, PettyType, PettyName, Currency, ForexRate, TotalForex, PayTo, FgReport, Remark"
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
                        Session("SelectCommand") = "EXEC S_MTNReportForm '''" + GVR.Cells(2).Text + "'''"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormMTNReport.frx"
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

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("WONo = " + QuotedStr(GVR.Cells(1).Text) + " AND ItemNo = " + GVR.Cells(2).Text.Trim)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            'countDt()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            'lb = GVR.FindControl("lbLocation")
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text ' + "|" + TrimStr(lb.Text)
            
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnWONO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWONO.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr, TransDate, ItemNo, MaintenanceItem, MaintenanceItemName, Job, JobName, Problem, PIC, Remark  FROM V_MTNReportForWO"
            ResultField = "TransNmbr, ItemNo, Job, JobName, MaintenanceItem, MaintenanceItemName, Problem, PIC"
            ViewState("Sender") = "btnWONO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Account Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            Session("Result") = Nothing
            CriteriaField = "TransNmbr, TransDate, ItemNo, MaintenanceItem, MaintenanceItemName, Job, JobName, Problem, PIC, Remark"
            Session("Filter") = "SELECT TransNmbr, TransDate, ItemNo, MaintenanceItem, MaintenanceItemName, Job, JobName, Problem, PIC, Remark  FROM V_MTNReportForWO"
            ResultField = "TransNmbr, ItemNo, Job, JobName, MaintenanceItem, MaintenanceItemName, Problem, PIC"
            Session("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "GridDt_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            GridView1.PageSize = ddlRow.SelectedValue
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
