Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Rpt_RptStockCard2_RptStockCard2
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim Result, ResultCat, StrUnion, FgReport As String
        Try
            Result = ReportGrid.ResultString
            ResultCat = "" 'ReportGrid.ResultCategory.ToString
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            StrUnion = "N"
            If cbUnion.Checked Then
                StrUnion = "Y"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_STRptStockCard2 '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', ' ', '" + Result + "', '', " + FgReport + "," + QuotedStr(ViewState("UserId").ToString)


            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCard.frx")
            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardValID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardValue.frx")
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
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                Labelmenu.Text = "Report Stock Card"

            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardValID" Then
                Labelmenu.Text = "Report Stock Card Value"

            End If
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                    ReportGrid.SetRpt("RptStockCard")

                ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardValID" Then
                    ReportGrid.SetRpt("RptStockCardVal")
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
        Dim Result, ResultCat, StrUnion, FgReport As String
        Try
            Result = ReportGrid.ResultString
            ResultCat = "" 'ReportGrid.ResultCategory.ToString
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            StrUnion = "N"
            If cbUnion.Checked Then
                StrUnion = "Y"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            Session("SelectCommand") = "EXEC S_STRptStockCard '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', ' ', '" + Result + "', '', " + FgReport + "," + QuotedStr(ViewState("UserId").ToString)
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCard.frx")
            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardValID" Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptStockCardValue.frx")
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
        Dim Result, ResultCat, StrUnion, FgReport As String
        Dim dt As DataTable
        Dim SQLString As String
        Try
            Result = ReportGrid.ResultString
            ResultCat = "" 'ReportGrid.ResultCategory.ToString
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                FgReport = "1"
            Else
                FgReport = "0"
            End If
            StrUnion = "N"
            If cbUnion.Checked Then
                StrUnion = "Y"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            SQLString = "EXEC S_STRptStockCard2 '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', ' ', '" + Result + "', '', " + FgReport + "," + QuotedStr(ViewState("UserId").ToString)

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            If Request.QueryString("ContainerId").ToString = "RptStockCardID" Then
                ExportGridToExcel("Stock Card")
            ElseIf Request.QueryString("ContainerId").ToString = "RptStockCardValID" Then
                ExportGridToExcel("Stock Card Value")
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class
