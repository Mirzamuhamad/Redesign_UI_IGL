Imports System.Data
Imports System.Data.SqlClient

Partial Class TrDPSuppRetur
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select distinct TransNmbr, Nmbr, TransDate, Status, FgReport, User_Type, User_Code, User_Name, Supplier, Attn, TotalReceipt, TotalDP, TotalCharge, TotalDiffRate, Remark From V_FNDPSuppReturHd "

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNDPSuppReturDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNDPSuppReturPay WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim Dr As DataRow

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
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                ElseIf ViewState("Sender") = "btnDPNo" Then
                    ' "DP_No, DP_Date, PPn_Rate, Currency, Rate, Base_Forex, PPn_Forex, Total_Forex, Base_Paid, PPn_Paid, Total_Paid"
                    tbDPNoDt.Text = Session("Result")(0).ToString
                    BindToDate(tbDPDateDt, Session("Result")(1).ToString)
                    BindToText(tbPPnRateDt, Session("Result")(2).ToString)
                    BindToDropList(ddlCurrDt, Session("Result")(3).ToString)

                    If ddlCurrDt.SelectedValue <> "IDR" Then
                        tbRateDt.Enabled = True
                    Else
                        tbRateDt.Enabled = False
                    End If

                    BindToText(tbDPRate, Session("Result")(4).ToString) 'BindToText(tbRateDt, Session("Result")(4).ToString)

                    BindToText(tbBaseDPDt, Session("Result")(5).ToString)
                    BindToText(tbPPNDPDt, Session("Result")(6).ToString)
                    BindToText(tbTotalDPDt, Session("Result")(7).ToString)

                    BindToText(tbBasePaidDt, Session("Result")(8).ToString)
                    BindToText(tbPPNPaidDt, Session("Result")(9).ToString)
                    BindToText(TbTotalPaidDt, Session("Result")(10).ToString)

                    BindToText(tbBaseForexDt, FormatNumber(CFloat(tbBaseDPDt.Text) - CFloat(tbBasePaidDt.Text), ViewState("DigitRate")))
                    BindToText(tbPPnForexDt, FormatNumber(CFloat(tbPPNDPDt.Text) - CFloat(tbPPNPaidDt.Text), ViewState("DigitRate")))

                    If tbSuppCode.Text.Trim = "" Then
                        BindToDropList(ddlUserType, Session("Result")(11).ToString)
                        BindToText(tbSuppCode, Session("Result")(12).ToString)
                        BindToText(tbSuppName, Session("Result")(13).ToString)
                    End If
                    'BindToText(tbTotalForexDt, Session("Result")(6).ToString)
                    ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                    ' ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'AttachScript("tambah(" + Me.tbBaseForexDt.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbTotalForexDt.ClientID + "); kalippnrate(" + Me.tbBaseForexDt.ClientID + "," + Me.tbRateDt.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbPPnRateDt.ClientID + "," + Me.tbTotalHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
                    AttachScript("tambah(" + Me.tbBaseForexDt.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbTotalForexDt.ClientID + "); kalippnrate(" + Me.tbBaseForexDt.ClientID + "," + Me.tbDPRate.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbPPnRateDt.ClientID + "," + Me.tbTotalHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())

                    '----------------------------------------
                    Dr = FindMaster("Rate", ddlCurrDt.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection"))
                    If Not Dr Is Nothing Then
                        tbRateDt.Text = FormatFloat(Dr("Rate").ToString, ViewState("DigitCurr"))
                        ViewState("DigitCurr") = Dr("digit")
                    Else
                        tbRateDt.Text = FormatFloat("0", ViewState("DigitCurr"))
                    End If
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

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            lbPayHome.Text = "Receipt (" + ViewState("Currency") + ")"
            lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            lbTotReceipt.Text = "Receipt (" + ViewState("Currency") + ")"
            lbTotDP.Text = "DP (" + ViewState("Currency") + ")"
            lbTotCharge.Text = "Charge (" + ViewState("Currency") + ")"

            FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser(" + QuotedStr("ReceiptDP" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankGiroDt, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillCombo(ddlBankGiroDt2, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If

            tbTotalReceipt.Attributes.Add("ReadOnly", "True")
            tbTotalDP.Attributes.Add("ReadOnly", "True")
            tbTotalCharge.Attributes.Add("ReadOnly", "True")
            tbSelisihKurs.Attributes.Add("ReadOnly", "True")
            tbDiffRate.Attributes.Add("ReadOnly", "True")

            tbTotalDPDt.Attributes.Add("ReadOnly", "True")
            TbTotalPaidDt.Attributes.Add("ReadOnly", "True")
            tbTotalForexDt.Attributes.Add("ReadOnly", "True")
            tbTotalHomeDt.Attributes.Add("ReadOnly", "True")
            tbBaseDPDt.Attributes.Add("ReadOnly", "True")
            tbBasePaidDt.Attributes.Add("ReadOnly", "True")
            tbPPNDPDt.Attributes.Add("ReadOnly", "True")
            tbPPNPaidDt.Attributes.Add("ReadOnly", "True")
            tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            tbReceiptHomeDt2.Attributes.Add("ReadOnly", "True")

            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDPRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbBaseForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPnForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbReceiptForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            tbBaseForexDt.Attributes.Add("OnBlur", "tambah(" + Me.tbBaseForexDt.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbTotalForexDt.ClientID + "); kalippnrate(" + Me.tbBaseForexDt.ClientID + "," + Me.tbDPRate.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbPPnRateDt.ClientID + "," + Me.tbTotalHomeDt.ClientID + "); setformatdt();")
            tbPPnForexDt.Attributes.Add("OnBlur", "tambah(" + Me.tbBaseForexDt.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbTotalForexDt.ClientID + "); kalippnrate(" + Me.tbBaseForexDt.ClientID + "," + Me.tbDPRate.ClientID + "," + Me.tbPPnForexDt.ClientID + "," + Me.tbPPnRateDt.ClientID + "," + Me.tbTotalHomeDt.ClientID + "); setformatdt();")

            tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); setformatdt2();")
            tbReceiptForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); setformatdt2();")

            tbChargeRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
            tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
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
            If DT.Rows.Count > 0 Then
                GridView1.HeaderRow.Cells(8).Text = "Receipt (" + ViewState("Currency") + ")"
                GridView1.HeaderRow.Cells(9).Text = "DP (" + ViewState("Currency") + ")"
                GridView1.HeaderRow.Cells(10).Text = "Charge (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

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
                Session("SelectCommand") = "EXEC S_FNFormVoucher " + Result + ",'DSR'," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNDPSuppRetur", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            ddlUserType.Enabled = State
            tbSuppCode.Enabled = State
            btnSupp.Visible = State
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
            If dt.Rows.Count > 0 Then
                GridDt.HeaderRow.Cells(9).Text = "Total (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            Dim dr As DataRow
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
            If dt.Rows.Count > 0 Then
                GridDt2.HeaderRow.Cells(9).Text = "Receipt (" + ViewState("Currency") + ")"
                GridDt2.HeaderRow.Cells(10).Text = "Charge (" + ViewState("Currency") + ")"
            End If
            For Each dr In dt.Rows
                If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Then
                    ViewState("PayType") = dt.Rows(0)("ReceiptType").ToString
                    Exit For
                End If
            Next
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
            ddlUserType.SelectedValue = "Supplier"
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            'ddlReport.SelectedValue = "Y"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTotalReceipt.Text = "0"
            tbTotalDP.Text = "0"
            tbTotalCharge.Text = "0"
            tbSelisihKurs.Text = "0"
            tbDiffRate.Text = "0"
            tbAttn.Text = ""
            tbRemark.Text = ""
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptDP" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Dim Dr As DataRow

        Try
            tbDPNoDt.Text = ""
            tbDPDateDt.SelectedDate = Nothing
            ddlCurrDt.SelectedValue = ViewState("Currency")

            tbRateDt.Text = "1"
            tbRateDt.Enabled = False
            tbDPRate.Text = ""
            tbPPnRateDt.Text = ""

            Dr = FindMaster("Rate", ddlCurrDt.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbRateDt.Text = FormatFloat(Dr("Rate").ToString, ViewState("DigitCurr"))
                ViewState("DigitCurr") = Dr("digit")
            Else
                tbRateDt.Text = FormatFloat("0", ViewState("DigitCurr"))
            End If

            tbBaseDPDt.Text = "0"
            tbBasePaidDt.Text = "0"
            tbBaseForexDt.Text = "0"

            tbPPNDPDt.Text = "0"
            tbPPNPaidDt.Text = "0"
            tbPPnForexDt.Text = "0"

            tbTotalDPDt.Text = "0"
            TbTotalPaidDt.Text = "0"
            tbTotalForexDt.Text = "0"
            tbTotalHomeDt.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            ddlPayTypeDt2.SelectedIndex = 0
            tbReceiptDateDt2.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbDocumentNoDt2.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            tbReceiptForexDt2.Text = "0"
            tbReceiptHomeDt2.Text = "0"
            tbChargeRateDt2.Text = "0"
            tbChargeForexDt2.Text = "0"
            tbChargeHomeDt2.Text = "0"
            tbRemarkDt2.Text = tbRemark.Text
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))
            ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))
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
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbSuppName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
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
                If Dr("DPNo").ToString = "" Then
                    lbStatus.Text = MessageDlg("DP No Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) = 1 And Dr("Currency").ToString <> ViewState("Currency") Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("AmountForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    Return False
                End If
            Else
                If tbDPNoDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("DP No Must Have Value")
                    tbDPNoDt.Focus()
                    Return False
                End If
                If ddlCurrDt.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrDt.Focus()
                    Return False
                End If
                If CFloat(tbRateDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    tbRateDt.Focus()
                    Return False
                End If
                'If tbAmountForexDt.Text = "0" Or tbAmountForexDt.Text = "" Then
                '    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                '    tbAmountForexDt.Focus()
                '    Return False
                'End If
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
                If Dr("ReceiptType").ToString = "" Then
                    lbStatus.Text = MessageDlg("Receipt Type Must Have Value")
                    Return False
                End If
                If Dr("FgMode").ToString = "G" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        Return False
                    End If
                    If Dr("BankGiro").ToString = "" Then
                        lbStatus.Text = MessageDlg("Bank Giro Must Have Value")
                        Return False
                    End If
                    If Dr("Due Date").ToString = "" Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        Return False
                    End If
                End If
                If Dr("FgMode").ToString = "B" Or Dr("FgMode").ToString = "K" Then
                    If Dr("Reference").ToString = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        Return False
                    End If
                End If
            Else
                If ddlPayTypeDt2.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Receipt Type Must Have Value")
                    ddlPayTypeDt2.Focus()
                    Return False
                End If
                If tbFgModeDt2.Text = "G" Then
                    If tbDocumentNoDt2.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        tbDocumentNoDt2.Focus()
                        Return False
                    End If
                    If ddlBankGiroDt2.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Bank Giro Must Have Value")
                        ddlBankGiroDt2.Focus()
                        Return False
                    End If
                    If tbDueDateDt2.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        tbDueDateDt2.Focus()
                        Return False
                    End If
                End If
                If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                    If tbVoucherNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        tbVoucherNo.Focus()
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("User_Type").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("User_Code").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("User_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbTotalReceipt, Dt.Rows(0)("TotalReceipt").ToString, ViewState("DigitHome"))
            BindToText(tbTotalDP, Dt.Rows(0)("TotalDP").ToString, ViewState("DigitHome"))
            BindToText(tbTotalCharge, Dt.Rows(0)("TotalCharge").ToString, ViewState("DigitHome"))
            BindToText(tbSelisihKurs, Dt.Rows(0)("TotalDiffRate").ToString, ViewState("DigitHome"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal DPNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("DPNo = " + QuotedStr(DPNo))
            If Dr.Length > 0 Then
                ' FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
                BindToText(tbDPNoDt, Dr(0)("DPNo").ToString)
                BindToDate(tbDPDateDt, Dr(0)("DPDate").ToString)
                BindToDropList(ddlCurrDt, Dr(0)("Currency").ToString)
                BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbDPRate, Dr(0)("DPRate").ToString)
                BindToText(tbPPnRateDt, Dr(0)("PPnRate").ToString)
                BindToText(tbTotalDPDt, Dr(0)("DPTotal").ToString)
                BindToText(TbTotalPaidDt, Dr(0)("DPPaidTotal").ToString)
                BindToText(tbTotalForexDt, Dr(0)("TotalForex").ToString)
                BindToText(tbTotalHomeDt, Dr(0)("TotalHome").ToString)
                BindToText(tbBaseDPDt, Dr(0)("DPBase").ToString)
                BindToText(tbBasePaidDt, Dr(0)("DPPaidBase").ToString)
                BindToText(tbBaseForexDt, Dr(0)("BaseForex").ToString)
                BindToText(tbPPNDPDt, Dr(0)("DPPPn").ToString)
                BindToText(tbPPNPaidDt, Dr(0)("DPPaidPPn").ToString)
                BindToText(tbPPnForexDt, Dr(0)("PPnForex").ToString)

                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))

                If ddlCurrDt.SelectedValue <> "IDR" Then
                    tbRateDt.Enabled = True
                Else
                    tbRateDt.Enabled = False
                End If
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = ItemNo
                BindToDropList(ddlPayTypeDt2, Dr(0)("ReceiptType").ToString)
                BindToDate(tbReceiptDateDt2, Dr(0)("ReceiptDate").ToString)
                BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
                BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                BindToText(tbReceiptForexDt2, Dr(0)("ReceiptForex").ToString)
                BindToText(tbReceiptHomeDt2, Dr(0)("ReceiptHome").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                BindToText(tbFgModeDt2, Dr(0)("FgMode").ToString)
                BindToDropList(ddlBankGiroDt2, Dr(0)("BankGiro").ToString)
                BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurrDt") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)
                If ddlChargeCurrDt2.SelectedValue = "" Then
                    tbChargeForexDt2.Text = "0"
                    tbChargeHomeDt2.Text = "0"
                End If
                tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
                tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
                tbRateDt2.Enabled = ddlCurrDt2.SelectedValue <> Session("Currency")
                ddlBankGiroDt2.Enabled = tbFgModeDt2.Text = "G"
                tbDueDateDt2.Enabled = tbFgModeDt2.Text = "G"
                ddlChargeCurrDt2.Enabled = tbFgModeDt2.Text = "B"
                tbChargeForexDt2.Enabled = tbFgModeDt2.Text = "B"
                tbChargeRateDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> Session("Currency") And tbFgModeDt2.Text = "B"
                tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
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
                Row = ViewState("Dt").Select("DPNo = " + QuotedStr(tbDPNoDt.Text))(0)
                Row.BeginEdit()
                Row("DPNo") = tbDPNoDt.Text
                Row("DPDate") = tbDPDateDt.SelectedDate
                Row("Currency") = ddlCurrDt.SelectedValue
                Row("ForexRate") = tbRateDt.Text
                Row("DPRate") = tbDPRate.Text
                If tbPPnRateDt.Text.Trim = "" Then
                    Row("PPnRate") = "0"
                Else
                    Row("PPnRate") = tbPPnRateDt.Text
                End If
                Row("DPBase") = tbBaseDPDt.Text
                Row("DPPPn") = tbPPNDPDt.Text
                Row("DPTotal") = tbTotalDPDt.Text
                Row("DPPaidBase") = tbBasePaidDt.Text
                Row("DPPaidPPn") = tbPPNPaidDt.Text
                Row("DPPaidTotal") = TbTotalPaidDt.Text
                Row("BaseForex") = tbBaseForexDt.Text
                Row("PPnForex") = tbPPnForexDt.Text
                Row("TotalForex") = tbTotalForexDt.Text
                Row("TotalHome") = tbTotalHomeDt.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("DPNo") = tbDPNoDt.Text
                dr("DPDate") = tbDPDateDt.SelectedDate
                dr("Currency") = ddlCurrDt.SelectedValue
                dr("ForexRate") = tbRateDt.Text
                dr("DPRate") = tbDPRate.Text
                If tbPPnRateDt.Text.Trim = "" Then
                    dr("PPnRate") = "0"
                Else
                    dr("PPnRate") = tbPPnRateDt.Text
                End If
                dr("DPBase") = tbBaseDPDt.Text
                dr("DPPPn") = tbPPNDPDt.Text
                dr("DPTotal") = tbTotalDPDt.Text
                dr("DPPaidBase") = tbBasePaidDt.Text
                dr("DPPaidPPn") = tbPPNPaidDt.Text
                dr("DPPaidTotal") = TbTotalPaidDt.Text
                dr("BaseForex") = tbBaseForexDt.Text
                dr("PPnForex") = tbPPnForexDt.Text
                dr("TotalForex") = tbTotalForexDt.Text
                dr("TotalHome") = tbTotalHomeDt.Text
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
            If ViewState("1Payment").ToString = "Y" Then
                If GetCountRecord(ViewState("Dt2")) >= 1 Then
                    If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                        If ViewState("StateDt2") <> "Edit" Then
                            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
                                lbStatus.Text = "Cannot input more than one receipt type"
                                ddlPayTypeDt2.Focus()
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()
                Row("ReceiptType") = ddlPayTypeDt2.SelectedValue
                Row("ReceiptName") = ddlPayTypeDt2.SelectedItem.Text
                Row("ReceiptDate") = Format(tbReceiptDateDt2.SelectedDate, "dd MMMM yyyy")
                Row("DocumentNo") = tbDocumentNoDt2.Text
                Row("Reference") = tbVoucherNo.Text
                If tbDueDateDt2.Enabled Then
                    Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                End If
                Row("BankGiro") = ddlBankGiroDt2.SelectedValue
                Row("BankGiroName") = ddlBankGiroDt2.SelectedItem.Text
                Row("FgMode") = tbFgModeDt2.Text
                Row("Currency") = ddlCurrDt2.SelectedValue
                Row("ForexRate") = tbRateDt2.Text
                Row("ReceiptForex") = tbReceiptForexDt2.Text
                Row("ReceiptHome") = tbReceiptHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row("ChargeCurrency") = ddlChargeCurrDt2.SelectedValue
                Row("ChargeRate") = tbChargeRateDt2.Text
                Row("ChargeForex") = tbChargeForexDt2.Text
                Row("ChargeHome") = tbChargeHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = CInt(lbItemNoDt2.Text)
                dr("ReceiptType") = ddlPayTypeDt2.SelectedValue
                dr("ReceiptName") = ddlPayTypeDt2.SelectedItem.Text
                dr("ReceiptDate") = Format(tbReceiptDateDt2.SelectedDate, "dd MMMM yyyy")
                dr("DocumentNo") = tbDocumentNoDt2.Text
                dr("Reference") = tbVoucherNo.Text
                If tbDueDateDt2.Enabled Then
                    dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
                dr("BankGiro") = ddlBankGiroDt2.SelectedValue
                dr("BankGiroName") = ddlBankGiroDt2.SelectedItem.Text
                dr("FgMode") = tbFgModeDt2.Text
                dr("Currency") = ddlCurrDt2.SelectedValue
                dr("ForexRate") = tbRateDt2.Text
                dr("ReceiptForex") = tbReceiptForexDt2.Text
                dr("ReceiptHome") = tbReceiptHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                dr("ChargeCurrency") = ddlChargeCurrDt2.Text
                dr("ChargeRate") = tbChargeRateDt2.Text
                dr("ChargeForex") = tbChargeForexDt2.Text
                dr("ChargeHome") = tbChargeHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                ViewState("PayType") = ddlPayTypeDt2.SelectedValue
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

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            'Save Hd

            'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
            'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
            'CR : ItemNo, ReceiptType, ReceiptName, ReceiptDate, DocumentNo, Currency, ForexRate, ReceiptForex, ReceiptHome, Remark, DueDate, BankGiro, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("DSR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINDPSuppReturHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "UserType, UserCode, Attn, TotalReceipt, TotalDP, TotalCharge, TotalDiffRate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("Y") + ", " + QuotedStr(ddlUserType.SelectedValue) + "," + QuotedStr(tbSuppCode.Text) + ", " + _
                QuotedStr(tbAttn.Text) + ", " + tbTotalReceipt.Text.Replace(",", "") + ", " + tbTotalDP.Text.Replace(",", "") + ", " + _
                tbTotalCharge.Text.Replace(",", "") + ", " + tbSelisihKurs.Text.Replace(",", "") + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDPSuppReturHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINDPSuppReturHd SET UserType =" + QuotedStr(ddlUserType.SelectedValue) + ", UserCode = " + QuotedStr(tbSuppCode.Text) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + _
                ", TotalReceipt = " + tbTotalReceipt.Text.Replace(",", "") + _
                ", TotalDP = " + tbTotalDP.Text.Replace(",", "") + _
                ", TotalCharge = " + tbTotalCharge.Text.Replace(",", "") + _
                ", TotalDiffRate = " + tbSelisihKurs.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, DPNo, DPDate, Currency, ForexRate, PPnRate, DPBase, DPPPn, DPTotal, DPPaidBase, DPPaidPPn, DPPaidTotal, BaseForex, PPnForex, TotalForex, TotalHome, DPRate, Remark FROM FINDPSuppReturDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("FINDPSuppReturDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, DueDate, BankGiro, FgMode, Currency, ForexRate, ReceiptForex, ReceiptHome, Remark, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome FROM FINDPSuppReturPay WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("FINDPSuppReturPay")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            btnadddt2.Visible = True
            btnAddDt2Ke2.Visible = True
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
            tbDPNoDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnadddt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
            If ViewState("1Payment").ToString = "Y" Then
                BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
            End If
            ddlPayTypeDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
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
            FDateName = "Receipt Date"
            FDateValue = "TransDate"
            FilterName = "Receipt No, Status, Date, Report, Supplier, Attn, Receipt, DP, Charge, Remark"
            FilterValue = "TransNmbr, Status, dbo.FormatDate(TransDate), FgReport, Supplier, Attn, TotalReceipt, TotalDP, TotalCharge, Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptDP" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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
                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "'''" + ",'DSR'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
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

    Dim TotalDP As Decimal = 0
    Dim BaseForex As Decimal = 0
    Dim PPnForex As Decimal = 0
    Dim ForexRate As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DPNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalDP += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalHome"))
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BaseForex"))
                    PPnForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PPnForex"))
                    ForexRate += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ForexRate"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbTotalDP.Text = FormatNumber(TotalDP, ViewState("DigitHome"))
                    tbSelisihKurs.Text = FormatNumber((((Convert.ToDecimal(BaseForex) + Convert.ToDecimal(PPnForex)) * Convert.ToDecimal(ForexRate)) - Convert.ToDecimal(tbTotalDP.Text)), ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        Finally
            tbDiffRate.Text = FormatNumber(Convert.ToDecimal(tbTotalReceipt.Text) - Convert.ToDecimal(tbTotalDP.Text) + Convert.ToDecimal(tbTotalCharge.Text) - Convert.ToDecimal(tbSelisihKurs.Text), ViewState("DigitHome"))
            AttachScript("setformatdt();", Page, Me.GetType())
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("DPNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalReceipt As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalReceipt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ReceiptHome"))
                    TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ChargeHome"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbTotalReceipt.Text = FormatNumber(TotalReceipt, ViewState("DigitHome"))
                    tbTotalCharge.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        Finally
            tbDiffRate.Text = FormatNumber(Convert.ToDecimal(tbTotalReceipt.Text) - Convert.ToDecimal(tbTotalDP.Text) + Convert.ToDecimal(tbTotalCharge.Text) - Convert.ToDecimal(tbSelisihKurs.Text), ViewState("DigitHome"))
            AttachScript("setformatdt();", Page, Me.GetType())
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPayTypeDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayTypeDt2.SelectedIndexChanged
        Dim dr As DataRow
        Dim VoucherNo As String

        Try

            dr = FindMaster("PayType", ddlPayTypeDt2.SelectedValue, ViewState("DBConnection"))
            If Not dr Is Nothing Then
                BindToText(tbFgModeDt2, dr("FgMode"))
                'BindToDropList(ddlReport, dr("FgReport"))
                BindToDropList(ddlCurrDt2, dr("Currency"))
            Else
                tbFgModeDt2.Text = "B"
            End If

            ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))

            'ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
            ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(dr("Currency")), ViewState("DBConnection"))
            AttachScript("kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();", Page, Me.GetType())
            VoucherNo = ""

            If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlPayTypeDt2.SelectedValue) + ", 'IN', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If

            tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Pay Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDPNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDPNo.Click
        Dim ResultField As String
        Try
            If tbSuppCode.Text.Trim = "" Then
                Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PPn_Rate, Currency, Rate, Base_Forex, PPn_Forex, Total_Forex, Base_Paid, PPn_Paid, Total_Paid FROM V_FNDPSuppPending WHERE Report = " + QuotedStr("Y") 'ddlReport.SelectedValue
            Else
                Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PPn_Rate, Currency, Rate, Base_Forex, PPn_Forex, Total_Forex, Base_Paid, PPn_Paid, Total_Paid FROM V_FNDPSuppPending WHERE Report = " + QuotedStr("Y") + " and User_Type = " + QuotedStr(ddlUserType.SelectedValue) + " and User_Code = " + QuotedStr(tbSuppCode.Text) 'ddlReport.SelectedValue
                'lbStatus.Text = Session("filter")
                'Exit Sub
            End If

            ResultField = "DP_No, DP_Date, PPn_Rate, Currency, Rate, Base_Forex, PPn_Forex, Total_Forex, Base_Paid, PPn_Paid, Total_Paid, User_Type, User_Code, User_Name"
            ViewState("Sender") = "btnDPNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn DP Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select User_Code, User_Name, Contact_Person, Term, Address_1, Address_2, Phone from VMsUserType Where User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn User Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("UserType", tbSuppCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("User_Code")
                tbSuppName.Text = Dr("User_Name")
                tbAttn.Text = Dr("Contact_Person")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbAttn.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb Supplier Code Text Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
        Try
            ChangeCurrency(ddlChargeCurrDt2, tbReceiptDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
            If ddlChargeCurrDt2.SelectedValue = "" Then
                tbChargeForexDt2.Text = "0"
                tbChargeHomeDt2.Text = "0"
            End If
            tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
            tbChargeRateDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptDP" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail DP must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Receipt must have at least 1 record")
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
            lbStatus.Text = "btnSaveAll Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail DP must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Receipt must have at least 1 record")
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

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbSuppCode.Text = ""
        tbSuppName.Text = ""
    End Sub
End Class
