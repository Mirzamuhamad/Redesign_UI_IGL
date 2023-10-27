Imports System.Data
Partial Class UserControl_CategoryReport
    Inherits System.Web.UI.UserControl

    Public Sub SetRpt(ByVal Rpt As String)
        Dim sqlstring As String
        Dim ds As DataSet
        Dim DR As DataRow
        Dim LI As ListItem
        Dim I As Integer

        Try
            sqlstring = "S_GetMsMenuGrid " + QuotedStr(Rpt)
            ds = SQLExecuteQuery(sqlstring, ViewState("Con"))
            I = ds.Tables(0).Rows.Count
            Dim FieldKey(I - 1) As String
            I = 0
            For Each DR In ds.Tables(0).Rows
                LI = New ListItem(DR("MsSelect").ToString, DR("MsSelect").ToString)
                DDLGrid1.Items.Add(LI)
                FieldKey(I) = DR("FieldKey").ToString
                I = I + 1
            Next
            ViewState("FieldKey") = FieldKey

            FillGroup()

        Catch ex As Exception
            lStatus.Text = "Set Rpt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillGroup()
        Dim sqlstring As String
        Dim ds As DataSet
        Dim dr As DataRow
        Try
            sqlstring = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserCat")) + ", 'Category Report', '',''"
            ds = SQLExecuteQuery(sqlstring, ViewState("Con"))
            ddlGroup.Items.Clear()
            For Each dr In ds.Tables(0).Rows
                Dim dl As New ListItem(dr("Description").ToString, dr("Code").ToString)
                ddlGroup.Items.Add(dl)
            Next
        Catch ex As Exception
            Throw New Exception("Fill Group Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub setConnection(ByVal con As String, ByVal userid As String)
        Try
            GetDataMs()
            'cbGrid1_CheckedChanged(Nothing, Nothing)
            dsGrid1.ConnectionString = con
            Grid1.DataBind()

            ViewState("Con") = con
            ViewState("UserCat") = userid
        Catch ex As Exception
            lStatus.Text = "Set Connection Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbGrid1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbGrid1.CheckedChanged, DDLGrid1.SelectedIndexChanged
        Try
            Grid1.Selection.UnselectAll()
            GetDataMs()
            'If cbGrid1.Checked Then
            '    dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(Session("UserId")) + "," + _
            '    QuotedStr(DDLGrid1.SelectedValue) + "," + QuotedStr(ddlGroup.SelectedValue) + ",''"
            '    DDLGrid1.Enabled = False
            '    ddlGroup.Enabled = False
            'Else
            '    dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
            '    DDLGrid1.Enabled = True
            '    ddlGroup.Enabled = True
            'End If
        Catch ex As Exception
            lStatus.Text = "cb Grid 1 Checked Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub GetDataMs()
        Try
            If cbGrid1.Checked Then
                dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(ViewState("UserCat")) + "," + _
                QuotedStr(DDLGrid1.SelectedValue) + "," + QuotedStr(ddlGroup.SelectedValue) + ",''"
                DDLGrid1.Enabled = False
                ddlGroup.Enabled = False
            Else
                dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
                DDLGrid1.Enabled = True
                ddlGroup.Enabled = True
            End If
        Catch ex As Exception
            lStatus.Text = "Get Data Ms Report :" + ex.ToString
        End Try
    End Sub
    Public Function ResultString() As String
        Dim ResultGrid1 As String
        Dim Bendera1 As Boolean
        Try
            If cbGrid1.Checked Then
                ResultGrid1 = SelectedGridDev(Grid1, ViewState("FieldKey")(DDLGrid1.SelectedIndex)).Replace("AND", "")
                Bendera1 = ResultGrid1.Length > 0
                If Bendera1 Then
                    Return " AND " + ResultGrid1 + " "
                Else
                    Return " "
                End If
            Else
                Return " "
            End If
        Catch ex As Exception
            lStatus.Text = "Result String Error : " + ex.ToString
        End Try
        Return ""
    End Function

    Public Function ResultCategory() As String
        Try
            Return " AND (P.ProductCategory = ''" + ddlGroup.SelectedValue + "'') "
        Catch ex As Exception
            Throw New Exception("result Category Error : " + ex.ToString)
        End Try
    End Function

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
