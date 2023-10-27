
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrPRFreight_TrPRFreight
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, Nmbr, TransDate, Status, PONo, Supplier, Supplier_Name, BLNo, AJUNo, ContainerNo, Remark, UserPrep, DatePrep, UserAppr, DateAppr FROM V_PRFreightHd"

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
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    tbPONo.Text = ""
                End If
                If ViewState("Sender") = "btnPONo" Then
                    tbPONo.Text = Session("Result")(0).ToString
                    tbSuppCode.Text = Session("Result")(1).ToString
                    tbSuppName.Text = Session("Result")(2).ToString
                End If
                If ViewState("Sender") = "btnPayNonTrade" Then
                    tbReference.Text = Session("Result")(0).ToString
                    tbConsignee.Text = Session("Result")(1).ToString
                    tbConsigneeName.Text = Session("Result")(2).ToString
                    ddlCurrDt2.SelectedValue = Session("Result")(3).ToString
                    ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    tbAmountForexDt2.Text = Session("Result")(4).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnCostFreight" Then
                    tbCostFreight.Text = Session("Result")(0).ToString
                    tbCostFreightName.Text = Session("Result")(1).ToString
                    tbReference.Text = ""
                End If
                If ViewState("Sender") = "btnConsignee" Then
                    tbConsignee.Text = Session("Result")(0).ToString
                    tbConsigneeName.Text = Session("Result")(1).ToString
                    tbReference.Text = ""
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'BindToText(tbSuppCode, drResult("Supp_Code"))
                        'BindToText(tbSuppName, drResult("Supplier_Name"))
                        'BindToText(tbPONo, drResult("PO_No"))
                        If CekExistData(ViewState("Dt"), "RRNo,Product", drResult("RR_No") + "|" + drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("RRNo") = drResult("RR_No")
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = TrimStr(drResult("Product_Name").ToString)
                            dr("Unit") = drResult("Unit")
                            dr("Qty") = drResult("Qty")
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
        'ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitQty") = 2
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
            FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

            ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If

            tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPnDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")            
            tbPPnForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")            
            tbTotalForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRateDt2.Attributes.Add("OnBlur", "setformatdt2();")
            tbAmountForexDt2.Attributes.Add("OnBlur", "setformatdt2('AF');")
            tbPPnDt2.Attributes.Add("OnBlur", "setformatdt2('PP');")
            tbPPnForexDt2.Attributes.Add("OnBlur", "setformatdt2('PF');")
            tbTotalForexDt2.Attributes.Add("OnBlur", "setformatdt2('TF');")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
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
        Return "SELECT * From V_PRFreightDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRFreightCost WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PRFormFreight " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormPRFreight.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRFreight", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            GridDt2.Columns(0).Visible = GridDt2.Rows.Count > 0
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
            tbPONo.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbBLNo.Text = ""
            tbAJUNo.Text = ""
            tbContainerNo.Text = ""
            tbRemark.Text = ""
            
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbRRNo.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbCostFreight.Text = ""
            tbCostFreightName.Text = ""
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbAmountForexDt2.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPPnDt2.Text = FormatFloat(0, ViewState("DigitPercent"))
            tbPPnForexDt2.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForexDt2.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbConsignee.Text = tbSuppCode.Text.Trim
            tbConsigneeName.Text = tbSuppName.Text.Trim
            ddlCostDistribution.SelectedValue = "Weight"
            ddlFgPrepaid.SelectedValue = "N"
            tbReference.Text = ""
            'tbReference.Enabled = ddlFgPrepaid.SelectedValue = "Y"
            btnPayNonTrade.Visible = ddlFgPrepaid.SelectedValue = "Y"
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
            If tbPONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PO No must have value")
                tbPONo.Focus()
                Return False
            End If
            If tbSuppCode.Text.Trim = "" Then
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
            'If Not Dr Is Nothing Then
            '    If Dr.RowState = DataRowState.Deleted Then
            '        Return True
            '    End If
            '    If Dr("Account").ToString = "" Then
            '        lbStatus.Text = MessageDlg("Account Must Have Value")
            '        Return False
            '    End If
            '    If CFloat(Dr("AmountForex").ToString) <= 0 Then
            '        lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
            '        Return False
            '    End If
            'Else
            '    If tbProductCode.Text.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Account Must Have Value")
            '        tbProductCode.Focus()
            '        Return False
            '    End If
            '    If CFloat(tbQty.Text) <= 0 Then
            '        lbStatus.Text = MessageDlg("Qty Must Have Value")
            '        tbQty.Focus()
            '        Return False
            '    End If
            'End If
            'Return True
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
                If Dr("CostFreightName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Cost Freight Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If Dr("ForexRate").ToString = "" Then
                    lbStatus.Text = MessageDlg("Forex Rate Must Have Value")
                    Return False
                End If
                If Dr("AmountForex").ToString = "" Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    Return False
                End If
                If Dr("Consignee_Name").ToString = "" Then
                    lbStatus.Text = MessageDlg("Consignee Must Have Value")
                    Return False
                End If
                If Dr("CostDistribution").ToString = "" Then
                    lbStatus.Text = MessageDlg("Cost Distribution Must Have Value")
                    Return False
                End If
                If Dr("FgPrepaid").ToString = "" Then
                    lbStatus.Text = MessageDlg("Prepaid Must Have Value")
                    Return False
                End If
                If Dr("Reference").ToString = "" Then
                    lbStatus.Text = MessageDlg("Reference Must Have Value")
                    Return False
                End If
            Else

                If tbCostFreightName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Freight Must Have Value")
                    tbCostFreight.Focus()
                    Return False
                End If
                
                If ddlCurrDt2.SelectedValue <> ViewState("Currency").ToString And CFloat(tbRateDt2.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Currenct Rate Must Have Value")
                    tbRateDt2.Focus()
                    Return False
                End If
                If CFloat(tbAmountForexDt2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    tbAmountForexDt2.Focus()
                    Return False
                End If
                If tbConsigneeName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Consignee Name Must Have Value")
                    tbConsignee.Focus()
                    Return False
                End If
                If tbReference.Text.Trim = "" And ddlFgPrepaid.SelectedValue = "Y" Then
                    lbStatus.Text = MessageDlg("Reference Must Have Value")
                    btnPayNonTrade.Focus()
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
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbBLNo, Dt.Rows(0)("BLNo").ToString)
            BindToText(tbAJUNo, Dt.Rows(0)("AJUNo").ToString)
            BindToText(tbContainerNo, Dt.Rows(0)("ContainerNo").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("RRNo = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbRRNo, Dr(0)("RRNo").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("CostFreight = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbCostFreight, Dr(0)("CostFreight").ToString)
                BindToText(tbCostFreightName, Dr(0)("CostFreightName").ToString)
                BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                BindToText(tbAmountForexDt2, Dr(0)("AmountForex").ToString)
                BindToText(tbPPnDt2, Dr(0)("PPn").ToString)
                BindToText(tbPPnForexDt2, Dr(0)("PPnForex").ToString)
                BindToText(tbTotalForexDt2, Dr(0)("TotalForex").ToString)
                BindToText(tbConsignee, Dr(0)("Consignee").ToString)
                BindToText(tbConsigneeName, Dr(0)("Consignee_Name").ToString)
                BindToDropList(ddlCostDistribution, Dr(0)("CostDistribution").ToString)
                BindToDropList(ddlFgPrepaid, Dr(0)("FgPrepaid").ToString)
                BindToText(tbReference, Dr(0)("Reference").ToString)
                'tbReference.Enabled = ddlFgPrepaid.SelectedValue = "Y"
                btnPayNonTrade.Visible = ddlFgPrepaid.SelectedValue = "Y"
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            'If CekDt() = False Then
            '    btnSaveDt.Focus()
            '    Exit Sub
            'End If

            'If ViewState("StateDt") = "Edit" Then
            '    Dim Row As DataRow
            '    Row = ViewState("Dt").Select("RRNo = " + QuotedStr("a"))(0)
            '    Row.BeginEdit()
            '    Row("Account") = tbProductCode.Text
            '    Row("AccountName") = tbProductName.Text

            '    Row.EndEdit()
            'Else
            '    'Insert
            '    Dim dr As DataRow
            '    dr = ViewState("Dt").NewRow
            '    dr("Account") = tbProductCode.Text
            '    dr("AccountName") = tbProductName.Text

            '    ViewState("Dt").Rows.Add(dr)
            'End If
            'MovePanel(pnlEditDt, PnlDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDtModif(ViewState("Dt"), GridDt)
            'StatusButtonSave(True)
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
                Row = ViewState("Dt2").Select("CostFreight = " + QuotedStr(tbCostFreight.Text))(0)
                Row.BeginEdit()
                Row("CostFreight") = tbCostFreight.Text
                Row("CostFreightName") = tbCostFreightName.Text
                Row("Currency") = ddlCurrDt2.SelectedValue
                Row("ForexRate") = tbRateDt2.Text
                Row("AmountForex") = tbAmountForexDt2.Text
                Row("PPn") = tbPPnDt2.Text
                Row("PPnForex") = tbPPnForexDt2.Text
                Row("PPnForex") = tbPPnForexDt2.Text
                Row("TotalForex") = tbTotalForexDt2.Text
                Row("Consignee_Name") = tbConsigneeName.Text
                Row("CostDistribution") = ddlCostDistribution.SelectedItem.Text
                Row("FgPrepaid") = ddlFgPrepaid.SelectedItem.Text
                Row("Reference") = tbReference.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("CostFreight") = tbCostFreight.Text
                dr("CostFreightName") = tbCostFreightName.Text
                dr("Currency") = ddlCurrDt2.SelectedValue
                dr("ForexRate") = tbRateDt2.Text
                dr("AmountForex") = tbAmountForexDt2.Text
                dr("PPnForex") = tbPPnForexDt2.Text
                dr("PPnForex") = tbPPnForexDt2.Text
                dr("TotalForex") = tbTotalForexDt2.Text
                dr("Consignee") = tbConsignee.Text
                dr("Consignee_Name") = tbConsigneeName.Text
                dr("CostDistribution") = ddlCostDistribution.SelectedItem.Text
                dr("FgPrepaid") = ddlFgPrepaid.SelectedItem.Text
                dr("Reference") = tbReference.Text
                ViewState("Dt2").Rows.Add(dr)
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("PC", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PRCFreightHd (TransNmbr, TransDate, STATUS, " + _
                "PONo, Supplier, BLNo, AJUNo, ContainerNo, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbPONo.Text) + "," + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbBLNo.Text) + "," + _
                QuotedStr(tbAJUNo.Text) + "," + QuotedStr(tbContainerNo.Text) + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCFreightHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCFreightHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", PONo =" + QuotedStr(tbPONo.Text) + _
                ", Supplier =" + QuotedStr(tbSuppCode.Text) + _
                ", BLNo =" + QuotedStr(tbBLNo.Text) + _
                ", AJUNo =" + QuotedStr(tbAJUNo.Text) + _
                ", ContainerNo =" + QuotedStr(tbContainerNo.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, RRNo, Product, Qty, Unit FROM PRCFreightDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE PRCFreightDt SET RRNo = @RRNo, Product = @Product, " + _
            "Qty = @Qty, Unit = @Unit " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND RRNo = @OldRRNo AND Product = @OldProduct ", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@RRNo", SqlDbType.VarChar, 20, "RRNo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldRRNo", SqlDbType.VarChar, 20, "RRNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCFreightDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND RRNo = @RRNo AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@RRNo", SqlDbType.VarChar, 20, "RRNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCFreightDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand(" SELECT TransNmbr, CostFreight, Currency, ForexRate, AmountForex, PPn, PPNForex, TotalForex, Consignee, CostDistribution, FgPrepaid, Reference FROM PRCFreightCost WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
                    "UPDATE PRCFreightCost SET CostFreight = @CostFreight, Currency = @Currency, ForexRate = @ForexRate, " + _
                    "AmountForex = @AmountForex, Consignee = @Consignee, CostDistribution = @CostDistribution, FgPrepaid = @FgPrepaid, " + _
                    "Reference = @Reference, PPn = @PPn, PPNForex = @PPNForex, TotalForex = @TotalForex " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND CostFreight = @OldCostFreight", con)
            ' Define output parameters.
            Update_Command2.Parameters.Add("@CostFreight", SqlDbType.VarChar, 5, "CostFreight")
            Update_Command2.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command2.Parameters.Add("@ForexRate", SqlDbType.Float, 18, "ForexRate")
            Update_Command2.Parameters.Add("@AmountForex", SqlDbType.Float, 18, "AmountForex")
            Update_Command2.Parameters.Add("@PPn", SqlDbType.Float, 18, "PPn")
            Update_Command2.Parameters.Add("@PPNForex", SqlDbType.Float, 18, "PPNForex")
            Update_Command2.Parameters.Add("@TotalForex", SqlDbType.Float, 18, "TotalForex")
            Update_Command2.Parameters.Add("@Consignee", SqlDbType.VarChar, 12, "Consignee")
            Update_Command2.Parameters.Add("@CostDistribution", SqlDbType.VarChar, 15, "CostDistribution")
            Update_Command2.Parameters.Add("@FgPrepaid", SqlDbType.VarChar, 1, "FgPrepaid")
            Update_Command2.Parameters.Add("@Reference", SqlDbType.VarChar, 20, "Reference")
            
            ' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldCostFreight", SqlDbType.VarChar, 5, "CostFreight")
            param2.SourceVersion = DataRowVersion.Original
            
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM PRCFreightCost WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND CostFreight = @CostFreight", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@CostFreight", SqlDbType.VarChar, 5, "CostFreight")
            param2.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("PRCFreightCost")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
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
                lbStatus.Text = MessageDlg("Detail RR must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Cost must have at least 1 record")
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
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
    '    Try
    '        Cleardt()
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        ViewState("StateDt") = "Insert"
    '        MovePanel(PnlDt, pnlEditDt)
    '        EnableHd(False)
    '        StatusButtonSave(False)
    '        btnRRNo.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub
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
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitExpenseCurr") = 0
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
            FDateName = "Cost Date"
            FDateValue = "TransDate"
            FilterName = "Cost No, Cost Date, PO No, Supplier Code, Supplier Name, BL No, AJU No, Container No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), PONo, Supplier, Supplier_Name, BLNo, AJUNo, ContainerNo, Remark"
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
                    btnGetDt.Visible = False
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
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        btnGetDt.Visible = True
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
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
                        Session("SelectCommand") = "EXEC S_PRFormFreight ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPRFreight.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Full" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt '''" + GVR.Cells(2).Text + "''','PAYMENTNONTRADE' , " + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
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

    Dim TotalExpense As Decimal = 0
    ' untuk tampilkan data total di grid


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("RRNo = " + QuotedStr(GVR.Cells(1).Text))
            For Each r In dr
                r.Delete()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("CostFreight = " + QuotedStr(GVR.Cells(1).Text))
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
            'row = ViewState("Dt").Rows(e.NewEditIndex)
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

    

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsProduct WHERE Fg_Active = 'Y' "
            ResultField = "Product_Code, Product_Name, UnitOrder"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Product Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbProductCode.Text + "|'Y'", ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProductCode.Text = Dr("ProductCode")
                tbProductName.Text = Dr("ProductName")
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsSupplier WHERE FgActive = 'Y'"
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Supp Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            tbPONo.Text = ""
        Catch ex As Exception
            lbStatus.Text = "tb Supp Code Text Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail RR must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Cost must have at least 1 record")
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

    
    Protected Sub btnPONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPONo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            If tbSuppCode.Text.Trim = "" Then
                Session("filter") = "SELECT * FROM V_PRFreightGetPORR "
            Else
                Session("filter") = "SELECT * FROM V_PRFreightGetPORR WHERE Supplier = " + QuotedStr(tbSuppCode.Text.Trim)
            End If
            ResultField = "PO_No, Supplier, Supplier_Name"
            ViewState("Sender") = "btnPONo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn PONo Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField, ResultSame, Filter As String
        Dim CriteriaField As String
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("Result") = Nothing
            Filter = ""
            If tbPONo.Text.Trim <> "" Then
                Filter = Filter + " AND PO_No = " + QuotedStr(tbPONo.Text)
            End If
            Session("Filter") = "EXEC S_PRFreightReff " + QuotedStr(Filter) + ", ''"
            ResultField = "PO_No, PO_Date, RR_No, RR_Date, SJ_Supplier_No, SJ_Supplier_Date, Product_Code, Product_Name, Qty, Unit"
            CriteriaField = "PO_No, PO_Date, , RR_No, RR_Date, SJ_Supplier_No, SJ_Supplier_Date, Product_Code, Product_Name"
            Session("ClickSame") = "RR_No"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "PO_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub BindGridDtModif(ByVal source As DataTable, ByVal gv As GridView)
    '    Dim IsEmpty As Boolean
    '    Dim DtTemp As DataTable
    '    Dim dr As DataRow()
    '    Try
    '        IsEmpty = False
    '        dr = source.Select("", "", DataViewRowState.CurrentRows)
    '        If dr.Count = 0 Then
    '            'If source.Rows.Count = 0 Then
    '            DtTemp = source.Clone
    '            DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
    '            IsEmpty = True
    '            gv.DataSource = DtTemp
    '        Else
    '            gv.DataSource = source
    '        End If
    '        gv.DataBind()
    '    Catch ex As Exception
    '        Throw New Exception("BindGridDtModif Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnCostFreight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCostFreight.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT CostFreightCode, CostFreightName FROM  VMsCostFreight"
            ResultField = "CostFreightCode, CostFreightName "
            ViewState("Sender") = "btnCostFreight"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn CostFreight Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnConsignee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsignee.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM  VMsConsignee"
            ResultField = "Consignee_Code, Consignee_Name "
            ViewState("Sender") = "btnConsignee"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Consignee Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCostFreight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCostFreight.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("CostFreight", tbCostFreight.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCostFreight.Text = Dr("CostFreightCode")
                tbCostFreightName.Text = Dr("CostFreightName")
                tbReference.Text = ""
            Else
                tbCostFreight.Text = ""
                tbCostFreightName.Text = ""
                tbReference.Text = ""
            End If
            tbCostFreight.Focus()
        Catch ex As Exception
            Throw New Exception("tb CostFreight change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbConsignee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbConsignee.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Consignee", tbConsignee.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbConsignee.Text = Dr("Consignee_Code")
                tbConsigneeName.Text = Dr("Consignee_Name")
                tbReference.Text = ""
            Else
                tbConsignee.Text = ""
                tbConsigneeName.Text = ""
                tbReference.Text = ""
            End If
            tbConsignee.Focus()
        Catch ex As Exception
            Throw New Exception("tb Consignee change Error : " + ex.ToString)
        End Try
    End Sub

    
    Protected Sub ddlCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDt2.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ddlCurrDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "DDL Curr Hd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlFgPrepaid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgPrepaid.SelectedIndexChanged
        Try
            'tbReference.Enabled = (ddlFgPrepaid.SelectedValue = "Y")
            btnPayNonTrade.Visible = ddlFgPrepaid.SelectedValue = "Y"
            tbReference.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddlFgPrepaid_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPayNonTrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPayNonTrade.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            If tbConsignee.Text.Trim = "" Then
                Session("filter") = "Select Payment_No, dbo.FormatDate(Payment_Date) AS Payment_Date, Supplier, Supplier_Name, Currency, dbo.FormatFloat(Total, dbo.DigitCurrForex(Currency)) AS Total, Remark FROM V_PRFreightGetPayNonTrade WHERE Account = dbo.GetAccountFreight(" + QuotedStr(tbCostFreight.Text.Trim) + ") "
            Else
                Session("filter") = "SELECT Payment_No, dbo.FormatDate(Payment_Date) AS Payment_Date, Supplier, Supplier_Name, Currency, dbo.FormatFloat(Total, dbo.DigitCurrForex(Currency)) AS Total, Remark FROM V_PRFreightGetPayNonTrade WHERE Account = dbo.GetAccountFreight(" + QuotedStr(tbCostFreight.Text.Trim) + ") AND Supplier = " + QuotedStr(tbConsignee.Text.Trim)
            End If
            ResultField = "Payment_No, Supplier, Supplier_Name, Currency, Total"
            ViewState("Sender") = "btnPayNonTrade"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn PONo Click Error : " + ex.ToString
        End Try
    End Sub
End Class
