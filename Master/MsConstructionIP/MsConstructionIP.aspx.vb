Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsConstructionIP_MsConstructionIP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAdd" Or ViewState("Sender") = "SearchEdit" Then
                Dim Acc As TextBox
                Dim AccName As Label
                If ViewState("Sender") = "SearchAdd" Then
                    Acc = DataGrid.FooterRow.FindControl("AccountAdd")
                    AccName = DataGrid.FooterRow.FindControl("AccountNameAdd")
                Else
                    Acc = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountEdit")
                    AccName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountNameEdit")
                End If

                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                Acc.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        lstatus.Text = ""
    End Sub
    Protected Sub AccountEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Acc = dgi.FindControl("AccountEdit")
            AccName = dgi.FindControl("AccountNameEdit")


            ds = SQLExecuteQuery("Select Account, Description From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
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

    Protected Sub AccountAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccountAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdd")
                AccName = dgi.FindControl("AccountNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountEdit")
                AccName = dgi.FindControl("AccountNameEdit")
            End If
            ds = SQLExecuteQuery("Select Account, Description AS AccountName From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("AccountName").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc Changed Error : " + ex.ToString
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
            SqlString = "SELECT * FROM V_MsContructionIPView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CIPCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormprintMaster5 'V_MsContructionIPView', 'CIPCode +''-''+ CIPName ', 'Start_Date', 'End_Date', 'FgActive', 'AccountName', 'Construction In Progress', 'CIP Code - Name', 'StartDate', 'EndDate', 'FG Active', 'Account', '" + StrFilter + "', " + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.Load

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
        Dim dbCode, dbName, tbAcc As TextBox
        Dim dbStartDate As BasicFrame.WebControls.BasicDatePicker
        Dim dbEndDate As Label
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("CIPCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("CIPNameAdd")
                dbStartDate = DataGrid.FooterRow.FindControl("StartDateAdd")
                dbEndDate = DataGrid.FooterRow.FindControl("EndDateAdd")
                tbAcc = DataGrid.FooterRow.FindControl("AccountAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "ContructionIP Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "ContructionIP Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If dbStartDate.Text.Trim.Length = 0 Then
                    lstatus.Text = "Start Date Must Be filled."
                    dbStartDate.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT CIPCode From MsContructionIP WHERE CIPCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "ContructionIP " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsContructionIP (CIPCode, CIPName,StartDate,EndDate,FgActive,Account,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + _
                ",'" + Format(dbStartDate.SelectedValue, "yyyy-MM-dd") + "',NULL,'Y'," + _
                "" + QuotedStr(tbAcc.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select * FROM VMsAccount"
                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAdd" Then
                    ViewState("Sender") = "SearchAdd"
                Else
                    ViewState("Sender") = "SearchEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CIPCode")

            SQLExecuteNonQuery("Delete from MsContructionIP where CIPCode = '" & txtID.Text & "'", ViewState("DBConnection"))
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
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
            txt = obj.FindControl("CIPNameEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, tbAcc As TextBox
        Dim lbCode As Label
        Dim dbStartDate As BasicFrame.WebControls.BasicDatePicker

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("CIPCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("CIPNameEdit")
            dbStartDate = DataGrid.Rows(e.RowIndex).FindControl("StartDateEdit")
            'dbEndDate = DataGrid.Rows(e.RowIndex).FindControl("EndDateEdit")
            tbAcc = DataGrid.Rows(e.RowIndex).FindControl("AccountEdit")

            If dbStartDate.Text.Trim.Length = 0 Then
                lstatus.Text = "Start Date Must Be filled."
                dbStartDate.Focus()
                Exit Sub
            End If

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "CIP Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsContructionIP set CIPName= " + QuotedStr(dbName.Text) + _
            ",StartDate = " + QuotedStr(Format(dbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                ",Account= " + QuotedStr(tbAcc.Text) + _
                     " where CIPCode = '" & lbCode.Text + "'"
           
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
