Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class TrTransferModule
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLTransferModuleHd"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbUse.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
                    tbModule.Text = Session("Result")(0).ToString
                    tbModuleName.Text = Session("Result")(1).ToString
                    tbMaxCap.Text = Session("Result")(2).ToString
                    tbUse.Text = Session("Result")(3).ToString
                    tbMN.Text = "0"

                    tbRepair.Text = "0"
                    tbAbnormal.Text = "0"
                    tbSaldoCap.Text = "0"

                    tbMNHd.Text = "0"
                    tbSaldo.Text = CInt(CFloat(tbPN.Text) - CFloat(tbMNHd.Text) - CFloat(tbRepairHd.Text) - CFloat(tbAbnormalHd.Text))


                    'tbSaldoCap.Text = CInt(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text))
                    'tbAdjust.Text = CInt(CFloat(tbOk.Text) - CFloat(tbLastSensus.Text))
                    'tbLuasAdjust.Text = CInt(CFloat(tbLastSensus.Text) - CFloat(tbOk.Text))

                    'FillCombo(tbModule, "SELECT DISTINCT Module, ModuleName FROM V_PLSensusPokokTBMDt WHERE TransNmbr =  " + QuotedStr(Session("Result")(0).ToString), True, "Module", "ModuleName", ViewState("DBConnection"))
                    'BindToDropList(tbModule, Session("Result")(3))

                End If
                'Hasil Searh Dialog Dari btn_batchHD
                If ViewState("Sender") = "btnBatchHD" Then
                    tbBatch.Text = Session("Result")(0).ToString
                    tbVarietas.Text = Session("Result")(1).ToString
                    tbVarietasID.Text = Session("Result")(2).ToString
                    tbBedeng.Text = Session("Result")(3).ToString
                    tbBedengID.Text = Session("Result")(4).ToString
                    tbPN.Text = Session("Result")(5).ToString

                    tbAbnormalHd.Text = "0"
                    tbRepairHd.Text = "0"
                    tbMNHd.Text = "0"
                    tbSaldo.Text = CInt(CFloat(tbPN.Text) - CFloat(tbMNHd.Text) - CFloat(tbRepairHd.Text) - CFloat(tbAbnormalHd.Text))

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
        FillCombo(ddlWork, "EXEC S_GetTeam", True, "Team_Code", "Team_Name", ViewState("DBConnection"))
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, Module, ModuleName, QtyMax, QtyUse, QtyOK, QtySaldo, QtyReject, QtyRepair, Remark FROM V_PLTransferModuleDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtJS(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, Module, ModuleName, QtyMax, QtyUse, QtyOK, QtySaldo, QtyReject, QtyRepair, Remark FROM V_PLTransferModuleDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PLFormTransferModule  " + Result + " , " + QuotedStr(ViewState("UserId").ToString)
                lbStatus.Text = Session("SelectCommand")
                Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormTransferModule.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLTransferModule", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            If ViewState("StateSave") = "Insert" Then
                tbRef.Text = GetAutoNmbr("PTM", "Y", Year(tbDate.SelectedDate), Month(tbDate.SelectedDate), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLTransferModuleHd (TransNmbr, Status, BatchNo, Varietas, Bedeng, QtyPN, QtyMN, QtyReject, QtyRepair, QtySaldo, TransDate, Remark, UserPrep, DatePrep, WorkBy) " + _
                "SELECT " + QuotedStr(tbRef.Text) + " , 'H', " + QuotedStr(tbBatch.Text) + ", " + QuotedStr(tbVarietasID.Text) + " , " + _
                QuotedStr(tbBedengID.Text) + ", " + tbPN.Text.Replace(",", "") + ", " + tbMNHd.Text.Replace(",", "") + ",  " + tbAbnormalHd.Text.Replace(",", "") + ", " + _
                tbRepairHd.Text.Replace(",", "") + ", " + tbSaldo.Text.Replace(",", "") + ", '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "' ," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ddlWork.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLTransferModuleHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLTransferModuleHd SET WorkBy = " + QuotedStr(ddlWork.SelectedValue) + " , Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " BatchNo = " + QuotedStr(tbBatch.Text) + ", Varietas = " + QuotedStr(tbVarietasID.Text) + ", Bedeng = " + QuotedStr(tbBedengID.Text) + ", " + _
                " QtyPN = " + tbPN.Text.Replace(",", "") + ",  QtyMN = " + tbMNHd.Text.Replace(",", "") + ",  QtyReject = " + tbAbnormalHd.Text.Replace(",", "") + ", QtyRepair = " + tbRepairHd.Text.Replace(",", "") + ", " + _
                " QtySaldo = " + tbSaldo.Text.Replace(",", "") + "," + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Module, QtyMax, QtyUse, QtyOK, QtySaldo, QtyReject, QtyRepair, Remark FROM PLTransferModuleDT WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLTransferModuleDT SET Module = @Module,QtyMax = @QtyMax, QtyUse = @QtyUse, QtyOK = @QtyOK, QtySaldo = @QtySaldo, QtyReject = @QtyReject, QtyRepair = @QtyRepair, Remark = @Remark  WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Module = @OldModule", con)
            'Define output parameters.
            Update_Command.Parameters.Add("@Module", SqlDbType.VarChar, 5, "Module")
            Update_Command.Parameters.Add("@QtyMax", SqlDbType.Float, 9, "QtyMax")
            Update_Command.Parameters.Add("@QtyUse", SqlDbType.Float, 9, "QtyUse")
            Update_Command.Parameters.Add("@QtyOK", SqlDbType.Float, 9, "QtyOK")
            Update_Command.Parameters.Add("@QtySaldo", SqlDbType.Float, 9, "QtySaldo")
            Update_Command.Parameters.Add("@QtyReject", SqlDbType.Float, 9, "QtyReject")
            Update_Command.Parameters.Add("@QtyRepair", SqlDbType.Float, 9, "QtyRepair")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")


            'Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldModule", SqlDbType.VarChar, 5, "Module")
            param.SourceVersion = DataRowVersion.Original
            'Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            'Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLTransferModuleDT WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Module = @Module", con)
            'Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Module", SqlDbType.VarChar, 5, "Module")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command


            Dim Dt As New DataTable("PLTransferModuleDT")

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
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            btnHome.Visible = False
            ddlWork.Focus()
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
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlWork.SelectedValue = ""
            tbBatch.Text = ""
            tbVarietas.Text = ""
            tbBedeng.Text = ""
            tbPN.Text = ""
            tbMNHd.Text = ""
            tbAbnormalHd.Text = ""
            tbRepairHd.Text = ""
            tbSaldo.Text = ""
            tbRemark.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbModule.Text = ""
            'tbBatchDate.SelectedDate = ViewState("ServerDate") 'Today
            tbModuleName.Text = ""
            tbMaxCap.Text = "0"
            tbUse.Text = "0"
            tbMN.Text = "0"
            tbRepair.Text = "0"
            tbSaldoCap.Text = "0"
            tbAbnormal.Text = "0"
            tbRemarkDt.Text = ""


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
                If CekDt(dr, "Module") = False Then
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

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            'Throw New Exception("CekHd Error : " + ex.ToString)
        End Try
    End Function


    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Module").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Module No Must Have Value")
                    Return False
                End If
                'If (Dr("Rotasi").ToString = "") Then
                '    lbStatus.Text = MessageDlg("Rotasi " + Dr("Rotasi").ToString + " must have value Rotasi")
                '    Return False
                'End If

            Else
                If tbModule.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Module Name Must Have Value")
                    tbModule.Focus()
                    Return False
                End If

                If tbModuleName.Text.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Module Name Must Have Value")
                    tbModuleName.Focus()
                    Return False
                End If
                If (tbMaxCap.Text.Trim = "") Then
                    lbStatus.Text = MessageDlg("Max Cap must have value")
                    Return False

                End If
                If (tbUse.Text.Trim) = 0 Then
                    lbStatus.Text = MessageDlg("Use must have value")
                    tbUse.Focus()
                    Return False
                End If

                'If (tbMN.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("MN Tone must have value")
                '    tbMN.Focus()
                '    Return False
                'End If
                'If (tbRepair.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Repair must have value")
                '    tbRepair.Focus()
                '    Return False
                'End If

                'If (tbSaldoCap.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Saldo Cap must have value")
                '    tbSaldoCap.Focus()
                '    Return False
                'End If
                'If (tbAbnormal.Text.Trim) < "0" Then
                '    lbStatus.Text = MessageDlg("Luas Adjust must have value")
                '    tbAbnormal.Focus()
                '    Return False
                'End If

                If (tbRemarkDt.Text.Trim) = "" Then
                    lbStatus.Text = MessageDlg("Remark must have value")
                    tbRemarkDt.Focus()
                    Return False
                End If
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
            FilterName = " Date, WorkBy, BatchNo, Varietas, Bedeng, QtyPN, QtyMN, QtyReject, QtyRepair, QtySaldo, Remark"
            FilterValue = " WorkBy, BatchNo, Varietas, Bedeng, QtyPN, QtyMN, QtyReject, QtyRepair, QtySaldo, TransNmbr, Status, Format_Name, Remark"
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
                        Session("SelectCommand") = "EXEC S_PLFormTransferModule  " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormTransferModule.frx"
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

        dr = ViewState("Dt").Select("Module = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("DtValue") = tbModule.Text
            tbModule.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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
            BindToDropList(ddlWork, Dt.Rows(0)("WorkBy").ToString)
            BindToText(tbBatch, Dt.Rows(0)("BatchNo").ToString)
            BindToText(tbVarietas, Dt.Rows(0)("VarietasName").ToString)
            BindToText(tbVarietasID, Dt.Rows(0)("Varietas").ToString)
            BindToText(tbBedeng, Dt.Rows(0)("BedengName").ToString)
            BindToText(tbBedengID, Dt.Rows(0)("Bedeng").ToString)
            BindToText(tbPN, Dt.Rows(0)("QtyPN").ToString)
            BindToText(tbMNHd, Dt.Rows(0)("QtyMN").ToString)
            BindToText(tbAbnormalHd, Dt.Rows(0)("QtyReject").ToString)
            BindToText(tbRepairHd, Dt.Rows(0)("QtyRepair").ToString)
            BindToText(tbSaldo, Dt.Rows(0)("QtySaldo").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            Type = SQLExecuteScalar("Select ModuleName From V_PLTransferModuleGetModule WHERE Module = " + QuotedStr(tbModule.Text), ViewState("DBConnection").ToString)
            'FillCombo(ddlBlok , "EXEC S_GetCostCtrDept " + QuotedStr(ddlTPH.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Modul As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Module = " + QuotedStr(Modul))
            If Dr.Length > 0 Then
                BindToText(tbModule, Dr(0)("Module").ToString)
                BindToText(tbModuleName, Dr(0)("ModuleName").ToString)
                BindToText(tbMaxCap, CFloat(Dr(0)("QtyMax").ToString))
                BindToText(tbUse, CFloat(Dr(0)("QtyUse").ToString))
                BindToText(tbMN, CFloat(Dr(0)("QtyOK").ToString))
                BindToText(tbRepair, CFloat(Dr(0)("QtyRepair").ToString))
                BindToText(tbSaldoCap, CFloat(Dr(0)("QtySaldo").ToString))
                BindToText(tbAbnormal, CFloat(Dr(0)("QtyReject").ToString))
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
                If ViewState("DtValue") <> tbModule.Text Then
                    If CekExistData(ViewState("Dt"), "Module ", tbModule.Text) Then
                        lbStatus.Text = "Module '" + tbModule.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Module = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row = ViewState("Dt").Select("Module = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("Module") = tbModule.Text
                Row("ModuleName") = tbModuleName.Text
                Row("QtyMax") = tbMaxCap.Text
                Row("QtyUse") = tbUse.Text
                Row("QtyOK") = tbMN.Text
                Row("QtySaldo") = tbSaldoCap.Text
                Row("QtyReject") = tbAbnormal.Text
                Row("QtyRepair") = tbRepair.Text
                Row("Remark") = tbRemarkDt.Text

                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                'If CekExistData(ViewState("Dt"), "BatcNo", tbBatchName.Text) Then
                '    lbStatus.Text = "BatcNo '" + tbBatchName.Text + "' has already exists"
                '    Exit Sub
                'End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Module") = tbModule.Text
                dr("ModuleName") = tbModuleName.Text
                dr("QtyMax") = tbMaxCap.Text
                dr("QtyUse") = tbUse.Text
                dr("QtyOK") = tbMN.Text
                dr("QtySaldo") = tbSaldoCap.Text
                dr("QtyReject") = tbAbnormal.Text
                dr("QtyRepair") = tbRepair.Text
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

    Protected Sub btnBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatch.Click
        Dim ResultField As String
        Try
            Session("filter") = " SELECT Module, ModuleName, MaxCap, QtyUse FROM V_PLTransferModuleGetModule"
            ResultField = "Module, ModuleName, MaxCap, QtyUse "
            ViewState("Sender") = "btnBatch"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBatchHD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatchHD.Click
        Dim ResultField As String
        Try
            Session("filter") = " SELECT Batch_No, Varietas_Name, Varietas, Bedeng_Name, Bedeng, Qty FROM V_PLTransferModuleGetBatch"
            ResultField = "Batch_No, Varietas_Name, Varietas, Bedeng_Name, Bedeng, Qty "
            ViewState("Sender") = "btnBatchHD"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbBatch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbBatch.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Batch_No, Varietas_Name, Varietas, Bedeng_Name, Bedeng, Qty FROM V_PLTransferModuleGetBatch WHERE Batch_No = " + QuotedStr(tbBatch.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                'tbLastSensus.Text = FormatFloat(Dr("QtySaldo").ToString, ViewState("DigitQty"))
                tbVarietas.Text = Dr("Varietas_Name").ToString
                tbVarietasID.Text = Dr("Varietas").ToString
                tbBedeng.Text = Dr("Bedeng_Name").ToString
                tbBedengID.Text = Dr("Bedeng").ToString
                tbPN.Text = CInt(Dr("Qty").ToString)
                tbMNHd.Text = "0"
                tbAbnormalHd.Text = "0"
                tbRepairHd.Text = "0"
                tbSaldo.Text = tbPN.Text
            End If
        Catch ex As Exception
            lbStatus.Text = "tbBatch_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub




End Class
