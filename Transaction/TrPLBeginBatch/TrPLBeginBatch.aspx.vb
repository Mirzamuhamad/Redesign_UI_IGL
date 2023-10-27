Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrPLBeginBatch_TrPLBeginBatch
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, TransDate, Status, BatchNo, Division, DivisionName, Varietas, VarietasName, TotalNilai, TotalMN, TotalPN, TotalBook, UserPrep, DatePrep, UserAppr, DateAppr, Remark " + _
                " FROM V_PLBeginBatchHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbTAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyOK.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyRepair.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyModuleM.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyBooking.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyMax.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyTertanam.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbSaldoCap.Attributes.Add("OnKeyDown", "return PressNumeric();")

                Session("AdvanceFilter") = ""
            End If

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnBlock" Then
                    BindToText(tbBlock, Session("Result")(0).ToString)
                    BindToText(tbBlockName, Session("Result")(1).ToString)
                    BindToText(tbQtyTertanam, Session("Result")(2).ToString)
                    BindToText(tbQtyMax, Session("Result")(3).ToString)
                    tbBooking_TextChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnJob" Then
                    BindToText(tbJob, Session("Result")(0).ToString)
                    BindToText(tbJobName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnModule" Then
                    BindToText(tbModule, Session("Result")(0).ToString)
                    BindToText(tbModuleName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnBedeng" Then
                    BindToText(tbBedeng, Session("Result")(0).ToString)
                    BindToText(tbBedengName, Session("Result")(1).ToString)
                End If



                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing
            End If
            'dsUseRollNo.ConnectionString = ViewState("DBConnection")
            lbStatus.Text = ""
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
                '  GridDt2.Columns(0).Visible = GetCountRecord(ViewState("Dt2")) > 0

            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                '  GridDt3.Columns(0).Visible = GetCountRecord(ViewState("Dt3")) > 0

                'BindDataDt3(ViewState("TransNmbr"))
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                PnlDt4.Visible = True
                pnlEditDt4.Visible = False
                '  GridDt4.Columns(0).Visible = GetCountRecord(ViewState("Dt4")) > 0
                'BindDataDt4(ViewState("TransNmbr"))
            Else
                pnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlDivisi, "EXEC S_GetPlantDivision", False, "DivisionCode", "DivisionName", ViewState("DBConnection"))
            FillCombo(ddlVarietas, "EXEC S_GetVarietas", True, "VarietasCode", "VarietasName", ViewState("DBConnection"))


            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            ' tbQtyBlock.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyBlock.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyTotal.Attributes.Add("ReadOnly", "True")
            'tbQtyTotal.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnBlur", "setformatdt()")



        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
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
            SQLString = GetStringHd
            DT = BindDataTransaction(SQLString, StrFilter, ViewState("DBConnection").ToString)
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLBeginBatchDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLBeginBatchMN WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLBeginBatchPN WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLBeginBatchBlock WHERE TransNmbr = " + QuotedStr(Nmbr)
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

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PLBeginBatch", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlDivisi.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try
            'V_PLBeginBatchPN
            tbBedeng.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
            tbBlock.Enabled = State
            'diremark tbStartTime.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt4 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
            ' Drow = ViewState("Dt2").Select("Job=" + QuotedStr(GVR.cell().text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt2)
            ' GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            ' Else
            ' Dim DtTemp As DataTable
            ' DtTemp = ViewState("Dt2").Clone
            ' DtTemp.Rows.Add(DtTemp.NewRow())
            ' GridDt2.DataSource = DtTemp
            ' GridDt2.DataBind()
            'GridDt2.Columns(0).Visible = False
            ' End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
            ' Drow = ViewState("Dt3").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBlockE.Text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt3)
            ' GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "ViewE"
            'Else
            'Dim DtTemp As DataTable
            'DtTemp = ViewState("Dt3").Clone
            'DtTemp.Rows.Add(DtTemp.NewRow())
            'GridDt3.DataSource = DtTemp
            'GridDt3.DataBind()
            'GridDt3.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            BindGridDt(dt, GridDt4)
            'Drow = ViewState("Dt4").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeMT.Text + "|" + LblBlockMT.Text))
            'If Drow.Length > 0 Then
            '    BindGridDt(Drow.CopyToDataTable, GridDt4)
            '    GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "ViewM"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt4").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt4.DataSource = DtTemp
            '    GridDt4.DataBind()
            '    GridDt4.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDt2(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            If IsEmpty = True Then
                gv.Columns(0).Visible = False
            Else
                gv.Columns(0).Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlVarietas.SelectedValue = ""
            tbBatchNo.Text = ""
            ddlVarietas.SelectedValue = ""
            tbQtyModule.Text = "0"
            tbQtyBedeng.Text = "0"
            tbAmount.Text = "0"
            tbBooking.Text = "0"
            tbRemarkHD.Text = ""
            Dim Division As String
            Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))
            ddlDivisi.SelectedValue = Division

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbJob.Text = ""
            tbJobName.Text = ""
            tbTAmount.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbModule.Text = ""
            tbModuleName.Text = ""
            tbQtyOK.Text = "0"
            tbQtyRepair.Text = "0"
            tbQtyModuleM.Text = "0"
            tbremarkjob.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbBedeng.Text = ""
            tbBedengName.Text = ""
            tbQtyE.Text = "0"
            tbremarkB.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbBlock.Text = ""
            tbBlockName.Text = ""
            tbQtyMax.Text = "0"
            tbQtyTertanam.Text = "0"
            tbQtyBooking.Text = "0"
            tbSaldoCap.Text = "0"
            tbRemarkDt.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt 4 Error " + ex.ToString)
        End Try
    End Sub



    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbBatchNo.Text = "" Then
                lbStatus.Text = MessageDlg("Bacth No must have value")
                tbBatchNo.Focus()
                Return False
            End If
            If ddlVarietas.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Varietas must have value")
                ddlVarietas.Focus()
                Return False
            End If
           
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If CFloat(tbWorkHour.Text) <= 0 Then
            '    tbWorkHour.Text = MessageDlg("Work Hour must have value")
            '    tbWorkHour.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Job").ToString = "" Then
                    lbStatus.Text = MessageDlg("Job Must Have Value")
                    Return False
                End If
                If LTrim(Dr("Amount").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Must Have Value")
                    Return False
                End If
                
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
                If Dr("Module").ToString = "" Then
                    lbStatus.Text = MessageDlg("Module Must Have Value")
                    Return False
                End If
                If Len(Dr("Qty").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Qty Module Must Have Value")
                    Return False
                End If

            Else
                If tbModule.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Module must have value")
                    tbModule.Focus()
                    Return False
                End If
                If tbQtyOK.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty OK must have value")
                    tbQtyOK.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt4(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Block").ToString = "" Then
                    lbStatus.Text = MessageDlg("Block Must Have Value")
                    Return False
                End If
                ''If Dr("StartTime").ToString = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                ''    Return False
                ''End If
                'If CFloat(Dr("Duration").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    Return False
                'End If


            Else
                If tbBlock.Text = "" Then
                    lbStatus.Text = MessageDlg("Block must have value")
                    tbModule.Focus()
                    Return False
                End If
                ''If tbStartTime.Text.Trim = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time must have value")
                ''    tbStartTime.Focus()
                ''    Return False
                ''End If
                'If CFloat(tbDuration.Text.Trim) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    tbDuration.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlDivisi, Dt.Rows(0)("Division").ToString)
            BindToDropList(ddlVarietas, Dt.Rows(0)("Varietas").ToString)
            BindToText(tbBatchNo, Dt.Rows(0)("BatchNo").ToString)
            BindToText(tbQtyModule, FormatNumber(Dt.Rows(0)("TotalMN").ToString), 0)
            BindToText(tbQtyBedeng, FormatNumber(Dt.Rows(0)("TotalPN").ToString), 0)
            BindToText(tbBooking, FormatNumber(Dt.Rows(0)("TotalBook").ToString), 0)
            BindToText(tbAmount, FormatNumber(Dt.Rows(0)("TotalNilai").ToString), 0)
            BindToText(tbRemarkHD, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Job = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbJob, Dr(0)("Job").ToString)
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbTAmount, CFloat(Dr(0)("Amount").ToString))
                BindToText(tbRemark, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Module = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbModule, Dr(0)("Module").ToString)
                BindToText(tbModuleName, Dr(0)("ModuleName").ToString)
                BindToText(tbQtyOK, Dr(0)("Qty").ToString)
                BindToText(tbQtyRepair, Dr(0)("QtyRepair").ToString)
                BindToText(tbQtyModuleM, Dr(0)("QtyModule").ToString)
                BindToText(tbremarkjob, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("Bedeng = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbBedeng, Dr(0)("Bedeng").ToString)
                BindToText(tbBedengName, Dr(0)("BedengName").ToString)
                BindToText(tbQtyE, Dr(0)("Qty").ToString)
                BindToText(tbremarkB, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt4(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("Block = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbBlock, Dr(0)("Block").ToString)
                BindToText(tbBlockName, Dr(0)("BlockName").ToString)
                BindToText(tbQtyMax, Dr(0)("QtyMax").ToString)
                BindToText(tbQtyTertanam, Dr(0)("QtyUse").ToString)
                BindToText(tbQtyBooking, Dr(0)("Qtytanam").ToString)
                BindToText(tbSaldoCap, Dr(0)("QtySaldo").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 4 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("Job = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("Job") = tbJob.Text
                Row("JobName") = tbJobName.Text
                Row("Amount") = tbTAmount.Text 'FormatFloat(tbTAmount.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemark.Text
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "Job", tbJob.Text) = True Then
                    lbStatus.Text = "Job " + tbJob.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Job") = tbJob.Text
                dr("JobName") = tbJobName.Text
                dr("Amount") = tbTAmount.Text
                dr("Remark") = tbRemark.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            'GridDt.Columns(1).Visible = True

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("Module = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("Module") = tbModule.Text
                Row("ModuleName") = tbModuleName.Text
                Row("Qty") = tbQtyOK.Text
                Row("QtyRepair") = tbQtyRepair.Text
                Row("QtyModule") = tbQtyModuleM.Text
                Row("Remark") = tbremarkjob.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("Module") = tbModule.Text
                dr("ModuleName") = tbModuleName.Text
                dr("Qty") = tbQtyOK.Text
                dr("QtyRepair") = tbQtyRepair.Text
                dr("QtyModule") = tbQtyModuleM.Text
                dr("Remark") = tbremarkjob.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
                tbCode.Text = GetAutoNmbr("BBT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLBeginBatchHd (TransNmbr, TransDate, Status, BatchNo, Division, " + _
                "Varietas, TotalMN, TotalPN, TotalBook, TotalNilai, Remark, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbBatchNo.Text) + "," + _
                QuotedStr(ddlDivisi.SelectedValue) + "," + QuotedStr(ddlVarietas.SelectedValue) + "," + _
                tbQtyModule.Text.Replace(",", "") + "," + tbQtyBedeng.Text.Replace(",", "") + "," + _
                tbBooking.Text.Replace(",", "") + "," + tbAmount.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemarkHD.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLBeginBatchHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLBeginBatchHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Division = " + QuotedStr(ddlDivisi.SelectedValue) + ",BatchNo = " + QuotedStr(tbBatchNo.Text) + ", Varietas = " + QuotedStr(ddlVarietas.SelectedValue) + _
                ", TotalMN = " + tbQtyModule.Text.Replace(",", "") + ", TotalPN = " + tbQtyBedeng.Text.Replace(",", "") + ", TotalBook =" + QuotedStr(tbBooking.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemarkHD.Text) + _
                ", TotalNilai = " + tbAmount.Text.Replace(",", "") + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
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

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Job, Amount, Remark FROM PLBeginBatchDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '        "UPDATE PLBeginBatchDt SET Job = @Job, Amount = @Amount, Remark = @Remark " + _
            '        " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Job = @OldJob  ", con)
            '' Define output parameters.
            'Update_Command.Parameters.Add("@Job", SqlDbType.VarChar, 8, "Job")
            'Update_Command.Parameters.Add("@Amount", SqlDbType.Float, 18, "Amount")
            'Update_Command.Parameters.Add("@StartDate", SqlDbType.DateTime, 8, "StartDate")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")


            '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldJob", SqlDbType.VarChar, 8, "Job")
            'param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM PLBeginBatchDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Job = @Job", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@Job", SqlDbType.VarChar, 8, "Job")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLBeginBatchDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, Module, Qty, Remark, QtyRepair FROM PLBeginBatchMN WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PLBeginBatchMN")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr,Bedeng, Qty, Remark FROM PLBeginBatchPN WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PLBeginBatchPN")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            cmdSql = New SqlCommand("SELECT TransNmbr, Block, QtyMax, QtyUse, QtyTanam, QtySaldo, Remark FROM PLBeginBatchBlock WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt4 As New DataTable("PLBeginBatchBlock")

            Dt4 = ViewState("Dt4")
            da.Update(Dt4)
            Dt4.AcceptChanges()
            ViewState("Dt4") = Dt4

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
                lbStatus.Text = MessageDlg("Detail 'Item' must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Material Waste' must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Machine Down Time' must have at least 1 record")
            '    Exit Sub
            'End If
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
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt2")) > 0
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            tbBatchNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbJob.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbBlock.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            BindDataDt4("")

            EnableHd(True)

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "Trasn No, Trasb Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria()", Page, Me.GetType())
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
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    BindDataDt4(ViewState("TransNmbr"))

                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, PnlDt4, GridDt4)

                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    'GridDt.Columns(1).Visible = True
                   
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        Dim Division As String
                        Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))

                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        BindDataDt4(ViewState("TransNmbr"))

                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        'GridDt.Columns(1).Visible = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PLFormBeginBatch " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPLBeginBatch.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            Dim GVR As GridViewRow
            If e.CommandName = "View" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                '  lbWODt2.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Job = " + QuotedStr(GVR.Cells(2).Text))
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
                End If
            ElseIf e.CommandName = "ViewA" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Module = " + QuotedStr(GVR.Cells(2).Text))
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
                End If
            ElseIf e.CommandName = "ViewE" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 2
               
                Dim drow As DataRow()
                If ViewState("Dt3") Is Nothing Then
                    BindDataDt3(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt3").Select("Bedeng = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        GridDt3.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewM" Then

                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 3
                
                Dim drow As DataRow()
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("Block = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt4)
                        GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt4").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt4.DataSource = DtTemp
                        GridDt4.DataBind()
                        GridDt4.Columns(0).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Dim TQtyWO As Double = 0
    Dim MaxItem As Integer = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Job")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    MaxItem = MaxItem + 1
                    TQtyWO = GetTotalSum(ViewState("Dt"), "Amount")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbAmount.Text = TQtyWO

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try

    '    Catch ex As Exception
    '        lbStatus.Text = "GridDt_RowDataBound Error : " & ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r, drt As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Job = " + QuotedStr(GVR.Cells(2).Text))
            For Each r In dr
                r.Delete()
            Next

            For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
                drt = ViewState("Dt2").Rows(i)
                If Not drt.RowState = DataRowState.Deleted Then
                    drt.Delete()
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TQty As Double = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Module")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    TQty = GetTotalSum(ViewState("Dt2"), "QtyModule")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbQtyModule.Text = TQty

        Catch ex As Exception
            lbStatus.Text = "GridDt2_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Module = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            dr = ViewState("Dt2").Select("Module = " + QuotedStr(GVR.Cells(1).Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Job' must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"

            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            'tbEquip.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("Bedeng = " + QuotedStr(ViewState("Dt3Value")))(0)
                Row.BeginEdit()
                Row("Bedeng") = tbBedeng.Text
                Row("BedengName") = tbBedengName.Text
                Row("Qty") = tbQtyE.Text
                Row("Remark") = tbremarkB.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("Bedeng") = tbBedeng.Text
                dr("BedengName") = tbBedengName.Text
                dr("Qty") = tbQtyE.Text
                dr("Remark") = tbremarkB.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt3"), GridDt3)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Bedeng").ToString = "" Then
                    lbStatus.Text = MessageDlg("Bedeng Must Have Value")
                    Return False
                End If
                If Dr("BedengName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Bedeng Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Bedeng Must Have Value")
                    Return False
                End If
                
            Else
                If tbBedeng.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Bedeng Must Have Value")
                    tbBedengName.Focus()
                    Return False
                End If
                If CFloat(tbQtyE.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Bedeng Must Have Value")
                    tbQtyE.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Dim TQtyB As Double = 0
    Protected Sub GridDt3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt3.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Bedeng")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    TQtyB = GetTotalSum(ViewState("Dt3"), "Qty")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbQtyBedeng.Text = TQtyB


        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("Bedeng = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            ViewState("Dt3Value") = GVR.Cells(1).Text
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJob.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Job_Code, Job_Name, Level_Plant, Job_No, Unit, FgCIP From V_MsJobPlant "
            ResultField = "Job_Code, Job_Name, Unit, FgCIP"
            ViewState("Sender") = "btnJob"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnJob_Click Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt4.Click
        Try
            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If
            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt4").Select("Block = " + QuotedStr(ViewState("Dt4Value")))(0)
                Row.BeginEdit()
                Row("Block") = tbBlock.Text
                Row("BlockName") = tbBlockName.Text
                Row("QtyMax") = tbQtyMax.Text.Replace(",", "")
                Row("QtyUse") = tbQtyTertanam.Text
                Row("Qtytanam") = tbQtyBooking.Text 'FormatFloat(tbQtyBooking.Text, ViewState("DigitQty"))
                Row("QtySaldo") = tbSaldoCap.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("Block") = tbBlock.Text
                dr("BlockName") = tbBlockName.Text
                dr("QtyMax") = tbQtyMax.Text.Replace(",", "")
                dr("QtyUse") = tbQtyTertanam.Text
                dr("Qtytanam") = tbQtyBooking.Text 'FormatFloat(tbQtyBooking.Text, ViewState("DigitQty"))
                dr("QtySaldo") = tbSaldoCap.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt4").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt4"), GridDt4)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt4 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Dim TQtyA As Double = 0
    Protected Sub GridDt4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt4.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    TQtyA = GetTotalSum(ViewState("Dt4"), "QtyTanam")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbBooking.Text = TQtyA

        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("Block = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            FillTextBoxDt4(GVR.Cells(1).Text)
            ViewState("Dt4Value") = GVR.Cells(1).Text
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            EnableDt4(False)
        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt4.Click, btnAddDt4ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            StatusButtonSave(False)
            EnableDt4(True)
            tbBlock.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt4.Click
        Try
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text, GVR.Cells(3).Text)
            ViewState("DtValue") = GVR.Cells(2).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    '   btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        '  CheckAllDt(GridRollOutput, sender)
    End Sub

    Protected Sub tbJob_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbJob.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Job_Code, Job_Name, Level_Plant, Job_No, Unit, FgCIP From V_MsJobPlant Where Job_Code= " + QuotedStr(tbJob.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbJob.Text = Dr("Job_Code")
                tbJobName.Text = Dr("Job_Name")
                tbAmount.Focus()
            Else
                tbJob.Text = ""
                tbJobName.Text = ""
                btnJob.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbJob_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    'Protected Sub tbQtyCapacity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyCapacity.TextChanged
    '    Try
    '        If (tbQtyCapacity.Text > 0) And (tbPerson.Text > 0) Then
    '            tbQtyWorkDay.Text = (CFloat(tbQtyWO.Text) / CFloat(tbQtyCapacity.Text)) / CInt(tbPerson.Text)
    '        Else : tbQtyWorkDay.Text = 0
    '        End If

    '        If (tbQtyCapacity.Text > 0) And (tbQtyWO.Text > 0) Then
    '            tbQtyTarget.Text = CFloat(tbQtyWO.Text) / CFloat(tbQtyCapacity.Text)
    '        Else : tbQtyTarget.Text = 0
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception("tbQtyCapacity_TextChanged Error : " + ex.ToString)
    '    End Try
    'End Sub
    
    Protected Sub tbQtyOK_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOK.TextChanged
        Try
            tbQtyModuleM.Text = CFloat(tbQtyOK.Text) + CFloat(tbQtyRepair.Text)
        Catch ex As Exception
            Throw New Exception("tbQtyOK_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyRepair_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyRepair.TextChanged
        Try
            tbQtyModuleM.Text = CFloat(tbQtyOK.Text) + CFloat(tbQtyRepair.Text)
        Catch ex As Exception
            Throw New Exception("tbQtyRepair_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyBooking_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyBooking.TextChanged
        Try
            If tbQtyTertanam.Text = "" Then tbQtyTertanam.Text = "0"
            tbSaldoCap.Text = CFloat(tbQtyMax.Text - tbQtyTertanam.Text - tbQtyBooking.Text)
        Catch ex As Exception
            Throw New Exception("tbQtyBooking_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbBedeng_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBedeng.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select BedengCode, BedengName From V_MsBedeng Where BedengCode= " + QuotedStr(tbBedeng.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbBedeng.Text = Dr("BedengCode")
                tbBedengName.Text = Dr("BedengName")
                btnBedeng.Focus()
            Else
                tbBedeng.Text = ""
                tbBedengName.Text = ""
                tbBedeng.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbBedeng_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbBlock_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBlock.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Block_Code, Block_Name From V_MsBlock Where Block_Code= " + QuotedStr(tbBlock.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbBlock.Text = Dr("Block_Code")
                tbBlockName.Text = Dr("Block_Name")
                btnBlock.Focus()
            Else
                tbBlock.Text = ""
                tbBlockName.Text = ""
                btnBlock.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbBlock_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbModule_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbModule.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Module_Code, Module_Name From V_MsModule Where Module_Code= " + QuotedStr(tbModule.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbModule.Text = Dr("Module_Code")
                tbModuleName.Text = Dr("Module_Name")
                btnModule.Focus()
            Else
                tbModule.Text = ""
                tbModuleName.Text = ""
                btnModule.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbModule_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModule.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Module_Code, Module_Name From V_MsModule "
            ResultField = "Module_Code, Module_Name"
            ViewState("Sender") = "btnModule"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnModule_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBedeng_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBedeng.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT BedengCode, BedengName From V_MsBedeng "
            ResultField = "BedengCode, BedengName"
            ViewState("Sender") = "btnBedeng"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnBedeng_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBlock.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Block_Code, Block_Name, Division, QtyUse, MaxCap From V_PLTransferBatchGetBlock "
            ResultField = "Block_Code, Block_Name, QtyUse, MaxCap"
            ViewState("Sender") = "btnBlock"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnBlock_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbBooking_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBooking.TextChanged
        Try
            tbQtyBooking.Text = CFloat(tbQtyMax.Text - tbQtyTertanam.Text)
            tbSaldoCap.Text = CFloat((tbQtyMax.Text - tbQtyTertanam.Text) - tbQtyBooking.Text)
        Catch ex As Exception
            lbStatus.Text = "tbBooking_TextChanged Error : " + ex.ToString
        End Try

    End Sub
End Class


