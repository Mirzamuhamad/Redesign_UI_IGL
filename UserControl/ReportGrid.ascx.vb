Imports System.Data
Imports System.Collections.Generic
Partial Class ReportGrid
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
                FieldKey(I) = DR("FieldKey").ToString
                I = I + 1
            Next
            cbGrid1.Visible = (Count >= 1)
            DDLGrid1.Visible = cbGrid1.Visible
            Grid1.Visible = cbGrid1.Visible
            ddlNotasi.Visible = cbGrid1.Visible
            DDLGrid2.Visible = cbGrid1.Visible
            Grid2.Visible = cbGrid1.Visible
            ViewState("FieldKey") = FieldKey
        Catch ex As Exception
            lStatus.Text = "Set Rpt Error : " + ex.ToString
        End Try
    End Sub

    Public Function ResultString() As String
        Dim ResultGrid1, ResultGrid2, Result As String
        Dim Bendera1, Bendera2 As Boolean
        Try
            If cbGrid1.Checked Then
                'AND ( A.Supplier In(''JK-A0002'',''JK-A0003'') OR A.Supplier In(''JK-A0005'',''JK-A0006'')
                ResultGrid1 = ""
                ResultGrid2 = ""


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


                Bendera1 = ResultGrid1.Length > 0
                Bendera2 = ResultGrid2.Length > 0

                Result = " AND (" + ResultGrid1 + " " + ResultGrid2 + ")"

                If Bendera1 Or Bendera2 Then
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
            'cbGrid1_CheckedChanged(Nothing, Nothing)
            dsGrid1.ConnectionString = con
            Grid1.DataBind()

            GetDataGrid2()
            'cbGrid2_CheckedChanged(Nothing, Nothing)
            dsGrid2.ConnectionString = con
            Grid2.DataBind()

            ViewState("Con") = con
            ViewState("UserGrid") = userid
        Catch ex As Exception
            lStatus.Text = "Set Connection Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbGrid2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid2.CheckedChanged, DDLGrid2.SelectedIndexChanged
        Try
            Grid2.Selection.UnselectAll()
            GetDataGrid2()
        Catch ex As Exception
            lStatus.Text = "cb Grid 2 Checked Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub cbGrid1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid1.CheckedChanged, DDLGrid1.SelectedIndexChanged
        Try
            Grid1.Selection.UnselectAll()
            GetDataGrid1()
        Catch ex As Exception
            lStatus.Text = "cb Grid 1 Checked Error :" + ex.ToString
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

    Protected Sub Grid1_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid1.BeforeColumnSortingGrouping
        GetDataGrid1()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub

    Protected Sub Grid1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid1.PageIndexChanged
        GetDataGrid1()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
    Protected Sub Grid1_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid1.ProcessColumnAutoFilter
        GetDataGrid1()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub

    Protected Sub Grid2_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid2.BeforeColumnSortingGrouping
        GetDataGrid2()
        'cbGrid2_CheckedChanged(Nothing, Nothing)
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub
    Protected Sub Grid2_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid2.PageIndexChanged
        GetDataGrid2()
        'cbGrid2_CheckedChanged(Nothing, Nothing)
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub
    Protected Sub Grid2_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid2.ProcessColumnAutoFilter
        GetDataGrid2()
        'cbGrid2_CheckedChanged(Nothing, Nothing)
        dsGrid2.ConnectionString = ViewState("Con")
        Grid2.DataBind()
    End Sub
End Class
