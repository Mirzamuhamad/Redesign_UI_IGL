Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class JListing
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLJournalHDAll WHERE TransClass IS NOT NULL "

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
                If ViewState("Sender") = "btnAcc" Then
                    tbAccCode.Text = Session("Result")(0).ToString
                    tbAccName.Text = Session("Result")(1).ToString
                    tbfgSubled.Text = Session("Result")(2).ToString
                    BindToDropList(ddlCurr, Session("Result")(3))
                    tbSubled.Enabled = tbfgSubled.Text <> "N"
                    btnSubled.Visible = tbSubled.Enabled
                    ChangeCurrency(ddlCurr, tbDate, tbRate, Session("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    If tbfgSubled.Text = "N" Then
                        tbSubled.Text = ""
                        tbSubledName.Text = ""
                    End If
                End If
                If ViewState("Sender") = "btnSubled" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        MaxItem = GetNewItemNo(ViewState("Dt"))

                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("ItemNo") = MaxItem
                        dr("Account") = drResult("Account")
                        dr("AccountName") = drResult("Description")
                        dr("Subled") = ""
                        dr("Costctr") = DBNull.Value
                        dr("FgSubled") = drResult("FgSubled")
                        dr("Remark") = ""
                        dr("Currency") = drResult("Currency")
                        dr("ForexRate") = GetCurrRate(drResult("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection"))
                        dr("DebitForex") = 0
                        dr("DebitHome") = 0
                        dr("CreditForex") = 0
                        dr("CreditHome") = 0
                        dr("TransClass") = "JE"
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
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
        FillRange(ddlRange)
        FillCombo(ddlJE, "EXEC S_GetTransType", False, "Trans_Type_Code", "Trans_Type_Name", ViewState("DBConnection"))
        FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlTransClass, "EXEC S_GetMsTransTypeJurnal", True, "Trans_Type", "Trans_Type_Name", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("SortExpressionDt") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        ddlCommand.Items.Remove("Complete")
        ddlCommand.Items.Remove("Delete")
        ddlCommand2.Items.Remove("Complete")
        ddlCommand2.Items.Remove("Delete")
        BtnAdd.Visible = False
        btnAdd2.Visible = False
        ddlCommand2.Visible = False

        tbDebitHome.Attributes.Add("ReadOnly", "True")
        tbCreditHome.Attributes.Add("ReadOnly", "True")
        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDebitForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbCreditForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbRate.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        Me.tbDebitForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        Me.tbCreditForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, StrTransClass As String

        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            
            'If AdvanceFilter.Trim.Length > 1 And StrFilter.Trim.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Trim.Length > 1 And StrFilter.Trim.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            If ddlTransClass.SelectedIndex = 0 Then
                StrTransClass = ""
            Else
                If ddlRange.SelectedIndex = 0 Then
                    If StrFilter.Length > 5 Then
                        StrTransClass = " and TransClass = " + QuotedStr(ddlTransClass.SelectedValue)
                    Else
                        StrTransClass = " TransClass = " + QuotedStr(ddlTransClass.SelectedValue)
                    End If
                Else
                    StrTransClass = " AND TransClass = " + QuotedStr(ddlTransClass.SelectedValue)
                End If
            End If
            'lbStatus.Text = GetStringHd + StrFilter + " " + StrTransClass
            'Exit Sub
            DT = BindDataTransaction(GetStringHd, StrFilter + StrTransClass, ViewState("DBConnection").ToString)
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
            'btnAdd2.Visible = btnGo2.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
            'If DT.Rows.Count > 0 Then
            'GridView1.HeaderRow.Cells(8).Text = "Debit (" + Session("Currency") + ")"
            'GridView1.HeaderRow.Cells(9).Text = "Credit (" + Session("Currency") + ")"
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String, ByVal TransClass As String) As String
        Return "SELECT * From V_GLJournalDt WHERE (Nominal <> 0 OR NominalHome <> 0) and Reference = " + QuotedStr(Nmbr) + " AND TransClass = " + QuotedStr(TransClass) + " Order By ItemNo "
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            btnGetDt.Visible = State
            tbRef.Enabled = State
            ddlJE.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String, ByVal TransClass As String)
        Try
            Dim dt As New DataTable
            Dim DV As DataView
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens, TransClass), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            DV = dt.DefaultView
            BindGridDt(dt, GridDt)
            GridDt.Columns(0).Visible = False
            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "ItemNo DESC"
            End If
            DV.Sort = ViewState("SortExpressionDt")
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
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
            'Save Hd
            If ViewState("StateHd") = "Insert" Then
                tbRef.Text = GetAutoNmbr("JE", "X", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO GLJournalHd (Reference, TransClass, Status, TransDate, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbRef.Text + "', " + QuotedStr(ddlJE.SelectedValue) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                SQLString = "UPDATE GLJOURNALHD SET FormatJE = '" + ddlJE.SelectedValue + "', Remark = '" + tbRemark.Text + "'," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
                " WHERE Reference = '" + tbRef.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Reference IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Reference") = tbRef.Text
                Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Reference, TransClass, ItemNo, Account, FgSubled, Subled, CostCtr, Currency, ForexRate, DebitForex, DebitHome, CreditForex, CreditHome, Remark FROM GLJournalDt WHERE Reference = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLJournalDt")

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
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("", "")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlJE.SelectedValue = ""
            'tbDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            lbItemNo.Text = ""
            tbAccCode.Text = ""
            tbAccName.Text = ""
            tbSubled.Text = ""
            tbSubledName.Text = ""
            tbRemarkDt.Text = ""
            ddlCostCtr.SelectedIndex = 0
            tbCreditForex.Text = "0"
            tbCreditHome.Text = "0"
            tbDebitForex.Text = "0"
            tbDebitHome.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnSubled.Visible = False
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

  


    Protected Sub tbAccCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Account", tbAccCode.Text + "|JE", ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbAccCode.Text = Dr("Account")
                tbAccName.Text = Dr("AccountName")
                BindToDropList(ddlCurr, Dr("Currency"))
                ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                tbfgSubled.Text = Dr("FgSubled")
            Else
                tbAccCode.Text = ""
                tbAccName.Text = ""
                tbfgSubled.Text = "N"
            End If
            ChangeFgSubLed(tbfgSubled, tbSubled, btnSubled)
            tbAccCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb acc Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        tbRate.Focus()
        AttachScript("RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();", Page, Me.GetType())
    End Sub


    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = "Reference must have value"
            '    tbRef.Focus()
            '    Return False
            'End If
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Account").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    Return False
                End If
                If Dr("FgSubled").ToString <> "N" And Dr("Subled").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Subled Must Be Filled for FgSubled other than N")
                    Return False
                End If
                If Dr("Currency").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    Return False
                End If
                If CFloat(Dr("DebitForex").ToString) = 0 And CFloat(Dr("CreditForex").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Debit or Credit Forex must have value")
                    Return False
                End If
            Else
                If tbAccCode.Text.Trim = "" Then
                    lbStatus.Text = "Account Must Have Value"
                    tbAccCode.Focus()
                    Return False
                End If
                If tbfgSubled.Text.Trim <> "N" And tbSubled.Text.Trim = "" Then
                    lbStatus.Text = "Subled Must Be Filled for FgSubled other than N"
                    tbSubled.Focus()
                    Return False
                End If
                If ddlCurr.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    ddlCurr.Focus()
                    Return False
                End If
                If CFloat(tbRate.Text) = 0 Then
                    lbStatus.Text = "Rate must have value"
                    tbRate.Focus()
                    Return False
                End If
                If CFloat(tbDebitForex.Text) = 0 And CFloat(tbCreditForex.Text) = 0 Then
                    lbStatus.Text = "Debit or Credit Forex must have value"
                    tbDebitForex.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub ddlJE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJE.SelectedIndexChanged
        'Try
        '    If ViewState("InputFormatJE") = "Y" Then
        '        RefreshMaster("S_GetFormatJE", "Format_Code", "Format_Name", ddlJE, Session("DBConnection"))
        '        ViewState("InputFormatJE") = Nothing
        '    End If
        'Catch ex As Exception
        '    lbStatus.Text = "ddl JE Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Status, Trans Class, Remark"
            FilterValue = "Reference, Status, TransClass, Remark"
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
                    ViewState("TransClass") = GVR.Cells(5).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"), ViewState("TransClass"))
                    BindDataDt(ViewState("Reference"), ViewState("TransClass"))
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        ViewState("TransClass") = GVR.Cells(5).Text
                        GridDt.PageIndex = 0
                        FillTextBoxHd(ViewState("Reference"), ViewState("TransClass"))
                        BindDataDt(ViewState("Reference"), ViewState("TransClass"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_GLFormJournal ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(5).Text) + "''," + QuotedStr(GVR.Cells(5).Text) + "," + QuotedStr(ViewState("UserId"))
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormJournal.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select DigitDecimal FROM MsCurrency WHERE CurrCode = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection")))
            'pnlEditDt.Visible = True
            'pnlDt.Visible = False
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbAccCode.Focus()
            btnGetDt.Enabled = False
            tbSubled.Enabled = tbfgSubled.Text <> "N"
            btnSubled.Enabled = tbSubled.Enabled
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCostCtr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCostCtr.Click
        Try
            ViewState("InputCostCtr") = "Y"
            AttachScript("OpenMaster('MsCostCtr')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMaster('MsCostCtr')();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Cost Ctr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCostCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostCtr.SelectedIndexChanged
        If ViewState("InputCostCtr") = "Y" Then
            RefreshMaster("S_GetCostCtr", "Cost_Ctr_Code", "Cost_Ctr_Name", ddlCostCtr, ViewState("DBConnection"))
            ViewState("InputCostCtr") = Nothing
        End If
    End Sub

    Protected Sub lbAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAccount.Click
        Try
            AttachScript("OpenMaster('MsAccount')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMsAccount();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Account Error : " + ex.ToString
        End Try
    End Sub


    Dim CrHome As Decimal = 0
    Dim DbHome As Decimal = 0

    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    'CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
                    'DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(7).Text = "Total:"
                    ' for the Footer, display the running totals
                    e.Row.Cells(8).Text = FormatNumber(DbHome, ViewState("DigitHome"))
                    e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    e.Row.Cells(10).Text = FormatNumber(CrHome, ViewState("DigitHome"))
                    e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal TransClass As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr) + " AND TransClass = " + QuotedStr(TransClass), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlJE, Dt.Rows(0)("TransClass").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbAccCode, Dr(0)("Account").ToString)
                BindToText(tbAccName, Dr(0)("Accountname").ToString)
                BindToText(tbfgSubled, Dr(0)("FgSubLed").ToString)
                BindToText(tbSubled, Dr(0)("SubLed").ToString)
                BindToText(tbSubledName, Dr(0)("Subled_Name").ToString)
                BindToDropList(ddlCostCtr, Dr(0)("CostCtr").ToString)
                BindToDropList(ddlCurr, Dr(0)("Currency").ToString)
                BindToText(tbRate, Dr(0)("ForexRate").ToString)
                BindToText(tbDebitForex, Dr(0)("DebitForex").ToString)
                BindToText(tbCreditForex, Dr(0)("CreditForex").ToString)
                BindToText(tbDebitHome, Dr(0)("DebitHome").ToString)
                BindToText(tbCreditHome, Dr(0)("CreditHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If            
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            ddlCommand.Visible = False
            BtnGo.Visible = False
            ddlCommand2.Visible = False
            btnGo2.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from V_MsAccountDt WHERE TransType = 'JE' "
            ResultField = "Account, Description, FgSubled, Currency"
            ViewState("Sender") = "btnAcc"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
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
            GetListCommand(Status, GridView1, "3,2,5", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNPayTrade", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next
            BindData("Reference+'|'+TransClass in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Account") = tbAccCode.Text
                Row("AccountName") = tbAccName.Text
                Row("Subled") = tbSubled.Text
                Row("CostCtr") = ddlCostCtr.SelectedValue
                Row("FgSubled") = tbfgSubled.Text
                Row("Remark") = tbRemarkDt.Text
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                End If
                Row("Currency") = ddlCurr.SelectedValue
                If Row("Currency") = "" Then
                    Row("Currency") = DBNull.Value
                End If
                Row("ForexRate") = tbRate.Text
                Row("DebitForex") = tbDebitForex.Text
                Row("DebitHome") = tbDebitHome.Text
                Row("CreditForex") = tbCreditForex.Text
                Row("CreditHome") = tbCreditHome.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("TransClass") = "JE"
                dr("Account") = tbAccCode.Text
                dr("AccountName") = tbAccName.Text
                dr("Subled") = tbSubled.Text
                dr("CostCtr") = ddlCostCtr.SelectedValue
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                End If
                dr("FgSubled") = tbfgSubled.Text
                dr("Remark") = tbRemarkDt.Text
                dr("Currency") = ddlCurr.SelectedValue
                If dr("Currency") = "" Then
                    dr("Currency") = DBNull.Value
                End If
                dr("ForexRate") = tbRate.Text
                dr("DebitForex") = tbDebitForex.Text
                dr("DebitHome") = tbDebitHome.Text
                dr("CreditForex") = tbCreditForex.Text
                dr("CreditHome") = tbCreditHome.Text
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
            ddlField.SelectedValue = "Reference"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
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
            If IsNothing(ViewState("Dt")) Then
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

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Dim CriteriaField As String
        Try
            Session("Result") = Nothing
            Session("Filter") = "select * from V_MsAccountDt WHERE TransType = 'JE' "
            ResultField = "Account, Description, Currency, FgSubled"
            CriteriaField = "Account, Description, Currency, FgSubled"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = "Currency"
            'Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Subled_No, Subled_Name from V_MsSubled WHERE SubledType = '" + tbfgSubled.Text + "'"
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Subled Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDt.Sorting
        Try
            If ViewState("SortOrderDt") = Nothing Or ViewState("SortOrderDt") = "DESC" Then
                ViewState("SortOrderDt") = "ASC"
            Else
                ViewState("SortOrderDt") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrderDt")
            BindDataDt(ViewState("Reference"), ViewState("TransClass"))
        Catch ex As Exception
            lbStatus.Text = "Grid View Dt Sorting Error : " + ex.ToString
        End Try
    End Sub
End Class
