Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPriceCOGS_TrPriceCOGS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            If Not IsPostBack Then
                InitProperty()
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today
                FillCombo(ddlUnit, "Select UnitCode,UnitName From MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
                FillCombo(ddlCurr, "Select CurrCode,CurrName From MsCurrency", False, "CurrCode", "CurrName", ViewState("DBConnection"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                dsUnit.ConnectionString = ViewState("DBConnection")
                pnlService.Visible = True
                BindData()
            End If

            dsUnit.ConnectionString = ViewState("DBConnection")

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProduct" Then
                    TbProduct.Text = Session("Result")(0).ToString
                    TbProductName.Text = Session("Result")(1).ToString
                    ddlUnit.SelectedValue = Session("Result")(2).ToString
                
                    ds = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProduct.Text), ViewState("DBConnection").ToString)
                    dr = ds.Tables(0).Rows(0)
                    BindToText(TbLastPrice, FormatFloat(dr("Price"), 2))
                    BindToText(TbLastEffective, Format(dr("EffectiveDate"), "yyyy-MM-dd"))
                    tbPrice.Text = FormatFloat(dr("Price"), 2)
                End If

            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
            tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
            TbPrise.Attributes.Add("OnKeyDown", "return PressNumeric();")
            lbstatus.Text = ""
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
            'lbstatus.Text = StrFilter + " " + Request.QueryString("MenuParam")
            'Exit Sub

            tempDS = SQLExecuteQuery("EXEC S_MsPriceListView " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
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

                lbProd = GVR.FindControl("Product")
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
                    lbstatus.Text = "Price must in numeric format"
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
                            tgl = Format(tbDate.SelectedDate, "yyyy-MM-dd")
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
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
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

    Protected Sub BtnNew_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNew.ServerClick
        PnlMain.Visible = False
        pnlService.Visible = False
        tbType.Text = Request.QueryString("MenuParam")

        pnlInput.Visible = True
        tbEffectiveDate.SelectedDate = ViewState("ServerDate")
        ddlCurr.SelectedValue = ViewState("Currency")

        ClearHd()
    End Sub

    Protected Sub btnCancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.ServerClick
        PnlMain.Visible = True
        pnlService.Visible = True
        pnlInput.Visible = False
        ClearHd()
        BindData()
    End Sub


    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")

            Session("filter") = "Select Product_Code, Product_Name, ProductType_Name, Product_SubGroup, ProductSubGrp_Name, Unit " + _
                                 "from VMsproduct WHERE Fg_Stock = 'Y' AND Fg_Active = 'Y' "
            FieldResult = "Product_Code, Product_Name, Unit"
            
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnProduct"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnProduct Error : " + ex.ToString
        End Try
    End Sub


    'Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
    '    Try
    '        ClearHd()
    '    Catch ex As Exception
    '        lbstatus.Text = "btnReset_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnReset_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.ServerClick
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
            TbProduct.Text = ""
            TbProductName.Text = ""
            ddlUnit.SelectedIndex = 0
            ddlCurr.SelectedValue = ViewState("Currency")
            TbLastPrice.Text = ""
            TbLastEffective.Text = ""
            tbPrice.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear HD error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub TbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbProduct.TextChanged
        Dim dr, drprice As DataRow
        Dim dt, dtprice As DataTable
        Dim StrSearch As String
        Try
            StrSearch = "Select Product_Code, Product_Name, ProductType_Name, Product_SubGroup, ProductSubGrp_Name, Unit " + _
                                 "from VMsproduct WHERE Fg_Stock = 'Y' AND Fg_Active = 'Y' And Product_Code = " + QuotedStr(TbProduct.Text)
            dt = SQLExecuteQuery(StrSearch, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count <= 0 Then
                TbProduct.Text = ""
                TbProductName.Text = ""
                ddlUnit.SelectedIndex = 0
                TbLastPrice.Text = ""
                TbLastEffective.Text = ""
                tbPrice.Text = ""
            Else
                dr = dt.Rows(0)
                BindToText(TbProduct, dr("Product_Code").ToString)
                BindToText(TbProductName, dr("Product_Name").ToString)
                ddlUnit.SelectedValue = dr("Unit").ToString

                dtprice = SQLExecuteQuery("EXEC S_MsPriceListGetInfo " + QuotedStr(TbProduct.Text), ViewState("DBConnection").ToString).Tables(0)
                drprice = dtprice.Rows(0)
                BindToText(TbLastPrice, FormatFloat(drprice("Price").ToString, 2))
                BindToText(TbLastEffective, Format(drprice("EffectiveDate"), "yyyy-MM-dd"))
                tbPrice.Text = FormatFloat(drprice("Price").ToString, 2)
            End If
        Catch ex As Exception
            lbstatus.Text = "TbProduct_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSave_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.ServerClick
        Dim SQLString As String = ""
        Dim CodeName As String = ""
        Try
            If TbProduct.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Product Must Have Value")
                TbProduct.Focus()
                Exit Sub
            End If
            If ddlCurr.SelectedValue.Trim = "" Then
                lbstatus.Text = MessageDlg("Currency Must Have Value")
                ddlCurr.Focus()
                Exit Sub
            End If
            If CFloat(tbPrice.Text) <= 0 Then
                lbstatus.Text = MessageDlg("Price Must Have Value")
                tbPrice.Focus()
                Exit Sub
            End If
            If ddlUnit.SelectedValue.Trim = "" Then
                lbstatus.Text = MessageDlg("Unit Must Have Value")
                ddlUnit.Focus()
                Exit Sub
            End If

            If SQLExecuteScalar("SELECT Product From MsPriceList WHERE EffectiveDate = " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyyMMdd")) + " And Product = " + QuotedStr(TbProduct.Text), ViewState("DBConnection").ToString).Length > 0 Then
                lbstatus.Text = MessageDlg("Data has already been exist")
                Exit Sub
            End If

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
                    'price = Replace(tbPrice.Text, ".", "")
                    price = Replace(tbPrice.Text, ",", "")

                    SQLString = "INSERT INTO MsPriceList (Product, Unit, Currency, EffectiveDate, Price) " + _
                                " SELECT " + QuotedStr(TbProduct.Text) + ", " + _
                                QuotedStr(ddlUnit.SelectedValue) + ", " + _
                                QuotedStr(ddlCurr.SelectedValue) + ", " + _
                                QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                                price
                    'QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd"))
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                Else
                    lbstatus.Text = MessageDlg("Effective Date must be greater then last effective")
                    Exit Sub
                End If
            Else
                Dim price As String
                price = Replace(tbPrice.Text, ",", "")
                'price = Replace(price, ",", "")

                SQLString = "INSERT INTO MsPriceList (Product, Unit, Currency, EffectiveDate, Price) " + _
                            "SELECT " + QuotedStr(TbProduct.Text) + ", " + _
                            QuotedStr(ddlUnit.SelectedValue) + ", " + _
                            QuotedStr(ddlCurr.SelectedValue) + ", " + _
                            QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                            price
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

    'Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
    '    Dim SQLString As String
    '    Dim StrFilter As String

    '    Try
    '        If ddlType.SelectedIndex = 0 Then
    '            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

    '            Session("DBConnection") = ViewState("DBConnection")
    '            'StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '            Session("PrintType") = "Print"
    '            'SQLString = "exec S_MsRentalPriceListPrint " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter)
    '            SQLString = "exec S_FormPriceListPrintService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))

    '            'lbstatus.Text = SQLString
    '            'SQLString = Replace(SQLString, "PayCode", "Payment_Code")
    '            'SQLString = Replace(SQLString, "PayName", "Payment_Name")
    '            'SQLString = Replace(SQLString, "CurrCode", "Currency")
    '            Session("SelectCommand") = SQLString
    '            Session("ReportFile") = ".../../../Rpt/RptPrintMsPriceService.frx"
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        Else
    '            Session("DBConnection") = ViewState("DBConnection")
    '            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '            Session("PrintType") = "Print"
    '            SQLString = "Exec S_FormPriceListPrintTrading " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))
    '            'SQLString = Replace(SQLString, "PayCode", "Payment_Code")
    '            'SQLString = Replace(SQLString, "PayName", "Payment_Name")
    '            'SQLString = Replace(SQLString, "CurrCode", "Currency")
    '            Session("SelectCommand") = SQLString
    '            Session("ReportFile") = ".../../../Rpt/RptPrintMsPriceTrading.frx"
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "BtnPrint_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub BtnPrint_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.ServerClick
        Dim SQLString As String
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"

            SQLString = "EXEC S_MsPriceListView " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter)

            'lbstatus.Text = SQLString
            'SQLString = Replace(SQLString, "PayCode", "Payment_Code")
            'SQLString = Replace(SQLString, "PayName", "Payment_Name")
            'SQLString = Replace(SQLString, "CurrCode", "Currency")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPriceCOGS.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "BtnPrint_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
    '    Try
    '        tbfilter2.Text = ""
    '        If pnlSearch.Visible Then
    '            pnlSearch.Visible = False
    '        Else
    '            pnlSearch.Visible = True
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "btn Expand Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnExpand_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.ServerClick
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

    Protected Sub BtnGo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.ServerClick
        Dim GVR As GridViewRow
        Dim Product As Label
        Dim AddType, Addnominal As String
        Dim CB As CheckBox
        Dim SQLString As String

        If ddlSatuan.SelectedIndex = 0 And TbPrise.Text = "" Then
            lbstatus.Text = MessageDlg("Additional Percentage Price must have value")
            TbPrise.Focus()
            Exit Sub
        End If
        If ddlSatuan.SelectedIndex = 1 And TbPrise.Text = "" Then
            lbstatus.Text = MessageDlg("Additional Nominal Price must have value")
            TbPrise.Focus()
            Exit Sub
        End If

        Try
            AddType = ddlSatuan.SelectedItem.ToString
            Addnominal = TbPrise.Text.Replace(",", "")

            For Each GVR In DataGrid.Rows
                'CB = GVR.FindControl("FgInput")
                CB = GVR.FindControl("cbSelect")
                If CB.Checked = True Then
                    Product = GVR.FindControl("Product")
                    SQLString = "EXEC S_MsPriceListChange " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + "," + QuotedStr(Product.Text) + ", " + QuotedStr(AddType) + "," + Addnominal
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next

            BindData()
            TbPrise.Text = ""
        Catch ex As Exception
            lbstatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtProduct As Label
        Dim SQLString, tgl As String

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtProduct = DataGrid.Rows(e.RowIndex).FindControl("Product")
            tgl = Format(tbDate.SelectedDate, "yyyy-MM-dd")

            SQLString = "EXEC S_MsPriceListDelete " + QuotedStr(txtProduct.Text) + "," + QuotedStr(tgl)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            BindData()

        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim Product As Label
        Dim tgl As String
        Dim txtPrice As TextBox
        Dim SQLString As String
        
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            'BindData()

            obj = DataGrid.Rows(e.NewEditIndex)

            txtPrice = obj.FindControl("Price")
            txtPrice.Text = Replace(txtPrice.Text, ",", "")
            Product = obj.FindControl("Product")
            tgl = Format(tbDate.SelectedDate, "yyyy-MM-dd")
            DataGrid.EditIndex = -1
            
            SQLString = "EXEC S_MsPriceListApply " + QuotedStr(Product.Text) + "," + QuotedStr(tgl) + "," + txtPrice.Text
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            BindData()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit Error :  " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    Try
    '        If ddlType.SelectedIndex = 0 Then
    '            DataGrid.PageIndex = 0
    '            DataGrid.EditIndex = -1
    '            BindData()
    '        Else
    '            DataGridTrad.PageIndex = 0
    '            DataGridTrad.EditIndex = -1
    '            BindDataTrading()
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSearch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.ServerClick
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(DataGrid, sender)
    End Sub


End Class
