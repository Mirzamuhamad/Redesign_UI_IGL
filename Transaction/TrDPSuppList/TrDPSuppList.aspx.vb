Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrDPSuppList
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select TransNmbr, Nmbr, Status, TransDate, FgReport, SuppInvNo, Supplier, " + _
                                        "Attn, PONo, POReport, PPNNo, PPNDate, PPNRate, Currency, Forexrate, BaseForex, " + _
                                        "PPn, PPNForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, " + _
                                        "DateAppr, BalanceBase, BalancePPn, User_Type, User_Code, User_Name, CostCtr From V_FNDPSuppListHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(3).ToString)
                    tbPONo.Text = ""
                    tbPOReport.Text = "N"
                    ddlCurr.Enabled = tbPONo.Text = ""
                    'ddlReport.Enabled = tbPOReport.Text = "Y"
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENTDP|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
                    
                    tbPPNNo.Enabled = False
                    tbPPNDate.Enabled = False
                    tbPPNRate.Enabled = False
                    tbPPN.Enabled = False

                End If
                If ViewState("Sender") = "btnPO" Then
                    'ResultField = "Reff_No, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr"
                    tbPONo.Text = Session("Result")(0).ToString
                    BindToDropList(ddlCurr, Session("Result")(1).ToString)
                    tbPPN.Text = "0"
                    ddlCurr.Enabled = tbPONo.Text = ""
                    'BindToText(tbPPN, Session("Result")(3).ToString)
                    ViewState("PONo") = Session("Result")(0).ToString
                    ViewState("DPForex") = CFloat(Session("Result")(5).ToString)
                    tbBaseForex.Text = CFloat(Session("Result")(5).ToString)
                    BindToText(tbPPN, Session("Result")(3).ToString)
                    BindToText(tbSuppCode, Session("Result")(6).ToString)
                    BindToText(tbSuppName, Session("Result")(7).ToString)
                    BindToText(tbPOReport, Session("Result")(8).ToString)
                    tbRemark.Text = TrimStr(Session("Result")(9).ToString)
                    BindToDropList(ddlCostCtr, Session("Result")(10).ToString)
                    ddlCostCtr.Enabled = (ddlCostCtr.SelectedValue = "") And (tbPONo.Text = "")
                    'ddlReport.Enabled = tbPOReport.Text = "Y"
                    'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
                    tbBaseForex.Text = FormatFloat(ViewState("DPForex"), ViewState("DigitCurr"))
                    'tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ", " + Me.tbPPN.Text + "); setformat();", Me.Page, Me.GetType())
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    Dim Dt As DataTable
                    Dim Dr As DataRow
                    Dim ExistRow As DataRow()
                    Dim MaxItem As String
                    Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Qty, Unit, PriceForex FROM V_FNDPSuppListGetPO WHERE Reff_No = " + QuotedStr(tbPONo.Text), ViewState("DBConnection").ToString).Tables(0)
                    If Dt.Rows.Count > 0 Then
                        Dr = Dt.Rows(0)
                        
                        For Each Dr In Dt.Rows
                            ExistRow = ViewState("Dt").Select("ProductName = " + QuotedStr(Dr("Product_Name").ToString))
                            If ExistRow.Count = 0 Then
                                MaxItem = GetNewItemNo(ViewState("Dt"))
                                'insert
                                Dim ds As DataRow
                                ds = ViewState("Dt").NewRow
                                ds("ItemNo") = MaxItem
                                ds("ProductName") = Dr("Product_Name").ToString
                                ds("Qty") = Dr("Qty").ToString
                                ds("Unit") = Dr("Unit").ToString
                                If ds("Unit") = "" Then
                                    ds("Unit") = DBNull.Value
                                End If
                                ds("PriceForex") = Dr("PriceForex").ToString
                                ds("AmountForex") = FormatFloat(CFloat(Dr("Qty").ToString) * CFloat(Dr("PriceForex").ToString), ViewState("DigitCurr"))
                                ViewState("Dt").Rows.Add(ds)
                            End If
                        Next
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        BindGridDt(ViewState("Dt"), GridDt)
                        StatusButtonSave(True)
                        'totalingDt()
                        GridDt.Columns(0).Visible = False
                    End If
                End If
                If ViewState("Sender") = "btnProduct" Then
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbQty, Session("Result")(2).ToString)
                    BindToDropList(ddlUnit, Session("Result")(3).ToString)
                    BindToText(tbPriceForex, Session("Result")(4).ToString)
                    tbAmountForex.Text = (CFloat(tbQty.Text) * CFloat(tbPriceForex.Text))
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
            If Len(tbPONo.Text) > 1 Then
                tbPPNNo.Enabled = True
                tbPPNDate.Enabled = True
                tbPPNRate.Enabled = True
                tbPPN.Enabled = True
            Else
                tbPPNNo.Enabled = False
                tbPPNDate.Enabled = False
                tbPPNRate.Enabled = False
                tbPPN.Enabled = False
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
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

            ViewState("SortExpression") = Nothing
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            Me.tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbPPNRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNForex.Attributes.Add("ReadOnly", "True")
            tbTotalForex.Attributes.Add("ReadOnly", "True")
            tbPPN.Attributes.Add("ReadOnly", "True")
            tbBaseForex.Attributes.Add("ReadOnly", "True")

            Me.tbBaseForex.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
            Me.tbPPN.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
            Me.tbQty.Attributes.Add("OnChange", "setformatdt();")
            Me.tbPriceForex.Attributes.Add("OnChange", "setformatdt();")
            Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Me.ddlCurr.Attributes.Add("OnChange", "setformat();")            
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr, ItemNo, ProductName, Qty, Unit, PriceForex, AmountForex From V_FNDPSuppListDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlUserType.Enabled = State
            tbSuppCode.Enabled = State
            btnSupp.Enabled = State
            tbPONo.Enabled = State
            btnPO.Enabled = State
            tbTransDate.Enabled = State
            ddlCurr.Enabled = tbPONo.Text = "" And State
            tbRate.Enabled = State And ddlCurr.SelectedValue <> ViewState("Currency")
            'tbPPNRate.Enabled = State 'And ddlReport.SelectedValue = "Y"
            'tbPPN.Enabled = State And ddlUserType.SelectedValue = "Supplier"

            'ddlReport.Enabled = State And tbPOReport.Text = "Y"
            'btnGetDt.Visible = State
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
            'If dt.Rows.Count > 0 Then
            '    dr = dt.Rows(0)
            '    ViewState("SJ_No") = dr("SJNo").ToString
            'End If

        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub SaveAll()
        Dim SQLString As String
        Dim tgl As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            'Save Hd
            If tbPPNDate.IsNull Then
                tgl = "NULL"
            Else
                tgl = QuotedStr(Format(tbPPNDate.SelectedDate, "yyyy-MM-dd"))
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
                tbTransNo.Text = GetAutoNmbr("DSI", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "Insert INTO FINDPSuppListHd (TransNmbr, Status, TransDate, FgReport, POReport, " + _
                "PONo, UserType, UserCode, SuppInvNo, Attn, Currency, ForexRate, PPNNo, PPNDate, " + _
                "CostCtr, " + _
                "PPNRate, BaseForex, PPN, PPNForex, TotalForex, Remark, UserPrep, DatePrep, BalanceBase, BalancePPN) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr("Y") + ", " + QuotedStr(tbPOReport.Text) + "," + QuotedStr(tbPONo.Text) + "," + _
                QuotedStr(ddlUserType.SelectedValue) + "," + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbSuppInvNo.Text) + "," + QuotedStr(tbAttn.Text) + "," + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(tbPPNNo.Text) + "," + _
                tgl + "," + QuotedStr(ddlCostCtr.SelectedValue) + "," + tbPPNRate.Text.Replace(",", "") + "," + _
                tbBaseForex.Text.Replace(",", "") + "," + tbPPN.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getdate(),0,0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDPSuppListHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINDPSuppListHd SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", FgReport=" + QuotedStr("Y") + ", PONo=" + QuotedStr(tbPONo.Text) + ", UserType=" + QuotedStr(ddlUserType.SelectedValue) + ", UserCode=" + QuotedStr(tbSuppCode.Text) + _
                ", Attn=" + QuotedStr(tbAttn.Text) + ", SuppInvNo=" + QuotedStr(tbSuppInvNo.Text) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate=" + tbRate.Text.Replace(",", "") + ", PPNNo=" + QuotedStr(tbPPNNo.Text) + _
                ", PPNDate=" + tgl + ", PPNRate=" + tbPPNRate.Text.Replace(",", "") + _
                ", CostCtr=" + QuotedStr(ddlCostCtr.SelectedValue) + _
                ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + ", PPN=" + tbPPN.Text.Replace(",", "") + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, ProductName, Qty, Unit, PriceForex, AmountForex FROM FINDPSuppListDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            ',ChargeCurrency,ChargeRate,ChargeForex,ChargeHome
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINDPSuppListDt SET ProductName = @ProductName, Qty = @Qty, Unit = @Unit, " + _
            "PriceForex = @PriceForex, AmountForex = @AmountForex " + _
            "WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @OldItemNo ", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ProductName", SqlDbType.VarChar, 60, "ProductName")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 23, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 23, "AmountForex")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINDPSuppListDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("{FINDPSuppListDt")

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
            tbPPNDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSuppInvNo.Text = ""
            tbPONo.Text = ""
            tbAttn.Text = ""
            tbPOReport.Text = "N"
            tbPPNNo.Text = ""
            tbPPN.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPPNRate.Text = "0"
            tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRate.Text = FormatFloat("1", ViewState("DigitRate"))
            'ddlReport.SelectedValue = "N"
            'ddlReport.Enabled = tbPOReport.Text = "Y"
            ddlUserType.SelectedValue = "Supplier"
            tbRemark.Text = ""
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            
            tbPPNNo.Enabled = False
            tbPPNDate.Enabled = False
            tbPPNRate.Enabled = False
            tbPPN.Enabled = False

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductName.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedValue = ""
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
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

            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("" + ddlUserType.SelectedValue + " must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If tbSuppName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("" + ddlUserType.SelectedValue + " must have value")
                tbSuppName.Focus()
                Return False
            End If
            'If tbPONo.Text.Trim = "" Then
            '    lbStatus.Text = "PO No. must have value"
            '    tbPONo.Focus()
            '    Return False
            'End If
            If tbSuppInvNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supp Inv No. must have value")
                tbSuppInvNo.Focus()
                Return False
            End If
            If ddlCostCtr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Cost Ctr must have value")
                ddlCostCtr.Focus()
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
            If CFloat(tbPPN.Text) < 0 Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN must have value")
                tbPPN.Focus()
                Return False
            End If
            If CFloat(tbPPNRate.Text) < 0 Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN Rate must have value")
                tbPPNRate.Focus()
                Return False
            End If
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

    'Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
    '    Try
    '        If Not (Dr Is Nothing) Then
    '            If Dr.RowState = DataRowState.Deleted Then
    '                Return True
    '            End If
    '            If Dr("PaymentType").ToString = "" Then
    '                lbStatus.Text = MessageDlg("Payment Type Must Have Value")
    '                Return False
    '            End If
    '            If Dr("FgMode").ToString = "G" Then
    '                If Dr("DocumentNo").ToString = "" Then
    '                    lbStatus.Text = MessageDlg("Document No Must Have Value")
    '                    Return False
    '                End If
    '                If Dr("BankPayment").ToString = "" Then
    '                    lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
    '                    Return False
    '                End If
    '                If Dr("GiroDate").ToString = "" Then
    '                    lbStatus.Text = MessageDlg("Giro Date Must Have Value")
    '                    Return False
    '                End If
    '                If Dr("DueDate").ToString = "" Then
    '                    lbStatus.Text = MessageDlg("Due Date Must Have Value")
    '                    Return False
    '                End If
    '            End If
    '            If Dr("FgMode").ToString = "B" Or Dr("FgMode").ToString = "K" Then
    '                If Dr("Reference").ToString = "" Then
    '                    lbStatus.Text = MessageDlg("Voucher No Must Have Value")
    '                    Return False
    '                End If
    '            End If
    '        Else
    '            If ddlPayTypeDt2.SelectedValue = "" Then
    '                lbStatus.Text = MessageDlg("Payment Type Must Have Value")
    '                ddlPayTypeDt2.Focus()
    '                Return False
    '            End If
    '            If CFloat(tbRateDt2.Text) = 0 Then
    '                lbStatus.Text = MessageDlg("Forex Rate Must Have Value")
    '                tbRateDt2.Focus()
    '                Return False
    '            End If
    '            If tbFgModeDt2.Text = "G" Then
    '                If tbDocumentNoDt2.Text.Trim = "" Then
    '                    lbStatus.Text = MessageDlg("Document No Must Have Value")
    '                    tbDocumentNoDt2.Focus()
    '                    Return False
    '                End If
    '                If ddlBankPaymentDt2.SelectedValue = "" Then
    '                    lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
    '                    ddlBankPaymentDt2.Focus()
    '                    Return False
    '                End If
    '                If tbGiroDateDt2.SelectedDate = Nothing Then
    '                    lbStatus.Text = MessageDlg("Giro Date Must Have Value")
    '                    tbGiroDateDt2.Focus()
    '                    Return False
    '                End If
    '                If tbDueDateDt2.SelectedDate = Nothing Then
    '                    lbStatus.Text = MessageDlg("Due Date Must Have Value")
    '                    tbDueDateDt2.Focus()
    '                    Return False
    '                End If
    '            End If
    '            If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
    '                If tbVoucherNo.Text.Trim = "" Then
    '                    lbStatus.Text = MessageDlg("Voucher No Must Have Value")
    '                    tbVoucherNo.Focus()
    '                    Return False
    '                End If
    '            End If
    '            If CFloat(tbPaymentForexDt2.Text) = 0 Then
    '                lbStatus.Text = MessageDlg("Payment Forex Must Have Value")
    '                tbPaymentForexDt2.Focus()
    '                Return False
    '            End If
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Throw New Exception("Cek Dt Error : " + ex.ToString)
    '    End Try
    'End Function

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Report, User, Supp Inv No., Attn, PO No, PPN No, PPN Date, PPN Rate, Currency, Rate, Base Forex, PPN, PPN Forex, Total Forex, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), FgReport, User_Name, SuppInvNo, Attn, PONo, PPNNo, dbo.FormatDate(PPnDate), PPNRate, Currency, ForexRate, BaseForex, PPN, PPNForex, TotalForex, Remark"
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
                    BindDataDt(ViewState("Reference"))
                    FillTextBoxHd(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            'Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        If Len(tbPONo.Text) > 1 Then
                            tbPPNNo.Enabled = True
                            tbPPNDate.Enabled = True
                            tbPPNRate.Enabled = True
                            tbPPN.Enabled = True
                        Else
                            tbPPNNo.Enabled = False
                            tbPPNDate.Enabled = False
                            tbPPNRate.Enabled = False
                            tbPPN.Enabled = False
                        End If
                        btnAddDt.Visible = False
                        btnAddDt2.Visible = False
                        GridDt.Columns(0).Visible = False
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

                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','DPSPY'," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    Dim SqlString As String

                    SqlString = "Declare @A VarChar(255) EXEC S_FNDPSuppListDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "

                    SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
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

    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then

                ElseIf e.Row.RowType = DataControlRowType.Footer Then

                    totalingDt()
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    ' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ChangeForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                'AttachScript("BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        totalingDt()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("ProductDt") = GVR.Cells(1).Text
            FillTextBoxDt(GVR.Cells(1).Text)
            btnSaveDt.Focus()
            StatusButtonSave(False)
            AttachScript("setformatdt('');", Page, Me.GetType())
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
            BindToDropList(ddlUserType, Dt.Rows(0)("User_Type").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("User_Code").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("User_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbSuppInvNo, Dt.Rows(0)("SuppInvNo").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbPOReport, Dt.Rows(0)("POReport").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbPPNNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPNDate, Dt.Rows(0)("PPnDate").ToString)
            BindToText(tbPPNRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ddlCurr.Enabled = tbPONo.Text = ""
            'ddlReport.Enabled = tbPOReport.Text = "Y"
            If tbPONo.Text <> "" Then
                tbPPNNo.Enabled = True
                tbPPNDate.Enabled = True
                tbPPNRate.Enabled = True
                tbPPN.Enabled = True
            Else
                tbPPNNo.Enabled = False
                tbPPNDate.Enabled = False
                tbPPNRate.Enabled = False
                tbPPN.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + Product)
            If Dr.Length > 0 Then
                lbItemNo.Text = Product.ToString
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
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
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
            ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        'Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT User_Code, User_Name, Currency, Term FROM VMsUserType WHERE User_Type = '" + ddlUserType.SelectedValue + "' AND User_Code = '" + tbSuppCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            Dr = FindMaster("UserType", tbSuppCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection").ToString)
            'If Dt.Rows.Count > 0 Then
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbSuppCode.Text = Dr("User_Code")
                tbSuppName.Text = Dr("User_Name")
                tbAttn.Text = Dr("Contact_Person")
                ddlCurr.SelectedValue = Dr("Currency")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbAttn.Focus()
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


    'Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
    '    Try
    '        ChangeCurrency(ddlChargeCurrDt2, tbPaymentDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
    '        If ddlChargeCurrDt2.SelectedValue = "" Then
    '            tbChargeForexDt2.Text = "0"
    '            tbChargeHomeDt2.Text = "0"
    '        End If
    '        tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
    '        tbChargeRateDt2.Focus()
    '        '            AttachScript("setformatDt();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Dim TotalForex As Decimal = 0
    Dim TotalBase As Decimal = 0
    Private Sub totalingDt()
        Dim dr As DataRow
        Dim amount As Double
        Try
            amount = 0
            ViewState("DataDt") = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    amount = amount + CFloat(dr("AmountForex").ToString)
                    ViewState("DataDt") = 1
                End If
            Next
            'tbBaseForex.Text = FormatFloat(amount, CInt(ViewState("DigitCurr")))
            'tbPPNForex.Text = FormatFloat((CFloat(tbPPN.Text) / 100) * CFloat(tbBaseForex.Text), ViewState("DigitCurr"))
            'tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    '    'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + ddlReport.SelectedValue + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    '    'tbPONo.Text = ""
    '    If ddlReport.SelectedValue = "N" Then
    '        tbPPN.Enabled = False
    '        tbPPN.Text = FormatFloat("0", ViewState("DigitPercent"))
    '    Else
    '        tbPPN.Enabled = True
    '    End If
    'End Sub

    Protected Sub tbRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRate.TextChanged
        'If ddlReport.SelectedValue = "Y" Then
        tbPPNRate.Text = tbRate.Text
        'End If
    End Sub

    'Protected Sub tbChargeForexDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbChargeForexDt2.TextChanged
    '    'tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
    '    'tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRateDt2.Text), ViewState("DigitQty"))
    '    'tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)), ViewState("DigitQty"))

    '    'ubahtbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
    '    'ubahtbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)), ViewState("DigitHome"))
    '    tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
    '    tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text)), ViewState("DigitHome"))

    '    'ChangeCurrency(ddlCurrDt2, tbTransDate, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
    '    totalingDt()
    '    'AttachScript("setformatDt();", Page, Me.GetType())
    'End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        btnPO.Enabled = ddlUserType.SelectedValue = "Supplier"
        tbSuppCode.Text = ""
        tbSuppName.Text = ""
        tbPONo.Text = ""
        tbPOReport.Text = "N"
        'tbPPN.Enabled = ddlUserType.SelectedValue = "Supplier"
        tbPPN.Text = 0
        'ddlReport.Enabled = tbPOReport.Text = "Y"
    End Sub

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
            newTrans()
            btnHome.Visible = False
            tbTransDate.Focus()

            btnAddDt.Visible = False
            btnAddDt2.Visible = False
            GridDt.Columns(0).Visible = False
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
            If ViewState("DataDt") = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
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
            If ViewState("DataDt") = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

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
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        tbProductName.Focus()
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
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNDPSuppList", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            If tbProductName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Product must have value")
                tbProductName.Focus()
                Exit Sub
            End If
            If CFloat(tbQty.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Qty must have value")
                tbQty.Focus()
                Exit Sub
            End If
            If ddlUnit.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Unit must have value")
                ddlUnit.Focus()
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                'If ViewState("DtValue") <> tbSJNo.Text + "|" + tbProductCode.Text Then
                '    If CekExistData(ViewState("Dt"), "SJNo,Product", tbSJNo.Text + "|" + tbProductCode.Text) Then
                '        lbStatus.Text = "SJ No " + tbSJNo.Text + " Product " + tbProductName.Text + " has been already exist"
                '        Exit Sub
                '    End If
                'End If
                Row = ViewState("Dt").Select("ItemNo = " + ViewState("ProductDt"))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("ItemNo") = lbItemNo.Text
                Row("ProductName") = tbProductName.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = ddlUnit.SelectedValue
                If Row("Unit") = "" Then
                    Row("Unit") = DBNull.Value
                End If
                Row("PriceForex") = tbPriceForex.Text
                Row("AmountForex") = tbAmountForex.Text

                Row.EndEdit()

            Else
                'Insert
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                'If CekExistData(ViewState("Dt"), "ProductName", tbProductName.Text) = True Then
                '    lbStatus.Text = MessageDlg(" Product " + tbProductName.Text + " has already been exist")
                '    Exit Sub
                'End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("ProductName") = tbProductName.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = ddlUnit.SelectedValue
                If dr("Unit") = "" Then
                    dr("Unit") = DBNull.Value
                End If
                dr("PriceForex") = tbPriceForex.Text
                dr("AmountForex") = tbAmountForex.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "select User_Code, User_Name, Contact_Person, Currency, Term FROM VMsUserType where User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Currency, Contact_Person"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPO.Click
        Dim ResultField As String
        Try
            If tbSuppCode.Text = "" Then
                Session("filter") = "SELECT Reff_No, Reff_Date, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr, Remark FROM v_fndpSuppgetPO "
            Else
                Session("filter") = "SELECT Reff_No, Reff_Date, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr, Remark FROM v_fndpSuppgetPO WHERE Supplier_Code = " + QuotedStr(tbSuppCode.Text)
            End If
            ResultField = "Reff_No, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr"
            ViewState("Sender") = "btnPO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search PO Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Product_Code, Product_Name, Qty, Unit, PriceForex FROM V_FNDPSuppListGetPO WHERE Reff_No = " + QuotedStr(tbPONo.Text)
            ResultField = "Product_Code, Product_Name, Qty, Unit, PriceForex "
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProduct_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged, tbPriceForex.TextChanged
        Try
            tbAmountForex.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)), ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPONo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPONo.TextChanged
        If Len(tbPONo.Text) > 1 Then
            tbPPNNo.Enabled = True
            tbPPNDate.Enabled = True
            tbPPNRate.Enabled = True
            tbPPN.Enabled = True
        Else
            tbPPNNo.Enabled = False
            tbPPNDate.Enabled = False
            tbPPNRate.Enabled = False
            tbPPN.Enabled = False
        End If
    End Sub
End Class
