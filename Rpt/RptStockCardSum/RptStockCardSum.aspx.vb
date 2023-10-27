Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Rpt_RptStockCardSum_RptStockCardSum
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim Result, Fgforce, FgReport As String
        Try
            Result = ReportGrid.ResultString
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_STRptStockCardSum '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + Result + "','','', " + FgReport + " ," + QuotedStr(ViewState("UserId").ToString) + "," + Fgforce
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardSum.frx")
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardSumVal.frx")
            End If
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
            'ReportGrid.setConnection(Session("DBConnection"))
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                Labelmenu.Text = "Report Stock Card Summary"

            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardSumValID" Then
                Labelmenu.Text = "Report Stock Card Summary Value"
            End If
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                    ReportGrid.SetRpt("RptStockCardSum")

                ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardSumValID" Then
                    ReportGrid.SetRpt("RptStockCardSumVal")
                End If
                tbStartDate.SelectedDate = ViewState("ServerDate")
                tbEndDate.SelectedDate = ViewState("ServerDate")
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim Result, Fgforce, FgReport As String
        Try
            Result = ReportGrid.ResultString
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            Session("SelectCommand") = "EXEC S_STRptStockCardSum '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + Result + "','',''," + FgReport + " ," + QuotedStr(ViewState("UserId").ToString) + "," + Fgforce
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardSum.frx")
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardSumVal.frx")
            End If
            AttachScript("openprintdlg();", Page, Me.GetType)
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
        Dim Result, Fgforce, FgReport As String
        Try
            Result = ReportGrid.ResultString
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            SQLString = "EXEC S_STRptStockCardSum '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + Result + "','',''," + FgReport + " ," + QuotedStr(ViewState("UserId").ToString) + "," + Fgforce

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            If Request.QueryString("ContainerId").ToString = "RptStockCardSumID" Then
                ExportGridToExcel("Stock Card Summary")
            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardSumValID" Then
                ExportGridToExcel("Stock Card Summary Value")
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class
