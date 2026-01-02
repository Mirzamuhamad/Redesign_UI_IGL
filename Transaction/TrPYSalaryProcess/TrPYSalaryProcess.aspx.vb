Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPYSalaryProcess_TrPYSalaryProcess
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct TransNmbr, Nmbr, Status, TransDate, Department, Dept_Name, Remark FROM V_PYSalaryProcessHd "

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
                    tbStatus.Text = Session("Result")(1).ToString
                    ddlSalaryProcess.SelectedValue = Session("Result")(0).ToString
                    
                    btnCompute.Visible = False
                    btnComplete.Visible = False
                    btnCompute.Visible = (tbStatus.Text = "P" Or tbStatus.Text = "C")
                    btnComplete.Visible = tbStatus.Text = "C"

                    tbCode.Text = ""
                    tbName.Text = ""
                    BindDataResult()
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                End If
                If ViewState("Sender") = "btnCode" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
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
            FillCombo(ddlMethod, "Select MethodCode, MethodName from V_MsMethodUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "MethodCode", "MethodName", ViewState("DBConnection"))
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
            tbName.Attributes.Add("ReadOnly", True)
            ddlMethod.SelectedValue = ""
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
            DT = SQLExecuteQuery("EXEC S_PYProcessViewResult " + QuotedStr(ddlSalaryProcess.SelectedValue) + ", " + QuotedStr(ddlGroupBy.SelectedItem.Text) + ", " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Viewstate("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count = 0 Then
                DtTemp = DT.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridView1.DataSource = DtTemp
            Else
                DV = DT.DefaultView
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "ItemNo ASC"
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
            Dt = SQLExecuteQuery("EXEC S_PYProcessViewLoan " + QuotedStr(ddlSalaryProcess.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
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

    Protected Sub ddlMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMethod.SelectedIndexChanged, ddlYear.SelectedIndexChanged, ddlMonth.SelectedIndexChanged
        Try
            FillCombo(ddlSalaryProcess, "SELECT A.ProcessCode FROM PYProcessHd A INNER JOIN V_MsMethodUser B ON A.Method = B.MethodCode " + _
			" WHERE B.UserId = " +QuotedStr(Viewstate("UserId").ToString)+ " and Status IN ('P', 'C', 'F') AND Method = " + QuotedStr(ddlMethod.SelectedValue) + " AND [Year] = " + ddlYear.SelectedValue + " AND [Month] = " + ddlMonth.SelectedValue, True, "ProcessCode", "ProcessCode", ViewState("DBConnection"))
            ddlSalaryProcess.SelectedValue = ""
            tbCode.Text = ""
            tbName.Text = ""
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "ddlMethod_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If index = 1 Then
                BindDataLoan()
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ComputeSalary(ByVal Langkah As Integer)
        Dim sqlstring As String
        Dim StrMsg As String
        Dim DS As DataSet
        Try
            'lbStatus.Text = MessageDlg("test")
            'Exit Sub
            'WaitOpen.Show("Computing Stock")
            'mpewait.Show()
            'Timer1.Enabled = True
            If Langkah = 0 Then
                sqlstring = "EXEC S_PYProcessComputeCek01Abs " + QuotedStr(ddlSalaryProcess.SelectedValue)
                DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))
                Langkah = 1
                ViewState("Langkah") = Langkah
                If DS.Tables(0).Rows.Count > 0 Then
                    pnlData.Visible = False
                    pnlView.Visible = True
                    btnYes.Visible = True
                    btnNo.Visible = True
                    btnReturn.Visible = False
                    'mpewait.Hide()
                    'WaitOpen.Hide()
                    lbMessage.Text = "Compute failed..., please make sure this employee have absence"
                    GridForView.DataSource = DS.Tables(0)
                    GridForView.DataBind()
                    Exit Sub
                End If
            End If
            If Langkah = 1 Then
                ComputeCek02()
                Langkah = 2
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 2 Then
                ComputeCek03()
                Langkah = 3
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 3 Then
                ComputeCek04()
                Langkah = 4
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 4 Then
                ComputeCek05()
                Langkah = 5
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 5 Then
                ComputeCek06()
                Langkah = 6
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 6 Then
                ComputeCek07()
                Langkah = 10
                ViewState("Langkah") = Langkah
            End If
            StrMsg = ""
            If Langkah = 10 Then
                StrMsg = doComputeProcess()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    WaitOpen.Hide()
                    lbStatus.Text = StrMsg
                    Exit Sub
                End If
                
                StrMsg = doComputeProcessPPh()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    'WaitOpen.Hide()
                    lbStatus.Text = StrMsg
                    Exit Sub
                End If
                'tampilan data
                tbCode.Text = ""
                tbName.Text = ""
                BindDataResult()
                BindDataLoan()
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                'mpewait.Hide()
                'WaitOpen.Hide()
                'lbStatus.Text = MessageDlg("Success Compute Salary Process")
            End If
        Catch ex As Exception
            lbStatus.Text = "Compute Salary Process Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCompute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCompute.Click
        Try
            'sp cek
            If ddlMethod.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Method Salary must be selected")
                Exit Sub
            End If
            If ddlYear.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Year Salary must be selected")
                Exit Sub
            End If
            If ddlMonth.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Month Salary must be selected")
                Exit Sub
            End If
            If ddlSalaryProcess.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Method Salary must be selected")
                Exit Sub
            End If
            If Not ((tbStatus.Text = "P") Or (tbStatus.Text = "C")) Then
                lbStatus.Text = MessageDlg("Compute Failed... Status Process Salary must be posted or Compute")
                Exit Sub
            End If
            ViewState("btnclick") = "Compute"
            ViewState("Langkah") = 0

            'WaitOpen.Show("Compute Analyzing")
            'System.Threading.Thread.Sleep(1000)
            'Timer1.Enabled = True
            ComputeSalary(ViewState("Langkah"))

            lbStatus.Text = MessageDlg("Compute Salary Process Complete")
            'lbStatus.Text = "Oke"
        Catch ex As Exception
            lbStatus.Text = "btn Compute Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    '    'WaitOpen.Hide()
    '    Timer1.Enabled = False

    '    ComputeSalary(ViewState("Langkah"))

    '    WaitOpen.Hide()

    '    'lbStatus.Text = "Oke"
    'End Sub

    Protected Sub ComputeCek02()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessComputeCek02Abs " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek03Ovt " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK LEMBUR YANG BELOM COMPLETE")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek03Ovt " + QuotedStr(ddlSalaryProcess.SelectedValue)

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
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek04Ben " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA BENEFIT YANG BELUM DIPOSTING")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek04Ben " + QuotedStr(ddlSalaryProcess.SelectedValue)

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
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek05Ded " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA DEDUCTION YANG BELUM DIPOSTING")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek05Ded " + QuotedStr(ddlSalaryProcess.SelectedValue)

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
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek06Loan " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("CEK ADA LEMBUR YANG BLOM COMPLETE")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek06Loan " + QuotedStr(ddlSalaryProcess.SelectedValue)

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
            sqlstring = "EXEC S_PYProcessComputeCek07PPh " + QuotedStr(ddlSalaryProcess.SelectedValue)

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
            'dt = SQLExecuteQuery("EXEC S_PYProcessComputeCek08Jmsk " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'If dt.Rows.Count <> 0 Then
            '    lbStatus.Text = MessageDlg("Cek Jamsostek")  'dt.Rows(0)("Status")
            '    Exit Sub
            'End If
            sqlstring = "EXEC S_PYProcessComputeCek08Jmsk " + QuotedStr(ddlSalaryProcess.SelectedValue)

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

    Function doComputeProcess() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYProcessCompute " + QuotedStr(ddlSalaryProcess.SelectedValue) + _
            ", " + QuotedStr(ViewState("UserId").ToString) + _
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
            Throw New Exception("do Compute Salary Error : " + ex.ToString)
        End Try
    End Function

    Function doComputeProcessPPh() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYProcessComputePPh " + QuotedStr(ddlSalaryProcess.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
            'lbStatus.Text = ViewState("DBConnection")
            'Exit Function
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If
            Return ""
        Catch ex As Exception
            Throw New Exception("do Compute Salary Error : " + ex.ToString)
        End Try
    End Function


    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Try
            If ddlMethod.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Method Salary must be selected")
                Exit Sub
            End If
            If ddlYear.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Year Salary must be selected")
                Exit Sub
            End If
            If ddlMonth.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Month Salary must be selected")
                Exit Sub
            End If
            If ddlSalaryProcess.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Method Salary must be selected")
                Exit Sub
            End If
            If Not (tbStatus.Text = "C") Then
                lbStatus.Text = MessageDlg("Complete Failed... Status Process Salary must be Compute")
                Exit Sub
            End If
            ViewState("btnclick") = "Complete"
            ViewState("Langkah") = 0
            CompleteSalary(ViewState("Langkah"))
        Catch ex As Exception
            lbStatus.Text = "btnComplete_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CompleteCek02()
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            sqlstring = "EXEC S_PYProcessFinishCek02Loan " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            sqlstring = "EXEC S_PYProcessFinishCek03Ovt " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            sqlstring = "EXEC S_PYProcessFinishCek04Ben " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            sqlstring = "EXEC S_PYProcessFinishCek05Ded " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            sqlstring = "EXEC S_PYProcessFinishCek06Medical " + QuotedStr(ddlSalaryProcess.SelectedValue)
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
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_PYProcessFinish " + QuotedStr(ddlSalaryProcess.SelectedValue) + _
            ", " + QuotedStr(ViewState("UserId").ToString) + _
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

    Protected Sub CompleteSalary(ByVal Langkah As Integer)
        Dim sqlstring As String
        Dim StrMsg As String
        Dim DS As DataSet
        Try
            If Langkah = 0 Then
                sqlstring = "EXEC S_PYProcessFinishCek01Adjust " + QuotedStr(ddlSalaryProcess.SelectedValue)
                DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))
                Langkah = 1
                ViewState("Langkah") = Langkah
                If DS.Tables(0).Rows.Count > 0 Then
                    pnlData.Visible = False
                    pnlView.Visible = True
                    btnYes.Visible = False
                    btnNo.Visible = False
                    btnReturn.Visible = True
                    'mpewait.Hide()
                    lbMessage.Text = "Complete failed..., please make sure this Salary Adjustment Posted"
                    GridForView.DataSource = DS.Tables(0)
                    GridForView.DataBind()
                    Exit Sub
                End If
            End If
            If Langkah = 1 Then
                CompleteCek02()
                Langkah = 2
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 2 Then
                CompleteCek03()
                Langkah = 3
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 3 Then
                CompleteCek04()
                Langkah = 4
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 4 Then
                CompleteCek05()
                Langkah = 5
                ViewState("Langkah") = Langkah
            End If
            If Langkah = 5 Then
                CompleteCek06()
                Langkah = 10
                ViewState("Langkah") = Langkah
            End If
            StrMsg = ""
            If Langkah = 10 Then
                StrMsg = doCompleteProcess()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    lbStatus.Text = StrMsg
                    Exit Sub
                End If
                'tampilan data
                tbCode.Text = ""
                tbName.Text = ""
                BindDataResult()
                BindDataLoan()
                MultiView1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                'mpewait.Hide()
                lbStatus.Text = MessageDlg("Success Complete Salary Process")
            End If
        Catch ex As Exception
            lbStatus.Text = "Complete Salary Process Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT ProcessCode, Status, Method, Year, Month Remark FROM PYProcessHd WHERE Status IN ('P', 'C', 'F') AND Method = " + QuotedStr(ddlMethod.SelectedValue) + " AND Year = " + QuotedStr(ddlYear.SelectedValue) + " AND Month = " + QuotedStr(ddlMonth.SelectedValue)
            ResultField = "ProcessCode, Status, Method, Year, Month"
            ViewState("Sender") = "btnProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpTo_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlGroupBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroupBy.SelectedIndexChanged
        Try
            tbCode.Text = ""
            tbName.Text = ""
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "ddlGroupBy_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim dt As DataTable
        Try
            If ddlGroupBy.SelectedValue = "Employee" Then
                dt = SQLExecuteQuery("Select DISTINCT Employee_No, Employee_Name from V_PYPayrollView WHERE ProcessCode = " + QuotedStr(ddlSalaryProcess.SelectedValue) + " and Employee_No = " + QuotedStr(tbCode.Text), ViewState("DBConnection")).Tables(0)
                If dt.Rows.Count > 0 Then
                    tbCode.Text = dt.Rows(0)("Employee_No").ToString
                    tbName.Text = dt.Rows(0)("Employee_Name").ToString
                Else
                    tbCode.Text = ""
                    tbName.Text = ""
                End If
            Else
                dt = SQLExecuteQuery("Select DISTINCT Payroll_Code, Payroll_Name, FgSlip from V_PYPayrollView WHERE ProcessCode = " + QuotedStr(ddlSalaryProcess.SelectedValue) + " and Payroll_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection")).Tables(0)
                If dt.Rows.Count > 0 Then
                    tbCode.Text = dt.Rows(0)("Payroll_Code").ToString
                    tbName.Text = dt.Rows(0)("Payroll_Name").ToString
                Else
                    tbCode.Text = ""
                    tbName.Text = ""
                End If
            End If
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "tbCode_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCode.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            If ddlGroupBy.SelectedValue = "Employee" Then
                Session("filter") = "SELECT DISTINCT Employee_No, Employee_Name FROM V_PYPayrollView WHERE ProcessCode = " + QuotedStr(ddlSalaryProcess.SelectedValue)
                ResultField = "Employee_No, Employee_Name"
            Else
                Session("filter") = "SELECT DISTINCT Payroll_Code, Payroll_Name, FgSlip FROM V_PYPayrollView WHERE ProcessCode = " + QuotedStr(ddlSalaryProcess.SelectedValue)
                ResultField = "Payroll_Code, Payroll_Name"
            End If
            ViewState("Sender") = "btnCode"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCode_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlSalaryProcess_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSalaryProcess.SelectedIndexChanged
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("Select ProcessCode, Status FROM PYProcessHd WHERE Status IN ('P', 'C', 'F') AND Method = " + QuotedStr(ddlMethod.SelectedValue) + " AND [Year] = " + ddlYear.SelectedValue + " AND [Month] = " + ddlMonth.SelectedValue + " AND ProcessCode = " + QuotedStr(ddlSalaryProcess.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                tbStatus.Text = dt.Rows(0)("Status")
            Else
                tbStatus.Text = ""
            End If
            btnCompute.Visible = False
            btnComplete.Visible = False
            btnCompute.Visible = (tbStatus.Text = "P" Or tbStatus.Text = "C")
            btnComplete.Visible = tbStatus.Text = "C"
            tbCode.Text = ""
            tbName.Text = ""
            BindDataResult()
            pnlData.Visible = True
            pnlView.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
        Catch ex As Exception
            lbStatus.Text = "ddlSalaryProcess_SelectedIndexChanged Error : " + ex.ToString
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
                CompleteSalary(ViewState("Langkah"))
            Else
                ComputeSalary(ViewState("Langkah"))
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
End Class
