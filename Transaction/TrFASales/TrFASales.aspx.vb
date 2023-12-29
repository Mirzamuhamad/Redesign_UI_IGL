Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrFASales
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFASalesHd"
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            tbRate.Enabled = True
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnUser" Then
                    tbUserCode.Text = Session("Result")(0).ToString
                    tbUserName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnPayment" Then
                    tbPayCode.Text = Session("Result")(0).ToString
                    tbPayName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnFA" Then
                    tbFACode.Text = Session("Result")(0).ToString
                    tbFAName.Text = Session("Result")(1).ToString
                    ddlCostCtr.SelectedValue = Session("Result")(2).ToString
                    tbSpecification.Text = Session("Result")(3).ToString
                End If
                If ViewState("Sender") = "btnFALoc" Then
                    tbFALocCode.Text = Session("Result")(0).ToString
                    tbFALocName.Text = Session("Result")(1).ToString
                    tbQtyDt2.Text = Session("Result")(2).ToString
                End If
                If Not ViewState("Sender") Is Nothing Then
                    ViewState("Sender") = Nothing
                End If
                If Not Session("Result") Is Nothing Then
                    Session("Result") = Nothing
                End If
                If Not Session("filter") Is Nothing Then
                    Session("filter") = Nothing
                End If
                If Not Session("Column") Is Nothing Then
                    Session("Column") = Nothing
                End If
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

    Protected Sub btnFALoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFALoc.Click
        Dim ResultField As String
        Try
            lbFADt2.Visible = True
            Session("Result") = Nothing
            Session("filter") = "EXEC S_GLFASalesGetLocation  " + QuotedStr(ddlFALocType.SelectedValue) + ", " + QuotedStr(lbFADt2.Text)
            ResultField = "FA_Location_Code, FA_Location_Name, Qty"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnFALoc"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Location Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbFALocCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFALocCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT FALocationCode, FALocationName FROM V_GLFAOpnameGetDetail WHERE FALocationType =" + QuotedStr(ddlLocationType.SelectedValue) + " AND FACode = " + QuotedStr(tbFA.Text) + " AND FALocationCode=" + QuotedStr(tbLocCode.Text) + "", ViewState("DBConnection").ToString).Tables(0)
            Dt = SQLExecuteQuery("EXEC S_GLFASalesGetFALocation  " + QuotedStr(ddlFALocType.SelectedValue) + ", " + QuotedStr(tbFALocCode.Text) + ", " + QuotedStr(lbFADt2.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbFALocCode.Text = Dr("FA_Location_Code")
                tbFALocName.Text = Dr("FA_Location_Name")
                tbQtyDt2.Text = Dr("Qty")
                'getQtyActual()
            Else
                tbFALocCode.Text = ""
                tbFALocName.Text = ""
                tbQtyDt2.Text = "0"
            End If
            tbFALocCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb FALocation Error : " + ex.ToString)
        End Try
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
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")


        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAmountForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbBaseForex.Attributes.Add("OnBlur", "BasePPnOtherTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        tbPPN.Attributes.Add("OnBlur", "BasePPnOtherTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        tbPPNForex.Attributes.Add("OnBlur", "BasePPnOtherTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        tbTotalForex.Attributes.Add("OnBlur", "BasePPnOtherTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")

        'tbQty.Attributes.Add("OnBlur", "setformatdt();")
        tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")
        'tbAmountBuy.Attributes.Add("OnBlur", "setformatdt();")
        tbAmountHome.Attributes.Add("OnBlur", "setformatdt();")

    End Sub
    Protected Sub ddlFALocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFALocType.SelectedIndexChanged
        tbFALocCode.Text = ""
        tbFALocName.Text = ""
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
        Return "SELECT * From V_GLFASalesDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLFASalesDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_GLFormFASales " + Result
                Session("ReportFile") = ".../../../Rpt/FormFASales.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_GLFASales", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlReport.Enabled = State
            ddlCurr.Enabled = State

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
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbFACode.Text Then
                    If CekExistData(ViewState("Dt"), "FixedAsset", tbFACode.Text) Then
                        lbStatus.Text = "Fixed Asset " + tbFACode.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("FixedAsset = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("FixedAsset") = tbFACode.Text
                Row("FA_Name") = tbFAName.Text
                Row("Qty") = tbQty.Text
                Row("AmountForex") = cekValue(tbAmountForex.Text)
                Row("AmountFA") = "0"
                Row("AmountHome") = tbAmountHome.Text
                Row("CostCtr") = ddlCostCtr.SelectedValue
                Row("Cost_Ctr_Name") = ddlCostCtr.SelectedItem.Text
                Row("Specification") = tbSpecification.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "FixedAsset", tbFACode.Text) Then
                    lbStatus.Text = "Fixed Asset " + tbFACode.Text + " has been already exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFACode.Text
                dr("FA_Name") = tbFAName.Text
                dr("Qty") = tbQty.Text
                dr("AmountForex") = cekValue(tbAmountForex.Text)
                dr("AmountFA") = "0"
                dr("AmountHome") = tbAmountHome.Text
                dr("CostCtr") = ddlCostCtr.SelectedValue
                dr("Cost_Ctr_Name") = ddlCostCtr.SelectedItem.Text
                dr("Specification") = tbSpecification.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("FALocationType+'|'+FALocationCode = " + QuotedStr(ddlFALocType.SelectedValue + "|" + tbFALocCode.Text))(0)
                Row.BeginEdit()
                Row("FixedAsset") = lbFADt2.Text
                Row("FALocationType") = ddlFALocType.SelectedValue
                Row("FALocationCode") = tbFALocCode.Text
                Row("FA_Location_Name") = tbFALocName.Text
                Row("Qty") = tbQtyDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("FixedAsset") = lbFADt2.Text
                dr("FALocationType") = ddlFALocType.SelectedValue
                dr("FALocationCode") = tbFALocCode.Text
                dr("FA_Location_Name") = tbFALocName.Text
                dr("Qty") = tbQtyDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            CountTotalDt()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Private Sub CountTotalDt()
        Dim QtyTotal As Double
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim havedetail As Boolean
        Try
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            QtyTotal = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
                    End If
                Next

            End If
            Dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))(0)

            Dr.BeginEdit()
            Dr("Qty") = QtyTotal 'FormatNumber(QtyTotal, ViewState("DigitQty"))
            'Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
            Dr.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)
            'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("FAJ", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO GLFASalesHd (TransNmbr, FgReport, Status, TransDate, " + _
                " UserType, UserCode, Attn, PayWith, PaymentType, Term, " + _
                " DueDate, PPNNo, PPNDate, PPNRate, Currency, ForexRate, BaseForex, PPN, PPnForex, TotalForex, Remark, UserPrep, DatePrep) " + _
                " SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlReport.SelectedValue) + _
                ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlUserType.SelectedValue) + "," + QuotedStr(tbUserCode.Text) + "," + QuotedStr(tbAttn.Text) + ", " + _
                QuotedStr(ddlPayWith.SelectedValue) + "," + QuotedStr(tbPayCode.Text) + "," + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + _
                tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + ", " + _
                tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFASalesHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFASalesHd SET UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                ", UserCode = " + QuotedStr(tbUserCode.Text) + ", Attn =" + QuotedStr(tbAttn.Text) + _
                ", PayWith = " + QuotedStr(ddlPayWith.SelectedValue) + _
                ", PaymentType = " + QuotedStr(tbPayCode.Text) + _
                ", Term = " + QuotedStr(ddlTerm.SelectedValue) + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + _
                ", PPnDate = '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + _
                ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FixedAsset, Qty, AmountForex, AmountHome, AmountFA, CostCtr FROM GLFASalesDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFASalesDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, FixedAsset, FALocationType, FALocationCode, Qty, Remark FROM GLFASalesDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("GLFATransferDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
            '    Exit Sub
            'End If
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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
            tbFilter.Text = tbCode.Text
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
            ChangeReport("Add", ddlReport.SelectedValue, True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True

            tbCode.Focus()
            ddlUserType.Enabled = True
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            ViewState("DigitCurr") = 0
            'ddlCurr.SelectedValue = ViewState("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlUserType.SelectedIndex = 0
            tbUserCode.Text = ""
            tbUserName.Text = ""
            tbAttn.Text = ""
            ddlPayWith.Enabled = True
            ddlPayWith.SelectedIndex = 0
            tbPayCode.Text = ""
            tbPayName.Text = ""
            ddlTerm.SelectedIndex = 0
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
            tbPpnRate.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbBaseForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            ddlPayWith_SelectedIndexChanged(Nothing, Nothing)
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbFACode.Text = ""
            tbFAName.Text = ""
            tbAmountForex.Text = "0"
            tbAmountHome.Text = "0"
            ddlCostCtr.SelectedValue = ""
            tbQty.Text = "0"
            tbSpecification.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            ddlFALocType.SelectedIndex = 0
            tbFALocCode.Text = ""
            tbFALocName.Text = ""
            tbQtyDt2.Text = "0"
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
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
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
        AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        btnFA.Focus()
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDtKe2.Click, btnAddDt2Ke2.Click, btnAddDt2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Function CekHd() As Boolean
        Try

            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("FA Sales Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlTerm.SelectedValue.Trim = "" And ddlPayWith.SelectedValue <> "Cash" Then
                lbStatus.Text = MessageDlg("Term must have value")
                ddlTerm.Focus()
                Return False
            End If
            If tbPayCode.Text.Trim = "" And ddlPayWith.SelectedValue = "Cash" Then
                lbStatus.Text = MessageDlg("Receipt must have value")
                tbPayCode.Focus()
                Return False
            End If
            If tbDueDate.IsNull Then
                lbStatus.Text = MessageDlg("Due Date must have value")
                tbDueDate.Focus()
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
                'If Dr("Factory").ToString.Trim = "" Then
                '    lbStatus.Text = "Factory Must Have Value"
                '    Return False
                'End If
                'If Dr("SJNo").ToString.Trim = "" Then
                '    lbStatus.Text = "BPB No Must Have Value"
                '    Return False
                'End If
                'If Dr("Product").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Must Have Value")
                '    Return False
                'End If
                'If Dr("UnitOrder").ToString.Trim = "" Then
                '    lbStatus.Text = "Unit Order Must Have Value"
                '    Return False
                'End If
                'If CFloat(Dr("QtyOrder")) <= 0 Then
                '    lbStatus.Text = "Qty Order Must Have Value"
                '    Return False
                'End If
            Else
                'If tbBPBNo.Text.Trim = "" Then
                '    lbStatus.Text = "BPB No Must Have Value"
                '    tbBPBNo.Focus()
                '    Return False
                'End If                
                'If tbProductCode.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Product Must Have Value")
                '    tbProductCode.Focus()
                '    Return False
                'End If
                'If tbUnit.Text.Trim = "" Then
                '    lbStatus.Text = "Unit Order Must Have Value"
                '    tbUnit.Focus()
                '    Return False
                'End If
                'If CFloat(tbQtyOrder.Text) <= 0 Then
                '    lbStatus.Text = "Qty Order Must Have Value"
                '    tbQtyOrder.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("FALocationCode").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    Return False
                End If
                If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If

            Else
                If tbFALocCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    tbFALocCode.Focus()
                    Return False
                End If
                If tbQtyDt2.Text = "0" Or tbQty.Text = "" Then
                    'lbStatus.Text = "Qty Must Have Value"
                    'Exit Function
                    'MessageDlg("Qty Must Have Value")
                    'tbQtyDt2.Focus()
                    'Return False
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
            FDateName = "TT Date, Due Date"
            FDateValue = "TransDate, DueDate"
            FilterName = "Reference, User Type, User Name, Attn, Currency, Remark"
            FilterValue = "TransNmbr, UserType, User_Name, Attn, Currency, Remark"
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
                    ChangeReport("View", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    EnableHd(False)
                    ddlUserType.Enabled = False
                    ddlPayWith.Enabled = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
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
                        BindDataDt2(ViewState("TransNmbr"))

                        ViewState("StateHd") = "Edit"
                        ddlUserType.Enabled = False
                        ddlPayWith.Enabled = True

                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                        ddlPayWithChange()
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
                        Session("SelectCommand") = "EXEC S_GLFormFASales '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormFASales.frx"
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
            End If
            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                Dim lbFA As Label

                lbFA = GVR.FindControl("lbFa")

                lbFADt2.Text = GVR.Cells(2).Text
                lbFANameDt2.Text = GVR.Cells(3).Text
                lbCostCtr.Text = GVR.Cells(5).Text
                lbCurr.Text = ddlCurr.SelectedValue
                MultiView1.ActiveViewIndex = 1

                ViewState("DigitCurr") = GetCurrDigit(lbCurr.Text, ViewState("DBConnection").ToString)
                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                End If
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text))) '+ " AND FAName = " + QuotedStr(TrimStr(lbFANameDt2.Text)) + " AND FAStatus = " + QuotedStr(TrimStr(lbStatusFA.Text)))
                '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = False
                End If

                If ViewState("StateHd") <> "View" Then
                    ddlPayWithChange()
                End If

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            If GetCountRecord(ViewState("Dt2")) <> 0 Then
                lbStatus.Text = " Data Detail exist"
                Exit Sub
            Else
                GVR = GridDt.Rows(e.RowIndex)
                dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR.Cells(2).Text))

                dr(0).Delete()
                BindGridDt(ViewState("Dt"), GridDt)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            End If

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalQty As Decimal = 0
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("FALocationType+'|'+FALocationCode = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            CountTotalDt()

            'BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
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
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0
    Dim PPnForex As Decimal = 0
    Dim TotalForex As Decimal = 0

    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "FixedAsset")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
                    tbPPNForex.Text = FormatNumber(((CFloat(tbBaseForex.Text) * CFloat(tbPPN.Text)) / 100).ToString, ViewState("DigitCurr").ToString)
                    tbTotalForex.Text = FormatNumber(CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr").ToString)
                End If

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlPayWith, Dt.Rows(0)("PayWith").ToString)
            BindToText(tbUserCode, Dt.Rows(0)("UserCode").ToString)
            BindToText(tbUserName, Dt.Rows(0)("User_Name").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToText(tbPayCode, Dt.Rows(0)("PaymentType").ToString)
            BindToText(tbPayName, Dt.Rows(0)("PaymentTypeName").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString)
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            tbPayCode.Enabled = ddlPayWith.SelectedIndex = 0
            btnPayment.Visible = ddlPayWith.SelectedIndex = 0
            ddlTerm.Enabled = ddlPayWith.SelectedIndex <> 0
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal FixedAsset As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FixedAsset= " + QuotedStr(FixedAsset))
            If Dr.Length > 0 Then
                BindToText(tbFACode, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FA_Name").ToString)
                BindToDropList(ddlCostCtr, Dr(0)("CostCtr").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToText(tbAmountHome, Dr(0)("AmountHome").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal FALoc As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("FALocationType+'|'+FALocationCode = " + QuotedStr(FALoc))
            If Dr.Length > 0 Then
                BindToDropList(ddlFALocType, Dr(0)("FALocationType").ToString)
                BindToText(tbFALocCode, Dr(0)("FALocationCode").ToString)
                BindToText(tbFALocName, Dr(0)("FA_Location_Name").ToString)
                BindToText(tbQtyDt2, Dr(0)("Qty").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
        ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
    End Sub


    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUser.Click
        Dim ResultField As String
        Try
            Session("filter") = "select User_Code, User_Name, Contact_Person, Term, Currency from VMsUserType where User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnUser"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn User Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbUserCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbUserCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("UserType", tbUserCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbUserCode.Text = Dr("User_Code")
                tbUserName.Text = Dr("User_Name")
                BindToText(tbAttn, Dr("Contact_Person"))
            Else
                tbUserCode.Text = ""
                tbUserName.Text = ""
                tbAttn.Text = ""
            End If
            tbUserCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb UserCode Error : " + ex.ToString)
        End Try
    End Sub


    Private Function cekValue(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function

    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTerm')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Term Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPayment.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsPayType WHERE FgType IN ('A', 'R') AND FgMode ='K'"
            ResultField = "Payment_Code, Payment_Name"
            ViewState("Sender") = "btnPayment"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Payment Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPayWith_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayWith.SelectedIndexChanged
        tbPayCode.Enabled = ddlPayWith.SelectedIndex = 0
        btnPayment.Visible = ddlPayWith.SelectedIndex = 0
        ddlTerm.Enabled = ddlPayWith.SelectedIndex <> 0
        tbPayCode.Text = ""
        tbPayName.Text = ""
        ddlTerm.SelectedIndex = 0
    End Sub

    Private Sub ddlPayWithChange()
        tbPayCode.Enabled = ddlPayWith.SelectedIndex = 0
        btnPayment.Visible = ddlPayWith.SelectedIndex = 0
        ddlTerm.Enabled = ddlPayWith.SelectedIndex <> 0
    End Sub

    Protected Sub tbPayCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPayCode.TextChanged
        Dim Dt As DataTable
        Try
            Dt = SQLExecuteQuery("Select Payment_Code, Payment_Name from VMsPayType Where FgType in ('A', 'R') AND FgMode ='K' AND Payment_Code = " + QuotedStr(tbPayCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                tbPayCode.Text = Dt.Rows(0)("Payment_Code")
                tbPayName.Text = Dt.Rows(0)("Payment_Name")
            Else
                tbPayCode.Text = ""
                tbPayName.Text = ""
            End If
            tbPayCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb PayCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnFA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFA.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsFixedAsset where Qty > 0"
            ResultField = "FA_Code, FA_Name, CostCtr, Specification"
            ViewState("Sender") = "btnFA"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn FA Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFACode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFACode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("FA", tbFACode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFACode.Text = Dr("FA_Code")
                tbFAName.Text = Dr("FA_Name")
                ddlCostCtr.SelectedValue = Dr("CostCtr")
            Else
                tbFACode.Text = ""
                tbFAName.Text = ""
                ddlCostCtr.SelectedValue = ""
            End If
            tbFACode.Focus()
        Catch ex As Exception
            Throw New Exception("tb FACode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbUserCode.Text = ""
        tbUserName.Text = ""
        tbAttn.Text = ""
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnBackDt2ke1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2ke1.Click, btnBackDt2ke2.Click
        Try
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAmountForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAmountForex.TextChanged
        tbAmountForex.Text = FormatNumber(CFloat(tbAmountForex.Text.Replace(",", "")), ViewState("DigitCurr").ToString)
        tbAmountHome.Text = FormatNumber(CFloat(tbRate.Text.Replace(",", "") * tbAmountForex.Text.Replace(",", "")), ViewState("DigitCurr").ToString)
    End Sub




End Class
