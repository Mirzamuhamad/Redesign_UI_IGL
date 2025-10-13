Imports System.Data
Imports System.Data.SqlClient

Partial Class TrMutasiPBB
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCMutasiPBBHd"

    'Private Function GetStringHd(ByVal Type As String) As String
    '    Return "SELECT * From V_PRCMutasiPBBHd "
    'End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCMutasiPBBDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNo ASC"
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCMutasiPBBDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCMutasiPBBDt3 WHERE TransNmbr = " + QuotedStr(Nmbr)
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


                If ViewState("Sender") = "btnUnit" Then
                    tbUnit.Text = Session("Result")(0).ToString
                    tbUnitName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnObject" Then
                    tbObject.Text = Session("Result")(0).ToString
                    tbObjectName.Text = Session("Result")(1).ToString
                End If


                If ViewState("Sender") = "btnArea" Then
                    tbArea.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnReference" Then
                    tbReference.Text = Session("Result")(0).ToString
                    tbNoSertifikat.Text = Session("Result")(3).ToString
                    ddlJenisDokumen.SelectedValue = Session("Result")(4).ToString
                    tbLuas.Text = Session("Result")(6).ToString
                    tbRemarkDt.Text = Session("Result")(7).ToString
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


                 If ViewState("Sender") = "btnGetreference" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    ''Insert Detail
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt").Select("Reference+'|'+ItemNo = " + QuotedStr(drResult("Reference").ToString + "|" + drResult("ItemNo").ToString))

                        ' lbstatus.text = ExistRow.Count
                        ' Exit Sub
                        If ExistRow.Count = 0 Then
                            'insert
                           
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                             Dim dt As DataTable = CType(ViewState("Dt"), DataTable)
                          
                            'If dt.Rows.Count = 0 Then
                            '    dr("ItemNo") = 1
                            'Else
                            '    dr("ItemNo") = dt.AsEnumerable().Max(Function(r) CInt(r("ItemNo"))) + 1
                            'End If
                            ' dr("BAP_No") = drResult("ItemNo").ToString
                            dr("ItemNo") = drResult("ItemNo").ToString
                            dr("Reference") = drResult("Reference").ToString
                            dr("JenisMutasi") = drResult("JenisMutasi").ToString
                            dr("JenisDokumen") = drResult("JenisDokumen").ToString
                            dr("Info") = drResult("Info").ToString.Replace("Sumber","Hasil")
                            dr("Luas") = drResult("Luas").ToString.Replace("&nbsp;", "")
                            dr("UnitCode") = drResult("UnitCode").ToString.Replace("&nbsp;", "")
                            dr("UnitName") = drResult("UnitName").ToString.Replace("&nbsp;", "")
                            dr("Nama") = drResult("Nama").ToString.Replace("&nbsp;", "")
                            dr("NoSertifikat") = drResult("NoDokumen").ToString.Replace("&nbsp;", "")
                            dr("Remark") = drResult("Remark").ToString.Replace("&nbsp;", "")
                            dr("SisaLuas") = 0
                            ViewState("Dt").Rows.Add(dr)
                        End If


                        ' 'Insert Sub Detail
                        ' lblBAPNumber.Text = drResult("BAP_No").ToString
                        ' lblSPKNumber.Text = drResult("No_SPK").ToString

                        ' 'MultiView1.ActiveViewIndex = 1

                        ' Dim drDtResult As DataRow
                        ' Dim ExistRowDT As DataRow()
                        ' Dim MaxItem As String
                        ' Dim DtPekerjaan As DataTable
                        ' Dim SQLString As String

                        ' Dim Item As String

                        

                        ' SQLString = "SELECT * FROM V_GetMutasiDokumenDetail WHERE Reference = " + QuotedStr(drResult("Reference").ToString) + " AND ItemNo = " + QuotedStr(drResult("ItemNo").ToString)
                        ' DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        ' For Each drDtResult In DtPekerjaan.Rows
                        '     ExistRowDT = ViewState("Dt2").Select("ItemDt2+'|'+NoDokDt2 = " + QuotedStr(drDtResult("ItemNo").ToString + "|" + drDtResult("NoSPpt").ToString))
                        '     If ExistRowDT.Count = 0 Then
                        '         Dim Dtdr As DataRow
                        '         Dtdr = ViewState("Dt2").NewRow
                        '         Dtdr("ItemDt2") = drDtResult("ItemNo")
                        '         Dtdr("NoDokDt2") = drDtResult("NoSPpt")
                        '         Dtdr("JenisDokDt2") = drDtResult("JenisDokumen")
                        '         Dtdr("Luas") = drDtResult("Luas")
                        '         Dtdr("Unit") = drDtResult("UnitCode")
                        '         Dtdr("UnitName") = drDtResult("UnitName")
                        '          Dtdr("Nama") = drDtResult("Nama")
                        '         Dtdr("Remark") = drDtResult("Remark")
                        '         ViewState("Dt2").Rows.Add(Dtdr)
                        '     End If
                        ' Next
                    Next
                     BindGridDt(ViewState("Dt"), GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    StatusButtonSave(True)
                    ' CountTotalDt2()
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

     Protected Sub btnGetDetail()

        Dim drow() As DataRow

        Try
            Dim Dr2 As DataRow
            Dim MaxItem As String

            Dim SQLString As String = "SELECT * FROM V_GetMutasiDokumenDetail WHERE Reference = " + QuotedStr(tbReference.Text) + " AND ItemNo = " + QuotedStr(lbitemNo.text)
            Dim DtPekerjaan As DataTable = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)


            'Insert Angsuran
            For Each drDtResult As DataRow In DtPekerjaan.Rows
                Dim ExistRowDT As DataRow() = ViewState("Dt2").Select("NoDokDt2+'|'+ItemDt2 = " + QuotedStr(drDtResult("NoSppt").ToString + "|" + drDtResult("ItemNo").ToString ))
                If ExistRowDT.Count = 0 Then
                    Dim Dtdr As DataRow
                    Dtdr = ViewState("Dt2").NewRow
                    Dtdr("Itemdt2") = lbItemNo.text
                    Dtdr("NoDokumen") = tbNoDok.text
                    Dtdr("NoDokDt2") = drDtResult("NoSppt")
                    Dtdr("JenisDokDt2") = drDtResult("JenisDokumen")
                    Dtdr("Luas") = drDtResult("Luas")
                    Dtdr("Unit") = drDtResult("UnitCode")
                    Dtdr("UnitName") = drDtResult("UnitName")
                    Dtdr("Remark") = drDtResult("Remark")
                    Dtdr("Nama") = drDtResult("Nama")
                    ViewState("Dt2").Rows.Add(Dtdr)
                Else
                    'Update jika sudah ada
                    Dim Row As DataRow
                    Row = ViewState("Dt2").Select("NoDokDt2+'|'+ItemDt2= " + QuotedStr(drDtResult("NoSppt".ToString) + "|" + drDtResult("ItemNo").ToString))(0)
                    Row.BeginEdit()
                    Row("Itemdt2") = lbItemNo.text
                    Row("NoDokumen") = tbNoDok.text
                    Row("NoDokDt2") = drDtResult("NoSppt")
                    Row("JenisDokDt2") = drDtResult("JenisDokumen")
                    Row("Luas") = drDtResult("Luas")
                    Row("Unit") = drDtResult("UnitCode")
                    Row("UnitName") = drDtResult("UnitName")
                    Row("Remark") = drDtResult("Remark")
                    Row("Nama") = drDtResult("Nama")
                    Row.EndEdit()
                End If
                
            Next
                ' MovePanel(pnlEditDt2, pnlDt2)
           
        Catch ex As Exception
            lbStatus.Text = "btnangsuran Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "SELECT * FROM V_GetPaymentPosting "
            ResultField = "PaymentNo, PaymentDate, Status, TotalPayment "
            CriteriaField = "PaymentNo, PaymentDate, Status, TotalPayment "
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


            tbLuas.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaLuas.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

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
                    Result = ExecSPCommandGo(ActionValue, "S_PRCMutasiPBB", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbArea.Text = ""
            tbAreaName.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbNoDok.Text = ""
            tbLuas.Text = 0
            tbSisaLuas.Text = 0
            tbUnitName.Text = ""
            tbInfo.Text = ""
            tbRemark.Text = ""
            tbObject.Text = ""
            tbObjectName.Text = ""
            tbReference.Text = ""
            tbNoSertifikat.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbDokumenDt.Text = ""
            tbUnit.Text = ""
            tbLuasDt2.Text = 0
            tbUnitName.Text = ""
            tbRemarkDt2.Text = ""
            tbPaymentNo.Text = ""
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

            If tbArea.Text = "" Then
                lbStatus.Text = MessageDlg("Area must have value")
                tbArea.Focus()
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
                If Dr("NoDokumen").ToString = "" Then
                    lbStatus.Text = MessageDlg("Dokumen Must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If

                If Dr("Luas").ToString = "" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If

                If Dr("SisaLuas").ToString = "" Then
                    lbStatus.Text = MessageDlg("Sisa Luas Must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If

            Else

                If tbNoDok.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Dokumen must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If

                If tbLuas.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Must have value !")
                    tbLuas.Focus()
                    Return False
                End If


                If tbSisaLuas.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa Luas Must Have Value")
                    tbSisaLuas.Focus()
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
                If Dr("NoDokDt2").ToString = "" Then
                    lbStatus.Text = MessageDlg("FA Location Must Have Value")
                    Return False
                End If
                If Dr("Luas").ToString = "0" Or Dr("Luas").ToString = "" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    Return False
                End If

            Else
                If tbLuasDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    tbLuasDt2.Focus()
                    Return False
                End If

                If tbDokumenDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Dokumen Must Have Value")
                    tbLuasDt2.Focus()
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
            BindToText(tbArea, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("AreaName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal NoDokumen As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(NoDokumen))
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbNoDok, Dr(0)("NoDokumen").ToString)
                BindToDropList(ddlJenisDokumen, Dr(0)("JenisDokumen").ToString)
                BindToDropList(ddlJenisMutasi, Dr(0)("JenisMutasi").ToString)
                BindToText(tbLuas, Dr(0)("Luas").ToString)
                BindToText(tbSisaLuas, Dr(0)("SisaLuas").ToString)
                BindToText(tbInfo, Dr(0)("Info").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbObject, Dr(0)("UnitCode").ToString)
                BindToText(tbNamaAwal, Dr(0)("Nama").ToString)
                BindToText(tbPaymentNo, Dr(0)("NoPayment").ToString)
                BindToText(tbObjectName, Dr(0)("UnitName").ToString)
                BindToText(tbReference, Dr(0)("Reference").ToString)
                BindToText(tbNoSertifikat, Dr(0)("NoSertifikat").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub FillTextBoxDt2(ByVal Dok As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("NoDokumen+'|'+NoDokDt2 = " + QuotedStr(Dok))
            If Dr.Length > 0 Then
                BindToDropList(ddlJenisDokDt2, Dr(0)("JenisDokDt2").ToString)
                BindToText(tbDokumenDt, Dr(0)("NoDokDt2").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbUnitName, Dr(0)("UnitName").ToString)
                BindToText(tbLuasDt2, Dr(0)("Luas").ToString)
                BindToText(tbNamaAkhir, Dr(0)("Nama").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim tbTotal As Double
        Try
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
                Row("ItemNo") = lbItemNo.Text
                Row("JenisMutasi") = ddlJenisMutasi.SelectedValue
                Row("JenisDokumen") = ddlJenisDokumen.SelectedValue
                Row("NoDokumen") = tbNoDok.Text
                Row("Luas") = tbLuas.Text
                Row("SisaLuas") = tbSisaLuas.Text
                Row("info") = tbInfo.Text
                Row("UnitCode") = tbObject.Text
                Row("UnitName") = tbObjectName.Text
                Row("Nama") = tbNamaAkhir.Text
                Row("NoPayment") = tbPaymentNo.Text
                Row("Remark") = tbRemark.Text
                Row("NoSertifikat") = tbNoSertifikat.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("JenisMutasi") = ddlJenisMutasi.SelectedValue
                dr("JenisDokumen") = ddlJenisDokumen.SelectedValue
                dr("NoDokumen") = tbNoDok.Text
                dr("Luas") = tbLuas.Text
                dr("SisaLuas") = tbSisaLuas.Text
                dr("info") = tbInfo.Text
                dr("UnitCode") = tbObject.Text
                dr("UnitName") = tbObjectName.Text
                dr("Nama") = tbNamaAkhir.Text
                dr("NoPayment") = tbPaymentNo.Text
                dr("Remark") = tbRemark.Text
                dr("NoSertifikat") = tbNoSertifikat.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            btnGetDetail()
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
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("NoDokumen+'|'+NoDokDt2 = " + QuotedStr(lbNoDokumen.Text + "|" + tbDokumenDt.Text))(0)
                Row.BeginEdit()
                Row("NoDokumen") = lbNoDokumen.Text
                Row("NoDokdt2") = tbDokumenDt.Text
                Row("JenisDokDt2") = ddlJenisDokDt2.SelectedValue
                Row("Luas") = tbLuasDt2.Text
                Row("Unit") = tbUnit.Text
                Row("UnitName") = tbUnitName.Text
                Row("Nama") = tbNamaAwal.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("NoDokumen") = lbNoDokumen.Text
                dr("NoDokdt2") = tbDokumenDt.Text
                dr("JenisDokDt2") = ddlJenisDokDt2.SelectedValue
                dr("Luas") = tbLuasDt2.Text
                dr("Unit") = tbUnit.Text
                dr("UnitName") = tbUnitName.Text
                dr("Nama") = tbNamaAwal.Text
                dr("Remark") = tbRemarkDt2.Text

                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("NoDokumen = " + QuotedStr(TrimStr(lbNoDokumen.Text)))
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

                tbCode.Text = GetAutoNmbr("PBB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO PRCMutasiPBBHd (TransNmbr, TransDate, Status, " + _
                "AreaCode, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbArea.Text) + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PRCMutasiPBBHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCMutasiPBBHd SET AreaCode =" + QuotedStr(tbArea.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate() WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,JenisMutasi,JenisDokumen,NoDokumen,Luas,SisaLuas,Info,UnitCode,Nama, NoPayment, Remark, Reference, NoSertifikat FROM PRCMutasiPBBDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PRCMutasiPBBDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr,NoDokumen,NoDokDt2,JenisDokDt2,Unit,Luas,Nama,Remark,ItemDt2 FROM PRCMutasiPBBDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PRCMutasiPBBDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, PaymentNo, Remark FROM PRCMutasiPBBDt3 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param3 As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command3 = New SqlCommand( _
            '        "UPDATE PLPlanLandJob SET JobPlant = @JobPlant, Team = @Team, @Qty = @Qty, StartDate = @StartDate, EndDate = @EndDate, Unit = @Unit, WorkDay = @WorkDay, " + _
            '        "Capacity = @Capacity, Person = @Person " + _
            '        "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @OldJobPlant ", con)

            '' Define output parameters.
            'Update_Command3.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
            'Update_Command3.Parameters.Add("@Team", SqlDbType.VarChar, 5, "Team")
            'Update_Command3.Parameters.Add("@StartDate", SqlDbType.DateTime, 8, "StartDate")
            'Update_Command3.Parameters.Add("@EndDate", SqlDbType.DateTime, 8, "EndDate")
            'Update_Command3.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            'Update_Command3.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            'Update_Command3.Parameters.Add("@WorkDay", SqlDbType.Float, 18, "WorkDay")
            'Update_Command3.Parameters.Add("@Capacity", SqlDbType.Float, 18, "Capacity")
            'Update_Command3.Parameters.Add("@Person", SqlDbType.Float, 18, "Person")

            '' Define intput (WHERE) parameters.
            'param3 = Update_Command3.Parameters.Add("@OldJobPlant", SqlDbType.VarChar, 5, "JobPlant")
            'param3.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command3

            '' Create the DeleteCommand.
            'Dim Delete_Command3 = New SqlCommand( _
            '    "DELETE FROM PLPlanLandJob WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @JobPlant", con)
            '' Add the parameters for the DeleteCommand.
            'param3 = Delete_Command3.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
            'param3.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command3

            Dim Dt3 As New DataTable("PLPlanLandJob")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Dim cekQty As Boolean

        Try

            'lbStatus.Text = GetCountRecord(ViewState("Dt2"))
            'Exit Sub
            If CekHd() = False Then
                Exit Sub
            End If

            If CekDt() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If

            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Dokumen must have at least 1 record")
                Exit Sub
            End If
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
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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

    'Private Function checkQty() As Boolean
    '    Dim drow() As DataRow
    '    Dim dt As New DataTable
    '    Dim GVR As GridViewRow
    '    Dim baris As Integer
    '    Dim i As Integer
    '    Dim fa As String
    '    Dim qty, QtyTotal As Integer
    '    Dim havedetail As Boolean

    '    Try
    '        If GetCountRecord(ViewState("Dt")) <> 0 Then
    '            dt = ViewState("Dt")
    '            baris = dt.Rows.Count
    '            For i = 0 To GetCountRecord(ViewState("Dt")) - 1 'For i = 0 To baris - 1
    '                GVR = GridDt.Rows(i)
    '                QtyTotal = 0

    '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(fa))

    '                If drow.Length > 0 Then
    '                    For Each Dr In drow.CopyToDataTable.Rows
    '                        If Not Dr.RowState = DataRowState.Deleted Then
    '                            QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
    '                        End If
    '                    Next
    '                End If

    '                If qty <> QtyTotal Then
    '                    lbStatus.Text = MessageDlg(GVR.Cells(3).Text + " Qty must be equall with Qty Total in Detail 2")
    '                    Return False
    '                End If
    '            Next
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        lbStatus.Text = "checkQty error : " + ex.ToString
    '    End Try
    'End Function

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True

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
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            ddlJenisMutasi_SelectedIndexChanged(Nothing, Nothing)
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
            JenisDokumen()
            tbDokumenDt.Enabled = True
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

                        Session("SelectCommand") = "EXEC S_FNFormMSertifikat '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormMSertifikat.frx"
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try

            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If

            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                'lbStatus.Text = GVR.Cells(6).Text

                

                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If

                

                Dim lbFA As Label

                lbFA = GVR.FindControl("lbFa")

                lbNoDokumen.Text = GVR.Cells(6).Text
                lbMutasi.Text = GVR.Cells(4).Text
                lbLuas.Text = GVR.Cells(8).Text
                lblJenisDokumen.Text = GVR.Cells(5).Text
                lbReference.Text = GVR.Cells(3).Text

                If lbNoDokumen.Text = "" Then
                    lbStatus.Text = MessageDlg("No Dokumen Must Have Value")
                    Exit Sub
                End If

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
                drow = ViewState("Dt2").Select("Itemdt2 = " + QuotedStr(TrimStr(GVR.Cells(2).Text)))
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

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), drCek() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt.Rows(e.RowIndex)

            'lbStatus.Text = GetCountRecord(ViewState("Dt2"))
            'Exit Sub
            drCek = ViewState("Dt2").Select("ItemDt2 = " + QuotedStr(GVR.Cells(2).Text))
            If drCek.Length > 0 Then
                'Delete Detail 2------------------------------------
                'dr = ViewState("Dt2").select("FALocationType+'|'+FALocationCode = " + QuotedStr(ddlFALocType.SelectedValue + "|" + tbFALocCode.Text))
                dr = ViewState("Dt2").Select("ItemDt2 = " + QuotedStr(GVR.Cells(2).Text))
                dr(0).Delete()

                'Delete Detail 1------------------------------------
                dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
                dr(0).Delete()
                BindGridDt(ViewState("Dt"), GridDt)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)

            Else

                'Delete Detail 1------------------------------------
                dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
                dr(0).Delete()
                BindGridDt(ViewState("Dt"), GridDt)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("NoDokumen+'|'+NoDokDt2 = " + QuotedStr(lbNoDokumen.Text + "|" + GVR.Cells(5).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("NoDokumen = " + QuotedStr(TrimStr(lbNoDokumen.Text)))
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
            tbDokumenDt.Enabled = False

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
            FillTextBoxDt2(lbNoDokumen.Text + "|" + GVR.Cells(1).Text)
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

    Protected Sub btnUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnit.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try

            sqlstring = "EXEC S_GetStructure " + QuotedStr(tbArea.Text)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "StructureCode, StructureName, Account, AccountNAme, ID"
            CriteriaField = "StructureCode, StructureName, Account, AccountNAme,ID"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnUnit"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnObject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnObject.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try

            sqlstring = "EXEC S_GetStructure " + QuotedStr(tbArea.Text)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "StructureCode, StructureName, Account, AccountNAme, ID"
            CriteriaField = "StructureCode, StructureName, Account, AccountNAme,ID"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnObject"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArea.Click
        Dim ResultField As String
        Try
            Session("filter") = "select ID, StructureName from V_MsStructure WHERE LevelCode = '01' "
            ResultField = "ID, StructureName"
            ViewState("Sender") = "btnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPaymentNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentNo.Click
        Dim ResultField As String
        Try
            Session("filter") = " SELECT * FROM V_GetPaymentPosting "
            ResultField = "PaymentNo, PaymentDate, Status, TotalPayment"
            ViewState("Sender") = "BtnPayment"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlJenisMutasi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJenisMutasi.SelectedIndexChanged

        Try

            If ddlJenisMutasi.SelectedValue = "Pemecahan" Then
                tbInfo.Text = "Hasil nomor dokumen pemecahan"
            ElseIf ddlJenisMutasi.SelectedValue = "Penggabungan" Then
                tbInfo.Text = "Hasil nomor dokumen penggabungan"
            ElseIf ddlJenisMutasi.SelectedValue = "Peningkatan" Then
                tbInfo.Text = "Hasil nomor dokumen peningkatan"
            ElseIf ddlJenisMutasi.SelectedValue = "Penurunan" Then
                tbInfo.Text = "Hasil nomor dokumen penurunan"
            ElseIf ddlJenisMutasi.SelectedValue = "Balik Nama" Then
                tbInfo.Text = "Hasil nomor dokumen Balik Nama"
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Private Sub JenisDokumen()
        If lbMutasi.Text = "Penggabungan" Or lbMutasi.Text = "Pemecahan" Then
            ddlJenisDokDt2.SelectedValue = lblJenisDokumen.Text
        End If
    End Sub


     Protected Sub btnReference_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReference.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT Reference,TransDate,Reference_Dok,NoDokumen,JenisMutasi,JenisDokumen,Luas,RemarkFROM V_GetMutasiDokumen" 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
           ' Session("filter") = "EXEC S_GetReferenceALL"
            ResultField = "Reference,TransDate,Reference_Dok,NoDokumen,JenisMutasi,JenisDokumen,Luas,Remark"
            CriteriaField = "Reference,TransDate,Reference_Dok,NoDokumen,JenisMutasi,JenisDokumen,Luas,Remark"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnReference"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchDlg();", Page, Me.GetType())
            AttachScript("OpenPopupRef();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "BtnRecvDoc Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetreference_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetreference.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            sqlstring = "SELECT * FROM V_GetMutasiDokumen"
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "Reference,TransDate,Reference_Dok,NoDokumen,JenisMutasi,JenisDokumen,Luas,Remark, UnitCode, UnitName,Nama, Info,ItemNo"
            CriteriaField = "Reference,TransDate,Reference_Dok,NoDokumen,JenisMutasi,JenisDokumen,Luas,Remark, UnitCode, UnitName,Nama, Info,ItemNo"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Reference"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetreference"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("FindMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub



End Class
