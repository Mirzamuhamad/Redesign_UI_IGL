Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrMTNWork_TrMTNWork
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, Revisi, TransDate, Status, ReffNmbr, Department, Dept_Name, RequestBy, RequestDate, FgOutSource, OutSourceTo, Remark, UserPrep, DatePrep, UserAppr, DateAppr, FgActive From V_MNWorkHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                ddlRow.SelectedValue = "15"
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnMTNItem" Then
                    BindToText(tbMTNItemCode, Session("Result")(1).ToString)
                    BindToText(tbMTNItemName, Session("Result")(2).ToString)
                    BindToText(tbJobCode, Session("Result")(3).ToString)
                    BindToText(tbJobName, Session("Result")(4).ToString)
                    tbJobName.Enabled = tbJobName.Text = ""
                End If
                If ViewState("Sender") = "btnSparepart" Then
                    BindToText(tbSparepartCode, Session("Result")(0).ToString)
                    BindToText(tbSparepartName, Session("Result")(1).ToString)
                    BindToText(tbUnit, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    'Dim i As Integer

                    'i = 1
                    For Each drResult In Session("Result").Rows
                        Dim dr As DataRow
                        'ResultField = "MTN_Code, MTN_Name, PIC, Job_Code, Job_Name, JobDescription, Schedule"
                        dr = ViewState("Dt").NewRow
                        dr("ItemNo") = GetNewItemNo(ViewState("Dt"))
                        dr("MTNItem") = drResult("MTN_Code")
                        dr("MTN_Item_Name") = drResult("MTN_Name")
                        dr("Job") = TrimStr(drResult("Job_Code").ToString)
                        dr("JobName") = drResult("Job_Name")
                        dr("Problem") = ""
                        dr("PIC") = drResult("PIC")
                        dr("Remark") = ""
                        ViewState("Dt").Rows.Add(dr)
                        FirstTime = False
                        'i = i + 1
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
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
            FillRange(ddlRange)
            FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))
            FillCombo(ddlYear1, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlYear2, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            tbOutsource.Enabled = ddlOutsource.SelectedValue = "Y"

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
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
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()            
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MNWorkDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MNWorkPart WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status, msg As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If
            If (ActionValue = "Print") Or (ActionValue = "Print Full") Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                If (ActionValue = "Print") Then
                    Session("SelectCommand") = "EXEC S_FNFormBuktiBankdt" + Result + ",'PAYMENTTRADE'," + QuotedStr(ViewState("UserId"))
                    Session("ReportFile") = ".../../../Rpt/FormPayTrade.frx"
                    'lbStatus.Text = Session("SelectCommand")
                    'Exit Sub
                Else
                    Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt " + Result + ",'PAYMENTTRADE'," + QuotedStr(ViewState("UserId"))
                    Session("ReportFile") = ".../../../Rpt/FormPayTrade.frx"
                    'lbStatus.Text = Session("SelectCommand")
                    'Exit Sub
                End If

                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                msg = ""

                GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, msg)
                

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_MTNWork", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                'BindData("TransNmbr in (" + ListSelectNmbr + ")")
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbReffNmbr.Enabled = State
            ddlDept.Enabled = State
            'tbRequestBy.Enabled = State
            'tbRequestDate.Enabled = State
            'ddlOutsource.Enabled = State
            'tbOutsource.Enabled = ddlOutsource.
            'tbSuppCode.Enabled = State  'ViewState("StateHd") = "Insert" And Count = 0
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)

        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            'Dim dr As DataRow
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt

            'If GetCountRecord(ViewState("Dt2")) > 0 Then
            '    dr = dt.Rows(0)
            '    ViewState("ProductType") = dr("ProductType").ToString
            'End If
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            lbRevisi.Text = "0"
            ddlDept.SelectedIndex = 0
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbReffNmbr.Text = ""
            tbRequestBy.Text = ""
            tbRequestDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlOutsource.SelectedValue = "N"
            tbOutsource.Text = ""
            tbOutsource.Enabled = False
            tbRemark.Text = ""
            ddlYear1.SelectedValue = ViewState("GLYear")
            ddlYear2.SelectedValue = ViewState("GLYear")
            ddlMonth1.SelectedValue = ViewState("GLPeriod")
            ddlMonth2.SelectedValue = ViewState("GLPeriod")
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbMTNItemCode.Text = ""
            tbMTNItemName.Text = ""
            tbJobCode.Text = ""
            tbJobName.Text = ""
            tbProblem.Text = ""
            tbPIC.Text = ""
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbSparepartCode.Text = ""
            tbSparepartName.Text = ""
            tbQty.Text = "0"
            tbUnit.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If ddlDept.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
                Return False
            End If
            If tbRequestBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Request By must have value")
                tbRequestBy.Focus()
                Return False
            End If
            If ddlOutsource.SelectedValue.Trim = "Y" And tbOutsource.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Outsource must have value")
                tbOutsource.Focus()
                Return False
            End If
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("MTN_Item_Name").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Maintenance Must Have Value")
                    tbMTNItemCode.Focus()
                    Return False
                End If
                If (Dr("Job").ToString <> "") And (Dr("JobName").ToString = "") Then
                    lbStatus.Text = MessageDlg("Job Must Have Value")
                    tbJobCode.Focus()
                    Return False
                End If
            Else                
                If tbMTNItemName.Text = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Must Have Value")
                    tbMTNItemCode.Focus()
                    Return False
                End If
                If (tbJobCode.Text <> "") And (tbJobName.Text = "") Then
                    lbStatus.Text = MessageDlg("Job Must Have Value")
                    tbJobCode.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Qty").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
            Else                
                If CFloat(tbQty.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            'If tbFgMode.Text = "G" Then
            '    If CekExistGiroOut(tbDocumentNo.Text.Trim, ViewState("DBConnection").ToString) = True Then
            '        lbStatus.Text = "Giro Payment '" + tbDocumentNo.Text.Trim + "' has already exists in Giro Listing'"
            '        Exit Sub
            '    End If
            'End If
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("MTNItem") = tbMTNItemCode.Text
                Row("MTN_Item_Name") = tbMTNItemName.Text
                Row("Job") = tbJobCode.Text
                Row("JobName") = tbJobName.Text
                Row("Problem") = tbProblem.Text
                Row("PIC") = tbPIC.Text
                Row("Remark") = tbRemarkDt.Text                
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("MTNItem") = tbMTNItemCode.Text
                dr("MTN_Item_Name") = tbMTNItemName.Text
                dr("Job") = tbJobCode.Text
                dr("JobName") = tbJobName.Text
                dr("Problem") = tbProblem.Text
                dr("PIC") = tbPIC.Text
                dr("Remark") = tbRemarkDt.Text
                
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt2").Select("Sparepart = " + QuotedStr(tbSparepartCode.Text))
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("Sparepart = " + QuotedStr(ViewState("Sparepart")))(0)

                Row.BeginEdit()
                Row("Sparepart") = tbSparepartCode.Text
                Row("Sparepart_Name") = tbSparepartName.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = tbUnit.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If
                dr = ViewState("Dt2").NewRow
                dr("Sparepart") = tbSparepartCode.Text
                dr("Sparepart_Name") = tbSparepartName.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = tbUnit.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub


    Private Function AllowedRecordDt2() As Integer
        Try
            If ViewState("Sparepart") = tbSparepartCode.Text.Trim Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("MWO", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                'FORMAT(A.TotalCharge, DigitCurrHome()) AS TotalCharge, A.Remark
                SQLString = "INSERT INTO MTNWorkHd (TransNmbr, TransDate, STATUS, Revisi, " + _
                "Department, RequestBy, RequestDate, FgOutSource, OutSourceTo, " + _
                "Remark, FgActive, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                lbRevisi.Text + ", " + QuotedStr(ddlDept.SelectedValue) + ", " + QuotedStr(tbRequestBy.Text) + _
                ", " + QuotedStr(Format(tbRequestDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlOutsource.SelectedValue) + _
                ", " + QuotedStr(tbOutsource.Text) + ", " + QuotedStr(tbRemark.Text) + ",'Y'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MTNWorkHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " AND Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                'lbStatus.Text = "Select Status FROM MTNWorkHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " AND Revisi = " + lbRevisi.Text
                'Exit Sub
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNWorkHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Department = " + QuotedStr(ddlDept.SelectedValue) + ", RequestBy = " + QuotedStr(tbRequestBy.Text) + _
                ", RequestDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", FgOutSource = " + QuotedStr(ddlOutsource.SelectedValue) + _
                ", OutSourceTo = " + QuotedStr(tbOutsource.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If

            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr is NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = lbRevisi.Text
                Row(I).EndEdit()
            Next

            If Not ViewState("Dt2") Is Nothing Then
                Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbCode.Text
                    Row(I)("Revisi") = lbRevisi.Text
                    Row(I).EndEdit()
                Next
            End If
            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()

            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, ItemNo, MTNItem, Job, JobName, Problem, PIC, Remark FROM MTNWorkDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MTNWorkDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            If Not ViewState("Dt2") Is Nothing Then
                'save dt2
                cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, Sparepart, Qty, Unit FROM MTNWorkPart WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi"), con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim Dt2 As New DataTable("MTNWorkPart")

                Dt2 = ViewState("Dt2")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("Dt2") = Dt2
            End If
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("", 0)
            BindDataDt2("", 0)
            pnlSchedule.Visible = True
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Request Date"
            FDateValue = "TransDate, RequestDate"
            FilterName = "MO No, MO Date, Status, Reff. Nmbr, Department, Request By, Request Date, Fg OutSource, OutSource To, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, ReffNmbr, Dept_Name, RequestBy, dbo.FormatDate(RequestDate), FgOutSource, OutSourceTo, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
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
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    tbOutsource.Enabled = False
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    pnlSchedule.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                        pnlSchedule.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf (DDL.SelectedValue = "Print") Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")

                        Session("SelectCommand") = "EXEC S_MTNWorkForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("SelectCommand2") = "EXEC S_MTNWorkFormPart " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + "," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormMTNWork.frx"

                        AttachScript("openprintdlg2ds2();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, Value As String
                    'klik

                    SqlString = "Declare @A VarChar(255) EXEC S_MTNWorkCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(HiddenRemarkRevisi.Value) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    AttachScript("revisi();", Page, Me.GetType)

                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    'If Result.Length > 2 Then
                    '    lbStatus.Text = MessageDlg(Result)
                    'Else
                    '    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    'End If
                    Value = ddlField.SelectedValue
                    tbFilter.Text = tbCode.Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    ddlField.SelectedValue = Value

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


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), ExistRow() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'ExistRow = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            'If ExistRow.Length > 0 Then
            '    lbStatus.Text = MessageDlg("Detail Invoice Exist, cannot delete data")
            '    Exit Sub
            'End If
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)


        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Sparepart = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToText(tbReffNmbr, Dt.Rows(0)("ReffNmbr").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbRequestBy, Dt.Rows(0)("RequestBy").ToString)
            BindToDate(tbRequestDate, Dt.Rows(0)("RequestDate").ToString)
            BindToDropList(ddlOutsource, Dt.Rows(0)("FgOutSource").ToString)
            BindToText(tbOutsource, Dt.Rows(0)("OutSourceTo").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            lbRevisi.Text = Dt.Rows(0)("Revisi").ToString
            If ddlOutsource.SelectedValue = "Y" Then
                tbOutsource.Enabled = True
            Else
                tbOutsource.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                BindToText(tbMTNItemCode, Dr(0)("MTNItem").ToString)
                BindToText(tbMTNItemName, Dr(0)("MTN_Item_Name").ToString)
                BindToText(tbJobCode, Dr(0)("Job").ToString)
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbProblem, Dr(0)("Problem").ToString)
                BindToText(tbPIC, Dr(0)("PIC").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal Sparepart As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Sparepart =" + QuotedStr(Sparepart.ToString))
            If Dr.Length > 0 Then
                BindToText(tbSparepartCode, Dr(0)("Sparepart").ToString)
                BindToText(tbSparepartName, Dr(0)("Sparepart_Name").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Sparepart") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbSparepartCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDtke2.Click, btnAdddt.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbMTNItemCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn saveall Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMTNItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMTNItem.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT MTN_Type_Name, MTN_Item, MTN_Item_Name, Job, JobName FROM V_MTNWorkGetItemJob"
            ResultField = "MTN_Type_Name, MTN_Item, MTN_Item_Name, Job, JobName"
            ViewState("Sender") = "btnMTNItem"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMTNItem Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMTNItemCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMTNItemCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT MTN_Item, MTN_Item_Name, Job, JobName FROM V_MTNWorkGetItemJob WHERE MTN_Item = " + QuotedStr(tbMTNItemCode.Text), ViewState("DBConnection")).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbMTNItemCode, Dr("MTN_Item").ToString)
                BindToText(tbMTNItemName, Dr("MTN_Item_Name").ToString)
                BindToText(tbJobCode, Dr("Job").ToString)
                BindToText(tbJobName, Dr("JobName").ToString)
                tbMTNItemCode.Focus()
            Else
                tbMTNItemCode.Text = ""
                tbMTNItemName.Text = ""
                tbJobCode.Text = ""
                tbJobName.Text = ""
                tbMTNItemCode.Focus()
            End If
            tbJobName.Enabled = tbJobCode.Text = ""
        Catch ex As Exception
            Throw New Exception("tbMTNItemCode change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            Dim dr As DataRow            
            Dim SQLString, lbCode As String
            lbCode = ViewState("UserId") + "-" + Format(Now, "yyMMdd-HHmm")
            SQLString = "DELETE FROM MTNWorkTemp WHERE Code = " + QuotedStr(lbCode)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            For Each dr In ViewState("Dt").Rows
                SQLString = "INSERT INTO MTNWorkTemp (Code, MTNItem, Job)" + _
                "SELECT " + QuotedStr(lbCode) + "," + QuotedStr(dr("MTNItem").ToString) + "," + QuotedStr(dr("Job").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            Next

            Dim dt As DataTable
            Dim drResult As DataRow
            Dim Row As DataRow()

            dt = SQLExecuteQuery("EXEC S_MTNWorkGetSparePart " + QuotedStr(lbCode), ViewState("DBConnection").ToString).Tables(0)

            For Each drResult In dt.Rows
                Row = ViewState("Dt2").Select("SparePart = " + QuotedStr(drResult("SparePart")))

                If Row.Count = 0 Then
                    dr = ViewState("Dt2").NewRow
                    dr("Sparepart") = drResult("Sparepart")
                    dr("Sparepart_Name") = drResult("Sparepart_Name")
                    dr("Qty") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                    dr("Unit") = drResult("Unit")
                    ViewState("Dt2").Rows.Add(dr)
                Else
                    dr = ViewState("Dt2").Select("SparePart = " + QuotedStr(drResult("SparePart")))
                    dr.BeginEdit()
                    If drResult("Qty") <> dr("Qty") Then
                        dr("Qty") = dr("Qty") + CFloat(drResult("Qty"))
                    End If
                End If
            Next

            BindGridDt(ViewState("Dt2"), GridDt2)
        Catch ex As Exception
            Throw New Exception("btnGenerate_Click change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlOutsource_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOutsource.TextChanged
        tbOutsource.Enabled = ddlOutsource.Text = "Y"
        tbOutsource.Text = ""
    End Sub

    Protected Sub tbJobCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbJobCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Job, JobName FROM V_MTNWorkGetItemJob WHERE MTN_Item = " + QuotedStr(tbMTNItemCode.Text) + " AND Job = " + QuotedStr(tbJobCode.Text), ViewState("DBConnection")).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbJobCode, Dr("Job").ToString)
                BindToText(tbJobName, Dr("JobName").ToString)
                tbJobCode.Focus()
            Else
                tbJobCode.Text = ""
                tbJobName.Text = ""
                tbJobCode.Focus()
            End If
            tbJobName.Enabled = tbJobName.Text = ""
        Catch ex As Exception
            Throw New Exception("tbJobCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSparepart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSparepart.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MTNWorkGetSparepart"
            ResultField = "Product_Code, Product_Name, Unit"
            ViewState("Sender") = "btnSparepart"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSparepart Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSparepartCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSparepartCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT * FROM V_MTNWorkGetSparepart WHERE Product_Code = " + QuotedStr(tbSparepartCode.Text), ViewState("DBConnection")).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbSparepartCode, Dr("Product_Code").ToString)
                BindToText(tbSparepartName, Dr("Product_Name").ToString)
                BindToText(tbUnit, Dr("Unit").ToString)
                tbSparepartCode.Focus()
            Else
                tbSparepartCode.Text = ""
                tbSparepartName.Text = ""
                tbUnit.Text = ""
                tbSparepartCode.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbSparepartCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField, Filter As String
        Dim CriteriaField As String
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("Result") = Nothing
            Filter = ""
            Session("Filter") = "EXEC S_MTNWorkGetSchedule " + ddlYear1.SelectedValue + "," + ddlMonth1.SelectedValue + "," + ddlWeek1.SelectedValue + "," + ddlYear2.SelectedValue + "," + ddlMonth2.SelectedValue + "," + ddlWeek2.SelectedValue
            ResultField = "MTN_Code, MTN_Name, PIC, Job_Code, Job_Name, JobDescription, Schedule"
            CriteriaField = "MTN_Code, MTN_Name, PIC, Job_Code, Job_Name, JobDescription, Schedule"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "ddlRow_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
