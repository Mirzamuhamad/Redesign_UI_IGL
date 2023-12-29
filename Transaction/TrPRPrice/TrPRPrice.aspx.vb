Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPRPrice_TrPRPrice
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRPriceHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If ViewState("DigitCurr") Is Nothing Then
                ViewState("DigitCurr") = 0
            End If
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

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnsupplier" Then
                    tbSupplier.Text = Session("Result")(0).ToString
                    tbSupplierName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnproduct" Then
                    tbProduct.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbSpecification, Session("Result")(2).ToString)
                    BindToDropList(ddlUnit, Session("Result")(3).ToString)
                    tbOldPrice.Text = GetPriceSupp(tbProduct.Text, ddlUnit.SelectedValue, tbSupplier.Text, ddlCurrency.SelectedValue, tbEffectiveDate.SelectedDate)
                    tbNewPrice.Text = tbOldPrice.Text
                    tbMOQ.Text = "1"
                    lbMOQ.Text = ddlUnit.SelectedValue
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        Dim Row As DataRow()
                        'Dim dre As SqlDataReader
                        'sini
                        Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(drResult("Product_Code")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("ProductCode") = drResult("Product_Code")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("Unit") = drResult("Unit")
                            dr("Remark") = ""
                            dr("Currency") = ViewState("Currency")
                            ViewState("DigitCurr") = ViewState("DigitHome")
                            dr("OldPrice") = FormatNumber(GetPriceSupp(dr("ProductCode"), dr("Unit"), tbSupplier.Text, dr("Currency"), tbEffectiveDate.SelectedDate), ViewState("DigitCurr"))
                            dr("NewPrice") = dr("OldPrice")
                            lbMOQ.Text = ddlUnit.SelectedValue
                            dr("MOQ") = "1"
                            dr("LeadTime") = "0"
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        FillCombo(ddlCurrency, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))

        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If

        tbOldPrice.Attributes.Add("ReadOnly", "True")
        tbSpecification.Attributes.Add("ReadOnly", "True")
        Me.tbOldPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbNewPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbMOQ.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbLeadTime.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbNewPrice.Attributes.Add("OnBlur", "setformat();")
        Me.tbMOQ.Attributes.Add("OnBlur", "setformat();")
        'Me.tbLeadTime.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
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
        Return "SELECT * From V_PRPriceDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
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
                Session("SelectCommand") = "EXEC S_PRFormPrice " + Result
                Session("ReportFile") = ".../../../Rpt/FormPRPrice.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRPrice", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("Reference in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'btnGetDt.Visible = State
            tbDate.Enabled = State
            'tbEffectiveDate.Enabled = State
            tbSupplier.Enabled = State
            BtnSupplier.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbRef.Text = GetAutoNmbr("PPR", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PRCPriceHd (TransNmbr, Status, TransDate, EffectiveDate, SuppQuoNo, Supplier, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbSuppQouNo.Text + "', '" + tbSupplier.Text + "', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCPriceHD WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCPriceHD SET EffectiveDate = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', Remark = '" + tbRemark.Text + "'," + _
                " SuppQuoNo = '" + tbSuppQouNo.Text + "', Supplier = '" + tbSupplier.Text + "'," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
                " WHERE TransNmbr = '" + tbRef.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbRef.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT * FROM PRCPriceDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PRCPriceDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
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
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "Reference"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbSuppQouNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            BtnSupplier.Visible = True
            tbSupplier.Enabled = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEffectiveDate.SelectedDate = ViewState("ServerDate") 'Today
            tbMOQ.Text = "1"
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbSuppQouNo.Text = ""
            tbSupplier.Text = ""
            tbSupplierName.Text = ""
            tbRemark.Text = ""
            tbEffectiveDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            'lbProductPrice.Text = ""
            'tbCode.Text = ""
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            tbRemarkDt.Text = ""
            tbNewPrice.Text = "0"
            tbOldPrice.Text = "0"
            tbMOQ.Text = "0"
            tbLeadTime.Text = "0"
            ddlCurrency.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = ViewState("DigitHome") 'ViewState("DigitHome")
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))

        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
            EnableHd(True)
            ViewState("StateHd") = "Insert"
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Function GetPriceSupp(ByVal Product As String, ByVal Unit As String, ByVal Supplier As String, ByVal Currency As String, ByVal effectivedate As DateTime) As String
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_FindPriceSupplier " + QuotedStr(Supplier) + "," + QuotedStr(Currency) + ", " + QuotedStr(Product) + ", " + QuotedStr(Unit) + "," + QuotedStr(Format(effectivedate, "yyyy-MM-dd")), ViewState("DBConnection").ToString)
        dr.Read()
        Return FormatNumber(dr("Price"), ViewState("DigitCurr"))
        dr.Close()
    End Function

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurrency, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        tbOldPrice.Text = GetPriceSupp(tbProduct.Text, ddlUnit.SelectedValue, tbSupplier.Text, ddlCurrency.SelectedValue, tbEffectiveDate.SelectedDate)
        tbNewPrice.Text = tbOldPrice.Text
        ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
        tbNewPrice.Focus()
        AttachScript("setformat();", Page, Me.GetType())
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        'lbProductPrice.Text = GetNewItemNo(ViewState("Dt"))
        'lbProductPrice.Text = ViewState("Dt")
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Reference must have value")
            '    tbRef.Focus()
            '    Return False
            'End If
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbEffectiveDate.IsNull Then
                lbStatus.Text = MessageDlg("Effective Date must have value")
                tbEffectiveDate.Focus()
                Return False
            End If
            If tbEffectiveDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Effective Date must greater than Transaction Date")
                tbEffectiveDate.Focus()
                Return False
            End If
            If tbSuppQouNo.Text.Trim = "" Then
                lbStatus.Text = "Quotation No must have value"
                tbSuppQouNo.Focus()
                Return False
            End If
            If tbSupplier.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSupplier.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ProductCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If Dr("MOQ").ToString.Trim = "0" Then
                    lbStatus.Text = MessageDlg("MOQ Must Have Value")
                    Return False
                End If
                'If Dr("LeadTime").ToString.Trim = "0" Then
                '    lbStatus.Text = MessageDlg("Lead Time Must Have Value")
                '    Return False
                'End If
                If CFloat(Dr("NewPrice").ToString.Trim) = "0" Then
                    lbStatus.Text = MessageDlg("New Price Must Have Value")
                    Return False
                End If
            Else
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If ddlCurrency.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrency.Focus()
                    Return False
                End If
                If CFloat(tbMOQ.Text) = "0" Then
                    lbStatus.Text = MessageDlg("MOQ Must Have Value")
                    tbMOQ.Focus()
                    Return False
                End If
                'If CInt(tbLeadTime.Text) = "0" Then
                '    lbStatus.Text = MessageDlg("Lead Time Must Have Value")
                '    tbLeadTime.Focus()
                '    Return False
                'End If
                If CFloat(tbNewPrice.Text) = "0" Then
                    lbStatus.Text = MessageDlg("New Price Must Have Value")
                    tbNewPrice.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Quotation No, Effective Date, Supplier, Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), Quotation_No, dbo.FormatDate(EffectiveDate), Supplier_Name, Remark"
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
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Delete" Then
                    Dim SqlString, Result As String

                    If Not GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Data Must be Hold Before Deleted")
                        Exit Sub
                    End If

                    'lbStatus.Text = ConfirmDlg("Sure to delete this data?")    

                    SqlString = "Declare @A VarChar(255) EXEC S_PRPriceDelete " + QuotedStr(GVR.Cells(2).Text) + ", " + (Session(Request.QueryString("KeyId"))("Year")).ToString + "," + (Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                    'SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
                    BindData(Session("AdvanceFilter"))
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRFormPrice '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormPRPrice.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ProductCode+'|'+Unit+'|'+Currency = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(4).Text + "|" + GVR.Cells(5).Text))
        dr(0).Delete()
        ' ViewState("Dt").AcceptChanges()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text + "|" + GVR.Cells(4).Text + "|" + GVR.Cells(5).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(4).Text + "|" + GVR.Cells(5).Text
            tbProduct.Focus()
            lbMOQ.Text = ddlUnit.SelectedValue
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)

            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbSuppQouNo, Dt.Rows(0)("Quotation_No").ToString)
            BindToText(tbSupplier, Dt.Rows(0)("Supplier_Code").ToString)
            BindToText(tbSupplierName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ProductCode+'|'+Unit+'|'+Currency = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbProduct, Dr(0)("ProductCode").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToDropList(ddlCurrency, Dr(0)("Currency").ToString)
                BindToText(tbOldPrice, Dr(0)("OldPrice").ToString)
                BindToText(tbNewPrice, Dr(0)("NewPrice").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbMOQ, Dr(0)("MOQ").ToString)
                BindToText(tbLeadTime, Dr(0)("LeadTime").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "Select Product_Code, Product_Name, Specification, Unit from VMsSuppProduct Where Supplier = " + QuotedStr(tbSupplier.Text)
            ResultField = "Product_Code, Product_Name, Specification, Unit"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            ViewState("InputProduct") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProduct.Text + "|" + ddlUnit.SelectedValue + "|" + ddlCurrency.SelectedValue Then
                    If CekExistData(ViewState("Dt"), "ProductCode,Unit,Currency", tbProduct.Text + "|" + ddlUnit.SelectedValue + "|" + ddlCurrency.SelectedValue) Then
                        lbStatus.Text = "Product '" + tbProductName.Text + "' Unit '" + ddlUnit.SelectedValue + "' Currency '" + ddlCurrency.SelectedValue + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductCode+'|'+Unit+'|'+Currency = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductCode") = tbProduct.Text
                Row("ProductName") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row("Currency") = ddlCurrency.SelectedValue
                If Row("Currency") = "" Then
                    Row("Currency") = DBNull.Value
                End If
                Row("OldPrice") = tbOldPrice.Text
                Row("NewPrice") = tbNewPrice.Text
                Row("MOQ") = tbMOQ.Text
                Row("LeadTime") = CInt(tbLeadTime.Text)
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ProductCode,Unit,Currency", tbProduct.Text + "|" + ddlUnit.SelectedValue + "|" + ddlCurrency.SelectedValue) Then
                    lbStatus.Text = "Product '" + tbProductName.Text + "' Unit '" + ddlUnit.SelectedValue + "' Currency '" + ddlCurrency.SelectedValue + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductCode") = tbProduct.Text
                dr("ProductName") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                dr("Currency") = ddlCurrency.SelectedValue
                If dr("Currency") = "" Then
                    dr("Currency") = DBNull.Value
                End If
                dr("OldPrice") = tbOldPrice.Text
                dr("NewPrice") = tbNewPrice.Text
                dr("MOQ") = tbMOQ.Text
                dr("LeadTime") = CInt(tbLeadTime.Text)
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("SuppProduct", tbSupplier.Text + "|" + tbProduct.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProduct.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                BindToText(tbSpecification, Dr("Specification"))
                BindToDropList(ddlUnit, Dr("Unit"))
                tbOldPrice.Text = GetPriceSupp(tbProduct.Text, ddlUnit.SelectedValue, tbSupplier.Text, ddlCurrency.SelectedValue, tbEffectiveDate.SelectedDate)
                tbNewPrice.Text = tbOldPrice.Text
                lbMOQ.Text = ddlUnit.SelectedValue
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbOldPrice.Text = "0"
                tbNewPrice.Text = "0"
            End If
            ddlCurrency.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSupplier_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSupplier.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSupplier.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSupplier.Text = Dr("Supplier_Code")
                tbSupplierName.Text = Dr("Supplier_Name")
            Else
                tbSupplier.Text = ""
                tbSupplierName.Text = ""
            End If
            tbRemark.Focus()
        Catch ex As Exception
            Throw New Exception("tb supplier Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSupplier.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsSupplier WHERE FgActive = 'Y'"
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnsupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Product_Code, Product_Name, Specification, Unit from VMsSuppProduct WHERE Supplier = " + QuotedStr(tbSupplier.Text)
            ResultField = "Product_Code, Product_Name, Specification, Unit"
            ViewState("Sender") = "btnproduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnit.SelectedIndexChanged
        If ViewState("InputUnit") = "Y" Then
            RefreshMaster("S_GetUnit", "Unit_Code", "Unit_Name", ddlUnit, ViewState("DBConnection"))
            ViewState("InputUnit") = Nothing
        End If
        tbOldPrice.Text = GetPriceSupp(tbProduct.Text, ddlUnit.SelectedValue, tbSupplier.Text, ddlCurrency.SelectedValue, tbEffectiveDate.SelectedDate)
        tbNewPrice.Text = tbOldPrice.Text
        lbMOQ.Text = ddlUnit.SelectedValue
        AttachScript("setformat();", Page, Me.GetType())
    End Sub

    Protected Sub lbSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupplier.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbSupplier_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ChangeEffective()
        Dim dr, dr2 As DataRow
        Dim Row As DataRow()
        Dim DtPrice As DataTable
        Dim DrPrice As DataRow
        Dim Dt As DataTable
        Dim SQLStringPrice As String        
        If GetCountRecord(ViewState("Dt")) <> 0 Then

            For Each dr In ViewState("Dt").Rows
                SQLStringPrice = "EXEC S_FindPriceSupplier " + QuotedStr(tbSupplier.Text) + ", " + QuotedStr(dr("Currency")) + ", " + QuotedStr(dr("ProductCode")) + ", " + QuotedStr(dr("Unit")) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd"))
                DtPrice = SQLExecuteQuery(SQLStringPrice, ViewState("DBConnection").ToString).Tables(0)

                dr2 = ViewState("Dt").Select("ProductCode+'|'+Unit+'|'+Currency = " + QuotedStr(dr("ProductCode") + "|" + dr("Unit") + "|" + dr("Currency")))(0)
                dr2.BeginEdit()

                If DtPrice.Rows.Count > 0 Then
                    DrPrice = DtPrice.Rows(0)
                    dr2("OldPrice") = FormatNumber(DrPrice("Price").ToString, CInt(ViewState("DigitCurr")))
                    dr2("NewPrice") = dr2("OldPrice")
                End If
                dr2.EndEdit()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
        End If
    End Sub

    Protected Sub tbEffectiveDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffectiveDate.SelectionChanged
        ChangeEffective()
    End Sub
    
    Protected Sub OnConfirm(ByVal sender As Object, ByVal e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
            execBtnGo(BtnGo)
        ElseIf confirmValue = "No" Then
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            execBtnGo(BtnGo)
        End If
    End Sub
    Sub execBtnGo(ByVal sender As Object)
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
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)

            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else

                    Result = ExecSPCommandGo(ActionValue, "S_PRPrice", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
