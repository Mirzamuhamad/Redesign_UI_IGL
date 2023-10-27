Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsDivision_MsDivision
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

            'bindDataGrid()
        End If
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
        Dim StrFilter, SqlString, MenuParam As String
        Dim dt As New DataTable
        Try
            StrFilter = GenerateFilter(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            If StrFilter <> "" Then
                MenuParam = " AND DeptLevel = " + Request.QueryString("MenuParam")
            Else
                MenuParam = " WHERE DeptLevel = " + Request.QueryString("MenuParam")
            End If
            SqlString = "SELECT * FROM MsDepartment " + StrFilter + MenuParam + " Order By DeptCode"
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)
            ViewState("Div") = dt

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
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
        Dim txt As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("DivNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        'Dim LblItem As Label
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("DivCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("DivNameAdd")
                'LblItem = DataGrid.FooterRow.FindControl("ItemNoAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Code must be filled.")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Description must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Division_Code From VMsDivision  WHERE Division_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Division " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                Dim i As Integer
                Dim dt As New DataTable
                Dim Cek As String
                Dim Row As DataRow()

                Cek = "SELECT Division_Code, Division_Name, DeptLevel FROM VMsDivision Order By Division_Code"
                dt = SQLExecuteQuery(Cek, ViewState("DBConnection")).Tables(0)

                If dt.Rows.Count > 0 Then
                    Row = ViewState("Div").select("")
                    i = Row.Length
                Else
                    Row = Nothing
                    i = 0
                End If

                'If i > 0 Then
                'LblItem.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
                'Else
                'LblItem.Text = "1"
                'End If

                'insert the new entry
                SQLString = "Insert into MsDepartment (DeptCode, DeptName, DeptLevel, DeptGroup, FgDriver, DirectType, UserId, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + Request.QueryString("MenuParam") + ", NULL, NULL, NULL, " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("DivCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("DivNameEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Description must be filled.")
                dbName.Focus()
                Exit Sub
            End If


            SQLString = "UPDATE MsDepartment set DeptName= " + QuotedStr(dbName.Text) + "" & _
            " WHERE DeptCode = " & QuotedStr(lbCode.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Dim dt As New DataTable
        Dim Cek As String
        Dim Dr As DataRow
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("DivCode")
            
            Cek = "SELECT Dept_Code, Dept_Name FROM VMsDepartment WHERE Group_Code = " + QuotedStr(txtID.Text)
            dt = SQLExecuteQuery(Cek, ViewState("DBConnection")).Tables(0)

            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                lstatus.Text = MessageDlg("Can not delete, Division has already use by Department " + QuotedStr(Dr("Dept_Name")))
                Exit Sub
            End If

            SQLExecuteNonQuery("DELETE FROM MsDepartment WHERE DeptCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Dim SqlString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            SqlString = "S_FormPrintMaster 'VMsDivision','Division_Code','Division_Name'," + QuotedStr(lblTitle.Text) + ",'Code','Description'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SqlString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Public Function GenerateFilter(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If

            If StrFilter <> "" Then
                StrFilter = " Where (" + StrFilter + ")"
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterMs Error : " + ex.ToString)
        End Try
    End Function


End Class
