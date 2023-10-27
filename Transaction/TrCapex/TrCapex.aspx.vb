Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrCapex_TrCapex
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLCapexHd"


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
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
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCategory" Then
                    tbCategoryCode.Text = Session("Result")(0).ToString
                    tbCategoryName.Text = Session("Result")(1).ToString
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error : " + ex.ToString
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
        Try
            FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlDepartment, "SELECT Dept_code, Dept_Name FROM VMsCostDept", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection"))
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tb01.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb02.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb03.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb04.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb05.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb06.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb07.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb08.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb09.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb10.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb11.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb12.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal01.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal02.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal03.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal04.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal05.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal06.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal07.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal08.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal09.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal10.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal11.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal12.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
            
            tb01.Attributes.Add("OnBlur", "setformatdt();")
            tb02.Attributes.Add("OnBlur", "setformatdt();")
            tb03.Attributes.Add("OnBlur", "setformatdt();")
            tb04.Attributes.Add("OnBlur", "setformatdt();")
            tb05.Attributes.Add("OnBlur", "setformatdt();")
            tb06.Attributes.Add("OnBlur", "setformatdt();")
            tb07.Attributes.Add("OnBlur", "setformatdt();")
            tb08.Attributes.Add("OnBlur", "setformatdt();")
            tb09.Attributes.Add("OnBlur", "setformatdt();")
            tb10.Attributes.Add("OnBlur", "setformatdt();")
            tb11.Attributes.Add("OnBlur", "setformatdt();")
            tb12.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal01.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal02.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal03.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal04.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal05.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal06.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal07.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal08.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal09.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal10.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal11.Attributes.Add("OnBlur", "setformatdt();")
            tbTotal12.Attributes.Add("OnBlur", "setformatdt();")
            tbPrice.Attributes.Add("OnBlur", "setformatdt();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
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
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
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
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Year DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Year As String, ByVal Revisi As String) As String
        Return "SELECT * From V_GLCapexDt WHERE Year = " + Year + " AND Revisi =" + Revisi
    End Function

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
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "3,2,4", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_GLCapex", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("LTRIM(STR(Year))+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlYear.Enabled = State
            'btnGetData.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Year As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Year, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDtExtended()
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
    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            If ViewState("StateDt") = "ChangeClass" Then
                StatusButtonSave(False)
            Else
                StatusButtonSave(True)
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("CapexCategory = " + QuotedStr(tbCategoryCode.Text) + " AND Department = " + QuotedStr(ddlDepartment.SelectedValue))

            If ViewState("StateDt") = "ChangeClass" Then
                'Dim Msg As String
                'Msg = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_PDForecastYearChange " + ddlYear.SelectedValue + ", " + lbRevisi.Text + ", " + QuotedStr(ViewState("CurrProductClass").ToString) + ", " + QuotedStr(ViewState("CurrProductSize").ToString) + ", " + QuotedStr(ViewState("CurrProductPrice").ToString) + ", " + QuotedStr(ddlClass.SelectedValue) + ", " + QuotedStr(ddlSize.SelectedValue) + ", " + QuotedStr(ddlPrice.SelectedValue) + ", @A Out ", Session("DBConnection").ToString)
                'If Msg.Length > 5 Then
                '    lbStatus.Text = Msg
                '    Exit Sub
                'End If
                'Dim Row As DataRow
                'If ExistRow.Count > AllowedRecord() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If
                'Row = ViewState("Dt").Select("ProductClass = " + QuotedStr(ViewState("CurrProductClass")) + " AND ProductSize = " + QuotedStr(ViewState("CurrProductSize")) + " AND ProductPrice = " + QuotedStr(ViewState("CurrProductPrice")))(0)
                'If CekDt() = False Then
                '    btnSaveDt.Focus()
                '    Exit Sub
                'End If
                'Row.BeginEdit()
                'Row("ProductClass") = ddlClass.SelectedValue
                'Row("ProductClassName") = ddlClass.SelectedItem.Text
                'Row("ProductSize") = ddlSize.SelectedValue
                'Row("ProductSizeName") = ddlSize.SelectedItem.Text
                'Row("ProductPrice") = ddlPrice.SelectedValue
                'Row("ProductPriceName") = ddlPrice.SelectedItem.Text
                'Row("Remark") = tbRemarkDt.Text.Trim
                'Row.EndEdit()

                'ViewState("CurrProductClass") = Nothing
                'ViewState("CurrProductSize") = Nothing
                'ViewState("CurrProductPrice") = Nothing
            ElseIf ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                If ExistRow.Count > AllowedRecord() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt").Select("CapexCategory = " + QuotedStr(ViewState("CapexCategory")) + " AND Department = " + QuotedStr(ViewState("Department")))(0)
                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("CapexCategory") = tbCategoryCode.Text
                Row("CapexCategoryName") = tbCategoryName.Text
                Row("Department") = ddlDepartment.SelectedValue
                Row("DeptName") = ddlDepartment.SelectedItem.Text
                Row("Price") = tbPrice.Text
                Row("Qty01") = tb01.Text
                Row("Qty02") = tb02.Text
                Row("Qty03") = tb03.Text
                Row("Qty04") = tb04.Text
                Row("Qty05") = tb05.Text
                Row("Qty06") = tb06.Text
                Row("Qty07") = tb07.Text
                Row("Qty08") = tb08.Text
                Row("Qty09") = tb09.Text
                Row("Qty10") = tb10.Text
                Row("Qty11") = tb11.Text
                Row("Qty12") = tb12.Text
                Row("Total01") = tbTotal01.Text
                Row("Total02") = tbTotal02.Text
                Row("Total03") = tbTotal03.Text
                Row("Total04") = tbTotal04.Text
                Row("Total05") = tbTotal05.Text
                Row("Total06") = tbTotal06.Text
                Row("Total07") = tbTotal07.Text
                Row("Total08") = tbTotal08.Text
                Row("Total09") = tbTotal09.Text
                Row("Total10") = tbTotal10.Text
                Row("Total11") = tbTotal11.Text
                Row("Total12") = tbTotal12.Text
                Row("Qty") = tbQty.Text
                Row("Total") = tbTotal.Text
                Row.EndEdit()

                ViewState("ProductClass") = Nothing
            Else
                'Insert
                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("CapexCategory") = tbCategoryCode.Text
                dr("CapexCategoryName") = tbCategoryName.Text
                dr("Department") = ddlDepartment.SelectedValue
                dr("DeptName") = ddlDepartment.SelectedItem.Text
                dr("Price") = tbPrice.Text
                dr("Qty01") = tb01.Text
                dr("Qty02") = tb02.Text
                dr("Qty03") = tb03.Text
                dr("Qty04") = tb04.Text
                dr("Qty05") = tb05.Text
                dr("Qty06") = tb06.Text
                dr("Qty07") = tb07.Text
                dr("Qty08") = tb08.Text
                dr("Qty09") = tb09.Text
                dr("Qty10") = tb10.Text
                dr("Qty11") = tb11.Text
                dr("Qty12") = tb12.Text
                dr("Total01") = tbTotal01.Text
                dr("Total02") = tbTotal02.Text
                dr("Total03") = tbTotal03.Text
                dr("Total04") = tbTotal04.Text
                dr("Total05") = tbTotal05.Text
                dr("Total06") = tbTotal06.Text
                dr("Total07") = tbTotal07.Text
                dr("Total08") = tbTotal08.Text
                dr("Total09") = tbTotal09.Text
                dr("Total10") = tbTotal10.Text
                dr("Total11") = tbTotal11.Text
                dr("Total12") = tbTotal12.Text
                dr("Qty") = tbQty.Text
                dr("Total") = tbTotal.Text
                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDtExtended()
            If ViewState("StateDt") = "ChangeMotif" Then
                StatusButtonSave(False)
            Else
                StatusButtonSave(True)
            End If
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub


    'Private Sub AddDetailToDt2(ByVal month As Integer, ByVal Qty As String)
    '    Dim dr As DataRow
    '    Try
    '        dr = ViewState("Dt2").NewRow
    '        dr("ProductClass") = ddlClass.SelectedValue
    '        dr("Month") = month
    '        dr("Days") = 1
    '        dr("Qty") = Qty
    '        ViewState("Dt2").Rows.Add(dr)
    '    Catch ex As Exception
    '        Throw New Exception("AddDetailToDt2 Error " + ex.ToString)
    '    End Try
    'End Sub
    'Private Sub EditDetailToDt2(ByVal bulan As Integer, ByVal Qty As String)
    '    Dim drow As DataRow()
    '    Dim dr As DataRow
    '    Dim pertamax As Boolean
    '    Try
    '        drow = ViewState("Dt2").Select("ProductClass = " + QuotedStr(ddlClass.SelectedValue) + " AND Month = " + bulan.ToString)
    '        pertamax = True
    '        If drow.Length > 0 Then
    '            For Each dr In drow
    '                If Not dr.RowState = DataRowState.Deleted Then
    '                    If Qty = "0" Then
    '                        dr.Delete()
    '                    Else
    '                        dr.BeginEdit()
    '                        If pertamax Then
    '                            dr("Qty") = Qty
    '                            pertamax = False
    '                        Else
    '                            dr("Qty") = "0"
    '                        End If
    '                        dr.EndEdit()
    '                    End If
    '                End If
    '            Next
    '        Else
    '            If Not Qty = "0" Then
    '                AddDetailToDt2(bulan, Qty)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception("EditDetailToDt2 Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Private Sub SumToDt(ByVal bulan As String)
    '    Dim drow As DataRow()
    '    Dim dr As DataRow
    '    Dim tot As Double
    '    Dim Prefix As String
    '    Try
    '        drow = ViewState("Dt2").Select("ProductClass =" + QuotedStr(lbProductClassDt2.Text) + " AND Month = " + bulan)
    '        tot = 0
    '        For Each dr In drow
    '            If Not dr.RowState = DataRowState.Deleted Then
    '                tot = tot + CFloat(dr("Qty").ToString)
    '            End If
    '        Next

    '        drow = ViewState("Dt").Select("ProductClass =" + QuotedStr(lbProductClassDt2.Text))

    '        dr = drow(0)

    '        If bulan = "10" Or bulan = "11" Or bulan = "12" Then
    '            Prefix = "Qty"
    '        Else
    '            Prefix = "Qty0"
    '        End If
    '        dr.BeginEdit()
    '        dr(Prefix + bulan) = FormatFloat(tot, CInt(Session("Digit")("Qty")))
    '        dr.EndEdit()

    '    Catch ex As Exception
    '        lbStatus.Text = "Sum to Dt Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function AllowedRecord() As Integer
        Try
            If ViewState("CapexCategory") = tbCategoryCode.Text And ViewState("Department") = ddlDepartment.SelectedValue Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

    Private Sub SaveAll()
        Dim SQLString, CekTrans As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = MessageDlg("Detail Data must be saved first")
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
                'tbPFI.Text = GetAutoNmbr("SF", "N", CInt(Session("GLYear")), CInt(Session("GLPeriod")), "", Session("DBConnection").ToString)

                CekTrans = SQLExecuteScalar("SELECT COUNT(YEAR) FROM GLCapexHd WHERE YEAR = " + ddlYear.SelectedValue, ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Capex for " + ddlYear.SelectedValue + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO GLCapexHd (Year, Status, Revisi, Remark, UserPrep, DatePrep, FgActive) " + _
                "SELECT " + ddlYear.SelectedValue + ", 'H', 0, " + _
                QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), 'Y'"

                ViewState("Year") = ddlYear.SelectedValue
                ViewState("Revisi") = "0"
            Else
                SQLString = "UPDATE GLCapexHd SET Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE Year = " + ddlYear.SelectedValue + " And Revisi = " + lbRevisi.Text
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Year IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Year") = ddlYear.SelectedValue
                Row(I)("Revisi") = lbRevisi.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Year, Revisi, CapexCategory, Department, Price, Qty01, Qty02, Qty03, Qty04, " + _
                                         "Qty05, Qty06, Qty07, Qty08, Qty09, Qty10, Qty11, Qty12, " + _
                                         "Total01, Total02, Total03, Total04, Total05, Total06, Total07, Total08, Total09, Total10, " + _
                                         "Total11, Total12, Qty, Total " + _
                                         "FROM GLCapexDt WHERE Year = " & ViewState("Year") & " AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLCapexDt")

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
            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr) = False Then
            '        Exit Sub
            '    End If
            'Next
            SaveAll()
            If lbStatus.Text.Length > 0 Then Exit Sub
            MovePanel(pnlInput, PnlHd)

            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = ddlYear.SelectedValue
            ddlField.SelectedValue = "Year"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True

            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ViewState("Year") = Now.Year.ToString
            ViewState("Month") = Now.Month.ToString
            ViewState("Revisi") = "0"
            newTrans()
            btnHome.Visible = False
            btnGetData.Visible = True
            ddlYear.Focus()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            ' Cleardt2()
            pnlDt.Visible = True
            BindDataDt("1", "0")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            BindToDropList(ddlYear, Now.Year.ToString)
            lbRevisi.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCategoryCode.Text = ""
            tbCategoryName.Text = ""
            ddlDepartment.SelectedIndex = 0
            tbPrice.Text = "0"
            tb01.Text = "0"
            tb02.Text = "0"
            tb03.Text = "0"
            tb04.Text = "0"
            tb05.Text = "0"
            tb06.Text = "0"
            tb07.Text = "0"
            tb08.Text = "0"
            tb09.Text = "0"
            tb10.Text = "0"
            tb11.Text = "0"
            tb12.Text = "0"
            tbTotal01.Text = "0"
            tbTotal02.Text = "0"
            tbTotal03.Text = "0"
            tbTotal04.Text = "0"
            tbTotal05.Text = "0"
            tbTotal06.Text = "0"
            tbTotal07.Text = "0"
            tbTotal08.Text = "0"
            tbTotal09.Text = "0"
            tbTotal10.Text = "0"
            tbTotal11.Text = "0"
            tbTotal12.Text = "0"
            tbQty.Text = "0"
            tbTotal.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    'Private Sub getDayOfMonth()
    '    Dim hari, i As Integer
    '    Try
    '        hari = Date.DaysInMonth(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue))
    '        ddlDay.Items.Clear()
    '        For i = 1 To hari
    '            ddlDay.Items.Add(i.ToString)
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("get Day Of Month" + ex.ToString)
    '    End Try
    'End Sub

    Function CekHd() As Boolean
        Try
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then

                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("Factory").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Factory Must Have Value")
                '    Return False
                'End If
                'If Dr("ProductMotif").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Motif Must Have Value")
                '    Return False
                'End If
                'If Dr("ProductSize").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Size Must Have Value")
                '    Return False
                'End If
                'If Dr("ProductMerk").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Merk Must Have Value")
                '    Return False
                'End If
                'If CFloat(Dr("Qty")) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    Return False
                'End If
            Else
                If tbCategoryCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Capex Category Must Have Value")
                    tbCategoryCode.Focus()
                    Return False
                End If
                If ddlDepartment.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Department Must Have Value")
                    ddlDepartment.Focus()
                    Return False
                End If
                If CFloat(tbPrice.Text) <= 0 Then
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

    'Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
    '    Try
    '        If Not (Dr Is Nothing) Then

    '            'If Dr.RowState = DataRowState.Deleted Then
    '            '    Return True
    '            'End If
    '            'If Dr("Factory").ToString.Trim = "" Then
    '            '    lbStatus.Text = MessageDlg("Factory Must Have Value")
    '            '    Return False
    '            'End If                
    '        Else
    '            'If CFloat(tbQty.Text) <= 0 Then
    '            '    lbStatus.Text = MessageDlg("Qty Must Have Value")
    '            '    tbQty.Focus()
    '            '    Return False
    '            'End If
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Throw New Exception("Cek Dt Error : " + ex.ToString)
    '    End Try
    'End Function

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr) = False Then
            '        Exit Sub
            '    End If
            'Next
            SaveAll()
            newTrans()
            ViewState("StateHd") = "Insert"
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
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
                    pnlDt.Visible = True
                    ViewState("Year") = GVR.Cells(2).Text
                    ViewState("Revisi") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Year"), ViewState("Revisi"))
                    BindDataDt(ViewState("Year"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnGetData.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("Year") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(4).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Year"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("Year"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If Not GVR.Cells(3).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must be Posted Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_GLcapexCreateRevisi " + GVR.Cells(2).Text + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = GVR.Cells(2).Text
                    ddlField.SelectedValue = "Year"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_GLFormCapex " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(GVR.Cells(4).Text)
                    Session("ReportFile") = Server.MapPath("~\Rpt\FormTrCapex.frx")
                    AttachScript("openprintdlg();", Page, Me.GetType)
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            If CheckCurrentYear() = False And ViewState("StateHd") = "Insert" Then
                lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " exist")
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbCategoryCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnAddDt2ke1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddDt2Ke1.Click, btnAddDt2Ke2.Click
    '    Try
    '        'Cleardt2()
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        ViewState("StateDt2") = "Insert"
    '        'MovePanel(pnlDt2, pnlEditDt2)
    '        EnableHd(False)
    '        StatusButtonSave(False)
    '        'ddlMonth.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub

    'Private Sub selectDt2()
    '    Try
    '        Dim drow As DataRow()
    '        If ViewState("Dt2") Is Nothing Then
    '            'BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
    '        Else
    '            drow = ViewState("Dt2").Select("ProductClass=" + QuotedStr(lbProductClassDt2.Text))
    '            If drow.Length > 0 Then
    '                BindGridDt(drow.CopyToDataTable, GridDt2)
    '                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '            Else
    '                Dim DtTemp As DataTable
    '                DtTemp = ViewState("Dt2").Clone
    '                DtTemp.Rows.Add(DtTemp.NewRow())
    '                GridDt2.DataSource = DtTemp
    '                GridDt2.DataBind()
    '                GridDt2.Columns(0).Visible = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception("select dt2 Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If

            If e.CommandName = "ChangeMotif" Then
                'Dim GVR As GridViewRow
                'Dim lbClass, lbSize, lbPrice As Label
                'GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                'lbClass = GVR.FindControl("lbProductClass")
                'lbSize = GVR.FindControl("lbProductSize")
                'lbPrice = GVR.FindControl("lbProductPrice")
                'FillTextBoxDt(lbClass.Text, lbSize.Text, lbPrice.Text)
                'MovePanel(pnlDt, pnlEditDt)
                'EnableHd(False)
                'ViewState("StateDt") = "ChangeMotif"
                'ViewState("CurrProductClass") = lbClass.Text
                'ViewState("CurrProductSize") = lbSize.Text
                'ViewState("CurrProductPrice") = lbPrice.Text
                'ddlClass.Focus()
                'StatusButtonSave(False)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
    '    'Dim ds As DataSet
    '    'Dim i As Integer
    '    Try
    '        If e.CommandName = "Insert" Then
    '            btnAddDt_Click(Nothing, Nothing)
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim lbCapexCateogry, lbDepartment As Label
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            lbCapexCateogry = GVR.FindControl("lbCategory")
            lbDepartment = GVR.FindControl("lbDepartment")
            dr = ViewState("Dt").Select("CapexCategory = " + QuotedStr(lbCapexCateogry.Text) + " ANd Department = " + QuotedStr(lbDepartment.Text))

            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 1)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lbCapexCategory, lbDepartment As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lbCapexCategory = GVR.FindControl("lbCategory")
            lbDepartment = GVR.FindControl("lbDepartment")
            FillTextBoxDt(lbCapexCategory.Text, lbDepartment.Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("CapexCategory") = lbCapexCategory.Text
            ViewState("Department") = lbDepartment.Text
            tbRemark.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Taon As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Year = " + Taon + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            'newTrans()
            ddlYear.SelectedValue = Taon
            lbRevisi.Text = Revisi
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal CapexCategory As String, ByVal Department As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("CapexCategory = " + QuotedStr(CapexCategory) + " AND Department = " + QuotedStr(Department))
            If Dr.Length > 0 Then
                BindToText(tbCategoryCode, Dr(0)("CapexCategory").ToString)
                BindToText(tbCategoryName, Dr(0)("CapexCategoryName").ToString)
                BindToDropList(ddlDepartment, Dr(0)("Department").ToString)
                BindToText(tbPrice, Dr(0)("Price").ToString)
                BindToText(tb01, Dr(0)("Qty01").ToString)
                BindToText(tb02, Dr(0)("Qty02").ToString)
                BindToText(tb03, Dr(0)("Qty03").ToString)
                BindToText(tb04, Dr(0)("Qty04").ToString)
                BindToText(tb05, Dr(0)("Qty05").ToString)
                BindToText(tb06, Dr(0)("Qty06").ToString)
                BindToText(tb07, Dr(0)("Qty07").ToString)
                BindToText(tb08, Dr(0)("Qty08").ToString)
                BindToText(tb09, Dr(0)("Qty09").ToString)
                BindToText(tb10, Dr(0)("Qty10").ToString)
                BindToText(tb11, Dr(0)("Qty11").ToString)
                BindToText(tb12, Dr(0)("Qty12").ToString)
                BindToText(tbTotal01, Dr(0)("Total01").ToString)
                BindToText(tbTotal02, Dr(0)("Total02").ToString)
                BindToText(tbTotal03, Dr(0)("Total03").ToString)
                BindToText(tbTotal04, Dr(0)("Total04").ToString)
                BindToText(tbTotal05, Dr(0)("Total05").ToString)
                BindToText(tbTotal06, Dr(0)("Total06").ToString)
                BindToText(tbTotal07, Dr(0)("Total07").ToString)
                BindToText(tbTotal08, Dr(0)("Total08").ToString)
                BindToText(tbTotal09, Dr(0)("Total09").ToString)
                BindToText(tbTotal10, Dr(0)("Total10").ToString)
                BindToText(tbTotal11, Dr(0)("Total11").ToString)
                BindToText(tbTotal12, Dr(0)("Total12").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbTotal, Dr(0)("Total").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnProductClass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnProductClass.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "select ProductClass, ProductSize, ProductPrice from VMsProductClass"
    '        ResultField = "ProductClass, ProductSize, ProductPrice"
    '        ViewState("Sender") = "btnProductClass"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Factory Click Error : " + ex.ToString
    '    End Try
    'End Sub



    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim DR, CurDr As DataRow
        Dim ds As DataSet
        Dim dt As DataTable
        Try
            If GetCountRecord(ViewState("Dt")) > 0 Then
                lbStatus.Text = MessageDlg("Data not empty")
                Exit Sub
            End If
            'If CheckCurrentYear() = False Then
            '    lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " exist")
            '    Exit Sub
            'End If
            ds = SQLExecuteQuery("EXEC S_GLCapexGetLastYearData " + ddlYear.SelectedValue, ViewState("DBConnection"))

            dt = ds.Tables(0)

            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data for last year")
                Exit Sub
            End If

            For Each CurDr In dt.Rows
                DR = ViewState("Dt").NewRow
                DR("CapexCategory") = CurDr("CapexCategoryCode")
                DR("CapexCategoryName") = CurDr("CapexCategoryName")
                DR("Department") = CurDr("DepartmentCode")
                DR("Price") = CurDr("Price")
                DR("Qty01") = CurDr("Qty01")
                DR("Qty02") = CurDr("Qty02")
                DR("Qty03") = CurDr("Qty03")
                DR("Qty04") = CurDr("Qty04")
                DR("Qty05") = CurDr("Qty05")
                DR("Qty06") = CurDr("Qty06")
                DR("Qty07") = CurDr("Qty07")
                DR("Qty08") = CurDr("Qty08")
                DR("Qty09") = CurDr("Qty09")
                DR("Qty10") = CurDr("Qty10")
                DR("Qty11") = CurDr("Qty11")
                DR("Qty12") = CurDr("Qty12")
                DR("Total01") = CurDr("Total01")
                DR("Total02") = CurDr("Total02")
                DR("Total03") = CurDr("Total03")
                DR("Total04") = CurDr("Total04")
                DR("Total05") = CurDr("Total05")
                DR("Total06") = CurDr("Total06")
                DR("Total07") = CurDr("Total07")
                DR("Total08") = CurDr("Total08")
                DR("Total09") = CurDr("Total09")
                DR("Total10") = CurDr("Total10")
                DR("Total11") = CurDr("Total11")
                DR("Total12") = CurDr("Total12")
                DR("Qty") = CurDr("Qty")
                DR("Total") = CurDr("Total")
                ViewState("Dt").Rows.Add(DR)
            Next

            dt = ds.Tables(0)
            If dt.Rows.Count = 0 Then
                Exit Sub
            End If

            'For Each CurDr In dt.Rows
            '    DR = ViewState("Dt2").NewRow
            '    DR("ProductClass") = CurDr("ProductClass")
            '    DR("Month") = CurDr("Month")
            '    DR("Days") = CurDr("Days")
            '    DR("Qty") = CurDr("Qty")
            '    ViewState("Dt2").Rows.Add(DR)
            'Next

            BindGridDtExtended()

        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindGridDtExtended()
        Try
            BindGridDt(ViewState("Dt"), GridDt)
            'If GetCountRecord(ViewState("Dt")) > 0 Then
            '    GridDt.Columns(1).Visible = True
            'Else
            '    GridDt.Columns(1).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("BindDtWithoutLoadFromDb Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
    '    Try
    '        ' getDayOfMonth()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl Month selected Index Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBackDt2.Click
    '    Try
    '        ' pnlDt2.Visible = False
    '        pnlDt.Visible = True
    '        If ViewState("StateHd") <> "View" Then
    '            BindGridDtExtended()
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btnBackDt2 Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnBackDuplicate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBackDuplicate.Click
    '    Try
    '        pnlDt.Visible = True
    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn back Duplicate Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function CheckCurrentYear() As Boolean
        Dim result As String
        Try
            result = SQLExecuteScalar("SELECT Count([Year]) FROM GLCapexHd WHERE [Year] = " + ddlYear.SelectedValue + " AND Revisi = " + lbRevisi.Text, ViewState("DBConnection"))
            'lbStatus.Text = "A" + result
            If result <> "0" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Cek Current Year Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub tbCategoryCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCategoryCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT CapexCategoryCode, capexCategoryName FROM MsCapexCategory WHERE CapexCategoryCode = " + QuotedStr(tbCategoryCode.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCategoryCode.Text = Dr("CapexCategoryCode")
                tbCategoryName.Text = Dr("CapexCategoryName")
            Else
                tbCategoryCode.Text = ""
                tbCategoryName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbCategoryCode_TextChanged change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCategory.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT CapexCategoryCode, CapexCategoryName FROM MsCapexCategory "
            ResultField = "CapexCategoryCode, CapexCategoryName"
            ViewState("Sender") = "btnCategory"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCategory_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPrice.TextChanged
        Try
            tbTotal01.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb01.Text), ViewState("DigitCurr"))
            tbTotal02.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb02.Text), ViewState("DigitCurr"))
            tbTotal03.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb03.Text), ViewState("DigitCurr"))
            tbTotal04.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb04.Text), ViewState("DigitCurr"))
            tbTotal05.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb05.Text), ViewState("DigitCurr"))
            tbTotal06.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb06.Text), ViewState("DigitCurr"))
            tbTotal07.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb07.Text), ViewState("DigitCurr"))
            tbTotal08.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb08.Text), ViewState("DigitCurr"))
            tbTotal09.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb09.Text), ViewState("DigitCurr"))
            tbTotal10.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb10.Text), ViewState("DigitCurr"))
            tbTotal11.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb11.Text), ViewState("DigitCurr"))
            tbTotal12.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb12.Text), ViewState("DigitCurr"))
            Jumlah()
        Catch ex As Exception
            lbStatus.Text = "tbPrice_TextChanged Error : " + ex.ToString
        End Try        
    End Sub

    Protected Sub tb01_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb01.TextChanged
        tbTotal01.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb01.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal01_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal01.TextChanged
        tb01.Text = FormatNumber(CFloat(tbTotal01.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb02_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb02.TextChanged
        tbTotal02.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb02.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal02_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal02.TextChanged
        tb02.Text = FormatNumber(CFloat(tbTotal02.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb03_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb03.TextChanged
        tbTotal03.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb03.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal03_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal03.TextChanged
        tb03.Text = FormatNumber(CFloat(tbTotal03.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb04_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb04.TextChanged
        tbTotal04.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb04.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal04_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal04.TextChanged
        tb04.Text = FormatNumber(CFloat(tbTotal04.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb05_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb05.TextChanged
        tbTotal05.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb05.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal05_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal05.TextChanged
        tb05.Text = FormatNumber(CFloat(tbTotal05.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb06_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb06.TextChanged
        tbTotal06.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb06.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal06_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal06.TextChanged
        tb06.Text = FormatNumber(CFloat(tbTotal06.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb07_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb07.TextChanged
        tbTotal07.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb07.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal07_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal07.TextChanged
        tb07.Text = FormatNumber(CFloat(tbTotal07.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb08_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb08.TextChanged
        tbTotal08.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb08.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal08_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal08.TextChanged
        tb08.Text = FormatNumber(CFloat(tbTotal08.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb09_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb09.TextChanged
        tbTotal09.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb09.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal09_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal09.TextChanged
        tb09.Text = FormatNumber(CFloat(tbTotal09.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb10_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb10.TextChanged
        tbTotal10.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb10.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal10_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal10.TextChanged
        tb10.Text = FormatNumber(CFloat(tbTotal10.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb11_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb11.TextChanged
        tbTotal11.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb11.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal11_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal11.TextChanged
        tb11.Text = FormatNumber(CFloat(tbTotal11.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tb12_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb12.TextChanged
        tbTotal12.Text = FormatNumber(CFloat(tbPrice.Text) * CFloat(tb12.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Protected Sub tbTotal12_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTotal12.TextChanged
        tb12.Text = FormatNumber(CFloat(tbTotal12.Text) / CFloat(tbPrice.Text), ViewState("DigitCurr"))
        Jumlah()
    End Sub

    Private Sub Jumlah()
        Try
            tbQty.Text = FormatNumber(CFloat(tb01.Text) + CFloat(tb02.Text) + CFloat(tb03.Text) + CFloat(tb04.Text) + CFloat(tb05.Text) + CFloat(tb06.Text) + CFloat(tb07.Text) + CFloat(tb08.Text) + CFloat(tb09.Text) + CFloat(tb10.Text) + CFloat(tb11.Text) + CFloat(tb12.Text), ViewState("DigitCurr"))
            tbTotal.Text = FormatNumber(CFloat(tbTotal01.Text) + CFloat(tbTotal02.Text) + CFloat(tbTotal03.Text) + CFloat(tbTotal04.Text) + CFloat(tbTotal05.Text) + CFloat(tbTotal06.Text) + CFloat(tbTotal07.Text) + CFloat(tbTotal08.Text) + CFloat(tbTotal09.Text) + CFloat(tbTotal10.Text) + CFloat(tbTotal11.Text) + CFloat(tbTotal12.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "Jumlah Error : " + ex.ToString
        End Try
    End Sub
End Class
