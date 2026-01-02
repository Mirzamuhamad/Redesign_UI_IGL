Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_SAUsers_SAUsers
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
        dsUserGroup.ConnectionString = ViewState("DBConnection")
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
            'tbFilter.Text = ""
            'tbfilter2.Text = ""
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
        Dim GVR As GridViewRow
        Dim fgAdmin As Label
        Dim btnReset As Button

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'lstatus.Text = StrFilter
            'SqlString = "Select A.*, B.UserGrpName As UserGroupName from SAUsers A LEFT OUTER JOIN SAUserGroup B On A.UserGroup = B.UserGrpCode" + StrFilter
            SqlString = "EXEC S_SAUserData " + QuotedStr(StrFilter) + ", '123!@#' "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "UserId ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            For Each GVR In DataGrid.Rows
                fgAdmin = GVR.FindControl("FgAdmin")
                btnReset = GVR.FindControl("btnReset")

                If Not fgAdmin Is Nothing Then
                    If fgAdmin.Text = "Y" Then
                        btnReset.Visible = False
                    End If
                End If
            Next
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'SAUsers','UserID','UserName','FgActive','User File','User Code','User Name','Active'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("DBConnection"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterSaUsers.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
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

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName, dbpassword As TextBox
        Dim cbxFgPeriod, cbxFgActive, ddluserGroup As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("UserIDAdd")
                dbName = DataGrid.FooterRow.FindControl("UserNameAdd")
                dbpassword = DataGrid.FooterRow.FindControl("UserPassword1Add")
                ddluserGroup = DataGrid.FooterRow.FindControl("UserGroupAdd")
                cbxFgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")
                cbxFgPeriod = DataGrid.FooterRow.FindControl("FgChangePeriodAdd")


                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "User ID must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "User Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If dbpassword.Text.Trim.Length < 4 Then
                    lstatus.Text = "User Password must be filled (min length 4 digit)."
                    dbpassword.Focus()
                    Exit Sub
                End If
                If ddluserGroup.Text.Trim.Length = 0 Then
                    lstatus.Text = "User Group Name must be filled."
                    ddluserGroup.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT User_Id From vSAUsers WHERE User_Id = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "User Id " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If


                'insert the new entry
                SQLString = "Insert into SAUsers (UserID, UserName, UserPassword1, UserGroup, FgActive, FgChangePeriod, FgAdmin, CreateBy, CreateDate ) " + _
                "SELECT '" + dbCode.Text.Replace("'", "''") + "', '" + dbName.Text.Replace("'", "''") + "', dbo.Cryption(" + QuotedStr(dbpassword.Text) + ",88), " + QuotedStr(ddluserGroup.SelectedValue) + ", " + QuotedStr(cbxFgActive.SelectedValue) + ", " + QuotedStr(cbxFgPeriod.SelectedValue) + ", 'N','" + ViewState("UserId").ToString + "', getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
            If e.CommandName = "Reset" Then
                Dim GVR As GridViewRow = Nothing
                Dim lbCode As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("UserID")

                SQLString = "IF EXISTS ( SELECT UserId FROM SAUsers WHERE UserID = " + QuotedStr(lbCode.Text) + " AND FgAdmin = 'N' ) BEGIN " + _
                            "  Update SAUsers set UserPassword1 = dbo.Cryption('" + lbCode.Text + ".123',88) where UserID = " + QuotedStr(lbCode.Text) + " END "
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                lstatus.Text = MessageDlg("Succes Reset Pasword  " + lbCode.Text + ".123")

                DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGrid.EditIndex = -1
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("UserID")

            SQLExecuteNonQuery("Delete from SAUsers where UserID = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        'Dim dbPwd As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("UserNameEdit")
            'dbPwd = obj.FindControl("UserPassword1Edit")
            'dbPwd.TextMode = TextBoxMode.Password
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        'Dim dbPwd As TextBox
        Dim lbCode As Label
        Dim CbxFgActive, CbxFgPeriod, ddlUserGroup As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("UserIDEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("UserNameEdit")
            'dbPwd = DataGrid.Rows(e.RowIndex).FindControl("UserPassword1Edit")
            ddlUserGroup = DataGrid.Rows(e.RowIndex).FindControl("UserGroupEdit")
            CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")
            CbxFgPeriod = DataGrid.Rows(e.RowIndex).FindControl("FgChangePeriodEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "User Name must be filled."
                dbName.Focus()
                Exit Sub
            End If
            'If dbPwd.Text.Trim.Length < 4 Then
            '    lstatus.Text = "User Password must be filled (min length 4 digit)."
            '    dbPwd.Focus()
            '    Exit Sub
            'End If
            'If ddlUserGroup.SelectedValue = 0 Then
            '    lstatus.Text = "User Group must be filled."
            '    ddlUserGroup.Focus()
            '    Exit Sub
            'End If
            'UserPassword1 = dbo.Cryption(" + QuotedStr(dbPwd.Text) + ",88), 
            SQLString = "Update SAUsers set UserName= " + QuotedStr(dbName.Text) + ", FgActive = " + QuotedStr(CbxFgActive.Text) + ", UserGroup = " + QuotedStr(ddlUserGroup.Text) + ", FgChangePeriod = " + QuotedStr(CbxFgPeriod.SelectedValue) + " where UserID = " & QuotedStr(lbCode.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
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
End Class
