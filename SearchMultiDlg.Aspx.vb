Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration

Partial Class SearchMultiDlg2
    Inherits System.Web.UI.Page


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                'Dim DvSearch As DataView
                ViewState("InFilter") = Session("Filter")
                ViewState("InColumn") = Session("Column")
                ViewState("InClickSame") = Session("ClickSame")

                Session("Filter") = Nothing
                Session("Column") = Nothing
                Session("ClickSame") = Nothing

                If Not Session("ResultSame") Is Nothing Then
                    ViewState("InResultSame") = Session("ResultSame")
                    Session("ResultSame") = Nothing
                End If

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
                GridView1.PageSize = ddlPager.SelectedValue
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
                    ViewState("PagerSize") = ddlPager.SelectedValue
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
        Dim CB As CheckBox
        Dim Row As GridViewRow
        Dim Dt As New DataTable
        Dim Dr As DataRow
        Dim NewColumn As DataColumn

        Dim Column(), ResultSame(20) As String
        Dim ICoumn(30) As Integer
        Dim i, j, count As Integer
        Try
            If GridView1.HeaderRow Is Nothing Then
                lbStatus.Text = "Data not exists"
                Exit Sub
            End If
            count = GridView1.HeaderRow.Cells.Count - 1
            Column = ViewState("InColumn")
            For i = 0 To Column.Length - 1
                j = -1
                For j = 0 To count
                    If Column(i).ToLower.Trim = GridView1.HeaderRow.Cells(j).Text.ToLower Then
                        ' cel index kolom hasil
                        ICoumn(i) = j

                        'create table with column result
                        NewColumn = New DataColumn(Column(i).Trim, GetType(String))
                        Dt.Columns.Add(NewColumn)
                    End If
                Next
            Next

            If Not (ViewState("InResultSame") Is Nothing) Then
                For j = 0 To ViewState("InResultSame").Length - 1
                    ResultSame(j) = ""
                Next
            End If

            For Each Row In GridView1.Rows
                CB = Row.FindControl("cbSelect")
                If CB.Checked Then
                    Dr = Dt.NewRow
                    For i = 0 To Column.Length - 1
                        If (Not Column(i).Trim = "") And ICoumn(i) >= 1 Then
                            Dr(Column(i).Trim) = Row.Cells(ICoumn(i)).Text

                            If Not (ViewState("InResultSame") Is Nothing) Then
                                For j = 0 To ViewState("InResultSame").Length - 1
                                    If ViewState("InResultSame")(j).Trim.ToLower = Column(i).Trim.ToLower Then
                                        If ResultSame(j).Trim = "" Then
                                            ResultSame(j) = Dr(Column(i).Trim).Trim
                                        ElseIf ResultSame(j).Trim <> Dr(Column(i).Trim).Trim Then
                                            lbStatus.Text = "Only 1 " + Column(i).Replace("_", " ") + " with value '" + Dr(Column(i).Trim) + "' or '" + ResultSame(j).Trim + "' allowed"
                                            Exit Sub
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                    Dt.Rows.Add(Dr)
                End If
            Next
            Session("Result") = Dt
            Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
        Catch ex As Exception
            lbStatus.Text = "Error jek : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataGrid()
        Dim tempDS As New DataSet()
        Dim StrFilter As String
        Dim DvSearch As DataView
        Try
            StrFilter = Generate5FilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, ddlField3.SelectedValue, ddlField4.SelectedValue, ddlField5.SelectedValue, tbFilter.Text, tbfilter2.Text, tbfilter3.Text, tbfilter4.Text, tbfilter5.Text, ddlNotasi.SelectedValue, ddlNotasi3.SelectedValue, ddlNotasi4.SelectedValue, ddlNotasi5.SelectedValue)
            If Not ViewState("InFilter").ToString.ToUpper.Contains("SELECT ") Then
                If ViewState("DBConnection") Is Nothing Then
                    tempDS = SQLExecuteQuery(ViewState("InFilter"), GetConString)
                Else
                    tempDS = SQLExecuteQuery(ViewState("InFilter"), ViewState("DBConnection").ToString)
                End If
                DvSearch = tempDS.Tables(0).DefaultView
                DvSearch.RowFilter = StrFilter
            Else
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
                DvSearch = tempDS.Tables(0).DefaultView
            End If
            DvSearch.Sort = ddlOrderBy.SelectedValue.Replace(" ", "_")
            GridView1.DataSource = DvSearch
            GridView1.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Error on btn search klik : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub cbSelectItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim status As Boolean
        Dim CheckStr As String
        Dim cb As CheckBox
        Dim GVR As GridViewRow
        Dim Count, indexFilter As Integer 'indexGrid, 
        Try
            'cb = sender
            'status = cb.Checked
            'GVR = cb.NamingContainer
            'Count = GridView1.HeaderRow.Cells.Count - 1
            'indexFilter = -1
            'For j = 0 To Count
            '    If ViewState("InClickSame").ToString.ToLower.Trim = GridView1.HeaderRow.Cells(j).Text.ToLower Then
            '        indexFilter = j
            '    End If
            'Next
            'CheckStr = GVR.Cells(indexFilter).Text
            'For Each GVR In GridView1.Rows
            '    If GVR.Cells(indexFilter).Text = CheckStr Then
            '        cb = GVR.FindControl("cbSelect")
            '        cb.Checked = status
            '    End If
            'Next
        Catch ex As Exception
            lbStatus.Text = "SElect Item Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindDataGrid()
    End Sub

    Protected Sub ddlPager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPager.SelectedIndexChanged
        If ViewState("PagerSize") <> ddlPager.SelectedValue Then
            ViewState("PagerSize") = ddlPager.SelectedValue
            'ddlPager2.SelectedValue = ddlPager.SelectedValue
            BindDataGrid()
        End If
        ViewState("PagerSize") = ddlPager.SelectedValue
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlPager.SelectedValue
        BindDataGrid()
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
       
        'If (e.Row.RowType = DataControlRowType.Header) Then
        '    ' Find the checkbox control in header and add an attribute
        '    Dim cb As CheckBox
        '    cb = e.Row.FindControl("cbSelectHd")
        '    cb.Attributes.Add("oncheckedchanged", "javascript:SelectCheck('" + cb.ClientID + "')")
        'End If
    End Sub
End Class
