Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPriceList_TrPriceList
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    
    Private Function GetStringHd(ByVal UserId As String) As String
        Return "SELECT A.* FROM V_MKPriceHd A INNER JOIN VMsDeptUser B ON A.Department = B.Department WHERE B.UserId = " + QuotedStr(UserId)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetGrade") = False
                FillCombo(ddlCurrency, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""

                ddlRow.SelectedValue = "20"
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("Product") = drResult("ProductCode")
                        dr("product_Name") = drResult("ProductName")
                        dr("OldPrice") = FormatNumber(drResult("OldPrice"), 6) 'FormatNumber(drResult("OldPrice"), ViewState("DigitCurr"))
                        dr("NewPrice") = FormatNumber(drResult("NewPrice"), 6) 'FormatNumber(drResult("NewPrice"), ViewState("DigitCurr"))
                        dr("Unit") = drResult("Unit")
                        dr("MOQ") = "1"
                        dr("AdjType") = "+"
                        dr("AdjPercent") = "0"
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = True
                    'Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    ddlUnit.SelectedValue = Session("Result")(2).ToString
                    tbOldPrice.Text = Session("Result")(3).ToString
                    tbNewPrice.Text = Session("Result")(4).ToString
                    tbMOQ.Text = "1"
                    tbAdjustPercent.Text = "0"
                End If
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    tbCustName.Text = Session("Result")(1).ToString
                    ddlCurrency.SelectedValue = Session("Result")(2).ToString
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
        FillCombo(ddlDept, "SELECT Department, DepartmentName FROM VMsDeptUser WHERE UserId = " + QuotedStr(ViewState("UserId").ToString), True, "Department", "DepartmentName", ViewState("DBConnection"))
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        'tbRef.Attributes.Add("ReadOnly", "True")
        tbOldPrice.Attributes.Add("ReadOnly", "True")
        tbCustName.Attributes.Add("ReadOnly", "True")
        tbProductName.Attributes.Add("ReadOnly", "True")
        Me.tbOldPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbNewPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbMOQ.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbAdjustPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbAdjPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbNewPrice.Attributes.Add("OnBlur", "setformat();")
        'Me.tbAdjustPercent.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            End If
            StrFilter = StrFilter.Replace("DepartmentName", "A.DepartmentName")
            DT = BindDataTransaction(GetStringHd(ViewState("UserId").ToString), StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * FROM V_MKPriceDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
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
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_MKPrice", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            btnGetDt.Visible = State
            'tbRef.Enabled = State
            tbCustCode.Enabled = State
            tbCustName.Enabled = State
            BtnCust.Visible = State
            ddlPriceIncludeTax.Enabled = State
            'tbQuotationNo.Enabled = State
            tbEffectiveDate.Enabled = State
            ddlCurrency.Enabled = State
            ddlDept.Enabled = State
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
                tbRef.Text = GetAutoNmbr("PPL", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlDept.SelectedValue, ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO MKTPriceHd (TransNmbr, Status, TransDate, QuotationNo, StartEffective, Customer, Currency, PriceIncludeTax, Remark, UserPrep, DatePrep, Department) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbQuotationNo.Text + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbCustCode.Text + "', '" + ddlCurrency.SelectedValue + "', '" + ddlPriceIncludeTax.SelectedValue + "', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(ddlDept.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM MKTPriceHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MKTPriceHd SET QuotationNo = " + QuotedStr(tbQuotationNo.Text) + _
                ", StartEffective = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                " Customer = " + QuotedStr(tbCustCode.Text) + _
                ", Currency = " + QuotedStr(ddlCurrency.SelectedValue) + _
                ", PriceIncludeTax = " + QuotedStr(ddlPriceIncludeTax.SelectedValue) + _
                ", Remark = '" + tbRemark.Text + "'," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate(), Department = " + QuotedStr(ddlDept.SelectedValue) + _
                " WHERE TransNmbr = '" + tbRef.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbRef.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT * FROM MKTPriceDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE MKTPriceDt SET Product = @Product, MOQ = @MOQ, Unit = @Unit, OldPrice = @OldPrice, AdjType = @AdjType, AdjPercent = @AdjPercent, NewPrice = @NewPrice WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @OldProduct ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@MOQ", SqlDbType.Float, 18, "MOQ")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@OldPrice", SqlDbType.Float, 18, "OldPrice")
            Update_Command.Parameters.Add("@AdjType", SqlDbType.VarChar, 1, "AdjType")
            Update_Command.Parameters.Add("@AdjPercent", SqlDbType.Float, 18, "AdjPercent")
            Update_Command.Parameters.Add("@NewPrice", SqlDbType.Float, 18, "NewPrice")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MKTPriceDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTPriceDt")

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
        Dim DtCek, DtCek2 As DataTable
        Dim SQLCek, SQLCek2 As String
        Dim DrCek2 As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            'If ViewState("StateHd") = "Insert" Then
            '    SQLCek = "EXEC S_MKPriceCekQuotaion " + QuotedStr(tbQuotationNo.Text)
            '    DtCek = SQLExecuteQuery(SQLCek, ViewState("DBConnection").ToString).Tables(0)
            '    If DtCek.Rows.Count > 0 Then
            '        SQLCek2 = "SELECT TOP 1 TransNmbr, TransDate, QuotationNo FROM MKTPriceHd ORDER BY TransNmbr DESC"
            '        DtCek2 = SQLExecuteQuery(SQLCek2, ViewState("DBConnection").ToString).Tables(0)
            '        If DtCek2.Rows.Count > 0 Then
            '            DrCek2 = DtCek2.Rows(0)
            '            lbStatus.Text = MessageDlg("Quotation No has already exists, last Quotation is " + DrCek2("QuotationNo"))
            '            tbQuotationNo.Focus()
            '            Exit Sub
            '        End If
            '    End If
            'Else
            '    If tbQuotationNo.Text.Trim <> ViewState("PrevQuotation") Then
            '        SQLCek = "EXEC S_MKPriceCekQuotaion " + QuotedStr(tbQuotationNo.Text)
            '        DtCek = SQLExecuteQuery(SQLCek, ViewState("DBConnection").ToString).Tables(0)
            '        If DtCek.Rows.Count > 0 Then
            '            SQLCek2 = "SELECT TOP 1 TransNmbr, TransDate, QuotationNo FROM MKTPriceHd ORDER BY TransNmbr DESC"
            '            DtCek2 = SQLExecuteQuery(SQLCek2, ViewState("DBConnection").ToString).Tables(0)
            '            If DtCek2.Rows.Count > 0 Then
            '                DrCek2 = DtCek2.Rows(0)
            '                lbStatus.Text = MessageDlg("Quotation No has already exists, last Quotation is " + DrCek2("QuotationNo"))
            '                tbQuotationNo.Focus()
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If

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
            ddlField.SelectedValue = "TransNmbr"
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
            EnableHd(True)
            btnHome.Visible = False
            Panel1.Visible = True
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Add") = "Clear"
            ViewState("DigitCurr") = 0
            tbAdjPercent.Text = "0"
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEffectiveDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlCurrency.SelectedValue = ViewState("Currency").ToString
            ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
            tbMOQ.Text = "1"
            pnlDt.Visible = True
            BindDataDt("")
            GridDt.Columns(1).Visible = False
            ViewState("PrevQuotation") = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbQuotationNo.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            ddlPriceIncludeTax.SelectedValue = "N"
            tbRemark.Text = ""
            ddlDept.SelectedValue = ""
            tbEffectiveDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbMOQ.Text = "0"
            tbOldPrice.Text = "0"
            ddlAdjType.SelectedValue = "+"
            tbAdjustPercent.Text = "0"
            tbNewPrice.Text = "0"
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
        Dim DtCek, DtCek2 As DataTable
        Dim SQLCek, SQLCek2 As String
        Dim DrCek2 As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            'If ViewState("StateHd") = "Insert" Then
            '    SQLCek = "EXEC S_MKPriceCekQuotaion " + QuotedStr(tbQuotationNo.Text)
            '    DtCek = SQLExecuteQuery(SQLCek, ViewState("DBConnection").ToString).Tables(0)
            '    If DtCek.Rows.Count > 0 Then
            '        SQLCek2 = "SELECT TOP 1 TransNmbr, TransDate, QuotationNo FROM MKTPriceHd ORDER BY TransNmbr DESC"
            '        DtCek2 = SQLExecuteQuery(SQLCek2, ViewState("DBConnection").ToString).Tables(0)
            '        If DtCek2.Rows.Count > 0 Then
            '            DrCek2 = DtCek2.Rows(0)
            '            lbStatus.Text = MessageDlg("Quotation No has already exists, last Quotation is " + DrCek2("QuotationNo"))
            '            tbQuotationNo.Focus()
            '            Exit Sub
            '        End If
            '    End If
            'Else
            '    If tbQuotationNo.Text.Trim <> ViewState("PrevQuotation") Then
            '        SQLCek = "EXEC S_MKPriceCekQuotaion " + QuotedStr(tbQuotationNo.Text)
            '        DtCek = SQLExecuteQuery(SQLCek, ViewState("DBConnection").ToString).Tables(0)
            '        If DtCek.Rows.Count > 0 Then
            '            SQLCek2 = "SELECT TOP 1 TransNmbr, TransDate, QuotationNo FROM MKTPriceHd ORDER BY TransNmbr DESC"
            '            DtCek2 = SQLExecuteQuery(SQLCek2, ViewState("DBConnection").ToString).Tables(0)
            '            If DtCek2.Rows.Count > 0 Then
            '                DrCek2 = DtCek2.Rows(0)
            '                lbStatus.Text = MessageDlg("Quotation No has already exists, last Quotation is " + DrCek2("QuotationNo"))
            '                tbQuotationNo.Focus()
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If

            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            EnableHd(True)
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurrency, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
        'tbOldPrice.Focus()       
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        If ViewState("Add") = "Clear" Then
            Cleardt()
        Else
            ddlAdjustType.SelectedValue = "+"
        End If

        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbQuotationNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Quotation No must have value")
                tbQuotationNo.Focus()
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
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustCode.Focus()
                Return False
            End If
            If ddlCurrency.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurrency.Focus()
                Return False
            End If
            If ddlPriceIncludeTax.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Price Include Tax must have value")
                ddlPriceIncludeTax.Focus()
                Return False
            End If
            If ddlDept.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                ddlDept.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If CFloat(Dr("MOQ").ToString.Trim) <= "0" Then
                    lbStatus.Text = MessageDlg("MOQ Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                If CFloat(Dr("NewPrice").ToString.Trim) <= "0" Then
                    lbStatus.Text = MessageDlg("New Price Must Have Value")
                    Return False
                End If
            Else
                If tbProductCode.ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If CFloat(tbMOQ.Text.Trim) <= "0" Then
                    lbStatus.Text = MessageDlg("MOQ Must Have Value")
                    tbMOQ.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If CFloat(tbNewPrice.Text.Trim) <= "0" Then
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
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Quotation No, Start Effective Date, Customer Code, Customer Name, Currency, Price Include Tax, Remark"
            FilterValue = "Reference, QuotationNo, dbo.FormatDate(StartEffective), Customer, Customer_Name, Currency, PriceIncludeTax, Remark"
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
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(1).Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    Panel1.Visible = False
                    btnHome.Visible = True
                    'Dim Dr As DataRow
                    'Dr = FindMaster("Rate", ddlCurrency.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), Session("DBConnection").ToString)
                    'If Not Dr Is Nothing Then
                    '    ViewState("DigitCurr") = Dr("digit")
                    '    AttachScript("setformat();", Page, Me.GetType())
                    'End If
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        Panel1.Visible = True
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt.Columns(1).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        ViewState("PrevQuotation") = tbQuotationNo.Text
                        tbAdjPercent.Text = "0"
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBCOnnection") = ViewState("DBConnection")
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_MKFormPrice " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormMKPrice.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
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
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
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
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd(ViewState("UserId").ToString), "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbQuotationNo, Dt.Rows(0)("QuotationNo").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("StartEffective").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToDropList(ddlCurrency, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlPriceIncludeTax, Dt.Rows(0)("PriceIncludeTax").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbMOQ, Dr(0)("MOQ").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToText(tbOldPrice, Dr(0)("OldPrice").ToString)
                BindToDropList(ddlAdjType, Dr(0)("AdjType").ToString)
                BindToText(tbAdjustPercent, Dr(0)("AdjPercent").ToString)
                BindToText(tbNewPrice, Dr(0)("NewPrice").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
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
            Session("DBConnection") = ViewState("DBConnection")
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "EXEC S_MKPriceGetDt " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd"))
            ResultField = "ProductCode, ProductName, OldPrice, NewPrice, Unit"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPrice.SelectedIndexChanged
    '    'If ViewState("InputProduct") = "Y" Then
    '    '    RefreshMaster("S_GetProductPrice", "Product_Price_Code", "Product_Price_Name", tbPrice, Session("DBConnection"))
    '    '    ViewState("InputProduct") = Nothing
    '    'End If
    '    Dim dr As SqlDataReader
    '    dr = SQLExecuteReader("EXEC S_GLSalesPriceGetPrice " + QuotedStr(ddlClass.SelectedValue) + ", " + QuotedStr(ddlSize.SelectedValue) + ", " + QuotedStr(tbPrice.Text) + ", " + QuotedStr(ddlCustType.SelectedValue) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")), Session("DBConnection"))
    '    dr.Read()
    '    tbOldPrice.Text = FormatNumber(dr("Price"), ViewState("DigitCurr"))
    '    tbNewPrice.Text = FormatNumber(dr("Price"), ViewState("DigitCurr"))
    '    dr.Close()
    '    'AttachScript("setformat();", Page, Me.GetType())
    '    tbNewPrice.Focus()
    'End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(tbProductCode.Text) Then
                    If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                        lbStatus.Text = "Product has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If


                Row.BeginEdit()
                'Row("ProductPrice") = tbCode.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("MOQ") = tbMOQ.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("OldPrice") = tbOldPrice.Text
                Row("AdjType") = ddlAdjType.SelectedValue
                Row("AdjPercent") = tbAdjustPercent.Text
                Row("NewPrice") = tbNewPrice.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                    lbStatus.Text = "Product '" + tbProductCode.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("MOQ") = tbMOQ.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("OldPrice") = tbOldPrice.Text
                dr("AdjType") = ddlAdjType.SelectedValue
                dr("AdjPercent") = tbAdjustPercent.Text
                dr("NewPrice") = tbNewPrice.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            ViewState("Add") = "NotClear"
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

    Protected Sub GetOldPrice()
        Dim dr1 As SqlDataReader
        Try
            dr1 = SQLExecuteReader("EXEC S_MKPriceGetDt " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection"))
            dr1.Read()
            tbOldPrice.Text = FormatNumber(dr1("OldPrice"), 6) 'FormatNumber(dr1("OldPrice"), ViewState("DigitCurr"))
            tbNewPrice.Text = FormatNumber(dr1("NewPrice"), 6) 'FormatNumber(dr1("NewPrice"), ViewState("DigitCurr"))
            dr1.Close()
            tbNewPrice.Focus()
            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Get Price Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                'If cb.Checked = False Then
                'btnGetSetZero.Visible = True
                'End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Private Sub bindDataSetCustType()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            
            Dim HaveSelect As Boolean
            Dim CekKey As String
            Dim dr As DataRow
            HaveSelect = False
            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")
                'lbStatus.Text = "3 : " + GVR.Cells(3).Text + " 4 : " + GVR.Cells(4).Text + " 5 : " + GVR.Cells(5).Text + " 6 : " + GVR.Cells(6).Text + " 7 : " + GVR.Cells(7).Text + " 8 : " + GVR.Cells(8).Text + " 9 : " + GVR.Cells(9).Text + " 10 : " + GVR.Cells(10).Text + " 11 : " + GVR.Cells(11).Text + " 14 : " + GVR.Cells(14).Text + " 15 : " + GVR.Cells(15).Text + " 16 : " + GVR.Cells(16).Text
                'Exit Sub
                '3 : Life Style Centro 4 : 5 : Motif Seri A 6 : 7 : 40x40 8 : 9 : KW A 10 : 11 : All Motif 12 : 13 : PT
                If CB.Checked Then
                    CekKey = TrimStr(GVR.Cells(2).Text)
                    HaveSelect = True
                    If CekExistData(ViewState("Dt"), "Product", CekKey) Then
                        dr = ViewState("Dt").Select("Product = " + QuotedStr(CekKey))(0)
                        dr.BeginEdit()
                        dr("Product") = TrimStr(GVR.Cells(2).Text)
                        dr("Product_Name") = TrimStr(GVR.Cells(3).Text)
                        dr("MOQ") = TrimStr(GVR.Cells(4).Text)
                        dr("Unit") = TrimStr(GVR.Cells(5).Text)
                        dr("OldPrice") = FormatNumber(TrimStr(GVR.Cells(6).Text), 6) 'FormatNumber(TrimStr(GVR.Cells(6).Text), ViewState("DigitCurr"))
                        dr("AdjType") = ddlAdjustType.SelectedValue
                        If dr("OldPrice") <> 0 Then
                            dr("AdjPercent") = tbAdjPercent.Text
                        Else
                            dr("AdjPercent") = 0
                        End If
                        If dr("AdjType") = "+" Then
                            dr("NewPrice") = FormatNumber((CFloat(dr("OldPrice")) + (CFloat(dr("AdjPercent")) * CFloat(dr("OldPrice"))) / 100), 6) 'FormatNumber((CFloat(dr("OldPrice")) + (CFloat(dr("AdjPercent")) * CFloat(dr("OldPrice"))) / 100), ViewState("DigitCurr"))
                        Else
                            dr("NewPrice") = FormatNumber((CFloat(dr("OldPrice")) - (CFloat(dr("AdjPercent")) * CFloat(dr("OldPrice"))) / 100), 6) 'FormatNumber((CFloat(dr("OldPrice")) - (CFloat(dr("AdjPercent")) * CFloat(dr("OldPrice"))) / 100), ViewState("DigitCurr"))
                        End If
                        'dr("NewPrice") = FormatNumber(tbNewPrice.Text, ViewState("DigitCurr")) 'FormatNumber(TrimStr(GVR.Cells(9).Text), ViewState("DigitCurr"))
                        dr.EndEdit()
                    End If
                End If
            Next
            If HaveSelect = False Then
                lbStatus.Text = "Please Check Product for Process"
                Exit Sub
            Else
                lbStatus.Text = "Set Adjust Success for Selected Product"
            End If
            BindGridDt(ViewState("Dt"), GridDt)
            'BindGridDtView()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("bindDataGridCustType Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            If (ddlAdjustType.SelectedValue = "") Or (CFloat(tbAdjPercent.Text) = 0) Then
                lbStatus.Text = "Set Selected be selected"
                ddlAdjustType.Focus()
                Exit Sub
            End If
            bindDataSetCustType()
        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCust.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsCustomer WHERE FgActive = 'Y'"
            ResultField = "Customer_Code, Customer_Name, Currency"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                ddlCurrency.SelectedValue = Dr("Currency")
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbCustCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GetTypeChange()
        Try
            If ddlAdjType.SelectedValue = "+" Then
                tbNewPrice.Text = CFloat(tbOldPrice.Text) + (CFloat(tbAdjustPercent.Text) * CFloat(tbOldPrice.Text)) / 100
            Else
                tbNewPrice.Text = CFloat(tbOldPrice.Text) - (CFloat(tbAdjustPercent.Text) * CFloat(tbOldPrice.Text)) / 100
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("GetTypeChange Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlAdjType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAdjType.SelectedIndexChanged
        GetTypeChange()
    End Sub

    Protected Sub tbAdjustPercent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAdjustPercent.TextChanged
        If CFloat(tbOldPrice.Text) = 0 Then
            tbAdjustPercent.Text = "0"
        Else
            GetTypeChange()
        End If
    End Sub

    Protected Sub BtnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "EXEC S_MKPriceGetDt " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd"))
            ResultField = "ProductCode, ProductName, Unit, OldPrice, NewPrice"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MKPriceGetDt " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                If Dr("ProductCode") = tbProductCode.Text Then
                    tbProductCode.Text = Dr("ProductCode")
                    tbProductName.Text = Dr("ProductName")
                    ddlUnit.SelectedValue = Dr("unit")
                    tbOldPrice.Text = Dr("OldPrice")
                    tbNewPrice.Text = Dr("NewPrice")
                    tbAdjustPercent.Text = "0"
                Else
                    tbProductCode.Text = ""
                    tbProductName.Text = ""
                    tbOldPrice.Text = "0"
                    tbNewPrice.Text = "0"
                    tbAdjustPercent.Text = "0"
                End If
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbOldPrice.Text = "0"
                tbNewPrice.Text = "0"
                tbAdjustPercent.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub GetAdjPercent()
        Dim HargaLama, HargaBaru, Hasil As Double
        Try
            HargaLama = tbOldPrice.Text
            HargaBaru = tbNewPrice.Text
            Hasil = 0
            If CFloat(tbOldPrice.Text) = 0 Then
                Exit Sub
            ElseIf CFloat(tbNewPrice.Text) < CFloat(tbOldPrice.Text) Then
                ddlAdjType.SelectedValue = "-"
            Else
                ddlAdjType.SelectedValue = "+"
            End If

            If CFloat(HargaBaru) > CFloat(HargaLama) Then
                Hasil = ((CFloat(HargaBaru) - CFloat(HargaLama)) / CFloat(HargaLama) * 100)
            Else
                Hasil = ((CFloat(HargaLama) - CFloat(HargaBaru)) / CFloat(HargaLama) * 100)
            End If

            tbAdjustPercent.Text = FormatFloat(Hasil, 4) 'FormatFloat(Hasil, ViewState("DigitCurr"))

            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("tbNewPrice_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbNewPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewPrice.TextChanged
        GetAdjPercent()
    End Sub

    Protected Sub lbCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCustomer_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            GridView1.PageSize = ddlRow.SelectedValue
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
