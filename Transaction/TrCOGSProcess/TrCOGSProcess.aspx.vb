Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

Partial Class Transaction_TrCOGSProcess_TrCOGSProcess
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_STProcessHd "

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("COGSProcess")
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            'If Request.QueryString("ContainerId").ToString = "TrCOGSProcessFGId" Then
            '    ViewState("FgType") = "FG"
            '    ViewState("FgTypeName") = "Finish Good"
            '    lblTitle.Text = "COGS Process Finish Good"
            'Else
            lblTitle.Text = "Process COGS Stock"
            'End If
            'GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            'GridForView.PageSize = CInt(ViewState("PageSizeGrid"))
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
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
        btnCompute.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnCompute, "").ToString())
        btnComplete.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnComplete, "").ToString())
        btnUnComplete.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnUnComplete, "").ToString())
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                'BtnGo.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            'btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "StartDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()

            'lbStatus.Text = CheckDataCount().ToString
            'Exit Sub

            BtnAdd.Visible = CheckDataCount()
            btnAdd2.Visible = CheckDataCount()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function CheckDataCount() As Boolean
        Dim SqlString, result As String
        Try
            SqlString = "Select count(StartDate) FROM V_STProcessHd WHERE Status in ('G', 'H', 'P', 'C') "
            result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
            'lbStatus.Text = "Result : " + result
            'Exit Function
            If result <> "0" Then
                Return False
            End If

            Return True

        Catch ex As Exception
            Throw New Exception("Check Data Count Error : " + ex.ToString)
        End Try
    End Function

    Private Function GetStringDt(ByVal StartDate As String, ByVal ProductType As String) As String
        Return "SELECT * From V_STProcessDt WHERE StartDate = " + QuotedStr(StartDate) + " And Product_Type = " + QuotedStr(ProductType) + " AND (BeginQty <> '0' OR Inqty <> '0' AND OutQty <> '0' OR BalanceQty <> '0') Order By Product "
    End Function


    Private Sub BindDataDt(ByVal StartDate As String, ByVal ProductType As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            'lbStatus.Text = (GetStringDt(StartDate, TypeDt))
            'Exit Sub
            dt = SQLExecuteQuery(GetStringDt(StartDate, ProductType), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
            GridDt.Columns(0).Visible = False
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub newTrans()
        Try
            ClearHd()
            pnlDt.Visible = True
            BindDataDt("", "")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            'BindToDropList(ddlYear, Now.Year.ToString)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim CekMenu As String
        Try
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                GridDt.PageIndex = 0
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    pnlDt.Visible = True
                    ViewState("StartDate") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("StartDate"))
                    FillCombo(ddlTypeDt, "EXEC S_GetProductType ", False, "ProductType", "TypeName", ViewState("DBConnection"))
                    'FillCombo(ddlTypeDt, "SELECT Type_Code,Type_Name From VMsProducttype ", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
                    BindDataDt(ViewState("StartDate"), ddlTypeDt.SelectedValue)
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnCompute.Visible = False
                    btnComplete.Visible = False
                    btnUnComplete.Visible = False
                    btnSave.Visible = False
                    lbMessageProcess.Text = ""
                ElseIf DDL.SelectedValue = "Compute" Then
                    If GVR.Cells(4).Text = "P" Or GVR.Cells(4).Text = "C" Then
                        'CekMenu = CheckMenuLevel("Compute", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("StateHd") = "Compute"
                        ViewState("StartDate") = GVR.Cells(2).Text
                        ModifyInput2(False, pnlInput, pnlDt, GridDt)

                        FillTextBoxHd(GVR.Cells(2).Text)
                        FillCombo(ddlTypeDt, "EXEC S_GetProductType ", False, "ProductType", "TypeName", ViewState("DBConnection"))
                        BindDataDt(GVR.Cells(2).Text, ddlTypeDt.SelectedValue)


                        btnHome.Visible = True
                        btnCompute.Visible = True
                        btnCompute.Enabled = True
                        btnComplete.Visible = False
                        btnUnComplete.Visible = False
                        btnSave.Visible = False
                        lbMessageProcess.Text = ""
                    Else
                        lbStatus.Text = MessageDlg("Status must Posted before compute")
                        Exit Sub
                    End If

                    'do compute
                ElseIf DDL.SelectedValue = "Complete" Then
                    If GVR.Cells(4).Text <> "C" Then
                        lbStatus.Text = MessageDlg("Status must Compute before Complete")
                        Exit Sub
                    End If
                    CekMenu = CheckMenuLevel("Complete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    MovePanel(PnlHd, pnlInput)
                    ViewState("StateHd") = "Complete"
                    ViewState("StartDate") = GVR.Cells(2).Text
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)

                    FillTextBoxHd(GVR.Cells(2).Text)
                    FillCombo(ddlTypeDt, "EXEC S_GetProductType ", False, "ProductType", "TypeName", ViewState("DBConnection"))
                    BindDataDt(GVR.Cells(2).Text, ddlTypeDt.SelectedValue)

                    btnHome.Visible = True
                    btnCompute.Visible = False
                    btnComplete.Visible = True
                    btnComplete.Enabled = True
                    btnUnComplete.Visible = False
                    btnSave.Visible = False
                    lbMessageProcess.Text = ""
                    'do Complete
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "P" Or GVR.Cells(4).Text = "C" Or GVR.Cells(4).Text = "F" Then
                        Exit Sub
                    End If
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    MovePanel(PnlHd, pnlInput)
                    ViewState("StartDate") = GVR.Cells(2).Text

                    FillTextBoxHd(GVR.Cells(2).Text)
                    FillCombo(ddlTypeDt, "EXEC S_GetProductType ", False, "ProductType", "TypeName", ViewState("DBConnection"))

                    tbEndDate.Enabled = True
                    tbStartDate.Enabled = False
                    btnHome.Visible = True
                    btnCompute.Visible = False
                    btnComplete.Visible = False
                    btnUnComplete.Visible = False
                    btnSave.Visible = True
                    lbMessageProcess.Text = ""
                ElseIf DDL.SelectedValue = "Un-Complete" Then
                    If GVR.Cells(4).Text <> "F" Then
                        lbStatus.Text = MessageDlg("Status must Complete before Un-Complete")
                        Exit Sub
                    End If
                    CekMenu = CheckMenuLevel("Un-Complete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    MovePanel(PnlHd, pnlInput)
                    ViewState("StateHd") = "Un-Complete"
                    ViewState("StartDate") = GVR.Cells(2).Text
                    'ModifyInput2(False, pnlInput, pnlDt, GridDt)

                    FillTextBoxHd(GVR.Cells(2).Text)
                    FillCombo(ddlTypeDt, "EXEC S_GetProductType ", False, "ProductType", "TypeName", ViewState("DBConnection"))
                    BindDataDt(GVR.Cells(2).Text, ddlTypeDt.SelectedValue)

                    btnHome.Visible = True
                    btnCompute.Visible = False
                    btnComplete.Visible = False
                    btnUnComplete.Visible = True
                    btnUnComplete.Enabled = True
                    lbMessageProcess.Text = ""
                    btnSave.Visible = False
                    'do Un-Complete
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
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
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal StartDate As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "StartDate = " + QuotedStr(StartDate), ViewState("DBConnection").ToString)
            'newTrans()
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ComputeCOGS(ByVal Langkah As Integer)
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
                'If ViewState("FgType").ToString = "FG" Then
                '    sqlstring = "EXEC S_STProcessCOGSComputeCekFG " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", 0, " + QuotedStr(ViewState("UserId").ToString)
                '    DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

                '    If DS.Tables(0).Rows.Count > 0 Then
                '        pnlInput.Visible = False
                '        pnlView.Visible = True
                '        btnYes.Visible = False
                '        btnNo.Visible = False
                '        btnReturn.Visible = True
                '        ''mpewait.Hide()
                '        lbMessage.Text = "Make sure COGS Material Completed"
                '        GridForView.PageIndex = 0
                '        GridForView.DataSource = DS.Tables(0)
                '        GridForView.DataBind()
                '        Exit Sub
                '    End If

                '    sqlstring = "EXEC S_STProcessCOGSComputeCek3 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                '    ", " + Session("GLYear").ToString + ", " + Session("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString)

                '    DS = SQLExecuteQueryExtend(sqlstring, ViewState("DBConnection"))

                '    If DS.Tables(0).Rows.Count > 0 Then
                '        pnlInput.Visible = False
                '        pnlView.Visible = True
                '        btnYes.Visible = False
                '        btnNo.Visible = False
                '        btnReturn.Visible = True
                '        'mpewait.Hide()
                '        lbMessage.Text = "Make sure this product have Bill of Material"
                '        GridForView.DataSource = DS.Tables(0)
                '        GridForView.DataBind()
                '        Exit Sub
                '    End If
                'End If

                sqlstring = "EXEC S_STProcessCOGSComputeCek " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                 ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString)

                'sqlstring = "SElect * From VMsCustomer"
                DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

                If DS.Tables(0).Rows.Count > 0 Then
                    ViewState("SQLCek") = sqlstring
                    pnlInput.Visible = False
                    pnlView.Visible = True
                    btnYes.Visible = False
                    btnNo.Visible = False
                    btnReturn.Visible = True
                    'mpewait.Hide()
                    lbMessage.Text = "Make sure all transactions above is correct..."
                    GridForView.PageIndex = 0
                    GridForView.DataSource = DS.Tables(0)
                    GridForView.DataBind()
                    btnCompute.Enabled = True
                    btnHome.Enabled = True
                    Exit Sub
                End If
                Langkah = 1
            End If

            If Langkah = 1 Then
                'If ViewState("FgType").ToString = "FG" Then
                '    sqlstring = "EXEC S_STProcessCOGSComputeCekFG " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", 1, " + QuotedStr(ViewState("UserId").ToString)
                '    DS = SQLExecuteQueryExtend(sqlstring, ViewState("DBConnection"))
                '    If DS.Tables(0).Rows.Count > 0 Then
                '        pnlInput.Visible = False
                '        pnlView.Visible = True
                '        btnYes.Visible = True
                '        btnNo.Visible = True
                '        btnReturn.Visible = False
                '        Langkah = 2
                '        ViewState("Langkah") = Langkah
                '        'mpewait.Hide()
                '        lbMessage.Text = "Opname WIP not exists, are you sure to process?"
                '        GridForView.DataSource = DS.Tables(0)
                '        GridForView.DataBind()
                '        Exit Sub
                '    End If
                'End If
                Langkah = 2
                ViewState("Langkah") = Langkah
            End If

            If Langkah = 2 Then
                sqlstring = "EXEC S_STProcessCOGSComputeCek2 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString)

                DS = SQLExecuteQueryExtend(sqlstring, ViewState("DBConnection"))

                If DS.Tables(0).Rows.Count > 0 Then
                    ViewState("SQLCek") = sqlstring
                    pnlInput.Visible = False
                    pnlView.Visible = True
                    btnYes.Visible = True
                    btnNo.Visible = True
                    btnReturn.Visible = False
                    Langkah = 10
                    ViewState("Langkah") = Langkah
                    'mpewait.Hide()
                    lbMessage.Text = "Labor and Overhead Cost have no value, are you sure to process?"
                    GridForView.DataSource = DS.Tables(0)
                    GridForView.DataBind()
                    btnCompute.Enabled = True
                    btnHome.Enabled = True
                    Exit Sub
                End If
                Langkah = 10
                ViewState("Langkah") = Langkah
            End If

            StrMsg = ""
            If Langkah = 10 Then
                'If ViewState("FgType").ToString.ToUpper = "FG" Then
                '    StrMsg = doComputeWIP01()
                '    If StrMsg <> "" Then
                '        'mpewait.Hide()
                '        lbMessageProcess.Text = StrMsg
                '        Exit Sub
                '    End If
                '    StrMsg = doComputeWIP02()
                '    If StrMsg <> "" Then
                '        'mpewait.Hide()
                '        lbMessageProcess.Text = StrMsg
                '        Exit Sub
                '    End If
                '    StrMsg = doComputeWIP03()
                '    If StrMsg <> "" Then
                '        'mpewait.Hide()
                '        lbMessageProcess.Text = StrMsg
                '        Exit Sub
                '    End If
                StrMsg = doComputeRM()
                If StrMsg.Trim <> "" Then
                    'mpewait.Hide()
                    lbMessageProcess.Text = StrMsg
                    btnCompute.Enabled = True
                    btnHome.Enabled = True
                    Exit Sub
                End If
                'End If
                'mpewait.Hide()
                btnCompute.Enabled = True
                btnHome.Enabled = True
                lbMessageProcess.Text = "Success Compute COGS"
            End If
        Catch ex As Exception
            lbStatus.Text = "Compute COGS Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCompute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCompute.Click
        Try
            ViewState("Langkah") = 0
            btnCompute.Enabled = False
            btnHome.Enabled = False
            ComputeCOGS(ViewState("Langkah"))
            BindData(Session("AdvanceFilter"))
            btnCompute.Enabled = True
            btnHome.Enabled = True

        Catch ex As Exception
            lbStatus.Text = "btn Compute Click Error : " + ex.ToString
        End Try
    End Sub

    Function doComputeRM() As String
        Dim sqlclearRAM, sqlstringStartA, sqlstringStartB, sqlstringStartC, sqlstringEndA, sqlstringEndB, sqlstringRM, sqlstringFG As String
        Dim ResultClearRAM, ResultBeginA, ResultBeginB, ResultBeginC, ResultRM, ResultFG, ResultEndA, ResultEndB As String
        Try
            sqlclearRAM = "EXEC ClearCacheRAM "
            sqlstringStartA = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessBeginA " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringStartB = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessBeginB " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringStartC = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessBeginC " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringRM = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessRM " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringFG = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessFG " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringEndA = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessEndA " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringEndB = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessEndB " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            'lbStatus.Text = ViewState("DBConnection")
            'Exit Function
            ResultClearRAM = SQLExecuteScalar(sqlclearRAM, ViewState("DBConnection"))
            If ResultClearRAM.Trim.Length > 5 Then
                Return ResultClearRAM
                btnCompute.Enabled = True
                Exit Function
            End If
            Thread.Sleep(2000)
            ResultBeginA = SQLExecuteScalar(sqlstringStartA, ViewState("DBConnection"))
            If ResultBeginA.Trim.Length > 5 Then
                Return ResultBeginA
                btnCompute.Enabled = True
                Exit Function
            Else
                Thread.Sleep(1000)
                ResultBeginB = SQLExecuteScalar(sqlstringStartB, ViewState("DBConnection"))
                If ResultBeginB.Trim.Length > 5 Then
                    Return ResultBeginB
                    btnCompute.Enabled = True
                    Exit Function
                Else
                    Thread.Sleep(1000)
                    ResultBeginC = SQLExecuteScalar(sqlstringStartC, ViewState("DBConnection"))
                    If ResultBeginC.Trim.Length > 5 Then
                        Return ResultBeginC
                        btnCompute.Enabled = True
                        Exit Function
                    Else
                        Thread.Sleep(5000)
                        ResultRM = SQLExecuteScalar(sqlstringRM, ViewState("DBConnection"))
                        If ResultRM.Trim.Length > 5 Then
                            Return ResultRM
                            btnCompute.Enabled = True
                            Exit Function
                        Else
                            Thread.Sleep(5000)
                            ResultFG = SQLExecuteScalar(sqlstringFG, ViewState("DBConnection"))
                            If ResultFG.Trim.Length > 5 Then
                                Return ResultFG
                                btnCompute.Enabled = True
                                Exit Function
                            Else
                                Thread.Sleep(3000)
                                ResultEndA = SQLExecuteScalar(sqlstringEndA, ViewState("DBConnection"))
                                If ResultEndA.Trim.Length > 5 Then
                                    Return ResultEndA
                                    btnCompute.Enabled = True
                                    Exit Function
                                Else
                                    Thread.Sleep(1000)
                                    ResultEndB = SQLExecuteScalar(sqlstringEndB, ViewState("DBConnection"))
                                    If ResultEndB.Trim.Length > 5 Then
                                        Return ResultEndB
                                        btnCompute.Enabled = True
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            Return ""
        Catch ex As Exception
            Throw New Exception("do Compute Raw Material Error : " + ex.ToString)
        End Try
    End Function

    Function doComputeWIP01() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessWIP01 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If
            Return ""
            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            'Return True
        Catch ex As Exception
            Throw New Exception("do Compute WIP 1 Error : " + ex.ToString)
        End Try
    End Function

    Function doComputeWIP02() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessWIP02 " + +QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If
            Return ""
            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            'Return True
        Catch ex As Exception
            Throw New Exception("do Compute WIP 2 Error : " + ex.ToString)
        End Try
    End Function

    Function doComputeWIP03() As String
        Dim sqlstring, Result As String
        Try
            sqlstring = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSComputeProcessWIP03 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Trim.Length > 1 Then
                Return Result
                Exit Function
            End If
            Return ""
            'btnReturn_Click(Nothing, Nothing)
            'BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate))
            'BindData(Session("AdvanceFilter"))
            'Return True
        Catch ex As Exception
            Throw New Exception("do Compute WIP 3 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            btnHome.Enabled = False
            btnComplete.Enabled = False
            sqlstring = "EXEC S_STProcessCOGSCompleteCek " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString)

            DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

            If DS.Tables(0).Rows.Count > 0 Then
                ViewState("SQLCek") = sqlstring
                pnlDt.Visible = False
                pnlInput.Visible = False
                pnlView.Visible = True
                btnYes.Visible = False
                btnNo.Visible = False
                btnReturn.Visible = True
                lbMessage.Text = "Make sure all transactions above is posted or processed compute"
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
                btnHome.Enabled = True
                Exit Sub
            End If
            doComplete()

            BindData(Session("AdvanceFilter"))
            'S_STProcessCOGSCompleteCek -- ada data dimunculkan, baru di blok 
            'S_STProcessCOGSCompleteProcess()
            btnHome.Enabled = True
            btnComplete.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Complete Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub doComplete()
        Dim sqlclearRAM, sqlstringBegin, sqlstring1, sqlstring2, sqlstring3, sqlstring4, sqlstring5, sqlstring6, sqlstring7, sqlstring8, sqlstring9, sqlstringEnd As String
        Dim ResultBegin, ResultEnd, Result1, Result2, Result3, Result4, Result5, Result6, Result7, Result8, Result9, ResultClear As String

        Try
            sqlclearRAM = "EXEC ClearCacheRAM "

            sqlstringBegin = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteBegin " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring1 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess01 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring2 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess02 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring3 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess03 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring4 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess04 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring5 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess05 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring6 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess06 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring7 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess07 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring8 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess08 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstring9 = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteProcess09 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            sqlstringEnd = "DECLARE @A VARCHAR(255) EXEC S_STProcessCOGSCompleteEnd " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString) + _
            ", @A OUT SELECT @A"

            ResultClear = SQLExecuteScalar(sqlclearRAM, ViewState("DBConnection"))
            If ResultClear.Length > 5 Then
                lbMessageProcess.Text = ResultClear
                btnComplete.Enabled = True
                btnHome.Enabled = True
                'lbMessage.Text = Result
                Exit Sub
            Else
                Thread.Sleep(2000)
                ResultBegin = SQLExecuteScalar(sqlstringBegin, ViewState("DBConnection"))
                If ResultBegin.Length > 5 Then
                    lbMessageProcess.Text = ResultBegin
                    btnComplete.Enabled = True
                    btnHome.Enabled = True
                    'lbMessage.Text = Result
                    Exit Sub
                Else
                    Thread.Sleep(500)
                    Result1 = SQLExecuteScalar(sqlstring1, ViewState("DBConnection"))
                    If Result1.Length > 5 Then
                        lbMessageProcess.Text = Result1
                        btnComplete.Enabled = True
                        btnHome.Enabled = True
                        'lbMessage.Text = Result
                        Exit Sub
                    Else
                        Thread.Sleep(500)
                        Result2 = SQLExecuteScalar(sqlstring2, ViewState("DBConnection"))
                        If Result2.Length > 5 Then
                            lbMessageProcess.Text = Result2
                            btnComplete.Enabled = True
                            btnHome.Enabled = True
                            'lbMessage.Text = Result
                            Exit Sub
                        Else
                            Thread.Sleep(500)
                            Result3 = SQLExecuteScalar(sqlstring3, ViewState("DBConnection"))
                            If Result3.Length > 5 Then
                                lbMessageProcess.Text = Result3
                                btnComplete.Enabled = True
                                btnHome.Enabled = True
                                'lbMessage.Text = Result
                                Exit Sub
                            Else
                                Thread.Sleep(500)
                                Result4 = SQLExecuteScalar(sqlstring4, ViewState("DBConnection"))
                                If Result4.Length > 5 Then
                                    lbMessageProcess.Text = Result4
                                    btnComplete.Enabled = True
                                    btnHome.Enabled = True
                                    'lbMessage.Text = Result
                                    Exit Sub
                                Else
                                    Thread.Sleep(500)
                                    Result5 = SQLExecuteScalar(sqlstring5, ViewState("DBConnection"))
                                    If Result5.Length > 5 Then
                                        lbMessageProcess.Text = Result5
                                        btnComplete.Enabled = True
                                        btnHome.Enabled = True
                                        'lbMessage.Text = Result
                                        Exit Sub
                                    Else
                                        Thread.Sleep(500)
                                        Result6 = SQLExecuteScalar(sqlstring6, ViewState("DBConnection"))
                                        If Result6.Length > 5 Then
                                            lbMessageProcess.Text = Result6
                                            btnComplete.Enabled = True
                                            btnHome.Enabled = True
                                            'lbMessage.Text = Result
                                            Exit Sub
                                        Else
                                            Thread.Sleep(500)
                                            Result7 = SQLExecuteScalar(sqlstring7, ViewState("DBConnection"))
                                            If Result7.Length > 5 Then
                                                lbMessageProcess.Text = Result7
                                                btnComplete.Enabled = True
                                                btnHome.Enabled = True
                                                'lbMessage.Text = Result
                                                Exit Sub
                                            Else
                                                Thread.Sleep(500)
                                                Result8 = SQLExecuteScalar(sqlstring8, ViewState("DBConnection"))
                                                If Result8.Length > 5 Then
                                                    lbMessageProcess.Text = Result8
                                                    btnComplete.Enabled = True
                                                    btnHome.Enabled = True
                                                    'lbMessage.Text = Result
                                                    Exit Sub
                                                Else
                                                    Thread.Sleep(500)
                                                    Result9 = SQLExecuteScalar(sqlstring9, ViewState("DBConnection"))
                                                    If Result9.Length > 5 Then
                                                        lbMessageProcess.Text = Result9
                                                        btnComplete.Enabled = True
                                                        btnHome.Enabled = True
                                                        'lbMessage.Text = Result
                                                        Exit Sub
                                                    Else
                                                        Thread.Sleep(500)
                                                        ResultEnd = SQLExecuteScalar(sqlstringEnd, ViewState("DBConnection"))
                                                        If ResultEnd.Length > 5 Then
                                                            lbMessageProcess.Text = ResultEnd
                                                            btnComplete.Enabled = True
                                                            btnHome.Enabled = True
                                                            'lbMessage.Text = Result
                                                            Exit Sub
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            'btnReturn_Click(Nothing, Nothing)
            'btnComplete.Visible = False
            'BindData(Session("AdvanceFilter"))
            'lbMessage.Text = "Success Complete COGS"
            'btnComplete.Enabled = True
            btnComplete.Visible = False
            btnHome.Enabled = True
            lbMessageProcess.Text = "Success Complete COGS"
        Catch ex As Exception
            Throw New Exception("do Complete Error : " + ex.ToString)
        End Try
    End Sub

    Public Function SQLExecuteQueryExtend(ByRef SQLString As String, ByVal Connection As String) As DataSet
        Dim ds As New DataSet
        Dim Mycon As New SqlConnection
        Dim MyCom As New SqlCommand
        Dim MyDa As New SqlDataAdapter
        Try

            Mycon = New SqlConnection(Connection + ";Connection Timeout=300")
            MyCom = New SqlCommand(SQLString, Mycon)

            MyDa = New SqlDataAdapter(MyCom)
            'MyDa = New SqlDataAdapter(SQLString, Mycon)
            Mycon.Open()
            MyDa.Fill(ds)

            Return ds
        Catch ex As Exception
            Throw New Exception("SQLExecuteQuery EXTEND Error: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SQLString)
        Finally
            If Not Mycon Is Nothing Then Mycon.Dispose()
            If Not MyDa Is Nothing Then MyDa.Dispose()
        End Try
    End Function

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Try
            pnlView.Visible = False
            pnlDt.Visible = True
            pnlInput.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Return Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            pnlView.Visible = False
            pnlDt.Visible = True
            pnlInput.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn No Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            ComputeCOGS(ViewState("Langkah"))
        Catch ex As Exception
            lbStatus.Text = "btn Yes Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            BindDataDt(Format("dd MMM yyyy", tbStartDate.SelectedDate), ddlTypeDt.SelectedValue)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Page Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridForView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridForView.PageIndexChanging
        Dim sqlstring As String
        Dim DS As DataSet
        Try
            If Not ViewState("SQLCek") Is Nothing Then
                GridForView.PageIndex = e.NewPageIndex
                sqlstring = ViewState("SQLCek")
                DS = SQLExecuteQuery(sqlstring, ViewState("DBConnection").ToString)
                GridForView.DataSource = DS.Tables(0)
                GridForView.DataBind()
            End If
        Catch ex As Exception
            lbStatus.Text = "grid for view page index changing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlTypeDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeDt.SelectedIndexChanged
        BindDataDt(ViewState("StartDate"), ddlTypeDt.SelectedValue)
    End Sub

    Protected Sub btnUnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnComplete.Click
        Dim sqlclearRAM, sqlstring01, sqlstring02, sqlstring03, sqlstring04, sqlstring05, sqlstring06, sqlstring07, sqlstring08, sqlstring09 As String
        Dim ResultClear, Result1, Result2, Result3, Result4, Result5, Result6, Result7, Result8, Result9 As String
        Try
            btnUnComplete.Enabled = False
            btnHome.Enabled = False
            sqlclearRAM = "EXEC ClearCacheRAM "

            sqlstring01 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess01 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring02 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess02 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring03 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess03 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring04 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess04 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring05 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess05 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring06 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess06 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring07 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess07 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring08 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess08 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            sqlstring09 = "DECLARE @A VarChar(255)  EXEC S_STProcessCOGSCompleteUnProcess09 " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyyMMdd")) + ", @A Out SELECT @A AS Msg"

            ResultClear = SQLExecuteScalar(sqlclearRAM, ViewState("DBConnection"))
            If ResultClear.Length > 5 Then
                lbMessage.Text = ResultClear
                btnUnComplete.Enabled = True
                btnHome.Enabled = True
                Exit Sub
            Else
                Thread.Sleep(500)
                Result1 = SQLExecuteScalar(sqlstring01, ViewState("DBConnection"))
                If Result1.Length > 5 Then
                    lbMessage.Text = Result1
                    btnUnComplete.Enabled = True
                    btnHome.Enabled = True
                    Exit Sub
                Else
                    Thread.Sleep(500)
                    Result2 = SQLExecuteScalar(sqlstring02, ViewState("DBConnection"))
                    If Result2.Length > 5 Then
                        lbMessage.Text = Result2
                        btnUnComplete.Enabled = True
                        btnHome.Enabled = True
                        Exit Sub
                    Else
                        Thread.Sleep(500)
                        Result3 = SQLExecuteScalar(sqlstring03, ViewState("DBConnection"))
                        If Result3.Length > 5 Then
                            lbMessage.Text = Result3
                            btnUnComplete.Enabled = True
                            btnHome.Enabled = True
                            Exit Sub
                        Else
                            Thread.Sleep(500)
                            Result4 = SQLExecuteScalar(sqlstring04, ViewState("DBConnection"))
                            If Result4.Length > 5 Then
                                lbMessage.Text = Result4
                                btnUnComplete.Enabled = True
                                btnHome.Enabled = True
                                Exit Sub
                            Else
                                Thread.Sleep(500)
                                Result5 = SQLExecuteScalar(sqlstring05, ViewState("DBConnection"))
                                If Result5.Length > 5 Then
                                    lbMessage.Text = Result5
                                    btnUnComplete.Enabled = True
                                    btnHome.Enabled = True
                                    Exit Sub
                                Else
                                    Thread.Sleep(500)
                                    Result6 = SQLExecuteScalar(sqlstring06, ViewState("DBConnection"))
                                    If Result6.Length > 5 Then
                                        lbMessage.Text = Result6
                                        btnUnComplete.Enabled = True
                                        btnHome.Enabled = True
                                        Exit Sub
                                    Else
                                        Thread.Sleep(500)
                                        Result7 = SQLExecuteScalar(sqlstring07, ViewState("DBConnection"))
                                        If Result7.Length > 5 Then
                                            lbMessage.Text = Result7
                                            btnUnComplete.Enabled = True
                                            btnHome.Enabled = True
                                            Exit Sub
                                        Else
                                            Thread.Sleep(500)
                                            Result8 = SQLExecuteScalar(sqlstring08, ViewState("DBConnection"))
                                            If Result8.Length > 5 Then
                                                lbMessage.Text = Result8
                                                btnUnComplete.Enabled = True
                                                btnHome.Enabled = True
                                                Exit Sub
                                            Else
                                                Thread.Sleep(500)
                                                Result9 = SQLExecuteScalar(sqlstring09, ViewState("DBConnection"))
                                                If Result9.Length > 5 Then
                                                    lbMessage.Text = Result9
                                                    btnUnComplete.Enabled = True
                                                    btnHome.Enabled = True
                                                    Exit Sub
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            btnReturn_Click(Nothing, Nothing)
            btnHome.Enabled = True
            'btnUnComplete.Enabled = True
            btnUnComplete.Visible = False
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "btn Complete Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    '    'mpewait.Hide()
    '    Timer1.Enabled = False
    'End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Dim SqlString As String
        Dim DS As DataSet
        Dim dr As DataRow
        Try
            DS = SQLExecuteQuery("EXEC S_STProcessCOGSGetStart ", ViewState("DBConnection").ToString)
            dr = DS.Tables(0).Rows(0)

            If dr("StartDate").ToString.Contains("1960") Then
                lbStatus.Text = MessageDlg("Cannot input new process, please complete last process first.")
                Exit Sub
            End If
            SqlString = "Insert INTO STCProcessHd (StartDate, EndDate, Status) SELECT " + QuotedStr(Format(CDate(dr("StartDate").ToString), "yyyy-MM-dd")) + "," + _
            QuotedStr(Format(CDate(dr("EndDate").ToString), "yyyy-MM-dd")) + ", 'H'"

            SQLExecuteNonQuery(SqlString, ViewState("DBConnection"))
            BindData("")
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "4,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_STProcessCOGS", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("StartDate in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            pnlInput.Visible = False
            PnlHd.Visible = True
            'btnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Home Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String
        Try
            SQLString = "Update STCProcessHd SET EndDate = " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + _
            " WHERE StartDate = " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"))
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            btnHome_Click(Nothing, Nothing)
            btnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
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
        Dim Result As String
        Try

            Result = ReportGrid.ResultString

            SQLString = "EXEC S_STProcessCOGSComputeCek " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                 ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + ", " + QuotedStr(ViewState("UserId").ToString)

            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("ProcessCOGS")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class