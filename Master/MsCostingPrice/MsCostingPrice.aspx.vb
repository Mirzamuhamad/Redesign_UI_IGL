Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_MsCostingPrice_MsCostingPrice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            'If Request.QueryString("ContainerId").ToString = "MsPriceListID" Then
            '    Labelmenu.Text = "PRODUCT PRICE"
            'ElseIf Request.QueryString("ContainerId").ToString = "MsPriceHPPID" Then
            '    Labelmenu.Text = "HPP PRICE"
            'End If

            If Not IsPostBack Then
                InitProperty()
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today
                'FillCombo(ddlUnit, "Select UnitCode,UnitName From MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
                'FillCombo(ddlCurr, "Select CurrCode,CurrName From MsCurrency", False, "CurrCode", "CurrName", ViewState("DBConnection"))
                'FillCombo(ddlUnitTrad, "Select UnitCode,UnitName From MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
                'FillCombo(ddlCurrTrad, "Select CurrCode,CurrName From MsCurrency", False, "CurrCode", "CurrName", ViewState("DBConnection"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                'ddlType.SelectedIndex = 0

                'If ddlType.SelectedIndex = 0 Then
                '    ddlField.Items.Clear()
                '    ddlField.Items.Add(New ListItem("Type", "PriceType"))
                '    ddlField.Items.Add(New ListItem("Product Code", "Product"))
                '    ddlField.Items.Add(New ListItem("Product Name", "ProductName"))
                '    ddlField.Items.Add(New ListItem("Area Service", "AreaServiceName"))

                '    ddlField2.Items.Clear()
                '    ddlField2.Items.Add(New ListItem("Type", "PriceType"))
                '    ddlField2.Items.Add(New ListItem("Product Code", "Product"))
                '    ddlField2.Items.Add(New ListItem("Product Name", "ProductName"))
                '    ddlField2.Items.Add(New ListItem("Area Service", "AreaServiceName"))
                'End If

                dsUnit.ConnectionString = ViewState("DBConnection")
                pnlService.Visible = True
                BindData()
            End If

            dsUnit.ConnectionString = ViewState("DBConnection")

            If Not Session("Result") Is Nothing Then
                'If ViewState("Sender") = "btnPSubGroup" Then
                '    'FieldResult = "Product_SubGroup_Code, Product_SubGroup_Name, Product_Group_Code, Product_Group_Name"
                '    tbPSubGroup.Text = Session("Result")(0).ToString
                '    tbPSubGroupName.Text = Session("Result")(1).ToString
                '    tbProductGroup.Text = Session("Result")(3).ToString
                'End If
                'If ViewState("Sender") = "btnCustomer" Then
                '    'FieldResult = "Customer_Code, Customer_Name"
                '    tbCustCode.Text = Session("Result")(0).ToString
                '    tbCustName.Text = Session("Result")(1).ToString
                'End If
                If ViewState("Sender") = "btnCosting" Then
                    'FieldResult = "Customer_Code, Customer_Name"
                    'If TbProduct.Text <> "" Or TbProduct.Text <> Session("Result")(0).ToString Then
                    TbCosting.Text = Session("Result")(0).ToString
                    TbCostingName.Text = Session("Result")(1).ToString
                    'ddlUnit.SelectedValue = Session("Result")(2).ToString
                    'ViewState("") = ""
                    'End If
                End If
                'If ViewState("Sender") = "btnProductTrading" Then
                '    'FieldResult = "Customer_Code, Customer_Name"
                '    TbProductTrad.Text = Session("Result")(0).ToString
                '    TbProductNameTrad.Text = Session("Result")(1).ToString
                '    ddlUnitTrad.SelectedValue = Session("Result")(2).ToString
                'End If

                If ViewState("Sender") = "btnSheet" Then
                    'FieldResult = "Customer_Code, Customer_Name"
                    TbSheet.Text = Session("Result")(0).ToString
                    TbSheetName.Text = Session("Result")(1).ToString
                End If
                'If ViewState("Sender") = "btnRegional" Then
                '    'FieldResult = "Customer_Code, Customer_Name"
                '    TbRegional.Text = Session("Result")(0).ToString
                '    TbRegionalName.Text = Session("Result")(1).ToString
                'End If

                'If ddlType.SelectedIndex = 0 Then
                ds = SQLExecuteQuery("EXEC S_MsCostingListGetInfo " + QuotedStr(TbCosting.Text) + "," + QuotedStr(TbSheet.Text), ViewState("DBConnection").ToString)
                'ElseIf ddlType.SelectedIndex = 1 Then
                '    ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProductTrad.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurrTrad.SelectedValue) + "," + "'1'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)
                'End If

                If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                    TbLastPrice.Text = ""
                    TbLastEffective.Text = ""
                    tbPrice.Text = ""
                Else
                    dr = ds.Tables(0).Rows(0)
                    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
                    TbLastPrice.Text = FormatFloat(TbLastPrice.Text, ViewState("DigitQty"))
                    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                    tbPrice.Text = Replace(dr("Price"), ".0000", "")
                End If

                'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                '    If ddlType.SelectedIndex = 0 Then
                '        TbLastPrice.Text = ""
                '        TbLastEffective.Text = ""
                '        tbPrice.Text = ""
                '    ElseIf ddlType.SelectedIndex = 1 Then
                '        TbLastPriceTrad.Text = ""
                '        TbLastEffectiveTrad.Text = ""
                '        tbPriceTrad.Text = ""
                '    End If
                'Else
                '    If ddlType.SelectedIndex = 0 Then
                '        dr = ds.Tables(0).Rows(0)
                '        BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
                '        BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                '        tbPrice.Text = Replace(dr("Price"), ".0000", "")
                '    ElseIf ddlType.SelectedIndex = 1 Then
                '        dr = ds.Tables(0).Rows(0)
                '        BindToText(TbLastPriceTrad, Replace(dr("Price"), ".0000", ""))
                '        BindToText(TbLastEffectiveTrad, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                '        tbPriceTrad.Text = Replace(dr("Price"), ".0000", "")
                '    End If
                'End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
            TbPrise.Attributes.Add("OnKeyDown", "return PressNumeric();")
            lbstatus.Text = ""
            'getValuePost = ""
        Catch ex As Exception
            lbstatus.Text = "Page Load Error : " + ex.ToString
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim tempDS As New DataSet()
        Dim GVR As GridViewRow 'GridViewRow
        Dim Price As TextBox
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If

            'lbstatus.Text = "EXEC S_MsPriceListViewService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))
            'Exit Sub

            tempDS = SQLExecuteQuery("EXEC S_MsCostingPriceListView " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            'QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd"))
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            'GVR = DataGrid.FindControl("Price")
            'Price = GVR.FindControl("Price")
            'Price.Attributes.Add("OnKeyDown", "return PressNumeric();")

            For Each GVR In DataGrid.Rows
                Price = GVR.FindControl("Price")
                Price.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    'Sub BindDataTrading()
    '    Dim tempDS As New DataSet()
    '    Dim GVR As GridViewRow 'GridViewRow
    '    Dim Price As TextBox
    '    Dim StrFilter As String

    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

    '        'tempDS = SQLExecuteQuery("EXEC S_MsSalesPriceListView " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")), ViewState("DBConnection").ToString)
    '        tempDS = SQLExecuteQuery("EXEC S_MsPriceListViewTrading " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)
    '        'QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd"))
    '        DataGridTrad.DataSource = tempDS.Tables(0)
    '        DataGridTrad.DataBind()

    '        For Each GVR In DataGridTrad.Rows
    '            Price = GVR.FindControl("Price")
    '            Price.Attributes.Add("OnKeyDown", "return PressNumeric();")
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim SQLString As String
        'Dim lbAdd, lbCode As Label
        Dim lbCust, lbProd, lbArea, lbCurr, lbDate As Label
        'Dim tbRate, tbTaxRate As TextBox
        Dim tbPrice As TextBox
        Dim GVR As GridViewRow
        Dim DDLInput As DropDownList
        Dim i As Integer
        Dim exe As Boolean

        Try
            exe = True

            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)

                lbCust = GVR.FindControl("Customer")
                lbProd = GVR.FindControl("Product")
                lbArea = GVR.FindControl("AreaService")
                lbCurr = GVR.FindControl("Currency")
                lbDate = GVR.FindControl("tbLastDate")
                tbPrice = GVR.FindControl("Price")
                tbPrice.Text = tbPrice.Text.Replace(".0000", "")

                'tbTaxRate = GVR.FindControl("TaxRate")
                'tbRate.Text = tbRate.Text.Replace(",", "")
                'tbTaxRate.Text = tbTaxRate.Text.Replace(",", "")

                If tbPrice.Text.Trim = "" Then
                    tbPrice.Text = "0"
                End If
                'If tbTaxRate.Text.Trim = "" Then
                '    tbTaxRate.Text = "0"
                'End If

                If Not IsNumeric(tbPrice.Text) Then
                    lbstatus.Text = "Price for " + lbCurr.Text + " must in numeric format"
                    exe = False
                    tbPrice.Focus()
                    Exit For
                End If
                'If Not IsNumeric(tbTaxRate.Text) Then
                '    lbstatus.Text = "Rate for " + lbCode.Text + " must in numeric format"
                '    exe = False
                '    tbTaxRate.Focus()
                '    Exit For
                'End If
            Next
            If exe Then
                For i = 0 To DataGrid.Rows.Count - 1
                    GVR = DataGrid.Rows(i)

                    DDLInput = GVR.FindControl("FgInput")
                    lbCust = GVR.FindControl("Customer")
                    lbProd = GVR.FindControl("Product")
                    lbArea = GVR.FindControl("AreaService")
                    lbCurr = GVR.FindControl("Currency")
                    lbDate = GVR.FindControl("tbLastDate")
                    tbPrice = GVR.FindControl("Price")
                    tbPrice.Text = tbPrice.Text.Replace(".0000", "")
                    'tbPrice.Text=

                    If DDLInput.Text = "Y" Then
                        SQLString = "INSERT INTO MsRentalPriceList (Customer,Product,AreaService,Currency,EffectiveDate,Price)  SELECT " + QuotedStr(lbCust.Text) + ", " + _
                        QuotedStr(lbProd.Text) + ", " + _
                        QuotedStr(lbArea.Text) + ", " + _
                        QuotedStr(lbCurr.Text) + ", " + _
                        QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                        Replace(tbPrice.Text, ",", "")
                        'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                    Else
                        Dim tgl As DateTime
                        If lbDate.Text = "" Then
                            tgl = Format(tbDate.SelectedDate, "yyyy-dd-MM")
                        Else
                            tgl = CDate(lbDate.Text)
                        End If
                        SQLString = "UPDATE MsRentalPriceList SET Price =" + Replace(tbPrice.Text, ",", "") + _
                        " WHERE Customer=" + QuotedStr(lbCust.Text) + _
                        " AND Product = " + QuotedStr(lbProd.Text) + _
                        " AND AreaService = " + QuotedStr(lbArea.Text) + _
                        " AND Currency = " + QuotedStr(lbCurr.Text) + _
                        " AND EffectiveDate = " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
                    End If
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
                BindData()
            End If

        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            'BindData()
            'If ddlType.SelectedIndex = 0 Then
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
            'Else
            'DataGridTrad.PageIndex = 0
            'DataGridTrad.EditIndex = -1
            'BindDataTrading()
            'End If
        Catch ex As Exception
            lbstatus.Text = "Date Selection Changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub CurrRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim tb As TextBox
    '    Dim Count As Integer
    '    'Dim dgi As GridViewRow

    '    Try
    '        tb = sender
    '        If tb.ID = "Price" Then
    '            Count = DataGrid.EditIndex
    '            'lbstatus.Text = CStr(Count)
    '            'Exit Sub
    '            'dgi = DataGrid.Rows(Count) '-1 for allowpaging = False   - 2 allowpaging = True
    '            'tbRate = dgi.FindControl("CurrRate")
    '            'tbTaxRate = dgi.FindControl("TaxRate")
    '            'tbTaxRate.Text = tbRate.Text
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = ex.ToString
    '    End Try
    'End Sub

    'Protected Sub CurrRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CurrRate.TextChanged
    'Dim CurrRate, tb As TextBox
    'Dim dgi As GridViewRow
    'Dim Count As Integer
    '   Try
    '      tb = sender
    '     Count = DataGrid.SelectedValue
    '    dgi = DataGrid.Rows(Count)
    '   CurrRate = dgi.FindControl("CurrRate")

    'Catch ex As Exception
    '   lbstatus.Text = ex.ToString
    'End Try
    'End Sub

    Protected Sub BtnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNew.Click
        PnlMain.Visible = False
        pnlService.Visible = False
        'pnlTrading.Visible = False
        tbType.Text = Request.QueryString("MenuParam")
        'If ddlType.SelectedIndex = 0 Then
        pnlInput.Visible = True
        'pnlInputTrading.Visible = False
        tbEffectiveDate.SelectedDate = ViewState("ServerDate")
        'ddlPriceType.SelectedIndex = 0
        'ddlCurr.SelectedValue = ViewState("Currency")
        'Else
        'pnlInput.Visible = False
        'pnlInputTrading.Visible = True
        'tbEffDateTrad.SelectedDate = ViewState("ServerDate")
        'ddlCurrTrad.SelectedValue = ViewState("Currency")
        'ddlPriceTypeTrad.SelectedIndex = 1
        'End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        PnlMain.Visible = True
        pnlService.Visible = True
        'pnlTrading.Visible = False
        pnlInput.Visible = False
        'pnlInputTrading.Visible = False
        ClearHd()
        BindData()
    End Sub

    'Protected Sub btnCancelTrad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelTrad.Click
    '    PnlMain.Visible = True
    '    pnlService.Visible = False
    '    'pnlTrading.Visible = True
    '    pnlInput.Visible = False
    '    pnlInputTrading.Visible = False
    '    ClearHdTrad()
    '    'BindDataTrading()
    'End Sub

    'Protected Sub btnCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomer.Click
    '    Dim FieldResult As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM VMsCustomer WHERE FgActive = 'Y'"
    '        FieldResult = "Customer_Code, Customer_Name"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnCustomer"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnCustomer Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnCosting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCosting.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")

            'If ddlPriceType.SelectedIndex = 0 Then
            Session("filter") = "SELECT CostingCode, CostingName FROM MsCosting"
            FieldResult = "CostingCode, CostingName"
            'ElseIf ddlPriceType.SelectedIndex = 1 Then
            '    Session("filter") = "SELECT * FROM V_MsProductTrade Where FgActive='Y'"
            '    FieldResult = "Product, Product_Name, Unit"
            'End If

            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnCosting"
        Catch ex As Exception
            lbstatus.Text = "btnProduct Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnProductTrad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductTrad.Click
    '    Dim FieldResult As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")


    '        If ddlPriceTypeTrad.SelectedIndex = 1 Then
    '            Session("filter") = "SELECT * FROM V_MsProductTrade Where FgActive='Y'"
    '            FieldResult = "Product, Product_Name, Unit"
    '        ElseIf ddlPriceTypeTrad.SelectedIndex = 0 Then
    '            Session("filter") = "SELECT * FROM V_MsRental Where FgActive='Y'"
    '            FieldResult = "Rental, Rental_Name, Unit"
    '        End If

    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnProductTrading"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnProductTrad_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSheet.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Sheet, SheetName FROM MsSheet"
            FieldResult = "Sheet, SheetName"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnSheet"
        Catch ex As Exception
            lbstatus.Text = "btnAreaService Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnRegional_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegional.Click
    '    Dim FieldResult As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT RegionalCode, RegionalName FROM MsRegional"
    '        FieldResult = "RegionalCode, RegionalName"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnRegional"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnRegional Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearHd()
        Catch ex As Exception
            lbstatus.Text = "btnReset_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnResetTrad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetTrad.Click
    '    Try
    '        ClearHdTrad()
    '    Catch ex As Exception
    '        lbstatus.Text = "btnResetTrad_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub ClearHd()
        Try
            'tbEffectiveDate.Text = ""
            'tbCustCode.Text = ""
            'tbCustName.Text = ""
            'ddlPriceType.SelectedIndex = 0
            'TbProduct.Text = ""
            'TbProductName.Text = ""
            'TbAreaService.Text = ""
            'TbAreaServiceName.Text = ""
            'ddlUnit.SelectedIndex = 0
            'ddlCurr.SelectedValue = ViewState("Currency")
            TbCosting.Text = ""
            TbCostingName.Text = ""
            TbSheet.Text = ""
            TbSheetName.Text = ""
            TbLastPrice.Text = ""
            TbLastEffective.Text = ""
            tbPrice.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear HD error : " + ex.ToString)
        End Try
    End Sub

    'Private Sub ClearHdTrad()
    '    Try
    '        ddlPriceTypeTrad.SelectedIndex = 1
    '        TbProductTrad.Text = ""
    '        TbProductNameTrad.Text = ""
    '        TbRegional.Text = ""
    '        TbRegionalName.Text = ""
    '        ddlUnitTrad.SelectedIndex = 0
    '        ddlCurrTrad.SelectedValue = ViewState("Currency")
    '        TbLastPriceTrad.Text = ""
    '        TbLastEffectiveTrad.Text = ""
    '        tbPriceTrad.Text = ""
    '    Catch ex As Exception
    '        Throw New Exception("ClearHdTrad error : " + ex.ToString)
    '    End Try
    'End Sub

    'Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    'Dim Dr As DataRow
    '    'Dim DT As DataTable

    '    Try
    '        ds = SQLExecuteQuery("Select * From VMsCustomer WHERE Customer_Code = " + QuotedStr(tbCustCode.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            tbCustCode.Text = ""
    '            tbCustName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(tbCustCode, dr("Customer_Code").ToString)
    '            BindToText(tbCustName, dr("Customer_Name").ToString)
    '        End If

    '        ds = Nothing

    '        ds = SQLExecuteQuery("EXEC S_MsRentalPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbLastPrice.Text = ""
    '            TbLastEffective.Text = ""
    '            tbPrice.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '            BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '            tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "tbCustCode Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub TbCosting_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbCosting.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * FROM MsCosting Where CostingCode = " + QuotedStr(TbCosting.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                TbCosting.Text = ""
                TbCostingName.Text = ""
                'ddlUnit.SelectedIndex = 0
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(TbCosting, dr("CostingCode").ToString)
                BindToText(TbCostingName, dr("CostingName").ToString)
                'ddlUnit.SelectedValue = dr("Unit").ToString
            End If


            ds = Nothing
            ds = SQLExecuteQuery("EXEC S_MsCostingListGetInfo " + QuotedStr(TbCosting.Text) + "," + QuotedStr(TbSheet.Text), ViewState("DBConnection").ToString)
            'ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + "'0'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                TbLastPrice.Text = ""
                TbLastEffective.Text = ""
                tbPrice.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
                TbLastPrice.Text = FormatFloat(TbLastPrice.Text, ViewState("DigitQty"))
                BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                tbPrice.Text = Replace(dr("Price"), ".0000", "")
            End If

            'ds = SQLExecuteQuery("EXEC S_MsRentalPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
            'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
            '    TbLastPrice.Text = ""
            '    TbLastEffective.Text = ""
            '    tbPrice.Text = ""
            'Else
            '    dr = ds.Tables(0).Rows(0)
            '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
            '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
            '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
            'End If
        Catch ex As Exception
            lbstatus.Text = "TbProduct_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub TbProductTrad_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbProductTrad.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try

    '        ds = SQLExecuteQuery("SELECT * FROM V_MsProductTrade Where FgActive='Y' And Product = " + QuotedStr(TbProductTrad.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbProductTrad.Text = ""
    '            TbProductNameTrad.Text = ""
    '            ddlUnitTrad.SelectedIndex = 0
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbProductTrad, dr("Product").ToString)
    '            BindToText(TbProductNameTrad, dr("Product_Name").ToString)
    '            ddlUnitTrad.SelectedValue = dr("Unit").ToString
    '        End If

    '        ds = Nothing
    '        ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProductTrad.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurrTrad.SelectedValue) + "," + "'1'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbLastPriceTrad.Text = ""
    '            TbLastEffectiveTrad.Text = ""
    '            tbPriceTrad.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbLastPriceTrad, Replace(dr("Price"), ".0000", ""))
    '            BindToText(TbLastEffectiveTrad, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '            tbPriceTrad.Text = Replace(dr("Price"), ".0000", "")
    '        End If

    '        'ds = SQLExecuteQuery("EXEC S_MsRentalPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
    '        'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '        '    TbLastPrice.Text = ""
    '        '    TbLastEffective.Text = ""
    '        '    tbPrice.Text = ""
    '        'Else
    '        '    dr = ds.Tables(0).Rows(0)
    '        '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '        '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '        '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        'End If
    '    Catch ex As Exception
    '        lbstatus.Text = "TbProductTrad_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub TbSheet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbSheet.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * FROM MsSheet Where Sheet = " + QuotedStr(TbSheet.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                TbSheet.Text = ""
                TbSheetName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(TbSheet, dr("Sheet").ToString)
                BindToText(TbSheetName, dr("SheetName").ToString)
            End If

            ds = Nothing
            ds = SQLExecuteQuery("EXEC S_MsCostingListGetInfo " + QuotedStr(TbCosting.Text) + "," + QuotedStr(TbSheet.Text), ViewState("DBConnection").ToString)
            'ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + "'0'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                TbLastPrice.Text = ""
                TbLastEffective.Text = ""
                tbPrice.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
                TbLastPrice.Text = FormatFloat(TbLastPrice.Text, ViewState("DigitQty"))
                BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                tbPrice.Text = Replace(dr("Price"), ".0000", "")
            End If

            'ds = SQLExecuteQuery("EXEC S_MsRentalPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
            'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
            '    TbLastPrice.Text = ""
            '    TbLastEffective.Text = ""
            '    tbPrice.Text = ""
            'Else
            '    dr = ds.Tables(0).Rows(0)
            '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
            '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
            '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
            'End If
        Catch ex As Exception
            lbstatus.Text = "TbAreaService Changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub TbRegional_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbRegional.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Try
    '        ds = SQLExecuteQuery("SELECT * FROM MsRegional Where RegionalCode = " + QuotedStr(TbRegional.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbRegional.Text = ""
    '            TbRegionalName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbRegional, dr("RegionalCode").ToString)
    '            BindToText(TbRegionalName, dr("RegionalName").ToString)
    '        End If

    '        ds = Nothing

    '        ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProductTrad.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurrTrad.SelectedValue) + "," + "'1'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbLastPriceTrad.Text = ""
    '            TbLastEffectiveTrad.Text = ""
    '            tbPriceTrad.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbLastPriceTrad, Replace(dr("Price"), ".0000", ""))
    '            BindToText(TbLastEffectiveTrad, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '            tbPriceTrad.Text = Replace(dr("Price"), ".0000", "")
    '        End If

    '        'ds = SQLExecuteQuery("EXEC S_MsSalesPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
    '        'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '        '    TbLastPrice.Text = ""
    '        '    TbLastEffective.Text = ""
    '        '    tbPrice.Text = ""
    '        'Else
    '        '    dr = ds.Tables(0).Rows(0)
    '        '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '        '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '        '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        'End If
    '    Catch ex As Exception
    '        lbstatus.Text = "TbRegional_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + "'0'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbLastPrice.Text = ""
    '            TbLastEffective.Text = ""
    '            tbPrice.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '            BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '            tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        End If

    '        'ds = SQLExecuteQuery("EXEC S_MsRentalPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbAreaService.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
    '        'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '        '    TbLastPrice.Text = ""
    '        '    TbLastEffective.Text = ""
    '        '    tbPrice.Text = ""
    '        'Else
    '        '    dr = ds.Tables(0).Rows(0)
    '        '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '        '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '        '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        'End If
    '    Catch ex As Exception
    '        lbstatus.Text = "ddlCurr_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlCurrTrad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrTrad.SelectedIndexChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProductTrad.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurrTrad.SelectedValue) + "," + "'1'" + "," + QuotedStr(Request.QueryString("MenuParam")), ViewState("DBConnection").ToString)

    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            TbLastPriceTrad.Text = ""
    '            TbLastEffectiveTrad.Text = ""
    '            tbPriceTrad.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(TbLastPriceTrad, Replace(dr("Price"), ".0000", ""))
    '            BindToText(TbLastEffectiveTrad, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '            tbPriceTrad.Text = Replace(dr("Price"), ".0000", "")
    '        End If

    '        'ds = SQLExecuteQuery("EXEC S_MsSalesPriceListGetInfo " + QuotedStr(tbCustCode.Text) + "," + QuotedStr(TbProduct.Text) + "," + QuotedStr(TbRegional.Text) + "," + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString)
    '        'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '        '    TbLastPrice.Text = ""
    '        '    TbLastEffective.Text = ""
    '        '    tbPrice.Text = ""
    '        'Else
    '        '    dr = ds.Tables(0).Rows(0)
    '        '    BindToText(TbLastPrice, Replace(dr("Price"), ".0000", ""))
    '        '    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
    '        '    tbPrice.Text = Replace(dr("Price"), ".0000", "")
    '        'End If
    '    Catch ex As Exception
    '        lbstatus.Text = "ddlCurrTrad_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SQLString As String = ""
        Dim CodeName As String = ""
        Try
            'If tbCustCode.Text.Trim = "" Then
            '    lbstatus.Text = "Customer Must Have Value"
            '    tbCustCode.Focus()
            '    Exit Sub
            'End If
            If TbCosting.Text.Trim = "" Then
                lbstatus.Text = "Costing Must Have Value"
                TbCosting.Focus()
                Exit Sub
            End If
            If TbSheet.Text.Trim = "" Then
                lbstatus.Text = "Sheet Must Have Value"
                TbSheet.Focus()
                Exit Sub
            End If
            'If ddlCurr.SelectedValue.Trim = "" Then
            '    lbstatus.Text = "Currency Must Have Value"
            '    ddlCurr.Focus()
            '    Exit Sub
            'End If
            If tbPrice.Text.Trim = "" Then
                lbstatus.Text = "Price Must Have Value"
                tbPrice.Focus()
                Exit Sub
            End If
            'If ddlUnit.SelectedValue.Trim = "" Then
            '    lbstatus.Text = "Unit Must Have Value"
            '    ddlUnit.Focus()
            '    Exit Sub
            'End If

            'If SQLExecuteScalar("SELECT PriceType,Product,Area,Unit,Currency From MsPriceList WHERE EffectiveDate = " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyyMMdd")) + " And PriceType = " + QuotedStr(ddlPriceType.SelectedItem.ToString.TrimStart.TrimEnd) + " And Product = " + QuotedStr(TbProduct.Text) + " And Area = " + QuotedStr(TbAreaService.Text) + " And Currency = " + QuotedStr(ddlCurr.SelectedValue) + " And Unit = " + QuotedStr(ddlUnit.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
            '    lbstatus.Text = "Data has already been exist"
            '    Exit Sub
            'End If
            'If SQLExecuteScalar("SELECT PriceType,Product,Area,Unit,Currency From MsPriceList WHERE EffectiveDate = " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyyMMdd")) + " And PriceType = " + QuotedStr(ddlPriceType.SelectedItem.ToString.TrimStart.TrimEnd) + " And Product = " + QuotedStr(TbProduct.Text) + " And Area = " + QuotedStr(TbAreaService.Text) + " And Currency = " + QuotedStr(ddlCurr.SelectedValue) + " And Unit = " + QuotedStr(ddlUnit.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
            '    lbstatus.Text = "Data has already been exist"
            '    Exit Sub
            'End If

            Dim tgl As String
            Dim tgl2 As Date

            If TbLastEffective.Text <> "" Then
                tgl = TbLastEffective.Text
                tgl2 = CDate(tgl)
            End If

            If TbLastPrice.Text <> "" And TbLastEffective.Text <> "" Then
                If tbEffectiveDate.SelectedDate > tgl2 Then
                    'QuotedStr(Format(tbDate.SelectedDate, "MM/dd/yyyy"))

                    'If SQLExecuteScalar("EXEC S_CekProductNameSpec " + QuotedStr(tbName.Text) + "," + QuotedStr(tbSpecification.Text), Session("DBConnection").ToString).Length > 0 Then
                    '    lstatus.Text = "Product Name " + QuotedStr(tbName.Text) + " and Specification " + QuotedStr(tbSpecification.Text) + " has already been exist"
                    '    Exit Sub
                    'End If
                    Dim price As String
                    price = Replace(tbPrice.Text, ".", "")
                    price = Replace(price, ",", "")

                    SQLString = "INSERT INTO MsCostingPrice (EffectiveDate,Costing,Sheet,Price,UserId,UserDate) SELECT " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                                QuotedStr(TbCosting.Text) + ", " + _
                                QuotedStr(TbSheet.Text) + ", " + _
                                price + ", " + _
                                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                    'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                Else
                    lbstatus.Text = "Effective Date must be greater then last effective"
                    Exit Sub
                End If
            Else
                Dim price As String
                price = Replace(tbPrice.Text, ".", "")
                price = Replace(price, ",", "")

                SQLString = "INSERT INTO MsCostingPrice (EffectiveDate,Costing,Sheet,Price,UserId,UserDate) SELECT " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                                QuotedStr(TbCosting.Text) + ", " + _
                                QuotedStr(TbSheet.Text) + ", " + _
                                price + ", " + _
                                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If

            BindData()
            ClearHd()
            pnlInput.Visible = False
            pnlService.Visible = True
            PnlMain.Visible = True
        Catch ex As Exception
            lbstatus.Text = "BtnSave_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub BtnSaveTrad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveTrad.Click
    '    Dim SQLString As String = ""
    '    Dim CodeName As String = ""
    '    Try
    '        'If tbCustCode.Text.Trim = "" Then
    '        '    lbstatus.Text = "Customer Must Have Value"
    '        '    tbCustCode.Focus()
    '        '    Exit Sub
    '        'End If
    '        If TbProductTrad.Text.Trim = "" Then
    '            lbstatus.Text = "Product Must Have Value"
    '            TbProductTrad.Focus()
    '            Exit Sub
    '        End If
    '        If TbRegional.Text.Trim = "" Then
    '            lbstatus.Text = "Regional Must Have Value"
    '            TbRegional.Focus()
    '            Exit Sub
    '        End If
    '        If ddlCurrTrad.SelectedValue.Trim = "" Then
    '            lbstatus.Text = "Currency Must Have Value"
    '            ddlCurrTrad.Focus()
    '            Exit Sub
    '        End If
    '        If tbPriceTrad.Text.Trim = "" Then
    '            lbstatus.Text = "Price Must Have Value"
    '            tbPriceTrad.Focus()
    '            Exit Sub
    '        End If
    '        If ddlUnitTrad.SelectedValue.Trim = "" Then
    '            lbstatus.Text = "Unit Must Have Value"
    '            ddlUnitTrad.Focus()
    '            Exit Sub
    '        End If

    '        If SQLExecuteScalar("SELECT PriceType,Product,Area,Unit,Currency From MsPriceList WHERE EffectiveDate = " + QuotedStr(Format(tbEffDateTrad.SelectedDate, "yyyyMMdd")) + " And PriceType = " + QuotedStr(ddlPriceTypeTrad.SelectedItem.ToString.TrimStart.TrimEnd) + " And Product = " + QuotedStr(TbProductTrad.Text) + " And Area = " + QuotedStr(TbRegional.Text) + " And Currency = " + QuotedStr(ddlCurrTrad.SelectedValue) + " And Unit = " + QuotedStr(ddlUnitTrad.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
    '            lbstatus.Text = "Data has already been exist"
    '            Exit Sub
    '        End If

    '        Dim tgl As String
    '        Dim tgl2 As Date

    '        If TbLastEffectiveTrad.Text <> "" Then
    '            tgl = TbLastEffectiveTrad.Text
    '            tgl2 = CDate(tgl)
    '        End If

    '        If TbLastPriceTrad.Text <> "" And TbLastEffectiveTrad.Text <> "" Then
    '            If tbEffDateTrad.SelectedDate > tgl2 Then
    '                'QuotedStr(Format(tbDate.SelectedDate, "MM/dd/yyyy"))

    '                'If SQLExecuteScalar("EXEC S_CekProductNameSpec " + QuotedStr(tbName.Text) + "," + QuotedStr(tbSpecification.Text), Session("DBConnection").ToString).Length > 0 Then
    '                '    lstatus.Text = "Product Name " + QuotedStr(tbName.Text) + " and Specification " + QuotedStr(tbSpecification.Text) + " has already been exist"
    '                '    Exit Sub
    '                'End If
    '                Dim price As String
    '                price = Replace(tbPriceTrad.Text, ".", "")
    '                price = Replace(price, ",", "")

    '                SQLString = "INSERT INTO MsPriceList (PriceType,Product,Area,Unit,Currency,EffectiveDate,Price,Type) SELECT " + QuotedStr(ddlPriceTypeTrad.SelectedItem.ToString.TrimStart.TrimEnd) + ", " + _
    '                            QuotedStr(TbProductTrad.Text) + ", " + _
    '                            QuotedStr(TbRegional.Text) + ", " + _
    '                            QuotedStr(ddlUnitTrad.SelectedValue) + ", " + _
    '                            QuotedStr(ddlCurrTrad.SelectedValue) + ", " + _
    '                            QuotedStr(Format(tbEffDateTrad.SelectedDate, "yyyy-MM-dd")) + ", " + _
    '                            price + ", " + _
    '                            QuotedStr(tbType.Text)
    '                'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
    '                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    '            Else
    '                lbstatus.Text = "Effective Date must be greater then last effective"
    '                Exit Sub
    '            End If
    '        Else
    '            Dim price As String
    '            price = Replace(tbPriceTrad.Text, ".", "")
    '            price = Replace(price, ",", "")

    '            SQLString = "INSERT INTO MsPriceList (PriceType,Product,Area,Unit,Currency,EffectiveDate,Price,Type) SELECT " + QuotedStr(ddlPriceTypeTrad.SelectedItem.ToString.TrimStart.TrimEnd) + ", " + _
    '                        QuotedStr(TbProductTrad.Text) + ", " + _
    '                        QuotedStr(TbRegional.Text) + ", " + _
    '                        QuotedStr(ddlUnitTrad.SelectedValue) + ", " + _
    '                        QuotedStr(ddlCurrTrad.SelectedValue) + ", " + _
    '                        QuotedStr(Format(tbEffDateTrad.SelectedDate, "yyyy-MM-dd")) + ", " + _
    '                        price + ", " + _
    '                        QuotedStr(tbType.Text)
    '            'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
    '            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    '        End If

    '        'BindDataTrading()
    '        ClearHd()
    '        pnlInputTrading.Visible = False
    '        'pnlTrading.Visible = True
    '        PnlMain.Visible = True
    '    Catch ex As Exception
    '        lbstatus.Text = "BtnSaveTrad_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Dim SQLString As String
        Dim StrFilter As String

        Try
            'If ddlType.SelectedIndex = 0 Then
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            Session("DBConnection") = ViewState("DBConnection")
            'StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            'SQLString = "exec S_MsRentalPriceListPrint " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter)
            'SQLString = "exec S_FormPriceListPrintService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))
            SQLString = "exec S_FormCostingPricePrintService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter) '+ "," + QuotedStr(Request.QueryString("MenuParam"))

            'lbstatus.Text = SQLString
            'SQLString = Replace(SQLString, "PayCode", "Payment_Code")
            'SQLString = Replace(SQLString, "PayName", "Payment_Name")
            'SQLString = Replace(SQLString, "CurrCode", "Currency")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsPriceService.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            'Else
            '    Session("DBConnection") = ViewState("DBConnection")
            '    StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            '    Session("PrintType") = "Print"
            '    SQLString = "Exec S_FormPriceListPrintTrading " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))
            '    'SQLString = Replace(SQLString, "PayCode", "Payment_Code")
            '    'SQLString = Replace(SQLString, "PayName", "Payment_Name")
            '    'SQLString = Replace(SQLString, "CurrCode", "Currency")
            '    Session("SelectCommand") = SQLString
            '    Session("ReportFile") = ".../../../Rpt/RptPrintMsPriceTrading.frx"
            '    AttachScript("openprintdlg();", Page, Me.GetType)
            'End If
        Catch ex As Exception
            lbstatus.Text = "BtnPrint_Click Error : " + ex.ToString
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
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Dim GVR As GridViewRow
        'Dim PriceType, Product, Area, Reg, Curr As Label
        Dim costing, sheet As Label
        Dim Type, nominal As String
        'Dim CB As DropDownList
        Dim CB As CheckBox
        Dim tb As TextBox
        Dim SQLString As String
        'Dim unit As String
        'Dim unit1 As Label
        'Dim unit As DropDownList

        If TbPrise.Text = "" Then
            If ddlSatuan.SelectedIndex = 0 Then
                lbstatus.Text = "Percent Price must have value"
                TbPrise.Focus()
                Exit Sub
            Else
                lbstatus.Text = "Nominal Price must have value"
                TbPrise.Focus()
                Exit Sub
            End If
        End If

        Try
            'If ddlType.SelectedIndex = 0 Then
            For Each GVR In DataGrid.Rows
                'CB = GVR.FindControl("FgInput")
                CB = GVR.FindControl("cbSelect")
                tb = GVR.FindControl("Price")
                tb.Text = Replace(tb.Text, ",", "")
                Type = ddlSatuan.SelectedItem.ToString
                nominal = TbPrise.Text

                If CB.Checked = True Then
                    costing = GVR.FindControl("Costing")
                    sheet = GVR.FindControl("Sheet")

                    'PriceType = GVR.FindControl("PriceType")
                    'Product = GVR.FindControl("Product")
                    'Area = GVR.FindControl("AreaService")
                    'Curr = GVR.FindControl("Currency")
                    'unit = GVR.Cells(8).Text.TrimStart.TrimEnd
                    'unit1 = GVR.FindControl("Unit")

                    'SQLString = "EXEC S_MsPriceChangeService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(PriceType.Text) + "," + QuotedStr(Product.Text) + "," + QuotedStr(Area.Text) + "," + QuotedStr(Curr.Text) + "," + QuotedStr(Type) + "," + nominal + "," + QuotedStr(unit1.Text) + "," + QuotedStr(Request.QueryString("MenuParam"))
                    SQLString = "EXEC S_MsCostingPriceChangeService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(costing.Text) + "," + QuotedStr(sheet.Text) + "," + QuotedStr(Type) + "," + nominal + "," + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next

            BindData()
            TbPrise.Text = ""
            'Else
            'For Each GVR In DataGridTrad.Rows
            '    'CB = GVR.FindControl("FgInput")
            '    CB = GVR.FindControl("cbSelect")
            '    tb = GVR.FindControl("Price")
            '    unit = GVR.FindControl("ddlUnitEdit")
            '    tb.Text = Replace(tb.Text, ",", "")
            '    Type = ddlSatuan.SelectedItem.ToString
            '    nominal = TbPrise.Text

            '    If CB.Checked = True Then
            '        PriceType = GVR.FindControl("PriceType")
            '        Product = GVR.FindControl("Product")
            '        Reg = GVR.FindControl("Regional")
            '        Curr = GVR.FindControl("Currency")

            '        SQLString = "EXEC S_MsPriceChangeTrading " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(PriceType.Text) + "," + QuotedStr(Product.Text) + "," + QuotedStr(Reg.Text) + "," + QuotedStr(Curr.Text) + "," + QuotedStr(Type) + "," + nominal + "," + QuotedStr(unit.SelectedValue) + "," + QuotedStr(Request.QueryString("MenuParam"))
            '        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            '    End If
            'Next

            'BindDataTrading()
            'TbPrise.Text = ""
            'End If
        Catch ex As Exception
            lbstatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        'Dim PriceType, Product, Area, Curr As Label
        Dim Costing, Sheet As Label
        Dim tgl As String
        Dim tgl2 As Date
        Dim txt As TextBox
        Dim SQLString As String
        'Dim unit1 As Label
        'Dim unitReplace As DropDownList

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            'BindData()

            obj = DataGrid.Rows(e.NewEditIndex)

            txt = obj.FindControl("Price")
            txt.Text = Replace(txt.Text, ",", "")
            Costing = obj.FindControl("Costing")
            Sheet = obj.FindControl("Sheet")

            'PriceType = obj.FindControl("PriceType")
            'Product = obj.FindControl("Product")
            'Area = obj.FindControl("AreaService")
            'unit1 = obj.FindControl("Unit")
            'unitReplace = obj.FindControl("ddlUnitEdit")
            'Curr = obj.FindControl("Currency")

            'tgl = obj.Cells(7).Text
            'If tgl = "&nbsp;" Then
            '    Exit Sub
            'End If
            'tgl2 = CDate(tgl)
            DataGrid.EditIndex = -1

            SQLString = "EXEC S_MsCostingPriceApply " + QuotedStr(Costing.Text) + "," + QuotedStr(Sheet.Text) + "," + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + txt.Text + "," + QuotedStr(ViewState("UserId").ToString)
            'SQLString = "EXEC S_MsPriceApply " + QuotedStr(PriceType.Text) + "," + QuotedStr(Product.Text) + "," + QuotedStr(Area.Text) + "," + QuotedStr(Curr.Text) + "," + QuotedStr(Format(tgl2, "yyyy-MM-dd")) + "," + txt.Text + "," + QuotedStr(unit1.Text) + "," + QuotedStr(unitReplace.SelectedValue)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            BindData()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit Error :  " + ex.ToString
        End Try
    End Sub

    'Protected Sub DataGridTrad_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridTrad.PageIndexChanging
    '    DataGridTrad.PageIndex = e.NewPageIndex
    '    BindDataTrading()
    'End Sub

    'Protected Sub DataGridTrad_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridTrad.RowEditing
    '    Dim obj As GridViewRow
    '    Dim PriceType, Product, Reg, Curr As Label
    '    Dim tgl As String
    '    Dim tgl2 As Date
    '    Dim txt As TextBox
    '    Dim Unit As Label
    '    Dim unitReplace As DropDownList
    '    Dim SQLString As String

    '    Try
    '        If CheckMenuLevel("Edit") = False Then
    '            Exit Sub
    '        End If

    '        DataGridTrad.EditIndex = e.NewEditIndex
    '        DataGridTrad.ShowFooter = False
    '        'BindData()

    '        obj = DataGridTrad.Rows(e.NewEditIndex)

    '        txt = obj.FindControl("Price")
    '        txt.Text = Replace(txt.Text, ",", "")
    '        PriceType = obj.FindControl("PriceType")
    '        Product = obj.FindControl("Product")
    '        Reg = obj.FindControl("Regional")
    '        Unit = obj.FindControl("Unit2")
    '        unitReplace = obj.FindControl("ddlUnitEdit")
    '        Curr = obj.FindControl("Currency")
    '        tgl = obj.Cells(10).Text
    '        tgl2 = CDate(tgl)
    '        DataGridTrad.EditIndex = -1

    '        SQLString = "EXEC S_MsPriceApply " + QuotedStr(PriceType.Text) + "," + QuotedStr(Product.Text) + "," + QuotedStr(Reg.Text) + "," + QuotedStr(Curr.Text) + "," + QuotedStr(Format(tgl2, "yyyy-MM-dd")) + "," + txt.Text + "," + QuotedStr(Unit.Text) + "," + QuotedStr(unitReplace.SelectedValue)

    '        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    '        BindDataTrading()
    '    Catch ex As Exception
    '        lbstatus.Text = "DataGrid_Edit Error :  " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            'If ddlType.SelectedIndex = 0 Then
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
            'Else
            '    DataGridTrad.PageIndex = 0
            '    DataGridTrad.EditIndex = -1
            '    BindDataTrading()
            'End If
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(DataGrid, sender)
    End Sub

    'Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
    '    If ddlType.SelectedIndex = 0 Then
    '        pnlService.Visible = True
    '        pnlTrading.Visible = False

    '        ddlField.Items.Clear()
    '        ddlField.Items.Add(New ListItem("Type", "PriceType"))
    '        ddlField.Items.Add(New ListItem("Product Code", "Product"))
    '        ddlField.Items.Add(New ListItem("Product Name", "ProductName"))
    '        ddlField.Items.Add(New ListItem("Area Service", "AreaServiceName"))

    '        ddlField2.Items.Clear()
    '        ddlField2.Items.Add(New ListItem("Type", "PriceType"))
    '        ddlField2.Items.Add(New ListItem("Product Code", "Product"))
    '        ddlField2.Items.Add(New ListItem("Product Name", "ProductName"))
    '        ddlField2.Items.Add(New ListItem("Area Service", "AreaServiceName"))

    '        BindData()
    '    Else
    '        pnlService.Visible = False
    '        pnlTrading.Visible = True

    '        ddlField.Items.Clear()
    '        ddlField.Items.Add(New ListItem("Type", "PriceType"))
    '        ddlField.Items.Add(New ListItem("Product Code", "Product"))
    '        ddlField.Items.Add(New ListItem("Product Name", "ProductName"))
    '        ddlField.Items.Add(New ListItem("Regional Name", "RegionalName"))

    '        ddlField2.Items.Clear()
    '        ddlField2.Items.Add(New ListItem("Type", "PriceType"))
    '        ddlField2.Items.Add(New ListItem("Product Code", "Product"))
    '        ddlField2.Items.Add(New ListItem("Product Name", "ProductName"))
    '        ddlField2.Items.Add(New ListItem("Regional Name", "RegionalName"))

    '        BindDataTrading()
    '    End If
    'End Sub

    'Protected Sub ddlPriceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriceType.SelectedIndexChanged
    '    TbProduct.Text = ""
    '    TbProductName.Text = ""
    '    TbAreaService.Text = ""
    '    TbAreaServiceName.Text = ""
    '    ddlUnit.SelectedIndex = 0
    '    If ddlPriceType.SelectedIndex = 1 Then
    '        pnlInput.Visible = False
    '        'pnlInputTrading.Visible = True
    '        'tbEffDateTrad.SelectedDate = ViewState("ServerDate")
    '        'ddlCurrTrad.SelectedValue = ViewState("Currency")
    '        'ddlPriceTypeTrad.SelectedIndex = 1
    '        'ddlType.SelectedIndex = 1
    '    End If
    'End Sub

    'Protected Sub ddlPriceTypeTrad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriceTypeTrad.SelectedIndexChanged
    '    TbProductTrad.Text = ""
    '    TbProductNameTrad.Text = ""
    '    TbRegional.Text = ""
    '    TbRegionalName.Text = ""
    '    ddlUnitTrad.SelectedIndex = 0
    '    If ddlPriceTypeTrad.SelectedIndex = 0 Then
    '        pnlInput.Visible = True
    '        pnlInputTrading.Visible = False
    '        tbEffectiveDate.SelectedDate = ViewState("ServerDate")
    '        ddlPriceType.SelectedIndex = 0
    '        ddlCurr.SelectedValue = ViewState("Currency")
    '        ddlType.SelectedIndex = 0
    '    End If
    'End Sub
End Class
