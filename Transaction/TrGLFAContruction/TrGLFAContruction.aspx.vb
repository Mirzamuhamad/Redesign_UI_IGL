Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrGLFAContruction_TrGLFAContruction
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFAContructionHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
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
                If ViewState("Sender") = "btnLocation" Then
                    tbLocation.Text = Session("Result")(0).ToString
                    tbLocationName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnReff" Then
                    tbReff.Text = Session("Result")(0).ToString
                    tbFAName.Text = Session("Result")(1).ToString
                    tbReff_TextChanged(Nothing, Nothing)
                    totalingDt()
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
        FillCombo(ddlFASubGroup, "SELECT FA_SubGrp_Code, FA_SubGrp_Name from VMsFAGroupSub WHERE Fg_Expendable = 'N' AND Fg_Process = 'Y'", True, "FA_SubGrp_Code", "FA_SubGrp_Name", ViewState("DBConnection"))
        FillCombo(ddlFAStatus, "EXEC S_GetFAStatus", True, "FAStatusCode", "FAStatusName", ViewState("DBConnection"))
        FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        
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
            GetStringHd = "Select * From V_GLFAContructionHd "
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLFAContructionDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_GLFormFAContruction " + Result
                Session("ReportFile") = ".../../../Rpt/FormGLFAContruction.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_GLFAContruction", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDate.Enabled = State
            btnReff.Visible = State
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
                If ViewState("DtValue") <> tbPONo.Text + "|" + tbAccount.Text.Trim + "|" + tbCostCtrDt.Text.Trim + "|" + tbCurr.Text.Trim Then
                    If CekExistData(ViewState("Dt"), "Reference+'|'+Account+'|'+CostCtr+'|'+Currency", tbPONo.Text + "|" + tbAccount.Text.Trim + "|" + tbCostCtrDt.Text.Trim + "|" + tbCurr.Text.Trim) Then
                        lbStatus.Text = "Reference " + tbPONo.Text + " Account " + tbAccountName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Reference+'|'+Account+'|'+CostCtr+'|'+Currency = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Reference") = tbPONo.Text
                Row("TransType") = tbTransType.Text
                Row("Account") = tbAccount.Text
                Row("Description") = tbAccountName.Text
                Row("CostCtr") = tbCostCtrDt.Text
                Row("Date") = tbRRDate.SelectedDate
                Row("Currency") = tbCurr.Text
                Row("ForexRate") = FormatFloat(tbRate.Text, ViewState("DigitRate").ToString)
                Row("AmountForex") = FormatFloat(tbForex.Text, ViewState("DigitCurr").ToString)
                Row("AmountHome") = FormatFloat(tbHome.Text, ViewState("DigitHome").ToString)
                Row("FgAddValue") = ddlAddValue.SelectedValue
                Row("Remark") = tbRemarkDt.Text

                If Not tbRRDate.Visible Then
                    Row("Date") = DBNull.Value
                Else
                    Row("Date") = tbRRDate.SelectedValue
                End If
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Reference+'|'+Account+'|'+CostCtr+'|'+Currency ", tbPONo.Text + "|" + tbAccount.Text.Trim + "|" + tbCostCtrDt.Text.Trim + "|" + tbCurr.Text.Trim) Then
                    lbStatus.Text = "Reference " + tbPONo.Text + " Account " + tbAccountName.Text + "'  has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Reference") = tbPONo.Text
                dr("PRNo") = tbTransType.Text
                dr("Account") = tbAccount.Text
                dr("AccountName") = tbAccountName.Text
                dr("CostCtr") = tbCostCtrDt.Text
                dr("Date") = tbRRDate.SelectedDate
                dr("ForexRate") = FormatFloat(tbRate.Text, ViewState("DigitRate").ToString)
                dr("AmountForex") = FormatFloat(tbForex.Text, ViewState("DigitCurr").ToString)
                dr("AmountHome") = FormatFloat(tbHome.Text, ViewState("DigitHome").ToString)
                dr("FgAddValue") = ddlAddValue.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            totalingDt()
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
                tbReference.Text = GetAutoNmbr("FAC", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO GLFAContructionHd (TransNmbr, Status, TransDate, ContructionIP, FACode, FAName, FASubGroup, FAStatus, FALocationType, FALocationCode, LifeMonth, " + _
                "Currency, TotalForex, TotalExpense, CostCtr, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbReff.Text) + ", " + QuotedStr(tbFA.Text) + ", " + QuotedStr(tbFAName.Text) + ", " + _
                QuotedStr(ddlFASubGroup.SelectedValue) + ", " + QuotedStr(ddlFAStatus.SelectedValue) + ", " + _
                QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(tbLocation.Text) + ", " + _
                tbLife.Text.Replace(",", "") + ", " + QuotedStr(ViewState("Currency").ToString) + ", " + _
                 tbTotalForex.Text.Replace(",", "") + ", " + tbTotalExpense.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlCostCtr.SelectedValue) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFAContructionHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFAContructionHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", ContructionIP = " + QuotedStr(tbReff.Text) + _
                ", FACode = " + QuotedStr(tbFA.Text) + ", FAName = " + QuotedStr(tbFAName.Text) + _
                ", FASubGroup = " + QuotedStr(ddlFASubGroup.SelectedValue) + ", FAStatus = " + QuotedStr(ddlFAStatus.SelectedValue) + _
                ", FALocationType = " + QuotedStr(ddlType.SelectedValue) + ", FALocationCode = " + QuotedStr(tbLocation.Text) + ", LifeMonth = " + tbLife.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + ", TotalExpense = " + tbTotalExpense.Text.Replace(",", "") + ", CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbReference.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, TransType, Reference, Account, CostCtr, Date, Currency, ForexRate, AmountForex, AmountHome, FgAddValue, Remark FROM GLFAContructionDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFAContructionDt")

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

            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbReference.Text
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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnAddDt.Visible = False
            btnAddDt2.Visible = False
            btnHome.Visible = False

            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbDate.SelectedDate = ViewState("ServerDate")
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbReff.Text = ""
            tbFA.Text = ""
            tbFAName.Text = ""
            ddlFASubGroup.SelectedValue = ""
            'ddlFAStatus.SelectedValue = ""
            ddlType.SelectedIndex = 0
            tbLocation.Text = ""
            tbLocationName.Text = ""
            tbLife.Text = "0"
            tbTotalForex.Text = "0"
            tbTotalExpense.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbPONo.Text = ""
            tbTransType.Text = ""
            tbAccount.Text = ""
            tbAccountName.Text = ""
            tbCurr.Text = ""
            tbRate.Text = ""
            ddlAddValue.SelectedValue = "Y"
            tbHome.Text = "0"
            tbForex.Text = "0"
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
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
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

    'Protected Sub tbFACode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPRNo.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Dim SQLString As String
    '    Try
    '        If Request.QueryString("ContainerId").ToString = "TrGLFAContructionID" Then
    '            SQLString = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " AND FACode = " + QuotedStr(tbFACode.Text.Trim)
    '        ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
    '            SQLString = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " AND FACode = " + QuotedStr(tbFACode.Text.Trim)
    '        Else
    '            SQLString = "SELECT FixedAsset AS FACode, FixedAssetName As FAName, Qty_Borrow AS Qty FROM V_GLFAContructionDt WHERE Qty_Borrow > 0 AND TransNmbr = " + QuotedStr(tbReff.Text) + " AND FixedAsset = " + QuotedStr(tbPRNo.Text.Trim)
    '        End If
    '        Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbPRNo.Text = Dr("FACode")
    '            tbPONo.Text = Dr("FAName")
    '            tbQty.Text = Dr("Qty")
    '            tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
    '        Else
    '            tbPRNo.Text = ""
    '            tbPONo.Text = ""
    '            tbQty.Text = "1"
    '        End If
    '        tbPRNo.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb FixedAsset Code TextChanged : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbTransType.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbReff.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Contruction IP No must have value")
                btnReff.Focus()
                Return False
            End If
            If tbFA.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Fixed Asset must have value")
                tbFA.Focus()
                Return False
            End If
            If tbFAName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Fixed Asset Name must have value")
                tbFAName.Focus()
                Return False
            End If
            If ddlFASubGroup.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("FA Sub Group must have value")
                ddlFASubGroup.Focus()
                Return False
            End If
            If ddlFAStatus.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("FA Status must have value")
                ddlFAStatus.Focus()
                Return False
            End If
            If tbLocation.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("FA Location must have value")
                tbLocation.Focus()
                Return False
            End If
            If ddlCostCtr.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Cost Center must have value")
                ddlCostCtr.Focus()
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

            Else
                If tbPONo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("PO No Must Have Value")
                    tbPONo.Focus()
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
            FilterName = "No, Date, Status"
            FilterValue = "TransNmbr, Trans_Date, Status"
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
            If Not (e.CommandName = "Sort") Or (e.CommandName = "Page") Then
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
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        btnAddDt.Visible = False
                        btnAddDt2.Visible = False

                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_GLFormFAContruction ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormGLFAContruction.frx"
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
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As DataRow = ViewState("Dt").Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Reference+'|'+Account+'|'+CostCtr+'|'+Currency = " + QuotedStr(GVR("Reference").ToString + "|" + GVR("Account").ToString) + "|" + GVR("CostCtr").ToString + "|" + GVR("Currency").ToString)
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As DataRow
        Try
            GVR = ViewState("Dt").Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR("Reference").ToString + "|" + GVR("Account").ToString + "|" + GVR("CostCtr").ToString + "|" + GVR("Currency").ToString
            FillTextBoxDt(GVR("Reference").ToString + "|" + GVR("Account").ToString + "|" + GVR("CostCtr").ToString + "|" + GVR("Currency").ToString)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbTransType.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Private Sub totalingDt()
        Dim dr As DataRow
        Dim Forex, Expense As Double
        Try
            Forex = 0
            Expense = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    If dr("FgAddValue").ToString = "Y" Then
                        Forex = Forex + CFloat(dr("AmountHome").ToString)
                    Else
                        Expense = Expense + CFloat(dr("AmountHome").ToString)
                    End If

                End If
            Next
            tbTotalForex.Text = FormatNumber(Forex, CInt(ViewState("DigitCurr")))
            tbTotalExpense.Text = FormatNumber(Expense, CInt(ViewState("DigitCurr")))
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbReff, Dt.Rows(0)("ContructionIP").ToString)
            BindToText(tbFA, Dt.Rows(0)("FACode").ToString)
            BindToText(tbFAName, Dt.Rows(0)("FAName").ToString)
            BindToDropList(ddlFASubGroup, Dt.Rows(0)("FASubGroup").ToString)
            BindToDropList(ddlFAStatus, Dt.Rows(0)("FAStatus").ToString)
            BindToDropList(ddlType, Dt.Rows(0)("FALocationType").ToString)
            BindToText(tbLocation, Dt.Rows(0)("FALocationCode").ToString)
            BindToText(tbLocationName, Dt.Rows(0)("FALocationName").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            tbTotalExpense.Text = FormatFloat(Dt.Rows(0)("TotalExpense").ToString, ViewState("DigitHome"))
            tbTotalForex.Text = FormatFloat(Dt.Rows(0)("TotalForex").ToString, ViewState("DigitHome"))
            BindToText(tbLife, Dt.Rows(0)("LifeMonth").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Reference As String)
        Dim Dr As DataRow()
        Try
            'Dr = ViewState("Dt").select("Account+'|'+Reference = " + QuotedStr(Reference))
            Dr = ViewState("Dt").select("Reference+'|'+Account+'|'+CostCtr+'|'+Currency = " + QuotedStr(Reference))

            If Dr.Length > 0 Then
                BindToText(tbTransType, Dr(0)("TransType").ToString)
                BindToText(tbPONo, Dr(0)("Reference").ToString)
                BindToText(tbAccount, Dr(0)("Account").ToString)
                BindToText(tbAccountName, Dr(0)("Description").ToString)
                BindToText(tbCurr, Dr(0)("Currency").ToString)
                BindToText(tbCostCtrDt, Dr(0)("CostCtr").ToString)
                BindToDate(tbRRDate, Dr(0)("Date").ToString)
                BindToDropList(ddlAddValue, Dr(0)("FgAddValue").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                tbRate.Text = FormatFloat(Dr(0)("ForexRate").ToString, ViewState("DigitRate"))
                If tbCurr.Text = ViewState("Currency") Then
                    tbForex.Text = FormatFloat(Dr(0)("AmountForex").ToString, ViewState("DigitHome"))
                    ViewState("DigitCurr") = ViewState("DigitHome")
                Else
                    ViewState("DigitCurr") = SQLExecuteScalar("Select dbo.DigitCurrForex('" + Dr("Currency").ToString + "') AS Digit", ViewState("DBConnection"))
                    tbForex.Text = FormatFloat(Dr(0)("AmountForex").ToString, ViewState("DigitCurr"))
                End If
                tbHome.Text = FormatFloat(Dr(0)("AmountHome").ToString, ViewState("DigitHome"))
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

    Private Function FindFixedAsset(ByVal Nmbr As String, ByVal FixedAsset As String) As String
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_STRROtherFindProd " + QuotedStr(tbTransType.Text), ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("FixedAsset")
    End Function


    Protected Sub btnLocation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocation.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT FA_Location_Code, FA_Location_Name FROM VMsFALocationAll where FA_Location_Type = " + QuotedStr(ddlType.SelectedValue)
            ResultField = "FA_Location_Code, FA_Location_Name"
            ViewState("Sender") = "btnLocation"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Source Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        tbLocation.Text = ""
        tbLocationName.Text = ""
    End Sub

    Protected Sub btnReff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReff.Click
        Dim ResultField As String
        Try
            Session("Filter") = "Select CIPCode, CIPName, StartDate, EndDate, Account from MsContructionIP WHERE EndDate IS NULL"
            ResultField = "CIPCode, CIPName"
            ViewState("Sender") = "btnReff"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Reference Error : " + ex.ToString
        End Try
    End Sub
    
    Protected Sub tbLocation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLocation.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindFALocationAll '" + tbLocation.Text + "', '" + ddlType.SelectedValue + "'", ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbLocation.Text = Dr("FA_Location_Code")
                tbLocationName.Text = Dr("FA_Location_Name")
            Else
                tbLocation.Text = ""
                tbLocationName.Text = ""
            End If
            tbLocation.Focus()
        Catch ex As Exception
            Throw New Exception("tb sourceError : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbReff_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReff.TextChanged
        Dim DS As DataSet
        Dim NewRow As DataRow
        Dim DigitCurr As String
        Try
            DS = SQLExecuteQuery("EXEC S_GLFAContructionGetDt " + QuotedStr(tbReff.Text), ViewState("DBConnection").ToString)
            
            For Each Dr In DS.Tables(0).Rows
                NewRow = ViewState("Dt").NewRow
                NewRow("TransType") = Dr("TransClass")
                NewRow("Reference") = Dr("Reference")
                NewRow("Date") = Dr("TransDate")
                NewRow("Account") = Dr("Account")
                NewRow("Description") = Dr("AccountName")
                NewRow("CostCtr") = Dr("CostCtr")
                NewRow("Currency") = Dr("Currency")
                NewRow("ForexRate") = FormatFloat(Dr("Rate"), ViewState("DigitRate").ToString)
                If Dr("Currency") = ViewState("Currency") Then
                    NewRow("AmountForex") = FormatFloat(Dr("AmountForex"), ViewState("DigitHome").ToString)
                Else
                    DigitCurr = SQLExecuteScalar("Select dbo.DigitCurrForex('" + Dr("Currency").ToString + "') AS Digit", ViewState("DBConnection"))
                    NewRow("AmountForex") = FormatFloat(Dr("AmountForex"), DigitCurr)
                End If
                NewRow("AmountHome") = FormatFloat(Dr("AmountHome"), ViewState("DigitHome").ToString)
                NewRow("FgAddValue") = "Y"
                NewRow("Remark") = ""
                ViewState("Dt").Rows.Add(NewRow)
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "tbReff_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
