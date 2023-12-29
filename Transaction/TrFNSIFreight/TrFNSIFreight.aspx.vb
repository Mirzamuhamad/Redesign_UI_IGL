Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrFNSIFreight_TrFNSIFreight
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select Distinct TransNmbr, Nmbr, Status, TransDate, FgReport, PurchaseFreight, PONo, Supplier, Supplier_Name, SupplierName, Attn, Currency, Term, DueDate, SuppInvoice, " + _
    "Forexrate, BaseForex, PPNForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, DateAppr, ContraBonNo, ContraBonDate From V_FNSIFreightHd"

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "Select * From V_FNSIFreightDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

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
                ViewState("Reference") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
                End If

                If ViewState("Sender") = "btnConsignee" Then
                    tbConsignee.Text = Session("Result")(0).ToString
                    BindToText(tbConsigneeName, Session("Result")(1).ToString)                    
                End If

                If ViewState("Sender") = "btnPO" Then
                    'ResultField = "Cost_No, PONo, Currency, Supplier, Supplier_Name"
                    tbPCNo.Text = Session("Result")(0).ToString
                    tbPONo.Text = Session("Result")(1).ToString
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    If Trim(tbSuppCode.Text) = "" Then
                        BindToText(tbSuppCode, Session("Result")(3).ToString)
                        BindToText(tbSuppName, Session("Result")(4).ToString)
                    End If
                    tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnCost" Then
                    tbCost.Text = Session("Result")(0).ToString
                    BindToText(tbCostName, Session("Result")(1).ToString)
                    BindToText(tbAmountEst, Session("Result")(2).ToString)
                    BindToText(tbAmountRealisasi, Session("Result")(2).ToString)
                    tbBalance.Text = FormatFloat(CFloat(tbAmountRealisasi.Text) + CFloat(tbPPNForexDt.Text), ViewState("DigitCurr"))
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
        ViewState("PayType") = ""
        ViewState("SortExpression") = Nothing
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
        FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If

        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")

        tbAmountRealisasi.Attributes.Add("OnBlur", "setformatDt('AF');")
        'tbPPnDt.Attributes.Add("OnBlur", "setformatDt('PP');")
        tbPPNForexDt.Attributes.Add("OnBlur", "setformatDt('PF');")
        tbBalance.Attributes.Add("OnBlur", "setformatDt('TF');")

        tbAmountRealisasi.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPnDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPNForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBalance.Attributes.Add("OnKeyDown", "return PressNumeric();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbFilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
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

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbSuppCode.Enabled = State
            'btnSupp.Visible = State
            btnPCNo.Enabled = State
            tbPONo.Enabled = State
            'ddlCurr.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            'Dim dr As DataRow
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'insert      
                'ddlReport.SelectedValue
                tbTransNo.Text = GetAutoNmbr("IFA", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "Insert INTO FINSIFreightHd (TransNmbr, Status, TransDate, ContraBonNo, ContraBonDate, FgReport, " + _
                "PurchaseFreight, PONo, Supplier, SuppInvoice, Term, DueDate, Currency, ForexRate, " + _
                "FgAgingStartDate, BaseForex, PPNForex, TotalForex, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr(tbCBno.Text) + ",'" + Format(tbCBDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                QuotedStr("Y") + "," + QuotedStr(tbPCNo.Text) + "," + QuotedStr(tbPONo.Text) + "," + _
                QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbSuppInvoice.Text) + "," + _
                QuotedStr(ddlTerm.SelectedValue) + "," + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + "," + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + _
                rbAgingDate.SelectedIndex.ToString + ", " + tbBaseForex.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getdate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINSIFreightHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINSIFreightHd SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", ContraBonNo = " + QuotedStr(tbCBno.Text) + ", ContraBonDate = '" + Format(tbCBDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", FgReport=" + QuotedStr("Y") + ", PONo=" + QuotedStr(tbPONo.Text) + ", PurchaseFreight=" + QuotedStr(tbPCNo.Text) + _
                ", SuppInvoice =" + QuotedStr(tbSuppInvoice.Text) + ", Term =" + QuotedStr(ddlTerm.SelectedValue) + _
                ", DueDate = " + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + _
                ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate=" + tbRate.Text.Replace(",", "") + _
                ", Supplier= " + QuotedStr(tbSuppCode.Text) + _
                ", FgAgingStartDate = " + rbAgingDate.SelectedIndex.ToString + ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + _
                ", PPNForex= " + tbPPNForex.Text.Replace(",", "") + ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark=" + QuotedStr(tbRemark.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
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
                Row(I)("TransNmbr") = tbTransNo.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, CostItem, AmountEst, AmountForex, PPn, PPnForex, TotalForex, PPnNo, PPnDate, PPnRate, Consignee, SuppInvNo, Remark FROM FINSIFreightDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand

            Dim param As SqlParameter

            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE FINSIFreightDt SET CostItem = @CostItem, AmountEst = @AmountEst, AmountForex = @AmountForex, " + _
                    "Remark = @Remark, PPn = @PPn, PPnForex = @PPnForex, TotalForex = @TotalForex, Consignee = @Consignee, " + _
                    "PPnNo = @PPnNo, PPNDate = @PPNDate, PPnRate = @PPnRate, SuppInvNo = @SuppInvNo " + _
                    "WHERE TransNmbr = '" & ViewState("Reference") & "' AND CostItem = @CostItemm", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@CostItem", SqlDbType.VarChar, 5, "CostItem")
            Update_Command.Parameters.Add("@AmountEst", SqlDbType.Float, 18, "AmountEst")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 18, "AmountForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@PPn", SqlDbType.Float, 18, "PPn")
            Update_Command.Parameters.Add("@PPnForex", SqlDbType.Float, 18, "PPnForex")
            Update_Command.Parameters.Add("@TotalForex", SqlDbType.Float, 18, "TotalForex")
            Update_Command.Parameters.Add("@Consignee", SqlDbType.VarChar, 12, "Consignee")
            Update_Command.Parameters.Add("@PPnNo", SqlDbType.VarChar, 30, "PPnNo")
            Update_Command.Parameters.Add("@PPNDate", SqlDbType.DateTime, 8, "PPNDate")
            Update_Command.Parameters.Add("@PPnRate", SqlDbType.Float, 18, "PPnRate")
            Update_Command.Parameters.Add("@SuppInvNo", SqlDbType.VarChar, 30, "SuppInvNo")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@CostItemm", SqlDbType.VarChar, 5, "CostItem")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINSIFreightDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND CostItem = @CostItem", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@CostItem", SqlDbType.VarChar, 5, "CostItem")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINSIFreightDt")

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
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt2") = 0
            ViewState("DigitExpenseCurr") = 0
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
            tbTransNo.Text = ""
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            tbCBDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbPCNo.Text = ""
            tbPONo.Text = ""
            tbCBno.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            ddlTerm.SelectedValue = ""
            tbDueDate.Clear()
            tbSuppInvoice.Text = ""
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRate.Text = "1"
            'ddlReport.SelectedValue = "Y"
            tbRemark.Text = ""
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            rbAgingDate.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCost.Text = ""
            tbCostName.Text = ""
            tbAmountEst.Text = "0"
            tbAmountRealisasi.Text = "0"            
            tbPPnDt.Text = "0"
            tbPPNForexDt.Text = "0"
            tbBalance.Text = "0"
            tbPPnNoDt.Text = ""
            tbPPnDateDt.Clear()
            tbPpnRateDt.Text = "0"
            tbConsignee.Text = ""
            tbConsigneeName.Text = ""
            tbSuppInvNo.Text = ""
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbDate.IsNull Then
            '    lbStatus.Text = MessageDlg("Date must have value")
            '    tbDate.Focus()
            '    Return False
            'End If
            'If ddlReqType.SelectedValue.Trim = "" Then
            '    lbStatus.Text = "Request Type must have value"
            '    ddlReqType.Focus()
            '    Return False
            'End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbTransDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbTransDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If (rbAgingDate.SelectedIndex = 1) And (tbCBno.Text = "") Then
                lbStatus.Text = MessageDlg("Contra Bon No. must have value")
                tbCBno.Focus()
                Return False
            End If

            If tbCBno.Text.Trim = "" Then
                lbStatus.Text = "Contra Bon No must have value"
                tbCBno.Focus()
                Return False
            End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If tbSuppName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppName.Focus()
                Return False
            End If
            If tbPCNo.Text.Trim = "" Then
                lbStatus.Text = "Purchase Cost No. must have value"
                tbPCNo.Focus()
                Return False
            End If
            If tbSuppInvoice.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supp Inv No. must have value")
                tbSuppInvoice.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Forex Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            'If CFloat(tbPPN.Text) < 0 Then 'And ddlReport.SelectedValue = "Y"
            '    lbStatus.Text = MessageDlg("PPN must have value")
            '    tbPPN.Focus()
            '    Return False
            'End If
            
            'If CFloat(tbBaseForex.Text) <= 0 Then
            '    lbStatus.Text = "Base Forex must have value"
            '    tbBaseForex.Focus()
            '    Return False
            'End If

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
                If Dr("CostItemName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Cost Freight Must Have Value")
                    Return False
                End If
                If Dr("Consignee_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Consignee Must Have Value")
                    Return False
                End If
                If CFloat(Dr("AmountForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Base Forex (Realisasi) Must Have Value")
                    Return False
                End If
                
            Else
                If tbCostName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Freight Must Have Value")
                    tbCost.Focus()
                    Return False
                End If
                If tbConsigneeName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Consignee Must Have Value")
                    tbConsignee.Focus()
                    Return False
                End If
                If CFloat(tbAmountRealisasi.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Base Forex (Realisasi) Must Have Value")
                    tbAmountRealisasi.Focus()
                    Return False
                End If
                
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Report, Supplier, PC No, PO No, Currency, Rate, Base Forex, PPN Forex, Total Forex, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), FgReport, SupplierName, PurchaseFreight, PONo, Currency, ForexRate, BaseForex, PPNForex, TotalForex, Remark"
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
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
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
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        FillTextBoxHd(ViewState("Reference"))
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'tbSuppCode.Enabled = True
                        'btnSupp.Visible = True
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
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBank '" + GVR.Cells(2).Text + "','DPPAYMENT'"
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"

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
                btnAdddt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0
    Dim PPnForex As Decimal = 0

    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "CostItem")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                    PPnForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PPnForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbBaseForex.Text = FormatNumber(BaseForex, CInt(ViewState("DigitCurr")))
                    tbPPNForex.Text = FormatFloat(PPnForex, CInt(ViewState("DigitCurr")))
                    tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text), CInt(ViewState("DigitCurr")))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("CostItem = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        totalingDt()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            'tambah 1 baris dari jimmy
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"), "Edit")
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, Session("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt") = "Edit"
            tbCost.Focus()
            StatusButtonSave(False)

            If tbPPnDt.Text <> 0 Then
                'tbPPnDateDt.SelectedDate = ViewState("ServerDate") 'Today
                tbPPnDateDt_SelectionChanged(Nothing, Nothing)
                tbPpnRateDt.Text = tbRate.Text

                tbPPnNoDt.Enabled = True
                tbPPnDateDt.Enabled = True
                tbPpnRateDt.Enabled = True
            Else
                tbPPnNoDt.Text = ""
                tbPPnDateDt.Clear()
                tbPpnRateDt.Text = ""

                tbPPnNoDt.Enabled = False
                tbPPnDateDt.Enabled = False
                tbPpnRateDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbPCNo, Dt.Rows(0)("PurchaseFreight").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToDate(tbCBDate, Dt.Rows(0)("ContraBonDate").ToString)
            BindToText(tbCBno, Dt.Rows(0)("ContraBonNo").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToText(tbSuppInvoice, Dt.Rows(0)("SuppInvoice").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("CostItem = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                tbCost.Text = ItemNo
                BindToText(tbCostName, Dr(0)("CostItemName").ToString)
                BindToText(tbAmountEst, Dr(0)("AmountEst").ToString)
                BindToText(tbAmountRealisasi, Dr(0)("AmountForex").ToString)
                BindToText(tbPPnDt, Dr(0)("PPn").ToString)
                BindToText(tbPPNForexDt, Dr(0)("PPnForex").ToString)
                BindToText(tbBalance, Dr(0)("TotalForex").ToString)
                BindToText(tbPPnNoDt, Dr(0)("PPnNo").ToString)
                BindToDate(tbPPnDateDt, Dr(0)("PPNDate").ToString)
                BindToText(tbPpnRateDt, Dr(0)("PPnRate").ToString)
                BindToText(tbConsignee, Dr(0)("Consignee").ToString)
                BindToText(tbConsigneeName, Dr(0)("Consignee_Name").ToString)
                BindToText(tbSuppInvNo, Dr(0)("SuppInvNo").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                ViewState("CostItem") = ItemNo
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

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Dim dt As New DataTable
        Dim dr As DataRow

        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If

            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())

            If pnlEditDt.Visible = True Then
                If tbPPnDt.Text <> 0 Then
                    'tbPPnDateDt.SelectedDate = ViewState("ServerDate") 'Today
                    tbPPnDateDt_SelectionChanged(Nothing, Nothing)
                    tbPpnRateDt.Text = tbRate.Text

                    tbPPnNoDt.Enabled = True
                    tbPPnDateDt.Enabled = True
                    tbPpnRateDt.Enabled = True
                Else
                    tbPPnNoDt.Text = ""
                    tbPPnDateDt.Clear()
                    tbPpnRateDt.Text = ""

                    tbPPnNoDt.Enabled = False
                    tbPPnDateDt.Enabled = False
                    tbPpnRateDt.Enabled = False
                End If
            End If

            'BindDataDt("")

            dt = SQLExecuteQuery(GetStringDt(ViewState("Reference")), ViewState("DBConnection").ToString).Tables(0)

            For i = 0 To dt.Rows.Count - 1
                'dr = ViewState("Dt").NewRow
                'dr("TransNmbr") = dt.Rows(i)("TransNmbr")
                'dr("CostItem") = dt.Rows(i)("CostItem")
                'dr("CostItemName") = dt.Rows(i)("CostItemName")
                'dr("AmountEst") = dt.Rows(i)("AmountEst")
                'dr("AmountForex") = dt.Rows(i)("AmountForex")
                'dr("PPn") = dt.Rows(i)("PPn")
                'dr("PPnForex") = dt.Rows(i)("PPnForex")
                'dr("TotalForex") = dt.Rows(i)("TotalForex")
                'dr("Balance") = dt.Rows(i)("Balance")
                'dr("PPnNo") = dt.Rows(i)("PPnNo")
                'dr("PPNDate") = dt.Rows(i)("PPNDate")
                'dr("PPnRate") = FormatFloat(tbRate.Text, ViewState("DigitHome"))
                'dr("Consignee") = dt.Rows(i)("Consignee")
                'dr("Consignee_Name") = dt.Rows(i)("Consignee_Name")
                'dr("SuppInvNo") = dt.Rows(i)("SuppInvNo")
                'dr("Remark") = dt.Rows(i)("Remark")
                'ViewState("Dt").Rows.Add(dr)

                'Dim data As String
                'data = QuotedStr(dt.Rows(i)("TransNmbr")) + "|" + QuotedStr(dt.Rows(i)("CostItem"))

                dr = ViewState("Dt").Select("TransNmbr+'|'+CostItem = " + QuotedStr(dt.Rows(i)("TransNmbr") + "|" + dt.Rows(i)("CostItem")))(0)
                dr.BeginEdit()
                dr("PPnRate") = FormatFloat(tbRate.Text, ViewState("DigitHome"))
                dr.EndEdit()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            lbStatus.Text = "ddl Curr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        'Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                ddlCurr.SelectedValue = Dr("Currency")
                ddlTerm.SelectedValue = Dr("Term")
                If rbAgingDate.SelectedIndex = 0 Then
                    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbTransDate.SelectedDate, ViewState("DBConnection").ToString)
                Else
                    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbCBDate.SelectedDate, ViewState("DBConnection").ToString)
                End If
                rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            'tbPCNo.Text = ""
            'tbPONo.Text = ""
            AttachScript("setformatdt();", Page, Me.GetType())
            btnPCNo.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency');", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Dim TotalForex As Decimal = 0
    Dim TotalBase As Decimal = 0
    Private Sub totalingDt()
        Dim TotalPayment As Decimal = 0
        Dim TotalCharge As Decimal = 0
        Dim TotalForex As Decimal = 0
        Dim BaseForex As Decimal = 0

        Dim dr As DataRow
        Try
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    TotalPayment = TotalPayment + CFloat(dr("AmountEst").ToString)
                    TotalCharge = TotalCharge + CFloat(dr("AmountForex").ToString)
                End If
            Next
            TotalForex = (TotalPayment - TotalCharge) / CFloat(tbRate.Text)
            tbBaseForex.Text = CStr(BaseForex)
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    '    tbPCNo.Text = ""
    '    tbPONo.Text = ""
    '    'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub


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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()

            btnHome.Visible = False
            tbTransDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbFilter2.Text = ""
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
        'Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            'MovePanel(pnlInput, PnlHd)
            'CurrFilter = tbFilter.Text
            'Value = ddlField.SelectedValue
            'tbFilter.Text = tbTransNo.Text
            'ddlField.SelectedValue = "TransNmbr"
            'btnSearch_Click(Nothing, Nothing)
            'tbFilter.Text = CurrFilter
            'ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "Save All Error : " + ex.ToString
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
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbCost.Focus()
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
            If ActionValue = "Print" Or (ActionValue = "Print Full") Then
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
                If (ActionValue = "Print") Then
                    Session("SelectCommand") = "EXEC S_FNFormBuktiBank " + Result + ",'DPPAYMENT'"
                    Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                End If
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNSIFreight", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
        Try
            If ViewState("StateDt") = "Edit" Then
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim Row As DataRow
               
                Row = ViewState("Dt").Select("TransNmbr+'|'+CostItem = " + QuotedStr(ViewState("Reference") + "|" + ViewState("CostItem")))(0)
                
                Row.BeginEdit()
                Row("CostItem") = tbCost.Text
                Row("CostItemName") = tbCostName.Text
                Row("AmountEst") = tbAmountEst.Text
                Row("AmountForex") = tbAmountRealisasi.Text
                Row("PPn") = tbPPnDt.Text
                Row("PPnForex") = tbPPNForexDt.Text
                Row("TotalForex") = tbBalance.Text
                Row("Balance") = CFloat(tbAmountEst.Text) - CFloat(tbBalance.Text)
                Row("PPnNo") = tbPPnNoDt.Text
                If tbPPnDateDt.IsNull Then
                    Row("PPNDate") = DBNull.Value
                Else
                    Row("PPNDate") = Format(tbPPnDateDt.SelectedDate, "yyyy-MM-dd")
                End If
                If tbPpnRateDt.Text = "" Then
                    Row("PPnRate") = "0"
                Else
                    Row("PPnRate") = tbPpnRateDt.Text
                End If
                Row("Consignee") = tbConsignee.Text
                Row("Consignee_Name") = tbConsigneeName.Text
                Row("SuppInvNo") = tbSuppInvNo.Text                
                Row("SuppInvNo") = tbSuppInvNo.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "CostItem", tbCost.Text) Then
                    lbStatus.Text = "Cost '" + tbCostName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("CostItem") = tbCost.Text
                dr("CostItemName") = tbCostName.Text
                dr("AmountEst") = tbAmountEst.Text
                dr("AmountForex") = tbAmountRealisasi.Text
                dr("PPn") = tbPPnDt.Text
                dr("PPnForex") = tbPPNForexDt.Text
                dr("TotalForex") = tbBalance.Text
                dr("Balance") = CFloat(tbAmountEst.Text) - CFloat(tbBalance.Text)
                dr("PPnNo") = tbPPnNoDt.Text
                If tbPPnDateDt.IsNull Then
                    dr("PPNDate") = DBNull.Value
                Else
                    dr("PPNDate") = Format(tbPPnDateDt.SelectedDate, "yyyy-MM-dd")
                End If
                If tbPpnRateDt.Text = "" Then
                    dr("PPnRate") = "0"
                Else
                    dr("PPnRate") = tbPpnRateDt.Text
                End If
                dr("Consignee") = tbConsignee.Text
                dr("Consignee_Name") = tbConsigneeName.Text
                dr("SuppInvNo") = tbSuppInvNo.Text                
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            totalingDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Supplier_Code, Supplier_Name, Currency, Term FROM VMsSupplier "
            ResultField = "Supplier_Code, Supplier_Name, Currency, Term"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPCNo.Click
        Dim ResultField As String
        Try
            'If tbSuppCode.Text = "" Then
            Session("filter") = "SELECT DISTINCT Cost_No, Cost_Date, Supplier, Supplier_Name, Currency, PONo, BLNo, AJUNo From V_FNSIFreightGetPurchaseCost WHERE Cost_No IS NOT NULL " 'ddlReport.SelectedValue
            'Else
            'Session("filter") = "SELECT DISTINCT Cost_No, Cost_Date, Supplier, Supplier_Name, Currency, PONo, BLNo, AJUNo From V_FNSIFreightGetPurchaseCost WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + " AND FgReport = 'Y'" 'ddlReport.SelectedValue
            'End If
            ResultField = "Cost_No, PONo, Currency, Supplier, Supplier_Name"
            ViewState("Sender") = "btnPO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search PO Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCost.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Cost_Freight, Cost_Freight_Name, AmountForex From V_FNSIFreightGetPurchaseCost Where Cost_No in (" + QuotedStr(tbPCNo.Text) + ", '')" '" and Supplier = " + QuotedStr(tbSuppCode.Text) + " and Currency = " + QuotedStr(ddlCurr.SelectedValue)
            ResultField = "Cost_Freight, Cost_Freight_Name, AmountForex"
            ViewState("Sender") = "btnCost"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Cost Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCost_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCost.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Dim Sql As String
        Try
            'Sql = "Select Cost_Freight, Cost_Freight_Name, AmountForex From V_FNSIFreightGetPurchaseCost Where Cost_No = " + QuotedStr(tbPCNo.Text) + " and Supplier = " + QuotedStr(tbSuppCode.Text) + " and Currency = " + QuotedStr(ddlCurr.SelectedValue) + " and Cost_Freight = " + QuotedStr(tbCost.Text.Trim)
            'Sql = "Select Cost_Freight, Cost_Freight_Name, AmountForex From V_FNSIFreightGetPurchaseCost Where Cost_No = " + QuotedStr(tbPCNo.Text) + " and Currency = " + QuotedStr(ddlCurr.SelectedValue) + " and Cost_Freight = " + QuotedStr(tbCost.Text.Trim)
            Sql = "Select Cost_Freight, Cost_Freight_Name, AmountForex From V_FNSIFreightGetPurchaseCost Where Cost_No = " + QuotedStr(tbPCNo.Text) + " and Cost_Freight = " + QuotedStr(tbCost.Text.Trim)
            dt = SQLExecuteQuery(Sql, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                'tbCost.Text = Dr("Cost_Freight").ToString
                tbCostName.Text = Dr("Cost_Freight_Name").ToString
                tbAmountEst.Text = Dr("AmountForex").ToString
                tbAmountRealisasi.Text = Dr("AmountForex").ToString
                tbBalance.Text = FormatFloat(0, ViewState("DigitCurr"))
            Else
                tbCost.Text = ""
                tbCostName.Text = ""
                tbAmountEst.Text = FormatFloat(0, ViewState("DigitCurr"))
                tbAmountRealisasi.Text = FormatFloat(0, ViewState("DigitCurr"))
                tbBalance.Text = FormatFloat(0, ViewState("DigitCurr"))
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Search Cost Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnConsignee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsignee.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Consignee_Code, Consignee_Name FROM VMsConsignee "
            ResultField = "Consignee_Code, Consignee_Name"
            ViewState("Sender") = "btnConsignee"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btnConsignee Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPPnDateDt_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPnDateDt.SelectionChanged
        Try
            tbPpnRateDt.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPnDateDt.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            If CFloat(tbPpnRateDt.Text) = 0 Then
                tbPpnRateDt.Text = tbRate.Text
                'tbPpnRateDt.Text = FormatNumber(tbPpnRateDt.Text, ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lbStatus.Text = "tbPPnDateDt_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        If rbAgingDate.SelectedIndex = 0 Then
            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbTransDate.SelectedDate, ViewState("DBConnection").ToString)
        Else
            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbCBDate.SelectedDate, ViewState("DBConnection").ToString)
        End If
        rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub tbConsignee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbConsignee.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Consignee", tbConsignee.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbConsignee.Text = Dr("Consignee_Code")
                tbConsigneeName.Text = Dr("Consignee_Name")
            Else
                tbConsignee.Text = ""
                tbConsigneeName.Text = ""
            End If            
        Catch ex As Exception
            Throw New Exception("tbConsignee_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAmountRealisasi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAmountRealisasi.TextChanged
        'tbBalance.Text = tbAmountEst.Text - tbAmountRealisasi.Text
        'tbPPnDt.Focus()
        ''tbRemarkDt.Focus()
    End Sub

    Protected Sub tbPPnDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPnDt.TextChanged
        Try
            If CFloat(tbAmountRealisasi.Text) > 0 Then
                tbPPNForexDt.Text = CFloat(tbAmountRealisasi.Text) * (CFloat(tbPPnDt.Text) / 100)
                tbPPNForexDt.Text = FormatNumber(tbPPNForexDt.Text, ViewState("DigitHome"))
            Else
                tbPPNForexDt.Text = 0
            End If

            tbBalance.Text = CFloat(tbAmountRealisasi.Text) + CFloat(tbPPNForexDt.Text)
            tbBalance.Text = FormatNumber(tbBalance.Text, ViewState("DigitHome"))

            If tbPPnDt.Text <> 0 Then
                'tbPPnDateDt.SelectedDate = ViewState("ServerDate") 'Today
                'tbPPnDateDt_SelectionChanged(Nothing, Nothing)
                tbPpnRateDt.Text = tbRate.Text
                tbPpnRateDt.Text = FormatNumber(tbPpnRateDt.Text, ViewState("DigitHome"))

                tbPPnNoDt.Enabled = True
                tbPPnDateDt.Enabled = True
                tbPpnRateDt.Enabled = True
            Else
                tbPPnNoDt.Text = ""
                tbPPnDateDt.Clear()
                tbPpnRateDt.Text = ""

                tbPPnNoDt.Enabled = False
                tbPPnDateDt.Enabled = False
                tbPpnRateDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "tbPPnDt_TextChanged Error : " + ex.ToString
        End Try

        'Try
        '    If CFloat(tbPPnDt.Text) <> 0 Then
        '        tbPpnRateDt.Enabled = True
        '        tbPPnNoDt.Enabled = True
        '        tbPPnDateDt.Enabled = True

        '        tbPPnDateDt.SelectedValue = tbTransDate.SelectedValue
        '        tbPpnRateDt.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPnDateDt.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
        '        If CFloat(tbPpnRateDt.Text) = 0 Then
        '            tbPpnRateDt.Text = tbRate.Text
        '        End If
        '    Else
        '        tbPpnRateDt.Text = "0"
        '        tbPPnNoDt.Text = ""
        '        tbPPnDateDt.Clear()
        '        tbPpnRateDt.Enabled = False
        '        tbPPnNoDt.Enabled = False
        '        tbPPnDateDt.Enabled = False
        '    End If
        'Catch ex As Exception
        '    lbStatus.Text = "tbPPnDt_TextChanged Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub rbAgingDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAgingDate.SelectedIndexChanged
        Try
            If rbAgingDate.SelectedIndex = 0 Then
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbTransDate.SelectedDate, ViewState("DBConnection").ToString)
            Else
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbCBDate.SelectedDate, ViewState("DBConnection").ToString)
            End If
        Catch ex As Exception
            lbStatus.Text = "rbAgingDate_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTransDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTransDate.SelectionChanged
        If rbAgingDate.SelectedIndex = 0 Then
            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbTransDate.SelectedDate, ViewState("DBConnection").ToString)
        End If
        rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub tbCBDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCBDate.SelectionChanged
        If rbAgingDate.SelectedIndex = 1 Then
            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbCBDate.SelectedDate, ViewState("DBConnection").ToString)
        End If
        rbAgingDate_SelectedIndexChanged(Nothing, Nothing)
    End Sub
End Class
