Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPEAccident_TrPEAccident
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PEAccident"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
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
                If ViewState("Sender") = "btnEmployee" Then
                    tbEmployee.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    'ResultField = "Emp_No,Emp_Name,Section_Code,Job_Title,Emp_Status_Name,Gender"
                    ddlDepartment.SelectedValue = Session("Result")(2).ToString
                    ddlJobTitle.SelectedValue = Session("Result")(3).ToString
                    ddlEmpStatus.SelectedValue = Session("Result")(4).ToString
                    tbJenisKelamin.Text = Session("Result")(5).ToString
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
        FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
        FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))

        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetString As String
        Try
            GetString = GetStringHd
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetString, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "transdate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
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
                Dim QCNo As String

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
                Session("SelectCommand") = "EXEC S_PEFormAccident " + Result + "," + QuotedStr(ViewState("UserId"))
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormPEAccident.frx"

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
                        Result = ExecSPCommandGo(ActionValue, "S_PEAccident", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbEmployee.Enabled = State
            tbEmpName.Enabled = False
            tbDate.Enabled = State
            ddlTipe.Enabled = State
            ddlDepartment.Enabled = False
            ddlJobTitle.Enabled = False
            ddlEmpStatus.Enabled = False
            tbJenisKelamin.Enabled = False
            tbLokasi.Enabled = State
            ddlMasaKerja.Enabled = State
            ddlJenisKecelakaan.Enabled = State
            tbKlinikRS.Enabled = State
            tbKondisi.Enabled = State
            tbAccidentTime.Enabled = State
            tbAccidentTime2.Enabled = State
            tbBagTubuh.Enabled = State
            ddlSaatKecelakaan.Enabled = State
            ddlPerawatan.Enabled = State
            tbUrutanKejadian.Enabled = State
            tbNamaSaksi.Enabled = State
            tbDepartmentSaksi.Enabled = State
            tbKondisi.Enabled = State
            tbTindakan.Enabled = State
            tbTindakan.Enabled = State
            tbSaranManager.Enabled = State
            tbSaranSafety.Enabled = State
            tbRemark.Enabled = State
            btnEmployee.Visible = State
            btnSaveAll.Visible = State
            btnSaveTrans.Visible = State
            btnBack.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
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
        'Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If tbAccidentTime.Text.Trim.Length < 2 Then
                    tbAccidentTime.Text = "0" + tbAccidentTime.Text
                End If
                If tbAccidentTime2.Text.Trim.Length < 2 Then
                    tbAccidentTime2.Text = "0" + tbAccidentTime2.Text
                End If


                'tbReference.Text = "1"
                tbReference.Text = GetAutoNmbr("AI", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = tbReference.Text
                SQLString = "INSERT INTO PEAccident (TransNmbr, Status, TransDate, AccidentType, EmpNumb, Department, JobTitle, EmpStatus,MasaKerja,Gender, " + _
                "AccidentTime, AccidentDamage, AccidentPlace, AccidentWhen, Perawatan, Hospital, TempatLuka, AccidentUraian," + _
                "NamaSaksi,DepartmentSaksi,KondisiTakAman,TindakanTakAman,SaranManager,SaranSafety,Remark,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlTipe.SelectedValue) + "," + _
                QuotedStr(tbEmployee.Text) + ", " + QuotedStr(ddlDepartment.SelectedValue) + "," + QuotedStr(ddlJobTitle.SelectedValue) + "," + QuotedStr(ddlEmpStatus.SelectedValue) + "," + _
                QuotedStr(ddlMasaKerja.SelectedValue) + ", " + QuotedStr(tbJenisKelamin.Text) + "," + QuotedStr(tbAccidentTime.Text + ":" + tbAccidentTime2.Text) + ", " + _
                QuotedStr(ddlJenisKecelakaan.SelectedValue) + ", " + QuotedStr(tbLokasi.Text) + ", " + QuotedStr(ddlSaatKecelakaan.SelectedValue) + "," + _
                QuotedStr(ddlPerawatan.SelectedValue) + "," + QuotedStr(tbKlinikRS.Text) + "," + QuotedStr(tbBagTubuh.Text) + "," + QuotedStr(tbUrutanKejadian.Text) + "," + _
                QuotedStr(tbNamaSaksi.Text) + "," + QuotedStr(tbDepartmentSaksi.Text) + "," + QuotedStr(tbKondisi.Text) + "," + QuotedStr(tbTindakan.Text) + "," + _
                QuotedStr(tbSaranManager.Text) + "," + QuotedStr(tbSaranSafety.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEAccident WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                If tbAccidentTime.Text.Trim.Length < 2 Then
                    tbAccidentTime.Text = "0" + tbAccidentTime.Text
                End If
                If tbAccidentTime2.Text.Trim.Length < 2 Then
                    tbAccidentTime2.Text = "0" + tbAccidentTime2.Text
                End If

                SQLString = "UPDATE PEAccident SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", AccidentType = " + QuotedStr(ddlTipe.SelectedValue) + ", EmpNumb = " + QuotedStr(tbEmployee.Text) + ", Department = " + QuotedStr(ddlDepartment.SelectedValue) + _
                ", JobTitle = " + QuotedStr(ddlJobTitle.SelectedValue) + ", EmpStatus = " + QuotedStr(ddlEmpStatus.SelectedValue) + ", MasaKerja = " + QuotedStr(ddlMasaKerja.SelectedValue) + " , AccidentTime = " + QuotedStr(tbAccidentTime.Text + ":" + tbAccidentTime2.Text) + _
                ", AccidentDamage = " + QuotedStr(ddlJenisKecelakaan.SelectedValue) + ", AccidentPlace = " + QuotedStr(tbLokasi.Text) + ", AccidentWhen = " + QuotedStr(ddlSaatKecelakaan.SelectedValue) + ", Perawatan = " + QuotedStr(ddlPerawatan.SelectedValue) + _
                ", Hospital = " + QuotedStr(tbKlinikRS.Text) + " , TempatLuka = " + QuotedStr(tbBagTubuh.Text) + " , AccidentUraian = " + QuotedStr(tbUrutanKejadian.Text) + ",Gender = " + QuotedStr(tbJenisKelamin.Text) + _
                ", NamaSaksi = " + QuotedStr(tbNamaSaksi.Text) + " , DepartmentSaksi = " + QuotedStr(tbDepartmentSaksi.Text) + " , KondisiTakAman = " + QuotedStr(tbKondisi.Text) + _
                ", TindakanTakAman = " + QuotedStr(tbTindakan.Text) + " , SaranSafety = " + QuotedStr(tbSaranSafety.Text) + " , SaranManager = " + QuotedStr(tbSaranManager.Text) + " , Remark = " + QuotedStr(tbRemark.Text) + " , UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
            End If

            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)


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


            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbReference.Text
            ddlField.SelectedValue = "Transnmbr"
            btnSearch_Click(Nothing, Nothing)

            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
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
            EnableHd(True)
            tbDate.SelectedDate = ViewState("ServerDate") 'Today                        
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbEmployee.Text = ""
            tbEmpName.Text = ""
            ddlJobTitle.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""
            ddlTipe.SelectedIndex = 0
            ddlDepartment.SelectedIndex = 0
            tbJenisKelamin.Text = ""
            ddlMasaKerja.SelectedIndex = 0
            ddlJenisKecelakaan.SelectedIndex = 0
            tbKlinikRS.Text = ""
            tbKondisi.Text = ""
            tbAccidentTime.Text = "00"
            tbAccidentTime2.Text = "00"
            tbBagTubuh.Text = ""
            ddlSaatKecelakaan.SelectedIndex = 0
            ddlPerawatan.SelectedIndex = 0
            tbLokasi.Text = ""
            tbUrutanKejadian.Text = ""
            tbNamaSaksi.Text = ""
            tbDepartmentSaksi.Text = ""
            tbKondisi.Text = ""
            tbTindakan.Text = ""
            tbSaranManager.Text = ""
            tbSaranSafety.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
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
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmployee.Click
        Dim ResultField As String
        Try

            Session("Filter") = " SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name, Emp_Status, Emp_Status_Name, Gender " + _
                                " FROM V_MsEmployee "
            ResultField = "Emp_No,Emp_Name,Department,Job_Title,Emp_Status,Gender"
            ViewState("Sender") = "btnEmployee"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Employee Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmployee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmployee.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try

            SQLString = " SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Emp_Status, Gender " + _
                        " FROM V_MsEmployee WHERE Emp_No = " + QuotedStr(tbEmployee.Text)
            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                BindToText(tbEmployee, Dr("Emp_No").ToString)
                BindToText(tbEmpName, Dr("Emp_Name").ToString)
                BindToDropList(ddlDepartment, Dr("Department").ToString)
                BindToDropList(ddlJobTitle, Dr("Job_Title").ToString)
                BindToDropList(ddlEmpStatus, Dr("Emp_Status").ToString)
                BindToText(tbJenisKelamin, Dr("Gender").ToString)
            Else
                tbEmployee.Text = ""
                tbEmpName.Text = ""
                ddlDepartment.SelectedIndex = 0
                ddlJobTitle.SelectedIndex = 0
                ddlEmpStatus.SelectedIndex = 0
                tbJenisKelamin.Text = ""
            End If
            tbEmployee.Focus()
        Catch ex As Exception
            Throw New Exception("tb Employee Code TextChanged : " + ex.ToString)
        End Try
    End Sub

  


    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Exit Function
            'End If
            If tbEmployee.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee must have value")
                tbEmployee.Focus()
                Return False
            End If
            If (tbAccidentTime.Text.Trim = "") Then
                lbStatus.Text = MessageDlg("Accident Hour must have value")
                tbAccidentTime.Focus()
                Return False
            End If
            If (tbAccidentTime2.Text.Trim = "") Then
                lbStatus.Text = MessageDlg("Accident Minute must have value")
                tbAccidentTime2.Focus()
                Return False
            End If
            If Not IsNumeric(tbAccidentTime.Text) Then
                lbStatus.Text = MessageDlg("Accident Hour must Numeric Value")
                tbAccidentTime.Focus()
                Return False
            End If
            If Not IsNumeric(tbAccidentTime2.Text) Then
                lbStatus.Text = MessageDlg("Accident Minute must Numeric Value")
                tbAccidentTime2.Focus()
                Return False
            End If

            If CInt(tbAccidentTime.Text) > "23" Then
                lbStatus.Text = MessageDlg("Accident Hour Value is Not Valid (00 - 23)")
                tbAccidentTime.Focus()
                Return False
            End If
            If CInt(tbAccidentTime2.Text) > "59" Then
                lbStatus.Text = MessageDlg("Accident Minute Value is Not Valid (00 - 59)")
                tbAccidentTime2.Focus()
                Return False
            End If

            If ddlSaatKecelakaan.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Moment Of Accident must have value")
                tbSaatKecelakaan.Focus()
                Return False
            End If
            

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
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
                    FillTextBoxHd(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    EnableHd(False)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        btnHome.Visible = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        EnableHd(True)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Dim QCNo, StatusPrint As String
                    Try
                        QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                        StatusPrint = GVR.Cells(3).Text.Replace("&nbsp;", "")
                        Session("SelectCommand") = "EXEC S_PEFormAccident ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPEAccident.frx"
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
    Protected Sub lbEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEmployee.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmployee')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbDepartment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDepartment.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsOrganizationFile')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Department Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbJobTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbJobTitle.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsJobTitle')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Job Title Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbEmpStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEmpStatus.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmpStatus')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Status Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbEmployee, Dt.Rows(0)("Emp_Code").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("Emp_Name").ToString)
            BindToText(tbJenisKelamin, Dt.Rows(0)("Gender").ToString)
            BindToDropList(ddlTipe, Dt.Rows(0)("AccidentType").ToString)
            BindToDropList(ddlDepartment, Dt.Rows(0)("Department").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("Job_Title_Code").ToString)
            BindToDropList(ddlEmpStatus, Dt.Rows(0)("Emp_Status_Code").ToString)
            BindToDropList(ddlMasaKerja, Dt.Rows(0)("MasaKerja").ToString)
            BindToText(tbAccidentTime, Dt.Rows(0)("AccidentTime").ToString)
            BindToText(tbAccidentTime2, Dt.Rows(0)("AccidentTime2").ToString)
            BindToDropList(ddlJenisKecelakaan, Dt.Rows(0)("AccidentDamage").ToString)
            BindToText(tbLokasi, Dt.Rows(0)("AccidentPlace").ToString)
            BindToDropList(ddlSaatKecelakaan, Dt.Rows(0)("AccidentWhen").ToString)
            BindToDropList(ddlPerawatan, Dt.Rows(0)("Perawatan").ToString)
            BindToText(tbKlinikRS, Dt.Rows(0)("Hospital").ToString)
            BindToText(tbBagTubuh, Dt.Rows(0)("TempatLuka").ToString)
            BindToText(tbUrutanKejadian, Dt.Rows(0)("AccidentUraian").ToString)
            BindToText(tbNamaSaksi, Dt.Rows(0)("NamaSaksi").ToString)
            BindToText(tbDepartmentSaksi, Dt.Rows(0)("DepartmentSaksi").ToString)
            BindToText(tbKondisi, Dt.Rows(0)("KondisiTakAman").ToString)
            BindToText(tbTindakan, Dt.Rows(0)("TindakanTakAman").ToString)
            BindToText(tbSaranManager, Dt.Rows(0)("SaranManager").ToString)
            BindToText(tbSaranSafety, Dt.Rows(0)("SaranSafety").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub
    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            Session("SelectCommand") = Nothing
            Session("ReportFile") = Nothing
            Session("PrintType") = Nothing
            WebReport1.Dispose()
        Catch ex As Exception
            lbStatus.Text = "page disposed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            'FilterName = "Reference, Date, Status, Warehouse, Warehouse Name, Subled, Subled Name, RR Form, RR From Name, Received By, Remark"
            FilterName = "TransNmbr, Status, TransDate, Accidenttype, Emp_Name, Organization," + _
            "Job_Title_Name, Emp_Status_Name, MasaKerja,Accident_Time" + _
            "AccidentDamage, AccidentPlace" 'AccidentWhen"
            '"Perawatan,Hospital,TempatLuka,AccidentUraian,NamaSaksi,SaksiSectionName,KondisiTakAman," + _
            '"TindakanTakAman,SaranManager,SaranSafety"

            FilterValue = "TransNmbr, Status, dbo.FormatDate(TransDate), Accidenttype, Emp_Name, DepartmentName, Job_Title_Name, Emp_Status_Name, MasaKerja,Accident_Time, AccidentDamage, AccidentPlace"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

End Class
