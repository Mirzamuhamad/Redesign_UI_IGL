Imports System.Data
Imports System.Collections.Generic
Partial Class UserControl_ReportDoubleGrid
    Inherits System.Web.UI.UserControl

    Public RptName As String

    Public Sub SetRpt(ByVal Rpt As String)
        Dim sqlstring As String
        Dim ds As DataSet
        Dim DR As DataRow
        Dim LI As ListItem
        Dim I, Count As Integer

        Try
            sqlstring = "S_GetMsMenuGrid " + QuotedStr(Rpt)
            ds = SQLExecuteQuery(sqlstring, ViewState("Con"))
            Count = ds.Tables(0).Rows.Count
            Dim FieldKey(Count - 1) As String
            I = 0
            For Each DR In ds.Tables(0).Rows
                LI = New ListItem(DR("MsSelect").ToString, DR("MsSelect").ToString)
                DDLGrid1.Items.Add(LI)
                DDLGrid2.Items.Add(LI)
                DDLGrid3.Items.Add(LI)
                DDLGrid4.Items.Add(LI)
                FieldKey(I) = DR("FieldKey").ToString
                I = I + 1
            Next
            ViewState("FieldKey") = FieldKey
            cbGrid1.Visible = (Count >= 1)
            DDLGrid1.Visible = cbGrid1.Visible
            Grid1.Visible = cbGrid1.Visible
            RefreshCek()
        Catch ex As Exception
            lStatus.Text = "Set Rpt Error : " + ex.ToString
        End Try
    End Sub

    Public Function ResultString() As String
        Dim ResultGrid1, ResultGrid2, ResultGrid3, ResultGrid4, Result As String
        Dim Bendera1, Bendera2, Bendera3, Bendera4 As Boolean
        Try
            If cbGrid1.Checked Then
                'AND ( A.Supplier In(''JK-A0002'',''JK-A0003'') OR A.Supplier In(''JK-A0005'',''JK-A0006'')
                ResultGrid1 = ""
                ResultGrid2 = ""
                ResultGrid3 = ""
                ResultGrid4 = ""

                ResultGrid1 = SelectedGridDev(Grid1, ViewState("FieldKey")(DDLGrid1.SelectedIndex)).Replace("AND", "")

                If cbGrid2.Checked Then
                    ResultGrid2 = SelectedGridDev(Grid2, ViewState("FieldKey")(DDLGrid2.SelectedIndex)).Replace("AND", "")
                    If ResultGrid2.Length > 0 Then
                        ResultGrid2 = ddlNotasi.SelectedValue + " " + SelectedGridDev(Grid2, ViewState("FieldKey")(DDLGrid2.SelectedIndex)).Replace("AND", "")
                    Else
                        ResultGrid2 = ""
                    End If
                Else
                    ResultGrid2 = ""
                End If

                If cbGrid3.Checked Then
                    ResultGrid3 = SelectedGridDev(Grid3, ViewState("FieldKey")(DDLGrid3.SelectedIndex)).Replace("AND", "")
                    If ResultGrid3.Length > 0 Then
                        ResultGrid3 = DDLNotasi2.SelectedValue + " " + SelectedGridDev(Grid3, ViewState("FieldKey")(DDLGrid3.SelectedIndex)).Replace("AND", "")
                    Else
                        ResultGrid3 = ""
                    End If
                Else
                    ResultGrid3 = ""
                End If

                If cbGrid4.Checked Then
                    ResultGrid4 = SelectedGridDev(Grid4, ViewState("FieldKey")(DDLGrid4.SelectedIndex)).Replace("AND", "")
                    If ResultGrid4.Length > 0 Then
                        ResultGrid4 = DDLNotasi3.SelectedValue + " " + SelectedGridDev(Grid4, ViewState("FieldKey")(DDLGrid4.SelectedIndex)).Replace("AND", "")
                    Else
                        ResultGrid4 = ""
                    End If
                Else
                    ResultGrid4 = ""
                End If


                Bendera1 = ResultGrid1.Length > 0
                Bendera2 = ResultGrid2.Length > 0
                Bendera3 = ResultGrid3.Length > 0
                Bendera4 = ResultGrid4.Length > 0

                Result = " AND (" + ResultGrid1 + " " + ResultGrid2 + " " + ResultGrid3 + " " + ResultGrid4 + ")"

                If Bendera1 Or Bendera2 Or Bendera3 Or Bendera4 Then
                    Return Result
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            lStatus.Text = "Result String Error : " + ex.ToString
        End Try
        Return ""
    End Function

    Public Sub setConnection(ByVal con As String, ByVal userid As String)
        Try
            GetDataGrid1()
            dsGrid1.ConnectionString = con
            Grid1.DataBind()

            GetDataGrid2()
            dsGrid2.ConnectionString = con
            Grid2.DataBind()

            GetDataGrid3()
            dsGrid3.ConnectionString = con
            Grid3.DataBind()

            GetDataGrid4()
            dsGrid4.ConnectionString = con
            Grid4.DataBind()

            ViewState("Con") = con
            ViewState("UserGrid") = userid
        Catch ex As Exception
            lStatus.Text = "Set Connection Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbGrid1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid1.CheckedChanged, DDLGrid1.SelectedIndexChanged
        Try
            Grid1.Selection.UnselectAll()
            RefreshCek()
            GetDataGrid1()
        Catch ex As Exception
            lStatus.Text = "cb Grid 1 Checked Error :" + ex.ToString
        End Try
    End Sub
    Protected Sub cbGrid2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid2.CheckedChanged, DDLGrid2.SelectedIndexChanged
        Try
            Grid2.Selection.UnselectAll()
            RefreshCek()
            GetDataGrid2()
        Catch ex As Exception
            lStatus.Text = "cb Grid 2 Checked Error :" + ex.ToString
        End Try
    End Sub
    Protected Sub cbGrid3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid3.CheckedChanged, DDLGrid3.SelectedIndexChanged
        Try
            Grid3.Selection.UnselectAll()
            RefreshCek()
            GetDataGrid3()
        Catch ex As Exception
            lStatus.Text = "cb Grid 3 Checked Error :" + ex.ToString
        End Try
    End Sub
    Protected Sub cbGrid4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid4.CheckedChanged, DDLGrid4.SelectedIndexChanged
        Try
            Grid4.Selection.UnselectAll()
            GetDataGrid4()
        Catch ex As Exception
            lStatus.Text = "cb Grid 4 Checked Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub GetDataGrid1()
        Try
            If cbGrid1.Checked Then
                dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
                QuotedStr(DDLGrid1.SelectedValue) + ",'',''"
            Else
                dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Grid 1 :" + ex.ToString
        End Try
    End Sub
    Protected Sub GetDataGrid2()
        Try
            If cbGrid2.Checked Then
                dsGrid2.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
                QuotedStr(DDLGrid2.SelectedValue) + ",'',''"
            Else
                dsGrid2.SelectCommand = "Select '' AS CODE, '' AS Description"
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Grid 2 :" + ex.ToString
        End Try
    End Sub
    Protected Sub GetDataGrid3()
        Try
            If cbGrid3.Checked Then
                dsGrid3.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
                QuotedStr(DDLGrid3.SelectedValue) + ",'',''"
            Else
                dsGrid3.SelectCommand = "Select '' AS CODE, '' AS Description"
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Grid 3 :" + ex.ToString
        End Try
    End Sub
    Protected Sub GetDataGrid4()
        Try
            If cbGrid4.Checked Then
                dsGrid4.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
                QuotedStr(DDLGrid4.SelectedValue) + ",'',''"
            Else
                dsGrid4.SelectCommand = "Select '' AS CODE, '' AS Description"
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Grid 4 :" + ex.ToString
        End Try
    End Sub

    Protected Sub Grid1_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid1.BeforeColumnSortingGrouping
        GetDataGrid1()
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
    Protected Sub Grid1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid1.PageIndexChanged
        GetDataGrid1()
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
    Protected Sub Grid1_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid1.ProcessColumnAutoFilter
        GetDataGrid1()
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub

    Protected Sub Grid2_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid2.BeforeColumnSortingGrouping
        GetDataGrid2()
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub
    Protected Sub Grid2_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid2.PageIndexChanged
        GetDataGrid2()
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub
    Protected Sub Grid2_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid2.ProcessColumnAutoFilter
        GetDataGrid2()
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub

    Protected Sub Grid3_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid3.BeforeColumnSortingGrouping
        GetDataGrid3()
        dsGrid3.ConnectionString = ViewState("Con")
        Grid3.DataBind()
    End Sub
    Protected Sub Grid3_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid3.PageIndexChanged
        GetDataGrid3()
        dsGrid3.ConnectionString = ViewState("Con")
        Grid3.DataBind()
    End Sub
    Protected Sub Grid3_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid3.ProcessColumnAutoFilter
        GetDataGrid3()
        dsGrid3.ConnectionString = ViewState("Con")
        Grid3.DataBind()
    End Sub

    Protected Sub Grid4_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid4.BeforeColumnSortingGrouping
        GetDataGrid4()
        dsGrid4.ConnectionString = ViewState("Con")
        Grid4.DataBind()
    End Sub
    Protected Sub Grid4_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid4.PageIndexChanged
        GetDataGrid4()
        dsGrid4.ConnectionString = ViewState("Con")
        Grid4.DataBind()
    End Sub
    Protected Sub Grid4_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid4.ProcessColumnAutoFilter
        GetDataGrid4()
        dsGrid4.ConnectionString = ViewState("Con")
        Grid4.DataBind()
    End Sub

    Protected Sub RefreshCek()
        div2.Visible = cbGrid1.Checked
        ddlNotasi.Visible = cbGrid1.Checked
        cbGrid2.Visible = cbGrid1.Checked
        DDLGrid2.Visible = cbGrid1.Checked
        If cbGrid1.Checked = False Then
            cbGrid2.Checked = False
        End If
        div3.Visible = cbGrid2.Checked
        DDLNotasi2.Visible = cbGrid2.Checked
        cbGrid3.Visible = cbGrid2.Checked
        DDLGrid3.Visible = cbGrid2.Checked
        If cbGrid2.Checked = False Then
            cbGrid3.Checked = False
        End If
        div4.Visible = cbGrid3.Checked
        DDLNotasi3.Visible = cbGrid3.Checked
        cbGrid4.Visible = cbGrid3.Checked
        DDLGrid4.Visible = cbGrid3.Checked
        If cbGrid3.Checked = False Then
            cbGrid4.Checked = False
        End If
    End Sub
End Class
