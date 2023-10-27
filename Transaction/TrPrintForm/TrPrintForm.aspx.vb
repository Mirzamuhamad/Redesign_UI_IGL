Imports System.Data
Partial Class Transaction_TrPrintForm_TrPrintForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                BindDatagrid()
            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "page load error : " + ex.ToString
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

    Private Sub BindDatagrid()
        Dim DS As DataSet
        Dim SqlString As String
        Try
            SqlString = "SELECT * FROM TrPrintForm"
            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Form ASC"
                ViewState("SortOrder") = "ASC"
            End If

            DataGrid.DataSource = DS.Tables(0)
            DataGrid.DataBind()

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Bind Data Grid Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.EditIndex = -1
            BindDatagrid()
        Catch ex As Exception
            lbStatus.Text = "datagrid row canceling edit Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Try
            DataGrid.EditIndex = e.NewEditIndex
            BindDatagrid()
        Catch ex As Exception
            lbStatus.Text = "datagrid row editing error : " + ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode, lbTitle As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            dbName = GVR.FindControl("UserNameEdit")
            lbCode = GVR.FindControl("FormEdit")
            lbTitle = GVR.FindControl("UserTitleEdit")

            If dbName.Text.Trim.Length = 0 Then
                lbStatus.Text = " User Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE TrPrintForm SET UserName =" + QuotedStr(dbName.Text) + _
            " WHERE Form=" + QuotedStr(lbCode.Text) + " AND UserTitle=" + QuotedStr(lbTitle.Text)
            SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DataGrid.EditIndex = -1
            BindDatagrid()
        Catch ex As Exception
            lbStatus.Text = "data grid row updating error : " + ex.ToString
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
            BindDatagrid()
        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
