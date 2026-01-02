Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrBeginningWIP_TrBeginningWIP
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_BeginningWIPHd"

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_BeginningWIPDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
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
                FillCombo(ddlWIP, "Select WIPCode, WIPName From MsWIP", True, "WIPCode", "WIPName", ViewState("DBConnection"))
                FillCombo(ddlWarehouse, "select Wrhs_Code, Wrhs_Name from VMsWarehouse where Wrhs_Type='Production'", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    ViewState("FgSubled") = "P"
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
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbQty.Attributes.Add("OnBlur", "hitung('');")
        tbPrice.Attributes.Add("OnBlur", "hitung('');")

        tbTotal.Attributes.Add("ReadOnly", "True")
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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
                    Result = ExecSPCommandGo(ActionValue, "S_MKBeginningWIP", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDate.Enabled = State
            ddlWarehouse.Enabled = State
            tbRemark.Enabled = State
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
                tbRef.Text = GetAutoNmbr("BW", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PROWIPBeginHd (TransNmbr, TransDate, Status, Warehouse, " + _
                            "Remark, UserPrep, DatePrep) " + _
                            "SELECT '" + tbRef.Text + "', '" + _
                            Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', 'H', '" + _
                            ddlWarehouse.SelectedValue + "','" + _
                            tbRemark.Text + "'," + _
                            QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PROWIPBeginHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PROWIPBeginHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                            "', Remark = '" + tbRemark.Text + _
                            "', Warehouse = '" + ddlWarehouse.SelectedValue + _
                            "', DateAppr = getDate() WHERE TransNmbr = '" + tbRef.Text + "'"
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
            Dim cmdSql As New SqlCommand("SELECT * FROM PROWIPBeginDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PROWIPBeginDt SET WIP = @WIP, Qty = @Qty, Price = @Price, Total = @Total, Remark = @Remark, FgSubLed = @FgSubLed, ProductFG = @ProductFG WHERE TransNmbr = '" & ViewState("Reference") & "' AND WIP = @WP", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@WIP", SqlDbType.VarChar, 20, "WIP")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Price", SqlDbType.Float, 18, "Price")
            Update_Command.Parameters.Add("@Total", SqlDbType.Float, 18, "Total")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@FgSubLed", SqlDbType.VarChar, 1, "FgSubLed")
            Update_Command.Parameters.Add("@ProductFG", SqlDbType.VarChar, 20, "ProductFG")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@WP", SqlDbType.VarChar, 20, "WIP")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROWIPBeginDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND WIP = @WIP", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@WIP", SqlDbType.VarChar, 20, "WIP")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROWIPBeginDt")

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
        'Dim DtCek, DtCek2 As DataTable
        'Dim SQLCek, SQLCek2 As String
        'Dim DrCek2 As DataRow

        Try
            If CekHd() = False Then
                Exit Sub
            End If

            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

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
            'Panel1.Visible = True
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
            'tbAdjPercent.Text = "0"
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
            'GridDt.Columns(1).Visible = False
            ViewState("PrevQuotation") = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            ddlWarehouse.SelectedIndex = 0
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            ddlWIP.SelectedIndex = 0
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbQty.Text = "0"
            tbPrice.Text = "0"
            tbTotal.Text = "0"
            tbRemarkDt.Text = ""
            ViewState("FgSubled") = "N"
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
        'Dim DtCek, DtCek2 As DataTable
        'Dim SQLCek, SQLCek2 As String
        'Dim DrCek2 As DataRow

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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        'If ViewState("Add") = "Clear" Then
        '    Cleardt()
        'Else
        '    'ddlAdjustType.SelectedValue = "+"
        'End If

        Cleardt()

        If CekHd() = False Then
            Exit Sub
        End If

        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        ddlWIP_SelectedIndexChanged(Nothing, Nothing)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
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
                If Dr("WIP").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("WIP Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Price").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
            Else
                If ddlWIP.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("WIP Must Have Value")
                    ddlWIP.Focus()
                    Return False
                End If
                If tbQty.ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If tbQty.ToString.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Qty cannot 0 value")
                    tbQty.Focus()
                    Return False
                End If
                If tbPrice.ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    tbPrice.Focus()
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
            FilterValue = "TransNmbr, QuotationNo, dbo.FormatDate(StartEffective), Customer, Customer_Name, Currency, PriceIncludeTax, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            Session("DateTime") = ViewState("ServerDate")
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
                    'Panel1.Visible = False
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
                        'Panel1.Visible = True
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt.Columns(1).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'ViewState("PrevQuotation") = tbQuotationNo.Text
                        'tbAdjPercent.Text = "0"
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
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
                        Session("SelectCommand") = "EXEC S_FormComplainPrint " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/RptMKTCustComplain.frx"
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

            ViewState("DtValue") = GVR.Cells(2).Text

            'dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(4).Text))
            dr = ViewState("Dt").Select("WIP = " + QuotedStr(ViewState("DtValue")))
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
            'ViewState("DtValue") = GVR.Cells(4).Text
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            ddlWIP_SelectedIndexChanged(Nothing, Nothing, "edit")
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WIP = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToDropList(ddlWIP, Dr(0)("WIP").ToString)
                BindToText(tbProductCode, Dr(0)("ProductFG").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbPrice, Dr(0)("Price").ToString)
                BindToText(tbTotal, Dr(0)("Total").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If tbQty.Text = "" Then
                tbQty.Text = 0
            End If

            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(ddlWIP.SelectedValue) Then
                    If CekExistData(ViewState("Dt"), "WIP", TrimStr(ddlWIP.SelectedValue)) Then
                        lbStatus.Text = "Data has already exists"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("WIP = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("WIP") = ddlWIP.SelectedValue
                Row("WIPName") = ddlWIP.SelectedItem.Text
                Row("ProductFG") = tbProductCode.Text
                Row("ProductName") = tbProductName.Text
                Row("Qty") = tbQty.Text
                Row("Price") = tbPrice.Text
                Row("Total") = tbTotal.Text
                Row("FgSubled") = ViewState("FgSubled")
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "WIP", TrimStr(ddlWIP.SelectedValue)) Then
                    lbStatus.Text = "Data has already exists"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WIP") = ddlWIP.SelectedValue
                dr("WIPName") = ddlWIP.SelectedItem.Text
                dr("ProductFG") = tbProductCode.Text
                dr("ProductName") = tbProductName.Text
                dr("Qty") = tbQty.Text
                dr("Price") = tbPrice.Text
                dr("Total") = tbTotal.Text
                dr("FgSubled") = ViewState("FgSubled")
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            'ViewState("Add") = "NotClear"
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

    'Protected Sub lbProblem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProblem.Click
    '    Try
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProblem')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lbRoom_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub lbWIP_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles lbWIP.Command
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsWIP')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbWIP_Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE FgActive = 'Y' And ProductCategory='Finish Goods'"
            ResultField = "Product_Code, Product_Name, Unit, Specification"

            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProduct_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String

        Try
            SQLString = "SELECT Product_Code AS ProductCode, Product_Name AS ProductName, Specification, Unit FROM VMsProduct WHERE FgActive = 'Y' And ProductCategory='Finish Goods' AND Product_Code = " + QuotedStr(tbProductCode.Text)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProductCode, Trim(Dr("ProductCode").ToString))
                BindToText(tbProductName, Trim(Dr("ProductName").ToString))
                ViewState("FgSubled") = "P"
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                ViewState("FgSubled") = "N"
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWIP_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal edit As String = "") Handles ddlWIP.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Dim wipType As String

        Try
            wipType = ddlWIP.SelectedValue

            SQLString = "SELECT * From MsWIP Where WIPCode = " + QuotedStr(wipType)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                ViewState("WIPType") = Dr("WIPType").ToString
            Else
                ViewState("WIPType") = ""
            End If

            If ViewState("WIPType") = "FG" Then
                If edit <> "edit" Then
                    tbProductCode.Text = ""
                    tbProductName.Text = ""
                End If
                tbProductCode.Enabled = True
                btnProduct.Enabled = True
            Else
                If edit <> "edit" Then
                    tbProductCode.Text = ""
                    tbProductName.Text = ""
                End If
                tbProductCode.Enabled = False
                btnProduct.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("ddlWIP_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
