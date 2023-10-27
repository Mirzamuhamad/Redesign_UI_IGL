Imports System.Data
Imports System.IO
Partial Class Rpt_GraphPembelianBulanan_ReportTemplate
    Inherits System.Web.UI.Page
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
                FillCombo(ddlStartYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlEndYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlStartMonth, "SELECT [Period], CONVERT(VARCHAR(2),[Period]) + ' - ' + [Description] AS Description From GLPeriod WHERE [Period] NOT IN (0,13)", False, "Period", "Description", ViewState("DBConnection").ToString)
                FillCombo(ddlEndMonth, "SELECT [Period], CONVERT(VARCHAR(2),[Period]) + ' - ' + [Description] AS Description From GLPeriod WHERE [Period] NOT IN (0,13)", False, "Period", "Description", ViewState("DBConnection").ToString)
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("GraphPembelianBulanan")
                BindToDropList(ddlStartYear, ViewState("GLYear").ToString)
                BindToDropList(ddlEndYear, ViewState("GLYear").ToString)
                BindToDropList(ddlStartMonth, ViewState("GLPeriod").ToString)
                BindToDropList(ddlEndMonth, ViewState("GLPeriod").ToString)
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub



    Private Sub print()
        Dim Result, CekPeriod As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            CekPeriod = CekRangePeriodSelected(ddlStartYear.SelectedValue, ddlStartMonth.SelectedValue, ddlEndYear.SelectedValue, ddlEndMonth.SelectedValue)
            If CekPeriod <> "" Then
                lbStatus.Text = MessageDlg(CekPeriod)
                Exit Sub
            End If

            Result = ReportGrid.ResultString

            Session("SelectCommand") = "EXEC S_EXERptMNGPurchaseMonthly 'PEMBELIAN BULANAN','PEMBELIAN'," + ddlStartYear.SelectedValue + "," + ddlStartMonth.SelectedValue + "," + ddlEndYear.SelectedValue + "," + ddlEndMonth.SelectedValue + ",'" + Result + "', '', '', " + rgData.SelectedValue + ", " + rgReport.SelectedValue + ", " + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = Server.MapPath("~\Rpt\RptPrintGraph2.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
            'lbStatus.Text = Session("SelectCommand")
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
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
        Dim SQLString, result As String
        Try

            result = ReportGrid.ResultString

            Session("DBConnection") = ViewState("DBConnection")
            SQLString = "EXEC S_EXERptMNGPurchaseMonthly 'PEMBELIAN BULANAN','PEMBELIAN'," + ddlStartYear.SelectedValue + "," + ddlStartMonth.SelectedValue + "," + ddlEndYear.SelectedValue + "," + ddlEndMonth.SelectedValue + ",'" + result + "', '', '', " + rgData.SelectedValue + ", " + rgReport.SelectedValue + ", " + QuotedStr(ViewState("UserId").ToString)

            dt = SQLExecuteQuery(SQLString, Session("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("GraphPembelianBulanan")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class
