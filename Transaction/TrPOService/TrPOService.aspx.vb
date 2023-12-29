Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPOService_TrPOService
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Private Function GetStringHd(ByVal FgImp As String) As String
        Return "SELECT DISTINCT A.* From V_PRPOHd A INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = " + QuotedStr(ViewState("UserId").ToString) + " AND POType ='Service'"
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
                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT PR_No) from V_PRPOGetRequest Where RequestType in ('Service', 'Transportation') ", ViewState("DBConnection").ToString)
            End If
            If Not Session("PostNmbr") Is Nothing Then
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) = '" + Session("PostNmbr") + "'")
                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT PR_No) from V_PRPOGetRequest Where RequestType in ('Service', 'Transportation') ", ViewState("DBConnection").ToString)
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
                    ddlTerm_SelectedIndexChanged(Nothing, Nothing)
                    'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'tbRateHd.Enabled = False
                    ModifyDt()
                ElseIf ViewState("Sender") = "btnPRNo" Then
                    tbPRNo.Text = Session("Result")(0).ToString
                    tbMONo.Text = TrimStr(Session("Result")(1).ToString)
                    tbEquip.Text = TrimStr(Session("Result")(2).ToString)
                    tbEquipName.Text = TrimStr(Session("Result")(3).ToString)
                    BindToDropList(ddlDept, Session("Result")(4).ToString)
                ElseIf ViewState("Sender") = "btnEquip" Then
                    tbEquip.Text = Session("Result")(1).ToString
                    tbEquipName.Text = Session("Result")(2).ToString
                ElseIf ViewState("Sender") = "btnProduct" Then
                    tbProductDt.Text = Session("Result")(0).ToString
                    tbProductNameDt.Text = Session("Result")(1).ToString
                    tbSpecificationDt.Text = Session("Result")(2).ToString
                    tbQtyOrderDt.Text = Session("Result")(3).ToString
                    ddlUnitOrderDt.SelectedValue = Session("Result")(4).ToString
                    tbQtyWrhsDt.Text = Session("Result")(3).ToString
                    ddlUnitWrhsDt.SelectedValue = Session("Result")(5).ToString
                ElseIf ViewState("Sender") = "btnGetDt" Then
                    Dim sqlstring As String
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Specification2") = drResult("Specification2")
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("QtyOrder") = FormatFloat(FindConvertUnitOrder(dr("Product").ToString, dr("UnitOrder").ToString, dr("Qty").ToString, ViewState("DBConnection")), ViewState("DigitQty"))
                            sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(dr("Product").ToString) + "," + QuotedStr(dr("Unit").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                            ViewState("PricePO") = 0
                            ViewState("PricePO") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                            dr("PriceForex") = FormatNumber(ViewState("PricePO"), CInt(ViewState("DigitCurr")))
                            dr("BrutoForex") = FormatNumber(CFloat(ViewState("PricePO")) * CFloat(drResult("Qty")), CInt(ViewState("DigitCurr")))
                            dr("Disc") = 0
                            dr("DiscForex") = 0
                            dr("PPH") = 0
                            dr("PPHForex") = 0
                            dr("NettoForex") = FormatNumber(CFloat(ViewState("PricePO")) * CFloat(drResult("Qty")), CInt(ViewState("DigitCurr")))
                            dr("FgPartialDelivery") = "N"
                            'dr("DeliveryDate") = Format(tbDate.SelectedDate, "dd MMM yyyy")

                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    totalingDt()
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    ' ModifyDt()
                ElseIf ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert

                        If FirstTime Then
                            'BindToDropList(ddlProductGroup, drResult("Product_Group"))
                            BindToText(tbPRNo, drResult("PR_No"))
                            BindToDropList(ddlDept, drResult("Department"))
                        End If
                        Dim sqlstring As String
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Specification2") = TrimStr(drResult("Specification2"))
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("QtyOrder") = FormatFloat(FindConvertUnitOrder(dr("Product").ToString, dr("UnitOrder").ToString, dr("Qty").ToString, ViewState("DBConnection")), ViewState("DigitQty"))
                            sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(dr("Product").ToString) + "," + QuotedStr(dr("Unit").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                            ViewState("PricePO") = 0
                            ViewState("PricePO") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                            dr("PriceForex") = FormatNumber(ViewState("PricePO"), CInt(ViewState("DigitCurr")))
                            dr("BrutoForex") = FormatNumber(CFloat(ViewState("PricePO")) * CFloat(drResult("Qty")), CInt(ViewState("DigitCurr")))
                            dr("Disc") = 0
                            dr("DiscForex") = 0
                            dr("PPH") = 0
                            dr("PPHForex") = 0
                            dr("NettoForex") = FormatNumber(CFloat(ViewState("PricePO")) * CFloat(drResult("Qty")), CInt(ViewState("DigitCurr")))
                            ViewState("Dt").Rows.Add(dr)
                        End If

                        FirstTime = False

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)

                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
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
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")

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
            FillCombo(ddlCurrHd, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrderDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrderDt2, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitWrhsDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlDelivery, "EXEC S_GetDelivery", True, "deliverycode", "deliveryname", ViewState("DBConnection"))
            FillCombo(ddlShipmentType, "EXEC S_GetShipment ", True, "ShipmentCode", "ShipmentName", ViewState("DBConnection"))
            FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))

            'FillCombo(ddlProductGroup, "EXEC S_GetProductGroup ", True, "Product_Group_Code", "Product_Group_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrExp") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbSpecificationDt.Attributes.Add("ReadOnly", "true")
            tbSpecificationDt2.Attributes.Add("ReadOnly", "true")

            tbPPNForex.Attributes.Add("ReadOnly", "True")
            tbPPHForex.Attributes.Add("ReadOnly", "True")
            tbBaseForex.Attributes.Add("ReadOnly", "True")
            tbDiscForex.Attributes.Add("ReadOnly", "True")
            tbDiscForexDt.Attributes.Add("ReadOnly", "True")
            tbQtyWrhsDt.Attributes.Add("ReadOnly", "True")
            'tbDPForex.Attributes.Add("ReadOnly", "True")
            tbPPHForexDt.Attributes.Add("ReadOnly", "True")
            tbTotalForex.Attributes.Add("ReadOnly", "True")
            tbTotalForexDt.Attributes.Add("ReadOnly", "True")
            tbAmountForexDt.Attributes.Add("ReadOnly", "True")

            'tbRateHd.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDP.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDPForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbOtherForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrderDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhsDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPHForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrderDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbRateHd.Attributes.Add("OnBlur", "setformathd('');")
            tbDP.Attributes.Add("OnBlur", "setformathd('DP');")
            tbDPForex.Attributes.Add("OnBlur", "setformathd('DPForex');")
            tbBaseForex.Attributes.Add("OnBlur", "setformathd('');")
            'tbDisc.Attributes.Add("OnBlur", "setformathd();")
            tbDiscForex.Attributes.Add("OnBlur", "setformathd('');")
            tbPPN.Attributes.Add("OnBlur", "setformathd('');")
            tbPPHForex.Attributes.Add("OnBlur", "setformathd('');")
            tbPPNForex.Attributes.Add("OnBlur", "setformathd('');")
            tbTotalForex.Attributes.Add("OnBlur", "setformathd('');")
            tbOtherForex.Attributes.Add("OnBlur", "setformathd('');")

            tbQtyOrderDt.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyWrhsDt.Attributes.Add("OnBlur", "setformatdt();")
            tbPriceForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbDiscDt.Attributes.Add("OnBlur", "setformatdt();")
            tbDiscForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbPPHDt.Attributes.Add("OnBlur", "setformatdt();")
            tbPPHForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbTotalForexDt.Attributes.Add("OnBlur", "setformatdt();")


            tbQtyOrderDt2.Attributes.Add("OnBlur", "setformatdt2();")
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
            'btnAdd2.Visible = btngo.Visible
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
    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PRPODt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_PRPODt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            ViewState("AdvanceFilter") = ""
            BindData(ViewState("AdvanceFilter"))
            pnlNav.Visible = True
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
        'Dim DT As DataTable
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
                Session("ReportFile") = ".../../../Rpt/FormPOservice.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)

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
                        'ElseIf ActionValue = "Post" Then

                        '    DT = ExecSPCekPosting("S_PRPOPostCek", Nmbr(j), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '    If (DT.Rows.Count > 0) Then
                        '        If (DT.Rows.Count > 0) And (DT.Rows(0)("NewPage").ToString = "Y") Then
                        '            Try
                        '                If ViewState("FgImp") = "N" Then
                        '                    ViewState("FgCekPOST") = "POGoods"
                        '                Else
                        '                    ViewState("FgCekPOST") = "POGoodsImp"
                        '                End If
                        '                ViewState("DTPost") = DT
                        '                Session("PostNmbr") = Nmbr(j)
                        '                Response.Redirect("..\..\Transaction\TrPRReq\FormCekPostPR.aspx")

                        '                'AttachScript("opencekpostingdlg();", Page, Me.GetType)
                        '            Catch ex As Exception
                        '                lbStatus.Text = "opencekpostingdlg Error = " + ex.ToString
                        '            End Try
                        '        Else
                        '            Result = ExecSPCommandGo(ActionValue, "S_PRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '            If Trim(Result) <> "" Then
                        '                lbStatus.Text = lbStatus.Text + Result + "<br />"
                        '            End If
                        '        End If
                        '    Else

                        '        Result = ExecSPCommandGo(ActionValue, "S_PRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '        If Trim(Result) <> "" Then
                        '            lbStatus.Text = lbStatus.Text + Result + "<br />"
                        '        End If
                        '    End If
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
            'tbSuppCode.Enabled = State
            btnPRNo.Enabled = State

            'btnSupp.Enabled = State
            'ddlfgInclude.Enabled = State
            'ddlProductGroup.Enabled = State
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
    Private Sub BindDataDt2(ByVal Nmbr As String, ByVal Revisi As String)
        Dim dt As New DataTable
        Dim Drow As DataRow()
        Try
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt

            Drow = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text))
            'BindGridDt(dt, GridDt2)

            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If


            'If ViewState("StateHd") = "View" And GridDt2.Columns(0).Visible Then
            '    GridDt2.Columns(0).Visible = False
            'Else
            '    GridDt2.Columns(0).Visible = True
            'End If
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
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
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
            ddlReport.Text = "Choose One"
            ddlDept.SelectedValue = ""
            ddlfgInclude.SelectedValue = "N"
            ViewState("FactorRate") = 1
            ddlfgAdditional.SelectedValue = "N"
            ddlReport.SelectedValue = "Y"
            tbEquip.Text = ""
            tbEquipName.Text = ""
            tbPRNo.Text = ""
            tbDeliveryAddr.Text = ""
            tbMONo.Text = ""
            tbBaseForex.Text = "0"
            'tbDisc.Text = "0"
            tbDiscForex.Text = "0"
            tbTotalForex.Text = "0"
            tbPPHForex.Text = "0"
            tbPPN.Text = "10"
            tbTOPRemark.Text = ""
            tbDeliveryCity.Text = ""
            tbSuppPONo.Text = ""
            tbPPNForex.Text = "0"
            ddlCurrHd.SelectedValue = ViewState("Currency")
            ddlTerm.SelectedIndex = 0
            ddlShipmentType.SelectedIndex = 0
            tbAttn.Text = ""
            tbRemark.Text = ""
            ddlDelivery.SelectedValue = ""
            tbDate.SelectedDate = Now.Date
            tbServiceDate.SelectedDate = Now.Date
            tbDP.Text = "0"
            tbDPForex.Text = "0"
            tbOtherForex.Text = "0"
            ddlfgAdditional_SelectedIndexChanged(Nothing, Nothing)
            'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateHd.Enabled = False
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
            tbSpecificationDt.Text = ""
            tbQtyOrderDt.Text = "0"
            tbQtyWrhsDt.Text = "0"
            tbPriceForexDt.Text = "0"
            tbAmountForexDt.Text = "0"
            tbDiscDt.Text = "0"
            tbDiscForexDt.Text = "0"
            tbPPHDt.Text = "0"
            tbPPHForexDt.Text = "0"
            tbTotalForexDt.Text = "0"
            'tbDeliveryDateDt.SelectedDate = ViewState("ServerDate") 'Now.Date
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbDeliveryDateDt2.SelectedDate = Nothing
            tbQtyOrderDt2.Text = "0"
            ddlUnitOrderDt2.SelectedIndex = 0
            tbRemarkDt2.Text = ""
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
            If tbPRNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PR No must have value")
                tbPRNo.Focus()
                Return False
            End If
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
                Return False
            End If
            If ddlReport.Text = "Choose One" Then
                lbStatus.Text = MessageDlg("Report must have value")
                ddlReport.Focus()
                Return False
            End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If ddlReport.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Report must have value")
                ddlReport.Focus()
                Return False
            End If
            'If CFloat(tbRateHd.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Rate must have value")
            '    tbRateHd.Focus()
            '    Return False
            'End If
            'If ddlCurrHd.SelectedValue <> ViewState("Currency") And CFloat(tbRateHd.Text) = 1 Then
            '    lbStatus.Text = MessageDlg("Rate must have value")
            '    tbRateHd.Focus()
            '    Return False
            'End If
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
                If Dr("QtyOrder").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Wrhs Must Have Value")
                    Return False
                End If
                If Dr("PriceForex").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
            Else
                If CFloat(tbQtyOrderDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If CFloat(tbQtyWrhsDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Wrhs Must Have Value")
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
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("QtyOrder").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
            Else
                If CFloat(tbQtyOrderDt2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
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
                Row("Specification") = tbSpecificationDt.Text
                Row("Qty") = tbQtyWrhsDt.Text
                Row("Unit") = ddlUnitWrhsDt.SelectedValue
                Row("QtyOrder") = tbQtyOrderDt.Text
                Row("UnitOrder") = ddlUnitOrderDt.SelectedValue
                Row("PriceForex") = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
                Row("BrutoForex") = tbAmountForexDt.Text
                Row("Disc") = tbDiscDt.Text
                Row("DiscForex") = tbDiscForexDt.Text
                Row("PPH") = tbPPHDt.Text
                Row("PPHForex") = tbPPHForexDt.Text
                Row("NettoForex") = tbTotalForexDt.Text
                'Row("FgPartialDelivery") = ddlPartial.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                'Row("DeliveryDate") = Format(tbDeliveryDateDt.SelectedDate, "dd MMMM yyyy")

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
                dr("Specification") = tbSpecificationDt.Text
                dr("Qty") = tbQtyWrhsDt.Text
                dr("Unit") = ddlUnitWrhsDt.SelectedValue
                dr("QtyOrder") = tbQtyOrderDt.Text
                dr("UnitOrder") = ddlUnitOrderDt.SelectedValue
                dr("PriceForex") = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
                dr("BrutoForex") = tbAmountForexDt.Text
                dr("Disc") = tbDiscDt.Text
                dr("DiscForex") = tbDiscForexDt.Text
                dr("PPH") = tbPPHDt.Text
                dr("PPHForex") = tbPPHForexDt.Text
                dr("NettoForex") = tbTotalForexDt.Text
                dr("FgPartialDelivery") = "N"
                dr("Remark") = tbRemarkDt.Text
                'dr("DeliveryDate") = Format(tbDeliveryDateDt.SelectedDate, "dd MMMM yyyy")

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

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt2").Select("Delivery = " + QuotedStr(Format(tbDeliveryDateDt2.SelectedDate, "dd MMMM yyyy")))
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("Delivery = " + QuotedStr(ViewState("Delivery")))(0)

                Row.BeginEdit()
                Row("Product") = lbProductDt2.Text
                Row("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMMM yyyy")
                Row("QtyOrder") = tbQtyOrderDt2.Text
                Row("UnitOrder") = ddlUnitOrderDt2.SelectedValue()
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If
                dr = ViewState("Dt2").NewRow
                dr("Product") = lbProductDt2.Text
                dr("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMMM yyyy")
                dr("QtyOrder") = tbQtyOrderDt2.Text
                dr("UnitOrder") = ddlUnitOrderDt2.SelectedValue()
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(Not DtExist())
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub totalingDt()
        Dim dr As DataRow
        Dim total, pph, disc As Double
        Try
            total = 0
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
            If ddlCurrHd.SelectedValue = ViewState("Currency") Then
                total = Math.Floor(total)
            End If
            'tbBaseForex.Text = FormatNumber(total, CInt(ViewState("DigitCurr")))
            If ddlfgInclude.SelectedValue = "N" Then
                tbBaseForex.Text = FormatNumber(total + tbDiscForex.Text, CInt(ViewState("DigitCurr")))
            Else
                '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
                tbBaseForex.Text = FormatNumber((total * 100 / (100 + tbPPN.Text)) + tbDiscForex.Text, CInt(ViewState("DigitCurr")))
            End If
            tbPPNForex.Text = FormatNumber((tbPPN.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
            tbTotalForex.Text = FormatNumber(tbBaseForex.Text - tbDiscForex.Text + tbPPNForex.Text - tbPPHForex.Text + tbOtherForex.Text, CInt(ViewState("DigitCurr")))
            tbDPForex.Text = FormatNumber((tbDP.Text / 100) * (tbBaseForex.Text - tbDiscForex.Text), ViewState("DigitCurr"))
            AttachScript("setformathd('');", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyDt()
        Dim Dt As DataTable
        Dim DR, CurrRow As DataRow
        Dim SelectRow As DataRow()
        Dim i, len As Integer
        Dim sqlstring As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Dt = ViewState("Dt")

            len = Dt.Rows.Count


            For i = 0 To len - 1
                If Not ViewState("Dt").Rows(i).RowState = DataRowState.Deleted Then
                    ViewState("Dt").Rows(i).BeginEdit()
                    'ViewState("Dt").Rows(i)("QtyOrder") = 0
                    'ViewState("Dt").Rows(i)("Qty") = 0
                    ViewState("Dt").Rows(i).EndEdit()
                End If
            Next

            For Each DR In Dt.Rows
                If Not DR.RowState = DataRowState.Deleted Then

                    SelectRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))
                    If SelectRow.Length = 0 Then
                        CurrRow = ViewState("Dt").NewRow
                        CurrRow("Product") = DR("Product")
                        CurrRow("ProductName") = DR("ProductName")
                        CurrRow("Specification") = TrimStr(DR("Specification"))
                        CurrRow("Qty") = DR("Qty")
                        CurrRow("Unit") = DR("Unit")
                        CurrRow("UnitOrder") = DR("UnitOrder")
                        CurrRow("QtyOrder") = FormatFloat(FindConvertUnitOrder(DR("Product").ToString, DR("UnitOrder").ToString, DR("Qty").ToString, ViewState("DBConnection")).ToString, ViewState("Digit")("Qty"))
                        sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("Unit").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                        CurrRow("PriceForex") = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
                        CurrRow("BrutoForex") = 0
                        CurrRow("Disc") = 0
                        CurrRow("DiscForex") = 0
                        CurrRow("PPH") = 0
                        CurrRow("PPHForex") = 0
                        CurrRow("NettoForex") = 0
                        CurrRow("FgPartialDelivery") = "N"
                        CurrRow("DeliveryDate") = Format(tbDate.SelectedDate, "dd MMM yyyy")
                        
                        ViewState("Dt").Rows.Add(CurrRow)
                    Else
                        CurrRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))(0)

                        CurrRow.BeginEdit()
                        'CurrRow("QtyOrder") = FormatFloat((CFloat(CurrRow("QtyOrder").ToString) + CFloat(DR("QtyOrder").ToString)), ViewState("Digit")("Qty"))
                        'CurrRow("Qty") = FormatFloat(FindConvertUnitOrder(CurrRow("Product").ToString, CurrRow("UnitOrder").ToString, CurrRow("QtyOrder").ToString, ViewState("DBConnection")).ToString, ViewState("Digit")("Qty"))
                        'CurrRow("Qty") = FormatNumber((CFloat(CurrRow("QtyOrder").ToString.Replace(",", "")) + CFloat(DR("Qty").ToString)).ToString, ViewState("Digit")("Qty"))
                        CurrRow.EndEdit()
                        lbStatus.Text = CurrRow("Qty")
                        Exit For
                    End If
                End If
            Next

            For i = 0 To GetCountRecord(ViewState("Dt")) - 1
                DR = ViewState("Dt").Rows(i)
                If Not DR.RowState = DataRowState.Deleted Then
                    If DR("QtyOrder") = 0 And DR("Qty") = 0 Then
                        'SelectRow = ViewState("Dt2").Select("Product=" + QuotedStr(DR("Product")))
                        'For len = 0 To SelectRow.Length - 1
                        '    SelectRow(len).Delete()
                        'Next
                        'DR.Delete()
                    Else
                        sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("UnitOrder").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                        DR("PriceForex") = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
                        DR("BrutoForex") = FormatNumber(CFloat(DR("Qty").ToString) * CFloat(DR("PriceForex").ToString), ViewState("DigitCurr"))
                        DR("PPhForex") = FormatNumber(CFloat(DR("PPhForex").ToString) * ViewState("FactorRate"), ViewState("DigitCurr"))
                        DR("NettoForex") = DR("BrutoForex")
                    End If
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
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

    Private Function AllowedRecordDt2() As Integer
        Try
            If ViewState("Delivery") = QuotedStr(Format(tbDeliveryDateDt2.SelectedDate, "dd MMMM yyyy")) Then
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

            'Save Hd
            'If ddlCheckBudget.SelectedValue = "N" Then
            '    BudgetYear = "0"
            '    BudgetMonth = "0"
            'Else
            '    BudgetYear = ddlYear.SelectedValue
            '    BudgetMonth = ddlMonth.SelectedValue
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("PS", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlDept.SelectedValue, ViewState("DBConnection").ToString)
                'If Request.QueryString("ContainerId").ToString = "POGoodsID" Then
                '    FgImport = "N"
                'Else
                '    FgImport = "Y"
                'End If
                'ubah disini
                SQLString = "INSERT INTO PRCPOHd (TransNmbr, Revisi, TransDate, STATUS, Department, FgReport, " + _
                "Supplier, Attn, Term, SuppContractNo, CustContractNo, ShipmentType, Delivery, " + _
                "DeliveryAddr, DeliveryCity, Currency, ForexRate, BaseForex, Disc, DiscForex, DP, DPForex, PPn, " + _
                "PPNForex, PPHForex, OtherForex, TotalForex, Remark, UserPrep, " + _
                "DatePrep, FgActive, POType, TermPayment, FgAddCost, AddCostRemark, FgPriceIncludeTax, " + _
                "PRNo, MONo, Equipment, FactorRate)" + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 0, " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlDept.SelectedValue) + ", " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + _
                ", " + QuotedStr(ddlTerm.SelectedValue) + ", " + QuotedStr(tbSuppPONo.Text) + ", " + QuotedStr(tbCustContractNo.Text) + _
                ", " + QuotedStr(ddlShipmentType.SelectedValue) + ", " + QuotedStr(ddlDelivery.SelectedValue) + _
                ", " + QuotedStr(tbDeliveryAddr.Text) + ", " + QuotedStr(tbDeliveryCity.Text) + ", " + _
                QuotedStr(ddlCurrHd.SelectedValue) + ", " + "0, " + tbBaseForex.Text.Replace(",", "") + ", " + _
                "0 , " + tbDiscForex.Text.Replace(",", "") + ", " + tbDP.Text.Replace(",", "") + _
                ", " + tbDPForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + _
                ", " + tbPPHForex.Text.Replace(",", "") + ", " + tbOtherForex.Text.Replace(",", "") + _
                ", " + tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y', 'Service' " + _
                ", " + QuotedStr(tbTOPRemark.Text) + ", " + QuotedStr(ddlfgAdditional.SelectedValue) + ", " + _
                QuotedStr(tbAddCostRemark.Text) + ", " + QuotedStr(ddlfgInclude.SelectedValue) + ", " + _
                QuotedStr(tbPRNo.Text) + ", " + QuotedStr(tbMONo.Text) + ", " + QuotedStr(tbEquip.Text) + _
                ", " + QuotedStr(ViewState("FactorRate"))
                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCPOHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCPOHd SET FgReport =" + QuotedStr(ddlReport.SelectedValue) + ", Supplier=" + QuotedStr(tbSuppCode.Text) + ", Department = " + QuotedStr(ddlDept.SelectedValue) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", Term=" + QuotedStr(ddlTerm.Text) + ", TermPayment= " + QuotedStr(tbTOPRemark.Text) + _
                ", ShipmentType= " + QuotedStr(ddlShipmentType.SelectedValue) + _
                ", Delivery=" + QuotedStr(ddlDelivery.SelectedValue) + ", DeliveryAddr=" + QuotedStr(tbDeliveryAddr.Text) + _
                ", DeliveryCity=" + QuotedStr(tbDeliveryCity.Text) + ", FgAddCost=" + QuotedStr(ddlfgAdditional.SelectedValue) + _
                ", Currency=" + QuotedStr(ddlCurrHd.SelectedValue) + ", AddCostRemark=" + QuotedStr(tbAddCostRemark.Text) + _
                ", BaseForex=" + tbBaseForex.Text.Replace(",", "") + _
                ", Disc= 0, DiscForex=" + tbDiscForex.Text.Replace(",", "") + _
                ", DP=" + tbDP.Text.Replace(",", "") + ", DPForex=" + tbDPForex.Text.Replace(",", "") + ", PPn=" + _
                tbPPN.Text.Replace(",", "") + ", PPNForex=" + tbPPNForex.Text.Replace(",", "") + ", PPHForex=" + _
                tbPPHForex.Text.Replace(",", "") + ", OtherForex=" + tbOtherForex.Text.Replace(",", "") + ", TotalForex=" + _
                tbTotalForex.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + ", FgPriceIncludeTax=" + QuotedStr(ddlfgInclude.SelectedValue) + ", SuppContractNo=" + _
                QuotedStr(tbSuppPONo.Text) + ", CustContractNo=" + QuotedStr(tbCustContractNo.Text) + ", PRNo = " + _
                QuotedStr(tbPRNo.Text) + ", MONo=" + QuotedStr(tbMONo.Text) + ", Equipment = " + QuotedStr(tbEquip.Text) + _
                ", FactorRate=" + QuotedStr(ViewState("FactorRate")) + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text
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

            If Not ViewState("Dt2") Is Nothing Then
                Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbCode.Text
                    Row(I)("Revisi") = lbRevisi.Text
                    Row(I).EndEdit()
                Next
            End If



            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, Product, Specification, QtyOrder, UnitOrder, Qty, Unit, PriceForex, BrutoForex, Disc, DiscForex, PPh, PPhForex, NettoForex, Remark FROM PRCPODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PRCPODt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            If Not ViewState("Dt2") Is Nothing Then
                cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, Product, Delivery, QtyOrder, UnitOrder, Remark FROM PRCPODt2 WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim Dt2 As New DataTable("PRCPODt2")

                Dt2 = ViewState("Dt2")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("Dt2") = Dt2
            End If


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
            'If ViewState("Dt2").Rows.Count = 0 And ViewState("FgPatrial") = "Y" Then
            '    lbStatus.Text = MessageDlg("Detail Partial must have at least 1 record")
            '    Exit Sub
            'End If
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
            'ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            'Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
            btnPRNo.Enabled = True
            GridDt.Columns(1).Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            Cleardt2()
            ddlReport.Enabled = True
            'btnAddDt2.Visible = True
            'btnAddDt2Ke2.Visible = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            PnlDt.Visible = True
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
            ViewState("DateFieldName") = FDateName.Split(",")
            ViewState("DateFieldValue") = FDateValue.Split(",")
            ViewState("FieldName") = FilterName.Split(",")
            ViewState("FieldValue") = FilterValue.Split(",")
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
                    'BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    'Menu1.TabIndex = 1
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    'Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    GridDt.Columns(1).Visible = True
                    btnAddDt.Visible = False
                    btnAddDt1.Visible = False
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
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        tbAddCostRemark.Enabled = ddlfgAdditional.SelectedIndex = 0
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        EnableHd(Not DtExist())
                        GridDt.Columns(1).Visible = False
                        btnAddDt.Visible = True
                        btnAddDt1.Visible = True
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
                    tbAddCostRemark.Enabled = ddlfgAdditional.SelectedIndex = 0
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_PRFormPO ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "''"
                    Session("ReportFile") = ".../../../Rpt/FormPOservice.frx"
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
            If e.CommandName = "Detail" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                lbProductDt2.Text = GVR.Cells(3).Text
                If GVR.Cells(17).Text = "N" Then
                    lbStatus.Text = MessageDlg(" Cannot Detail Partial Delivery N")
                    Exit Sub
                End If

                MultiView1.ActiveViewIndex = 0
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                Else
                    drow = ViewState("Dt2").Select("Product=" + QuotedStr(GVR.Cells(3).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status PO is not Post, cannot close product")
                    Exit Sub
                End If
                'If GVR.Cells(24).Text <> "&nbsp;" Then
                '    lbStatus.Text = MessageDlg("Product Closed Already")
                '    Exit Sub
                'End If
                ViewState("ProductClose") = GVR.Cells(3).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text) + " AND Delivery = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()

            dr = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If


            'BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select DigitDecimal FROM MsCurrency WHERE CurrCode = " + QuotedStr(ddlCurrHd.SelectedValue)))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("ProductDt") = GVR.Cells(2).Text
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
            FillTextBoxDt2(lbProductDt2.Text, CDate(GVR.Cells(1).Text))
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select DigitDecimal FROM MsCurrency WHERE CurrCode = " + QuotedStr(ViewState("Currency"))))
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            ViewState("Delivery") = GVR.Cells(1).Text
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Part Editing Error : " + ex.ToString
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
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbPRNo, Dt.Rows(0)("PRNo").ToString)
            BindToText(tbMONo, Dt.Rows(0)("MONo").ToString)
            BindToText(tbEquip, Dt.Rows(0)("Equipment").ToString)
            BindToText(tbEquipName, Dt.Rows(0)("EquipmentName").ToString)
            BindToDate(tbServiceDate, Dt.Rows(0)("ReqServiceDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("SupplierName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbSuppPONo, Dt.Rows(0)("SuppContractNo").ToString)
            BindToText(tbCustContractNo, Dt.Rows(0)("CustContractNo").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToText(tbTOPRemark, Dt.Rows(0)("TermPayment").ToString)
            BindToDropList(ddlDelivery, Dt.Rows(0)("Delivery").ToString)
            BindToDropList(ddlShipmentType, Dt.Rows(0)("ShipmentType").ToString)
            BindToText(tbDeliveryAddr, Dt.Rows(0)("DeliveryAddr").ToString)
            BindToText(tbDeliveryCity, Dt.Rows(0)("DeliveryCity").ToString)
            BindToDropList(ddlCurrHd, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlfgInclude, Dt.Rows(0)("FgPriceIncludeTax").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            'BindToText(tbRateHd, Dt.Rows(0)("ForexRate").ToString) ', ViewState("Digit")("Rate")
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            'BindToText(tbDisc, Dt.Rows(0)("Disc").ToString)
            BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPN").ToString) ', ViewState("Digit")("Percent")
            BindToText(tbDP, Dt.Rows(0)("DP").ToString) ', ViewState("Digit")("Percent")
            BindToText(tbDPForex, Dt.Rows(0)("DPForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPNForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPHForex, Dt.Rows(0)("PPHForex").ToString, ViewState("DigitCurr"))
            BindToText(tbOtherForex, Dt.Rows(0)("OtherForex").ToString, ViewState("DigitCurr"))
            BindToDropList(ddlfgAdditional, Dt.Rows(0)("FgAddCost").ToString)
            BindToText(tbAddCostRemark, Dt.Rows(0)("AddCostRemark").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            ddlReport.Enabled = False
            AttachScript("setformathd('');", Me.Page, Me.GetType)
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
                BindToText(tbSpecificationDt, Dr(0)("Specification").ToString)
                BindToText(tbspecificationDt2, Dr(0)("Specification2").ToString)
                BindToText(tbQtyWrhsDt, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitWrhsDt, Dr(0)("Unit").ToString)
                BindToText(tbQtyOrderDt, Dr(0)("QtyOrder").ToString)
                BindToDropList(ddlUnitOrderDt, Dr(0)("UnitOrder").ToString)
                BindToText(tbPriceForexDt, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForexDt, Dr(0)("BrutoForex").ToString)
                BindToText(tbDiscDt, Dr(0)("Disc").ToString)
                BindToText(tbDiscForexDt, Dr(0)("DiscForex").ToString)
                BindToText(tbPPHDt, Dr(0)("PPH").ToString)
                BindToText(tbPPHForexDt, Dr(0)("PPHForex").ToString)
                BindToText(tbTotalForexDt, Dr(0)("NettoForex").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal Product As String, ByVal DeliveryDate As Date)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Product = " + QuotedStr(Product) + " AND Delivery =" + QuotedStr(DeliveryDate.ToString))
            If Dr.Length > 0 Then
                tbDeliveryDateDt2.SelectedDate = Dr(0)("Delivery").ToString
                BindToText(tbQtyOrderDt2, Dr(0)("QtyOrder").ToString)
                BindToDropList(ddlUnitOrderDt2, Dr(0)("UnitOrder").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name, Currency, Term, Contact_Person, Phone FROM VMsSupplier"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Supplier_Code, Supplier_Name, Contact_Person, Currency, Term"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                tbAttn.Text = Dr("Contact_Person")
                ddlCurrHd.SelectedValue = Dr("Currency")
                BindToDropList(ddlTerm, Dr("Term"))
                ddlTerm_SelectedIndexChanged(Nothing, Nothing)
                'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                'tbRateHd.Enabled = False
                ModifyDt()

                tbAttn.Focus()
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbAttn.Text = ""
                tbSuppCode.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tb supplier change Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnRequestPR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequestPR.Click
    '    Dim ResultField, CriteriaField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "EXEC S_PRPOReff 'Goods', '', '" + tbCode.Text + "', " + lbRevisi.Text
    '        CriteriaField = "PR_No, Product, Product_Name, Specification, Unit"
    '        ViewState("CriteriaField") = CriteriaField.Split(",")
    '        ResultField = "PR_No, Product, Product_Name, Specification, Qty, Unit, UnitOrder"
    '        ViewState("Sender") = "btnRequestPR"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Dim dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If lbProductDt2.Text = "" Or lbProductDt2.Text = "&nbsp;" Then
                Exit Sub
            End If
            Cleardt2()

            dr = ViewState("Dt").Select("Product=" + QuotedStr(lbProductDt2.Text))(0)
            tbQtyOrderDt2.Text = dr("QtyOrder").ToString
            ddlUnitOrderDt2.SelectedValue = dr("UnitOrder").ToString
            tbDeliveryDateDt2.SelectedDate = CDate(dr("DeliveryDate"))

            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub


    'Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
    '    Dim index As Integer
    '    Try
    '        index = Int32.Parse(e.Item.Value)
    '        MultiView1.ActiveViewIndex = index
    '        btnSaveTrans.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "Menu Item Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlCurrHd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrHd.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                FillCombo(ddlCurrHd, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateHd.Enabled = False
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


            SaveAll()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlfgAdditional_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlfgAdditional.SelectedIndexChanged
        Try
            If ddlfgAdditional.SelectedValue = "Y" Then
                tbAddCostRemark.Enabled = True
            Else
                tbAddCostRemark.Enabled = False
            End If
            tbAddCostRemark.Text = ""
        Catch ex As Exception
            lbStatus.Text = "DDL Curr Expense Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnitOrderDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitOrderDt.SelectedIndexChanged
        Dim sqlstring As String
        tbQtyWrhsDt.Text = FormatFloat(FindConvertUnitOrder(tbProductDt.Text, ddlUnitOrderDt.SelectedValue, tbQtyOrderDt.Text, ViewState("DBConnection").ToString).ToString, ViewState("DigitQty"))
        sqlstring = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(tbProductDt.Text) + "," + QuotedStr(ddlUnitOrderDt.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
        ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
        tbPriceForexDt.Text = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
        AttachScript("setformatdt();", Me.Page, Me.GetType())
        btnSaveDt.Focus()
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If tbPRNo.Text = "" Then
                lbStatus.Text = MessageDlg("PR No must have value")
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "EXEC S_PRPOServiceReff 'Service', " + QuotedStr(tbPRNo.Text) + ", " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text
            ResultField = "Product, Product_Name, Specification, Specification2, Qty, Unit, UnitOrder"
            Session("DBConnection") = ViewState("DBConnection")
            CriteriaField = "Product, Product_Name, Specification, Specification2, Qty, Unit, UnitOrder"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            'Session("filter") = "Select PR_No, PR_Date, [Type_Name], Product, Product_Name, Specification, Specification2, Qty, Unit from V_PRPOGetRequest Where RequestType = 'Goods' "
            Session("filter") = "EXEC S_PRPOOut 'Service', '','','',0"
            ViewState("CheckDlg") = False
            ResultField = "PR_No, Require_Date, Require_Partial, Product, Product_Name, Department, Department_Name, RequestBy, Specification, Specification2, Qty, Unit, UnitOrder, Product_Group, Product_Group_Name"
            CriteriaField = "PR_No, Require_Date, Require_Partial, Product, Product_Name, Specification, Specification2, Unit, UnitOrder, Product_Group, Product_Group_Name, Department, Department_Name, RequestBy"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "PR_No, Department"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search PO No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyOrderDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrderDt.TextChanged
        If ddlUnitOrderDt.SelectedValue = ddlUnitWrhsDt.SelectedValue Then
            tbQtyWrhsDt.Text = tbQtyOrderDt.Text
            AttachScript("setformatdt();", Me.Page, Me.GetType())
            tbQtyOrderDt.Focus()
        End If
    End Sub

    Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
        tbPPN.Enabled = (ddlReport.SelectedValue = "Y")
        If tbPPN.Enabled = False Then
            tbPPN.Text = "0"
        Else
            tbPPN.Text = "10"
        End If
        tbPPN.Text = FormatFloat(tbPPN.Text, ViewState("DigitQty"))
        AttachScript("setformathd('');", Me.Page, Me.GetType)
    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        ViewState("InputCurrency") = "Y"
        AttachScript("OpenMaster('MsCurrency')();", Page, Me.GetType())
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
            tbTOPRemark.Focus()
        Catch ex As Exception
            Throw New Exception("tb supplier change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPRNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPRNo.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "EXEC S_PRPOServiceReff 'Service','', " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text
            CriteriaField = "PR_NO"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            ResultField = "PR_NO, MONo, Equipment, Equipment_Name, Department, Department_Name"
            ViewState("Sender") = "btnPRNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnEquip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEquip.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "EXEC S_PRPOServiceReff 'Service', " + QuotedStr(tbPRNo.Text) + ", " + QuotedStr(tbSuppCode.Text) + " , '" + tbCode.Text + "', " + lbRevisi.Text
            CriteriaField = "Product, Product_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            ResultField = "Product, Product_Name"
            ViewState("Sender") = "btnEquip"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Equipment Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt1.Click
        'Dim dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Cleardt()

            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "EXEC S_PRPOServiceReff 'Service', " + QuotedStr(tbPRNo.Text) + ", " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text
            Session("DBConnection") = ViewState("DBConnection")
            CriteriaField = "Product, Product_Name, Specification, Qty, Unit, UnitOrder"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            ResultField = "Product, Product_Name, Specification, Qty, Unit, UnitOrder"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPriceForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForexDt.TextChanged
        Try
            tbAmountForexDt.Text = FormatFloat(tbQtyOrderDt.Text * tbPriceForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatFloat((tbQtyOrderDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            tbPriceForexDt.Text = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
            AttachScript("setformatdt();", Me.Page, Me.GetType())
            tbRemarkDt.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDiscDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscDt.TextChanged
        Try
            tbDiscForexDt.Text = FormatNumber((tbDiscDt.Text / 100) * tbAmountForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber((tbQtyOrderDt.Text * tbPriceForexDt.Text) - tbDiscForexDt.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPPHDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPHDt.TextChanged
        Try
            If ddlfgInclude.SelectedValue = "N" Then
                ViewState("FactorRate") = 1
            Else
                ViewState("FactorRate") = 100 / (100 + FormatFloat(tbPPN.Text, ViewState("DigitQty")))
            End If
            
            'tbPPHForexDt.Text = FormatNumber((tbPPHDt.Text / 100) * ViewState("FactorRate") * tbTotalForexDt.Text, ViewState("DigitCurr"))
            tbPPHForexDt.Text = FormatNumber((tbPPHDt.Text / 100) * ViewState("FactorRate") * tbTotalForexDt.Text, ViewState("DigitCurr"))
           
            ' select  306,000.00
            AttachScript("setformatdt();", Me.Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("tbPPHDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductDt.TextChanged
        Dim Dt As DataTable
        Dim SqlString As String
        Try
            Dt = SQLExecuteQuery("EXEC S_PRPOServiceReff 'Service' , " + QuotedStr(tbPRNo.Text) + ", " + QuotedStr(tbSuppCode.Text) + ", '" + tbCode.Text + "', " + lbRevisi.Text, ViewState("DBConnection")).Tables(0)
            If Not Dt Is Nothing Then
                BindToText(tbProductDt, Dt("Product").ToString)
                BindToText(tbProductNameDt, Dt("Product_Name").ToString)
                BindToText(tbSpecificationDt, Dt("Specification").ToString)
                tbQtyOrderDt.Text = FormatFloat(FindConvertUnitOrder(Dt("Product").ToString, Dt("UnitOrder").ToString, Dt("Qty").ToString, ViewState("DBConnection")), ViewState("DigitQty"))
                SqlString = "EXEC S_FindPriceSupplier " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(Dt("Product").ToString) + "," + QuotedStr(Dt("Unit").ToString) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                tbPriceForexDt.Text = FormatNumber(SQLExecuteScalar(SqlString, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
                BindToDropList(ddlUnitWrhsDt, Dt("Unit").ToString)
                BindToDropList(ddlUnitOrderDt, Dt("UnitOrder").ToString)
            Else
                tbProductDt.Text = ""
                tbProductNameDt.Text = ""
                tbSpecificationDt.Text = ""
                tbQtyWrhsDt.Text = "0"
                tbQtyOrderDt.Text = "0"
                tbPriceForexDt.Text = "0"
            End If
            tbSpecificationDt.Focus()
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
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
            Throw New Exception("ddlfgInclude_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            ModifyDt()
        Catch ex As Exception
            Throw New Exception("tbDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
