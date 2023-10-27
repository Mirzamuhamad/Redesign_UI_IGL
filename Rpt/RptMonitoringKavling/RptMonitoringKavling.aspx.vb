Imports System.Data
Imports System.Data.SqlClient
Imports System.IO


Partial Class Rpt_RptMonitoringKavling
    Inherits System.Web.UI.Page
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlsitePlan, "SELECT LevelCode, LevelName FROM MsLevelProperty ", True, "LevelCode", "LevelName", ViewState("DBConnection"))
                'ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                'ReportGrid.SetRpt("RptCostingReport")
                'cbEmp_CheckedChanged(Nothing, Nothing)
            End If
            ' ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProcess" Then
                    BindToText(tbProcess, Session("Result")(0).ToString)
                End If

                If Not ViewState("Sender") Is Nothing Then
                    ViewState("Sender") = Nothing
                End If
                If Not Session("Result") Is Nothing Then
                    Session("Result") = Nothing
                End If
            End If
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
        Dim result As String
        Dim Str As String
        Try
            'result = ReportGrid.ResultString

            If tbProcess.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Site Plan must have values")
                btnProcess.Focus()
                Exit Sub
            End If

            'If cbEmp.Checked Then
            '    If (tbEmpNo1.Text.Trim = "") Or (tbEmpNo2.Text.Trim = "") Then
            '        lbStatus.Text = MessageDlg("Employee No must have values")
            '        tbEmpNo1.Focus()
            '        Exit Sub
            '    End If
            '    Str = " AND A.EmpNumb >= " + QuotedStr(tbEmpNo1.Text) + " AND A.EmpNumb <= " + tbEmpNo2.Text + ""
            'Else : Str = ""
            'End If

            'modify this one
            ViewState("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_RptMonitoringKavling " + QuotedStr(tbProcess.Text) + ", " + QuotedStr(ViewState("UserId"))

            Session("ReportFile") = ".../../../Rpt/RptMonitoringKavling.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim result As String
        Dim Str As String
        Try
            'result = ReportGrid.ResultString

            If tbProcess.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Process Salary must have values")
                btnProcess.Focus()
                Exit Sub
            End If

            'If cbEmp.Checked Then
            '    If (tbEmpNo1.Text.Trim = "") Or (tbEmpNo2.Text.Trim = "") Then
            '        lbStatus.Text = MessageDlg("Employee No must have values")
            '        tbEmpNo1.Focus()
            '        Exit Sub
            '    End If
            '    Str = " AND A.EmpNumb >= " + QuotedStr(tbEmpNo1.Text) + " AND A.EmpNumb <= " + tbEmpNo2.Text + ""
            'Else : Str = ""
            'End If

            'modify this one
            ViewState("PrintType") = "Preview"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_RptMonitoringKavling " + QuotedStr(tbProcess.Text) + ", " + QuotedStr(ViewState("UserId"))

            Session("ReportFile") = ".../../../Rpt/RptMonitoringKavling.frx"
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
        Dim Str As String
        Try
            'Result = ReportGrid.ResultString

            If tbProcess.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Site Plan must have values")
                btnProcess.Focus()
                Exit Sub
            End If

            'If cbEmp.Checked Then
            '    If (tbEmpNo1.Text.Trim = "") Or (tbEmpNo2.Text.Trim = "") Then
            '        lbStatus.Text = MessageDlg("Employee No must have values")
            '        tbEmpNo1.Focus()
            '        Exit Sub
            '    End If
            '    Str = " AND A.EmpNumb >= " + QuotedStr(tbEmpNo1.Text) + " AND A.EmpNumb <= " + tbEmpNo2.Text + ""
            'Else : Str = ""
            'End If

            ViewState("PrintType") = "Preview"
            Session("DBConnection") = ViewState("DBConnection")
            SQLString = "EXEC S_RptMonitoringKavling " + QuotedStr(tbProcess.Text) + ", " + QuotedStr(ViewState("UserId"))

            'lbStatus.Text = SQLString
            'Exit Sub
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("RptMonitoringKavling")

        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ID, ParentID,StructureCode,StructureName,LevelCode FROM MsStructure WHERE LevelCode = " + QuotedStr(ddlsitePlan.SelectedValue)
            ResultField = "StructureCode"
            ViewState("Sender") = "btnProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProcess_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub cbEmp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEmp.CheckedChanged
    '    Try
    '        tbEmpNo1.Enabled = cbEmp.Checked
    '        tbEmpNo2.Enabled = cbEmp.Checked

    '        tbEmpNo1.Text = ""
    '        tbEmpNo2.Text = ""
    '    Catch ex As Exception
    '        lbStatus.Text = "cbEmp_CheckedChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlsitePlan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsitePlan.SelectedIndexChanged
        Try
            tbProcess.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddlsitePlan_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub



End Class
