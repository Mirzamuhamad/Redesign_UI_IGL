Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPEMedicalClaim
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PEMedicalClaim"
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
                If ViewState("Sender") = "btnEmployee" Then
                    tbEmployee.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlJobLevel, Session("Result")(2).ToString)
                    BindToDropList(ddlJobTitle, Session("Result")(3).ToString)
                    BindToDropList(ddlEmpStatus, Session("Result")(4).ToString)
                    tbJenisKelamin.Text = Session("Result")(5).ToString
                End If

                If ViewState("Sender") = "btnFamily" Then
                    tbFamilyType.Text = Session("Result")(0).ToString
                    tbFamilyName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnMedical" Then
                    tbMedicalCode.Text = Session("Result")(0).ToString
                    tbMedicalName.Text = Session("Result")(1).ToString
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
        FillCombo(ddlDept, "EXEC S_GetDepartment", True, "Department_Code", "Department_Name", ViewState("DBConnection"))
        FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
        FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))
        FillCombo(ddlCurrency, "EXEC S_GetCurrency", True, "Currency", "Currency_Name", ViewState("DBConnection"))

        'FillCombo(ddlUnitReq, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
        'FillCombo(ddlUnitWrhs, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

        'tbAmountClaim.Attributes.Add("ReadOnly", "True")
        'tbAmountPaid.Attributes.Add("ReadOnly", "True")
        'tbAmountPercent.Attributes.Add("ReadOnly", "True")
        'tbAmountToPaid.Attributes.Add("ReadOnly", "True")

        
        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmountClaim.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmountPaid.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmountPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmountToPaid.Attributes.Add("OnKeyDown", "return PressNumeric();")


        tbAmountClaim.Attributes.Add("OnBlur", "setformat();")
        tbAmountPaid.Attributes.Add("OnBlur", "setformat();")
        tbAmountPercent.Attributes.Add("OnBlur", "setformat();")
        tbAmountToPaid.Attributes.Add("OnBlur", "setformat();")
        
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
        Return "SELECT * From V_PEMedicalClaimDT WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormMedicalClaim " + Result + "," + QuotedStr(ViewState("UserId"))
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormMedicalClaim.frx"
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

                        Result = ExecSPCommandGo(ActionValue, "S_PEMedicalClaim", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PEMedicalClaim", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlJobTitle.Enabled = False
            'ddlJobLevel.Enabled = False
            'ddlEmpStatus.Enabled = False
            'btnFamily.Enabled = False
            'tbFamilyType.Enabled = False
            'tbFamilyName.Enabled = False
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
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbMedicalCode.Text Then
                    If CekExistData(ViewState("Dt"), "Medical", tbMedicalCode.Text) Then
                        lbStatus.Text = "Medical '" + tbMedicalName.Text(+"' has already exists")
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
                Row("ClaimType") = ddlClaim.Text
                Row("ReceiptNo") = tbReceiptNo.Text
                Row("ReceiptDate") = ReceiptDate.SelectedDate
                Row("AmountClaim") = FormatFloat(tbAmountClaim.Text.Replace(",", ""), ViewState("DigitCurr"))
                Row("PercentPaid") = FormatFloat(tbAmountPercent.Text.Replace(",", ""), ViewState("DigitPercent"))
                Row("AmountPaid") = FormatFloat(tbAmountPaid.Text.Replace(",", ""), ViewState("DigitCurr"))
                Row("AmountToBePaid") = FormatFloat(tbAmountToPaid.Text.Replace(",", ""), ViewState("DigitCurr"))
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Medical", tbMedicalCode.Text) Then
                    lbStatus.Text = "Medical '" + tbMedicalCode.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Medical") = tbMedicalCode.Text()
                dr("Medical_Name") = tbMedicalName.Text()
                dr("ClaimType") = ddlClaim.Text()
                dr("ReceiptNo") = tbReceiptNo.Text()
                dr("ReceiptDate") = ReceiptDate.SelectedDate()
                dr("AmountClaim") = FormatFloat(tbAmountClaim.Text.Replace(",", ""), ViewState("DigitCurr"))
                dr("PercentPaid") = FormatFloat(tbAmountPercent.Text.Replace(",", ""), ViewState("DigitPercent"))
                dr("AmountPaid") = FormatFloat(tbAmountPaid.Text.Replace(",", ""), ViewState("DigitCurr"))
                dr("AmountToBePaid") = FormatFloat(tbAmountToPaid.Text.Replace(",", ""), ViewState("DigitCurr"))
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
                tbTransNo.Text = GetAutoNmbr("MC", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PEMedicalClaimHD (TransNmbr, STATUS, Transdate, Department, EmpNumb, JobTitle, JobLevel," + _
                            "MedicalFor, FamilyType, FamilyName, CurrCode,ForexRate,TotalForex,Remark,UserPrep,DatePrep,FgPPhYear) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlDept.SelectedValue) + _
                "," + QuotedStr(tbEmployee.Text) + ", " + QuotedStr(ddlJobTitle.SelectedValue) + "," + QuotedStr(ddlJobLevel.SelectedValue) + _
                "," + QuotedStr(ddlMedicalFor.SelectedValue) + ", " + QuotedStr(tbFamilyType.Text) + ", " + QuotedStr(tbFamilyName.Text) + _
                "," + QuotedStr(ddlCurrency.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + tbTotalForex.Text.Replace(",", "") + _
                "," + QuotedStr(tbRemark.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate(),NULL"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEMedicalClaimhd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEMedicalClaimHD SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "',Department = " + QuotedStr(ddlDept.SelectedValue) + _
                ",EmpNumb = " + QuotedStr(tbEmployee.Text) + ", JobTitle = " + QuotedStr(ddlJobTitle.Text) + _
                ",JobLevel = " + QuotedStr(ddlJobLevel.SelectedValue) + ", MedicalFor = " + QuotedStr(ddlMedicalFor.SelectedValue) + _
                ",FamilyType = " + QuotedStr(tbFamilyType.Text) + ", FamilyName = " + QuotedStr(tbFamilyName.Text) + _
                ",CurrCode = " + QuotedStr(ddlCurrency.Text) + ",ForexRate = " + tbRate.Text.Replace(",", "") + _
                ",TotalForex = " + tbTotalForex.Text.Replace(",", "") + ",UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
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
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, Medical, ClaimType, ReceiptNo, ReceiptDate, AmountClaim, PercentPaid, AmountPaid, AmountTobePaid, " + _
                                         " Remark FROM PEMedicalClaimdt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            'lbStatus.Text = " SELECT TransNmbr, Medical, ClaimType, ReceiptNo, ReceiptDate, AmountClaim, PercentPaid, AmountPaid, AmountTobePaid, " + _
            '                " Remark FROM PEMedicalClaimdt WHERE TransNmbr = '" & ViewState("Reference") & "'"
            'Exit Sub

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("{PEMedicalClaimDt")

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
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlDept.SelectedValue = ""
            ddlJobTitle.SelectedValue = ""
            ddlJobLevel.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""
            ddlMedicalFor.SelectedIndex = 0
            ddlCurrency.SelectedValue = ViewState("Currency")
            tbRate.Enabled = False
            tbRate.Text = FormatFloat(1, ViewState("DigitRate"))
            tbEmployee.Text = ""
            tbEmpName.Text = ""
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitHome"))
            ViewState("DigitCurr") = ViewState("DigitHome")
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
            ddlClaim.SelectedIndex = 0
            tbRemarkDt.Text = ""
            tbReceiptNo.Text = ""
            ReceiptDate.SelectedDate = ViewState("ServerDate")
            tbAmountClaim.Text = "0"
            tbAmountPercent.Text = "0"
            tbAmountToPaid.Text = "0"
            tbAmountPaid.Text = "0"
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
            If tbEmployee.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee Must have value")
                tbEmployee.Focus()
                Return False
            End If
            If tbFamilyType.Enabled = True Then
                If tbFamilyType.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Family Type Must have value")
                    tbFamilyType.Focus()
                    Return False
                End If
            End If


            If ddlMedicalFor.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Medical For must have value")
                ddlMedicalFor.Focus()
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
                If Dr("ReceiptNo").ToString.Trim = "" Then
                    lbStatus.Text = "Receipt No. Must Have Value"
                    Return False
                End If
                If CFloat(Dr("AmountClaim").ToString) <= 0 Then
                    lbStatus.Text = "Amount Claim Must Have Value"
                    Return False
                End If
                If CFloat(Dr("PercentPaid").ToString) < 0 Then
                    lbStatus.Text = "Reimbursment Must Have Value"
                    Return False
                End If
                If CFloat(Dr("AmountPaid").ToString) < 0 Then
                    lbStatus.Text = "Amount Paid Must Have Value"
                    Return False
                End If
                If CFloat(Dr("AmountToBePaid").ToString) < 0 Then
                    lbStatus.Text = "Amount To Be Paid Must Have Value"
                    Return False
                End If
                If CFloat(Dr("AmountPaid").ToString) > CFloat(Dr("AmountToBePaid").ToString) Then
                    lbStatus.Text = "Amount Paid cannot greater than Amount to be Paid"""
                    Return False
                End If
            Else
                If tbMedicalCode.Text.Trim = "" Then
                    lbStatus.Text = "Medical Code Must Have Value"
                    tbMedicalCode.Focus()
                    Return False
                End If
                If tbReceiptNo.Text.Trim = "" Then
                    lbStatus.Text = "Receipt No. Must Have Value"
                    tbMedicalCode.Focus()
                    Return False
                End If
                If CFloat(tbAmountClaim.Text.Trim) <= 0 Then
                    lbStatus.Text = "Amount Claim Must Have Value"
                    tbAmountClaim.Focus()
                    Return False
                End If
                If CFloat(tbAmountPercent.Text.Trim) < 0 Then
                    lbStatus.Text = "Reimbursment Must Have Value"
                    tbAmountClaim.Focus()
                    Return False
                End If
                If CFloat(tbAmountToPaid.Text.Trim) < 0 Then
                    lbStatus.Text = "Amount To Be Paid Must Have Value"
                    tbAmountToPaid.Focus()
                    Return False
                End If
                If CFloat(tbAmountPaid.Text.Trim) < 0 Then
                    lbStatus.Text = "Amount Paid Must Have Value"
                    tbAmountPaid.Focus()
                    Return False
                End If
                If CFloat(tbAmountPaid.Text.Trim) > CFloat(tbAmountToPaid.Text) Then
                    lbStatus.Text = "Amount Paid cannot greater than Amount to be Paid"""
                    tbAmountPaid.Focus()
                    Return False
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
            FilterName = "Reference, Date, Request Type, Department, Request By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), RequestType, Department, RequestBy, Remark"
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
                        tbRate.Enabled = ddlCurrency.SelectedValue <> ViewState("Currency")
                        tbFamilyType.Enabled = ddlMedicalFor.SelectedValue = "Family"
                        btnFamily.Visible = ddlMedicalFor.SelectedValue = "Family"
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
                        Session("SelectCommand") = "EXEC S_PEFormMedicalClaim ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormMedicalClaim.frx"
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Dim AmountPaid As Decimal
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Medical")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))                    
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    AmountPaid = GetTotalSum(ViewState("Dt"), "AmountPaid")
                    tbTotalForex.Text = FormatFloat(AmountPaid, ViewState("DigitCurr"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try


    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Medical = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = tbMedicalCode.Text()
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
            BindToText(tbEmployee, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department_Code").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("JobTitleCode").ToString)
            BindToDropList(ddlJobLevel, Dt.Rows(0)("JobLevelCode").ToString)
            BindToDropList(ddlEmpStatus, Dt.Rows(0)("Emp_Status_Code").ToString)
            BindToDropList(ddlMedicalFor, Dt.Rows(0)("MedicalFor").ToString)
            BindToText(tbJenisKelamin, Dt.Rows(0)("Gender").ToString)
            If ddlMedicalFor.SelectedValue = "Employee" Then
                tbFamilyType.Text = ""
                tbFamilyName.Text = ""
                btnFamily.Enabled = False
            Else
                BindToText(tbFamilyType, Dt.Rows(0)("FamilyType").ToString)
                BindToText(tbFamilyType, Dt.Rows(0)("FamilyName").ToString)
                tbFamilyType.Enabled = True
            End If

            BindToDropList(ddlCurrency, Dt.Rows(0)("CurrCode").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                ViewState("DigitCurr") = ViewState("DigitHome")
            Else
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrency.SelectedValue), ViewState("DBConnection"))
            End If
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("RemarkHD").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        'Dim infois, peimen, cas As Double
        Try
            Dr = ViewState("Dt").select("Medical = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                'infois = 0
                'peimen = 0
                'cas = 0
                'For Each Dr In ViewState("Dt").Rows
                '    If Not Dr.RowState = DataRowState.Deleted Then
                '        infois = infois + CFloat(Dr("InvoiceHome").ToString)
                '        peimen = peimen + CFloat(Dr("ReceiptHome").ToString)
                '        cas = cas + CFloat(Dr("ChargeHome").ToString)
                '    End If
                'Next
                BindToText(tbMedicalCode, Dr(0)("Medical").ToString)
                BindToText(tbMedicalName, Dr(0)("Medical_Name").ToString)
                BindToDropList(ddlClaim, Dr(0)("ClaimType").ToString)
                BindToText(tbReceiptNo, Dr(0)("ReceiptNo").ToString)
                BindToDate(ReceiptDate, Dr(0)("ReceiptDate").ToString)
                BindToText(tbAmountClaim, Dr(0)("AmountClaim").ToString, ViewState("DigitHome"))
                BindToText(tbAmountPercent, Dr(0)("PercentPaid").ToString)
                BindToText(tbAmountPaid, Dr(0)("AmountPaid").ToString, ViewState("DigitHome"))
                BindToText(tbAmountToPaid, Dr(0)("AmountToBePaid").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                lbPrev.Text = tbAmountPaid.Text

                If tbAmountClaim.Text = "" Then
                    tbAmountClaim.Text = "0"
                End If
                If tbAmountPercent.Text = "" Then
                    tbAmountPercent.Text = "0"
                End If
                If tbAmountPaid.Text = "" Then
                    tbAmountPaid.Text = "0"
                End If
                If tbAmountToPaid.Text = "" Then
                    tbAmountToPaid.Text = "0"
                End If

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
    Protected Sub btnEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmployee.Click
        Dim ResultField As String
        Try

            Session("Filter") = " SELECT Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name, Emp_Status, Emp_Status_Name, Gender " + _
                                " FROM V_MsEmployee WHERE Department like '" + ddlDept.SelectedValue + "%'"
            ResultField = "Emp_No,Emp_Name,Job_Level,Job_Title,Emp_Status,Gender"
            ViewState("Sender") = "btnEmployee"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Employee Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnFamily_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFamily.Click
        Dim ResultField As String

        Try

            Session("filter") = "SELECT DISTINCT FamilyType,FamilyName FROM MsEmpFamily WHERE EmpNumb = " + QuotedStr(tbEmployee.Text)
            ResultField = "FamilyType,FamilyName"
            ViewState("Sender") = "btnFamily"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Family Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbEmployee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmployee.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try

            
            SQLString = " SELECT Emp_No, Emp_Name, Job_Level, Job_Title,  Emp_Status, Gender " + _
                        " FROM V_MsEmployee WHERE Department like '%" + ddlDept.SelectedValue + "%' AND Emp_No = " + QuotedStr(tbEmployee.Text)

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbEmployee, Dr("Emp_No").ToString)
                BindToText(tbEmpName, Dr("Emp_Name").ToString)
                BindToDropList(ddlJobLevel, Dr("Job_Level").ToString)
                BindToDropList(ddlJobTitle, Dr("Job_Title").ToString)
                BindToDropList(ddlEmpStatus, Dr("Emp_Status").ToString)
                BindToText(tbJenisKelamin, Dr("Gender").ToString)
            Else
                tbEmployee.Text = ""
                tbEmpName.Text = ""
                ddlJobLevel.SelectedIndex = 0
                ddlJobTitle.SelectedIndex = 0
                ddlEmpStatus.SelectedIndex = 0
                tbJenisKelamin.Text = ""
            End If
            tbFamilyType_TextChanged(Nothing, Nothing)
            tbEmployee.Focus()
        Catch ex As Exception
            Throw New Exception("tb Employee Code TextChanged : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbFamilyType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFamilyType.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try

            SQLString = "SELECT DISTINCT FamilyType,FamilyName FROM MsEmpFamily WHERE EmpNumb = " + QuotedStr(tbEmployee.Text) + _
                        " AND FamilyType = " + QuotedStr(tbFamilyType.Text)

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbFamilyType.Text = Dr("FamilyType").ToString
                tbFamilyName.Text = Dr("FamilyName").ToString
            Else
                tbFamilyType.Text = ""
                tbFamilyName.Text = ""
            End If
            tbFamilyType.Focus()
        Catch ex As Exception
            Throw New Exception("tb Family Type Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddldept_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDept.SelectedIndexChanged
        Try
            tbEmployee.Text = ""
            tbEmployee_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("tb Family Type Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlMedicalFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMedicalFor.SelectedIndexChanged


        Try
            If ddlMedicalFor.SelectedValue = "Employee" Then
                tbFamilyType.Text = ""
                tbFamilyName.Text = ""
                tbFamilyType.Enabled = False
                tbFamilyName.Enabled = False
                btnFamily.Enabled = False
            Else
                tbFamilyType.Enabled = True
                tbFamilyName.Enabled = True
                btnFamily.Enabled = True
            End If

        Catch ex As Exception
            Throw New Exception("ddl Medical For Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnMedical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMedical.Click
        Dim ResultField As String

        Try

            Session("filter") = "SELECT Medical_Code, Medical_Name FROM V_MsMedical "
            ResultField = "Medical_Code,Medical_Name"
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

            SQLString = "SELECT DISTINCT Medical_Code,Medical_Name FROM V_MsMedical WHERE Medical_Code = " + QuotedStr(tbMedicalCode.Text)

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbMedicalCode.Text = Dr("Medical_code").ToString
                tbMedicalName.Text = Dr("Medical_Name").ToString
            Else
                tbMedicalCode.Text = ""
                tbMedicalName.Text = ""
            End If
            tbFamilyType.Focus()
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
                ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                AttachScript("setformat();", Page, Me.GetType())
                tbRate.Focus()
            Catch ex As Exception
                lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
            End Try



        Catch ex As Exception
            Throw New Exception("ddl Medical For Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAmountClaim_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAmountClaim.TextChanged
        Try
            tbAmountPaid.Text = FormatFloat((CFloat(tbAmountPercent.Text) * CFloat(tbAmountClaim.Text) / 100), ViewState("DigitCurr"))
            tbAmountToPaid.Text = FormatFloat(tbAmountPaid.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAmountPercent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAmountPercent.TextChanged
        Try

            If (CFloat(tbAmountPercent.Text) < 0) Or (CFloat(tbAmountPercent.Text) > 100) Then
                lbStatus.Text = MessageDlg("Amount Percent Value Is Not Valid (0 - 100)")
                tbAmountPercent.Text = "0"
                tbAmountPercent.Focus()
            End If
            tbAmountPaid.Text = FormatFloat((CFloat(tbAmountPercent.Text) * CFloat(tbAmountClaim.Text) / 100), ViewState("DigitCurr"))
            tbAmountToPaid.Text = FormatFloat(tbAmountPaid.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAmountToPaid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAmountToPaid.TextChanged
        Try

            If CFloat(tbAmountClaim.Text) = 0 Then
                tbAmountPercent.Text = 0
            Else
                tbAmountPercent.Text = (CFloat(tbAmountToPaid.Text) * CFloat(tbAmountClaim.Text) / 100).ToString
            End If


        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEmployee.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmployee')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub lbFamily_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFamily.Click
    '    Try
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmpF')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Employee Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackDt2.Click, btnBackDt2Ke2.Click
    '    MultiView1.ActiveViewIndex = 0
    'End Sub
End Class
