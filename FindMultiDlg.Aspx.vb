Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Generic
Imports System.IO
'Imports System.Web
'Imports System.Web.UI
'Imports System.Web.UI.Control
Imports System.Web.UI.WebControls

Partial Public Class FindMultiDlg
    Inherits System.Web.UI.Page

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindDataGrid()
    End Sub

    'Private Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
    '    Dim Result(20) As String
    '    Dim Column() As String
    '    Dim i, j, R, count As Integer
    '    Try
    '        count = GridView1.HeaderRow.Cells.Count - 1
    '        Column = ViewState("InColumn")
    '        For i = 0 To Column.Length - 1
    '            'Result(i) = GridView1.SelectedRow.Cells(Column(i)).Text
    '            R = -1
    '            For j = 0 To count
    '                If Column(i).ToLower.Trim = GridView1.HeaderRow.Cells(j).Text.ToLower Then
    '                    R = j
    '                End If
    '            Next
    '            If R >= 0 Then
    '                Result(i) = GridView1.SelectedRow.Cells(R).Text
    '            End If
    '        Next
    '        Session("Result") = Result
    '        'Response.Write("<script language='javascript'> { window.opener.location.Reload();  window.close();}</script>")
    '        Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
    '        'DirectCast(GridView1.SelectedRow.FindControl("btnSelect"), Button).Attributes.Add("Onclick", "javascript:CloseWindow()")
    '    Catch ex As Exception
    '        lbStatus.Text = "Selected Index Changed Error " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GetSelectedRecords()
        Dim Dt As New DataTable
        Try
            Dt.Columns.AddRange(New DataColumn(2) {New DataColumn("DokCode"), New DataColumn("DokName"), New DataColumn("Remark")})
            For Each Row As GridViewRow In GridView1.Rows
                If Row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(Row.Cells(0).FindControl("chkRow"), CheckBox)
                    If chkRow.Checked Then
                        Dim sCode As String = Row.Cells(1).Text
                        Dim sName As String = Row.Cells(2).Text
                        Dim sRemark As String = Row.Cells(3).Text
                        Dt.Rows.Add(sCode, sName, sRemark)
                    End If
                End If
                'GridDt.DataSource = Dt
                'GridDt.DataBind()
            Next
            Session("Result") = Dt
            'Session("Result") = Result
            'Response.Write("<script language='javascript'> { window.opener.location.Reload();  window.close();}</script>")
            Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
            'DirectCast(GridView1.SelectedRow.FindControl("btnSelect"), Button).Attributes.Add("Onclick", "javascript:CloseWindow()")
        Catch ex As Exception
            lbStatus.Text = "btnApply_Click Error " + ex.ToString
        End Try
        'BindGridDt(ViewState("Dt"), GridDt)
    End Sub

    Protected Sub VisiblePager(ByVal vbool As Boolean)
        'btnApply.Visible = vbool
        'btnApply2.Visible = vbool
        'ddlPager.Visible = vbool
        'ddlPager2.Visible = vbool
        'lbRecord.Visible = vbool
        'lbRecord2.Visible = vbool
        'lbPager.Visible = vbool
        'lbPager2.Visible = vbool
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                ViewState("InFilter") = Session("Filter")
                ViewState("InColumn") = Session("Column")

                Session("Filter") = Nothing
                Session("Column") = Nothing

                If Not Session("DBConnection") Is Nothing Then
                    ViewState("DBConnection") = Session("DBConnection")
                    Session("DBConnection") = Nothing
                End If
                If Not Session("CriteriaField") Is Nothing Then
                    ViewState("CriteriaField") = Session("CriteriaField")
                    Session("CriteriaField") = Nothing
                End If
                If Not Session("ColumnDefault") Is Nothing Then
                    ViewState("ColumnDefault") = Session("ColumnDefault")
                    Session("ColumnDefault") = Nothing
                End If

                ViewState("PagerSize") = ddlPager.SelectedValue
                VisiblePager(False)
                If ViewState("CriteriaField") Is Nothing Then
                    Dim dt As DataTable
                    Dim dc As DataColumn
                    dt = GetSchema(ViewState("InFilter"), ViewState("DBConnection"))
                    For Each dc In dt.Columns
                        ddlOrderBy.Items.Add(dc.ColumnName.Replace("_", " "))
                        ddlField.Items.Add(dc.ColumnName.Replace("_", " "))
                        ddlField2.Items.Add(dc.ColumnName.Replace("_", " "))
                        ddlField3.Items.Add(dc.ColumnName.Replace("_", " "))
                        ddlField4.Items.Add(dc.ColumnName.Replace("_", " "))
                        ddlField5.Items.Add(dc.ColumnName.Replace("_", " "))
                    Next
                Else
                    Dim I As Integer
                    Dim Column() As String
                    Column = ViewState("CriteriaField")
                    For I = 0 To Column.Count - 1
                        ddlOrderBy.Items.Add(Column(I).Trim.Replace("_", " "))
                        ddlField.Items.Add(Column(I).Trim.Replace("_", " "))
                        ddlField2.Items.Add(Column(I).Trim.Replace("_", " "))
                        ddlField3.Items.Add(Column(I).Trim.Replace("_", " "))
                        ddlField4.Items.Add(Column(I).Trim.Replace("_", " "))
                        ddlField5.Items.Add(Column(I).Trim.Replace("_", " "))
                    Next
                End If
                If Not ViewState("ColumnDefault") Is Nothing Then
                    Dim J As Integer
                    Dim DColumn(), DefaultField As String
                    DColumn = ViewState("ColumnDefault")
                    For J = 0 To DColumn.Count - 1
                        DefaultField = DColumn(J).Replace("_", " ").Trim
                        If J = 0 Then
                            If ddlField.Items.Contains(ddlField.Items.FindByValue(DefaultField)) Then
                                ddlField.SelectedValue = DefaultField
                                ddlField2.SelectedValue = DefaultField
                                ddlField3.SelectedValue = DefaultField
                                ddlField4.SelectedValue = DefaultField
                                ddlField5.SelectedValue = DefaultField
                                ViewState("DefaultField") = DefaultField
                            Else
                                ddlField.SelectedIndex = 0
                                ViewState("DefaultField") = ddlField.SelectedValue
                            End If
                        ElseIf J = 1 Then
                            If ddlField2.Items.Contains(ddlField2.Items.FindByValue(DefaultField)) Then
                                ddlField2.SelectedValue = DefaultField
                            Else
                                ddlField2.SelectedIndex = ViewState("DefaultField")
                            End If
                        ElseIf J = 2 Then
                            If ddlField3.Items.Contains(ddlField3.Items.FindByValue(DefaultField)) Then
                                ddlField3.SelectedValue = DefaultField
                            Else
                                ddlField3.SelectedIndex = ViewState("DefaultField")
                            End If
                        ElseIf J = 3 Then
                            If ddlField4.Items.Contains(ddlField4.Items.FindByValue(DefaultField)) Then
                                ddlField4.SelectedValue = DefaultField
                            Else
                                ddlField4.SelectedIndex = ViewState("DefaultField")
                            End If
                        ElseIf J = 4 Then
                            If ddlField5.Items.Contains(ddlField5.Items.FindByValue(DefaultField)) Then
                                ddlField5.SelectedValue = DefaultField
                            Else
                                ddlField5.SelectedIndex = ViewState("DefaultField")
                            End If
                        End If
                    Next
                End If
                BindDataGrid()
            End If
        Catch ex As Exception
            lbStatus.Text = "Error on Load : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindDataGrid()
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            tbfilter3.Text = ""
            tbfilter4.Text = ""
            tbfilter5.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataGrid()
        Dim tempDS As New DataSet()
        Dim dvSearch As DataView
        Dim StrFilter As String
        Try
            StrFilter = Generate5FilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, ddlField3.SelectedValue, ddlField4.SelectedValue, ddlField5.SelectedValue, tbFilter.Text, tbfilter2.Text, tbfilter3.Text, tbfilter4.Text, tbfilter5.Text, ddlNotasi.SelectedValue, ddlNotasi3.SelectedValue, ddlNotasi4.SelectedValue, ddlNotasi5.SelectedValue)
            If ViewState("InFilter").ToString.ToUpper.Contains("SELECT ") Then
                If StrFilter.Length > 3 Then
                    If ViewState("InFilter").ToString.ToUpper.Contains("WHERE ") Then
                        StrFilter = " And (" + StrFilter + ") "
                    Else
                        StrFilter = " Where (" + StrFilter + ") "
                    End If
                End If
                If ViewState("DBConnection") Is Nothing Then
                    tempDS = SQLExecuteQuery(ViewState("InFilter") + StrFilter, GetConString)
                Else
                    tempDS = SQLExecuteQuery(ViewState("InFilter") + StrFilter, ViewState("DBConnection").ToString)
                End If
                dvSearch = tempDS.Tables(0).DefaultView
            Else
                If ViewState("DBConnection") Is Nothing Then
                    tempDS = SQLExecuteQuery(ViewState("InFilter"), GetConString)
                Else
                    tempDS = SQLExecuteQuery(ViewState("InFilter"), ViewState("DBConnection").ToString)
                End If
                dvSearch = tempDS.Tables(0).DefaultView
                dvSearch.RowFilter = StrFilter
            End If

            If ViewState("pk") Is Nothing Then
                Dim a As Integer
                Dim pk As String
                Dim Pertamax As Boolean
                pk = ""
                Pertamax = True
                For a = 0 To dvSearch.Table.Columns.Count - 1
                    If Pertamax Then
                        pk = dvSearch.Table.Columns(a).ColumnName
                        Pertamax = False
                    Else
                        pk = pk + ";" + dvSearch.Table.Columns(a).ColumnName.Trim
                    End If
                Next
                GridView1.DataSource = pk
                ViewState("pk") = pk
            End If

            dvSearch.Sort = ddlOrderBy.SelectedValue.Replace(" ", "_")
            VisiblePager(dvSearch.Count > 0)
            'GridView1.SettingsPager.PageSize = CInt(ViewState("PagerSize").ToString)
            GridView1.DataSource = dvSearch
            GridView1.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Error on btn search klik : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged
        BindDataGrid()
    End Sub

    Protected Sub ddlPager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPager.SelectedIndexChanged
        If ViewState("PagerSize") <> ddlPager.SelectedValue Then
            ViewState("PagerSize") = ddlPager.SelectedValue
            ddlPager2.SelectedValue = ddlPager.SelectedValue
            BindDataGrid()
        End If
    End Sub

    Protected Sub ddlPager2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPager2.SelectedIndexChanged
        If ViewState("PagerSize") <> ddlPager2.SelectedValue Then
            ViewState("PagerSize") = ddlPager2.SelectedValue
            ddlPager.SelectedValue = ddlPager2.SelectedValue
            BindDataGrid()
        End If
    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        GetSelectedRecords()
    End Sub

End Class