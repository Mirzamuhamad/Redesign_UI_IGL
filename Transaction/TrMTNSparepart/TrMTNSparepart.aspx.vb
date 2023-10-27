Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrMTNSparepart_TrMTNSparepart
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_MTNSparepartHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        If Not CekExistData(ViewState("Dt"), "Product+'|'+Location", drResult("Product")) Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("Location") = ""
                            dr("Location_Name") = ""
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnMONo" Then
                    tbSPReqNo.Text = Session("Result")(0).ToString
                    tbMONo.Text = Session("Result")(1).ToString
                    ddlMachine.SelectedValue = Session("Result")(2).ToString
                    tbRequestBy.Text = Session("Result")(3).ToString
                    tbRequestByName.Text = Session("Result")(4).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProduct.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    tbSpecification.Text = Session("Result")(2).ToString
                    tbQty.Text = FormatFloat(Session("Result")(3).ToString, ViewState("DigitQty"))
                    ddlunit.SelectedValue = Session("Result")(4).ToString
                    GetInfo(tbProduct.Text)
                End If
                If ViewState("Sender") = "btnRequestBy" Then
                    tbRequestBy.Text = Session("Result")(0).ToString
                    tbRequestByName.Text = Session("Result")(1).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
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

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        If Request.QueryString("ContainerId").ToString = "TrMTNSparepartIssueID" Then
            lbjudul.Text = "Maintenance Sparepart - Issue"
            ViewState("SPType") = "Issue"
        Else
            lbjudul.Text = "Maintenance Sparepart - Return"
            ViewState("SPType") = "Return"
        End If
        ViewState("SetLocation") = False
        FillCombo(ddlwrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
        FillCombo(ddlMachine, "Select MachineCode, MachineName FROM MsMachine ", True, "MachineCode", "MachineName", ViewState("DBConnection"))
        FillCombo(ddlunit, "Select UnitCode, UnitName FROM MsUnit ", True, "UnitCode", "UnitName", ViewState("DBConnection"))
        tbSpecification.Attributes.Add("ReadOnly", "True")
        'tbMachineName.Attributes.Add("ReadOnly", "True")
        tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            GetStringHd1 = "Select * From V_MTNSparepartHd WHERE SPType = " + QuotedStr(ViewState("SPType").ToString)
            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_MTNSparepartDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

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
            If ActionValue = "Print" Then
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
                If Request.QueryString("ContainerId").ToString = "TrMTNSparepartIssueID" Then
                    'lbStatus.Text = Result
                    'Exit Sub
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_MTNSparepartForm " + Result
                    Session("ReportFile") = ".../../../Rpt/MTNSPIssueForm.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)
                Else
                    'lbStatus.Text = Result
                    'Exit Sub
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_MTNSparepartForm " + Result
                    Session("ReportFile") = ".../../../Rpt/MTNSPReturnForm.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)
                End If
            Else

                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_MTNSparepart", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("TransNmbr in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbRef.Enabled = State
            ddlwrhs.Enabled = State 'And tbReqNo.Text = ""
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            btnMONo.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If ViewState("SPType") = "Issue" Then
                    tbRef.Text = GetAutoNmbr("MTSPI", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)
                Else
                    tbRef.Text = GetAutoNmbr("MTSPR", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)
                End If
                SQLString = "INSERT INTO MTNSparepartHd (TransNmbr, Status, TransDate, SPRequestNo, MONo, Machine, Warehouse, FgSubled, SubLed, RequestBy, Remark, UserPrep, DatePrep, SPType ) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbSPReqNo.Text) + "," + QuotedStr(tbMONo.Text) + ", " + QuotedStr(ddlMachine.SelectedValue) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbSubLed.Text) + ", " + _
                QuotedStr(tbRequestBy.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(ViewState("SPType"))
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM MTNSparepartHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNSparepartHd SET SPRequestNo = " + QuotedStr(tbSPReqNo.Text) + ", MONo = " + QuotedStr(tbMONo.Text) + ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                " Machine = " + QuotedStr(ddlMachine.SelectedValue) + ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + ", SubLed = " + QuotedStr(tbSubLed.Text) + ", RequestBy = " + QuotedStr(tbRequestBy.Text) + ", Remark = '" + tbRemark.Text + "'," + _
                " UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate() " + _
                " WHERE TransNmbr = '" + tbRef.Text + "' AND SPType = " + QuotedStr(ViewState("SPType"))
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Location, Qty, Unit, Remark FROM MTNSparepartDt WHERE TransNmbr = '" & ViewState("Reference") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MTNSparepartDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            'tbAdjPercent.Text = "0"
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlwrhs.SelectedIndex = 0
            'ddlPurpose.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubLed.Text = ""
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbSubLedName.Text = ""
            tbSPReqNo.Text = ""
            tbMONo.Text = ""
            ddlMachine.SelectedValue = ""
            tbRequestBy.Text = ""
            tbRequestByName.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            ddlunit.SelectedValue = ""
            tbQty.Text = FormatFloat(0, ViewState("DigitQty"))
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        If ViewState("SetLocation") Then
            FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ViewState("SetLocation") = False
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbSPReqNo.Text = "" Then
                lbStatus.Text = MessageDlg("Sparepart Request must have value")
                btnMONo.Focus()
                Return False
            End If
            If ddlMachine.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Machine must have value")
                ddlMachine.Focus()
                Return False
            End If
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlwrhs.Focus()
                Return False
            End If
            If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubLed.Focus()
                Return False
            End If
            If tbRequestBy.Text = "" Then
                lbStatus.Text = MessageDlg("Project Manager must have value")
                tbRequestBy.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If TrimStr(Dr("Location").ToString.Trim) = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            Else
                If tbProduct.Text = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If ddlLocation.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    ddlLocation.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If ddlunit.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlunit.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Status, SP Request No, MO No, Machine, Request By, Warehouse, Remark"
            FilterValue = "TransNmbr, Status, SPRequestNo, MONo, MachineName, RequestByName, WarehouseName, Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("Status") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(0).Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    'Dim Dr As DataRow
                    'Dr = FindMaster("Rate", ddlCurrency.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), Session("DBConnection").ToString)
                    'If Not Dr Is Nothing Then
                    '    ViewState("DigitCurr") = Dr("digit")
                    '    AttachScript("setformat();", Page, Me.GetType())
                    'End If
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt.Columns(0).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'tbAdjPercent.Text = "0"
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBCOnnection") = ViewState("DBConnection")
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        If Request.QueryString("ContainerId").ToString = "TrMTNSparepartIssueID" Then

                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_MTNSparepartForm  " + QuotedStr(GVR.Cells(2).Text)
                            Session("ReportFile") = ".../../../Rpt/MTNSPIssueForm.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else

                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_MTNSparepartForm  " + QuotedStr(GVR.Cells(2).Text)
                            Session("ReportFile") = ".../../../Rpt/MTNSPReturnForm.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        End If
                        
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

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

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Sparepart Request is not Post, cannot close Detail Item Product")
                    Exit Sub
                End If
                ViewState("MachineClose") = GVR.Cells(4).Text
                ViewState("MaintenanceItemClose") = GVR.Cells(6).Text
                ViewState("JobClose") = GVR.Cells(8).Text
                If GVR.Cells(14).Text <> "Y" Then
                    AttachScript("closing();", Page, Me.GetType)
                Else
                    ViewState("MachineClose") = Nothing
                    ViewState("MaintenanceItemClose") = Nothing
                    ViewState("JobClose") = Nothing
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            Dim lb As Label
            lb = GVR.FindControl("lbLocation")
            dr = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(GVR.Cells(1).Text + "|" + TrimStr(lb.Text)))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lb = GVR.FindControl("lbLocation")

            ViewState("DtValue") = GVR.Cells(1).Text + "|" + TrimStr(lb.Text)
            If ViewState("SetLocation") Then
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSPReqNo, Dt.Rows(0)("SPRequestNo").ToString)
            BindToText(tbMONo, Dt.Rows(0)("MONo").ToString)
            BindToDropList(ddlMachine, Dt.Rows(0)("Machine").ToString)

            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("SubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("SubLed_Name").ToString)

            BindToText(tbRequestBy, Dt.Rows(0)("RequestBy").ToString)
            BindToText(tbRequestByName, Dt.Rows(0)("RequestByName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product+'|'+Location = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToDropList(ddlunit, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                GetInfo(tbProduct.Text)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(tbProduct.Text) + "|" + TrimStr(ddlLocation.SelectedValue) Then
                    If CekExistData(ViewState("Dt"), "Product,Location", tbProduct.Text + "|" + TrimStr(ddlLocation.SelectedValue)) Then
                        lbStatus.Text = "Product " + tbProductName.Text + " Location " + ddlLocation.SelectedItem.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("ProductName") = tbProductName.Text
                Row("Location") = ddlLocation.SelectedValue
                Row("Location_Name") = ddlLocation.SelectedItem.Text
                Row("Specification") = tbSpecification.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = ddlunit.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product, Location", tbProduct.Text + "|" + TrimStr(ddlLocation.SelectedValue)) Then
                    lbStatus.Text = "Product " + tbProductName.Text + " Location " + ddlLocation.SelectedItem.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProduct.Text
                dr("ProductName") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                dr("Location") = ddlLocation.SelectedValue
                dr("Location_Name") = ddlLocation.SelectedItem.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = ddlunit.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            PnlInfo.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            PnlInfo.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                'If cb.Checked = False Then
                'btnGetSetZero.Visible = True
                'End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlwrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("WrhsUser", ddlwrhs.SelectedValue + "|" + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubLed.Text = Dr("FgSubLed")
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            Else
                tbFgSubLed.Text = "N"
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            ViewState("SetLocation") = True
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlwrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Protected Sub btnGetdata_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "Select Product, Product_Name, Specification, Qty, Unit from V_MTNSparepartReff WHERE Request_No = " + QuotedStr(tbSPReqNo.Text)
            ResultField = "Product, Product_Name, Specification, Qty, Unit"
            ViewState("Sender") = "btnGetData"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnGetdata_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMONo.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select distinct Request_No, dbo.FormatDate(Request_Date) AS Request_Date, MO_No, Machine, MachineName, RequestBy, RequestByName, Remark from V_MTNSparepartReff WHERE SPType = " + QuotedStr(ViewState("SPType"))
            ResultField = "Request_No, MO_No, Machine, RequestBy, RequestByName"
            ViewState("Sender") = "btnMONo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnMONo_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnRequestBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestBy.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select * from V_MsEmployee"
            ResultField = "Emp_No, Emp_Name, Department"
            ViewState("Sender") = "btnRequestBy"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnRequestBy_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbRequestBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRequestBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "Select EmpNumb, EmpName FROM MsEmployee WHERE EmpNumb = " + QuotedStr(tbRequestBy.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbRequestBy.Text = Dr("EmpNumb")
                tbRequestByName.Text = Dr("EmpName")
            Else
                tbRequestBy.Text = ""
                tbRequestByName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbRequestBy_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select Product, Product_Name, Specification, Qty, Unit from V_MTNSparepartReff WHERE Request_No = " + QuotedStr(tbSPReqNo.Text)
            ResultField = "Product, Product_Name, Specification, Qty, Unit"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnProduct_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
        Dim StringSQL As String
        Dim Dt As DataTable
        Try
            StringSQL = "Select Product, Product_Name, Specification, Qty, Unit from V_MTNSparepartReff WHERE Request_No = " + QuotedStr(tbSPReqNo.Text) + " and Product = " + QuotedStr(tbProduct.Text)
            Dt = SQLExecuteQuery(StringSQL, ViewState("DBConnection")).Tables(0)
            If Dt.Rows.Count > 0 Then
                tbProduct.Text = Dt.Rows(0)("Product")
                tbProductName.Text = Dt.Rows(0)("Product_Name")
                tbSpecification.Text = Dt.Rows(0)("Specification")
                ddlunit.SelectedValue = Dt.Rows(0)("Unit")
                tbQty.Text = FormatFloat(Dt.Rows(0)("Qty"), ViewState("DigitQty"))
                GetInfo(tbProduct.Text)
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbQty.Text = FormatFloat(0, ViewState("DigitQty"))
                ddlunit.Text = ""
                PnlInfo.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("tbProduct_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    Dim QtyOnHand As Decimal = 0
    Protected Sub GridInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridInfo.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Qty")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    QtyOnHand += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(0).Text = "Total On Hand :"
                    ' for the Footer, display the running totals
                    e.Row.Cells(1).Text = FormatFloat(QtyOnHand, ViewState("DigitQty"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Info Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
End Class
