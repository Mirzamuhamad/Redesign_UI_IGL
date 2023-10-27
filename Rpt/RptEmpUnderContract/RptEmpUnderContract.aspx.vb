
Partial Class Rpt_RptEmpUnderContract_RptEmpUnderContract
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim Result, EmpStatus As String
        Try
            If rbStatus.SelectedIndex = 1 Then
                EmpStatus = "And A.Fg_Active = ''Y'' "
            ElseIf rbStatus.SelectedIndex = 2 Then
                EmpStatus = " And A.Fg_Active = ''N'' "
            Else
                EmpStatus = " And A.Fg_Active In (''Y'', ''N'') "
            End If

            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_PERptEmpUnderContract '" + Result + "', '', '" + EmpStatus + "', " + rbType.SelectedIndex.ToString()
            Session("ReportFile") = Server.MapPath("~\Rpt\RptEmpUnderContract.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
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
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptEmpUnderContract")
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Session("PrintType") = "Preview"
        btnPrint_Click(Nothing, Nothing)
    End Sub

End Class
