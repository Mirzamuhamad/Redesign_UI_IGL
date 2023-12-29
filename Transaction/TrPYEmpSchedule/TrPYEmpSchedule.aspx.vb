Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPYEmpSchedule_TrPYEmpSchedule
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct TransNmbr, Nmbr, Status, TransDate, Department, Dept_Name, Remark FROM V_PYEmpScheduleHd "

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
                btnCopy.Visible = False
                btnPattern.Visible = False
                btnClear.Visible = False
            End If
            dsShift.ConnectionString = ViewState("DBConnection")
            lbStatus.Text = ""
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnEmpTo" Then
                    tbEmpNoTo.Text = Session("Result")(0).ToString
                    tbEmpNameTo.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnEmpFrom" Then
                    tbEmpNoFrom.Text = Session("Result")(0).ToString
                    tbEmpNameFrom.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnEmp" Then
                    tbFilter.Text = Session("Result")(0).ToString
                    tbFilterName.Text = Session("Result")(1).ToString
                    BindData()
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
        Try
            FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlDepartmentTo, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlDepartmentFrom, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlPattern, "Select PatternCode, PatternName+' ('+PatternShift+')' AS PatternName from VMsPattern", True, "PatternCode", "PatternName", ViewState("DBConnection"))
            FillCombo(ddlShiftA, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftB, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftC, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftD, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftE, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftF, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlShiftX, "EXEC S_GetShift", False, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbEndDate.SelectedDate = tbStartDate.SelectedDate.AddMonths(1).AddDays(-1)
            ddlDepartment.SelectedIndex = 0
            tbStartIndex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub MoveView(ByVal page As Integer)
        pnlCopy.Visible = False
        pnlPattern.Visible = False
        PnlHd.Visible = False
        pnlView.Visible = False
        If page = 0 Then
            PnlHd.Visible = True
        ElseIf page = 1 Then
            pnlView.Visible = True
        ElseIf page = 2 Then
            pnlCopy.Visible = True
        ElseIf page = 3 Then
            pnlPattern.Visible = True
        End If
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Dim Hari As Integer
        Dim TotalColumn As Integer
        'Dim tgl As DateTime
        Try
            StrFilter = ""
            Hari = CStr(DateDiff(DateInterval.Day, tbStartDate.SelectedDate, tbEndDate.SelectedDate)) + 1
            If Hari > 180 Then
                GridView1.DataSource = Nothing
                GridView1.DataBind()
                lbStatus.Text = "Range Periode cannot greater than 180 days"
                Exit Sub
            End If
            DT = BindDataTransaction("EXEC S_PYArrangeShiftView " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlDepartment.SelectedValue) + ", " + QuotedStr(tbFilter.Text), "", ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
            End If
            MoveView(0)
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EmpNumb DESC"
            End If
            btnCopy.Visible = (DT.Rows.Count >= 1)
            btnPattern.Visible = (DT.Rows.Count >= 1)
            btnClear.Visible = (DT.Rows.Count >= 1)
            TotalColumn = 180 + 5
            'lbStatus.Text = CStr(TotalColumn)
            'Exit Sub
            'referesh view data
            For I As Integer = 5 To (TotalColumn - 1)
                GridView1.Columns(I).Visible = False
            Next
            For I As Integer = 0 To (Hari - 1)
                GridView1.Columns(I + 5).Visible = True
                GridView1.Columns(I + 5).HeaderText = Format(tbStartDate.SelectedDate.AddDays(I), "ddd - dd MMM yy")
            Next
            'GridView1.Columns(5).Visible = False
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindSimulation(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Dim Hari As Integer
        Dim TotalColumn As Integer
        Dim HolidayOff As String

        'Dim tgl As DateTime
        Try
            StrFilter = ""
            Hari = CStr(DateDiff(DateInterval.Day, tbStartDate.SelectedDate, tbEndDate.SelectedDate)) + 1
            If Hari > 180 Then
                GridSimulasi.DataSource = Nothing
                GridSimulasi.DataBind()
                lbStatus.Text = "Range Periode cannot greater than 180 days"
                Exit Sub
            End If
            If cbHolidayOff.Checked = True Then
                HolidayOff = "Y"
            Else
                HolidayOff = "N"
            End If
            DT = SQLExecuteQuery("EXEC S_PYArrangeShiftSimulasi " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbPatternFormula.Text.Trim) + ", " + tbStartIndex.Text + ", " + _
                                 QuotedStr(ddlShiftA.SelectedItem.Text) + ", " + QuotedStr(ddlShiftB.SelectedItem.Text) + ", " + QuotedStr(ddlShiftC.SelectedItem.Text) + ", " + QuotedStr(ddlShiftD.SelectedItem.Text) + ", " + QuotedStr(ddlShiftE.SelectedItem.Text) + ", " + QuotedStr(ddlShiftF.SelectedItem.Text) + ", " + _
                                 QuotedStr(ddlShiftX.SelectedItem.Text)+", "+QuotedStr(HolidayOff), ViewState("DBConnection").ToString).Tables(0)
            'If DT.Rows.Count = 0 Then
            'lbStatus.Text = "No Data"
            'End If
            DV = DT.DefaultView
            TotalColumn = 180
            For I As Integer = 0 To (TotalColumn - 1)
                GridSimulasi.Columns(I).Visible = False
            Next
            For I As Integer = 0 To (Hari - 1)
                GridSimulasi.Columns(I).Visible = True
                GridSimulasi.Columns(I).HeaderText = Format(tbStartDate.SelectedDate.AddDays(I), "ddd - dd MMM yy")
            Next
            'GridView1.Columns(5).Visible = False
            GridSimulasi.DataSource = DV
            GridSimulasi.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData("")
    End Sub

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        Try
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridView1.EditIndex = -1
            BindData()
        Catch ex As Exception
            lbStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        
    End Sub

    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        Dim obj As GridViewRow
        Dim txt As DropDownList
        Try
            'If CheckMenuLevel("Edit") = False Then
            'Exit Sub
            'End If
            GridView1.EditIndex = e.NewEditIndex
            'DataGrid.ShowFooter = False
            BindData()
            obj = GridView1.Rows(e.NewEditIndex)
            txt = obj.FindControl("Shift001Edit")
            txt.Focus()
        Catch ex As Exception
            lbStatus.Text = "GridView1_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim SQLString As String
        Dim lbCode As Label
        Dim cbShift001, cbShift002, cbShift003, cbShift004, cbShift005, cbShift006, cbShift007, cbShift008, cbShift009 As DropDownList
        Dim cbShift010, cbShift011, cbShift012, cbShift013, cbShift014, cbShift015, cbShift016, cbShift017, cbShift018, cbShift019 As DropDownList
        Dim cbShift020, cbShift021, cbShift022, cbShift023, cbShift024, cbShift025, cbShift026, cbShift027, cbShift028, cbShift029 As DropDownList
        Dim cbShift030, cbShift031, cbShift032, cbShift033, cbShift034, cbShift035, cbShift036, cbShift037, cbShift038, cbShift039 As DropDownList
        Dim cbShift040, cbShift041, cbShift042, cbShift043, cbShift044, cbShift045, cbShift046, cbShift047, cbShift048, cbShift049 As DropDownList
        Dim cbShift050, cbShift051, cbShift052, cbShift053, cbShift054, cbShift055, cbShift056, cbShift057, cbShift058, cbShift059 As DropDownList
        Dim cbShift060, cbShift061, cbShift062, cbShift063, cbShift064, cbShift065, cbShift066, cbShift067, cbShift068, cbShift069 As DropDownList
        Dim cbShift070, cbShift071, cbShift072, cbShift073, cbShift074, cbShift075, cbShift076, cbShift077, cbShift078, cbShift079 As DropDownList
        Dim cbShift080, cbShift081, cbShift082, cbShift083, cbShift084, cbShift085, cbShift086, cbShift087, cbShift088, cbShift089 As DropDownList
        Dim cbShift090, cbShift091, cbShift092, cbShift093, cbShift094, cbShift095, cbShift096, cbShift097, cbShift098, cbShift099 As DropDownList
        Dim cbShift100, cbShift101, cbShift102, cbShift103, cbShift104, cbShift105, cbShift106, cbShift107, cbShift108, cbShift109 As DropDownList
        Dim cbShift110, cbShift111, cbShift112, cbShift113, cbShift114, cbShift115, cbShift116, cbShift117, cbShift118, cbShift119 As DropDownList
        Dim cbShift120, cbShift121, cbShift122, cbShift123, cbShift124, cbShift125, cbShift126, cbShift127, cbShift128, cbShift129 As DropDownList
        Dim cbShift130, cbShift131, cbShift132, cbShift133, cbShift134, cbShift135, cbShift136, cbShift137, cbShift138, cbShift139 As DropDownList
        Dim cbShift140, cbShift141, cbShift142, cbShift143, cbShift144, cbShift145, cbShift146, cbShift147, cbShift148, cbShift149 As DropDownList
        Dim cbShift150, cbShift151, cbShift152, cbShift153, cbShift154, cbShift155, cbShift156, cbShift157, cbShift158, cbShift159 As DropDownList
        Dim cbShift160, cbShift161, cbShift162, cbShift163, cbShift164, cbShift165, cbShift166, cbShift167, cbShift168, cbShift169 As DropDownList
        Dim cbShift170, cbShift171, cbShift172, cbShift173, cbShift174, cbShift175, cbShift176, cbShift177, cbShift178, cbShift179, cbShift180 As DropDownList
        Dim tgl As DateTime
        Dim hari As Integer
        Try
            lbCode = GridView1.Rows(e.RowIndex).FindControl("EmpNumbEdit")

            Hari = CStr(DateDiff(DateInterval.Day, tbStartDate.SelectedDate, tbEndDate.SelectedDate)) + 1
            If hari >= 1 Then
                cbShift001 = GridView1.Rows(e.RowIndex).FindControl("Shift001Edit")
                tgl = tbStartDate.SelectedDate
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift001.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 2 Then
                cbShift002 = GridView1.Rows(e.RowIndex).FindControl("Shift002Edit")
                tgl = tbStartDate.SelectedDate.AddDays(1)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift002.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 3 Then
                cbShift003 = GridView1.Rows(e.RowIndex).FindControl("Shift003Edit")
                tgl = tbStartDate.SelectedDate.AddDays(2)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift003.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 4 Then
                cbShift004 = GridView1.Rows(e.RowIndex).FindControl("Shift004Edit")
                tgl = tbStartDate.SelectedDate.AddDays(3)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift004.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 5 Then
                cbShift005 = GridView1.Rows(e.RowIndex).FindControl("Shift005Edit")
                tgl = tbStartDate.SelectedDate.AddDays(4)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift005.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 6 Then
                cbShift006 = GridView1.Rows(e.RowIndex).FindControl("Shift006Edit")
                tgl = tbStartDate.SelectedDate.AddDays(5)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift006.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 7 Then
                cbShift007 = GridView1.Rows(e.RowIndex).FindControl("Shift007Edit")
                tgl = tbStartDate.SelectedDate.AddDays(6)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift007.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 8 Then
                cbShift008 = GridView1.Rows(e.RowIndex).FindControl("Shift008Edit")
                tgl = tbStartDate.SelectedDate.AddDays(7)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift008.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 9 Then
                cbShift009 = GridView1.Rows(e.RowIndex).FindControl("Shift009Edit")
                tgl = tbStartDate.SelectedDate.AddDays(8)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift009.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 10 Then
                cbShift010 = GridView1.Rows(e.RowIndex).FindControl("Shift010Edit")
                tgl = tbStartDate.SelectedDate.AddDays(9)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift010.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 11 Then
                cbShift011 = GridView1.Rows(e.RowIndex).FindControl("Shift011Edit")
                tgl = tbStartDate.SelectedDate.AddDays(10)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift011.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 12 Then
                cbShift012 = GridView1.Rows(e.RowIndex).FindControl("Shift012Edit")
                tgl = tbStartDate.SelectedDate.AddDays(11)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift012.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 13 Then
                cbShift013 = GridView1.Rows(e.RowIndex).FindControl("Shift013Edit")
                tgl = tbStartDate.SelectedDate.AddDays(12)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift013.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 14 Then
                cbShift014 = GridView1.Rows(e.RowIndex).FindControl("Shift014Edit")
                tgl = tbStartDate.SelectedDate.AddDays(13)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift014.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 15 Then
                cbShift015 = GridView1.Rows(e.RowIndex).FindControl("Shift015Edit")
                tgl = tbStartDate.SelectedDate.AddDays(14)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift015.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 16 Then
                cbShift016 = GridView1.Rows(e.RowIndex).FindControl("Shift016Edit")
                tgl = tbStartDate.SelectedDate.AddDays(15)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift016.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 17 Then
                cbShift017 = GridView1.Rows(e.RowIndex).FindControl("Shift017Edit")
                tgl = tbStartDate.SelectedDate.AddDays(16)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift017.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 18 Then
                cbShift018 = GridView1.Rows(e.RowIndex).FindControl("Shift018Edit")
                tgl = tbStartDate.SelectedDate.AddDays(17)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift018.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 19 Then
                cbShift019 = GridView1.Rows(e.RowIndex).FindControl("Shift019Edit")
                tgl = tbStartDate.SelectedDate.AddDays(18)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift019.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 20 Then
                cbShift020 = GridView1.Rows(e.RowIndex).FindControl("Shift020Edit")
                tgl = tbStartDate.SelectedDate.AddDays(19)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift020.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 21 Then
                cbShift021 = GridView1.Rows(e.RowIndex).FindControl("Shift021Edit")
                tgl = tbStartDate.SelectedDate.AddDays(20)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift021.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 22 Then
                cbShift022 = GridView1.Rows(e.RowIndex).FindControl("Shift022Edit")
                tgl = tbStartDate.SelectedDate.AddDays(21)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift022.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 23 Then
                cbShift023 = GridView1.Rows(e.RowIndex).FindControl("Shift023Edit")
                tgl = tbStartDate.SelectedDate.AddDays(22)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift023.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 24 Then
                cbShift024 = GridView1.Rows(e.RowIndex).FindControl("Shift024Edit")
                tgl = tbStartDate.SelectedDate.AddDays(23)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift024.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 25 Then
                cbShift025 = GridView1.Rows(e.RowIndex).FindControl("Shift025Edit")
                tgl = tbStartDate.SelectedDate.AddDays(24)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift025.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 26 Then
                cbShift026 = GridView1.Rows(e.RowIndex).FindControl("Shift026Edit")
                tgl = tbStartDate.SelectedDate.AddDays(25)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift026.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 27 Then
                cbShift027 = GridView1.Rows(e.RowIndex).FindControl("Shift027Edit")
                tgl = tbStartDate.SelectedDate.AddDays(26)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift027.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 28 Then
                cbShift028 = GridView1.Rows(e.RowIndex).FindControl("Shift028Edit")
                tgl = tbStartDate.SelectedDate.AddDays(27)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift028.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 29 Then
                cbShift029 = GridView1.Rows(e.RowIndex).FindControl("Shift029Edit")
                tgl = tbStartDate.SelectedDate.AddDays(28)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift029.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 30 Then
                cbShift030 = GridView1.Rows(e.RowIndex).FindControl("Shift030Edit")
                tgl = tbStartDate.SelectedDate.AddDays(29)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift030.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 31 Then
                cbShift031 = GridView1.Rows(e.RowIndex).FindControl("Shift031Edit")
                tgl = tbStartDate.SelectedDate.AddDays(30)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift031.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 32 Then
                cbShift032 = GridView1.Rows(e.RowIndex).FindControl("Shift032Edit")
                tgl = tbStartDate.SelectedDate.AddDays(31)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift032.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 33 Then
                cbShift033 = GridView1.Rows(e.RowIndex).FindControl("Shift033Edit")
                tgl = tbStartDate.SelectedDate.AddDays(32)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift033.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 34 Then
                cbShift034 = GridView1.Rows(e.RowIndex).FindControl("Shift034Edit")
                tgl = tbStartDate.SelectedDate.AddDays(33)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift034.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 35 Then
                cbShift035 = GridView1.Rows(e.RowIndex).FindControl("Shift035Edit")
                tgl = tbStartDate.SelectedDate.AddDays(34)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift035.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 36 Then
                cbShift036 = GridView1.Rows(e.RowIndex).FindControl("Shift036Edit")
                tgl = tbStartDate.SelectedDate.AddDays(35)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift036.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 37 Then
                cbShift037 = GridView1.Rows(e.RowIndex).FindControl("Shift037Edit")
                tgl = tbStartDate.SelectedDate.AddDays(36)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift037.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 38 Then
                cbShift038 = GridView1.Rows(e.RowIndex).FindControl("Shift038Edit")
                tgl = tbStartDate.SelectedDate.AddDays(37)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift038.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 39 Then
                cbShift039 = GridView1.Rows(e.RowIndex).FindControl("Shift039Edit")
                tgl = tbStartDate.SelectedDate.AddDays(38)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift039.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 40 Then
                cbShift040 = GridView1.Rows(e.RowIndex).FindControl("Shift040Edit")
                tgl = tbStartDate.SelectedDate.AddDays(39)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift040.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 41 Then
                cbShift041 = GridView1.Rows(e.RowIndex).FindControl("Shift041Edit")
                tgl = tbStartDate.SelectedDate.AddDays(40)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift041.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 42 Then
                cbShift042 = GridView1.Rows(e.RowIndex).FindControl("Shift042Edit")
                tgl = tbStartDate.SelectedDate.AddDays(41)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift042.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 43 Then
                cbShift043 = GridView1.Rows(e.RowIndex).FindControl("Shift043Edit")
                tgl = tbStartDate.SelectedDate.AddDays(42)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift043.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 44 Then
                cbShift044 = GridView1.Rows(e.RowIndex).FindControl("Shift044Edit")
                tgl = tbStartDate.SelectedDate.AddDays(43)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift044.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 45 Then
                cbShift045 = GridView1.Rows(e.RowIndex).FindControl("Shift045Edit")
                tgl = tbStartDate.SelectedDate.AddDays(44)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift045.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 46 Then
                cbShift046 = GridView1.Rows(e.RowIndex).FindControl("Shift046Edit")
                tgl = tbStartDate.SelectedDate.AddDays(45)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift046.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 47 Then
                cbShift047 = GridView1.Rows(e.RowIndex).FindControl("Shift047Edit")
                tgl = tbStartDate.SelectedDate.AddDays(46)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift047.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 48 Then
                cbShift048 = GridView1.Rows(e.RowIndex).FindControl("Shift048Edit")
                tgl = tbStartDate.SelectedDate.AddDays(47)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift048.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 49 Then
                cbShift049 = GridView1.Rows(e.RowIndex).FindControl("Shift049Edit")
                tgl = tbStartDate.SelectedDate.AddDays(48)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift049.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 50 Then
                cbShift050 = GridView1.Rows(e.RowIndex).FindControl("Shift050Edit")
                tgl = tbStartDate.SelectedDate.AddDays(49)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift050.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 51 Then
                cbShift051 = GridView1.Rows(e.RowIndex).FindControl("Shift051Edit")
                tgl = tbStartDate.SelectedDate.AddDays(50)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift051.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 52 Then
                cbShift052 = GridView1.Rows(e.RowIndex).FindControl("Shift052Edit")
                tgl = tbStartDate.SelectedDate.AddDays(51)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift052.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 53 Then
                cbShift053 = GridView1.Rows(e.RowIndex).FindControl("Shift053Edit")
                tgl = tbStartDate.SelectedDate.AddDays(52)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift053.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 54 Then
                cbShift054 = GridView1.Rows(e.RowIndex).FindControl("Shift054Edit")
                tgl = tbStartDate.SelectedDate.AddDays(53)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift054.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 55 Then
                cbShift055 = GridView1.Rows(e.RowIndex).FindControl("Shift055Edit")
                tgl = tbStartDate.SelectedDate.AddDays(54)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift055.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 56 Then
                cbShift056 = GridView1.Rows(e.RowIndex).FindControl("Shift056Edit")
                tgl = tbStartDate.SelectedDate.AddDays(55)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift056.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 57 Then
                cbShift057 = GridView1.Rows(e.RowIndex).FindControl("Shift057Edit")
                tgl = tbStartDate.SelectedDate.AddDays(56)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift057.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 58 Then
                cbShift058 = GridView1.Rows(e.RowIndex).FindControl("Shift058Edit")
                tgl = tbStartDate.SelectedDate.AddDays(57)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift058.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 59 Then
                cbShift059 = GridView1.Rows(e.RowIndex).FindControl("Shift059Edit")
                tgl = tbStartDate.SelectedDate.AddDays(58)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift059.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 60 Then
                cbShift060 = GridView1.Rows(e.RowIndex).FindControl("Shift060Edit")
                tgl = tbStartDate.SelectedDate.AddDays(59)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift060.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 61 Then
                cbShift061 = GridView1.Rows(e.RowIndex).FindControl("Shift061Edit")
                tgl = tbStartDate.SelectedDate.AddDays(60)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift061.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 62 Then
                cbShift062 = GridView1.Rows(e.RowIndex).FindControl("Shift062Edit")
                tgl = tbStartDate.SelectedDate.AddDays(61)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift062.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 63 Then
                cbShift063 = GridView1.Rows(e.RowIndex).FindControl("Shift063Edit")
                tgl = tbStartDate.SelectedDate.AddDays(62)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift063.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 64 Then
                cbShift064 = GridView1.Rows(e.RowIndex).FindControl("Shift064Edit")
                tgl = tbStartDate.SelectedDate.AddDays(63)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift064.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 65 Then
                cbShift065 = GridView1.Rows(e.RowIndex).FindControl("Shift065Edit")
                tgl = tbStartDate.SelectedDate.AddDays(64)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift065.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 66 Then
                cbShift066 = GridView1.Rows(e.RowIndex).FindControl("Shift066Edit")
                tgl = tbStartDate.SelectedDate.AddDays(65)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift066.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 67 Then
                cbShift067 = GridView1.Rows(e.RowIndex).FindControl("Shift067Edit")
                tgl = tbStartDate.SelectedDate.AddDays(66)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift067.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 68 Then
                cbShift068 = GridView1.Rows(e.RowIndex).FindControl("Shift068Edit")
                tgl = tbStartDate.SelectedDate.AddDays(67)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift068.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 69 Then
                cbShift069 = GridView1.Rows(e.RowIndex).FindControl("Shift069Edit")
                tgl = tbStartDate.SelectedDate.AddDays(68)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift069.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 70 Then
                cbShift070 = GridView1.Rows(e.RowIndex).FindControl("Shift070Edit")
                tgl = tbStartDate.SelectedDate.AddDays(69)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift070.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 71 Then
                cbShift071 = GridView1.Rows(e.RowIndex).FindControl("Shift071Edit")
                tgl = tbStartDate.SelectedDate.AddDays(70)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift071.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 72 Then
                cbShift072 = GridView1.Rows(e.RowIndex).FindControl("Shift072Edit")
                tgl = tbStartDate.SelectedDate.AddDays(71)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift072.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 73 Then
                cbShift073 = GridView1.Rows(e.RowIndex).FindControl("Shift073Edit")
                tgl = tbStartDate.SelectedDate.AddDays(72)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift073.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 74 Then
                cbShift074 = GridView1.Rows(e.RowIndex).FindControl("Shift074Edit")
                tgl = tbStartDate.SelectedDate.AddDays(73)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift074.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 75 Then
                cbShift075 = GridView1.Rows(e.RowIndex).FindControl("Shift075Edit")
                tgl = tbStartDate.SelectedDate.AddDays(74)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift075.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 76 Then
                cbShift076 = GridView1.Rows(e.RowIndex).FindControl("Shift076Edit")
                tgl = tbStartDate.SelectedDate.AddDays(75)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift076.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 77 Then
                cbShift077 = GridView1.Rows(e.RowIndex).FindControl("Shift077Edit")
                tgl = tbStartDate.SelectedDate.AddDays(76)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift077.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 78 Then
                cbShift078 = GridView1.Rows(e.RowIndex).FindControl("Shift078Edit")
                tgl = tbStartDate.SelectedDate.AddDays(77)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift078.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 79 Then
                cbShift079 = GridView1.Rows(e.RowIndex).FindControl("Shift079Edit")
                tgl = tbStartDate.SelectedDate.AddDays(78)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift079.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 80 Then
                cbShift080 = GridView1.Rows(e.RowIndex).FindControl("Shift080Edit")
                tgl = tbStartDate.SelectedDate.AddDays(79)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift080.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 81 Then
                cbShift081 = GridView1.Rows(e.RowIndex).FindControl("Shift081Edit")
                tgl = tbStartDate.SelectedDate.AddDays(80)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift081.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 82 Then
                cbShift082 = GridView1.Rows(e.RowIndex).FindControl("Shift082Edit")
                tgl = tbStartDate.SelectedDate.AddDays(81)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift082.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 83 Then
                cbShift083 = GridView1.Rows(e.RowIndex).FindControl("Shift083Edit")
                tgl = tbStartDate.SelectedDate.AddDays(82)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift083.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 84 Then
                cbShift084 = GridView1.Rows(e.RowIndex).FindControl("Shift084Edit")
                tgl = tbStartDate.SelectedDate.AddDays(83)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift084.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 85 Then
                cbShift085 = GridView1.Rows(e.RowIndex).FindControl("Shift085Edit")
                tgl = tbStartDate.SelectedDate.AddDays(84)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift085.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 86 Then
                cbShift086 = GridView1.Rows(e.RowIndex).FindControl("Shift086Edit")
                tgl = tbStartDate.SelectedDate.AddDays(85)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift086.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 87 Then
                cbShift087 = GridView1.Rows(e.RowIndex).FindControl("Shift087Edit")
                tgl = tbStartDate.SelectedDate.AddDays(86)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift087.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 88 Then
                cbShift088 = GridView1.Rows(e.RowIndex).FindControl("Shift088Edit")
                tgl = tbStartDate.SelectedDate.AddDays(87)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift088.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 89 Then
                cbShift089 = GridView1.Rows(e.RowIndex).FindControl("Shift089Edit")
                tgl = tbStartDate.SelectedDate.AddDays(88)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift089.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 90 Then
                cbShift090 = GridView1.Rows(e.RowIndex).FindControl("Shift090Edit")
                tgl = tbStartDate.SelectedDate.AddDays(89)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift090.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 91 Then
                cbShift091 = GridView1.Rows(e.RowIndex).FindControl("Shift091Edit")
                tgl = tbStartDate.SelectedDate.AddDays(90)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift091.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 92 Then
                cbShift092 = GridView1.Rows(e.RowIndex).FindControl("Shift092Edit")
                tgl = tbStartDate.SelectedDate.AddDays(91)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift092.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 93 Then
                cbShift093 = GridView1.Rows(e.RowIndex).FindControl("Shift093Edit")
                tgl = tbStartDate.SelectedDate.AddDays(92)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift093.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 94 Then
                cbShift094 = GridView1.Rows(e.RowIndex).FindControl("Shift094Edit")
                tgl = tbStartDate.SelectedDate.AddDays(93)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift094.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 95 Then
                cbShift095 = GridView1.Rows(e.RowIndex).FindControl("Shift095Edit")
                tgl = tbStartDate.SelectedDate.AddDays(94)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift095.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 96 Then
                cbShift096 = GridView1.Rows(e.RowIndex).FindControl("Shift096Edit")
                tgl = tbStartDate.SelectedDate.AddDays(95)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift096.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 97 Then
                cbShift097 = GridView1.Rows(e.RowIndex).FindControl("Shift097Edit")
                tgl = tbStartDate.SelectedDate.AddDays(96)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift097.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 98 Then
                cbShift098 = GridView1.Rows(e.RowIndex).FindControl("Shift098Edit")
                tgl = tbStartDate.SelectedDate.AddDays(97)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift098.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 99 Then
                cbShift099 = GridView1.Rows(e.RowIndex).FindControl("Shift099Edit")
                tgl = tbStartDate.SelectedDate.AddDays(98)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift099.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 100 Then
                cbShift100 = GridView1.Rows(e.RowIndex).FindControl("Shift100Edit")
                tgl = tbStartDate.SelectedDate.AddDays(99)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift100.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 101 Then
                cbShift101 = GridView1.Rows(e.RowIndex).FindControl("Shift101Edit")
                tgl = tbStartDate.SelectedDate.AddDays(100)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift101.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 102 Then
                cbShift102 = GridView1.Rows(e.RowIndex).FindControl("Shift102Edit")
                tgl = tbStartDate.SelectedDate.AddDays(101)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift102.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 103 Then
                cbShift103 = GridView1.Rows(e.RowIndex).FindControl("Shift103Edit")
                tgl = tbStartDate.SelectedDate.AddDays(102)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift103.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 104 Then
                cbShift104 = GridView1.Rows(e.RowIndex).FindControl("Shift104Edit")
                tgl = tbStartDate.SelectedDate.AddDays(103)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift104.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 105 Then
                cbShift105 = GridView1.Rows(e.RowIndex).FindControl("Shift105Edit")
                tgl = tbStartDate.SelectedDate.AddDays(104)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift105.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 106 Then
                cbShift106 = GridView1.Rows(e.RowIndex).FindControl("Shift106Edit")
                tgl = tbStartDate.SelectedDate.AddDays(105)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift106.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 107 Then
                cbShift107 = GridView1.Rows(e.RowIndex).FindControl("Shift107Edit")
                tgl = tbStartDate.SelectedDate.AddDays(106)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift107.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 108 Then
                cbShift108 = GridView1.Rows(e.RowIndex).FindControl("Shift108Edit")
                tgl = tbStartDate.SelectedDate.AddDays(107)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift108.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 109 Then
                cbShift109 = GridView1.Rows(e.RowIndex).FindControl("Shift109Edit")
                tgl = tbStartDate.SelectedDate.AddDays(108)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift109.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 110 Then
                cbShift110 = GridView1.Rows(e.RowIndex).FindControl("Shift110Edit")
                tgl = tbStartDate.SelectedDate.AddDays(109)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift110.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 111 Then
                cbShift111 = GridView1.Rows(e.RowIndex).FindControl("Shift111Edit")
                tgl = tbStartDate.SelectedDate.AddDays(110)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift111.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 112 Then
                cbShift112 = GridView1.Rows(e.RowIndex).FindControl("Shift112Edit")
                tgl = tbStartDate.SelectedDate.AddDays(111)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift112.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 113 Then
                cbShift113 = GridView1.Rows(e.RowIndex).FindControl("Shift113Edit")
                tgl = tbStartDate.SelectedDate.AddDays(112)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift113.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 114 Then
                cbShift114 = GridView1.Rows(e.RowIndex).FindControl("Shift114Edit")
                tgl = tbStartDate.SelectedDate.AddDays(113)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift114.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 115 Then
                cbShift115 = GridView1.Rows(e.RowIndex).FindControl("Shift115Edit")
                tgl = tbStartDate.SelectedDate.AddDays(114)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift115.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 116 Then
                cbShift116 = GridView1.Rows(e.RowIndex).FindControl("Shift116Edit")
                tgl = tbStartDate.SelectedDate.AddDays(115)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift116.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 117 Then
                cbShift117 = GridView1.Rows(e.RowIndex).FindControl("Shift117Edit")
                tgl = tbStartDate.SelectedDate.AddDays(116)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift117.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 118 Then
                cbShift118 = GridView1.Rows(e.RowIndex).FindControl("Shift118Edit")
                tgl = tbStartDate.SelectedDate.AddDays(117)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift118.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 119 Then
                cbShift119 = GridView1.Rows(e.RowIndex).FindControl("Shift119Edit")
                tgl = tbStartDate.SelectedDate.AddDays(118)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift119.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 120 Then
                cbShift120 = GridView1.Rows(e.RowIndex).FindControl("Shift120Edit")
                tgl = tbStartDate.SelectedDate.AddDays(119)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift120.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 121 Then
                cbShift121 = GridView1.Rows(e.RowIndex).FindControl("Shift121Edit")
                tgl = tbStartDate.SelectedDate.AddDays(120)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift121.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 122 Then
                cbShift122 = GridView1.Rows(e.RowIndex).FindControl("Shift122Edit")
                tgl = tbStartDate.SelectedDate.AddDays(121)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift122.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 123 Then
                cbShift123 = GridView1.Rows(e.RowIndex).FindControl("Shift123Edit")
                tgl = tbStartDate.SelectedDate.AddDays(122)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift123.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 124 Then
                cbShift124 = GridView1.Rows(e.RowIndex).FindControl("Shift124Edit")
                tgl = tbStartDate.SelectedDate.AddDays(123)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift124.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 125 Then
                cbShift125 = GridView1.Rows(e.RowIndex).FindControl("Shift125Edit")
                tgl = tbStartDate.SelectedDate.AddDays(124)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift125.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 126 Then
                cbShift126 = GridView1.Rows(e.RowIndex).FindControl("Shift126Edit")
                tgl = tbStartDate.SelectedDate.AddDays(125)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift126.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 127 Then
                cbShift127 = GridView1.Rows(e.RowIndex).FindControl("Shift127Edit")
                tgl = tbStartDate.SelectedDate.AddDays(126)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift127.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 128 Then
                cbShift128 = GridView1.Rows(e.RowIndex).FindControl("Shift128Edit")
                tgl = tbStartDate.SelectedDate.AddDays(127)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift128.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 129 Then
                cbShift129 = GridView1.Rows(e.RowIndex).FindControl("Shift129Edit")
                tgl = tbStartDate.SelectedDate.AddDays(128)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift129.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 130 Then
                cbShift130 = GridView1.Rows(e.RowIndex).FindControl("Shift130Edit")
                tgl = tbStartDate.SelectedDate.AddDays(129)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift130.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 131 Then
                cbShift131 = GridView1.Rows(e.RowIndex).FindControl("Shift131Edit")
                tgl = tbStartDate.SelectedDate.AddDays(130)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift131.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 132 Then
                cbShift132 = GridView1.Rows(e.RowIndex).FindControl("Shift132Edit")
                tgl = tbStartDate.SelectedDate.AddDays(131)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift132.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 133 Then
                cbShift133 = GridView1.Rows(e.RowIndex).FindControl("Shift133Edit")
                tgl = tbStartDate.SelectedDate.AddDays(132)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift133.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 134 Then
                cbShift134 = GridView1.Rows(e.RowIndex).FindControl("Shift134Edit")
                tgl = tbStartDate.SelectedDate.AddDays(133)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift134.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 135 Then
                cbShift135 = GridView1.Rows(e.RowIndex).FindControl("Shift135Edit")
                tgl = tbStartDate.SelectedDate.AddDays(134)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift135.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 136 Then
                cbShift136 = GridView1.Rows(e.RowIndex).FindControl("Shift136Edit")
                tgl = tbStartDate.SelectedDate.AddDays(135)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift136.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 137 Then
                cbShift137 = GridView1.Rows(e.RowIndex).FindControl("Shift137Edit")
                tgl = tbStartDate.SelectedDate.AddDays(136)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift137.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 138 Then
                cbShift138 = GridView1.Rows(e.RowIndex).FindControl("Shift138Edit")
                tgl = tbStartDate.SelectedDate.AddDays(137)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift138.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 139 Then
                cbShift139 = GridView1.Rows(e.RowIndex).FindControl("Shift139Edit")
                tgl = tbStartDate.SelectedDate.AddDays(138)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift139.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 140 Then
                cbShift140 = GridView1.Rows(e.RowIndex).FindControl("Shift140Edit")
                tgl = tbStartDate.SelectedDate.AddDays(139)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift140.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 141 Then
                cbShift141 = GridView1.Rows(e.RowIndex).FindControl("Shift141Edit")
                tgl = tbStartDate.SelectedDate.AddDays(140)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift141.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 142 Then
                cbShift142 = GridView1.Rows(e.RowIndex).FindControl("Shift142Edit")
                tgl = tbStartDate.SelectedDate.AddDays(141)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift142.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 143 Then
                cbShift143 = GridView1.Rows(e.RowIndex).FindControl("Shift143Edit")
                tgl = tbStartDate.SelectedDate.AddDays(142)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift143.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 144 Then
                cbShift144 = GridView1.Rows(e.RowIndex).FindControl("Shift144Edit")
                tgl = tbStartDate.SelectedDate.AddDays(143)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift144.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 145 Then
                cbShift145 = GridView1.Rows(e.RowIndex).FindControl("Shift145Edit")
                tgl = tbStartDate.SelectedDate.AddDays(144)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift145.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 146 Then
                cbShift146 = GridView1.Rows(e.RowIndex).FindControl("Shift146Edit")
                tgl = tbStartDate.SelectedDate.AddDays(145)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift146.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 147 Then
                cbShift147 = GridView1.Rows(e.RowIndex).FindControl("Shift147Edit")
                tgl = tbStartDate.SelectedDate.AddDays(146)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift147.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 148 Then
                cbShift148 = GridView1.Rows(e.RowIndex).FindControl("Shift148Edit")
                tgl = tbStartDate.SelectedDate.AddDays(147)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift148.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 149 Then
                cbShift149 = GridView1.Rows(e.RowIndex).FindControl("Shift149Edit")
                tgl = tbStartDate.SelectedDate.AddDays(148)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift149.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 150 Then
                cbShift150 = GridView1.Rows(e.RowIndex).FindControl("Shift150Edit")
                tgl = tbStartDate.SelectedDate.AddDays(149)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift150.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 151 Then
                cbShift151 = GridView1.Rows(e.RowIndex).FindControl("Shift151Edit")
                tgl = tbStartDate.SelectedDate.AddDays(150)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift151.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 152 Then
                cbShift152 = GridView1.Rows(e.RowIndex).FindControl("Shift152Edit")
                tgl = tbStartDate.SelectedDate.AddDays(151)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift152.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 153 Then
                cbShift153 = GridView1.Rows(e.RowIndex).FindControl("Shift153Edit")
                tgl = tbStartDate.SelectedDate.AddDays(152)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift153.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 154 Then
                cbShift154 = GridView1.Rows(e.RowIndex).FindControl("Shift154Edit")
                tgl = tbStartDate.SelectedDate.AddDays(153)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift154.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 155 Then
                cbShift155 = GridView1.Rows(e.RowIndex).FindControl("Shift155Edit")
                tgl = tbStartDate.SelectedDate.AddDays(154)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift155.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 156 Then
                cbShift156 = GridView1.Rows(e.RowIndex).FindControl("Shift156Edit")
                tgl = tbStartDate.SelectedDate.AddDays(155)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift156.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 157 Then
                cbShift157 = GridView1.Rows(e.RowIndex).FindControl("Shift157Edit")
                tgl = tbStartDate.SelectedDate.AddDays(156)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift157.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 158 Then
                cbShift158 = GridView1.Rows(e.RowIndex).FindControl("Shift158Edit")
                tgl = tbStartDate.SelectedDate.AddDays(157)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift158.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 159 Then
                cbShift159 = GridView1.Rows(e.RowIndex).FindControl("Shift159Edit")
                tgl = tbStartDate.SelectedDate.AddDays(158)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift159.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 160 Then
                cbShift160 = GridView1.Rows(e.RowIndex).FindControl("Shift160Edit")
                tgl = tbStartDate.SelectedDate.AddDays(159)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift160.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 161 Then
                cbShift161 = GridView1.Rows(e.RowIndex).FindControl("Shift161Edit")
                tgl = tbStartDate.SelectedDate.AddDays(160)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift161.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 162 Then
                cbShift162 = GridView1.Rows(e.RowIndex).FindControl("Shift162Edit")
                tgl = tbStartDate.SelectedDate.AddDays(161)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift162.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 163 Then
                cbShift163 = GridView1.Rows(e.RowIndex).FindControl("Shift163Edit")
                tgl = tbStartDate.SelectedDate.AddDays(162)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift163.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 164 Then
                cbShift164 = GridView1.Rows(e.RowIndex).FindControl("Shift164Edit")
                tgl = tbStartDate.SelectedDate.AddDays(163)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift164.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 165 Then
                cbShift165 = GridView1.Rows(e.RowIndex).FindControl("Shift165Edit")
                tgl = tbStartDate.SelectedDate.AddDays(164)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift165.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 166 Then
                cbShift166 = GridView1.Rows(e.RowIndex).FindControl("Shift166Edit")
                tgl = tbStartDate.SelectedDate.AddDays(165)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift166.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 167 Then
                cbShift167 = GridView1.Rows(e.RowIndex).FindControl("Shift167Edit")
                tgl = tbStartDate.SelectedDate.AddDays(166)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift167.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 168 Then
                cbShift168 = GridView1.Rows(e.RowIndex).FindControl("Shift168Edit")
                tgl = tbStartDate.SelectedDate.AddDays(167)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift168.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 169 Then
                cbShift169 = GridView1.Rows(e.RowIndex).FindControl("Shift169Edit")
                tgl = tbStartDate.SelectedDate.AddDays(168)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift169.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 170 Then
                cbShift170 = GridView1.Rows(e.RowIndex).FindControl("Shift170Edit")
                tgl = tbStartDate.SelectedDate.AddDays(169)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift170.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 171 Then
                cbShift171 = GridView1.Rows(e.RowIndex).FindControl("Shift171Edit")
                tgl = tbStartDate.SelectedDate.AddDays(170)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift171.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 172 Then
                cbShift172 = GridView1.Rows(e.RowIndex).FindControl("Shift172Edit")
                tgl = tbStartDate.SelectedDate.AddDays(171)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift172.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 173 Then
                cbShift173 = GridView1.Rows(e.RowIndex).FindControl("Shift173Edit")
                tgl = tbStartDate.SelectedDate.AddDays(172)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift173.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 174 Then
                cbShift174 = GridView1.Rows(e.RowIndex).FindControl("Shift174Edit")
                tgl = tbStartDate.SelectedDate.AddDays(173)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift174.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 175 Then
                cbShift175 = GridView1.Rows(e.RowIndex).FindControl("Shift175Edit")
                tgl = tbStartDate.SelectedDate.AddDays(174)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift175.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 176 Then
                cbShift176 = GridView1.Rows(e.RowIndex).FindControl("Shift176Edit")
                tgl = tbStartDate.SelectedDate.AddDays(175)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift176.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 177 Then
                cbShift177 = GridView1.Rows(e.RowIndex).FindControl("Shift177Edit")
                tgl = tbStartDate.SelectedDate.AddDays(176)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift177.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 178 Then
                cbShift178 = GridView1.Rows(e.RowIndex).FindControl("Shift178Edit")
                tgl = tbStartDate.SelectedDate.AddDays(177)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift178.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 179 Then
                cbShift179 = GridView1.Rows(e.RowIndex).FindControl("Shift179Edit")
                tgl = tbStartDate.SelectedDate.AddDays(178)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift179.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            If hari >= 180 Then
                cbShift180 = GridView1.Rows(e.RowIndex).FindControl("Shift180Edit")
                tgl = tbStartDate.SelectedDate.AddDays(179)
                SQLString = "EXEC S_PYArrangeShiftUpdate " + QuotedStr(lbCode.Text) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(cbShift180.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridView1.EditIndex = -1
            BindData()

        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "DataGrid_Update Error: " & ex.ToString
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

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        Try
            BindData()
        Catch ex As Exception
            lbStatus.Text = "ddlDepartment_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStartDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartDate.SelectionChanged, tbEndDate.SelectionChanged
        Try
            BindData()
        Catch ex As Exception
            lbStatus.Text = "Date_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            'ViewState("StateHd") = "Insert"
            MoveView(2)
            EnableHd(False)
            ddlCopyFrom.SelectedIndex = 0
            ddlDepartmentTo.Enabled = False
            ddlDepartmentFrom.Visible = ddlCopyFrom.SelectedValue = "Organization"
            ddlDepartmentTo.Visible = ddlCopyFrom.SelectedValue = "Organization"
            tbEmpNoTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
            tbEmpNameTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
            btnEmpTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
            tbEmpNoFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
            tbEmpNameFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
            btnEmpFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
            ddlDepartmentTo.SelectedValue = ddlDepartment.SelectedValue
            ddlDepartmentFrom.SelectedValue = ""
            tbEmpNoFrom.Text = ""
            tbEmpNameFrom.Text = ""
            tbEmpNoTo.Text = ""
            tbEmpNameTo.Text = ""
            'ModifyInput2(True, pnlInput, PnlDt, GridDt)
            'ViewState("TransNmbr") = ""
            'newTrans()
            'btnHome.Visible = False
            'tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCopyFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCopyFrom.SelectedIndexChanged
        ddlDepartmentFrom.Visible = ddlCopyFrom.SelectedValue = "Organization"
        ddlDepartmentTo.Visible = ddlCopyFrom.SelectedValue = "Organization"
        tbEmpNoTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
        tbEmpNameTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
        btnEmpTo.Visible = ddlCopyFrom.SelectedValue = "Employee"
        tbEmpNoFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
        tbEmpNameFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
        btnEmpFrom.Visible = ddlCopyFrom.SelectedValue = "Employee"
    End Sub

    Protected Sub btnCancelCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCopy.Click
        Try
            MoveView(0)
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btnCancelCopy_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnOKCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKCopy.Click
        Dim hasil As String
        Try
            If ddlCopyFrom.SelectedValue = "Employee" Then
                If tbEmpNoFrom.Text.Trim = "" Then
                    lbStatus.Text = "Employee Source must have value"
                    tbEmpNoFrom.Focus()
                    Exit Sub
                End If
                If tbEmpNoTo.Text.Trim = "" Then
                    lbStatus.Text = "Employee Destination must have value"
                    tbEmpNoTo.Focus()
                    Exit Sub
                End If
                If tbEmpNoFrom.Text.Trim = tbEmpNoTo.Text.Trim Then
                    lbStatus.Text = "Employee Destination cannot same with Employee Source"
                    tbEmpNoTo.Focus()
                    Exit Sub
                End If
                hasil = SQLExecuteScalar("DECLARE @EMessage	VARCHAR(7900) EXEC S_PYArrangeShiftCopy " + QuotedStr(tbEmpNoFrom.Text.Trim) + ", " + QuotedStr(tbEmpNoTo.Text.Trim) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", 1, " + ViewState("UserId").ToString + ", @EMessage OUT SELECT @EMessage", ViewState("DBConnection").ToString)
                If hasil.Length > 5 Then
                    lbStatus.Text = MessageDlg(hasil)
                    Exit Sub
                End If
            Else
                If ddlDepartmentFrom.SelectedValue = "" Then
                    lbStatus.Text = "Organization Source must have value"
                    ddlDepartmentFrom.Focus()
                    Exit Sub
                End If
                If ddlDepartmentFrom.SelectedValue = ddlDepartmentTo.SelectedValue Then
                    lbStatus.Text = "Organization Source cannot same with Organization Destination"
                    ddlDepartmentFrom.Focus()
                    Exit Sub
                End If
                hasil = SQLExecuteScalar("DECLARE @EMessage	VARCHAR(7900) EXEC S_PYArrangeShiftCopy " + QuotedStr(ddlDepartmentFrom.SelectedValue) + ", " + QuotedStr(ddlDepartmentTo.SelectedValue) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + ViewState("UserId").ToString + ", @EMessage OUT SELECT @EMessage", ViewState("DBConnection").ToString)
                If hasil.Length > 5 Then
                    lbStatus.Text = MessageDlg(hasil)
                    Exit Sub
                End If
            End If
            MoveView(0)
            BindData()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btnOKCopy_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpTo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "Select Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name FROM V_MsEmployee Where Fg_Active = 'Y' AND Department LIKE '" + ddlDepartment.SelectedValue + "%' and Emp_No <> " + QuotedStr(tbEmpNoFrom.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnEmpTo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpTo_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpFrom.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "Select Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name FROM V_MsEmployee Where Fg_Active = 'Y' AND Department LIKE '" + ddlDepartment.SelectedValue + " %' and Emp_No <> " + QuotedStr(tbEmpNoTo.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnEmpFrom"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpFrom_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNoTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNoTo.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Department LIKE '" + ddlDepartment.SelectedValue + "%' and Emp_No = " + QuotedStr(tbEmpNoTo.Text) + " and Emp_No <> " + QuotedStr(tbEmpNoFrom.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpNoTo.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpNameTo.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbEmpNoTo.Text = ""
                tbEmpNameTo.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbEmpNoTo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEmpNoFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNoFrom.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Department LIKE '" + ddlDepartment.SelectedValue + "%' and Emp_No = " + QuotedStr(tbEmpNoFrom.Text) + " and Emp_No <> " + QuotedStr(tbEmpNoTo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpNoFrom.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpNameFrom.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbEmpNoFrom.Text = ""
                tbEmpNameFrom.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbEmpNoFrom_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPattern_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPattern.Click
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbemp As Label
            Dim Pertamax As Boolean
            Dim Result As String

            Pertamax = True
            Result = ""
            For Each GVR In GridView1.Rows
                CB = GVR.FindControl("cbSelect")
                lbemp = GVR.FindControl("EmpNumb")
                If CB.Checked Then
                    If Pertamax Then
                        Result = "''" + lbemp.Text.Trim + "''"
                        Pertamax = False
                    Else
                        Result = Result + ",''" + lbemp.Text.Trim + "''"
                    End If
                End If
            Next
            'Result = Result + "'"
            ViewState("StrEmp") = " And Emp_No IN ( " + Result + ")"
            If Result.ToString.Length <= 2 Then
                MoveView(0)
                lbStatus.Text = "Employee must be selected"
                Exit Sub
            End If
            'lbStatus.Text = ViewState("StrEmp")
            'Exit Sub
            MoveView(3)
            EnableHd(False)
            BindDataHoliday()
            ddlPattern.SelectedValue = ""
            tbStartIndex.Text = "1"
            ddlPattern_SelectedIndexChanged(Nothing, Nothing)

        Catch ex As Exception
            lbStatus.Text = "Btn Pattern Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOKPattern_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKPattern.Click
        Dim HolidayOff, hasil As String
        Try
            If cbHolidayOff.Checked = True Then
                HolidayOff = "Y"
            Else
                HolidayOff = "N"
            End If
            hasil = SQLExecuteNonQuery("Declare @A VarChar(255) EXEC S_PYArrangeShiftRoster " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", 1, " + QuotedStr(tbPatternFormula.Text.Trim) + ", " + tbStartIndex.Text.Trim + ", " + QuotedStr(ddlShiftA.SelectedValue) + ", " + _
                    QuotedStr(ddlShiftB.SelectedValue) + ", " + QuotedStr(ddlShiftC.SelectedValue) + ", " + QuotedStr(ddlShiftD.SelectedValue) + ", " + QuotedStr(ddlShiftE.SelectedValue) + ", " + QuotedStr(ddlShiftF.SelectedValue) + ", " + QuotedStr(ddlShiftX.SelectedValue) + ", " + QuotedStr(HolidayOff) + ", '" + ViewState("StrEmp") + "', " + _
                 QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A ", ViewState("DBConnection").ToString)
            If hasil.Length >= 5 Then
                lbStatus.Text = hasil
                Exit Sub
            End If
            MoveView(0)
            BindData()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btnOKPattern_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelPattern_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelPattern.Click
        Try
            MoveView(0)
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btnCancelPattern_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub EnableHd(ByVal Bool As Boolean)
        Try
            btnCopy.Visible = Bool
            btnPattern.Visible = Bool
            btnClear.Visible = Bool
            'btnSchedule.Visible = Bool
            'btnUnSchedule.Visible = Bool
            ddlDepartment.Enabled = Bool
            tbStartDate.Enabled = Bool
            tbEndDate.Enabled = Bool
        Catch ex As Exception
            lbStatus.Text = "EnableHd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub EnableShift(ByVal bool As Boolean)
        ddlShiftA.Enabled = bool
        ddlShiftB.Enabled = bool
        ddlShiftC.Enabled = bool
        ddlShiftD.Enabled = bool
        ddlShiftE.Enabled = bool
        ddlShiftF.Enabled = bool
        ddlShiftX.Enabled = bool
    End Sub


    Protected Sub ddlPattern_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPattern.SelectedIndexChanged
        Dim dt As DataTable
        Dim dr As DataRow
        Try
            EnableShift(False)
            If ddlPattern.SelectedValue <> "" Then
                dt = SQLExecuteQuery("SELECT * from VMsPattern WHERE PatternCode = " + QuotedStr(ddlPattern.SelectedValue), ViewState("DBConnection")).Tables(0)
                dr = dt.Rows(0)
                tbPatternFormula.Text = TrimStr(UCase(dr("PatternShift").ToString))
                ddlShiftA.SelectedValue = TrimStr(dr("ShiftA").ToString)
                ddlShiftB.SelectedValue = TrimStr(dr("ShiftB").ToString)
                ddlShiftC.SelectedValue = TrimStr(dr("ShiftC").ToString)
                ddlShiftD.SelectedValue = TrimStr(dr("ShiftD").ToString)
                ddlShiftE.SelectedValue = TrimStr(dr("ShiftE").ToString)
                ddlShiftF.SelectedValue = TrimStr(dr("ShiftF").ToString)
                ddlShiftX.SelectedValue = TrimStr(dr("ShiftX").ToString)
                ddlShiftA.Enabled = tbPatternFormula.Text.Contains("A")
                ddlShiftB.Enabled = tbPatternFormula.Text.Contains("B")
                ddlShiftC.Enabled = tbPatternFormula.Text.Contains("C")
                ddlShiftD.Enabled = tbPatternFormula.Text.Contains("D")
                ddlShiftE.Enabled = tbPatternFormula.Text.Contains("E")
                ddlShiftF.Enabled = tbPatternFormula.Text.Contains("F")
                ddlShiftX.Enabled = tbPatternFormula.Text.Contains("X")
                BindSimulation()
            Else
                tbPatternFormula.Text = ""
                ddlShiftA.SelectedValue = ""
                ddlShiftB.SelectedValue = ""
                ddlShiftC.SelectedValue = ""
                ddlShiftD.SelectedValue = ""
                ddlShiftE.SelectedValue = ""
                ddlShiftF.SelectedValue = ""
                ddlShiftX.SelectedValue = ""
                GridSimulasi.DataSource = Nothing
                GridSimulasi.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlPattern_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShiftA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShiftA.SelectedIndexChanged, ddlShiftB.SelectedIndexChanged, ddlShiftC.SelectedIndexChanged, ddlShiftD.SelectedIndexChanged, ddlShiftE.SelectedIndexChanged, ddlShiftF.SelectedIndexChanged, ddlShiftX.SelectedIndexChanged
        Try
            BindSimulation()
        Catch ex As Exception
            lbStatus.Text = "ddlShift_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStartIndex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartIndex.TextChanged
        Try
            BindSimulation()
        Catch ex As Exception
            lbStatus.Text = "tbStartIndex_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim hasil As String
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbemp As Label
            Dim Pertamax As Boolean
            Dim Result As String

            Pertamax = True
            Result = ""
            For Each GVR In GridView1.Rows
                CB = GVR.FindControl("cbSelect")
                lbemp = GVR.FindControl("EmpNumb")
                If CB.Checked Then
                    If Pertamax Then
                        Result = "''" + lbemp.Text.Trim + "''"
                        Pertamax = False
                    Else
                        Result = Result + ",''" + lbemp.Text.Trim + "''"
                    End If
                End If
            Next
            'Result = Result + "'"
            ViewState("StrEmp") = " And Emp_No IN ( " + Result + ")"
            If Result.ToString.Length <= 2 Then
                MoveView(0)
                lbStatus.Text = "Employee must be selected"
                Exit Sub
            End If
            hasil = SQLExecuteNonQuery("Declare @A VarChar(255) EXEC S_PYArrangeShiftRoster " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + QuotedStr(tbPatternFormula.Text.Trim) + ", " + tbStartIndex.Text.Trim + ", " + QuotedStr(ddlShiftA.SelectedValue) + ", " + _
                    QuotedStr(ddlShiftB.SelectedValue) + ", " + QuotedStr(ddlShiftC.SelectedValue) + ", " + QuotedStr(ddlShiftD.SelectedValue) + ", " + QuotedStr(ddlShiftE.SelectedValue) + ", " + QuotedStr(ddlShiftF.SelectedValue) + ", " + QuotedStr(ddlShiftX.SelectedValue) + ", 'N', '" + ViewState("StrEmp") + "', " + _
                 QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A ", ViewState("DBConnection").ToString)
            If hasil.Length >= 5 Then
                lbStatus.Text = hasil
                Exit Sub
            End If
            BindData()
        Catch ex As Exception
            lbStatus.Text = "btnOKPattern_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataSchedule()
        Dim Dt As DataTable
        Dim DV As DataView
        Dim hari As Integer
        Try
            Dt = SQLExecuteQuery("EXEC S_PYArrangeShiftSchedule " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
            End If
            Hari = CStr(DateDiff(DateInterval.Day, tbStartDate.SelectedDate, tbEndDate.SelectedDate)) + 1
            GridSchedule.PageSize = hari
            DV = Dt.DefaultView
            If ViewState("SortSchedule") = Nothing Then
                ViewState("SortSchedule") = "Emp_No ASC"
            End If
            DV.Sort = ViewState("SortSchedule")
            GridSchedule.DataSource = DV
            GridSchedule.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataSchedule Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataUnSchedule()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYArrangeShiftUnSchedule " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
            End If
            GridSchedule.PageSize = 10
            DV = Dt.DefaultView
            If ViewState("SortSchedule") = Nothing Then
                ViewState("SortSchedule") = "Emp_No ASC"
            End If
            DV.Sort = ViewState("SortSchedule")
            GridSchedule.DataSource = DV
            GridSchedule.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataUnSchedule Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataHoliday()
        Dim Dt As DataTable
        Try
            Dt = SQLExecuteQuery("Select HolidayDate, HolidayName from MsHoliday WHERE HolidayDate BETWEEN " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + " AND " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + " Order By HolidayDate", ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                pnlHoliday.Visible = False
            Else
                GridHoliday.DataSource = Dt
                GridHoliday.DataBind()
            End If
            
        Catch ex As Exception
            lbStatus.Text = "BindDataSchedule Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSchedule.Click
        Try
            ViewState("ScheduleView") = "Y"
            ddlDepartment.SelectedValue = ""
            btnCopy.Visible = False
            btnPattern.Visible = False
            btnClear.Visible = False
            MoveView(1)
            BindDataSchedule()
        Catch ex As Exception
            lbStatus.Text = "btnSchedule_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSchedule.Click
        Try
            ViewState("ScheduleView") = "N"
            ddlDepartment.SelectedValue = ""
            btnCopy.Visible = False
            btnPattern.Visible = False
            btnClear.Visible = False
            MoveView(1)
            BindDataUnSchedule()
        Catch ex As Exception
            lbStatus.Text = "btnUnSchedule_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridSchedule_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridSchedule.PageIndexChanging
        GridSchedule.PageIndex = e.NewPageIndex
        If ViewState("ScheduleView") = "N" Then
            BindDataUnSchedule()
        Else
            BindDataSchedule()
        End If
    End Sub

    Protected Sub GridSchedule_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridSchedule.Sorting
        Try
            If ViewState("SortOrder2") = Nothing Or ViewState("SortOrder2") = "DESC" Then
                ViewState("SortOrder2") = "ASC"
            Else
                ViewState("SortOrder2") = "DESC"
            End If
            ViewState("SortSchedule") = e.SortExpression + " " + ViewState("SortOrder2")
            If ViewState("ScheduleView") = "N" Then
                BindDataUnSchedule()
            Else
                BindDataSchedule()
            End If
        Catch ex As Exception
            lbStatus.Text = "GridSchedule_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridHoliday_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridHoliday.PageIndexChanging
        GridHoliday.PageIndex = e.NewPageIndex
        BindDataHoliday()
    End Sub

    Protected Sub cbHolidayOff_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbHolidayOff.CheckedChanged
        Try
            If ddlPattern.SelectedValue <> "" Then
                BindSimulation()
            End If
        Catch ex As Exception
            lbStatus.Text = "cbHolidayOff_CheckedChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ResultField As String 'ResultSame 
        Try
            Session("Result") = Nothing
            Session("Filter") = "SELECT * From MsEmployee "

            ResultField = "EmpNumb, EmpName"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnEmp"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())


        Catch ex As Exception
            lbStatus.Text = "btnSection_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFilter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFilter.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable

        Try

            DT = SQLExecuteQuery("SELECT * From MsEmployee Where EmpNumb  = " + QuotedStr(tbFilter.Text), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbFilter.Text = Dr("EmpNumb")
                tbFilterName.Text = Dr("EmpName")
            Else
                tbFilter.Text = ""
                tbFilterName.Text = ""
            End If
            BindData()
        Catch ex As Exception
            Throw New Exception("tbSection_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
