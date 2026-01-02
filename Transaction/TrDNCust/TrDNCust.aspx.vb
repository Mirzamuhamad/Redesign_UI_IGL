Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrDNCust
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FNDNCustHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlCostCenter, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                'FillCombo(ddlPType, "EXEC S_GetProductType", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    tbCustName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(3).ToString)
                    BindToDropList(ddlTerm, Session("Result")(4).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                End If
                If ViewState("Sender") = "btnAccount" Then
                    tbAccount.Text = Session("Result")(0).ToString
                    tbAccountName.Text = Session("Result")(1).ToString
                    tbFgSubled.Text = Session("Result")(2).ToString
                    tbFgCostCtr.Text = Session("Result")(4).ToString
                    tbSubled.Enabled = tbFgSubled.Text <> "N"
                    btnSubled.Visible = tbSubled.Enabled
                    ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
                    If tbFgSubled.Text = "N" Then
                        tbSubled.Text = ""
                        tbSubledName.Text = ""
                    End If
                    If tbFgCostCtr.Text = "N" Then
                        ddlCostCenter.SelectedIndex = 0
                    End If
                    btnSubled.Enabled = tbSubled.Enabled

                    'tbSubled.Enabled = tbFgSubled.Text <> "N"
                    'btnSubled.Visible = tbSubled.Enabled
                    'tbSubled.Text = ""
                    'tbSubledName.Text = ""
                    'ChangeFgSubLed(tbFgSubled, tbSubled, btnSubled)
                End If

                If ViewState("Sender") = "btnSubled" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
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
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        Me.tbBaseForex.Attributes.Add("ReadOnly", "True")
        Me.tbPPNForex.Attributes.Add("ReadOnly", "True")
        Me.tbTotalForex.Attributes.Add("ReadOnly", "True")
        Me.tbAmountForex.Attributes.Add("ReadOnly", "True")

        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Me.tbBaseForex.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        Me.tbDisc.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'%'); setformat();")
        Me.tbDiscForex.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")
        Me.tbPPN.Attributes.Add("OnBlur", "BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ",'-'); setformat();")

        Me.tbQty.Attributes.Add("OnBlur", "kali(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbAmountForex.ClientID + "); setformatdt();")
        Me.tbPrice.Attributes.Add("OnBlur", "kali(" + tbQty.ClientID + "," + tbPrice.ClientID + "," + tbAmountForex.ClientID + "); setformatdt();")
        'Me.tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNDNCustDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
        Dim HaveCheck As Boolean

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

                HaveCheck = False
                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked = True Then
                        HaveCheck = True
                    End If
                Next

                If HaveCheck = True Then
                    For Each GVR In GridView1.Rows
                        CB = GVR.FindControl("cbSelect")
                        If CB.Checked Then
                            If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                                lbStatus.Text = "Data must be Posting first!"
                                Exit Sub
                            End If

                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    Next
                Else
                    lbStatus.Text = "Data must have at least 1 selected!"
                    Exit Sub
                End If
                Result = Result + "'"

                Session("SelectCommand") = "EXEC S_FNFormCNDNCust " + Result + ", 'DN' "
                Session("ReportFile") = ".../../../Rpt/FormCNDNCust.frx"
                Session("DBConnection") = ViewState("DBConnection")
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNDNCust", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlReport.Enabled = State
            tbCustCode.Enabled = State
            btnCust.Visible = State
            ddlCurr.Enabled = State
            'ddlPType.Enabled = State
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
                If ViewState("DtValue") <> lbItemNo.Text Then
                    If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
                        lbStatus.Text = "Item No " + lbItemNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Account") = tbAccount.Text
                Row("AccountName") = tbAccountName.Text
                Row("FgSubled") = tbFgSubled.Text
                Row("Subled") = tbSubled.Text
                Row("SubledName") = tbSubledName.Text
                Row("CostCtr") = ddlCostCenter.SelectedValue
                Row("Cost_Ctr_Name") = ddlCostCenter.SelectedItem.Text
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                    Row("Cost_Ctr_Name") = DBNull.Value
                End If
                Row("Qty") = tbQty.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("PriceForex") = tbPrice.Text
                Row("AmountForex") = tbAmountForex.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) = True Then
                    lbStatus.Text = "Item No " + lbItemNo.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("Account") = tbAccount.Text
                dr("AccountName") = tbAccountName.Text
                dr("FgSubled") = tbFgSubled.Text
                dr("Subled") = tbSubled.Text
                dr("SubledName") = tbSubledName.Text
                dr("CostCtr") = ddlCostCenter.SelectedValue
                dr("Cost_Ctr_Name") = ddlCostCenter.SelectedItem.Text
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                    dr("Cost_Ctr_Name") = DBNull.Value
                End If
                dr("Qty") = tbQty.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("PriceForex") = tbPrice.Text
                dr("AmountForex") = tbAmountForex.Text
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

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If tbPpnRate.Text.Trim = "" Then
                'tbPpnRate.Text = tbRate.Text
                If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                    tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
                Else
                    tbPpnRate.Text = FormatNumber(1, ViewState("DigitCurr"))
                End If
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbRef.Text = GetAutoNmbr("DNC", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINDNCustHd (TransNmbr, FgReport, Status, TransDate, Customer, Attn, " + _
                "Term, DueDate, PPnNo, PPnDate, PPnRate, Currency, ForexRate, BaseForex, Disc, DiscForex, PPn, PPnForex, TotalForex, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + QuotedStr("Y") + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCustCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedDate, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + tbDisc.Text.Replace(",", "") + ", " + tbDiscForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + _
                tbPPNForex.Text.Replace(",", "") + ", " + tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDNCustHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)

                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE FINDNCustHd SET Customer = " + QuotedStr(tbCustCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", Term = " + QuotedStr(ddlTerm.SelectedValue) + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + _
                ", PPnDate = '" + Format(tbPPndate.SelectedDate, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", DueDate = '" + Format(tbDueDate.SelectedDate, "yyyy-MM-dd") + "'" + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + ", Disc = " + tbDisc.Text.Replace(",", "") + ", DiscForex = " + tbDiscForex.Text.Replace(",", "") + _
                ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + ""
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
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Account, FgSubled, Subled, CostCtr, Qty, Unit, PriceForex, AmountForex, Remark FROM FINDNCustDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINDNCustDt SET ItemNo = @ItemNo, Account = @Account, " + _
            "FgSubled = @FgSubled, Subled = @Subled, CostCtr = @CostCtr, " + _
            "Qty = @Qty, Unit = @Unit, PriceForex = @PriceForex, AmountForex = @AmountForex, " + _
            "Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 12, "Account")
            Update_Command.Parameters.Add("@FgSubled", SqlDbType.VarChar, 1, "FgSubled")
            Update_Command.Parameters.Add("@Subled", SqlDbType.VarChar, 20, "Subled")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 5, "CostCtr")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 22, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 22, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 22, "AmountForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            
            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINDNCustDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINDNCustDt")

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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            Else
                tbPpnRate.Text = FormatNumber(1, ViewState("DigitCurr"))
            End If
            btnHome.Visible = False
            'ddlReport.Focus()
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
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbAttn.Text = ""
            ddlTerm.SelectedIndex = 0
            'ddlPType.SelectedIndex = 0
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
            tbPpnRate.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbBaseForex.Text = "0"
            tbDisc.Text = "0"
            tbDiscForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            EnableHd(True)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbAccount.Text = ""
            tbAccountName.Text = ""
            tbFgSubled.Text = "N"
            tbSubled.Text = ""
            tbSubledName.Text = ""
            tbSubled.Enabled = False
            btnSubled.Visible = False
            'ddlCostCenter.SelectedIndex = 0
            ddlCostCenter.SelectedValue = ddlCostCtr.SelectedValue
            'ddlCurr.SelectedValue = ViewState("Currency")
            tbQty.Text = "0"
            tbPrice.Text = FormatFloat(0, ViewState("DigitCurr").ToString)
            tbAmountForex.Text = FormatFloat(0, ViewState("DigitCurr").ToString)
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

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsCustomer"
            ResultField = "Customer_Code, Customer_Name, Currency, Contact_Person, Term"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                BindToDropList(ddlCurr, Dr("Currency"))
                BindToText(tbAttn, Dr("Contact_Person"))
                BindToDropList(ddlTerm, Dr("Term"))
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency")
                tbAttn.Text = ""
            End If
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            AttachScript("setformat();", Page, Me.GetType())
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        tbPPnNo.Enabled = True
        tbPPndate.Enabled = True
        tbPpnRate.Enabled = ddlCurr.SelectedValue <> ViewState("Currency")
        If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")                
            tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitRate"))
        Else
            tbPpnRate.Text = FormatNumber(1, ViewState("DigitRate"))
        End If
        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
        AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            btnAccount.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Transaction Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If ddlCostCtr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Cost Center must have value")
                ddlCostCtr.Focus()
                Return False
            End If
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                btnCust.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
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
                If Dr("Account").ToString = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    Return False
                End If
                If Dr("FgSubled").ToString <> "N" And TrimStr(Dr("SubLed").ToString) = "" Then
                    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                    Return False
                End If
                If Dr("FgCostCtr").ToString = "Y" And Dr("CostCtr").ToString = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("PriceForex").ToString = "0" Or Dr("PriceForex").ToString = "" Then
                    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                    Return False
                End If
                If Dr("AmountForex").ToString = "0" Or Dr("AmountForex").ToString = "" Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    Return False
                End If
            Else
                If tbAccount.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    tbAccount.Focus()
                    Return False
                End If
                If tbFgSubled.Text <> "N" Then
                    If tbSubledName.Text = "" Then
                        lbStatus.Text = MessageDlg("SubLed Have Value")
                        tbSubled.Focus()
                        Return False
                    End If
                End If
                If tbFgCostCtr.Text = "Y" Then
                    If ddlCostCenter.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Cost Center Have Value")
                        ddlCostCenter.Focus()
                        Return False
                    End If
                End If
                If tbQty.Text = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text.Replace(",", "")) = 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If tbPrice.Text = "" Then
                    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                    tbPrice.Focus()
                    Return False
                End If
                If CFloat(tbPrice.Text.Replace(",", "")) = 0 Then
                    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                    tbPrice.Focus()
                    Return False
                End If
                If tbAmountForex.Text = "0" Or tbAmountForex.Text = "" Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    tbAmountForex.Focus()
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
            FDateName = "Date, Due Date"
            FDateValue = "TransDate, DueDate"
            FilterName = "Reference, Date, Status, Customer Code, Customer Name, Term, Due Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Customer_Code, Customer_Name, Term_Name, dbo.FormatDate(DueDate), Remark"
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
                    ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
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
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        tbRate.Enabled = tbPpnRate.Enabled
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                            lbStatus.Text = "Data must be Posting first!"
                            Exit Sub
                        End If

                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormDNCust '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormDNCust.frx"
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
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        'Dim Dr As DataRow
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = lbItemNo.Text
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Enabled = tbSubled.Enabled
            ddlCostCenter.Enabled = tbFgCostCtr.Text = "Y"
            btnAccount.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCust.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Customer Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTerm')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Term Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0

    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
                    AttachScript("BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer_Code").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term_Code").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbDisc, Dt.Rows(0)("Disc").ToString, ViewState("DigitPercent"))
            BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            'BindToDropList(ddlPType, Dt.Rows(0)("ProductType").ToString)
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
                lbItemNo.Text = ItemNo.ToString
                BindToText(tbAccount, Dr(0)("Account").ToString)
                BindToText(tbAccountName, Dr(0)("AccountName").ToString)
                BindToText(tbFgSubled, Dr(0)("FgSubled").ToString)
                BindToText(tbSubled, Dr(0)("Subled").ToString)
                BindToText(tbSubledName, Dr(0)("SubledName").ToString)
                BindToText(tbFgCostCtr, Dr(0)("FgCostCtr").ToString)
                BindToDropList(ddlCostCenter, Dr(0)("CostCtr").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToText(tbPrice, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
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

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
    'End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccount.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsAccountDt WHERE TransType = 'DNC' AND ( Currency = " + QuotedStr(ddlCurr.SelectedValue) + " OR Currency = " + QuotedStr(ViewState("Currency").ToString) + ") "
            ResultField = "Account, Description, FgSubled, Currency,FgCostCtr"
            ViewState("Sender") = "btnAccount"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Account Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            If tbFgSubled.Text = "N" Then
                Exit Sub
            End If
            Session("filter") = "SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgSubled = " + QuotedStr(tbFgSubled.Text)
            ResultField = "Subled_No, Subled_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Subled click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbSubled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubled.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Subled", tbFgSubled.Text + "|" + tbSubled.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubled.Text = Dr("Subled_No")
                tbSubledName.Text = Dr("Subled_Name")
            Else
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
            tbSubled.Focus()
        Catch ex As Exception
            Throw New Exception("tb Subled Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccount.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Try
            Dt = SQLExecuteQuery("SELECT * FROM V_MsAccountDt WHERE TransType = 'DNC' AND ( Currency = " + QuotedStr(ddlCurr.SelectedValue) + " OR Currency = " + QuotedStr(ViewState("Currency").ToString) + ") AND Account = " + QuotedStr(tbAccount.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAccount.Text = Dr("Account")
                tbAccountName.Text = Dr("Description")
                tbFgSubled.Text = TrimStr(Dr("FgSubled").ToString)
                tbSubled_TextChanged(Nothing,Nothing)
                tbFgCostCtr.Text = TrimStr(Dr("FgCostCtr").ToString)
                tbSubled.Enabled = (tbFgSubled.Text <> "N")
            Else
                tbAccount.Text = ""
                tbAccountName.Text = ""
                tbFgSubled.Text = ""
                tbSubled.Text = ""
                tbSubledName.Text = ""
                tbSubled.Enabled = False
            End If
            btnSubled.Visible = tbSubled.Enabled
            ddlCostCenter.Enabled = tbFgCostCtr.Text <> "N"
            If tbFgCostCtr.Text = "N" Then
                ddlCostCenter.SelectedIndex = 0
            End If
            'tbSubled.Focus()
        Catch ex As Exception
            Throw New Exception("tb Subled Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPPndate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPndate.SelectionChanged
        Try
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")                
                tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitRate"))
                tbPpnRate.Enabled = True
            Else
                tbPpnRate.Text = FormatNumber(1, ViewState("DigitRate"))
                tbPpnRate.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("tbPPndate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
