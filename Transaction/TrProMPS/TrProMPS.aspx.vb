Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrProMPS_TrProMPS
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct TransNmbr, Nmbr, Status, TransDate, Department, Dept_Name, Remark FROM V_ProMPSHd "

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
                'If ViewState("Sender") = "btnStart" Then
                '    tbStartPeriod.Text = Session("Result")(0).ToString
                '    lbStartPeriod.Text = Session("Result")(1).ToString
                '    btnCreateMRP.Visible = tbStartPeriod.Text <> "" And tbEndPeriod.Text <> ""

                '    BindDataResult()
                '    pnlData.Visible = True
                '    pnlView.Visible = False
                '    MultiView1.ActiveViewIndex = 0
                '    Menu1.Items.Item(0).Selected = True
                'End If
                'If ViewState("Sender") = "btnEnd" Then
                '    tbEndPeriod.Text = Session("Result")(0).ToString
                '    lbEndPeriod.Text = Session("Result")(1).ToString
                '    btnCreateMRP.Visible = tbStartPeriod.Text <> "" And tbEndPeriod.Text <> ""

                '    BindDataResult()
                '    pnlData.Visible = True
                '    pnlView.Visible = False
                '    MultiView1.ActiveViewIndex = 0
                '    Menu1.Items.Item(0).Selected = True
                'End If
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
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))            
            FillCombo(ddlWorkCtr, "EXEC S_GetWorkCtr ", True, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
            FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "EXEC S_GetCostCtrDept " + QuotedStr(ddlDept.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            'tbStartDate.SelectedDate = ViewState("ServerDate")
            'tbEndDate.SelectedDate = ViewState("ServerDate")
            'btnProcess.Visible = True
            'btnCreateMRP.Visible = False

            'btnCreateMRP.Visible = tbStartDate.Text <> "" And tbStartDate.Text <> ""
            tbPeriode.SelectedDate = ViewState("ServerDate")

            BindDataResult()
            pnlData.Visible = True
            pnlView.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
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
            'System.Threading.Thread.Sleep(3000)
            StrFilter = ""
            'DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(Format(tbPeriode.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count = 0 Then
                DtTemp = DT.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridView1.DataSource = DtTemp
            Else
                DV = DT.DefaultView
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "Product DESC"
                End If
                DV.Sort = ViewState("SortExpression")
                GridView1.DataSource = DV
            End If
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataResultExport(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DtTemp As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            GridView2.Visible = True
            'System.Threading.Thread.Sleep(3000)
            StrFilter = ""
            'DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            DT = SQLExecuteQuery("EXEC S_PDPlanP " + QuotedStr(Format(tbPeriode.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count = 0 Then
                DtTemp = DT.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridView2.DataSource = DtTemp
            Else
                DV = DT.DefaultView
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "Product DESC"
                End If
                'DV.Sort = ViewState("SortExpression")
                GridView2.DataSource = DV
            End If
            GridView2.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BindDataMRP()
        Dim Dt, DtTemp As DataTable
        Dim DV As DataView
        Try
            'Dt = SQLExecuteQuery("EXEC S_PDFormMRP " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + ViewState("UserId").ToString, ViewState("DBConnection").ToString).Tables(0)
            'Dt = SQLExecuteQuery("EXEC S_PDFormMRP " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + ViewState("UserId").ToString, ViewState("DBConnection").ToString).Tables(0)

            'lbStatus.Text = "EXEC S_PDMRP " + QuotedStr(Format(tbPeriode.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId").ToString)
            'Exit Sub

            Dt = SQLExecuteQuery("EXEC S_PDMRP " + QuotedStr(Format(tbPeriode.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                DtTemp = Dt.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridMRP.DataSource = DtTemp
            Else
                DV = Dt.DefaultView
                If ViewState("SortExpressionLoan") = Nothing Then
                    ViewState("SortExpressionLoan") = "Product DESC"
                End If
                DV.Sort = ViewState("SortExpressionLoan")
                GridMRP.DataSource = DV
                'GridMRP.Columns(0).Visible = False
                'GridMRP.Columns(1).Visible = False
            End If
            MovePanel(pnlCreateMRP, pnlViewMRP)
            GridMRP.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataMRP Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataMRPExport()
        Dim Dt, DtTemp As DataTable
        Dim DV As DataView
        Try
            GridView3.Visible = True
            'Dt = SQLExecuteQuery("EXEC S_PDFormMRP " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + ViewState("UserId").ToString, ViewState("DBConnection").ToString).Tables(0)
            'Dt = SQLExecuteQuery("EXEC S_PDFormMRP " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + ViewState("UserId").ToString, ViewState("DBConnection").ToString).Tables(0)
            Dt = SQLExecuteQuery("EXEC S_PDMRP " + QuotedStr(Format(tbPeriode.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count = 0 Then
                DtTemp = Dt.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridView3.DataSource = DtTemp
            Else
                DV = Dt.DefaultView
                If ViewState("SortExpressionLoan") = Nothing Then
                    ViewState("SortExpressionLoan") = "Product DESC"
                End If
                DV.Sort = ViewState("SortExpressionLoan")
                GridView3.DataSource = DV
                'GridMRP.Columns(0).Visible = False
                'GridMRP.Columns(1).Visible = False
            End If
            MovePanel(pnlCreateMRP, pnlViewMRP)
            GridView3.DataBind()
        Catch ex As Exception
            lbStatus.Text = "BindDataMRP Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataPR()
        Dim Dt, DtTemp As DataTable
        Dim DV As DataView
        Try
            Dt = SQLExecuteQuery("EXEC S_PDMRPGetPR " + QuotedStr(ddlWorkCtr.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString).Tables(0)
            'End If
            DV = Dt.DefaultView
            If DV.Count = 0 Then
                GridPR.Visible = False
            Else
                GridPR.Visible = True
                If ViewState("SortExpressionPR") = Nothing Then
                    ViewState("SortExpressionPR") = "Product DESC"
                End If
                DV.Sort = ViewState("SortExpressionPR")
                GridPR.DataSource = DV
                GridPR.DataBind()
            End If

        Catch ex As Exception
            lbStatus.Text = "BindDataPR Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindDataResult()
    End Sub

    Protected Sub GridMRP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridMRP.PageIndexChanging
        Try
            GridMRP.PageIndex = e.NewPageIndex
            BindDataMRP()
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

    Protected Sub GridMRP_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridMRP.RowCancelingEdit
        Try
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridMRP.EditIndex = -1
            BindDataMRP()
        Catch ex As Exception
            lbStatus.Text = "GridMRP_RowCancelingEdit Error : " + vbCrLf + ex.ToString
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

    Protected Sub GridMRP_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridMRP.Sorting
        Try
            If ViewState("SortOrderLoan") = Nothing Or ViewState("SortOrderLoan") = "DESC" Then
                ViewState("SortOrderLoan") = "ASC"
            Else
                ViewState("SortOrderLoan") = "DESC"
            End If
            ViewState("SortExpressionLoan") = e.SortExpression + " " + ViewState("SortOrderLoan")
            BindDataMRP()
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
                Try
                    BindDataMRP()
                    pnlData.Visible = True
                    pnlView.Visible = False
                    MultiView1.ActiveViewIndex = 1
                    Menu1.Items.Item(1).Selected = True
                    'lbStatus.Text = MessageDlg("Process MRP has finished !")
                    btnCreatePR.Visible = True
                    'btnCreateWO.Visible = True
                Catch ex As Exception
                    lbStatus.Text = "btnCreateMRP_Click Error : " + ex.ToString
                End Try                
            Else
                BindDataResult()
                'btnProcess.Visible = True
                'btnCreateMRP.Visible = False
                btnCreatePR.Visible = False
                'btnCreateWO.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnCreateMRP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateMRP.Click
    '    Dim EMessage As String
    '    Try
    '        'sp cek
    '        'If tbStartPeriod.Text = "" Then
    '        '    lbStatus.Text = MessageDlg("Start Period must be selected")
    '        '    Exit Sub
    '        'End If
    '        'If tbEndPeriod.Text = "" Then
    '        '    lbStatus.Text = MessageDlg("End Period must be selected")
    '        '    Exit Sub
    '        'End If
    '        ''ViewState("btnclick") = "Compute"
    '        ''ViewState("Langkah") = 0
    '        'If tbStartDate.Text = "" Then
    '        '    lbStatus.Text = MessageDlg("Start Date must be selected")
    '        '    Exit Sub
    '        'End If
    '        'If tbEndDate.Text = "" Then
    '        '    lbStatus.Text = MessageDlg("End Date must be selected")
    '        '    Exit Sub
    '        'End If
    '        'EMessage = CreateMRP1(tbStartPeriod.Text, tbEndPeriod.Text)
    '        EMessage = CreateMRP1(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"))
    '        If Len(EMessage) > 5 Then
    '            lbStatus.Text = EMessage
    '            Exit Sub
    '        End If
    '        'EMessage = CreateMRP2(tbStartPeriod.Text, tbEndPeriod.Text)
    '        EMessage = CreateMRP2(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"))
    '        If Len(EMessage) > 5 Then
    '            lbStatus.Text = EMessage
    '            Exit Sub
    '        End If
    '        'EMessage = CreateMRP3(tbStartPeriod.Text, tbEndPeriod.Text)
    '        EMessage = CreateMRP3(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"))
    '        If Len(EMessage) > 5 Then
    '            lbStatus.Text = EMessage
    '            Exit Sub
    '        End If
    '        BindDataMRP()
    '        pnlData.Visible = True
    '        pnlView.Visible = False
    '        MultiView1.ActiveViewIndex = 1
    '        Menu1.Items.Item(1).Selected = True            
    '        lbStatus.Text = MessageDlg("Process MRP has finished !")
    '        btnCreatePR.Visible = True
    '        btnCreateWO.Visible = True
    '    Catch ex As Exception
    '        lbStatus.Text = "btnCreateMRP_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Function CreateMRP1() As String
    '    Try
    '        Dim Result As String
    '        Result = ""
    '        'Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMPSCreateMRP1 " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + ViewState("UserId").ToString + ", @A Out ", ViewState("DBConnection").ToString)
    '        Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMRP " + ViewState("UserId").ToString + ", @A Out ", ViewState("DBConnection").ToString)
    '        Return Result
    '    Catch ex As Exception
    '        lbStatus.Text = "CreateMRP1 Error : " + ex.ToString
    '        Return lbStatus.Text
    '    End Try
    'End Function

    'Function CreateMRP2(ByVal StartP As String, ByVal EndP As String) As String
    '    Dim dt As DataTable
    '    Try
    '        Dim Level As String
    '        Dim Result As String
    '        Result = ""
    '        dt = SQLExecuteQuery("Select DISTINCT ProductLvl FROM(PROMRPPrior) ORDER BY ProductLvl ", ViewState("DBConnection").ToString).Tables(0)
    '        For Each dr In dt.Rows
    '            Level = dr("ProductLvl").ToString
    '            Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMPSCreateMRP2 " + QuotedStr(Level) + ", " + ViewState("UserId").ToString + ", @A Out ", ViewState("DBConnection").ToString)
    '            If Len(Result) > 5 Then
    '                Return Result
    '                Exit For
    '            End If
    '        Next
    '        Return Result
    '    Catch ex As Exception
    '        lbStatus.Text = "CreateMRP2 Error : " + ex.ToString
    '        Return lbStatus.Text
    '    End Try
    'End Function

    'Function CreateMRP3(ByVal StartP As String, ByVal EndP As String) As String
    '    Try
    '        Dim Result As String
    '        Result = ""
    '        Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMPSCreateMRP3 " + ViewState("UserId").ToString + ", @A Out ", ViewState("DBConnection").ToString)
    '        Return Result
    '    Catch ex As Exception
    '        lbStatus.Text = "CreateMRP3 Error : " + ex.ToString
    '        Return lbStatus.Text
    '    End Try
    'End Function

    Function CreatePR() As String
        Try
            Dim Result As String
            Result = ""
            Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMRPCreatePR " + QuotedStr(ddlWorkCtr.SelectedValue) + ", " + QuotedStr(ddlToPurchase.SelectedValue) + ", " + QuotedStr(ddlDept.SelectedValue) + ", " + QuotedStr(ddlCostCtr.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out ", ViewState("DBConnection").ToString)
            Return Result
        Catch ex As Exception
            lbStatus.Text = "CreatePR Error : " + ex.ToString
            Return lbStatus.Text
        End Try
    End Function

    'Function CreateWO() As String
    '    Try
    '        Dim Result As String
    '        Result = ""
    '        'Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMRPCreateWO " + QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + ViewState("UserId").ToString + ", @A Out ", ViewState("DBConnection").ToString)
    '        Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDMRPCreateWO " + QuotedStr(ViewState("UserId").ToString) + ", @A Out ", ViewState("DBConnection").ToString)
    '        Return Result
    '    Catch ex As Exception
    '        Return "CreateWO Error : " + ex.ToString
    '    End Try
    'End Function

    'Protected Sub btnStartPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartPeriod.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DbConnection")
    '        If tbEndPeriod.Text = "" Then
    '            Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod "
    '        Else
    '            Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode <= " + QuotedStr(tbEndPeriod.Text)
    '        End If
    '        ResultField = "PeriodCode, PeriodName"
    '        ViewState("Sender") = "btnStart"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnStartPeriod_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnEndPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndPeriod.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DbConnection")
    '        If tbStartPeriod.Text = "" Then
    '            Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod "
    '        Else
    '            Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode >= " + QuotedStr(tbStartPeriod.Text)
    '        End If
    '        ResultField = "PeriodCode, PeriodName"
    '        ViewState("Sender") = "btnEnd"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnEndPeriod_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbStartPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartPeriod.TextChanged
    '    Dim dt As DataTable
    '    Try
    '        If tbEndPeriod.Text = "" Then
    '            dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbStartPeriod.Text), ViewState("DBConnection")).Tables(0)
    '        Else
    '            dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbStartPeriod.Text) + " AND PeriodCode <= " + QuotedStr(tbEndPeriod.Text), ViewState("DBConnection")).Tables(0)
    '        End If
    '        If dt.Rows.Count > 0 Then
    '            tbStartPeriod.Text = dt.Rows(0)("PeriodCode").ToString
    '            lbStartPeriod.Text = dt.Rows(0)("PeriodName").ToString
    '            If tbEndPeriod.Text < tbStartPeriod.Text Then
    '                tbEndPeriod.Text = ""
    '                lbEndPeriod.Text = ""
    '            End If
    '        Else
    '            tbStartPeriod.Text = ""
    '            lbStartPeriod.Text = ""
    '        End If
    '        BindDataResult()
    '        pnlData.Visible = True
    '        pnlView.Visible = False
    '        MultiView1.ActiveViewIndex = 0
    '        Menu1.Items.Item(0).Selected = True
    '    Catch ex As Exception
    '        lbStatus.Text = "tbStartPeriod_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbEndPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndPeriod.TextChanged
    '    Dim dt As DataTable
    '    Try
    '        If tbStartPeriod.Text = "" Then
    '            dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbEndPeriod.Text), ViewState("DBConnection")).Tables(0)
    '        Else
    '            dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbEndPeriod.Text) + " AND PeriodCode >= " + QuotedStr(tbStartPeriod.Text), ViewState("DBConnection")).Tables(0)
    '        End If
    '        If dt.Rows.Count > 0 Then
    '            tbEndPeriod.Text = dt.Rows(0)("PeriodCode").ToString
    '            lbEndPeriod.Text = dt.Rows(0)("PeriodName").ToString
    '            If tbEndPeriod.Text < tbStartPeriod.Text Then
    '                tbEndPeriod.Text = ""
    '                lbEndPeriod.Text = ""
    '            End If
    '        Else
    '            tbEndPeriod.Text = ""
    '            lbEndPeriod.Text = ""
    '        End If
    '        BindDataResult()
    '        pnlData.Visible = True
    '        pnlView.Visible = False
    '        MultiView1.ActiveViewIndex = 0
    '        Menu1.Items.Item(0).Selected = True
    '    Catch ex As Exception
    '        lbStatus.Text = "tbEndPeriod_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Try
            pnlData.Visible = True
            pnlView.Visible = False
            ' pnlDt.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Return Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTrigger.Click
    '    Try
    '        BindDataResult()
    '    Catch ex As Exception
    '        lbStatus.Text = "button clik Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnCreatePR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreatePR.Click
        'Dim EMessage As String
        Try
            MovePanel(pnlViewMRP, pnlCreateMRP)
            BindDataPR()
            'sp cek
            ''If tbStartPeriod.Text = "" Then
            'If tbStartDate.Text = "" Then
            '    lbStatus.Text = MessageDlg("Start Date must be selected")
            '    Exit Sub
            'End If
            ''If tbEndPeriod.Text = "" Then
            'If tbEndDate.Text = "" Then
            '    lbStatus.Text = MessageDlg("End Date must be selected")
            '    Exit Sub
            'End If
            'EMessage = CreatePR(tbStartPeriod.Text, tbEndPeriod.Text)
            'SQLExecuteNonQuery("EXEC S_PDMRPCreatePR " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbStatus.Text = "btnCreatePR_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCreateWO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateWO.Click
        'Dim EMessage As String
        Try
            'sp cek
            ''If tbStartPeriod.Text = "" Then
            'If tbStartDate.Text = "" Then
            '    lbStatus.Text = MessageDlg("Start Date must be selected")
            '    Exit Sub
            'End If
            ''If tbEndPeriod.Text = "" Then
            'If tbEndDate.Text = "" Then
            '    lbStatus.Text = MessageDlg("End Date must be selected")
            '    Exit Sub
            'End If
            'EMessage = CreateWO(tbStartPeriod.Text, tbEndPeriod.Text)
            'EMessage = CreateWO()
            'If Len(EMessage) > 5 Then
            '    lbStatus.Text = EMessage
            '    Exit Sub
            'End If
            'lbStatus.Text = MessageDlg("Process Create WO has finished !")
            SQLExecuteNonQuery("EXEC S_PDMRPCreateWO " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbStatus.Text = "btnCreateWO_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbStartDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartDate.SelectionChanged
    '    'btnCreateMRP.Visible = tbStartDate.Text <> "" And tbStartDate.Text <> ""

    '    'BindDataResult()
    '    'pnlData.Visible = True
    '    'pnlView.Visible = False
    '    'MultiView1.ActiveViewIndex = 0
    '    'Menu1.Items.Item(0).Selected = True
    'End Sub

    'Protected Sub tbEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDate.SelectionChanged
    '    'btnCreateMRP.Visible = tbStartDate.Text <> "" And tbStartDate.Text <> ""
    '    'BindDataResult()
    '    'pnlData.Visible = True
    '    'pnlView.Visible = False
    '    'MultiView1.ActiveViewIndex = 0
    '    'Menu1.Items.Item(0).Selected = True
    'End Sub

    'Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
    '    BindDataResult()
    '    pnlData.Visible = True
    '    pnlView.Visible = False
    '    MultiView1.ActiveViewIndex = 0
    '    Menu1.Items.Item(0).Selected = True
    'End Sub

    Protected Sub GridPR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridPR.PageIndexChanging
        Try
            GridPR.PageIndex = e.NewPageIndex
            BindDataPR()
        Catch ex As Exception
            lbStatus.Text = "GridPR_PageIndexChanging Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridPR_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridPR.Sorting
        Try
            If ViewState("SortOrderPR") = Nothing Or ViewState("SortOrderPR") = "DESC" Then
                ViewState("SortOrderPR") = "ASC"
            Else
                ViewState("SortOrderPR") = "DESC"
            End If
            ViewState("SortExpressionPR") = e.SortExpression + " " + ViewState("SortOrderPR")
            BindDataPR()
        Catch ex As Exception
            lbStatus.Text = "GridPR_Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim SQLString, Hasil As String
        Dim lbCode As Label
        Dim tbQtyNR As TextBox
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            If ddlWorkCtr.SelectedValue.Trim = "" Then
                lbStatus.Text = "Work Center must be fill"
                ddlWorkCtr.Focus()
                Exit Sub
            End If
            If ddlDept.SelectedValue.Trim = "" Then
                lbStatus.Text = "Department must be fill"
                ddlDept.Focus()
                Exit Sub
            End If
            If ddlCostCtr.SelectedValue.Trim = "" Then
                lbStatus.Text = "Cost Ctr must be fill"
                ddlCostCtr.Focus()
                Exit Sub
            End If
            exe = True
            For i = 0 To GridPR.Rows.Count - 1
                GVR = GridPR.Rows(i)
                lbCode = GVR.FindControl("Product")
                tbQtyNR = GVR.FindControl("QtyNR")
                tbQtyNR.Text = tbQtyNR.Text.Replace(",", "")
                If tbQtyNR.Text.Trim = "" Then
                    tbQtyNR.Text = "0"
                End If
                If Not IsNumeric(tbQtyNR.Text) Then
                    lbStatus.Text = "Qty NR for Product " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbQtyNR.Focus()
                    Exit For
                End If
            Next
            If exe Then
                For i = 0 To GridPR.Rows.Count - 1
                    GVR = GridPR.Rows(i)
                    lbCode = GVR.FindControl("Product")
                    tbQtyNR = GVR.FindControl("QtyNR")
                    tbQtyNR.Text = tbQtyNR.Text.Replace(",", "")
                    If tbQtyNR.Text.Trim = "" Then
                        tbQtyNR.Text = "0"
                    End If
                    SQLString = "UPDATE PROMRPProduct SET QtyNR =" + tbQtyNR.Text + " WHERE Period = 'RM' AND Product = " + QuotedStr(lbCode.Text.Trim) + " and UserId = " + QuotedStr(ViewState("UserId").ToString)
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
                Hasil = CreatePR()
                If Hasil.Trim.Length > 5 Then
                    lbStatus.Text = Hasil
                    Exit Sub
                End If
                BindDataMRP()
            End If
        Catch ex As Exception
            lbStatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDept.SelectedIndexChanged
        Try
            FillCombo(ddlCostCtr, "EXEC S_GetCostCtrDept " + QuotedStr(ddlDept.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "ddlDept_SelectedIndexChanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlWorkCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkCtr.SelectedIndexChanged
        Try
            BindDataPR()
        Catch ex As Exception
            lbStatus.Text = "ddlWorkCtr_SelectedIndexChanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            If MultiView1.ActiveViewIndex = 0 Then
                'lbStatus.Text = "masuk 0"
                BindDataResultExport()
                ExportGridToExcel()
                GridView2.Visible = False
            Else
                'lbStatus.Text = "masuk 1"
                BindDataMRPExport()
                ExportGridToExcel()
                GridView3.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "btnImport_Click error : " + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel()
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname, StrWorkname As String

        If MultiView1.ActiveViewIndex = 0 Then
            StrWorkname = Trim("Production Plan")
        Else
            StrWorkname = Trim("MRP")
        End If

        worksheetname = Left(StrWorkname, 31)

        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + worksheetname + ".xls"

        'If CekDt() = False Then
        '    Exit Sub
        'End If

        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      

        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)

        If MultiView1.ActiveViewIndex = 0 Then
            GridView2.Parent.Controls.Add(form)
            form.Attributes("runat") = "server"
            form.Controls.Add(GridView2)
        Else
            GridView3.Parent.Controls.Add(form)
            form.Attributes("runat") = "server"
            form.Controls.Add(GridView3)
        End If
        
        Me.Controls.Add(form)

        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub tbPeriode_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPeriode.SelectionChanged
        If Menu1.Items.Item(0).Selected = True Then
            BindDataResult()
        ElseIf Menu1.Items.Item(1).Selected = True Then
            BindDataMRP()
        End If
    End Sub
End Class


