Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPLSensusPokokTBM
    Inherits System.Web.UI.Page


    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLSensusPokokTBMHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbOk.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbLuasAdjust.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbSDHI.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbLastSensus.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbLsensus.Attributes.Add("OnKeyDown", "return PressNumeric();")

                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnBatch" Then
                    ddlBlock.SelectedValue = FormatFloat(Session("Result")(0).ToString, ViewState("DigitQty"))
                    tbLastSensus.Text = Session("Result")(1).ToString
                    tbOk.Text = "0"
                    tbSaldoCap.Text = Session("Result")(3).ToString
                    tbAdjust.Text = "0"

                    tbOk.Text = "0"
                    tbLSensus.Text = "0"
                    tbSDHI.Text = "0"
                    tbLuasAdjust.Text = "0"

                    tbSaldoCap.Text = CInt(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text))
                    tbAdjust.Text = CInt(CFloat(tbOk.Text) - CFloat(tbLastSensus.Text))
                    tbLuasAdjust.Text = CInt(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text))

                    FillCombo(ddlBlock, "SELECT DISTINCT Block, BlockName FROM V_PLSensusPokokTBMDt WHERE TransNmbr =  " + QuotedStr(Session("Result")(0).ToString), True, "Block", "BlockName", ViewState("DBConnection"))
                    BindToDropList(ddlBlock, Session("Result")(3))

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
        FillCombo(ddlSensus, "EXEC S_GetTeam", True, "Team_Code", "Team_Name", ViewState("DBConnection"))
        FillCombo(ddlBlock, "SELECT * FROM V_PLSensusPokokTBMGetBlock", True, "Block", "BlockName", ViewState("DBConnection"))
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
        lblTitle.Text = "Seleksi Pokok PerBlock"
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
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
            SQLString = GetStringHd
            DT = BindDataTransaction(SQLString, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT TransNmbr, Block, QtyMax, QtyOK,QtySaldo, QtyAdjust,LuasLastSensus, LuasTanam, LuasAdjust, BlockName, QtyLastSensus,  QtyReject, Remark From V_PLSensusPokokTBMDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtJS(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, Block, QtyMax, QtyOK,QtySaldo, QtyAdjust,LuasLastSensus, LuasTanam, LuasAdjust, BlockName, QtyLastSensus, QtyReject, Remark From V_PLSensusPokokTBMDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
                Session("SelectCommand") = "EXEC S_PLSensusPokokTBM " + Result + " , " + QuotedStr(ViewState("UserId").ToString)
                lbStatus.Text = Session("SelectCommand")
                Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSensusPokokTBM.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSensusPokokTBM", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'BtnGetDt.Enabled = State
            'tbSensus.Enabled = State
            ddlSensus.Enabled = State
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If ViewState("StateDt") = "Edit" Then
                ''lbStatus.Text = ViewState("DtValue") + "11111"
                ''Exit Sub
                Dim Row As DataRow
                Row = ViewState("Dt").Select("Block = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("Block") = ddlBlock.SelectedValue
                Row("QtyMax") = tbLastSensus.Text
                Row("QtyOK") = tbOk.Text
                Row("QtySaldo") = tbSaldoCap.Text
                Row("QtyAdjust") = tbAdjust.Text
                Row("LuasLastSensus") = tbLsensus.Text
                Row("LuasTanam") = tbSDHI.Text
                Row("LuasAdjust") = tbLuasAdjust.Text
                Row("Remark") = tbRemarkDt.Text
                Row("QtyLastSensus") = "0"
                Row("QtyReject") = "0"
            Else
                If CekDt() = False Then
                    Exit Sub
                End If
                'Insert()
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Block") = ddlBlock.SelectedValue
                dr("QtyMax") = tbLastSensus.Text
                dr("QtyOK") = tbOk.Text
                dr("QtySaldo") = tbSaldoCap.Text
                dr("QtyAdjust") = tbAdjust.Text
                dr("LuasLastSensus") = tbLsensus.Text
                dr("LuasTanam") = tbSDHI.Text
                dr("LuasAdjust") = tbLuasAdjust.Text
                dr("Remark") = tbRemarkDt.Text
                dr("QtyLastSensus") = "0"
                dr("QtyReject") = "0"
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
                tbRef.Text = GetAutoNmbr("SPB", "Y", Year(tbDate.SelectedDate), Month(tbDate.SelectedDate), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLSensusPokokTBMHD (TransNmbr, RotasiNo, Status, TransDate, Remark, UserPrep, DatePrep, SensusBy) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + QuotedStr(tbRotasi.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ddlSensus.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSensusPokokTBMHD WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLSensusPokokTBMHD SET RotasiNo = " + QuotedStr(tbRotasi.Text) + ", SensusBy = " + QuotedStr(ddlSensus.SelectedValue) + " , Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Block, QtyMax, QtyLastSensus, QtyOK, QtyReject, Remark, QtyAdjust, LuasLastSensus, LuasTanam, LuasAdjust From PLSensusPokokTBMDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PLSensusPokokTBMDt")

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
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            btnHome.Visible = False
            ddlSensus.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ' ViewState("TransNmbr") = 0
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
            tbRemark.Text = ""
            ddlSensus.SelectedValue = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlBlock.SelectedValue = ""
            'tbBatchDate.SelectedDate = ViewState("ServerDate") 'Today
            tbLastSensus.Text = "0"
            tbOk.Text = "0"
            tbSaldoCap.Text = "0"
            tbAdjust.Text = "0"
            tbLSensus.Text = "0"
            tbSDHI.Text = "0"
            tbLuasAdjust.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
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

    'Protected Sub btnBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatch.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = " SELECT Block, BlockName, Area, MaxCap, Qty, QtySisip  FROM V_PLSensusPokokTBMGetBlock"
    '        ResultField = "Block, BlockName, Area, MaxCap, Qty"
    '        ViewState("Sender") = "btnBatch"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Acc Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click

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

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            'Throw New Exception("CekHd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                'If Dr("Block").ToString.Trim = "0" Then
                '    lbStatus.Text = MessageDlg("Block No Must Have Value")
                '    Return False
                'End If
                'If (Dr("Rotasi").ToString = "") Then
                '    lbStatus.Text = MessageDlg("Rotasi " + Dr("Rotasi").ToString + " must have value Rotasi")
                '    Return False
                'End If

            Else
                If (ddlBlock.SelectedValue.Trim = "") Then
                    lbStatus.Text = MessageDlg("Block must have value")
                    Return False

                End If

                If tbLastSensus.Text.Trim < "0" Then
                    lbStatus.Text = MessageDlg("Last Sensus Must Have Value")
                    tbLastSensus.Focus()
                    Return False
                End If
                
                'If (tbOk.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Ok must have value")
                '    tbOk.Focus()
                '    Return False
                'End If

                'If (tbAdjust.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Double Tone must have value")
                '    tbAdjust.Focus()
                '    Return False
                'End If
                'If (tbLsensus.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Luas Sensus Tone must have value")
                '    tbLsensus.Focus()
                '    Return False
                'End If

                'If (tbSDHI.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("SDHI must have value")
                '    tbSDHI.Focus()
                '    Return False
                'End If
                'If (tbLuasAdjust.Text.Trim) < "0" Then
                '    lbStatus.Text = MessageDlg("Luas Adjust must have value")
                '    tbLuasAdjust.Focus()
                '    Return False
                'End If

                'If (tbRemarkDt.Text.Trim) = "" Then
                '    lbStatus.Text = MessageDlg("Remark must have value")
                '    tbRemarkDt.Focus()
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
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridView1.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = " RotasiNo,Date, SensusBy, Remark"
            FilterValue = "RotasiNo, TransNmbr, Status, Format_Name, Remark"
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
                        Session("SelectCommand") = "EXEC S_PLFormSensusPokokTBM " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormSensusPokokTBM.frx"
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
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Block = " + QuotedStr(ddlBlock.SelectedValue) + GVR.Cells(0).Text)
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            pnlDt.Visible = False
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = GVR.Cells(1).Text
            ddlBlock.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlBlock_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBlock.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT QtyMax, QtyOk, QtySaldo, QtyAdjust, LuasLastSensus, LuasTanam, LuasAdjust FROM V_PLSensusPokokTBMDt WHERE Block = " + QuotedStr(ddlBlock.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                'tbLastSensus.Text = FormatFloat(Dr("QtySaldo").ToString, ViewState("DigitQty"))
                tbLastSensus.Text = CInt(Dr("QtySaldo").ToString)
                tbSaldoCap.Text = CInt(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text))
                tbAdjust.Text = CInt(CFloat(tbOk.Text) - CFloat(tbLastSensus.Text))
                tbLuasAdjust.Text = CInt(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text))
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlBlock_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
    Dim CrHome As Decimal = 0
    Dim DbHome As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Dim Type As String
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRotasi, Dt.Rows(0)("RotasiNo").ToString)
            BindToDropList(ddlSensus, Dt.Rows(0)("SensusBy").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            Type = SQLExecuteScalar("Select BlockName From V_PLSensusPokokTBMGetBlock WHERE Block = " + QuotedStr(ddlBlock.SelectedValue), ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Block As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Block = " + QuotedStr(Block))
            If Dr.Length > 0 Then

                BindToDropList(ddlBlock, Dr(0)("Block").ToString)
                BindToText(tbLastSensus, Dr(0)("QtyMax").ToString)
                BindToText(tbOk, Dr(0)("QtyOK").ToString)
                BindToText(tbSaldoCap, Dr(0)("QtySaldo").ToString)
                BindToText(tbAdjust, Dr(0)("QtyAdjust").ToString)
                BindToText(tbLsensus, Dr(0)("LuasLastSensus").ToString)
                BindToText(tbSDHI, Dr(0)("LuasTanam").ToString)
                BindToText(tbLuasAdjust, Dr(0)("LuasAdjust").ToString)
                'BindToText(tbRotasi, Dr(0)("RotasiNo").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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

    Protected Sub tbOk_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbOk.TextChanged, tbLastSensus.TextChanged, tbLuasAdjust.TextChanged, tbAdjust.TextChanged, tbSaldoCap.TextChanged, tbSDHI.TextChanged
        Try

            tbAdjust.Text = CInt(CFloat(tbOk.Text) - CFloat(tbLastSensus.Text))
            tbSaldoCap.Text = FormatFloat(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text), ViewState("DigitQty"))
            'tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbOk_TextChanged Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbLSensus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLSensus.TextChanged, tbLastSensus.TextChanged, tbLuasAdjust.TextChanged, tbAdjust.TextChanged, tbSaldoCap.TextChanged
        Try
            tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLsensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbLSensus_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSDHI_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSDHI.TextChanged, tbLastSensus.TextChanged, tbLuasAdjust.TextChanged, tbAdjust.TextChanged, tbSaldoCap.TextChanged
        Try
            tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLsensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbOk_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub
End Class
