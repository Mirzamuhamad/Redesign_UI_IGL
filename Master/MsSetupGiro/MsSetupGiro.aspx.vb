Imports System.Data
Partial Class Master_MsSetupGiro_MsSetupGiro
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
            ddlType.Items.Add("A/R")
            ddlType.Items.Add("A/P")
        End If
        dsCurrency.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then
            Dim Acc As TextBox
            Dim Desc As Label
            If ViewState("Sender") = "SearchAdd" Then
                Acc = DataGrid.FooterRow.FindControl("AccountAdd")
                Desc = DataGrid.FooterRow.FindControl("DescriptionAdd")
            Else
                Acc = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountEdit")
                Desc = DataGrid.Rows(DataGrid.EditIndex).FindControl("DescriptionEdit")
            End If

            Acc.Text = Session("Result")(0).ToString
            Desc.Text = Session("Result")(1).ToString

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
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

    Private Sub bindDataGrid()
        Dim SqlString As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        Try
            SqlString = "SELECT A.*, B.Description FROM MsSetupGiro A INNER JOIN VMsAccount B ON A.Account = B.Account WHERE Type =" + QuotedStr(ddlType.SelectedValue)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
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
            ViewState("SortExpression") = e.SortExpression
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Try
            If e.CommandName = "Insert" Then
                Dim tbAcc As TextBox
                Dim ddlCurr As DropDownList

                ddlCurr = DataGrid.FooterRow.FindControl("CurrencyAdd")
                tbAcc = DataGrid.FooterRow.FindControl("AccountAdd")


                If tbAcc.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Account must be filled.")
                    tbAcc.Focus()
                    Exit Sub
                End If
                SQLString = "SELECT Type From MsSetupGiro WHERE Type = " + QuotedStr(ddlType.SelectedValue) + " AND Currency = " + QuotedStr(ddlCurr.SelectedValue)
                If SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Data Exist, cannot insert data")
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsSetupGiro (Type, Currency, Account, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(ddlCurr.SelectedValue) + ", " + _
                QuotedStr(tbAcc.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Dim FieldResult As String
                Dim ddlCurr As DropDownList
                If e.CommandName = "SearchAdd" Then
                    ViewState("Sender") = "SearchAdd"
                    ddlCurr = DataGrid.FooterRow.FindControl("CurrencyAdd")
                    Session("filter") = "Select * FROM VMsAccount WHERE Currency=" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(ddlCurr.SelectedValue)
                Else
                    ViewState("Sender") = "SearchEdit"
                    ddlCurr = DataGrid.Rows(DataGrid.EditIndex).FindControl("CurrencyEdit")
                    Session("filter") = "Select * FROM VMsAccount WHERE Currency= " + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(ddlCurr.SelectedValue)
                End If
                Session("DBConnection") = ViewState("DBConnection")
                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("Currency")
            If txtID.Text = "" Then Exit Sub

            SQLExecuteNonQuery("Delete from MsSetupGiro where type = " + QuotedStr(ddlType.SelectedValue) + " AND Currency = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim lb As DropDownList
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            lb = obj.FindControl("CurrencyEdit")
            If lb.Text = "" Then
                DataGrid.EditIndex = -1
                Exit Sub
            End If
            txt = obj.FindControl("AccountEdit")
            ddlType.Enabled = False
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbAcc As TextBox
        Dim lbCode As DropDownList
        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("CurrencyEdit")
            tbAcc = DataGrid.Rows(e.RowIndex).FindControl("AccountEdit")

            If tbAcc.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Account Must Have Value")
                tbAcc.Focus()
                Exit Sub
            End If

            SQLString = "Update MsSetupGiro set Account= " + QuotedStr(tbAcc.Text) & _
            " where Currency = " & QuotedStr(lbCode.Text) + " AND Type = " + QuotedStr(ddlType.SelectedValue)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
            ddlType.Enabled = True
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
            ddlType.Enabled = True
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Try
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "ddl type select index changed error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            If Not IsPostBack Then
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = "error page complete load : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim Curr As DropDownList
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim sqlstring As String
        Try
            tb = sender
            If tb.ID = "AccountAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdd")
                AccName = dgi.FindControl("DescriptionAdd")
                Curr = dgi.FindControl("CurrencyAdd")
                sqlstring = "SELECT * From VMsAccount Where (Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(Curr.SelectedValue) + ") AND Account = " + QuotedStr(Acc.Text)
                ds = SQLExecuteQuery(sqlstring, ViewState("DBConnection").ToString)
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountEdit")
                AccName = dgi.FindControl("DescriptionEdit")
                Curr = dgi.FindControl("CurrencyEdit")
                sqlstring = "SELECT * From VMsAccount Where (Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(Curr.SelectedValue) + ") AND Account = " + QuotedStr(Acc.Text)
                ds = SQLExecuteQuery(sqlstring, ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc Changed Error : " + ex.ToString
        End Try
    End Sub
End Class