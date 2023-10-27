Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrBOM
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PROBOMHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    tbSpecificationHd.Text = Session("Result")(3).ToString
                    'tbCustCode.Text = Session("Result")(4).ToString
                    'tbCustName.Text = Session("Result")(5).ToString

                    CekProductExists()
                End If
                If ViewState("Sender") = "btnMaterial" Then
                    tbMaterialCode.Text = Session("Result")(0).ToString
                    tbMaterialName.Text = Session("Result")(1).ToString
                    ddlUnitDt.SelectedValue = TrimStr(Session("Result")(2).ToString)
                    tbSpecificationDt.Text = Session("Result")(3).ToString
                    tbQtyDt.Text = Session("Result")(4).ToString
                End If
                If ViewState("Sender") = "btnMaterialAlternate" Then
                    tbMaterialAlternate.Text = Session("Result")(0).ToString
                    tbMaterialAlternateName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnProcess" Then
                    tbProcessCode.Text = Session("Result")(0).ToString
                    tbProcessName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnSuccessor" Then
                    tbSuccessorCode.Text = Session("Result")(0).ToString
                    tbSuccessorName.Text = Session("Result")(1).ToString

                    EnableOutputType(tbSuccessorCode.Text)
                End If
                If ViewState("Sender") = "btnProductOutput" Then
                    tbProductOutputCode.Text = Session("Result")(0).ToString
                    tbProductOutputName.Text = Session("Result")(1).ToString
                    ddlUnitOutput.SelectedValue = Session("Result")(2).ToString
            End If
                If ViewState("Sender") = "btnFormulaProcess" Then
                    tbFormulaProcess.Text = Session("Result")(0).ToString
                    tbFormulaName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        If CekExistData(ViewState("Dt"), "Process,Material", lblProcessCodeDt.Text + "|" + drResult("MaterialCode")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Process") = lblProcessCodeDt.Text
                            dr("Material") = drResult("MaterialCode")
                            dr("MaterialName") = TrimStr(drResult("MaterialName").ToString)
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            dr("Specification") = drResult("Specification")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    'BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(ViewState("Dt").Rows.count = 0)
                    'Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing

                    Dim drow As DataRow()
                    drow = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt)
                        GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt.DataSource = DtTemp
                        GridDt.DataBind()
                        GridDt.Columns(0).Visible = False
                    End If
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
        FillCombo(ddlUnitDt, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
        FillCombo(ddlUnitOutput, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
        FillCombo(ddlBomType, "SELECT BOMType FROM V_GetBOMType", False, "BOMType", "BOMType", ViewState("DBConnection"))
        FillCombo(ddlUnitCompare, "EXEC S_GetUnitFG " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(ddlBomType.SelectedValue), True, "Unit", "Unit", ViewState("DBConnection"))


        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'tbCustName.Attributes.Add("ReadOnly", "True")
        tbProductName.Attributes.Add("ReadOnly", "True")
        tbMaterialName.Attributes.Add("ReadOnly", "True")
        tbTotalgr.Attributes.Add("ReadOnly", "True")
        tbTotalPremix.Attributes.Add("ReadOnly", "True")
        tbLength.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbWidth.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbSheet.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbGSM.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbRatio.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyOutput.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyCompare.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbLength.Attributes.Add("OnBlur", "setformatHd('Hd');")
        tbWidth.Attributes.Add("OnBlur", "setformatHd('Hd');")
        tbSheet.Attributes.Add("OnBlur", "setformatHd('Hd');")
        tbGSM.Attributes.Add("OnBlur", "setformatHd('Hd');")
        tbRatio.Attributes.Add("OnBlur", "setformatHd('Hd');")
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
        Return "SELECT * From V_PROBOMDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROBOMDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringProcess(ByVal Nmbr As String) As String
        Return "SELECT * From V_PROBOMProcess WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                        Result = ExecSPCommandGo(ActionValue, "S_PDBOM", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'tbEffectiveDate.Enabled = State
            'tbProductCode.Enabled = State
            'btnProduct.Visible = State
            'tbLength.Enabled = State
            'tbWidth.Enabled = State
            'tbSheet.Enabled = State
            'tbGSM.Enabled = State
            ddlBomType.Enabled = State
            'tbRatio.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        'Try
        '    Dim dt As New DataTable
        '    ViewState("Dt") = Nothing
        '    dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
        '    ViewState("Dt") = dt
        '    BindGridDt(dt, GridDt)
        'Catch ex As Exception
        '    Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        'End Try

        Dim dt As New DataTable
        Dim Drow As DataRow()
        Try
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt

            Drow = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text))

            'BindGridDt(dt, GridDt2)

            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt)
                GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt.DataSource = DtTemp
                GridDt.DataBind()
                GridDt.Columns(0).Visible = False
            End If

        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataProcess(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Process") = Nothing
            dt = SQLExecuteQuery(GetStringProcess(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Process") = dt
            BindGridDt(dt, GridProcess)
        Catch ex As Exception
            Throw New Exception("BindDataProcess Error : " + ex.ToString)
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
            btnSaveAll.Visible = False
            btnSaveTrans.Visible = False
            btnBack.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> lblProcessCodeDt.Text + "|" + tbMaterialCode.Text Then
                    If CekExistData(ViewState("Dt"), "Process,Material", lblProcessCodeDt.Text + "|" + tbMaterialCode.Text) Then
                        lbStatus.Text = "Process " + lblProcessNameDt.Text + ", Material '" + tbMaterialCode.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                'Row = ViewState("Dt").Select(" Process = " + QuotedStr(lblProcessCodeDt.Text) + " AND Material = " + QuotedStr(tbMaterialCode.Text))(0)
                Row = ViewState("Dt").Select(" Process+'|'+Material = " + QuotedStr(ViewState("DtValue")))(0)

                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Process") = lblProcessCodeDt.Text
                Row("Material") = tbMaterialCode.Text
                Row("MaterialName") = tbMaterialName.Text
                Row("MaterialAlternate") = tbMaterialAlternate.Text
                Row("MaterialAlternateName") = tbMaterialAlternateName.Text
                Row("Qty") = tbQtyDt.Text
                Row("Unit") = ddlUnitDt.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row("Specification") = tbSpecificationDt.Text
                Row("QtyCompare") = tbQtyCompare.Text
                Row("UnitCompare") = ddlUnitCompare.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Process, Material", lblProcessCodeDt.Text + "|" + tbMaterialCode.Text) Then
                    lbStatus.Text = "Process " + QuotedStr(lblProcessCodeDt.Text) + ", Material '" + tbMaterialCode.Text + "' has already exists"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Process") = lblProcessCodeDt.Text
                dr("Material") = tbMaterialCode.Text
                dr("MaterialName") = tbMaterialName.Text
                dr("MaterialAlternate") = tbMaterialAlternate.Text
                dr("MaterialAlternateName") = tbMaterialAlternateName.Text
                dr("Qty") = tbQtyDt.Text
                dr("Unit") = ddlUnitDt.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                dr("Specification") = tbSpecificationDt.Text
                dr("QtyCompare") = tbQtyCompare.Text
                dr("UnitCompare") = ddlUnitCompare.SelectedValue
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'BindGridDt(ViewState("Dt"), GridDt)
            Dim drow As DataRow()
            drow = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt)
                GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt.DataSource = DtTemp
                GridDt.DataBind()
                GridDt.Columns(0).Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
        StatusButtonSave(True)
        btnSaveTrans.Focus()
        btnSaveAll.Visible = False
        btnSaveTrans.Visible = False
        btnBack.Visible = False
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDtProcess.Visible = False Then
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

                Dim Dt2 As DataTable
                Dim Dr2 As DataRow
                Dim SQLString2, SetCode As String

                SQLString2 = "SELECT * FROM V_GetBOMType WHERE BOMType = " + QuotedStr(ddlBomType.SelectedValue)
                Dt2 = SQLExecuteQuery(SQLString2, ViewState("DBConnection").ToString).Tables(0)
                If Dt2.Rows.Count > 0 Then
                    Dr2 = Dt2.Rows(0)
                    SetCode = Dr2("SetCode")
                Else
                    SetCode = "BOM"
                End If

                tbTransNo.Text = GetAutoNmbr(SetCode, "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PROBOMHd (TransNmbr, Status, Transdate, EffectiveDate, Product, Remark, UserPrep, DatePrep, BOMType, TSLength, TSWidth, TSSheetPerPack, TSGSM, TSRatioPremix, TSTotalGr, TSTotalPremixGr) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbProductCode.Text) + ", '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(ddlBomType.SelectedValue) + ", " + tbLength.Text.Replace(",", "") + ", " + tbWidth.Text.Replace(",", "") + ", " + _
                tbSheet.Text.Replace(",", "") + ", " + tbGSM.Text.Replace(",", "") + ", " + tbRatio.Text.Replace(",", "") + ", " + tbTotalgr.Text.Replace(",", "") + ", " + tbTotalPremix.Text.Replace(",", "")

                ViewState("Reference") = tbTransNo.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PROBOMHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PROBOMHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', EffectiveDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", Product = " + QuotedStr(tbProductCode.Text) + _
                ", Remark = '" + tbRemark.Text + "', DateAppr = GetDate(), BOMType = " + QuotedStr(ddlBomType.SelectedValue) + _
                ", TSLength = " + tbLength.Text.Replace(",", "") + ", TSWidth = " + tbWidth.Text.Replace(",", "") + ", TSSheetPerPack = " + tbSheet.Text.Replace(",", "") + _
                ", TSGSM = " + tbGSM.Text.Replace(",", "") + ", TSRatioPremix = " + tbRatio.Text.Replace(",", "") + ", TSTotalGr = " + tbTotalgr.Text.Replace(",", "") + ", TSTotalPremixGr = " + tbTotalPremix.Text.Replace(",", "") + _
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

            If Not ViewState("DtAlternate") Is Nothing Then
                Row = ViewState("DtAlternate").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbTransNo.Text
                    Row(I).EndEdit()
                Next
            End If

            If Not ViewState("Process") Is Nothing Then
                Row = ViewState("Process").Select("TransNmbr IS NULL")
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Process, Material, Qty, Unit, Remark, MaterialAlternate, QtyCompare, UnitCompare FROM PROBOMDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PROBOMDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'Save Process
            Dim SQLStringProcess As String
            If Not ViewState("Process") Is Nothing Then
                SQLStringProcess = " SELECT TransNmbr, Process, Successor, OutputType, FgSubkon, ProductOutput, QtyOutput, UnitOutput, FormulaNo, Remark FROM PROBOMProcess WHERE TransNmbr = " + QuotedStr(ViewState("Reference"))

                cmdSql = New SqlCommand(SQLStringProcess, con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand

                Dim param As SqlParameter
                ' Create the UpdateCommand.
                Dim Update_Command = New SqlCommand( _
                        "UPDATE PROBOMProcess SET Process = @Process, Successor = @Successor, OutputType = @OutputType, FgSubkon = @FgSubkon, ProductOutput = @ProductOutput, QtyOutput = @QtyOutput, UnitOutput = @UnitOutput, FormulaNo = @FormulaNo, Remark = @Remark FROM PROBOMProcess WHERE TransNmbr = '" & ViewState("Reference") & "' AND Process = @OldProcess ", con)
                ' Define output parameters.
                Update_Command.Parameters.Add("@Process", SqlDbType.VarChar, 10, "Process")
                Update_Command.Parameters.Add("@Successor", SqlDbType.VarChar, 10, "Successor")
                Update_Command.Parameters.Add("@OutputType", SqlDbType.VarChar, 5, "OutputType")
                Update_Command.Parameters.Add("@FgSubkon", SqlDbType.VarChar, 1, "FgSubkon")
                Update_Command.Parameters.Add("@ProductOutput", SqlDbType.VarChar, 20, "ProductOutput")
                Update_Command.Parameters.Add("@QtyOutput", SqlDbType.Decimal, 18, "QtyOutput")
                Update_Command.Parameters.Add("@UnitOutput", SqlDbType.VarChar, 5, "UnitOutput")
                Update_Command.Parameters.Add("@FormulaNo", SqlDbType.VarChar, 20, "FormulaNo")
                Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
                ' Define intput (WHERE) parameters.
                param = Update_Command.Parameters.Add("@OldProcess", SqlDbType.VarChar, 10, "Process")
                param.SourceVersion = DataRowVersion.Original
                ' Attach the update command to the DataAdapter.
                da.UpdateCommand = Update_Command

                ' Create the DeleteCommand.
                Dim Delete_Command = New SqlCommand( _
                    "DELETE FROM PROBOMProcess WHERE TransNmbr = '" & ViewState("Reference") & "' AND Process = @Process", con)
                ' Add the parameters for the DeleteCommand.
                param = Delete_Command.Parameters.Add("@Process", SqlDbType.VarChar, 10, "Process")
                param.SourceVersion = DataRowVersion.Original
                da.DeleteCommand = Delete_Command

                Dim DtProcess As New DataTable("PROBOMProcess")

                DtProcess = ViewState("Process")
                da.Update(DtProcess)
                DtProcess.AcceptChanges()
                ViewState("Process") = DtProcess
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

            If GetCountRecord(ViewState("Process")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Process must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Process").Rows
                If CekProcess(dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Material must have at least 1 record")
            '    Exit Sub
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
            ViewState("DigitCurr") = 0

            MovePanel(PnlHd, pnlInput)
            'ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDtProcess, GridProcess)
            newTrans()
            EnableHd(True)
            BOMTypeChange()
            btnHome.Visible = False
            tbDate.Focus()
            tbLength.Enabled = True
            tbWidth.Enabled = True
            tbSheet.Enabled = True
            tbGSM.Enabled = True
            tbRatio.Enabled = True
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Reference") = ""
            ClearHd()
            Cleardt()
            ClearProcess()
            'pnlDt.Visible = True
            BindDataDt("")
            BindDataProcess("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEffectiveDate.SelectedDate = ViewState("ServerDate") 'Today
            'tbCustCode.Text = ""
            'tbCustName.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0
            tbSpecificationHd.Text = ""
            ddlBomType.SelectedValue = "PE"
            tbLength.Text = "0"
            tbWidth.Text = "0"
            tbSheet.Text = "0"
            tbGSM.Text = "0"
            tbRatio.Text = "0"
            tbTotalgr.Text = "0"
            tbTotalPremix.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbMaterialCode.Text = ""
            tbMaterialName.Text = ""
            tbSpecificationDt.Text = ""
            ddlUnitDt.Text = ""
            tbQtyDt.Text = "0"
            tbRemarkDt.Text = ""
            tbMaterialAlternate.Text = ""
            tbMaterialAlternateName.Text = ""
            tbQtyCompare.Text = "0"
            ddlUnitCompare.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearProcess()
        Try
            tbProcessCode.Text = ""
            tbProcessName.Text = ""
            tbSuccessorCode.Text = ""
            tbSuccessorName.Text = ""
            ddlOutputType.SelectedValue = "WIP"
            ddlSubkon.SelectedValue = "N"
            tbProductOutputCode.Text = ""
            tbProductOutputName.Text = ""
            tbRemarkProcess.Text = ""
            tbQtyOutput.Text = "1"
            tbFormulaProcess.Text = ""
            ddlUnitOutput.SelectedIndex = 0
            tbFormulaName.Text = ""
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

        FillCombo(ddlUnitCompare, "EXEC S_GetUnitFG " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(ddlBomType.SelectedValue), True, "Unit", "Unit", ViewState("DBConnection"))

        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbEffectiveDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Effective Date must greater than BOM Date")
                tbEffectiveDate.Focus()
                Return False
            End If
            If tbProductCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Product must have value")
                tbProductCode.Focus()
                Return False
            End If

            If ddlBomType.SelectedValue = "TISSUE" Then
                If CFloat(tbLength.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Length must have value")
                    tbLength.Focus()
                    Return False
                End If

                If CFloat(tbWidth.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Width must have value")
                    tbWidth.Focus()
                    Return False
                End If

                If CFloat(tbSheet.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Sheet must have value")
                    tbSheet.Focus()
                    Return False
                End If

                If CFloat(tbGSM.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("GSM must have value")
                    tbGSM.Focus()
                    Return False
                End If

                If CFloat(tbRatio.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Ratio Premix must have value")
                    tbRatio.Focus()
                    Return False
                End If

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
                If Dr("Material").ToString.Trim = "" Then
                    lbStatus.Text = "Material Must Have Value"
                    Return False
                End If

                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = "Qty Material " + Dr("MaterialName").ToString.Trim + " Must Have Value"
                    Return False
                End If

                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = "Unit Material " + Dr("MaterialName").ToString.Trim + " Must Have Value"
                    Return False
                End If

                If CFloat(Dr("QtyCompare").ToString) <= 0 Then
                    lbStatus.Text = "Qty Compare Material " + Dr("MaterialName").ToString.Trim + " Must Have Value"
                    Return False
                End If

                If Dr("UnitCompare").ToString.Trim = "" Then
                    lbStatus.Text = "Unit Compare Material " + Dr("MaterialName").ToString.Trim + " Must Have Value"
                    Return False
                End If

            Else
                If tbMaterialCode.Text.Trim = "" Then
                    lbStatus.Text = "Material Must Have Value"
                    tbMaterialCode.Focus()
                    Return False
                End If
                If CFloat(tbQtyDt.Text) <= 0 Then
                    lbStatus.Text = "Qty Must Have Value"
                    tbQtyDt.Focus()
                    Return False
                End If
                If ddlUnitDt.SelectedValue.Trim = "" Then
                    lbStatus.Text = "Unit Must Have Value"
                    ddlUnitDt.Focus()
                    Return False
                End If
                If CFloat(tbQtyCompare.Text) <= 0 Then
                    lbStatus.Text = "Qty Compare Must Have Value"
                    tbQtyCompare.Focus()
                    Return False
                End If
                If ddlUnitCompare.SelectedValue.Trim = "" Then
                    lbStatus.Text = "Unit Compare Must Have Value"
                    ddlUnitCompare.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekProcess(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Process").ToString.Trim = "" Then
                    lbStatus.Text = "Process Must Have Value"
                    Return False
                End If
                If Dr("OutputType").ToString.Trim = "" Then
                    lbStatus.Text = "Output Type Must Have Value"
                    Return False
                End If
                If Dr("OutputType").ToString.Trim = "WIP" Then
                    If Dr("ProductOutput").ToString.Trim = "" Then
                        lbStatus.Text = "Product Output Must Have Value"
                        Return False
                    End If
                    If Dr("UnitOutput").ToString.Trim = "" Then
                        lbStatus.Text = "Unit Output Must Have Value"
                        Return False
                    End If
                End If
                If CFloat(Dr("QtyOutput").ToString.Trim) <= "0" Then
                    lbStatus.Text = "Qty Output Must Have Value"
                    Return False
                End If                
            Else
                If tbProcessCode.Text.Trim = "" Then
                    lbStatus.Text = "Process Must Have Value"
                    tbProcessCode.Focus()
                    Return False
                End If
                If ddlOutputType.SelectedValue.Trim = "" Then
                    lbStatus.Text = "Output Type Must Have Value"
                    ddlOutputType.Focus()
                    Return False
                End If
                If ddlOutputType.SelectedValue.Trim = "WIP" Then
                    If tbProductOutputCode.Text.Trim = "" Then
                        lbStatus.Text = "Product Output Must Have Value"
                        tbProductOutputCode.Focus()
                        Return False
                    End If
                    If ddlUnitOutput.SelectedValue.Trim = "" Then
                        lbStatus.Text = "Unit Output Must Have Value"
                        ddlUnitOutput.Focus()
                        Return False
                    End If
                End If
                If CFloat(tbQtyOutput.Text.Trim) <= "0" Then
                    lbStatus.Text = "Qty Output Must Have Value"
                    tbQtyOutput.Focus()
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
            FDateName = "Date, Effective Date"
            FDateValue = "TransDate, EffectiveDate"
            FilterName = "Reference, Product Code, Product Name, Remark"
            FilterValue = "TransNmbr, Product, Product_Name, Remark"
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
                    'GridDt.PageIndex = 0
                    GridProcess.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataProcess(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDtProcess, GridProcess)
                    ModifyInput(False, pnlTissue)
                    btnHome.Visible = True
                    EnableHd(False)
                    BOMTypeChange()
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
                        'GridDt.PageIndex = 0
                        GridProcess.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataProcess(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        btnHome.Visible = False
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDtProcess, GridProcess)
                        ModifyInput(True, pnlTissue)
                        EnableHd(GetCountRecord(ViewState("Process")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                    BOMTypeChange()
                ElseIf DDL.SelectedValue = "Copy New" Then
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("StateHd") = "Insert"
                    ViewState("DigitCurr") = 0
                    MovePanel(PnlHd, pnlInput)
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    ModifyInput2(True, pnlInput, pnlDtProcess, GridProcess)
                    ModifyInput(True, pnlTissue)
                    'newTrans()
                    btnHome.Visible = False
                    GridProcess.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    ViewState("Status") = "H"

                    CopyDataDt(ViewState("Reference"))
                    CopyDataProcess(ViewState("Reference"))


                    ViewState("Reference") = ""
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = False
                    EnableHd(GetCountRecord(ViewState("Process")) = 0)
                    tbTransNo.Text = ""
                    tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
                    tbEffectiveDate.SelectedDate = ViewState("ServerDate")

                    BOMTypeChange()
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PDFormBOM ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPDBOM.frx"
                        Session("DBConnection") = ViewState("DBConnection")
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "DetailAlternate" Then

                
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                MultiView1.ActiveViewIndex = 2

                Dim drow As DataRow()

                btnSaveAll.Visible = False
                btnSaveTrans.Visible = False
                btnBack.Visible = False

                btnHome.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr As DataRow()
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        dr = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text) + " AND Material = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()

        dr = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text))
        If dr.Length > 0 Then
            BindGridDt(dr.CopyToDataTable, GridDt)
            GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
        Else
            Dim DtTemp As DataTable
            DtTemp = ViewState("Dt").Clone
            DtTemp.Rows.Add(DtTemp.NewRow())
            GridDt.DataSource = DtTemp
            GridDt.DataBind()
            GridDt.Columns(0).Visible = False
        End If

        'BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            FillCombo(ddlUnitCompare, "EXEC S_GetUnitFG " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(ddlBomType.SelectedValue), True, "Unit", "Unit", ViewState("DBConnection"))

            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            ViewState("DtValue") = lblProcessCodeDt.Text + "|" + tbMaterialCode.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbMaterialCode.Focus()
            StatusButtonSave(False)

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbMaterial.Click
        Try
            ViewState("InputCostCtr") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbMaterial_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            'BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            'BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbProductCode, Dt.Rows(0)("Product").ToString)
            BindToText(tbProductName, Dt.Rows(0)("Product_Name").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbSpecificationHd, Dt.Rows(0)("Specification").ToString)
            BindToDropList(ddlBomType, Dt.Rows(0)("BOMType").ToString)
            BindToText(tbLength, Dt.Rows(0)("TSLength").ToString)
            BindToText(tbWidth, Dt.Rows(0)("TSWidth").ToString)
            BindToText(tbSheet, Dt.Rows(0)("TSSheetPerPack").ToString)
            BindToText(tbGSM, Dt.Rows(0)("TSGSM").ToString)
            BindToText(tbRatio, Dt.Rows(0)("TSRatioPremix").ToString)
            BindToText(tbLength, Dt.Rows(0)("TSLength").ToString)
            BindToText(tbTotalgr, Dt.Rows(0)("TSTotalGr").ToString)
            BindToText(tbTotalPremix, Dt.Rows(0)("TSTotalPremixGr").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Process = " + QuotedStr(lblProcessCodeDt.Text) + " AND Material = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbMaterialCode, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("MaterialName").ToString)
                BindToText(tbQtyDt, Dr(0)("Qty").ToString)
                BindToDropList(ddlUnitDt, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToDropList(ddlUnitCompare, Dr(0)("UnitCompare").ToString)
                BindToText(tbSpecificationDt, Dr(0)("Specification").ToString)
                BindToText(tbMaterialAlternate, Dr(0)("MaterialAlternate").ToString)
                BindToText(tbMaterialAlternateName, Dr(0)("MaterialAlternateName").ToString)
                BindToText(tbQtyCompare, Dr(0)("QtyCompare").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxProcess(ByVal KeyDt As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Process").select("Process = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbProcessCode, Dr(0)("Process").ToString)
                BindToText(tbProcessName, Dr(0)("ProcessName").ToString)
                BindToText(tbSuccessorCode, Dr(0)("Successor").ToString)
                BindToText(tbSuccessorName, Dr(0)("SuccessorName").ToString)
                BindToDropList(ddlOutputType, Dr(0)("OutputType").ToString)
                If IsDBNull(Dr(0)("FgSubkon")) Then
                    ddlSubkon.SelectedValue = "N"
                Else
                    BindToDropList(ddlSubkon, Dr(0)("FgSubkon").ToString)
                End If
                BindToText(tbProductOutputCode, Dr(0)("ProductOutput").ToString)
                BindToText(tbProductOutputName, Dr(0)("ProductOutputName").ToString)
                BindToText(tbQtyOutput, Dr(0)("QtyOutput").ToString)
                BindToDropList(ddlUnitOutput, Dr(0)("UnitOutput").ToString)
                BindToText(tbFormulaProcess, Dr(0)("FormulaNo").ToString)
                BindToText(tbRemarkProcess, Dr(0)("Remark").ToString)
                tbFormulaName.Text = Dr(0)("FormulaName").ToString
            End If
        Catch ex As Exception
            Throw New Exception("FillTextBoxProcess error : " + ex.ToString)
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
            Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y'"
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
            SQLString = "SELECT Product_Code AS ProductCode, Product_Name AS ProductName, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' AND Product_Code = " + QuotedStr(tbProductCode.Text)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProductCode, Trim(Dr("ProductCode").ToString))
                BindToText(tbProductName, Trim(Dr("ProductName").ToString))
                BindToText(tbSpecificationHd, Trim(Dr("Specification").ToString))
                'BindToText(tbCustCode, Trim(Dr("Customer_Code").ToString))
                'BindToText(tbCustName, Trim(Dr("Customer_Name")))
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecificationHd.Text = ""
                'tbCustCode.Text = ""
                'tbCustName.Text = ""
            End If
            CekProductExists()
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

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "EXEC S_PDBOMGetMaterial '' "
            ResultField = "MaterialCode, MaterialName, Unit, Specification, Qty"
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
            SQLString = "EXEC S_PDBOMGetMaterial " + QuotedStr(tbMaterialCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbMaterialCode, Dr("MaterialCode").ToString)
                BindToText(tbMaterialName, Dr("MaterialName").ToString)
                BindToDropList(ddlUnitDt, Dr("Unit").ToString)
                BindToText(tbSpecificationDt, Dr("Specification").ToString)
                BindToText(tbQtyDt, Dr("Qty").ToString)
            Else
                tbMaterialCode.Text = ""
                tbMaterialName.Text = ""
                ddlUnitDt.SelectedIndex = 0
                tbSpecificationDt.Text = ""
                tbQtyDt.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tbSONo_TextChanged Error : " + ex.ToString)
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
                dr("Process") = R("Process")
                dr("Material") = R("Material")
                dr("MaterialName") = R("MaterialName")
                dr("Qty") = R("Qty")
                dr("Unit") = R("Unit")
                dr("Remark") = R("Remark")
                dr("Specification") = R("Specification")
                dr("MaterialAlternate") = R("MaterialAlternate")
                dr("MaterialAlternateName") = R("MaterialAlternateName")
                dr("QtyCompare") = R("QtyCompare")
                dr("UnitCompare") = R("UnitCompare")
                ViewState("Dt").Rows.Add(dr)
            Next
            'GridDt.Columns(6).Visible = ViewState("Status") = "P"
            'BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CopyDataProcess(ByVal Nmbr As String)
        Try
            Dim dt, dtSource As New DataTable
            dtSource = SQLExecuteQuery(GetStringProcess(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Process") = Nothing
            dt = SQLExecuteQuery(GetStringProcess(""), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Process") = dt
            For Each R In dtSource.Rows
                Dim dr As DataRow
                dr = ViewState("Process").NewRow
                dr("Process") = R("Process")
                dr("ProcessName") = R("ProcessName")
                dr("Successor") = R("Successor")
                dr("SuccessorName") = R("SuccessorName")
                dr("OutputType") = R("OutputType")
                dr("ProductOutput") = R("ProductOutput")
                dr("ProductOutputName") = R("ProductOutputName")
                dr("QtyOutput") = R("QtyOutput")
                dr("UnitOutput") = R("UnitOutput")
                dr("FormulaNo") = R("FormulaNo")
                dr("Remark") = R("Remark")
                ViewState("Process").Rows.Add(dr)
            Next
            BindGridDt(ViewState("Process"), GridProcess)
        Catch ex As Exception
            Throw New Exception("CopyDataProcess Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDtProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDtProcess.Click, btnAddDt2Process.Click
        ClearProcess()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateProcess") = "Insert"
        MovePanel(pnlDtProcess, pnlEditDtProcess)
        EnableHd(False)
        EnabledFormulaNo()
        EnableOutputType("")
        StatusButtonSave(False)
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ProcessCode, ProcessName FROM MsProcess"
            ResultField = "ProcessCode, ProcessName"

            ViewState("Sender") = "btnProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProcess_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProcessCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProcessCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT ProcessCode, ProcessName FROM MsProcess WHERE ProcessCode = " + QuotedStr(tbProcessCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbProcessCode, Dr("ProcessCode").ToString)
                BindToText(tbProcessName, Dr("ProcessName").ToString)
            Else
                tbProcessCode.Text = ""
                tbProcessName.Text = ""
            End If
            tbProductOutputCode.Text = ""
            tbProductOutputName.Text = ""
        Catch ex As Exception
            Throw New Exception("tbProcessCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSuccessor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSuccessor.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ProcessCode, ProcessName FROM MsProcess WHERE ProcessCode <> " + QuotedStr(tbProcessCode.Text)
            ResultField = "ProcessCode, ProcessName"

            ViewState("Sender") = "btnSuccessor"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnSuccessor_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuccessorCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuccessorCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT ProcessCode, ProcessName FROM MsProcess WHERE ProcessCode = " + QuotedStr(tbSuccessorCode.Text) + " AND ProcessCode <> " + QuotedStr(tbProcessCode.Text)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbSuccessorCode, Dr("ProcessCode").ToString)
                BindToText(tbSuccessorName, Dr("ProcessName").ToString)
            Else
                tbSuccessorCode.Text = ""
                tbSuccessorName.Text = ""
            End If

            EnableOutputType(tbSuccessorCode.Text)

            tbProductOutputCode.Text = ""
            tbProductOutputName.Text = ""
        Catch ex As Exception
            Throw New Exception("tbSuccessorCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProductOutput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductOutput.Click
        Dim ResultField As String
        Try
            If ddlOutputType.SelectedValue.Trim = "WIP" Then
                Session("filter") = "SELECT WIPCode, WIPName, Unit FROM VMsWIPProcess WHERE Process = " + QuotedStr(tbProcessCode.Text)
                ResultField = "WIPCode, WIPName, Unit"
            Else
                Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE ProductCategory IN ('Finish Good', 'Semi FG')"
                ResultField = "Product_Code, Product_Name, Unit"
            End If

            ViewState("Sender") = "btnProductOutput"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnProductOutput_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductOutputCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductOutputCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            If ddlOutputType.SelectedValue.Trim = "WIP" Then
                SQLString = "SELECT WIPCode, WIPName, Unit FROM VMsWIPProcess WHERE WIPCode = " + QuotedStr(tbProductOutputCode.Text) + " and Process = " + QuotedStr(tbProcessCode.Text)
                Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
                If Dt.Rows.Count > 0 Then
                    Dr = Dt.Rows(0)
                    BindToText(tbProductOutputCode, Dr("WIPCode").ToString)
                    BindToText(tbProductOutputName, Dr("WIPName").ToString)
                    BindToDropList(ddlUnitOutput, Dr("Unit").ToString)
                Else
                    tbProductOutputCode.Text = ""
                    tbProductOutputName.Text = ""
                    ddlUnitOutput.SelectedIndex = 0
                End If
            Else
                SQLString = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE ProductCategory IN ('Finish Good', 'Semi FG') AND Product_Code = " + QuotedStr(tbProductOutputCode.Text)

                Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
                If Dt.Rows.Count > 0 Then
                    Dr = Dt.Rows(0)
                    BindToText(tbProductOutputCode, Dr("Product_Code").ToString)
                    BindToText(tbProductOutputName, Dr("Product_Name").ToString)
                    BindToDropList(ddlUnitOutput, Dr("Unit").ToString)
                Else
                    tbProductOutputCode.Text = ""
                    tbProductOutputName.Text = ""
                    ddlUnitOutput.SelectedIndex = 0
                End If
            End If
            
            
            tbQtyOutput.Text = "1"
        Catch ex As Exception
            Throw New Exception("tbProductOutputCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlHaveOutput_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOutputType.TextChanged
        Try
            tbProductOutputCode.Text = ""
            tbProductOutputName.Text = ""
            tbQtyOutput.Text = "1"
            ddlUnitOutput.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("ddlHaveOutput_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelProcess.Click
        Try
            MovePanel(pnlEditDtProcess, pnlDtProcess)
            EnableHd(GetCountRecord(ViewState("Process")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btnCancelProcess_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveProcess.Click
        Dim Row As DataRow
        Try
            If ViewState("StateProcess") = "Edit" Then
                If ViewState("DtValueProcess") <> tbProcessCode.Text Then
                    If CekExistData(ViewState("Process"), "Process", tbProcessCode.Text) Then
                        lbStatus.Text = "Process '" + tbProcessName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Process").Select("Process = " + QuotedStr(ViewState("DtValueProcess")))(0)
                If CekProcess() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Process") = tbProcessCode.Text
                Row("ProcessName") = tbProcessName.Text
                Row("Successor") = tbSuccessorCode.Text
                Row("SuccessorName") = tbSuccessorName.Text
                Row("OutputType") = ddlOutputType.SelectedValue
                Row("FgSubkon") = ddlSubkon.SelectedValue
                Row("ProductOutput") = tbProductOutputCode.Text
                Row("ProductOutputName") = tbProductOutputName.Text
                Row("Remark") = tbRemarkProcess.Text
                Row("QtyOutput") = tbQtyOutput.Text
                Row("UnitOutput") = ddlUnitOutput.SelectedValue
                Row("FormulaNo") = tbFormulaProcess.Text
                Row("FormulaName") = tbFormulaName.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekProcess() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Process"), "Process", tbProcessCode.Text) Then
                    lbStatus.Text = "Process '" + tbProcessName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Process").NewRow
                dr("Process") = tbProcessCode.Text
                dr("ProcessName") = tbProcessName.Text
                dr("Successor") = tbSuccessorCode.Text
                dr("SuccessorName") = tbSuccessorName.Text
                dr("OutputType") = ddlOutputType.SelectedValue
                dr("FgSubkon") = ddlSubkon.SelectedValue
                dr("ProductOutput") = tbProductOutputCode.Text
                dr("ProductOutputName") = tbProductOutputName.Text
                dr("Remark") = tbRemarkProcess.Text
                dr("QtyOutput") = tbQtyOutput.Text
                dr("UnitOutput") = ddlUnitOutput.SelectedValue
                dr("FormulaNo") = tbFormulaProcess.Text
                dr("FormulaName") = tbFormulaName.Text
                ViewState("Process").Rows.Add(dr)
            End If
            MovePanel(pnlEditDtProcess, pnlDtProcess)
            EnableHd(GetCountRecord(ViewState("Process")) = 0)
            BindGridDt(ViewState("Process"), GridProcess)
        Catch ex As Exception
            lbStatus.Text = "btnSaveProcess_Click Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
        StatusButtonSave(True)
    End Sub

    Protected Sub GridProcess_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridProcess.PageIndexChanging
        Try
            GridProcess.PageIndex = e.NewPageIndex
            GridProcess.DataSource = ViewState("Process")
            GridProcess.DataBind()
        Catch ex As Exception
            lbStatus.Text = "GridProcess_PageIndexChanging Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridProcess_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridProcess.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDtProcess_Click(Nothing, Nothing)
            ElseIf e.CommandName = "DetailMaterial" Then

                Dim GVR As GridViewRow
                GVR = GridProcess.Rows(Convert.ToInt32(e.CommandArgument))

                lblProcessCodeDt.Text = GVR.Cells(2).Text
                lblProcessNameDt.Text = " - " + GVR.Cells(3).Text
                ViewState("FormulaNo") = GVR.Cells(6).Text

                If GVR.Cells(2).Text = "&nbsp;" Then
                    lbStatus.Text = "Process " + lblProcessCodeDt.Text + " - " + lblProcessNameDt.Text + " not exists have Detail"
                    Exit Sub
                End If

                If ViewState("StateHd") = "View" Then
                    btnAddDt.Visible = False
                    btnAddDt2.Visible = False
                    btnGetDt.Visible = False
                Else
                    btnAddDt.Visible = True
                    btnAddDt2.Visible = True
                    btnGetDt.Visible = True
                End If

                MultiView1.ActiveViewIndex = 1

                Dim drow As DataRow()
                If ViewState("Dt") Is Nothing Then
                    BindDataDt(ViewState("Reference"))
                Else
                    drow = ViewState("Dt").Select("Process = " + QuotedStr(lblProcessCodeDt.Text.Trim))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt)
                        GridDt.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt.DataSource = DtTemp
                        GridDt.DataBind()
                        GridDt.Columns(0).Visible = False
                    End If
                End If
                btnSaveAll.Visible = False
                btnSaveTrans.Visible = False
                btnBack.Visible = False

                btnHome.Visible = False
                End If
        Catch ex As Exception
            lbStatus.Text = "GridProcess_RowCommand Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub GridProcess_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridProcess.RowDeleting
        Dim dr, dr2, dr3 As DataRow()
        Dim GVR As GridViewRow = GridProcess.Rows(e.RowIndex)

        dr3 = ViewState("Process").Select("Process= " + QuotedStr(GVR.Cells(2).Text))
        dr3(0).Delete()

        dr = ViewState("Dt").Select("Process = " + QuotedStr(GVR.Cells(2).Text))
        For I As Integer = 0 To (dr.Count - 1)
            dr(I).Delete()
        Next
        
        BindGridDt(ViewState("Process"), GridProcess)
        EnableHd(GetCountRecord(ViewState("Process")) = 0)
    End Sub

    Protected Sub GridProcess_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridProcess.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridProcess.Rows(e.NewEditIndex)
            FillTextBoxProcess(GVR.Cells(2).Text)
            ViewState("DtValueProcess") = tbProcessCode.Text
            MovePanel(pnlDtProcess, pnlEditDtProcess)
            EnableHd(False)
            ViewState("StateProcess") = "Edit"
            tbProcessCode.Focus()
            StatusButtonSave(False)
            EnabledFormulaNo()
            EnableOutputType(TrimStr(GVR.Cells(4).Text))
        Catch ex As Exception
            lbStatus.Text = "GridProcess_RowEditing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt.Click, btnBackDt2.Click
        MultiView1.ActiveViewIndex = 0
        btnSaveAll.Visible = Not (ViewState("StateHd") = "View")
        btnSaveTrans.Visible = Not (ViewState("StateHd") = "View")
        btnBack.Visible = Not (ViewState("StateHd") = "View")
        btnHome.Visible = (ViewState("StateHd") = "View")
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String
        Dim CriteriaField As String
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("Result") = Nothing

            Session("Filter") = "EXEC S_PDBOMGetMaterial  ''"

            ResultField = "MaterialCode, MaterialName, Specification, Qty, Unit"
            CriteriaField = "MaterialCode, MaterialName, Specification, Unit"
            Session("ClickSame") = "MaterialCode"

            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = "MaterialCode"
            'Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlBomType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBomType.SelectedIndexChanged
        Try

            If ddlBomType.SelectedValue.Trim = "TISSUE" Then
                pnlTissue.Visible = True
            Else
                pnlTissue.Visible = False
            End If

            tbWidth.Text = "0"
            tbLength.Text = "0"
            tbSheet.Text = "0"
            tbGSM.Text = "0"
            tbRatio.Text = "0"
            tbTotalgr.Text = "0"
            tbTotalPremix.Text = "0"

        Catch ex As Exception
            lbStatus.Text = "ddlBomType_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BOMTypeChange()
        Try
            If ddlBomType.SelectedValue.Trim = "TISSUE" Then
                pnlTissue.Visible = True
            Else
                pnlTissue.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "BOMTypeChange Error : " + ex.ToString
        End Try
    End Sub

    Private Sub CountTotal()
        Try
            tbTotalgr.Text = (CFloat(tbLength.Text) * CFloat(tbWidth.Text) * CFloat(tbSheet.Text) * CFloat(tbGSM.Text)) / 10000

            tbTotalPremix.Text = (CFloat(tbLength.Text) * CFloat(tbWidth.Text) * CFloat(tbSheet.Text) * CFloat(tbGSM.Text) * CFloat(tbRatio.Text)) / 10000
        Catch ex As Exception
            lbStatus.Text = "CountTotal Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbWidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWidth.TextChanged, tbLength.TextChanged, tbSheet.TextChanged, tbGSM.TextChanged, tbRatio.TextChanged
    '    Try
    '        CountTotal()
    '    Catch ex As Exception
    '        lbStatus.Text = "tbWidth_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub EnabledFormulaNo()
        Try
            If ddlBomType.SelectedValue.Trim = "PE" Then
                tbFormulaProcess.Enabled = True
                btnFormulaProcess.Visible = True
            Else
                tbFormulaProcess.Enabled = False
                btnFormulaProcess.Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "BOMTypeChange Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnFormulaProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFormulaProcess.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT FormulaNo, FormulaName FROM V_PDBOMGetFormula"
            ResultField = "FormulaNo, FormulaName"

            ViewState("Sender") = "btnFormulaProcess"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnFormulaProcess_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFormulaProcess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFormulaProcess.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT DISTINCT FormulaNo, FormulaName FROM V_PDBOMGetFormula WHERE FormulaNo = " + QuotedStr(tbFormulaProcess.Text)
            
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbFormulaProcess, Dr("FormulaNo").ToString)
                tbFormulaName.Text = Dr("FormulaName").ToString
            Else
                tbFormulaProcess.Text = ""
                tbFormulaName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbSONo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaterialAlternate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialAlternate.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Product_Code AS MaterialCode, Product_Name AS MaterialName, Unit FROM VMsProduct WHERE Fg_Active = 'Y' AND ProductCategory = 'Material' "
            ResultField = "MaterialCode, MaterialName"
            ViewState("Sender") = "btnMaterialAlternate"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterialAlternate_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMaterialAlternate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterialAlternate.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT Product_Code AS MaterialCode, Product_Name AS MaterialName, Unit FROM VMsProduct WHERE Fg_Active = 'Y' AND ProductCategory = 'Material' AND Product_Code = " + QuotedStr(tbMaterialAlternate.Text)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToText(tbMaterialAlternate, Dr("MaterialCode").ToString)
                BindToText(tbMaterialAlternateName, Dr("MaterialName").ToString)                
            Else
                tbMaterialAlternate.Text = ""
                tbMaterialAlternateName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbMaterialAlternate_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableOutputType(ByVal SuccessorCode As String)
        If SuccessorCode = "" Then
            ddlOutputType.SelectedValue = "FG"
            tbProductOutputCode.Enabled = False
            btnProductOutput.Visible = False
            tbQtyOutput.Enabled = False
            tbQtyOutput.Enabled = False
        Else
            ddlOutputType.SelectedValue = "WIP"
            tbProductOutputCode.Enabled = True
            btnProductOutput.Visible = True
            tbQtyOutput.Enabled = True
            tbQtyOutput.Enabled = True
        End If
    End Sub

    Private Sub CekProductExists()       
        Dim SQLString, result As String
        Try
            SQLString = "Declare @A VarChar(255) EXEC S_PDBOMCekProduct " + QuotedStr(tbProductCode.Text) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
            result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)

            If result.Length > 2 Then
                lbStatus.Text = MessageDlg(result)
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecificationHd.Text = ""
                Exit Sub
            End If

        Catch ex As Exception
            Throw New Exception("CekProductExists Error : " + ex.ToString)
        End Try
    End Sub

End Class

