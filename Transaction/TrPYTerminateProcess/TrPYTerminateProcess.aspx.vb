Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPYTerminateProcess_TrPYTerminateProcess
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct TransNmbr, Nmbr, Status, TransDate, Department, Dept_Name, Remark FROM V_PYTerminateProcessHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProcess" Then
                    'Process_Code, Status, Effective, Employee_ID, Employee_Name, Hire_Date, MasaKerja, PHKReason, FgSalaryPartial, Total_Process, Total_Paid"
                    ddlYear.SelectedValue = CDate(Session("Result")(2).ToString).Year.ToString
                    ddlMonth.SelectedValue = CDate(Session("Result")(2).ToString).Month.ToString
                    tbStatus.Text = Session("Result")(1).ToString
                    tbTerminateNo.Text = Session("Result")(0).ToString
                    tbEmpNo.Text = Session("Result")(3).ToString
                    tbEmpName.Text = Session("Result")(4).ToString
                    tbEndDate.Text = Session("Result")(2).ToString
                    tbHireDate.Text = Session("Result")(5).ToString
                    tbMasaKerja.Text = Session("Result")(6).ToString
                    tbPHKReason.Text = Session("Result")(7).ToString
                    tbTotalProses.Text = Session("Result")(9).ToString
                    tbTotalPaid.Text = Session("Result")(10).ToString

                    btnCompute.Visible = tbStatus.Text = "H" Or tbStatus.Text = "G"
                    btnComplete.Visible = tbStatus.Text = "G"
                    btnUnComplete.Visible = tbStatus.Text = "P"
                    
                    pnlView.Visible = False
                    pnlData.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    BindDataResult()
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
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
        ViewState("GLPeriodName") = Session(Request.QueryString("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Request.QueryString("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Request.QueryString("KeyId"))("Address1")
        ViewState("Address2") = Session(Request.QueryString("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Request.QueryString("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Request.QueryString("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Request.QueryString("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Private Sub SetInit()
        Try
            FillCombo(ddlYear, "EXEC S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlMonth, "EXEC S_GetPeriod", True, "Period", "Description", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            btnCompute.Visible = False
            btnComplete.Visible = False
            btnUnComplete.Visible = False
            ddlYear.SelectedValue = ViewState("GLYear")
            ddlMonth.SelectedValue = ViewState("GLPeriod")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataResult(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DtTemp As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            System.Threading.Thread.Sleep(3000)
            StrFilter = ""
            DT = SQLExecuteQuery("EXEC S_PYTerminateViewResult " + QuotedStr(tbTerminateNo.Text) + ", " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count = 0 Then
                DtTemp = DT.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridView1.DataSource = DtTemp
            Else
                DV = DT.DefaultView
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "Code DESC"
                End If
                DV.Sort = ViewState("SortExpression")
                GridView1.DataSource = DV
            End If
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BindDataLoan()
        Dim Dt, DtTemp As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PYProcessViewLoan " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                DtTemp = Dt.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridLoan.DataSource = DtTemp
            Else
                DV = Dt.DefaultView
                If ViewState("SortExpressionLoan") = Nothing Then
                    ViewState("SortExpressionLoan") = "LoanNo DESC"
                End If
                DV.Sort = ViewState("SortExpressionLoan")
                GridLoan.DataSource = DV
            End If
            GridLoan.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataLoan Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindDataResult()
    End Sub

    Protected Sub GridLoan_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridLoan.PageIndexChanging
        Try
            GridLoan.PageIndex = e.NewPageIndex
            BindDataLoan()
        Catch ex As Exception
            lbStatus.Text = "GridView1_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        Try
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridView1.EditIndex = -1
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "GridView1_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridLoan_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridLoan.RowCancelingEdit
        Try
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridLoan.EditIndex = -1
            BindDataLoan()
        Catch ex As Exception
            lbStatus.Text = "GridLoan_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridLoan_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridLoan.Sorting
        Try
            If ViewState("SortOrderLoan") = Nothing Or ViewState("SortOrderLoan") = "DESC" Then
                ViewState("SortOrderLoan") = "ASC"
            Else
                ViewState("SortOrderLoan") = "DESC"
            End If
            ViewState("SortExpressionLoan") = e.SortExpression + " " + ViewState("SortOrderLoan")
            BindDataLoan()
        Catch ex As Exception
            lbStatus.Text = "Grid Loan Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If index = 1 Then
                BindDataLoan()
            Else
                BindDataResult()
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ComputeTerminate(ByVal Langkah As Integer)
        'Dim sqlstring As String
        Dim StrMsg As String
        'Dim DS As DataSet
        Try
            'lbStatus.Text = MessageDlg("test")
            'Exit Sub
            'WaitOpen.Show("Computing Stock")
            'mpewait.Show()
            'Timer1.Enabled = True
            If Langkah = 0 Then
                'sqlstring = "EXEC S_PYProcessComputeCek01Abs " + QuotedStr(tbTerminateNo.Text)
                'DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))
                'Langkah = 1
                'ViewState("Langkah") = Langkah
                'If DS.Tables(0).Rows.Count > 0 Then
                '    pnlData.Visible = False
                '    pnlView.Visible = True
                '    btnYes.Visible = True
                '    btnNo.Visible = True
                '    btnReturn.Visible = False
                '    'mpewait.Hide()
                '    lbMessage.Text = "Compute failed..., please make sure this employee have absence"
                '    GridForView.DataSource = DS.Tables(0)
                '    GridForView.DataBind()
                '    Exit Sub
                'End If
                Langkah = 10
                ViewState("Langkah") = Langkah
            End If
            
            If Langkah = 10 Then
                StrMsg = doComputeTerminate()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    lbStatus.Text = StrMsg
                    Exit Sub
                End If
                'tampilan data
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                BindDataResult()
                'mpewait.Hide()
                tbStatus.Text = "G"
                btnCompute.Visible = tbStatus.Text = "H" Or tbStatus.Text = "G"
                btnComplete.Visible = tbStatus.Text = "G"
                btnUnComplete.Visible = tbStatus.Text = "P"
                lbStatus.Text = MessageDlg("Success Compute Salary Process")
            End If
        Catch ex As Exception
            lbStatus.Text = "Compute Salary Process Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCompute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCompute.Click
        Try
            'sp cek
            If ddlYear.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Year Salary must be selected")
                Exit Sub
            End If
            If ddlMonth.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Month Salary must be selected")
                Exit Sub
            End If
            If tbTerminateNo.Text = "" Then
                lbStatus.Text = MessageDlg("Terminate No must be selected")
                Exit Sub
            End If
            If tbEmpNo.Text = "" Then
                lbStatus.Text = MessageDlg("Employee ID must be selected")
                Exit Sub
            End If
            If Not (tbStatus.Text = "H" Or tbStatus.Text = "G") Then
                lbStatus.Text = MessageDlg("Compute Failed... Status Process Terminate must be Hold or Get Approval")
                Exit Sub
            End If
            ViewState("btnclick") = "Compute"
            ViewState("Langkah") = 0
            ComputeTerminate(ViewState("Langkah"))
        Catch ex As Exception
            lbStatus.Text = "btn Compute Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek02()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessComputeCek02Abs " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Compute failed..., please posting this data absence first."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek01 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek03()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek03Ovt " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK LEMBUR YANG BELOM COMPLETE")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek03Ovt " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Compute failed..., please posting this data employee overtime first."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek03 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek04()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek04Ben " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA BENEFIT YANG BELUM DIPOSTING")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek04Ben " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Compute failed..., please posting this data employee benefit first."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek04 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek05()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek05Ded " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA DEDUCTION YANG BELUM DIPOSTING")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek05Ded " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Compute failed..., please posting this data employee deduction first."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek05 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek06()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek06Loan " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA LEMBUR YANG BLOM COMPLETE")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek06Loan " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Compute failed..., Please cek this Data Employee Loan"
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek06 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ComputeCek07()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            sqlstring = "EXEC S_PYProcessComputeCek07PPh " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))
            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Make sure all transactions above is posted"
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek07 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ComputeCek08()
        Dim sqlstring As String
        Dim DS As DataSet
        'Dim dt As DataTable
        Try
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek08Jmsk " + QuotedStr(tbTerminateNo.Text), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("Cek Jamsostek")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek08Jmsk " + QuotedStr(tbTerminateNo.Text)

            'sqlstring = "SElect * From VMsCustomer"
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Make sure all transactions above is posted"
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "Cek08 Error : " + ex.ToString
        End Try
    End Sub

    Function doComputeTerminate() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYTerminateGetCalculation " + QuotedStr(tbTerminateNo.Text) + ", " + QuotedStr(tbEmpNo.Text) + ", '" + Format(CDate(tbEndDate.Text), "yyyy-MM-dd") + "', " + QuotedStr(tbPHKReason.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If

            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            Return ""
        Catch ex As Exception
            Throw New Exception("do Compute Terminate Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Try
            If ddlYear.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Year Salary must be selected")
                Exit Sub
            End If
            If ddlMonth.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Month Salary must be selected")
                Exit Sub
            End If
            If tbTerminateNo.Text = "" Then
                lbStatus.Text = MessageDlg("Terminate No must be selected")
                Exit Sub
            End If
            If tbEmpNo.Text = "" Then
                lbStatus.Text = MessageDlg("Employee ID must be selected")
                Exit Sub
            End If

            If Not (tbStatus.Text = "G") Then
                lbStatus.Text = MessageDlg("Posting Failed... Status Process Terminate must be Get Approval")
                Exit Sub
            End If
            ViewState("btnclick") = "Complete"
            ViewState("Langkah") = 0
            CompleteTerminate(ViewState("Langkah"))
        Catch ex As Exception
            lbStatus.Text = "btnComplete_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek02()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek02Loan " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Complete failed..., please posting this data Employee Loan."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "CompleteCek02 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek03()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek03Ovt " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Complete failed..., please posting this data Employee Overtime."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "CompleteCek03 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek04()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek04Ben " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Complete failed..., please posting this data Employee Benefit."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "CompleteCek04 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek05()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek05Ded " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Complete failed..., please posting this data Employee Deduction."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "CompleteCek05 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek06()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek06Medical " + QuotedStr(tbTerminateNo.Text)
            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                pnlData.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                'mpewait.Hide()
                lbMessage.Text = "Complete failed..., please posting this data Employee Medical Claim."
                GridForView.PageIndex = 0
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                Exit Sub
            End If
        Catch ex As Exception
            lbStatus.Text = "CompleteCek06 Error : " + ex.ToString
        End Try
    End Sub

    Function doCompleteProcess() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYTerminatePost " + QuotedStr(tbTerminateNo.Text) + ", " + QuotedStr(tbEmpNo.Text) + ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"
            'lbStatus.Text = ViewState("DBConnection")
            'Exit Function
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If
            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            Return ""
        Catch ex As Exception
            Throw New Exception("do Complete Salary Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub CompleteTerminate(ByVal Langkah As Integer)
        'Dim sqlstring As String
        Dim StrMsg As String
        'Dim DS As DataSet
        Try
            If Langkah = 0 Then
                'sqlstring = "EXEC S_PYProcessFinishCek01Adjust " + QuotedStr(tbTerminateNo.Text)
                'DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))
                Langkah = 10
                ViewState("Langkah") = Langkah
                'If DS.Tables(0).Rows.Count > 0 Then
                'pnlData.Visible = False
                'pnlView.Visible = True
                'btnYes.Visible = False
                'btnNo.Visible = False
                'btnReturn.Visible = True
                'mpewait.Hide()
                'lbMessage.Text = "Complete failed..., please make sure this Salary Adjustment Posted"
                'GridForView.DataSource = DS.Tables(0)
                'GridForView.DataBind()
                'Exit Sub
                'End If
            End If
            If Langkah = 10 Then
                StrMsg = doCompleteProcess()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    lbStatus.Text = StrMsg
                    Exit Sub
                End If
                'tampilan data
                BindDataResult()
                BindDataLoan()
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                'mpewait.Hide()
                tbStatus.Text = "P"
                btnCompute.Visible = tbStatus.Text = "H" Or tbStatus.Text = "G"
                btnComplete.Visible = tbStatus.Text = "G"
                btnUnComplete.Visible = tbStatus.Text = "P"
                lbStatus.Text = MessageDlg("Success Complete Terminate Process")
            End If
        Catch ex As Exception
            lbStatus.Text = "Complete Salary Process Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            If ddlYear.SelectedValue = "" Or ddlMonth.SelectedValue = "" Then
                Session("filter") = "SELECT Process_Code, Status, dbo.FormatDate(Effective) AS Effective, Employee_ID, Employee_Name, dbo.FormatDate(HireDate) AS Hire_Date, MasaKerja, PHKReason, FgSalaryPartial, dbo.FormatFloat(TotalProcess,dbo.DigitCurrHome()) AS Total_Process, dbo.FormatFloat(TotalPaid,dbo.DigitCurrHome()) As Total_Paid FROM V_PYTerminateHd "
            Else
                Session("filter") = "SELECT Process_Code, Status, dbo.FormatDate(Effective) AS Effective, Employee_ID, Employee_Name, dbo.FormatDate(HireDate) AS Hire_Date, MasaKerja, PHKReason, FgSalaryPartial, dbo.FormatFloat(TotalProcess,dbo.DigitCurrHome()) AS Total_Process, dbo.FormatFloat(TotalPaid,dbo.DigitCurrHome()) As Total_Paid FROM V_PYTerminateHd WHERE Year(Effective) = " + QuotedStr(ddlYear.SelectedValue) + " AND Month(Effective) = " + QuotedStr(ddlMonth.SelectedValue)
            End If
            ResultField = "Process_Code, Status, Effective, Employee_ID, Employee_Name, Hire_Date, MasaKerja, PHKReason, FgSalaryPartial, Total_Process, Total_Paid"
            ViewState("Sender") = "btnProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpTo_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Try
            pnlData.Visible = True
            pnlView.Visible = False
            ' pnlDt.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Return Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            pnlView.Visible = False
            pnlData.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn No Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            If ViewState("btnclick") = "Complete" Then
                CompleteTerminate(ViewState("Langkah"))
            Else
                ComputeTerminate(ViewState("Langkah"))
            End If

        Catch ex As Exception
            lbStatus.Text = "btn Yes Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTrigger.Click
    '    Try
    '        BindDataResult()
    '    Catch ex As Exception
    '        lbStatus.Text = "button clik Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub btnUnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnComplete.Click
        Dim sqlstring, Result As String
        Try
            If Not (tbStatus.Text = "P") Then
                lbStatus.Text = MessageDlg("Un-Posting Failed... Status Process Terminate must be Posted")
                Exit Sub
            End If
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYTerminateUnPost " + QuotedStr(tbTerminateNo.Text) + ", " + QuotedStr(tbEmpNo.Text) + ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                lbStatus.Text = Result
                Exit Sub
            End If
            tbStatus.Text = "G"
            btnCompute.Visible = tbStatus.Text = "H" Or tbStatus.Text = "G"
            btnComplete.Visible = tbStatus.Text = "G"
            btnUnComplete.Visible = tbStatus.Text = "P"
        Catch ex As Exception
            lbStatus.Text = "btnUnComplete_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
