Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrFAOpname_TrFAOpname
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFAOpnameHd"

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

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnFA" Then
                    tbFA.Text = Session("Result")(0).ToString
                    tbFAName.Text = Session("Result")(1).ToString
                    tbLocCode.Text = ""
                    tbLocName.Text = ""
                ElseIf ViewState("Sender") = "btnLoc" Then
                    tbLocCode.Text = Session("Result")(0).ToString
                    tbLocName.Text = Session("Result")(1).ToString
                    tbQtyActual.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitQty"))
                    tbQtySystem.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitQty"))
                    'getQtyActual()
                ElseIf ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim drcount As DataRow()
                    'Dim dt As DataTable

                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        'insert
                        drcount = ViewState("Dt").Select("FixedAsset = " + QuotedStr(drResult("FACode").ToString) + _
                                            " AND LocationType=" + QuotedStr(drResult("FALocationType").ToString) + _
                                            " AND LocationCode=" + QuotedStr(drResult("FALocationCode").ToString))
                        If drcount.Length = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("FixedAsset") = drResult("FACode")
                            dr("FAName") = drResult("FAName")
                            dr("LocationType") = drResult("FALocationType")
                            dr("LocationCode") = drResult("FALocationCode")
                            dr("LocationName") = drResult("FALocationName")
                            dr("QtySystem") = drResult("QtyActual")
                            dr("QtyActual") = drResult("QtyActual")
                            dr("QtyOpname") = "0"
                            dr("Remark") = DBNull.Value
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
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
        Try
            FillRange(ddlRange)
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbQtyOpname.Attributes.Add("ReadOnly", "True")
            tbQtySystem.Attributes.Add("ReadOnly", "True")

            tbQtyOpname.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtySystem.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyActual.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbQtyOpname.Attributes.Add("OnBlur", "setformatdt();")
            tbQtySystem.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyActual.Attributes.Add("OnBlur", "setformatdt();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
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

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data")
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
        Return "SELECT * From V_GLFAOpnameDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
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
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_GLFAOpname", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
    Private Sub EnableHd(ByVal State As Boolean)
        Try
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
    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim filter As String
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("FixedAsset = " + QuotedStr(tbFA.Text) + " AND LocationType = " + QuotedStr(ddlLocationType.SelectedValue) + " AND LocationCode = " + QuotedStr(tbLocCode.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                If ExistRow.Length > AllowedRecordDt() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If


                filter = "FixedAsset = " + QuotedStr(ViewState("FixedAsset")) + " AND LocationType = " + _
                         QuotedStr(ViewState("LocationType")) + " AND LocationCode = " + QuotedStr(ViewState("LocationCode"))

                Row = ViewState("Dt").Select(filter)(0)
                Row.BeginEdit()
                Row("FixedAsset") = tbFA.Text
                Row("FAName") = tbFAName.Text
                Row("LocationType") = ddlLocationType.SelectedValue
                Row("LocationCode") = tbLocCode.Text
                Row("LocationName") = tbLocName.Text
                Row("QtySystem") = tbQtySystem.Text
                Row("QtyActual") = tbQtyActual.Text
                Row("QtyOpname") = tbQtyOpname.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFA.Text
                dr("FAName") = tbFAName.Text
                dr("LocationType") = ddlLocationType.SelectedValue
                dr("LocationCode") = tbLocCode.Text
                dr("LocationName") = tbLocName.Text
                dr("QtySystem") = tbQtySystem.Text
                dr("QtyActual") = tbQtyActual.Text
                dr("QtyOpname") = tbQtyOpname.Text
                dr("Remark") = tbRemarkDt.Text

                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Function AllowedRecordDt() As Integer
        Try
            If ViewState("FixedAsset") = tbFA.Text And ViewState("LocationType") = ddlLocationType.SelectedValue And ViewState("LocationCode") = tbLocCode.Text Then
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
            If pnlDt.Visible = False Then
                lbStatus.Text = MessageDlg("Detail Data must be saved first")
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
                tbCode.Text = GetAutoNmbr("GLO", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO GLFAOpnameHd (TransNmbr, TransDate, Status, OpnameDate, Operator, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ",'H', " + _
                QuotedStr(Format(tbOpname.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(tbOperator.Text) + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFAOpnameHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFAOpnameHd SET OpnameDate = " + QuotedStr(Format(tbOpname.SelectedValue, "yyyy-MM-dd")) + _
                ", Operator = " + QuotedStr(tbOperator.Text) + ", Remark =" + QuotedStr(tbRemark.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FixedAsset, LocationType, LocationCode, QtySystem, QtyActual, QtyOpname, Remark FROM GLFAOpnameDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFAOpnameDt")

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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbOpname.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbOperator.Text = ViewState("UserId").ToString
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlLocationType.SelectedIndex = 2
            tbRemarkDt.Text = ""
            tbFA.Text = ""
            tbFAName.Text = ""
            tbLocCode.Text = "0"
            tbLocName.Text = "0"
            tbQtyActual.Text = "0"
            tbQtyOpname.Text = "0"
            tbQtySystem.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbOperator.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Operator must have value")
                tbOperator.Focus()
                Return False
            End If
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbOpname.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Opname Date must have value")
                tbOpname.Focus()
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
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("Product").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Must Have Value")
                '    btnAddDt.Focus()
                '    Return False
                'End If
            Else
                If tbFA.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Fixed Asset Must Have Value")
                    tbFA.Focus()
                    Return False
                End If
                If tbLocCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    tbLocCode.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
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
            FDateName = "Date, Opname Date"
            FDateValue = "TransDate, OpnameDate"
            FilterName = "Opname No, Status, Operator, Remark"
            FilterValue = "TransNmbr, Status, Operator, Remark"
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
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("SelectCommand") = "EXEC S_GLFormFAOpname " + QuotedStr(GVR.Cells(2).Text)
                    Session("ReportFile") = Server.MapPath("~\Rpt\FormFAOpname.frx")
                    Session("DBConnection") = ViewState("DBConnection")
                    AttachScript("openprintdlg();", Page, Me.GetType)
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Cleardt()
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(1).Text) + " AND LocationType = " + QuotedStr(GVR.Cells(3).Text) + " AND LocationCode = " + QuotedStr(GVR.Cells(4).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text, GVR.Cells(3).Text, GVR.Cells(4).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("FixedAsset") = GVR.Cells(1).Text
            ViewState("LocationType") = GVR.Cells(3).Text
            ViewState("LocationCode") = GVR.Cells(4).Text
            tbFA.Focus()
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
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbOpname, Dt.Rows(0)("OpnameDate").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal FA As String, ByVal LocType As String, ByVal LocCode As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FixedAsset = " + QuotedStr(FA) + " AND LocationType = " + QuotedStr(LocType) + " AND LocationCode = " + QuotedStr(LocCode))
            If Dr.Length > 0 Then
                BindToText(tbFA, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FAName").ToString)
                BindToText(tbLocCode, Dr(0)("LocationCode").ToString)
                BindToText(tbLocName, Dr(0)("LocationName").ToString)
                BindToDropList(ddlLocationType, Dr(0)("LocationType").ToString)
                BindToText(tbQtyActual, Dr(0)("QtyActual").ToString)
                BindToText(tbQtySystem, Dr(0)("QtySystem").ToString)
                BindToText(tbQtyOpname, Dr(0)("QtyOpname").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbFA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFA.TextChanged
        Dim Dt As DataTable
        Try
            Dt = SQLExecuteQuery("SELECT FACode, FAName FROM V_GLFAOpnameGetDetail WHERE FACode = " + QuotedStr(tbFA.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                tbFA.Text = Dt.Rows(0)("FACode")
                tbFAName.Text = Dt.Rows(0)("FAName")
            Else
                tbFA.Text = ""
                tbFAName.Text = ""
            End If
            tbLocCode.Text = ""
            tbLocName.Text = ""
        Catch ex As Exception
            Throw New Exception("tb FA change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnFA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFA.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, BuyingDate, QtyActual FROM V_GLFAOpnameGetDetail"
            ResultField = "FACode, FAName"
            ViewState("Sender") = "btnFA"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn FA Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlLocationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocationType.SelectedIndexChanged
        Try
            tbLocCode.Text = ""
            tbLocName.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddl Loc Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnLoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoc.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "EXEC S_GLFAOpnameGetLocation  " + QuotedStr(ddlLocationType.SelectedValue) + ", " + QuotedStr(tbFA.Text)
            ResultField = "FA_Location_Code, FA_Location_Name, Qty"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnLoc"
            'Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Location Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbLocCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLocCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT FALocationCode, FALocationName FROM V_GLFAOpnameGetDetail WHERE FALocationType =" + QuotedStr(ddlLocationType.SelectedValue) + " AND FACode = " + QuotedStr(tbFA.Text) + " AND FALocationCode=" + QuotedStr(tbLocCode.Text) + "", ViewState("DBConnection").ToString).Tables(0)
            Dt = SQLExecuteQuery("EXEC S_GLFAOpnameGetFALocation  " + QuotedStr(ddlLocationType.SelectedValue) + ", " + QuotedStr(tbFA.Text) + ", " + QuotedStr(tbLocCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbLocCode.Text = Dr("FA_Location_Code")
                tbLocName.Text = Dr("FA_Location_Name")
                tbQtyActual.Text = Dr("Qty")
                tbQtySystem.Text = Dr("Qty")
                'getQtyActual()
            Else
                tbLocCode.Text = ""
                tbLocName.Text = ""
            End If
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Loc Code change Error : " + ex.ToString)
        End Try
        'Dim Dr As DataRow
        'Dim ds As DataSet
        'Dim sqlstring As String
        'Try
        '    sqlstring = "SELECT FALocationCode, FALocationName FROM V_GLFAOpnameGetDetail WHERE FALocationType =" + QuotedStr(ddlLocationType.SelectedValue) + " AND FACode = " + QuotedStr(tbFA.Text) + " AND FALocationCode=" + QuotedStr(tbLocCode.Text)
        '    ds = SQLExecuteQuery(sqlstring, ViewState("DBConnection"))

        '    Dr = ds.Tables(0).Rows(0)
        '    If Not Dr Is Nothing Then
        '        tbLocCode.Text = Dr("FALocationCode")
        '        tbLocName.Text = Dr("FALocationName")
        '        getQtyActual()
        '    Else
        '        tbLocCode.Text = ""
        '        tbLocName.Text = ""
        '    End If
        'Catch ex As Exception
        '    Throw New Exception("tb Loc Code change Error : " + ex.ToString)
        'End Try
    End Sub

    Private Sub getQtyActual()
        Dim sqlstring, result As String
        Try
            If tbFA.Text.Trim = "" Or tbLocCode.Text.Trim = "" Then
                Exit Sub
            End If
            sqlstring = "EXEC S_GLFAOpnameGetQtyActual " + QuotedStr(Format(tbOpname.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(tbFA.Text) + "," + QuotedStr(ddlLocationType.SelectedValue) + "," + QuotedStr(tbLocCode.Text)

            result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
            tbQtySystem.Text = result
            tbQtyActual.Text = result
            AttachScript("setformatdt();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("getQtyActual Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            Session("Result") = Nothing
            Session("Filter") = "SELECT FACode, FAName, FALocationCode, FALocationName, FALocationType, QtyActual FROM V_GLFAOpnameGetDetail"
            ResultField = "FACode, FAName, FALocationCode, FALocationName, FALocationType, QtyActual"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridDt.SelectedIndexChanged

    End Sub
End Class
