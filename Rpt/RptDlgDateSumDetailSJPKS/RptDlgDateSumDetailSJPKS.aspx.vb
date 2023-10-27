Imports System.Data
Imports System.IO
Imports System.Data.SqlClient

Partial Class Rpt_RptDlgDateRekapSJPKS_ReportTemplate
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

                btnPrint.Visible = (CStr(ViewState("FastRpt")).ToString <> "")
                If TrimStr(dt.Rows(0)("FgPrintValue").ToString) = "0" Or TrimStr(dt.Rows(0)("FgPrintValue").ToString) = "2" Or TrimStr(dt.Rows(0)("FgPrintValue").ToString) = "3" Or TrimStr(dt.Rows(0)("FgPrintValue").ToString) = "4" Or TrimStr(dt.Rows(0)("FgPrintValue").ToString) = "5" Then
                    ViewState("xPrint") = CInt(dt.Rows(0)("FgPrintValue").ToString.Trim)
                Else
                    ViewState("xPrint") = 1
                End If
                RBType.Visible = (RptType.Count >= 3)
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
                cbPrintValue.Visible = dt.Rows(0)("FgPrintValue").ToString = "Y"
                cbForceNewPage.Visible = dt.Rows(0)("FgForceNewPage").ToString = "Y"
                lgRpt.Visible = RBType.Visible
                fsRpt.Visible = RBType.Visible
                lgRpt2.Visible = RBType2.Visible
                fsRpt2.Visible = RBType2.Visible
                tbStartDate.SelectedDate = ViewState("ServerDate")
                tbEndDate.SelectedDate = ViewState("ServerDate")
            End If


            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustomer, Session("Result")(0).ToString)
                    BindToText(tbCustonerName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnContract" Then
                    BindToText(tbNoContract, Session("Result")(2).ToString)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
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
        'ViewState("GLYear") = Session(Request.QueryString("KeyId"))("Year")
        'ViewState("GLPeriod") = Session(Request.QueryString("KeyId"))("Period")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
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
        Dim Result, FgPrintValue, FgForceNewPage, FgCust, FgKontrak As String
        Dim FgReport, FgReport2, RptCount As Integer
        Dim FastRpt() As String
        Try
            If tbStartDate.SelectedValue > tbEndDate.SelectedValue Then
                lbStatus.Text = MessageDlg("Start Date must be lower than EndDate")
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

            If cbCuss.Checked Then

                FgCust = "Y"
            Else

                FgCust = "N"
            End If

            If cbKontrak.Checked Then

                FgKontrak = "Y"
            Else

                FgKontrak = "N"
            End If

            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", " + QuotedStr(FgPrintValue) + "," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            FastRpt = ViewState("FastRpt").ToString.Split("|")
            RptCount = FastRpt.Count()
            If FgReport >= RptCount Then
                Session("ReportFile") = Server.MapPath("~\Rpt\" + FastRpt(RptCount - 1))
            Else
                Session("ReportFile") = Server.MapPath("~\Rpt\" + FastRpt(FgReport))
            End If

            If ViewState("xPrint") = 5 Then
                Session("SelectCommand2") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '2'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand3") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '3'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand4") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '4'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand5") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '5'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                AttachScriptAJAX("openreport5();", Page, Me.GetType)
            ElseIf ViewState("xPrint") = 4 Then
                Session("SelectCommand2") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '2'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand3") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '3'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand4") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '4'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                AttachScriptAJAX("openreport4();", Page, Me.GetType)
            ElseIf ViewState("xPrint") = 3 Then
                Session("SelectCommand2") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '2'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("SelectCommand3") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '3'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                AttachScriptAJAX("openreport3();", Page, Me.GetType)
            ElseIf ViewState("xPrint") = 2 Then
                Session("SelectCommand2") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '2'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                AttachScriptAJAX("openreport2();", Page, Me.GetType)

            ElseIf ViewState("xPrint") = 0 Then
                Dim Form1, Form2 As String
                Form1 = "1"
                Form2 = "2"
                Session("SelectCommand2") = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '1'," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
                'Session("SelectCommand2") = "EXEC S_PLRptRekapTonase2 " + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', '" + Result + "', '', '', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", '2', " + QuotedStr(ViewState("UserId").ToString)
                'Session("ReportFile") = ".../../../Rpt/RptRekapSJPKSCPO.frx"
                AttachScriptAJAX("openreport1();", Page, Me.GetType)
            Else
                AttachScriptAJAX("openreport();", Page, Me.GetType)
            End If
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
        Dim FastRpt() As String
        Dim Result, FgPrintValue, FgForceNewPage, FgCust, FgKontrak As String
        Dim RptCount, FgReport, FgReport2 As Integer
        Try
            Result = ReportGrid.ResultString
            If tbStartDate.SelectedValue > tbEndDate.SelectedValue Then
                lbStatus.Text = "Start Date must be lower than EndDate"
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



            If cbCuss.Checked Then
                
                FgCust = "Y"
            Else
                
                FgCust = "N"
            End If

            If cbKontrak.Checked Then

                FgKontrak = "Y"
            Else

                FgKontrak = "N"
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
            'SQLString = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', '" + Result + "', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", " + QuotedStr(FgPrintValue) + ", " + QuotedStr(ViewState("UserId").ToString)
            SQLString = ViewState("ExeSP") + " '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "','" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "'," + QuotedStr(tbCustomer.Text) + "," + QuotedStr(tbNoContract.Text) + ", '" + Result + "','','', " + QuotedStr(ViewState("MenuParam").ToString) + ", " + FgReport.ToString + ", " + FgReport2.ToString + ", " + QuotedStr(FgForceNewPage) + ", " + QuotedStr(FgPrintValue) + "," + QuotedStr(FgCust) + "," + QuotedStr(FgKontrak) + ", " + QuotedStr(ViewState("UserId").ToString)
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

    Protected Sub btnCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomer.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = " SELECT TransNmbr,Status,Customer,CustomerName FROM V_PLSJPKsHd WHERE Status = 'P'  "
            CriteriaField = "Customer,CustomerName "
            ResultField = "Customer,CustomerName"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnContract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContract.Click
        Dim ResultField, CriteriaField As String
        Try
            
            Session("filter") = "SELECT A.TransNmbr,A.Status,B.ContractNo FROM V_PLSJPksHd A INNER JOIN PLSJPksDt B ON A.TransNmbr = B.TransNmbr WHERE Status='P'"
            CriteriaField = "TransNmbr,Status,ContractNo"
            ResultField = "TransNmbr,Status,ContractNo"
            ViewState("Sender") = "btnContract"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbCustomer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustomer.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustomer.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustomer.Text = Dr("Customer_Code")
                tbCustonerName.Text = Dr("Customer_Name")
            Else
                tbCustomer.Text = ""
                tbCustonerName.Text = ""

            End If
            tbCustomer.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbCuss_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCuss.CheckedChanged
        Try
            Dim FgCust As String

            If cbCuss.Checked Then
                tbCustomer.Visible = True
                btnCustomer.Visible = True
                tbCustonerName.Visible = True

            Else
                tbCustomer.Visible = False
                tbCustonerName.Visible = False
                btnCustomer.Visible = False
                tbCustomer.Text = ""
                tbCustonerName.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("Check Box Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbKontrak_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbKontrak.CheckedChanged
        Try
            Dim FgKontrak As String

            If cbKontrak.Checked Then
                tbNoContract.Visible = True
                btnContract.Visible = True
            Else
                tbNoContract.Visible = False
                btnContract.Visible = False
                tbNoContract.Text = ""

            End If

        Catch ex As Exception
            Throw New Exception("Check Box Error : " + ex.ToString)
        End Try
    End Sub

End Class
