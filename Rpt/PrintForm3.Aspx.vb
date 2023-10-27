Imports System.IO
Imports System.IO.Stream
Imports System.Data

Partial Class Rpt_PrintForm3
    Inherits System.Web.UI.Page

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            'Session("SelectCommand") = Nothing
            'Session("SelectCommand2") = Nothing
            'Session("SelectCommand3") = Nothing
            'Session("ReportFile") = Nothing
            'Session("PrintType") = Nothing
            Me.WebReport1.Dispose()
        Catch ex As Exception
            lbstatus.Text = "page disposed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                ViewState("SelectCommand") = Session("SelectCommand")
                ViewState("SelectCommand2") = Session("SelectCommand2")
                ViewState("SelectCommand3") = Session("SelectCommand3")
                ViewState("ReportFile") = Session("ReportFile")
                ViewState("PrintType") = Session("PrintType")
                ViewState("DBConnection") = Session("DBConnection")

                Session("SelectCommand") = Nothing
                Session("SelectCommand2") = Nothing
                Session("SelectCommand3") = Nothing
                Session("ReportFile") = Nothing
                Session("PrintType") = Nothing
                Session("DBConnection") = Nothing

                If Me.WebReport1.ReportFile = "" Then
                    If ViewState("PrintType") = "Preview" Then
                        WebReport1.ReportFile = ViewState("ReportFile")
                        'WebReport1.ShowPrint = False
                        WebReport1.ShowRefreshButton = False
                        WebReport1.Prepare()
                    Else
                        WebReport1.ReportFile = ViewState("ReportFile")
                        'WebReport1.ShowPrint = False
                        WebReport1.ShowRefreshButton = False
                        WebReport1.Prepare()

                        Dim export As New FastReport.Export.Pdf.PDFExport
                        Dim a As New MemoryStream
                        WebReport1.Report.Export(export, a)
                        Dim buffer = a.ToArray

                        If Not buffer Is Nothing Then
                            Response.ContentType = "application/pdf"
                            Response.AddHeader("content-length", buffer.Length.ToString())
                            Response.BinaryWrite(buffer)
                        Else
                            lbstatus.Text = "ga ada isi"
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            lbstatus.Text = "page load error : " + ex.ToString
        End Try
    End Sub

    Protected Sub WebReport1_StartReport(ByVal sender As Object, ByVal e As System.EventArgs) Handles WebReport1.StartReport
        Try
            Dim DS1 As DataSet
            DS1 = SQLExecuteQuery(ViewState("SelectCommand"), ViewState("DBConnection"))
            WebReport1.Report.RegisterData(DS1.Tables(0), "DS")

            Dim DS2 As DataSet
            DS2 = SQLExecuteQuery(ViewState("SelectCommand2"), ViewState("DBConnection"))
            WebReport1.Report.RegisterData(DS2.Tables(0), "DS2")

            Dim DS3 As DataSet
            DS3 = SQLExecuteQuery(ViewState("SelectCommand3"), ViewState("DBConnection"))
            WebReport1.Report.RegisterData(DS3.Tables(0), "DS3")
        Catch ex As Exception
            lbstatus.Text = "Start Report Error : " + ex.ToString
        End Try
    End Sub
End Class
