Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsEmployeeMethod_MsEmployeeMethod
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            'bindDataGrid()
            If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                lbjudul.Text = "Setting Employee Method Salary"
                FillCombo(ddlMethod, "Select MethodCode, MethodName FROM MsMethod ", True, "MethodCode", "MethodName", ViewState("DBConnection"))
            Else
                lbjudul.Text = "Setting Employee Method THR"
                FillCombo(ddlMethod, "Select MethodTHRCode, MethodTHRName FROM MsMethodTHR ", True, "MethodTHRCode", "MethodTHRName", ViewState("DBConnection"))
            End If
        End If
        dsMethodSalary.ConnectionString = ViewState("DBConnection")
        dsMethodTHR.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM V_MsEmployeeMethod " + StrFilter
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                DataGrid.Columns(7).Visible = False
            Else
                DataGrid.Columns(6).Visible = False
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim ddl As DropDownList
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                ddl = obj.FindControl("MethodSalaryEdit")
            Else
                ddl = obj.FindControl("MethodTHREdit")
            End If
            ddl.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        'Dim SQLString As String
        'Dim dbCode, dbName, dbDays As TextBox
        Try
            If e.CommandName = "Insert" Then
               
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(DataGrid, sender)
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim lbCode As Label
        Dim CbMethod As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("EmpNumbEdit")
            If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                CbMethod = DataGrid.Rows(e.RowIndex).FindControl("MethodSalaryEdit")
                SQLString = "UPDATE MsEmployee SET MethodSalary = " + QuotedStr(CbMethod.SelectedValue) + " WHERE EmpNumb = " & QuotedStr(lbCode.Text)
            Else
                CbMethod = DataGrid.Rows(e.RowIndex).FindControl("MethodTHREdit")
                SQLString = "UPDATE MsEmployee SET MethodTHR = " + QuotedStr(CbMethod.SelectedValue) + " WHERE EmpNumb = " & QuotedStr(lbCode.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("EmpNumb")
            If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                SQLExecuteNonQuery("UPDATE MsEmployee SET MethodSalary = NULL WHERE EmpNumb = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            Else
                SQLExecuteNonQuery("UPDATE MsEmployee SET MethodTHR = NULL WHERE EmpNumb = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            End If
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim StrFilter As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        Session("PrintType") = "Print"
    '        Session("SelectCommand") = "S_FormPrintMaster3 'MsEmployeeMethod','DurationCode','DurationName','Days','Maintenance Interval File','Period Code','Period Name','Days'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "btn Print Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub bindDataSetMethod()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lb As Label
            Dim HaveSelect As Boolean
            Dim SQLString As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lb = GVR.FindControl("EmpNumb")
                If CB.Checked Then
                    HaveSelect = True
                    If Request.QueryString("ContainerId").ToString = "MsEmpMethodSalaryID" Then
                        SQLString = "Update MsEmployee SET MethodSalary = " + QuotedStr(ddlMethod.SelectedValue) + " WHERE EmpNumb = " + QuotedStr(lb.Text.Trim)
                    Else
                        SQLString = "Update MsEmployee SET MethodTHR = " + QuotedStr(ddlMethod.SelectedValue) + " WHERE EmpNumb = " + QuotedStr(lb.Text.Trim)
                    End If
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            If HaveSelect = False Then
                lstatus.Text = "Please Check Employee for Setting Method"
                Exit Sub
            Else
                lstatus.Text = "Setting Method Success for Selected Employee"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataGridsetMethod Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            If ddlMethod.SelectedValue = "" Then
                lstatus.Text = "Method must be selected"
                ddlMethod.Focus()
                Exit Sub
            End If
            bindDataSetMethod()
        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub
End Class
