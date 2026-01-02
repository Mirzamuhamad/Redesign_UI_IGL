Imports System.Drawing.Printing
Imports CrystalDecisions.Shared


Partial Class Rpt_PrintMaster
    Inherits System.Web.UI.Page

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            'Session("Report") = Nothing
            CrystalReportViewer1.Dispose()
        Catch ex As Exception
            lbstatus.Text = "page on disposed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            CrystalReportViewer1.ReportSource = Session("Report")
            'CrystalReportViewer1.SeparatePages = False
            CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX
            CrystalReportViewer1.DataBind()



            Dim report As CrystalDecisions.CrystalReports.Engine.ReportDocument
            report = Session("Report")
            report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "ExportedReport")

            Dim client = New System.Net.WebClient()
            Dim buffer = client.DownloadData("ExportedReport")
            If Not buffer Is Nothing Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", buffer.Length.ToString())
                Response.BinaryWrite(buffer)
            Else
                lbstatus.Text = "ga ada isi"
            End If

            'report.PrintOptions.PrinterName = "FinePrint pdfFactory Pro"
            'report.
            'report.PrintToPrinter(1, False, 1, 1)

        Catch ex As Exception
            lbstatus.Text = "page load Error : " + ex.ToString
        End Try
    End Sub
End Class
