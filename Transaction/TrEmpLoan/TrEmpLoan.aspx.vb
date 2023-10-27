Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrEmpLoan_TrEmpLoan
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PYLoanHd"
    Protected EditProcess As Boolean

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
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
                If ViewState("Sender") = "btnEmp" Then
                    BindToText(tbEmpNo, Session("Result")(0).ToString)
                    BindToText(tbEmpName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = TrimStr(drResult("ProductName").ToString)
                            dr("Specification") = TrimStr(drResult("Specification").ToString)
                            dr("Remark") = TrimStr(drResult("Remark").ToString)
                            dr("QtyWrhs") = drResult("QtyWrhs")
                            dr("UnitWrhs") = drResult("UnitWrhs")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = drResult("QtyRoll")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error : " + ex.ToString
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
            FillCombo(ddlJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
            FillCombo(ddlLoan, "SELECT * FROM MsLoan", True, "LoanCode", "LoanName", ViewState("DBConnection"))
            FillCombo(ddlCurrency, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If

            tbAmountLoan.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbInterest.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbangsuran.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountAngsurDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountLoan.Attributes.Add("OnBlur", "setformat();")
            tbInterest.Attributes.Add("OnBlur", "setformatt();")
            tbAmountInterest.Attributes.Add("OnBlur", "setformat();")
            tbTotalPembayaran.Attributes.Add("OnBlur", "setformat();")
            tbangsuran.Attributes.Add("OnBlur", "setformat();")
            tbAmountbeginDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountAngsurDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountPokokDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountBungaDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountEndDt.Attributes.Add("OnBlur", "setformatdt();")
            'tbExtend.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
                ViewState("SortExpression") = "TransNmbr DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PYLoanDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlLoanType.Enabled = State
            ddlLoan.Enabled = State
            tbStartClaim.Enabled = State
            tbAmountLoan.Enabled = State
            tbInterest.Enabled = State
            ddlTerm.Enabled = State
            tbangsuran.Enabled = State
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
            BindGridDtExtended()
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
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

    Private Function AllowedRecord() As Integer
        Try
            If ViewState("Period") = tbPeriode.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function


    Private Sub SaveAll()
        Dim SQLString, CekTrans As String
        Dim I As Integer
        Try
            If pnlEditDt.Visible = True Then
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
                tbCode.Text = GetAutoNmbr("EL", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                CekTrans = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM PYLoanHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " AND Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Employee Loan No. " + tbCode.Text + " and Revisi " + lbRevisi.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO PYLoanHd (TransNmbr, Status, TransDate, Revisi, " + _
                "LoanType, JobLevel, EmpNumb, Loan, " + _
                "StartClaim, CurrCode, AmountLoan, " + _
                "Interest, QtyPeriod, TermPeriod, " + _
                "Angsuran, TotalBunga, TotalPay, " + _
                "Remark, UserPrep, DatePrep, FgActive) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', 0, " + _
                QuotedStr(ddlLoanType.SelectedValue) + "," + QuotedStr(ddlJobLevel.SelectedValue) + "," + QuotedStr(tbEmpNo.Text) + ", " + QuotedStr(ddlLoan.SelectedValue) + ", '" + _
                Format(tbStartClaim.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlCurrency.SelectedValue) + ", " + tbAmountLoan.Text.Replace(",", "") + "," + _
                tbInterest.Text.Replace(",", "") + "," + tbQtyPeriod.Text.Replace(",", "") + ", " + QuotedStr(ddlTerm.SelectedValue) + "," + _
                tbangsuran.Text.Replace(",", "") + ", " + tbAmountInterest.Text.Replace(",", "") + ", " + tbTotalPembayaran.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate(), 'Y'"

                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                SQLString = "UPDATE PYLoanHd SET LoanType = " + QuotedStr(ddlLoanType.SelectedValue) + _
                ", JobLevel = " + QuotedStr(ddlJobLevel.SelectedValue) + _
                ", EmpNumb = " + QuotedStr(tbEmpNo.Text) + _
                ", Loan = " + QuotedStr(ddlLoan.SelectedValue) + _
                ", StartClaim = " + QuotedStr(Format(tbStartClaim.SelectedValue, "yyyy-MM-dd")) + _
                ", CurrCode = " + QuotedStr(ddlCurrency.SelectedValue) + _
                ", AmountLoan = " + tbAmountLoan.Text.Replace(",", "") + _
                ", Interest = " + tbInterest.Text.Replace(",", "") + _
                ", QtyPeriod = " + tbQtyPeriod.Text.Replace(",", "") + _
                ", TermPeriod = " + QuotedStr(ddlTerm.SelectedValue) + _
                ", Angsuran = " + tbangsuran.Text.Replace(",", "") + _
                ", TotalBunga = " + tbAmountInterest.Text.Replace(",", "") + _
                ", TotalPay = " + tbTotalPembayaran.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = lbRevisi.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, Periode, StartClaim, AmountBegin, AmountAngsur, AmountPokok, AmountBunga, AmountEnd, Remark " + _
                                         " FROM PYLoanDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PYLoanDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("TransNmbr") = Now.Year.ToString
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()

            pnlDt.Visible = True
            BindDataDt("", "0")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = Now.Date
            lbRevisi.Text = "0"
            ddlLoanType.SelectedIndex = 0
            ddlJobLevel.SelectedIndex = 0
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
            ddlLoan.SelectedIndex = 0
            tbStartClaim.SelectedDate = Now.Date
            ddlTerm.SelectedIndex = 0
            ddlCurrency.SelectedIndex = 0
            tbAmountLoan.Text = "0"
            tbInterest.Text = "0"
            tbAmountInterest.Text = "0"
            tbTotalPembayaran.Text = "0"
            ddlTerm.SelectedIndex = 0
            tbangsuran.Text = "0"
            tbQtyPeriod.Text = "0"
            tbRemark.Text = ""
            ddlCurrency.SelectedValue = ViewState("Currency")
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbPeriode.Text = "0"
            tbStartClaimDt.SelectedDate = ViewState("ServerDate")
            tbAmountbeginDt.Text = "0"
            tbAmountAngsurDt.Text = "0"
            tbAmountPokokDt.Text = "0"
            tbAmountBungaDt.Text = "0"
            tbAmountEndDt.Text = "0"
            tbRemarkDt.Text = ""
            tbDonePaid.Text = "N"
            tbExtend.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value. ")
                tbDate.Focus()
                Return False
            End If
            If ddlLoanType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Payment Type must have value. ")
                ddlLoanType.Focus()
                Return False
            End If
            If ddlJobLevel.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Job Level must have value. ")
                ddlJobLevel.Focus()
                Return False
            End If
            If tbEmpNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee must have value. ")
                tbEmpNo.Focus()
                Return False
            End If
            If ddlLoan.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Loan Type must have value. ")
                ddlLoan.Focus()
                Return False
            End If
            If tbStartClaim.IsNull Then
                lbStatus.Text = MessageDlg("Start Claim must have value. ")
                tbStartClaim.Focus()
                Return False
            End If
            If tbStartClaim.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Start Claim must earlier than Employee Loan Date")
                tbStartClaim.Focus()
                Return False
            End If
            If CFloat(tbAmountLoan.Text) <= "0" Then
                lbStatus.Text = MessageDlg("Amount Loan must have value. ")
                tbAmountLoan.Focus()
                Return False
            End If
            If CFloat(tbInterest.Text) < "0" Then
                lbStatus.Text = MessageDlg("Interest % must have value. ")
                tbInterest.Focus()
                Return False
            End If
            If ddlTerm.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Term Period must have value. ")
                ddlTerm.Focus()
                Return False
            End If
            If CFloat(tbQtyPeriod.Text) <= "0" Then
                lbStatus.Text = MessageDlg("Qty Period must have value. ")
                tbQtyPeriod.Focus()
                Return False
            End If
            If (CFloat(tbangsuran.Text.Trim) <= "0") And (ddlLoanType.SelectedValue.Trim = "Angsuran") Then
                lbStatus.Text = MessageDlg("Amount Angsuran must have value. ")
                tbangsuran.Focus()
                Return False
            End If
            If CFloat(tbangsuran.Text.Trim) > CFloat(tbAmountLoan.Text.Trim) Then
                lbStatus.Text = MessageDlg("Amount Angsuran must be less than Amount Loan. ")
                tbangsuran.Focus()
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


            Else
                If CFloat(tbAmountAngsurDt.Text.Trim) <= "0" Then
                    lbStatus.Text = MessageDlg("Amount Installment Must Have Value")
                    tbAmountAngsurDt.Focus()
                    Return False
                End If
                EditProcess = False
                tbAmountPokokDt.Text = CFloat(tbAmountAngsurDt.Text) - CFloat(tbAmountBungaDt.Text)
                tbAmountEndDt.Text = CFloat(tbAmountbeginDt.Text) - CFloat(tbAmountPokokDt.Text)
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
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
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    Panel1.Visible = False
                    pnlDt.Visible = True
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0
                    GridDt.Columns(1).Visible = False
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnGetData.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Panel1.Visible = True
                        tbExtend.Text = "0"
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(4).Text
                        GridDt.PageIndex = 0
                        GridDt.Columns(1).Visible = True
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        LoanTypeChange()
                        btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If Not GVR.Cells(3).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must be Posted Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_PYLoanCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + GVR.Cells(4).Text + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = GVR.Cells(2).Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PYFormLoan '''" + GVR.Cells(2).Text + "|" + GVR.Cells(4).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormLoan.frx"
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
        'Try
        '    Dim dr() As DataRow
        '    Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        '    dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(1).Text))
        '    dr(0).Delete()
        '    BindGridDt(ViewState("Dt"), GridDt)
        '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        'Catch ex As Exception
        '    lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        'End Try
    End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("StateDt") = "Edit"
            ViewState("Period") = GVR.Cells(2).Text
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            tbAmountAngsurDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Taon As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Taon) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Taon
            lbRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCode, Dt.Rows(0)("TransNmbr").ToString)
            BindToDropList(ddlLoanType, Dt.Rows(0)("LoanType").ToString)
            BindToDropList(ddlJobLevel, Dt.Rows(0)("JobLevel").ToString)
            BindToText(tbEmpNo, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlLoan, Dt.Rows(0)("Loan").ToString)
            BindToDate(tbStartClaim, Dt.Rows(0)("StartClaim").ToString)
            BindToDropList(ddlCurrency, Dt.Rows(0)("CurrCode").ToString)
            BindToText(tbAmountLoan, Dt.Rows(0)("AmountLoan").ToString)
            BindToText(tbInterest, Dt.Rows(0)("Interest").ToString)
            BindToText(tbQtyPeriod, Dt.Rows(0)("QtyPeriod").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("TermPeriod").ToString)
            BindToText(tbangsuran, Dt.Rows(0)("Angsuran").ToString)
            BindToText(tbAmountInterest, Dt.Rows(0)("TotalBunga").ToString)
            BindToText(tbTotalPembayaran, Dt.Rows(0)("TotalPay").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Period As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Periode = " + Period)

            If Dr.Length > 0 Then
                BindToText(tbPeriode, Dr(0)("Periode").ToString)
                BindToDate(tbStartClaimDt, Dr(0)("StartClaim").ToString)
                BindToText(tbAmountbeginDt, Dr(0)("AmountBegin").ToString)
                BindToText(tbAmountAngsurDt, Dr(0)("AmountAngsur").ToString)
                BindToText(tbAmountPokokDt, Dr(0)("AmountPokok").ToString)
                BindToText(tbAmountBungaDt, Dr(0)("AmountBunga").ToString)
                BindToText(tbAmountEndDt, Dr(0)("AmountEnd").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbDonePaid, Dr(0)("DonePaid").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindGridDtExtended()
        Try
            BindGridDt(ViewState("Dt"), GridDt)
            'If GetCountRecord(ViewState("Dt")) > 0 Then
            '    GridDt.Columns(1).Visible = True
            'Else
            '    GridDt.Columns(1).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("BindGridDtExtended Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True

            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            LoanTypeChange()
            btnHome.Visible = False
            btnGetData.Visible = True
            tbCode.Focus()
            EnableHd(True)
            Panel1.Visible = True
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
            If ActionValue = "Print" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(4).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_PYFormLoan " + Result
                Session("ReportFile") = ".../../../Rpt/FormLoan.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "3,2,4", ListSelectNmbr, Nmbr, lbStatus.Text)

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else

                        Result = ExecSPCommandGo(ActionValue, "S_PYLoan", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) IN (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim Angsuran, Bunga, AmountLoan As Double
        Dim StartClaim As Date
        Dim JangkaWaktu, TermPay As Integer
        Dim Dt As DataTable
        Dim ds As DataSet
        Dim CurDr, Dr As DataRow
        Try
            If Not CekHd() Then
                Exit Sub
            End If

            If ddlTerm.SelectedValue.Trim = "Month" Then
                Bunga = CFloat(tbInterest.Text.Trim.Replace(",", "")) / 1200
                TermPay = 0
            ElseIf ddlTerm.SelectedValue.Trim = "Half Month" Then
                Bunga = CFloat(tbInterest.Text.Trim.Replace(",", "")) / 2400
                TermPay = 1
            Else
                Bunga = CFloat(tbInterest.Text.Trim.Replace(",", "")) / 5200
                TermPay = 2
            End If

            Angsuran = tbangsuran.Text.Replace(",", "")
            JangkaWaktu = tbQtyPeriod.Text
            AmountLoan = tbAmountLoan.Text.Replace(",", "")
            StartClaim = tbStartClaim.SelectedDate

            Session("DBConnection") = ViewState("DBConnection")

            If GetCountRecord(ViewState("Dt")) > 0 Then
                lbStatus.Text = MessageDlg("Data not empty")
                Exit Sub
            End If

            ds = SQLExecuteQuery("EXEC S_PYLoanGetDt " + Angsuran.ToString + ", " + JangkaWaktu.ToString + ", " + Bunga.ToString + ", " + AmountLoan.ToString + ", '" + Format(StartClaim, "yyyy-MM-dd") + "', " + TermPay.ToString, ViewState("DBConnection").ToString)
            Dt = ds.Tables(0)

            For Each CurDr In Dt.Rows
                Dr = ViewState("Dt").NewRow
                Dr("Periode") = CurDr("Period")
                Dr("StartClaim") = CurDr("StartClaim")
                Dr("AmountBegin") = CurDr("SaldoAwal")
                Dr("AmountAngsur") = CurDr("Angsuran")
                Dr("AmountPokok") = CurDr("Pokok")
                Dr("AmountBunga") = CurDr("Bunga")
                Dr("AmountEnd") = CurDr("SaldoAkhir")
                Dr("DonePaid") = "N"
                ViewState("Dt").Rows.Add(Dr)
            Next
            countDt()
            BindGridDtExtended()
        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        'Try
        '    Cleardt()

        '    If CekHd() = False Then
        '        Exit Sub
        '    End If

        '    ViewState("StateDt") = "Insert"
        '    MovePanel(pnlDt, pnlEditDt)
        '    EnableHd(False)
        '    StatusButtonSave(False)
        '    tbProduct.Focus()
        'Catch ex As Exception
        '    lbStatus.Text = "btn add dt error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If

            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            SaveAll()
            newTrans()
            EnableHd(True)
            LoanTypeChange()
            ViewState("StateHd") = "Insert"
            EditProcess = True
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("Periode = " + tbPeriode.Text)

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                'If EditProcess = False Then
                '    Dim confirmValue As String = Request.Form("confirm_value")
                '    If confirmValue = "Yes" Then
                '        UpdateAngsuran(tbPeriode.Text, True, tbAmountAngsurDt.Text)
                '    Else
                '        UpdateAngsuran(tbPeriode.Text, False, tbAmountAngsurDt.Text)
                '    End If
                'End If

                Row = ViewState("Dt").Select("Periode = " + ViewState("Period"))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Periode") = tbPeriode.Text
                Row("StartClaim") = tbStartClaimDt.SelectedDate
                Row("AmountBegin") = FormatNumber(tbAmountbeginDt.Text, ViewState("DigitCurr"))
                Row("AmountAngsur") = FormatNumber(tbAmountAngsurDt.Text, ViewState("DigitCurr"))
                Row("AmountPokok") = FormatNumber(tbAmountPokokDt.Text, ViewState("DigitCurr"))
                Row("AmountBunga") = FormatNumber(tbAmountBungaDt.Text, ViewState("DigitCurr"))
                Row("AmountEnd") = FormatNumber(tbAmountEndDt.Text, ViewState("DigitCurr"))
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                ViewState("Period") = Nothing
            Else
                'Insert
                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("Periode") = tbPeriode.Text
                dr("StartClaim") = tbStartClaimDt.SelectedDate
                dr("AmountBegin") = FormatNumber(tbAmountbeginDt.Text, ViewState("DigitCurr"))
                dr("AmountAngsur") = FormatNumber(tbAmountAngsurDt.Text, ViewState("DigitCurr"))
                dr("AmountPokok") = FormatNumber(tbAmountPokokDt.Text, ViewState("DigitCurr"))
                dr("AmountBunga") = FormatNumber(tbAmountBungaDt.Text, ViewState("DigitCurr"))
                dr("AmountEnd") = FormatNumber(tbAmountEndDt.Text, ViewState("DigitCurr"))
                dr("Remark") = tbRemarkDt.Text
                dr("DonePaid") = "N"

                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDtExtended()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
            If lbStatus.Text.Length > 0 Then Exit Sub
            MovePanel(pnlInput, PnlHd)

            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            EditProcess = True
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
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

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Job_Level = " + QuotedStr(ddlJobLevel.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmp_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlJobLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJobLevel.SelectedIndexChanged
        Try
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddlJobLevel_SelectedIndexChanged Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNo.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQL As String
        Try
            SQL = "SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Job_Level = " + QuotedStr(ddlJobLevel.SelectedValue) + " AND Emp_No = " + QuotedStr(tbEmpNo.Text)

            Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbEmpNo.Text = Dr("Emp_No")
                tbEmpName.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbEmpNo.Text = ""
                tbEmpName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlLoanType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLoanType.TextChanged
        Try
            LoanTypeChange()
        Catch ex As Exception
            Throw New Exception("ddlLoanType_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub LoanTypeChange()
        Try
            If ddlLoanType.SelectedValue.Trim = "Angsuran" Then
                tbangsuran.Enabled = True
                tbQtyPeriod.Enabled = False
                lbJangka.Text = "Jangka Angsuran"
                tbQtyPeriod.Text = "1"
            Else
                tbangsuran.Enabled = False
                tbQtyPeriod.Enabled = True
                lbJangka.Text = "Jangka Pembayaran"
                tbQtyPeriod.Text = "12"
                tbangsuran.Text = "0"
            End If

            If IsNothing(ViewState("Dt")) Then
                tbangsuran.Enabled = True
            Else
                tbangsuran.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("LoanTypeChange Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub countDt()
        Dim dr As DataRow
        Dim hasil, hasil2 As Double
        Try
            hasil = 0
            hasil2 = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    hasil = hasil + CFloat(dr("AmountAngsur").ToString)
                    hasil2 = hasil2 + CFloat(dr("AmountBunga").ToString)
                End If
            Next
            tbTotalPembayaran.Text = FormatNumber(hasil, ViewState("DigitCurr"))
            tbAmountInterest.Text = FormatNumber(hasil2, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("Count Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub UpdateAngsuran(ByVal Period As Integer, ByVal FgAll As Boolean, ByVal Amount As Double)
        Dim Sisa, Percent As Double
        Dim LastPeriod As Integer
        Dim IPeriod, MPeriod As Integer
        Dim ClaimD, LastDate As Date
        Dim Td, Tm, Ty As Integer
        Dim Dr As DataRow

        Try
            LastPeriod = tbPeriode.Text
            IPeriod = LastPeriod + 1
            MPeriod = LastPeriod + 10

            'CdsDt.Last
            LastDate = tbStartClaimDt.SelectedDate
            Td = LastDate.Day
            Tm = LastDate.Month
            Ty = LastDate.Year

            Do While IPeriod <= MPeriod

                If ddlTerm.SelectedValue = "Month" Then
                    Tm = Tm + 1
                    If Tm > 12 Then
                        Tm = 1
                        Ty = Ty + 1
                    End If
                    If Tm = 2 Then
                        If Ty Mod 4 = 0 Then
                            If Td > 29 Then Td = 29
                        Else
                            If Td > 28 Then Td = 28
                        End If
                    End If
                    ClaimD = Ty.ToString + "-" + Tm.ToString + "-" + Td.ToString
                ElseIf ddlTerm.SelectedValue = "Half Month" Then
                    ClaimD = LastDate + CDate("15")
                ElseIf ddlTerm.SelectedValue = "Week" Then
                    ClaimD = LastDate + CDate("7")
                End If

                LastDate = ClaimD
                Dr = ViewState("Dt").NewRow
                Dr("Periode") = IPeriod
                Dr("StartClaim") = ClaimD
                Dr("AmountBegin") = 0
                Dr("AmountAngsur") = 0
                Dr("AmountPokok") = 0
                Dr("AmountBunga") = 0
                Dr("AmountEnd") = 0
                ViewState("Dt").Rows.Add(Dr)
                IPeriod = IPeriod + 1
            Loop

            'CdsDt.Last
            LastPeriod = tbPeriode.Text
            If ddlLoanType.SelectedValue = "Berjangka" Then
                LastPeriod = tbPeriode.Text
            End If

            Sisa = CFloat(tbAmountEndDt.Text)

            If ddlTerm.SelectedValue = "Week" Then
                Percent = CFloat(tbInterest.Text) / 5200
            ElseIf ddlTerm.SelectedValue = "Half Month" Then
                Percent = CFloat(tbInterest.Text) / 2400
            Else
                Percent = CFloat(tbInterest.Text) / 1200
            End If
            EditProcess = True

            'If Not CdsDt.Eof Then begin()
            '    CdsDt.Next;
            'end;

            Dim Row As DataRow
            For Each Row In ViewState("Dt").Rows
                If tbDonePaid.Text <> "Y" Then

                    Row.BeginEdit()
                    Row("DonePaid") = "N"
                    Row("AmountBegin") = Sisa
                    If (Row("Periode") = LastPeriod) Then
                        Row("AmountBunga") = CFloat(Row("AmountBegin")) * Percent
                        Row("AmountPokok") = Row("AmountBegin")
                        Row("AmountAngsur") = CFloat(Row("AmountPokok")) + CFloat(Row("AmountBunga"))
                    Else
                        Row("AmountBunga") = Row("AmountBegin") * Percent
                        If FgAll = True Then
                            Row("AmountAngsur") = Amount
                        ElseIf Row("Periode") > Period Then
                            If ddlLoanType.SelectedValue = "Angsuran" Then
                                Row("AmountAngsur") = tbangsuran.Text
                            Else
                                Row("AmountAngsur") = tbTotalPembayaran.Text / CFloat(tbQtyPeriod.Text)
                            End If
                        End If
                        Row("AmountPokok") = CFloat(Row("AmountAngsur")) - CFloat(Row("AmountBunga"))
                    End If
                    Row("AmountEnd") = CFloat(Row("AmountBegin")) - CFloat(Row("AmountPokok"))

                    If (CFloat(Row("AmountEnd")) < 0) Or (CFloat(Row("AmountPokok")) < 0) Then
                        Row("AmountBunga") = CFloat(Row("AmountBegin")) * Percent
                        Row("AmountPokok") = Row("AmountBegin")
                        Row("AmountAngsur") = CFloat(Row("AmountPokok")) + CFloat(Row("AmountBunga"))
                        Row("AmountEnd") = 0
                    End If
                    Row.EndEdit()
                Else
                    Row.BeginEdit()
                    Row("AmountBegin") = Sisa
                    Row("AmountEnd") = CFloat(Row("AmountBegin")) - CFloat(Row("AmountPokok"))
                    Row.EndEdit()
                End If
                Sisa = Row("AmountEnd")
            Next

            'CdsDt.Filter :=  'AmountAngsur <= 0';
            'CdsDt.Filtered := True;

            'CdsDt.First;
            'WHILE NOT CdsDt.Eof DO CdsDt.Delete;

            'CdsDt.Filter :=  '';
            'CdsDt.Filtered := False;

        Catch ex As Exception
            Throw New Exception("Count Dt Error : " + ex.ToString)
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

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            If (ddlAdjustType.SelectedValue = "") Or (CFloat(tbExtend.Text) = 0) Then
                lbStatus.Text = MessageDlg("Extend must have value")
                tbExtend.Focus()
                Exit Sub
            End If
            If ddlAdjustType.SelectedValue = "All Forward" Then
                bindDataSetChangeClaimAll()
            Else
                bindDataSetChangeClaim()
            End If

        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetChangeClaimAll()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox

            Dim CekKey As String
            Dim dr As DataRow
            For Each GVR In GridDt.Rows

                CB = GVR.FindControl("cbSelect")

                CekKey = TrimStr(GVR.Cells(2).Text)

                If CekExistData(ViewState("Dt"), "Periode", CekKey) Then
                    dr = ViewState("Dt").Select("Periode = " + CekKey)(0)
                    If TrimStr(dr("DonePaid")) = "N" Then
                        dr.BeginEdit()
                        dr("Periode") = TrimStr(GVR.Cells(2).Text)
                        dr("StartClaim") = DateAdd("D", CFloat(tbExtend.Text), TrimStr(GVR.Cells(3).Text))
                        dr("AmountBegin") = FormatNumber(TrimStr(GVR.Cells(4).Text), ViewState("DigitCurr"))
                        dr("AmountAngsur") = FormatNumber(TrimStr(GVR.Cells(5).Text), ViewState("DigitCurr"))
                        dr("AmountPokok") = FormatNumber(TrimStr(GVR.Cells(6).Text), ViewState("DigitCurr"))
                        dr("AmountBunga") = FormatNumber(TrimStr(GVR.Cells(7).Text), ViewState("DigitCurr"))
                        dr("AmountEnd") = FormatNumber(TrimStr(GVR.Cells(8).Text), ViewState("DigitCurr"))
                        dr("Remark") = TrimStr(GVR.Cells(9).Text)
                        dr("DonePaid") = "N"
                        dr.EndEdit()
                    End If
                End If
            Next

            lbStatus.Text = MessageDlg("Change Start Claim Success")

            BindGridDt(ViewState("Dt"), GridDt)

            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("bindDataSetChangeClaimAll Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetChangeClaim()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox

            Dim HaveSelect As Boolean
            Dim CekKey As String
            Dim dr As DataRow
            HaveSelect = False
            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked Then
                    CekKey = TrimStr(GVR.Cells(2).Text)
                    HaveSelect = True
                    If CekExistData(ViewState("Dt"), "Periode", CekKey) Then
                        dr = ViewState("Dt").Select("Periode = " + CekKey)(0)
                        dr.BeginEdit()
                        dr("Periode") = TrimStr(GVR.Cells(2).Text)
                        dr("StartClaim") = DateAdd("D", CFloat(tbExtend.Text), TrimStr(GVR.Cells(3).Text))
                        dr("AmountBegin") = FormatNumber(TrimStr(GVR.Cells(4).Text), ViewState("DigitCurr"))
                        dr("AmountAngsur") = FormatNumber(TrimStr(GVR.Cells(5).Text), ViewState("DigitCurr"))
                        dr("AmountPokok") = FormatNumber(TrimStr(GVR.Cells(6).Text), ViewState("DigitCurr"))
                        dr("AmountBunga") = FormatNumber(TrimStr(GVR.Cells(7).Text), ViewState("DigitCurr"))
                        dr("AmountEnd") = FormatNumber(TrimStr(GVR.Cells(8).Text), ViewState("DigitCurr"))
                        dr("Remark") = TrimStr(GVR.Cells(9).Text)
                        dr("DonePaid") = "N"
                        dr.EndEdit()
                    End If
                End If
            Next
            If HaveSelect = False Then
                lbStatus.Text = MessageDlg("No Period Selected")
                Exit Sub
            Else
                lbStatus.Text = MessageDlg("Change Start Claim Success")
            End If
            BindGridDt(ViewState("Dt"), GridDt)
            'BindGridDtView()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("bindDataGridCustType Error : " + ex.ToString)
        End Try
    End Sub
End Class