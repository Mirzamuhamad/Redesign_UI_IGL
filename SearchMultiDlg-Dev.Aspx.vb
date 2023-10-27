Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxPager
Imports System.Windows.Forms
Imports System.Collections.Generic


Partial Class SearchMultiDlg
    Inherits System.Web.UI.Page

    
    Protected Sub form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Disposed

    End Sub


    Protected Sub VisiblePager(ByVal vbool As Boolean)
        btnApply.Visible = vbool
        btnApply2.Visible = vbool
        ddlPager.Visible = vbool
        ddlPager2.Visible = vbool
        lbRecord.Visible = vbool
        lbRecord2.Visible = vbool
        lbPager.Visible = vbool
        lbPager2.Visible = vbool
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
                If Not Session("ResultSame") Is Nothing Then
                    ViewState("InResultSame") = Session("ResultSame")
                    Session("ResultSame") = Nothing
                End If
                If Not Session("CriteriaField") Is Nothing Then
                    ViewState("CriteriaField") = Session("CriteriaField")
                    Session("CriteriaField") = Nothing
                End If
                If Not Session("ColumnDefault") Is Nothing Then
                    ViewState("ColumnDefault") = Session("ColumnDefault")
                    Session("ColumnDefault") = Nothing
                End If
                VisiblePager(False)
                ViewState("PagerSize") = ddlPager.SelectedValue
                ViewState("Check") = True
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
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Error on load : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click, btnApply2.Click
        Dim Dt As New DataTable
        Dim Dr As DataRow
        Dim NewColumn As DataColumn

        Dim ContentColumn(), Fields(), Columns(), ResultSame(20) As String
        Dim ICoumns(30) As Integer
        Dim DataSelected As List(Of Object)
        Dim IRecord, IColumn, IField, FieldCount, ColumnCount, RecordCount As Integer
        Try
            If ViewState("pk") Is Nothing Then
                Exit Sub
            End If
            DataSelected = GridView1.GetSelectedFieldValues(ViewState("pk"))
            Columns = ViewState("InColumn")
            Fields = ViewState("pk").ToString.Split(";")
            ColumnCount = Columns.Length - 1
            FieldCount = Fields.Length - 1
            RecordCount = DataSelected.Count - 1

            'btnSelect.Text = CStr(RecordCount)
            'create table with column result
            For IColumn = 0 To ColumnCount
                NewColumn = New DataColumn(Columns(IColumn).Trim, GetType(String))
                Dt.Columns.Add(NewColumn)
            Next
            For IRecord = 0 To RecordCount
                ContentColumn = DataSelected(IRecord).ToString.Split("|")
                Dr = Dt.NewRow
                For IColumn = 0 To ColumnCount
                    For IField = 0 To FieldCount
                        If Columns(IColumn).Trim.ToLower = Fields(IField).Trim.ToLower Then
                            If ContentColumn(IField) = "~Xtra#Base64AAEAAAD/////AQAAAAAAAAAEAQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEICgIAAAAGAgAAAAAL" Then
                                Dr(Columns(IColumn).Trim) = ""
                            Else
                                Dr(Columns(IColumn).Trim) = ContentColumn(IField)
                            End If
                            ICoumns(IColumn) = IField
                        End If
                    Next
                Next
                Dt.Rows.Add(Dr)
            Next

            If Not ViewState("InResultSame") Is Nothing Then
                For j = 0 To ViewState("InResultSame").Length - 1
                    ResultSame(j) = ""
                Next

                For Each Dr In Dt.Rows
                    'Dr = Dt.NewRow                    
                    For i = 0 To ColumnCount
                        'lbStatus.Text = lbStatus.Text + ", " + Columns(i).Trim.ToLower
                        If (Not Columns(i).Trim = "") And ICoumns(i) >= 0 Then
                            For j = 0 To ViewState("InResultSame").Length - 1

                                If ViewState("InResultSame")(j).Trim.ToLower = Columns(i).Trim.ToLower Then
                                    If ResultSame(j).Trim = "" Then
                                        ResultSame(j) = Dr(Columns(i).Trim).Trim
                                        'lbStatus.Text = lbStatus.Text + ", " + Dr(Columns(i).Trim).Trim
                                    ElseIf ResultSame(j).Trim <> Dr(Columns(i).Trim).Trim Then
                                        lbStatus.Text = "Only 1 " + Columns(i).Replace("_", " ") + " with value '" + Dr(Columns(i).Trim) + "' or '" + ResultSame(j).Trim + "' allowed"
                                        Exit Sub
                                    End If
                                End If
                            Next
                        End If
                    Next
                    'Exit Sub
                Next

                'Exit Sub
                'Dim same As String()
                'Dim I As Integer
                'same = ViewState("InResultSame")

                'For Each r In Dt.Rows
                '    For C = 0 To Columns.Length - 1
                '        For I = 0 To same.Count - 1
                '        Next
                '    Next

                'Next

                'dt2 = Dt.DefaultView.ToTable(True, same(I))

                'If Dt.Rows.Count <> dt2.Rows.Count Then
                '    lbStatus.Text = MessageDlg("Cannot get duplicate values on field " + same(I))
                '    Exit Sub
                'End If
                'Next
            End If
            Session("Result") = Dt
            Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "Error jek : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataGrid()
        Dim tempDS As New DataSet()
        Dim StrFilter As String
        Dim dvSearch As DataView
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
            dvSearch.Sort = ddlOrderBy.SelectedValue.Replace(" ", "_")

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
                GridView1.KeyFieldName = pk
                ViewState("pk") = pk
            End If
            GridView1.SettingsPager.PageSize = CInt(ViewState("PagerSize").ToString)
            VisiblePager(dvSearch.Count > 0)
            GridView1.DataSource = dvSearch
            GridView1.DataBind()

            'Dim gvcol As New GridViewCommandColumn("#")
            'If GridView1.Columns.IndexOf(gvcol) = -1 Then
            '    gvcol.ShowSelectCheckbox = True
            '    GridView1.Columns.Add(gvcol)
            '    GridView1.Columns("#").VisibleIndex = 0
            'End If
            Dim gvcol As New GridViewCommandColumn("#")
            Dim aaa As New MyTemplate
            If ViewState("Check") Then
                gvcol.HeaderTemplate = aaa
                gvcol.ShowSelectCheckbox = True
                GridView1.Columns.Add(gvcol)
                'GridView1.Columns(GridView1.Columns.Count - 1).VisibleIndex = 0
                GridView1.Columns("#").VisibleIndex = 0
                ViewState("Check") = False
            Else
                gvcol = GridView1.Columns("#")
                gvcol.HeaderTemplate = aaa
            End If
        Catch ex As Exception
            lbStatus.Text = "Error on btn search klik : " + ex.ToString
        End Try
    End Sub

    Public Class MyTemplate
        Implements System.Web.UI.ITemplate

        Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim cb As New ASPxCheckBox ', cbCheckAll, cbUnCheckAll
            cb.ToolTip = "Select/Unselect all rows on the page"
            'cbCheckAll.ToolTip = "Select all page"
            'cbUnCheckAll.ToolTip = "UnSelect all page"
            cb.ClientSideEvents.CheckedChanged = "function(s, e) { GridView1.SelectAllRowsOnPage(s.GetChecked()); }"
            'cbCheckAll.ClientSideEvents.CheckedChanged = "function(s, e) { GridView1.SelectRows(); }"
            'cbUnCheckAll.ClientSideEvents.CheckedChanged = "function(s, e) { GridView1.UnselectRows(); }"
            container.Controls.Add(cb)
            'container.Controls.Add(cbCheckAll)
            'container.Controls.Add(cbUnCheckAll)
        End Sub
    End Class

    Protected Sub GridView1_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles GridView1.BeforeColumnSortingGrouping
        BindDataGrid()
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        Dim gridView As ASPxGridView = CType(sender, ASPxGridView)
        For Each column As GridViewColumn In gridView.Columns
            If TypeOf column Is GridViewDataColumn Then
                CType(column, GridViewDataColumn).Settings.AutoFilterCondition = AutoFilterCondition.Contains
            End If
        Next column
    End Sub

    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged
        BindDataGrid()
    End Sub

    Protected Sub GridView1_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles GridView1.ProcessColumnAutoFilter
        BindDataGrid()
    End Sub
    
    'Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click
    '    Try
    '        'GridView1.Selection.SelectAll()
    '        'BindDataGrid()
    '        'GridView1.
    '        'btnSelect.Text = CStr(GridView1.GetSelectedFieldValues(ViewState("pk")).I
    '        'btnSelect.Text = CStr(GridView1..Count - 1)
    '        'For i As Integer = 0 To GridView1.Controls.Count - 1
    '        'GridControl1.MainView.GetRow(i)
    '        'Next
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Protected Sub btnUnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSelect.Click
    '    GridView1.Selection.UnselectAll()
    'End Sub

    'Protected Sub btnAll_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAll.Click
    '    Dim Dt As New DataTable
    '    Dim Dr As DataRow
    '    Dim NewColumn As DataColumn

    '    Dim ContentColumn(), Fields(), Columns(), ResultSame(20) As String
    '    Dim ICoumns(30) As Integer
    '    Dim DataSelected As List(Of Object)
    '    'Dim drall As DataRow()
    '    Dim IRecord, IColumn, IField, FieldCount, ColumnCount, RecordCount As Integer
    '    Try
    '        If ViewState("pk") Is Nothing Then
    '            Exit Sub
    '        End If

    '        GridView1.Selection.SelectAll()

    '        'DataSelected = GridView1.GetSelectedFieldValues(ViewState("pk"))
    '        'Columns = ViewState("InColumn")
    '        'Fields = ViewState("pk").ToString.Split(";")
    '        'ColumnCount = Columns.Length - 1
    '        'FieldCount = Fields.Length - 1
    '        'RecordCount = DataSelected.Count - 1

    '        ''create table with column result
    '        'For IColumn = 0 To ColumnCount
    '        '    NewColumn = New DataColumn(Columns(IColumn).Trim, GetType(String))
    '        '    Dt.Columns.Add(NewColumn)
    '        'Next
    '        'For IRecord = 0 To RecordCount
    '        '    ContentColumn = DataSelected(IRecord).ToString.Split("|")
    '        '    Dr = Dt.NewRow
    '        '    For IColumn = 0 To ColumnCount
    '        '        For IField = 0 To FieldCount
    '        '            If Columns(IColumn).Trim.ToLower = Fields(IField).Trim.ToLower Then
    '        '                If ContentColumn(IField) = "~Xtra#Base64AAEAAAD/////AQAAAAAAAAAEAQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEICgIAAAAGAgAAAAAL" Then
    '        '                    Dr(Columns(IColumn).Trim) = ""
    '        '                Else
    '        '                    Dr(Columns(IColumn).Trim) = ContentColumn(IField)
    '        '                End If
    '        '                ICoumns(IColumn) = IField
    '        '            End If
    '        '        Next
    '        '    Next
    '        '    Dt.Rows.Add(Dr)
    '        'Next

    '        'If Not ViewState("InResultSame") Is Nothing Then
    '        '    For j = 0 To ViewState("InResultSame").Length - 1
    '        '        ResultSame(j) = ""
    '        '    Next

    '        '    For Each Dr In Dt.Rows
    '        '        'Dr = Dt.NewRow
    '        '        For i = 0 To ColumnCount
    '        '            If (Not Columns(i).Trim = "") And ICoumns(i) >= 1 Then
    '        '                For j = 0 To ViewState("InResultSame").Length - 1
    '        '                    If ViewState("InResultSame")(j).Trim.ToLower = Columns(i).Trim.ToLower Then
    '        '                        If ResultSame(j).Trim = "" Then
    '        '                            ResultSame(j) = Dr(Columns(i).Trim).Trim
    '        '                        ElseIf ResultSame(j).Trim <> Dr(Columns(i).Trim).Trim Then
    '        '                            lbStatus.Text = "Only 1 " + Columns(i).Replace("_", " ") + " with value '" + Dr(Columns(i).Trim) + "' or '" + ResultSame(j).Trim + "' allowed"
    '        '                            Exit Sub
    '        '                        End If
    '        '                    End If
    '        '                Next
    '        '            End If
    '        '        Next
    '        '    Next

    '        'End If
    '        'Session("Result") = Dt
    '        'Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
    '    Catch ex As Exception
    '        lbStatus.Text = lbStatus.Text + "Error jek : " + ex.ToString
    '    End Try
    'End Sub

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
End Class
