Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsMaintenanceType_MsMaintenanceType
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
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            ddlRow.SelectedValue = "15"
            'bindDataGrid()
        End If
        dsGetSection.ConnectionString = ViewState("DBConnection")
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
            DataGrid.PageSize = ddlRow.SelectedValue
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
        Dim dt As New DataTable
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            SqlString = "SELECT A.*, B.MTNSectionName FROM MsMaintenanceType A INNER JOIN VMsMaintenanceSection B ON A.MaintenanceSection = B.MTNSectionCode " + StrFilter + " Order By A.MaintenanceTypeCode"
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
            txt = obj.FindControl("MaintenanceTypeNameEdit")
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
        Dim ddlGroup As DropDownList
       
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("MaintenanceTypeCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("MaintenanceTypeNameAdd")
                ddlGroup = DataGrid.FooterRow.FindControl("MaintenanceSectionAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Maintenance Type Code must be filled.")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Maintenance Type Name must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If
                If ddlGroup.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Maintenance Section must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT MaintenanceTypeCode From VMsMaintenanceType WHERE MaintenanceTypeCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Maintenance Type " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsMaintenanceType (MaintenanceTypeCode, MaintenanceTypeName, MaintenanceSection, UserId, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlGroup.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName As Label
                Dim gvr As GridViewRow
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("MaintenanceTypeCode")
                lbName = gvr.FindControl("MaintenanceTypeName")

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Maintenance Type', '" + Request.QueryString("KeyId") + "','" + lbCode.Text + "|" + lbName.Text + "','AssMaintenanceTypeUser');", True)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlGroup As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("MaintenanceTypeCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("MaintenanceTypeNameEdit")
            ddlGroup = DataGrid.Rows(e.RowIndex).FindControl("MaintenanceSectionEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Maintenance Type Name must be filled.")
                dbName.Focus()
                Exit Sub
            End If
            If ddlGroup.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Maintenance Section must be filled.")
                ddlGroup.Focus()
                Exit Sub
            End If


            SQLString = "UPDATE MsMaintenanceType set MaintenanceTypeName = " + QuotedStr(dbName.Text) + ", " & _
            " MaintenanceSection = " + QuotedStr(ddlGroup.SelectedValue) + _
            " WHERE MaintenanceTypeCode = " & QuotedStr(lbCode.Text)

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

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MaintenanceTypeCode")

            SQLExecuteNonQuery("DELETE FROM MsMaintenanceType WHERE MaintenanceTypeCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'VMsMaintenanceType','MaintenanceTypeCode','MaintenanceTypeName','MTNSectionName','Maintenance Type File','Maintenance Type Code','Maintenance Type Name','Maintenance Section'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
