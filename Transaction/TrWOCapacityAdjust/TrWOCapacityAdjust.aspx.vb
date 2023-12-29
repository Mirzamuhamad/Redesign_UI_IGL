Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrWOCapacityAdjust
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLWOPlanHd WHERE Status = 'P'"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                BtnAdd.Visible = False
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            'hasil dari search dialog
            'If Not Session("Result") Is Nothing Then
            '    If ViewState("Sender") = "btnBlock" Then
            '        tbCode.Text = Session("Result")(0).ToString
            '        BindToText(tbName, Session("Result")(1).ToString)
            '    End If

            '    If ViewState("Sender") = "btnWoNo" Then
            '        tbWoNo.Text = Session("Result")(0).ToString
            '        'BindToText(tbName, Session("Result")(1).ToString)
            '    End If
            '    Session("Result") = Nothing
            '    ViewState("Sender") = Nothing
            'End If

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
        FillCombo(ddlWorkBy, "EXEC S_GetTeamNew", True, "Team_Code", "Team_Name", ViewState("DBConnection"))
        FillCombo(ddlDivisi, "EXEC S_GetDIvisi", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr,Type, DivisiBlok, DivisiBlokName, StatusTanam, Percentage, QtyTotal, QtyCancel, NormaHK, NormaStd, WorkDay, TargetHK, StartDate, EndDate From V_PLWOPlanDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtJS(ByVal Nmbr As String) As String
        Return "TransNmbr,Type, DivisiBlok, DivisiBlokName, StatusTanam, Percentage, QtyTotal, QtyCancel, NormaHK, NormaStd, WorkDay, TargetHK, StartDate, EndDate From V_PLWOPlanDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            btnAdd2.Visible = False
            BtnAdd.Visible = False
            ddlCommand.Visible = False
            ddlCommand2.Visible = False
            BtnGo.Visible = False
            btnGo2.Visible = False
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
                Session("SelectCommand") = "EXEC S_FormTrBatchBlockAdjust " + Result + " , " + QuotedStr(ViewState("UserId").ToString)
                lbStatus.Text = Session("SelectCommand")
                Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormTrBatchBlockAdjust.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSensusPanen", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
                tbRef.Text = GetAutoNmbr("WOP", "Y", Year(tbDate.SelectedDate), Month(tbDate.SelectedDate), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = tbRef.Text
                'Exit Sub

                SQLString = "INSERT INTO PLWOPlanHd (TransNmbr, TransDate, Status, WorkBy, Divisi, JobPlant, UserPrep, DatePrep, UserAppr, DateAppr) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "',  'H', " + _
                QuotedStr(ddlWorkBy.SelectedValue) + ", " + QuotedStr(ddlDivisi.SelectedValue) + " , " + QuotedStr(tbJobCode.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLWOPlanHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLWOPlanHd SET WorkBy = " + QuotedStr(ddlWorkBy.SelectedValue) + _
                ", Divisi = " + QuotedStr(ddlDivisi.SelectedValue) + " , JobPlant = " + QuotedStr(tbJobCode.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", DateAppr = getDate()" + _
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
            Dim cmdSql As New SqlCommand("TransNmbr, DivisiBlok, DivisiBlokName, StatusTanam, Percentage, QtyTotal, QtyCancel, NormaHK, NormaStd, WorkDay, TargetHK, StartDate, EndDate From V_PLWOPlanDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLSensusPanenDt SET Block = @Block, ProposeWeek= @ProposeWeek, Density = @Density, Areal= @Areal, StartWeight= @StartWeight, EndWeight= @EndWeight, TotStartWeight= @TotStartWeight, TotEndWeight = @TotEndWeight, Remark = @Remark " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Block = @OldBlock", con)
            ' Define output parameters.

            Update_Command.Parameters.Add("@Block", SqlDbType.VarChar, 5, "Block")
            Update_Command.Parameters.Add("@ProposeWeek", SqlDbType.Float, 18, "ProposeWeek")
            Update_Command.Parameters.Add("@Density", SqlDbType.Int, 4, "Density")
            Update_Command.Parameters.Add("@Areal", SqlDbType.Float, 9, "Areal")
            Update_Command.Parameters.Add("@StartWeight", SqlDbType.Float, 9, "StartWeight")
            Update_Command.Parameters.Add("@EndWeight", SqlDbType.Float, 9, "EndWeight")
            Update_Command.Parameters.Add("@TotStartWeight", SqlDbType.Float, 9, "TotStartWeight")
            Update_Command.Parameters.Add("@TotEndWeight", SqlDbType.Float, 9, "TotEndWeight")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldBlock", SqlDbType.VarChar, 5, "Block")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLSensusPanenDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Block = @Block", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Block", SqlDbType.VarChar, 5, "Block")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLSensusPanenDt")

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            ' lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            ' Exit Sub
            ' End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "Block") = False Then
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
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DtRemark") = ""
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        'Dim SQLString As String
        Try
            tbRef.Text = ""
            ddlDivisi.SelectedValue = ""
            ddlWorkBy.SelectedValue = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbJobCode.Text = ""
            '  Dim Division As String
            ' Division = SQLExecuteScalar("EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection"))
            '            ddlDivision.SelectedValue = Division

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            ddlWeek.SelectedValue = ""
            tbStartDate.Text = ""
            tbEndDate.Text = ""
            tbDensity.Text = ""
            tbAreal.Text = ""
            tbStartWeight.Text = ""
            tbEndWeight.Text = ""
            tbTotEndWeight.Text = ""
            tbTotStartWeight.Text = ""
            tbRemarkDt.Text = ViewState("DtRemark")

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
            If ddlDivisi.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Check By must have value")
                ddlDivisi.Focus()
                Return False
            End If
            'If CFloat(tbQtyTanam.Text) < 0 Then
            '    lbStatus.Text = MessageDlg("Qty Tanam must have value")
            '    tbQtyTanam.Focus()
            '    Return False
            'End If

            If Len(tbJobCode.Text.Trim) > 60 Then
                lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
                tbJobCode.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Block").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("TK Panen Must Have Value")
                    Return False
                End If

                If Dr("ProposeWeek").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Propose Week must have value")
                    ddlWeek.Focus()
                    Return False
                End If
                'If Dr("Remark").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Remark Must Have Value")
                '    Return False
                'End If
            Else
                If tbCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Block Must Have Value")
                    tbCode.Focus()
                    Return False
                End If


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
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub cbSelectDt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridDt, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransNmbr"
            FilterName = "TransNmbr, Date, WoNo, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), WoNo, Remark"
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
                    tbKapasitas.Enabled = True
                    btnSaveEdit.Visible = True
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
                        Session("SelectCommand") = "EXEC S_PLFormTrBatchBlockAdjust " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormTrBatchBlockAdjust.frx"
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
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSaveEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveEdit.Click
        Dim SQLString As String
        Dim Result As String

        Dim cbselek As CheckBox
        Dim GRW As GridViewRow
        Dim Kapasitas As TextBox
        Dim lbBlok, lbType As Label
        Try
            '  Kapasitas = GV
            ''cb = sender
            For Each GRW In GridDt.Rows
                cbselek = GRW.FindControl("cbSelectdt")
                Kapasitas = GRW.FindControl("tbkapasitas")
                lbBlok = GRW.FindControl("lbBlok")
                lbType = GRW.FindControl("lbType")

                SQLString = "Declare @A VarChar(255) EXEC S_PLWOPlanAdjustApply " + QuotedStr(tbRef.Text) + ", " + QuotedStr(Kapasitas.Text) + "," + QuotedStr(lbType.Text) + "," + QuotedStr(lbBlok.Text) + ""
                Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
                
            Next
            '  BindDataDt(ViewState("TransNmbr"))
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
        'BtnSaveEdit_Click(Nothing, Nothing)
    End Sub

   


    'Dim TotalAmount, TotalBalance As Double


    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
    '                ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
    '                'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
    '                ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                TotalAmount = GetTotalSum(ViewState("Dt"), "Amount")
    '            End If

    '            tbTAmount.Text = FormatNumber(TotalAmount, ViewState("DigitHome"))
    '            TotalBalance = Val(tbTAmount.Text) - Val(tbTDepr.Text)
    '            tbTBalance.Text = FormatNumber(TotalBalance, ViewState("DigitHome"))

    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        dr = ViewState("Dt").Select("Block = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = tbCode.Text
            tbCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlDivisi, Dt.Rows(0)("Divisi").ToString)
            BindToDropList(ddlWorkBy, Dt.Rows(0)("WorkBy").ToString)
            BindToText(tbJobCode, Dt.Rows(0)("JobPlant").ToString)
            BindToText(tbJobName, Dt.Rows(0)("JobPlantName").ToString)
            BindToText(tbPerson, Dt.Rows(0)("Person").ToString)
            'FillCombo(ddlBlok , "EXEC S_GetCostCtrDept " + QuotedStr(ddlTPH.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Block = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbCode, Dr(0)("Block").ToString)
                BindToText(tbName, Dr(0)("BlockName").ToString)
                BindToDropList(ddlWeek, Dr(0)("ProposeWeek").ToString)
                BindToText(tbStartDate, Dr(0)("StartDate").ToString)
                BindToText(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbDensity, Dr(0)("Density").ToString)
                BindToText(tbAreal, Dr(0)("Areal").ToString)
                BindToText(tbStartWeight, Dr(0)("startWeight").ToString)
                BindToText(tbEndWeight, Dr(0)("EndWeight").ToString)
                BindToText(tbTotStartWeight, Dr(0)("TotStartWeight").ToString)
                BindToText(tbTotEndWeight, Dr(0)("TotEndWeight").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If CekDt() = False Then
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbCode.Text Then
                    If CekExistData(ViewState("Dt"), "Block ", tbCode.Text) Then
                        lbStatus.Text = "Block '" + tbName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Block = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("Block") = tbCode.Text
                'Row("BlockName") = tbName.Text
                Row("ProposeWeek") = ddlWeek.SelectedValue
                'Row("StartDate") = tbStartDate.Text
                'Row("EndDate") = tbEndDate.Text
                Row("Density") = tbDensity.Text
                Row("Areal") = tbAreal.Text
                Row("startWeight") = tbStartWeight.Text
                Row("EndWeight") = tbEndWeight.Text
                Row("TotStartWeight") = tbTotStartWeight.Text
                Row("TotEndWeight") = tbTotEndWeight.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Block", tbCode.Text) Then
                    lbStatus.Text = "Block '" + tbName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Block") = tbCode.Text
                'dr("BlockName") = tbName.Text
                dr("ProposeWeek") = ddlWeek.SelectedValue
                'dr("StartDate") = tbStartDate.Text
                'dr("EndDate") = tbEndDate.Text
                dr("Density") = tbDensity.Text
                dr("Areal") = tbAreal.Text
                dr("startWeight") = tbStartWeight.Text
                dr("EndWeight") = tbEndWeight.Text
                dr("TotStartWeight") = tbTotStartWeight.Text
                dr("TotEndWeight") = tbTotEndWeight.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            ViewState("DtRemark") = tbRemarkDt.Text
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

    Protected Sub btnBlock_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBlock.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT BlockCode, BlockName From MsBlock " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "BlockCode, BlockName"
            ResultField = "BlockCode, BlockName"

            ViewState("Sender") = "btnBlock"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT BlockCode, BlockName From MsBlock Where Block = '" + tbCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCode.Text = Dr("BlockCode").ToString
                tbName.Text = Dr("BlockName").ToString
            Else
                tbCode.Text = ""
                tbName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb TK PanenCode Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub ddlWeek_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWeek.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Week_No, Start_Date, End_Date FROM V_MsWeek WHERE Week_No = " + QuotedStr(ddlWeek.SelectedValue), ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                ddlWeek.SelectedValue = Dr("Week_No").ToString
                tbStartDate.Text = Dr("End_Date").ToString
                tbEndDate.Text = Dr("End_Date").ToString
            Else
                ddlWeek.SelectedValue = ""
                tbStartDate.Text = ""
                tbEndDate.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("ddlWeek Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbDensity_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDensity.TextChanged, tbAreal.TextChanged, tbStartWeight.TextChanged, tbEndWeight.TextChanged, tbTotStartWeight.TextChanged, tbTotEndWeight.TextChanged
        Try

            'tbTanam.Text = CInt(CFloat(tbTanamDt.Text))
            tbTotStartWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbStartWeight.Text)
            tbTotEndWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbEndWeight.Text)
            'tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbTanamDt_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub TbKapasitas_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKapasitas.TextChanged
        Try

            'tbTanam.Text = CInt(CFloat(tbTanamDt.Text))
            tbTotStartWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbStartWeight.Text)
            tbTotEndWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbEndWeight.Text)
            'tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbTanamDt_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStartWeight_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDensity.TextChanged, tbAreal.TextChanged, tbStartWeight.TextChanged, tbEndWeight.TextChanged, tbTotStartWeight.TextChanged, tbTotEndWeight.TextChanged
        Try

            'tbTanam.Text = CInt(CFloat(tbTanamDt.Text))
            tbTotStartWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbStartWeight.Text)
            tbTotEndWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbEndWeight.Text)
            'tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbTanamDt_TextChanged Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbEndWeight_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDensity.TextChanged, tbAreal.TextChanged, tbStartWeight.TextChanged, tbEndWeight.TextChanged, tbTotStartWeight.TextChanged, tbTotEndWeight.TextChanged
        Try

            'tbTanam.Text = CInt(CFloat(tbTanamDt.Text))
            tbTotStartWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbStartWeight.Text)
            tbTotEndWeight.Text = CFloat(tbDensity.Text) * CFloat(tbAreal.Text) * CFloat(tbEndWeight.Text)
            'tbLuasAdjust.Text = FormatFloat(CFloat(tbSDHI.Text) - CFloat(tbLastSensus.Text), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbTanamDt_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnWoNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnWono.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT Wo_No, Wo_Date, WorkBy, Reference, Job, Job_Name, Qty, Unit, Start_Date, End_Date FROM V_PLWOPlanForSensusPanen "
    '        ResultField = "Wo_No, WorkBy"
    '        ViewState("Sender") = "btnWono"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Cust Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub lbTKPanen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTKPanen.Click
    '    Try
    '        ViewState("InputProduct") = "Y"
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTeam')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Product Error : " + ex.ToString
    '    End Try
    'End Sub



    'Protected Sub btnBatchNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatchNo.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT BatchNo, BatchDate, Varietas_Name FROM V_MsBatch Where FgActive = 'Y'"
    '        ResultField = "BatchNo, Varietas_Name"
    '        ViewState("Sender") = "btnBatchNo"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Cust Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridDt.SelectedIndexChanged

    End Sub

    Protected Sub tbKapasitas_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKapasitas.TextChanged
        Dim lbNormaHk As Label
        Dim GRW As GridViewRow
        For Each GRW In GridDt.Rows
            Dim Kapasitas As TextBox

            lbNormaHk = GRW.FindControl("lbNormaHk")
            Kapasitas = GRW.FindControl("tbkapasitas")


            'lbNormaHk.Text = Val(Kapasitas.Text) * 2
        Next
        Try

        Catch ex As Exception
            lbStatus.Text = "textChange Cust Error : " + ex.ToString
        End Try
    End Sub

   
    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim cbselek As CheckBox
        Dim GRW As GridViewRow
        Dim Kapasitas As TextBox
        Try
            '  Kapasitas = GV
            ''cb = sender
            For Each GRW In GridDt.Rows
                cbselek = GRW.FindControl("cbSelect")
                Kapasitas = GRW.FindControl("tbkapasitas")

                If tbKapasitas.Text = "" Then
                    lbStatus.Text = MessageDlg("Kapasitas Must Be Value")
                    tbKapasitas.Focus()
                    Exit Sub
                End If

                If cbselek.Checked Then
                    Kapasitas.Text = tbKapasitas.Text

                Else

                    lbStatus.Text = "Data Must Be Checked First"
                    Exit Sub
                End If

            Next

        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

  
End Class
