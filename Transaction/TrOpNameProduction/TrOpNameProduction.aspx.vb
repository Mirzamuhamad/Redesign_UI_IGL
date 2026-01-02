Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System.Xml
Imports System.Data.OleDb
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrOpNameProduction_TrOpNameProduction
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT Distinct TransNmbr, Nmbr, TransDate, Status, Shift, ShiftName, OpnameType, Warehouse, Wrhs_Name, Remark FROM V_PROOpnameHd " 'Where OpnameType = " + QuotedStr(Request.QueryString("Menuparam").ToString)

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PROOpnameDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Private Function GetStringDtWO(ByVal Nmbr As String) As String
        Return "EXEC S_PDOpnameGetWODt " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String

        Try
            'If Request.QueryString("ContainerId").ToString = "TrOpNameProductionID" Then
            '    Labelmenu.Text = "Opname Production " + Request.QueryString("MenuParam").ToString
            'ElseIf Request.QueryString("ContainerId").ToString = "TrOpNameProductionTissueID" Then
            '    Labelmenu.Text = "Opname Production " + Request.QueryString("MenuParam").ToString
            'End If
            Labelmenu.Text = "Opname Production " + Request.QueryString("MenuParam").ToString

            If Not IsPostBack Then
                InitProperty()
                ViewState("SetGrade") = False
                FillCombo(ddlUnit, "Select unitcode, unitname From MsUnit", True, "unitcode", "unitname", ViewState("DBConnection"))
                FillCombo(ddlShift, "Select Shift, ShiftName From VMsShiftProduction", False, "Shift", "ShiftName", ViewState("DBConnection"))
                'FillCombo(ddlWarehouse, "select Wrhs_Code, Wrhs_Name from VMsWarehouse where Wrhs_Type='Production'", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
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
                    TbProductName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlUnit, Session("Result")(2).ToString)
                    'tbQtySystem.Text = Session("Result")(2).ToString
                    BindToDropList(ddlType, Session("Result")(3).ToString)
                End If

                If ViewState("Sender") = "btnProductFG" Then
                    tbProductCodeFG.Text = Session("Result")(0).ToString
                    TbProductNameFG.Text = Session("Result")(1).ToString
                    ViewState("FgSubled") = "P"
                    If ViewState("Menuparam") = "TS" Then
                        SQLString = "Exec S_PDOpnameProductionWOLast  " + QuotedStr(tbProductCode.Text) + "," + QuotedStr(tbProductCodeFG.Text) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
                        Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

                        If Dt.Rows.Count > 0 Then
                            Dr = Dt.Rows(0)
                            tbWONo.Text = Trim(Dr("WONo").ToString)
                        Else
                            tbWONo.Text = ""
                        End If
                        tbWONo.Focus()
                    End If
                End If

                If ViewState("Sender") = "btnWO" Then
                    tbWONo.Text = Session("Result")(0).ToString
                End If

                If ViewState("Sender") = "btnWOAll" Then
                    tbWONoAll.Text = Session("Result")(0).ToString
                    tbWONoAll_TextChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        If Not CekExistData(ViewState("Dt"), "Product", TrimStr(drResult("Product"))) Then
                            'Dim dr As DataRow
                            'Dim dre As SqlDataReader
                            Dr = ViewState("Dt").NewRow
                            Dr("Type") = drResult("Type")
                            Dr("Product") = drResult("Product")
                            Dr("Product_Name") = drResult("Product_Name")
                            Dr("Qty") = 0 'drResult("Qty")
                            Dr("Unit") = drResult("Unit")
                            Dr("Remark") = Dr("Remark")
                            Dr("WONo") = ""
                            If ViewState("menuparam") <> "TS" Then
                                Dr("ProductFG") = ""
                            End If

                            ViewState("Dt").Rows.Add(Dr)
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
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        'tbQtySystem.Attributes.Add("ReadOnly", "True")
        'tbQtyAdjust.Attributes.Add("ReadOnly", "True")
        If Request.QueryString("MenuParam").ToString = "PE" Then
            'If Request.QueryString("ContainerId").ToString = "TrOpNameProductionID" Then
            ViewState("Menuparam") = "PE"
            ViewState("StrKode") = "OP"
        ElseIf Request.QueryString("MenuParam").ToString = "TS" Then
            ViewState("Menuparam") = "TS"
            ViewState("StrKode") = "OPT"
        Else
            ViewState("Menuparam") = "PP"
            ViewState("StrKode") = "OPP"
        End If


        If ViewState("Menuparam") = "TS" Then
            lbltitik.Visible = True
            tbWONo.Visible = True
            lblWOno.Visible = True
            btnWO.Visible = True
            lbltitikFG.Visible = True
            tbProductCodeFG.Visible = True
            lblProductFG.Visible = True
            TbProductNameFG.Visible = True
            btnProductFG.Visible = True
            Label17.Visible = True
            GridDt.Columns(5).Visible = True
            GridDt.Columns(6).Visible = True
            GridDt.Columns(7).Visible = True
        Else
            lbltitik.Visible = False
            tbWONo.Visible = False
            lblWOno.Visible = False
            btnWO.Visible = False
            'lbltitikFG.Visible = False
            'tbProductCodeFG.Visible = False
            'lblProductFG.Visible = False
            'TbProductNameFG.Visible = False
            'btnProductFG.Visible = False
            'Label17.Visible = False
            'GridDt.Columns(5).Visible = False
            'GridDt.Columns(6).Visible = False
            'GridDt.Columns(7).Visible = False
        End If

        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'tbQtyActual.Attributes.Add("OnBlur", "hitung('');")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            End If
            GetStringHd1 = "SELECT Distinct TransNmbr, Nmbr, TransDate, Status, Shift, ShiftName, OpnameType, Warehouse, Wrhs_Name, Remark From V_PROOpnameHd WHERE OpnameType = " + QuotedStr(ViewState("Menuparam").ToString)

            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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
                    Result = ExecSPCommandGo(ActionValue, "S_PDOpnameProduction", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'tbRef.Enabled = State
            tbDate.Enabled = State
            'ddlShift.Enabled = State
            'ddlType.Enabled = State
            'ddlWarehouse.Enabled = State
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
            lbStatus.Text = ""
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
            
            If ViewState("Menuparam") = "TS" Then
                ViewState("StrKode") = "OPT"
            ElseIf ViewState("Menuparam") = "PE" Then
                ViewState("StrKode") = "OP"
            Else
                ViewState("StrKode") = "OPP"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                
                tbRef.Text = GetAutoNmbr(ViewState("StrKode").ToString, "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PROOpnameHd (TransNmbr, TransDate, Status, Shift, OpnameType, Remark, UserPrep, DatePrep) " + _
                            "SELECT '" + tbRef.Text + "', '" + _
                            Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', 'H', " + _
                            QuotedStr(ddlShift.SelectedValue) + ", " + _
                            QuotedStr(ViewState("Menuparam")) + ", '" + _
                            tbRemark.Text + "', " + _
                            QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PROOpnameHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PROOpnameHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                            " Shift = " + QuotedStr(ddlShift.SelectedValue) + _
                            ", Remark = " + QuotedStr(tbRemark.Text) + _
                            ", DateAppr = getDate() WHERE TransNmbr = '" + tbRef.Text + "'"
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Qty, Unit, Remark, FgSubLed, ProductFG, Type, WONo FROM PROOpnameDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PROOpnameDt SET Product = @Product, Qty = @Qty, Unit = @Unit, WONo = @WONo, Type = @Type, Remark = @Remark, FgSubLed = @FgSubLed, ProductFG = @ProductFG WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Produk", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@FgSubLed", SqlDbType.VarChar, 1, "FgSubLed")
            Update_Command.Parameters.Add("@ProductFG", SqlDbType.VarChar, 20, "ProductFG")
            Update_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            Update_Command.Parameters.Add("@Type", SqlDbType.VarChar, 20, "Type")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@Produk", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PROOpnameDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Product = @Produk", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Produk", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PROOpnameDt")

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
            ddlType_SelectedIndexChanged(Nothing, Nothing)
            'GridDt.Columns(1).Visible = False
            ViewState("PrevQuotation") = ""
            MultiView1.ActiveViewIndex = 0
            btnSaveAll.Visible = True
            btnSaveTrans.Visible = True
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            ddlShift.SelectedIndex = 0
            tbRemark.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt(Optional ByVal send As String = "")
        Try
            If send <> "ddlType" Or send = "" Then
                ddlType.SelectedIndex = 0
            End If
            tbProductCode.Text = ""
            TbProductName.Text = ""

            tbProductCodeFG.Text = ""
            TbProductNameFG.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedIndex = 0
            tbRemarkDt.Text = ""
            tbWONo.Text = ""
            ViewState("FgSubled") = "N"

            If ViewState("Menuparam") <> "TS" Then
                tbProductCodeFG.Enabled = ddlType.SelectedValue = "WIP"
                btnProductFG.Visible = ddlType.SelectedValue = "WIP"
                'tbWONo.Enabled = False
                btnWO.Visible = False 'Not (ddlType.SelectedIndex = 0 Or ddlType.SelectedIndex = 2)
            Else
                tbProductCodeFG.Enabled = True
                btnProductFG.Visible = True
                'tbWONo.Enabled = False
                'btnWO.Visible = True
                If ddlType.SelectedValue = "FG" Then
                    lblWOno.Visible = False
                    lbltitik.Visible = False
                    tbWONo.Visible = False
                    btnWO.Visible = False
                Else
                    lblWOno.Visible = True
                    lbltitik.Visible = True
                    tbWONo.Visible = True
                    btnWO.Visible = True
                End If
            End If
            MultiView1.ActiveViewIndex = 0
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

        'If ddlType.SelectedIndex = 0 Then
        '    tbProductCodeFG.Enabled = True
        '    btnProductFG.Enabled = True
        'Else
        '    tbProductCodeFG.Enabled = False
        '    btnProductFG.Enabled = False
        'End If

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
            'If ViewState("Menuparam") <> "PE" Then
            '    If tbWONo.Text = "" Then
            '        lbStatus.Text = MessageDlg("WO No must have value")
            '        tbWONo.Focus()
            '        Return False
            '    End If
            'End If
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
                If Dr("Type").ToString = "WIP" Then
                    If Dr("ProductFG").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("ProductFG Must Have Value")
                        Return False
                    End If
                End If
                If ViewState("Menuparam") = "TS" And ddlType.SelectedValue = "WIP" Then
                    If Dr("WONo").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("WO No Must Have Value")
                        tbQty.Focus()
                        Return False
                    End If
                End If
                If Dr("Qty").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty System Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If ddlType.SelectedValue = "WIP" Then
                    If tbProductCodeFG.Text.Trim = "" Then '
                        lbStatus.Text = MessageDlg("ProductFG Must Have Value")
                        tbProductCodeFG.Focus()
                        Return False
                    End If
                End If
                If ViewState("Menuparam") = "TS" And ddlType.SelectedValue = "WIP" Then
                    If tbWONo.Text = "" Then
                        lbStatus.Text = MessageDlg("WO No Must Have Value")
                        tbQty.Focus()
                        Return False
                    End If
                End If
                If tbQty.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    ddlUnit.Focus()
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
                        'If ddlType.SelectedIndex = 0 Then
                        '    ddlWarehouse.SelectedIndex = 0
                        '    ddlWarehouse.Enabled = False
                        'End If
                        'ddlType_SelectedIndexChanged(Nothing, Nothing)
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Copy New" Then
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("StateHd") = "Insert"
                    ViewState("DigitCurr") = 0

                    MovePanel(PnlHd, pnlInput)
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = False
                    GridDt.PageIndex = 0

                    CopyDataDt(ViewState("Reference"))
                    'BindDataDt(ViewState("Reference"))

                    FillTextBoxHd(ViewState("Reference"))
                    GridDt.Columns(1).Visible = True
                    tbRef.Text = ""

                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    EnableHd(True)
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
            MultiView1.ActiveViewIndex = 0
            btnSaveAll.Visible = True
            btnSaveTrans.Visible = True
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

            'ViewState("DtValue") = GVR.Cells(3).Text
            If ViewState("Menuparam") = "TS" Then
                ViewState("DtValue") = TrimStr(GVR.Cells(3).Text) + "|" + TrimStr(GVR.Cells(5).Text) + "|" + TrimStr(GVR.Cells(7).Text)
            Else : ViewState("DtValue") = TrimStr(GVR.Cells(3).Text)
            End If
            'dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(4).Text))
            If ViewState("Menuparam") = "TS" Then
                dr = ViewState("Dt").Select("Product+'|'+ProductFG+'|'+WONo = " + QuotedStr(ViewState("DtValue")))
            Else : dr = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))
            End If

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
            'ViewState("DtValue") = GVR.Cells(3).Text
            If ViewState("Menuparam") = "TS" Then
                ViewState("DtValue") = TrimStr(GVR.Cells(3).Text) + "|" + TrimStr(GVR.Cells(5).Text) + "|" + TrimStr(GVR.Cells(7).Text)
            Else : ViewState("DtValue") = TrimStr(GVR.Cells(3).Text)
            End If

            FillTextBoxDt(ViewState("DtValue"), TrimStr(GVR.Cells(2).Text))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
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
            ViewState("Menuparam") = Dt.Rows(0)("OpnameType").ToString
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlShift, Dt.Rows(0)("Shift").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String, ByVal Type As String)
        Dim Dr As DataRow()
        Try
            If ViewState("Menuparam") = "TS" Then
                Dr = ViewState("Dt").select("Product+'|'+ProductFG+'|'+WONo = " + QuotedStr(Product))
            Else : Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            End If

            If Dr.Length > 0 Then
                BindToText(tbWONo, Dr(0)("WONo").ToString)
                BindToDropList(ddlType, Dr(0)("Type").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(TbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbProductCodeFG, Dr(0)("ProductFG").ToString)
                BindToText(TbProductNameFG, Dr(0)("ProductName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()

            If ViewState("Menuparam") <> "TS" Then
                tbProductCodeFG.Enabled = ddlType.SelectedValue = "WIP"
                btnProductFG.Visible = ddlType.SelectedValue = "WIP"
                'tbWONo.Enabled = False
                btnWO.Visible = False 'ddlType.SelectedIndex = 1
            Else
                tbProductCodeFG.Enabled = True
                btnProductFG.Visible = True
                'tbWONo.Enabled = False
                btnWO.Visible = True
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(tbProductCode.Text) + "|" + TrimStr(tbProductCodeFG.Text) + "|" + TrimStr(tbWONo.Text) Then
                    If CekExistData(ViewState("Dt"), "Product,ProductFG,WONo", TrimStr(tbProductCode.Text) + "|" + TrimStr(tbProductCodeFG.Text) + "|" + TrimStr(tbWONo.Text)) Then
                        lbStatus.Text = "Data has already exists"
                        Exit Sub
                    End If
                End If

                'Row = ViewState("Dt").Select("Product+'|'+ProductFG+'|'+WONo = " + QuotedStr(ViewState("DtValue")))(0)
                If ViewState("Menuparam") = "TS" Then
                    Row = ViewState("Dt").Select("Product+'|'+ProductFG+'|'+WONo = " + QuotedStr(ViewState("DtValue")))(0)
                Else : Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                End If

                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()

                Row("Type") = ddlType.SelectedValue
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = TbProductName.Text
                Row("ProductFG") = tbProductCodeFG.Text
                Row("ProductName") = TbProductNameFG.Text
                Row("WONo") = tbWONo.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = ddlUnit.SelectedValue
                Row("FgSubled") = ViewState("FgSubled")
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Product", TrimStr(tbProductCode.Text)) Then
                    lbStatus.Text = "Data has already exists"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Type") = ddlType.SelectedValue
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = TbProductName.Text
                dr("ProductFG") = tbProductCodeFG.Text
                dr("ProductName") = TbProductNameFG.Text
                dr("WONo") = tbWONo.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = ddlUnit.SelectedValue
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

    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbProduct_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        'If ddlType.SelectedIndex = 0 Then
        '    ddlWarehouse.SelectedIndex = 0
        '    ddlWarehouse.Enabled = False
        'Else
        '    ddlWarehouse.SelectedIndex = 0
        '    ddlWarehouse.Enabled = True
        'End If

        If ViewState("Menuparam") <> "TS" Then
            tbProductCodeFG.Enabled = ddlType.SelectedValue = "WIP"
            btnProductFG.Visible = ddlType.SelectedValue = "WIP"
            'tbWONo.Enabled = False
            btnWO.Visible = False  'ddlType.SelectedIndex = 1
        Else
            tbProductCodeFG.Enabled = True
            btnProductFG.Visible = True
            'tbWONo.Enabled = False
            If ddlType.SelectedValue = "FG" Then
                lblWOno.Visible = False
                lbltitik.Visible = False
                tbWONo.Visible = False
                btnWO.Visible = False
            Else
                lblWOno.Visible = True
                lbltitik.Visible = True
                tbWONo.Visible = True
                btnWO.Visible = True
            End If
            'btnWO.Visible = True
        End If

        Cleardt("ddlType")
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable

        Try
            'Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)

            DT = SQLExecuteQuery("Exec S_PDOpnameGetDt '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlShift.SelectedValue) + "," + QuotedStr(ddlType.SelectedValue) + "," + QuotedStr(ViewState("Menuparam")) + "," + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                ddlType.SelectedValue = Dr("Type")
                tbProductCode.Text = Dr("Product")
                TbProductName.Text = Dr("Product_Name")
                tbQty.Text = "0"
                BindToDropList(ddlUnit, Dr("Unit").ToString)
            Else
                ddlType.SelectedIndex = 0
                tbProductCode.Text = ""
                TbProductName.Text = ""
                tbQty.Text = "0"
                ddlUnit.SelectedIndex = 0
            End If
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "select * from V_MsBuild"
            'Session("filter") = "select * from VMsProduct"            
            Session("filter") = "Exec S_PDOpnameGetDt '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlShift.SelectedValue) + ", " + QuotedStr(ddlType.SelectedValue) + "," + QuotedStr(ViewState("Menuparam")) + ",''"
            ResultField = "Product, Product_Name, Unit, Type"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProduct_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If ViewState("StateHd") = "View" Then
                Exit Sub
            End If
            'DIREMARK DLU
            'If ViewState("Menuparam") <> "TS" Then
            '    If CekHd() = False Then
            '        Exit Sub
            '    End If
            '    Session("Result") = Nothing
            '    Session("Filter") = "Exec S_PDOpnameGetDt '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlShift.SelectedValue) + ", '','',''"
            '    ResultField = "Type, Product, Product_Name, Unit"
            '    Session("Column") = ResultField.Split(",")
            '    ViewState("Sender") = "btnGetDt"
            '    Session("DBConnection") = ViewState("DBConnection")
            '    AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'Else
            'tbWONoAll.Text = ""
            'lbCode.Text = ""
            'LbName.Text = ""
            'lbProductFG.Text = ""
            MultiView1.ActiveViewIndex = 1
            Menu1.Items.Item(1).Selected = True
            BindDataWO("")
            PnlWOgetdata.Visible = False
            StatusButtonSave(False)
            'End If

            tbWONoAll.Text = ""
            lbCode.Text = ""
            LbName.Text = ""
            LbSpec.Text = ""
            lbProductFG.Text = ""

        Catch ex As Exception
            lbStatus.Text = "btnGetDt_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProductFG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductFG.Click
        Dim ResultField As String

        Try
            If ViewState("Menuparam") <> "TS" Then
                Session("filter") = "Select DISTINCT A.Product AS Product_Code, P.ProductName AS Product_Name, P.Specification, P.Unit " + _
                    "from PROBOMHd A INNER JOIN PROBOMProcess B ON A.TransNmbr = B.TransNmbr " + _
                    "INNER JOIN MsProduct P ON A.Product = P.ProductCode " + _
                    "WHERE A.Status = 'P' AND B.OutputType = 'WIP' AND P.WorkCtr = " + QuotedStr(ViewState("Menuparam")) + " AND B.ProductOutput = " + QuotedStr(tbProductCode.Text)
            Else
                Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' And ProductCategory='Finish Good'"
            End If

            'Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' And ProductCategory='Finish Good'"
            ResultField = "Product_Code, Product_Name, Unit, Specification"

            ViewState("Sender") = "btnProductFG"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProductFG_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCodeFG_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCodeFG.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String

        Try
            If ViewState("Menuparam") <> "TS" Then
                SQLString = "Select DISTINCT A.Product AS ProductCode, P.ProductName AS ProductName, P.Specification, P.Unit " + _
                    "from PROBOMHd A INNER JOIN PROBOMProcess B ON A.TransNmbr = B.TransNmbr " + _
                    "INNER JOIN MsProduct P ON A.Product = P.ProductCode " + _
                    "WHERE A.Status = 'P' AND B.OutputType = 'WIP' AND P.WorkCtr = 'PE' AND B.ProductOutput = " + QuotedStr(tbProductCode.Text) + " AND A.Product = " + QuotedStr(tbProductCodeFG.Text)
            Else
                SQLString = "SELECT Product_Code AS ProductCode, Product_Name AS ProductName, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' And ProductCategory='Finish Good' AND Product_Code = " + QuotedStr(tbProductCodeFG.Text)
            End If
            
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProductCodeFG, Trim(Dr("ProductCode").ToString))
                BindToText(TbProductNameFG, Trim(Dr("ProductName").ToString))
                ViewState("FgSubled") = "P"
            Else
                tbProductCodeFG.Text = ""
                TbProductNameFG.Text = ""
                ViewState("FgSubled") = "N"
            End If
            'tbProductCodeFG.Focus()

            Dt = Nothing
            Dr = Nothing

            '================
            SQLString = "Exec S_PDOpnameProductionWOLast  " + QuotedStr(tbProductCode.Text) + "," + QuotedStr(tbProductCodeFG.Text) + "," + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbWONo.Text = Trim(Dr("WONo").ToString)
            Else
                tbWONo.Text = ""
            End If
            tbWONo.Focus()
        Catch ex As Exception
            Throw New Exception("tbProductCodeFG_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelWO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelWO.Click
        Try
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            PnlWOgetdata.Visible = True
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btnCancelWO_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbWONoAll_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWONoAll.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable

        Try
            DT = SQLExecuteQuery("Exec S_PDOpnameFindWO '" + tbWONoAll.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbWONoAll.Text = Dr("WONo")
                lbProductFG.Text = Dr("WODate")
                lbCode.Text = Dr("ProductFG")
                LbName.Text = Dr("ProductFG_Name")
                LbSpec.Text = Dr("Specification")
                BindDataWO(tbWONoAll.Text)
            End If
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataWO(ByVal Referens As String)
        Try
            Dim dtWO As New DataTable
            ViewState("DtWO") = Nothing
            dtWO = SQLExecuteQuery(GetStringDtWO(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtWO") = dtWO
            BindGridDt(dtWO, GridExcel)
            GridExcel.Columns(0).Visible = True
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub saveWO()
        Dim Row As DataRow
        Dim Dr As DataRow
        Try
            Dim lbType, lbProduct, lbProductName, lbUnit, lbProductFGName As Label
            Dim tbQty As TextBox
            Dim QtyFG As Double
            Dim GVR As GridViewRow
            Dim i As Integer
            Dim exe As Boolean
            Try
                exe = True
                QtyFG = 0
                

                For i = 0 To GridExcel.Rows.Count - 1

                    GVR = GridExcel.Rows(i)
                    lbType = GVR.FindControl("Type")
                    lbProduct = GVR.FindControl("Product")
                    lbProductName = GVR.FindControl("ProductName")
                    lbProductFG = GVR.FindControl("ProductFG")
                    lbProductFGName = GVR.FindControl("ProductFGName")
                    tbQty = GVR.FindControl("Qty")
                    lbUnit = GVR.FindControl("Unit")

                    tbQty.Text = tbQty.Text.Replace(",", "")
                    If tbQty.Text.Trim = "" Then
                        tbQty.Text = "0"
                    End If

                    If Not IsNumeric(tbQty.Text) Then
                        lbStatus.Text = "Qty " + tbQty.Text + " must in numeric format"
                        exe = False
                        tbQty.Focus()
                        Exit For
                    End If

                Next
                If exe Then
                    ' simpan ke GridDT
                    For i = 0 To GridExcel.Rows.Count - 1
                        GVR = GridExcel.Rows(i)

                        lbType = GVR.FindControl("Type")
                        lbProduct = GVR.FindControl("Product")
                        lbProductName = GVR.FindControl("Product_Name")
                        tbQty = GVR.FindControl("Qty")
                        lbUnit = GVR.FindControl("Unit")
                        lbProductFG = GVR.FindControl("ProductFG")
                        lbProductFGName = GVR.FindControl("ProductFG_Name")
                        tbQty.Text = tbQty.Text.Replace(",", "")

                        If Not CekExistData(ViewState("Dt"), "WONo = " + QuotedStr(tbWONoAll.Text) + " And Product", TrimStr(lbProduct.Text)) Then
                            If tbQty.Text <> "0" Then
                                Dr = ViewState("Dt").NewRow
                                Dr("WONo") = tbWONoAll.Text
                                Dr("Type") = lbType.Text
                                Dr("Product") = lbProduct.Text
                                Dr("Product_Name") = lbProductName.Text
                                Dr("ProductFG") = lbProductFG.Text
                                Dr("ProductName") = lbProductFGName.Text
                                Dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                                Dr("Unit") = lbUnit.Text
                                ViewState("Dt").Rows.Add(Dr)
                            End If
                        Else
                            Row = ViewState("Dt").Select("WONo = " + QuotedStr(tbWONoAll.Text) + " And Product = " + QuotedStr(lbProduct.Text))(0)
                            If tbQty.Text <> "0" Then
                                Row.BeginEdit()
                                Row("WONo") = tbWONoAll.Text
                                Row("Type") = lbType.Text
                                Row("Product") = lbProduct.Text
                                Row("Product_Name") = lbProductName.Text
                                Row("ProductFG") = lbProductFG.Text
                                Row("ProductName") = lbProductFGName.Text
                                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                                Row("Unit") = lbUnit.Text
                                Row.EndEdit()
                            End If
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = True
                End If
            Catch ex As Exception
                lbStatus.Text = " Save WO error : " + ex.ToString
            End Try
        Catch ex As Exception
            lbStatus.Text = "Save WO Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            saveWO()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            PnlWOgetdata.Visible = True
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btnOK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub btnWO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWO.Click
        Dim ResultField As String
        Try
            'Session("filter") = "SELECT DISTINCT WONo, WODate, ProductFG FROM VPDOpnameProductionWO Where ProductFG = " + QuotedStr(tbProductCodeFG.Text.Trim)
            'lbStatus.Text = Session("filter")
            'Exit Sub
            Session("filter") = "SELECT DISTINCT WONo, WODate, ProductFG FROM VPDOpnameProductionWO Where ProductFG = " + QuotedStr(tbProductCodeFG.Text.Trim) + " AND WorkCtr = " + QuotedStr(ViewState("Menuparam"))
            ResultField = "WONo"

            ViewState("Sender") = "btnWO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProductFG_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnWOAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOAll.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT WONo, WODate FROM VPDOpnameProductionWO "
            ResultField = "WONo"

            ViewState("Sender") = "btnWOAll"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProductFG_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub CopyDataDt(ByVal Nmbr As String)
        Try
            Dim dt, dtSource As New DataTable
            dtSource = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)

            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(""), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt

            For Each R In dtSource.Rows
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Type") = R("Type")
                dr("Product") = R("Product")
                dr("Product_Name") = R("Product_Name")
                dr("ProductFG") = R("ProductFG")
                dr("ProductName") = R("ProductName")
                dr("WONo") = R("WONo")
                dr("Qty") = R("Qty")
                dr("Unit") = R("Unit")
                dr("FgSubled") = R("FgSubled")
                dr("Remark") = R("Remark")
                ViewState("Dt").Rows.Add(dr)
            Next
            'GridDt.Columns(6).Visible = ViewState("Status") = "P"
            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            MultiView1.ActiveViewIndex = 2
            btnSaveAll.Visible = False
            btnSaveTrans.Visible = False
            ddlSheets.Items.Clear()
            ddlFindProductCode.Items.Clear()
            ddlFindType.Items.Clear()
            ddlFindQty.Items.Clear()
            ddlFindRemark.Items.Clear()
            ddlFindUnit.Items.Clear()
            GridView2.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub GetExcelSheets(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03 
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                Exit Select
            Case ".xlsx"
                'Excel 07 
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        'Get the Sheets in Excel WorkBoo 
        conStr = String.Format(conStr, FilePath, isHDR)
        'lbStatus.Text = conStr
        'Exit Sub
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        cmdExcel.Connection = connExcel
        If connExcel.State = ConnectionState.Closed Then
            connExcel.Open()
        End If
        'Bind the Sheets to DropDownList 
        ddlSheets.Items.Clear()
        'ddlSheets.Items.Add(New ListItem("--Select Sheet--", ""))
        ddlSheets.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        ddlSheets.DataTextField = "TABLE_NAME"
        ddlSheets.DataValueField = "TABLE_NAME"
        ddlSheets.DataBind()
        connExcel.Close()

        ddlSheets.SelectedIndex = 0
        DataExcel(ddlSheets.SelectedValue, FilePath, Extension, isHDR)
        'txtTable.Text = ""
        'lblFileName.Text = Path.GetFileName(FilePath)
        'Dim query As String = "SELECT [Nik],[Nama],[datelog], [timelog] FROM [" + ddlSheets.SelectedValue + "]"
        'Dim conn As New OleDbConnection(conStr)
        'If conn.State = ConnectionState.Closed Then
        '    conn.Open()
        'End If
        'Dim cmd As New OleDbCommand(query, conn)
        'Dim da As New OleDbDataAdapter(cmd)
        'Dim ds As New DataSet()
        'da.Fill(ds)
        'GridView1.DataSource = ds.Tables(0)
        'GridView1.DataBind()
        'da.Dispose()
        'conn.Close()
        'conn.Dispose()
    End Sub
    Private Sub DataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-03 
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    Exit Select
                Case ".xlsx"
                    'Excel 07 
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            GridView2.DataSource = ds.Tables(0)
            GridView2.DataBind()

            Dim count As Integer
            count = ds.Tables(0).Columns.Count - 1
            ddlFindProductCode.Items.Clear()
            ddlFindProductCode.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindType.Items.Clear()
            ddlFindType.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindQty.Items.Clear()
            ddlFindQty.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindRemark.Items.Clear()
            ddlFindRemark.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindUnit.Items.Clear()
            ddlFindUnit.Items.Add(New ListItem("--Choose One--", ""))
            For j = 0 To count
                ddlFindType.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindProductCode.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindQty.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindUnit.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindRemark.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))

            Next
            da.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            lbStatus.Text = "DataExcel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUploadExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadExcel.Click
        If fileuploadExcel.HasFile Then
            Dim FileName As String = Path.GetFileName(fileuploadExcel.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fileuploadExcel.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            Dim FilePath As String = Server.MapPath("~/ExcelHSE/" + FileName)
            fileuploadExcel.SaveAs(FilePath)
            ViewState("FilePath") = FilePath
            ViewState("Extension") = Extension
            GetExcelSheets(FilePath, Extension, "Yes")
            GridView2.Visible = True
            
        End If
    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            ' call sp S_STOpnameTempDelete
            SQLExecuteNonQuery("EXEC S_PROOpnameTempDelete " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlShift.SelectedValue), ViewState("DBConnection").ToString)
            importtoTemp()
            importsave()

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            GridDt.Columns(1).Visible = True
            'PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            btnSaveAll.Visible = True
            btnSaveTrans.Visible = True
            'BindDataDt(tbCode.Text)
        Catch ex As Exception
            lbStatus.Text = "btnGenerate_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importtoTemp()
        Try
            Dim GVR As GridViewRow
            Dim SQLstring As String
            If (ddlFindProductCode.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Product Code must have value")
                ddlFindProductCode.Focus()
                Exit Sub
            End If
            'If (ddlFindProductFG.SelectedValue = "") Then
            '    lbStatus.Text = MessageDlg("Product FG must have value")
            '    ddlFindProductFG.Focus()
            '    Exit Sub
            'End If
            If (ddlFindQty.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Qty must have value")
                ddlFindQty.Focus()
                Exit Sub
            End If
            'If (ddlFindWONo.SelectedValue = "") Then
            '    lbStatus.Text = MessageDlg("WO No must have value")
            '    ddlFindWONo.Focus()
            '    Exit Sub
            'End If

            Dim IdFindProductCode, IdFindType, IdFindQty, IdFindUnit, IdFindRemark As Integer
            Dim VarFindProductCode, VarFindType, VarFindQty, VarFindUnit, VarFindRemark As String
            IdFindProductCode = ddlFindProductCode.SelectedIndex - 1
            IdFindType = ddlFindType.SelectedIndex - 1
            IdFindQty = ddlFindQty.SelectedIndex - 1
            IdFindUnit = ddlFindUnit.SelectedIndex - 1
            IdFindRemark = ddlFindRemark.SelectedIndex - 1
            
            For Each GVR In GridView2.Rows
                If IdFindType < 0 Then
                    VarFindType = ""
                Else
                    VarFindType = GVR.Cells(IdFindType).Text
                    ' ddlLocation_SelectedIndexChanged(Nothing, Nothing)                    
                End If

                If IdFindProductCode < 0 Then
                    VarFindProductCode = ""
                Else
                    VarFindProductCode = GVR.Cells(IdFindProductCode).Text
                    ' tbCode_TextChanged(Nothing, Nothing)
                End If
                'lbStatus.Text = VarFindItem
                'Exit Sub
                If IdFindQty <= 0 Then
                    VarFindQty = "0"
                Else
                    VarFindQty = GVR.Cells(IdFindQty).Text
                    If GVR.Cells(IdFindQty).Text = "" Then
                        VarFindQty = "0"
                    End If
                    ' tbQtyActual_TextChanged(Nothing, Nothing)
                End If

                If IdFindRemark < 0 Then
                    VarFindRemark = ""
                Else
                    VarFindRemark = GVR.Cells(IdFindRemark).Text
                End If

                If IdFindUnit < 0 Then
                    VarFindUnit = ""
                Else
                    VarFindUnit = GVR.Cells(IdFindUnit).Text
                    ' tbQtyActual_TextChanged(Nothing, Nothing)
                End If

                VarFindType = TrimStr(VarFindType)
                VarFindProductCode = TrimStr(VarFindProductCode)
                VarFindQty = TrimStr(VarFindQty)
                VarFindUnit = TrimStr(VarFindUnit)
                VarFindRemark = TrimStr(VarFindRemark)
                If VarFindQty.Length = 0 Then
                    VarFindQty = "0"
                End If
                If VarFindProductCode.Length > 0 Then
                    SQLstring = "EXEc S_PROOpnameTempImportDt " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlShift.SelectedValue) + ", " + _
                    QuotedStr(VarFindProductCode) + ", " + QuotedStr(VarFindType) + ", " + VarFindQty.Replace(",", "") + ", " + QuotedStr(VarFindUnit) + ", " + QuotedStr(VarFindRemark)
                    SQLExecuteNonQuery(SQLstring, ViewState("DBConnection"))
                End If
            Next
        Catch ex As Exception
            lbStatus.Text = "importtoTemp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importsave()
        Dim DrProduct, Row, Dr As DataRow
        Dim DtProduct As DataTable

        Try
            DtProduct = SQLExecuteQuery("EXEC S_PROOpnameTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlShift.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            'lbStatus.Text = "EXEC S_STOpnameTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim)
            'Exit Sub

            For Each DrProduct In DtProduct.Rows
                If CekExistData(ViewState("Dt"), "Product", TrimStr(DrProduct("Product_Code"))) Then
                    Row = ViewState("Dt").Select("Product = " + QuotedStr(TrimStr(DrProduct("Product_Code"))))(0)
                    Row.BeginEdit()
                    Row("Type") = DrProduct("Type")
                    Row("Product") = DrProduct("Product_Code")
                    Row("Product_Name") = DrProduct("Product_Name")
                    Row("Unit") = DrProduct("Unit")
                    Row("Remark") = DrProduct("Remark")
                    Row("Qty") = FormatFloat(DrProduct("Qty"), ViewState("DigitQty"))
                    Row.EndEdit()
                Else
                    Dr = ViewState("Dt").NewRow
                    Dr("Type") = DrProduct("Type")
                    Dr("Product") = DrProduct("Product_Code")
                    Dr("Product_Name") = DrProduct("Product_Name")
                    Dr("Unit") = DrProduct("Unit")
                    Dr("Remark") = DrProduct("Remark")
                    Dr("Qty") = FormatFloat(DrProduct("Qty"), ViewState("DigitQty"))
                    ViewState("Dt").Rows.Add(Dr)
                End If
            Next
            '    'BindGridDt(ViewState("Dt"), GridDt)
            '    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            '    'GridDt.Columns(1).Visible = True
            '    'PnlInfo.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

        Catch ex As Exception
            lbStatus.Text = "importsave Error : " + ex.ToString
        End Try
    End Sub
End Class
