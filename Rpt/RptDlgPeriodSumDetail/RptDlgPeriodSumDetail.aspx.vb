'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.IO
Imports System.Data.SqlClient

Partial Class Rpt_RptDlgPeriodSumDetail_ReportTemplate
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                Dim dt As DataTable
                Dim RptType(), RptType2() As String
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                dt = SQLExecuteQuery("Select ExeSP, RptTitle, MenuDll, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                ReportGrid.SetRpt(dt.Rows(0)("MenuDll").ToString)
                lblTitle.Text = dt.Rows(0)("RptTitle").ToString
                ViewState("FastRpt") = dt.Rows(0)("FastRpt").ToString
                ViewState("MenuParam") = dt.Rows(0)("MenuParam").ToString
                ViewState("ExeSP") = "EXEC " + dt.Rows(0)("ExeSP").ToString
                RptType = dt.Rows(0)("ReportType").ToString.Split("|")
                RBType.Visible = (RptType.Count >= 3)
                btnPrint.Visible = (CStr(ViewState("FastRpt")).ToString <> "")
                If RptType.Count >= 3 Then
                    Dim LI As ListItem
                    RBType.Items.Clear()
                    lbReportType.Text = RptType(0) + " : "
                    For J = 1 To RptType.Count - 1
                        LI = New ListItem(RptType(J).ToString, CStr(J - 1))
                        RBType.Items.Add(LI)
                    Next
                    RBType.SelectedIndex = 0
                End If

                RptType2 = dt.Rows(0)("ReportType2").ToString.Split("|")
                RBType2.Visible = (RptType2.Count >= 3)
                If RptType2.Count >= 3 Then
                    Dim LI2 As ListItem
                    RBType2.Items.Clear()
                    lbReportType2.Text = RptType2(0) + " : "
                    For J = 1 To RptType2.Count - 1
                        LI2 = New ListItem(RptType2(J).ToString, CStr(J - 1))
                        RBType2.Items.Add(LI2)
                    Next
                    RBType2.SelectedIndex = 0
                End If
                Me.tbPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
                cbPrintValue.Visible = dt.Rows(0)("FgPrintValue").ToString = "Y"
                cbForceNewPage.Visible = dt.Rows(0)("FgForceNewPage").ToString = "Y"
                lgRpt.Visible = RBType.Visible
                fsRpt.Visible = RBType.Visible
                lgRpt2.Visible = RBType2.Visible
                fsRpt2.Visible = RBType2.Visible
                FillCombo(ddlStartYear, "Select Year FROM GLYear", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlEndYear, "Select Year FROM GLYear", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlStartMonth, "Select Period, Description FROM GLPeriod WHERE Period BETWEEN 1 and 12", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlEndMonth, "Select Period, Description FROM GLPeriod WHERE Period BETWEEN 1 and 12", False, "Period", "Description", ViewState("DBConnection"))
                ddlStartYear.SelectedValue = ViewState("GLYear")
                ddlEndYear.SelectedValue = ViewState("GLYear")
                ddlStartMonth.SelectedValue = ViewState("GLPeriod")
                ddlEndMonth.SelectedValue = ViewState("GLPeriod")

                'ddlPeriod.Attributes.Add("onchange", "return period2();")
                'ddlStartYear.Attributes.Add("onchange", "return period2();")
                'ddlStartMonth.Attributes.Add("onchange", "return period2();")
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
        'ViewState("FgAdmin") = Session(Request.QueryString("KeyId"))("FgAdmin")
        'ViewState("Currency") = Session(Request.QueryString("KeyId"))("Currency")
        ViewState("GLYear") = Session(Request.QueryString("KeyId"))("Year")
        ViewState("GLPeriod") = Session(Request.QueryString("KeyId"))("Period")
        'ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try

    End Sub

    Private Sub print()
        Dim cekPeriod, Result, FgPrintValue, FgForceNewPage As String
        Dim FgReport, FgReport2, RptCount As Integer
        Dim FastRpt() As String
        Try
            cekPeriod = CekRangePeriodSelected(ddlStartYear.SelectedValue, ddlStartMonth.SelectedValue, ddlEndYear.SelectedValue, ddlEndMonth.SelectedValue)
            If cekPeriod <> "" Then
                lbStatus.Text = MessageDlg(cekPeriod)
                Exit Sub
            End If
            If RBType.Visible = True Then
                FgReport = RBType.SelectedValue
            Else
                FgReport = 0
            End If
            If RBType2.Visible = True Then
                FgReport2 = RBType2.SelectedValue
            Else
                FgReport2 = 0
            End If
            If cbPrintValue.Visible = True Then
                If cbPrintValue.Checked Then
                    FgPrintValue = "Y"
                Else
                    FgPrintValue = "N"
                End If
            Else
                FgPrintValue = "N"
            End If
            If cbForceNewPage.Visible = True Then
                If cbForceNewPage.Checked Then
                    FgForceNewPage = "Y"
                Else
                    FgForceNewPage = "N"
                End If
            Else
                FgForceNewPage = "N"
            End If
            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = ViewState("ExeSP") + " " + ddlStartYear.SelectedValue + ", " + ddlStartMonth.SelectedValue + "," + ddlPeriod.SelectedValue.ToString + ", " + tbPeriod.Text.Replace(",", "") + "," + ddlEndYear.SelectedValue + "," + ddlEndMonth.SelectedValue + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", " + QuotedStr(FgPrintValue) + ", " + QuotedStr(ViewState("UserId").ToString)
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            FastRpt = ViewState("FastRpt").ToString.Split("|")
            RptCount = FastRpt.Count()
            If FgReport >= RptCount Then
                Session("ReportFile") = Server.MapPath("~\Rpt\" + FastRpt(RptCount - 1))
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\" + FastRpt(FgReport))
            End If
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "MyFun1", "openprintdlg();", True)
            AttachScriptAJAX("openprintdlg();", Page, Me.GetType)
            'AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
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

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim SQLString As String
        Dim CekPeriod, Result, FgPrintValue, FgForceNewPage As String
        Dim FgReport, FgReport2, RptCount As Integer
        Dim FastRpt() As String
        Try
            CekPeriod = CekRangePeriodSelected(ddlStartYear.SelectedValue, ddlStartMonth.SelectedValue, ddlEndYear.SelectedValue, ddlEndMonth.SelectedValue)
            If CekPeriod <> "" Then
                lbStatus.Text = MessageDlg(CekPeriod)
                Exit Sub
            End If
            Result = ReportGrid.ResultString
            If RBType.Visible = True Then
                FgReport = RBType.SelectedValue
            Else
                FgReport = 0
            End If
            If RBType2.Visible = True Then
                FgReport2 = RBType2.SelectedValue
            Else
                FgReport2 = 0
            End If
            If cbPrintValue.Visible = True Then
                If cbPrintValue.Checked Then
                    FgPrintValue = "Y"
                Else
                    FgPrintValue = "N"
                End If
            Else
                FgPrintValue = "N"
            End If
            'If cbForceNewPage.Visible = True Then
            '    If cbForceNewPage.Checked Then
            '        FgForceNewPage = "Y"
            '    Else
            '        FgForceNewPage = "N"
            '    End If
            'Else
            FgForceNewPage = "X"
            'End If
            SQLString = ViewState("ExeSP") + " " + ddlStartYear.SelectedValue + ", " + ddlStartMonth.SelectedValue + "," + ddlPeriod.SelectedValue.ToString + ", " + tbPeriod.Text.Replace(",", "") + "," + ddlEndYear.SelectedValue + "," + ddlEndMonth.SelectedValue + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", " + QuotedStr(FgPrintValue) + ", " + QuotedStr(ViewState("UserId").ToString)
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            FastRpt = ViewState("FastRpt").ToString.Split("|")
            RptCount = FastRpt.Count()
            If FgReport >= RptCount Then
                ExportGridToExcel(FastRpt(RptCount - 1))
            Else
                ExportGridToExcel(FastRpt(FgReport))
            End If
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPeriod.TextChanged, ddlPeriod.SelectedIndexChanged, ddlStartYear.SelectedIndexChanged, ddlStartMonth.SelectedIndexChanged
        Dim range, tahun, bulan, tahun2, bulan2, xTahun, xbulan As Integer
        Try
            If (CInt(tbPeriod.Text) = 0) Then
                tbPeriod.Text = "1"
            End If
            range = (CInt(ddlPeriod.SelectedValue) * CInt(tbPeriod.Text)) - 1
            tahun = CInt(ddlStartYear.SelectedValue)
            bulan = CInt(ddlStartMonth.SelectedValue)
            bulan2 = bulan + range

            xTahun = range \ 12
            xbulan = range Mod 12

            tahun2 = tahun + xTahun
            bulan2 = bulan + xbulan
            If bulan2 > 12 Then
                tahun2 = tahun2 + 1
                bulan2 = bulan2 - 12
            End If

            'lbStatus.Text = tahun2.ToString + " - " + bulan2.ToString
            'Exit Sub
            'If bulan2 > 24 Then
            '    bulan2 = bulan2 - 24
            '    tahun2 = tahun + 2
            'ElseIf bulan2 > 12 Then
            '    bulan2 = bulan2 - 12
            '    tahun2 = tahun + 1
            'Else
            '    tahun2 = tahun
            'End If
            ddlEndYear.SelectedValue = tahun2.ToString
            ddlEndMonth.SelectedValue = bulan2.ToString
        Catch ex As Exception
            lbStatus.Text = "tbPeriod_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
