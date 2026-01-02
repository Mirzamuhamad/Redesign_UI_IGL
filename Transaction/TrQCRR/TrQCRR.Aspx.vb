Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrQCRR
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_QCRRHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select DISTINCT  TransNmbr, Nmbr, TransDate, Status, InspectionType, ReffType, ReffNmbr, InspectedParty, InspectedPartyName, InspectedWrhs, InspectedWrhsName, Remark From V_QCRRHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                ViewState("SetLocation") = False
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
                If ViewState("Sender") = "btnReffNmbr" Then
                    tbReffNmbr.Text = Session("Result")(0).ToString
                    tbParty.Text = Session("Result")(1).ToString
                    tbPartyName.Text = Session("Result")(2).ToString
                    ddlWrhs.SelectedValue = Session("Result")(3).ToString
                End If
                If ViewState("Sender") = "btnParty" Then
                    tbParty.Text = Session("Result")(0).ToString
                    tbPartyName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnProd" Then

                    If ddlReffType.SelectedValue = "" Then
                        tbProdCode.Text = Session("Result")(0).ToString
                        tbProdName.Text = Session("Result")(1).ToString
                        BindToText(tbSpec, Session("Result")(2).ToString)
                        ddlLocation.SelectedValue = ""
                        tbQty.Text = "0"
                        BindToText(tbUnit, Session("Result")(3).ToString)
                        tbIncomingDate.SelectedDate = ViewState("ServerDate")
                        tbInspectionDate.SelectedDate = ViewState("ServerDate")
                    Else
                        'Product, Product_Name, specification, Location, Qty, Unit, RR_Date
                        tbProdCode.Text = Session("Result")(0).ToString
                        tbProdName.Text = Session("Result")(1).ToString
                        BindToText(tbSpec, Session("Result")(2).ToString)
                        BindToDropList(ddlLocation, Session("Result")(3).ToString)
                        BindToText(tbQty, Session("Result")(4).ToString)
                        BindToText(tbUnit, Session("Result")(5).ToString)
                        BindToDate(tbIncomingDate, Session("Result")(6).ToString)
                        BindToDate(tbInspectionDate, Session("Result")(6).ToString)
                    End If
                End If
                If ViewState("Sender") = "btnDefect" Then
                    '"Defect_Code, Defect_Name, Defect_Group_Name, Standard"
                    tbDefect.Text = Session("Result")(0).ToString
                    tbDefectName.Text = Session("Result")(1).ToString
                    tbDefectGroup.Text = Session("Result")(2).ToString
                    tbStandard.Text = Session("Result")(3).ToString
                End If


                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product_Code")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Location") = ""
                            dr("Location_Name") = ""
                            dr("Qty") = "0"
                            dr("Unit") = drResult("Unit")
                            dr("SamplingSize") = ""
                            dr("QtySS") = "0"
                            dr("IncomingDate") = ViewState("ServerDate")
                            dr("InspectionDate") = ViewState("ServerDate")
                            dr("InspectedBy") = ViewState("UserId").ToString
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
                End If
                If ViewState("Sender") = "btnGetRR" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("ProductName") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Location") = drResult("Location")
                            dr("Location_Name") = drResult("Location_Name")
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            dr("SamplingSize") = ""
                            dr("QtySS") = "0"
                            dr("IncomingDate") = drResult("RR_Date")
                            dr("InspectionDate") = drResult("RR_Date")
                            dr("InspectedBy") = ViewState("UserId").ToString
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
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
        FillCombo(ddlSamplingSize, "Select SamplingCode, SamplingName from VMsSampling", True, "SamplingCode", "SamplingName", ViewState("DBConnection"))
        FillCombo(ddlWrhs, "SELECT Wrhs_Code, Wrhs_Name FROM VMsWarehouse WHERE Wrhs_Type IN ( 'Owner', 'Deposit Out')", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat();")
        Me.tbQtySS.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtySS.Attributes.Add("OnBlur", "setformat();")
        Me.tbSpec.Attributes.Add("ReadOnly", "True")
        Me.tbUnit.Attributes.Add("ReadOnly", "True")
        Me.tbStandard.Attributes.Add("ReadOnly", "True")
        Me.tbDefectGroup.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * From V_QCRRDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtPartial(ByVal Nmbr As String) As String
        Return "SELECT * From V_QCRRDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
    Public Function ExecSPCekPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal UserId As String, Optional ByVal Connection As String = "Nothing") As DataTable
        Dim Mycon As New SqlConnection
        Dim DT As DataTable

        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Mycon = New SqlConnection(Connection)

        Dim sqlstring As String
        sqlstring = ""
        If PrimaryKey.Length = 1 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 2 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 3 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(UserId)
        End If
        Try
            DT = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim sql As New SqlCommand
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


                Session("SelectCommand") = "EXEC S_PRFormPR " + Result + "'"
                'If Result.Trim = "" Then
                '    Session("SelectCommand") = "EXEC S_PRFormPR '''0''' "
                'End If

                'Dim CekResult As String = DirectCast(SQLExecuteScalar(Session("SelectCommand"), ViewState("DBConnection").ToString), String)
                If Result.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Data Selected")
                    Exit Sub
                End If


                Session("ReportFile") = ".../../../Rpt/FormPR.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
            ElseIf ActionValue = "Print Schedule" Then
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

                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_PRFormPRSchedule " + Result + "'"
                'If Result.Trim = "" Then
                '    Session("SelectCommand") = "EXEC S_PRFormPR '''0''' "
                'End If

                'Dim CekResult As String = DirectCast(SQLExecuteScalar(Session("SelectCommand"), ViewState("DBConnection").ToString), String)
                If Result.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Data Selected")
                    Exit Sub
                End If


                Session("ReportFile") = ".../../../Rpt/FormPRSchedule.frx"
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
                    ElseIf ActionValue = "Post" Then
                        Result = ExecSPCommandGo(ActionValue, "S_QCRR", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                        'End If
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_QCRR", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnGetDt.Visible = State
            ddlWrhs.Enabled = State
            ddlReffType.Enabled = State
            tbReffNmbr.Enabled = State
            btnReffNmbr.Enabled = State
            tbParty.Enabled = ddlReffType.SelectedValue = "" And State
            btnParty.Enabled = ddlReffType.SelectedValue = "" And State
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

    Private Sub BindDataDtPartial(ByVal Nmbr As String)
        Dim dt As New DataTable
        Dim Drow As DataRow()
        Try
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDtPartial(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt

            Drow = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridPartial)
                GridPartial.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridPartial.DataSource = DtTemp
                GridPartial.DataBind()
                GridPartial.Columns(0).Visible = False
            End If
            'If ViewState("StateHd") = "View" And GridDt2.Columns(0).Visible Then
            '    GridDt2.Columns(0).Visible = False
            'Else
            '    GridDt2.Columns(0).Visible = True
            'End If
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
                If ViewState("DtValue") <> tbProdCode.Text Then
                    If CekExistData(ViewState("Dt"), "Product", tbProdCode.Text) Then
                        lbStatus.Text = "Product '" + tbProdCode.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProdCode.Text
                Row("ProductName") = tbProdName.Text
                Row("Specification") = tbSpec.Text
                Row("Location") = ddlLocation.SelectedValue
                Row("Location_Name") = ddlLocation.SelectedItem.Text
                Row("IncomingDate") = tbIncomingDate.SelectedDate
                Row("InspectionDate") = tbInspectionDate.SelectedDate
                Row("InspectedBy") = tbInspectionBy.Text
                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                Row("Unit") = tbUnit.Text
                Row("SamplingSize") = ddlSamplingSize.SelectedValue
                Row("QtySS") = FormatFloat(tbQtySS.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemarkDt.Text

                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product", tbProdCode.Text) Then
                    lbStatus.Text = "Product '" + tbProdCode.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProdCode.Text
                dr("ProductName") = tbProdName.Text
                dr("Specification") = tbSpec.Text
                dr("Location") = ddlLocation.SelectedValue
                dr("Location_Name") = ddlLocation.SelectedItem.Text
                dr("IncomingDate") = tbIncomingDate.SelectedDate
                dr("InspectionDate") = tbInspectionDate.SelectedDate
                dr("InspectedBy") = tbInspectionBy.Text
                dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                dr("Unit") = tbUnit.Text
                dr("SamplingSize") = ddlSamplingSize.SelectedValue
                dr("QtySS") = FormatFloat(tbQtySS.Text, ViewState("DigitQty"))
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
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
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
                tbTransNo.Text = GetAutoNmbr("QCIR", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = GetAutoNmbr("PR", "N", CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), Dep2digit, ViewState("DBConnection").ToString)
                'tbRef.Text = GetAutoNmbr("PR", ddlReport.SelectedValue, CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO QCRRHd (TransNmbr, STATUS, Transdate, InspectionType, ReffType, ReffNmbr, InspectedParty, InspectedWrhs, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlInspectionType.SelectedValue) + ", " + QuotedStr(ddlReffType.SelectedValue) + ", " + QuotedStr(tbReffNmbr.Text) + ", " + QuotedStr(tbParty.Text) + ", " + QuotedStr(ddlWrhs.SelectedValue) + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("Reference") = tbTransNo.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM QCRRHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE QCRRHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', InspectionType = '" + ddlInspectionType.SelectedValue + "'," + _
                " ReffType = " + QuotedStr(ddlReffType.SelectedValue) + ", ReffNmbr = " + QuotedStr(tbReffNmbr.Text) + ", InspectedWrhs = " + QuotedStr(ddlWrhs.SelectedValue) + ", InspectedParty = " + QuotedStr(tbParty.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = '" + tbTransNo.Text + "'"
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
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next
            If Not ViewState("Dt2") Is Nothing Then
                Row = ViewState("Dt2").Select("TransNmbr IS NULL")
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Location, Qty, Unit, SamplingSize, QtySS, IncomingDate, InspectionDate, InspectedBy, Remark FROM QCRRDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("QCRRDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2

            Dim SqlString2 As String
            If Not ViewState("Dt2") Is Nothing Then
                SqlString2 = " SELECT TransNmbr, Product, ItemNo, Defect, Result, StatusOK, Remark FROM QCRRDt2 WHERE TransNmbr = " + QuotedStr(ViewState("Reference"))

                cmdSql = New SqlCommand(SqlString2, con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim DtPartial As New DataTable("QCRRDt2")

                DtPartial = ViewState("Dt2")
                da.Update(DtPartial)
                DtPartial.AcceptChanges()
                ViewState("Dt2") = DtPartial
            End If

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        'Dim Row As DataRow
        Dim ExistRow As DataRow()
        Try
            If CekHd() = False Then
                Exit Sub
            End If

            'BindDataDt(ViewState("Dt"))
            'Row = ViewState("Dt2").Select("Product = " + QuotedStr(""))(0)
            'GetCountRecord(ViewState("Dt2").Select("Product = " + QuotedStr(dr("Product"))))
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    ExistRow = ViewState("Dt2").Select("Product = " + QuotedStr(dr("Product")))
                    If ExistRow.Count = 0 Then
                        lbStatus.Text = MessageDlg("Detail QC must have at least 1 record for Product " + dr("Product"))
                        Exit Sub
                    End If
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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("Reference") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            'GridDt.Columns(1).Visible = False
            BindDataDt("")
            BindDataDtPartial("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            ViewState("SetLocation") = True
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlWrhs.SelectedValue = ""
            ddlWrhs.Enabled = True
            ddlInspectionType.SelectedIndex = 0
            tbParty.Text = ""
            tbPartyName.Text = ""
            ddlReffType.SelectedValue = ""
            tbReffNmbr.Text = ""
            btnReffNmbr.Visible = ddlReffType.SelectedValue <> ""
            tbParty.Enabled = ddlReffType.SelectedValue = ""
            btnParty.Visible = tbParty.Enabled
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProdCode.Text = ""
            tbProdName.Text = ""
            tbSpec.Text = ""
            ddlLocation.SelectedValue = ""
            tbRemarkDt.Text = ""
            tbUnit.Text = ""
            tbQty.Text = "0"
            tbQtySS.Text = "0"
            ddlSamplingSize.SelectedValue = ""
            'PnlInfo.Visible = False
            'PnlInfo2.Visible = False
            tbIncomingDate.SelectedDate = ViewState("ServerDate")
            tbInspectionDate.SelectedDate = ViewState("ServerDate")
            tbInspectionBy.Text = ViewState("UserId").ToString
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        If ViewState("SetLocation") Then
            FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ViewState("SetLocation") = False
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Protected Sub btnAddPartial_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddPartial.Click, btnAddDt2Ke2.Click
        Dim dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If lbProductDt2.Text = "" Or lbProductDt2.Text = "&nbsp;" Then
                Exit Sub
            End If
            Cleardt2()
            'untuk ambil data di Dt
            dr = ViewState("Dt").Select("Product=" + QuotedStr(lbProductDt2.Text))(0)
            lbItemNo.Text = GetNewItemNo2(ViewState("Dt2"), lbProductDt2.Text)
            ViewState("StateDtPartial") = "Insert"
            MovePanel(PnlPartial, pnlEditPartial)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbDefect.Text = ""
            tbDefectName.Text = ""
            tbDefectGroup.Text = ""
            tbStandard.Text = ""
            tbResult.Text = ""
            ddlStatusQC.SelectedValue = "Y"
            tbRemarkDt2.Text = ""
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
            If tbParty.Text = "" And ddlReffType.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Party must have value")
                tbParty.Focus()
                Return False
            End If
            If tbReffNmbr.Text = "" And ddlReffType.SelectedValue <> "" Then
                lbStatus.Text = MessageDlg("Reference must have value")
                tbReffNmbr.Focus()
                Return False
            End If
            If ddlWrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlWrhs.Focus()
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
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    Return False
                End If

                If TrimStr(Dr("Location").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    Return False
                End If
                If TrimStr(Dr("SamplingSize").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Sampling Size Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtySS").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty SS Must Have Value")
                    Return False
                End If
            Else
                If tbProdCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Code Must Have Value")
                    tbProdCode.Focus()
                    Return False
                End If
                If ddlLocation.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    ddlLocation.Focus()
                    Return False
                End If

                If ddlSamplingSize.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Sampling Size Must Have Value")
                    ddlSamplingSize.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtySS.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty SS Must Have Value")
                    tbQtySS.Focus()
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
            FilterName = "Reference, Date, Request Type, Warehouse, Request By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), RequestType, Warehouse, RequestBy, Remark"
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
                    'If ViewState("Status") = "P" Then
                    '    GridDt.Columns(1).Visible = True
                    'Else
                    '    GridDt.Columns(1).Visible = False
                    'End If

                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    'lbStatus.Text = ViewState("Reference")
                    'Exit Sub


                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    BindDataDtPartial(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    btnAddPartial.Visible = False
                    btnAddDt2Ke2.Visible = False
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, PnlPartial, GridPartial)
                    btnHome.Visible = True

                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        'GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataDtPartial(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        btnAddPartial.Visible = True
                        btnAddDt2Ke2.Visible = True
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
                        Session("SelectCommand") = "EXEC S_PRFormPR ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPR.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Schedule" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRFormPRSchedule " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormPRSchedule.frx"
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
            ElseIf e.CommandName = "DetailPartial" Then

                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                lbProductDt2.Text = GVR.Cells(2).Text
                lbProductDtName2.Text = " - " + GVR.Cells(3).Text
                lbQtyDt2.Text = GVR.Cells(6).Text + " " + GVR.Cells(7).Text
                lbSamplingSize.Text = GVR.Cells(8).Text

                If lbSamplingSize.Text = "" Then
                    lbStatus.Text = "Sampling Size must have values"
                    Exit Sub
                End If
                ViewState("DtValue") = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1

                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDtPartial(ViewState("Reference"))
                Else
                    drow = ViewState("Dt2").Select("Product=" + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridPartial)
                        GridPartial.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridPartial.DataSource = DtTemp
                        GridPartial.DataBind()
                        GridPartial.Columns(0).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim dr2() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()

        dr2 = ViewState("Dt2").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
        For I = 0 To dr2.Count - 1
            dr2(I).Delete()
        Next
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = tbProdCode.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            'GetInfo()
            tbProdCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            ViewState("InputCostCtr") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMaster('MsCostCtr')();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlCostCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostCtr.SelectedIndexChanged
    '    If ViewState("InputCostCtr") = "Y" Then
    '        RefreshMaster("S_GetCostCtr", "Cost_Ctr_Code", "Cost_Ctr_Name", ddlCostCtr, ViewState("DBConnection"))
    '        ViewState("InputCostCtr") = Nothing
    '    End If
    'End Sub

    '' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                ' add the UnitPrice and QuantityTotal to the running total variables
    '                CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
    '                'CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
    '                DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
    '                'DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                e.Row.Cells(7).Text = "Total:"
    '                ' for the Footer, display the running totals
    '                e.Row.Cells(8).Text = Format(DbHome, "###,##0.##")
    '                e.Row.Cells(10).Text = Format(CrHome, "###,##0.##")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlInspectionType, Dt.Rows(0)("InspectionType").ToString)
            BindToDropList(ddlReffType, Dt.Rows(0)("ReffType").ToString)
            BindToText(tbReffNmbr, Dt.Rows(0)("ReffNmbr").ToString)
            BindToText(tbParty, Dt.Rows(0)("InspectedParty").ToString)
            BindToText(tbPartyName, Dt.Rows(0)("InspectedPartyName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlWrhs, Dt.Rows(0)("InspectedWrhs").ToString)
            btnReffNmbr.Visible = ddlReffType.SelectedValue <> ""
            tbParty.Enabled = ddlReffType.SelectedValue = ""
            btnParty.Visible = tbParty.Enabled
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbProdCode, Dr(0)("Product").ToString)
                BindToText(tbProdName, Dr(0)("ProductName").ToString)
                BindToText(tbSpec, Dr(0)("Specification").ToString)
                If ViewState("SetLocation") Then
                    FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                    ViewState("SetLocation") = False
                End If
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                BindToDropList(ddlSamplingSize, Dr(0)("SamplingSize").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbQtySS, Dr(0)("QtySS").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToDate(tbIncomingDate, Dr(0)("IncomingDate").ToString)
                BindToDate(tbInspectionDate, Dr(0)("InspectionDate").ToString)
                BindToText(tbInspectionBy, Dr(0)("InspectedBy").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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

    Protected Sub btnProd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProd.Click
        Dim ResultField As String
        Try
            If ddlReffType.SelectedValue = "" Then
                Session("filter") = "Select * from VMsProduct WHERE FgQC = 'Y' AND Fg_Active = 'Y' "
                ResultField = "Product_Code, Product_Name, specification, Unit"
            Else
                Session("filter") = "Select Product, Product_Name, Specification, Location, Location_Name, Qty, Unit, RR_Date from V_QCRRReff WHERE RR_No = " + QuotedStr(tbReffNmbr.Text)
                ResultField = "Product, Product_Name, specification, Location, Qty, Unit, RR_Date"
            End If
            ViewState("Sender") = "btnProd"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProdCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProdCode.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            If ddlReffType.SelectedValue = "" Then
                dt = SQLExecuteQuery("Select Product_Code, Product_Name, Specification, Unit from VMsProduct WHERE FgQC = 'Y' AND Fg_Active = 'Y' and Product_Code = " + QuotedStr(tbProdCode.Text), ViewState("DBConnection").ToString).Tables(0)
                If dt.Rows.Count > 0 Then
                    Dr = dt.Rows(0)
                    tbProdCode.Text = Dr("Product_Code").ToString
                    tbProdName.Text = Dr("Product_Name").ToString
                    BindToText(tbSpec, Dr("Specification").ToString)
                    BindToText(tbUnit, Dr("Unit").ToString)
                Else
                    tbProdCode.Text = ""
                    tbProdName.Text = ""
                    tbSpec.Text = ""
                    ddlLocation.SelectedValue = ""
                    tbQty.Text = "0"
                    tbUnit.Text = ""
                    ddlSamplingSize.SelectedValue = ""
                    tbQtySS.Text = "0"
                End If
            Else
                dt = SQLExecuteQuery("Select Product, Product_Name, Specification, Location, Location_Name, Qty, Unit, RR_Date from V_QCRRReff WHERE RR_No = " + QuotedStr(tbReffNmbr.Text) + " and Product_Code = " + QuotedStr(tbProdCode.Text), ViewState("DBConnection").ToString).Tables(0)
                If dt.Rows.Count > 0 Then
                    Dr = dt.Rows(0)
                    tbProdCode.Text = Dr("Product").ToString
                    tbProdName.Text = Dr("Product_Name").ToString
                    BindToText(tbSpec, Dr("Specification").ToString)
                    BindToDropList(ddlLocation, Dr("Location").ToString)
                    BindToText(tbQty, Dr("Qty").ToString)
                    BindToText(tbUnit, Dr("Unit").ToString)
                    BindToDate(tbIncomingDate, Dr("RR_Date"))
                    BindToDate(tbInspectionDate, Dr("RR_Date"))
                Else
                    tbProdCode.Text = ""
                    tbProdName.Text = ""
                    tbSpec.Text = ""
                    ddlLocation.SelectedValue = ""
                    tbQty.Text = "0"
                    tbUnit.Text = ""
                    ddlSamplingSize.SelectedValue = ""
                    tbQtySS.Text = "0"
                End If
            End If
            tbProdCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb acc Code Error : " + ex.ToString)
        End Try
    End Sub

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
    Protected Sub FillTextBoxDt2(ByVal Product As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Product = " + QuotedStr(Product) + " AND ItemNo =" + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbDefect, Dr(0)("Defect").ToString)
                BindToText(tbDefectName, Dr(0)("Defect_Name").ToString)
                BindToText(tbDefectGroup, Dr(0)("Defect_Group_Name").ToString)
                BindToText(tbStandard, Dr(0)("Standard").ToString)
                BindToText(tbResult, Dr(0)("Result").ToString)
                BindToDropList(ddlStatusQC, Dr(0)("StatusOK").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
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
                If Dr("Defect").ToString = "" Then
                    lbStatus.Text = MessageDlg("Defect Must Have Value")
                    Return False
                End If
            Else
                If tbDefect.Text = "" Then
                    lbStatus.Text = MessageDlg("Defect Must Have Value")
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
            ExistRow = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text) + " and ItemNo = " + lbItemNo.Text)
            If ViewState("StateDtPartial") = "Edit" Then
                Dim Row As DataRow
                'If ExistRow.Count > AllowedRecordDt2() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If
                Row = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text) + " and ItemNo = " + ViewState("ItemNo").ToString)(0)
                Row.BeginEdit()
                Row("Product") = lbProductDt2.Text
                Row("ItemNo") = lbItemNo.Text
                Row("Defect") = tbDefect.Text
                Row("Defect_Name") = tbDefectName.Text
                Row("Defect_Group_Name") = tbDefectGroup.Text
                Row("Standard") = tbStandard.Text
                Row("StatusOK") = ddlStatusQC.SelectedValue
                Row("Result") = tbResult.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If
                dr = ViewState("Dt2").NewRow
                dr("Product") = lbProductDt2.Text
                dr("ItemNo") = lbItemNo.Text
                dr("Defect") = tbDefect.Text
                dr("Defect_Name") = tbDefectName.Text
                dr("Defect_Group_Name") = tbDefectGroup.Text
                dr("Standard") = tbStandard.Text
                dr("StatusOK") = ddlStatusQC.SelectedValue
                dr("Result") = tbResult.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditPartial, PnlPartial)
            EnableHd(Not DtExist())
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridPartial)
                GridPartial.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridPartial.DataSource = DtTemp
                GridPartial.DataBind()
                GridPartial.Columns(0).Visible = False
            End If
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save dtPartial Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnbackDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbackDt3.Click, btnBackPartial.Click
        'Dim Row As DataRow
        Try
            'If ViewState("StateHd") <> "View" Then


            '    Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)

            '    If Row("FgPartial") = "Y" Then


            '        'Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
            '        Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
            '        'lbStatus.Text = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0).ToString
            '        'lbStatus.Text = Row("Product")
            '        'Exit Sub

            '        Row.BeginEdit()
            '        Row("QtyOrder") = FormatFloat(lbQtyOrderDt2.Text, ViewState("DigitQty"))
            '        Row("Qty") = FormatFloat(FindConvertUnit(Row("Product"), Row("UnitOrder"), Row("QtyOrder"), ViewState("DBConnection")), ViewState("DigitQty"))
            '        Row.EndEdit()

            '        BindGridDt(ViewState("Dt"), GridDt)

            '    End If


            'End If

            'lbStatus.Text = Row("QtyOrder")
            'Exit Sub
            MultiView1.ActiveViewIndex = 0



        Catch ex As Exception
            lbStatus.Text = "Back dt Error"

        End Try


    End Sub

    Protected Sub GridPartial_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridPartial.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridPartial.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Product = " + QuotedStr(lbProductDt2.Text) + " AND ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            EnableHd(Not DtExist())
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("Product=" + QuotedStr(lbProductDt2.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridPartial)
                GridPartial.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridPartial.DataSource = DtTemp
                GridPartial.DataBind()
                GridPartial.Columns(0).Visible = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridPartial_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridPartial.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridPartial.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbProductDt2.Text, GVR.Cells(1).Text)
            'Format(CDate(GVR.Cells(1).Text), "dd MMM yyyy")
            'GetInfo2()
            MovePanel(PnlPartial, pnlEditPartial)
            EnableHd(False)
            ViewState("StateDtPartial") = "Edit"
            ViewState("ItemNo") = GVR.Cells(1).Text
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Part Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditPartial, PnlPartial)
            EnableHd(Not DtExist())
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField As String 'ResultSame 
        Try
            If Not CekHd() Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If ddlReffType.SelectedValue = "" Then
                Session("filter") = "Select * from VMsProduct WHERE FgQC = 'Y' AND Fg_Active = 'Y' "
                ResultField = "Product_Code, Product_Name, Specification, Unit"
                ViewState("Sender") = "btnGetDt"
            Else
                Session("filter") = "Select Product, Product_Name, Specification, Location, Location_Name, Qty, Unit, RR_Date from V_QCRRReff WHERE RR_No = " + QuotedStr(tbReffNmbr.Text)
                ResultField = "Product, Product_Name, Specification, Location, Location_Name, Qty, Unit, RR_Date"
                ViewState("Sender") = "btnGetRR"
            End If
            Session("Column") = ResultField.Split(",")

            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnParty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnParty.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select * FROM VMsSupplier WHERE FgActive = 'Y' "
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnParty"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnParty_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbParty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbParty.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            dt = SQLExecuteQuery("Select Supplier_Code, Supplier_Name FROM VMsSupplier WHERE FgActive = 'Y' and Supplier_Code = " + QuotedStr(tbParty.Text), ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbParty.Text = Dr("Supplier_Code").ToString
                tbPartyName.Text = Dr("Supplier_Name").ToString
            Else
                tbParty.Text = ""
                tbPartyName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbParty_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlReffType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReffType.SelectedIndexChanged
        Try
            btnReffNmbr.Visible = ddlReffType.SelectedValue <> ""
            tbParty.Enabled = ddlReffType.SelectedValue = ""
            btnParty.Visible = tbParty.Enabled
        Catch ex As Exception
            Throw New Exception("ddlReffType_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnReffNmbr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReffNmbr.Click
        Dim ResultField, Type As String
        Try
            Type = ""
            If ddlReffType.SelectedValue = "RR PO" Then
                Type = "RR"
            ElseIf ddlReffType.SelectedValue = "RR Service" Then
                Type = "RS"
            End If
            Session("filter") = "Select DISTINCT RR_No, dbo.FormatDate(RR_Date) AS RR_Date, Supplier, Supplier_Name, PO_No, Warehouse, Warehouse_Name, SJSuppNo, Remark from V_QCRRReff WHERE RRType = " + QuotedStr(Type)
            ResultField = "RR_No, Supplier, Supplier_Name, Warehouse"
            ViewState("Sender") = "btnReffNmbr"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnReffNmbr_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlSamplingSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSamplingSize.SelectedIndexChanged
        Dim hasil As String
        Try
            hasil = SQLExecuteScalar("SELECT dbo.GetQtySS(" + QuotedStr(ddlSamplingSize.SelectedValue) + ", " + tbQty.Text.Replace(",", "") + ")", ViewState("DBConnection").ToString)
            tbQtySS.Text = FormatFloat(CFloat(hasil), ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("ddlSamplingSize_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnDefect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefect.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Defect_Code, Defect_Name, Defect_Group_Name, Standard, Acceptable, MinReject FROM VMsSamplingAccept WHERE Sampling = " + QuotedStr(lbSamplingSize.Text)
            ResultField = "Defect_Code, Defect_Name, Defect_Group_Name, Standard"
            ViewState("Sender") = "btnDefect"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnParty_Click Error : " + ex.ToString
        End Try

    End Sub

    Public Function GetNewItemNo2(ByVal Dt As DataTable, ByVal Product As String) As String
        Dim Row As DataRow()
        Dim R As DataRow
        Dim MaxItem As Integer = 0
        Row = Dt.Select("Product = " + QuotedStr(Product))
        For Each R In Row
            If CInt(R("ItemNo").ToString) > MaxItem Then
                MaxItem = CInt(R("ItemNo").ToString)
            End If
        Next
        MaxItem = MaxItem + 1
        Return CStr(MaxItem)
    End Function

    Protected Sub tbDefect_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDefect.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            dt = SQLExecuteQuery("Select Defect_Code, Defect_Name, Defect_Group_Name, Standard, Acceptable, MinReject FROM VMsSamplingAccept WHERE Sampling = " + QuotedStr(lbSamplingSize.Text) + " AND Defect_Code = " + QuotedStr(tbDefect.Text), ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbDefect.Text = Dr("Defect_Code").ToString
                tbDefectName.Text = Dr("Defect_Name").ToString
                tbStandard.Text = Dr("Standard").ToString
                tbDefectGroup.Text = Dr("Defect_Group_Name").ToString
            Else
                tbDefect.Text = ""
                tbDefectName.Text = ""
                tbStandard.Text = ""
                tbDefectGroup.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbDefect_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
