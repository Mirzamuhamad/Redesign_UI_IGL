Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsAccountSetup_MsAccountSetup
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

            'bindDataGrid()
        End If
        dsGroupRpt.ConnectionString = ViewState("DBConnection")

        Dim GroupCode As New TextBox
        Dim GroupName As New Label
        If Not Session("Result") Is Nothing Then
            If Session("Sender") = "SearchAdd" Or Session("Sender") = "SearchEdit" Then
                If Session("Sender") = "SearchAdd" Then
                    GroupCode = DataGrid.FooterRow.FindControl("GroupCodeAdd")
                    GroupName = DataGrid.FooterRow.FindControl("GroupNameAdd")
                Else
                    GroupCode = DataGrid.Rows(DataGrid.EditIndex).FindControl("GroupCodeEdit")
                    GroupName = DataGrid.Rows(DataGrid.EditIndex).FindControl("GroupNameEdit")
                End If
            End If

            GroupCode.Text = Session("Result")(0).ToString
            GroupName.Text = Session("Result")(1).ToString
            GroupCode.Focus()

            Session("Result") = Nothing
            Session("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
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
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM VMsSetupAccount " + StrFilter + " ORDER BY GroupRpt "
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
        Dim Type As DropDownList
        Dim GroupCode As TextBox
        Dim txt As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("ddlGrpCodeEdit")
            Type = obj.FindControl("ddlTypeEdit")
            GroupCode = obj.FindControl("GroupCodeEdit")
            Type.Focus()
            ViewState("Key") = txt.Text + "|" + Type.SelectedValue + "|" + GroupCode.Text
            ViewState("GroupRpt") = txt.Text
            ViewState("Type") = Type.SelectedValue
            ViewState("GroupCode") = GroupCode.Text
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
        Dim dbGroupCode As TextBox
        Dim dbGroupName As Label
        Dim ddlGroupRpt, ddlType As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                ddlGroupRpt = DataGrid.FooterRow.FindControl("ddlGroupRptAdd")
                ddlType = DataGrid.FooterRow.FindControl("ddlTypeAdd")
                dbGroupCode = DataGrid.FooterRow.FindControl("GroupCodeAdd")
                dbGroupName = DataGrid.FooterRow.FindControl("GroupNameAdd")

                If ddlGroupRpt.SelectedValue.Trim.Length = 0 Then
                    lstatus.Text = "Group Rpt must be filled."
                    ddlGroupRpt.Focus()
                    Exit Sub
                End If
                If ddlType.SelectedValue.Trim.Length = 0 Then
                    lstatus.Text = "Type must be filled."
                    ddlType.Focus()
                    Exit Sub
                End If
                If dbGroupCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Group Code must be filled."
                    dbGroupCode.Focus()
                    Exit Sub
                End If
                If dbGroupName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Group Name must be filled."
                    dbGroupName.Focus()
                    Exit Sub
                End If

                SQLString = "SELECT GroupRpt, Type, GroupCode FROM MsAccountSetup WHERE GroupRpt = " + QuotedStr(ddlGroupRpt.SelectedValue) + " AND Type = " + QuotedStr(ddlType.SelectedValue) + " AND GroupCode = " + QuotedStr(dbGroupCode.Text)
                If SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Data Exist, cannot insert data")
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsAccountSetup (GroupRpt, Type, GroupCode, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ddlGroupRpt.SelectedValue) + ", " + QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(dbGroupCode.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Session("DBConnection") = ViewState("DBConnection")
                Dim FieldResult As String
                If e.CommandName = "SearchAdd" Then
                    ddlType = DataGrid.FooterRow.FindControl("ddlTypeAdd")
                    Session("filter") = "SELECT Code, Description FROM VMsAccGroupUnion WHERE Type = " + QuotedStr(ddlType.SelectedValue)
                Else
                    GVR = DataGrid.Rows(DataGrid.EditIndex)
                    ddlType = GVR.FindControl("ddlTypeEdit")
                    Session("filter") = "SELECT Code, Description FROM VMsAccGroupUnion WHERE Type = " + QuotedStr(ddlType.SelectedValue)
                End If
                FieldResult = "Code, Description"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAdd" Then
                    Session("Sender") = "SearchAdd"
                Else
                    Session("Sender") = "SearchEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
                End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbGroupCode As TextBox
        Dim lbCode As Label
        Dim ddlType As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("ddlGrpCodeEdit")
            dbGroupCode = DataGrid.Rows(e.RowIndex).FindControl("GroupCodeEdit")
            ddlType = DataGrid.Rows(e.RowIndex).FindControl("ddlTypeEdit")

            If ddlType.SelectedValue.Trim.Length = 0 Then
                lstatus.Text = "Type must be filled."
                ddlType.Focus()
                Exit Sub
            End If
            If dbGroupCode.Text.Trim.Length = 0 Then
                lstatus.Text = "Group Code must be filled."
                dbGroupCode.Focus()
                Exit Sub
            End If

            If ViewState("Key") <> lbCode.Text + "|" + ddlType.SelectedValue + "|" + dbGroupCode.Text Then
                SQLString = "SELECT GroupRpt, Type, GroupCode FROM MsAccountSetup WHERE GroupRpt = " + QuotedStr(lbCode.Text) + " AND Type = " + QuotedStr(ddlType.SelectedValue) + " AND GroupCode = " + QuotedStr(dbGroupCode.Text)
                If SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Data Exist, cannot insert data")
                    Exit Sub
                End If
            End If

            SQLString = "Update MsAccountSetup set Type= " + QuotedStr(ddlType.SelectedValue) + "," & _
            "GroupCode = " + QuotedStr(dbGroupCode.Text) + " where GroupRpt = " & QuotedStr(lbCode.Text) & " AND Type = " & QuotedStr(ViewState("Type")) & " AND GroupCode = " & QuotedStr(ViewState("GroupCode"))

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID, txtType, txtGroupCode As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("GrpCode")
            txtType = DataGrid.Rows(e.RowIndex).FindControl("Type")
            txtGroupCode = DataGrid.Rows(e.RowIndex).FindControl("GroupCode")

            SQLExecuteNonQuery("DELETE FROM MsAccountSetup WHERE GroupRpt= '" + txtID.Text + "' AND Type = '" + txtType.Text + "' AND GroupCode = '" + txtGroupCode.Text + "'", ViewState("DBConnection").ToString)
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
            Session("SelectCommand") = "S_FormPrintMaster3 'VMsSetupAccount','GroupAlias','Type','GroupName','Account Classification','Setup Code','Type','Group'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbGroupCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim GroupCode, tb As TextBox
        Dim GroupName As Label
        Dim Count As Integer
        Dim ddlType As DropDownList
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "GroupCodeAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                GroupCode = dgi.FindControl("GroupCodeAdd")
                GroupName = dgi.FindControl("GroupNameAdd")
                ddlType = dgi.FindControl("ddlTypeAdd")
                ds = SQLExecuteQuery("SELECT * FROM VMsAccGroupUnion WHERE Type = " + QuotedStr(ddlType.SelectedValue) + " AND Code = " + QuotedStr(GroupCode.Text), ViewState("DBConnection").ToString)
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                GroupCode = dgi.FindControl("GroupCodeEdit")
                GroupName = dgi.FindControl("GroupNameEdit")
                ddlType = dgi.FindControl("ddlTypeEdit")
                ds = SQLExecuteQuery("SELECT * FROM VMsAccGroupUnion WHERE Type = " + QuotedStr(ddlType.SelectedValue) + " AND Code = " + QuotedStr(GroupCode.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                GroupCode.Text = ""
                GroupName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                GroupCode.Text = dr("Code").ToString
                GroupName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tbGroupCode_TextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GroupCode As TextBox
        Dim GroupName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tb As DropDownList
        Try
            tb = sender
            If tb.ID = "ddlTypeAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                GroupCode = dgi.FindControl("GroupCodeAdd")
                GroupName = dgi.FindControl("GroupNameAdd")

                GroupCode.Text = ""
                GroupName.Text = ""
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                GroupCode = dgi.FindControl("GroupCodeEdit")
                GroupName = dgi.FindControl("GroupNameEdit")

                GroupCode.Text = ""
                GroupName.Text = ""
            End If

        Catch ex As Exception
            lstatus.Text = "tbGroupCode_TextChanged Changed Error : " + ex.ToString
        End Try
    End Sub
End Class
