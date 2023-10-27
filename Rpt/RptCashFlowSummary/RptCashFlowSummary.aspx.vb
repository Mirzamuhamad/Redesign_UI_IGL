Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class RptCashFlowForcash
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            'If (ddlPeriod.SelectedValue = ddlPeriodEnd.SelectedValue) And (ddlYear.SelectedValue = ddlYearEnd.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
            'lbStatus.Text = "Compare period cannot same with current period"
            'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            'Else
            print()
            'End If
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
                ReportGrid.SetRpt("RptCashFlowSummary")
                'tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")

                FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                'FillCombo(ddlYearEnd, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                'FillCombo(ddlCurrency, "SELECT CurrCode FROM MsCurrency WHERE CurrCode in ('IDR', 'Rp', 'USD', 'US$') ", False, "CurrCode", "CurrCode", ViewState("DBConnection"))

                'FillCombo(ddlPeriodEnd, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                    ddlYear.SelectedValue = ViewState("GLYear").ToString.Trim
                End If
                If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").ToString.Trim)) Then
                    ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString.Trim
                End If
                'tbRate.Text = SQLExecuteScalar("Select TOP 1 dbo.FormatFloat(CurrRate,2) from MsCurrRate WHERE CurrCode IN ('USD','US$') and CurrDate <= GetDate() AND CurrRate > 0 ORDER BY CurrDate DESC ", ViewState("DBConnection"))

            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub print()
        Try
            Dim result As String
            result = ReportGrid.ResultString
            'If tbRate.Text.Trim = "" Then
            '    lbStatus.Text = "Currency rate must be inputed"
            '    Exit Sub
            'End If
            If ddlCurrency.SelectedIndex = 1 Then
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_GLRptCashFlowSummary " + ddlYear.SelectedValue + _
                "," + ddlPeriod.SelectedValue + ", " + QuotedStr(ddlCurrency.SelectedValue) + ",'" + result + "', 0 "
                Session("SelectCommand2") = "EXEC S_GLRptCashFlowSummary " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + ", " + QuotedStr(ddlCurrency.SelectedValue) + ",'" + result + "', 2 "
                Session("ReportFile") = Server.MapPath("~\Rpt\RptCashFlowSummaryAct.frx")

                'lbStatus.Text = Session("SelectCommand1")
                'Exit Sub
                AttachScript("openreport2();", Page, Me.GetType)
            Else
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_GLRptCashFlowSummary " + ddlYear.SelectedValue + _
                "," + ddlPeriod.SelectedValue + ", " + QuotedStr(ddlCurrency.SelectedValue) + ",'" + result + "', 0 "
                Session("ReportFile") = Server.MapPath("~\Rpt\RptCashFlowSummary.frx")
                AttachScript("openreport();", Page, Me.GetType)
            End If

            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub


        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            'If (ddlPeriod.SelectedValue = ddlPeriodEnd.SelectedValue) And (ddlYear.SelectedValue = ddlYearEnd.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
            'lbStatus.Text = "Compare period cannot same with current period"
            'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            'Else
            print()
            'End If
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim SQLString, result As String
        Try
            result = ReportGrid.ResultString
            SQLString = "EXEC S_GLRptCashFlowSummary " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + ", " + QuotedStr(ddlCurrency.SelectedValue) + ",'" + result + "', 1 "

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptCashFlowSummary")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class