Imports System.Data
Partial Class UserControl_ReportSingleGrid
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
                FieldKey(I) = DR("FieldKey").ToString
                I = I + 1
            Next
            cbGrid1.Visible = (Count >= 1)
            DDLGrid1.Visible = cbGrid1.Visible
            Grid1.Visible = cbGrid1.Visible
            ViewState("FieldKey") = FieldKey
        Catch ex As Exception
            lStatus.Text = "Set Rpt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub cbGrid1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid1.CheckedChanged, DDLGrid1.SelectedIndexChanged
        Try
            Grid1.Selection.UnselectAll()
            GetDataMs()
            'If cbGrid1.Checked Then
            '    dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
            '    QuotedStr(DDLGrid1.SelectedValue) + ",'',''"
            'Else
            '    dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
            'End If
        Catch ex As Exception
            lStatus.Text = "cb Grid 1 Checked Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub GetDataMs()
        Try
            If cbGrid1.Checked Then
                dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserGrid")) + "," + _
                QuotedStr(DDLGrid1.SelectedValue) + ",'',''"
            Else
                dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Ms Report :" + ex.ToString
        End Try
    End Sub
    Public Function ResultString() As String
        Dim ResultGrid1, Result As String
        Dim Bendera1 As Boolean
        Try
            If cbGrid1.Checked Then
                ResultGrid1 = SelectedGridDev(Grid1, ViewState("FieldKey")(DDLGrid1.SelectedIndex)).Replace("AND", "")

                Bendera1 = ResultGrid1.Length > 0

                Result = " AND (" + ResultGrid1 + ")"

                If Bendera1 Then
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
            GetDataMs()
            'cbGrid1_CheckedChanged(Nothing, Nothing)
            dsGrid1.ConnectionString = con
            Grid1.DataBind()

            ViewState("Con") = con
            ViewState("UserGrid") = userid
        Catch ex As Exception
            lStatus.Text = "Set Connection Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Grid1_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles Grid1.BeforeColumnSortingGrouping
        GetDataMs()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
    Protected Sub Grid1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid1.PageIndexChanged
        GetDataMs()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
    Protected Sub Grid1_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles Grid1.ProcessColumnAutoFilter
        GetDataMs()
        'cbGrid1_CheckedChanged(Nothing, Nothing)
        dsGrid1.ConnectionString = ViewState("Con")
        Grid1.DataBind()
    End Sub
End Class
