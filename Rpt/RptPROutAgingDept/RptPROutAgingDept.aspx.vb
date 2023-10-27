Imports System.Data
Partial Class Rpt_RptPROutAgingDept_RptPROutAgingDept
    Inherits System.Web.UI.Page
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptPROutAgingDept")
                tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Print()
        Dim Result As String
        Dim force, Range1, Range2, Range3, Range4 As String
        Try
            Result = ReportGrid.ResultString

            If cbForce.Checked Then
                force = "0"
            Else
                force = "1"
            End If

            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4
            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_PRRptPROutAging '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "'," + Range1 + "," + Range2 + "," + Range3 + ",1,'" + Result + "','','','', " + RBFoward.SelectedIndex.ToString + ", " + RBType.SelectedIndex.ToString + "," + force + "," + QuotedStr(ViewState("UserId").ToString)
            If RBType.SelectedIndex = 0 Then
                Session("ReportFile") = ".../../../Rpt/RptPROutAgingDept.frx"
            Else
                Session("ReportFile") = ".../../../Rpt/RptPROutAgingDeptDetail.frx"
            End If
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "Print Error : " + ex.ToString
        End Try
    End Sub
End Class
