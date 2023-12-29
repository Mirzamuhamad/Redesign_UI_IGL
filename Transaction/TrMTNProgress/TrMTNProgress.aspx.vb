
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrMTNProgress_TrMTNProgress
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_MTNProgressHd"

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
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnMONo" Then
                    'ResultField = "MO_No, MO_Date, Machine, Machine_Name, FgSubkon, ProjectManager, ProjectManager_Name"
                    tbMONo.Text = Session("Result")(0).ToString
                    BindToText(tbMachineCode, Session("Result")(2).ToString)
                    BindToText(tbMachineName, Session("Result")(3).ToString)
                    If Session("Result")(4).ToString = "Y" Then
                        ddlPractitioner.SelectedIndex = 0
                    Else : ddlPractitioner.SelectedIndex = 1
                    End If
                    BindToText(tbProjectManager, Session("Result")(5).ToString)
                    BindToText(tbProjectManagerName, Session("Result")(6).ToString)
                End If
                If ViewState("Sender") = "btnReviewedBy" Then
                    BindToText(tbReviewedBy, Session("Result")(0).ToString)
                    BindToText(tbReviewedByName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnAcknowledgeBy1" Then
                    BindToText(tbAcknowledgeBy1, Session("Result")(0).ToString)
                    BindToText(tbAcknowledgeBy1Name, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnAcknowledgeBy2" Then
                    BindToText(tbAcknowledgeBy2, Session("Result")(0).ToString)
                    BindToText(tbAcknowledgeBy2Name, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaintenanceItem" Then
                    'ResultField = "MaintenanceItem, MaintenanceItem_Name, Job, Job_Name, ItemSpec, JobDescription, Remark, ItemQty"
                    BindToText(tbMaintenanceItem, Session("Result")(0).ToString)
                    BindToText(tbMaintenanceItemName, Session("Result")(1).ToString)
                    BindToText(tbJob, Session("Result")(2).ToString)
                    BindToText(tbJobName, Session("Result")(3).ToString)
                    BindToText(tbItemSpec, Session("Result")(4).ToString)
                    BindToText(tbJobDescription, Session("Result")(5).ToString)
                    BindToText(tbRemark, Session("Result")(6).ToString)
                    BindToText(tbItemQty, Session("Result")(7).ToString)
                End If
                If ViewState("Sender") = "btnPIC" Then
                    BindToText(tbPICCode, Session("Result")(0).ToString)
                    BindToText(tbPICName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterial" Then
                    BindToText(tbMaterial, Session("Result")(0).ToString)
                    BindToText(tbMaterialName, Session("Result")(1).ToString)
                    BindToText(tbQtyDt3, Session("Result")(3).ToString)
                    BindToDropList(ddlUnit, Session("Result")(4).ToString)
                End If
                Session("filter") = Nothing
                Session("Column") = Nothing
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
            Else
                PnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            'FillCombo(ddlMaintenanceItem, "Select ItemNo, ItemName FROM MsMaintenanceItem ", True, "ItemNo", "ItemName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbQtyDt3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyDt3.Attributes.Add("OnBlur", "setformatdt();")

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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_MTNProgressDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_MTNProgressDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_MTNProgressDt3 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
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
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_MTNProgress", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")

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
            tbDate.Enabled = State
            btnMONo.Visible = State
            tbReviewedBy.Enabled = State
            btnReviewedBy.Visible = State
            tbAcknowledgeBy1.Enabled = State
            btnAcknowledgeBy1.Visible = State
            tbAcknowledgeBy2.Enabled = State
            btnAcknowledgeBy2.Visible = State
            tbRemark.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbMONo.Text = ""
            tbMachineCode.Text = ""
            tbMachineName.Text = ""
            ddlPractitioner.SelectedIndex = 0
            tbProjectManager.Text = ""
            tbProjectManagerName.Text = ""
            tbReviewedBy.Text = ""
            tbReviewedByName.Text = ""
            tbAcknowledgeBy1.Text = ""
            tbAcknowledgeBy1Name.Text = ""
            tbAcknowledgeBy2.Text = ""
            tbAcknowledgeBy2Name.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbMaintenanceItem.Text = ""
            tbMaintenanceItemName.Text = ""
            tbItemSpec.Text = ""
            tbItemQty.Text = ""
            'tbExplanation.Text = ""
            tbJob.Text = ""
            tbJobName.Text = ""
            tbJobDescription.Text = ""
            ddlPriority.SelectedValue = ""
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbStartTime.Text = "00:00"
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbEndTime.Text = "00:00"
            tbProgress.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbPICCode.Text = ""
            tbPICName.Text = ""
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            FillMaintenanceItem()
            ddlMaintenanceItem.SelectedValue = ""
            tbMaterial.Text = ""
            tbMaterialName.Text = ""
            tbQtyDt3.Text = "0"
            ddlUnit.SelectedValue = ""
            tbRemarkDt3.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbMONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("MO No must have value")
                tbMONo.Focus()
                Return False
            End If
            If tbReviewedByName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Reviewed By must have value")
                tbReviewedBy.Focus()
                Return False
            End If
            If tbAcknowledgeBy1Name.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Acknowledge By 1 must have value")
                tbAcknowledgeBy1.Focus()
                Return False
            End If
            If tbAcknowledgeBy2Name.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Acknowledge By 2 must have value")
                tbAcknowledgeBy2.Focus()
                Return False
            End If

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
                If Dr("MaintenanceItem_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    Return False
                End If
                'If Dr("StartDate").Then Then
                '    lbStatus.Text = MessageDlg("Start Date Must Have Value")
                '    Return False
                'End If
                'If Dr("EndDate").Then Then
                '    lbStatus.Text = MessageDlg("End Date Must Have Value")
                '    Return False
                'End If
                If Dr("StartTime").ToString = "" Then
                    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                    Return False
                End If
                If Dr("EndTime").ToString = "" Then
                    lbStatus.Text = MessageDlg("End Time Must Have Value")
                    Return False
                End If
            Else
                If tbMaintenanceItemName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    tbMaintenanceItem.Focus()
                    Return False
                End If
                If tbStartDate.IsNull Then
                    lbStatus.Text = MessageDlg("Start Date Must Have Value")
                    tbStartDate.Focus()
                    Return False
                End If
                If tbStartTime.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                    tbStartTime.Focus()
                    Return False
                End If
                If tbEndDate.IsNull Then
                    lbStatus.Text = MessageDlg("End Date Must Have Value")
                    tbEndDate.Focus()
                    Return False
                End If
                If tbEndTime.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("End Time Must Have Value")
                    tbEndTime.Focus()
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
                If Dr("PICName").ToString = "" Then
                    lbStatus.Text = MessageDlg("PIC Must Have Value")
                    Return False
                End If

            Else
                If tbPICName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("PIC Code Must Have Value")
                    tbPICCode.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbMONo, Dt.Rows(0)("MONO").ToString)
            BindToText(tbMachineCode, Dt.Rows(0)("Machine").ToString)
            BindToText(tbMachineName, Dt.Rows(0)("Machine_Name").ToString)
            BindToDropList(ddlPractitioner, Dt.Rows(0)("Practitioner").ToString)
            BindToText(tbProjectManager, Dt.Rows(0)("ProjectManager").ToString)
            BindToText(tbProjectManagerName, Dt.Rows(0)("ProjectManager_Name").ToString)
            BindToText(tbReviewedBy, Dt.Rows(0)("ReviewedBy").ToString)
            BindToText(tbReviewedByName, Dt.Rows(0)("ReviewedBy_Name").ToString)
            BindToText(tbAcknowledgeBy1, Dt.Rows(0)("AcknowledgeBy1").ToString)
            BindToText(tbAcknowledgeBy1Name, Dt.Rows(0)("AcknowledgeBy1_Name").ToString)
            BindToText(tbAcknowledgeBy2, Dt.Rows(0)("AcknowledgeBy2").ToString)
            BindToText(tbAcknowledgeBy2Name, Dt.Rows(0)("AcknowledgeBy2_Name").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("MaintenanceItem = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbMaintenanceItem, Dr(0)("MaintenanceItem").ToString)
                BindToText(tbMaintenanceItemName, Dr(0)("MaintenanceItem_Name").ToString)
                BindToText(tbJob, Dr(0)("Job").ToString)
                BindToText(tbJobName, Dr(0)("Job_Name").ToString)
                BindToText(tbItemSpec, Dr(0)("ItemSpec").ToString)
                BindToText(tbJobDescription, Dr(0)("JobDescription").ToString)
                'BindToText(tbExplanation, Dr(0)("Explanation").ToString)
                BindToDropList(ddlPriority, Dr(0)("Priority").ToString)
                BindToText(tbItemQty, Dr(0)("ItemQty").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbStartTime, Dr(0)("StartTime").ToString)
                BindToText(tbEndTime, Dr(0)("EndTime").ToString)
                BindToText(tbProgress, Dr(0)("Progress").ToString)
                BindToText(tbRemark, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("PIC = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbPICCode, Dr(0)("PIC").ToString)
                BindToText(tbPICName, Dr(0)("PICName").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("MaintenanceItem = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                FillMaintenanceItem()
                BindToDropList(ddlMaintenanceItem, Dr(0)("MaintenanceItem").ToString)
                BindToText(tbMaterial, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("Material_Name").ToString)
                BindToText(tbQtyDt3, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt3, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("MaintenanceItem = " + QuotedStr(tbMaintenanceItem.Text))(0)
                Row.BeginEdit()
                Row("MaintenanceItem") = tbMaintenanceItem.Text
                Row("MaintenanceItem_Name") = tbMaintenanceItemName.Text
                Row("Job") = tbJob.Text
                Row("Job_Name") = tbJobName.Text
                Row("ItemSpec") = tbItemSpec.Text
                Row("JobDescription") = tbJobDescription.Text
                'Row("Explanation") = tbExplanation.Text
                Row("Priority") = ddlPriority.SelectedValue
                Row("ItemQty") = tbItemQty.Text
                Row("StartDate") = tbStartDate.SelectedDate
                Row("StartTime") = tbStartTime.Text
                Row("EndDate") = tbEndDate.SelectedDate
                Row("EndTime") = tbEndTime.Text
                Row("Progress") = tbProgress.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("MaintenanceItem") = tbMaintenanceItem.Text
                dr("MaintenanceItem_Name") = tbMaintenanceItemName.Text
                dr("Job") = tbJob.Text
                dr("Job_Name") = tbJobName.Text
                dr("ItemSpec") = tbItemSpec.Text
                dr("JobDescription") = tbJobDescription.Text
                'dr("Explanation") = tbExplanation.Text
                dr("Priority") = ddlPriority.SelectedValue
                dr("ItemQty") = tbItemQty.Text
                dr("StartDate") = tbStartDate.SelectedDate
                dr("StartTime") = tbStartTime.Text
                dr("EndDate") = tbEndDate.SelectedDate
                dr("EndTime") = tbEndTime.Text
                dr("Progress") = tbProgress.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("PIC = " + QuotedStr(tbPICCode.Text))(0)
                Row.BeginEdit()
                Row("PIC") = tbPICCode.Text
                Row("PICName") = tbPICName.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("PIC") = tbPICCode.Text
                dr("PICName") = tbPICName.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

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
                tbCode.Text = GetAutoNmbr("MP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO MTNProgressHd (TransNmbr, TransDate, STATUS, " + _
                "MONo, Machine, Practitioner, ProjectManager, ReviewedBy, AcknowledgeBy1, AcknowledgeBy2, Remark, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbMONo.Text) + "," + QuotedStr(tbMachineCode.Text) + "," + QuotedStr(ddlPractitioner.SelectedValue) + "," + _
                QuotedStr(tbProjectManager.Text) + "," + QuotedStr(tbReviewedBy.Text) + "," + _
                QuotedStr(tbAcknowledgeBy1.Text) + "," + QuotedStr(tbAcknowledgeBy2.Text) + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MTNProgressHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNProgressHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", MONo = " + QuotedStr(tbMONo.Text) + ", Machine = " + QuotedStr(tbMachineCode.Text) + _
                ", Practitioner = " + QuotedStr(ddlPractitioner.SelectedValue) + _
                ", ProjectManager = " + QuotedStr(tbProjectManager.Text) + ", ReviewedBy = " + QuotedStr(tbReviewedBy.Text) + _
                ", AcknowledgeBy1 = " + QuotedStr(tbAcknowledgeBy1.Text) + ", AcknowledgeBy2 = " + QuotedStr(tbAcknowledgeBy2.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, MaintenanceItem, Job, ItemSpec, JobDescription, ItemQty, Priority, StartDate, StartTime, EndDate, EndTime, Progress, Remark FROM MTNProgressDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MTNProgressDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, PIC, PICName, Remark FROM MTNProgressPIC WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("MTNProgressPIC")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, MaintenanceItem, Material, Qty, Unit, Remark FROM MTNProgressMaterial WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("MTNProgressMaterial")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3
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
                lbStatus.Text = MessageDlg("Detail 'Maintenance Item & Job' must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'PIC' must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt3")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Use up Material / Job' must have at least 1 record")
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
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbMaintenanceItem.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
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
            tbPICCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            EnableHd(True)
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
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "Report No, Report Date, Mo No, Machine, Practitioner, Project Manager, Reviewed By, Acknowledge By 1, Acknowledge By 2, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), MONo, Machine, Practitioner, ProjectManager, ReviewedBy, AcknowledgeBy1, AcknowledgeBy2, Remark"
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
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PEFormCandidate " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormMTNProgress.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
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
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            For Each r In dr
                r.Delete()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("EduLevel = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
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
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Maintenance Item & Job' must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'PIC' must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt3")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Use up Material / Job' must have at least 1 record")
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
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMONo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT DISTINCT MO_No, MO_Date, Machine, Machine_Name, FgSubkon, ProjectManager, ProjectManager_Name FROM V_MTNProgressReff WHERE Status = 'P'"
            ResultField = "MO_No, MO_Date, Machine, Machine_Name, FgSubkon, ProjectManager, ProjectManager_Name"
            ViewState("Sender") = "btnMONo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn MONo Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            ddlMaintenanceItem.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue))(0)
                Row.BeginEdit()
                Row("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
                Row("MaintenanceItem_Name") = ddlMaintenanceItem.SelectedItem.Text
                Row("Material") = tbMaterial.Text
                Row("Material_Name") = tbMaterialname.Text
                Row("Qty") = tbQtyDt3.Text
                If ddlUnit.SelectedValue = "" Then
                    Row("Unit") = DBNull.Value
                Else
                    Row("Unit") = ddlUnit.SelectedValue
                End If

                Row("Remark") = tbRemarkDt3.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
                dr("MaintenanceItem_Name") = ddlMaintenanceItem.SelectedItem.Text
                dr("Material") = tbMaterial.Text
                dr("Material_Name") = tbMaterialname.Text
                dr("Qty") = tbQtyDt3.Text
                If ddlUnit.SelectedValue = "" Then
                    dr("Unit") = DBNull.Value
                Else
                    dr("Unit") = ddlUnit.SelectedValue
                End If

                dr("Remark") = tbRemarkDt3.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt3"), GridDt3)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("MaintenanceItem_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    Return False
                End If
                If Dr("Material_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                
            Else
                If ddlMaintenanceItem.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    ddlMaintenanceItem.Focus()
                    Return False
                End If
                If tbMaterialname.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    tbMaterialname.Focus()
                    Return False
                End If
                If CFloat(tbQtyDt3.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQtyDt3.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReviewedBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReviewedBy.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Job_Level, Job_Level_Name, Emp_Status, Emp_Status_Name, Work_Place, Work_Place_Name, Department, Department_Name, Section, Section_Name, Sub_Section, Sub_Section_Name FROM V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnReviewedBy"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnReviewedBy Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAcknowledgeBy1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcknowledgeBy1.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Job_Level, Job_Level_Name, Emp_Status, Emp_Status_Name, Work_Place, Work_Place_Name, Department, Department_Name, Section, Section_Name, Sub_Section, Sub_Section_Name FROM V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAcknowledgeBy1"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAcknowledgeBy1 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAcknowledgeBy2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcknowledgeBy2.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Job_Level, Job_Level_Name, Emp_Status, Emp_Status_Name, Work_Place, Work_Place_Name, Department, Department_Name, Section, Section_Name, Sub_Section, Sub_Section_Name FROM V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAcknowledgeBy2"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAcknowledgeBy2 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAcknowledgeBy1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAcknowledgeBy1.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindEmployee " + QuotedStr(tbAcknowledgeBy1.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAcknowledgeBy1.Text = Dr("Emp_No")
                tbAcknowledgeBy1Name.Text = Dr("Emp_Name")
            Else
                tbAcknowledgeBy1.Text = ""
                tbAcknowledgeBy1Name.Text = ""
            End If
            tbAcknowledgeBy1.Focus()
        Catch ex As Exception
            Throw New Exception("tbAcknowledgeBy1 change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAcknowledgeBy2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAcknowledgeBy2.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindEmployee " + QuotedStr(tbAcknowledgeBy2.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAcknowledgeBy2.Text = Dr("Emp_No")
                tbAcknowledgeBy2Name.Text = Dr("Emp_Name")
            Else
                tbAcknowledgeBy2.Text = ""
                tbAcknowledgeBy2Name.Text = ""
            End If
            tbAcknowledgeBy2.Focus()
        Catch ex As Exception
            Throw New Exception("tbAcknowledgeBy2 change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbReviewedBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReviewedBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindEmployee " + QuotedStr(tbReviewedBy.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbReviewedBy.Text = Dr("Emp_No")
                tbReviewedByName.Text = Dr("Emp_Name")
            Else
                tbReviewedBy.Text = ""
                tbReviewedByName.Text = ""
            End If
            tbReviewedBy.Focus()
        Catch ex As Exception
            Throw New Exception("tbReviewedBy change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaintenanceItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaintenanceItem.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT DISTINCT MaintenanceItem, MaintenanceItem_Name, Job, Job_Name, ItemSpec, JobDescription, Remark, ItemQty FROM V_MTNProgressReff"
            ResultField = "MaintenanceItem, MaintenanceItem_Name, Job, Job_Name, ItemSpec, JobDescription, Remark, ItemQty"
            ViewState("Sender") = "btnMaintenanceItem"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaintenanceItem Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPIC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPIC.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Job_Level, Job_Level_Name, Emp_Status, Emp_Status_Name, Work_Place, Work_Place_Name, Department, Department_Name, Section, Section_Name, Sub_Section, Sub_Section_Name FROM V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnPIC"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnPIC Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPICCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPICCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindEmployee " + QuotedStr(tbPICCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbPICCode.Text = Dr("Emp_No")
                tbPICName.Text = Dr("Emp_Name")
            Else
                tbPICCode.Text = ""
                tbPICName.Text = ""
            End If
            tbPICCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbPICCode change Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillMaintenanceItem()
        Try
            FillCombo(ddlMaintenanceItem, "SELECT DISTINCT MaintenanceItem, MaintenanceItem_Name FROM V_MTNProgressReff WHERE MO_No = " + QuotedStr(tbMONo.Text), True, "MaintenanceItem", "MaintenanceItem_Name", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "FillMaintenanceItem error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Product, Product_Name, Specification, Qty, Unit FROM V_MTNProgressGetMaterial WHERE MONo = " + QuotedStr(tbMONo.Text)
            ResultField = "Product, Product_Name, Specification, Qty, Unit"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterial Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMaterial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterial.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("SELECT Product, Product_Name, Specification, Qty, Unit FROM V_MTNProgressGetMaterial WHERE MONo = " + QuotedStr(tbMONo.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterial.Text = Dr("Product")
                tbMaterialName.Text = Dr("Product_Name")
                tbQtyDt3.Text = Dr("Qty")
                ddlUnit.SelectedValue = Dr("Unit")
            Else
                tbMaterial.Text = ""
                tbMaterialName.Text = ""
                tbQtyDt3.Text = ""
                ddlUnit.SelectedValue = ""
            End If
            tbMaterial.Focus()
        Catch ex As Exception
            Throw New Exception("tbMaterial_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
