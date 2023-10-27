Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports BasicFrame.WebControls

Partial Class Transaction_TrCIPOffeRecv_TrCIPOffeRecv
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT * FROM V_FINCIPOffeRecvHd"
    Public strTransNmbr As String
    Private Shared PageSize As Integer = 5
    Dim tb As TextBox

    'Private Function GetStringDt(ByVal Nmbr As String) As String
    '    Return "SELECT * FROM FINCIPInstrJobDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    'End Function

    'Private Function GetStringDt2(ByVal Nmbr As String) As String
    '    Return "SELECT * FROM V_PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                'BindDataToGridView("S_PRCFindCIPRecvDoc", GVNoRecvDoc, ViewState("DBConnection").ToString)
                'BindGridArea()
                'BindGridCIPLicenRecvDoc()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupplier1" Then
                    tbSuppCode1.Text = Session("Result")(0).ToString
                    tbSuppName1.Text = Session("Result")(1).ToString
                    'BindToText(tbPPn, Session("Result")(1).ToString, ViewState("DigitCurr"))
                    'BindToText(tbBaseForex, Session("Result")(2).ToString, ViewState("DigitCurr"))
                    'BindToText(tbDisc, Session("Result")(3).ToString, ViewState("DigitCurr"))
                    'BindToText(tbPPh, Session("Result")(4).ToString, ViewState("DigitCurr"))
                    'BindToText(tbPPnValue, Session("Result")(5).ToString, ViewState("DigitCurr"))
                    'BindToText(tbRemark, Session("Result")(6).ToString)
                End If
                If ViewState("Sender") = "btnSupplier2" Then
                    tbSuppCode2.Text = Session("Result")(0).ToString
                    tbSuppName2.Text = Session("Result")(1).ToString
                    'ddlCurrDt2.SelectedValue = Session("Result")(1).ToString
                    'tbRateDt2.Text = Session("Result")(2).ToString
                    'tbPaymentForexDt2.Text = (CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString)).ToString
                    'tbRateDt2.Enabled = False
                    'If tbNoLandPurchase.Text.Trim = "" Then
                    '    BindToDropList(ddlUserType, Session("Result")(5).ToString)
                    '    BindToText(tbNoLandPurchase, Session("Result")(6).ToString)
                    '    BindToText(tbUserName, Session("Result")(7).ToString)
                    'End If
                    'AttachScript("setformatdt()", Page, Me.GetType)
                End If
                If ViewState("Sender") = "btnSupplier3" Then
                    tbSuppCode3.Text = Session("Result")(0).ToString
                    tbSuppName3.Text = Session("Result")(1).ToString
                    'tbStartJobDate.Text = Session("Result")(3).ToString
                    'BindToDate(tbStartJobDate, Dt.Rows(0)("ApplfileDate").ToString)
                    'BindToDate(tbStartJobDate, Session("Result")(3).ToString)
                    'BindToDropList(ddlCurrDt, Session("Result")(3).ToString)
                    'tbFgType.Text = Session("Result")(4).ToString.ToUpper
                    'tbHGBNo.Text = Session("Result")(4).ToString
                    'tbRemark.Text = Session("Result")(8).ToString
                    'ddlCurrDt.Enabled = (tbFgType.Text = "PL")
                    'tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"
                    'btnSubled.Visible = tbSubledDt.Enabled
                    'ViewState("FgType") = tbFgType.Text

                    'tbPPnNo.Enabled = (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2"))
                    'tbPPndate.Enabled = tbPPnNo.Enabled

                    'If ViewState("FgType") = "BS" Then
                    '    ddlCurrDt.Enabled = False
                    'Else : ddlCurrDt.Enabled = True
                    'End If

                    'ddlCostCenterDt.Enabled = tbFgCostCtr.Text <> "N"
                    'If tbFgSubledDt.Text = "N" Then
                    '    tbSubledDt.Text = ""
                    '    tbSubledNameDt.Text = ""
                    'End If
                    'If tbFgCostCtr.Text = "N" Then
                    '    ddlCostCenterDt.SelectedIndex = 0
                    'End If
                    'btnSubled.Enabled = tbSubledDt.Enabled()

                    'ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                    'ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
                    'ChangeFgSubLed(tbFgSubledDt, tbSubledDt, btnSubled)
                    'AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
                End If
                If ViewState("Sender") = "btnSupplier4" Then
                    tbSuppCode4.Text = Session("Result")(0).ToString
                    tbSuppName4.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnSupplier5" Then
                    tbSuppCode5.Text = Session("Result")(0).ToString
                    tbSuppName5.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnInstruksiNo" Then
                    tbInstruksiNo.Text = Session("Result")(0).ToString
                    'tbBaseForex.Text = Session("Result")(5).ToString 'ViewState("DigitHome")
                    BindToText(tbBaseForex, Session("Result")(5).ToString, ViewState("DigitHome"))
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
                'fupOfferFile1.Attributes("onchange") = "fileUploadOfferFile1(this)"
            End If

            'If Session("FileUpload1") Is Nothing AndAlso fupOfferFile1.HasFile Then
            '    Session("FileUpload1") = fupOfferFile1
            '    lbOfferFile1.Text = fupOfferFile1.FileName
            'ElseIf Session("FileUpload1") IsNot Nothing AndAlso (Not fupOfferFile1.HasFile) Then
            '    fupOfferFile1 = DirectCast(Session("FileUpload1"), FileUpload)
            '    lbOfferFile1.Text = fupOfferFile1.FileName
            'ElseIf fupOfferFile1.HasFile Then
            '    Session("FileUpload1") = fupOfferFile1
            '    lbOfferFile1.Text = fupOfferFile1.FileName
            'End If

            'If Session("FileUpload2") Is Nothing AndAlso fupOfferFile2.HasFile Then
            '    Session("FileUpload2") = fupOfferFile2
            '    lbOfferFile2.Text = fupOfferFile2.FileName
            'ElseIf Session("FileUpload2") IsNot Nothing AndAlso (Not fupOfferFile2.HasFile) Then
            '    fupOfferFile2 = DirectCast(Session("FileUpload2"), FileUpload)
            '    lbOfferFile2.Text = fupOfferFile2.FileName
            'ElseIf fupOfferFile2.HasFile Then
            '    Session("FileUpload2") = fupOfferFile2
            '    lbOfferFile2.Text = fupOfferFile2.FileName
            'End If

            'If Session("FileUpload3") Is Nothing AndAlso fupOfferFile3.HasFile Then
            '    Session("FileUpload3") = fupOfferFile3
            '    lbOfferFile3.Text = fupOfferFile3.FileName
            'ElseIf Session("FileUpload3") IsNot Nothing AndAlso (Not fupOfferFile3.HasFile) Then
            '    fupOfferFile3 = DirectCast(Session("FileUpload3"), FileUpload)
            '    lbOfferFile3.Text = fupOfferFile3.FileName
            'ElseIf fupOfferFile3.HasFile Then
            '    Session("FileUpload3") = fupOfferFile3
            '    lbOfferFile3.Text = fupOfferFile3.FileName
            'End If

            'If Session("FileUpload4") Is Nothing AndAlso fupOfferFile4.HasFile Then
            '    Session("FileUpload4") = fupOfferFile4
            '    lbOfferFile4.Text = fupOfferFile4.FileName
            'ElseIf Session("FileUpload4") IsNot Nothing AndAlso (Not fupOfferFile4.HasFile) Then
            '    fupOfferFile4 = DirectCast(Session("FileUpload4"), FileUpload)
            '    lbOfferFile4.Text = fupOfferFile4.FileName
            'ElseIf fupOfferFile4.HasFile Then
            '    Session("FileUpload4") = fupOfferFile4
            '    lbOfferFile4.Text = fupOfferFile4.FileName
            'End If

            'If Session("FileUpload5") Is Nothing AndAlso fupOfferFile5.HasFile Then
            '    Session("FileUpload5") = fupOfferFile5
            '    lbOfferFile5.Text = fupOfferFile5.FileName
            'ElseIf Session("FileUpload5") IsNot Nothing AndAlso (Not fupOfferFile5.HasFile) Then
            '    fupOfferFile5 = DirectCast(Session("FileUpload5"), FileUpload)
            '    lbOfferFile5.Text = fupOfferFile5.FileName
            'ElseIf fupOfferFile5.HasFile Then
            '    Session("FileUpload5") = fupOfferFile5
            '    lbOfferFile5.Text = fupOfferFile5.FileName
            'End If

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

    Private Sub SetInit()
        'Dim strSQL As String
        Try
            FillRange(ddlRange)
            'lbPayHome.Text = "Notaris (" + ViewState("Currency") + ")"'
            'lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            'lbDisc.Text = "Potongan" '(" + ViewState("Currency") + ")"
            'lbPPnForex.Text = "PPn Value" '(" + ViewState("Currency") + ")"
            'lbPPn.Text = "PPn %" '(" + ViewState("Currency") + ")"
            'lbPPh.Text = "PPh %" '(" + ViewState("Currency") + ")"
            'lbTotBiaya.Text = "Total Biaya" '(" + ViewState("Currency") + ")"
            'FillCombo(ddlStructureCode, "SELECT * FROM MsStructure ORDER BY ID ASC", True, "StructureCode", "StructureCode", ViewState("DBConnection"))
            'FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlUnit, "SELECT Unit_Code, Unit_Name FROM VMsUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "SELECT PayCode, PayName FROM MsPayType WHERE FgType='P' ", True, "PayCode", "PayName", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser(" + QuotedStr("PaymentNT" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment ('*')", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment 'Y' ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'ViewState("AccPPn") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc ", ViewState("DBConnection"))
            'ViewState("AccPPn2") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc2 ", ViewState("DBConnection"))
            'ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("PPN") = 11
            ViewState("DigitCurrAcc") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
                'ddlCommand.Items.Add("Print Full")
                'ddlCommand2.Items.Add("Print Full")
            End If
            lbTitle.Text = "Job Quotation" 'Request.QueryString("MenuName").ToString
            'tbPPn.Attributes.Add("ReadOnly", "True")
            'tbDisc.Attributes.Add("ReadOnly", "True")
            'tbPPnValue.Attributes.Add("ReadOnly", "True")
            'tbPPh.Attributes.Add("ReadOnly", "True")
            'tbBaseForex.Attributes.Add("ReadOnly", "True")
            'tbPPhValue.Attributes.Add("ReadOnly", "True")

            tbBaseForex.Attributes.Add("OnBlur", "setformatforhd();")
            tbPriceForex1.Attributes.Add("OnBlur", "setformatforhd();")
            tbPriceForex2.Attributes.Add("OnBlur", "setformatforhd();")
            tbPriceForex3.Attributes.Add("OnBlur", "setformatforhd();")
            tbPriceForex4.Attributes.Add("OnBlur", "setformatforhd();")
            tbPriceForex5.Attributes.Add("OnBlur", "setformatforhd();")
            'tbDPP.Attributes.Add("OnBlur", "setformatforhd();")
            'tbDisc.Attributes.Add("OnBlur", "setformatforhd();")
            'tbPPn.Attributes.Add("OnBlur", "setformatforhd();")
            'tbPPnValue.Attributes.Add("OnBlur", "setformatforhd();")
            'tbPPh.Attributes.Add("OnBlur", "setformatforhd();")
            'tbPPhValue.Attributes.Add("OnBlur", "setformatforhd();")
            'tbTotalAmount.Attributes.Add("OnBlur", "setformatforhd();")

            'tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            'tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            'tbPaymentHomeDt2.Attributes.Add("ReadOnly", "True")

            tbPriceForex1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForex2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForex3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForex4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForex5.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
            'tbPaymentForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            'tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")

            'tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")
            'tbPaymentForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")

            ' tbChargeRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
            'tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindGridArea()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT AreaCode, AreaName FROM MsArea") 'WHERE FgActive='Y' ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVArea.DataSource = dt
                        GVArea.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    'Public Function FilterDataDlg(ByVal Field1 As String, ByVal Filter1 As String) As String
    Public Function FilterDataDlg(ByVal Filter1 As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                'StrFilter = Field1.Replace(" ", "_") + " LIKE '%" + Filter1 + "%'"
                StrFilter = " LIKE '%" + Filter1 + "%'"
            Else
                StrFilter = ""
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterDlg Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FilterBindDataGridSupplier()
        'Dim ds As New DataSet()
        'Dim dvSearch As DataView
        Dim strFilter As String
        Try
            ''StrFilter = FilterDataDlg(ddlField.SelectedValue, tbFilter.Text)
            ''StrFilter = FilterDataDlg(tbFilter.Text)
            'StrFilter = FilterDataDlg(txtSearchSupplier.Text)
            'If ViewState("InFilter").ToString.ToUpper.Contains("SELECT ") Then
            '    If StrFilter.Length > 3 Then
            '        If ViewState("InFilter").ToString.ToUpper.Contains("WHERE ") Then
            '            StrFilter = " AND (" + StrFilter + ") "
            '        Else
            '            StrFilter = " WHERE (" + StrFilter + ") "
            '        End If
            '    End If
            '    If ViewState("DBConnection") Is Nothing Then
            '        ds = SQLExecuteQuery(ViewState("InFilter") + StrFilter, GetConString)
            '    Else
            '        ds = SQLExecuteQuery(ViewState("InFilter") + StrFilter, ViewState("DBConnection").ToString)
            '    End If
            '    dvSearch = ds.Tables(0).DefaultView
            'Else
            '    If ViewState("DBConnection") Is Nothing Then
            '        ds = SQLExecuteQuery(ViewState("InFilter"), GetConString)
            '    Else
            '        ds = SQLExecuteQuery(ViewState("InFilter"), ViewState("DBConnection").ToString)
            '    End If
            '    dvSearch = ds.Tables(0).DefaultView
            '    dvSearch.RowFilter = StrFilter
            'End If

            'If ViewState("pk") Is Nothing Then
            '    Dim a As Integer
            '    Dim pk As String
            '    Dim Pertamax As Boolean
            '    pk = ""
            '    Pertamax = True
            '    For a = 0 To dvSearch.Table.Columns.Count - 1
            '        If Pertamax Then
            '            pk = dvSearch.Table.Columns(a).ColumnName
            '            Pertamax = False
            '        Else
            '            pk = pk + ";" + dvSearch.Table.Columns(a).ColumnName.Trim
            '        End If
            '    Next
            '    GVSupplier.DataSource = pk
            '    ViewState("pk") = pk
            'End If

            ''dvSearch.Sort = ddlOrderBy.SelectedValue.Replace(" ", "_")
            ''VisiblePager(dvSearch.Count > 0)
            ''GridView1.SettingsPager.PageSize = CInt(ViewState("PagerSize").ToString)
            'GVSupplier.DataSource = dvSearch
            'GVSupplier.DataBind()
            strFilter = "SELECT AreaCode, AreaName FROM MsArea" ' WHERE FgActive='Y' AND SuppName LIKE '%" + txtSearchSupplier.Text.Trim() + "%'"
            'Dim conStr As String = ViewState("DBConnection").ToString
            'Using con As New SqlConnection(conStr)
            '    Using cmd As New SqlCommand(strFilter)
            '        Using sda As New SqlDataAdapter()
            '            cmd.Connection = con
            '            sda.SelectCommand = cmd
            '            Using dt As New DataTable()
            '                sda.Fill(dt)
            '                GVSupplier.DataSource = dt
            '                GVSupplier.DataBind()
            '            End Using
            '        End Using
            '    End Using
            'End Using
            'If Not String.IsNullOrEmpty(txtSearchArea.Text.Trim()) Then
            '    'strFilter += String.Format(" WHERE SuppName LIKE = '{0}'", txtSearchSupplier.Text.Trim())
            '    strFilter += String.Format(" AND AreaName LIKE '%", txtSearchArea.Text.Trim())
            'End If
            'Dim conStr As String = ViewState("DBConnection").ToString
            'Using con As New SqlConnection(conStr)
            '    Using ds As New DataSet()
            '        con.Open()
            '        Using cmd As New SqlCommand(strFilter)
            '            Using sda As New SqlDataAdapter(cmd)
            '                sda.Fill(ds, "MsArea")
            '                GVArea.DataSource = ds.Tables(0).DefaultView
            '                GVArea.DataBind()
            '            End Using
            '        End Using
            '    End Using
            'End Using
        Catch ex As Exception
            lbStatus.Text = "Error on btn search klik : " + ex.ToString
        End Try
    End Sub

    Private Sub BindGridCIPLicenRecvDoc()
        Dim strSQL = "S_PRCFindCIPRecvDoc"
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand(strSQL, con)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    cmd.CommandType = CommandType.StoredProcedure
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVNoRecvDoc.DataSource = dt
                        GVNoRecvDoc.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    'Private Shared Function GetData(ByVal cmd As SqlCommand, ByVal pageIndex As Integer) As DataSet
    'Private Function GetData(ByVal cmd As SqlCommand, ByVal pageIndex As Integer) As DataSet
    '    Dim conStr As String = ViewState("DBConnection").ToString
    '    Using con As New SqlConnection(conStr)
    '        Using sda As New SqlDataAdapter()
    '            cmd.Connection = con
    '            sda.SelectCommand = cmd
    '            Using ds As New DataSet()
    '                sda.Fill(ds, "Customers")
    '                Dim dt As New DataTable("Pager")
    '                dt.Columns.Add("PageIndex")
    '                dt.Columns.Add("PageSize")
    '                dt.Columns.Add("RecordCount")
    '                dt.Rows.Add()
    '                dt.Rows(0)("PageIndex") = pageIndex
    '                dt.Rows(0)("PageSize") = PageSize
    '                dt.Rows(0)("RecordCount") = cmd.Parameters("@RecordCount").Value
    '                ds.Tables.Add(dt)
    '                Return ds
    '            End Using
    '        End Using
    '    End Using
    'End Function

    'Public Shared Function GetSupplier(ByVal searchTerm As String, ByVal pageIndex As Integer) As String
    '    Dim query As String = "[S_FindSuppCIPLicAdmInv]"
    '    Dim cmd As New SqlCommand(query)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm)
    '    cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
    '    cmd.Parameters.AddWithValue("@PageSize", PageSize)
    '    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
    '    'Return GetData(cmd, pageIndex).GetXml()
    'End Function

    Private Sub CountTotalAmount()
        Dim PriceForex As Double
        Dim Dr As DataRow
        Try
            PriceForex = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    PriceForex = PriceForex + CFloat(Dr("PriceForex").ToString)
                End If
            Next
            tbBaseForex.Text = FormatNumber(PriceForex, ViewState("DigitHome"))
            'tbTotalAmount.Text = FormatNumber(CFloat(tbBaseForex.Text) - CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPhValue.Text), ViewState("DigitHome"))
        Catch ex As Exception
            Throw New Exception("Count Baseforex Error : " + ex.ToString)
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()
            If DT.Rows.Count > 0 Then
                'GridView1.HeaderRow.Cells(10).Text = "Payment (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Other (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(12).Text = "Expense (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
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
                'Session("SelectCommand") = "EXEC S_FNFormBuktiBank " + Result + ",'PAYMENTNONTRADE'"
                'Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                Session("SelectCommand") = "EXEC S_FNFormJobQuotation " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormJobQuotation.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
                'ElseIf ActionValue = "Print Full" Then
                '    Dim GVR As GridViewRow
                '    Dim CB As CheckBox
                '    Dim Pertamax As Boolean

                '    Pertamax = True
                '    Result = ""

                '    For Each GVR In GridView1.Rows
                '        CB = GVR.FindControl("cbSelect")
                '        If CB.Checked Then
                '            ListSelectNmbr = GVR.Cells(2).Text
                '            If Pertamax Then
                '                Result = "'''" + ListSelectNmbr + "''"
                '                Pertamax = False
                '            Else
                '                Result = Result + ",''" + ListSelectNmbr + "''"
                '            End If
                '        End If
                '    Next
                '    Result = Result + "'"
                '    Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt " + Result + ",'PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                '    Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
                '    Session("DBConnection") = ViewState("DBConnection")
                '    AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_FINCIPOffeRecv", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'tbCode.Enabled = State
            'btnInstruksiNo.Visible = State
            'ddlUserType.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    'Private Sub BindDataDt(ByVal Nmbr As String)
    '    Try
    '        Dim dt As New DataTable
    '        ViewState("Dt") = Nothing
    '        dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt") = dt
    '        BindGridDt(dt, GridDt)
    '        'If dt.Rows.Count > 0 Then
    '        'GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
    '        'End If
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Private Sub BindDataDt2(ByVal Nmbr As String)
    '    Try
    '        Dim dt As New DataTable
    '        'Dim dr As DataRow
    '        ViewState("Dt2") = Nothing
    '        dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt2") = dt
    '        BindGridDt(dt, GridDt2)
    '        'GridDt2.HeaderRow.Cells(10).Text = "Payment (" + ViewState("Currency") + ")"
    '        'GridDt2.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
    '        'If dt.Rows.Count > 0 Then
    '        '    For Each dr In dt.Rows
    '        '        If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Or dr("FgMode").ToString = "O" Then
    '        '            ViewState("PayType") = dt.Rows(0)("PaymentType").ToString
    '        '            Exit For
    '        '        End If
    '        '    Next
    '        '    'GridDt2.Columns(0).Visible = True
    '        '    'lbStatus.Text = "a"
    '        '    'Exit Sub
    '        'Else
    '        '    'GridDt2.Columns(0).Visible = False
    '        '    'lbStatus.Text = "b"
    '        '    'Exit Sub
    '        'End If
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            'MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'StatusButtonSave(True)
        Catch ex As Exception
            'lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbInstruksiNo.Text = ""
            tbBaseForex.Text = "0"
            tbSuppCode1.Text = ""
            tbSuppName1.Text = ""
            tbSuppCode2.Text = ""
            tbSuppName2.Text = ""
            tbSuppCode3.Text = ""
            tbSuppName3.Text = ""
            tbSuppCode4.Text = ""
            tbSuppName4.Text = ""
            tbSuppCode5.Text = ""
            tbSuppName5.Text = ""

            tbOfferNo1.Text = ""
            tbOfferNo2.Text = ""
            tbOfferNo3.Text = ""
            tbOfferNo4.Text = ""
            tbOfferNo5.Text = ""

            lbOfferFile1.Text = ""
            lbOfferFile2.Text = ""
            lbOfferFile3.Text = ""
            lbOfferFile4.Text = ""
            lbOfferFile5.Text = ""

            tbPriceForex1.Text = "0"
            tbPriceForex2.Text = "0"
            tbPriceForex3.Text = "0"
            tbPriceForex4.Text = "0"
            tbPriceForex5.Text = "0"

            tbDurasi1.Text = "0"
            tbDurasi2.Text = "0"
            tbDurasi3.Text = "0"
            tbDurasi4.Text = "0"
            tbDurasi5.Text = "0"

            chkAcc1.Checked = False
            chkAcc2.Checked = False
            chkAcc3.Checked = False
            chkAcc4.Checked = False
            chkAcc5.Checked = False
            'ddlReport.SelectedValue = "Y"
            'ddlUserType.SelectedValue = "Supplier"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbStartJobDate1.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFinishJobDate1.SelectedDate = ViewState("ServerDate")
            tbStartJobDate2.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFinishJobDate2.SelectedDate = ViewState("ServerDate")
            tbStartJobDate3.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFinishJobDate3.SelectedDate = ViewState("ServerDate")
            tbStartJobDate4.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFinishJobDate4.SelectedDate = ViewState("ServerDate")
            tbStartJobDate5.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbFinishJobDate5.SelectedDate = ViewState("ServerDate")

            'tbDisc.Text = "0"
            'tbDPP.Text = "0"
            ''tbPPn.Text = "0"  'FormatFloat(0, ViewState("DigitCurr"))
            'tbPPn.Text = ViewState("PPN")
            'tbPPnValue.Text = "0"
            'tbPPh.Text = "0"
            'tbPPhValue.Text = "0"
            'tbTotalAmount.Text = "0"
            'tbAttn.Text = ""
            tbRemark.Text = ""
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue            
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            'ddlStructureCode.SelectedIndex = 0
            'tbAccountDt.Text = ""
            'tbAccountNameDt.Text = ""
            'tbFgSubledDt.Text = "N"
            tbJobName.Text = ""
            tbQty.Text = "0"
            'tbSubledNameDt.Text = ""
            'tbSubledDt.Enabled = False
            'btnSubled.Visible = False
            'ddlUnit.SelectedIndex = 0
            tbPriceForex.Text = "0"
            'tbLocationName.Text = ""
            'ddlCurrDt.SelectedValue = ViewState("Currency")
            'tbRateDt.Text = FormatFloat(1, ViewState("DigitRate"))
            'tbRateDt.Enabled = False
            'ddlCurrDt.Enabled = False
            'tbBiaya.Text = "0"
            'tbAmountHomeDt.Text = "0"
            tbRemarkDt.Text = ""
            'tbPPndate.SelectedDate = Nothing
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            'ddlPayTypeDt2.SelectedIndex = 0
            'ddlBankPaymentDt2.SelectedIndex = 0
            'tbPaymentDateDt2.SelectedDate = tbDate.SelectedDate
            'tbDocumentNoDt2.Text = ""
            ''tbVoucherNo.Enabled = False
            'tbNominal.Text = "0"
            'tbDueDateDt2.SelectedDate = Nothing
            ''tbGiroDateDt2.SelectedDate = Nothing
            ''ddlCurrDt2.SelectedValue = ViewState("Currency")
            ''tbPaymentForexDt2.Text = "0"
            ''tbPaymentHomeDt2.Text = "0"
            'tbRemarkDt2.Text = "" 'tbRemark.Text
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
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
            'If tbTimeJobDate.SelectedDate = Nothing Then
            '    lbStatus.Text = MessageDlg("Waktu pelaksanaan must have value")
            '    tbTimeJobDate.Focus()
            '    Return False
            'End If
            'If tbStartJobDate.SelectedDate = Nothing Then
            '    lbStatus.Text = MessageDlg("Mulai pelaksanaan must have value")
            '    tbStartJobDate.Focus()
            '    Return False
            'End If
            'If tbFinishJobDate.SelectedDate = Nothing Then
            '    lbStatus.Text = MessageDlg("Selesai pelaksanaan must have value")
            '    tbFinishJobDate.Focus()
            '    Return False
            'End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If tbAreaCode.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Area Code must have value")
            '    tbAreaCode.Focus()
            '    Return False
            'End If
            'If tbAreaName.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Area Name must have value")
            '    tbAreaName.Focus()
            '    Return False
            'End If
            If tbInstruksiNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Instruksi must have value")
                tbInstruksiNo.Focus()
                Return False
            End If
            If tbBaseForex.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Total HPS must have value")
                tbBaseForex.Focus()
                Return False
            End If

            'If tbDurasi.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Durasi must have value")
            '    tbDurasi.Focus()
            '    Return False
            'End If
            'If tbRemark.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Remark must have value")
            '    tbRemark.Focus()
            '    Return False
            'End If

            'If CFloat(tbDisc.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Payment must have value")
            '    tbPPn.Focus()
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
                'If Dr("Account").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Account Must Have Value")
                '    Return False
                'End If
                'If ddlCostCenterDt.Enabled = True Then
                '    If ddlCostCenterDt.SelectedIndex = 0 Then
                '        lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                '        Return False
                '    End If
                'End If
                'If Dr("FgSubLed").ToString <> "N" And Dr("SubLed").ToString = "" Then
                '    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                '    Return False
                'End If
                'If (Dr("Account").ToString = ViewState("AccPPn") Or Dr("Account").ToString = ViewState("AccPPn2")) And Dr("PPnNo").ToString = "" Then
                '    lbStatus.Text = MessageDlg("PPn No Must Have Value")
                '    Return False
                'End If
                'If (Dr("Account").ToString = ViewState("AccPPn") Or Dr("Account").ToString = ViewState("AccPPn2")) And Dr("PPnDate").ToString Is Nothing Then
                '    lbStatus.Text = MessageDlg("PPn Date Must Have Value")
                '    Return False
                'End If
                'If Dr("Currency").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Currency Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("ForexRate").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("ForexRate").ToString) = 1 And Dr("Currency").ToString <> ViewState("Currency") Then
                '    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("AmountForex").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
                '    Return False
                'End If
            Else
                If tbJobName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Uraian pekerjaan Must Have Value")
                    tbJobName.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= "0" Then
                    lbStatus.Text = MessageDlg("Area Must be greater than nol")
                    tbQty.Focus()
                    Return False
                End If
                'If ddlUnit.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Satuan Must Have Value")
                '    ddlUnit.Focus()
                '    Return False
                'End If
                If CFloat(tbPriceForex.Text) <= "0" Then
                    lbStatus.Text = MessageDlg("Estimasi/HPS Must be greater than nol")
                    tbPriceForex.Focus()
                    Return False
                End If
                If tbPriceForex.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Estimasi/HPS Must Have Value")
                    tbPriceForex.Focus()
                    Return False
                End If
                If tbRemark.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Remark Must Have Value")
                    tbRemark.Focus()
                    Return False
                End If
                'If ddlStructureCode.Enabled = True Then
                '    If ddlStructureCode.SelectedIndex = 0 Then
                '        lbStatus.Text = MessageDlg("Structure Code Must Have Value")
                '        Return False
                '    End If
                'End If
                'If ddlCostCenterDt.Enabled = True Then
                '    If ddlCostCenterDt.SelectedIndex = 0 Then
                '        lbStatus.Text = MessageDlg("Unit Must Have Value")
                '        Return False
                '    End If
                'End If
                'If tbFgSubledDt.Text <> "N" And tbSubledDt.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                '    btnSubled.Focus()
                '    Return False
                'End If
                'If (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2")) And (tbPPnNo.Text = "") Then
                '    lbStatus.Text = MessageDlg("PPn No Must Have Value")
                '    Return False
                'End If
                'If (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2")) And (tbPPndate.SelectedDate = Nothing) Then
                '    lbStatus.Text = MessageDlg("PPn Date Must Have Value")
                '    Return False
                'End If
                'If ddlCurrDt.SelectedValue = "" Then
                '    lbStatus.Text = MessageDlg("Currency Must Have Value")
                '    ddlCurrDt.Focus()
                '    Return False
                'End If
                'If CFloat(tbRateDt.Text) <= 0 Then
                '    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                '    tbRateDt.Focus()
                '    Return False
                'End If
                'If CFloat(tbRateDt.Text) = 1 And ddlCurrDt.SelectedValue <> ViewState("Currency") Then
                '    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                '    tbRateDt.Focus()
                '    Return False
                'End If
                'If CFloat(tbAmountForexDt.Text) <= 0 Then
                '    lbStatus.Text = MessageDlg("Price Must Have Value")
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
                If Dr("PaymentType").ToString = "" Then
                    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                    Return False
                End If
                If Dr("FgMode").ToString = "G" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        Return False
                    End If
                    If Dr("BankPayment").ToString = "" Then
                        lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
                        Return False
                    End If
                    If Dr("Due Date").ToString = "" Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        Return False
                    End If
                End If
                If Dr("FgMode").ToString = "D" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("DP No Must Have Value")
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
                'If ddlPayTypeDt2.SelectedValue = "" Then
                '    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                '    ddlPayTypeDt2.Focus()
                '    Return False
                'End If
                'If tbFgModeDt2.Text = "G" Then
                '    If tbDocumentNoDt2.Text.Trim = "" Then
                '        lbStatus.Text = MessageDlg("Document No Must Have Value")
                '        tbDocumentNoDt2.Focus()
                '        Return False
                '    End If
                '    If ddlBankPaymentDt2.SelectedValue = "" Then
                '        lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
                '        ddlBankPaymentDt2.Focus()
                '        Return False
                '    End If
                '    If tbGiroDateDt2.SelectedDate = Nothing Then
                '        lbStatus.Text = MessageDlg("Giro Date Must Have Value")
                '        tbGiroDateDt2.Focus()
                '        Return False
                '    End If
                '    If tbDueDateDt2.SelectedDate = Nothing Then
                '        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                '        tbDueDateDt2.Focus()
                '        Return False
                '    End If
                'End If
                'If tbFgModeDt2.Text = "D" Then
                '    If tbDocumentNoDt2.Text.Trim = "" Then
                '        lbStatus.Text = MessageDlg("DP No Must Have Value")
                '        tbDocumentNoDt2.Focus()
                '        Return False
                '    End If
                'End If
                'If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                '    If tbVoucherNo.Text.Trim = "" Then
                '        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                '        tbVoucherNo.Focus()
                '        Return False
                '    End If
                'End If
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
            BindToText(tbInstruksiNo, Dt.Rows(0)("InstruksiNo").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitHome"))
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            'BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbSuppCode1, Dt.Rows(0)("SuppCode1").ToString)
            BindToText(tbSuppName1, Dt.Rows(0)("SuppName1").ToString)
            BindToText(tbOfferNo1, Dt.Rows(0)("OfferNo1").ToString)
            lbOfferFile1.Text = Dt.Rows(0)("OfferFile1").ToString
            BindToText(tbPriceForex1, Dt.Rows(0)("PriceForex1").ToString, ViewState("DigitHome"))
            BindToDate(tbStartJobDate1, Dt.Rows(0)("StartJobDate1").ToString)
            BindToDate(tbFinishJobDate1, Dt.Rows(0)("FinishJobDate1").ToString)
            BindToText(tbDurasi1, Dt.Rows(0)("Durasi1").ToString)

            BindToText(tbSuppCode2, Dt.Rows(0)("SuppCode2").ToString)
            BindToText(tbSuppName2, Dt.Rows(0)("SuppName2").ToString)
            BindToText(tbOfferNo2, Dt.Rows(0)("OfferNo2").ToString)
            lbOfferFile2.Text = Dt.Rows(0)("OfferFile2").ToString
            BindToText(tbPriceForex2, Dt.Rows(0)("PriceForex2").ToString, ViewState("DigitHome"))
            BindToDate(tbStartJobDate2, Dt.Rows(0)("StartJobDate2").ToString)
            BindToDate(tbFinishJobDate2, Dt.Rows(0)("FinishJobDate2").ToString)
            BindToText(tbDurasi2, Dt.Rows(0)("Durasi2").ToString)

            BindToText(tbSuppCode3, Dt.Rows(0)("SuppCode3").ToString)
            BindToText(tbSuppName3, Dt.Rows(0)("SuppName3").ToString)
            BindToText(tbOfferNo3, Dt.Rows(0)("OfferNo3").ToString)
            lbOfferFile3.Text = Dt.Rows(0)("OfferFile3").ToString
            BindToText(tbPriceForex3, Dt.Rows(0)("PriceForex3").ToString, ViewState("DigitHome"))
            BindToDate(tbStartJobDate3, Dt.Rows(0)("StartJobDate3").ToString)
            BindToDate(tbFinishJobDate3, Dt.Rows(0)("FinishJobDate3").ToString)
            BindToText(tbDurasi3, Dt.Rows(0)("Durasi3").ToString)

            BindToText(tbSuppCode4, Dt.Rows(0)("SuppCode4").ToString)
            BindToText(tbSuppName4, Dt.Rows(0)("SuppName4").ToString)
            BindToText(tbOfferNo4, Dt.Rows(0)("OfferNo4").ToString)
            lbOfferFile4.Text = Dt.Rows(0)("OfferFile4").ToString
            BindToText(tbPriceForex4, Dt.Rows(0)("PriceForex4").ToString, ViewState("DigitHome"))
            BindToDate(tbStartJobDate4, Dt.Rows(0)("StartJobDate4").ToString)
            BindToDate(tbFinishJobDate4, Dt.Rows(0)("FinishJobDate4").ToString)
            BindToText(tbDurasi4, Dt.Rows(0)("Durasi4").ToString)

            BindToText(tbSuppCode5, Dt.Rows(0)("SuppCode5").ToString)
            BindToText(tbSuppName5, Dt.Rows(0)("SuppName5").ToString)
            BindToText(tbOfferNo5, Dt.Rows(0)("OfferNo5").ToString)
            lbOfferFile5.Text = Dt.Rows(0)("OfferFile5").ToString
            BindToText(tbPriceForex5, Dt.Rows(0)("PriceForex5").ToString, ViewState("DigitHome"))
            BindToDate(tbStartJobDate5, Dt.Rows(0)("StartJobDate5").ToString)
            BindToDate(tbFinishJobDate5, Dt.Rows(0)("FinishJobDate5").ToString)
            BindToText(tbDurasi5, Dt.Rows(0)("Durasi5").ToString)

            If Dt.Rows(0)("FgAcc1").ToString = "Y" Then
                chkAcc1.Checked = True
                'chkAcc1.Enabled = False
                'chkAcc2.Enabled = False
                'chkAcc3.Enabled = False
                
            ElseIf Dt.Rows(0)("FgAcc2").ToString = "Y" Then
                chkAcc2.Checked = True
                chkAcc1.Disabled = True
                chkAcc2.Disabled = True
                chkAcc3.Disabled = True
                chkAcc4.Disabled = True
                chkAcc5.Disabled = True
               
            ElseIf Dt.Rows(0)("FgAcc3").ToString = "Y" Then
                chkAcc3.Checked = True
                chkAcc1.Disabled = True
                chkAcc2.Disabled = True
                chkAcc3.Disabled = True
                chkAcc4.Disabled = True
                chkAcc5.Disabled = True
            ElseIf Dt.Rows(0)("FgAcc4").ToString = "Y" Then
                chkAcc4.Checked = True
                chkAcc1.Disabled = True
                chkAcc2.Disabled = True
                chkAcc3.Disabled = True
                chkAcc4.Disabled = True
                chkAcc5.Disabled = True
            ElseIf Dt.Rows(0)("FgAcc5").ToString = "Y" Then
                chkAcc5.Checked = True
                chkAcc1.Disabled = True
                chkAcc2.Disabled = True
                chkAcc3.Disabled = True
                chkAcc4.Disabled = True
                chkAcc5.Disabled = True
            ElseIf (Dt.Rows(0)("FgAcc1").ToString = "N") Or (Dt.Rows(0)("FgAcc2").ToString = "N") Or (Dt.Rows(0)("FgAcc3").ToString = "N") Or (Dt.Rows(0)("FgAcc4").ToString = "N") Or (Dt.Rows(0)("FgAcc5").ToString = "N") Then
                chkAcc1.Checked = False
                chkAcc2.Checked = False
                chkAcc3.Checked = False
                chkAcc4.Checked = False
                chkAcc5.Checked = False
                'chkAcc1.Enabled = True
                'chkAcc2.Enabled = True
                'chkAcc3.Enabled = True
                'chkAcc1.Disabled = True
                'chkAcc2.Disabled = True
                'chkAcc3.Disabled = True
            End If

            'ViewState("DigitCurr") = SQLExecuteScalar("SELECT Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            'If ViewState("DigitCurr") = Nothing Then
            '    ViewState("DigitCurr") = 0
            'End If
            'BindToText(tbDisc, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitHome"))
            'BindToText(tbDPP, Dt.Rows(0)("DPPForex").ToString, ViewState("DigitHome"))
            'BindToText(tbPPn, Dt.Rows(0)("PPn").ToString, CInt(ViewState("DigitCurr")))
            'BindToText(tbPPnValue, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitHome"))
            'BindToText(tbPPh, Dt.Rows(0)("PPh").ToString, ViewState("DigitHome"))
            'BindToText(tbPPhValue, Dt.Rows(0)("PPhForex").ToString, ViewState("DigitHome"))
            'BindToText(tbTotalAmount, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitHome"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                ' FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
                'BindToDropList(ddlUserType, Dr(0)("CIPAI").ToString)
                'BindToDropList(ddlStructureCode, Dr(0)("StructureCode").ToString)
                'BindToText(tbAccountDt, Dr(0)("Account").ToString)
                'BindToText(tbAccountNameDt, Dr(0)("AccountName").ToString)
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbQty, Dr(0)("Area").ToString)
                'BindToText(tbSubledNameDt, Dr(0)("SubledName").ToString)
                'BindToText(tbPPnNo, Dr(0)("PPnNo").ToString)
                'BindToDate(tbPPndate, Dr(0)("PPNDate").ToString)
                'BindToDropList(ddlUnit, Dr(0)("UnitCode").ToString)
                'BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                'BindToText(tbLocationName, Dr(0)("LocationName").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'BindToText(tbFgType, Dr(0)("FgType").ToString)
                'ddlCurrDt.Enabled = (tbFgType.Text = "PL")
                'tbRateDt.Enabled = ddlCurrDt.SelectedValue <> ViewState("Currency").ToString
                'ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
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
                'BindToDropList(ddlPayTypeDt2, Dr(0)("PaymentType").ToString)
                'tbPaymentDateDt2.SelectedDate = tbDate.SelectedDate
                tbDueDateDt2.SelectedDate = Dr(0)("PaymentDate").ToString
                BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
                BindToText(tbNominal, Dr(0)("Nominal").ToString)
                'BindToDate(tbGiroDateDt2, Dr(0)("GiroDate").ToString)
                'BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
                'BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                'BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                'BindToText(tbPaymentForexDt2, Dr(0)("PaymentForex").ToString)
                'BindToText(tbPaymentHomeDt2, Dr(0)("PaymentHome").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                'BindToText(tbFgModeDt2, Dr(0)("FgMode").ToString)
                'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                'BindToDropList(ddlBankPaymentDt2, Dr(0)("BankPayment").ToString)
                'BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                'BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                'BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                'BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)

                'If ddlChargeCurrDt2.SelectedValue = "" Then
                '    tbChargeForexDt2.Text = "0"
                '    tbChargeHomeDt2.Text = "0"
                'End If
                'tbRateDt2.Enabled = ddlCurrDt2.SelectedValue <> Session("Currency")
                'ddlBankPaymentDt2.Enabled = tbFgModeDt2.Text = "G"
                'tbGiroDateDt2.Enabled = tbFgModeDt2.Text = "G"
                'tbDueDateDt2.Enabled = tbFgModeDt2.Text = "G"
                'ddlChargeCurrDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeForexDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeRateDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> Session("Currency") And tbFgModeDt2.Text = "B" And ddlChargeCurrDt2.SelectedValue <> ""
                'tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
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
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                'Row("CIPAI") = ddlUserType.SelectedValue
                Row("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
                'Row("FgSubled") = tbFgSubledDt.Text
                'Row("Subled") = tbSubledDt.Text
                'Row("SubledName") = tbSubledNameDt.Text
                Row("Area") = Val(tbQty.Text)
                'Row("UnitCode") = ddlUnit.SelectedValue
                Row("PriceForex") = Val(tbPriceForex.Text)
                'Row("LocationName") = tbLocationName.Text
                'Row("UnitCode") = ddlUnit.SelectedValue
                'If tbPPndate.Enabled Then
                '    Row("PPnDate") = tbPPndate.SelectedDate.ToString
                'Else
                '    Row("PPnDate") = DBNull.Value
                'End If
                'Row("BeaForex") = tbBiaya.Text
                'Row("ForexRate") = tbRateDt.Text
                'If Row("CostCtr") = "" Then
                '    Row("Costctr") = DBNull.Value
                'End If
                'Row("Amount") = Val(tbSubledDt.Text) * Val(tbAmountForexDt.Text)
                'Row("AmountHome") = tbAmountHomeDt.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgActive") = "Y"
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                'dr("CIPAI") = ddlUserType.SelectedValue
                dr("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
                dr("Area") = Val(tbQty.Text)
                'dr("UnitCode") = ddlUnit.SelectedValue
                dr("PriceForex") = Val(tbPriceForex.Text)
                'dr("LocationName") = tbLocationName.Text
                'dr("BeaForex") = tbBiaya.Text
                'dr("Account") = tbAccountDt.Text
                'dr("AccountName") = tbAccountNameDt.Text
                'dr("FgSubled") = tbFgSubledDt.Text
                'dr("Subled") = tbSubledDt.Text
                'dr("SubledName") = tbSubledNameDt.Text
                'dr("FgType") = tbFgType.Text
                'dr("PPnNo") = tbPPnNo.Text
                'dr("PPnDate") = tbPPndate.SelectedDate
                'If tbPPndate.Enabled Then
                '    dr("PPnDate") = tbPPndate.SelectedDate.ToString
                'Else
                '    dr("PPnDate") = DBNull.Value
                'End If
                'dr("Currency") = ddlCurrDt.SelectedValue
                'dr("ForexRate") = tbRateDt.Text
                'dr("CostCtr") = ddlCostCenterDt.SelectedValue
                'If dr("CostCtr") = "" Then
                '    dr("Costctr") = DBNull.Value
                'End If
                'dr("Amount") = Val(tbSubledDt.Text) * Val(tbAmountForexDt.Text)
                'dr("AmountHome") = tbAmountHomeDt.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgActive") = "Y"
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            CountTotalAmount()
            StatusButtonSave(True)
            ' GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try
            'If ViewState("1Payment").ToString = "Y" Then
            '    If GetCountRecord(ViewState("Dt2")) >= 1 Then
            '        'If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
            '        If ViewState("StateDt2") <> "Edit" Then
            '            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
            '                lbStatus.Text = "Cannot input more than one payment type"
            '                Exit Sub
            '            End If
            '        End If
            '        'End If
            '    End If
            'End If
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            'If tbFgModeDt2.Text = "G" Then
            '    If CekExistGiroOut(tbDocumentNoDt2.Text.Trim, ViewState("DBConnection").ToString) = True Then
            '        lbStatus.Text = "Giro Payment '" + tbDocumentNoDt2.Text.Trim + "' has already exists in Giro Listing'"
            '        Exit Sub
            '    End If
            'End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()
                'Row("PaymentType") = ddlPayTypeDt2.SelectedValue
                'Row("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
                Row("PaymentDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Row("DocumentNo") = tbDocumentNoDt2.Text
                Row("Nominal") = tbNominal.Text
                'If tbGiroDateDt2.Enabled Then
                '    Row("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
                'Else
                '    Row("GiroDate") = DBNull.Value
                'End If

                'If tbDueDateDt2.Enabled Then
                '    Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                'Else
                '    Row("DueDate") = DBNull.Value
                'End If
                'Row("BankPayment") = ddlBankPaymentDt2.SelectedValue
                'Row("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                'Row("FgMode") = tbFgModeDt2.Text
                'Row("Currency") = ddlCurrDt2.SelectedValue
                'Row("ForexRate") = tbRateDt2.Text
                'Row("PaymentForex") = tbPaymentForexDt2.Text
                'Row("PaymentHome") = tbPaymentHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                'Row("ChargeCurrency") = ddlChargeCurrDt2.SelectedValue
                'Row("ChargeRate") = tbChargeRateDt2.Text
                'Row("ChargeForex") = tbChargeForexDt2.Text
                'Row("ChargeHome") = tbChargeHomeDt2.Text
                'Row("Remark") = tbRemarkDt2.Text
                Row("FgActive") = "Y"
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = CInt(lbItemNoDt2.Text)
                'dr("PaymentType") = ddlPayTypeDt2.SelectedValue
                'dr("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
                dr("PaymentDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                dr("DocumentNo") = tbDocumentNoDt2.Text
                dr("Nominal") = tbNominal.Text
                'dr("Reference") = tbVoucherNo.Text
                'If tbGiroDateDt2.Enabled Then
                '    dr("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
                'Else
                '    dr("GiroDate") = DBNull.Value
                'End If
                'If tbDueDateDt2.Enabled Then
                '    dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                'Else
                '    dr("DueDate") = DBNull.Value
                'End If
                'dr("BankPayment") = ddlBankPaymentDt2.SelectedValue
                'dr("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                'dr("FgMode") = tbFgModeDt2.Text
                'dr("Currency") = ddlCurrDt2.SelectedValue
                'dr("ForexRate") = tbRateDt2.Text
                'dr("PaymentForex") = tbPaymentForexDt2.Text
                'dr("PaymentHome") = tbPaymentHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                dr("FgActive") = "Y"
                'dr("ChargeCurrency") = ddlChargeCurrDt2.Text
                'dr("ChargeRate") = tbChargeRateDt2.Text
                'dr("ChargeForex") = tbChargeForexDt2.Text
                'dr("ChargeHome") = tbChargeHomeDt2.Text
                'dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            'If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
            '    ViewState("PayType") = ddlPayTypeDt2.SelectedValue
            'End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
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
        Dim FgAcc1, FgAcc2, FgAcc3, FgAcc4, FgAcc5 As String
        Dim path1, path2, path3, path4, path5, namafile1, namafile2, namafile3, namafile4, namafile5 As String
        'Dim I As Integer
        Try
            System.Threading.Thread.Sleep(7000)
            'If CFloat(tbDisc.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Payment Notaris must have value")
            '    tbPPn.Focus()
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
                'If SQLExecuteScalar("SELECT TransNmbr FROM FINCIPOffeRecvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                '    lbStatus.Text = "No. Terima Penawaran" + QuotedStr(tbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If
                'If SQLExecuteScalar("SELECT InstruksiNo FROM FINCIPOffeRecvHd WHERE InstruksiNo = " + QuotedStr(tbInstruksiNo.Text), ViewState("DBConnection").ToString).Length > 0 Then
                '    '    lbStatus.Text = "No. Instruksi " + QuotedStr(tbInstruksiNo.Text) + " has already been exist"
                '    Response.Write("<script language='javascript'> { alert('No. Instruksi : " + tbInstruksiNo.Text + " has already been exist');}</script>")
                '    tbInstruksiNo.Focus()
                '    'Exit Sub
                'End If
                If chkAcc1.Checked = True Then
                    FgAcc1 = "Y"
                Else
                    FgAcc1 = "N"
                End If
                If chkAcc2.Checked = True Then
                    FgAcc2 = "Y"
                Else
                    FgAcc2 = "N"
                End If
                If chkAcc3.Checked = True Then
                    FgAcc3 = "Y"
                Else
                    FgAcc3 = "N"
                End If
                If chkAcc4.Checked = True Then
                    FgAcc4 = "Y"
                Else
                    FgAcc4 = "N"
                End If
                If chkAcc5.Checked = True Then
                    FgAcc5 = "Y"
                Else
                    FgAcc5 = "N"
                End If

                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("CPP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                path1 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile1.FileName
                namafile1 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile1.FileName
                If Right(namafile1, 4) <> ".pdf" Then
                    namafile1 = ""
                End If
                

                path2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile2.FileName
                namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile2.FileName
                If Right(namafile2, 4) <> ".pdf" Then
                    namafile2 = ""
                End If

                path3 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile3.FileName
                namafile3 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile3.FileName
                If Right(namafile3, 4) <> ".pdf" Then
                    namafile3 = ""
                End If

                path4 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile4.FileName
                namafile4 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile4.FileName
                If Right(namafile4, 4) <> ".pdf" Then
                    namafile4 = ""
                End If

                path5 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile5.FileName
                namafile5 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile5.FileName
                If Right(namafile5, 4) <> ".pdf" Then
                    namafile5 = ""
                End If

                SQLString = "INSERT INTO FINCIPOffeRecvHd(TransNmbr,TransDate,Status,InstruksiNo,BaseForex,SuppCode1,OfferNo1,PriceForex1,OfferFile1, " + _
                "StartJobDate1,FinishJobDate1,Durasi1,FgAcc1,SuppCode2,OfferNo2,PriceForex2,OfferFile2,StartJobDate2,FinishJobDate2,Durasi2,FgAcc2, " + _
                "SuppCode3,OfferNo3,PriceForex3,OfferFile3,StartJobDate3,FinishJobDate3,Durasi3,FgAcc3,SuppCode4,OfferNo4,PriceForex4,OfferFile4,StartJobDate4, " + _
                "FinishJobDate4,Durasi4,FgAcc4,SuppCode5,OfferNo5,PriceForex5,OfferFile5,StartJobDate5,FinishJobDate5,Durasi5,FgAcc5,Remark,FgReport,FgActive,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbInstruksiNo.Text) + ", " + _
                QuotedStr(tbBaseForex.Text.Replace(",", "")) + ", " + QuotedStr(tbSuppCode1.Text) + ", " + QuotedStr(tbOfferNo1.Text) + ", " + _
                QuotedStr(tbPriceForex1.Text.Replace(",", "")) + ", " + QuotedStr(namafile1) + ", " + QuotedStr(Format(tbStartJobDate1.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbFinishJobDate1.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbDurasi1.Text.Replace(",", "")) + ", " + QuotedStr(FgAcc1) + ", " + _
                QuotedStr(tbSuppCode2.Text) + ", " + QuotedStr(tbOfferNo2.Text) + ", " + QuotedStr(tbPriceForex2.Text.Replace(",", "")) + ", " + QuotedStr(namafile2) + ", " + _
                QuotedStr(Format(tbStartJobDate2.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbFinishJobDate2.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbDurasi2.Text.Replace(",", "")) + ", " + QuotedStr(FgAcc2) + ", " + QuotedStr(tbSuppCode3.Text) + ", " + QuotedStr(tbOfferNo3.Text) + ", " + _
                QuotedStr(tbPriceForex3.Text.Replace(",", "")) + ", " + QuotedStr(namafile3) + ", " + QuotedStr(Format(tbStartJobDate3.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbFinishJobDate3.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbDurasi3.Text.Replace(",", "")) + ", " + QuotedStr(FgAcc3) + ", " + _
                QuotedStr(tbSuppCode4.Text) + ", " + QuotedStr(tbOfferNo4.Text) + ", " + _
                QuotedStr(tbPriceForex4.Text.Replace(",", "")) + ", " + QuotedStr(namafile4) + ", " + QuotedStr(Format(tbStartJobDate4.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbFinishJobDate4.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbDurasi4.Text.Replace(",", "")) + ", " + QuotedStr(FgAcc4) + ", " + _
                QuotedStr(tbSuppCode5.Text) + ", " + QuotedStr(tbOfferNo5.Text) + ", " + _
                QuotedStr(tbPriceForex5.Text.Replace(",", "")) + ", " + QuotedStr(namafile5) + ", " + QuotedStr(Format(tbStartJobDate5.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbFinishJobDate5.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbDurasi5.Text.Replace(",", "")) + ", " + QuotedStr(FgAcc5) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr("Y") + ", " + QuotedStr("Y") + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text
                fupOfferFile1.SaveAs(path1)
                fupOfferFile2.SaveAs(path2)
                fupOfferFile3.SaveAs(path3)
                fupOfferFile4.SaveAs(path4)
                fupOfferFile5.SaveAs(path5)
            Else
                'Dim cekStatus As String
                'cekStatus = SQLExecuteScalar("SELECT Status FROM FINCIPOffeRecvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "P" Then
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If
                If chkAcc1.Checked = True Then
                    FgAcc1 = "Y"
                Else
                    FgAcc1 = "N"
                End If
                If chkAcc2.Checked = True Then
                    FgAcc2 = "Y"
                Else
                    FgAcc2 = "N"
                End If
                If chkAcc3.Checked = True Then
                    FgAcc3 = "Y"
                Else
                    FgAcc3 = "N"
                End If
                If chkAcc4.Checked = True Then
                    FgAcc4 = "Y"
                Else
                    FgAcc4 = "N"
                End If
                If chkAcc5.Checked = True Then
                    FgAcc5 = "Y"
                Else
                    FgAcc5 = "N"
                End If

                'Dim path1, path2, path3, namafile1, namafile2, namafile3 As String
                path1 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile1.FileName
                namafile1 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile1.FileName

            
                If Right(fupOfferFile1.FileName, 4) <> ".pdf" Then
                    namafile1 = lbOfferFile1.Text
                Else
                    namafile1 = namafile1
                End If


                path2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile2.FileName
                namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile2.FileName

                If Right(namafile2, 4) <> ".pdf" Then
                    namafile2 = lbOfferFile2.Text
                End If

                path3 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile3.FileName
                namafile3 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile3.FileName
                If Right(namafile3, 4) <> ".pdf" Then
                    namafile3 = lbOfferFile3.Text
                End If

                path4 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile4.FileName
                namafile4 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile4.FileName

                If Right(namafile4, 4) <> ".pdf" Then
                    namafile4 = lbOfferFile4.Text
                End If

                path5 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile5.FileName
                namafile5 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupOfferFile5.FileName

                If Right(namafile5, 4) <> ".pdf" Then
                    namafile5 = lbOfferFile5.Text
                End If

                SQLString = "UPDATE FINCIPOffeRecvHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", InstruksiNo =" + QuotedStr(tbInstruksiNo.Text) + _
                ", BaseForex =" + QuotedStr(tbBaseForex.Text.Replace(",", "")) + ", SuppCode1 = " + QuotedStr(tbSuppCode1.Text) + ", OfferNo1 = " + QuotedStr(tbOfferNo1.Text) + _
                ", PriceForex1 =" + QuotedStr(tbPriceForex1.Text.Replace(",", "")) + ", OfferFile1 =" + QuotedStr(namafile1) + _
                ", StartJobDate1 =" + QuotedStr(Format(tbStartJobDate1.SelectedValue, "yyyy-MM-dd")) + _
                ", FinishJobDate1 =" + QuotedStr(Format(tbFinishJobDate1.SelectedValue, "yyyy-MM-dd")) + ", Durasi1 = " + QuotedStr(tbDurasi1.Text.Replace(",", "")) + _
                ", FgAcc1 = " + QuotedStr(FgAcc1) + ", SuppCode2 = " + QuotedStr(tbSuppCode2.Text) + ", OfferNo2 = " + QuotedStr(tbOfferNo2.Text) + _
                ", PriceForex2 =" + QuotedStr(tbPriceForex2.Text.Replace(",", "")) + ", OfferFile2 =" + QuotedStr(namafile2) + _
                ", StartJobDate2 =" + QuotedStr(Format(tbStartJobDate2.SelectedValue, "yyyy-MM-dd")) + _
                ", FinishJobDate2 =" + QuotedStr(Format(tbFinishJobDate2.SelectedValue, "yyyy-MM-dd")) + ", Durasi2 = " + QuotedStr(tbDurasi2.Text.Replace(",", "")) + _
                ", FgAcc2 = " + QuotedStr(FgAcc2) + ", SuppCode3 = " + QuotedStr(tbSuppCode3.Text) + ", OfferNo3 = " + QuotedStr(tbOfferNo3.Text) + _
                ", PriceForex3 =" + QuotedStr(tbPriceForex3.Text.Replace(",", "")) + ", OfferFile3 =" + QuotedStr(namafile3) + _
                ", StartJobDate3 =" + QuotedStr(Format(tbStartJobDate3.SelectedValue, "yyyy-MM-dd")) + _
                ", FinishJobDate3 =" + QuotedStr(Format(tbFinishJobDate3.SelectedValue, "yyyy-MM-dd")) + ", Durasi3 = " + QuotedStr(tbDurasi3.Text.Replace(",", "")) + _
                ", FgAcc3 = " + QuotedStr(FgAcc3) + ", SuppCode4 = " + QuotedStr(tbSuppCode4.Text) + ", OfferNo4 = " + QuotedStr(tbOfferNo4.Text) + _
                ", PriceForex4 =" + QuotedStr(tbPriceForex4.Text.Replace(",", "")) + ", OfferFile4 =" + QuotedStr(namafile4) + _
                ", StartJobDate4 =" + QuotedStr(Format(tbStartJobDate4.SelectedValue, "yyyy-MM-dd")) + _
                ", FinishJobDate4 =" + QuotedStr(Format(tbFinishJobDate4.SelectedValue, "yyyy-MM-dd")) + ", Durasi4 = " + QuotedStr(tbDurasi4.Text.Replace(",", "")) + _
                ", FgAcc4 = " + QuotedStr(FgAcc4) + ", SuppCode5 = " + QuotedStr(tbSuppCode5.Text) + ", OfferNo5 = " + QuotedStr(tbOfferNo5.Text) + _
                ", PriceForex5 =" + QuotedStr(tbPriceForex5.Text.Replace(",", "")) + ", OfferFile5 =" + QuotedStr(namafile5) + _
                ", StartJobDate5 =" + QuotedStr(Format(tbStartJobDate5.SelectedValue, "yyyy-MM-dd")) + _
                ", FinishJobDate5 =" + QuotedStr(Format(tbFinishJobDate5.SelectedValue, "yyyy-MM-dd")) + ", Durasi5 = " + QuotedStr(tbDurasi5.Text.Replace(",", "")) + _
                ", FgAcc5 = " + QuotedStr(FgAcc5) + ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep= " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
                fupOfferFile1.SaveAs(path1)
                fupOfferFile2.SaveAs(path2)
                fupOfferFile3.SaveAs(path3)
                fupOfferFile4.SaveAs(path4)
                fupOfferFile5.SaveAs(path5)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            'Dim Row As DataRow()

            'Row = ViewState("Dt").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    Row(I).EndEdit()
            'Next

            'Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    'If Row(I)("FgMode") = "E" Then
            '    '    Row(I)("FgValue") = -1
            '    'Else
            '    '    Row(I)("FgValue") = 1
            '    'End If
            '    Row(I).EndEdit()
            'Next

            'save dt
            'Dim ConnString As String = ViewState("DBConnection").ToString
            'con = New SqlConnection(ConnString)
            'con.Open()                                            'JobCode,  
            'Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,JobName,Area,UnitCode,PriceForex,Remark,FgActive FROM FINCIPInstrJobDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand. 'Account=@Account, JobCode=@JobCode,
            'Dim Update_Command = New SqlCommand( _
            '  "UPDATE FINCIPInstrJobDt SET JobName=@JobName,Area=@Area,UnitCode=@UnitCode,PriceForex=@PriceForex,Remark=@Remark  " + _
            '  "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            '' Define output parameters.
            ''Update_Command.Parameters.Add("@JobCode", SqlDbType.VarChar, 12, "JobCode")
            'Update_Command.Parameters.Add("@JobName", SqlDbType.VarChar, 100, "JobName")
            ''Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 20, "Account")
            'Update_Command.Parameters.Add("@Area", SqlDbType.Decimal, 18, "Area")
            ''Update_Command.Parameters.Add("@PPnDate", SqlDbType.DateTime, 8, "PPnDate")
            'Update_Command.Parameters.Add("@UnitCode", SqlDbType.VarChar, 6, "UnitCode")
            'Update_Command.Parameters.Add("@PriceForex", SqlDbType.Decimal, 18, "PriceForex")
            ''Update_Command.Parameters.Add("@LocationName", SqlDbType.VarChar, 50, "LocationName")
            ''Update_Command.Parameters.Add("@BeaForex", SqlDbType.Decimal, 18, "BeaForex")
            ''Update_Command.Parameters.Add("@Amount", SqlDbType.Decimal, 18, "Amount")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM PRCIPLicenAdmInvDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            'Dim Dt As New DataTable("PRCIPLicenAdmInvDt")

            'Dt = ViewState("Dt")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("Dt") = Dt

            'save dt2
            'cmdSql = New SqlCommand("SELECT TransNmbr,ItemNo,PaymentType,DocumentNo,BankPayment,PaymentDate,Nominal,Remark,FgActive FROM FINPayCIPAICr WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand

            'Dim param2 As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command2 = New SqlCommand( _
            '        "UPDATE FINPayCIPAICr SET PaymentType=@PaymentType,DocumentNo=@DocumentNo,BankPayment=@BankPayment,PaymentDate=@PaymentDate, " + _
            '        "Nominal=@Nominal,Remark=@Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            '' Define output parameters.
            'Update_Command2.Parameters.Add("@PaymentType", SqlDbType.VarChar, 5, "PaymentType")
            'Update_Command2.Parameters.Add("@DocumentNo", SqlDbType.VarChar, 60, "DocumentNo")
            'Update_Command2.Parameters.Add("@BankPayment", SqlDbType.VarChar, 5, "BankPayment")
            'Update_Command2.Parameters.Add("@PaymentDate", SqlDbType.DateTime, 12, "PaymentDate")
            'Update_Command2.Parameters.Add("@Nominal", SqlDbType.Decimal, 18, "Nominal")
            'Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ''Update_Command2.Parameters.Add("@ChargeCurrency", SqlDbType.VarChar, 255, "ChargeCurrency")
            ''Update_Command2.Parameters.Add("@ChargeRate", SqlDbType.Decimal, 18, "ChargeRate")
            ''Update_Command2.Parameters.Add("@ChargeForex", SqlDbType.Decimal, 18, "ChargeForex")
            ''Update_Command2.Parameters.Add("@ChargeHome", SqlDbType.Decimal, 18, "ChargeHome")
            '' Define intput (WHERE) parameters.
            'param2 = Update_Command2.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            'param2.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command2

            '' Create the DeleteCommand.
            'Dim Delete_Command2 = New SqlCommand( _
            '    "DELETE FROM FINPayCIPAICr WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            '' Add the parameters for the DeleteCommand.
            'param2 = Delete_Command2.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            'param2.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command2

            'Dim Dt2 As New DataTable("FINPayCIPAICr")

            'Dt2 = ViewState("Dt2")
            'da.Update(Dt2)
            'Dt2.AcceptChanges()
            'ViewState("Dt2") = Dt2
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
            'If CFloat(tbTotalSelisih.Text) <> 0 Then
            '    lbStatus.Text = MessageDlg("Debet - Credit must be balance")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Job must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    'If ViewState("PayType").ToString = "" Then
            '    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            btnHome.Visible = False
            fupOfferFile1.Visible = True
            fupOfferFile2.Visible = True
            fupOfferFile3.Visible = True
            fupOfferFile4.Visible = True
            fupOfferFile5.Visible = True
            'Session.Remove("FileUpload1")
            'Session.Remove("FileUpload2")
            'Session.Remove("FileUpload3")
            'Session.Remove("FileUpload4")
            'Session.Remove("FileUpload5")
            MultiView1.ActiveViewIndex = 0
            'Menu1.Items.Item(0).Selected = True
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
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            'tbAccountDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

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
            lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
            'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
            '    BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
            'End If
            'btnDocNo.Visible = False
            tbDocumentNoDt2.Enabled = True
            'ddlPayTypeDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            'ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrAcc") = 0
            ViewState("DigitExpenseCurr") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            'Cleardt2()
            PnlDt.Visible = True
            btnAdddt.Visible = False
            btnAddDt2.Visible = False
            btnAddDtke2.Visible = False
            btnAddDt2Ke2.Visible = False
            '----------------------------'
            btnDeleteDoc1.Visible = False
            btnDeleteDoc2.Visible = False
            btnDeleteDoc3.Visible = False
            btnDeleteDoc4.Visible = False
            btnDeleteDoc5.Visible = False
            'BindDataDt("")
            'BindDataDt2("")
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

    'Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
    '    Dim FDateName, FDateValue, FilterName, FilterValue As String
    '    Try
    '        FDateName = "Payment Date"
    '        FDateValue = "TransDate"
    '        FilterName = "Payment No, Payment Date, User Type, User Payment, Attn, DP No, Voucher No, Remark, Account, Account Name"
    '        FilterValue = "TransNmbr, dbo.FormatDate(TransDate), UserType, UserPayment, Attn, DPNo, Voucher_No, Remark, Account, Accountname"
    '        Session("DateFieldName") = FDateName.Split(",")
    '        Session("DateFieldValue") = FDateValue.Split(",")
    '        Session("FieldName") = FilterName.Split(",")
    '        Session("FieldValue") = FilterValue.Split(",")
    '        AttachScript("OpenFilterCriteria();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
    '    End Try
    'End Sub

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
                    'BindDataDt(ViewState("TransNmbr"))
                    'BindDataDt2(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    'Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    btnSaveTrans.Visible = True

                    lbOfferFile1.Enabled = True
                    lbOfferFile2.Enabled = True
                    lbOfferFile3.Enabled = True
                    lbOfferFile4.Enabled = True
                    lbOfferFile5.Enabled = True
                    '-----------------------------'
                    fupOfferFile1.Visible = False
                    fupOfferFile1.Enabled = True
                    fupOfferFile2.Visible = False
                    fupOfferFile2.Enabled = True
                    fupOfferFile3.Visible = False
                    fupOfferFile3.Enabled = True
                    fupOfferFile4.Visible = False
                    fupOfferFile4.Enabled = True
                    fupOfferFile5.Visible = False
                    fupOfferFile5.Enabled = True
                    '-----------------------------'
                    btnDeleteDoc1.Visible = True
                    btnDeleteDoc2.Visible = True
                    btnDeleteDoc3.Visible = True
                    btnDeleteDoc4.Visible = True
                    btnDeleteDoc5.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        'GridDt.PageIndex = 0
                        'BindDataDt(ViewState("TransNmbr"))
                        'BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False


                      
                       
                        '-----------------------------'
                        fupOfferFile1.Visible = True
                        fupOfferFile2.Visible = True
                        fupOfferFile3.Visible = True
                        fupOfferFile4.Visible = True
                        fupOfferFile5.Visible = True
                        '-----------------------------'
                        btnAdddt.Visible = False
                        btnAddDt2.Visible = False
                        btnAddDtke2.Visible = False
                        btnAddDt2Ke2.Visible = False
                        '-------------------------------'
                        btnDeleteDoc1.Visible = True
                        btnDeleteDoc2.Visible = True
                        btnDeleteDoc3.Visible = True
                        btnDeleteDoc4.Visible = True
                        btnDeleteDoc5.Visible = True
                        MultiView1.ActiveViewIndex = 0
                        'Menu1.Items.Item(0).Selected = True
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0) And GetCountRecord(ViewState("Dt2")) = 0)
                        'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                    ElseIf GVR.Cells(3).Text = "P" Then
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        'BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(False, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = True
                        btnSaveTrans.Visible = True
                        'btnSaveAll.Visible = True
                        '-----------------------------'
                        fupOfferFile1.Visible = True
                        lbOfferFile1.Visible = True
                        lbOfferFile1.Enabled = True
                        fupOfferFile2.Visible = True
                        lbOfferFile2.Visible = True
                        lbOfferFile2.Enabled = True
                        fupOfferFile3.Visible = True
                        lbOfferFile3.Visible = True
                        lbOfferFile3.Enabled = True
                        fupOfferFile4.Visible = True
                        lbOfferFile4.Visible = True
                        lbOfferFile4.Enabled = True
                        fupOfferFile5.Visible = True
                        lbOfferFile5.Visible = True
                        lbOfferFile5.Enabled = True
                        '----------------------------'
                        btnDeleteDoc1.Visible = True
                        btnDeleteDoc2.Visible = True
                        btnDeleteDoc3.Visible = True
                        btnDeleteDoc4.Visible = True
                        btnDeleteDoc5.Visible = True
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'Else
                        '    lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        '    Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormJobQuotation '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormJobQuotation.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                    'ElseIf DDL.SelectedValue = "Print Full" Then
                    '    Try
                    '        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    '        If CekMenu <> "" Then
                    '            lbStatus.Text = CekMenu
                    '            Exit Sub
                    '        End If
                    '        Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt '''" + GVR.Cells(2).Text + "''','PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                    '        Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
                    '        Session("DBConnection") = ViewState("DBConnection")
                    '        AttachScript("openprintdlg();", Page, Me.GetType)
                    '    Catch ex As Exception
                    '        lbStatus.Text = "btn print Error = " + ex.ToString
                    '    End Try
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
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        ' add the UnitPrice and QuantityTotal to the running total variables
            '        TotalExpense += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '        tbPPh.Text = FormatNumber(TotalExpense, ViewState("DigitHome"))
            '    End If
            'End If
            'tbTotalSelisih.Text = FormatNumber(CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPh.Text) - CFloat(tbBeaTanah.Text), ViewState("DigitHome"))
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            CountTotalAmount()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalPaymentForex As Decimal = 0
    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Dim TotalOther As Decimal = 0

    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    'If DataBinder.Eval(e.Row.DataItem, "FgMode") = "E" Then
                    '    TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                    'ElseIf DataBinder.Eval(e.Row.DataItem, "FgMode") = "O" Then
                    '    TotalOther += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                    'Else
                    '    TotalPayment += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                    '    TotalPaymentForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentForex"))
                    'End If
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'If GetCountRecord(ViewState("Dt2")) > 0 Then
                    '    tbDisc.Text = FormatNumber(TotalPayment, ViewState("DigitHome"))
                    '    tbPPn.Text = FormatNumber(TotalPaymentForex, ViewState("DigitCurr"))
                    '    tbPPnValue.Text = FormatNumber(TotalOther, ViewState("DigitHome"))
                    '    tbBeaTanah.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
                    'Else
                    '    tbDisc.Text = FormatNumber(0, ViewState("DigitHome"))
                    '    tbPPn.Text = FormatNumber(0, ViewState("DigitCurr"))
                    '    tbPPnValue.Text = FormatNumber(0, ViewState("DigitHome"))
                    '    tbBeaTanah.Text = FormatNumber(0, ViewState("DigitHome"))
                    'End If
                End If
                'AttachScript("setformat();", Page, Me.GetType())
                'tbTotalSelisih.Text = FormatNumber(CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPh.Text) - CFloat(tbBeaTanah.Text), ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt2 Row Data Bound Error : " + ex.ToString
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
            'row = ViewState("Dt").Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
            CountTotalAmount()
            'tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"
            'btnSubled.Enabled = tbSubledDt.Enabled
            'ddlCostCenterDt.Enabled = GVR.Cells(6).Text <> "N"
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
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            'btnDocNo.Visible = tbFgModeDt2.Text = "D"
            'tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlPayTypeDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayTypeDt2.SelectedIndexChanged
    '    Try
    '        'Dim VoucherNo As String

    '        'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
    '        'Public Sub ChangePaymentType4(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
    '        'ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
    '        'tbPaymentHomeDt2.Text = FormatFloat(CFloat(tbPaymentForexDt2.Text) * CFloat(tbRateDt2.Text), ViewState("DigitCurr"))
    '        'tbChargeHomeDt2.Text = FormatFloat(CFloat(tbChargeRateDt2.Text) * CFloat(tbChargeForexDt2.Text), ViewState("DigitExpenseCurr"))
    '        'AttachScript("kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();", Page, Me.GetType())
    '        'btnDocNo.Visible = tbFgModeDt2.Text = "D"
    '        'tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
    '        'tbDocumentNoDt2.Text = ""
    '        'VoucherNo = ""
    '        'If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
    '        '    VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'Y', " + QuotedStr(ddlPayTypeDt2.SelectedValue) + ", 'OUT', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
    '        'End If
    '        'tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
    '        'tbVoucherNo.Text = VoucherNo
    '        btnSaveDt.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl Pay Type Select Index Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccount.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT * FROM V_MsAccountDt WHERE TransType = 'PYN' "
    '        ResultField = "Account, Description, FgSubled, Currency, FgType,FgCostCtr"
    '        ViewState("Sender") = "btnAccount"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "Btn Account Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbAccountDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccountDt.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Account", tbAccountDt.Text + "|PYN", ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbAccountDt.Text = Dr("Account")
    '            'tbAccountNameDt.Text = Dr("AccountName")
    '            'BindToDropList(ddlCurrDt, Dr("CurrCode"))
    '            'tbFgType.Text = Dr("FgType").ToString.ToUpper
    '            'ddlCurrDt.Enabled = tbFgType.Text = "PL"
    '            'tbFgSubledDt.Text = Dr("FgSubled")

    '            'ViewState("FgType") = tbFgType.Text
    '            'If ViewState("FgType") = "BS" Then
    '            '    ddlCurrDt.Enabled = False
    '            'Else : ddlCurrDt.Enabled = True
    '            'End If

    '            tbSubledDt_TextChanged(Nothing, Nothing)
    '            'tbFgCostCtr.Text = TrimStr(Dr("FgCostCtr").ToString)
    '            'tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"

    '            'ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
    '            'ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
    '            'AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
    '        Else
    '            tbAccountDt.Text = ""
    '            'tbAccountNameDt.Text = ""
    '            'tbFgSubledDt.Text = ""
    '            tbSubledDt.Text = ""
    '            tbSubledNameDt.Text = ""
    '            tbSubledDt.Enabled = False
    '        End If

    '        'btnSubled.Visible = tbSubledDt.Enabled()
    '        'ddlCostCenterDt.Enabled = tbFgCostCtr.Text <> "N"
    '        'If tbFgCostCtr.Text = "N" Then
    '        '    ddlCostCenterDt.SelectedIndex = 0
    '        'End If
    '        'tbPPnNo.Enabled = (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2"))
    '        'tbPPndate.Enabled = tbPPnNo.Enabled
    '        'ChangeFgSubLed(tbFgSubledDt, tbSubledDt, btnSubled)
    '        'tbAccountDt.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb Product change Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
    '    Dim ResultField As String
    '    Try
    '        'If tbFgSubledDt.Text = "N" Then
    '        '    Exit Sub
    '        'End If
    '        Session("filter") = "SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgSubled = " + QuotedStr(tbFgSubledDt.Text)
    '        ResultField = "Subled_No, Subled_Name"
    '        ViewState("Sender") = "btnSubled"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Subled click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbSubledDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledDt.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        'If tbFgSubledDt.Text = "N" Then
    '        '    tbSubledDt.Text = ""
    '        '    tbSubledNameDt.Text = ""
    '        '    Exit Sub
    '        'End If

    '        'Dr = FindMaster("Subled", tbFgSubledDt.Text + "|" + tbSubledDt.Text, ViewState("DBConnection"))
    '        If Not Dr Is Nothing Then
    '            tbSubledDt.Text = Dr("Subled_No")
    '            'tbSubledNameDt.Text = Dr("Subled_Name")
    '        Else
    '            tbSubledDt.Text = ""
    '            'tbSubledNameDt.Text = ""
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "tb Subled Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbNoLandPurchase_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNoLandPurchase.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("UserType", tbNoLandPurchase.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
    '        If Not Dr Is Nothing Then
    '            tbNoLandPurchase.Text = Dr("User_Code")
    '            tbUserName.Text = Dr("User_Name")
    '            tbAttn.Text = Dr("Contact_Person")
    '        Else
    '            tbNoLandPurchase.Text = ""
    '            tbUserName.Text = ""
    '            tbAttn.Text = ""
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "tb User Code Text Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
    '    Try
    '        ChangeCurrency(ddlChargeCurrDt2, tbPaymentDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
    '        If ddlChargeCurrDt2.SelectedValue = "" Then
    '            tbChargeForexDt2.Text = "0"
    '            tbChargeHomeDt2.Text = "0"
    '        End If
    '        ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
    '        tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
    '        tbChargeRateDt2.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
    '    tbNoLandPurchase.Text = ""
    '    tbUserName.Text = ""
    'End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If CFloat(tbTotalSelisih.Text) <> 0 Then
            '    lbStatus.Text = MessageDlg("Debet - Credit must be balance")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    'If ViewState("PayType").ToString = "" Then
            '    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnDocNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocNo.Click
    '    Dim ResultField As String
    '    Try
    '        If tbNoLandPurchase.Text.Trim = "" Then
    '            Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
    '            "WHERE PPn = 0 and Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
    '        Else
    '            Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
    '            "WHERE PPn = 0 and User_Type = " + QuotedStr(".") + " AND User_Code =" + QuotedStr(tbNoLandPurchase.Text) + " AND Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
    '        End If
    '        ResultField = "DP_No, Currency, Rate, Total_Forex, Total_Paid, User_Type, User_Code, User_Name"
    '        ViewState("Sender") = "btnDocNo"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType()) 'ddlUserType.SelectedValue
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Doc No Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlCurrDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDt.SelectedIndexChanged
    '    Try
    '        ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
    '        ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
    '        AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl Currency Error : " + ex.ToString
    '    End Try
    'End Sub

    'Public Sub ChangePaymentType4(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
    '    Try
    '        If State = "Add" Then
    '            Dim dr As DataRow
    '            TxFgMode.Text = "O"
    '            If Not Payment.Trim = "" Then
    '                dr = FindMaster("PayType", Payment, DBConnection)
    '                If Not dr Is Nothing Then
    '                    BindToText(TxFgMode, dr("FgMode").ToString)
    '                    BindToDropList(ddlCurr, dr("Currency").ToString)
    '                End If
    '            End If
    '            If Not TxFgMode.Text = "G" Then
    '                txduedate.SelectedDate = Nothing
    '                txduedate.DisplayType = DatePickerDisplayType.TextBox
    '                ddlbank.SelectedIndex = 0
    '            Else
    '                txduedate.SelectedDate = txdate.SelectedDate
    '                txduedate.DisplayType = DatePickerDisplayType.TextBoxAndImage
    '            End If

    '            ChangeCurrency(ddlCurr, txdate, txRate, ViewState("Currency"), ViewState("DigitCurr"), DBConnection)
    '        End If
    '        txduedate.Enabled = TxFgMode.Text = "G"
    '        ddlbank.Enabled = TxFgMode.Text = "G"
    '    Catch ex As Exception
    '        Throw New Exception("Change Payment Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnSupplier1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupplier1.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Supplier_Code,Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "Supplier_Code,Supplier_Name"
            CriteriaField = "Supplier_Code,Supplier_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSupplier1"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier1 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupplier2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier2.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Supplier_Code,Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "Supplier_Code,Supplier_Name"
            CriteriaField = "Supplier_Code,Supplier_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSupplier2"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier1 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupplier3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier3.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Supplier_Code,Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "Supplier_Code,Supplier_Name"
            CriteriaField = "Supplier_Code,Supplier_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSupplier3"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier1 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupplier4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier4.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Supplier_Code,Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "Supplier_Code,Supplier_Name"
            CriteriaField = "Supplier_Code,Supplier_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSupplier4"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier4 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupplier5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier5.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Supplier_Code,Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "Supplier_Code,Supplier_Name"
            CriteriaField = "Supplier_Code,Supplier_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSupplier5"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier5 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GVNoRecvDoc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVNoRecvDoc.SelectedIndexChanged
        Try
        Catch ex As Exception
            lbStatus.Text = "Selected Index Changed Error " + ex.ToString
        End Try
    End Sub

    Protected Sub GVNoRecvDoc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVNoRecvDoc.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
        '    e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
        '    e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        '    e.Row.Attributes("style") = "cursor:pointer"
        '    e.Row.Attributes("ondblclick") = "javascript:ClosePopupRecvDoc();"
        'End If
    End Sub

    Protected Sub OnDataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        'For i As Integer = 0 To GVNoRecvDoc.Columns.Count - 1
        '    Dim cell As New TableHeaderCell()
        '    Dim txtSearch As New TextBox()
        '    txtSearch.Attributes("placeholder") = GVNoRecvDoc.Columns(i).HeaderText
        '    txtSearch.CssClass = "search_textbox"
        '    cell.Controls.Add(txtSearch)
        '    row.Controls.Add(cell)
        'Next
        'GVNoRecvDoc.HeaderRow.Parent.Controls.AddAt(1, row)

        Dim str As String = Nothing
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        If str = Nothing Then
            Return
        Else
            If (GVNoRecvDoc.Rows.Count > 0) Then
                For i As Integer = 2 To GVNoRecvDoc.Columns.Count - 1
                    Dim cell As New TableHeaderCell()
                    Dim txtSearch As New TextBox()
                    txtSearch.Attributes("placeholder") = GVNoRecvDoc.Columns(i).HeaderText
                    txtSearch.CssClass = "search_textbox"
                    cell.Controls.Add(txtSearch)
                    row.Controls.Add(cell)
                Next
                GVNoRecvDoc.HeaderRow.Parent.Controls.AddAt(1, row)
            End If
        End If

    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GVNoRecvDoc.PageIndex = e.NewPageIndex
        Me.BindGridCIPLicenRecvDoc()
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopupNoRecvDoc();", True)
    End Sub

    Protected Sub GVNoRecvDoc_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles GVNoRecvDoc.SelectedIndexChanging
        'GVNoRecvDoc.SelectedIndex = -1
    End Sub

    Protected Sub btnInstruksiNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstruksiNo.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "EXEC S_FINGetInstrJobNo" '+ QuotedStr(tbCode.Text) 'WHERE Fg_Active = 'Y'
            'Session("filter") = "SELECT AreaCode,AreaName FROM MsArea" ' WHERE FgActive = 'Y' "
            ResultField = "TransNmbr, TransDate, Status, Nama_Area, Pekerjaan, Total_HPS"
            CriteriaField = "TransNmbr, TransDate, Status, Nama_Area, Pekerjaan, Total_HPS"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnInstruksiNo"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnInstruksiNo_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GVArea_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVArea.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupArea();"
        End If
    End Sub

    Protected Sub lbOfferFile1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOfferFile1.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("OfferFile1").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("OfferFile1").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbOfferFile1_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbOfferFile2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOfferFile2.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("OfferFile2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("OfferFile2").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbOfferFile2_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbOfferFile3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOfferFile3.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("OfferFile3").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("OfferFile3").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbOfferFile3_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbOfferFile4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOfferFile4.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("OfferFile4").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("OfferFile4").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbOfferFile4_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbOfferFile5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOfferFile5.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("OfferFile5").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("OfferFile5").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbOfferFile5_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFinishJobDate1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate1.SelectionChanged, tbStartJobDate1.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate1.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate1.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi1.Text = Sqlstring

        If tbDurasi1.Text < 0 Then
            lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
            tbStartJobDate1.SelectedDate = tbFinishJobDate1.SelectedDate
            Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate1.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate1.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
            tbDurasi1.Text = Sqlstring
            tbStartJobDate1.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub tbFinishJobDate2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate2.SelectionChanged, tbStartJobDate2.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate2.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate2.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi2.Text = Sqlstring

        If tbDurasi2.Text < 0 Then
            lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
            tbStartJobDate2.SelectedDate = tbFinishJobDate2.SelectedDate
            Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate2.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate2.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
            tbDurasi2.Text = Sqlstring
            tbStartJobDate2.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub tbFinishJobDate3_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate3.SelectionChanged, tbStartJobDate3.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate3.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate3.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi3.Text = Sqlstring

        If tbDurasi3.Text < 0 Then
            lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
            tbStartJobDate3.SelectedDate = tbFinishJobDate3.SelectedDate
            Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate3.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate3.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
            tbDurasi3.Text = Sqlstring
            tbStartJobDate3.Focus()
            Exit Sub
        End If

    End Sub

    Protected Sub tbFinishJobDate4_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate4.SelectionChanged, tbStartJobDate4.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate4.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate4.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi4.Text = Sqlstring


        If tbDurasi4.Text < 0 Then
            lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
            tbStartJobDate4.SelectedDate = tbFinishJobDate4.SelectedDate
            Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate4.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate4.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
            tbDurasi4.Text = Sqlstring
            tbStartJobDate4.Focus()
            Exit Sub
        End If
    End Sub

    Protected Sub tbFinishJobDate5_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate5.SelectionChanged, tbStartJobDate5.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate5.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate5.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi5.Text = Sqlstring

        If tbDurasi5.Text < 0 Then
            lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
            tbStartJobDate5.SelectedDate = tbFinishJobDate5.SelectedDate
            Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate5.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate5.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
            tbDurasi5.Text = Sqlstring
            tbStartJobDate5.Focus()
            Exit Sub
        End If

    End Sub

    Protected Sub btnDeleteDoc1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc1.Click
        Dim strSQL As String
        strSQL = "UPDATE FINCIPOffeRecvHd SET OfferFile1=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbOfferFile1.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnDeleteDoc2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc2.Click
        Dim strSQL As String
        strSQL = "UPDATE FINCIPOffeRecvHd SET OfferFile2=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbOfferFile2.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnDeleteDoc3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc3.Click
        Dim strSQL As String
        strSQL = "UPDATE FINCIPOffeRecvHd SET OfferFile3=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbOfferFile3.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnDeleteDoc4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc4.Click
        Dim strSQL As String
        strSQL = "UPDATE FINCIPOffeRecvHd SET OfferFile4=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbOfferFile4.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnDeleteDoc5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc5.Click
        Dim strSQL As String
        strSQL = "UPDATE FINCIPOffeRecvHd SET OfferFile5=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbOfferFile5.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

End Class
