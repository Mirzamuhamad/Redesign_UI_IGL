Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrSO_TrSO
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Private Function GetStringHd(ByVal FgImp As String) As String
        Return "SELECT DISTINCT A.TransNmbr, A.Nmbr, A.Revisi, A.TransDate, A.Status, A.SOKind, A.SOType, A.ProductGroup, A.ProductGroupName, " + _
                "A.Customer, A.CustomerName, A.Attn, A.Phone, A.CustPONo, A.ContractNo, A.IRRNo, A.FgNeedDelivery, A.Delivery, " + _
                "A.DeliveryName, A.DeliveryAddr, A.DeliveryCity, A.DeliveryCityName, A.DeliveryCostBy, A.Term, A.TermRemark, " + _
                "A.FgPriceIncludeTax, A.Currency, A.ForexRate, A.BaseForex, A.Disc, A.DiscForex, A.DP, A.DPForex, A.PPn, A.PPnForex, " + _
                "A.TotalForex, A.Remark, A.UserPrep, A.DatePrep, A.UserAppr, A.DateAppr, A.FgActive, A.FactorRate, A.Department, " + _
                "A.DepartmentName, A.RemarkRevisi, A.DecPlacePrice, A.DecPlaceBaseForex " + _
                " From V_MKSOHd A " ' INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = " + QuotedStr(ViewState("UserId").ToString) + " And A.SOType <>'Service' "
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


                'lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT PR_No) from V_MKSOGetRequest Where RequestType = 'Goods' ", ViewState("DBConnection").ToString)
            End If

            'Untuk hide Qty1 dan unit 1 sampe 3
            GridDt.Columns(9).Visible = False
            GridDt.Columns(10).Visible = False
            GridDt.Columns(11).Visible = False
            GridDt.Columns(12).Visible = False
            GridDt.Columns(13).Visible = False
            GridDt.Columns(14).Visible = False
            '===========================
            If Not Session("PostNmbr") Is Nothing Then
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) = '" + Session("PostNmbr") + "'")
                'lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT PR_No) from V_MKSOGetRequest Where RequestType = 'Goods' ", ViewState("DBConnection").ToString)
                Session("PostNmbr") = Nothing

            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCustomer" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    tbCustName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                    BindToDropList(ddlCurrHd, Session("Result")(3).ToString)
                    BindToDropList(ddlTerm, Session("Result")(4).ToString)
                    BindToText(tbPhone, Session("Result")(5).ToString)
                    'BindToDropList(ddlfgInclude, Session("Result")(6).ToString)
                    ddlTerm_SelectedIndexChanged(Nothing, Nothing)
                    tbCustCode_TextChanged(Nothing, Nothing)
                    'ddlfgInclude_SelectedIndexChanged(Nothing, Nothing)
                    'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    'tbRateHd.Enabled = False
                ElseIf ViewState("Sender") = "btnProductDt" Then
                    'Product, Product_Name, Specification, Specification2, Unit, Price
                    tbProductDt.Text = Session("Result")(0).ToString
                    tbProductNameDt.Text = Session("Result")(1).ToString
                    tbSpecificationDt.Text = Session("Result")(2).ToString
                    tbSpecificationDt2.Text = Session("Result")(3).ToString
                    BindToDropList(ddlUnitWrhsDt, Session("Result")(4).ToString)
                    BindToDropList(ddlUnitCommision, Session("Result")(4).ToString)
                    tbPriceForexDt.Text = FormatNumber(Session("Result")(5).ToString, ViewState("DigitCurr"))
                    BindToDropList(ddlUnitOrderDt, Session("Result")(4).ToString)
                ElseIf ViewState("Sender") = "btnProductGroup" Then
                    tbProductGroup.Text = Session("Result")(0).ToString
                    tbProductGroupName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnDelivery" Then
                    tbdelivery.Text = Session("Result")(0).ToString
                    tbdeliveryName.Text = Session("Result")(1).ToString
                    tbAddress.Text = Session("Result")(2).ToString + "" + Session("Result")(3).ToString
                    BindToDropList(ddlCity, Session("Result")(4).ToString)
                    'ElseIf ViewState("Sender") = "btnRequestPR" Then
                    '    tbRequestPR.Text = Session("Result")(0).ToString
                    '    tbProductPR.Text = Session("Result")(1).ToString
                    '    tbProductNamePR.Text = Session("Result")(2).ToString
                    '    BindToText(tbSpecificationPR, Session("Result")(3).ToString)
                    '    BindToText(tbQtyPR, Session("Result")(4).ToString)
                    '    BindToDropList(ddlUnitPR, Session("Result")(5).ToString)
                    '    BindToText(txUnitOrder, Session("Result")(6).ToString)
                ElseIf ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()

                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Specification2") = TrimStr(drResult("Specification2"))
                            dr("QtyOrder") = FormatFloat(0, ViewState("DigitQty"))
                            dr("Qty") = FormatFloat(0, ViewState("DigitQty"))
                            dr("QtyM2") = FormatFloat(0, ViewState("DigitQty"))
                            dr("QtyRoll") = FormatFloat(0, ViewState("DigitQty"))
                            dr("Unit") = drResult("Unit")
                            dr("UnitOrder") = drResult("UnitOrder")
                            dr("UnitPack") = drResult("UnitPack")
                            dr("UnitM2") = drResult("UnitM2")
                            dr("PriceForex") = FormatNumber(drResult("Price"), ViewState("DigitCurr"))
                            dr("AmountForex") = FormatNumber(0, ViewState("DigitCurr"))
                            dr("Disc") = FormatFloat(0, ViewState("DigitQty"))
                            dr("DiscForex") = FormatNumber(0, ViewState("DigitCurr"))
                            dr("NettoForex") = FormatNumber(0, ViewState("DigitCurr"))
                            dr("FgPartialDelivery") = "Y"
                            dr("DeliveryDate") = tbDate.SelectedValue
                            dr("CommisionForex") = FormatNumber(0, ViewState("DigitCurr"))
                            dr("CommisionUnit") = drResult("Unit")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'ModifyDt()
                    'ViewState("Dt2") = Nothing
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If


            If Not ViewState("ProductClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_MKSOClosing " + QuotedStr(tbCode.Text) + "," + lbRevisi.Text + "," + QuotedStr(ViewState("ProductClose").ToString) + "," + QuotedStr(HiddenRemarkClose.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
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
    End Sub
    Private Sub SetInit()
        Try
            If Request.QueryString("ContainerId").ToString = "SOID" Then
                lblJudul.Text = "Sales Order"
                ViewState("FgImp") = "N"
            End If
            If Request.QueryString("ContainerId").ToString = "SOExpID" Then
                lblJudul.Text = "SO Export"
                ViewState("FgImp") = "Y"
            End If
            FillRange(ddlRange)
            FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))

            FillCombo(ddlCurrHd, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrderDt2, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitSub, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitWrhsDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitOrderDt, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlUnitCommision, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            FillCombo(ddlCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitPackDt2, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 2
            'ViewState("DigitCurr") = -1
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
                ddlCommand.Items.Add("Print 2")
                ddlCommand2.Items.Add("Print 2")
            End If

            tbPPNForex.Attributes.Add("ReadOnly", "True")
            tbBaseForex.Attributes.Add("ReadOnly", "True")
            tbDiscForex.Attributes.Add("ReadOnly", "True")
            'tbDiscForexDt.Attributes.Add("ReadOnly", "True")
            'tbDPForex.Attributes.Add("ReadOnly", "True")
            tbQtyM2Dt.Attributes.Add("ReadOnly", "True")
            tbQtyRollDt.Attributes.Add("ReadOnly", "True")
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
            tbPPNForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbOtherForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyM2Dt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyRollDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyWrhsDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrderDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDiscForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyOrderDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtySub.Attributes.Add("OnKeyDown", "return PressNumeric();")


            'tbRateHd.Attributes.Add("OnBlur", "setformathd('');")
            tbDP.Attributes.Add("OnBlur", "setformathd('DP');")
            tbDPForex.Attributes.Add("OnBlur", "setformathd('DPForex');")
            tbBaseForex.Attributes.Add("OnBlur", "setformathd('BaseForex');")
            'tbDisc.Attributes.Add("OnBlur", "setformathd();")
            tbDiscForex.Attributes.Add("OnBlur", "setformathd('');")
            tbPPN.Attributes.Add("OnBlur", "setformathd('');")
            tbPPNForex.Attributes.Add("OnBlur", "setformathd('');")
            tbTotalForex.Attributes.Add("OnBlur", "setformathd('');")
            'tbOtherForex.Attributes.Add("OnBlur", "setformathd();")

            tbQtyM2Dt.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyRollDt.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyWrhsDt.Attributes.Add("OnBlur", "setformatdt();")
            tbQtyOrderDt.Attributes.Add("OnBlur", "setformatdt();")
            tbPriceForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbDiscDt.Attributes.Add("OnBlur", "setformatdt();")
            tbDiscForexDt.Attributes.Add("OnBlur", "setformatdt();")
            tbTotalForexDt.Attributes.Add("OnBlur", "setformatdt();")

            tbQtyOrderDt2.Attributes.Add("OnBlur", "setformatdt2();")
            tbQtySub.Attributes.Add("OnBlur", "setformatDtSub();")
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
    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKSODt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKSODt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function
    Private Function GetStringDtSub(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKSODtSub WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
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
                Session("SelectCommand") = "EXEC S_MKFormSO " + Result + ", " + QuotedStr(ViewState("UserId"))

                Session("ReportFile") = ".../../../Rpt/FormSO.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            ElseIf ActionValue = "Print 2" Then
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
                Session("SelectCommand") = "EXEC S_MKFormSO " + Result + ", " + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormSO2.frx"
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

                        '    DT = ExecSPCekPosting("S_MKSOPostCek", Nmbr(j), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '    If (DT.Rows.Count > 0) Then
                        '        If (DT.Rows.Count > 0) And (DT.Rows(0)("NewPage").ToString = "Y") Then
                        '            Try
                        '                If ViewState("FgImp") = "N" Then
                        '                    ViewState("FgCekPOST") = "SO"
                        '                Else
                        '                    ViewState("FgCekPOST") = "SOExp"
                        '                End If
                        '                ViewState("DTPost") = DT
                        '                Session("PostNmbr") = Nmbr(j)
                        '                Response.Redirect("..\..\Transaction\TrPRReq\FormCekPostPR.aspx")

                        '                'AttachScript("opencekpostingdlg();", Page, Me.GetType)
                        '            Catch ex As Exception
                        '                lbStatus.Text = "opencekpostingdlg Error = " + ex.ToString
                        '            End Try
                        '        Else
                        '            Result = ExecSPCommandGo(ActionValue, "S_MKSO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '            If Trim(Result) <> "" Then
                        '                lbStatus.Text = lbStatus.Text + Result + "<br />"
                        '            End If
                        '        End If
                        '    Else

                        '        Result = ExecSPCommandGo(ActionValue, "S_MKSO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        '        If Trim(Result) <> "" Then
                        '            lbStatus.Text = lbStatus.Text + Result + "<br />"
                        '        End If
                        '    End If
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_MKSO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

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
            tbCustCode.Enabled = State
            btnCust.Enabled = State
            ddlSOType.Enabled = State
            ddlfgInclude.Enabled = State
            'btnGetData.Enabled = State
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
    Private Sub BindDataDtSub(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("DtSub") = Nothing
            dt = SQLExecuteQuery(GetStringDtSub(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtSub") = dt
            BindGridDt(dt, GridSub)
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
            MovePanel(pnlEditDtSub, pnlDtSub)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel PR Error : " + ex.ToString
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

            If ViewState("DtSub") Is Nothing Then
                piar = False
            Else
                piar = ViewState("DtSub").Rows.Count > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtSub").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function


    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            lbRevisi.Text = "0"
            ddlSOKind.SelectedIndex = 0
            ddlSOType.SelectedIndex = 0
            ddlDept.SelectedValue = ""
            ddlfgInclude.SelectedValue = "N"
            tbProductGroup.Text = ""
            tbProductGroupName.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbAttn.Text = ""
            tbPhone.Text = ""
            tbCustPONo.Text = ""
            tbContractNo.Text = ""
            tbIRRNo.Text = ""
            ddlNeedDelivery.SelectedIndex = 0
            tbdelivery.Text = ""
            tbdeliveryName.Text = ""
            tbAddress.Text = ""
            ddlCostBy.Text = ""
            tbBaseForex.Text = 0
            'tbDisc.Text = "0"
            tbDiscForex.Text = "0"
            tbTotalForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            ddlCurrHd.SelectedValue = ViewState("Currency")
            ddlTerm.SelectedIndex = 0
            tbTermRemark.Text = ""
            tbRemark.Text = ""
            lbRemarkRevisi.Text = ""
            tbDate.SelectedDate = Now.Date
            tbDP.Text = FormatFloat("0", ViewState("DigitQty"))
            tbDPForex.Text = "0"
            ViewState("FactorRate") = 1
            'ddlfgInclude.SelectedValue = "Y"
            tbPriceDec.Text = "0"
            tbTotalDec.Text = "0"

            'tbPPN.Enabled = False
            ddlfgInclude_SelectedIndexChanged(Nothing, Nothing) 'pake dl

            'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateHd.Enabled = False

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
            tbSpecificationDt2.Text = ""
            tbQtyM2Dt.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyRollDt.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyWrhsDt.Text = FormatFloat("0", ViewState("DigitQty"))
            tbQtyOrderDt.Text = FormatFloat("0", ViewState("DigitQty"))
            tbPriceForexDt.Text = "0"
            tbAmountForexDt.Text = "0"
            tbDiscDt.Text = "0"
            tbDiscForexDt.Text = "0"
            tbTotalForexDt.Text = "0"
            tbcommision.Text = "0"
            ddlUnitCommision.SelectedValue = ""
            tbDeliveryDate.SelectedDate = Now.Date
            ddlPartial.SelectedIndex = 0
            ddlUnitOrderDt.SelectedIndex = 0
            lbUnitPack.Text = ""
            lbUnitM2.Text = ""
            tbDeliveryDate.SelectedDate = ViewState("ServerDate") 'Now.Date
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbDeliveryDateDt2.SelectedDate = Date.Today
            tbQtyOrderDt2.Text = "0"
            tbQtyPackDt2.Text = "0"
            ddlUnitOrderDt2.SelectedIndex = 0
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub ClearDtSub()
        Try
            Lbitem.Text = "0"
            tbDetailRemark.Text = ""
            tbQtySub.Text = "0"
            ddlUnitSub.SelectedIndex = 0
            tbPriceSub.Text = "0"
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
            'If tbIRRNo.Text.Trim = "" And ddlSOKind.SelectedIndex = 1 Then
            '    lbStatus.Text = MessageDlg("IRR No must have value")
            '    btnIRRNo.Focus()
            '    Return False
            'End If
            If tbProductGroup.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Product Group must have value")
                tbProductGroup.Focus()
                Return False
            End If
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustCode.Focus()
                Return False
            End If
            If tbCustPONo.Text.Trim = "" And ddlSOKind.SelectedIndex = 0 Then
                lbStatus.Text = MessageDlg("PO Cust No must have value")
                tbCustPONo.Focus()
                Return False
            End If
            If tbContractNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Contract No must have value")
                tbContractNo.Focus()
                Return False
            End If
            If tbdelivery.Text.Trim = "" And ddlNeedDelivery.SelectedValue = "Y" Then
                lbStatus.Text = MessageDlg("Delivery Place must have value")
                tbdelivery.Focus()
                Return False
            End If
            If ddlCostBy.SelectedValue.Trim = "" And ddlNeedDelivery.SelectedValue = "Y" Then
                lbStatus.Text = MessageDlg("Delivery Cost by must have value")
                ddlCostBy.Focus()
                Return False
            End If
            If tbTermRemark.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Term Remark must have value")
                tbTermRemark.Focus()
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
                If Dr("Product").ToString = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Exit Function
                End If
                If ddlSOKind.SelectedValue <> "Sample" Then
                    If Dr("PriceForex").ToString = "0" Then
                        lbStatus.Text = MessageDlg("Price Must Have Value")
                        Return False
                    End If
                End If
                If Dr("CommisionUnit").ToString = "" Then
                    lbStatus.Text = MessageDlg("Commision Unit Must Have Value")
                    Return False
                End If
            Else
                If (tbProductDt.Text) = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If ddlSOKind.SelectedValue <> "Sample" Then
                    If CFloat(tbPriceForexDt.Text) <= 0 Then
                        lbStatus.Text = MessageDlg("Price Must Have Value")
                        Return False
                    End If
                End If
                If ddlUnitCommision.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Commision Unit Must Have Value")
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
                If Dr("Qty").ToString = "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
            Else
                If tbDeliveryDateDt2.SelectedValue = Nothing Then
                    lbStatus.Text = MessageDlg("Delivery Date Must Have Value")
                    tbDeliveryDateDt2.Focus()
                    Return False
                End If
                If CFloat(tbQtyOrderDt2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQtyOrderDt2.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Function CekDtSub(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
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
                Row("Specification2") = tbSpecificationDt2.Text
                Row("QtyOrder") = FormatFloat(tbQtyOrderDt.Text, ViewState("DigitQty"))
                Row("UnitOrder") = ddlUnitOrderDt.SelectedValue
                Row("Qty") = FormatFloat(tbQtyWrhsDt.Text, ViewState("DigitQty"))
                Row("Unit") = ddlUnitWrhsDt.SelectedValue
                Row("QtyM2") = FormatFloat(tbQtyM2Dt.Text, ViewState("DigitQty"))
                Row("QtyRoll") = FormatFloat(tbQtyRollDt.Text, ViewState("DigitQty"))
                Row("UnitPerRoll") = FormatFloat(tbQtyPerRoll.Text, ViewState("DigitQty"))
                Row("UnitPerM2") = FormatFloat(tbQtyPerM2.Text, ViewState("DigitQty"))
                Row("PriceForex") = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
                Row("AmountForex") = FormatNumber(tbAmountForexDt.Text, ViewState("DigitCurr"))
                Row("Disc") = FormatNumber(tbDiscDt.Text, ViewState("DigitPercent"))
                Row("DiscForex") = FormatNumber(tbDiscForexDt.Text, ViewState("DigitCurr"))
                Row("NettoForex") = FormatNumber(tbTotalForexDt.Text, ViewState("DigitCurr"))
                Row("CommisionForex") = FormatNumber(tbcommision.Text, ViewState("DigitCurr"))
                Row("CommisionUnit") = ddlUnitCommision.SelectedValue
                Row("FgPartialDelivery") = "Y" 'ddlPartial.SelectedValue
                Row("DeliveryDate") = tbDeliveryDate.SelectedDate
                Row("Remark") = tbRemarkDt.Text
                Row("UnitPack") = lbUnitPack.Text
                Row("UnitM2") = lbUnitM2.Text
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
                dr("Specification2") = tbSpecificationDt2.Text
                dr("QtyOrder") = FormatFloat(tbQtyOrderDt.Text, ViewState("DigitQty"))
                dr("UnitOrder") = ddlUnitOrderDt.SelectedValue
                dr("Qty") = FormatFloat(tbQtyWrhsDt.Text, ViewState("DigitQty"))
                dr("Unit") = ddlUnitWrhsDt.SelectedValue
                dr("QtyM2") = FormatFloat(tbQtyM2Dt.Text, ViewState("DigitQty"))
                dr("QtyRoll") = FormatFloat(tbQtyRollDt.Text, ViewState("DigitQty"))
                dr("UnitPerRoll") = FormatFloat(tbQtyPerRoll.Text, ViewState("DigitQty"))
                dr("UnitPerM2") = FormatFloat(tbQtyPerM2.Text, ViewState("DigitQty"))
                dr("PriceForex") = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
                dr("AmountForex") = FormatNumber(tbAmountForexDt.Text, ViewState("DigitCurr"))
                dr("Disc") = FormatNumber(tbDiscDt.Text, ViewState("DigitCurr"))
                dr("DiscForex") = FormatNumber(tbDiscForexDt.Text, ViewState("DigitCurr"))
                dr("NettoForex") = FormatNumber(tbTotalForexDt.Text, ViewState("DigitCurr"))
                dr("CommisionForex") = FormatNumber(tbcommision.Text, ViewState("DigitCurr"))
                dr("CommisionUnit") = ddlUnitCommision.SelectedValue
                dr("FgPartialDelivery") = "Y" 'ddlPartial.SelectedValue
                dr("DeliveryDate") = tbDeliveryDate.SelectedDate
                dr("Remark") = tbRemarkDt.Text
                dr("UnitPack") = lbUnitPack.Text
                dr("UnitM2") = lbUnitM2.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(Not DtExist())
            'ModifyDt()
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
            If CekDtSub() = False Then
                btnSavePR.Focus()
                Exit Sub
            End If

            Dim ExistRow As DataRow()
            ExistRow = ViewState("DtSub").Select("Product = " + QuotedStr(lbProductSub.Text) + " AND ItemNo = " + QuotedStr(Lbitem.Text))

            If ViewState("StateDtSub") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordPR() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("DtSub").Select("Product = " + QuotedStr(ViewState("ProductPR")) + " AND ItemNo = " + QuotedStr(ViewState("RequestPR")))(0)

                Row.BeginEdit()
                Row("Product") = lbProductSub.Text
                Row("Remark") = tbDetailRemark.Text
                Row("Qty") = FormatFloat(tbQtySub.Text, ViewState("DigitQty"))
                Row("Unit") = ddlUnitSub.SelectedValue
                Row("Price") = FormatNumber(tbPriceSub.Text, ViewState("DigitCurr"))
                Row.EndEdit()
            Else
                Dim dr As DataRow

                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("DtSub").NewRow
                dr("Product") = lbProductSub.Text
                dr("ItemNo") = Lbitem.Text
                dr("Remark") = tbDetailRemark.Text
                dr("Qty") = FormatFloat(tbQtySub.Text, ViewState("DigitQty"))
                dr("Unit") = ddlUnitSub.SelectedValue
                dr("Price") = FormatNumber(tbPriceSub.Text, ViewState("DigitCurr"))
                ViewState("DtSub").Rows.Add(dr)
            End If
            MovePanel(pnlEditDtSub, pnlDtSub)
            EnableHd(Not DtExist())
            'BindGridDt(ViewState("DtSub"), GridSub)
            Dim drow As DataRow()
            drow = ViewState("DtSub").Select("Product=" + QuotedStr(lbProductSub.Text.Trim))
            BindGridDt(drow.CopyToDataTable, GridSub)
            'ModifyDt()

            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save PR Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim Row2 As DataRow
        Dim SqlString As String
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text.Trim) + " and Delivery = " + QuotedStr(Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")))
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text.Trim) + " and Delivery = " + QuotedStr(ViewState("Delivery")))(0)

                Row.BeginEdit()
                Row("Product") = lbProductSub.Text
                Row("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")
                Row("QtyOrder") = tbQtyOrderDt2.Text
                Row("UnitOrder") = ddlUnitOrderDt2.SelectedValue
                Row("QtyPack") = tbQtyPackDt2.Text
                Row("UnitPack") = ddlUnitPackDt2.SelectedValue
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
                dr("Product") = lbProductSub.Text
                dr("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy")
                dr("QtyOrder") = tbQtyOrderDt2.Text
                dr("UnitOrder") = ddlUnitOrderDt2.SelectedValue
                dr("QtyPack") = tbQtyPackDt2.Text
                dr("UnitPack") = ddlUnitPackDt2.SelectedValue
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(Not DtExist())
            'BindGridDt(ViewState("Dt2"), GridDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text.Trim))
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
            'ModifyDt()

            If ViewState("StateHd") <> "View" Then
                Row2 = ViewState("Dt").Select("Product = " + QuotedStr(lbProductSub.Text))(0)
                Row2.BeginEdit()

                'dtUnit = SQLExecuteQuery("Select Unit, UnitWrhs from VMsProductConvert WHERE ProductCode = " + QuotedStr(lbProductSub.Text), ViewState("DBConnection")).Tables(0)
                dtUnit = SQLExecuteQuery("Select UnitPerM2, UnitPerRoll from VMsProduct WHERE Product_Code = " + QuotedStr(lbProductSub.Text), ViewState("DBConnection")).Tables(0)
                drUnit = dtUnit.Rows(0)

                tbQtyPerRoll.Text = drUnit("UnitPerRoll")
                tbQtyPerM2.Text = drUnit("UnitPerM2")

                Dim Qty, QtyPack, QtyM2 As Double
                Row2("QtyOrder") = FormatFloat(CFloat(lbQtyOrderDt2.Text.Replace(",", "")), ViewState("DigitQty"))
                Qty = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(Row2("Product").ToString) + ", " + QuotedStr(Row2("Unit")) + ", " + lbQtyOrderDt2.Text.Replace(",", "") + ", " + QuotedStr(Row2("UnitOrder").ToString) + " )", ViewState("DBConnection"))
                Row2("Qty") = FormatFloat(Qty, ViewState("DigitQty"))

                QtyPack = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(Row2("Product").ToString) + ", " + QuotedStr(Row2("UnitPack").ToString) + ", " + lbQtyOrderDt2.Text.Replace(",", "") + ", " + QuotedStr(Row2("UnitOrder").ToString) + " )", ViewState("DBConnection"))
                Row2("QtyRoll") = FormatFloat(QtyPack, ViewState("DigitQty"))

                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(Row2("Product").ToString) + ", " + QuotedStr(Row2("UnitM2").ToString) + ", " + lbQtyOrderDt2.Text.Replace(",", "") + ", " + QuotedStr(Row2("UnitOrder").ToString) + " )", ViewState("DBConnection"))
                Row2("QtyM2") = FormatFloat(QtyM2, ViewState("DigitQty"))

                SqlString = "EXEC S_MKSOGetPrice " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(lbProductSub.Text) + "," + QuotedStr(Row2("UnitOrder").ToString)
                Row2("PriceForex") = FormatNumber(SQLExecuteScalar(SqlString, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))

                If Row2("QtyRoll") = "Infinity" Then Row2("QtyRoll") = FormatFloat(0, ViewState("DigitQty"))
                If Row2("QtyM2") = "Infinity" Then Row2("QtyM2") = FormatFloat(0, ViewState("DigitQty"))

                'If CFloat(drUnit("UnitPerRoll")) = 0 Then
                '    Row2("QtyRoll") = FormatFloat(0, ViewState("DigitQty"))
                'Else
                '    Row2("QtyRoll") = FormatFloat(CFloat(lbQtyOrderDt2.Text) / CFloat(drUnit("UnitPerRoll")), ViewState("DigitQty"))
                'End If

                Row2("AmountForex") = FormatNumber(lbQtyOrderDt2.Text * CFloat(Row2("PriceForex")), ViewState("DigitCurr"))
                Row2("NettoForex") = FormatNumber(Row2("AmountForex") - Row2("DiscForex"), ViewState("DigitCurr"))
                Row2.EndEdit()
                totalingDt()
                BindGridDt(ViewState("Dt"), GridDt)

            End If
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
        Dim total, disc As Double
        Try
            total = 0
            disc = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    total = total + CFloat(dr("AmountForex").ToString)
                    disc = disc + CFloat(dr("DiscForex").ToString)
                End If
            Next

            tbDiscForex.Text = FormatNumber(disc * ViewState("FactorRate"), CInt(ViewState("DigitCurr")))
            tbBaseForex.Text = FormatNumber(total * ViewState("FactorRate"), CInt(ViewState("DigitCurr")))
            'If ddlCurrHd.SelectedValue = ViewState("Currency") Then
            '    total = Math.Floor(total)
            'End If
            'tbBaseForex.Text = FormatNumber(total, CInt(ViewState("DigitCurr")))
            'If ddlfgInclude.SelectedValue = "N" Then
            '    tbBaseForex.Text = FormatNumber(total + CFloat(tbDiscForex.Text), CInt(ViewState("DigitCurr")))
            'Else
            '    '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
            '    tbBaseForex.Text = FormatNumber((total * 100 / (100 + CFloat(tbPPN.Text))) + CFloat(tbDiscForex.Text), CInt(ViewState("DigitCurr")))
            'End If
            tbPPNForex.Text = FormatNumber((CFloat(tbPPN.Text) / 100) * (CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text)), ViewState("DigitCurr"))
            tbTotalForex.Text = FormatNumber(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) + CFloat(tbPPNForex.Text), CInt(ViewState("DigitCurr")))
            tbDPForex.Text = FormatNumber((CFloat(tbDP.Text) / 100) * (CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text)), ViewState("DigitCurr"))
            'AttachScript("setformathd();", Page, Me.GetType())

        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyDt()
        Dim DtSub, Dt As DataTable
        Dim DR, CurrRow As DataRow
        Dim SelectRow As DataRow()
        Dim i, len As Integer
        Dim sqlstring As String
        Dim QtyWrhs As Double
        Try
            DtSub = ViewState("DtSub")
            Dt = ViewState("Dt")
            len = Dt.Rows.Count
            For i = 0 To len - 1
                If Not ViewState("Dt").Rows(i).RowState = DataRowState.Deleted Then
                    ViewState("Dt").Rows(i).BeginEdit()
                    ViewState("Dt").Rows(i)("Qty") = 0
                    ViewState("Dt").Rows(i).EndEdit()

                End If
            Next
            For Each DR In DtSub.Rows
                If Not DR.RowState = DataRowState.Deleted Then
                    SelectRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))

                    If SelectRow.Length = 0 Then
                        Dim dtUnit As DataTable
                        Dim drUnit As DataRow
                        dtUnit = SQLExecuteQuery("Select UnitPerM2, UnitPerRoll from VMsProduct WHERE Product_Code = " + QuotedStr(DR("Product")), ViewState("DBConnection")).Tables(0)
                        drUnit = dtUnit.Rows(0)

                        CurrRow = ViewState("Dt").NewRow
                        CurrRow("Qty") = DR("Qty")
                        CurrRow("Unit") = DR("Unit")
                        CurrRow("UnitPerRoll") = drUnit("UnitPerRoll")
                        CurrRow("UnitPerM2") = drUnit("UnitPerM2")


                        Dim QtyRoll, QtyM2 As Double
                        If UCase(CurrRow("Unit")) = "ROLL" Then
                            'Row2("QtyRoll") = FormatFloat(CFloat(lbQtyOrderDt2.Text), ViewState("DigitQty"))
                            QtyRoll = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(CurrRow("Product").ToString) + ", " + CurrRow("Unit") + ", " + lbQtyOrderDt2.Text.Replace(",", ""), ViewState("DBConnection"))
                            CurrRow("QtyRoll") = FormatFloat(QtyRoll, ViewState("DigitQty"))

                        ElseIf UCase(CurrRow("Unit")) = "M2" Then
                            QtyM2 = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(CurrRow("Product").ToString) + ", " + CurrRow("Unit") + ", " + lbQtyOrderDt2.Text.Replace(",", ""), ViewState("DBConnection"))
                            CurrRow("QtyM2") = FormatFloat(QtyM2, ViewState("DigitQty"))
                        Else
                            If CFloat(drUnit("UnitPerRoll")) = 0 Then
                                CurrRow("QtyRoll") = FormatFloat(0, ViewState("DigitQty"))
                            Else
                                CurrRow("QtyRoll") = FormatFloat(CFloat(DR("Qty")) / CFloat(drUnit("UnitPerRoll")), ViewState("DigitQty"))
                            End If
                            If CFloat(drUnit("UnitPerM2")) = 0 Then
                                CurrRow("QtyM2") = FormatFloat(0, ViewState("DigitQty"))
                            Else
                                CurrRow("QtyM2") = FormatFloat(CFloat(DR("Qty")) / CFloat(drUnit("UnitPerM2")), ViewState("DigitQty"))
                            End If
                        End If


                        'Dim QtyResult As Double
                        'QtyResult = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(lbProductSub.Text) + ", " + QuotedStr(ddlUnitOrderDt.SelectedValue) + ", " + tbQtyDt.Text.Replace(",", ""), ViewState("DBConnection"))

                        sqlstring = "EXEC S_MKSOGetPrice " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("UnitOrder").ToString)
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                        CurrRow("Price") = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
                        ViewState("Dt").Rows.Add(CurrRow)
                    Else
                        CurrRow = ViewState("Dt").Select("Product =" + QuotedStr(DR("Product")))(0)
                        QtyWrhs = CFloat(CurrRow("Qty").ToString) + CFloat(DR("Qty").ToString)
                        'QtyOrder = SQLExecuteScalar("EXEC S_MKSOGetPrice " + QuotedStr(CurrRow("Product").ToString) + ", " + QuotedStr(CurrRow("UnitOrder").ToString) + ", " + QtyWrhs.ToString.Replace(",", ""), ViewState("DBConnection"))
                        CurrRow.BeginEdit()

                        CurrRow("Qty") = FormatFloat(QtyWrhs, ViewState("DigitQty"))
                        CurrRow("QtyOrder") = FormatFloat(QtyWrhs, ViewState("DigitQty"))
                        If CFloat(CurrRow("UnitPerRoll")) <> 0 Then
                            CurrRow("QtyRoll") = FormatFloat(CFloat(CurrRow("Qty").ToString) / CFloat(CurrRow("UnitPerRoll").ToString), ViewState("DigitQty"))
                        End If
                        If CFloat(CurrRow("UnitPerM2")) <> 0 Then
                            CurrRow("QtyM2") = FormatFloat(CFloat(CurrRow("Qty").ToString) / CFloat(CurrRow("UnitPerM2").ToString), ViewState("DigitQty"))
                        End If
                        CurrRow.EndEdit()
                    End If
                End If
            Next

            For i = 0 To GetCountRecord(ViewState("Dt")) - 1
                DR = ViewState("Dt").Rows(i)
                If Not ViewState("Dt").Rows(i).RowState = DataRowState.Deleted Then

                    'untuk hapus dt2 ketika hapus DtPR
                    If DR("Qty") <> 0 Then

                        ' DR.Delete()
                        ' Exit For
                        'SelectRow = ViewState("Dt").Select("Product = " + QuotedStr(DR("Product"))) - 1

                        'For len = 0 To SelectRow.Length - 1
                        '    SelectRow(len).Delete()
                        'Next
                        'Dim dr2 As DataRow()
                        'dr2 = ViewState("Dt2").Select("Product = " + QuotedStr(Product) + " and PRNo = " + QuotedStr(PRNo))
                        'For j As Integer = 0 To (dr2.Count - 1)
                        '    dr2(j).Delete()
                        'Next
                    Else
                        'untuk edit price jika sudah ada isi
                        If CFloat(DR("UnitPerRoll")) <> 0 Then
                            DR("QtyRoll") = FormatFloat(CFloat(DR("Qty").ToString) / CFloat(DR("UnitPerRoll").ToString), ViewState("DigitQty"))
                        End If
                        If CFloat(DR("UnitPerM2")) <> 0 Then
                            DR("QtyM2") = FormatFloat(CFloat(DR("Qty").ToString) / CFloat(DR("UnitPerM2").ToString), ViewState("DigitQty"))
                        End If
                        sqlstring = "EXEC S_MKSOGetPrice " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(DR("Product").ToString) + "," + QuotedStr(DR("UnitOrder").ToString)
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
                        DR("PriceForex") = FormatNumber(SQLExecuteScalar(sqlstring, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))
                        'DR("BrutoForex") = FormatNumber(CFloat(DR("Qty").ToString) * CFloat(DR("PriceForex").ToString), ViewState("DigitCurr"))
                        'DR("NettoForex") = DR("BrutoForex")
                    End If
                End If

            Next
            BindGridDt(ViewState("Dt"), GridDt)
            totalingDt()
            'lbTotalSisa.Text = CFloat(lbTotalPR.Text) - CFloat(lbQtyPO.Text)

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
            If ViewState("ProductPR") = lbProductSub.Text And ViewState("RequestPR") = Lbitem.Text Then
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
            If ViewState("Delivery") = Format(tbDeliveryDateDt2.SelectedDate, "dd MMM yyyy") Then
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

            If ViewState("StateHd") = "Insert" Then
                If ddlSOType.SelectedIndex = 0 Then
                    ViewState("StrType") = "SOL"
                Else
                    ViewState("StrType") = "SOE"
                End If
                'tbCode.Text = GetAutoNmbr(ViewState("StrType"), "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), tbProductGroup.Text, ViewState("DBConnection").ToString)
                tbCode.Text = GetAutoNmbr(ViewState("StrType"), "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlDept.SelectedValue, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO MKTSOHd (TransNmbr, Revisi, TransDate, STATUS, SOKind, SOType, ProductGroup, " + _
                "Customer, Department, Attn, Phone, Term, TermRemark, FgPriceIncludeTax, CustPONo, ContractNo, IRRNo, FgNeedDelivery, DeliveryCode, " + _
                "DeliveryAddr, DeliveryCity, DeliveryCostBy, Currency, ForexRate, BaseForex, DiscForex, DP, DPForex, PPn, " + _
                "PPNForex, TotalForex, Remark, UserPrep, " + _
                "DatePrep, FgActive, FactorRate," + _
                "DecPlacePrice, DecPlaceBaseForex)" + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 0, " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlSOKind.SelectedValue) + ", " + QuotedStr(ddlSOType.SelectedValue) + ", " + QuotedStr(tbProductGroup.Text) + ", " + QuotedStr(tbCustCode.Text) + ", " + _
                QuotedStr(ddlDept.SelectedValue) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbPhone.Text) + ", " + QuotedStr(ddlTerm.SelectedValue) + ", " + QuotedStr(tbTermRemark.Text) + ", " + QuotedStr(ddlfgInclude.SelectedValue) + _
                ", " + QuotedStr(tbCustPONo.Text) + ", " + QuotedStr(tbContractNo.Text) + ", " + QuotedStr(tbIRRNo.Text) + _
                ", " + QuotedStr(ddlNeedDelivery.SelectedValue) + ", " + QuotedStr(tbdelivery.Text) + _
                ", " + QuotedStr(tbAddress.Text) + ", " + QuotedStr(ddlCity.SelectedValue) + ", " + QuotedStr(ddlCostBy.SelectedValue) + ", " + _
                QuotedStr(ddlCurrHd.SelectedValue) + ", 0, " + tbBaseForex.Text.Replace(",", "") + ", " + _
                tbDiscForex.Text.Replace(",", "") + ", " + tbDP.Text.Replace(",", "") + _
                ", " + tbDPForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + _
                ", " + tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y'" + _
                ", " + QuotedStr(ViewState("FactorRate")) + _
                ", " + tbPriceDec.Text + ", " + tbTotalDec.Text
                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = "0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MKTSOHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MKTSOHd SET SOKind =" + QuotedStr(ddlSOKind.SelectedValue) + ", ProductGroup=" + QuotedStr(tbProductGroup.Text) + ", Department = " + QuotedStr(ddlDept.SelectedValue) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", SOType=" + QuotedStr(ddlSOType.SelectedValue) + ", Customer=" + QuotedStr(tbCustCode.Text) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", Phone=" + QuotedStr(tbPhone.Text) + ", Term=" + QuotedStr(ddlTerm.Text) + ", Termremark= " + QuotedStr(tbTermRemark.Text) + _
                ", FgPriceIncludeTax= " + QuotedStr(ddlfgInclude.SelectedValue) + _
                ", CustPONo= " + QuotedStr(tbCustPONo.Text) + ", ContractNo= " + QuotedStr(tbContractNo.Text) + _
                ", IRRNo= " + QuotedStr(tbIRRNo.Text) + ", FgNeedDelivery= " + QuotedStr(ddlNeedDelivery.SelectedValue) + _
                ", DeliveryCode=" + QuotedStr(tbdelivery.Text) + ", DeliveryAddr=" + QuotedStr(tbAddress.Text) + _
                ", DeliveryCity=" + QuotedStr(ddlCity.SelectedValue) + ", DeliveryCostBy=" + QuotedStr(ddlCostBy.SelectedValue) + _
                ", Currency=" + QuotedStr(ddlCurrHd.SelectedValue) + _
                ", BaseForex=" + tbBaseForex.Text.Replace(",", "") + _
                ", DiscForex=" + tbDiscForex.Text.Replace(",", "") + _
                ", DP=" + tbDP.Text.Replace(",", "") + ", DPForex=" + tbDPForex.Text.Replace(",", "") + _
                ", PPn=" + tbPPN.Text.Replace(",", "") + ", PPNForex=" + tbPPNForex.Text.Replace(",", "") + _
                ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                ", FactorRate=" + QuotedStr(ViewState("FactorRate")) + _
                ",  DecPlacePrice = " + tbPriceDec.Text + ", DecPlaceBaseForex = " + tbTotalDec.Text + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lbRevisi.Text
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

            Row = ViewState("DtSub").Select("TransNmbr IS NULL")
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
            Dim sqlstring1 As String
            sqlstring1 = ""
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Revisi, Product, Specification, Specification2, QtyOrder, UnitOrder, Qty, Unit, QtyM2, QtyRoll, PriceForex, AmountForex, Disc, DiscForex, NettoForex, CommisionForex, CommisionUnit, FgPartialDelivery, DeliveryDate, Remark, UnitPerRoll, UnitPerM2  FROM MKTSODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE MKTSODt SET Specification = @Specification, QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Qty = @Qty, Unit = @Unit, QtyM2 = @QtyM2, " + _
                    "QtyRoll = @QtyRoll, PriceForex = @PriceForex, AmountForex = @AmountForex, " + _
                    "Disc = @Disc, DiscForex = @DiscForex, NettoForex = @NettoForex, CommisionForex = @CommisionForex, " + _
                    "CommisionUnit = @CommisionUnit, Remark = @Remark, FgPartialDelivery = @FgPartialDelivery, DeliveryDate = @DeliveryDate, UnitPerM2 = @UnitPerM2, " + _
                    "UnitPerRoll = @UnitPerRoll, Specification2 = @Specification2 " + _
                     "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi") & " AND Product = @OldProduct ", con)

            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 20, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@Specification", SqlDbType.VarChar, 255, "Specification")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Float, 20, "QtyOrder")
            Update_Command.Parameters.Add("@QtyM2", SqlDbType.Float, 20, "QtyM2")
            Update_Command.Parameters.Add("@QtyRoll", SqlDbType.Float, 20, "QtyRoll")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 20, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 20, "AmountForex")
            Update_Command.Parameters.Add("@Disc", SqlDbType.Float, 20, "Disc")
            Update_Command.Parameters.Add("@DiscForex", SqlDbType.Float, 20, "DiscForex")
            Update_Command.Parameters.Add("@NettoForex", SqlDbType.Float, 20, "NettoForex")
            Update_Command.Parameters.Add("@CommisionForex", SqlDbType.Float, 20, "CommisionForex")
            Update_Command.Parameters.Add("@CommisionUnit", SqlDbType.VarChar, 5, "CommisionUnit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@FgPartialDelivery", SqlDbType.VarChar, 1, "FgPartialDelivery")
            Update_Command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime, 20, "DeliveryDate")
            Update_Command.Parameters.Add("@UnitPerM2", SqlDbType.Float, 20, "UnitPerM2")
            Update_Command.Parameters.Add("@UnitPerRoll", SqlDbType.Float, 20, "UnitPerRoll")
            Update_Command.Parameters.Add("@Specification2", SqlDbType.VarChar, 255, "Specification2")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MKTSODt WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTSODt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            If Not ViewState("Dt2") Is Nothing Then
                cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, Product, Delivery, QtyOrder, UnitOrder, QtyPack, UnitPack, Remark FROM MKTSODt2 WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                'Dim param1 As SqlParameter
                '' Create the UpdateCommand.
                'Dim Update_Command1 = New SqlCommand( _
                '        "UPDATE MKTSODt2 SET Delivery = @Delivery, QtyOrder = @QtyOrder1, UnitOrder = @UnitOrder1, QtyPack = @QtyPack1, " + _
                '        "UnitPack = @UnitPack1, Remark = @Remark1 " + _
                '        "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi") & " AND Product = @OldProduct1 AND Delivery = @OldDelivery ", con)

                'Update_Command.Parameters.Add("@Product1", SqlDbType.VarChar, 20, "Product")
                'Update_Command.Parameters.Add("@Delivery", SqlDbType.DateTime, 20, "Delivery")
                'Update_Command.Parameters.Add("@QtyOrder1", SqlDbType.Float, 20, "QtyOrder")
                'Update_Command.Parameters.Add("@UnitOrder1", SqlDbType.VarChar, 5, "UnitOrder")
                'Update_Command.Parameters.Add("@QtyPack1", SqlDbType.Float, 20, "QtyPack1")
                'Update_Command.Parameters.Add("@UnitPack1", SqlDbType.VarChar, 5, "UnitPack1")
                'Update_Command.Parameters.Add("@Remark1", SqlDbType.VarChar, 255, "Remark")
                '' Define intput (WHERE) parameters.
                'param1 = Update_Command.Parameters.Add("@OldProduct1", SqlDbType.VarChar, 20, "Product")
                'param1.SourceVersion = DataRowVersion.Original
                'param1 = Update_Command.Parameters.Add("@OldDelivery", SqlDbType.DateTime, 20, "Delivery")
                'param1.SourceVersion = DataRowVersion.Original
                '' Attach the update command to the DataAdapter.
                'da.UpdateCommand = Update_Command

                '' Create the DeleteCommand.
                'Dim Delete_Command1 = New SqlCommand( _
                '    "DELETE FROM MKTSODt2 WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi") & " AND Product = @Product1 AND Delivery = @Delivery ", con)
                '' Add the parameters for the DeleteCommand.
                'param1 = Delete_Command.Parameters.Add("@Product1", SqlDbType.VarChar, 20, "Product")
                'param1.SourceVersion = DataRowVersion.Original
                'param1 = Delete_Command.Parameters.Add("@Delivery", SqlDbType.DateTime, 20, "Delivery")
                'param1.SourceVersion = DataRowVersion.Original
                'da.DeleteCommand = Delete_Command

                Dim Dt2 As New DataTable("MKTSODt2")

                Dt2 = ViewState("Dt2")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("Dt2") = Dt2

            End If

            If Not ViewState("DtSub") Is Nothing Then
                'save DtSub
                cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, Product, ItemNo, Remark, Qty, Unit, Price FROM MKTSODtSub WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Revisi = " & ViewState("Revisi"), con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                'Dim param1 As SqlParameter
                '' Create the UpdateCommand.
                'Dim Update_Command1 = New SqlCommand( _
                '        "UPDATE MKTSODtSub  SET Qty = @Qty2, Unit = @Unit2, " + _
                '        "Price = @Price, Remark = @Remark2 " + _
                '        "WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi") & " AND Product = @OldProduct2 AND ItemNo = @OldItemNo ", con)

                'Update_Command.Parameters.Add("@Product2", SqlDbType.VarChar, 20, "Product")
                'Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
                'Update_Command.Parameters.Add("@Qty2", SqlDbType.Float, 20, "Qty")
                'Update_Command.Parameters.Add("@Unit2", SqlDbType.VarChar, 5, "Unit")
                'Update_Command.Parameters.Add("@Remark2", SqlDbType.VarChar, 255, "Remark")
                '' Define intput (WHERE) parameters.
                'param1 = Update_Command.Parameters.Add("@OldProduct2", SqlDbType.VarChar, 20, "Product")
                'param1.SourceVersion = DataRowVersion.Original
                'param1 = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.VarChar, 20, "ItemNo")
                'param1.SourceVersion = DataRowVersion.Original
                '' Attach the update command to the DataAdapter.
                'da.UpdateCommand = Update_Command

                '' Create the DeleteCommand.
                'Dim Delete_Command1 = New SqlCommand( _
                '    "DELETE FROM MKTSODtSub WHERE TransNmbr = " & QuotedStr(ViewState("TransNmbr")) & " AND Revisi = " & ViewState("Revisi") & " AND Product = @Product2 ", con)
                '' Add the parameters for the DeleteCommand.
                'param1 = Delete_Command.Parameters.Add("@Product2", SqlDbType.VarChar, 20, "Product")
                'param1.SourceVersion = DataRowVersion.Original
                'param1 = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
                'param1.SourceVersion = DataRowVersion.Original
                'da.DeleteCommand = Delete_Command

                Dim DtSub As New DataTable("MKTSODtSub")

                DtSub = ViewState("DtSub")
                da.Update(DtSub)
                DtSub.AcceptChanges()
                ViewState("DtSub") = DtSub
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
            'If ViewState("DtSub").Rows.Count = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Sub Product must have at least 1 record")
            '    Exit Sub
            'End If

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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbProductGroup.Focus()
            GridDt.Columns(2).Visible = False
            ddlSOType.Enabled = True
            btnIRRNo.Visible = False
            tbBaseForex.Text = 0
            tbDiscForex.Text = 0
            'ViewState("Dt2") = Nothing
            'ddlNeedDelivery_SelectedIndexChanged(Nothing, Nothing)
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            'ViewState("DigitCurr") = -1
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()
            Cleardt2()
            ClearDtSub()
            ddlSOKind.Enabled = True
            btnAddDtSub.Visible = True
            btnAddDtSubke2.Visible = True
            btnAddDt2.Visible = True
            btnAddDt2Ke2.Visible = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            PnlDt.Visible = True
            BindDataDtSub("", 0)
            BindDataDt("", 0)
            BindDataDt2("", 0)
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
            FilterName = "IRR No, Customer, Customer Name, Delivery, Remark"
            FilterValue = "TransNmbr, Customer, CustomerName, Delivery, Remark"
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

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit

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
                    Menu1.TabIndex = 0
                    BindDataDtSub(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDtSub, GridSub)
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    GridDt.Columns(2).Visible = True
                    ddlSOType.Enabled = False





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
                        BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDtSub(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDtSub, GridSub)
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        tbdelivery.Enabled = ddlNeedDelivery.SelectedIndex = 0
                        tbAddress.Enabled = tbdelivery.Enabled
                        ddlCity.Enabled = tbdelivery.Enabled
                        ddlCostBy.Enabled = tbdelivery.Enabled
                        btnHome.Visible = False
                        btnAddDtSub.Visible = True
                        btnAddDtSubke2.Visible = True
                        btnAddDt2.Visible = True
                        btnAddDt2Ke2.Visible = True
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(Not DtExist())
                        ddlSOType.Enabled = False
                        GridDt.Columns(2).Visible = False

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

                    Dim ResultCek, SqlStringCek, Result, SqlString, Value As String
                    'klik
                    If HiddenRemarkRevisi.Value <> "False Value" Then

                        'HiddenRemarkRevisi.Value = ""
                        'SqlStringCek = "EXEC S_MKSOCreateRevisiCek " + QuotedStr(HiddenRemarkRevisi.Value)
                        ' ''ResultCek = SQLExecuteScalar(SqlStringCek, ViewState("DBConnection"))
                        'ResultCek = ResultCek.Replace("0", "")
                        
                        AttachScript("revisi();", Page, Me.GetType)

                        'ResultCek = HiddenRemarkRevisi.Value
                        'lbStatus.Text = HiddenRemarkRevisi.Value
                        'Exit Sub

                        If HiddenRemarkRevisi.Value <> "" Then
                            SqlString = "Declare @A VarChar(255) EXEC S_MKSOCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(HiddenRemarkRevisi.Value) + ", @A OUT SELECT @A"
                            Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                            Result = Result.Replace("0", "")

                            If Trim(Result) <> "" Then
                                lbStatus.Text = MessageDlg(Result)
                            End If
                            ' ResultCek = ""

                        Else
                            'ResultCek = ""
                            Exit Sub
                        End If
                        'If Result.Length > 2 Then
                        '    lbStatus.Text = MessageDlg(Result)
                        'Else
                        '    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        'End If
                    End If
                        'SqlString = "Declare @A VarChar(255) EXEC S_MKSOCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                        'Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))

                        'Result = Result.Replace("0", "")
                        'If Trim(Result) <> "" Then
                        '    'lbStatus.Text = MessageDlg(Result)
                        'End If
                        'CurrFilter = tbFilter.Text

                        Value = ddlField.SelectedValue
                        tbFilter.Text = tbCode.Text
                        ddlField.SelectedValue = "TransNmbr"
                        btnSearch_Click(Nothing, Nothing)
                        'tbFilter.Text = CurrFilter
                        ddlField.SelectedValue = Value
                        tbdelivery.Enabled = ddlNeedDelivery.SelectedIndex = 0
                        tbAddress.Enabled = tbdelivery.Enabled
                        ddlCity.Enabled = tbdelivery.Enabled
                        ddlCostBy.Enabled = tbdelivery.Enabled
                    ddlSOType.Enabled = False

                    ElseIf DDL.SelectedValue = "Generate DO" Then

                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))

                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        index = Convert.ToInt32(e.CommandArgument)
                        GVR = GridView1.Rows(index)

                        If Not GVR.Cells(4).Text = "P" Then
                            lbStatus.Text = MessageDlg("Data Must Post Before Generate DO")
                            Exit Sub
                        End If

                        Dim Result, SqlString1, CurrFilter, Value As String
                        'Disini

                        SqlString1 = "Declare @A VarChar(255) EXEC S_MKSOGenerateDO " + QuotedStr(GVR.Cells(2).Text) + ", " + GVR.Cells(3).Text + ", " + CInt(ViewState("GLYear")).ToString + ", " + CInt(ViewState("GLPeriod")).ToString + ", '', '', " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"

                        Result = SQLExecuteScalar(SqlString1, ViewState("DBConnection"))

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
                        tbdelivery.Enabled = ddlNeedDelivery.SelectedIndex = 0
                        tbAddress.Enabled = tbdelivery.Enabled
                        ddlCity.Enabled = tbdelivery.Enabled
                        ddlCostBy.Enabled = tbdelivery.Enabled
                        ddlSOType.Enabled = False

                    ElseIf DDL.SelectedValue = "Print" Then
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_MKFormSO ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "'', " + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormSO.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    ElseIf DDL.SelectedValue = "Print 2" Then
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_MKFormSO ''" + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "'', " + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormSO2.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Else
                        lbStatus.Text = "Cannot print or preview, status SO No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
                        Exit Sub
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
                lbProductSub.Text = GVR.Cells(3).Text
                lbProductDt2.Text = GVR.Cells(3).Text

                lbProductDtName2.Text = " - " + GVR.Cells(4).Text
                lbProductNameSub.Text = " - " + GVR.Cells(4).Text
                If GVR.Cells(16).Text = "N" Then
                    lbStatus.Text = MessageDlg("Cannot Detail Partial Delivery N")
                    Exit Sub
                End If
                MultiView1.ActiveViewIndex = 2
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                Else
                    drow = ViewState("Dt2").Select("Product = " + QuotedStr(GVR.Cells(3).Text))
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
                
            ElseIf e.CommandName = "DetailSub" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                lbProductSub.Text = GVR.Cells(3).Text
                lbProductDt2.Text = GVR.Cells(3).Text
                'If GVR.Cells(17).Text = "N" Then
                '    lbStatus.Text = " Cannot Detail Partial Delivery N"
                '    Exit Sub
                'End If

                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("DtSub") Is Nothing Then
                    BindDataDtSub(ViewState("TransNmbr"), ViewState("Revisi"))
                Else
                    drow = ViewState("DtSub").Select("Product=" + QuotedStr(GVR.Cells(3).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridSub)
                        GridSub.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("DtSub").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridSub.DataSource = DtTemp
                        GridSub.DataBind()
                        GridSub.Columns(0).Visible = False
                    End If
                End If

            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status SO is not Post, cannot close product")
                    Exit Sub
                End If
                If GVR.Cells(24).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Product Closed Already")
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
            Dim dr, dr2 As DataRow()
            Dim dr1 As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr2 = ViewState("Dt2").Select("Product = " + QuotedStr(GVR.Cells(3).Text)) 'QuotedStr(dr("Product").ToString))
            For I As Integer = 0 To (dr2.Count - 1)
                dr2(I).Delete()
            Next

            'dr2 = ViewState("Dt2").Select("Product = " + QuotedStr(GVR.Cells(3).Text))
            'If dr2.Length > 0 Then
            '    BindGridDt(dr2.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If
            'BindGridDt(ViewState("Dt2"), GridDt2)

            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(3).Text))
            dr(0).Delete()

            'GridDt2_RowDeleting(False, Nothing)


            'dr2 = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text)) 'QuotedStr(dr("Product").ToString))
            'For I As Integer = 0 To (dr2.Count - 1)
            '    dr2(I).Delete()
            'Next


            'BindGridDt(ViewState("Dt2"), GridDt2)

            BindGridDt(ViewState("Dt"), GridDt)
            totalingDt()
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GenerateDt2Delete(ByVal Product As String)
        Dim Result, ListSelectNmbr As String
        Dim Pertamax As Boolean
        Try
            Pertamax = True
            Result = ""
            For Each drResult In ViewState("Dt2").Rows
                If Not (drResult.RowState = DataRowState.Deleted) Then
                    ListSelectNmbr = Product
                    If Pertamax Then
                        Result = "'''" + ListSelectNmbr + "''"
                        Pertamax = False
                    Else
                        Result = Result + ",''" + ListSelectNmbr + "''"
                    End If
                End If
            Next

            If Result <> "" Then
                Result = Result + "'"
            Else
                Result = QuotedStr("''")
            End If

            Dim drdelete As DataRow()
            drdelete = ViewState("Dt2").Select("Product = " + QuotedStr(Product))
            For I As Integer = 0 To (drdelete.Count - 1)
                drdelete(I).Delete()
            Next

        Catch ex As Exception
            lbStatus.Text = "GenerateDt2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        'Dim Row2 As DataRow
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    QtySisa = QtySisa + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyOrder"))
                    'TotalQty = GetTotalSum(ViewState("Dt"), "QtyOrder")
                End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then

            End If
            tbQtyOrderDt.Text = CStr(QtySisa)
            'lbQtyOrderSisa.Text = CFloat(tbQtyreq.Text) ' - QtySisa
            lbQtyOrderDt2.Text = CFloat(tbQtyOrderDt.Text) ' - QtySisa
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim Row2 As DataRow
        Dim SqlString As String
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
            Row2 = ViewState("Dt").Select("Product = " + QuotedStr(lbProductDt2.Text))(0)
            Row2.BeginEdit()

            Row2("QtyOrder") = FormatFloat(lbQtyOrderDt2.Text, ViewState("DigitQty"))


            SqlString = "EXEC S_MKSOGetPrice " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(ddlCurrHd.SelectedValue) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(lbProductSub.Text) + "," + QuotedStr(GVR.Cells(2).Text)
            Row2("PriceForex") = FormatNumber(SQLExecuteScalar(SqlString, ViewState("DBConnection")), CInt(ViewState("DigitCurr")))

            dtUnit = SQLExecuteQuery("Select UnitPerM2, UnitPerRoll from VMsProduct WHERE Product_Code = " + QuotedStr(lbProductDt2.Text), ViewState("DBConnection")).Tables(0)
            drUnit = dtUnit.Rows(0)

            tbQtyPerRoll.Text = drUnit("UnitPerRoll")
            tbQtyPerM2.Text = drUnit("UnitPerM2")

            Dim Qty, QtyRoll, QtyM2 As Double
            Qty = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(Row2("Product").ToString) + ", " + Row2("UnitOrder") + ", " + lbQtyOrderDt2.Text.Replace(",", ""), ViewState("DBConnection"))
            Row2("Qty") = FormatFloat(Qty, ViewState("DigitQty"))
            If UCase(Row2("Unit")) = "ROLL" Then
                'Row2("QtyRoll") = FormatFloat(CFloat(lbQtyOrderDt2.Text), ViewState("DigitQty"))
                QtyRoll = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(Row2("Product").ToString) + ", " + Row2("UnitOrder") + ", " + lbQtyOrderDt2.Text.Replace(",", ""), ViewState("DBConnection"))
                Row2("QtyRoll") = FormatFloat(QtyRoll, ViewState("DigitQty"))

            ElseIf UCase(Row2("Unit")) = "M2" Then
                QtyM2 = SQLExecuteScalar("EXEC S_MKSOConvertUnitOrder " + QuotedStr(Row2("Product").ToString) + ", " + Row2("UnitOrder") + ", " + lbQtyOrderDt2.Text.Replace(",", ""), ViewState("DBConnection"))
                Row2("QtyM2") = FormatFloat(QtyM2, ViewState("DigitQty"))

            Else
                If CFloat(drUnit("UnitPerRoll")) = 0 Then
                    Row2("QtyRoll") = FormatFloat(0, ViewState("DigitQty"))
                Else
                    Row2("QtyRoll") = FormatFloat(CFloat(lbQtyOrderDt2.Text) / CFloat(drUnit("UnitPerRoll")), ViewState("DigitQty"))
                End If
                If CFloat(drUnit("UnitPerM2")) = 0 Then
                    Row2("QtyM2") = FormatFloat(0, ViewState("DigitQty"))
                Else
                    Row2("QtyM2") = FormatFloat(CFloat(lbQtyOrderDt2.Text) / CFloat(drUnit("UnitPerM2")), ViewState("DigitQty"))
                End If

            End If
            Row2("AmountForex") = FormatNumber(CFloat(lbQtyOrderDt2.Text) * CFloat(Row2("PriceForex")), ViewState("DigitCurr"))
            Row2.EndEdit()

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Dim QtySisa As Decimal = 0
    Dim Total As Decimal = 0
    Dim TotalQty As Decimal
    Protected Sub GridSub_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridSub.RowDataBound

    End Sub
    Protected Sub GridSub_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridSub.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridSub.Rows(e.RowIndex)

            dr = ViewState("DtSub").Select("Product = " + QuotedStr(lbProductSub.Text) + " AND ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'ModifyDt()
            dr = ViewState("DtSub").Select("Product=" + QuotedStr(lbProductSub.Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridSub)
                GridSub.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("DtSub").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridSub.DataSource = DtTemp
                GridSub.DataBind()
                GridSub.Columns(0).Visible = False
            End If
            'BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt PR Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(3).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("ProductDt") = GVR.Cells(3).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridSub_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridSub.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridSub.Rows(e.NewEditIndex)
            FillTextBoxDtSub(lbProductSub.Text, GVR.Cells(1).Text)
            MovePanel(pnlDtSub, pnlEditDtSub)
            EnableHd(False)
            ViewState("StateDtSub") = "Edit"
            ViewState("RequestPR") = GVR.Cells(1).Text
            ViewState("ProductPR") = lbProductSub.Text
            btnSavePR.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt PR Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbProductDt2.Text, CDate(GVR.Cells(1).Text))
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            ViewState("Delivery") = GVR.Cells(1).Text
            StatusButtonSave(False)
            tbQtyOrderDt2.Focus()
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
            BindToDropList(ddlSOKind, Dt.Rows(0)("SOKind").ToString)
            BindToDropList(ddlSOType, Dt.Rows(0)("SOType").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbProductGroup, Dt.Rows(0)("ProductGroup").ToString)
            BindToText(tbProductGroupName, Dt.Rows(0)("ProductGroupName").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbPhone, Dt.Rows(0)("Phone").ToString)
            BindToText(tbCustPONo, Dt.Rows(0)("CustPONo").ToString)
            BindToText(tbContractNo, Dt.Rows(0)("ContractNo").ToString)
            BindToText(tbIRRNo, Dt.Rows(0)("IRRNo").ToString)
            BindToDropList(ddlNeedDelivery, Dt.Rows(0)("FgNeedDelivery").ToString)
            BindToText(tbdelivery, Dt.Rows(0)("Delivery").ToString)
            BindToText(tbdeliveryName, Dt.Rows(0)("DeliveryName").ToString)
            BindToText(tbAddress, Dt.Rows(0)("DeliveryAddr").ToString)
            BindToDropList(ddlCity, Dt.Rows(0)("DeliveryCity").ToString)
            BindToDropList(ddlCostBy, Dt.Rows(0)("DeliveryCostBy").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToText(tbTermRemark, Dt.Rows(0)("TermRemark").ToString)
            BindToDropList(ddlfgInclude, Dt.Rows(0)("FgPriceIncludeTax").ToString)
            BindToDropList(ddlCurrHd, Dt.Rows(0)("Currency").ToString)
            ViewState("DigitCurr") = 2 'GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            'BindToText(tbDisc, Dt.Rows(0)("Disc").ToString)
            BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPN").ToString) ', ViewState("Digit")("Percent")
            BindToText(tbDP, Dt.Rows(0)("DP").ToString) ', ViewState("Digit")("Percent")
            BindToText(tbDPForex, Dt.Rows(0)("DPForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPNForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbPriceDec, Dt.Rows(0)("DecPlacePrice").ToString)
            BindToText(tbTotalDec, Dt.Rows(0)("DecPlaceBaseForex").ToString)
            lbRemarkRevisi.Text = Dt.Rows(0)("RemarkRevisi").ToString
            ViewState("FactorRate") = Dt.Rows(0)("FactorRate")
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
                ViewState("DigitCurr") = 2
                BindToText(tbProductDt, Dr(0)("Product").ToString)
                BindToText(tbProductNameDt, Dr(0)("ProductName").ToString)
                BindToText(tbSpecificationDt, Dr(0)("Specification").ToString)
                BindToText(tbSpecificationDt2, Dr(0)("Specification2").ToString)
                BindToText(tbQtyOrderDt, Dr(0)("QtyOrder").ToString)
                BindToText(tbQtyWrhsDt, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitWrhsDt, Dr(0)("Unit").ToString)
                BindToDropList(ddlUnitOrderDt, Dr(0)("UnitOrder").ToString)
                BindToText(tbQtyM2Dt, Dr(0)("QtyM2").ToString)
                BindToText(tbQtyRollDt, Dr(0)("QtyRoll").ToString)
                BindToText(tbQtyPerRoll, Dr(0)("UnitPerRoll").ToString)
                BindToText(tbQtyPerM2, Dr(0)("UnitPerM2").ToString)
                BindToText(tbPriceForexDt, Dr(0)("PriceForex").ToString, ViewState("DigitCurr"))
                BindToText(tbAmountForexDt, Dr(0)("AmountForex").ToString, ViewState("DigitCurr"))
                BindToText(tbDiscDt, Dr(0)("Disc").ToString, ViewState("DigitCurr"))
                BindToText(tbDiscForexDt, Dr(0)("DiscForex").ToString, ViewState("DigitCurr"))
                BindToText(tbTotalForexDt, Dr(0)("NettoForex").ToString, ViewState("DigitCurr"))
                BindToText(tbcommision, Dr(0)("CommisionForex").ToString, ViewState("DigitCurr"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToDropList(ddlUnitCommision, Dr(0)("CommisionUnit").ToString)
                BindToDropList(ddlPartial, Dr(0)("FgPartialDelivery").ToString)
                BindToDate(tbDeliveryDate, Dr(0)("DeliveryDate").ToString)
                lbUnitPack.Text = Dr(0)("UnitPack").ToString
                lbUnitM2.Text = Dr(0)("UnitM2").ToString
                ddlUnitOrderDt.Enabled = CFloat(Dr(0)("QtyOrder")) = 0
                tbProductDt.Enabled = CFloat(Dr(0)("QtyOrder")) = 0
                btnProductDt.Enabled = CFloat(Dr(0)("QtyOrder")) = 0
                BindToText(tbQtyDO, Dr(0)("QtyDO").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal Product As String, ByVal DeliveryDate As Date)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Product = " + QuotedStr(Product) + " AND Delivery =" + QuotedStr(Format(DeliveryDate.Date, "dd MMM yyyy")))
            'lbStatus.Text = Dr.Length.ToString
            'Exit Sub
            If Dr.Length > 0 Then
                'Lbitem.Text = Dr(0)("ItemNo").ToString
                tbDeliveryDateDt2.SelectedDate = Dr(0)("Delivery").ToString
                BindToText(tbQtyOrderDt2, Dr(0)("QtyOrder").ToString)
                BindToText(tbQtyPackDt2, Dr(0)("QtyPack").ToString)
                BindToDropList(ddlUnitOrderDt2, Dr(0)("UnitOrder").ToString)
                BindToDropList(ddlUnitPackDt2, Dr(0)("UnitPack").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDtSub(ByVal Product As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtSub").select("Product = " + QuotedStr(Product) + " AND ItemNo = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then

                Lbitem.Text = Dr(0)("ItemNo").ToString
                BindToText(tbDetailRemark, Dr(0)("Remark").ToString)
                BindToText(tbQtySub, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitSub, Dr(0)("Unit").ToString)
                BindToText(tbPriceSub, Dr(0)("Price").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box Dt PR error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            'Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            Dr = FindMaster("CustAddressAll", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Cust_Code")
                tbCustName.Text = Dr("Cust_Name")
                BindToText(tbAttn, Dr("Contact_Person").ToString)
                BindToText(tbPhone, Dr("Phone").ToString)
                BindToDropList(ddlCurrHd, Dr("Currency"))
                BindToDropList(ddlTerm, Dr("Term"))
                BindToText(tbdelivery, Dr("Delivery_Code").ToString)
                BindToText(tbdeliveryName, Dr("Delivery_Name").ToString)
                BindToText(tbAddress, Dr("Delivery_Addr1").ToString + "" + Dr("Delivery_Addr2").ToString)
                BindToDropList(ddlCity, Dr("City").ToString)
                'BindToDropList(ddlfgInclude, Dr("FgPPN").ToString)
                BindToDropList(ddlCurrHd, Dr("Currency"))
                ddlTerm_SelectedIndexChanged(Nothing, Nothing)
                If Dr("FgPPN").ToString = "Y" Then
                    tbPPN.Text = "10"
                    tbPPN.Enabled = True
                Else
                    tbPPN.Text = "0"
                    tbPPN.Enabled = False
                End If
                'ddlfgInclude_SelectedIndexChanged(Nothing, Nothing)
                'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                'tbRateHd.Enabled = False

                tbAttn.Focus()
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                tbAttn.Text = ""
                tbPhone.Text = ""
                tbdelivery.Text = ""
                tbdeliveryName.Text = ""
                tbAddress.Text = ""
                ddlCity.SelectedValue = ""
                ddlfgInclude.SelectedValue = "N"
                tbPPN.Text = "0"
                ddlfgInclude_SelectedIndexChanged(Nothing, Nothing)
                tbCustCode.Focus()
            End If
        Catch ex As Exception
            lbStatus.Text = "tbCustCode_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnRequestPR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequestPR.Click
    '    Dim ResultField, CriteriaField As String
    '    Try
    '        Session("filter") = "EXEC S_MKSOReff 'Goods', '', '" + tbCode.Text + "', " + lbRevisi.Text
    '        CriteriaField = "PR_No, Product, Product_Name, Specification, Unit"
    '        ViewState("CriteriaField") = CriteriaField.Split(",")
    '        ResultField = "PR_No, Product, Product_Name, Specification, Qty, Unit, UnitOrder"
    '        ViewState("Sender") = "btnRequestPR"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
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
            ddlUnitOrderDt2.SelectedValue = dr("UnitOrder").ToString
            FillCombo(ddlUnitPackDt2, "EXEC S_FindUnit " + QuotedStr(dr("UnitPack").ToString) + ", " + QuotedStr(dr("UnitM2").ToString), False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            ddlUnitPackDt2.SelectedValue = dr("UnitPack").ToString
            'If dr("PriceInUnit").ToString = "M2" Then
            '    'tbQtyOrderDt2.Text = dr("QtyM2").ToString
            '    ddlUnitOrderDt2.SelectedValue = "M2"
            'ElseIf dr("PriceInUnit").ToString = "Roll" Then
            '    'tbQtyOrderDt2.Text = dr("QtyRoll").ToString
            '    ddlUnitOrderDt2.SelectedValue = "Roll"
            'Else
            '    'tbQtyOrderDt2.Text = dr("Qty").ToString
            '    ddlUnitOrderDt2.SelectedValue = dr("Unit").ToString
            'End If
            'tbDeliveryDateDt2.SelectedDate = CDate(dr("Delivery"))
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbQtyOrderDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDtSub_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDtSub.Click, btnAddDtSubke2.Click
        Try
            ClearDtSub()
            If CekHd() = False Then
                Exit Sub
            End If
            Dim dr As DataRow
            dr = ViewState("Dt").Select("Product=" + QuotedStr(lbProductSub.Text))(0)
            tbQtySub.Text = dr("Qty").ToString
            ddlUnitSub.SelectedValue = dr("Unit").ToString
            'If dr("PriceInUnit").ToString = "M2" Then
            '    tbQtySub.Text = dr("QtyM2").ToString
            '    ddlUnitSub.SelectedValue = "M2"
            'ElseIf dr("PriceInUnit").ToString = "Roll" Then
            '    tbQtySub.Text = dr("QtyRoll").ToString
            '    ddlUnitSub.SelectedValue = "ROLL"
            'Else
            '    tbQtySub.Text = dr("Qty").ToString
            '    ddlUnitSub.SelectedValue = dr("Unit").ToString
            'End If
            ViewState("StateDtSub") = "Insert"

            Dim drow As DataRow()
            Dim dt As DataTable
            drow = ViewState("DtSub").Select("Product=" + QuotedStr(lbProductSub.Text))
            If drow.Length > 0 Then
                dt = drow.CopyToDataTable
                Lbitem.Text = GetNewItemNo(dt)
            Else
                'Lbitem.Text = GetNewItemNo(ViewState("DtSub"))
                Lbitem.Text = "1"
            End If
            MovePanel(pnlDtSub, pnlEditDtSub)
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
            lbProductSub.Text = lbProductDt2.Text
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
            'ChangeCurrency(ddlCurrHd, tbDate, tbRateHd, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateHd.Enabled = False
            ddlCurrHd.Focus()
        Catch ex As Exception
            lbStatus.Text = "DDL Curr Hd Error : " + ex.ToString
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
            If ViewState("DtSub").Rows.Count = 0 Then
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

    Public Function FindConvertM2(ByVal Product As String, ByVal Qty As Double, ByVal Connection As String) As Double
        Dim sqlstring As String
        Dim Hasil As String
        Try
            sqlstring = "EXEC S_FindConvertM2 " + QuotedStr(Product) + ", " + Qty.ToString
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Double.Parse(Hasil)
        Catch ex As Exception
            Throw New Exception("Exec SP Find Convert Unit Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            'If ddlShipmentType.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Shipment Type must have value")
            '    Exit Sub
            'End If
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "EXEC S_MKSOReff '" + tbCustCode.Text + "', '" + ddlCurrHd.SelectedValue + "', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '', " + QuotedStr(ddlfgInclude.SelectedValue)
            ResultField = "Product, Product_Name, Specification, Specification2, Unit, Price, UnitOrder, UnitPack, UnitM2"
            CriteriaField = "Product, Product_Name, Specification, Specification2, Unit, UnitOrder, UnitPack, UnitM2"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
    '    Try
    '        Session("filter") = "Select PR_No, PR_Date, [Type_Name], Product, Product_Name, Specification, Qty, Unit from V_MKSOGetRequest Where RequestType = 'Goods' "
    '        ViewState("CheckDlg") = False
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search PO No Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub tbQtyDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrderDt.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ConvertM2", tbProductDt.Text + "|" + tbQtyWrhsDt.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbQtyM2Dt.Text = FormatFloat(Dr("QtyM2").ToString, ViewState("DigitQty"))
                tbQtyRollDt.Text = FormatFloat(Dr("QtyRoll").ToString, ViewState("DigitQty"))
                tbQtyWrhsDt.Text = FormatFloat(tbQtyWrhsDt.Text, ViewState("DigitQty"))
                tbQtyOrderDt.Text = FormatFloat(tbQtyOrderDt.Text, ViewState("DigitQty"))

                If ddlUnitWrhsDt.SelectedValue = "M2" Then
                    tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyM2Dt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
                ElseIf ddlUnitWrhsDt.SelectedValue = "Roll" Then
                    tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyRollDt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
                Else
                    tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyWrhsDt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
                End If
                tbDiscForexDt.Text = FormatNumber(CFloat(tbDiscDt.Text) * CFloat(tbAmountForexDt.Text) / 100, ViewState("DigitCurr"))
                tbTotalForexDt.Text = FormatNumber(CFloat(tbAmountForexDt.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
            Else
                tbQtyM2Dt.Text = FormatFloat(0, ViewState("DigitQty"))
                tbQtyRollDt.Text = FormatFloat(0, ViewState("DigitQty"))
                tbQtyWrhsDt.Text = FormatFloat(0, ViewState("DigitQty"))
                tbAmountForexDt.Text = FormatNumber(0, ViewState("DigitCurr"))
                tbDiscForexDt.Text = FormatNumber(0, ViewState("DigitCurr"))
                tbTotalForexDt.Text = FormatNumber(0, ViewState("DigitCurr"))
            End If

            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            ddlUnitOrderDt.Focus()
        Catch ex As Exception
            Throw New Exception("tbQtyDt_TextChanged Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        ViewState("InputCurrency") = "Y"
        Session("DBConnection") = ViewState("DBConnection")
        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Term", ddlTerm.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbTermRemark, Dr("Term_Name").ToString)
            Else
                tbTermRemark.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnProductDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductDt.Click
        Dim ResultField As String
        Try
            Session("filter") = "EXEC S_MKSOReff '" + tbCustCode.Text + "', '" + ddlCurrHd.SelectedValue + "', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '', " + QuotedStr(ddlfgInclude.SelectedValue)
            ResultField = "Product, Product_Name, Specification, Specification2, Unit, Price"
            ViewState("Sender") = "btnProductDt"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnProductGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductGroup.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Product_Group_Code, Product_Group_Name FROM VMsProductGroup"
            ResultField = "Product_Group_Code, Product_Group_Name"
            ViewState("Sender") = "btnProductGroup"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Customer_Code, Customer_Name, Currency, Term, Contact_Person, Phone, PPN FROM VMsCustomer Where FgActive='Y'"
            ResultField = "Customer_Code, Customer_Name, Contact_Person, Currency, Term, Phone, PPN"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDelivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelivery.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Delivery_Code, Delivery_Name, Delivery_Addr1, Delivery_Addr2, City FROM VMsCustAddress WHERE Cust_Code = " + QuotedStr(tbCustCode.Text.Trim)
            ResultField = "Delivery_Code, Delivery_Name, Delivery_Addr1, Delivery_Addr2, City"
            ViewState("Sender") = "btnDelivery"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductGroup.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ProductGroup", tbProductGroup.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbProductGroup, Dr("Product_Group_Code").ToString)
                BindToText(tbProductGroupName, Dr("Product_Group_Name").ToString)
            Else
                tbProductGroup.Text = ""
                tbProductGroupName.Text = ""
            End If
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlNeedDelivery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNeedDelivery.SelectedIndexChanged
        Try
            If ddlNeedDelivery.SelectedValue = "Y" Then
                tbdelivery.Enabled = True
                btnDelivery.Enabled = True
                tbAddress.Enabled = True
                ddlCity.Enabled = True
                ddlCostBy.Enabled = True
                ddlCostBy.SelectedIndex = 1
            Else
                tbdelivery.Enabled = False
                btnDelivery.Enabled = False
                tbAddress.Enabled = False
                ddlCity.Enabled = False
                ddlCostBy.Enabled = False
                ddlCostBy.SelectedIndex = 0
            End If
            tbdelivery.Text = ""
            tbdeliveryName.Text = ""
            tbAddress.Text = ""
            ddlCity.SelectedValue = ""
            ddlNeedDelivery.Focus()
        Catch ex As Exception
            Throw New Exception("Need Delivery SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbdelivery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbdelivery.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("CustAddress", tbCustCode.Text + "|" + tbdelivery.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbdelivery, Dr("Delivery_Code").ToString)
                BindToText(tbdeliveryName, Dr("Delivery_Name").ToString)
                BindToText(tbAddress, Dr("Delivery_Addr1").ToString + "" + Dr("Delivery_Addr2").ToString)
                BindToDropList(ddlCity, Dr("City").ToString)
                ddlCity.Focus()
            Else
                tbdelivery.Text = ""
                tbdeliveryName.Text = ""
                tbAddress.Text = ""
                ddlCity.SelectedValue = ""
                tbAddress.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductDt.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_MKSOReff " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurrHd.SelectedValue) + ", '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbProductDt.Text.Trim) + ", " + QuotedStr(ddlfgInclude.SelectedValue), ViewState("DBConnection")).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProductDt, Dr("Product").ToString)
                BindToText(tbProductNameDt, Dr("Product_Name").ToString)
                BindToText(tbSpecificationDt, Dr("Specification").ToString)
                BindToText(tbSpecificationDt2, Dr("Specification2").ToString)
                BindToText(tbPriceForexDt, Dr("Price").ToString)
                BindToDropList(ddlUnitWrhsDt, Dr("Unit").ToString)
                BindToDropList(ddlUnitOrderDt, Dr("UnitOrder").ToString)
                lbUnitPack.Text = Dr("UnitPack").ToString
                lbUnitM2.Text = Dr("UnitM2").ToString
                BindToDropList(ddlUnitCommision, Dr("Unit").ToString)
                tbPriceForexDt.Focus()
            Else
                tbProductDt.Text = ""
                tbProductNameDt.Text = ""
                tbSpecificationDt.Text = ""
                tbSpecificationDt2.Text = ""
                tbPriceForexDt.Text = FormatNumber(0, ViewState("DigitCurr"))
                lbUnitPack.Text = ""
                lbUnitM2.Text = ""
                ddlUnitCommision.SelectedValue = ""
                ddlUnitOrderDt.SelectedValue = ""
                tbProductDt.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tb Customer change Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub btnAddDtSO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDtSO.Click
        'Dim dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If lbProductDt2.Text = "" Or lbProductDt2.Text = "&nbsp;" Then
            '    Exit Sub
            'End If
            Cleardt()

            'dr = ViewState("Dt2").Select("Product=" + QuotedStr(tbProductDt.Text))(0)
            'tbQtyOrderDt2.Text = dr("QtyOrder").ToString
            'ddlUnitOrderDt2.SelectedValue = dr("UnitOrder").ToString
            'tbDeliveryDateDt2.SelectedDate = CDate(dr("DeliveryDate"))

            ViewState("StateDt") = "Insert"
            btnProductDt.Enabled = True
            tbProductDt.Enabled = True
            ddlUnitOrderDt.Enabled = True
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbProductDt.Focus()

        Catch ex As Exception
            Throw New Exception("btnAddDtSO_Click Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub ddlSOKind_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSOKind.SelectedIndexChanged
        Try
            If ddlSOKind.SelectedIndex = 0 Then
                tbIRRNo.Text = ""
                tbCustPONo.Text = ""
                btnIRRNo.Enabled = False
                tbCustPONo.Enabled = True
                ddlTerm.Enabled = True
                ddlCurrHd.Enabled = True
                tbPPN.Text = "10"
                tbPPN.Enabled = True
                'tbDisc.Text = "0"
                'tbDisc.Enabled = True
                tbDP.Text = "0"
                tbDPForex.Text = "0"
                tbDP.Enabled = True
                tbDPForex.Enabled = True

            ElseIf ddlSOKind.SelectedIndex = 1 Then
                tbIRRNo.Text = ""
                tbCustPONo.Text = ""
                btnIRRNo.Enabled = True
                tbCustPONo.Enabled = False
                ddlTerm.Enabled = False
                ddlCurrHd.Enabled = False
                tbPPN.Enabled = False
                'tbDisc.Text = "0"
                'tbDisc.Enabled = False
                tbDP.Text = "0"
                tbDPForex.Text = "0"
                tbDP.Enabled = False
                tbDPForex.Enabled = False
            Else
                tbIRRNo.Text = ""
                tbCustPONo.Text = ""
                btnIRRNo.Enabled = False
                tbCustPONo.Enabled = False
                ddlTerm.Enabled = False
                ddlCurrHd.Enabled = False
                tbPPN.Text = "0"
                tbPPN.Enabled = False
                'tbDisc.Text = "0"
                'tbDisc.Enabled = False
                tbDP.Text = "0"
                tbDPForex.Text = "0"
                tbDP.Enabled = False
                tbDPForex.Enabled = False

            End If
        Catch ex As Exception
            Throw New Exception("ddlSOKind_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnbackSO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbackSO.Click, btnbackSO2.Click, btnbackPR.Click, btnbackPR0.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub tbPriceForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForexDt.TextChanged
        Try
            If tbPriceForexDt.Text = "" Then
                tbPriceForexDt.Text = "0"
            End If
            ViewState("DigitCurr") = 2 'GetCurrDigit(ddlCurrHd.SelectedValue, ViewState("DBConnection").ToString)
            If ddlUnitOrderDt.SelectedValue = "M2" Then
                tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyM2Dt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
            ElseIf ddlUnitOrderDt.SelectedValue = "Roll" Then
                tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyRollDt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
            Else
                tbAmountForexDt.Text = FormatNumber(CFloat(tbQtyOrderDt.Text) * CFloat(tbPriceForexDt.Text), ViewState("DigitCurr"))
            End If
            tbDiscForexDt.Text = FormatNumber(CFloat(tbDiscDt.Text) * CFloat(tbAmountForexDt.Text) / 100, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber(CFloat(tbAmountForexDt.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
            tbPriceForexDt.Text = FormatNumber(tbPriceForexDt.Text, ViewState("DigitCurr"))
            
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            tbRemarkDt.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
        'If ddlUnitOrderDt.SelectedIndex = 0 Then
        '    tbAmountForexDt.Text = tbQtyDt.Text * tbPriceForexDt.Text
        'ElseIf ddlUnitOrderDt.SelectedIndex = 1 Then
        '    tbAmountForexDt.Text = tbQtyM2Dt.Text * tbPriceForexDt.Text
        'Else
        '    tbAmountForexDt.Text = tbQtyRollDt.Text * tbPriceForexDt.Text
        'End If
        'tbTotalForexDt.Text = tbAmountForexDt.Text - tbDiscForexDt.Text
    End Sub

    Protected Sub lbProductGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProductGroup.Click
        ViewState("InputProductGroup") = "Y"


        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProductGrp')();", Page, Me.GetType())
    End Sub

    Protected Sub lbCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        ViewState("InputCustomer") = "Y"

        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
    End Sub


    Protected Sub tbDiscDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscDt.TextChanged
        Try
            If tbDiscDt.Text = "" Then
                tbDiscDt.Text = "0"
            End If
            tbDiscForexDt.Text = FormatNumber(CFloat(tbDiscDt.Text) * CFloat(tbAmountForexDt.Text) / 100, ViewState("DigitCurr"))
            tbDiscDt.Text = FormatFloat(tbDiscDt.Text, ViewState("DigitPercent"))
            tbTotalForexDt.Text = FormatNumber(CFloat(tbAmountForexDt.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbDiscDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDiscForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscForexDt.TextChanged
        Try
            If tbDiscForexDt.Text = "" Then
                tbDiscForexDt.Text = "0"
            End If
            tbDiscDt.Text = FormatFloat(CFloat(tbDiscForexDt.Text) / CFloat(tbAmountForexDt.Text) * 100, ViewState("DigitPercent"))
            tbDiscForexDt.Text = FormatNumber(tbDiscForexDt.Text, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatNumber(CFloat(tbAmountForexDt.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbDiscDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub ddlfgInclude_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlfgInclude.SelectedIndexChanged
        Try
            If ddlfgInclude.SelectedValue = "N" Then
                ViewState("FactorRate") = 1
                'tbBaseForex.Text = FormatNumber(Total + tbDiscForex.Text, CInt(ViewState("DigitCurr")))
            Else
                ViewState("FactorRate") = 100 / (100 + FormatFloat(tbPPN.Text, ViewState("DigitQty")))
            End If
            tbBaseForex.Text = FormatNumber(CFloat(tbBaseForex.Text.Replace(",", "")) * ViewState("FactorRate"), CInt(ViewState("DigitCurr")))
            tbDiscForex.Text = FormatNumber(CFloat(tbDiscForex.Text.Replace(",", "")) * ViewState("FactorRate"), CInt(ViewState("DigitCurr")))
        Catch ex As Exception
            lbStatus.Text = "ddlfgInclude_SelectedIndexChanged Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbcommision_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcommision.TextChanged
        Try
            tbcommision.Text = FormatNumber(tbcommision.Text, ViewState("DigitCurr"))
            tbRemarkDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbcommision_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnitDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitOrderDt.SelectedIndexChanged
        Try
            tbPriceForexDt_TextChanged(Nothing, Nothing)
            'Dim dtUnit As DataTable
            'Dim drUnit As DataRow
            'Try
            '    ddlUnitCommision.SelectedValue = ddlUnitDt.SelectedValue
            '    Dim QtyResult As Double
            '    QtyResult = SQLExecuteScalar("EXEC S_MKSOGetPrice '" + tbCustCode.Text + "', '" + ddlCurrHd.SelectedValue + "', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbProductDt.Text) + ", " + QuotedStr(ddlUnitDt.SelectedValue), ViewState("DBConnection"))
            '    tbPriceForexDt.Text = FormatFloat(QtyResult, ViewState("DigitQty"))
            '    tbPriceForexDt_TextChanged(Nothing, Nothing)
            '    tbDiscDt_TextChanged(Nothing, Nothing)

            '    dtUnit = SQLExecuteQuery("Select UnitPerM2, UnitPerRoll from VMsProduct WHERE Product_Code = " + QuotedStr(tbProductDt.Text), ViewState("DBConnection")).Tables(0)
            '    drUnit = dtUnit.Rows(0)

            '    tbQtyPerRoll.Text = drUnit("UnitPerRoll")
            '    tbQtyPerM2.Text = drUnit("UnitPerM2")
            '    Dim Qty, QtyRoll, QtyM2 As Double
            '    'If CFloat(drUnit("UnitPerRoll")) = 0 Then
            '    '    tbQtyRollDt.Text = FormatFloat(0, ViewState("DigitQty"))
            '    'Else
            '    '    tbQtyRollDt.Text = FormatFloat(CFloat(tbQtyDt.Text) / CFloat(drUnit("UnitPerRoll")), ViewState("DigitQty"))
            '    'End If
            '    'If CFloat(drUnit("UnitPerM2")) = 0 Then
            '    '    tbQtyM2Dt.Text = FormatFloat(0, ViewState("DigitQty"))
            '    'Else
            '    '    tbQtyM2Dt.Text = FormatFloat(CFloat(tbQtyDt.Text) / CFloat(drUnit("UnitPerM2")), ViewState("DigitQty"))
            '    'End If
            '    Qty = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProductDt.Text) + ", " + QuotedStr(ddlUnitDt.SelectedValue) + ", " + tbQtyOrder.Text.Replace(",", "") + ", " + QuotedStr(ddlUnitDt.SelectedValue) + ")", ViewState("DBConnection"))
            '    tbQtyDt.Text = FormatNumber(Qty, ViewState("DigitQty"))
            '    QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProductDt.Text) + ", " + QuotedStr(ddlUnitDt.SelectedValue) + ", " + tbQtyOrder.Text.Replace(",", "") + ", 'Roll')", ViewState("DBConnection"))
            '    tbQtyRollDt.Text = FormatNumber(QtyRoll, ViewState("DigitQty"))
            '    QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProductDt.Text) + ", " + QuotedStr(ddlUnitDt.SelectedValue) + ", " + tbQtyOrder.Text.Replace(",", "") + ", 'M2')", ViewState("DBConnection"))
            '    tbQtyM2Dt.Text = FormatNumber(QtyM2, ViewState("DigitQty"))

            'Catch ex As Exception
            '    lbStatus.Text = "ddlUnitDt_SelectedIndexChanged Error : " + ex.ToString
            'End Try
        Catch ex As Exception
            lbStatus.Text = "ddlUnitDt_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyPackDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyPackDt2.TextChanged
        Try
            Dim Qty As Double
            Qty = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(lbProductDt2.Text) + ", " + QuotedStr(ddlUnitOrderDt2.SelectedValue) + ", " + tbQtyPackDt2.Text.Replace(",", "") + ", " + QuotedStr(ddlUnitPackDt2.SelectedValue) + " )", ViewState("DBConnection"))
            tbQtyOrderDt2.Text = FormatFloat(Qty, ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbQtyPackDt2_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnitPackDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitPackDt2.SelectedIndexChanged
        Try
            tbQtyPackDt2_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "tbQtyPackDt2_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyOrderDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrderDt2.TextChanged
        Try
            Dim Qty As Double
            Qty = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(lbProductDt2.Text) + ", " + QuotedStr(ddlUnitPackDt2.SelectedValue) + ", " + tbQtyOrderDt2.Text.Replace(",", "") + ", " + QuotedStr(ddlUnitOrderDt2.SelectedValue) + " )", ViewState("DBConnection"))
            tbQtyPackDt2.Text = FormatFloat(Qty, ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbQtyPackDt2_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
