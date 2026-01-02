Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports BasicFrame.WebControls

Partial Class TrMTNServiceRequest
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_MTNServiceRequestHd"
    Public strTransNmbr As String
    Private Shared PageSize As Integer = 5
    Dim tb As TextBox

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM FINCIPInstrJobDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    'Private Function GetStringDt2(ByVal Nmbr As String) As String
    '    Return "SELECT * FROM V_PRCIPLicenAdmInvDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                'FillCombo(ddlSatuan, "SELECT Unit_Code, Unit_Name FROM VMsUnit ", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                SetInit()
                'BindDataToGridView("S_PRCFindCIPRecvDoc", GVNoRecvDoc, ViewState("DBConnection").ToString)
                BindGridArea()
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
                    'tbPackJob.Text = Session("Result")(0).ToString
                    'tbApplfileNo.Text = Session("Result")(2).ToString
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
                If ViewState("Sender") = "btnArea" Then
                    tbAreaCode.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnKavling" Then
                    tbKavlingCode.Text = Session("Result")(0).ToString
                    tbKavlingName.Text = Session("Result")(1).ToString
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
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
            'FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
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
            lbTitle.Text = "Service Request" 'Request.QueryString("MenuName").ToString
            'tbPPn.Attributes.Add("ReadOnly", "True")
            'tbDisc.Attributes.Add("ReadOnly", "True")
            'tbPriceForex.Attributes.Add("OnBlur", "setformathd();")
            'tbQty.Attributes.Add("OnBlur", "setformathd();")
            'tbBiayaSatuan.Attributes.Add("OnBlur", "setformathd();")
            'Me.tbPriceForex.Attributes.Add("ReadOnly", "True")
            'Me.tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Me.tbBiayaSatuan.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            'tbBaseForex.Text = FormatNumber(PriceForex, ViewState("DigitHome"))
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
            If (ActionValue = "Print") Or (ActionValue = "Print Full") Then
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
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_MTFormServiceRequest" + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormServiceRequest.frx"
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
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
                        Result = ExecSPCommandGo(ActionValue, "S_MTServiceRequest", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnArea.Visible = State
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
            'BindGridDt(dt, GridDt)
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

    'Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
    '    Try
    '        MovePanel(pnlEditDt, PnlDt)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
    '    Try
    '        'MovePanel(pnlEditDt2, pnlDt2)
    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '        'StatusButtonSave(True)
    '    Catch ex As Exception
    '        'lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbAreaCode.Text = ""
            tbAreaName.Text = ""
            tbKavlingCode.Text = ""
            tbKavlingName.Text = ""
            tbRequestBy.Text = ""
            tbContactNo.Text = ""
            tbEmail.Text = ""
            'tbBaseForex.Text = "0"
            'tbDurasi.Text = "0"
            'ddlReport.SelectedValue = "Y"
            'ddlUserType.SelectedValue = "Supplier"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbTimeJobDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbStartJobDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            'tbFinishJobDate.SelectedDate = ViewState("ServerDate")
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
            'tbJobName.Text = ""
            'tbQty.Text = "0"
            'ddlSatuan.SelectedValue = ""
            'tbBiayaSatuan.Text = "0"
            'tbSubledNameDt.Text = ""
            'tbSubledDt.Enabled = False
            'btnSubled.Visible = False
            'ddlUnit.SelectedIndex = 0
            'tbPriceForex.Text = "0"
            'tbLocationName.Text = ""
            'ddlCurrDt.SelectedValue = ViewState("Currency")
            'tbRateDt.Text = FormatFloat(1, ViewState("DigitRate"))
            'tbRateDt.Enabled = False
            'ddlCurrDt.Enabled = False
            'tbBiaya.Text = "0"
            'tbAmountHomeDt.Text = "0"
            'tbRemarkDt.Text = ""
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
            If tbAreaCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Area Code must have value")
                tbAreaCode.Focus()
                Return False
            End If
            If tbAreaName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Area Name must have value")
                tbAreaName.Focus()
                Return False
            End If
            If tbKavlingCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Kavling Code must have value")
                tbKavlingCode.Focus()
                Return False
            End If
            If tbKavlingName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Kavling Name must have value")
                tbKavlingName.Focus()
                Return False
            End If
            If tbContactNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Contact No must have value")
                tbContactNo.Focus()
                Return False
            End If
            If tbEmail.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Email must have value")
                tbEmail.Focus()
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
                'If tbJobName.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Uraian pekerjaan Must Have Value")
                '    tbJobName.Focus()
                '    Return False
                'End If
                'If CFloat(tbQty.Text) <= "0" Then
                '    lbStatus.Text = MessageDlg("Area Must be greater than nol")
                '    tbQty.Focus()
                '    Return False
                'End If

                'If tbQty.Text = "" Then
                '    lbStatus.Text = MessageDlg("Luas Must have value !")
                '    tbQty.Focus()
                '    Return False
                'End If
                'If ddlUnit.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Satuan Must Have Value")
                '    ddlUnit.Focus()
                '    Return False
                'End If
                'If CFloat(tbPriceForex.Text) <= "0" Then
                '    lbStatus.Text = MessageDlg("Estimasi/HPS Must be greater than nol")
                '    tbPriceForex.Focus()
                '    Return False
                'End If
                'If tbPriceForex.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Estimasi/HPS Must Have Value")
                '    tbPriceForex.Focus()
                '    Return False
                'End If


                'If tbRemark.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Remark Must Have Value")
                '    tbRemark.Focus()
                '    Return False
                'End If
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
            BindToText(tbAreaCode, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("AreaName").ToString)
            BindToText(tbKavlingCode, Dt.Rows(0)("KavlingCode").ToString)
            BindToText(tbKavlingName, Dt.Rows(0)("KavlingName").ToString)
            BindToText(tbRequestBy, Dt.Rows(0)("RequestBy").ToString)
            BindToText(tbContactNo, Dt.Rows(0)("ContactNo").ToString)
            BindToText(tbEmail, Dt.Rows(0)("Email").ToString)
            'BindToDate(tbTimeJobDate, Dt.Rows(0)("TimeJobDate").ToString)
            'BindToDate(tbStartJobDate, Dt.Rows(0)("StartJobDate").ToString)
            'BindToDate(tbFinishJobDate, Dt.Rows(0)("FinishJobDate").ToString)
            'BindToText(tbDurasi, Dt.Rows(0)("Durasi").ToString)
            'ViewState("DigitCurr") = SQLExecuteScalar("SELECT Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            'If ViewState("DigitCurr") = Nothing Then
            '    ViewState("DigitCurr") = 0
            'End If
            'BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitHome"))
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
                'lbItemNo.Text = ItemNo.ToString
                ' FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
                'BindToDropList(ddlUserType, Dr(0)("CIPAI").ToString)
                'BindToDropList(ddlStructureCode, Dr(0)("StructureCode").ToString)
                'BindToText(tbAccountDt, Dr(0)("Account").ToString)
                'BindToDropList(ddlSatuan, Dr(0)("UnitCode").ToString)
                'BindToText(tbJobName, Dr(0)("JobName").ToString)
                'BindToText(tbQty, Dr(0)("Area").ToString, ViewState("DigitHome"))
                'BindToText(tbPriceForex, Dr(0)("PriceForex").ToString, ViewState("DigitHome"))
                'BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'BindToText(tbBiayaSatuan, Dr(0)("BiayaSatuan").ToString, ViewState("DigitHome"))
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
                'lbItemNoDt2.Text = ItemNo
                'BindToDropList(ddlPayTypeDt2, Dr(0)("PaymentType").ToString)
                'tbPaymentDateDt2.SelectedDate = tbDate.SelectedDate
                'tbDueDateDt2.SelectedDate = Dr(0)("PaymentDate").ToString
                'BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
                'BindToText(tbNominal, Dr(0)("Nominal").ToString)
                'BindToDate(tbGiroDateDt2, Dr(0)("GiroDate").ToString)
                'BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
                'BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                'BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                'BindToText(tbPaymentForexDt2, Dr(0)("PaymentForex").ToString)
                'BindToText(tbPaymentHomeDt2, Dr(0)("PaymentHome").ToString)
                'BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
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

    'Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
    '    Try
    '        If CekDt() = False Then
    '            'btnSaveDt.Focus()
    '            Exit Sub
    '        End If

    '        'If ViewState("StateDt") = "Edit" Then
    '        '    Dim Row As DataRow
    '        '    Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
    '        '    Row.BeginEdit()
    '        '    Row("JobName") = tbJobName.Text
    '        '    Row("Area") = Val(tbQty.Text)
    '        '    Row("UnitCode") = ddlSatuan.SelectedValue
    '        '    Row("BiayaSatuan") = tbBiayaSatuan.Text
    '        '    Row("PriceForex") = tbPriceForex.Text
    '        '    Row("Remark") = tbRemarkDt.Text
    '        '    Row("FgActive") = "Y"
    '        '    Row.EndEdit()
    '        'Else
    '        '    'Insert
    '        '    Dim dr As DataRow
    '        '    dr = ViewState("Dt").NewRow
    '        '    dr("ItemNo") = CInt(lbItemNo.Text)
    '        '    dr("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
    '        '    dr("Area") = tbQty.Text
    '        '    dr("UnitCode") = ddlSatuan.SelectedValue
    '        '    dr("PriceForex") = tbPriceForex.Text
    '        '    dr("BiayaSatuan") = tbBiayaSatuan.Text
    '        '    dr("Remark") = tbRemarkDt.Text
    '        '    dr("FgActive") = "Y"
    '        '    ViewState("Dt").Rows.Add(dr)
    '        'End If
    '        'MovePanel(pnlEditDt, PnlDt)
    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
    '        'BindGridDt(ViewState("Dt"), GridDt)
    '        CountTotalAmount()
    '        StatusButtonSave(True)
    '        ' GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
    '    Catch ex As Exception
    '        lbStatus.Text = "btn save Dt Error : " + ex.ToString
    '    Finally
    '        If Not con Is Nothing Then con.Dispose()
    '        If Not da Is Nothing Then da.Dispose()
    '    End Try
    'End Sub

    'Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
    '    Try
    '        'If ViewState("1Payment").ToString = "Y" Then
    '        '    If GetCountRecord(ViewState("Dt2")) >= 1 Then
    '        '        'If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
    '        '        If ViewState("StateDt2") <> "Edit" Then
    '        '            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
    '        '                lbStatus.Text = "Cannot input more than one payment type"
    '        '                Exit Sub
    '        '            End If
    '        '        End If
    '        '        'End If
    '        '    End If
    '        'End If
    '        If CekDt2() = False Then
    '            'btnSaveDt2.Focus()
    '            Exit Sub
    '        End If
    '        'If tbFgModeDt2.Text = "G" Then
    '        '    If CekExistGiroOut(tbDocumentNoDt2.Text.Trim, ViewState("DBConnection").ToString) = True Then
    '        '        lbStatus.Text = "Giro Payment '" + tbDocumentNoDt2.Text.Trim + "' has already exists in Giro Listing'"
    '        '        Exit Sub
    '        '    End If
    '        'End If
    '        If ViewState("StateDt2") = "Edit" Then
    '            Dim Row As DataRow
    '            'Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
    '            Row.BeginEdit()
    '            'Row("PaymentType") = ddlPayTypeDt2.SelectedValue
    '            'Row("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
    '            'Row("PaymentDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'Row("DocumentNo") = tbDocumentNoDt2.Text
    '            'Row("Nominal") = tbNominal.Text
    '            'If tbGiroDateDt2.Enabled Then
    '            '    Row("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'Else
    '            '    Row("GiroDate") = DBNull.Value
    '            'End If

    '            'If tbDueDateDt2.Enabled Then
    '            '    Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'Else
    '            '    Row("DueDate") = DBNull.Value
    '            'End If
    '            'Row("BankPayment") = ddlBankPaymentDt2.SelectedValue
    '            'Row("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
    '            'Row("FgMode") = tbFgModeDt2.Text
    '            'Row("Currency") = ddlCurrDt2.SelectedValue
    '            'Row("ForexRate") = tbRateDt2.Text
    '            'Row("PaymentForex") = tbPaymentForexDt2.Text
    '            'Row("PaymentHome") = tbPaymentHomeDt2.Text
    '            'Row("Remark") = tbRemarkDt2.Text
    '            'Row("ChargeCurrency") = ddlChargeCurrDt2.SelectedValue
    '            'Row("ChargeRate") = tbChargeRateDt2.Text
    '            'Row("ChargeForex") = tbChargeForexDt2.Text
    '            'Row("ChargeHome") = tbChargeHomeDt2.Text
    '            'Row("Remark") = tbRemarkDt2.Text
    '            Row("FgActive") = "Y"
    '            Row.EndEdit()
    '        Else
    '            'Insert
    '            Dim dr As DataRow

    '            dr = ViewState("Dt2").NewRow
    '            'dr("ItemNo") = CInt(lbItemNoDt2.Text)
    '            'dr("PaymentType") = ddlPayTypeDt2.SelectedValue
    '            'dr("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
    '            'dr("PaymentDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'dr("DocumentNo") = tbDocumentNoDt2.Text
    '            'dr("Nominal") = tbNominal.Text
    '            'dr("Reference") = tbVoucherNo.Text
    '            'If tbGiroDateDt2.Enabled Then
    '            '    dr("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'Else
    '            '    dr("GiroDate") = DBNull.Value
    '            'End If
    '            'If tbDueDateDt2.Enabled Then
    '            '    dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
    '            'Else
    '            '    dr("DueDate") = DBNull.Value
    '            'End If
    '            'dr("BankPayment") = ddlBankPaymentDt2.SelectedValue
    '            'dr("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
    '            'dr("FgMode") = tbFgModeDt2.Text
    '            'dr("Currency") = ddlCurrDt2.SelectedValue
    '            'dr("ForexRate") = tbRateDt2.Text
    '            'dr("PaymentForex") = tbPaymentForexDt2.Text
    '            'dr("PaymentHome") = tbPaymentHomeDt2.Text
    '            'dr("Remark") = tbRemarkDt2.Text
    '            dr("FgActive") = "Y"
    '            'dr("ChargeCurrency") = ddlChargeCurrDt2.Text
    '            'dr("ChargeRate") = tbChargeRateDt2.Text
    '            'dr("ChargeForex") = tbChargeForexDt2.Text
    '            'dr("ChargeHome") = tbChargeHomeDt2.Text
    '            'dr("Remark") = tbRemarkDt2.Text
    '            ViewState("Dt2").Rows.Add(dr)
    '        End If
    '        'If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
    '        '    ViewState("PayType") = ddlPayTypeDt2.SelectedValue
    '        'End If
    '        'MovePanel(pnlEditDt2, pnlDt2)
    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
    '        'BindGridDt(ViewState("Dt2"), GridDt2)

    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn save dt2 Error : " + ex.ToString
    '    Finally
    '        If Not con Is Nothing Then con.Dispose()
    '        If Not da Is Nothing Then da.Dispose()
    '    End Try
    'End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
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
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("SVR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO MTNServiceRequestHd(TransNmbr,TransDate,Status,AreaCode,KavlingCode,RequestBy,ContactNo,Email,Remark,FgReport,FgActive,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbAreaCode.Text) + ", " + _
                QuotedStr(tbKavlingCode.Text) + ", " + QuotedStr(tbRequestBy.Text) + ", " + QuotedStr(tbContactNo.Text) + ", " + _
                QuotedStr(tbEmail.Text) + ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr("Y") + ", " + QuotedStr("Y") + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text     'QuotedStr(Format(tbTimeJobDate.SelectedValue, "yyyy-MM-dd"))
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM MTNServiceRequestHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNServiceRequestHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", AreaCode =" + QuotedStr(tbAreaCode.Text) + _
                ", KavlingCode =" + QuotedStr(tbKavlingCode.Text) + ", RequestBy =" + QuotedStr(tbRequestBy.Text) + _
                ", ContactNo =" + QuotedStr(tbContactNo.Text) + ", Email =" + QuotedStr(tbEmail.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", UserPrep= " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)     'QuotedStr(Format(tbTimeJobDate.SelectedValue, "yyyy-MM-dd"))
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
            'Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,JobName,Area,UnitCode,BiayaSatuan,PriceForex,Remark,FgActive FROM FINCIPInstrJobDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand. 'Account=@Account, JobCode=@JobCode,
            'Dim Update_Command = New SqlCommand( _
            '  "UPDATE FINCIPInstrJobDt SET JobName=@JobName,Area=@Area,UnitCode=@UnitCode,BiayaSatuan=@BiayaSatuan,PriceForex=@PriceForex,Remark=@Remark  " + _
            '  "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            '' Define output parameters.
            ''Update_Command.Parameters.Add("@JobCode", SqlDbType.VarChar, 12, "JobCode")
            'Update_Command.Parameters.Add("@JobName", SqlDbType.VarChar, 100, "JobName")
            ''Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 20, "Account")
            'Update_Command.Parameters.Add("@Area", SqlDbType.Decimal, 18, "Area")
            ''Update_Command.Parameters.Add("@PPnDate", SqlDbType.DateTime, 8, "PPnDate")
            'Update_Command.Parameters.Add("@BiayaSatuan", SqlDbType.VarChar, 6, "BiayaSatuan")
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
            '    "DELETE FROM FINCIPInstrJobDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            'Dim Dt As New DataTable("FINCIPInstrJobDt")

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
            'ModifyInput2(True, pnlInput, PnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            btnHome.Visible = False
            'MultiView1.ActiveViewIndex = 0
            'Menu1.Items.Item(0).Selected = True
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
    '        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
    '        'tbAccountDt.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
    '    Try
    '        Cleardt2()
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        ViewState("StateDt2") = "Insert"
    '        MovePanel(pnlDt2, pnlEditDt2)
    '        EnableHd(False)
    '        StatusButtonSave(False)
    '        lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
    '        'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
    '        '    BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
    '        'End If
    '        'btnDocNo.Visible = False
    '        tbDocumentNoDt2.Enabled = True
    '        'ddlPayTypeDt2.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub

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
            'PnlDt.Visible = True
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
                    'GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    'BindDataDt2(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput(False, pnlInput)
                    'ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    'ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    'MultiView1.ActiveViewIndex = 0
                    'Menu1.Items.Item(0).Selected = True
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
                        'GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        'BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput(True, pnlInput)
                        'ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        'MultiView1.ActiveViewIndex = 0
                        'Menu1.Items.Item(0).Selected = True
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
                        'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
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
                        Session("SelectCommand") = "EXEC S_MTFormServiceRequest '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormServiceRequest.frx"
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
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '        '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        '        ' add the UnitPrice and QuantityTotal to the running total variables
    '        '        TotalExpense += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
    '        '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '        '        tbPPh.Text = FormatNumber(TotalExpense, ViewState("DigitHome"))
    '        '    End If
    '        'End If
    '        'tbTotalSelisih.Text = FormatNumber(CFloat(tbDisc.Text) + CFloat(tbPPnValue.Text) - CFloat(tbPPh.Text) - CFloat(tbBeaTanah.Text), ViewState("DigitHome"))
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
    '    Try
    '        Dim dr() As DataRow
    '        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
    '        'dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
    '        'dr(0).Delete()
    '        'BindGridDt(ViewState("Dt"), GridDt)
    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '        dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
    '        dr(0).Delete()
    '        BindGridDt(ViewState("Dt"), GridDt)
    '        CountTotalAmount()
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
    '    End Try
    'End Sub

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

    'Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = GridDt.Rows(e.NewEditIndex)
    '        'row = ViewState("Dt").Rows(e.NewEditIndex)
    '        FillTextBoxDt(GVR.Cells(1).Text)
    '        MovePanel(PnlDt, pnlEditDt)
    '        EnableHd(False)
    '        ViewState("StateDt") = "Edit"
    '        btnSaveDt.Focus()
    '        StatusButtonSave(False)
    '        CountTotalAmount()
    '        'tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"
    '        'btnSubled.Enabled = tbSubledDt.Enabled
    '        'ddlCostCenterDt.Enabled = GVR.Cells(6).Text <> "N"
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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

    'Protected Sub btnRecvDoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRecvDoc.Click
    'Dim ResultField, CriteriaField As String
    'Try
    '    Session("filter") = "SELECT TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark FROM PRCIPRecvDocHd WHERE Status<>'D' " 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
    '    'Session("filter") = "EXEC S_PRCFindCIPRecvDoc"
    '    ResultField = "TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark"
    '    CriteriaField = "TransNmbr,TransDate,ApplfileNo,ApplfileDate,HGBNo,PICName,BrokerName,RelatedOffcName,Remark"
    '    ViewState("CriteriaField") = CriteriaField.Split(",")
    '    Session("Column") = ResultField.Split(",")
    '    ViewState("Sender") = "btnRecvDoc"
    '    Session("DBConnection") = ViewState("DBConnection")
    '    'AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    AttachScript("OpenPopup();", Page, Me.GetType())
    'Catch ex As Exception
    '    lbStatus.Text = "BtnRecvDoc Click Error : " + ex.ToString
    'End Try
    'End Sub

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

    Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArea.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ID, StructureName FROM V_MsStructure WHERE LevelCode = '01' "
            ResultField = "ID, StructureName"
            ViewState("Sender") = "btnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnArea_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnKavling_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKavling.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT StructureCode, StructureName FROM V_MsStructure WHERE ParentID = " + QuotedStr(tbAreaCode.Text)
            ResultField = "StructureCode, StructureName"
            ViewState("Sender") = "btnKavling"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnKavling_Click Error : " + ex.ToString
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

    'Protected Sub tbFinishJobDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFinishJobDate.SelectionChanged, tbStartJobDate.SelectionChanged
    '    'tbDurasi.Text = FormatFloat(tbFinishJobDate.SelectedValue, 0) - FormatFloat(tbStartJobDate.SelectedValue, 0)
    '    Dim Sqlstring As String
    '    Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
    '    tbDurasi.Text = Sqlstring

    '    If tbDurasi.Text < 0 Then
    '        lbStatus.Text = MessageDlg("Start Date cannot grether more than finish date !!!")
    '        tbStartJobDate.SelectedDate = tbFinishJobDate.SelectedDate
    '        Sqlstring = SQLExecuteScalar("EXEC S_GetSlisihHari '" + Format(tbStartJobDate.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbFinishJobDate.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
    '        tbDurasi.Text = Sqlstring
    '        tbStartJobDate.Focus()
    '        Exit Sub
    '    End If
    'End Sub

    'Protected Sub lbArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbArea.Click
    '    Try
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsArea')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lbArea_Click Error : " + ex.ToString
    '    End Try
    'End Sub

End Class
