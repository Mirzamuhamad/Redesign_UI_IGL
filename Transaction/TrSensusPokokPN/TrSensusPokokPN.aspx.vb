Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSensusPokokPN_TrSensusPokokPN
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLSensusPokokPNHd"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbDoubleTone.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbBusuk.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbPatah.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbPatah.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
                    tbBatchName.Text = Session("Result")(0).ToString
                    tbBatchDate.SelectedDate = Session("Result")(1).ToString
                    tbVriates.Text = Session("Result")(2).ToString
                    tbRotasi.Text = "0"
                    tbTransferBatch.Text = FormatFloat(Session("Result")(4).ToString, ViewState("DigitQty"))
                    tbVriatesCode.Text = Session("Result")(5).ToString
                    tbBedengName.Text = Session("Result")(6).ToString
                    tbDoubleTone.Text = "0"
                    tbBusuk.Text = "0"
                    tbPatah.Text = "0"
                    tbTanamPN.Text = FormatFloat(CFloat(tbTransferBatch.Text) + CFloat(tbDoubleTone.Text) + CFloat(tbBusuk.Text) + Val(tbPatah.Text), ViewState("DigitQty"))
                    FillCombo(ddlBedeng, "SELECT DISTINCT Bedeng, BedengName FROM V_PLSensusPokokPNGetBatch WHERE Batch_No =  " + QuotedStr(Session("Result")(0).ToString), True, "Bedeng", "BedengName", ViewState("DBConnection"))
                    BindToDropList(ddlBedeng, Session("Result")(3))


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
        FillCombo(ddlBedeng, "SELECT DISTINCT Bedeng, BedengName FROM V_PLSensusPokokPNGetBatch", True, "Bedeng", "BedengName", ViewState("DBConnection"))
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
        Return "SELECT TransNMbr, BatchNo, Bedeng, QtyBatch, QtyReject, QtyDoubleTone, Remark, Rotasi, WONo, QtyAbnormal, QtyPatah, BatchDate, Varietas, VarietasName, QtySaldo, BedengName From V_PLSensusPokokPNDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PLFormSensusPokokPN  " + Result + " , " + QuotedStr(ViewState("UserId").ToString)
                lbStatus.Text = Session("SelectCommand")
                Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSensusPokokPN.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSensusPokokPN", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
                tbRef.Text = GetAutoNmbr("SPP", "Y", Year(tbDate.SelectedDate), Month(tbDate.SelectedDate), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PLSensusPokokPNHd (TransNmbr, Status, TransDate, Remark, UserPrep, DatePrep, SensusBy) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ddlSensus.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSensusPokokPNHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLSensusPokokPNHd SET SensusBy = '" + ddlSensus.SelectedValue + "', Remark = " + QuotedStr(tbRemark.Text) + "," + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, BatchNo, Bedeng, QtyBatch, QtyReject, QtyDoubleTone, Remark, Rotasi, WONo, QtyAbnormal, QtyPatah FROM PLSensusPokokPNDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '        "UPDATE PLSensusPokokPNDt SET BatchNo = @BatchNo,Bedeng = @Bedeng, QtyBatch = @QtyBatch, QtyReject = @QtyReject, QtyDoubleTone = @QtyDoubleTone, Remark = @Remark, Rotasi = @Rotasi, WONo = @WONo , QtyAbnormal = @QtyAbnormal,QtyPatah = @QtyPatah WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND BatchNo = @OldBatchNo", con)
            ''Define output parameters.
            'Update_Command.Parameters.Add("@BatchNo", SqlDbType.VarChar, 20, "BatchNo")
            'Update_Command.Parameters.Add("@Bedeng", SqlDbType.VarChar, 5, "Bedeng")
            'Update_Command.Parameters.Add("@QtyBatch", SqlDbType.Float, 9, "QtyBatch")
            'Update_Command.Parameters.Add("@QtyReject", SqlDbType.Float, 9, "QtyReject")
            'Update_Command.Parameters.Add("@QtyDoubleTone", SqlDbType.Float, 9, "QtyDoubleTone")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 225, "Remark")
            'Update_Command.Parameters.Add("@Rotasi", SqlDbType.Int, 4, "Rotasi")
            'Update_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            'Update_Command.Parameters.Add("@QtyAbnormal", SqlDbType.Float, 9, "QtyAbnormal")
            'Update_Command.Parameters.Add("@QtyPatah", SqlDbType.Float, 9, "QtyPatah")


            ''Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldBatchNo", SqlDbType.VarChar, 5, "BatchNo")
            'param.SourceVersion = DataRowVersion.Original
            ''Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            ''Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM PLSensusPokokPNDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND BatchNo = @BatchNo", con)
            ''Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@BatchNo", SqlDbType.VarChar, 5, "BatchNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLSensusPokokPNDt")

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
                If CekDt(dr, "BatchNo") = False Then
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
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlSensus.SelectedValue = ""

            '  Dim Division As String
            ' Division = SQLExecuteScalar("EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection"))
            '            ddlDivision.SelectedValue = Division

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbBatchName.Text = ""
            tbBatchDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemarkDt.Text = ""
            ddlBedeng.SelectedValue = ""
            tbVriates.Text = ""
            tbTransferBatch.Text = "0"
            tbDoubleTone.Text = "0"
            tbBusuk.Text = "0"
            tbPatah.Text = "0"
            tbTanamPN.Text = "0"


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
                If CekDt(dr, "BatchNo") = False Then
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
                If Dr("BatchNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Batch No Must Have Value")
                    Return False
                End If
                If (Dr("Rotasi").ToString = "") Then
                    lbStatus.Text = MessageDlg("Rotasi " + Dr("Rotasi").ToString + " must have value Rotasi")
                    Return False
                End If

            Else
                If tbBatchName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Batch Must Have Value")
                    tbBatchName.Focus()
                    Return False
                End If
                If (ddlBedeng.SelectedValue.Trim = "") Then
                    lbStatus.Text = MessageDlg("Bedeng must have value")
                    Return False

                End If
                If (tbTransferBatch.Text.Trim) = 0 Then
                    lbStatus.Text = MessageDlg("Transfer Batch must have value")
                    tbTransferBatch.Focus()
                    Return False
                End If

                'If (tbDoubleTone.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Double Tone must have value")
                '    tbDoubleTone.Focus()
                '    Return False
                'End If
                'If (tbPatah.Text.Trim) = 0 Then
                '    lbStatus.Text = MessageDlg("Patah Tone must have value")
                '    tbPatah.Focus()
                '    Return False
                'End If

                If (tbTanamPN.Text.Trim) = 0 Then
                    lbStatus.Text = MessageDlg("Tanam PN must have value")
                    tbTanamPN.Focus()
                    Return False
                End If

                If (tbRemark.Text.Trim) = "" Then
                    lbStatus.Text = MessageDlg("Remark must have value")
                    tbRemark.Focus()
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
            FilterName = "Date, SensusBy, Remark"
            FilterValue = "TransNmbr, Status, Format_Name, Remark"
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
                        Session("SelectCommand") = "EXEC S_PLFormSensusPokokPN  " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormSensusPokokPN.frx"
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
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "BatchNo")) Then
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

        dr = ViewState("Dt").Select("BatchNo = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("DtValue") = tbBatchName.Text
            tbBatchName.Focus()
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
            'newTrans(
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            ' BindToText(tbRotasi, Dt.Rows(0)("Rotasi").ToString)
            BindToDropList(ddlSensus, Dt.Rows(0)("SensusBy").ToString)
            Type = SQLExecuteScalar("Select Varietas_Name FROM V_PLSensusPokokPNGetBatch WHERE Batch_No = " + QuotedStr(ddlBedeng.SelectedValue), ViewState("DBConnection").ToString)

            'FillCombo(ddlBlok , "EXEC S_GetCostCtrDept " + QuotedStr(ddlTPH.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("BatchNo = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbBatchName, Dr(0)("BatchNo").ToString)
                BindToDate(tbBatchDate, Dr(0)("BatchDate").ToString)
                BindToText(tbVriates, Dr(0)("VarietasName").ToString)
                BindToText(tbVriatesCode, Dr(0)("Varietas").ToString)
                BindToDropList(ddlBedeng, Dr(0)("Bedeng").ToString)
                BindToText(tbBedengName, Dr(0)("BedengName").ToString)
                BindToText(tbTransferBatch, CFloat(Dr(0)("QtyBatch").ToString))
                BindToText(tbBusuk, CFloat(Dr(0)("QtyReject").ToString))
                BindToText(tbDoubleTone, CFloat(Dr(0)("QtyDoubleTone").ToString))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbRotasi, Dr(0)("Rotasi").ToString)
                BindToText(tbPatah, CFloat(Dr(0)("QtyPatah").ToString))
                BindToText(tbTanamPN, CFloat(Dr(0)("QtySaldo").ToString))
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
                If ViewState("DtValue") <> tbBatchName.Text Then
                    If CekExistData(ViewState("Dt"), "BatcNo ", tbBatchName.Text) Then
                        lbStatus.Text = "BatcNo '" + tbBatchName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("BatchNo = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row.BeginEdit()
                Row("BatchNo") = tbBatchName.Text
                Row("BatchDate") = tbBatchDate.SelectedDate
                Row("VarietasName") = tbVriates.Text
                Row("Varietas") = tbVriatesCode.Text
                Row("Bedeng") = ddlBedeng.Text
                Row("Bedeng") = ddlBedeng.SelectedValue
                Row("BedengName") = tbBedengName.Text
                Row("QtyBatch") = tbTransferBatch.Text
                Row("QtyReject") = tbBusuk.Text
                Row("QtyDoubleTone") = tbDoubleTone.Text
                Row("Remark") = tbRemarkDt.Text
                Row("Rotasi") = tbRotasi.Text
                Row("QtyPatah") = tbPatah.Text
                Row("QtySaldo") = tbTanamPN.Text

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
                dr("BatchNo") = tbBatchName.Text
                dr("BatchDate") = tbBatchDate.SelectedDate
                dr("Varietas") = tbVriatesCode.Text
                dr("VarietasName") = tbVriates.Text
                dr("Bedeng") = ddlBedeng.SelectedValue
                dr("BedengName") = tbBedengName.Text
                dr("QtyBatch") = tbTransferBatch.Text
                dr("QtyReject") = tbBusuk.Text
                dr("QtyDoubleTone") = tbDoubleTone.Text
                dr("Remark") = tbRemarkDt.Text
                dr("Rotasi") = tbRotasi.Text
                dr("QtyPatah") = tbPatah.Text
                dr("QtySaldo") = tbTanamPN.Text
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
            Session("filter") = "SELECT  Batch_No, Batch_Date, Varietas, Varietas_Name, Bedeng, BedengName, Qty  FROM V_PLSensusPokokPNGetBatch"
            ResultField = "Batch_No, Batch_Date, Varietas_Name, Bedeng, Qty,Varietas, BedengName"
            ViewState("Sender") = "btnBatch"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlBedeng_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBedeng.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            

            Dt = SQLExecuteQuery("SELECT Bedeng, BedengName, Qty FROM V_PLSensusPokokPNGetBatch WHERE Batch_No = " + QuotedStr(tbBatchName.Text) + " AND Bedeng = " + QuotedStr(ddlBedeng.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbTransferBatch.Text = FormatFloat(Dr("Qty").ToString, ViewState("DigitQty"))
                tbTanamPN.Text = FormatFloat((CFloat(tbTransferBatch.Text) + CFloat(tbDoubleTone.Text)) - (CFloat(tbBusuk.Text) + Val(tbPatah.Text)), ViewState("DigitQty"))
                tbBedengName.Text = Dr("BedengName").ToString
            End If
            Exit Sub
        Catch ex As Exception
            lbStatus.Text = "ddlBedeng_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDoubleTone_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDoubleTone.TextChanged, tbBusuk.TextChanged, tbPatah.TextChanged
        Try
            tbTanamPN.Text = FormatFloat(CFloat((tbTransferBatch.Text) + CFloat(tbDoubleTone.Text)) - (CFloat(tbBusuk.Text) + Val(tbPatah.Text)), ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbDoubleTone_TextChanged Error : " + ex.ToString
        End Try
    End Sub


End Class
