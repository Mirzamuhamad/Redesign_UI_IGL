Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptUserRight_RptUserRight
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
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
                ReportGrid.SetRpt("RptUserRight")
                'tbStartDate.SelectedDate = Now
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub print()
        Dim Result As String
        Try
            Result = ReportGrid.ResultString

            'If rbGroup.SelectedIndex.ToString = "0" Then
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_SARptUserRight '" + Result + "','','', " + rbGroup.SelectedIndex.ToString + " , " + rbStatus.SelectedIndex.ToString
            Session("ReportFile") = Server.MapPath("~\Rpt\RptUserRight.frx")
            
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim SQLString As String
        Dim Result As String
        Try
            Result = ReportGrid.ResultString

            SQLString = "EXEC S_SARptUserRight '" + Result + "','','', " + rbGroup.SelectedIndex.ToString + " , " + rbStatus.SelectedIndex.ToString

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptUserRight")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
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
End Class
