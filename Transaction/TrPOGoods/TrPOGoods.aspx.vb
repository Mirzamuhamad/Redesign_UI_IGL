Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPOGoods_TrPOGoods
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Private Function GetStringHd(ByVal FgImp As String) As String
        Return "SELECT DISTINCT TransNmbr, Nmbr, Revisi, TransDate, Status, FgReport, ShipmentType, SuppContractNo, CustContractNo, Supplier, Attn, Term, TermPayment, FgAddCost, AddCostRemark, Delivery, DeliveryAddr, DeliveryCity, Currency, ForexRate, BaseForex, Disc, DiscForex, DP, DPForex, PPn, PPnForex, PPhForex, OtherForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, DateAppr, FgActive, POType, SupplierName, FgPriceIncludeTax, FactorRate, A.Department, A.DepartmentName, DecPlacePrice, DecPlaceBaseForex From V_PRPOHd A " 'INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = " + QuotedStr(ViewState("UserId").ToString) + " AND POType <> 'Service' "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                'lbCount.Text = SQLExecuteScalar("SELECT Count(PR_No) from V_PRPOGetRequest2 ", ViewState("DBConnection").ToString)
                lbCount.Text = SQLExecuteScalar("EXEC S_PRPOReff '','',0, '20500101',0 ", ViewState("DBConnection").ToString)

                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblJudul.Text = dt.Rows(0)("MenuName").ToString

            End If
            If Not Session("PostNmbr") Is Nothing Then
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) = '" + Session("PostNmbr") + "'")
                lbCount.Text = SQLExecuteScalar("SELECT Count(PR_No) from V_PRPOGetRequest2 ", ViewState("DBConnection").ToString)
                Session("PostNmbr") = Nothing

            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                    BindToDropList(ddlCurrHd, Session("Result")(3).ToString)
                    BindToDropList(ddlfgAdditional, Session("Result")(3).ToString)
                    BindToDropList(ddlTerm, Session("Result")(4).ToString)
                    BindToDropList(ddlReport, Session("Result")(5).ToString)
                    ddlTerm_SelectedIndexChanged(Nothing, Nothing)
                    UpdatePrice()
                    'BindToDropList(ddlfgInclude, Session("Result")(5).ToString)

                    If ddlReport.SelectedValue = "Y" Then
                        tbPPN.Text = ViewState("PPN")
                        ' tbPPN.Enabled = True
                    Else
                        tbPPN.Text = "0"
                        ' tbPPN.Enabled = False
                    End If
                    'lps ddlfgInclude_SelectedIndexChanged(Nothing, Nothing)

                    'tbPPNForex.Text = FormatNumber((tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
                    'sebelumny pph di tambah
                    'tbTotalForex.Text = FormatNumber((tbBaseForex.Text - tbDiscForex.Text) + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text, CInt(ViewState("DigitCurr")))

                    tbSuppCode_TextChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"))
                    tbRateHd.Enabled = False
                ElseIf ViewState("Sender") = "btnRequestPR" Then
                    tbRequestPR.Text = Session("Result")(0).ToString
                    tbProductPR.Text = Session("Result")(1).ToString
                    tbProductNamePR.Text = Session("Result")(2).ToString
                    BindToText(tbSpecificationPR, Session("Result")(3).ToString)
                    BindToText(tbQtyPR, Session("Result")(4).ToString)
                    BindToText(tbQtyPROri, Session("Result")(8).ToString)
                    BindToDropList(ddlUnitPR, Session("Result")(5).ToString)
                    BindToDate(tbPRDelivery, Session("Result")(6).ToString)
                    BindToDate(tbDeliveryDateDt2, Session("Result")(6).ToString)
                ElseIf ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("DtPR").Select("Product = " + QuotedStr(drResult("Product")) + " AND RequestNo = " + QuotedStr(drResult("PR_No")) + " AND PRDelivery = " + QuotedStr(drResult("DeliveryDate")) + " AND Delivery = " + QuotedStr(drResult("DeliveryDate")))
                        
                        If Row.Count = 0 Then
                            dr = ViewState("DtPR").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("RequestNo") = drResult("PR_No")
                            dr("PRDelivery") = drResult("DeliveryDate")
                            dr("Delivery") = drResult("DeliveryDate")
                            dr("Qty") = FormatFloat(drResult("QtySchedule"), ViewState("DigitQty"))
                            dr("QtyPR") = FormatFloat(drResult("QtyPROri"), ViewState("DigitQty"))
                            dr("Unit") = drResult("Unit")
                            ViewState("DtPR").Rows.Add(dr)
                        Else
                            dr = ViewState("DtPR").Select("Product = " + QuotedStr(drResult("Product")) + " AND RequestNo = " + QuotedStr(drResult("PR_No")))(0)
                            dr.BeginEdit()
                            If drResult("QtySchedule") <> dr("Qty") Then
                                dr("Qty") = dr("Qty") + CFloat(drResult("QtySchedule"))
                            End If
                        End If
                    Next
                    'GenerateDt2()
                    'GenerateDtDelivery()
                    BindGridDt(ViewState("DtPR"), GridPR)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
                    ModifyDt()

                ElseIf ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim row() As DataRow
                    Dim FirstTime As Boolean = True
                    Dim Qty As Double

                    For Each drResult In Session("Result").Rows
                        'insert

                        If FirstTime Then
                            BindToDropList(ddlType, "Lokal")
                            'BindToDropList(ddlType, "Import")
                            tbSuppCode_TextChanged(Nothing, Nothing)
                            BindToDropList(ddlDept, drResult("Department").ToString)
                            ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"))
                            tbRateHd.Enabled = False
                        End If
                        Qty = 0

                        row = ViewState("DtPR").Select("Product = " + QuotedStr(drResult("Product").ToString) + " AND RequestNo = " + QuotedStr(drResult("PR_No").ToString) + " and Delivery = " + QuotedStr(drResult("DeliveryDate").ToString) + " and PRDelivery = " + QuotedStr(drResult("DeliveryDate").ToString))
                        'If CekExistData(ViewState("DtPR"), "RequestNo,Product,PRDelivery,Delivery", drResult("PR_No") + "|" + drResult("Product")) = False Then
                        If row.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("DtPR").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("PRDelivery") = drResult("DeliveryDate")
                            dr("Delivery") = drResult("DeliveryDate")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("RequestNo") = drResult("PR_No")
                            dr("Qty") = drResult("QtySchedule")
                            dr("QtyPR") = drResult("QtyPROri")
                            dr("Unit") = drResult("Unit")
                            ViewState("DtPR").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next

                    'GenerateDt2()
                    'GenerateDtDelivery()
                    BindGridDt(ViewState("DtPR"), GridPR)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
                    ModifyDt()
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            If Not ViewState("deletetrans") Is Nothing Then
                Dim Result, ListSelectNmbr, msg, ActionValue, status As String
                Dim Nmbr(100) As String
                Dim j As Integer
                If HiddenRemarkDelete.Value = "true" Then
                    If sender.ID.ToString = "BtnGo" Then
                        ActionValue = ddlCommand.SelectedValue
                    Else
                        ActionValue = ddlCommand2.SelectedValue
                    End If

                    Status = CekStatus(ActionValue)
                    ListSelectNmbr = ""
                    msg = ""

                    '3 = status, 2 & 3 = key, 
                    GetListCommand("G|H", GridView1, "4,2,3", ListSelectNmbr, Nmbr, msg)

                    If ListSelectNmbr = "" Then Exit Sub

                    For j = 0 To (Nmbr.Length - 1)
                        If Nmbr(j) = "" Then
                            Exit For
                        Else

                            Result = ExecSPCommandGo("Delete", "S_PRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + "<br />"
                            End If
                        End If
                    Next
                    BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
                    If msg.Trim <> "" Then
                        lbStatus.Text = MessageDlg(msg)
                    End If

                End If
                ViewState("deletetrans") = Nothing
                HiddenRemarkDelete.Value = ""
                'GridDt.Columns(0).Visible = False
            End If

            If Not ViewState("ProductClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_PRPOClosing " + QuotedStr(tbCode.Text) + "," + lbRevisi.Text + "," + QuotedStr(ViewState("ProductClose").ToString) + "," + QuotedStr(HiddenRemarkClose.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    If result.Length > 2 Then
                        lbStatus.Text = MessageDlg(result)
                    Else
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    End If
                End If
                ViewState("ProductClose") = Nothing
                HiddenRemarkClose.Value = ""
                'GridDt.Columns(0).Visible = False
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
        ViewState("PPN") = SQLExecuteScalar("Select Max(PPN) FROM MsPPN ", ViewState("DBConnection"))
    End Sub

    Private Sub SetInit()

      
        Try
            If Request.QueryString("ContainerId").ToString = "POGoodsID" Then
                lblJudul.Text = "PO Goods"
                ViewState("FgImp") = "N"
            End If
            If Request.QueryString("ContainerId").ToString = "POGoodsImpID" Then
                lblJudul.Text = "PO Goods Import"
                ViewState("FgImp") = "Y"
            End If
            FillRange(ddlRange)
            'FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))
            FillCombo(ddlDept, "SELECT Dept_Code, Dept_Name FROM VMsDepartment ", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlCurrHd, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrderDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitPR, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitWrhsDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlDelivery, "EXEC S_GetDelivery", True, "deliverycode", "deliveryname", ViewState("DBConnection"))
            FillCombo(ddlShipmentType, "EXEC S_GetShipment ", True, "ShipmentCode", "ShipmentName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = -1
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
                'ddlCommand.Items.Add("Print 2")
                'ddlCommand2.Items.Add("Print 2")
                'ddlCommand.Items.Add("Print Delivery")
                'ddlCommand2.Items.Add("Print Delivery")
            End If
            tbSpecification.Attributes.Add("ReadOnly", "true")
            tbSpecificationPR.Attributes.Add("ReadOnly", "true")
            'tbRemarkPrice.Attributes.Add("ReadOnly", "true")
            tbPPNForex.Attributes.Add("ReadOnly", "True")
            tbPPHForex.Attributes.Add("ReadOnly", "True")
            tbBaseForex.Attributes.Add("ReadOnly", "True")
            tbDiscForex.Attributes.Add("ReadOnly", "True")
            'tbQtyPR.Attributes.Add("ReadOnly", "True")
            tbQtyWrhsFreeDt.Attributes.Add("ReadOnly", "True")
            tbQtyWrhsPODt.Attributes.Add("ReadOnly", "True")
            tbPPHForexDt.Attributes.Add("ReadOnly", "True")
            tbTotalForex.Attributes.Add("ReadOnly", "True")
            tbTotalForexDt.Attributes.Add("ReadOnly", "True")
            tbAmountForexDt.Attributes.Add("ReadOnly", "True")

            tbDP.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDPForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOtherForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyPR.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrderDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhsFreeDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhsPODt.Attributes.Add("OnKeyDown", "return PressNumeric();")





            tbDP.Attributes.Add("OnBlur", "setformathd('DP');")
            tbDPForex.Attributes.Add("OnBlur", "setformathd('DPForex');")
            ' tbBaseForex.Attributes.Add("OnBlur", "setformathd('');")
            tbDiscForex.Attributes.Add("OnBlur", "setformathd('');")
            tbPPN.Attributes.Add("OnBlur", "setformathd('');")
            tbPPHForex.Attributes.Add("OnBlur", "setformathd('');")
            tbPPNForex.Attributes.Add("OnBlur", "setformathd('');")
            tbOtherForex.Attributes.Add("OnBlur", "setformathd('');")
            tbTotalForex.Attributes.Add("OnBlur", "setformathd('');")

            tbDiscDt.Attributes.Add("OnBlur", "setformatdt('DP');")
            tbDiscForexDt.Attributes.Add("OnBlur", "setformatdt('DPForex');")
            'tbQtyOrderDt.Attributes.Add("OnBlur", "setformatdtpr();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    'Private Sub GetInfo()
    '    Dim SqlString As String
    '    Dim DS As DataSet
    '    Try
    '        SqlString = "EXEC S_PRPOInfo " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(tbProductDt.Text) + "," + QuotedStr(ddlUnitWrhsDt.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
    '        DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))
    '        tbRemarkPrice.Text = DS.Tables(0).Rows(0)(0).ToString
    '        PnlInfo.Visible = DS.Tables(0).Rows(0)(0).ToString <> ""
    '    Catch ex As Exception
    '        Throw New Exception("get info error : " + ex.ToString)
    '    End Try
    'End Sub

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
            'diganti dengan jika advanced, maka tidak perlu hrs sesuai filter dlg
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If


            DT = BindDataTransaction(GetStringHd(ViewState("FgImp")), StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data")
                pnlNav.Visible = False
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

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoProductConvert " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PanelInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub


    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PRPODt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Private Function GetStringDtPR(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PRPODtPR WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            ViewState("AdvanceFilter") = ""
            BindData(ViewState("AdvanceFilter"))
            pnlNav.Visible = True

            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub
    Public Function ExecSPCekPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal UserId As String, Optional ByVal Connection As String = "Nothing") As DataTable
        Dim Mycon As New SqlConnection
        Dim DT As DataTable

        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Mycon = New SqlConnection(Connection)

        Dim sqlstring As String
        sqlstring = ""
        If PrimaryKey.Length = 1 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 2 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 3 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(UserId)
        End If
        Try
            DT = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status, msg As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer

        Try

            'GVR = GridView1.Rows(index)

            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If

            If ActionValue = "Print" Or ActionValue = "Print 2" Then
                'Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean
                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows

                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
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
                Session("SelectCommand") = "EXEC S_PRFormPO " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                If ActionValue = "Print" Then
                    Session("ReportFile") = ".../../../Rpt/FormPO.frx"
                Else
                    Session("ReportFile") = ".../../../Rpt/FormPO2.frx"
                End If
                AttachScript("openprintdlg();", Page, Me.GetType)
            
            ElseIf ActionValue = "Delete" Then

                If HiddenRemarkDelete.Value <> "False Value" Then
                    HiddenRemarkDelete.Value = ""
                    Dim CB As CheckBox
                    Dim Pertamax As Boolean
                    Pertamax = True
                    Result = ""

                    For Each GVR In GridView1.Rows

                        CB = GVR.FindControl("cbSelect")
                        If CB.Checked Then
                            ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If

                        End If
                    Next


                    Result = Result + "'"
                    ViewState("deletetrans") = ListSelectNmbr
                    AttachScript("deletetrans();", Page, Me.GetType)

                End If

                ' HiddenRemarkDelete.Value = ""

            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                msg = ""

                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, msg)

                If ListSelectNmbr = "" Then Exit Sub

                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                      
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + "<br />"
                        End If
                    End If
                Next
                'lbStatus.Text = msg
                'Exit Sub
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
                If msg.Trim <> "" Then
                    lbStatus.Text = MessageDlg(msg)
                End If
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
            ddlType.Enabled = State
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
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtPR(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("DtPR") = Nothing
            dt = SQLExecuteQuery(GetStringDtPR(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtPR") = dt
            BindGridDt(dt, GridPR)
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
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelPR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelPR.Click
        Try
            MovePanel(pnlEditDtPR, pnlDtPR)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel PR Error : " + ex.ToString
        End Try
    End Sub

    Private Function DtExist() As Boolean
        Dim dete, piar As Boolean
        Try
            If ViewState("Dt") Is Nothing Then
                dete = False
            Else
                dete = GetCountRecord(ViewState("Dt")) > 0
            End If

            If ViewState("DtPR") Is Nothing Then
                piar = False
            Else
                piar = ViewState("DtPR").Rows.Count > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtPR").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function

    Private Sub ClearHd()
        Try
            tbSuppCode.Text = ""
            tbCode.Text = ""
            tbSuppName.Text = ""
            lbRevisi.Text = "0"
            ddlReport.Text = "Y"
            ddlDept.SelectedValue = ""
            tbBaseForex.Text = "0"
            tbDiscForex.Text = "0"
            tbTotalForex.Text = "0"
            tbPPHForex.Text = "0"
            tbPPN.Text = ViewState("PPN")
            tbSuppPONo.Text = ""
            tbPPNForex.Text = "0"
            ddlCurrHd.SelectedValue = ViewState("Currency")
            ddlTerm.SelectedIndex = 0
            ddlShipmentType.SelectedIndex = 0
            tbAttn.Text = ""
            tbRemark.Text = ""
            ddlDelivery.SelectedValue = ""
            tbDate.SelectedDate = Now.Date
            tbDP.Text = "0"
            tbDPForex.Text = "0"
            tbOtherForex.Text = "0"
            tbDeliveryAddr.Text = ""
            tbDeliveryCity.Text = ""
            tbTOPRemark.Text = ""
            ddlDelivery.SelectedValue = ""
            ddlfgInclude.SelectedValue = "N"
            ViewState("FactorRate") = 1
            ddlfgAdditional.SelectedValue = "N"
            tbPPN0.Text = "0"
            tbDPForex0.Text = "0"
            tbCustContractNo.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbRemarkDt.Text = ""
            tbProductDt.Text = ""
            tbProductNameDt.Text = ""
            tbQtyPR.Text = "0"
            tbQtyOrderDt.Text = "0"
            tbQtyWrhsFreeDt.Text = "0"
            tbQtyWrhsPODt.Text = "0"
            tbQtyPack.Text = "0"
            tbPriceForexDt.Text = "0"
            tbAmountForexDt.Text = "0"
            tbDiscDt.Text = "0"
            tbDiscForexDt.Text = "0"
            tbPPHDt.Text = "0"
            tbPPHForexDt.Text = "0"
            tbTotalForexDt.Text = "0"
            'ddlPartial.SelectedValue = "Y"
            tbDeliveryDateDt.SelectedDate = ViewState("ServerDate") 'Now.Date
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub CleardtPR()
        Try
            tbRequestPR.Text = ""
            tbProductPR.Text = ""
            tbPRDelivery.SelectedDate = Date.Today
            tbDeliveryDateDt2.SelectedDate = Date.Today
            tbProductNamePR.Text = ""
            tbSpecificationPR.Text = ""
            tbQtyPR.Text = "0"
            tbQtyPROri.Text = "0"
            ddlUnitPR.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
                Return False
            End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If ddlTerm.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Term must have value")
                ddlTerm.Focus()
                Return False
            End If
            If ddlReport.Text = "Choose One" Then
                lbStatus.Text = MessageDlg("Report must have value")
                ddlReport.Focus()
                Return False
            End If
           
            If ddlShipmentType.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Shipment Type must have value")
                ddlShipmentType.Focus()
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
                If CFloat(Dr("QtyOrder").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("PriceForex").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
            Else
                If CFloat(tbQtyDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If CFloat(tbPriceForexDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDtPR(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("RequestNo").ToString = "" Then
                    lbStatus.Text = MessageDlg("Request No Must Have Value")
                    Return False
                End If
            Else
                If tbRequestPR.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Request No Must Have VAlue")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("Product = " + QuotedStr(tbProductDt.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                If ExistRow.Count > AllowedRecordDt() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("ProductDt")))(0)

                Row.BeginEdit()
                Row("Product") = tbProductDt.Text
                Row("ProductName") = tbProductNameDt.Text
                Row("Specification") = tbSpecification.Text
                Row("QtyWrhsPO") = tbQtyWrhsPODt.Text
                Row("QtyWrhsFree") = tbQtyWrhsFreeDt.Text
                Row("Qty") = tbQtyDt.Text
                Row("Unit") = ddlUnitWrhsDt.SelectedValue

                Row("QtyOrder") = tbQtyOrderDt.Text
                Row("UnitOrder") = ddlUnitOrderDt.SelectedValue
                Row("QtyPack") = tbQtyPack.Text
                Row("PriceForex") = tbPriceForexDt.Text
                Row("BrutoForex") = FormatNumber(tbAmountForexDt.Text, CInt(ViewState("DigitCurr")))
                Row("Disc") = tbDiscDt.Text
                Row("DiscForex") = tbDiscForexDt.Text
                Row("PPH") = tbPPHDt.Text
                Row("PPHForex") = tbPPHForexDt.Text
                Row("NettoForex") = FormatNumber(tbTotalForexDt.Text, CInt(ViewState("DigitCurr")))
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
                dr("Product") = tbProductDt.Text
                dr("ProductName") = tbProductNameDt.Text
                dr("Specification") = tbSpecification.Text
                dr("QtyWrhsPO") = tbQtyWrhsPODt.Text
                dr("QtyWrhsFree") = tbQtyWrhsFreeDt.Text
                dr("Qty") = tbQtyPR.Text
                dr("Unit") = ddlUnitWrhsDt.SelectedValue
                dr("QtyOrder") = tbQtyOrderDt.Text
                dr("UnitOrder") = ddlUnitOrderDt.SelectedValue
                dr("QtyPack") = tbQtyPack.Text
                dr("PriceForex") = tbPriceForexDt.Text
                dr("BrutoForex") = FormatNumber(tbAmountForexDt.Text, CInt(ViewState("DigitCurr")))
                dr("Disc") = tbDiscDt.Text
                dr("DiscForex") = tbDiscForexDt.Text
                dr("PPH") = tbPPHDt.Text
                dr("PPHForex") = tbPPHForexDt.Text
                dr("NettoForex") = FormatNumber(tbTotalForexDt.Text, CInt(ViewState("DigitCurr")))
                dr("Remark") = tbRemarkDt.Text
                
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(Not DtExist())
            totalingDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnSavePR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSavePR.Click
        Try
            If CekDtPR() = False Then
                btnSavePR.Focus()
                Exit Sub
            End If

            Dim ExistRow As DataRow()
            ExistRow = ViewState("DtPR").Select("Product = " + QuotedStr(tbProductPR.Text) + " AND RequestNo = " + QuotedStr(tbRequestPR.Text) + " and Delivery = " + QuotedStr(Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")) + " and PRDelivery = " + QuotedStr(Format(tbPRDelivery.SelectedDate, "dd MMM yyyy")))
            If ViewState("StateDtPR") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordPR() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("DtPR").Select("Product = " + QuotedStr(ViewState("ProductPR")) + " AND RequestNo = " + QuotedStr(ViewState("RequestPR")) + " and Delivery = " + QuotedStr(ViewState("Delivery")) + " and PRDelivery = " + QuotedStr(ViewState("PRDelivery")))(0)

                Row.BeginEdit()
                Row("Product") = tbProductPR.Text
                Row("ProductName") = tbProductNamePR.Text
                Row("PRDelivery") = Format(tbPRDelivery.SelectedDate, "dd MMM yyyy")
                Row("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")
                Row("Specification") = tbSpecificationPR.Text
                Row("RequestNo") = tbRequestPR.Text
                Row("Qty") = tbQtyPR.Text
                Row("QtyPR") = tbQtyPROri.Text
                Row("Unit") = ddlUnitPR.SelectedValue()
                Row.EndEdit()
            Else
                Dim dr As DataRow

                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("DtPR").NewRow
                dr("Product") = tbProductPR.Text
                dr("ProductName") = tbProductNamePR.Text
                dr("PRDelivery") = Format(tbPRDelivery.SelectedDate, "dd MMM yyyy")
                dr("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")
                dr("Specification") = tbSpecificationPR.Text
                dr("RequestNo") = tbRequestPR.Text
                dr("Qty") = tbQtyPR.Text
                dr("QtyPR") = tbQtyPROri.Text
                dr("Unit") = ddlUnitPR.SelectedValue()
                ViewState("DtPR").Rows.Add(dr)

            End If
            MovePanel(pnlEditDtPR, pnlDtPR)
            EnableHd(Not DtExist())
            BindGridDt(ViewState("DtPR"), GridPR)
            ModifyDt()
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save PR Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub


    Private Sub totalingDt()
        Dim dr As DataRow
        Dim total, totalppn, pph, disc As Double
        Try
            total = 0
            totalppn = 0
            pph = 0
            disc = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    total = total + CFloat(dr("NettoForex").ToString)
                    pph = pph + CFloat(dr("PPHForex").ToString)
                    disc = disc + CFloat(dr("DiscForex").ToString)
                End If
            Next

            tbPPHForex.Text = FormatNumber(pph, CInt(ViewState("DigitCurr")))
            tbDiscForex.Text = FormatNumber(disc * ViewState("FactorRate"), CInt(ViewState("DigitCurr")))

            'menyebabkan data netto dan base tidak sama 38.50 base 38.00
            'If ddlCurrHd.SelectedValue = ViewState("Currency") Then
            '    total = Math.Floor(total)
            'End If
            If ddlfgInclude.SelectedValue = "N" Then
                tbBaseForex.Text = total + tbDiscForex.Text
            Else

                '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
                tbBaseForex.Text = (total * 100 / (100 + tbPPN.Text)) + tbDiscForex.Text
            End If
            tbPPNForex.Text = (tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text)
            'sebelumny pph di tambah
            tbTotalForex.Text = (tbBaseForex.Text - tbDiscForex.Text) + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text
            tbDPForex.Text = (tbDP.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text)
            AttachScript("setformathd('');", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub UpdatePrice()
        Dim DR As DataRow
        Dim i As Integer
        Dim sqlstring As String
        Dim PriceForex As Double
        Try
            For i = 0 To GetCountRecord(ViewState("Dt")) - 1
                DR = ViewState("Dt").Rows(i)
                If Not ViewState("Dt").Rows(i).RowState = DataRowState.Deleted Then
                    If CFloat(DR("PriceForex")) = 0 Then
                        sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("UnitOrder").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                        PriceForex = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                        DR.BeginEdit()
                        DR("PriceForex") = FormatNumber(PriceForex, ViewState("DigitCurr"))
                        DR("BrutoForex") = CFloat(DR("QtyOrder").ToString) * CFloat(PriceForex)
                        DR("DiscForex") = 0
                        DR("DiscForex") = 0
                        DR("PPhForex") = CFloat(DR("QtyOrder").ToString) * CFloat(PriceForex) * CFloat(DR("PPh"))
                        DR("NettoForex") = (CFloat(DR("QtyOrder").ToString) * CFloat(PriceForex))
                        DR.EndEdit()
                    End If
                End If
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            totalingDt()
        Catch ex As Exception
            Throw New Exception("Update Price Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub ModifyDt()
        Dim DtPR, Dt As DataTable
        Dim DR, CurrRow As DataRow
        Dim SelectRow As DataRow()
        Dim i, len As Integer
        Dim QtyWrhs, QtyOrder As Double
        Dim UnitOrder, UnitWrhs, sqlstring As String
        Try
            DtPR = ViewState("DtPR")
            Dt = ViewState("Dt")

            'reset Qty Wrhs = 0 utk dt
            len = Dt.Rows.Count
            For i = 0 To len - 1
                If Not ViewState("Dt").Rows(i).RowState = DataRowState.Deleted Then
                    ViewState("Dt").Rows(i).BeginEdit()
                    ViewState("Dt").Rows(i)("Qty") = 0
                    ViewState("Dt").Rows(i).EndEdit()
                End If
            Next

            For Each DR In DtPR.Rows
                If Not DR.RowState = DataRowState.Deleted Then
                    SelectRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))
                    If SelectRow.Length = 0 Then
                        'insert row baru
                        Dim dtUnit As DataTable
                        Dim drUnit As DataRow
                        dtUnit = SQLExecuteQuery("EXEC S_PRPOConvertUnitOrder " + QuotedStr(DR("Product").ToString) + ", " + QuotedStr(DR("Unit").ToString) + ", " + QuotedStr(DR("Qty").ToString.Replace(",", "")), ViewState("DBConnection")).Tables(0)
                        drUnit = dtUnit.Rows(0)

                        CurrRow = ViewState("Dt").NewRow
                        CurrRow("Product") = DR("Product")
                        CurrRow("ProductName") = DR("ProductName")
                        CurrRow("Specification") = TrimStr(DR("Specification").ToString)
                        CurrRow("QtyWrhsPO") = FormatFloat(CFloat(drUnit("QtyPR").ToString), ViewState("DigitQty"))
                        CurrRow("QtyWrhsFree") = FormatFloat(CFloat(drUnit("QtyNonPR").ToString), ViewState("DigitQty"))
                        CurrRow("Qty") = FormatFloat(CFloat(drUnit("QtyTotal").ToString), ViewState("DigitQty"))
                        CurrRow("QtyOrder") = FormatFloat(CFloat(drUnit("QtyPR").ToString), ViewState("DigitQty"))
                        CurrRow("Unit") = DR("Unit")
                        CurrRow("UnitOrder") = drUnit("UnitOrder")
                        If CurrRow("UnitOrder") = DR("Unit") Then
                            CurrRow("QtyPack") = 1
                        Else
                            CurrRow("QtyPack") = FormatFloat(drUnit("QtyPR").ToString / drUnit("QtyOrder").ToString, ViewState("DigitQty"))
                        End If

                        sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("Unit").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))

                        CurrRow("PriceForex") = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))

                        CurrRow("BrutoForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString)
                        CurrRow("Disc") = 0
                        CurrRow("DiscForex") = 0
                        CurrRow("PPH") = 0
                        CurrRow("PPHForex") = 0
                        CurrRow("NettoForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString)
                        ViewState("Dt").Rows.Add(CurrRow)
                    Else
                        'Edit row Dt
                        CurrRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))(0)
                        QtyWrhs = CFloat(CurrRow("Qty").ToString) + CFloat(DR("Qty").ToString)
                        UnitOrder = CurrRow("UnitOrder").ToString
                        UnitWrhs = CurrRow("Unit").ToString
                        If (UnitWrhs = UnitOrder) Then
                            QtyOrder = QtyWrhs
                        Else
                            QtyOrder = SQLExecuteScalar("EXEC S_PRPOConvertUnitOrder " + QuotedStr(CurrRow("Product").ToString) + ", " + QuotedStr(UnitOrder) + ", " + QtyWrhs.ToString.Replace(",", ""), ViewState("DBConnection"))
                        End If
                        CurrRow.BeginEdit()
                        CurrRow("Qty") = FormatFloat(QtyWrhs, ViewState("DigitQty"))
                        CurrRow("QtyOrder") = FormatFloat(QtyOrder, ViewState("DigitQty"))
                        CurrRow("BrutoForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString)
                        CurrRow("DiscForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString) * CFloat(CurrRow("Disc").ToString / 100.0)
                        CurrRow("NettoForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString) - CFloat(CurrRow("DiscForex").ToString)
                        CurrRow("PPhForex") = CFloat(CurrRow("QtyOrder").ToString) * CFloat(CurrRow("PriceForex").ToString) * CFloat(CurrRow("PPh").ToString * ViewState("FactorRate") / 100.0)
                        CurrRow.EndEdit()
                    End If
                End If
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            GridDt.Columns(0).Visible = True
            totalingDt()
        Catch ex As Exception
            Throw New Exception("Modify Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Function AllowedRecordDt() As Integer
        Try
            If ViewState("ProductDt") = tbProductDt.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function
    Private Function AllowedRecordPR() As Integer
        Try
            If ViewState("ProductPR") = tbProductPR.Text And ViewState("RequestPR") = tbRequestPR.Text And ViewState("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy") And ViewState("PRDelivery") = Format(tbPRDelivery.SelectedDate, "dd MMM yyyy") Then
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
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = MessageDlg("Detail Data must be saved first")
            '    Exit Sub
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("PO", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlDept.SelectedValue, ViewState("DBConnection").ToString)
                'ubah disini
                SQLString = "INSERT INTO PRCPOHd (TransNmbr, Revisi, TransDate, STATUS, Department, FgReport, " + _
                "Supplier, Attn, Term, SuppContractNo, CustContractNo, ShipmentType, Delivery, " + _
                "DeliveryAddr, DeliveryCity, Currency, ForexRate, BaseForex, Disc, DiscForex, DP, DPForex, PPn, " + _
                "PPNForex, PPHForex, OtherForex, TotalForex, Remark, UserPrep, " + _
                "DatePrep, FgActive, POType, TermPayment, FgAddCost, AddCostRemark, FgPriceIncludeTax, FactorRate, " + _
                "DecPlacePrice, DecPlaceBaseForex)" + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 0, " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlDept.SelectedValue) + ", " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + _
                ", " + QuotedStr(ddlTerm.SelectedValue) + ", " + QuotedStr(tbSuppPONo.Text) + ", " + QuotedStr(tbCustContractNo.Text) + _
                ", " + QuotedStr(ddlShipmentType.SelectedValue) + ", " + QuotedStr(ddlDelivery.SelectedValue) + _
                ", " + QuotedStr(tbDeliveryAddr.Text) + ", " + QuotedStr(tbDeliveryCity.Text) + ", " + _
                QuotedStr(ddlCurrHd.SelectedValue) + ", " + tbRateHd.Text.Replace(",", "") + ", " + tbBaseForex.Text.Replace(",", "") + ", " + _
                "0 , " + tbDiscForex.Text.Replace(",", "") + ", " + tbDP.Text.Replace(",", "") + _
                ", " + tbDPForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + _
                ", " + tbPPHForex.Text.Replace(",", "") + ", " + tbOtherForex.Text.Replace(",", "") + _
                ", " + tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y', " + QuotedStr(ddlType.SelectedValue) + _
                ", " + QuotedStr(tbTOPRemark.Text) + ", " + QuotedStr(ddlfgAdditional.SelectedValue) + ", " + _
                QuotedStr(tbAddCostRemark.Text) + ", " + QuotedStr(ddlfgInclude.SelectedValue) + ", " + QuotedStr(ViewState("FactorRate")) + _
                ", " + tbDPForex0.Text + ", " + tbPPN0.Text

                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCPOHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PRCPOHd SET FgReport =" + QuotedStr(ddlReport.SelectedValue) + ", Department = " + QuotedStr(ddlDept.SelectedValue) + ", Supplier=" + QuotedStr(tbSuppCode.Text) + _
                ", TransDate =" + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", Term=" + QuotedStr(ddlTerm.Text) + ", TermPayment= " + QuotedStr(tbTOPRemark.Text) + _
                ", ShipmentType= " + QuotedStr(ddlShipmentType.SelectedValue) + _
                ", Delivery=" + QuotedStr(ddlDelivery.SelectedValue) + ", DeliveryAddr=" + QuotedStr(tbDeliveryAddr.Text) + _
                ", DeliveryCity=" + QuotedStr(tbDeliveryCity.Text) + ", FgAddCost=" + QuotedStr(ddlfgAdditional.SelectedValue) + _
                ", Currency=" + QuotedStr(ddlCurrHd.SelectedValue) + ", AddCostRemark=" + QuotedStr(tbAddCostRemark.Text) + _
                ", BaseForex=" + tbBaseForex.Text.Replace(",", "") + _
                ", ForexRate=" + tbRateHd.Text.Replace(",", "") + _
                ", Disc= 0, DiscForex=" + tbDiscForex.Text.Replace(",", "") + _
                ", DP=" + tbDP.Text.Replace(",", "") + ", DPForex=" + tbDPForex.Text.Replace(",", "") + ", PPn=" + _
                tbPPN.Text.Replace(",", "") + ", PPNForex=" + tbPPNForex.Text.Replace(",", "") + ", PPHForex=" + _
                tbPPHForex.Text.Replace(",", "") + ", OtherForex=" + tbOtherForex.Text.Replace(",", "") + ", TotalForex=" + _
                tbTotalForex.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + ", SuppContractNo=" + _
                QuotedStr(tbSuppPONo.Text) + ", CustContractNo=" + QuotedStr(tbCustContractNo.Text) + ", FactorRate=" + QuotedStr(ViewState("FactorRate")) + _
                ",  DecPlacePrice = " + tbDPForex0.Text + ", DecPlaceBaseForex = " + tbPPN0.Text + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text

                'lbStatus.Text = SQLString
                'Exit Sub
            End If
            SQLString = ChangeQuoteNull(SQLString)
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

            Row = ViewState("DtPR").Select("TransNmbr IS NULL")
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
            SQLString = " SELECT TransNmbr, Revisi, Product, Specification, QtyWrhsPO, QtyWrhsFree, Qty, Unit, PriceForex, QtyOrder, UnitOrder, QtyPack, BrutoForex, Disc, DiscForex, PPh, PPhForex, NettoForex, Remark FROM PRCPODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi")

            Dim cmdSql As New SqlCommand(SQLString, con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCPODt SET Product = @Product, Specification = @Specification, QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Qty = @Qty, Unit = @Unit, QtyWrhsPO = @QtyWrhsPO, " + _
                    "QtyWrhsFree = @QtyWrhsFree, PriceForex = @PriceForex, QtyPack = @QtyPAck, BrutoForex = @BrutoForex, " + _
                    "Disc = @Disc, DiscForex = @DiscForex, NettoForex = @NettoForex, PPh = @pph, PPhForex = @PPhForex, " + _
                    " Remark = @Remark " + _
                    "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = @OldRevisi AND Product = @OldProduct ", con)

            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Specification", SqlDbType.VarChar, 255, "Specification")
            Update_Command.Parameters.Add("@QtyWrhsPO", SqlDbType.Float, 18, "QtyWrhsPO")
            Update_Command.Parameters.Add("@QtyWrhsFree", SqlDbType.Float, 18, "QtyWrhsFree")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 18, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@QtyPack", SqlDbType.Float, 18, "QtyPack")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 18, "PriceForex")
            Update_Command.Parameters.Add("@BrutoForex", SqlDbType.Float, 18, "BrutoForex")
            Update_Command.Parameters.Add("@Disc", SqlDbType.Float, 18, "Disc")
            Update_Command.Parameters.Add("@DiscForex", SqlDbType.Float, 18, "DiscForex")
            Update_Command.Parameters.Add("@NettoForex", SqlDbType.Float, 18, "NettoForex")
            Update_Command.Parameters.Add("@pph", SqlDbType.Float, 18, "pph")
            Update_Command.Parameters.Add("@pphForex", SqlDbType.Float, 18, "pphForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldRevisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCPODt WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = @Revisi AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Revisi", SqlDbType.Int, 4, "Revisi")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCPODt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dtPR
            cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, PRDelivery, Delivery, Product, RequestNo, Qty, Unit FROM PRCPODtPR WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param1 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command1 = New SqlCommand( _
                    "UPDATE PRCPODtPR SET Product = @Product, PRDelivery = @PRDelivery, Delivery = @Delivery, RequestNo = @RequestNo, Qty = @Qty, Unit = @Unit " + _
                     " WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & "  AND Revisi = @OldRevisi AND Product = @OldProduct And PRDelivery = @OldPRDelivery AND Delivery = @OldDelivery AND RequestNo = @OldRequestNo", con)

            Update_Command1.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command1.Parameters.Add("@PRDelivery", SqlDbType.DateTime, 20, "PRDelivery")
            Update_Command1.Parameters.Add("@Delivery", SqlDbType.DateTime, 20, "Delivery")
            Update_Command1.Parameters.Add("@Qty", SqlDbType.Float, 20, "Qty")
            Update_Command1.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command1.Parameters.Add("@RequestNo", SqlDbType.VarChar, 20, "RequestNo")

            ' Define intput (WHERE) parameters.
            param1 = Update_Command1.Parameters.Add("@OldRevisi", SqlDbType.Int, 4, "Revisi")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldPRDelivery", SqlDbType.DateTime, 20, "PRDelivery")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldDelivery", SqlDbType.DateTime, 20, "Delivery")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Update_Command1.Parameters.Add("@OldRequestNo", SqlDbType.VarChar, 20, "RequestNo")
            param1.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command1

            ' Create the DeleteCommand.
            Dim Delete_Command1 = New SqlCommand( _
                "DELETE FROM PRCPODtPR WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = @Revisi AND Product = @Product And PRDelivery = @PRDelivery And Delivery = @Delivery AND RequestNo = @RequestNo", con)
            ' Add the parameters for the DeleteCommand.
            param1 = Delete_Command1.Parameters.Add("@Revisi", SqlDbType.Int, 4, "Revisi")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@PRDelivery", SqlDbType.DateTime, 20, "PRDelivery")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@Delivery", SqlDbType.DateTime, 20, "Delivery")
            param1.SourceVersion = DataRowVersion.Original
            param1 = Delete_Command1.Parameters.Add("@RequestNo", SqlDbType.VarChar, 20, "RequestNo")
            param1.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command1

            Dim DtPR As New DataTable("PRCPODtPR")

            DtPR = ViewState("DtPR")
            da.Update(DtPR)
            DtPR.AcceptChanges()
            ViewState("DtPR") = DtPR

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
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            If ViewState("DtPR").Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Detail PR must have at least 1 record")
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
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = -1
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 1
            Menu1.Items.Item(1).Selected = True
            GridDt.Columns(0).Visible = False
            GridDt.Columns(1).Visible = False
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            CleardtPR()
            ddlReport.Enabled = True
            btnAddDtPR.Visible = True
            btnAddDtPRke2.Visible = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            PnlDt.Visible = True
            BindDataDtPR("", 0)
            BindDataDt("", 0)
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "PO No, Supplier, Supplier Name, Delivery, Shipment Type, Remark"
            FilterValue = "TransNmbr, Supplier, SupplierName, Delivery, ShipmentType, Remark"
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
        BindData(ViewState("AdvanceFilter"))
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow
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
                    ViewState("Revisi") = GVR.Cells(3).Text
                    ViewState("Status") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    Menu1.TabIndex = 1
                    ViewState("StateHd") = "View"
                    BindDataDtPR(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    GridDt.Columns(0).Visible = False
                    ModifyInput2(False, pnlInput, pnlDtPR, GridPR)
                    MultiView1.ActiveViewIndex = 0
                    btnAddDtPR.Visible = False
                    btnAddDtPRke2.Visible = False
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    GridDt.Columns(1).Visible = tbStatus.Text = "P"
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDtPR(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDtPR, GridPR)
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = False
                        btnAddDtPR.Visible = True
                        btnAddDtPRke2.Visible = True

                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(Not DtExist())
                        GridDt.Columns(0).Visible = True
                        GridDt.Columns(1).Visible = False
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
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_PRPOCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = tbCode.Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Or DDL.SelectedValue = "Print 2" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    'If Not GVR.Cells(4).Text = "P" Then
                    '    lbStatus.Text = MessageDlg("Cannot Print Must Post")
                    '    Exit Sub
                    'End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_PRFormPO ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "''"
                    If DDL.SelectedValue = "Print" Then
                        Session("ReportFile") = ".../../../Rpt/FormPO.frx"
                    Else
                        Session("ReportFile") = ".../../../Rpt/FormPO2.frx"
                    End If
                    AttachScript("openprintdlg();", Page, Me.GetType)
                ElseIf DDL.SelectedValue = "Print Delivery" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'If Not GVR.Cells(4).Text = "P" Then
                    '    lbStatus.Text = MessageDlg("Cannot Print Must Post")
                    '    Exit Sub
                    'End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_PRFormPOSchedule ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "''"
                    Session("ReportFile") = ".../../../Rpt/FormPOSchedule.frx"
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
            BindData(ViewState("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status PO is not Post, cannot close product")
                    Exit Sub
                End If
                
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr, drPR As DataRow()
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()

            drPR = ViewState("DtPR").Select("Product = " + QuotedStr(GVR.Cells(4).Text))
            For I As Integer = 0 To (drPR.Count - 1)
                drPR(I).Delete()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridPR_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridPR.RowDeleting
        Try
            Dim dr(), drcek(), drProduct() As DataRow
            Dim GVR As GridViewRow = GridPR.Rows(e.RowIndex)
            ViewState("DeleteProduct") = GVR.Cells(4).Text
            dr = ViewState("DtPR").Select("RequestNo = " + QuotedStr(GVR.Cells(1).Text) + " And Product = " + QuotedStr(GVR.Cells(4).Text) + " AND PRDelivery = " + QuotedStr(GVR.Cells(2).Text) + " AND Delivery = " + QuotedStr(GVR.Cells(3).Text))
            dr(0).Delete()

            'hapus detail product untuk product yg tidak ada
            drcek = ViewState("DtPR").Select("Product = " + QuotedStr(ViewState("DeleteProduct")))
            If drcek.Count = 0 Then
                drProduct = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DeleteProduct")))
                drProduct(0).Delete()
            End If
            ModifyDt()
            'GenerateDtDelete(GVR.Cells(1).Text, GVR.Cells(4).Text)

            BindGridDt(ViewState("DtPR"), GridPR)
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(ViewState("DtPR").Rows.Count > 0)

        Catch ex As Exception
            lbStatus.Text = "Grid Dt PR Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("ProductDt") = GVR.Cells(2).Text
            GetInfo(GVR.Cells(7).Text)
            If (GVR.Cells(8).Text) = (GVR.Cells(11).Text) Then
                tbQtyOrderDt.Enabled = False
                tbQtyPack.Enabled = False
            Else
                tbQtyOrderDt.Enabled = True
                tbQtyPack.Enabled = True
            End If

            ddlUnitOrderDt.Enabled = True
            tbQtyPR.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridPR_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridPR.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridPR.Rows(e.NewEditIndex)
            FillTextBoxDtPR(GVR.Cells(1).Text, GVR.Cells(4).Text, GVR.Cells(2).Text, GVR.Cells(3).Text)
            MovePanel(pnlDtPR, pnlEditDtPR)
            EnableHd(False)
            ViewState("StateDtPR") = "Edit"
            ViewState("RequestPR") = GVR.Cells(1).Text
            ViewState("PRDelivery") = GVR.Cells(2).Text
            ViewState("Delivery") = GVR.Cells(3).Text
            ViewState("ProductPR") = GVR.Cells(4).Text
            btnSavePR.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt PR Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd(ViewState("FgImp")), "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            'newTrans()
            tbCode.Text = Nmbr
            lbRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlType, Dt.Rows(0)("POType").ToString)
            BindToText(tbStatus, Dt.Rows(0)("Status").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("SupplierName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbSuppPONo, Dt.Rows(0)("SuppContractNo").ToString)
            BindToText(tbCustContractNo, Dt.Rows(0)("CustContractNo").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToText(tbTOPRemark, Dt.Rows(0)("TermPayment").ToString)
            BindToDropList(ddlDelivery, Dt.Rows(0)("Delivery").ToString)
            BindToDropList(ddlShipmentType, Dt.Rows(0)("ShipmentType").ToString)
            BindToText(tbDeliveryAddr, Dt.Rows(0)("DeliveryAddr").ToString)
            BindToText(tbDeliveryCity, Dt.Rows(0)("DeliveryCity").ToString)
            BindToDropList(ddlCurrHd, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlfgInclude, Dt.Rows(0)("FgPriceIncludeTax").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPN").ToString, ViewState("DigitPercent"))
            BindToText(tbDP, Dt.Rows(0)("DP").ToString, ViewState("DigitPercent"))
            BindToText(tbDPForex, Dt.Rows(0)("DPForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPNForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPHForex, Dt.Rows(0)("PPHForex").ToString, ViewState("DigitCurr"))
            BindToText(tbOtherForex, Dt.Rows(0)("OtherForex").ToString, ViewState("DigitCurr"))
            BindToDropList(ddlfgAdditional, Dt.Rows(0)("FgAddCost").ToString)
            BindToText(tbAddCostRemark, Dt.Rows(0)("AddCostRemark").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbDPForex0, Dt.Rows(0)("DecPlacePrice").ToString)
            BindToText(tbPPN0, Dt.Rows(0)("DecPlaceBaseForex").ToString)
            BindToText(tbRateHd, Dt.Rows(0)("ForexRate").ToString)
            ViewState("FactorRate") = Dt.Rows(0)("FactorRate")
            ddlReport.Enabled = False
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductDt, Dr(0)("Product").ToString)
                BindToText(tbProductNameDt, Dr(0)("ProductName").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQtyWrhsPODt, Dr(0)("QtyWrhsPO").ToString)
                BindToText(tbQtyWrhsFreeDt, Dr(0)("QtyWrhsFree").ToString)
                BindToText(tbQtyDt, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitWrhsDt, Dr(0)("Unit").ToString)
                BindToText(tbQtyOrderDt, Dr(0)("QtyOrder").ToString)
                BindToDropList(ddlUnitOrderDt, Dr(0)("UnitOrder").ToString)
                BindToText(tbQtyPack, Dr(0)("QtyPack").ToString)
                BindToText(tbPriceForexDt, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForexDt, Dr(0)("BrutoForex").ToString)
                BindToText(tbDiscDt, Dr(0)("Disc").ToString)
                BindToText(tbDiscForexDt, Dr(0)("DiscForex").ToString)
                BindToText(tbPPHDt, Dr(0)("PPH").ToString)
                BindToText(tbPPHForexDt, Dr(0)("PPHForex").ToString)
                BindToText(tbTotalForexDt, Dr(0)("NettoForex").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                GetInfo(tbProductDt.Text)
                'GetInfo()
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDtPR(ByVal RequestNo As String, ByVal Product As String, ByVal PRDelivery As String, ByVal Delivery As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtPR").select("RequestNo = " + QuotedStr(RequestNo) + " AND Product = " + QuotedStr(Product) + " AND PRDelivery = " + QuotedStr(PRDelivery) + " AND Delivery = " + QuotedStr(Delivery))
            If Dr.Length > 0 Then
                BindToText(tbProductPR, Dr(0)("Product").ToString)
                BindToDate(tbPRDelivery, Dr(0)("PRDelivery").ToString)
                BindToDate(tbDeliveryDateDt2, Dr(0)("Delivery").ToString)
                BindToText(tbProductNamePR, Dr(0)("ProductName").ToString)
                BindToText(tbSpecificationPR, Dr(0)("Specification").ToString)
                BindToText(tbQtyPR, Dr(0)("Qty").ToString)
                BindToText(tbQtyPROri, Dr(0)("QtyPR").ToString)
                BindToDropList(ddlUnitPR, Dr(0)("Unit").ToString)
                BindToText(tbRequestPR, Dr(0)("RequestNo").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box Dt PR error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name, Currency, Term, Contact_Person, Phone, PPN FROM VMsSupplier WHERE Fgactive = 'Y' "
            ResultField = "Supplier_Code, Supplier_Name, Contact_Person, Currency, Term, PPN"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Friend Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                tbAttn.Text = Dr("Contact_Person")
                ddlCurrHd.SelectedValue = Dr("Currency")
                BindToDropList(ddlTerm, Dr("Term"))
                UpdatePrice()
                BindToDropList(ddlReport, Dr("Reported"))
                If Dr("Reported").ToString = "Y" Then
                    tbPPN.Text = ViewState("PPN")
                Else
                    tbPPN.Text = "0"
                End If
                tbPPNForex.Text = FormatNumber((tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
                'sebelumny pph di tambah
                tbTotalForex.Text = FormatNumber((tbBaseForex.Text - tbDiscForex.Text) + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text, CInt(ViewState("DigitCurr")))

                'ViewState("Supplier") = "1"
                tbAttn.Focus()
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbAttn.Text = ""
                ddlReport.SelectedIndex = 1
                tbPPNForex.Text = FormatNumber((tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
                'sebelumny pph di tambah
                tbTotalForex.Text = FormatNumber((tbBaseForex.Text - tbDiscForex.Text) + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text, CInt(ViewState("DigitCurr")))

                tbSuppCode.Focus()
            End If
            ddlTerm_SelectedIndexChanged(Nothing, Nothing)

        Catch ex As Exception
            Throw New Exception("tb supplier change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnRequestPR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequestPR.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "EXEC S_PRPOReff " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ",1"
            CriteriaField = "PR_No, Product, Product_Name, Specification, Unit"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            ResultField = "PR_No, Product, Product_Name, Specification, QtySchedule, Unit, DeliveryDate, Qty_Book, QtyPROri"
            ViewState("Sender") = "btnRequestPR"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnAddDtPR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDtPR.Click, btnAddDtPRke2.Click
        Try
            CleardtPR()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDtPR") = "Insert"
            MovePanel(pnlDtPR, pnlEditDtPR)
            EnableHd(False)
            StatusButtonSave(False)
            btnSavePR.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
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

    Protected Sub ddlCurrHd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrHd.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                FillCombo(ddlCurrHd, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
           
            ddlCurrHd.Focus()
        Catch ex As Exception
            lbStatus.Text = "DDL Curr Hd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShipmentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShipmentType.SelectedIndexChanged
        Try

        Catch ex As Exception
            lbStatus.Text = "DDl DeliveryType Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If ViewState("DtPR").Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Detail PR must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnitOrderDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitOrderDt.SelectedIndexChanged
        Dim sqlstring As String
        Dim QtyResult As Double
        Try
            If ddlUnitOrderDt.SelectedValue = ddlUnitWrhsDt.SelectedValue Then
                tbQtyOrderDt.Enabled = False
                tbQtyPack.Enabled = False
            Else
                tbQtyOrderDt.Enabled = True
                tbQtyPack.Enabled = True
            End If

            If ddlUnitOrderDt.SelectedValue <> ddlUnitWrhsDt.SelectedValue Then
                QtyResult = SQLExecuteScalar("S_FindConvertion  '', " + QuotedStr(ddlUnitOrderDt.SelectedValue) + "," + QuotedStr(ddlUnitWrhsDt.SelectedValue), ViewState("DBConnection"))
                tbQtyOrderDt.Text = FormatFloat(tbQtyDt.Text / QtyResult, ViewState("DigitQty"))
                If CFloat(QtyResult) <> 0 Then
                    tbQtyPack.Text = -Int(-QtyResult)
                Else
                    tbQtyOrderDt.Text = "0"
                    tbQtyWrhsFreeDt.Text = "0"
                End If
            Else
                tbQtyOrderDt.Text = FormatFloat(tbQtyWrhsPODt.Text, ViewState("DigitQty"))
                tbQtyPack.Text = "1"
            End If
            tbQtyOrderDt_TextChanged(Nothing, Nothing)

            sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(tbProductDt.Text) + "," + QuotedStr(ddlUnitOrderDt.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            tbPriceForexDt.Text = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
            tbPriceForexDt_TextChanged(Nothing, Nothing)
            tbDiscDt_TextChanged(Nothing, Nothing)
            tbPPHDt_TextChanged(Nothing, Nothing)
            GetInfo(ddlUnitWrhsDt.Text)
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            btnSaveDt.Focus()


        Catch ex As Exception
            lbStatus.Text = "ddlUnitOrderDt_SelectedIndexChanged Error : " + ex.ToString
        End Try

        
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "EXEC S_PRPOReff " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ",1"
            ResultField = "PR_No, Product, DeliveryDate, QtySchedule, Product_Name, Specification, Unit, Require_Date, QtyPROri"
            CriteriaField = "PR_No, DeliveryDate, QtySchedule, Product, Product_Name, Specification, Unit, Require_Date, QtyPROri"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")

            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField As String
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
                Exit Sub
            End If

            Session("filter") = "EXEC S_PRPOReff '','',0, '20500101',1 "

            ResultField = "DeliveryDate, Product_Group, Product_Group_Name, QtySchedule, PR_No, Product, Product_Name, Specification, Qty, Unit, QtyPROri, RequestTo, Department"
            CriteriaField = "DeliveryDate, Product_Group, Product_Group_Name, PR_No, DeliveryDate, QtySchedule, Product, Product_Name, Specification, Unit, QtyPROri, RequestTo, Department"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")

            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCount_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyOrderDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrderDt.TextChanged
        Try
            If tbQtyOrderDt.Text = "" Then
                Exit Sub
            End If
            If ddlUnitOrderDt.SelectedValue <> ddlUnitWrhsDt.SelectedValue Then

                If CFloat(tbQtyOrderDt.Text) <> 0 Then
                    tbQtyPack.Text = tbQtyWrhsPODt.Text / tbQtyOrderDt.Text

                    tbQtyPack.Text = -Int(CFloat(-tbQtyPack.Text))
                    If CFloat(tbQtyPack.Text) = 0 Then
                        tbQtyPack.Text = "1"
                    End If

                End If

             
            Else
                Dim QtyResult As Double
                QtyResult = SQLExecuteScalar("EXEC S_PRPOConvertUnitOrder " + QuotedStr(tbProductDt.Text) + ", " + QuotedStr(ddlUnitOrderDt.SelectedValue) + ", " + tbQtyOrderDt.Text.Replace(",", ""), ViewState("DBConnection"))

                tbQtyOrderDt.Text = FormatFloat(QtyResult, ViewState("DigitQty"))
                If ddlUnitOrderDt.SelectedValue <> ddlUnitWrhsDt.SelectedValue Then

                    tbQtyPack.Text = CInt(tbQtyDt.Text / QtyResult)
                Else : tbQtyPack.Text = "1"
                End If
            End If

            ' tbQtyWrhsPODt.Text = tbQtyOrderDt.Text * tbQtyPack.Text
            tbQtyDt.Text = CFloat(tbQtyOrderDt.Text) * CFloat(tbQtyPack.Text)

            If CFloat(tbQtyDt.Text) > CFloat(tbQtyWrhsPODt.Text) Then
                tbQtyWrhsFreeDt.Text = CFloat(tbQtyDt.Text) - CFloat(tbQtyWrhsPODt.Text)
            Else : tbQtyWrhsFreeDt.Text = CFloat(tbQtyWrhsPODt.Text) - CFloat(tbQtyDt.Text)

            End If

            tbAmountForexDt.Text = tbQtyDt.Text * tbPriceForexDt.Text
            tbQtyWrhsFreeDt.Text = FormatFloat(tbQtyWrhsFreeDt.Text, ViewState("DigitQty"))
            tbQtyDt.Text = FormatFloat(tbQtyDt.Text, ViewState("DigitQty"))

            tbAmountForexDt.Text = FormatFloat(tbAmountForexDt.Text, ViewState("DigitQty"))
            tbTotalForexDt.Text = FormatNumber((tbQtyDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
            tbQtyOrderDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQtyOrderDt_TextChanged Error : " + ex.ToString
        End Try
        'If ddlUnitOrderDt.SelectedValue = ddlUnitOrderDt.SelectedValue Then
        '    tbQtyOrderDt.Text = tbQtyOrderDt.Text
        '    tbQtyOrderDt.Focus()
        'End If
    End Sub

    Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
        tbPPN.Enabled = (ddlReport.SelectedValue = "Y")
        If tbPPN.Enabled = False Then
            tbPPN.Text = "0"
        Else
            tbPPN.Text = ViewState("PPN")
        End If
        tbPPN.Text = FormatFloat(tbPPN.Text, ViewState("DigitQty"))

        tbPPNForex.Text = FormatNumber((tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
        'sebelumny pph di tambah
        tbTotalForex.Text = FormatNumber((tbBaseForex.Text - tbDiscForex.Text) + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text, CInt(ViewState("DigitCurr")))

        AttachScript("setformathd('');", Me.Page, Me.GetType)
    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        ViewState("InputCurrency") = "Y"
        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
    End Sub


    Protected Sub ddlDelivery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDelivery.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("delivery", ddlDelivery.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbDeliveryAddr, Dr("address1").ToString + " " + Dr("address2").ToString)
                BindToText(tbDeliveryCity, Dr("city").ToString)
            Else
                tbDeliveryAddr.Text = ""
                tbDeliveryCity.Text = ""
            End If
            ddlShipmentType.Focus()
        Catch ex As Exception
            Throw New Exception("tb supplier change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Term", ddlTerm.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbTOPRemark, Dr("Term_Name").ToString)
            Else
                tbTOPRemark.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb supplier change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPriceForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForexDt.TextChanged
        Try
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            tbPriceForexDt.Text = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
            If CFloat(tbDiscDt.Text) <> 0 Then
                tbDiscForexDt.Text = FormatNumber((tbDiscDt.Text / 100) * tbAmountForexDt.Text, ViewState("DigitCurr"))
            End If
            tbAmountForexDt.Text = FormatNumber(tbQtyDt.Text * tbPriceForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber((tbQtyDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
            tbPPHDt_TextChanged(Nothing, Nothing)
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            ddlPartial.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDiscDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscDt.TextChanged
        Try
            If tbDiscDt.Text.Trim = "" Then
                tbDiscDt.Text = "0"
            End If
            tbDiscForexDt.Text = FormatNumber((tbDiscDt.Text / 100) * tbAmountForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber((tbQtyOrderDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPPHDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPHDt.TextChanged
        Try
            If tbPPHDt.Text.Trim = "" Then
                tbPPHDt.Text = "0"
            End If
            If ddlfgInclude.SelectedValue = "N" Then
                ViewState("FactorRate") = 1
            Else
                ViewState("FactorRate") = 100 / (100 + FormatFloat(tbPPN.Text, ViewState("DigitQty")))
            End If

            tbPPHForexDt.Text = FormatNumber((tbPPHDt.Text / 100) * ViewState("FactorRate") * tbTotalForexDt.Text, ViewState("DigitCurr"))
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("tbPPHDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupplier.Click
        ViewState("InputSupplier") = "Y"
        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
    End Sub

    Protected Sub ddlfgInclude_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlfgInclude.SelectedIndexChanged
        Try
            If ddlfgInclude.SelectedValue = "N" Then
                ViewState("FactorRate") = 1
            Else
                ViewState("FactorRate") = 100 / (100 + FormatFloat(tbPPN.Text, ViewState("DigitQty")))
            End If
            totalingDt()
        Catch ex As Exception
            lbStatus.Text = "ddlfgInclude_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try

        Catch ex As Exception
            Throw New Exception("tbDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDiscForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscForexDt.TextChanged
        Try
            If tbDiscForexDt.Text.Trim = "" Then
                tbDiscForexDt.Text = "0"
            End If
            tbDiscDt.Text = FormatNumber((tbDiscForexDt.Text * 100) / tbAmountForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber((tbQtyOrderDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tbDiscForexDt_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyPack_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyPack.TextChanged
        Try
            If ddlUnitOrderDt.SelectedValue <> ddlUnitWrhsDt.SelectedValue Then
                
                If tbQtyPack.Text <> "0" Then
                    tbQtyOrderDt.Text = tbQtyWrhsPODt.Text / tbQtyPack.Text
                    tbQtyOrderDt.Text = -Int(CFloat(-tbQtyOrderDt.Text))
                    tbQtyDt.Text = tbQtyOrderDt.Text * tbQtyPack.Text
                    If tbQtyDt.Text > tbQtyWrhsPODt.Text Then
                        tbQtyWrhsFreeDt.Text = tbQtyDt.Text - tbQtyWrhsPODt.Text
                    Else : tbQtyWrhsFreeDt.Text = tbQtyWrhsPODt.Text - tbQtyDt.Text

                    End If
                    tbAmountForexDt.Text = tbQtyDt.Text * tbPriceForexDt.Text
                    tbQtyWrhsFreeDt.Text = FormatFloat(tbQtyWrhsFreeDt.Text, ViewState("DigitQty"))
                    tbQtyDt.Text = FormatFloat(tbQtyDt.Text, ViewState("DigitQty"))
                    tbAmountForexDt.Text = FormatFloat(tbAmountForexDt.Text, ViewState("DigitQty"))
                    tbTotalForexDt.Text = FormatNumber((tbQtyDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "tbQtyPack_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
