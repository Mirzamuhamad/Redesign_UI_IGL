Imports System.Data
Partial Class Master_ClosingAccountingPeriod_ClosingAccountingPeriod
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                BindDataView()
                'lbStatus.Text = "EXEC S_GLPeriodClosingView " + Session("GLYear") + "," + Session("GLPeriod")
                'Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Page Load Error :" + ex.ToString
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
        ViewState("GLPeriodName") = Session(Request.QueryString("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Request.QueryString("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Request.QueryString("KeyId"))("Address1")
        ViewState("Address2") = Session(Request.QueryString("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Request.QueryString("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Request.QueryString("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Request.QueryString("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    ' Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

    '     Try
    '         lbStatus.Text = MessageDlg("Update data success!.")

    '   Catch ex As Exception
    '        lbStatus.Text = "btn OK Error : " + ex.ToString
    '    End Try
    ' End Sub

    Protected Sub BindDataView()
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("EXEC S_GLPeriodClosingView " + ViewState("GLYear").ToString + "," + ViewState("GLPeriod").ToString, ViewState("DBConnection").ToString)
            dr = ds.Tables(0).Rows(0)
            lblYearMonth1.Text = dr("year").ToString + " - " + dr("PeriodName").ToString
            lblYearMonth2.Text = ""
            If dr("yearClose").ToString <> "" Then
                lblYearMonth2.Text = dr("yearClose").ToString + " - " + dr("PeriodNameClose").ToString
            End If
            lblYear.Text = dr("Year").ToString
            lblYear2.Text = dr("YearClose").ToString
            lblPeriod.Text = dr("Period").ToString
            lblPeriod2.Text = dr("PeriodClose").ToString
            btnClose.Visible = dr("FgClose").ToString = "N"
            btnUnClose.Visible = dr("FgUnClose").ToString = "N"
        Catch ex As Exception
            lbStatus.Text = "Page Load Error :" + ex.ToString
        End Try
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("EXEC S_GLPeriodClosingPost " + QuotedStr(lblYear.Text) + "," + QuotedStr(lblPeriod.Text) + "," + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
            BindDataView()

        Catch ex As Exception
            lbStatus.Text = "Page Load Error :" + ex.ToString
        End Try
    End Sub



    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnClose.Click
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("EXEC S_GLPeriodClosingUnPost " + QuotedStr(lblYear2.Text) + "," + QuotedStr(lblPeriod2.Text), ViewState("DBConnection").ToString)
            BindDataView()
        Catch ex As Exception
            lbStatus.Text = "Page Load Error :" + ex.ToString
        End Try
    End Sub



End Class
