Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization

Partial Class Transaction_TrRDFormulaUji_TrRDFormulaUji
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_RDFormulaUjiHd"

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
                If ViewState("Sender") = "btnSample" Then
                    BindToText(tbSample, Session("Result")(0).ToString)
                    BindToText(tbSampleName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustomer, Session("Result")(0).ToString)
                    BindToText(tbCustomerName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnTrialNo" Then
                    tbTrialNo.Text = Session("Result")(0).ToString
                    BindToText(tbSample, Session("Result")(1).ToString)
                    BindToText(tbSampleName, Session("Result")(2).ToString)
                    BindToText(tbCustomer, Session("Result")(3).ToString)
                    BindToText(tbCustomerName, Session("Result")(4).ToString)
                    BindToDropList(ddlUserType, Session("Result")(5).ToString)
                End If
                If ViewState("Sender") = "btnCriteria" Then
                    BindToText(tbCriteria, Session("Result")(0).ToString)
                    BindToText(tbCriteriaName, Session("Result")(1).ToString)
                    BindToText(tbStandard, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("XTime+'|'+TimeType+'|'+Criteria = " + QuotedStr(tbXtimeHd.Text + "|" + ddlTimeTypeHd.SelectedValue + "|" + drResult("CriteriaCode")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("XTime") = tbXtimeHd.Text
                            dr("TimeType") = ddlTimeTypeHd.SelectedValue
                            dr("Criteria") = drResult("CriteriaCode")
                            dr("CriteriaName") = drResult("CriteriaName")
                            dr("Standard") = drResult("Standard")
                            dr("Result") = ""
                            dr("Conclusion") = "OK"
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
        Try
            FillRange(ddlRange)
            'FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbXTime.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbXtimeHd.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbStart.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEnd.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbLength.Attributes.Add("OnBlur", "setformat();")
            'Me.tbQtyFormulasi.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * From V_RDFormulaUjiDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            GridView1.PageSize = ddlShowRecord.SelectedValue
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

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_RDFormulaUji", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDate.Enabled = State
            tbSample.Enabled = State
            btnSample.Visible = State
            tbCustomer.Enabled = State
            btnCustomer.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr) + " Order By ItemNo, TimeType DESC, Criteria ", ViewState("DBConnection").ToString).Tables(0)
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
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTrialNo.Text = ""
            tbSample.Text = ""
            tbSampleName.Text = ""

            ddlUserType.SelectedIndex = 0
            tbCustomer.Text = ""
            tbCustomerName.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbXTime.Text = "0"
            ddlTimeType.SelectedIndex = 0
            tbCriteria.Text = ""
            tbCriteriaName.Text = ""
            tbStandard.Text = ""
            ddlConclusion.SelectedIndex = 0
            tbStart.Text = "0"
            tbEnd.Text = "0"
            tbResult.Text = ""
            tbResultgr.Text = "0"
            tbResultpercen.Text = "0"
            tbRemarkDt.Text = ""
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
            If tbSample.Text = "" Then
                lbStatus.Text = MessageDlg("Sample must have value")
                tbSample.Focus()
                Return False
            End If
            If tbCustomerName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustomer.Focus()
                Return False
            End If
            If tbStartDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Delivery Date must have value")
                tbStartDate.Focus()
                Return False
            End If
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
                If CFloat(Dr("XTime").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("X Time Must Have Value")
                    Return False
                End If
                If Dr("Criteria").ToString = "" Then
                    lbStatus.Text = MessageDlg("Criteria Must Have Value")
                    Return False
                End If
                'If Dr("StartDate").Then Then
                '    lbStatus.Text = MessageDlg("Start Date Must Have Value")
                '    Return False
                'End If
                'If Dr("EndDate").Then Then
                '    lbStatus.Text = MessageDlg("End Date Must Have Value")
                '    Return False
                'End If                
            Else
                If CFloat(tbXTime.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("X Time Must Have Value")
                    tbXTime.Focus()
                    Return False
                End If
                If tbCriteriaName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Criteria Must Have Value")
                    tbCriteria.Focus()
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
            BindToText(tbTrialNo, Dt.Rows(0)("FormulaTrialNo").ToString)
            BindToText(tbSample, Dt.Rows(0)("Sample").ToString)
            BindToText(tbSampleName, Dt.Rows(0)("SampleName").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbCustomer, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustomerName, Dt.Rows(0)("CustomerName").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbLocation, Dt.Rows(0)("Location").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("XTime+'|'+TimeType+'|'+Criteria = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbXTime, Dr(0)("XTime").ToString)
                BindToDropList(ddlTimeType, Dr(0)("TimeType").ToString)
                BindToText(tbCriteria, Dr(0)("Criteria").ToString)
                BindToText(tbCriteriaName, Dr(0)("CriteriaName").ToString)
                BindToText(tbStandard, Dr(0)("Standard").ToString)
                BindToText(tbResult, Dr(0)("Result").ToString)
                BindToDropList(ddlConclusion, Dr(0)("Conclusion").ToString)
                BindToText(tbStart, Dr(0)("WeightStart").ToString)
                BindToText(tbEnd, Dr(0)("WeightEnd").ToString)
                BindToText(tbResultgr, Dr(0)("Resultgr").ToString)
                BindToText(tbResultpercen, Dr(0)("Resultpercen").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbXTime.Text.Trim + "|" + ddlTimeType.SelectedValue + "|" + tbCriteria.Text Then
                    If CekExistData(ViewState("Dt"), "XTime,TimeType,Criteria", tbXTime.Text.Trim + "|" + ddlTimeType.SelectedValue + "|" + tbCriteria.Text) Then
                        lbStatus.Text = tbXTime.Text + " " + ddlTimeType.SelectedValue + " Criteria Sample " + tbCriteriaName.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("XTime+'|'+TimeType+'|'+Criteria = " + QuotedStr(tbXTime.Text.Trim + "|" + ddlTimeType.SelectedValue + "|" + tbCriteria.Text))(0)
                Row.BeginEdit()
                Row("XTime") = tbXTime.Text
                Row("TimeType") = ddlTimeType.SelectedValue
                Row("Criteria") = tbCriteria.Text
                Row("CriteriaName") = tbCriteriaName.Text
                Row("Standard") = tbStandard.Text
                Row("Result") = tbResult.Text
                Row("Conclusion") = ddlConclusion.Text
                Row("WeightStart") = FormatNumber(tbStart.Text, ViewState("DigitQty"))
                Row("WeightEnd") = FormatNumber(tbEnd.Text, ViewState("DigitQty"))
                Row("Resultgr") = FormatNumber(tbResultgr.Text, ViewState("DigitQty"))
                Row("Resultpercen") = FormatNumber(tbResultpercen.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt"), "XTime,TimeType,Criteria", tbXTime.Text.Trim + "|" + ddlTimeType.SelectedValue + "|" + tbCriteria.Text) Then
                    lbStatus.Text = tbXTime.Text + " " + ddlTimeType.SelectedValue + " Criteria Sample " + tbCriteriaName.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("XTime") = tbXTime.Text
                dr("TimeType") = ddlTimeType.SelectedValue
                dr("Criteria") = tbCriteria.Text
                dr("CriteriaName") = tbCriteriaName.Text
                dr("Standard") = tbStandard.Text
                dr("Result") = tbResult.Text
                dr("Conclusion") = ddlConclusion.Text
                dr("WeightStart") = FormatNumber(tbStart.Text, ViewState("DigitQty"))
                dr("WeightEnd") = FormatNumber(tbEnd.Text, ViewState("DigitQty"))
                dr("Resultgr") = FormatNumber(tbResultgr.Text, ViewState("DigitQty"))
                dr("Resultpercen") = FormatNumber(tbResultpercen.Text, ViewState("DigitQty"))
                dr("Remark") = tbRemarkDt.Text
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
                tbCode.Text = GetAutoNmbr("RDFUJ", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO RDFormulaUjiHd (TransNmbr, TransDate, STATUS, Sample, Customer, FormulaTrialNo, StartDate, EndDate, Location, Remark, UserPrep, DatePrep, UserType ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbSample.Text) + "," + QuotedStr(tbCustomer.Text) + ", " + QuotedStr(tbTrialNo.Text) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbLocation.Text) + ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr(ddlUserType.SelectedValue)
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM RDFormulaUjiHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE RDFormulaUjiHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                            ", Sample = " + QuotedStr(tbSample.Text) + _
                            ", Customer = " + QuotedStr(tbCustomer.Text) + _
                            ", FormulaTrialNo = " + QuotedStr(tbTrialNo.Text) + _
                            ", StartDate = '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + _
                            "', EndDate = '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + _
                            "', Location = " + QuotedStr(tbLocation.Text) + _
                            ", Remark = " + QuotedStr(tbRemark.Text) + _
                            ", UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                            ", DatePrep = GetDate()" + _
                            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
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
            Dim cmdSql As New SqlCommand("SELECT  TransNmbr, XTime, TimeType, Criteria, Standard, Result, Conclusion, Remark, WeightStart, WeightEnd  FROM RDFormulaUjiDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE RDFormulaUjiDt SET XTime = @XTime, TimeType = @TimeType, Criteria = @Criteria, Standard = @Standard, Result = @Result, Conclusion = @Conclusion, Remark = @Remark,  " + _
                    "WeightStart = @WeightStart, WeightEnd = @WeightEnd " + _
                    "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND XTime = @OldXTime AND TimeType = @OldTimeType ", con)

            Update_Command.Parameters.Add("@XTime", SqlDbType.VarChar, 5, "XTime")
            Update_Command.Parameters.Add("@TimeType", SqlDbType.VarChar, 12, "TimeType")
            Update_Command.Parameters.Add("@Criteria", SqlDbType.VarChar, 5, "Criteria")
            Update_Command.Parameters.Add("@Standard", SqlDbType.VarChar, 100, "Standard")
            Update_Command.Parameters.Add("@Result", SqlDbType.VarChar, 100, "Result")
            Update_Command.Parameters.Add("@Conclusion", SqlDbType.VarChar, 8, "Conclusion")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@WeightStart", SqlDbType.Float, 18, "WeightStart")
            Update_Command.Parameters.Add("@WeightEnd", SqlDbType.Float, 18, "WeightEnd")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldXTime", SqlDbType.VarChar, 5, "XTime")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldTimeType", SqlDbType.VarChar, 12, "TimeType")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM RDFormulaUjiDt WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND XTime = @XTime AND TimeType = @TimeType ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@XTime", SqlDbType.VarChar, 5, "XTime")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@TimeType", SqlDbType.VarChar, 12, "TimeType")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("RDFormulaUjiDt")

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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbCriteria.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
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
            FDateName = "Trial Date"
            FDateValue = "TransDate"
            FilterName = "Formula Trial No, Formula Trial Date, Sample, Customer, Start Date, End Date, Location, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), SampleName, CustomerName, dbo.FormatDate(StartDate), dbo.FormatDate(EndDate), Location, Remark"
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
                        Session("SelectCommand") = "EXEC S_TrRDFormulaUjiForm ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormTrRDFormulaUji.frx"
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
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("XTime+'|'+TimeType+'|'+Criteria = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
            For Each r In dr
                r.Delete()
            Next
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
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomer.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT * FROM VMsCustomer Where FgActive = 'Y' "
            'ResultField = "Customer_Code, Customer_Name"
            Session("filter") = "SELECT User_Code, User_Name, Contact_Person FROM VMsUserType WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCustomer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustomer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustomer.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT Customer_Code, Customer_Name FROM VMsCustomer WHERE FgActive = 'Y' AND Customer_Code = " + QuotedStr(tbCustomer.Text), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count > 0 Then
            Dr = FindMaster("UserType", tbCustomer.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbCustomer.Text = Dr("User_Code")
                tbCustomerName.Text = Dr("User_Name")
            Else
                tbCustomer.Text = ""
                tbCustomerName.Text = ""
            End If
            tbCustomer.Focus()
        Catch ex As Exception
            Throw New Exception("tbCustomer change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCriteria.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select CriteriaCode, CriteriaName, Standard from MsSampleCriteria WHERE SampleCode = " + QuotedStr(tbSample.Text)
            ResultField = "CriteriaCode, CriteriaName, Standard"
            ViewState("Sender") = "btnCriteria"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCriteria Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSample_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSample.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select SampleCode, SampleName from MsSample "
            ResultField = "SampleCode, SampleName"
            ViewState("Sender") = "btnSample"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnSample_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSample_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSample.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT SampleCode, SampleName FROM MsSample WHERE SampleCode = " + QuotedStr(tbSample.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbSample.Text = Dr("SampleCode")
                tbSampleName.Text = Dr("SampleName")
            Else
                tbSample.Text = ""
                tbSampleName.Text = ""
            End If
            tbSample.Focus()
        Catch ex As Exception
            Throw New Exception("tbSample_TextChanged change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCriteria_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCriteria.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select CriteriaCode, CriteriaName, Standard from MsSampleCriteria WHERE SampleCode = " + QuotedStr(tbSample.Text) + " AND CriteriaCode = " + QuotedStr(tbCriteria.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCriteria.Text = Dr("CriteriaCode")
                tbCriteriaName.Text = Dr("CriteriaName")
                tbStandard.Text = Dr("Standard")
            Else
                tbCriteria.Text = ""
                tbCriteriaName.Text = ""
                tbStandard.Text = ""
            End If
            tbCriteria.Focus()
        Catch ex As Exception
            Throw New Exception("tbCriteria_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnTrialNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTrialNo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            If tbSample.Text = "" And tbCustomer.Text = "" Then
                Session("filter") = "Select Trial_No, dbo.FormatDate(Trial_Date) AS Trial_Date, Sample, SampleName, UserType, UserCode, UserName, dbo.FormatDate(DeliveryDate) AS Delivery_Date, Remark from V_RDFormulaUjiReff "
            ElseIf tbSample.Text = "" And tbCustomer.Text <> "" Then
                Session("filter") = "Select Trial_No, dbo.FormatDate(Trial_Date) AS Trial_Date, Sample, SampleName, UserType, UserCode, UserName, dbo.FormatDate(DeliveryDate) AS Delivery_Date, Remark from V_RDFormulaUjiReff WHERE UserCode = " + QuotedStr(tbCustomer.Text)
            ElseIf tbSample.Text <> "" And tbCustomer.Text = "" Then
                Session("filter") = "Select Trial_No, dbo.FormatDate(Trial_Date) AS Trial_Date, Sample, SampleName, UserType, UserCode, UserName, dbo.FormatDate(DeliveryDate) AS Delivery_Date, Remark from V_RDFormulaUjiReff WHERE Sample = " + QuotedStr(tbSample.Text)
            Else
                Session("filter") = "Select Trial_No, dbo.FormatDate(Trial_Date) AS Trial_Date, Sample, SampleName, UserType, UserCode, UserName, dbo.FormatDate(DeliveryDate) AS Delivery_Date, Remark from V_RDFormulaUjiReff WHERE UserCode = " + QuotedStr(tbCustomer.Text) + " and Sample = " + QuotedStr(tbSample.Text)
            End If
            ResultField = "Trial_No, Sample, SampleName, UserCode, UserName, UserType"
            ViewState("Sender") = "btnTrialNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn ConfirmNo Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField As String
        Try
            If CFloat(tbXtimeHd.Text) <= 0 Then
                lbStatus.Text = "Time period must have value"
                tbXtimeHd.Focus()
                Exit Sub
            End If
            If tbSample.Text = "" Then
                lbStatus.Text = "Sample must have value"
                tbSample.Focus()
                Exit Sub
            End If
            If tbTrialNo.Text = "" Then
                lbStatus.Text = "Formulir Trial No must have value"
                tbTrialNo.Focus()
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "Select CriteriaCode, CriteriaName, Standard from MsSampleCriteria WHERE SampleCode = " + QuotedStr(tbSample.Text)
            ResultField = "CriteriaCode, CriteriaName, Standard"
            ViewState("Sender") = "btnGetData"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetData_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbCustomer.Text = ""
        tbCustomerName.Text = ""
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub tbEnd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEnd.TextChanged
        Try
            If tbEnd.Text.Trim = "" Then
                tbEnd.Text = "0"
            End If

            tbResultgr.Text = FormatFloat(tbStart.Text - tbEnd.Text, ViewState("DigitQty"))
            If CFloat(tbResultgr.Text) = 0 Then
                tbResultpercen.Text = CFloat("0")
            Else
                tbResultpercen.Text = (tbResultgr.Text / tbStart.Text) * 100
            End If
            tbResultpercen.Text = FormatNumber(tbResultpercen.Text, ViewState("DigitQty"))
            tbRemarkDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbEnd_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStart.TextChanged
        Try
            If tbStart.Text.Trim = "" Then
                tbStart.Text = "0"
            End If
            tbResultgr.Text = FormatFloat(tbStart.Text - tbEnd.Text, ViewState("DigitQty"))
            If CFloat(tbResultgr.Text) = 0 Then
                tbResultpercen.Text = 0
            Else
                tbResultpercen.Text = (tbResultgr.Text / tbStart.Text) * 100
            End If
            tbResultpercen.Text = FormatNumber(tbResultpercen.Text, ViewState("DigitQty"))
            tbEnd.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbStart_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
