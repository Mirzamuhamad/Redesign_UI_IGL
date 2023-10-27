Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSensusPokokMN
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PLSensusPokokMNHd"

    'Protected GetStringDt2 As String = "SELECT * From PLSensusPokokMNDt2"

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLSensusPokokMNDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function


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
            'hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnModule" Then
                    tbModule.Text = Session("Result")(0).ToString
                    BindToText(tbModuleName, Session("Result")(1).ToString)
                    BindToText(tbQtyMax, Session("Result")(2).ToString)
                    tbQtyModule.Text = "0"
                    tbQtyOk.Text = "0"
                    tbQtyReject.Text = "0"
                    tbQtyRepair.Text = "0"
                End If


                If ViewState("Sender") = "btnBatch" Then
                    tbBatchNo.Text = Session("Result")(0).ToString
                    BindToText(tbBatchName, Session("Result")(1).ToString)
                    'BindToText(tbQtyModuleDt, Session("Result")(2).ToString)
                    tbQtyModuleDt.Text = "0"
                    tbQtyOkDt.Text = "0"
                    tbQtyRejectDt.Text = "0"
                    tbQtyRepairDt.Text = "0"
                End If

                If ViewState("Sender") = "btnModuleDt" Then
                    tbModuleDt.Text = Session("Result")(0).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
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

        'FillCombo(ddlDivision, "EXEC S_GetTeamNew", True, "Division", "DivisionName", ViewState("DBConnection"))
        'FillCombo(ddlStartWeek, "EXEC S_GetWeek", False, "Week_No", "Week", ViewState("DBConnection"))
        'FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
        FillCombo(ddlSensusBy, "EXEC S_GetTeam", True, "Team_Code", "Team_Name", ViewState("DBConnection"))

        'FillCombo(tbBatch, "SELECT * FROM V_PLTransferModuleGetBatch", True, "Batch_No", "BatchName", ViewState("DBConnection"))
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        lbStatus.Text = "Batch Block Adjust File"
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
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            Drow = ViewState("Dt2").Select("Module=" + QuotedStr(ViewState("Module")))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, Module, ModuleName, QtyMax, QtyModule, QtyOK, QtyReject, QtyRepair, Remark From V_PLSensusPokokMNDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtJS(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, Module, ModuleName, QtyMax, QtyModule, QtyOK, QtyReject, QtyRepair, Remark From V_PLSensusPokokMNDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_PLFormSensusPokokMN " + Result + " , " + QuotedStr(ViewState("UserId").ToString)
                lbStatus.Text = Session("SelectCommand")
                Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSensusPokokMN.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSensusPokokMN", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Or (ViewState("StateHd") = "View") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbRef.Text = GetAutoNmbr("SPM", "Y", Year(tbDate.SelectedDate), Month(tbDate.SelectedDate), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = tbRef.Text
                'Exit Sub

                SQLString = "INSERT INTO PLSensusPokokMNHd (TransNmbr, Transdate, Status, SensusBy, Remark, UserPrep, DatePrep, UserAppr, DateAppr) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(ddlSensusBy.SelectedValue) + ", " + _
                QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSensusPokokMNHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLSensusPokokMNHd SET SensusBy = " + QuotedStr(ddlSensusBy.SelectedValue) + _
                 ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", DateAppr = getDate()" + _
                " , DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Module, QtyMax, QtyModule, QtyOK, QtyReject, QtyRepair, Remark  FROM PLSensusPokokMNDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLSensusPokokMNDt SET Module = @Module, QtyMax = @QtyMax, QtyModule= @QtyModule, QtyOK = @QtyOK, QtyReject= @QtyReject, QtyRepair= @QtyRepair, Remark= @Remark " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Module = @OldModule", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Module", SqlDbType.VarChar, 5, "Module")
            Update_Command.Parameters.Add("@QtyMax", SqlDbType.Float, 9, "QtyMax")
            Update_Command.Parameters.Add("@QtyModule", SqlDbType.Float, 9, "QtyModule")
            Update_Command.Parameters.Add("@QtyOK", SqlDbType.Float, 9, "QtyOk")
            Update_Command.Parameters.Add("@QtyReject", SqlDbType.Float, 9, "QtyReject")
            Update_Command.Parameters.Add("@QtyRepair", SqlDbType.Float, 9, "QtyRepair")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldModule", SqlDbType.VarChar, 5, "Module")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLSensusPokokMNDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Module = @Module", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Module", SqlDbType.VarChar, 5, "Module")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLSensusPokokMNDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt


            'updatePrimary Key
            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next
            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, BatchNo, Module, QtyModule, QtyOK, QtyReject, QtyRepair, Remark FROM PLSensusPokokMNDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PLSensusPokokMNDt2")

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            ' lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            ' Exit Sub
            ' End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "Module") = False Then
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
            btnHome.Visible = False
            tbDate.Focus()
            EnableHd(True)
            pnlDt2.Visible = False
            btnAddDt3.Visible = False
            btnAddDt4.Visible = False
            'btnAddDt.Visible = False
            'btnAddDt2.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbBatchNo.Text = ""
            tbModuleDt.Text = ""
            tbQtyModuleDt.Text = ""
            tbQtyOkDt.Text = ""
            tbQtyRejectDt.Text = ""
            tbQtyRepairDt.Text = ""
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt3.Click, btnAddDt4.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbBatchNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DtRemark") = ""
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    'Private Sub VisibleDt()
    '    Try
    '        tbModule.Enabled = False
    '        tbModuleName.Enabled = False
    '        'tbRotation.Enabled = False
    '        tbStartDate.Enabled = False
    '        tbEndDate.Enabled = False
    '        tbQty.Enabled = False
    '        tbUnit.Enabled = False
    '        tbWorkDay.Enabled = False
    '        tbCapacity.Enabled = False
    '    Catch ex As Exception
    '        Throw New Exception("new Record Error " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub ClearHd()
        'Dim SQLString As String
        Try
            tbRef.Text = ""
            tbModule.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbModule.Text = ""
            tbQtyMax.Text = ""
            tbQtyModule.Text = ""
            tbQtyOk.Text = ""
            tbQtyReject.Text = ""
            tbQtyRepair.Text = ""
            tbRemark.Text = ""

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
                If CekDt(dr, "CheckBy") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()

        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
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
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If

            'If tbWoNo.Text = "" Then
            '    lbStatus.Text = MessageDlg("Wo no must have value")
            '    ddlWeek.Focus()
            '    Return False
            'End If
            If ddlSensusBy.Text = "" Then
                lbStatus.Text = MessageDlg("Sensus must have value")
                ddlSensusBy.Focus()
                Return False
            End If
            'If CFloat(tbQtyTanam.Text) < 0 Then
            '    lbStatus.Text = MessageDlg("Qty Tanam must have value")
            '    tbQtyTanam.Focus()
            '    Return False
            'End If

            If Len(tbRemark.Text.Trim) > 60 Then
                lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
                tbRemark.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            'If Not (Dr Is Nothing) Then
            '    If Dr.RowState = DataRowState.Deleted Then
            '        Return True
            '    End If
            '    If Dr("Block").ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("TK Panen Must Have Value")
            '        Return False
            '    End If

            '    If Dr("Team").ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Propose Week must have value")
            '        ddlTeam.Focus()
            '        Return False
            '    End If
            '    'If Dr("Remark").ToString.Trim = "" Then
            '    '    lbStatus.Text = MessageDlg("Remark Must Have Value")
            '    '    Return False
            '    'End If
            'Else
            '    If tbCode.Text.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Block Must Have Value")
            '        tbCode.Focus()
            '        Return False
            '    End If


            'If CFloat(tbGawang.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Gawang Must Have Value")
            '    tbGawang.Focus()
            '    Return False
            'End If

            'If tbRemarkDt.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Remark Must Have Value")
            '    tbRemarkDt.Focus()
            '    Return False
            'End If
            'End If
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
            FDateValue = "TransNmbr"
            FilterName = "TransNmbr, Module, QtyMax, QtyModule, QtyOK, QtyReject, QtyRepair, Remark"
            FilterValue = "TransNmbr, Module, QtyMax, QtyModule, QtyOK, QtyReject, QtyRepair, Remark"
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
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnAddDt3.Visible = False
                    btnAddDt4.Visible = False
                    pnlDt2.Visible = False
                    'GridDt2.Columns(0).Visible = False
                    'pnlDt2.Visible = True
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
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnAddDt3.Visible = True
                        btnAddDt4.Visible = True
                        pnlDt2.Visible = False
                        'btnAddDt2.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PLFormSensusPokokMN " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormSensusPokokMN.frx"
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
    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Session("SelectCommand") = Nothing
        'Session("ReportFile") = Nothing
        'WebReport1.Dispose()
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


            'If e.CommandName = "ViewA" Then
            '    MovePanel(pnlDt2, pnlEditDt2)
            '    GridDt.Visible = False
            '    pnlEditDt.Visible = False
            '    pnlInput.Visible = true

            'End If
            Dim GVR As GridViewRow
            If e.CommandName = "ViewA" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                '  lbWODt2.Text = GVR.Cells(2).Text
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2")
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
            ElseIf e.CommandName = "ViewE" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2")
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
            ElseIf e.CommandName = "ViewM" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                ViewState("Module") = GVR.Cells(2).Text
                 Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))

                Else
                    drow = ViewState("Dt2").Select("Module = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        pnlDt2.Visible = True
                        btnAddDt3.Visible = True
                        btnAddDt4.Visible = True
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                        pnlDt2.Visible = True
                        btnAddDt3.Visible = True
                        btnAddDt4.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Module")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        dr = ViewState("Dt").Select("Module = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            'Dim dr() As DataRow
            'Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)

            'dr = ViewState("Dt2").Select("BatchNo = " + QuotedStr(GVR.Cells(1).Text))
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt2")) = 0)

            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("BatchNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            dr = ViewState("Dt2").Select("BatchNo = " + QuotedStr(GVR.Cells(1).Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = tbModule.Text
            tbModule.Focus()
            StatusButtonSave(False)

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlSensusBy, Dt.Rows(0)("SensusBy").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)


            'FillCombo(ddlBlok , "EXEC S_GetCostCtrDept " + QuotedStr(ddlTPH.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Module = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbModule, Dr(0)("Module").ToString)
                BindToText(tbQtyMax, Dr(0)("QtyMax").ToString)
                BindToText(tbQtyModule, Dr(0)("QtyModule").ToString)
                BindToText(tbQtyOk, Dr(0)("QtyOK").ToString)
                BindToText(tbQtyReject, Dr(0)("QtyReject").ToString)
                BindToText(tbQtyRepair, Dr(0)("QtyRepair").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'VisibleDt()
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("BatchNo = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbBatchNo, Dr(0)("BatchNo").ToString)
                'BindToText(tbBatchName, Dr(0)("BatchName").ToString)
                BindToText(tbQtyModuleDt, Dr(0)("QtyModule").ToString)
                BindToText(tbQtyOkDt, Dr(0)("QtyOk").ToString)
                BindToText(tbQtyRejectDt, Dr(0)("QtyReject").ToString)
                BindToText(tbQtyRepairDt, Dr(0)("QtyRepair").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If CekDt() = False Then
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbModule.Text Then
                    If CekExistData(ViewState("Dt"), "Module ", tbModule.Text) Then
                        lbStatus.Text = "Module '" + tbModuleName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Module = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("Module") = tbModule.Text
                Row("QtyMax") = tbQtyMax.Text
                Row("QtyModule") = tbQtyModule.Text
                Row("QtyOk") = tbQtyOk.Text
                Row("QtyReject") = tbQtyReject.Text
                Row("QtyRepair") = tbQtyRepair.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Module", tbModule.Text) Then
                    lbStatus.Text = "Module '" + tbModuleName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Module") = tbModule.Text
                dr("QtyMax") = tbQtyMax.Text
                dr("QtyModule") = tbQtyModule.Text
                dr("QtyOk") = tbQtyOk.Text
                dr("QtyReject") = tbQtyReject.Text
                dr("QtyRepair") = tbQtyRepair.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            'ViewState("DtRemark") = tbRemarkDt.Text
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            GridDt.Columns(0).Visible = True
            GridDt.Columns(1).Visible = True
            StatusButtonSave(True)

        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Dim Row As DataRow
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then

                Row = ViewState("Dt2").Select("BatchNo = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("BatchNo") = tbBatchNo.Text
                Row("Module") = tbModuleDt.Text
                Row("QtyModule") = tbQtyModuleDt.Text
                Row("QtyOk") = tbQtyOkDt.Text
                Row("QtyReject") = tbQtyRejectDt.Text
                Row("QtyRepair") = tbQtyRepairDt.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("BatchNo") = tbBatchNo.Text
                dr("Module") = tbModuleDt.Text
                dr("QtyModule") = tbQtyModuleDt.Text
                dr("QtyOk") = tbQtyOkDt.Text
                dr("QtyReject") = tbQtyRejectDt.Text
                dr("QtyRepair") = tbQtyRepairDt.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok=" + QuotedStr(lbWODt2.Text))
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                'If Dr("Warehouse").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Warehouse Must Have Value")
                '    Return False
                'End If
                'If Dr("Subled").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Subled Must Have Value")
                '    Return False
                'End If

            Else
                'If ddlStatusTanam.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Warehouse must have value")
                '    ddlStatusTanam.Focus()
                '    Return False
                'End If
                'If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                '    lbStatus.Text = MessageDlg("SubLed must have value")
                '    tbSubLed.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function
    Private Function DtExist() As Boolean
        Dim dete, piar As Boolean
        Try
            If ViewState("Dt") Is Nothing Then
                dete = False
            Else
                dete = GetCountRecord(ViewState("Dt")) > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtPR").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt2")) = 0
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnModule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModule.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT Module_Code, Module_Name, MaxCap From V_MsModule " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "Module_Code, Module_Name, MaxCap"
            ResultField = "Module_Code, Module_Name, MaxCap"

            ViewState("Sender") = "btnModule"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBatch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBatch.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT BatchNo, Varietas, QtyBatch From MsBatch " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "BatchNo, Varietas, QtyBatch"
            ResultField = "BatchNo, Varietas, QtyBatch"

            ViewState("Sender") = "btnBatch"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnModuleDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModuleDt.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT Module, ModuleName From V_PLSensusPokokMNDt " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "Module, ModuleName"
            ResultField = "Module, ModuleName"

            ViewState("Sender") = "btnModuleDt"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyRejectDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyRejectDt.TextChanged
        Try

            tbQtyReject.Text = CFloat(tbQtyRejectDt.Text)
        Catch ex As Exception
            Throw New Exception("tbQtyReject Error : " + e.ToString)
        End Try
    End Sub

    'Protected Sub ddlStartWeek_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartWeek.SelectedIndexChanged
    '    Try

    '        tbEndWeek.Text = CFloat(ddlStartWeek.Text) + CFloat("1")
    '    Catch ex As Exception
    '        Throw New Exception("ddlWeek Error : " + ex.ToString)
    '    End Try
    'End Sub

End Class
