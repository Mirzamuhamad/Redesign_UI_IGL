Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO

Partial Class Transaction_TrPYAbsImport_TrPYAbsImport
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
            End If
            lbStatus.Text = ""
            If Not Session("Result") Is Nothing Then
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
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
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub MoveView(ByVal page As Integer)
        pnlImport.Visible = False
        pnlSorted.Visible = False
        PnlHd.Visible = False
        pnlArrange.Visible = False
        pnlAbsenceNIS.Visible = False
        pnlScheduleNIS.Visible = False
        pnlImportDb.Visible = False

        If page = 0 Then
            PnlHd.Visible = True
        ElseIf page = 1 Then
            pnlImport.Visible = True
        ElseIf page = 2 Then
            pnlSorted.Visible = True
        ElseIf page = 3 Then
            pnlArrange.Visible = True
        ElseIf page = 4 Then
            pnlAbsenceNIS.Visible = True
        ElseIf page = 5 Then
            pnlScheduleNIS.Visible = True
        ElseIf page = 6 Then
            pnlImportDb.Visible = True
        End If
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        DataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "YES")
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            DataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "YES")
            'BindData(Session("AdvanceFilter"))
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

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim dt As DataTable
        Try
            If ddlFieldNIK.SelectedValue = "" Then
                lbStatus.Text = "Field Absence Card must be selected"
                ddlFieldNIK.Focus()
                Exit Sub
            End If
            If ddlFieldDate.SelectedValue = "" Then
                lbStatus.Text = "Field Date Log must be selected"
                ddlFieldDate.Focus()
                Exit Sub
            End If
            If ddlFieldTime.SelectedValue = "" Then
                lbStatus.Text = "Field Time Log must be selected"
                ddlFieldTime.Focus()
                Exit Sub
            End If
            dt = GetDataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "Yes")
            Dim Waktu As String
            Dim tgl As DateTime
            Dim dr As DataRow
            For Each dr In dt.Rows()
                If dr(ddlFieldNIK.SelectedValue).ToString.Length >= 1 And IsDate(dr(ddlFieldDate.SelectedValue)) And IsDate(dr(ddlFieldDate.SelectedValue)) Then
                    Try
                        tgl = CDate(dr(ddlFieldTime.SelectedValue))
                    Catch ex As Exception
                        tgl = CDate(dr(ddlFieldDate.SelectedValue))
                    End Try
                    Waktu = Right("0" + CStr(tgl.Hour).Trim, 2) + ":" + Right("0" + CStr(tgl.Minute).Trim, 2)
                    SQLExecuteNonQuery("EXEC S_PYAbsImportUpdate " + QuotedStr(dr(ddlFieldNIK.SelectedValue)) + ", " + QuotedStr(dr(ddlFieldDate.SelectedValue)) + ", " + QuotedStr(Waktu), ViewState("DBConnection"))
                End If
            Next
            MoveView(1)
            BindDataImport()
            'EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btnGenerate_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub EnableHd(ByVal Bool As Boolean)
        Try
            'btnCopy.Visible = Bool
            'btnPattern.Visible = Bool
            'btnClear.Visible = Bool
            'btnUnSchedule.Visible = Bool
            'ddlDepartment.Enabled = Bool
            tbStartDate.Enabled = Bool
            tbEndDate.Enabled = Bool
        Catch ex As Exception
            lbStatus.Text = "EnableHd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataImport()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYAbsImportData '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count = 0 Then
            '    lbStatus.Text = "No Data"
            'End If
            DV = Dt.DefaultView
            If ViewState("SortImport") = Nothing Then
                ViewState("SortImport") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = Dt
                ShowGridViewIfEmpty(DT2, GridImport)
            Else
                DV.Sort = ViewState("SortImport")
                GridImport.DataSource = DV
                GridImport.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "BindDataImport Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataImportDb()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYAbsImportData '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count = 0 Then
            '    lbStatus.Text = "No Data"
            'End If
            DV = Dt.DefaultView
            If ViewState("SortImportDb") = Nothing Then
                ViewState("SortImportDb") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = Dt
                ShowGridViewIfEmpty(DT2, GridImportDb)
            Else
                DV.Sort = ViewState("SortImportDb")
                GridImportDb.DataSource = DV
                GridImportDb.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "BindDataImportDb Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataSort(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Try
            DT = SQLExecuteQuery("S_PYAbsDailyBindSort ", ViewState("DBConnection").ToString).Tables(0)
            DV = DT.DefaultView
            If ViewState("SortSorted") = Nothing Then
                ViewState("SortSorted") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = DT
                ShowGridViewIfEmpty(DT2, GridSorted)
            Else
                DV.Sort = ViewState("SortSorted")
                GridSorted.DataSource = DV
                GridSorted.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Sort Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataArrange(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Try
            DT = SQLExecuteQuery("S_PYAbsDailyBindArrange '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString).Tables(0)
            DV = DT.DefaultView
            If ViewState("SortArrange") = Nothing Then
                ViewState("SortArrange") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = DT
                'ShowGridViewIfEmpty(DT2, GridArrange)
            Else
                DV.Sort = ViewState("SortArrange")
                GridArrange.DataSource = DV
                GridArrange.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Arrange Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BindDataAbsence()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYAbsDailyBindCek1 '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count = 0 Then
            '    lbStatus.Text = "No Data"
            'End If
            DV = Dt.DefaultView
            If ViewState("SortAbsence") = Nothing Then
                ViewState("SortAbsence") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = Dt
                ShowGridViewIfEmpty(DT2, GridAbsenceNIS)
            Else
                DV.Sort = ViewState("SortAbsence")
                GridAbsenceNIS.DataSource = DV
                GridAbsenceNIS.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "BindDataAbsence Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataSchedule()
        Dim Dt As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYAbsDailyBindCek2 '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count = 0 Then
            '    lbStatus.Text = "No Data"
            'End If
            DV = Dt.DefaultView
            If ViewState("SortSchedule") = Nothing Then
                ViewState("SortSchedule") = "EmpNumb ASC"
            End If
            If DV.Count = 0 Then
                Dim DT2 As DataTable = Dt
                ShowGridViewIfEmpty(DT2, GridScheduleNIS)
            Else
                DV.Sort = ViewState("SortSchedule")
                GridScheduleNIS.DataSource = DV
                GridScheduleNIS.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "BindDataSchedule Error : " + ex.ToString
        End Try
    End Sub

    Private Sub GetExcelSheets(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03 
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                Exit Select
            Case ".xlsx"
                'Excel 07 
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        'Get the Sheets in Excel WorkBoo 
        conStr = String.Format(conStr, FilePath, isHDR)
        'lbStatus.Text = conStr
        'Exit Sub
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        cmdExcel.Connection = connExcel
        If connExcel.State = ConnectionState.Closed Then
            connExcel.Open()
        End If
        'Bind the Sheets to DropDownList 
        ddlSheets.Items.Clear()
        'ddlSheets.Items.Add(New ListItem("--Select Sheet--", ""))
        ddlSheets.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        ddlSheets.DataTextField = "TABLE_NAME"
        ddlSheets.DataValueField = "TABLE_NAME"
        ddlSheets.DataBind()
        connExcel.Close()

        ddlSheets.SelectedIndex = 0
        DataExcel(ddlSheets.SelectedValue, FilePath, Extension, isHDR)
        'txtTable.Text = ""
        'lblFileName.Text = Path.GetFileName(FilePath)
        'Dim query As String = "SELECT [Nik],[Nama],[datelog], [timelog] FROM [" + ddlSheets.SelectedValue + "]"
        'Dim conn As New OleDbConnection(conStr)
        'If conn.State = ConnectionState.Closed Then
        '    conn.Open()
        'End If
        'Dim cmd As New OleDbCommand(query, conn)
        'Dim da As New OleDbDataAdapter(cmd)
        'Dim ds As New DataSet()
        'da.Fill(ds)
        'GridView1.DataSource = ds.Tables(0)
        'GridView1.DataBind()
        'da.Dispose()
        'conn.Close()
        'conn.Dispose()
    End Sub

    Function GetDataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String) As DataTable
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-03 
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    Exit Select
                Case ".xlsx"
                    'Excel 07 
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            lbStatus.Text = "DataExcel Error : " + ex.ToString
            Return Nothing
        End Try
    End Function

    Private Sub DataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-03 
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    Exit Select
                Case ".xlsx"
                    'Excel 07 
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            GridView1.DataSource = ds.Tables(0)
            GridView1.DataBind()

            Dim count As Integer
            count = ds.Tables(0).Columns.Count - 1
            ddlFieldNIK.Items.Clear()
            ddlFieldNIK.Items.Add(New ListItem("--Choose One--", ""))
            ddlFieldDate.Items.Clear()
            ddlFieldDate.Items.Add(New ListItem("--Choose One--", ""))
            ddlFieldTime.Items.Clear()
            ddlFieldTime.Items.Add(New ListItem("--Choose One--", ""))
            For j = 0 To count
                ddlFieldNIK.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFieldDate.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFieldTime.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
            Next
            da.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            lbStatus.Text = "DataExcel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If fileuploadExcel.HasFile Then
            Dim FileName As String = Path.GetFileName(fileuploadExcel.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fileuploadExcel.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            Dim FilePath As String = Server.MapPath("~/ImportExcel/" + FileName)
            fileuploadExcel.SaveAs(FilePath)
            ViewState("FilePath") = FilePath
            ViewState("Extension") = Extension
            GetExcelSheets(FilePath, Extension, "Yes")
        End If
    End Sub

    Protected Sub ddlSheets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSheets.SelectedIndexChanged
        Try
            DataExcel(ddlSheets.SelectedValue, ViewState("FilePath"), ViewState("Extension"), "Yes")
            MoveView(0)
        Catch ex As Exception
            lbStatus.Text = "ddlSheets_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridImport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridImport.PageIndexChanging
        Try
            GridImport.PageIndex = e.NewPageIndex
            BindDataImport()
        Catch ex As Exception
            lbStatus.Text = "GridImport_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridImport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridImport.Sorting
        Try
            If ViewState("SortOrder2") = Nothing Or ViewState("SortOrder2") = "DESC" Then
                ViewState("SortOrder2") = "ASC"
            Else
                ViewState("SortOrder2") = "DESC"
            End If
            ViewState("SortImport") = e.SortExpression + " " + ViewState("SortOrder2")
            BindDataImport()
        Catch ex As Exception
            lbStatus.Text = "GridImport_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ExecuteSort()
        Try
            SQLExecuteNonQuery("EXEC S_PYAbsDailyExecSort " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbStatus.Text = "ExecuteSort Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSorted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSorted.Click
        Try
            MoveView(2)
            ExecuteSort()
            BindDataSort()
        Catch ex As Exception
            lbStatus.Text = "btnSorted_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridSorted_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridSorted.PageIndexChanging
        Try
            GridSorted.PageIndex = e.NewPageIndex
            BindDataSort()
        Catch ex As Exception
            lbStatus.Text = "GridSorted_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridSorted_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridSorted.Sorting
        Try
            If ViewState("SortOrder3") = Nothing Or ViewState("SortOrder3") = "DESC" Then
                ViewState("SortOrder3") = "ASC"
            Else
                ViewState("SortOrder3") = "DESC"
            End If
            ViewState("SortSorted") = e.SortExpression + " " + ViewState("SortOrder3")
            BindDataSort()
        Catch ex As Exception
            lbStatus.Text = "GridImport_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ExecuteArrange()
        Try
            SQLExecuteNonQuery("EXEC S_PYAbsDailyExecArrange '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbStatus.Text = "ExecuteArrange Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ExecuteImportDb()
        Try
            SQLExecuteNonQuery("EXEC S_PYAbsDailyExecImport '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbStatus.Text = "ExecuteArrange Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnArrange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArrange.Click
        Try
            MoveView(3)
            ExecuteArrange()
            BindDataArrange()
        Catch ex As Exception
            lbStatus.Text = "btnArrange_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            MoveView(1)
            BindDataImport()
        Catch ex As Exception
            lbStatus.Text = "btnImport_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnImportDb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportDb.Click
        Try
            MoveView(6)
            ExecuteImportDb()
            BindDataImportDb()
        Catch ex As Exception
            lbStatus.Text = "btnImportDb_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAbsence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAbsence.Click
        Try
            MoveView(4)
            BindDataAbsence()
        Catch ex As Exception
            lbStatus.Text = "btnAbsence_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSchedule.Click
        Try
            MoveView(5)
            BindDataSchedule()
        Catch ex As Exception
            lbStatus.Text = "btnSchedule_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridArrange_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridArrange.PageIndexChanging
        Try
            GridArrange.PageIndex = e.NewPageIndex
            BindDataSort()
        Catch ex As Exception
            lbStatus.Text = "GridArrange_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridArrange_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridArrange.Sorting
        Try
            If ViewState("SortOrder4") = Nothing Or ViewState("SortOrder4") = "DESC" Then
                ViewState("SortOrder4") = "ASC"
            Else
                ViewState("SortOrder4") = "DESC"
            End If
            ViewState("SortArrange") = e.SortExpression + " " + ViewState("SortOrder4")
            BindDataArrange()
        Catch ex As Exception
            lbStatus.Text = "GridArrange_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridAbsenceNIS_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridAbsenceNIS.PageIndexChanging
        Try
            GridAbsenceNIS.PageIndex = e.NewPageIndex
            BindDataAbsence()
        Catch ex As Exception
            lbStatus.Text = "GridAbsenceNIS_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridAbsenceNIS_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridAbsenceNIS.Sorting
        Try
            If ViewState("SortOrder5") = Nothing Or ViewState("SortOrder5") = "DESC" Then
                ViewState("SortOrder5") = "ASC"
            Else
                ViewState("SortOrder5") = "DESC"
            End If
            ViewState("SortAbsence") = e.SortExpression + " " + ViewState("SortOrder5")
            BindDataAbsence()
        Catch ex As Exception
            lbStatus.Text = "GridAbsenceNIS_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridScheduleNIS_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridScheduleNIS.PageIndexChanging
        Try
            GridScheduleNIS.PageIndex = e.NewPageIndex
            BindDataSchedule()
        Catch ex As Exception
            lbStatus.Text = "GridShceduleNIS_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridScheduleNIS_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridScheduleNIS.Sorting
        Try
            If ViewState("SortOrder6") = Nothing Or ViewState("SortOrder6") = "DESC" Then
                ViewState("SortOrder6") = "ASC"
            Else
                ViewState("SortOrder6") = "DESC"
            End If
            ViewState("SortSchedule") = e.SortExpression + " " + ViewState("SortOrder6")
            BindDataSchedule()
        Catch ex As Exception
            lbStatus.Text = "GridShceduleNIS_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridImportDb_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridImportDb.PageIndexChanging
        Try
            GridImportDb.PageIndex = e.NewPageIndex
            BindDataImportDb()
        Catch ex As Exception
            lbStatus.Text = "GridImportDb_PageIndexChanging Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridImportDb_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridImportDb.Sorting
        Try
            If ViewState("SortOrder7") = Nothing Or ViewState("SortOrder7") = "DESC" Then
                ViewState("SortOrder7") = "ASC"
            Else
                ViewState("SortOrder7") = "DESC"
            End If
            ViewState("SortImportDb") = e.SortExpression + " " + ViewState("SortOrder7")
            BindDataImportDb()
        Catch ex As Exception
            lbStatus.Text = "GridImportDb_Sorting Error : " + ex.ToString
        End Try
    End Sub
End Class
