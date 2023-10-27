
Partial Class Rpt_RptCOA_RptCOA
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try

            Session("PrintType") = "Print"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Print()
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_GLRptMsAccount " + RBList1.SelectedIndex.ToString + ", '' "
            'lbStatus.Text = Session("SelectCommand")
            Session("ReportFile") = ".../../../Rpt/RptCOA.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "Print Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        If Not Page.IsPostBack Then
            InitProperty()
            'FillComboAll(ddlCompany, "Select Company FROM VMsCompany", False, "Company", "Company", ViewState("DBConnection").ToString)
        End If
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

End Class
