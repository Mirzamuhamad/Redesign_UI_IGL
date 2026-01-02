Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrGLAdjustCOGS_TrGLAdjustCOGS
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select DISTINCT TransNmbr, Status, TransDate, EffectiveDate, Warehouse, WarehouseName, Remark, UserPrep, UserAppr From V_GLAdjustCOGSHD "

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
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnAcc" Then
                    tbProduct.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    Label8.Text = Session("Result")(3)
                    ViewState("FgCostCtr") = Session("Result")(4).ToString
                    ViewState("FgType") = Session("Result")(5).ToString
                    
                    'ChangeCurrency(ddlUnit, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    
                End If
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
                        dr("Product") = drResult("Product")
                        dr("ProductName") = drResult("ProductName")
                        dr("Specification") = drResult("Specification")
                        dr("Remark") = ""
                        dr("Currency") = drResult("Currency")
                        dr("ForexRate") = GetCurrRate(drResult("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection"))
                        
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
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
        FillCombo(ddlWrhs, "EXEC S_GetWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("SortExpression") = Nothing
        ViewState("FgCostCtr") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
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
            'lbStatus.Text = ddlRange.SelectedValue + " " + StrFilter
            'Exit Sub
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLAdjustCOGSDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_GLFormAdjustCOGS " + Result
                Session("ReportFile") = ".../../../Rpt/FormAdjustCOGS.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_GLAdjustCOGS", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbRef.Enabled = State
            ddlWrhs.Enabled = State
            BtnGetDt.Visible = ddlWrhs.Enabled

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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                Row = ViewState("Dt").Select("Product = " + QuotedStr(tbProduct.Text))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("ProductName") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                Row("Unit") = Label8.Text
                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemarkDt.Text
                Row("OldPrice") = FormatFloat(tbCurrent.Text, ViewState("DigitQty"))
                Row("NewPrice") = FormatFloat(tbNew.Text, ViewState("DigitQty"))
                Row("Total") = FormatFloat(tbTotal.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemarkDt.Text

                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProduct.Text
                dr("ProductName") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                dr("Unit") = Label8.Text
                dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                dr("Remark") = tbRemarkDt.Text
                dr("OldPrice") = FormatFloat(tbCurrent.Text, ViewState("DigitQty"))
                dr("NewPrice") = FormatFloat(tbNew.Text, ViewState("DigitQty"))
                dr("Total") = FormatFloat(tbTotal.Text, ViewState("DigitQty"))
                dr("Remark") = tbRemarkDt.Text
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
                tbRef.Text = GetAutoNmbr("ADC", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO GLAdjustCOGSHd (TransNmbr, Status, TransDate, Warehouse, StartDate, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + ddlWrhs.SelectedValue + "', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLAdjustCOGSHD WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLAdjustCOGSHD SET Warehouse = '" + ddlWrhs.SelectedValue + "', Remark = '" + tbRemark.Text + "'," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate(), " + _
                " StartDate = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Qty, Unit, NewPrice, OldPrice, Total, Remark FROM GLAdjustCOGSDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLAdjustCOGSDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ClearHd()
            EnableHd(True)
            BtnGetDt.Enabled = True
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbRemark.Text = ""
            ddlWrhs.SelectedValue = ""
            tbEffectiveDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            tbCurrent.Text = "0"
            tbNew.Text = "0"
            tbTotal.Text = "0"
            Label8.Text = ""
            'ChangeCurrency(ddlUnit, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Product_Code, Product_Name, Specification, Qty from V_STProcessCOGSForAdj WHERE WrhsCode = " + QuotedStr(ddlWrhs.SelectedValue) + " AND End_Date - 1 = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'"
            ResultField = "Product_Code, Product_Name, Specification, Qty "
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        'Dr = FindMaster("ProductJE", tbProduct.Text + "|" + ddlJE.SelectedValue, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbProduct.Text = Dr("Product")
    '            tbProductName.Text = Dr("ProductName")
    '            BindToDropList(ddlUnit, Dr("Currency"))
    '            ddlCurr_SelectedIndexChanged(Nothing, Nothing)
    '            tbfgSubled.Text = Dr("FgSubled")
    '            ViewState("FgCostCtr") = Dr("FgCostCtr")
    '            ViewState("FgType") = Dr("FgType")
    '            If ViewState("FgCostCtr") = "N" Then
    '                ddlCostCtr.SelectedValue = ""
    '                ddlCostCtr.Enabled = False
    '            Else : ddlCostCtr.Enabled = True
    '            End If
    '            If ViewState("FgType") = "BS" Then
    '                ddlUnit.Enabled = False
    '            Else : ddlUnit.Enabled = True
    '            End If

    '        Else
    '            tbProduct.Text = ""
    '            tbProductName.Text = ""
    '            tbfgSubled.Text = "N"
    '            ddlCostCtr.SelectedValue = ""
    '            ddlCostCtr.Enabled = True
    '            ddlUnit.Enabled = False
    '        End If
    '        ChangeFgSubLed(tbfgSubled, tbSubled, btnSubled)
    '        tbDebitForex.Text = FormatNumber(CFloat(tbDebitForex.Text) * CFloat(tbRate.Text), ViewState("DigitCurr"))
    '        tbCreditHome.Text = FormatNumber(CFloat(tbCreditForex.Text) * CFloat(tbRate.Text), ViewState("DigitCurr"))
    '        tbProduct.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb acc Code Error : " + ex.ToString)
    '    End Try
    'End Sub


    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
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
            If ddlWrhs.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlWrhs.Focus()
                Return False
            End If

            If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
                lbStatus.Text = MessageDlg("Date must be inputed in Producting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
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
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty must have value")
                    Return False
                End If
                If Dr("OldPrice").ToString.Trim = Dr("NewPrice").ToString.Trim Then
                    lbStatus.Text = MessageDlg("New Price must be different with current price")
                    Return False
                End If
                
            Else
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If Label8.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Qty must have value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbCurrent.Text) = CFloat(tbNew.Text) Then
                    lbStatus.Text = MessageDlg("New Price must be different with current price")
                    Return False
                End If
                If CFloat(tbTotal.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Total Adjust must have value")
                    tbTotal.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub ddlWrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Dim DS As DataSet
        Try
            DS = SQLExecuteQuery("EXEC S_GLAdjustCOGSFindEndDate '" + ddlWrhs.SelectedValue + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'", ViewState("DBConnection").ToString)
            Dr = DS.Tables(0).Rows(0)
            If Not Dr Is Nothing Then
                BindToDate(tbEffectiveDate, Dr("EndDate").ToString)
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlWrhs_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "TransNmbr, Status, Remark"
            FilterValue = "TransNmbr, Status, Remark"
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
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
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
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_GLFormAdjustCOGS ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormAdjustCOGS.frx"
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
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                BtnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue)))
            'pnlEditDt.Visible = True
            'pnlDt.Visible = False
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbProduct.Focus()
            BtnGetDt.Enabled = False
            'tbProduct_TextChanged(Nothing, Nothing)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    Dim CrHome As Decimal = 0
    Dim DbHome As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
                    ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'DbHome = GetTotalSum(ViewState("Dt"), "DebitHome")
                    'CrHome = GetTotalSum(ViewState("Dt"), "CreditHome")
                    'e.Row.Cells(7).Text = "Total:"
                    ' for the Footer, display the running totals
                    'e.Row.Cells(8).Text = FormatNumber(DbHome, ViewState("DigitHome"))
                    'e.Row.Cells(10).Text = FormatNumber(CrHome, ViewState("DigitHome"))
                End If
                ' tbDebit.Text = FormatNumber(DbHome, ViewState("DigitHome"))
                ' tbCredit.Text = FormatNumber(CrHome, ViewState("DigitHome"))
                '  tbSelisih.Text = FormatNumber(DbHome - CrHome, ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGetDt.Click
        Dim DS As DataSet
        Dim NewRow As DataRow
        Dim ExistRow As DataRow()

        Try
            DS = SQLExecuteQuery("EXEC S_GLAdjustCOGSGetDt '" + ddlWrhs.SelectedValue + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', ''", ViewState("DBConnection").ToString)
            If DS.Tables(0).Rows.Count > 0 Then
                ExistRow = ViewState("Dt").Select("Product = " + QuotedStr(tbProduct.Text))
            End If
            For Each dr In DS.Tables(0).Rows
                NewRow = ViewState("Dt").NewRow
                NewRow("Product") = dr("ProductCode")
                NewRow("ProductName") = dr("ProductName")
                NewRow("specification") = dr("specification")
                NewRow("OldPrice") = FormatFloat(dr("Price"), ViewState("DigitCurr").ToString)
                NewRow("NewPrice") = FormatFloat(dr("Price"), ViewState("DigitCurr").ToString)
                NewRow("Total") = FormatFloat(NewRow("NewPrice") - NewRow("OldPrice"), ViewState("DigitCurr").ToString)
                NewRow("Qty") = FormatFloat(dr("Qty"), ViewState("DigitQty").ToString)
                NewRow("Unit") = dr("Unit")
                NewRow("Remark") = ""

                ViewState("Dt").Rows.Add(NewRow)
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "BtnGetDt Error : " + ex.ToString
        End Try
        'Session("Result") = Nothing
        'Session("Filter") = "select * from V_MsProductDt WHERE TransType = 'JE' "
        'ResultField = "Product, Description, Currency, FgSubled"
        'Session("Column") = ResultField.Split(",")
        ''ResultSame = "Currency"
        ''Session("ResultSame") = ResultSame.Split(",")
        'ViewState("Sender") = "btnGetDt"
        'Session("DBConnection") = ViewState("DBConnection")
        'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
       
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlWrhs, Dt.Rows(0)("Warehouse").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Productname").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, FormatFloat(Dr(0)("Qty").ToString, ViewState("DigitQty")))
                Label8.Text = Dr(0)("Unit").ToString
                BindToText(tbCurrent, FormatFloat(Dr(0)("OldPrice").ToString, ViewState("DigitQty")))
                BindToText(tbNew, FormatFloat(Dr(0)("NewPrice").ToString, ViewState("DigitQty")))
                BindToText(tbTotal, FormatFloat(Dr(0)("Total").ToString, ViewState("DigitQty")))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'ChangeCurrency(ddlUnit, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            End If

        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub tbNew_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNew.TextChanged
        Try
            tbTotal.Text = FormatFloat(tbNew.Text - tbCurrent.Text, ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbNew_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProduct_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProduct.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "EXEC S_GLAdjustCOGSGetDt '" + ddlWrhs.SelectedValue + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', ' AND Product_Code = '" + QuotedStr(tbProduct.Text) + "''"
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProduct.Text = Dr("ProductCode")
                tbProductName.Text = Dr("ProductName")
                tbSpecification.Text = Dr("specification")
                Label8.Text = Dr("Unit")
                tbCurrent.Text = FormatFloat(Dr("Price"), ViewState("DigitCurr").ToString)
                tbNew.Text = FormatFloat(Dr("Price"), ViewState("DigitCurr").ToString)
                tbTotal.Text = FormatFloat(tbNew.Text - tbCurrent.Text, ViewState("DigitCurr").ToString)
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                Label8.Text = ""
                tbCurrent.Text = FormatFloat(0, ViewState("DigitCurr").ToString)
                tbNew.Text = FormatFloat(0, ViewState("DigitCurr").ToString)
                tbTotal.Text = FormatFloat(0, ViewState("DigitCurr").ToString)
            End If

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "BtnGetDt Error : " + ex.ToString
        End Try
    End Sub
End Class
