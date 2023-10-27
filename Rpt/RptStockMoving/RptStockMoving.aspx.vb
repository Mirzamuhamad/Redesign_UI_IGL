Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Rpt_RptStockMoving_RptStockMoving
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            'ReportGrid.setConnection(Session("DBConnection"))
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptStockMoving")
                tbTransDate.SelectedDate = ViewState("ServerDate") 'Now.Date
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
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
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

    Private Sub print()
        Dim result, cat, Range1, Range2, Range3, Range4, Range5, Range6, Range7, cbcek As String
        Try
            result = ReportGrid.ResultString
            'cat = ReportGrid.ResultCategory.ToString
            cat = ""

            If cbValue.Checked = True Then
                cbcek = "1"
            Else
                cbcek = "0"
            End If
            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4

            Range5 = RangeControl1.getRange5
            Range6 = RangeControl1.getRange6
            Range7 = RangeControl1.getRange7

            Session("DBConnection") = ViewState("DBConnection")
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            Session("SelectCommand") = "EXEC S_STRptStockMoving '" + Format(tbTransDate.SelectedValue, "yyyy-MM-dd") + "', " + Range1 + _
            "," + Range2 + "," + Range3 + "," + Range4 + "," + Range5 + "," + Range6 + ",'" + result + "','','', " + cbcek + ", " + QuotedStr(ViewState("UserId"))

            Session("ReportFile") = ".../../../Rpt/RptStockMoving.frx"
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
        Dim Result As String
        Dim dt As DataTable
        Dim SQLString As String
        Dim cat, Range1, Range2, Range3, Range4, cbcek, Range5, Range6, Range7 As String
        Try
            Result = ReportGrid.ResultString
            'cat = ReportGrid.ResultCategory.ToString
            cat = " "

            If cbValue.Checked = True Then
                cbcek = "1"
            Else
                cbcek = "0"
            End If
            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4

            Range5 = RangeControl1.getRange5
            Range6 = RangeControl1.getRange6
            Range7 = RangeControl1.getRange7

            Session("DBConnection") = ViewState("DBConnection")
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))

            Session("PrintType") = "Preview"

            SQLString = "EXEC S_STRptStockMoving '" + Format(tbTransDate.SelectedValue, "yyyy-MM-dd") + "', " + Range1 + _
            "," + Range2 + "," + Range3 + "," + Range4 + "," + Range5 + "," + Range6 + ",'" + Result + "','','', " + cbcek + ", " + QuotedStr(ViewState("UserId"))

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("Stock Aging")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class
