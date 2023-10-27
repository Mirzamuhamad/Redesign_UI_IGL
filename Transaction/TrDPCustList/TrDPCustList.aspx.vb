Imports System.Data
Imports BasicFrame.WebControls
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrDPCustList_TrDPCustList
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If ViewState("DigitCurr") Is Nothing Then
                ViewState("DigitCurr") = 0
            End If
            If ViewState("DP") Is Nothing Then
                ViewState("DP") = 0
            End If

            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            'Hasil dari Advance Filter
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            'Hasil dari Search Dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnSO" Then
                    '"Reff_No, Customer, Customer_Name, Attn, Currency, DP_Forex, DP, PPN "
                    If tbCustCode.Text = "" Or tbCustCode.Text = Session("Result")(1).ToString Then
                        'ResultField = "Reff_No, Customer, Customer_Name, Attn, Currency, Base_Forex, DP, PPN, FgReport, CostCtr"
                        tbSONo.Text = Session("Result")(0).ToString
                        tbCustCode.Text = Session("Result")(1).ToString
                        tbCustName.Text = Session("Result")(2).ToString
                        tbAttn.Text = Session("Result")(3).ToString
                        BindToDropList(ddlCurr, Session("Result")(4).ToString)
                        BindToText(tbBaseForex, Session("Result")(5).ToString)
                        BindToText(tbDP, Session("Result")(6).ToString)
                        ViewState("DP") = Session("Result")(6).ToString
                        BindToText(tbPPN, Session("Result")(7).ToString)
                        'tbCostCtrCode.Text = Session("Result")(4).ToString
                        'tbCostCtrCode_TextChanged(Nothing, Nothing)
                        BindToDropList(ddlCostCtr, Session("Result")(9).ToString)
                        ddlCostCtr.Enabled = (ddlCostCtr.SelectedValue = "") And (tbSONo.Text = "")
                        ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))

                        ViewState("SOData") = SQLExecuteQuery("SELECT Reff_No, Revisi, Reff_Date, Product_Code, Product_Name, Qty, Unit, PriceForex, Amount_Forex FROM V_FNDPCustListGetSO WHERE Reff_No = " + QuotedStr(tbSONo.Text), ViewState("DBConnection")).Tables(0)
                        Dim drResult As DataRow
                        ViewState("INo") = 1
                        For Each drResult In ViewState("SOData").Rows
                            'insert
                            'ViewState("SJ_No") = drResult("SJ_No")
                            If CekExistData(ViewState("Dt"), "ProductName", drResult("Product_Name")) = False Then
                                Dim dr As DataRow
                                dr = ViewState("Dt").NewRow
                                dr("ItemNo") = ViewState("INo")
                                dr("ProductName") = drResult("Product_Name")
                                dr("Unit") = drResult("Unit")
                                dr("Qty") = drResult("Qty")
                                dr("PriceForex") = FormatFloat(CFloat(drResult("PriceForex")) * CFloat(ViewState("DP")) / 100.0, 4)
                                dr("AmountForex") = FormatFloat(CFloat(drResult("Amount_Forex")) * CFloat(ViewState("DP")) / 100.0, ViewState("DigitCurr"))
                                ViewState("Dt").Rows.Add(dr)
                            End If
                            ViewState("INo") = ViewState("INo") + 1
                        Next
                        BindGridDt(ViewState("Dt"), GridDt)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        AttachScript("setformat();", Page, Me.GetType())
                        AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Me.Page, Me.GetType())
                        ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End If
                
                If ViewState("Sender") = "btnCustTax" Then
                    BindToText(tbCustTaxAddress, Session("Result")(0).ToString)
                    BindToText(tbCustTaxNPWP, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnProduct" Then
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbQty, Session("Result")(2).ToString)
                    BindToDropList(ddlUnit, Session("Result")(3).ToString)
                    BindToText(tbPriceForex, Session("Result")(4).ToString)
                    tbAmountForex.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)), ViewState("DigitCurr"))
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    ViewState("INo") = 1
                    For Each drResult In Session("Result").Rows
                        'insert
                        'ViewState("SJ_No") = drResult("SJ_No")
                        If CekExistData(ViewState("Dt"), "ProductName", drResult("Product_Name")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = ViewState("INo")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Unit") = drResult("Unit")
                            dr("Qty") = drResult("Qty")
                            dr("PriceForex") = drResult("PriceForex")
                            dr("AmountForex") = FormatFloat(drResult("PriceForex") * drResult("Qty"), ViewState("DigitCurr"))
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        ViewState("INo") = ViewState("INo") + 1
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    AttachScript("setformat();", Page, Me.GetType())
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
                
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error :" + ex.ToString
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
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction("Select * From V_FNDPCustList", StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_FNDPCustListDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
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
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput(True, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ViewState("StateHd") = "Insert"
            newTrans()
            MovePanel(PnlHd, pnlInput)
            ChangeReport("Add", "Y", True, tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnHome.Visible = False
            tbTransDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            'ddlReport.SelectedValue = "N"
            tbSONo.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbAttn.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbRate.Text = "1"
            tbRate.Enabled = False
            tbPPNNo.Text = ""
            tbPPNDate.Clear()
            tbPPNRate.Text = "0"
            tbDP.Text = "0"
            tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPPN.Text = "10"
            tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRemark.Text = ""
            tbCustTaxAddress.Text = ""
            tbCustTaxNPWP.Text = ""
            tbBaseForex.Enabled = False
            ddlCostCtr.SelectedIndex = 0
        Catch ex As Exception
            lbStatus.Text = "ClearHd Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearDt()
        Try
            tbProductName.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedValue = ""
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
        Catch ex As Exception
            lbStatus.Text = "ClearDt Error : " + ex.ToString
        End Try
    End Sub
        
    Private Sub newTrans()
        Try            
            ClearHd()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
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
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNDPCustList", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
                ViewState("TransNmbr") = GVR.Cells(2).Text
                If DDL.SelectedValue = "View" Then
                    btnHome.Visible = True                    
                    MovePanel(PnlHd, pnlInput)
                    FillTextBox(GVR)
                    BindDataDt(ViewState("TransNmbr"))
                    ' change report harus diatas modifyinput dan dibawah filTextBox
                    ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
                    ModifyInput(False, pnlInput)
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)                    
                    ViewState("StateHd") = "View"
                ElseIf DDL.SelectedValue = "Edit" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    ViewState("StateHd") = "Edit"
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        MovePanel(PnlHd, pnlInput)
                        FillTextBox(GVR)
                        BindDataDt(GVR.Cells(2).Text)
                        btnHome.Visible = False
                        ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
                        ModifyInput(True, pnlInput)
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        GridDt.Columns(0).Visible = GetCountRecord(ViewState("Dt")) > 0
                        tbBaseForex.Enabled = False
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
                        If GVR.Cells(3).Text <> "P" Then
                            lbStatus.Text = MessageDlg("Status " + GVR.Cells(2).Text + " must be P")
                            Exit Sub
                        End If
                        'If (GVR.Cells(3).Text = "P") Then '2015-06-03-0191
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormDPCustInv '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormDPCustInv.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                        'Else
                        ''lbStatus.Text = "Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                        'lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
                        'Exit Sub
                        'End If

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Tax" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        If GVR.Cells(3).Text <> "P" Then
                            lbStatus.Text = MessageDlg("Status " + GVR.Cells(2).Text + " must be P")
                            Exit Sub
                        End If
                        'If (GVR.Cells(3).Text = "P") Then
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormDPCustInvFPS '''" + GVR.Cells(2).Text + "'''"
                        
                        Session("ReportFile") = ".../../../Rpt/FormDPCustInvFaktur.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                        'Else
                        'lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                        'Exit Sub
                        'End If
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

                    SqlString = "Declare @A VarChar(255) EXEC S_FNDPCustListDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "

                    SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Row Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillTextBox(ByVal e As GridViewRow)
        Dim Dt As DataTable
        Dim Nmbr As String
        Try
            Nmbr = e.Cells(2).Text
            Dt = BindDataTransaction("Select * From V_FNDPCustList", "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbSONo, Dt.Rows(0)("SONo").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbDP, Dt.Rows(0)("DP").ToString)
            BindToText(tbPPNNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPNDate, Dt.Rows(0)("PPnDate").ToString)
            BindToText(tbPPNRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbCustTaxAddress, Dt.Rows(0)("CustTaxAddress").ToString)
            BindToText(tbCustTaxNPWP, Dt.Rows(0)("CustTaxNPWP").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Function cekhd() As Boolean
        Try
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbTransDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbTransDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbCustName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Customer Cannot Empty.")
                tbCustCode.Focus()
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
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            'If CFloat(tbBaseForex.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Base Forex must be input values")
            '    tbBaseForex.Focus()
            '    Return False
            'End If
            If tbPPN.Text.Length <= 0 Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN No Cannot Empty.")
                tbPPN.Focus()
                Return False
            End If
            'If CFloat(tbTotalForex.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Total Forex must be input values")
            '    tbTotalForex.Focus()
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub SaveData()
        Dim SQLString As String
        Dim tgl As String
        Try
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
                tbTransNo.Text = GetAutoNmbr("DC", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "Insert INTO FINDPCustList (TransNmbr, Status, TransDate, FgReport, " + _
                "SONo, Customer, Attn, CustTaxAddress, CustTaxNPWP, Currency, ForexRate, DP, PPNNo, PPNDate, " + _
                "PPNRate, BaseForex, PPN, PPNForex, TotalForex, Remark, UserPrep, DatePrep, TotalReceipt, CostCtr) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr("Y") + "," + QuotedStr(tbSONo.Text) + "," + _
                QuotedStr(tbCustCode.Text) + "," + QuotedStr(tbAttn.Text) + "," + _
                QuotedStr(tbCustTaxAddress.Text) + ", " + QuotedStr(tbCustTaxNPWP.Text) + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + _
                tbDP.Text.Replace(",", "") + "," + QuotedStr(tbPPNNo.Text) + "," + _
                tgl + "," + tbPPNRate.Text.Replace(",", "") + "," + _
                tbBaseForex.Text.Replace(",", "") + "," + tbPPN.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", Getdate(),0," + QuotedStr(ddlCostCtr.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDPCustList WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINDPCustList SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", FgReport=" + QuotedStr("Y") + ", SONo=" + QuotedStr(tbSONo.Text) + ", Customer=" + QuotedStr(tbCustCode.Text) + _
                ", CustTaxAddress = " + QuotedStr(tbCustTaxAddress.Text) + ", CustTaxNPWP = " + QuotedStr(tbCustTaxNPWP.Text) + _
                ", Attn=" + QuotedStr(tbAttn.Text) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate=" + tbRate.Text.Replace(",", "") + ", PPNNo=" + QuotedStr(tbPPNNo.Text) + _
                ", DP=" + tbDP.Text.Replace(",", "") + ", PPNDate=" + tgl + ", PPNRate=" + tbPPNRate.Text.Replace(",", "") + _
                ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + ", PPN=" + tbPPN.Text.Replace(",", "") + _
                ", PPNForex= " + tbPPNForex.Text.Replace(",", "") + ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark=" + QuotedStr(tbRemark.Text) + ", CostCtr=" + QuotedStr(ddlCostCtr.SelectedValue) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, ProductName, Qty, Unit, PriceForex, AmountForex " + _
                                        " FROM FINDPCustListDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINDPCustListDt SET ProductName = @ProductName, Qty = @Qty, Unit = @Unit, " + _
            "PriceForex = @PriceForex, AmountForex = @AmountForex " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo ", con)

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
                "DELETE FROM FINDPCustListDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINDPCustListDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()

            ViewState("Dt") = Dt

        Catch ex As Exception
            Throw New Exception("Save Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))

            'ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))

            ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbRate.Focus()
            
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr ERror : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    'End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency');", Page, Me.GetType)
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMsCurrency();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                tbAttn.Text = Dr("Contact_Person")
                tbSONo.Text = ""
                tbCustTaxAddress.Text = ""
                tbCustTaxNPWP.Text = ""
            Else
                tbCustName.Text = ""
                tbCustCode.Text = ""
                tbAttn.Text = ""
                tbSONo.Text = ""
                tbCustTaxAddress.Text = ""
                tbCustTaxNPWP.Text = ""
            End If
            tbCustCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb CustCode Code ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Customer_Code, Customer_Name, Currency, Term FROM VMsCustomer"
            ResultField = "Customer_Code, Customer_Name, Currency, Term"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lkbAdvanceSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbAdvanceSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "DP No., Status, DP Date, Report, Customer, Attn, SO No, PPN No, PPN Date, PPN Rate, " + _
            " Currency, Rate, Base Forex, PPn, PPN Forex, Total Forex, Remark"
            FilterValue = "TransNmbr, Status, dbo.FormatDate(TransDate), FgReport, Customer_Name, Attn, SONo, PPNNo, dbo.FormatDate(PPnDate), PPNRate, " + _
            " Currency, ForexRate, BaseForex, PPn, PPNForex, TotalForex, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
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


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
            BtnSearch_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "Btn Back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            BtnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If cekhd() = False Then
                Exit Sub
            End If

            SaveData()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save New Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSO.Click
        Dim ResultField As String
        Try
            If tbCustCode.Text = "" Then
                Session("filter") = "SELECT DISTINCT Reff_No, Reff_Date, FgReport, Customer, Customer_Name, Attn, CostCtr, Currency, dbo.FormatFloat(Base_Forex, dbo.DigitCurrForex(Currency)) AS Base_Forex, DP, PPN, dbo.FormatFloat(Total_Forex, dbo.DigitCurrForex(Currency)) AS Total_Forex FROM V_FNDPCustListGetSO WHERE Customer IS NOT NULL "
            Else
                Session("filter") = "SELECT DISTINCT Reff_No, Reff_Date, FgReport, Customer, Customer_Name, Attn, CostCtr, Currency, dbo.FormatFloat(Base_Forex, dbo.DigitCurrForex(Currency)) AS Base_Forex, DP, PPN, dbo.FormatFloat(Total_Forex, dbo.DigitCurrForex(Currency)) AS Total_Forex FROM V_FNDPCustListGetSO WHERE Customer = " + QuotedStr(tbCustCode.Text)
            End If
            ResultField = "Reff_No, Customer, Customer_Name, Attn, Currency, Base_Forex, DP, PPN, FgReport, CostCtr"
            ViewState("Sender") = "btnSO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search SO Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTransDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTransDate.SelectionChanged
        Try
            tbPPNDate.SelectedDate = tbTransDate.SelectedDate
        Catch ex As Exception
            lbStatus.Text = "tbTransDate_SelectionChanged : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCustTax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustTax.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT CustCode, CustTaxAddress, CustTaxNPWP FROM V_MsCustTaxAddress WHERE CustCode = " + QuotedStr(tbCustCode.Text)
            ResultField = "CustTaxAddress, CustTaxNPWP"
            ViewState("Sender") = "btnCustTax"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn CustTax Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCustCode.Enabled = State
            btnCust.Visible = State
            btnSO.Visible = State
            ddlCurr.Enabled = State
            tbPPN.Enabled = State
            BtnGetData.Visible = False
            tbRate.Enabled = ddlCurr.SelectedValue <> ViewState("Currency")
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            ClearDt()
            If cekhd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            tbProductName.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add DP error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGetData.Click
        Dim ResultField, CriteriaField, DefaultField, ResultSame As String
        Try
            Session("Filter") = "SELECT Reff_No, Revisi, Reff_Date, Product_Code, Product_Name, Qty, Unit, PriceForex FROM V_FNDPCustListGetSO WHERE Reff_No = " + QuotedStr(tbSONo.Text)
            ResultField = "Reff_No, Revisi, Reff_Date, Product_Code, Product_Name, Qty, Unit, PriceForex"
            DefaultField = "Reff_No"
            CriteriaField = "Reff_No, Revisi, Reff_Date, Product_Code, Product_Name, Qty, Unit, PriceForex"
            Session("ClickSame") = "Reff_No"
            ViewState("Sender") = "btnGetDt"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "SO_No"
            Session("ResultSame") = ResultSame.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
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
                tbPriceForex.Text = FormatFloat(CFloat(Dr(0)("PriceForex")), 4)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Private Sub totalingDt()
        Dim dr As DataRow
        Dim amount As Double
        Try
            amount = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    amount = amount + CFloat(dr("AmountForex").ToString)
                End If
            Next
            tbBaseForex.Text = FormatFloat(amount, CInt(ViewState("DigitCurr")))
            tbPPNForex.Text = FormatFloat((CFloat(tbPPN.Text) / 100) * CFloat(tbBaseForex.Text), ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
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
                Row("PriceForex") = FormatFloat(tbPriceForex.Text, 4)
                Row("AmountForex") = FormatFloat(tbAmountForex.Text, ViewState("DigitCurr"))
                
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
                dr("PriceForex") = FormatFloat(tbPriceForex.Text, 4)
                dr("AmountForex") = FormatFloat(tbAmountForex.Text, ViewState("DigitCurr"))
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

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Product_Code, Product_Name, Qty, Unit, PriceForex FROM V_FNDPCustListGetSO WHERE Reff_No = " + QuotedStr(tbSONo.Text)
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

    Protected Sub tbPPNDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPNDate.SelectionChanged
        Try
            If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                tbPPNRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPNDate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            Else
                tbPPNRate.Text = FormatFloat("0", ViewState("DigitCurr"))
            End If
        Catch ex As Exception
            lbStatus.Text = "tbPPNDate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDP.TextChanged
        Try
            If CFloat(tbDP.Text) > ViewState("DP") Then
                lbStatus.Text = MessageDlg("DP can not higher than " + ViewState("DP").ToString + "%")
                tbDP.Focus()
                Exit Sub
            End If

            'BindDataDt("")

            BindGridDt(ViewState("Dt"), GridDt)

            ViewState("DataDP") = SQLExecuteQuery("EXEC S_FNDPCustListHitungDP  " + QuotedStr(tbSONo.Text) + "," + tbDP.Text.Replace(",", ""), ViewState("DBConnection")).Tables(0)
            Dim drResult As DataRow
            ViewState("Item") = 1
            For Each drResult In ViewState("DataDP").Rows
                'insert
                'ViewState("SJ_No") = drResult("SJ_No")
                If CekExistData(ViewState("Dt"), "ProductName", drResult("Product_Name")) = False Then
                    Dim dr As DataRow
                    lbStatus.Text = "AAA"
                    Exit Sub
                    dr = ViewState("Dt").NewRow
                    dr = ViewState("Dt").BeginEdit()
                    dr("ItemNo") = ViewState("Item")
                    dr("ProductName") = drResult("Product_Name")
                    dr("Unit") = drResult("Unit")
                    dr("Qty") = drResult("Qty")
                    dr("PriceForex") = FormatFloat(drResult("PriceForex"), 4)
                    dr("AmountForex") = FormatFloat(drResult("AmountForex"), ViewState("DigitCurr"))
                    ViewState("Dt").Rows.Add(dr)
                Else
                    Dim Row As DataRow()

                    'Row = ViewState("Dt").Select("ProductName " + drResult("Product_Name"))(0)
                    Row = ViewState("Dt").Select("ProductName = " + QuotedStr(drResult("Product_Name")))
                    'If CekDt() = False Then
                    '    Exit Sub
                    'End If
                    
                    For I = 0 To Row.Length - 1
                        Row(I).BeginEdit()
                        Row(I)("PriceForex") = FormatFloat(drResult("PriceForex"), 4)
                        Row(I)("AmountForex") = FormatFloat(drResult("AmountForex"), ViewState("DigitCurr"))
                        Row(I).EndEdit()
                    Next
                End If
                ViewState("Item") = ViewState("Item") + 1
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            AttachScript("setformat();", Page, Me.GetType())
            AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Me.Page, Me.GetType())
            ddlCurr_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "tbDP_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
