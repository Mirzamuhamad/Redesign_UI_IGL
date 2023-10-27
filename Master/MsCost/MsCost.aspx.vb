Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsCost_MsCost
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            'ViewState("SortExpression") = Nothing
            'DataGrid.PageSize = CInt(Session("PageSizeGrid"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnAccExpAdd" Or ViewState("Sender") = "btnAccExpEdit" Then
                Dim AccountExp As TextBox
                Dim AccountExpName As Label
                If ViewState("Sender") = "btnAccExpAdd" Then
                    AccountExp = DataGrid.FooterRow.FindControl("AccountExpAdd")
                    AccountExpName = DataGrid.FooterRow.FindControl("AccountExpNameAdd")
                Else
                    AccountExp = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountExpEdit")
                    AccountExpName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountExpNameEdit")
                End If
                AccountExp.Text = Session("Result")(0).ToString
                AccountExpName.Text = Session("Result")(1).ToString
                AccountExp.Focus()
            ElseIf ViewState("Sender") = "btnAccAdjAdd" Or ViewState("Sender") = "btnAccAdjEdit" Then
                Dim AccountAdj As TextBox
                Dim AccountAdjName As Label
                If ViewState("Sender") = "btnAccAdjAdd" Then
                    AccountAdj = DataGrid.FooterRow.FindControl("AccountAdjAdd")
                    AccountAdjName = DataGrid.FooterRow.FindControl("AccountAdjNameAdd")
                Else
                    AccountAdj = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountAdjEdit")
                    AccountAdjName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountAdjNameEdit")
                End If
                AccountAdj.Text = Session("Result")(0).ToString
                AccountAdjName.Text = Session("Result")(1).ToString
                AccountAdj.Focus()
            End If
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
            'SqlString = "Select A.*,B.AccountName AS AccExpName ,C.AccountName AS AccAdjName from MsCostFreight A LEFT OUTER JOIN MsAccount B ON A.AccTransit = B.Account " + _
            '           "LEFT OUTER JOIN MsAccount C ON A.AccAdjust = C.Account " + StrFilter + " Order By A.CostFreightName"
            SqlString = "SELECT * FROM VMsCostFreight " + StrFilter + " Order By CostFreightName"


            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CostFreightCode"
                ViewState("SortOrder") = "ASC"
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
            Session("SelectCommand") = "S_FormPrintMaster4 'VMsCostFreight','CostFreightCode','CostFreightName','AccExpName','AccAdjName','Cost File','Cost Code','Cost Name','Account Expense Name','Account Adjustment Name'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
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
        Dim dbCode, dbName, cbxAccountExp As TextBox, cbxAccountAdj As TextBox
        Try


            If e.CommandName = "Insert" Then

                dbCode = DataGrid.FooterRow.FindControl("CostCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("CostNameAdd")
                cbxAccountExp = DataGrid.FooterRow.FindControl("AccountExpAdd")
                cbxAccountAdj = DataGrid.FooterRow.FindControl("AccountAdjAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Cost Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Cost Code Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                


                If cbxAccountExp.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Account Expense must be filled');</script>"
                    cbxAccountExp.Focus()
                    Exit Sub
                End If


                If cbxAccountAdj.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Account Adjust must be filled');</script>"
                    cbxAccountAdj.Focus()
                    Exit Sub
                End If

            
                ' Session("DBConnection").ToString)


                If SQLExecuteScalar("SELECT CostFreightCode From VMsCostFreight WHERE CostFreightCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Cost " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'lstatus.Text = "Insert into MsCostFreight(CostFreightCode, CostFreightName, AccTransit, AccAdjust, UserId, UserDate ) "
                'Exit Sub


                'insert the new entry
                SQLString = "Insert into MsCostFreight(CostFreightCode, CostFreightName, AccTransit, AccAdjust, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + "," + QuotedStr(cbxAccountExp.Text) + "," + QuotedStr(cbxAccountAdj.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                bindDataGrid()

            ElseIf e.CommandName = "btnAccExpAdd" Or e.CommandName = "btnAccExpEdit" Then
                Dim FieldResult As String

                If e.CommandName = "btnAccExpAdd" Then
                    Session("filter") = "Select * FROM VMsAccount "
                    ViewState("Sender") = "btnAccExpAdd"
                Else
                    Session("filter") = "Select * FROM VMsAccount "
                    ViewState("Sender") = "btnAccExpEdit"
                End If
                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())

            ElseIf e.CommandName = "btnAccAdjAdd" Or e.CommandName = "btnAccAdjEdit" Then
                Dim FieldResult As String

                If e.CommandName = "btnAccAdjAdd" Then
                    Session("filter") = "Select * FROM VMsAccount "
                    ViewState("Sender") = "btnAccAdjAdd"
                Else
                    Session("filter") = "Select * FROM VMsAccount "
                    ViewState("Sender") = "btnAccAdjEdit"
                End If
                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CostCode")

            SQLExecuteNonQuery("Delete from MsCostFreight where CostFreightCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("CostNameEdit")
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, cbxAccountExp As TextBox, cbxAccountAdj As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("CostCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("CostNameEdit")
            cbxAccountExp = DataGrid.Rows(e.RowIndex).FindControl("AccountExpEdit")
            cbxAccountAdj = DataGrid.Rows(e.RowIndex).FindControl("AccountAdjEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Cost Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If cbxAccountExp.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Account Expense must be filled');</script>"
                cbxAccountExp.Focus()
                Exit Sub
            End If


            If cbxAccountAdj.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Account Adjust must be filled');</script>"
                cbxAccountAdj.Focus()
                Exit Sub
            End If

            SQLString = "Update MsCostFreight Set CostFreightName= " + QuotedStr(dbName.Text) + "," & _
            "AccTransit = " + QuotedStr(cbxAccountExp.Text) + ",AccAdjust = " + QuotedStr(cbxAccountAdj.Text) + " where CostFreightCode = " & QuotedStr(lbCode.Text)

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
    Protected Sub AccountExpEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Acc = dgi.FindControl("AccountExpEdit")
            AccName = dgi.FindControl("AccountExpNameEdit")


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

    Protected Sub AccountAdjEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Acc = dgi.FindControl("AccountAdjEdit")
            AccName = dgi.FindControl("AccountAdjNameEdit")


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


    Protected Sub AccountExpAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try



            tb = sender
            If tb.ID = "AccountExpAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountExpAdd")
                AccName = dgi.FindControl("AccountExpNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountExpEdit")
                AccName = dgi.FindControl("AccountExpNameEdit")
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

    Protected Sub AccountAdjAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccountAdjAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdjAdd")
                AccName = dgi.FindControl("AccountAdjNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountAdjEdit")
                AccName = dgi.FindControl("AccountAdjNameEdit")
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
End Class
