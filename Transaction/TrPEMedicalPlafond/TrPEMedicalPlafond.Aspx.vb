Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPEMedicalPlafond
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PEMedicalInHd"
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
                If Not Session("PostNmbr") = Nothing Then
                    tbFilter.Text = Session("PostNmbr")
                    btnSearch_Click(Nothing, Nothing)
                    Session("PostNmbr") = Nothing
                    tbFilter.Text = ""
                End If
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnMedical" Then
                    tbMedicalCode.Text = Session("Result")(0).ToString
                    tbMedicalName.Text = Session("Result")(1).ToString
                    tbPlafond.Text = Session("Result")(2).ToString
                    tbAmountPlafond.Text = Session("Result")(2).ToString                    
                    tbJobLevel.Text = Session("Result")(3).ToString
                    ddlCheckPlafond.SelectedValue = Session("Result")(4).ToString
                    ddlCheck1Plafond.SelectedValue = Session("Result")(5).ToString
                    If ddlCheckPlafond.SelectedValue = "N" Then
                        ddlCheckPlafond.Enabled = False
                        tbAmountPlafond.Enabled = False
                        tbAmountPlafond.Text = "0"
                    Else
                        ddlCheckPlafond.Enabled = True
                        tbAmountPlafond.Enabled = True
                        'tbAmountPlafond.Text = "0"
                    End If
                    If ddlCheck1Plafond.SelectedValue = "N" Then
                        ddlCheck1Plafond.Enabled = False
                        tbAmount1Plafond.Enabled = False
                        tbAmount1Plafond.Text = "0"
                    Else
                        ddlCheck1Plafond.Enabled = True
                        tbAmount1Plafond.Enabled = True
                        tbAmount1Plafond.Text = "0"
                    End If
                    'tbPlafond_TextChanged(Nothing, Nothing)
                    tbJobLevel_TextChanged(Nothing, Nothing)
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
        FillRange(ddlRange)
        FillCombo(ddlJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
        FillCombo(ddlCurrency, "EXEC S_GetCurrency", True, "Currency", "Currency_Name", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

        tbAmountPlafond.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmount1Plafond.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbAmountPlafond.Attributes.Add("OnBlur", "setformat();")
        tbAmount1Plafond.Attributes.Add("OnBlur", "setformat();")
        
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
            'If Len(StrFilter) = 0 Then
            '    StrFilter = " UserId = '" + ViewState("UserId").ToString + "'"
            'Else
            '    StrFilter = StrFilter + " AND UserId = '" + ViewState("UserId").ToString + "'"
            'End If
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
        Return "SELECT * From V_PEMedicalIndt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
    Public Function ExecSPCekPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal UserId As String, Optional ByVal Connection As String = "Nothing") As DataTable
        Dim Mycon As New SqlConnection
        Dim DT As DataTable

        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Mycon = New SqlConnection(Connection)

        Dim sqlstring As String
        sqlstring = ""
        If PrimaryKey.Length = 1 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 2 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 3 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(UserId)
        End If
        Try
            DT = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

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
                Session("SelectCommand") = "EXEC S_PEFormMedicalIn " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormMedicalIn.frx"
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
                    ElseIf ActionValue = "Post" Then

                        Result = ExecSPCommandGo(ActionValue, "S_PEMedicalIn", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PEMedicalIn", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

            'ddlDept.Enabled = False

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
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Dim Joblvl As String
        Try
            Joblvl = "''"
            If tbJobLevel.Text.Trim = "Y" Then
                Joblvl = ""
            ElseIf tbJobLevel.Text.Trim = "N" Then
                Joblvl = ddlJobLevel.SelectedValue
            End If

            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbMedicalCode.Text Then
                    If CekExistData(ViewState("Dt"), "Medical,JobLevel", tbMedicalCode.Text + "|" + Joblvl) Then
                        lbStatus.Text = "Medical '" + tbMedicalName.Text + "' and Job Level '" + Joblvl + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Medical = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Medical") = tbMedicalCode.Text
                Row("Medical_Name") = tbMedicalName.Text
                'Row("JobLevel") = Joblvl
                Row("JobLevel") = ddlJobLevel.SelectedValue
                Row("JobLevel_Name") = ddlJobLevel.Text
                Row("FgCheckPlafond") = ddlCheckPlafond.SelectedValue
                Row("AmountPlafond") = tbAmountPlafond.Text
                Row("FgCheck1Plafond") = ddlCheck1Plafond.SelectedValue
                Row("Amount1Plafond") = tbAmount1Plafond.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Medical,JobLevel", tbMedicalCode.Text + "|" + Joblvl) Then
                    lbStatus.Text = "Medical '" + tbMedicalName.Text + " and Job Level " + Joblvl + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Medical") = tbMedicalCode.Text
                dr("Medical_Name") = tbMedicalName.Text
                'dr("JobLevel") = Joblvl
                dr("JobLevel") = ddlJobLevel.SelectedValue
                dr("JobLevel_Name") = ddlJobLevel.Text
                dr("FgCheckPlafond") = ddlCheckPlafond.SelectedValue
                dr("AmountPlafond") = tbAmountPlafond.Text
                dr("FgCheck1Plafond") = ddlCheck1Plafond.SelectedValue
                dr("Amount1Plafond") = tbAmount1Plafond.Text
                dr("Remark") = tbRemarkDt.Text
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
                tbTransNo.Text = GetAutoNmbr("MI", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                ViewState("Reference") = tbTransNo.Text
                SQLString = "INSERT INTO PEMedicalInHD (TransNmbr, STATUS, Transdate, EffectiveDate, Currency, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                "," + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEMedicalInHD WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEMedicalInHD SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "',EffectiveDate = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + _
                "',Currency = " + QuotedStr(ddlCurrency.SelectedValue) + _
                ",UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
                ",Remark = " + QuotedStr(tbRemark.Text) + ", DateAppr = getDate()" + _
                "WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")

            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, Medical, JobLevel, FgCheckPlafond, AmountPlafond, FgCheck1Plafond, Amount1Plafond," + _
                                         " Remark FROM PEMedicalIndt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PEMedicalInDt SET FgCheckPlafond = @FgCheckPlafond, AmountPlafond = @AmountPlafond, FgCheck1Plafond = @FgCheck1Plafond, Amount1Plafond = @Amount1Plafond, Remark = @Remark, EndDate = @EndDate WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Medical = @Medical AND JobLevel = @JobLevel ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@FgCheckPlafond", SqlDbType.VarChar, 20, "FgCheckPlafond")
            Update_Command.Parameters.Add("@AmountPlafond", SqlDbType.Float, 20, "AmountPlafond")
            Update_Command.Parameters.Add("@FgCheck1Plafond", SqlDbType.VarChar, 20, "FgCheck1Plafond")
            Update_Command.Parameters.Add("@Amount1Plafond", SqlDbType.Float, 20, "Amount1Plafond")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 20, "Remark")
            Update_Command.Parameters.Add("@EndDate", SqlDbType.DateTime, 20, "EndDate")


            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@Medical", SqlDbType.VarChar, 20, "Medical")
            param = Update_Command.Parameters.Add("@JobLevel", SqlDbType.VarChar, 20, "JobLevel")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PEMedicalInDt WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Medical = @Medical AND JobLevel = @JobLevel ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Medical", SqlDbType.VarChar, 20, "Medical")
            param = Delete_Command.Parameters.Add("@JobLevel", SqlDbType.VarChar, 20, "JobLevel")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PEMedicalIndt")

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
            tbFilter.Text = tbTransNo.Text
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
            EnableHd(False)
            btnHome.Visible = False
            tbDate.Focus()
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
            GridDt.Columns(1).Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()

        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String

        Try
            tbTransNo.Text = ""
            SQLString = "EXEC S_PEMedicalInCekTransdate " + QuotedStr(ViewState("ServerDate"))

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            tbDate.SelectedDate = ViewState("ServerDate")
            If Not Dr Is Nothing Then
                tbEffectiveDate.SelectedDate = Dr("Transdate").ToString
            Else
                tbEffectiveDate.SelectedDate = ViewState("ServerDate")
            End If

            'Today
            ddlCurrency.SelectedValue = ViewState("Currency")
            tbRemark.Text = ""
            'tbRate.Text = ""
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbMedicalCode.Text = ""
            tbMedicalName.Text = ""
            ddlJobLevel.SelectedIndex = 0
            ddlJobLevel.Enabled = False
            tbRemarkDt.Text = ""
            ddlCheckPlafond.SelectedValue = "N"
            ddlCheck1Plafond.SelectedValue = "N"
            tbAmountPlafond.Text = "0"
            tbAmount1Plafond.Text = "0"
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
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbEffectiveDate.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Effective Date Must have value")
                tbEffectiveDate.Focus()
                Return False
            End If


            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Medical").ToString.Trim = "" Then
                    lbStatus.Text = "Medical Code Must Have Value"
                    Return False
                End If

                If tbJobLevel.Text.Trim = "N" Then
                    If Dr("JobLevel").ToString.Trim = "" Then
                        lbStatus.Text = "Job Level Must Have Value"
                        Return False
                    End If
                End If
                If CFloat(tbPlafond.ToString) > 0 Then
                    If CFloat(Dr("AmountPlafond").ToString) < 0 Then
                        lbStatus.Text = "Amount Plafond Must Have Value"
                        Return False
                    End If
                End If
                If Dr("FgCheck1Plafond").ToString.Trim = "Y" Then
                    If CFloat(Dr("Amount1Plafond").ToString) < 0 Then
                        lbStatus.Text = "Amount Plafond 1 Claim Must Have Value"
                        Return False
                    End If
                End If

            Else
                If tbMedicalCode.Text.Trim = "" Then
                    lbStatus.Text = "Medical Code Must Have Value"
                    tbMedicalCode.Focus()
                    Return False
                End If

                If tbJobLevel.Text.Trim = "N" Then
                    If ddlJobLevel.Text.Trim = "" Then
                        lbStatus.Text = "Job Level Must Have Value"
                        ddlJobLevel.Focus()
                        Return False
                    End If
                End If

                If tbAmountPlafond.Enabled = True Then
                    If CFloat(tbAmountPlafond.Text) <= 0 Then
                        lbStatus.Text = "Amount Plafond Must Have Value"
                        tbAmountPlafond.Focus()
                        Return False
                    End If
                End If

                If ddlCheck1Plafond.SelectedValue = "Y" Then
                    If CFloat(tbAmount1Plafond.Text) <= 0 Then
                        lbStatus.Text = "Amount Plafond 1 Claim Must Have Value"
                        tbAmount1Plafond.Focus()
                        Return False
                    End If
                End If


                End If
                Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    'Protected Sub ddlJE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJE.SelectedIndexChanged
    '    Try
    '        If ViewState("InputFormatJE") = "Y" Then
    '            RefreshMaster("S_GetFormatJE", "Format_Code", "Format_Name", ddlJE, ViewState("DBConnection"))
    '            ViewState("InputFormatJE") = Nothing
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl JE Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Effective Date, Currency, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), EffectiveDate, Currency, Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("Status") = GVR.Cells(3).Text
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                    End If
                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    BindDataDt(ViewState("Reference"))
                    FillTextBoxHd(ViewState("Reference"))
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
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
                        Session("SelectCommand") = "EXEC S_PEFormMedicalin ''" + QuotedStr(GVR.Cells(2).Text) + "''" + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormMedicalin.frx"
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
        Dim index As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Copy" Then
                Dim GVR As GridViewRow
                Try
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridDt.Rows(index)
                    FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(4).Text)
                    MovePanel(pnlDt, pnlEditDt)
                    EnableHd(False)
                    ViewState("StateDt") = "Insert"
                    tbMedicalCode.Focus()
                    StatusButtonSave(False)
                Catch ex As Exception
                    lbStatus.Text = "Grid Dt Row Copy Error : " + ex.ToString
                End Try
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Dim AmountPaid As Decimal
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Medical")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                'BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))                    
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                AmountPaid = GetTotalSum(ViewState("Dt"), "AmountPaid")
    '                tbTotalForex.Text = CStr(AmountPaid) ' FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                'AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try


    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Medical = " + QuotedStr(GVR.Cells(2).Text) + " AND JobLevel = " + QuotedStr(GVR.Cells(4).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(4).Text)
            ViewState("DtValue") = tbMedicalCode.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbMedicalCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToDropList(ddlCurrency, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRemark, Dt.Rows(0)("RemarkHd").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Medical As String, ByVal JobLevel As String)
        Dim Dr As DataRow()

        Try
            Dr = ViewState("Dt").select("Medical = " + QuotedStr(Medical) + " AND JobLevel = " + QuotedStr(JobLevel))
            If Dr.Length > 0 Then

                BindToText(tbMedicalCode, Dr(0)("Medical").ToString)
                BindToText(tbMedicalName, Dr(0)("Medical_Name").ToString)
                BindToDropList(ddlJobLevel, Dr(0)("JobLevel").ToString)
                BindToDropList(ddlCheckPlafond, Dr(0)("FgCheckPlafond").ToString)
                BindToText(tbAmountPlafond, Dr(0)("AmountPlafond").ToString, ViewState("DigitHome"))
                BindToText(tbPlafond, Dr(0)("AmountPlafond").ToString)
                BindToDropList(ddlCheck1Plafond, Dr(0)("FgCheck1Plafond").ToString)
                BindToText(tbAmount1Plafond, Dr(0)("Amount1Plafond").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                tbJobLevel_TextChanged(Nothing, Nothing)
                tbPlafond_TextChanged(Nothing, Nothing)
                ddlCheck1Plafond_TextChanged(Nothing, Nothing)
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
 
    Protected Sub btnMedical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMedical.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Medical_Code, Medical_Name,Plafond1Claim,FgAllJobLevel,FgCheckPlafond,FgCheck1Claim FROM V_MsMedical "
            ResultField = "Medical_Code,Medical_Name,Plafond1Claim,FgAllJobLevel,FgCheckPlafond,FgCheck1Claim"
            ViewState("Sender") = "btnMedical"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Family Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbMedicalCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMedicalCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try

            SQLString = "SELECT DISTINCT Medical_Code,Medical_Name,Plafond1Claim FROM V_MsMedical WHERE Medical_Code = " + QuotedStr(tbMedicalCode.Text)

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbMedicalCode.Text = Dr("Medical_code").ToString
                tbMedicalName.Text = Dr("Medical_Name").ToString
                tbPlafond.Text = Dr("Plafond1Claim").ToString
                tbAmountPlafond.Text = Dr("Plafond1Claim").ToString
                tbPlafond_TextChanged(Nothing, Nothing)
            Else
                tbMedicalCode.Text = ""
                tbMedicalName.Text = ""
                tbPlafond.Text = ""
                tbAmountPlafond.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tb Family Type Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        Try

            Try
                If ViewState("InputCurrency") = "Y" Then
                    RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurrency, ViewState("DBConnection"))
                    ViewState("InputCurrency") = Nothing
                End If
                'ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                AttachScript("setformat();", Page, Me.GetType())
                'tbRate.Focus()
            Catch ex As Exception
                lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
            End Try



        Catch ex As Exception
            Throw New Exception("ddl Medical For Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub lbJobLevel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbJobLevel.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsJobLevel')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Job Level Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub
   
    Protected Sub tbPlafond_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPlafond.TextChanged
        Try
            If CFloat(tbPlafond.Text) > 0 Then
                tbAmountPlafond.Enabled = True
                ddlCheckPlafond.Enabled = True
                ddlCheckPlafond.SelectedIndex = 0
            ElseIf CFloat(tbPlafond.Text) <= 0 Then
                tbAmountPlafond.Enabled = False
                ddlCheckPlafond.Enabled = False
                ddlCheckPlafond.SelectedValue = "N"
            End If
        Catch ex As Exception
            lbStatus.Text = "tb Plafond Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbJobLevel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbJobLevel.TextChanged
        Try
            If tbJobLevel.Text = "Y" Then
                ddlJobLevel.Enabled = False
            ElseIf tbJobLevel.Text = "N" Then
                ddlJobLevel.Enabled = True
            End If

        Catch ex As Exception
            lbStatus.Text = "tb Job Level Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String

        Try
            tbTransNo.Text = ""
            SQLString = "EXEC S_PEMedicalInCekTransdate " + QuotedStr(ViewState("ServerDate"))

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbDate.SelectedDate = Dr("Transdate").ToString
            Else
                tbDate.SelectedDate = ViewState("ServerDate")
            End If

        Catch ex As Exception
            lbStatus.Text = "tb Date Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub ddlCheck1Plafond_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCheck1Plafond.TextChanged
        If ddlCheck1Plafond.SelectedValue = "N" Then
            ddlCheck1Plafond.Enabled = False
            tbAmount1Plafond.Enabled = False
            tbAmount1Plafond.Text = "0"
        Else
            ddlCheck1Plafond.Enabled = True
            tbAmount1Plafond.Enabled = True
        End If
    End Sub
End Class
