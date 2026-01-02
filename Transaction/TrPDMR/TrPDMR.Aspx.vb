Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPDMR
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected EnableMRType As String
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "SELECT * FROM V_PROMRHd "
    End Function

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
                If Not Session("PostNmbr") = Nothing Then
                    tbFilter.Text = Session("PostNmbr")
                    btnSearch_Click(Nothing, Nothing)
                    Session("PostNmbr") = Nothing
                    tbFilter.Text = ""
                End If
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnReturNo" Then
                    tbReturNo.Text = Session("Result")(0).ToString
                    ddlWarehouse.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnWONo" Then
                    tbWONo.Text = TrimStr(Session("Result")(0).ToString)
                    ddlShift.SelectedValue = TrimStr(Session("Result")(1).ToString)
                    tbStartDate.SelectedDate = TrimStr(Session("Result")(2).ToString)
                    tbStartTime.Text = TrimStr(Session("Result")(3).ToString)
                    tbEndDate.SelectedDate = TrimStr(Session("Result")(4).ToString)
                    tbEndTime.Text = TrimStr(Session("Result")(5).ToString)
                    tbProductCode.Text = ""
                    tbProductName.Text = ""
                    tbQty.Text = "0"
                    lbUnit.Text = ""
                    tbBOMNo.Text = ""
                    'tbProductCode.Text = TrimStr(Session("Result")(6).ToString)
                    'tbProductName.Text = TrimStr(Session("Result")(7).ToString)
                    'tbQty.Text = TrimStr(Session("Result")(8).ToString)
                    'lbUnit.Text = TrimStr(Session("Result")(9).ToString)
                    'tbBOMNo.Text = TrimStr(Session("Result")(10).ToString)
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = TrimStr(Session("Result")(0).ToString)
                    tbProductName.Text = TrimStr(Session("Result")(1).ToString)
                    tbQty.Text = TrimStr(Session("Result")(2).ToString)
                    lbUnit.Text = TrimStr(Session("Result")(3).ToString)
                    tbBOMNo.Text = TrimStr(Session("Result")(4).ToString)
                End If
                If ViewState("Sender") = "btnMaterial" Then
                    tbMaterialCode.Text = TrimStr(Session("Result")(0).ToString)
                    tbMaterialName.Text = TrimStr(Session("Result")(1).ToString)
                    lbUnitDt2.Text = TrimStr(Session("Result")(2).ToString)
                    tbQtyDt2.Text = TrimStr(Session("Result")(3).ToString)
                    tbSpecMaterial.Text = TrimStr(Session("Result")(4).ToString)
                End If
                'If ViewState("Sender") = "btnGetDt" Then
                '    Dim drResult As DataRow
                '    For Each drResult In Session("Result").Rows
                '        Dim dr As DataRow
                '        dr = ViewState("Dt").NewRow
                '        dr("Product") = drResult("Product_Code")
                '        dr("ProductName") = drResult("Product_Name")
                '        dr("Specification") = TrimStr(drResult("Specification"))
                '        dr("Qty") = "0"
                '        dr("Unit") = drResult("Unit")
                '        dr("QtyReq") = "0"
                '        dr("UnitReq") = drResult("Unit_Order")
                '        dr("QtyIn") = drResult("Qty_Budget")
                '        dr("QtyOut") = drResult("Qty_Issue")
                '        dr("QtyBook") = drResult("Qty_Book")
                '        dr("QtyPO") = "0"
                '        dr("Remark") = ""
                '        ViewState("Dt").Rows.Add(dr)
                '    Next
                '    BindGridDt(ViewState("Dt"), GridDt)
                '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                'End If
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
        FillCombo(ddlWorkCtr, "SELECT WorkCtr_Code, WorkCtr_Name FROM VMsWorkCtr", True, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
        FillCombo(ddlWarehouse, "SELECT Wrhs_Code, Wrhs_Name FROM VMsWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
        FillCombo(ddlShift, "SELECT ShiftCode, ShiftName FROM MsShift WHERE FgActive = 'Y'", True, "ShiftCode", "ShiftName", ViewState("DBConnection"))
        
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            'ddlCommand.Items.Add("Print")
            'ddlCommand2.Items.Add("Print")
        End If
        tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            'If Len(StrFilter) = 0 Then
            '    StrFilter = " UserId = '" + ViewState("UserId").ToString + "'"
            'Else
            '    StrFilter = StrFilter + " AND UserId = '" + ViewState("UserId").ToString + "'"
            'End If
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
        Return "SELECT * From V_PROMRDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROMRDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
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
                Session("SelectCommand") = "EXEC S_PDFormBOM " + Result
                Session("ReportFile") = ".../../../Rpt/FormPDBOM.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PDMR", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlMRType.Enabled = State
            btnReturNo.Visible = State
            If EnableMRType = "Y" Then
                ddlMRType_SelectedIndexChanged(Nothing, Nothing)
            End If
            ddlWorkCtr.Enabled = State
            ddlWarehouse.Enabled = State
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

    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim dt As New DataTable
        Dim Drow As DataRow()
        Try
            ViewState("DtMaterial") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtMaterial") = dt

            Drow = ViewState("DtMaterial").Select("WONO+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text))
            'BindGridDt(dt, GridDetail2)

            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDetail2)
                GridDetail2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("DtMaterial").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDetail2.DataSource = DtTemp
                GridDetail2.DataBind()
                GridDetail2.Columns(0).Visible = False
            End If

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
                If ViewState("DtValue") <> tbWONo.Text + "|" + tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "WONo, Product", tbWONo.Text + "|" + tbProductCode.Text) Then
                        lbStatus.Text = "WONo " + QuotedStr(tbWONo.Text) + " And Product " + QuotedStr(tbProductCode.Text) + " has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("WONO+'|'+Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If            
                Row.BeginEdit()
                Row("WONo") = tbWONo.Text
                Row("Product") = tbProductCode.Text
                Row("ProductName") = tbProductName.Text
                Row("Shift") = ddlShift.SelectedValue
                Row("ShiftName") = ddlShift.SelectedItem.Text
                Row("StartDate") = tbStartDate.SelectedValue
                Row("EndDate") = tbEndDate.SelectedDate
                Row("StartTime") = tbStartTime.Text
                Row("EndTime") = tbEndTime.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = lbUnit.Text
                Row("BOMNo") = tbBOMNo.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "WONo, Product", tbWONo.Text + "|" + tbProductCode.Text) Then
                    lbStatus.Text = "WONo " + QuotedStr(tbWONo.Text) + " AND Product " + QuotedStr(tbProductCode.Text) + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WONo") = tbWONo.Text
                dr("Product") = tbProductCode.Text
                dr("ProductName") = tbProductName.Text
                dr("Shift") = ddlShift.SelectedValue
                dr("ShiftName") = ddlShift.SelectedItem.Text
                dr("StartDate") = tbStartDate.SelectedDate
                dr("EndDate") = tbEndDate.SelectedDate
                dr("StartTime") = tbStartTime.Text
                dr("EndTime") = tbEndTime.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = lbUnit.Text
                dr("BOMNo") = tbBOMNo.Text
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
                tbTransNo.Text = GetAutoNmbr("IMR", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PROMRHd (TransNmbr, Status, Transdate, MRType, TTReturNo, WorkCtr, Warehouse, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlMRType.SelectedValue) + ", " + QuotedStr(tbReturNo.Text) + ", " + QuotedStr(ddlWorkCtr.SelectedValue) + _
                ", " + QuotedStr(ddlWarehouse.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                ViewState("Reference") = tbTransNo.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PROMRHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PROMRHD SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', MRType = " + QuotedStr(ddlMRType.SelectedValue) + ", " + _
                " TTReturNo = " + QuotedStr(tbReturNo.Text) + ", WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue) + ", " + _
                " Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + ", Remark = " + QuotedStr(tbRemark.Text) + ", DateAppr = GetDate()" + _
                " WHERE TransNmbr = '" + tbTransNo.Text + "'"
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            If Not ViewState("DtMaterial") Is Nothing Then
                Row = ViewState("DtMaterial").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbTransNo.Text
                    Row(I).EndEdit()
                Next
            End If

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, WONo, Product, Shift, StartDate, StartTime, EndDate, EndTime, Qty, Unit, BOMNo FROM PROMRDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PROMRDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2

            Dim SqlString2 As String
            If Not ViewState("DtMaterial") Is Nothing Then
                SqlString2 = " SELECT TransNmbr, Product, WONo, Material, Qty, Unit, Remark FROM PROMRDt2 WHERE TransNmbr = " + QuotedStr(ViewState("Reference"))

                cmdSql = New SqlCommand(SqlString2, con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim Dt2 As New DataTable("PROMRDt2")

                Dt2 = ViewState("DtMaterial")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("DtMaterial") = Dt2
            End If
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
            tbFilter.Text = tbTransNo.Text
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
            ViewState("DigitCurr") = 0
            ViewState("Reference") = ""
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableMRType = "Y"
            EnableHd(True)
            btnHome.Visible = False
            tbDate.Focus()
            btnAddMaterial.Visible = True
            btnAddDt2Ke2.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlMRType.SelectedValue = "BIASA"
            tbReturNo.Text = ""
            ddlWorkCtr.SelectedIndex = 0
            ddlWarehouse.SelectedIndex = 0
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbWONo.Text = ""
            ddlShift.SelectedIndex = 0
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbStartTime.Text = "00:00"
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbEndTime.Text = "00:00"
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbQty.Text = "0"
            lbUnit.Text = ""
            tbBOMNo.Text = ""
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
            ViewState("StateHd") = "Insert"
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

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

    Protected Sub btnAddMaterial_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddMaterial.Click, btnAddDt2Ke2.Click
        'Dim dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If lbProductCodeDt2.Text = "" Or lbProductCodeDt2.Text = "&nbsp;" Then
                Exit Sub
            End If
            Cleardt2()
            ViewState("StateDt2") = "Insert"
            MovePanel(PnlDetail2, pnlEditMaterial)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbMaterialCode.Text = ""
            tbMaterialName.Text = ""
            tbQtyDt2.Text = "0"
            lbUnitDt2.Text = ""
            tbRemarkDt2.Text = ""
            tbSpecMaterial.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If ddlMRType.SelectedValue.Trim = "PENGGANTIAN" Then
                If tbReturNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Return No must have value")
                    tbReturNo.Focus()
                    Return False
                End If                
            End If
            If ddlWorkCtr.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Work Center must have value")
                ddlWorkCtr.Focus()
                Return False
            End If
            If ddlWarehouse.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlWarehouse.Focus()
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
                If Dr("WONo").ToString.Trim = "" Then
                    lbStatus.Text = "WONO Must Have Value"
                    Return False
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = "Product Must Have Value"
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = "Qty Must Have Value"
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = "Unit Must Have Value"
                    Return False
                End If
                
                If GetCountRecord(ViewState("DtMaterial")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail Material must have value")
                    Return False
                End If

            Else

                If tbWONo.Text.Trim = "" Then
                    lbStatus.Text = "WO NO Must Have Value"
                    tbWONo.Focus()
                    Return False
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = "Product Must Have Value"
                    tbProductCode.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = "Qty Must Have Value"
                    tbQty.Focus()
                    Return False
                End If
                If lbUnit.Text.Trim = "" Then
                    lbStatus.Text = "Unit Must Have Value"
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
            FilterName = "Reference, Status, MR Type, Return Mtr No, Work Center, Warehouse, Remark"
            FilterValue = "TransNmbr, Status, MRType, TTReturNo, WrkCtrName, WrhsName, Remark"
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
                    ViewState("Status") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    BindDataDt2(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    btnAddMaterial.Visible = False
                    btnAddDt2Ke2.Visible = False
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, PnlDetail2, GridDetail2)
                    btnHome.Visible = True
                    EnableHd(False)
                    EnableMRType = "N"
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        'GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataDt2(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        btnAddMaterial.Visible = True
                        btnAddDt2Ke2.Visible = True
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, PnlDetail2, GridDetail2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                    EnableMRType = "Y"
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PDFormMR ''" + QuotedStr(GVR.Cells(2).Text) + "'', 0"
                        Session("SelectCommand2") = "EXEC S_PDFormMR ''" + QuotedStr(GVR.Cells(2).Text) + "'', 1"

                        'Session("SelectCommand") = "EXEC S_PDFormBOM ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPDMR.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlgs();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                    EnableMRType = "Y"
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
            ElseIf e.CommandName = "DetailAlternate" Then

                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                lbProductCodeDt2.Text = GVR.Cells(2).Text + "|" + GVR.Cells(8).Text
                lbProductNameDt2.Text = " - " + GVR.Cells(9).Text
                lbProduct.Text = GVR.Cells(8).Text
                lbBom.Text = GVR.Cells(12).Text
                lbWO.Text = GVR.Cells(2).Text
                lbQty.Text = GVR.Cells(10).Text

                If GetCountRecord(ViewState("Dt")) = 0 Then
                    lbStatus.Text = "Detail WO must fill first"
                    Exit Sub
                End If

                MultiView1.ActiveViewIndex = 1

                Dim drow As DataRow()
                If ViewState("DtMaterial") Is Nothing Then
                    BindDataDt2(ViewState("Reference"))
                Else
                    drow = ViewState("DtMaterial").Select("WONO+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text.Trim))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDetail2)
                        GridDetail2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("DtMaterial").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDetail2.DataSource = DtTemp
                        GridDetail2.DataBind()
                        GridDetail2.Columns(0).Visible = False
                    End If
                End If
                btnSaveAll.Visible = False
                btnSaveTrans.Visible = False
                btnBack.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr, dr2 As DataRow()
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("WONO+'|'+Product = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(8).Text))
        dr(0).Delete()

        dr2 = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(8).Text))
        For I As Integer = 0 To (dr2.Count - 1)
            dr2(I).Delete()
        Next

        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

        ddlMRType_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text + "|" + GVR.Cells(8).Text)
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + GVR.Cells(8).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbMaterialCode.Focus()
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
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlMRType, Dt.Rows(0)("MRType").ToString)
            BindToText(tbReturNo, Dt.Rows(0)("TTReturNo").ToString)
            BindToDropList(ddlWorkCtr, Dt.Rows(0)("WorkCtr").ToString)
            BindToDropList(ddlWarehouse, Dt.Rows(0)("Warehouse").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WONo +'|'+Product = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbWONo, Dr(0)("WONo").ToString)
                BindToDropList(ddlShift, Dr(0)("Shift").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToText(tbStartTime, Dr(0)("StartTime").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbEndTime, Dr(0)("EndTime").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                lbUnit.Text = Dr(0)("Unit").ToString
                BindToText(tbBOMNo, Dr(0)("BOMNo").ToString)
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

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ProductCode, ProductName, Qty, Unit, BOMNo FROM V_PROMRGetWO WHERE WONo = " + QuotedStr(tbWONo.Text)
            ResultField = "ProductCode, ProductName, Qty, Unit, BOMNo"
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
            SQLString = "SELECT ProductCode, ProductName, Qty, Unit, BOMNo FROM V_PROMRGetWO WHERE WONo = " + QuotedStr(tbWONo.Text) + " AND ProductCode = " + QuotedStr(tbProductCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProductCode, Dr("ProductCode").ToString)
                BindToText(tbProductName, Dr("ProductName").ToString)
                BindToText(tbQty, Dr("Qty").ToString)
                lbUnit.Text = Dr("Unit")
                BindToText(tbBOMNo, Dr("BOMNo").ToString)
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbQty.Text = "0"
                lbUnit.Text = ""
                tbBOMNo.Text = "0"
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
    '    Dim ResultField As String 'ResultSame 
    '    Dim iYear, iMonth As String
    '    Try
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If

    '        If ddlYear.SelectedValue.ToString = "" Then
    '            iYear = 0
    '        Else
    '            iYear = ddlYear.SelectedValue.ToString
    '        End If
    '        If ddlMonth.SelectedValue.ToString = "" Then
    '            iMonth = 0
    '        Else
    '            iMonth = ddlMonth.SelectedValue.ToString
    '        End If
    '        Session("filter") = "EXEC S_PRRequestReff '" + ddlProductType.SelectedValue + "', " + iYear + " , " + iMonth + " , '" + ddlDept.SelectedValue + "', '" + ddlPurpose.SelectedValue + "'"
    '        ResultField = "Product_Code, Product_Name, Specification, Unit, Unit_Order, Qty_Budget, Qty_Issue, Qty_Book"

    '        Session("Column") = ResultField.Split(",")
    '        ViewState("Sender") = "btnGetDt"
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Get Data Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function DtExist() As Boolean
        Dim dete, piar As Boolean
        Try
            If ViewState("Dt") Is Nothing Then
                dete = False
            Else
                dete = GetCountRecord(ViewState("Dt")) > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtPR").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxDt2(ByVal WONo As String, ByVal Material As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtMaterial").select("WONo+'|'+Product = " + QuotedStr(WONo) + " AND Material = " + QuotedStr(Material))
            If Dr.Length > 0 Then
                BindToText(tbMaterialCode, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("MaterialName").ToString)
                BindToText(tbQtyDt2, Dr(0)("Qty").ToString)
                lbUnitDt2.Text = Dr(0)("Unit").ToString
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                BindToText(tbSpecMaterial, Dr(0)("Specification").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Material").ToString = 0 Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString = 0 Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If                
            Else
                If tbMaterialCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(tbQtyDt2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If lbUnitDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            Dim ExistRow As DataRow()
            ExistRow = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text) + " AND Material = " + QuotedStr(tbMaterialCode.Text))

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text) + " AND Material = " + QuotedStr(ViewState("Material")))(0)

                Row.BeginEdit()
                Row("Product") = lbProduct.Text
                Row("WONo") = lbWO.Text
                Row("Material") = tbMaterialCode.Text
                Row("MaterialName") = tbMaterialName.Text
                Row("Unit") = lbUnitDt2.Text
                Row("Qty") = FormatNumber(tbQtyDt2.Text, ViewState("DigitCurr"))
                Row("Remark") = tbRemarkDt2.Text
                Row("Specification") = tbSpecMaterial.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("DtMaterial").NewRow
                dr("Product") = lbProduct.Text
                dr("WONo") = lbWO.Text
                dr("Material") = tbMaterialCode.Text
                dr("MaterialName") = tbMaterialName.Text
                dr("Unit") = lbUnitDt2.Text
                dr("Qty") = FormatNumber(tbQtyDt2.Text, ViewState("DigitCurr"))
                dr("Remark") = tbRemarkDt2.Text
                dr("Specification") = tbSpecMaterial.Text
                ViewState("DtMaterial").Rows.Add(dr)
            End If
            MovePanel(pnlEditMaterial, PnlDetail2)
            EnableHd(Not DtExist())
            'BindGridDt(ViewState("Dt2"), GridDetail2)
            Dim drow As DataRow()

            drow = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDetail2)
                GridDetail2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("DtMaterial").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDetail2.DataSource = DtTemp
                GridDetail2.DataBind()
                GridDetail2.Columns(0).Visible = False
            End If
            StatusButtonSave(True)
            btnSaveTrans.Focus()
            btnSaveAll.Visible = False
            btnSaveTrans.Visible = False
            btnBack.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btnSaveDt2_Click Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Function AllowedRecordDt2() As Integer
        Try
            If ViewState("Material") = tbMaterialCode.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnbackDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbackDt3.Click, btnBackMaterial.Click
        MultiView1.ActiveViewIndex = 0
        btnSaveAll.Visible = True
        btnSaveTrans.Visible = True
        btnBack.Visible = True

        btnSaveAll.Visible = Not btnHome.Visible
        btnSaveTrans.Visible = Not btnHome.Visible
        btnBack.Visible = Not btnHome.Visible
    End Sub

    Protected Sub GridDetail2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDetail2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDetail2.Rows(e.RowIndex)
            dr = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text) + " AND Material = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()

            dr = ViewState("DtMaterial").Select("WONo+'|'+Product = " + QuotedStr(lbProductCodeDt2.Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDetail2)
                GridDetail2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("DtMaterial").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDetail2.DataSource = DtTemp
                GridDetail2.DataBind()
                GridDetail2.Columns(0).Visible = False
            End If
            'BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDetail2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDetail2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDetail2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbProductCodeDt2.Text, GVR.Cells(1).Text)

            MovePanel(PnlDetail2, pnlEditMaterial)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            ViewState("Material") = GVR.Cells(1).Text
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Part Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditMaterial, PnlDetail2)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
            btnSaveAll.Visible = False
            btnSaveTrans.Visible = False
            btnBack.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT MaterialCode, MaterialName, Specification, Unit, (Qty*" + lbQty.Text + ") AS Qty FROM V_PROMRGetBOM WHERE BOMNo = " + QuotedStr(lbBom.Text) + " AND ProductCode = " + QuotedStr(lbProduct.Text)
            ResultField = "MaterialCode, MaterialName, Unit, Qty, Specification"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterial_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMaterialCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterialCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT MaterialCode, MaterialName, Specification, Unit, Qty*" + lbQty.Text + " AS Qty FROM V_PROMRGetBOM WHERE BOMNo = " + QuotedStr(lbBom.Text) + " AND ProductCode = " + QuotedStr(lbProduct.Text) + " AND MaterialCode = " + QuotedStr(tbMaterialCode.Text)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbMaterialCode, Dr("MaterialCode").ToString)
                BindToText(tbMaterialName, Dr("MaterialName").ToString)
                BindToText(tbQtyDt2, Dr("Qty").ToString)
                lbUnitDt2.Text = Dr("Unit").ToString
                tbSpecMaterial.Text = Dr("Specification").ToString
            Else
                tbMaterialCode.Text = ""
                tbMaterialName.Text = ""
                tbQtyDt2.Text = "0"
                lbUnitDt2.Text = ""
                tbSpecMaterial.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbMaterialCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnReturNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_PROMRGetHd"
            ResultField = "ReturnNo, WrhsCode"
            ViewState("Sender") = "btnReturNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnReturNo_Click Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlMRType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMRType.SelectedIndexChanged
        Try
            If ddlMRType.SelectedValue = "PENGGANTIAN" Then
                btnReturNo.Visible = True
                label18.Visible = True
            Else
                btnReturNo.Visible = False
                label18.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlMRType_SelectedIndexChanged Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbWorkCenter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWorkCenter.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsWorkCtr')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbWorkCenter_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnWONo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWONo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT WONo, ShiftCode, ShiftName, StartDate, StartTime, EndDate, EndTime FROM V_PROMRGetWO"
            ResultField = "WONo, ShiftCode, StartDate, StartTime, EndDate, EndTime"
            ViewState("Sender") = "btnWONo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnWONo_Click Click Error : " + ex.ToString
        End Try
    End Sub
End Class
