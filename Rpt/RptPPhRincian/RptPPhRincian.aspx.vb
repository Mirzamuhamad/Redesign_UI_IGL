Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptPPhRincian_RptPPhRincian
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            ViewState("PrintType") = "Print"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Print()
        Dim Result, FgForceNewPage As String
        Try
            Result = ReportGrid.ResultString
            If cbForceNewPage.Visible = True Then
                If cbForceNewPage.Checked Then
                    FgForceNewPage = "Y"
                Else
                    FgForceNewPage = "N"
                End If
            Else
                FgForceNewPage = "N"
            End If
            If tbCode.Text = "" Then
                lbStatus.Text = MessageDlg("Process Salary must have value")
                btnSearch.Focus()
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_PYRptPPhRincian " + QuotedStr(tbCode.Text) + ",'" + Result + "','',''," + QuotedStr(FgForceNewPage) + ", 0," + QuotedStr(ViewState("UserId").ToString)
            ' lbStatus.Text = Session("SelectCommand")
            ' Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPPhRincian.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "Print Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            ViewState("PrintType") = "Preview"
            Print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
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
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try

            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptPPhRincian")
            End If

            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString

                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim Result As String
        Dim dt As DataTable
        Dim SQLString, FgForceNewPage As String
        Try
            Result = ReportGrid.ResultString
            If cbForceNewPage.Visible = True Then
                If cbForceNewPage.Checked Then
                    FgForceNewPage = "Y"
                Else
                    FgForceNewPage = "N"
                End If
            Else
                FgForceNewPage = "N"
            End If
            If tbCode.Text = "" Then
                lbStatus.Text = MessageDlg("Process Salary must have value")
                btnSearch.Focus()
                Exit Sub
            End If
            ViewState("PrintType") = "Preview"

            Session("SelectCommand") = "EXEC S_PYRptPPhRincian " + QuotedStr(tbCode.Text) + ",'" + Result + "','',''," + QuotedStr(FgForceNewPage) + ", 0," + QuotedStr(ViewState("UserId").ToString)

            dt = SQLExecuteQuery(Session("SelectCommand"), ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptPPhRincian")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_PYProcessHdPrint WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)
            ResultField = "Process_Salary"
            ViewState("Sender") = "btnSearch"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Search From Error : " + ex.ToString
        End Try
    End Sub
End Class
