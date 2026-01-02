Imports System.Data
Partial Class Transaction_MsFormDoc_MsFormDoc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
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
        'Dim DS As DataSet
        Dim SqlString As String
        Try
            'DS = SQLExecuteQuery("SELECT * FROM MsFormDoc", ViewState("DBConnection"))
            'DataGrid.DataSource = DS.Tables(0)
            'DataGrid.DataBind()

            SqlString = "SELECT * FROM MsFormDoc "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FormCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
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
        Dim dbIso As TextBox
        Dim lbForm As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            dbIso = GVR.FindControl("ISODocNoEdit")
            lbForm = GVR.FindControl("FormEdit")

            If dbIso.Text.Trim.Length = 0 Then
                lbStatus.Text = "<script language='javascript'>alert('ISO DOC. No must be filled');</script>"
                dbIso.Focus()
                Exit Sub
            End If
            
            SQLString = "UPDATE MsFormDoc SET ISODocNo =" + QuotedStr(dbIso.Text) + _
            " WHERE FormCode =" + QuotedStr(lbForm.Text)
            SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DataGrid.EditIndex = -1
            BindDatagrid()
        Catch ex As Exception
            lbStatus.Text = "data grid row updating error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
            ViewState("SortOrder") = "ASC"
        Else
            ViewState("SortOrder") = "DESC"
        End If
        ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
        BindDatagrid()
    End Sub
End Class
