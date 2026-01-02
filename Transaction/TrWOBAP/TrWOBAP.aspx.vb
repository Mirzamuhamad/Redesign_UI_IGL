Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbcf
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrWOBAP
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLWOBAPHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlDivision, "Select Division_Code, Division_Name from V_MsDivision ", True, "Division_Code", "Division_Name", ViewState("DBConnection"))
                'FillCombo(ddlDivision, "Select DivisiBlokCode, DivisiBlokName from V_MsDivisiBlock ", True, "DivisiBlokCode", "DivisiBlokName", ViewState("DBConnection"))
                'FillCombo(ddlJob, "Select Job_Code, Job_Name from V_MsJobPlant ", False, "Job_Code", "Job_Name", ViewState("DBConnection"))
                'lbCount.Text = SQLExecuteScalar("EXEC S_PLWOBAPReffCount '','' ", ViewState("DBConnection").ToString)
                lbCount.Text = SQLExecuteScalar("SELECT DISTINCT COUNT(A.Result_No) FROM V_PLWOBAPGetReff A Left OUTER JOIN V_PLWOBAPHDDt B ON A.Result_No = B.WOResult  WHERE coalesce(B.Status,'')=''   OR B.Status = 'D' and Result_No IS NOT NULL ", ViewState("DBConnection").ToString)
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnWorkBy" Then
                    tbWorkbyCode.Text = Session("Result")(0).ToString
                    tbWorkByName.Text = Session("Result")(1).ToString

                    If Session("Result")(2).ToString = "Borongan" Then
                        ddlFgBorongan.SelectedValue = "Y"
                    Else
                        ddlFgBorongan.SelectedValue = "N"
                    End If
                End If

                If ViewState("Sender") = "btnKontraktor" Then
                    tbKontraktorCode.Text = Session("Result")(0).ToString
                    tbKontraktorName.Text = Session("Result")(1).ToString

                End If

                If ViewState("Sender") = "btnDivisiBlok" Then
                    ddlDivisiBlok.Text = Session("Result")(0).ToString
                    tbDivisiBlokName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnJob" Then
                    ddlJob.Text = Session("Result")(0).ToString
                    tbJobName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnWOResult" Then
                    tbWOResult.Text = Session("Result")(0).ToString
                    tbWOResultDate.SelectedDate = Session("Result")(1).ToString
                    tbWONo.Text = Session("Result")(2).ToString
                    tbType.Text = Session("Result")(3).ToString
                    ddlDivisiBlok.Text = Session("Result")(4).ToString
                    tbDivisiBlokName.Text = Session("Result")(5).ToString
                    ddlJob.Text = Session("Result")(6).ToString
                    tbJobName.Text = Session("Result")(7).ToString
                    tbQtyResult.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitQty"))
                    tbQtyDone.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitQty"))
                    tbQty.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitQty"))
                    tbUnit.Text = Session("Result")(9).ToString
                    tbPriceForex.Text = FormatFloat(Session("Result")(10).ToString, ViewState("DigitQty"))
                    tbAmountForex.Text = FormatFloat(Session("Result")(11).ToString, ViewState("DigitQty"))
                    tbDisc.Text = Session("Result")(12).ToString
                    tbDiscForex.Text = FormatFloat(Session("Result")(13).ToString, ViewState("DigitQty"))
                    tbNettoForex.Text = FormatFloat(Session("Result")(14).ToString, ViewState("DigitQty"))
                    tbRemarkDt.Text = Session("Result")(15).ToString
                    '    
                End If
                'End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim FirstTime As Boolean = True
                    Dim Supplier, WorkBy As String
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            BindToDropList(ddlDivision, drResult("Division").ToString)
                            BindToDropList(ddlFgBorongan, drResult("FgBorongan").ToString)
                            ddlFgBorongan.Enabled = False
                            BindToText(tbWorkbyCode, drResult("WorkBy").ToString)
                            BindToText(tbWorkByName, drResult("TeamName").ToString)
                            BindToText(tbKontraktorCode, drResult("Supplier").ToString)
                            BindToText(tbKontraktorName, drResult("Supplier_Name").ToString)
                            BindToText(tbTotalForex, FormatNumber(drResult("AmountForex").ToString, 2))
                        End If

                        Supplier = drResult("Supplier").ToString.Trim
                        WorkBy = drResult("WorkBy").ToString
                        ExistRow = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Supplier) + " AND Type = " + QuotedStr(WorkBy))
                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("WOResult") = drResult("Result_No").ToString
                            dr("WOResultDate") = drResult("Result_Date").ToString
                            dr("WONo") = drResult("WO_No").ToString
                            dr("Type") = drResult("Type").ToString
                            dr("DivisiBlok") = drResult("DivisiBlok").ToString
                            dr("DivisiBlokName") = drResult("DivisiBlokName").ToString
                            dr("Job") = drResult("Job").ToString
                            dr("JobName") = drResult("Job_Name").ToString
                            dr("QtyResult") = FormatNumber(drResult("Qty").ToString, 2)
                            dr("QtyDone") = FormatNumber(drResult("Qty").ToString, 2)
                            dr("Qty") = FormatNumber(drResult("Qty").ToString, 2)
                            dr("Unit") = drResult("Unit").ToString
                            dr("PriceForex") = FormatNumber(drResult("Price").ToString, 2)
                            dr("AmountForex") = FormatNumber(drResult("AmountForex").ToString, 2)
                            dr("Disc") = FormatNumber(drResult("Disc").ToString, 2)
                            dr("DiscForex") = FormatNumber(drResult("DiscForex").ToString, 2)
                            dr("NettoForex") = FormatNumber(drResult("NettoForex").ToString, 2)
                            dr("Remark") = drResult("Remark").ToString
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
                    '    'Session("ResultSame") = Nothing
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
        'ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitQty") = 2
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
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Me.tbQtyWrhs.Attributes.Add("ReadOnly", "True")

        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtyDone.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtyResult.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbAmountForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbNettoForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "QtyxPrice(" + Me.tbQty.ClientID + "); setformatdt();")

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
        Return "SELECT * From V_PLWOBAPDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PRFormReqRetur " + Result
                Session("ReportFile") = ".../../../Rpt/FormPRRetur.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLWOBAP", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ''btnGetDt.Visible = State            
            'ddlDivision.Enabled = State
            'tbWorkbyCode.Enabled = State
            'btnReffNo.Visible = State
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            'tbKontraktorCode.Enabled = State
            'tbRemark.Enabled = State
            'ddlFgBorongan.Enabled = State
            'btnSupp.Visible = State
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
            pnlEditDt.Visible = "False"
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
    '    Dim Row As DataRow
    '    Try
    '        If ViewState("StateDt") = "Edit" Then
    '            If ViewState("DtValue") <> tbWOResult.Text Then
    '                If CekExistData(ViewState("Dt"), "WOResult", tbWOResult.Text) Then
    '                    lbStatus.Text = "WOResult " + tbWOResult.Text + " has already been exist"
    '                    Exit Sub
    '                End If
    '            End If
    '            Row = ViewState("Dt").Select("WOResult = " + QuotedStr(ViewState("DtValue")))(0)
    '            If CekDt() = False Then
    '                Exit Sub
    '            End If

    '            Row.BeginEdit()
    '            Row("WOResult") = tbWOResult.Text
    '            Row("WOResultDate") = tbWOResultDate.Text
    '            Row("WONo") = tbWONo.Text
    '            Row("Qty") = tbQty.Text
    '            Row("Type") = tbType.Text
    '            Row("DivisiBlok") = ddlDivisiBlok.Text
    '            Row("DivisiBlokName") = tbDivisiBlokName.Text
    '            Row("Job") = ddlJob.Text
    '            Row("JobName") = tbJobName.Text
    '            Row("QtyResult") = tbQtyResult.Text
    '            Row("QtyDone") = tbQtyDone.Text
    '            Row("Unit") = tbUnit.Text
    '            Row("PriceForex") = tbPriceForex.Text
    '            Row("AmountForex") = tbAmountForex.Text
    '            Row("Disc") = tbDisc.Text
    '            Row("DiscForex") = tbDiscForex.Text
    '            Row("NettoForex") = tbNettoForex.Text
    '            Row("Remark") = tbRemarkDt.Text
    '            Row.EndEdit()

    '            'End If
    '            'Row("Qty") = tbQtyWrhs.Text
    '            'Row("Unit") = tbUnitWrhs.Text
    '            'Row("Remark") = tbRemarkDt.Text
    '            'Row.EndEdit()
    '            'ViewState("Dt").AcceptChanges()
    '        Else
    '            'Insert
    '            If CekDt() = False Then
    '                Exit Sub
    '            End If
    '            If CekExistData(ViewState("Dt"), "WOResult", tbWOResult.Text) Then
    '                lbStatus.Text = "WOResult " + tbWOResult.Text + " has already been exist"
    '                Exit Sub
    '            End If
    '            Dim dr As DataRow
    '            dr = ViewState("Dt").NewRow
    '            dr("WOResult") = tbWOResult.Text
    '            dr("WOResultDate") = tbWOResultDate.Text
    '            dr("WONo") = tbWONo.Text
    '            dr("Qty") = tbQty.Text
    '            dr("Type") = tbType.Text
    '            dr("DivisiBlok") = ddlDivisiBlok.Text
    '            dr("DivisiBlokName") = tbDivisiBlokName.Text
    '            dr("Job") = ddlJob.Text
    '            dr("JobName") = tbJobName.Text
    '            dr("QtyResult") = tbQtyResult.Text
    '            dr("QtyDone") = tbQtyDone.Text
    '            dr("Unit") = tbUnit.Text
    '            dr("PriceForex") = tbPriceForex.Text
    '            dr("AmountForex") = tbAmountForex.Text
    '            dr("Disc") = tbDisc.Text
    '            dr("DiscForex") = tbDiscForex.Text
    '            dr("NettoForex") = tbNettoForex.Text
    '            dr("Remark") = tbRemarkDt.Text
    '            ViewState("Dt").Rows.Add(dr)

    '        End If
    '        MovePanel(pnlEditDt, pnlDt)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '        BindGridDt(ViewState("Dt"), GridDt)
    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn save Dt Error : " + ex.ToString
    '    Finally
    '        If Not con Is Nothing Then con.Dispose()
    '        If Not da Is Nothing Then da.Dispose()
    '    End Try
    'End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("WOResult = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("WOResult") = tbWOResult.Text
                Row("WOResultDate") = tbWOResultDate.Text
                Row("WONo") = tbWONo.Text
                Row("Qty") = tbQty.Text
                Row("Type") = tbType.Text
                Row("DivisiBlok") = ddlDivisiBlok.Text
                Row("DivisiBlokName") = tbDivisiBlokName.Text
                Row("Job") = ddlJob.Text
                Row("JobName") = tbJobName.Text
                Row("QtyResult") = tbQtyResult.Text
                Row("QtyDone") = tbQtyDone.Text
                Row("Unit") = tbUnit.Text
                Row("PriceForex") = tbPriceForex.Text
                Row("AmountForex") = tbAmountForex.Text
                Row("Disc") = tbDisc.Text
                Row("DiscForex") = tbDiscForex.Text
                Row("NettoForex") = tbNettoForex.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "WOResult", tbWOResult.Text) Then
                    lbStatus.Text = "WOResult " + tbWOResult.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WOResult") = tbWOResult.Text
                dr("WOResultDate") = tbWOResultDate.Text
                dr("WONo") = tbWONo.Text
                dr("Qty") = tbQty.Text
                dr("Type") = tbType.Text
                dr("DivisiBlok") = ddlDivisiBlok.Text
                dr("DivisiBlokName") = tbDivisiBlokName.Text
                dr("Job") = ddlJob.Text
                dr("JobName") = tbJobName.Text
                dr("QtyResult") = tbQtyResult.Text
                dr("QtyDone") = tbQtyDone.Text
                dr("Unit") = tbUnit.Text
                dr("PriceForex") = tbPriceForex.Text
                dr("AmountForex") = tbAmountForex.Text
                dr("Disc") = tbDisc.Text
                dr("DiscForex") = tbDiscForex.Text
                dr("NettoForex") = tbNettoForex.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)

                End If
                MovePanel(pnlEditDt, pnlDt)
                'GridDt.Columns(1).Visible = True

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
                tbRef.Text = GetAutoNmbr("WOB", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PLWOBAPHd (TransNmbr, TransDate,Status, Type," + _
                "Divisi, Team, FgBorongan, Supplier, Currency, TotalForex, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ",'" + Format(tbTransDate.SelectedDate, "yyyy-MM-dd") + "','H'," + QuotedStr(tbTypehd.Text) + "," + _
                QuotedStr(ddlDivision.SelectedValue) + ", " + QuotedStr(tbWorkbyCode.Text) + "," + _
                QuotedStr(ddlFgBorongan.SelectedValue) + "," + QuotedStr(tbKontraktorCode.Text) + ", " + QuotedStr(tbCurrency.Text) + ", " + QuotedStr(tbTotalForex.Text).Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLWOBAPHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLWOBAPHd SET Supplier = " + QuotedStr(tbKontraktorCode.Text) + ", Type = " + QuotedStr(tbTypehd.Text) + ", Divisi = " + QuotedStr(ddlDivision.SelectedValue) + ", Currency = " + QuotedStr(tbCurrency.Text) + ", TotalForex = " + tbTotalForex.Text + _
                ", FgBorongan = " + QuotedStr(ddlFgBorongan.SelectedValue) + ", Team = " + QuotedStr(tbWorkbyCode.Text) + ",  Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbTransDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + ""
            End If
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
            Dim cmdSql As New SqlCommand("SELECT *  FROM PLWOBAPDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PLWOBAPDt")

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least  1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(False)
            btnHome.Visible = False
            tbRef.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
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
            ' tbTypehd.Text = ""
            ddlDivision.Text = ""
            tbWorkbyCode.Text = ""
            tbWorkByName.Text = ""

            tbKontraktorCode.Text = ""
            tbKontraktorName.Text = ""
            tbTotalForex.Text = 0
            tbRemark.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbWOResult.Text = ""
            tbWONo.Text = ""
            tbType.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            ddlDivisiBlok.Text = ""
            tbDivisiBlokName.Text = ""
            tbJobName.Text = ""
            tbQtyResult.Text = "0"
            tbUnit.Text = ""
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
            tbDisc.Text = ""
            tbDiscForex.Text = ""
            tbNettoForex.Text = ""

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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbWorkbyCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWorkbyCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbWorkbyCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbWorkbyCode.Text = Dr("Team")
                tbWorkByName.Text = Dr("Team_Name")
                'BindToText(tbAttn, Dr("Contact_Person"))
            Else
                tbWorkbyCode.Text = ""
                tbWorkByName.Text = ""
                'tbAttn.Text = ""
            End If
            tbWorkbyCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb WorkbyCode Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Try
    '        If ddlReffType.SelectedValue = "Others" Then
    '            Dt = SQLExecuteQuery("SELECT * FROM VMsProductOH WHERE Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
    '        Else
    '            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, QtyOrder, UnitOrder, QtyWrhs, UnitWrhs FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' AND Reference = '" + tbReffNo.Text + "' AND Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
    '        End If

    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbProductCode.Text = Dr("Product_Code")
    '            tbProductName.Text = Dr("Product_Name")
    '            If ddlReffType.SelectedValue = "Others" Then
    '                tbQty.Text = Dr("On_Hand")
    '                BindToDropList(ddlUnit, Dr("Unit"))
    '                tbQtyWrhs.Text = Dr("On_Hand")
    '                tbUnitWrhs.Text = Dr("Unit")
    '            Else
    '                tbQty.Text = Dr("QtyOrder")
    '                BindToDropList(ddlUnit, Dr("UnitOrder"))
    '                tbQtyWrhs.Text = Dr("QtyWrhs")
    '                tbUnitWrhs.Text = Dr("UnitWrhs")
    '            End If
    '        Else
    '            tbProductCode.Text = ""
    '            tbProductName.Text = ""
    '            tbQty.Text = "0"
    '            ddlUnit.SelectedIndex = 0
    '            tbQtyWrhs.Text = "0"
    '            tbUnitWrhs.Text = ""
    '        End If
    '        tbQtyWrhs.Text = FindConvertUnit(tbProductCode.Text, ddlUnit.SelectedValue, tbQty.Text, ViewState("DBConnection").ToString).ToString
    '        AttachScript("setformatdt();", Page, Me.GetType())
    '        tbProductCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb ProductCode Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbWOResult.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = "RR No must have value"
            '    tbRef.Focus()
            '    Return False
            'End If
            If tbTransDate.IsNull Then
                lbStatus.Text = MessageDlg("RR Date must have value")
                tbTransDate.Focus()
                Return False
            End If
            'If CInt(ViewState("GLYear")) <> Year(tbTransDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbTransDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbWorkbyCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Workby must have value")
                btnWorkBy.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("WOResult").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If

                If Dr("QtyResult").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("QtyResult Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyDone").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Done Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Qty Must Have Value")
                    Return False
                End If
            Else
                If tbWOResult.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("QtyResult Must Have Value")
                    tbWOResult.Focus()
                    Return False
                End If
                If tbQtyResult.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Result Must Have Value")
                    tbQtyResult.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty  Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtyDone.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Done Must Have Value")
                    tbQtyDone.Focus()
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
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridView1.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Status, Supplier, Reff No, PO No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Supplier, ReffNo, PONo, Remark"
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
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        ViewState("DigitCurr") = GetCurrDigit(ViewState("Currency"), ViewState("DBConnection").ToString)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        If LCase(ViewState("UserId")) <> "admin" Then
                            CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                            If CekMenu <> "" Then
                                lbStatus.Text = CekMenu
                                Exit Sub
                            End If
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_PRFormReqRetur ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormPRRetur.frx"
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

    Protected Sub GridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridDt.RowCancelingEdit

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

    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound

    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("WOResult = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            ViewState("DtValue") = GVR.Cells(1).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbTypehd, Dt.Rows(0)("Type").ToString)
            BindToDropList(ddlDivision, Dt.Rows(0)("Divisi").ToString)
            BindToText(tbWorkbyCode, Dt.Rows(0)("Team").ToString)
            BindToText(tbWorkByName, Dt.Rows(0)("Team_Name").ToString)
            BindToDropList(ddlFgBorongan, Dt.Rows(0)("FgBorongan").ToString)
            BindToText(tbKontraktorCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbKontraktorName, Dt.Rows(0)("Supplier_Name").ToString)
            ViewState("DigitCurr") = 0
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal WOResult As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WOResult = " + QuotedStr(WOResult))
            If Dr.Length > 0 Then
                BindToText(tbWOResult, Dr(0)("WOResult").ToString)
                BindToDate(tbWOResultDate, Dr(0)("WOResultDate").ToString)
                BindToText(tbWONo, Dr(0)("WONo").ToString)
                BindToText(tbType, Dr(0)("Type").ToString)
                BindToText(ddlDivisiBlok, Dr(0)("DivisiBlok").ToString)
                BindToText(tbDivisiBlokName, Dr(0)("DivisiBlokName").ToString)
                BindToText(ddlJob, Dr(0)("Job").ToString)
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbQtyResult, Dr(0)("QtyResult").ToString)
                BindToText(tbQtyDone, Dr(0)("QtyDone").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToText(tbDisc, Dr(0)("Disc").ToString)
                BindToText(tbDiscForex, Dr(0)("DiscForex").ToString)
                BindToText(tbNettoForex, Dr(0)("NettoForex").ToString)
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

    'Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged, ddlUnit.TextChanged
    '    Try
    '        tbQtyWrhs.Text = FindConvertUnit(tbProductCode.Text, ddlUnit.SelectedValue, tbQty.Text, ViewState("DBConnection").ToString).ToString
    '        tbQtyWrhs.Text = FormatFloat(tbQtyWrhs.Text, ViewState("DigitQty"))
    '        tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
    '        tbQty.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
    '    End Try
    'End Sub


    'Protected Sub ddlReffType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReffType.SelectedIndexChanged
    '    tbReffNo.Text = ""
    '    tbPONo.Text = ""
    '    btnReffNo.Visible = Not ddlReffType.SelectedValue = "Others"
    '    tbReffNo.Enabled = Not btnReffNo.Visible
    'End Sub

    'Protected Sub btnReffNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReffNo.Click
    '    Dim ResultField As String 'ResultSame 
    '    Try
    '        If tbSuppCode.Text = "" Then
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' "
    '            Else
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' and Report = " + QuotedStr(ddlReport.SelectedValue)
    '            End If
    '        Else
    '            If ViewState("StateHd") = "Insert" Then
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' AND Supplier = '" + tbSuppCode.Text + "' "
    '            Else
    '                Session("filter") = "SELECT distinct Reference, [Date], Report, PONo, Supplier, Supplier_Name, Attn FROM V_PRReqReturReff WHERE Type = '" + ddlReffType.SelectedValue + "' AND Supplier = '" + tbSuppCode.Text + "' and Report = " + QuotedStr(ddlReport.SelectedValue)
    '            End If
    '        End If
    '        ResultField = "Reference, PONo, Supplier, Supplier_Name, Attn, Report"
    '        ViewState("Sender") = "btnReffNo"
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn get Dt Error : " + ex.ToString
    '    End Try

    'End Sub



    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    Try
    '        tbReffNo.Text = ""
    '        tbPONo.Text = ""
    '    Catch ex As Exception
    '        lbStatus.Text = "ddlReport_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub
    Protected Sub btnWorkBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkBy.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Team_Code, Team_Name, Team_Type from V_MsTeam "
            ResultField = "Team_Code, Team_Name, Team_Type"
            ViewState("Sender") = "btnWorkBy"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Workby Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnKontraktor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKontraktor.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Supplier_Code, Supplier_Name from V_MsSupplier "
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnKontraktor"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Kontraktor Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDivisiBlok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivisiBlok.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select DivisiBlokCode, DivisiBlokName from V_MsDivisiBlock "
            ResultField = "DivisiBlokCode, DivisiBlokName"
            ViewState("Sender") = "btnDivisiBlok"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Kontraktor Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJob.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Job_Code, Job_Name from V_MsJobPlant"
            ResultField = "Job_Code, Job_Name"
            ViewState("Sender") = "btnJob"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Job Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnWOResult_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOResult.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select * from V_PLWOBAPGetReff"
            ResultField = "Result_No, Result_Date, WO_No, Type, DivisiBlok, DivisiBlokName, Job, Job_Name,Qty, Unit, Price, AmountForex, Disc, DiscForex, NettoForex, Remark"
            ViewState("Sender") = "btnWOResult"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Job Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, sqlstring, ResultSame, CriteriaField As String
        Try

            sqlstring = "EXEC S_PLWOBAPReff '',''"
            Session("filter") = sqlstring
            ResultField = "Supplier, Supplier_Name, WorkBy, TeamName, Result_No, Result_Date, Division, DivisionName, WO_No, Type, DivisiBlok, DivisiBlokName,  FgBorongan, Job, Job_Name," + _
                            ",Qty, Unit, Price, AmountForex, Disc, DiscForex, NettoForex, Remark, Status,FgInput "

            CriteriaField = "Supplier, Supplier_Name, WorkBy, TeamName, Result_No, Result_Date, Division, DivisionName, WO_No, Type, DivisiBlok, DivisiBlokName, FgBorongan, Job, Job_Name," + _
                            ",Qty, Unit, Price, AmountForex, Disc, DiscForex, NettoForex, Remark, Status,FgInput "

            Session("DBConnection") = ViewState("DBConnection")
            'Session("ClickSame") = "Supplier, WorkBy "
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Supplier, WorkBy "
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetDt Error : " + ex.ToString
        End Try
    End Sub


End Class
