Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Rpt_RptStockAvailable_RptStockAvailable
    Inherits System.Web.UI.Page
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            'ReportGrid.setConnection(Session("DBConnection"))
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptStockAvailable")
                'If Request.QueryString("ContainerId").ToString = "RptStockAvailableFGID" Then
                '    ReportGrid.SetRpt("RptStockAvailableFG")
                'Else
                '    ReportGrid.SetRpt("RptStockAvailableRM")
                'End If
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
        Dim ResultGrid, Resultcat, Fgforce, FgValue, Str3, FgReport As String
        Try
            ResultGrid = ReportGrid.ResultString
            Resultcat = "" 'ReportGrid.ResultCategory.ToString
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            If cbValue.Checked Then
                FgValue = "1"
            Else
                FgValue = "0"
            End If
            If cbQty.Checked Then
                Str3 = " AND QtyBalance < QtyMin "
            Else
                Str3 = ""
            End If
            FgReport = "''"
            Session("DBConnection") = ViewState("DBConnection")
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_STRptStockAvailable " + QuotedStr(tbTransDate.SelectedValue) + ", '" + Resultcat + _
            "','" + ResultGrid + "', '', " + FgValue + ", " + FgReport + ", " + QuotedStr(ViewState("UserId")) + "," + Fgforce
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptStockAvailable.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim ResultGrid, Resultcat, Fgforce, FgValue, Str3, FgReport As String
        Try
            ResultGrid = ReportGrid.ResultString
            'Resultcat = ReportGrid.ResultCategory.ToString
            Resultcat = ""
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            If cbValue.Checked Then
                FgValue = "1"
            Else
                FgValue = "0"
            End If
            If cbQty.Checked Then
                Str3 = " AND QtyBalance < QtyMin "
            Else
                Str3 = ""
            End If
            FgReport = "''"
            Session("DBConnection") = ViewState("DBConnection")
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            Session("PrintType") = "Preview"
            Session("SelectCommand") = "EXEC S_STRptStockAvailable " + QuotedStr(tbTransDate.SelectedValue) + ", '" + Resultcat + _
            "','" + ResultGrid + "', '" + Str3 + "', " + FgValue + ", " + FgReport + ", " + QuotedStr(ViewState("UserId")) + "," + Fgforce
            Session("ReportFile") = ".../../../Rpt/RptStockAvailable.frx"
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
        Dim ResultGrid, Resultcat, Fgforce, FgValue, Str3, FgReport As String
        Dim Result As String
        Dim dt As DataTable
        Dim SQLString As String

        Try
            ResultGrid = ReportGrid.ResultString
            'Resultcat = ReportGrid.ResultCategory.ToString
            Resultcat = ""
            If cbForce.Checked Then
                Fgforce = "1"
            Else
                Fgforce = "0"
            End If
            If cbValue.Checked Then
                FgValue = "1"
            Else
                FgValue = "0"
            End If
            If cbQty.Checked Then
                Str3 = " AND QtyBalance < QtyMin "
            Else
                Str3 = ""
            End If
            FgReport = "''"
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Preview"
            SQLString = "EXEC S_STRptStockAvailable " + QuotedStr(tbTransDate.SelectedValue) + ", '" + ResultCat + _
            "','" + ResultGrid + "', '', " + FgValue + ", " + FgReport + ", " + QuotedStr(ViewState("UserId")) + "," + FgForce
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("Stock Available")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class
