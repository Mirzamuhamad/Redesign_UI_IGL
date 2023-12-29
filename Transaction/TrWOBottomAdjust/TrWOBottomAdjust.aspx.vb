Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class TrWOBottomAdjust
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLWOAdjustHd"

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

                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT TransNmbr) FROM V_PLWOPlanHd ", ViewState("DBConnection").ToString)
                If Not Request.QueryString("transid") Is Nothing Then
                    If Request.QueryString("transid").ToString.Length > 1 Then
                        'lbStatus.Text = Request.QueryString("transid").ToString
                        'Exit Sub
                        ddlRange.SelectedValue = "0"
                        'CurrFilter = tbFilter.Text
                        'Value = ddlField.SelectedValue
                        tbFilter.Text = Request.QueryString("transid").ToString
                        ddlField.SelectedValue = "TransNmbr"
                        btnSearch_Click(Nothing, Nothing)
                        'tbFilter.Text = CurrFilter
                        'ddlField.SelectedValue = Value
                    End If
                End If

            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            'hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnWo" Then
                    tbWoNo.Text = Session("Result")(0).ToString
                    FillCombo(ddlDivision, "SELECT DISTINCT Divisi, Divisiname FROM V_PLWOPlanHd WHERE TransNmbr =  " + QuotedStr(Session("Result")(0).ToString), False, "Divisi", "Divisiname", ViewState("DBConnection"))
                    BindToDropList(ddlDivision, Session("Result")(1).ToString)
                    FillCombo(ddlWorkBy, "SELECT DISTINCT WorkBy, WorkByName FROM V_PLWOPlanHd WHERE TransNmbr =  " + QuotedStr(Session("Result")(0).ToString), False, "WorkBy", "WorkByName", ViewState("DBConnection"))
                    BindToDropList(ddlWorkBy, Session("Result")(2).ToString)
                    'FillCombo(ddlJob, "SELECT DISTINCT JobPlant, JobPlantName FROM V_PLWOPlanHd WHERE TransNmbr =  " + QuotedStr(Session("Result")(0).ToString), False, "JobPlant", "JobPlantName", ViewState("DBConnection"))
                    'BindToDropList(ddlJob, Session("Result")(3).ToString)
                    tbQtyHd.Text = Session("Result")(4).ToString
                    tbUnitHd.Text = Session("Result")(5).ToString
                    tbRemark.Text = Session("Result")(6).ToString
                End If

                If ViewState("Sender") = "btnDt" Then
                    tbType.Text = Session("Result")(0).ToString
                    tbDivisiCode.Text = Session("Result")(1).ToString
                    tbDivisiName.Text = Session("Result")(2).ToString
                    tbStartDate.SelectedDate = Session("Result")(3).ToString
                    tbEndDate.SelectedDate = Session("Result")(4).ToString
                    tbQty.Text = Session("Result")(5).ToString
                    tbWorkBy.Text = Session("Result")(17).ToString
                    tbSplit.Text = Session("Result")(18).ToString
                    tbPerson.Text = Session("Result")(11).ToString
                    tbCapacity.Text = Session("Result")(7).ToString
                    tbWorkDay.Text = Session("Result")(8).ToString
                    tbKontraktor.Text = Session("Result")(14).ToString
                    ddlFg.SelectedValue = Session("Result")(13).ToString
                End If


                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            BindToText(tbWoNo, drResult("TransNmbr").ToString)
                            BindToDropList(ddlDivision, drResult("Divisi").ToString)
                            BindToDropList(ddlWorkBy, drResult("WorkBy").ToString)
                            BindToDropList(ddlJob, drResult("JobPlant").ToString)
                            BindToText(tbQtyHd, drResult("Qty").ToString)
                            BindToText(tbUnitHd, drResult("Unit").ToString)
                        End If

                        If CekExistData(ViewState("Dt"), "DivisiBlok", drResult("DivisiBlok")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Type") = drResult("Type")
                            dr("DivisiBlok") = drResult("DivisiBlok")
                            dr("SplitNmbr") = drResult("QtyBlok")
                            dr("WorkBy") = drResult("WorkBy")
                            dr("Person") = drResult("Person")
                            If drResult("Qty") > 0 Then
                                dr("Qty") = drResult("Qty").ToString.Replace(", ", "")
                            End If
                            dr("NormaHK") = drResult("NormaHK")
                            dr("WorkDay") = drResult("WorkDay")
                            dr("StartDate") = drResult("StartDate")
                            dr("EndDate") = drResult("EndDate")
                            dr("Supplier") = drResult("Supplier")
                            dr("FgBorongan") = drResult("FgBorongan")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)

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
        FillCombo(ddlWorkBy, "EXEC S_GetTeam ", True, "Team_Code", "Team_Name", ViewState("DBConnection"))
        FillCombo(ddlDivision, "EXEC S_GetTeamNew ", True, "Division", "DivisionName", ViewState("DBConnection"))
        FillCombo(ddlJob, "EXEC S_GetJob ", True, "JobCode", "JobName", ViewState("DBConnection"))
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Kalo ada java script
        'tbQty.Attributes.Add("ReadOnly", "True")
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQtyOrder.Attributes.Add("OnBlur", "kurang(" + tbQtyOrder.ClientID + "," + tbQty.ClientID + "); setformat();")
        'Me.tbQty.Attributes.Add("OnBlur", "kurang(" + tbQtyOrder.ClientID + "," + tbQty.ClientID + "); setformat();")
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
        Return "SELECT * From V_PLWOAdjustDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
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
                Session("SelectCommand") = "EXEC S_PLWOAdjustFormNew " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub

                Session("ReportFile") = ".../../../Rpt/FormWoAdjust.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PLWOAdjust", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    'Private Sub EnableHd(ByVal State As Boolean)
    '    Try
    '        tbSuppCode.Enabled = State
    '        btnSupp.Visible = State
    '        btnPO.Visible = State
    '        btnGetDt.Visible = State
    '        ddlWrhs.Enabled = State
    '        'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
    '        tbSubled.Enabled = State And tbFgSubled.Text.Trim <> "N"
    '        btnSubled.Visible = tbSubled.Enabled
    '    Catch ex As Exception
    '        Throw New Exception("Enable Hd Error " + ex.ToString)
    '    End Try
    'End Sub

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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbTransNo.Text = GetAutoNmbr("WOA", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLWOAdjustHd (TransNmbr,Status,TransDate,WONo, JobPlant, WorkBy, Qty, Unit, Remark, Divisi,Section, UserPrep , DatePrep, UserAppr, DateAppr) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbWoNo.Text) + ", " + QuotedStr(ddlJob.SelectedValue) + ", " + QuotedStr(ddlWorkBy.SelectedValue) + ", " + _
                QuotedStr(tbQtyHd.Text) + ", " + QuotedStr(tbUnitHd.Text) + ", " + QuotedStr(tbRemark.Text) + "," + QuotedStr(ddlDivision.SelectedValue) + ",'Blok'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLWOAdjustHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLWOAdjustHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", WONo = " + QuotedStr(tbWoNo.Text) + _
                ", JobPlant = " + QuotedStr(ddlJob.SelectedValue) + ", WorkBy = " + QuotedStr(ddlWorkBy.SelectedValue) + _
                ", Qty = " + QuotedStr(tbQtyHd.Text) + ", Unit = " + QuotedStr(tbUnitHd.Text) + _
                ", Divisi = " + QuotedStr(ddlDivision.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " DateAppr = getDate()" + _
                " WHERE TransNmbr = '" + tbTransNo.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Type, DivisiBlok, SplitNmbr, WorkBy, StartDate, EndDate, Qty, FgBorongan, Supplier, NormaHK, Person, WorkDay FROM PLWOAdjustDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLWOAdjustDt SET Type = @Type, DivisiBlok = @DivisiBlok,SplitNmbr = @SplitNmbr,StartDate = @StartDate,EndDate = @EndDate, Qty = @Qty, NormaHK = @NormaHK, Person = @Person, WorkDay = @WorkDay, FgBorongan = @FgBorongan, Supplier = @Supplier,  " + _
                    " WHERE TransNmbr = '" & ViewState("Reference") & "' AND DivisiBlok = @OldDivisiBlok", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Type", SqlDbType.VarChar, 8, "Type")
            Update_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            Update_Command.Parameters.Add("@SplitNmbr", SqlDbType.Int, 4, "SplitNmbr")
            Update_Command.Parameters.Add("@StartDate", SqlDbType.DateTime, 8, "StartDate")
            Update_Command.Parameters.Add("@EndDate", SqlDbType.DateTime, 8, "EndDate")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 9, "Qty")
            Update_Command.Parameters.Add("@NormaHK", SqlDbType.Float, 9, "NormaHK")
            Update_Command.Parameters.Add("@Person", SqlDbType.Int, 4, "Person")
            Update_Command.Parameters.Add("@WorkDay", SqlDbType.VarChar, 5, "WorkDay")
            Update_Command.Parameters.Add("@FgBorongan", SqlDbType.VarChar, 1, "FgBorongan")
            Update_Command.Parameters.Add("@Supplier", SqlDbType.VarChar, 12, "Supplier")

            
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldDivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLWOAdjustDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND DivisiBlok = @DivisiBlok ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLWOAdjustDt")

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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "DivisiBlok") = False Then
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbWoNo.Text = ""
            'ddlReport.SelectedValue = "N"
            'ddlJob.SelectedValue = ""
            'ddlWorkBy.SelectedValue = ""
            'ddlDivision.SelectedValue = ""
            tbQtyHd.Text = ""
            tbUnitHd.Text = ""
            tbRemark.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbType.Text = ""
            tbDivisiCode.Text = ""
            tbDivisiName.Text = ""
            tbSplit.Text = ""
            tbPerson.Text = ""
            tbQty.Text = "0"
            tbCapacity.Text = ""
            tbWorkBy.Text = ""
            tbWorkDay.Text = ""
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Today
            tbKontraktor.Text = ""
            'ddlFg.SelectedValue = ""

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
                If CekDt(dr, "DIvisiBlok") = False Then
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
        'EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbWoNo.Text = "" Then
                lbStatus.Text = MessageDlg("Wo No must have value")
                tbWoNo.Focus()
                Return False
            End If
            If ddlJob.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Job must have value")
                ddlJob.Focus()
                Return False
            End If
            If ddlWorkBy.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Work By must have value")
                ddlWorkBy.Focus()
                Return False
            End If
            If tbQtyHd.Text = "" Then
                lbStatus.Text = MessageDlg("Qty must have value")
                tbQtyHd.Focus()
                Return False
            End If
            If ddlDivision.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlDivision.Focus()
                Return False
            End If
            If tbUnitHd.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Unit must have value")
                tbUnitHd.Focus()
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
                If Dr("Type").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("divisiblok").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Actual Must Have Value")
                    Return False
                End If
            Else
                If tbType.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Type Must Have Value")
                    tbType.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Wrhs Must Have Value")
                    tbQty.Focus()
                    Return False
                End If

                'If r.IsMatch(tbSplit.Text) = False Then
                '    lbStatus.Text = "Please enter Number in to Split"
                '    tbSplit.Focus()
                '    Exit Function
                'End If


                If tbDivisiCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Divisi Wrhs Must Have Value")
                    tbDivisiCode.Focus()
                    Return False
                End If
                If tbWorkBy.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("WorkBy Wrhs Must Have Value")
                    tbWorkBy.Focus()
                    Return False
                End If
                If tbWorkDay.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("WorkDay Wrhs Must Have Value")
                    tbWorkDay.Focus()
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
            FilterName = "Reference, Date, Status, Wo No, Job Plant, Work By, Qty, Unit, Divisi, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, WoNo, JobPlant, WorkBy, Qty, Unit, Divisi, Section, Remark"
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
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("SelectCommand") = "EXEC S_PLWOAdjustFormNew" + "'" + QuotedStr(GVR.Cells(2).Text) + "''"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormWoAdjust.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "REJECT RR|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'Reject RR' "
                        Dim dt As DataTable
                        dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                        If dt.Rows.Count > 0 Then
                            Dim i As Integer
                            Dim pageurl As String
                            pageurl = HttpContext.Current.Request.Url.AbsoluteUri
                            i = pageurl.IndexOf("&transid")
                            If i > 0 Then
                                pageurl = pageurl.Substring(0, i)
                            End If
                            'lbStatus.Text = "***" + pageurl + "****"
                            'Exit Sub
                            Session("PrevPageStock") = pageurl  'HttpContext.Current.Request.Url.AbsoluteUri
                            AttachScript("OpenTransactionSelf('TrStockLot', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                        Else
                            lbStatus.Text = "Transaction does not need input Lot No"
                        End If
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
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            'EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbType.Focus()
            'btnGetDt.Enabled = False
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
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbWoNo, Dt.Rows(0)("WONo").ToString)
            BindToDropList(ddlJob, Dt.Rows(0)("Jobplant").ToString)
            BindToDropList(ddlWorkBy, Dt.Rows(0)("WorkBy").ToString)
            BindToDropList(ddlDivision, Dt.Rows(0)("Divisi").ToString)
            BindToText(tbQtyHd, Dt.Rows(0)("Qty").ToString)
            BindToText(tbUnitHd, Dt.Rows(0)("Unit").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)


            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Type = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbType, Dr(0)("Type").ToString)
                BindToText(tbDivisiCode, Dr(0)("DivisiBlok").ToString)
                BindToText(tbDivisiName, Dr(0)("BlockName").ToString)
                BindToText(tbSplit, Dr(0)("SplitNmbr").ToString)
                BindToText(tbWorkBy, Dr(0)("WorkBy").ToString)
                BindToText(tbPerson, Dr(0)("Person").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbCapacity, Dr(0)("NormaHK").ToString)
                BindToText(tbWorkDay, Dr(0)("WorkDay").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbKontraktor, Dr(0)("NormaHK").ToString)
                BindToDropList(ddlFg, Dr(0)("FgBorongan").ToString)

            End If
            'Dt = BindDataTransaction(GetStringDt(tbTransNo.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
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
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbDivisiCode.Text Then
                    If CekExistData(ViewState("Dt"), "DivisiBlok", tbDivisiCode.Text) Then
                        lbStatus.Text = "DivisiBlok '" + tbDivisiName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Type") = tbType.Text
                Row("DivisiBlok") = tbDivisiCode.Text
                Row("SplitNmbr") = tbSplit.Text
                Row("WorkBy") = tbWorkBy.Text
                Row("Person") = tbPerson.Text
                Row("Qty") = tbQty.Text
                Row("NormaHK") = tbCapacity.Text
                Row("WorkDay") = tbWorkDay.Text
                Row("StartDate") = tbStartDate.Text
                Row("EndDate") = tbEndDate.Text
                Row("Supplier") = tbKontraktor.Text
                Row("FgBorongan") = ddlFg.SelectedValue
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "DivisiBlok", tbDivisiCode.Text) Then
                    lbStatus.Text = "DivisiBlok '" + tbDivisiName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Type") = tbType.Text
                dr("DivisiBlok") = tbDivisiCode.Text
                dr("SplitNmbr") = tbSplit.Text
                dr("WorkBy") = tbWorkBy.Text
                dr("Person") = tbPerson.Text
                dr("Qty") = tbQty.Text
                dr("NormaHK") = tbCapacity.Text
                dr("WorkDay") = tbWorkDay.Text
                dr("StartDate") = tbStartDate.Text
                dr("EndDate") = tbEndDate.Text
                dr("Supplier") = tbKontraktor.Text
                dr("FgBorongan") = ddlFg.SelectedValue
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnWo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnWo.Click
        Dim ResultField As String
        Try

            Session("filter") = "SELECT TransNmbr, TransDate, Divisi,DivisiName, WorkBy, WorkByName, Type, JobPlant, jobPlantName, Supplier, SupplierName, QtyWeek, Qty, Unit, EstStartWeek, EstEndWeek, Remark FROM V_PLWOPlanHd"
            ResultField = "TransNmbr, DivisiName, WorkByName, JobPlantName, Qty, Unit, Remark"
            ViewState("Sender") = "btnWo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDt.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi FROM V_PLWOPlanWoAdjust"
            ResultField = "Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi"
            CriteriaField = "Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi"
            ViewState("Sender") = "btnDt"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    'Protected Sub btnPO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPO.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        If tbSuppCode.Text = "" Then
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "Select * from V_STRejectRRGetSJ "
    '            Else
    '                Session("filter") = "Select * from V_STRejectRRGetSJ WHERE Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
    '            End If
    '        Else
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "Select * from V_STRejectRRGetSJ Where Supplier_Code = " + QuotedStr(tbSuppCode.Text)
    '            Else
    '                Session("filter") = "Select * from V_STRejectRRGetSJ WHERE Supplier_Code = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
    '            End If
    '        End If
    '        ResultField = "SJ_No, Supplier_Code, Supplier_Name, Attn, Report, Warehouse"
    '        ViewState("Sender") = "btnPO"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn PO Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbSuppCode.Text = Dr("Supplier_Code")
    '            tbSuppName.Text = Dr("Supplier_Name")
    '            tbAttn.Text = Dr("Contact_Person")
    '            tbReturNo.Text = ""
    '        Else
    '            tbSuppCode.Text = ""
    '            tbSuppName.Text = ""
    '            tbAttn.Text = ""
    '            tbReturNo.Text = ""
    '        End If
    '        tbReturNo.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb Supp Code Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT TransNmbr, Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi FROM V_PLWOPlanWoAdjust"
            ResultField = "TransNmbr, Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi"
            CriteriaField = "TransNmbr, Type, DivisiBlok,DivisiBlokName, StartDate, EndDate, Qty, NormaHK, WorkDay, Percentage, FgBorongan, JobPlant, Person, Remark, Section, Supplier, Unit, WorkBy, QtyBlok, Divisi"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "TransNmbr"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search PO No Error : " + ex.ToString
        End Try
    End Sub

End Class
