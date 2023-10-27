
Partial Class Rpt_RptEmpStatistikFR_ReportTemplate
    Inherits System.Web.UI.Page

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
                FillCombo(ddlStartYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlEndYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                ReportGrid.SetRpt("RptEmpStatistikFR")
                BindToDropList(ddlStartYear, ViewState("GLYear").ToString)
                BindToDropList(ddlEndYear, ViewState("GLYear").ToString)                
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Private Sub print()
        Dim Result As String
        Try
            If ddlEndYear.SelectedValue < ddlStartYear.SelectedValue Then
                lbStatus.Text = MessageDlg("End Year must Greater than Start Year")
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Result = ReportGrid.ResultString
            Session("SelectCommand") = "EXEC S_PERptEmpStatistikFR " + ddlStartYear.SelectedValue + "," + ddlEndYear.SelectedValue + ",'" + Result + "', '', '', '',  " + ViewState("UserId").ToString
            Session("ReportFile") = Server.MapPath("~\Rpt\RptEmpStatistikFR.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
        End Try
    End Sub
End Class
