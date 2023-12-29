Imports System.Data
Partial Class Transaction_TrUnDelete_TrUnDelete
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                'BindDatagrid()
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

    'Private Sub BindDatagrid()
    '    Dim DS As DataSet
    '    Dim SqlString As String
    '    Try
    '        SqlString = "SELECT * FROM VTrUnDelete"
    '        DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

    '        If ViewState("SortExpression") = Nothing Then
    '            ViewState("SortExpression") = "PTKPCode ASC"
    '            ViewState("SortOrder") = "ASC"
    '        End If

    '        DataGrid.DataSource = DS.Tables(0)
    '        DataGrid.DataBind()

    '        BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Grid Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
    '    Try
    '        DataGrid.EditIndex = -1
    '        BindDatagrid()
    '    Catch ex As Exception
    '        lbStatus.Text = "datagrid row canceling edit Error : " + ex.ToString
    '    End Try
    'End Sub
    'Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
    '    Dim obj As GridViewRow
    '    Dim tbRate As TextBox
    '    Try
    '        DataGrid.EditIndex = e.NewEditIndex
    '        BindDatagrid()
    '        obj = DataGrid.Rows(e.NewEditIndex)
    '        tbRate = obj.FindControl("PTKPRateEdit")
    '        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
    '    Catch ex As Exception
    '        lbStatus.Text = "datagrid row editing error : " + ex.ToString
    '    End Try
    'End Sub


    'Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
    '    Dim SQLString As String
    '    Dim dbName As TextBox
    '    Dim lbCode, lbTitle As Label
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = DataGrid.Rows(e.RowIndex)
    '        dbName = GVR.FindControl("PTKPRateEdit")
    '        lbCode = GVR.FindControl("PTKPNameEdit")
    '        lbTitle = GVR.FindControl("PTKPCodeEdit")

    '        If dbName.Text.Trim.Length = 0 Then
    '            lbStatus.Text = "PTKP Rate must be filled."
    '            dbName.Focus()
    '            Exit Sub
    '        End If

    '        SQLString = "UPDATE TrUnDelete SET PTKPRate =" + dbName.Text.Replace(",", "") + _
    '        " WHERE PTKPCode =" + QuotedStr(lbTitle.Text)
    '        SQLExecuteQuery(SQLString, ViewState("DBConnection"))
    '        DataGrid.EditIndex = -1
    '        BindDatagrid()
    '    Catch ex As Exception
    '        lbStatus.Text = "data grid row updating error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
    '    Try
    '        If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
    '            ViewState("SortOrder") = "ASC"
    '        Else
    '            ViewState("SortOrder") = "DESC"
    '        End If
    '        ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
    '        BindDatagrid()
    '    Catch ex As Exception
    '        lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnVerivy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerivy.Click
        Try
            Dim Result As String
            'lbVerivy.Visible = False
            Result = ExecSPPosting("S_FNCustInvCekUnDelete", tbCode.Text, CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Trim(Result) <> "" Then
                lbStatus.Text = lbStatus.Text + Result + " <br/>"
            Else
                lbStatus.Text = "Verified"
                'lbVerivy.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btnVerivy_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnDelete.Click
        Try
            Dim Result As String
            'lbUnDelete.Visible = False
            Result = ExecSPPosting("S_FNCustInvUnDelete", tbCode.Text, CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Trim(Result) <> "" Then
                lbStatus.Text = lbStatus.Text + Result + " <br/>"
            Else
                lbStatus.Text = "Un-Delete is succeeded"
                'lbUnDelete.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btnUnDelete_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
