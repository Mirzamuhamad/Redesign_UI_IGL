Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptPLSummaryAdv_RptPLSummaryAdv
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            If (ddlYear2.SelectedValue > ddlYear.SelectedValue) Or (ddlYear.SelectedValue = ddlYear2.SelectedValue And CInt(ddlPeriod2.SelectedValue) > CInt(ddlPeriod.SelectedValue)) Then
                lbStatus.Text = MessageDlg("Compare period cannot greater than current period")
                Exit Sub
            End If
            If (ddlPeriod.SelectedValue = ddlPeriod2.SelectedValue) And (ddlYear.SelectedValue = ddlYear2.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
                lbStatus.Text = MessageDlg("Compare period cannot same with current period")
                'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            Else
                Session("PrintType") = "Print"
                print()
            End If
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
                ReportGrid.SetRpt("RptPLSummaryAdv")
                FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlYear2, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriod2, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                    ddlYear.SelectedValue = ViewState("GLYear").ToString.Trim
                End If
                If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").ToString.Trim)) Then
                    ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString.Trim
                End If
                If CFloat(ViewState("GLPeriod").ToString) = 1 Then
                    If ddlYear2.Items.Contains(ddlYear2.Items.FindByValue(ViewState("GLYear").ToString.Trim - 1)) Then
                        ddlYear2.SelectedValue = ViewState("GLYear").ToString.Trim - 1
                    End If
                    If ddlPeriod2.Items.Contains(ddlPeriod2.Items.FindByValue(12)) Then
                        ddlPeriod2.SelectedValue = 12
                    End If
                Else
                    If ddlYear2.Items.Contains(ddlYear2.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                        ddlYear2.SelectedValue = ViewState("GLYear").ToString.Trim
                    End If
                    If ddlPeriod2.Items.Contains(ddlPeriod2.Items.FindByValue(ViewState("GLPeriod").ToString.Trim - 1)) Then
                        ddlPeriod2.SelectedValue = ViewState("GLPeriod").ToString.Trim - 1
                    End If
                End If
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub print()
        Try
            Dim YTD, ReportType As String
            Dim Result As String
            Result = ReportGrid.ResultString
            If cbYTD.Checked Then
                YTD = "1"
            Else
                YTD = "0"
            End If
            If Request.QueryString("ContainerId").ToString = "RptPLSummaryAdvExId" Then
                ReportType = "1"
            Else
                ReportType = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_GLRptPLSummaryAdv " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYear2.SelectedValue + "," + ddlPeriod2.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + "," + rbType.SelectedValue + "," + YTD + ", '" + Result + "', '', ''"
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            If rbType.SelectedValue = "0" Then
                If YTD = "1" Then
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdvYTD.frx")
                Else
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdv.frx")
                End If
            ElseIf rbType.SelectedValue = "1" Then
                If YTD = "1" Then
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdvGroupYTD.frx")
                Else
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdvGroup.frx")
                End If
            Else
                If YTD = "1" Then
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdvClassYTD.frx")
                Else
                    Session("ReportFile") = Server.MapPath("~\Rpt\RptPLSummaryAdvClass.frx")
                End If

            End If
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            If (ddlYear2.SelectedValue > ddlYear.SelectedValue) Or (ddlYear.SelectedValue = ddlYear2.SelectedValue And CInt(ddlPeriod2.SelectedValue) > CInt(ddlPeriod.SelectedValue)) Then
                lbStatus.Text = "Compare period cannot greater than current period"
                Exit Sub
            End If
            If (ddlPeriod.SelectedValue = ddlPeriod2.SelectedValue) And (ddlYear.SelectedValue = ddlYear2.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
                lbStatus.Text = "Compare period cannot same with current period"
                'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            Else
                Session("PrintType") = "Preview"
                print()
            End If
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
    
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim SQLString As String
        Dim YTD, ReportType As String
        Dim Result As String

        Try
            Result = ReportGrid.ResultString
            If cbYTD.Checked Then
                YTD = "1"
            Else
                YTD = "0"
            End If
            If Request.QueryString("ContainerId").ToString = "RptPLSummaryAdvExId" Then
                ReportType = "1"
            Else
                ReportType = "0"
            End If
            SQLString = "EXEC S_GLRptPLSummaryAdv " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYear2.SelectedValue + "," + ddlPeriod2.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + "," + rbType.SelectedValue + "," + YTD + ", '" + Result + "', '', ''"

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptPLSummaryAdv")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class