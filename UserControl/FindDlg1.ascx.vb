Imports System.Data
Partial Class UserControl_FindDlg1
    Inherits System.Web.UI.UserControl

    Public status As Boolean
    Public Event LoginStatus(ByVal sender As Object, ByVal e As EventArgs)

    'Public ReadOnly Property Islogin()
    '    Get
    '        Islogin = status
    '    End Get
    'End Property

    Protected Sub Login(ByVal sender As Object, ByVal e As EventArgs)
        ModalPopupExtender1.Show()
        'If isn LoginStatus Then
        If status = False Then
            status = True
            'LoginStatus(Me, EventArgs.Empty)
            ModalPopupExtender1.Hide()
        Else
            lbStatus.Text = "Sorry user name and password could not find"
        End If
    End Sub

    Public Sub EnableModelDialog(ByVal visibility As Boolean)
        If visibility Then
            ModalPopupExtender1.Show()
        Else
            ModalPopupExtender1.Hide()
        End If
    End Sub

    'Public Sub SetRpt(ByVal Rpt As String)
    '    Dim sqlstring As String
    '    Dim ds As DataSet
    '    Dim DR As DataRow
    '    Dim LI As ListItem
    '    Dim I As Integer

    '    Try
    '        sqlstring = "S_GetMsMenuGrid " + QuotedStr(Rpt)
    '        ds = SQLExecuteQuery(sqlstring)
    '        I = ds.Tables(0).Rows.Count
    '        Dim FieldKey(I - 1) As String
    '        I = 0
    '        For Each DR In ds.Tables(0).Rows
    '            LI = New ListItem(DR("MsSelect").ToString, DR("MsSelect").ToString)
    '            DDLGrid1.Items.Add(LI)
    '            FieldKey(I) = DR("FieldKey").ToString
    '            I = I + 1
    '        Next
    '        ViewState("FieldKey") = FieldKey
    '    Catch ex As Exception
    '        lStatus.Text = "Set Rpt Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub GetDataMs()
        'Try
        '    If cbGrid1.Checked Then
        '        dsGrid1.SelectCommand = "EXEC S_GetMsReport " + QuotedStr(Session("UserId")) + "," + _
        '        QuotedStr(DDLGrid1.SelectedValue) + ",'',''"
        '    Else
        '        dsGrid1.SelectCommand = "Select '' AS CODE, '' AS Description"
        '    End If
        'Catch ex As Exception
        '    lStatus.Text = "Get Data Ms Report :" + ex.ToString
        'End Try
    End Sub
    Public Function ResultString() As String
        'Dim ResultGrid1, Result As String
        'Dim Bendera1 As Boolean
        Try
            'If cbGrid1.Checked Then
            '    ResultGrid1 = SelectedGridDev(Grid1, ViewState("FieldKey")(DDLGrid1.SelectedIndex)).Replace("AND", "")

            '    Bendera1 = ResultGrid1.Length > 0

            '    Result = " AND (" + ResultGrid1 + ")"

            '    If Bendera1 Then
            '        Return Result
            '    Else
            '        Return ""
            '    End If
            'Else
            '    Return ""
            'End If
        Catch ex As Exception
            lbStatus.Text = "Result String Error : " + ex.ToString
        End Try
        Return ""
    End Function
    Public Sub setConnection(ByVal con As String)
        Try
            GetDataMs()
            'cbGrid1_CheckedChanged(Nothing, Nothing)
            'dsGrid1.ConnectionString = con
            'Grid1.DataBind()

            ViewState("Con") = con

        Catch ex As Exception
            lbStatus.Text = "Set Connection Error : " + ex.ToString
        End Try
    End Sub


End Class
