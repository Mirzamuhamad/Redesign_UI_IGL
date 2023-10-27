Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Rpt_RptARDetail_RptARDetail
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
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
                ReportGrid.SetRpt("RptARDetail")
                tbStartDate.SelectedDate = ViewState("ServerDate")
                tbEndDate.SelectedDate = ViewState("ServerDate")
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
    Private Sub print()
        Dim Result, FgReport As String
        Try
            Result = ReportGrid.ResultString
            If cbAll.Checked = True Then
                FgReport = "YYYYYY"
            Else
                FgReport = ""
                If cbSales.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbDN.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbCN.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbReceiptAR.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbGiro.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbDP.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
            End If

            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FNRptARDetail '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + _
            "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + Result + "','','','" + _
             FgReport + "'," + QuotedStr(ViewState("UserId").ToString)
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = Server.MapPath("~\Rpt\RptARDetail.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
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
        Dim Result, FgReport As String
        Dim dt As DataTable
        Dim SQLString As String
        Try
            Result = ReportGrid.ResultString
            If cbAll.Checked = True Then
                FgReport = "YYYYYY"
            Else
                FgReport = ""
                If cbSales.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbDN.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbCN.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbReceiptAR.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbGiro.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
                If cbDP.Checked Then
                    FgReport = FgReport + "Y"
                Else
                    FgReport = FgReport + "N"
                End If
            End If

            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            SQLString = "EXEC S_FNRptARDetail '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + _
            "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "','" + Result + "','','','" + _
             FgReport + "'," + QuotedStr(ViewState("UserId").ToString)


            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("FNARDetail")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class