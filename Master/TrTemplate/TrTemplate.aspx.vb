Imports System.Data
Imports System.Data.SqlClient

Partial Class TrTemplate
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCIPTemplateHd"

    'Private Function GetStringHd(ByVal Type As String) As String
    '    Return "SELECT * From V_PRCMTSertifikatHd "
    'End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPTemplateDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNo ASC"
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPTemplateDt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNoDt2 ASC"
    End Function


    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCMTSertifikatDt3 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "BtnPayment" Then
                    tbPaymentNoDt.Text = Session("Result")(0).ToString
                End If


                If ViewState("Sender") = "btnPIC" Then
                    tbPIC.Text = Session("Result")(0).ToString
                    tbPICName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnSpv1" Then
                    tbSpv1.Text = Session("Result")(0).ToString
                    tbSpv1Name.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnSpv2" Then
                    tbSpv2.Text = Session("Result")(0).ToString
                    tbSpv2Name.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnDireksi" Then
                    tbDireksi.Text = Session("Result")(0).ToString
                    tbDireksiName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnQc" Then
                    tbQc.Text = Session("Result")(0).ToString
                    tbQcName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnPICDt2" Then
                    tbPICDt2.Text = Session("Result")(0).ToString
                    tbPICNameDt2.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnSpv1Dt2" Then
                    tbSpv1Dt2.Text = Session("Result")(0).ToString
                    tbSpv1NameDt2.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnSpv2Dt2" Then
                    tbSpv2Dt2.Text = Session("Result")(0).ToString
                    tbSpv2NameDt2.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnDireksiDt2" Then
                    tbDireksiDt2.Text = Session("Result")(0).ToString
                    tbDireksiNameDt2.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnQcDt2" Then
                    tbQcDt2.Text = Session("Result")(0).ToString
                    tbQcNameDt2.Text = Session("Result")(1).ToString
                End If


                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt3").Select("PaymentNo = " + QuotedStr(drResult("PaymentNo").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt3").NewRow
                            dr("PaymentNo") = drResult("PaymentNo")
                            ViewState("Dt3").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt3"), GridDt3)

                    EnableHd(GetCountRecord(ViewState("Dt3")) = 0)
                End If


                'StatusButtonSave(True)
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "SELECT * FROM PRCIPTemplateHd "
            ResultField = "PaymentNo, Status, Remark "
            CriteriaField = "PaymentNo, Status, Remark "
            Session("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
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
            btnSaveTrans.Focus()


            If MultiView1.ActiveViewIndex = 2 Then

                'lbStatus.Text = ViewState("StateHD") = "View"
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                BindDataDt3(ViewState("TransNmbr"))
                'If lbStatus.Text = "True" Then
                '    GridDt3.Columns(0).Visible = True
                'End If

                'If lbStatus.Text = "False" Then
                '    GridDt3.Columns(0).Visible = False
                'End If


            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            'FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If


            tbPercen.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTargetWaktu.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBiaya1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBiaya2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbPercenDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTargetWaktuDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBiaya1Dt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBiaya2Dt2.Attributes.Add("OnKeyDown", "return PressNumeric();")


        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
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
            'DT = BindDataTransaction(GetStringHd(ViewState("TransferType").ToString, ViewState("FgExpendable").ToString), StrFilter, ViewState("DBConnection").ToString)
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate ASC"
            End If
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
            btnAdd2.Visible = False
            ddlCommand.Visible = False
            ddlCommand2.Visible = False
            BtnGo.Visible = False
            btnGo2.Visible = False
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
                    Result = ExecSPCommandGo(ActionValue, "S_PRCMTSertifikat", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        'lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        lbStatus.Text = lbStatus.Text + MessageDlg(Result) + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
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

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
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

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbTahapan.Text = ""
            tbPercen.Text = 0
            tbTargetWaktu.Text = 0
            tbBiaya1.Text = 0
            tbBiaya2.Text = 0
            tbPIC.Text = ""
            tbPICName.Text = ""
            tbSpv1.Text = ""
            tbSpv1Name.Text = ""
            tbSpv2.Text = ""
            tbSpv2Name.Text = ""
            tbDireksi.Text = ""
            tbDireksiName.Text = ""
            tbQc.Text = ""
            tbQcName.Text = ""
            tbRemarkDt.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbTahapanDt2.Text = ""
            tbPercenDt2.Text = 0
            tbTargetWaktuDt2.Text = 0
            tbBiaya1Dt2.Text = 0
            tbBiaya2Dt2.Text = 0
            tbPICDt2.Text = ""
            tbPICNameDt2.Text = ""
            tbSpv1Dt2.Text = ""
            tbSpv1NameDt2.Text = ""
            tbSpv2Dt2.Text = ""
            tbSpv2NameDt2.Text = ""
            tbDireksiDt2.Text = ""
            tbDireksiNameDt2.Text = ""
            tbQcDt2.Text = ""
            tbQcNameDt2.Text = ""
            tbRemarkDtDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If


            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
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
                If Dr("Tahapan").ToString = "" Then
                    lbStatus.Text = MessageDlg("Dokumen Must Have Value")
                    tbTahapan.Focus()
                    Return False
                End If

            Else

                If tbTahapan.Text = "" Then
                    lbStatus.Text = MessageDlg("Tahapan must Have Value")
                    tbTahapan.Focus()
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
                If Dr("Tahapan").ToString = "" Then
                    tbTahapanDt2.Text = MessageDlg("Tahapan Must Have Value")
                    Return False
                End If
                

            Else
                If tbTahapanDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Tahapan Must Have Value")
                    tbTahapanDt2.Focus()
                    Return False
                End If



            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal NoItem As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(NoItem))
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbTahapan, Dr(0)("Tahapan").ToString)
                BindToText(tbPercen, Dr(0)("Percen").ToString)
                BindToText(tbTargetWaktu, Dr(0)("TargetWaktu").ToString)
                BindToText(tbBiaya1, Dr(0)("Biaya1").ToString)
                BindToText(tbBiaya2, Dr(0)("Biaya2").ToString)
                BindToText(tbPIC, Dr(0)("PIC").ToString)
                BindToText(tbPICName, Dr(0)("PICName").ToString)
                BindToText(tbSpv1, Dr(0)("SPV1").ToString)
                BindToText(tbSpv1Name, Dr(0)("SPVName1").ToString)
                BindToText(tbSpv2, Dr(0)("SPV2").ToString)
                BindToText(tbSpv2Name, Dr(0)("SPVName2").ToString)
                BindToText(tbDireksi, Dr(0)("Direksi").ToString)
                BindToText(tbDireksiName, Dr(0)("DireksiName").ToString)
                BindToText(tbQc, Dr(0)("QcVerified").ToString)
                BindToText(tbQcName, Dr(0)("QcVerifiedName").ToString)
                BindToDropList(ddlExtDateUpdate, Dr(0)("FgPermanent").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub FillTextBoxDt2(ByVal Dok As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(Dok))
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = Dr(0)("ItemNoDt2").ToString
                BindToText(tbTahapanDt2, Dr(0)("Tahapan").ToString)
                BindToText(tbPercenDt2, Dr(0)("Percen").ToString)
                BindToText(tbTargetWaktuDt2, Dr(0)("TargetWaktu").ToString)
                BindToText(tbBiaya1Dt2, Dr(0)("Biaya1").ToString)
                BindToText(tbBiaya2Dt2, Dr(0)("Biaya2").ToString)
                BindToText(tbPICDt2, Dr(0)("PIC").ToString)
                BindToText(tbPICNameDt2, Dr(0)("PICName").ToString)
                BindToText(tbSpv1Dt2, Dr(0)("SPV1").ToString)
                BindToText(tbSpv1NameDt2, Dr(0)("SPVName1").ToString)
                BindToText(tbSpv2Dt2, Dr(0)("SPV2").ToString)
                BindToText(tbSpv2NameDt2, Dr(0)("SPVName2").ToString)
                BindToText(tbDireksiDt2, Dr(0)("Direksi").ToString)
                BindToText(tbDireksiNameDt2, Dr(0)("DireksiName").ToString)
                BindToText(tbQcDt2, Dr(0)("QcVerified").ToString)
                BindToText(tbQcNameDt2, Dr(0)("QcVerifiedName").ToString)
                BindToDropList(ddlExtDateUpdateDt2, Dr(0)("FgPermanent").ToString)
                BindToText(tbRemarkDtDt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt3(ByVal PaymentNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("PaymentNo = " + QuotedStr(PaymentNo))
            If Dr.Length > 0 Then
                BindToText(tbPaymentNoDt, Dr(0)("PaymentNo").ToString)
                BindToText(tbRemarkdt3, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub

    'Private Function HasPermanentFlag(ByVal newFlagValue As String, ByRef tahapanY As String) As Boolean
    '    tahapanY = ""

    '    ' Kalau flag baru bukan Y, langsung lolos
    '    If newFlagValue.Trim().ToUpper() <> "Y" Then
    '        Return False
    '    End If

    '    ' --- Cek di GridDt ---
    '    For Each row As DataRow In ViewState("Dt").Rows
    '        If row("FgPermanent").ToString().Trim().ToUpper() = "Y" Then
    '            tahapanY = row("Tahapan").ToString()
    '            Return True
    '        End If
    '    Next

    '    ' --- Cek di GridDt2 ---
    '    For Each row As DataRow In ViewState("Dt2").Rows
    '        If row("FgPermanent").ToString().Trim().ToUpper() = "Y" Then
    '            tahapanY = row("Tahapan").ToString()
    '            Return True
    '        End If
    '    Next
    '    Return False
    'End Function


    Private Function HasPermanentFlag(ByVal newFlagValue As String, ByVal currentItemNo As String, ByRef tahapanY As String) As Boolean
        'tahapanY = ""

        ' Hanya periksa jika flag baru = "Y"
        If newFlagValue.Trim().ToUpper() <> "Y" Then
            Return False
        End If

        ' --- Cek di GridDt ---
        For Each row As DataRow In ViewState("Dt").Rows
            Dim itemNoDt As String = ""
            If row.Table.Columns.Contains("ItemNo") Then
                itemNoDt = row("ItemNo").ToString().Trim()
            End If

            ' Abaikan baris yang sedang diedit
            If itemNoDt <> currentItemNo.Trim() Then

                If row("FgPermanent").ToString().Trim().ToUpper() = "Y" Then
                    tahapanY = row("Tahapan").ToString()
                    Return True
                End If
            End If
        Next

        ' --- Cek di GridDt2 ---
        For Each row As DataRow In ViewState("Dt2").Rows
            Dim itemNoDt2 As String = ""
            If row.Table.Columns.Contains("ItemNoDt2") Then
                itemNoDt2 = row("ItemNoDt2").ToString().Trim()
            ElseIf row.Table.Columns.Contains("ItemNo") Then
                itemNoDt2 = row("ItemNo").ToString().Trim()
            End If

            ' Abaikan baris yang sedang diedit
            If itemNoDt2 <> currentItemNo.Trim() Then

                If row("FgPermanent").ToString().Trim().ToUpper() = "Y" Then
                    tahapanY = row("Tahapan").ToString()
                    Return True
                End If
            End If
        Next

        Return False
    End Function




    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim tbTotal As Double
        Try

            If HasPermanentFlag(ddlExtDateUpdate.SelectedValue, lbItemNo.Text, tbTahapan.Text) Then
                lbStatus.Text = MessageDlg("Sudah ada Exp Date Update = 'Y' di tahapan '" & tbTahapan.Text & "', tidak boleh di tambah kembali Exp Date Update selanjutnya akan default 'N'.")
                ddlExtDateUpdate.SelectedValue = "N"
            End If

            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                'Row("ItemNo") = lbItemNo.Text
                Row("Tahapan") = tbTahapan.Text
                Row("Percen") = tbPercen.Text
                Row("TargetWaktu") = tbTargetWaktu.Text
                Row("Biaya1") = tbBiaya1.Text
                Row("Biaya2") = tbBiaya2.Text
                Row("PIC") = tbPIC.Text
                Row("PICName") = tbPICName.Text
                Row("SPV1") = tbSpv1.Text
                Row("SPVName1") = tbSpv1Name.Text
                Row("SPV2") = tbSpv2.Text
                Row("SPVName2") = tbSpv2Name.Text
                Row("Direksi") = tbDireksi.Text
                Row("DireksiName") = tbDireksiName.Text
                Row("QcVerified") = tbQc.Text
                Row("QcVerifiedName") = tbQcName.Text
                Row("FgPermanent") = ddlExtDateUpdate.SelectedValue
                Row("Remark") = tbRemarkDt.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("Tahapan") = tbTahapan.Text
                dr("Percen") = tbPercen.Text
                dr("TargetWaktu") = tbTargetWaktu.Text
                dr("Biaya1") = tbBiaya1.Text
                dr("Biaya2") = tbBiaya2.Text
                dr("PIC") = tbPIC.Text
                dr("PICName") = tbPICName.Text
                dr("SPV1") = tbSpv1.Text
                dr("SPVName1") = tbSpv1Name.Text
                dr("SPV2") = tbSpv2.Text
                dr("SPVName2") = tbSpv2Name.Text
                dr("Direksi") = tbDireksi.Text
                dr("DireksiName") = tbDireksiName.Text
                dr("QcVerified") = tbQc.Text
                dr("QcVerifiedName") = tbQcName.Text
                dr("FgPermanent") = ddlExtDateUpdate.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            MovePanel(pnlEditDt, PnlDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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
            If HasPermanentFlag(ddlExtDateUpdateDt2.SelectedValue, lbItemNoDt2.Text, tbTahapanDt2.Text) Then
                lbStatus.Text = MessageDlg("Sudah ada Exp Date Update = 'Y' di tahapan '" & tbTahapan.Text & "', tidak boleh di tambah kembali Exp Date Update selanjutnya akan default 'N'.")
                ddlExtDateUpdate.SelectedValue = "N"
            End If

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(lbItem.Text + "|" + lbItemNoDt2.Text))(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItem.Text
                Row("ItemNoDt2") = lbItemNoDt2.Text
                Row("Tahapan") = tbTahapanDt2.Text
                Row("Percen") = tbPercenDt2.Text
                Row("TargetWaktu") = tbTargetWaktuDt2.Text
                Row("Biaya1") = tbBiaya1Dt2.Text
                Row("Biaya2") = tbBiaya2Dt2.Text
                Row("PIC") = tbPICDt2.Text
                Row("PICName") = tbPICNameDt2.Text
                Row("SPV1") = tbSpv1Dt2.Text
                Row("SPVName1") = tbSpv1NameDt2.Text
                Row("SPV2") = tbSpv2Dt2.Text
                Row("SPVName2") = tbSpv2NameDt2.Text
                Row("Direksi") = tbDireksiDt2.Text
                Row("DireksiName") = tbDireksiNameDt2.Text
                Row("QcVerified") = tbQcDt2.Text
                Row("QcVerifiedName") = tbQcNameDt2.Text
                Row("FgPermanent") = ddlExtDateUpdateDt2.SelectedValue
                Row("Remark") = tbRemarkDtDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = lbItem.Text
                dr("ItemNoDt2") = lbItemNoDt2.Text
                dr("Tahapan") = tbTahapanDt2.Text
                dr("Percen") = tbPercenDt2.Text
                dr("TargetWaktu") = tbTargetWaktuDt2.Text
                dr("Biaya1") = tbBiaya1Dt2.Text
                dr("Biaya2") = tbBiaya2Dt2.Text
                dr("PIC") = tbPICDt2.Text
                dr("PICName") = tbPICNameDt2.Text
                dr("SPV1") = tbSpv1Dt2.Text
                dr("SPVName1") = tbSpv1NameDt2.Text
                dr("SPV2") = tbSpv2Dt2.Text
                dr("SPVName2") = tbSpv2NameDt2.Text
                dr("Direksi") = tbDireksiDt2.Text
                dr("DireksiName") = tbDireksiNameDt2.Text
                dr("QcVerified") = tbQcDt2.Text
                dr("QcVerifiedName") = tbQcNameDt2.Text
                dr("FgPermanent") = ddlExtDateUpdateDt2.SelectedValue
                dr("Remark") = tbRemarkDtDt2.Text
                ViewState("Dt2").Rows.Add(dr)

                ' --- Kosongkan field di Dt jika Dt2 punya data untuk Item yang sama ---
                Dim itemNoParent As String = dr("ItemNo").ToString().Trim()

                ' Cari baris induk di Dt
                Dim drParent() As DataRow = ViewState("Dt").Select("ItemNo = " & QuotedStr(itemNoParent))
                If drParent.Length > 0 Then
                    Dim rowParent As DataRow = drParent(0)

                    ' Kosongkan / reset field di induk
                    rowParent("Percen") = 0
                    rowParent("TargetWaktu") = 0
                    rowParent("Biaya1") = 0
                    rowParent("Biaya2") = 0
                    rowParent("PIC") = ""
                    rowParent("PICName") = ""
                    rowParent("SPV1") = ""
                    rowParent("SPVName1") = ""
                    rowParent("SPV2") = ""
                    rowParent("SPVName2") = ""
                    rowParent("Direksi") = ""
                    rowParent("DireksiName") = ""
                    rowParent("QcVerified") = ""
                    rowParent("QcVerifiedName") = ""
                    rowParent("FgPermanent") = "N"
                    rowParent("Remark") = ""
                End If

                ' Rebind grid setelah perubahan
                BindGridDt(ViewState("Dt"), GridDt)
                BindGridDt(ViewState("Dt2"), GridDt2)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(lbItem.Text))
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

            'CountTotalDt()
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

    Private Sub Cleardt3()
        Try
            tbPaymentNoDt.Text = ""
            tbRemarkdt3.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                Row = ViewState("Dt3").Select("PaymentNo = " + QuotedStr(ViewState("Dt3Value")))(0)
                Row.BeginEdit()
                Row("PaymentNo") = tbPaymentNoDt.Text
                Row("Remark") = tbRemarkdt3.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("PaymentNo") = tbPaymentNoDt.Text
                dr("Remark") = tbRemarkdt3.Text


                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
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
                If Dr("PaymentNo").ToString = "" Then
                    lbStatus.Text = MessageDlg("Payment No Must Have Value")
                    Return False
                End If
            Else
                If tbPaymentNoDt.Text = "" Then
                    lbStatus.Text = MessageDlg("Payment No Must Have Value")
                    tbPaymentNoDt.Focus()
                    Return False
                End If


            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("PaymentNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
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

    'Private Sub CountTotalDt()
    '    Dim QtyTotal As Double
    '    Dim Dr As DataRow
    '    Dim drow As DataRow()
    '    Dim havedetail As Boolean
    '    Try
    '        drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
    '        QtyTotal = 0
    '        If drow.Length > 0 Then
    '            havedetail = False
    '            For Each Dr In drow.CopyToDataTable.Rows
    '                If Not Dr.RowState = DataRowState.Deleted Then
    '                    QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
    '                End If
    '            Next
    '        End If
    '        Dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))(0)

    '        Dr.BeginEdit()
    '        Dr("Qty") = QtyTotal 'FormatNumber(QtyTotal, ViewState("DigitQty"))
    '        Dr("AmountForex") = FormatNumber(QtyTotal * Dr("PriceForex"), ViewState("DigitCurr"))
    '        Dr("AmountHome") = FormatNumber(QtyTotal * Dr("PriceForex") * Dr("ForexRate"), ViewState("DigitHome"))
    '        'Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
    '        If CFloat(Dr("LifeMonth")) <> 0 Then
    '            Dr("AmountProcessDepr") = FormatNumber(QtyTotal * Dr("PriceForex") * Dr("ForexRate") * CFloat(Dr("LifeProcessDepr")) / CFloat(Dr("LifeMonth")), ViewState("DigitHome"))
    '        End If
    '        Dr.EndEdit()
    '        BindGridDt(ViewState("Dt"), GridDt)
    '        'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
    '    Catch ex As Exception
    '        Throw New Exception("Count Total Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

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

                tbCode.Text = GetAutoNmbr("TMP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PRCIPTemplateHd (TransNmbr, TransDate, Status, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'Y', " + _
               QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PRCIPTemplateHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCIPTemplateHd SET  Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate(), UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", TransDate =  " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,Tahapan,Percen,TargetWaktu,Biaya1,Biaya2,PIC,Spv1,Spv2,Direksi,QcVerified,FgPermanent,Remark FROM PRCIPTemplateDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PRCIPTemplateDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr,ItemNo,ItemNoDt2,Tahapan,Percen,TargetWaktu,Biaya1,Biaya2,PIC,Spv1,Spv2,Direksi,QcVerified,FgPermanent,Remark FROM PRCIPTemplateDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PRCIPTemplateDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, PaymentNo, Remark FROM PRCMTSertifikatDt3 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand



            Dim Dt3 As New DataTable("PRCMTSertifikatDt3")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub cekpercen()
        Try
            ' --- Hitung total persen dari GridDt ---
            Dim totalPercen As Decimal = 0

            For Each row As GridViewRow In GridDt.Rows
                ' Pastikan kolom ke-4 adalah Percen (atau ubah sesuai posisi sebenarnya)
                Dim percenValue As String = row.Cells(4).Text.Trim() ' index kolom "Percen"
                If IsNumeric(percenValue) Then
                    totalPercen += Convert.ToDecimal(percenValue)
                End If
            Next

            ' --- Validasi hasil ---
            If totalPercen <> 100 Then
                lbStatus.Text = "Total Persen harus sama dengan 100%. Saat ini total = " & totalPercen.ToString() & "%"
                Exit Sub
            End If


            lbStatus.Text = "Data berhasil disimpan."
        Catch ex As Exception
            lbStatus.Text = "Error saat menyimpan: " & ex.Message
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Dim cekQty As Boolean

        Try
            ' === Hitung total persen di GridView1 (header) ===
            Dim totalPercenGridView1 As Decimal = 0

            For Each row As GridViewRow In GridDt.Rows
                ' Pastikan kolom ke-4 adalah Persen (ubah sesuai urutan sebenarnya)
                Dim percenText As String = ""
                Try
                    percenText = row.Cells(4).Text.Trim()
                Catch
                    percenText = "0"
                End Try

                If IsNumeric(percenText) Then
                    totalPercenGridView1 += Convert.ToDecimal(percenText)
                End If
            Next

            ' === Hitung total persen di GridDt (detail) ===
            Dim totalPercenGridDt As Decimal = 0
            For Each row2 As GridViewRow In GridDt2.Rows
                ' Kolom ke-4 juga Percen (ubah kalau posisi beda)
                Dim percenText As String = row2.Cells(3).Text.Trim()
                If IsNumeric(percenText) Then
                    totalPercenGridDt += Convert.ToDecimal(percenText)
                End If
            Next

            'lbStatus.Text = totalPercenGridDt
            'Exit Sub

            ' === Total gabungan ===
            Dim totalGabungan As Decimal = totalPercenGridView1 + totalPercenGridDt

            ' === Validasi total gabungan harus 100 ===
            'If totalGabungan <> 100 Then
            '    lbStatus.Text = MessageDlg("Total persen gabungan antara GridView1 dan GridDt harus sama dengan 100%. Saat ini total = " & totalGabungan.ToString() & "%")

            '    Exit Sub
            'End If


            If totalGabungan > 100 Then
                lbStatus.Text = MessageDlg("Total % Template saat ini = " & totalGabungan.ToString() & "% melebihi 100%,harap diperiksa kembali")

                Exit Sub
            End If


            If totalGabungan < 100 Then
                lbStatus.Text = MessageDlg("Total % Template saat ini = " & totalGabungan.ToString() & "% Kurang dari 100%,harap diperiksa kembali")

                Exit Sub
            End If





            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Dokumen must have at least 1 record")
            '    Exit Sub
            'End If

            'cekQty = checkQty()
            'If cekQty = False Then
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Dim cekQty As Boolean

        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If

            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Dokumen must have at least 1 record")
            '    Exit Sub
            'End If

            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            'cekQty = checkQty()
            'If cekQty = False Then
            '    Exit Sub
            'End If

            SaveAll()
            newTrans()
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            btnAdd2.Visible = False
            ddlCommand.Visible = False
            ddlCommand2.Visible = False
            BtnGo.Visible = False
            btnGo2.Visible = False
           

            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click

        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            'EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            ' === Tambahkan logika nomor urut otomatis di sini ===
            Dim dt As DataTable = CType(ViewState("Dt2"), DataTable)
            Dim nextNumber As Integer = 1

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                nextNumber = dt.Rows.Count + 1
            End If

            lbItemNoDt2.Text = nextNumber.ToString()
            ' ====================================================

            'lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
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
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            EnableHd(True)
            btnAdd2.Visible = False
            ddlCommand.Visible = False
            ddlCommand2.Visible = False
            BtnGo.Visible = False
            btnGo2.Visible = False
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Status, Area, Remark"
            FilterValue = "TransNmbr, Status, Area, Remark"
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
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True


                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "Y" Then
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Active to edit")
                        Exit Sub
                    End If

                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("SelectCommand") = "EXEC S_FNFormMSertifikat '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormMSertifikat.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try

                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        Dim SQLString As String
                        SQLString = "UPDATE PRCIPTemplateHd SET  Status = 'N' WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text)
                        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                        BindData("TransNmbr in ('" + GVR.Cells(2).Text + "')")
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

                lbItem.Text = GVR.Cells(2).Text
                lbTahapan.Text = GVR.Cells(3).Text

                MultiView1.ActiveViewIndex = 1


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
                drow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(TrimStr(lbItem.Text)))
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
            btnBackDt2ke2.Visible = False
            btnAddDt2Ke2.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), drCek() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            ' Cek apakah GridDt2 punya anak dari item ini
            drCek = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))

            If drCek.Length > 0 Then
                '=== Delete semua Detail 2 yang terkait ===
                dr = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
                For Each d As DataRow In dr
                    d.Delete()
                Next
                ViewState("Dt2").AcceptChanges()
                BindGridDt(ViewState("Dt2"), GridDt2)
            End If

            '=== Delete Detail 1 ===
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
            For Each d As DataRow In dr
                d.Delete()
            Next
            ViewState("Dt").AcceptChanges()
            BindGridDt(ViewState("Dt"), GridDt)

            '=== Update Enable Header ===
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(lbItem.Text + "|" + GVR.Cells(1).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(TrimStr(lbItem.Text)))
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

        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDt(GVR.Cells(2).Text)

            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)


            'If ViewState("InputCurrency") = "Y" Then
            'ddlCurrDt.DataBind()
            'ViewState("InputCurrency") = Nothing
            'End If
            'ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRateDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbItem.Text + "|" + GVR.Cells(1).Text)
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


    Protected Sub btnPIC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPIC.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnPIC"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnspv1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnspv1.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnSpv1"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSpv2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSpv2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnSpv2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDireksi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDireksi.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnDireksi"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnQc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQc.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnQc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnPICDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPICDt2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnPICDt2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnspv1Dt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnspv1Dt2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnSpv1Dt2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSpv2Dt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSpv2Dt2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnSpv2Dt2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDireksiDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDireksiDt2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnDireksiDt2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnQcDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQcDt2.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnQcDt2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

End Class
