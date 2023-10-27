Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptLeaveCard_RptLeaveCard
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
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

    Private Sub print()
        Dim Result As String
        Try
            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_PERptLeaveCard " + ddlStartYear.SelectedValue + ", '" + Result + "', '', '', " + RBType.SelectedIndex.ToString

            If RBType.SelectedIndex = 0 Then
                Session("ReportFile") = Server.MapPath("~\Rpt\RptLeaveCardEmp.frx")
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\RptLeaveCardDept.frx")
            End If

            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try

            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                FillCombo(ddlStartYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
                If ddlStartYear.Items.Contains(ddlStartYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                    ddlStartYear.SelectedValue = ViewState("GLYear").ToString.Trim
                End If
                ReportGrid.SetRpt("RptLeaveCard")
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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
        Dim dt As DataTable
        Dim SQLString As String
        Dim Result As String
        Try

            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            SQLString = "EXEC S_PERptLeaveCard " + ddlStartYear.SelectedValue + ", '" + Result + "', '', '', " + RBType.SelectedIndex.ToString

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptGLSubled")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class
