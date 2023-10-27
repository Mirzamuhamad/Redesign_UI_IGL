Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.OleDb.OleDbDataReader
Imports System.Web.Services
Imports System.Configuration
Imports System.IO
Imports BasicFrame.WebControls

Public Class lbStatus
    Public Sub New()

    End Sub
End Class

Partial Class Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT * FROM V_PRCIPLicenAdmInvHd"
    Public strTransNmbr As String
    '  Public Nilai, sTermCode As String
    Private Shared PageSize As Integer = 5
    Dim tb As TextBox

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    'Private Function GetStringDt2(ByVal Nmbr As String) As String
    '    Return "SELECT * FROM V_PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                'BindDataToGridView("S_PRCFindCIPRecvDoc", GVNoRecvDoc, ViewState("DBConnection").ToString)
                BindGridSupplier()
                'BindGridCIPLicenRecvDoc()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnGetNoLP" Then
                    'tbApplfileNo.Text = Session("Result")(0).ToString
                    'BindToText(tbPPn, Session("Result")(1).ToString, ViewState("DigitCurr"))
                    'BindToText(tbBaseForex, Session("Result")(2).ToString, ViewState("DigitCurr"))
                    'BindToText(tbDisc, Session("Result")(3).ToString, ViewState("DigitCurr"))
                    'BindToText(tbPPh, Session("Result")(4).ToString, ViewState("DigitCurr"))
                    'BindToText(tbPPnValue, Session("Result")(5).ToString, ViewState("DigitCurr"))
                    'BindToText(tbRemark, Session("Result")(6).ToString)
                End If
                If ViewState("Sender") = "btnDocNo" Then
                    'tbDocumentNoDt2.Text = Session("Result")(0).ToString
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
                If ViewState("Sender") = "btnRecvDoc" Then
                    tbRecvDoc.Text = Session("Result")(0).ToString
                    tbApplfileNo.Text = Session("Result")(2).ToString
                    'tbApplfileDate.Text = Session("Result")(3).ToString
                    'BindToDate(tbApplfileDate, Dt.Rows(0)("ApplfileDate").ToString)
                    BindToDate(tbApplfileDate, Session("Result")(3).ToString)
                    'BindToDropList(ddlCurrDt, Session("Result")(3).ToString)
                    'tbFgType.Text = Session("Result")(4).ToString.ToUpper
                    tbHGBNo.Text = Session("Result")(4).ToString
                    tbRemark.Text = Session("Result")(8).ToString
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
                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            'If Session("FileUpload1") Is Nothing AndAlso fupRecvFile.HasFile Then
            '    Session("FileUpload1") = fupRecvFile
            '    lbRecvFile.Text = fupRecvFile.FileName
            'ElseIf Session("FileUpload1") IsNot Nothing AndAlso (Not fupRecvFile.HasFile) Then
            '    fupRecvFile = DirectCast(Session("FileUpload1"), FileUpload)
            '    lbRecvFile.Text = fupRecvFile.FileName
            'ElseIf fupRecvFile.HasFile Then
            '    Session("FileUpload1") = fupRecvFile
            '    lbRecvFile.Text = fupRecvFile.FileName
            'End If

            'If Session("FileUpload2") Is Nothing AndAlso fupApplFile.HasFile Then
            '    Session("FileUpload2") = fupApplFile
            '    lbApplFile.Text = fupApplFile.FileName
            'ElseIf Session("FileUpload2") IsNot Nothing AndAlso (Not fupApplFile.HasFile) Then
            '    fupApplFile = DirectCast(Session("FileUpload2"), FileUpload)
            '    lbApplFile.Text = fupApplFile.FileName
            'ElseIf fupApplFile.HasFile Then
            '    Session("FileUpload2") = fupApplFile
            '    lbApplFile.Text = fupApplFile.FileName
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
            lbDisc.Text = "Potongan" '(" + ViewState("Currency") + ")"
            lbPPnForex.Text = "PPn Value" '(" + ViewState("Currency") + ")"
            lbPPn.Text = "PPn %" '(" + ViewState("Currency") + ")"
            lbPPh.Text = "PPh %" '(" + ViewState("Currency") + ")"
            lbTotBiaya.Text = "Total Biaya" '(" + ViewState("Currency") + ")"
            'FillCombo(ddlStructureCode, "SELECT * FROM MsStructure ORDER BY ID ASC", True, "StructureCode", "StructureCode", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlPPh, "SELECT PPHCode, PPhName FROM V_MsPPh ", True, "PPHCode", "PPhName", ViewState("DBConnection"))
            'FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
            FillCombo(ddlUnit, "SELECT Unit_Code, Unit_Name FROM VMsUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "SELECT PayCode, PayName FROM MsPayType WHERE FgType='P' ", True, "PayCode", "PayName", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser(" + QuotedStr("PaymentNT" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment ('*')", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment 'Y' ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'ViewState("AccPPn") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc ", ViewState("DBConnection"))
            'ViewState("AccPPn2") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc2 ", ViewState("DBConnection"))
            'ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("PPN") = 0
            ViewState("DigitCurrAcc") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
                'ddlCommand.Items.Add("Print Full")
                'ddlCommand2.Items.Add("Print Full")
            End If
            lbTitle.Text = "CIP License & Adm Invoice" 'Request.QueryString("MenuName").ToString
            'fupRecvFile.Attributes("onchange") = "UploadfupRecvFile(this)"
            'fupApplFile.Attributes("onchange") = "UploadfupApplFile(this)"
            'tbPPn.Attributes.Add("ReadOnly", "True")
            'tbDisc.Attributes.Add("ReadOnly", "True")
            'tbPPnValue.Attributes.Add("ReadOnly", "True")
            'tbPPh.Attributes.Add("ReadOnly", "True")
            'tbBaseForex.Attributes.Add("ReadOnly", "True")
            'tbPPhValue.Attributes.Add("ReadOnly", "True")

            tbBaseForex.Attributes.Add("OnBlur", "setformatforhd();")
            tbDPP.Attributes.Add("OnBlur", "setformatforhd();")
            tbDisc.Attributes.Add("OnBlur", "setformatforhd();")
            tbPPn.Attributes.Add("OnBlur", "setformatforhd();")
            tbPPnValue.Attributes.Add("OnBlur", "setformatforhd();")
            tbPPh.Attributes.Add("OnBlur", "setformatforhd();")
            tbPPhValue.Attributes.Add("OnBlur", "setformatforhd();")
            tbTotalAmount.Attributes.Add("OnBlur", "setformatforhd();")
            tbType.Attributes.Add("OnBlur", "setformathd();")

            'tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            'tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            'tbPaymentHomeDt2.Attributes.Add("ReadOnly", "True")

            tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPn.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPh.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")

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

    Private Sub BindGridSupplier()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT SuppCode, SuppName FROM MsSupplier WHERE FgActive='Y' ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVSupplier.DataSource = dt
                        GVSupplier.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    '<System.Web.Services.WebMethod()> _
    'Public Shared Function GetXRange(ByVal sTermCode As String) As String
    Public Function GetXRange(ByVal sTermCode As String) As String
        Dim Nilai As String
        Dim sdr As SqlDataReader
        Dim conStr As String = ViewState("DBConnection").ToString
        'Dim constr As String = ConfigurationManager.ConnectionStrings("DBConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                'Using sdr As New SqlDataReader()
                cmd.Connection = con
                cmd.CommandText = "SELECT xrange FROM MsTerm WHERE TermCode = " + QuotedStr(sTermCode)    'QuotedStr(ddlTerm.SelectedValue)
                con.Open()
                sdr = cmd.ExecuteReader
                While sdr.Read
                    If sdr.HasRows = True Then
                        Nilai = sdr("xrange").ToString()  'DateTime.Now.ToString()
                        Return Nilai
                    End If
                End While
                sdr.Close()
                'End Using
            End Using
        End Using
    End Function

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
            strFilter = "SELECT SuppCode, SuppName FROM MsSupplier WHERE FgActive='Y' " 'AND SuppName LIKE '%" + txtSearchSupplier.Text.Trim() + "%'"
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
            If Not String.IsNullOrEmpty(txtSearchSupplier.Text.Trim()) Then
                'strFilter += String.Format(" WHERE SuppName LIKE = '{0}'", txtSearchSupplier.Text.Trim())
                strFilter += String.Format(" AND SuppName LIKE '%", txtSearchSupplier.Text.Trim())
            End If
            Dim conStr As String = ViewState("DBConnection").ToString
            Using con As New SqlConnection(conStr)
                Using ds As New DataSet()
                    con.Open()
                    Using cmd As New SqlCommand(strFilter)
                        Using sda As New SqlDataAdapter(cmd)
                            sda.Fill(ds, "MsSupplier")
                            GVSupplier.DataSource = ds.Tables(0).DefaultView
                            GVSupplier.DataBind()
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            lbStatus.Text = "Error on btn search klik : " + ex.ToString
        End Try
    End Sub

    Private Sub BindGridCIPLicenRecvDoc()
        'Dim constr As String = ConfigurationManager.ConnectionStrings("ConnStr").ConnectionString
        'Dim conStr As String = ViewState("DBConnection").ToString
        'Using con As New SqlConnection(conStr)
        '    Using cmd As New SqlCommand("SELECT TransNmbr,JmlBiaya,TtlHrgTanah,BiayaBPHTB,BiayaSurvey,BiayaLainLain,Remark FROM GLLandPurchaseHd")
        '        Using sda As New SqlDataAdapter()
        '            cmd.Connection = con
        '            sda.SelectCommand = cmd
        '            Using dt As New DataTable()
        '                sda.Fill(dt)
        '                GVNoRecvDoc.DataSource = dt
        '                GVNoRecvDoc.DataBind()
        '            End Using
        '        End Using
        '    End Using
        'End Using
        '-----------------------------------------------------------------------------'
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
        Dim TotalForex As Double
        Dim Dr As DataRow
        Try
            TotalForex = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotalForex = TotalForex + CFloat(Dr("BeaForex").ToString)
                End If
            Next
            tbBaseForex.Text = FormatNumber(TotalForex, ViewState("DigitHome"))
            tbDPP.Text = FormatNumber(CFloat(tbBaseForex.Text) - CFloat(tbDisc.Text), ViewState("DigitHome"))
            tbTotalAmount.Text = FormatNumber(CFloat(tbBaseForex.Text) - CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPhValue.Text), ViewState("DigitHome"))
        Catch ex As Exception
            Throw New Exception("Count TotalForex Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CalculateTotalAmount()
        Dim TotalAmount, TotalInvoice, TotalDpp, TotalPotongan, TotalPPN, TotalPPH As Double
        Dim Dr As DataRow
        Try
            TotalInvoice = 0
            TotalDpp = 0
            TotalPotongan = 0
            TotalPPN = 0
            TotalPPH = 0
            TotalAmount = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotalInvoice = TotalInvoice + CFloat(Dr("BeaForex").ToString)
                    'TotalDpp = TotalDpp + CFloat(Dr("Dpp").ToString)
                    'TotalPotongan = TotalPotongan + CFloat(Dr("Potongan").ToString)
                    'TotalPPN = TotalPPN + CFloat(Dr("PpnInvoice").ToString)
                    'TotalPPH = TotalPPH + CFloat(Dr("PPhInvoice").ToString)
                    'TotalAmount = TotalAmount + CFloat(Dr("TotalAmount").ToString)
                End If
            Next
            'TbTotalInvoiceHd.Text = FormatNumber(TotalInvoice, ViewState("DigitHome"))
            'tbTotalPotongan.Text = FormatNumber(TotalPotongan, ViewState("DigitHome"))
            'tbTotalDpp.Text = FormatNumber(TotalDpp, ViewState("DigitHome"))
            'TbtotalPPN.Text = FormatNumber(TotalPPN, ViewState("DigitHome"))
            'TbTotalPPH.Text = FormatNumber(TotalPPH, ViewState("DigitHome"))
            'tbTotalInvoice.Text = FormatNumber(TotalAmount, ViewState("DigitHome"))

            'tbTotalAmount.Text = FormatNumber(CFloat(tbTotalDpp.Text) + CFloat(TbtotalPPN.Text) - CFloat(TbTotalPPH.Text), ViewState("DigitHome"))

            tbBaseForex.Text = FormatNumber(TotalInvoice, ViewState("DigitHome"))
            'tbDPP.Text = FormatNumber(CFloat(tbBaseForex.Text) - CFloat(tbDisc.Text), ViewState("DigitHome"))
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
                Session("SelectCommand") = "EXEC S_FNFormCIPAdmInv '" + Result + "'," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormSuppAdmINV.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRCIPLicenAdmInv", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnSupplier.Visible = State
            'ddlUserType.Enabled = State
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
            'If dt.Rows.Count > 0 Then
            'GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

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
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbApplfileNo.Text = ""
            tbRecvDoc.Text = ""
            tbHGBNo.Text = ""
            'ddlReport.SelectedValue = "Y"
            'ddlUserType.SelectedValue = "Supplier"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbSPSDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbApplfileDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbBaseForex.Text = "0"
            tbDisc.Text = "0"
            tbDPP.Text = "0"
            'tbPPn.Text = "0"  'FormatFloat(0, ViewState("DigitCurr"))
            tbPPn.Text = ViewState("PPN")
            tbPPnValue.Text = "0"
            tbPPh.Text = "0"
            tbPPhValue.Text = "0"
            tbTotalAmount.Text = "0"
            tbDocNo1.Text = ""
            tbDocNo2.Text = ""
            tbRemark.Text = ""
            lbRecvFile.Text = ""
            lbApplFile.Text = ""
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
            ddlUnit.SelectedIndex = 0
            tbPriceForex.Text = "0"
            tbLocationName.Text = ""
            'ddlCurrDt.SelectedValue = ViewState("Currency")
            'tbRateDt.Text = FormatFloat(1, ViewState("DigitRate"))
            'tbRateDt.Enabled = False
            'ddlCurrDt.Enabled = False
            tbBiaya.Text = "0"
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
            If tbSPSDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("SPS Date must have value")
                tbSPSDate.Focus()
                Return False
            End If
            If tbApplfileDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Tgl Berkas Permohonan must have value")
                tbApplfileDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Code must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If tbSuppName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Name must have value")
                tbSuppName.Focus()
                Return False
            End If
            If tbRecvDoc.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Terima Dokumen must have value")
                tbRecvDoc.Focus()
                Return False
            End If
            If tbApplfileNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Berkas Permohonan must have value")
                tbApplfileNo.Focus()
                Return False
            End If
            If tbHGBNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Alas Hak must have value")
                tbHGBNo.Focus()
                Return False
            End If
            If tbRemark.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Remark must have value")
                tbRemark.Focus()
                Return False
            End If

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
                    lbStatus.Text = MessageDlg("Job Name Must Have Value")
                    tbJobName.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Satuan Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If tbLocationName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Name Must Have Value")
                    tbLocationName.Focus()
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
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            'BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("SuppCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("SuppName").ToString)
            BindToText(tbRecvDoc, Dt.Rows(0)("RecvDocNo").ToString)
            BindToText(tbApplfileNo, Dt.Rows(0)("ApplfileNo").ToString)
            BindToDate(tbApplfileDate, Dt.Rows(0)("ApplfileDate").ToString)
            BindToDate(tbSPSDate, Dt.Rows(0)("SPSDate").ToString)
            BindToText(tbHGBNo, Dt.Rows(0)("HGBNo").ToString)
            BindToText(tbDocNo1, Dt.Rows(0)("DocNo1").ToString)
            BindToText(tbDocNo2, Dt.Rows(0)("DocNo2").ToString)
            lbRecvFile.Text = Dt.Rows(0)("RecvFile").ToString
            lbApplFile.Text = Dt.Rows(0)("ApplFile").ToString
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("TermPayment").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbForexRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitHome"))
            BindToDropList(ddlPPh, Dt.Rows(0)("PPHCode").ToString)
            BindToText(tbType, Dt.Rows(0)("Type").ToString)

            'ViewState("DigitCurr") = SQLExecuteScalar("SELECT Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            'If ViewState("DigitCurr") = Nothing Then
            '    ViewState("DigitCurr") = 0
            'End If
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitHome"))
            BindToText(tbDisc, Dt.Rows(0)("DiscForex").ToString, ViewState("DigitHome"))
            BindToText(tbDPP, Dt.Rows(0)("DPPForex").ToString, ViewState("DigitHome"))
            BindToText(tbPPn, Dt.Rows(0)("PPn").ToString, CInt(ViewState("DigitCurr")))
            BindToText(tbPPnValue, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitHome"))
            BindToText(tbPPh, Dt.Rows(0)("PPh").ToString, ViewState("DigitHome"))
            BindToText(tbPPhValue, Dt.Rows(0)("PPhForex").ToString, ViewState("DigitHome"))
            BindToText(tbTotalAmount, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitHome"))
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
                BindToText(tbQty, Dr(0)("Qty").ToString)
                'BindToText(tbSubledNameDt, Dr(0)("SubledName").ToString)
                'BindToText(tbPPnNo, Dr(0)("PPnNo").ToString)
                'BindToDate(tbPPndate, Dr(0)("PPNDate").ToString)
                BindToDropList(ddlUnit, Dr(0)("UnitCode").ToString)
                'BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbLocationName, Dr(0)("LocationName").ToString)
                BindToText(tbBiaya, Dr(0)("BeaForex").ToString)
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
                Row("Qty") = Val(tbQty.Text)
                Row("UnitCode") = ddlUnit.SelectedValue
                Row("PriceForex") = Val(tbPriceForex.Text)
                Row("LocationName") = tbLocationName.Text
                Row("UnitCode") = ddlUnit.SelectedValue
                Row("BeaForex") = tbBiaya.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgActive") = "Y"
                'Row("FgSubled") = tbFgSubledDt.Text
                'Row("Subled") = tbSubledDt.Text
                'Row("SubledName") = tbSubledNameDt.Text
                'If tbPPndate.Enabled Then
                '    Row("PPnDate") = tbPPndate.SelectedDate.ToString
                'Else
                '    Row("PPnDate") = DBNull.Value
                'End If
                'Row("ForexRate") = tbRateDt.Text
                'If Row("CostCtr") = "" Then
                '    Row("Costctr") = DBNull.Value
                'End If
                'Row("Amount") = Val(tbSubledDt.Text) * Val(tbAmountForexDt.Text)
                'Row("AmountHome") = tbAmountHomeDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                'dr("CIPAI") = ddlUserType.SelectedValue
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
                dr("Qty") = Val(tbQty.Text)
                dr("UnitCode") = ddlUnit.SelectedValue
                dr("PriceForex") = Val(tbPriceForex.Text)
                dr("LocationName") = tbLocationName.Text
                dr("BeaForex") = tbBiaya.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgActive") = "Y"
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
        Dim I As Integer
        Dim pathRecv, pathAppl, namafileRecv, namafileAppl As String
        Try
            'System.Threading.Thread.Sleep(7000)
            'If CFloat(tbDisc.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Payment Notaris must have value")
            '    tbPPn.Focus()
            '    Exit Sub
            'End If

            'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
            'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
            'CR : ItemNo, PaymentType, PaymentName, PaymentDate, DocumentNo, Currency, ForexRate, PaymentForex, PaymentHome, Remark, DueDate, BankPayment, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("CLI", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                pathRecv = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupRecvFile.FileName
                namafileRecv = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupRecvFile.FileName

                pathAppl = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupApplFile.FileName
                namafileAppl = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupApplFile.FileName

                SQLString = "INSERT INTO PRCIPLicenAdmInvHd(TransNmbr,TransDate,Status,SuppCode,RecvDocNo,ApplfileNo,ApplfileDate,HGBNo,CIPAdm,SPSDate,DocNo1,DocNo2,RecvFile,ApplFile,Term," + _
                "TermPayment,DueDate,Currency,ForexRate,BaseForex,Disc,DiscForex,DPPForex,PPn,PPnForex,PPh,PPhForex,TotalForex,PPHCode,Type,Remark,FgReport,FgActive,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbSuppCode.Text) + ", " + _
                QuotedStr(tbRecvDoc.Text) + ", " + QuotedStr(tbApplfileNo.Text) + ", " + QuotedStr(Format(tbApplfileDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbHGBNo.Text) + ", " + QuotedStr(ddlCIP.SelectedValue) + ", " + QuotedStr(Format(tbSPSDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbDocNo1.Text) + ", " + QuotedStr(tbDocNo2.Text) + ", " + QuotedStr(namafileRecv) + ", " + QuotedStr(namafileAppl) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + "," + QuotedStr(ddlTerm.SelectedValue) + ", " + QuotedStr(Format(tbDueDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + QuotedStr(tbForexRate.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbBaseForex.Text.Replace(",", "")) + ", " + QuotedStr(tbDisc.Text.Replace(",", "")) + ", " + QuotedStr(tbDisc.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbDPP.Text.Replace(",", "")) + ", " + QuotedStr(tbPPn.Text.Replace(",", "")) + ", " + QuotedStr(tbPPnValue.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbPPh.Text.Replace(",", "")) + ", " + QuotedStr(tbPPhValue.Text.Replace(",", "")) + ", " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(ddlPPh.SelectedValue) + "," + QuotedStr(tbType.Text) + ", " + QuotedStr(tbRemark.Text) + ", " + _
                QuotedStr("Y") + ", " + QuotedStr("Y") + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text 'tbTotalSelisih.Text.Replace(",", "")
                fupRecvFile.SaveAs(pathRecv)
                fupApplFile.SaveAs(pathAppl)
            Else
                'Dim cekStatus As String
                'cekStatus = SQLExecuteScalar("SELECT Status FROM PRCIPLicenAdmInvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "P" Then
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If
                pathRecv = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupRecvFile.FileName
                namafileRecv = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupRecvFile.FileName

                pathAppl = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupApplFile.FileName
                namafileAppl = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupApplFile.FileName

                SQLString = "UPDATE PRCIPLicenAdmInvHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", SuppCode =" + QuotedStr(tbSuppCode.Text) + ", RecvDocNo =" + QuotedStr(tbRecvDoc.Text) + ", ApplfileNo =" + QuotedStr(tbApplfileNo.Text) + _
                ", ApplfileDate =" + QuotedStr(Format(tbApplfileDate.SelectedValue, "yyyy-MM-dd")) + ", HGBNo =" + QuotedStr(tbHGBNo.Text) + _
                ", CIPAdm = " + QuotedStr(ddlCIP.SelectedValue) + ", SPSDate =" + QuotedStr(Format(tbSPSDate.SelectedValue, "yyyy-MM-dd")) + _
                ", DocNo1 = " + QuotedStr(tbDocNo1.Text) + ", DocNo2 = " + QuotedStr(tbDocNo1.Text) + _
                ", RecvFile = " + QuotedStr(namafileRecv) + ", ApplFile = " + QuotedStr(namafileAppl) + ", Term = " + QuotedStr(ddlTerm.SelectedValue) + _
                ", TermPayment = " + QuotedStr(ddlTerm.SelectedValue) + ", DueDate = " + QuotedStr(Format(tbDueDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + QuotedStr(tbForexRate.Text.Replace(",", "")) + _
                ", BaseForex = " + QuotedStr(tbBaseForex.Text.Replace(",", "")) + ", DiscForex = " + QuotedStr(tbDisc.Text.Replace(",", "")) + _
                ", DPPForex = " + QuotedStr(tbDPP.Text.Replace(",", "")) + ", PPn = " + QuotedStr(tbPPn.Text.Replace(",", "")) + _
                ", PPnForex = " + QuotedStr(tbPPnValue.Text.Replace(",", "")) + ", PPh = " + QuotedStr(tbPPh.Text.Replace(",", "")) + _
                ", PPhForex = " + QuotedStr(tbPPhValue.Text.Replace(",", "")) + ", TotalForex = " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + _
                ", PPHCode = " + QuotedStr(ddlPPh.SelectedValue) + ", Type = " + QuotedStr(tbType.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep= " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) 'ddlUserType.SelectedValue, tbAttn.Text, tbTotalSelisih.Text.Replace(",", "")
                fupRecvFile.SaveAs(pathRecv)
                fupApplFile.SaveAs(pathAppl)
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
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()                                            'JobCode,  
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark,FgActive FROM PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand. 'Account=@Account, JobCode=@JobCode,
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCIPLicenAdmInvDt SET JobName=@JobName,Qty=@Qty,UnitCode=@UnitCode,PriceForex=@PriceForex,LocationName=@LocationName, " + _
                    "BeaForex=@BeaForex,Remark=@Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            'Update_Command.Parameters.Add("@JobCode", SqlDbType.VarChar, 12, "JobCode")
            Update_Command.Parameters.Add("@JobName", SqlDbType.VarChar, 100, "JobName")
            'Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 20, "Account")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Decimal, 18, "Qty")
            'Update_Command.Parameters.Add("@PPnDate", SqlDbType.DateTime, 8, "PPnDate")
            Update_Command.Parameters.Add("@UnitCode", SqlDbType.VarChar, 6, "UnitCode")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Decimal, 18, "PriceForex")
            Update_Command.Parameters.Add("@LocationName", SqlDbType.VarChar, 50, "LocationName")
            Update_Command.Parameters.Add("@BeaForex", SqlDbType.Decimal, 18, "BeaForex")
            'Update_Command.Parameters.Add("@Amount", SqlDbType.Decimal, 18, "Amount")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCIPLicenAdmInvDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCIPLicenAdmInvDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'ExecuteImportDB()
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Job must have at least 1 record")
                Exit Sub
            End If
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
            fupRecvFile.Visible = True
            fupApplFile.Visible = True
            'Session.Remove("FileUpload1")
            'Session.Remove("FileUpload2")
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
            tbSPSDate.SelectedDate = ViewState("ServerDate")
            ddlCurr.SelectedValue = ViewState("Currency")
            tbDueDate.SelectedDate = ViewState("ServerDate")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            'Cleardt2()
            tbForexRate.Text = "1"
            tbForexRate.Enabled = False
            PnlDt.Visible = True
            btnDeleteDoc1.Visible = False
            btnDeleteDoc2.Visible = False
            BindDataDt("")
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
                    BindDataDt(ViewState("TransNmbr"))
                    'BindDataDt2(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    'Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    btnSaveTrans.Visible = True
                    '-----------------------------'
                    fupRecvFile.Visible = True
                    fupRecvFile.Enabled = True
                    fupApplFile.Visible = True
                    fupApplFile.Enabled = True
                    lbRecvFile.Visible = True
                    lbRecvFile.Enabled = True
                    lbApplFile.Visible = True
                    lbApplFile.Enabled = True
                    '-----------------------------'
                    btnDeleteDoc1.Visible = True
                    btnDeleteDoc2.Visible = True
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
                        'BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        '-----------------------------'
                        fupRecvFile.Visible = True
                        fupApplFile.Visible = True
                        '-----------------------------'
                        btnDeleteDoc1.Visible = True
                        btnDeleteDoc2.Visible = True
                        MultiView1.ActiveViewIndex = 0
                        'Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
                        'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                    ElseIf GVR.Cells(3).Text = "P" Then
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(False, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = True
                        btnSaveTrans.Visible = True
                        'btnSaveAll.Visible = True
                        fupRecvFile.Visible = True
                        lbRecvFile.Visible = True
                        lbRecvFile.Enabled = True
                        fupApplFile.Visible = True
                        lbApplFile.Visible = True
                        lbApplFile.Enabled = True
                        btnDeleteDoc1.Visible = True
                        btnDeleteDoc2.Visible = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        'Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormCIPAdmInv '" + GVR.Cells(2).Text + "'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormSuppAdmINV.frx"
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

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            'row = ViewState("Dt").Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            'FillTextBoxDt(CInt(GVR.Cells(1).Text))  'CInt(lbItemNo.Text)
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

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            'dr = ViewState("Dt").Select("ItemNo = " + CInt(GVR.Cells(1).Text))
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

    'Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                ' add the UnitPrice and QuantityTotal to the running total variables
    '                'If DataBinder.Eval(e.Row.DataItem, "FgMode") = "E" Then
    '                '    TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
    '                'ElseIf DataBinder.Eval(e.Row.DataItem, "FgMode") = "O" Then
    '                '    TotalOther += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
    '                'Else
    '                '    TotalPayment += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
    '                '    TotalPaymentForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentForex"))
    '                'End If
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                'If GetCountRecord(ViewState("Dt2")) > 0 Then
    '                '    tbDisc.Text = FormatNumber(TotalPayment, ViewState("DigitHome"))
    '                '    tbPPn.Text = FormatNumber(TotalPaymentForex, ViewState("DigitCurr"))
    '                '    tbPPnValue.Text = FormatNumber(TotalOther, ViewState("DigitHome"))
    '                '    tbBeaTanah.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
    '                'Else
    '                '    tbDisc.Text = FormatNumber(0, ViewState("DigitHome"))
    '                '    tbPPn.Text = FormatNumber(0, ViewState("DigitCurr"))
    '                '    tbPPnValue.Text = FormatNumber(0, ViewState("DigitHome"))
    '                '    tbBeaTanah.Text = FormatNumber(0, ViewState("DigitHome"))
    '                'End If
    '            End If
    '            'AttachScript("setformat();", Page, Me.GetType())
    '            'tbTotalSelisih.Text = FormatNumber(CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPh.Text) - CFloat(tbBeaTanah.Text), ViewState("DigitHome"))
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt2 Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
    '    Try
    '        Dim dr() As DataRow
    '        Dim GVR As GridViewRow
    '        GVR = GridDt2.Rows(e.RowIndex)
    '        dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
    '        dr(0).Delete()
    '        BindGridDt(ViewState("Dt2"), GridDt2)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = GridDt2.Rows(e.NewEditIndex)
    '        FillTextBoxDt2(GVR.Cells(1).Text)
    '        MovePanel(pnlDt2, pnlEditDt2)
    '        EnableHd(False)
    '        'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
    '        'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
    '        ViewState("StateDt2") = "Edit"
    '        StatusButtonSave(False)
    '        'btnDocNo.Visible = tbFgModeDt2.Text = "D"
    '        'tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
    '        btnSaveDt2.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
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

    Protected Sub btnRecvDoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRecvDoc.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark FROM PRCIPRecvDocHd WHERE Status<>'D' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
            ResultField = "TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark"
            CriteriaField = "TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnRecvDoc"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "BtnRecvDoc Click Error : " + ex.ToString
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

    Protected Sub btnSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier.Click
        Dim ResultField, CriteriaField As String
        Try
            'Session("filter") = "EXEC S_CNTRABReff " + QuotedStr(tbCode.Text) 'WHERE Fg_Active = 'Y'
            Session("filter") = "SELECT SuppCode,SuppName FROM MsSupplier WHERE FgActive = 'Y' AND SuppType <> 'GEN'  "
            ResultField = "SuppCode,SuppName"
            CriteriaField = "SuppCode,SuppName"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")

            ViewState("Sender") = "btnSupplier"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSupplier_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GVSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSupplier.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupSupplier();"
        End If
    End Sub

    Protected Sub btnSearchSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchSupplier.Click
        BindGridSupplier()
        FilterBindDataGridSupplier()
    End Sub

    Protected Sub lbRecvFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRecvFile.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("RecvFile").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("RecvFile").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbRecvFile_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbApplFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbApplFile.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("ApplFile").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("ApplFile").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbRecvFile_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If

            ChangeCurrency(ddlCurr, tbDate, tbForexRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbForexRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbForexRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddlCurr_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTerm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTerm.TextChanged
        'Try
        '    If Not IsNumeric(tbTerm.Text) Then
        '        tbTerm.Text = "0"
        '    End If
        '    tbDueDate.SelectedDate = tbDate.SelectedDate.AddDays(CInt(tbTerm.Text))
        '    'ChangeCurrency(ddlCurr, tbDate, tbForexRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        'Catch ex As Exception
        '    lbStatus.Text = "tb Term text Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        'tbTerm.Text = GetXRange(ddlTerm.SelectedValue)
        'tbDueDate.SelectedDate = tbDate.SelectedDate.AddDays(CInt(tbTerm.Text))
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub ddlTerm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.TextChanged
        'Dim strSQL As String
        'strSQL = "SELECT xrange FROM MsTerm WHERE TermCode = " + QuotedStr(ddlTerm.SelectedValue)
        'tbTerm.Text = SQLExecuteScalar(strSQL, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub ddlPPh_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPPh.SelectedIndexChanged
        Dim Type As String
        Try
            Type = SQLExecuteScalar("SELECT Type FROM MsPPH WHERE PphCode = '" + ddlPPh.SelectedValue + "'", ViewState("DBConnection"))
            tbType.Text = Type

            If ddlPPh.SelectedValue = "" Then
                tbPPh.Enabled = False
                tbPPhValue.Enabled = False
                tbPPh.Text = 0
                tbPPhValue.Text = 0
                tbTotalAmount.Text = FormatFloat(CFloat(tbDPP.Text) + CFloat(tbPPnValue.Text), ViewState("DigitCurr"))
            Else
                tbPPh.Enabled = True
                tbPPhValue.Enabled = True
                If Type = "-" Then
                    tbTotalAmount.Text = FormatFloat(CFloat(tbDPP.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPhValue.Text), ViewState("DigitCurr"))
                Else
                    tbTotalAmount.Text = FormatFloat(CFloat(tbDPP.Text) + CFloat(tbPPnValue.Text) + CFloat(tbPPhValue.Text), ViewState("DigitCurr"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlPPh_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupplier.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbSupplier_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ExecuteImportDB()
        Dim GVR As GridViewRow
        Dim strSQL As String
        Try
            '    For Each GVR In GridDt.Rows            
            '    Next
            '    '    lstatus.Text = "Records inserted successfully"
            'Catch ex As Exception
            '    'lstatus.Text = "Import to database Error : " + ex.ToString
            '    Response.Write("Import to database Error")
            'End Try
        'Try
            'For Each GVR In GridDt.Rows
            '    strSQL = "INSERT INTO PRCIPLicenAdmInvDt(TransNmbr,ItemNo,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark,FgActive,UserID,UserDate) " + _
            '     " SELECT " + ViewState("TransNmbr") + QuotedStr(GVR.Cells(0).Text) + "," + QuotedStr(GVR.Cells(1).Text) + "," + QuotedStr(GVR.Cells(2).Text) + "," + _
            '    QuotedStr(GVR.Cells(3).Text) + "," + QuotedStr(GVR.Cells(4).Text) + "," + QuotedStr(GVR.Cells(5).Text) + "," + QuotedStr(GVR.Cells(6).Text) + ", " + _
            '    QuotedStr(GVR.Cells(7).Text) + "," + QuotedStr("Y") + ", " + QuotedStr(ViewState("UserId").ToString) + ",Getdate() "
            '    SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
            'Next
        '-----------------------------------------------------------------------------------------------'
        'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next
            'save dt
            For Each GVR In GridDt.Rows
                Dim ConnString As String = ViewState("DBConnection").ToString
                con = New SqlConnection(ConnString)
                con.Open()                                            'JobCode,  
                Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark,FgActive FROM PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
                da = New SqlDataAdapter(cmdSql)
                Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                '-----------------------------------------------------------------------------------------------'
                'strSQL = "INSERT INTO PRCIPLicenAdmInvDt(TransNmbr,ItemNo,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark,FgActive) " + _
                '   " SELECT " + ViewState("TransNmbr") + QuotedStr(GVR.Cells(0).Text) + "," + QuotedStr(GVR.Cells(1).Text) + "," + QuotedStr(GVR.Cells(2).Text) + "," + _
                '    QuotedStr(GVR.Cells(3).Text) + "," + QuotedStr(GVR.Cells(4).Text) + "," + QuotedStr(GVR.Cells(5).Text) + "," + QuotedStr(GVR.Cells(6).Text) + ", " + _
                '    QuotedStr(GVR.Cells(7).Text) + "," + QuotedStr("Y")
                'SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
                strSQL = "S_PRCIPLicenAdmInvDt"
                Dim conStr As String = ViewState("DBConnection").ToString
                Using con As New SqlConnection(conStr)
                    con.Open()
                    Using cmd As New SqlCommand(strSQL, con)
                        cmd.Connection = con
                        cmd.CommandType = CommandType.StoredProcedure 'AddWithValue
                        cmd.Parameters.Add("@TransNmbr", SqlDbType.VarChar, 30).Value = (GVR.Cells(0).Text)
                        cmd.Parameters.Add("@ItemNo", SqlDbType.Int, 6).Value = (GVR.Cells(1).Text)
                        cmd.Parameters.Add("@JobName", SqlDbType.VarChar, 100).Value = (GVR.Cells(2).Text)
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal, 18).Value = Convert.ToInt32(GVR.Cells(3).Text)
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar, 6).Value = (GVR.Cells(4).Text)
                        cmd.Parameters.Add("@PriceForex", SqlDbType.Decimal, 18).Value = Convert.ToInt32(GVR.Cells(5).Text)
                        cmd.Parameters.Add("@LocationName", SqlDbType.VarChar, 18).Value = (GVR.Cells(6).Text)
                        cmd.Parameters.Add("@BeaForex", SqlDbType.Decimal, 18).Value = Convert.ToInt32(GVR.Cells(7).Text) '(ViewState("UserId").ToString)
                        cmd.Parameters.Add("@Remark", SqlDbType.VarChar, 255).Value = (GVR.Cells(8).Text)  'Now
                        cmd.Parameters.Add("@FgActive", SqlDbType.VarChar, 1).Value = "Y"
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                '-----------------------------------------------------------------------------------------------'
                Dim param As SqlParameter
                'Dim Update_Command = New SqlCommand( _
                '    "UPDATE PRCIPLicenAdmInvDt SET JobName=@JobName,Qty=@Qty,UnitCode=@UnitCode,PriceForex=@PriceForex,LocationName=@LocationName, " + _
                '    "BeaForex=@BeaForex,Remark=@Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
                '' Define output parameters.
                'Update_Command.Parameters.Add("@JobName", SqlDbType.VarChar, 100).Value = (GVR.Cells(1).Text)  ', "JobName")
                'Update_Command.Parameters.Add("@Qty", SqlDbType.Decimal, 18).Value = (GVR.Cells(2).Text)  ', "Qty")
                'Update_Command.Parameters.Add("@UnitCode", SqlDbType.VarChar, 6).Value = (GVR.Cells(3).Text)  ', "UnitCode")
                'Update_Command.Parameters.Add("@PriceForex", SqlDbType.Decimal, 18).Value = (GVR.Cells(4).Text)  ', "PriceForex")
                'Update_Command.Parameters.Add("@LocationName", SqlDbType.VarChar, 50).Value = (GVR.Cells(5).Text)  ', "LocationName")
                'Update_Command.Parameters.Add("@BeaForex", SqlDbType.Decimal, 18).Value = (GVR.Cells(6).Text)  ', "BeaForex")
                'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255).Value = (GVR.Cells(7).Text)  ', "Remark")
                '' Define intput (WHERE) parameters.
                'param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10).Value = (GVR.Cells(0).Text)  ', "ItemNo")
                'param.SourceVersion = DataRowVersion.Original
                '' Attach the update command to the DataAdapter.
                'da.UpdateCommand = Update_Command

                ' Create the DeleteCommand.
                Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCIPLicenAdmInvDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
                ' Add the parameters for the DeleteCommand.
                param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10).Value = (GVR.Cells(0).Text)  ', "ItemNo")
                param.SourceVersion = DataRowVersion.Original
                da.DeleteCommand = Delete_Command
            Next

            Dim Dt As New DataTable("PRCIPLicenAdmInvDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            'lstatus.Text = "Import to database Error : " + vbCrLf + ex.ToString
            Response.Write("Import to database Error")
            'Response.Write("<script language='javascript'> { alert "Import to database Error" }</script>")
        End Try
    End Sub

    Protected Sub Excel_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        'Dim Result As New DataTable
        Dim dt As New DataTable()
        Dim dr As DataRow
        Dim conStr As String = ""
        Try
            dt.Columns.Add("ItemNo", GetType(Integer))
            'dt.Columns.Add("JobName", GetType(String))
            dt.Columns.Add("Qty", GetType(String))
            'dt.Columns.Add("UnitCode", GetType(String))
            ''dt.Columns.Add("PriceForex", GetType(Decimal))
            dt.Columns.Add("PriceForex", GetType(String))
            'dt.Columns.Add("LocationName", GetType(String))
            ''dt.Columns.Add("BeaForex", GetType(Decimal))
            dt.Columns.Add("BeaForex", GetType(String))
            'dt.Columns.Add("Remark", GetType(String))

            Select Case Extension
                Case ".xls"
                    'Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                    Exit Select
                Case ".xlsx"
                    'Excel 07
                    conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            conStr = String.Format(conStr, FilePath, isHDR)
            Dim connExcel As New OleDbConnection(conStr)
            Dim cmdExcel As New OleDbCommand()
            Dim oda As New OleDbDataAdapter()

            cmdExcel.Connection = connExcel
            'Get the name of First Sheet
            connExcel.Open()
            Dim dtExcelSchema As DataTable
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
            Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
            connExcel.Close()
            'Read Data from First Sheet
            connExcel.Open()
            cmdExcel.CommandText = "SELECT ItemNo,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark FROM [" & SheetName & "]"
            oda.SelectCommand = cmdExcel
            oda.Fill(dt)
            Dim sdr As OleDbDataReader = cmdExcel.ExecuteReader()
            'If sdr.HasRows Then
            While sdr.Read()
                If CekExistData(ViewState("Dt"), "ItemNo", CInt(sdr.GetValue(0).ToString())) Then
                    Dim Row As DataRow
                    Row = ViewState("Dt").Select("ItemNo = " + sdr.GetValue(0).ToString())(0)   'lbItemNo.Text
                    Row.BeginEdit()
                    Row("JobName") = sdr.GetValue(1).ToString()
                    Row("Qty") = sdr.GetValue(2).ToString()
                    Row("UnitCode") = sdr.GetValue(3).ToString()
                    Row("PriceForex") = sdr.GetValue(4).ToString()
                    Row("LocationName") = sdr.GetValue(5).ToString()
                    Row("BeaForex") = sdr.GetValue(6).ToString()
                    Row("Remark") = sdr.GetValue(7).ToString()
                    'Row("FgActive") = "Y"
                    Row.EndEdit()
                    'CountTotalAmount()
                Else
                    'Dim dr As DataRow
                    dr = ViewState("Dt").NewRow
                    dr("ItemNo") = CInt(sdr.GetValue(0).ToString()) 'GetNewItemNo(ViewState("Dt"))
                    dr("JobName") = sdr.GetValue(1).ToString()
                    dr("Qty") = sdr.GetValue(2).ToString()
                    dr("UnitCode") = sdr.GetValue(3).ToString()
                    dr("PriceForex") = sdr.GetValue(4).ToString()
                    dr("LocationName") = sdr.GetValue(5).ToString()
                    dr("BeaForex") = sdr.GetValue(6).ToString()
                    dr("Remark") = sdr.GetValue(7).ToString()
                    'dr("FgActive") = "Y"
                    'ViewState("Dt").Rows.Add(dr)
                    'CountTotalAmount()
                End If
                'ViewState("Dt").Rows.Add(dr)
            End While
            ViewState("Dt").Rows.Add(dr)
            'End If
            GridDt.DataSource = dt
            GridDt.DataBind()
            connExcel.Close()
            GridDt.Columns(0).Visible = True
        Catch ex As Exception
            lbStatus.Text = "btnApply_Click Error " + ex.ToString
        End Try
        'BindGridDt(ViewState("Dt"), GridDt)
    End Sub

    Private Sub Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 07
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)

        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()

        cmdExcel.Connection = connExcel
        'Get the name of First Sheet
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * FROM [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        GridDt.DataSource = dt
        GridDt.DataBind()
        connExcel.Close()
        '-----------------------------------------------------------------------------------------------------'
        ''ModifyInput2(True, pnlInput, PnlDt, GridDt)
        GridDt.Columns(0).Visible = True
        CountTotalAmount()
        '-----------------------------------------------------------------------------------------------------'
        'Bind Data to GridView
        'GridDt.Caption = Path.GetFileName(FilePath)
        'GridDt.DataSource = dt
        'GridDt.DataBind()
    End Sub

    'Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
    '    Try
    '        If fuLocation.HasFile Then
    '            Dim FileName As String = Path.GetFileName(fuLocation.PostedFile.FileName)
    '            Dim Extension As String = Path.GetExtension(fuLocation.PostedFile.FileName)
    '            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")
    '            Dim FilePath As String = Server.MapPath("~/ImportExcel/" + FileName)
    '            'fuLocation.SaveAs(FilePath)
    '            'ViewState("FilePath") = FilePath
    '            'ViewState("Extension") = Extension
    '            Excel_To_Grid(FilePath, Extension, "Yes")
    '            'Import_To_Grid(FilePath, Extension, "Yes")
    '            'CountTotalAmount()
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btnUpload_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnDeleteDoc1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc1.Click
        Dim strSQL As String
        strSQL = "UPDATE PRCIPLicenAdmInvHd SET RecvFile=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbRecvFile.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnDeleteDoc2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc2.Click
        Dim strSQL As String
        strSQL = "UPDATE PRCIPLicenAdmInvHd SET ApplFile=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbApplFile.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub
End Class
