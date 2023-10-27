Imports System.Data
Imports System.IO

Partial Class Rpt_RptStockMutationSum_RptStockMutationSum
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            'ReportGrid.setConnection(Session("DBConnection"))
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                tbStartDate.SelectedDate = ViewState("ServerDate")
                tbEndDate.SelectedDate = ViewState("ServerDate")
                'If Request.QueryString("ContainerId").ToString = "RptStockMutationSumID" Then
                '    ReportGrid.SetRpt("RptStockMutationSumFG")
                pnlharga.Visible = True
                'Else
                '    ReportGrid.SetRpt("RptStockMutationSumRM")
                '    pnlharga.Visible = True
                'End If
                ReportGrid.SetRpt("RptStockMutationSum")
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
    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
    Private Sub Print()
        Dim ResultGrid, ResultCat, FgValue, FgForce, FgReport, FgReject As String
        Try
            ResultGrid = ReportGrid.ResultString
            ResultCat = ""
            If cbForce.Checked Then
                FgForce = "1"
            Else
                FgForce = "0"
            End If
            If cbValue.Checked Then
                FgValue = "Y"
            Else
                FgValue = "N"
            End If
            'FgReject = RbReject.SelectedValue
            FgReport = "'0'"

            Session("DBConnection") = ViewState("DBConnection")
            If rgReportType.SelectedIndex = 0 Then
                Session("SelectCommand") = "EXEC S_STRptStockMutationSum '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + ResultCat + "', '" + ResultGrid + "','" + FgValue + "', " + FgReport + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + FgForce
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockMutationSum.frx")
            Else
                Session("SelectCommand") = "EXEC S_STRptStockMutationSumDt '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + ResultCat + "', '" + ResultGrid + "','" + FgValue + "', " + FgReport + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + FgForce
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockMutationSumDt.frx")
            End If

            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "Print Error : " + ex.ToString
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
        Dim FgReport As String
        Dim Result As String
        Dim dt As DataTable
        Dim SQLString As String
        Dim cat, Range1, Range2, Range3, ResultCat, ResultGrid, FgReject, Range4, cbcek, FgValue, FgForce As String
        Try
            ResultGrid = ReportGrid.ResultString
            ResultCat = ""
            If cbForce.Checked Then
                FgForce = "1"
            Else
                FgForce = "0"
            End If
            If cbValue.Checked Then
                FgValue = "Y"
            Else
                FgValue = "N"
            End If
            FgReport = "0"
            'FgReject = RbReject.SelectedValue
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            If rgReportType.SelectedIndex = 0 Then
                SQLString = "EXEC S_STRptStockMutationSum '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + ResultCat + "', '" + ResultGrid + "','" + FgValue + "', " + FgReport + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + FgForce

            Else
                SQLString = "EXEC S_STRptStockMutationSumDt '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + ResultCat + "', '" + ResultGrid + "','" + FgValue + "', " + FgReport + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + FgForce
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
            End If
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("Stock Mutation Sum")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub



End Class
