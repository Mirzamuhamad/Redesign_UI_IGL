Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPRBidding_TrPRBidding
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PRBiddingHd"

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
                If ViewState("Sender") = "btnproduct" Then
                    tbProduct.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbSuppCode, Session("Result")(2).ToString)
                    BindToText(tbSuppName, Session("Result")(3).ToString)
                    BindToDropList(ddlUnit, Session("Result")(4).ToString)
                    'BindToText(tbPrice, FormatNumber(Session("Result")(5).ToString), ViewState("DigitCurr"))
                    BindToText(tbPrice, Session("Result")(5).ToString)
                    BindToDropList(ddlTerm, Session("Result")(6).ToString)
                    BindToText(tbLeadTime, Session("Result")(7).ToString)
                    BindToText(tbPrice0, Session("Result")(8).ToString)
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        'Dim dre As SqlDataReader
                        dr = ViewState("Dt").NewRow
                        dr("Product") = drResult("Product_Code")
                        dr("Product_Name") = drResult("Product_Name")
                        dr("Supplier") = drResult("Supplier_Code")
                        dr("Supplier_Name") = drResult("Supplier_Name")
                        dr("Unit") = drResult("Unit")
                        dr("MOQ") = drResult("MOQ")
                        dr("Price") = drResult("Price")
                        dr("Term") = drResult("Term_Code")
                        dr("DeliveryTime") = drResult("LeadTime")
                        dr("Selected") = "N"
                        ViewState("Dt").Rows.Add(dr)
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
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
        FillCombo(ddlTerm, "SELECT TermCode, TermName From MsTerm", True, "TermCode", "TermName", ViewState("DBConnection"))

        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbLeadTime.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbTop.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPrice.Attributes.Add("OnBlur", "setformat();")
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
        Return "SELECT * FROM V_PRBiddingDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PRFormBidding " + Result
                Session("ReportFile") = ".../../../Rpt/FormPRBidding.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRBidding", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnGetDt.Visible = State
            tbDate.Enabled = State
            ddlType.Enabled = State
            tbTop.Enabled = State
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
                tbRef.Text = GetAutoNmbr("SB", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PRCBiddingHd (TransNmbr, Status, TransDate, Type, TopRecord, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlType.SelectedValue) + ", " + tbTop.Text + ", " + QuotedStr(tbRemark.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PRCBiddingHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCBiddingHd SET Type = " + QuotedStr(ddlType.SelectedValue) + _
                            ", TopRecord = " + tbTop.Text + _
                            ", Remark = " + QuotedStr(tbRemark.Text) + _
                            ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Supplier, Unit, Price, Term, DeliveryTime, Selected, MOQ  FROM PRCBiddingDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCBiddingDt SET Product = @Product, Supplier = @Supplier, Unit = @Unit, Price = @Price, Term = @Term, DeliveryTime = @DeliveryTime, " + _
                    "Selected = @Selected, MOQ = @MOQ " + _
                     "WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Supplier = @OldSupplier AND Product = @OldProduct AND Unit = @OldUnit", con)

            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Supplier", SqlDbType.VarChar, 20, "Supplier")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Price", SqlDbType.Float, 20, "Price")
            Update_Command.Parameters.Add("@Term", SqlDbType.VarChar, 5, "Term")
            Update_Command.Parameters.Add("@DeliveryTime", SqlDbType.Int, 4, "DeliveryTime")
            Update_Command.Parameters.Add("@Selected", SqlDbType.VarChar, 1, "Selected")
            Update_Command.Parameters.Add("@MOQ", SqlDbType.Float, 20, "MOQ")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldSupplier", SqlDbType.VarChar, 20, "Supplier")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldUnit", SqlDbType.VarChar, 5, "Unit")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCBiddingDt WHERE TransNmbr = " & QuotedStr(ViewState("Reference")) & " AND Supplier = @Supplier AND Product = @Product AND Unit = @Unit ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Supplier", SqlDbType.VarChar, 20, "Supplier")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCBiddingDt")

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
        Dim Dr As DataRow
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
            tbTop.Focus()
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
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbTop.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProduct.Text = ""
            tbProductName.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbPrice.Text = "0"
            tbLeadTime.Text = "0"
            ddlSelect.SelectedValue = "N"
            
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
            newTrans()
            EnableHd(True)
            ViewState("StateHd") = "Insert"
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Function GetPriceSupp(ByVal Product As String, ByVal Unit As String, ByVal Supplier As String, ByVal Currency As String, ByVal effectivedate As DateTime) As String
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_FindPriceSupplier " + QuotedStr(Supplier) + "," + QuotedStr(Currency) + ", " + QuotedStr(Product) + ", " + QuotedStr(Unit) + "," + QuotedStr(Format(effectivedate, "yyyy-MM-dd")), ViewState("DBConnection"))
        dr.Read()
        Return FormatNumber(dr("Price"), ViewState("DigitCurr"))
        dr.Close()
    End Function

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
            If ddlType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Type must have value")
                ddlType.Focus()
                Return False
            End If
            If CInt(tbTop.Text) = "0" Then
                lbStatus.Text = "Top Record must have value"
                tbTop.Focus()
                Return False
            End If
            If Len(tbRemark.Text) > 255 Then
                lbStatus.Text = MessageDlg("Remark max length 255")
                tbRemark.Focus()
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
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    Return False
                End If
                If Dr("Supplier").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Supplier Code Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                If Dr("Price").ToString.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
                If Dr("Term").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Term Must Have Value")
                    Return False
                End If
                'Dilepas karena di Supplier Quotation lead time dilepas
                'If Dr("DeliveryTime").ToString.Trim = "0" Then
                'lbStatus.Text = MessageDlg("Delivery Time Must Have Value")
                'Return False
                'End If
                If Dr("Selected").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Selected Must Have Value")
                    Return False
                End If
            Else
                If tbProduct.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    tbProduct.Focus()
                    Return False
                End If
                If tbSuppCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Supplier Code Must Have Value")
                    tbSuppCode.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If CFloat(tbPrice.Text) = "0" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    tbPrice.Focus()
                    Return False
                End If
                If ddlTerm.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Term Must Have Value")
                    ddlTerm.Focus()
                    Return False
                End If
                'Dilepas karena di Supplier Quotation lead time dilepas
                'If CInt(tbLeadTime.Text) = "0" Then
                '    lbStatus.Text = MessageDlg("Lead Time Must Have Value")
                'tbLeadTime.Focus()
                'Return False
                'End If
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
            ViewState("DBConnection") = ViewState("DBConnection")
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Type, Top Record, Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), Type, TopRecord, Remark"
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
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        ViewState("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PRFormBidding ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPRBidding.frx"
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
                btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product+'|'+Supplier+'|'+Unit = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(3).Text + "|" + GVR.Cells(5).Text))
        dr(0).Delete()
        ' ViewState("Dt").AcceptChanges()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text + "|" + GVR.Cells(3).Text + "|" + GVR.Cells(5).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(3).Text + "|" + GVR.Cells(5).Text
            tbProduct.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)

            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlType, Dt.Rows(0)("Type").ToString)
            BindToText(tbTop, Dt.Rows(0)("TopRecord").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product+'|'+Supplier+'|'+Unit = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbProduct, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbSuppCode, Dr(0)("Supplier").ToString)
                BindToText(tbSuppName, Dr(0)("Supplier_Name").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                'BindToText(tbPrice, FormatNumber(Dr(0)("Price").ToString), ViewState("DigitCurr"))
                BindToText(tbPrice, Dr(0)("Price").ToString)
                BindToText(tbPrice0, Dr(0)("MOQ").ToString)
                BindToDropList(ddlTerm, Dr(0)("Term").ToString)
                BindToText(tbLeadTime, Dr(0)("DeliveryTime").ToString)
                BindToDropList(ddlSelect, Dr(0)("Selected").ToString)
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

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("DBConnection") = ViewState("DBConnection")
            Session("Result") = Nothing
            Session("Filter") = "SELECT DISTINCT Product_Code, Product_Name, Supplier_Code, Supplier_Name, Unit, Price, Term_Code, Term_Name, LeadTime, MOQ FROM V_PRBiddingGetDt WHERE Start_Date <= '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'"
            ResultField = "Product_Code, Product_Name, Supplier_Code, Supplier_Name, Unit, Price, Term_Code, LeadTime, MOQ"
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
            ViewState("DBConnection") = ViewState("DBConnection")
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
                If ViewState("DtValue") <> tbProduct.Text + "|" + tbSuppCode.Text + "|" + ddlUnit.SelectedValue Then
                    If CekExistData(ViewState("Dt"), "Product,Supplier,Unit", tbProduct.Text + "|" + tbSuppCode.Text + "|" + ddlUnit.SelectedValue) Then
                        lbStatus.Text = "Product '" + tbProductName.Text + "' Supplier '" + tbSuppCode.Text + "' Unit '" + ddlUnit.SelectedValue + "' has already exists"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("Product+'|'+Supplier+'|'+Unit = " + QuotedStr(ViewState("DtValue")))(0)

                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProduct.Text
                Row("Product_Name") = tbProductName.Text
                Row("Supplier") = tbSuppCode.Text
                Row("Supplier_Name") = tbSuppName.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("MOQ") = FormatFloat(tbPrice0.Text, ViewState("DigitQty"))
                Row("Price") = tbPrice.Text
                Row("Term") = ddlTerm.SelectedValue
                Row("DeliveryTime") = tbLeadTime.Text
                Row("Selected") = ddlSelect.SelectedValue
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product,Supplier,Unit", tbProduct.Text + "|" + tbSuppCode.Text + "|" + ddlUnit.SelectedValue) Then
                    lbStatus.Text = "Product '" + tbProductName.Text + "' Supplier '" + tbSuppCode.Text + "' Unit '" + ddlUnit.SelectedValue + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProduct.Text
                dr("Product_Name") = tbProductName.Text
                dr("Supplier") = tbSuppCode.Text
                dr("Supplier_Name") = tbSuppName.Text
                dr("Unit") = ddlUnit.SelectedValue
                dr("MOQ") = FormatFloat(tbPrice0.Text, ViewState("DigitQty"))
                dr("Price") = tbPrice.Text
                dr("Term") = ddlTerm.SelectedValue
                dr("DeliveryTime") = tbLeadTime.Text
                dr("Selected") = ddlSelect.SelectedValue
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'CountSelect()
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
        Dim Ds As DataSet
        Dim Dr As DataRow
        Try
            Ds = SQLExecuteQuery("SELECT Product_Code, Product_Name, Supplier_Code, Supplier_Name, Unit, Price, Term_Code, LeadTime, MOQ FROM V_PRBiddingGetDt WHERE Product_Code = '" + tbProduct.Text + "' AND Start_Date <= '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' ", ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbProduct.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                BindToText(tbSuppCode, Dr("Supplier_Code"))
                BindToText(tbSuppName, Dr("Supplier_Name"))
                BindToDropList(ddlUnit, Dr("Unit"))
                'BindToText(tbPrice, FormatNumber(Dr("Price")), ViewState("DigitCurr"))
                BindToText(tbPrice0, Dr("MOQ"))
                BindToText(tbPrice, Dr("Price"))
                BindToDropList(ddlTerm, Dr("Term_Code"))
                BindToText(tbLeadTime, Dr("LeadTime"))
                ddlSelect.SelectedValue = "N"
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbPrice.Text = "0"
                tbPrice0.Text = "0"
                tbLeadTime.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tb Product Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            tbRemark.Focus()
        Catch ex As Exception
            Throw New Exception("tb supplier Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProduct.Click
        Dim ResultField As String
        Try
            ViewState("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT DISTINCT Product_Code, Product_Name, Supplier_Code, Supplier_Name, Unit, Price, Term_Code, LeadTime, MOQ FROM V_PRBiddingGetDt WHERE Start_Date <= '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "'"
            ResultField = "Product_Code, Product_Name, Supplier_Code, Supplier_Name, Unit, Price, Term_Code, LeadTime, MOQ"
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
        AttachScript("setformat();", Page, Me.GetType())
    End Sub

    Private Sub CountSelect()
        Dim FgSelect As String
        Dim Dr As DataRow
        Dim drow As DataRow()
        Try
            drow = ViewState("Dt").Select("Selected = 'Y'")
            FgSelect = ""
            If drow.Length >= 1 Then
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        FgSelect = "N"
                    End If
                Next
            End If
            FgSelect = ddlSelect.SelectedValue
            Dr = ViewState("Dt").Select("Selected = 'Y'")(0)
            Dr.BeginEdit()
            Dr("Selected") = FgSelect.ToString
            Dr.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)
            ddlSelect.SelectedValue = FgSelect.ToString
        Catch ex As Exception
            Throw New Exception("CountSelect Error : " + ex.ToString)
        End Try
    End Sub
End Class
