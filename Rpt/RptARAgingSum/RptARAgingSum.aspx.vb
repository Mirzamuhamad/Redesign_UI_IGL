Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Rpt_RptARAgingSum_RptARAgingSum
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            'ReportGrid.setConnection(Session("DBConnection"))
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptARAgingSum")
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
        Dim result, Range1, Range2, Range3, Range4 As String
        
        Try
            result = ReportGrid.ResultString
            'cat = ReportGrid.ResultCategory


            'If RBFwd.SelectedIndex.ToString = "0" Then
            '    FgFwd = "1"
            'Else : FgFwd = "-1"
            'End If

            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4

            'modify this one
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FNRptARAgingSum '" + tbTransDate.SelectedValue + "', '" + _
            result + "', '', ''," + _
            Range1 + "," + Range2 + "," + Range3 + "," + Range4 + ", " + _
            "0 , 1, " + QuotedStr(ViewState("UserId"))
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub


            Session("ReportFile") = ".../../../Rpt/RptARAgingSum.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim result, Range1, Range2, Range3, Range4 As String
        Try
            result = ReportGrid.ResultString
            'cat = ReportGrid.ResultCategory



            'If RBFwd.SelectedIndex.ToString = "0" Then
            '    FgFwd = "1"
            'Else : FgFwd = "-1"
            'End If

            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4

            'modify this one
            Session("PrintType") = "Preview"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FNRptARAgingSum '" + tbTransDate.SelectedValue + "', '" + _
            result + "', '', ''," + _
            Range1 + "," + Range2 + "," + Range3 + "," + Range4 + ", " + _
            "0 , 1, " + QuotedStr(ViewState("UserId"))
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptARAgingSum.frx"
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
        Dim result, Range1, Range2, Range3, Range4 As String
        Try
            result = ReportGrid.ResultString
            'cat = ReportGrid.ResultCategory



            'If RBFwd.SelectedIndex.ToString = "0" Then
            '    FgFwd = "1"
            'Else : FgFwd = "-1"
            'End If

            Range1 = RangeControl1.getRange1
            Range2 = RangeControl1.getRange2
            Range3 = RangeControl1.getRange3
            Range4 = RangeControl1.getRange4



            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            SQLString = "EXEC S_FNRptARAgingSum '" + tbTransDate.SelectedValue + "', '" + _
            result + "', '', ''," + _
            Range1 + "," + Range2 + "," + Range3 + "," + Range4 + ", " + _
            "0 , 1, " + QuotedStr(ViewState("UserId"))

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("FNARAgingSum")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class