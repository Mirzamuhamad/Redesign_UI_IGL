Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptBSSheet2_RptBSSheet2
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try
            ViewState("PrintType") = "Print"
            If (ddlPeriod.SelectedValue = ddlPeriodEnd.SelectedValue) And (ddlYear.SelectedValue = ddlYearEnd.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
                lbStatus.Text = "Compare period cannot same with current period"
                'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            Else
                print()
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlYearEnd, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriodEnd, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))

                If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                    ddlYear.SelectedValue = ViewState("GLYear").ToString.Trim
                End If
                If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").ToString.Trim)) Then
                    ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString.Trim
                End If
                If CFloat(ViewState("GLPeriod").ToString) = 1 Then
                    If ddlYearEnd.Items.Contains(ddlYearEnd.Items.FindByValue(ViewState("GLYear").ToString.Trim - 1)) Then
                        ddlYearEnd.SelectedValue = ViewState("GLYear").ToString.Trim - 1
                    End If
                    If ddlPeriodEnd.Items.Contains(ddlPeriodEnd.Items.FindByValue(12)) Then
                        ddlPeriodEnd.SelectedValue = 12
                    End If
                Else
                    If ddlYearEnd.Items.Contains(ddlYearEnd.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                        ddlYearEnd.SelectedValue = ViewState("GLYear").ToString.Trim
                    End If
                    If ddlPeriodEnd.Items.Contains(ddlPeriodEnd.Items.FindByValue(ViewState("GLPeriod").ToString.Trim - 1)) Then
                        ddlPeriodEnd.SelectedValue = ViewState("GLPeriod").ToString.Trim - 1
                    End If
                End If

                'If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").Trim)) Then
                '    ddlYear.SelectedValue = ViewState("GLYear").Trim
                'End If
                'If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").Trim)) Then
                '    ddlPeriod.SelectedValue = ViewState("GLPeriod").Trim
                'End If
                'If CFloat(ViewState("GLPeriod").ToString) = 1 Then
                '    If ddlYearEnd.Items.Contains(ddlYearEnd.Items.FindByValue(ViewState("GLYear").Trim - 1)) Then
                '        ddlYearEnd.SelectedValue = ViewState("GLYear").Trim - 1
                '    End If
                '    If ddlPeriodEnd.Items.Contains(ddlPeriodEnd.Items.FindByValue(12)) Then
                '        ddlPeriodEnd.SelectedValue = 12
                '    End If
                'Else
                '    If ddlYearEnd.Items.Contains(ddlYearEnd.Items.FindByValue(ViewState("GLYear").Trim)) Then
                '        ddlYearEnd.SelectedValue = ViewState("GLYear").Trim
                '    End If
                '    If ddlPeriodEnd.Items.Contains(ddlPeriodEnd.Items.FindByValue(ViewState("GLPeriod").Trim - 1)) Then
                '        ddlPeriodEnd.SelectedValue = ViewState("GLPeriod").Trim - 1
                '    End If
                'End If
            End If
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


    Private Sub print()
        Dim Value, ReportType As String
        Try
            If cbValue.Checked Then
                Value = "1"
            Else
                Value = "0"
            End If
            If Request.QueryString("ContainerId").ToString = "RptBSSheet2ExId" Then
                ReportType = "1"
            Else
                ReportType = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")

            Session("SelectCommand") = "EXEC S_GLRptBSheet " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYearEnd.SelectedValue + _
            "," + ddlPeriodEnd.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + "," + rbType.SelectedIndex.ToString + ",1," + Value '+ ", " + ReportType

            Session("SelectCommand2") = "EXEC S_GLRptBSheet " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYearEnd.SelectedValue + _
            "," + ddlPeriodEnd.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + "," + rbType.SelectedIndex.ToString + ",2," + Value '+ ", " + ReportType

            'lbStatus.Text = Session("SelectCommand2")
            'Exit Sub
            If rbType.SelectedValue = "0" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptBSSheet2Sum.frx")
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\RptBSSheet2.frx")
            End If
            AttachScript("openprintdlgs();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            If (ddlPeriod.SelectedValue = ddlPeriodEnd.SelectedValue) And (ddlYear.SelectedValue = ddlYearEnd.SelectedValue) And (ddlBudgetCurrent.SelectedValue = ddlBudgetCompare.SelectedValue) Then
                lbStatus.Text = "Compare period cannot same with current period"
                'ddlPeriodEnd.SelectedValue = ddlPeriod.SelectedValue - 1
            Else
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
        GridExport2.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        form.Controls.Add(GridExport2)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Dim dt, dt2 As DataTable
        Dim SQLString, SQLString2, Value, ReportType As String
        Try

            If cbValue.Checked Then
                Value = "1"
            Else
                Value = "0"
            End If
            If Request.QueryString("ContainerId").ToString = "RptBSSheet2ExId" Then
                ReportType = "1"
            Else
                ReportType = "0"
            End If

            SQLString = "EXEC S_GLRptBSheet " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYearEnd.SelectedValue + _
            "," + ddlPeriodEnd.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + ", " + rbType.SelectedIndex.ToString + ",1," + Value '+ ", " + ReportType

            SQLString2 = "EXEC S_GLRptBSheet " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + "," + ddlBudgetCurrent.SelectedIndex.ToString + "," + ddlYearEnd.SelectedValue + _
            "," + ddlPeriodEnd.SelectedValue + "," + ddlBudgetCompare.SelectedIndex.ToString + "," + rbType.SelectedIndex.ToString + ",2," + Value '+ ", " + ReportType

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            dt2 = SQLExecuteQuery(SQLString2, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport2.DataSource = dt2
            GridExport.DataBind()
            GridExport2.DataBind()
            ExportGridToExcel("RptBSSheet2")

        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class