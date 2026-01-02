Imports System.Data
Imports BasicFrame.WebControls
Imports System.Data.SqlClient

Partial Class Transaction_TrPOImportStatusV2_TrPOImportStatusV2
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PRPOImportStatusDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If ViewState("DigitCurr") Is Nothing Then
                ViewState("DigitCurr") = 0
            End If
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If

            lbStatus.Text = ""

            'Hasil dari Advance Filter
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            'Hasil dari Search Dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If

            GridDt.Columns(0).Visible = False
        Catch ex As Exception
            lbStatus.Text = "Form Load Error :" + ex.ToString
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
            ViewState("SortExpression") = Nothing
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbTotalContainer.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalContainer.Attributes.Add("OnBlur", "setformat();")
                 
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction("Select TransNmbr, Nmbr, Revisi, TransDate, Supplier, Supplier_Name From V_PRPOImportStatus", StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False                
                'BtnGo.Visible = False
            End If
            'ddlCommand.Visible = DT.Rows.Count > 0
            'BtnGo.Visible = DT.Rows.Count > 0
            'ddlCommand2.Visible = ddlCommand.Visible
            'btnGo2.Visible = BtnGo.Visible
            'btnAdd2.Visible = BtnGo.Visible
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

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection")).Tables(0)
            ViewState("Dt") = dt
            GridDt.DataSource = dt
            GridDt.DataBind()
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("BindDataDt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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

    'Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
    '    Try
    '        ModifyInput(True, pnlInput)
    '        newTrans()
    '        ViewState("DigitCurr") = 0
    '        MovePanel(PnlHd, pnlInput)
    '        ChangeReport("Add", ddlReport.SelectedValue, True, tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    '        ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        btnHome.Visible = False
    '        tbInvoiceNo.Enabled = True
    '        tbInvoiceNo.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "Btn Add Error : " + ex.ToString
    '    End Try
    'End Sub

    'Private Sub EnableHd(ByVal State As Boolean)
    '    'Dim Count As Integer
    '    Try
    '        'tbDRDateWrhs.Enabled = State
    '        'tbDRDateETA.Enabled = State
    '        'tbDRDateETD.Enabled = State
    '        'tbShipDateETD.Enabled = State
    '        'tbShipDateETA.Enabled = State
    '        'tbShipDateCustom.Enabled = State
    '        'tbDeliveryInLand.Enabled = State
    '        'tbInvoiceNomor.Enabled = State
    '        'tbBLNo.Enabled = State
    '        'tbContainerNo.Enabled = State
    '        'tbTotalContainer.Enabled = State
    '        'tbAJUNo.Enabled = State
    '        'tbRespon.Enabled = State
    '    Catch ex As Exception
    '        Throw New Exception("Enable Hd Error " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub EnableDt(ByVal State As Boolean)
        Try
            tbDRDateWrhs.Enabled = State
            tbDRDateETA.Enabled = State
            tbDRDateETD.Enabled = State
            tbShipDateETD.Enabled = State
            tbShipDateETA.Enabled = State
            tbShipDateCustom.Enabled = State
            tbDeliveryInLand.Enabled = State
            tbInvoiceNomor.Enabled = State
            tbBLNo.Enabled = State
            tbContainerNo.Enabled = State
            tbTotalContainer.Enabled = State
            tbAJUNo.Enabled = State
            tbRespon.Enabled = State
        Catch ex As Exception
            Throw New Exception("EnableDt Error " + ex.ToString)
        End Try
    End Sub

    'Private Sub newTrans()
    '    Try
    '        ViewState("StateHd") = "Insert"
    '        tbDRDateWrhs.Clear()
    '        tbDRDateETA.Clear()
    '        tbDRDateETD.Clear()
    '        tbShipDateETD.Clear()
    '        tbShipDateETA.Clear()
    '        tbShipDateCustom.Clear()

    '        tbDeliveryInLand.Text = ""
    '        tbInvoiceNomor.Text = ""
    '        tbBLNo.Text = ""
    '        tbContainerNo.Text = ""
    '        tbTotalContainer.Text = "0"
    '        tbAJUNo.Text = ""
    '        tbRespon.Text = ""
    '    Catch ex As Exception
    '        Throw New Exception("new Record Error " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub clearDt()
        Try
            'ViewState("StateHd") = "Insert"
            tbDRDateWrhs.Clear()
            tbDRDateETA.Clear()
            tbDRDateETD.Clear()
            tbShipDateETD.Clear()
            tbShipDateETA.Clear()
            tbShipDateCustom.Clear()

            tbDeliveryInLand.Text = ""
            tbInvoiceNomor.Text = ""
            tbBLNo.Text = ""
            tbContainerNo.Text = ""
            tbTotalContainer.Text = "0"
            tbAJUNo.Text = ""
            tbRespon.Text = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    'Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
    '    Dim Status As String
    '    Dim Result, ListSelectNmbr, ActionValue As String
    '    Dim Nmbr(100) As String
    '    Dim j As Integer
    '    Try
    '        If sender.ID.ToString = "BtnGo" Then
    '            ActionValue = ddlCommand.SelectedValue
    '        Else
    '            ActionValue = ddlCommand2.SelectedValue
    '        End If
    '        Status = CekStatus(ActionValue)

    '        ListSelectNmbr = ""
    '        GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
    '        If ListSelectNmbr = "" Then Exit Sub
    '        For j = 0 To (Nmbr.Length - 1)
    '            If Nmbr(j) = "" Then
    '                Exit For
    '            Else
    '                Result = ExecSPCommandGo(ActionValue, "S_FNSIBegin", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
    '                If Trim(Result) <> "" Then
    '                    lbStatus.Text = lbStatus.Text + Result + " <br/>"

    '                End If
    '            End If
    '        Next
    '        BindData("InvoiceNo in (" + ListSelectNmbr + ")")
    '    Catch ex As Exception
    '        lbStatus.Text = "Go Command Error : " + ex.ToString
    '    End Try
    'End Sub

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
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(GVR)
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(0).Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    MovePanel(PnlHd, pnlInput)
                    'ModifyInput(False, pnlInput)
                    'EnableHd(False)
                ElseIf DDL.SelectedValue = "Edit" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    MovePanel(PnlHd, pnlInput)
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(GVR)
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(0).Visible = False
                    ViewState("StateHd") = "Edit"
                    btnHome.Visible = False
                    ModifyInput2(True, pnlInput, pnlDt, GridDt)
                    ViewState("InvoiceNo") = GVR.Cells(2).Text
                    'ModifyInput(True, pnlInput)
                    'ViewState("StateHd") = "Edit"
                    'EnableHd(True)
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Print
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Row Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillTextBoxHd(ByVal e As GridViewRow)
        Dim Dt As DataTable
        Dim Nmbr As String

        Try
            Nmbr = e.Cells(2).Text
            Dt = BindDataTransaction("Select * From V_PRPOImportStatus", "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbInvoiceNo.Text = Nmbr
            BindToText(tbRevisi, Dt.Rows(0)("Revisi").ToString)
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)

            'BindToDate(tbDRDateWrhs, Dt.Rows(0)("DRDateWrhs").ToString)
            'BindToDate(tbDRDateETA, Dt.Rows(0)("DRDateETA").ToString)
            'BindToDate(tbDRDateETD, Dt.Rows(0)("DRDateETD").ToString)
            'BindToDate(tbShipDateETD, Dt.Rows(0)("ShipDateETD").ToString)
            'BindToDate(tbShipDateETA, Dt.Rows(0)("ShipDateETA").ToString)
            'BindToDate(tbShipDateCustom, Dt.Rows(0)("ShipDateCustom").ToString)
            'BindToText(tbDeliveryInLand, Dt.Rows(0)("DeliveryInLand").ToString)
            'BindToText(tbInvoiceNomor, Dt.Rows(0)("InvoiceNo").ToString)
            'BindToText(tbBLNo, Dt.Rows(0)("BLNo").ToString)
            'BindToText(tbContainerNo, Dt.Rows(0)("ContainerNo").ToString)
            'If Dt.Rows(0)("TotalContainer").ToString = "" Then
            '    tbTotalContainer.Text = "0"
            'Else
            '    BindToText(tbTotalContainer, Dt.Rows(0)("TotalContainer").ToString)
            'End If

            'BindToText(tbAJUNo, Dt.Rows(0)("AJUNo").ToString)
            'BindToText(tbRespon, Dt.Rows(0)("Respon").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Private Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()

        Try
            Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                'BindToText(tbQuotationNo, Dr(0)("QuotationNo").ToString)
                'BindToText(tbProductCode, Dr(0)("Product").ToString)
                'BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                'BindToText(tbBuildCode, Dr(0)("Building").ToString)
                'BindToText(TbBuildName, Dr(0)("Building_Name").ToString)
                'BindToDate(tbInspectedInstall, Dr(0)("InspectedInstall").ToString)
                'BindToText(tbQtyInstall, Dr(0)("QtyInstall").ToString)
                'BindToText(tbQty, Dr(0)("Qty").ToString)
                'BindToText(tbQtyTotal, Dr(0)("QtyTotal").ToString)
                'BindToText(tbQtyFree, Dr(0)("QtyFree").ToString)
                'BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                'BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                'BindToText(tbOldContractNo, Dr(0)("OldContractNo").ToString)
                'BindToDropList(ddlBranchService, Dr(0)("BranchService").ToString)

                lbItemNo.Text = Dr(0)("ItemNo").ToString
                If Dr(0)("DRDateWrhs").ToString = "" Then
                    tbDRDateWrhs.Clear()
                Else
                    BindToDate(tbDRDateWrhs, Dr(0)("DRDateWrhs").ToString)
                End If

                If Dr(0)("DRDateETA").ToString = "" Then
                    tbDRDateETA.Clear()
                Else
                    BindToDate(tbDRDateETA, Dr(0)("DRDateETA").ToString)
                End If

                If Dr(0)("DRDateETD").ToString = "" Then
                    tbDRDateETD.Clear()
                Else
                    BindToDate(tbDRDateETD, Dr(0)("DRDateETD").ToString)
                End If

                If Dr(0)("ShipDateETA").ToString = "" Then
                    tbShipDateETA.Clear()
                Else
                    BindToDate(tbShipDateETA, Dr(0)("ShipDateETA").ToString)
                End If

                If Dr(0)("ShipDateETD").ToString = "" Then
                    tbShipDateETD.Clear()
                Else
                    BindToDate(tbShipDateETD, Dr(0)("ShipDateETD").ToString)
                End If

                If Dr(0)("ShipDateCustom").ToString = "" Then
                    tbShipDateCustom.Clear()
                Else
                    BindToDate(tbShipDateCustom, Dr(0)("ShipDateCustom").ToString)
                End If

                BindToText(tbDeliveryInLand, Dr(0)("DeliveryInLand").ToString)
                BindToText(tbInvoiceNomor, Dr(0)("InvoiceNo").ToString)
                BindToText(tbBLNo, Dr(0)("BLNo").ToString)
                BindToText(tbContainerNo, Dr(0)("ContainerNo").ToString)
                If Dr(0)("TotalContainer").ToString = "" Then
                    tbTotalContainer.Text = "0"
                Else
                    BindToText(tbTotalContainer, Dr(0)("TotalContainer").ToString)
                End If

                BindToText(tbAJUNo, Dr(0)("AJUNo").ToString)
                BindToText(tbRespon, Dr(0)("Respon").ToString)
            End If
        Catch ex As Exception
            lbStatus.Text = "FillTextBoxDt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnSaveNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
    '    Try
    '        If cekhd() = False Then
    '            Exit Sub
    '        End If
    '        SaveData()
    '        'BtnAdd_Click(Nothing, Nothing)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Save New Error : " + ex.ToString
    '    End Try
    'End Sub

    Function cekHd() As Boolean
        Dim Infois As String
        Try
            If tbInvoiceNo.Text.Trim.Length <= 0 Then
                lbStatus.Text = MessageDlg("Trans. No. Cannot Empty.")
                tbInvoiceNo.Focus()
                Return False
            End If
            If tbSuppName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Supplier Cannot Empty.")
                tbSuppCode.Focus()
                Return False
            End If

            If ViewState("StateHd") = "Insert" Then
                Infois = SQLExecuteScalar("SELECT COALESCE(InvoiceNo, '') FROM FINBeginSI WHERE InvoiceNo = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
                If Infois.Length > 0 Then
                    lbStatus.Text = MessageDlg("Invoice No Exist, Cannot Save Data")
                    tbInvoiceNo.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    'Function cekDt() As Boolean
    '    Dim Infois As String
    '    Try
    '        If tbInvoiceNo.Text.Trim.Length <= 0 Then
    '            lbStatus.Text = MessageDlg("Trans. No. Cannot Empty.")
    '            tbInvoiceNo.Focus()
    '            Return False
    '        End If
    '        If tbSuppName.Text.Length <= 0 Then
    '            lbStatus.Text = MessageDlg("Supplier Cannot Empty.")
    '            tbSuppCode.Focus()
    '            Return False
    '        End If

    '        If ViewState("StateHd") = "Insert" Then
    '            Infois = SQLExecuteScalar("SELECT COALESCE(InvoiceNo, '') FROM FINBeginSI WHERE InvoiceNo = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
    '            If Infois.Length > 0 Then
    '                lbStatus.Text = MessageDlg("Invoice No Exist, Cannot Save Data")
    '                tbInvoiceNo.Focus()
    '                Return False
    '            End If
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Throw New Exception("Cek Hd Error : " + ex.ToString)
    '    End Try
    'End Function

    Protected Sub SaveData()
        Dim SQLString As String
        Dim DRDateWrhs, DRDateETA, DRDateETD, ShipDateETD, ShipDateETA, ShipDateCustom As String
        Dim DT As DataTable
        Try
            If tbDRDateWrhs.IsNull Then
                DRDateWrhs = "NULL"
            Else : DRDateWrhs = QuotedStr(Format(tbDRDateWrhs.SelectedDate, "yyyy-MM-dd"))
            End If

            If tbDRDateETA.IsNull Then
                DRDateETA = "NULL"
            Else : DRDateETA = QuotedStr(Format(tbDRDateETA.SelectedDate, "yyyy-MM-dd"))
            End If

            If tbDRDateETD.IsNull Then
                DRDateETD = "NULL"
            Else : DRDateETD = QuotedStr(Format(tbDRDateETD.SelectedDate, "yyyy-MM-dd"))
            End If

            If tbShipDateETD.IsNull Then
                ShipDateETD = "NULL"
            Else : ShipDateETD = QuotedStr(Format(tbShipDateETD.SelectedDate, "yyyy-MM-dd"))
            End If

            If tbShipDateETA.IsNull Then
                ShipDateETA = "NULL"
            Else : ShipDateETA = Format(tbShipDateETA.SelectedDate, "yyyy-MM-dd")
            End If

            If tbShipDateCustom.IsNull Then
                ShipDateCustom = "NULL"
            Else : ShipDateCustom = Format(tbShipDateCustom.SelectedDate, "yyyy-MM-dd")
            End If

            DT = SQLExecuteQuery("SELECT * FROM PRCPOImportStatus WHERE TransNmbr = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString).Tables(0)
           
            'Save Hd
            If DT.Rows.Count = 0 Then
                'insert                
                SQLString = "INSERT INTO PRCPOImportStatus (TransNmbr, " + _
                "DRDateWrhs, DRDateETA, DRDateETD, ShipDateETD, ShipDateETA, ShipDateCustom, DeliveryInLand, TotalContainer, " + _
                "ContainerNo, InvoiceNo, BLNo, AJUNo, Respon, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbInvoiceNo.Text) + _
                "," + DRDateWrhs + "," + DRDateETA + "," + DRDateETD + "," + ShipDateETD + _
                "," + ShipDateETA + "," + ShipDateCustom + _
                "," + QuotedStr(tbDeliveryInLand.Text) + "," + tbTotalContainer.Text.Replace(",", "") + "," + QuotedStr(tbContainerNo.Text) + _
                "," + QuotedStr(tbInvoiceNomor.Text) + "," + QuotedStr(tbBLNo.Text) + "," + QuotedStr(tbAJUNo.Text) + "," + QuotedStr(tbRespon.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else                
                'edit
                SQLString = "UPDATE PRCPOImportStatus SET DRDateWrhs = " + DRDateWrhs + _
                ", DRDateETA = " + DRDateETA + ", DRDateETD = " + DRDateETD + _
                ", ShipDateETD = " + ShipDateETD + ", ShipDateETA = " + ShipDateETA + ", ShipDateCustom = " + ShipDateCustom + _
                ", DeliveryInLand =" + QuotedStr(tbDeliveryInLand.Text) + ", TotalContainer=" + tbTotalContainer.Text.Replace(",", "") + _
                ", ContainerNo=" + QuotedStr(tbContainerNo.Text) + ", InvoiceNo=" + QuotedStr(tbInvoiceNomor.Text) + _
                ", BLNo=" + QuotedStr(tbBLNo.Text) + ", AJUNo=" + QuotedStr(tbAJUNo.Text) + _
                ", Respon=" + QuotedStr(tbRespon.Text) + ", UserId=" + QuotedStr(ViewState("UserId").ToString) + _
                ", UserDate = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbInvoiceNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Save Data Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "select * FROM VMsSupplier "
    '        ResultField = "Supplier_Code, Supplier_Name, Currency, Term"
    '        ViewState("Sender") = "btnSupp"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Acc Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbSuppCode.Text = Dr("Supplier_Code")
    '            tbSuppName.Text = Dr("Supplier_Name")
    '        Else
    '            tbSuppName.Text = ""
    '            tbSuppCode.Text = ""
    '        End If
    '        tbSuppCode.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "tb SuppCode Code ERror : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lkbAdvanceSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbAdvanceSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            'FDateName = "Trans. Date, D.R. Wrhs, D.R. ETA, D.R. ETD, Act. Ship. ETD, Act. Ship. ETA, Act. Ship. F. Customs"
            'FDateValue = "Transdate, DRDateWrhs, DRDateETA, DRDateETD, ShipDateETD, ShipDateETA, ShipDateCustom"
            'FilterName = "Trans. No, Supplier Code, Supplier Name, InLand Delivery, Container No, Invoice No, Bill of Lading No, AJU No, Customs Response"
            'FilterValue = "TransNmbr, Supplier, Supplier_Name, DeliveryInLand, ContainerNo, InvoiceNo, BLNo, AJUNo, Respon"

            FDateName = "Trans. Date"
            FDateValue = "Transdate"
            FilterName = "Trans. No, Supplier Code, Supplier Name"
            FilterValue = "TransNmbr, Supplier, Supplier_Name"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
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
        'If ViewState("Add") = "Clear" Then
        '    Cleardt()
        'Else
        '    'ddlAdjustType.SelectedValue = "+"
        'End If

        clearDt()

        If cekhd() = False Then
            Exit Sub
        End If

        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        'If ViewState("StateHd") = "Insert" Then
        '    Row = Nothing
        '    i = 0
        'Else
        Row = ViewState("Dt").select("")
        i = Row.Length
        'End If

        If i > 0 Then
            lbItemNo.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNo.Text = "1"
        End If

        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        StatusButtonSave(False)
    End Sub

    Protected Sub btnCancelDt_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click, btnBack.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        'btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CurrFilter, Value As String
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            MovePanel(pnlInput, PnlHd)

            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbInvoiceNo.Text
            ddlField.SelectedValue = "TransNmbr"
            BtnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        'Dim DRDateWrhs, DRDateETA, DRDateETD, ShipDateETD, ShipDateETA, ShipDateCustom As Date

        Try
            'If tbQty.Text = "" Then
            '    tbQty.Text = 0
            'End If

            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(lbItemNo.Text) Then
                    If CekExistData(ViewState("Dt"), "ItemNo", TrimStr(lbItemNo.Text)) Then
                        lbStatus.Text = "Item No has already exists"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If cekHd() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("ItemNo") = lbItemNo.Text

                If tbDRDateWrhs.IsNull Then
                    'DRDateWrhs = "NULL"
                    Row("STRDRDateWrhs") = ""
                Else
                    'DRDateWrhs = QuotedStr(Format(tbDRDateWrhs.SelectedDate, "yyyy-MM-dd"))
                    Row("STRDRDateWrhs") = Format(tbDRDateWrhs.SelectedDate, "yyyy-MM-dd")
                    Row("DRDateWrhs") = tbDRDateWrhs.SelectedDate
                End If

                If tbDRDateETA.IsNull Then
                    'DRDateETA = "NULL"
                    Row("STRDRDateETA") = ""
                Else
                    'DRDateETA = QuotedStr(Format(tbDRDateETA.SelectedDate, "yyyy-MM-dd"))
                    Row("STRDRDateETA") = Format(tbDRDateETA.SelectedDate, "yyyy-MM-dd")
                    Row("DRDateETA") = tbDRDateETA.SelectedDate
                End If

                If tbDRDateETD.IsNull Then
                    'DRDateETD = "NULL"
                    Row("STRDRDateETD") = ""
                Else
                    'DRDateETD = QuotedStr(Format(tbDRDateETD.SelectedDate, "yyyy-MM-dd"))
                    Row("STRDRDateETD") = Format(tbDRDateETD.SelectedDate, "yyyy-MM-dd")
                    Row("DRDateETD") = tbDRDateETD.SelectedDate
                End If

                If tbShipDateETA.IsNull Then
                    'ShipDateETA = "NULL"
                    Row("STRShipDateETA") = ""
                Else
                    'ShipDateETA = Format(tbShipDateETA.SelectedDate, "yyyy-MM-dd")
                    Row("STRShipDateETA") = Format(tbShipDateETA.SelectedDate, "yyyy-MM-dd")
                    Row("ShipDateETA") = tbShipDateETA.SelectedDate
                End If

                If tbShipDateETD.IsNull Then
                    'ShipDateETD = "NULL"
                    Row("STRShipDateETD") = ""
                Else
                    'ShipDateETD = QuotedStr(Format(tbShipDateETD.SelectedDate, "yyyy-MM-dd"))
                    Row("STRShipDateETD") = Format(tbShipDateETD.SelectedDate, "yyyy-MM-dd")
                    Row("ShipDateETD") = tbShipDateETD.SelectedDate
                End If

                If tbShipDateCustom.IsNull Then
                    'ShipDateCustom = "NULL"
                    Row("STRShipDateCustom") = ""
                Else
                    'ShipDateCustom = Format(tbShipDateCustom.SelectedDate, "yyyy-MM-dd")
                    Row("STRShipDateCustom") = Format(tbShipDateCustom.SelectedDate, "yyyy-MM-dd")
                    Row("ShipDateCustom") = tbShipDateCustom.SelectedDate
                End If

                Row("DeliveryInLand") = tbDeliveryInLand.Text
                Row("TotalContainer") = tbTotalContainer.Text.Replace(",", "")
                Row("ContainerNo") = tbContainerNo.Text
                Row("InvoiceNo") = tbInvoiceNomor.Text
                Row("BLNo") = tbBLNo.Text
                Row("AJUNo") = tbAJUNo.Text
                Row("Respon") = tbRespon.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If cekHd() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemNo", TrimStr(lbItemNo.Text)) Then
                    lbStatus.Text = "Item No '" + lbItemNo.Text + "' has already exists"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text

                If tbDRDateWrhs.IsNull Then
                    'DRDateWrhs = "NULL"
                    dr("STRDRDateWrhs") = ""
                Else
                    'DRDateWrhs = tbDRDateWrhs.SelectedDate
                    dr("STRDRDateWrhs") = Format(tbDRDateWrhs.SelectedDate, "yyyy-MM-dd")
                    dr("DRDateWrhs") = tbDRDateWrhs.SelectedDate
                End If

                If tbDRDateETA.IsNull Then
                    'DRDateETA = "NULL"
                    dr("STRDRDateETA") = ""
                Else
                    'DRDateETA = tbDRDateETA.SelectedDate
                    dr("STRDRDateETA") = Format(tbDRDateETA.SelectedDate, "yyyy-MM-dd")
                    dr("DRDateETA") = tbDRDateETA.SelectedDate
                End If

                If tbDRDateETD.IsNull Then
                    'DRDateETD = "NULL"
                    dr("STRDRDateETD") = ""
                Else
                    'DRDateETD = QuotedStr(Format(tbDRDateETD.SelectedDate, "yyyy-MM-dd"))
                    dr("STRDRDateETD") = Format(tbDRDateETD.SelectedDate, "yyyy-MM-dd")
                    dr("DRDateETD") = tbDRDateETD.SelectedDate
                End If

                If tbShipDateETA.IsNull Then
                    'ShipDateETA = "NULL"
                    dr("STRShipDateETA") = ""
                Else
                    'ShipDateETA = Format(tbShipDateETA.SelectedDate, "yyyy-MM-dd")
                    dr("STRShipDateETA") = Format(tbShipDateETA.SelectedDate, "yyyy-MM-dd")
                    dr("ShipDateETA") = tbShipDateETA.SelectedDate
                End If

                If tbShipDateETD.IsNull Then
                    'ShipDateETD = "NULL"
                    dr("STRShipDateETD") = ""
                Else
                    'ShipDateETD = QuotedStr(Format(tbShipDateETD.SelectedDate, "yyyy-MM-dd"))
                    dr("STRShipDateETD") = Format(tbShipDateETD.SelectedDate, "yyyy-MM-dd")
                    dr("ShipDateETD") = tbShipDateETD.SelectedDate
                End If

                If tbShipDateCustom.IsNull Then
                    'ShipDateCustom = "NULL"
                    dr("STRShipDateCustom") = ""
                Else
                    'ShipDateCustom = Format(tbShipDateCustom.SelectedDate, "yyyy-MM-dd")
                    dr("STRShipDateCustom") = Format(tbShipDateCustom.SelectedDate, "yyyy-MM-dd")
                    dr("ShipDateCustom") = tbShipDateCustom.SelectedDate
                End If

                dr("DeliveryInLand") = tbDeliveryInLand.Text
                dr("TotalContainer") = tbTotalContainer.Text.Replace(",", "")
                dr("ContainerNo") = tbContainerNo.Text
                dr("InvoiceNo") = tbInvoiceNomor.Text
                dr("BLNo") = tbBLNo.Text
                dr("AJUNo") = tbAJUNo.Text
                dr("Respon") = tbRespon.Text
                ViewState("Dt").Rows.Add(dr)
            End If

            MovePanel(pnlEditDt, pnlDt)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            GridDt.Columns(0).Visible = True
        Catch ex As Exception
            lbStatus.Text = "btnSaveDt_Click Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "GridDt_PageIndexChanging Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "GridDt_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            ViewState("DtValue") = GVR.Cells(1).Text

            'dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(4).Text))
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
            If GetCountRecord(ViewState("Dt")) = 0 Then
                GridDt.Columns(0).Visible = False
            End If
            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            lbStatus.Text = "GridDt_RowDeleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            'ViewState("DtValue") = GVR.Cells(4).Text
            ViewState("DtValue") = GVR.Cells(1).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "GridDt_RowEditing Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String

        Try
            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbInvoiceNo.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, DRDateWrhs, DRDateETA, DRDateETD, ShipDateETA, ShipDateETD, ShipDateCustom, DeliveryInLand, TotalContainer, InvoiceNo, BLNo, AJUNo, ContainerNo, Respon FROM PRCPOImportStatusDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PRCPOImportStatusDt SET ItemNo = @ItemNo, DRDateWrhs = @DRDateWrhs, DRDateETA = @DRDateETA, DRDateETD = @DRDateETD, ShipDateETA = @ShipDateETA, ShipDateETD = @ShipDateETD, ShipDateCustom = @ShipDateCustom, DeliveryInLand = @DeliveryInLand, TotalContainer = @TotalContainer, InvoiceNo = @InvoiceNo, BLNo = @BLNo, AJUNo = @AJUNo, ContainerNo = @ContainerNo, Respon = @Respon WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @ItemN", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@DRDateWrhs", SqlDbType.DateTime, 8, "DRDateWrhs")
            Update_Command.Parameters.Add("@DRDateETA", SqlDbType.DateTime, 8, "DRDateETA")
            Update_Command.Parameters.Add("@DRDateETD", SqlDbType.DateTime, 8, "DRDateETD")
            Update_Command.Parameters.Add("@ShipDateETA", SqlDbType.DateTime, 8, "ShipDateETA")
            Update_Command.Parameters.Add("@ShipDateETD", SqlDbType.DateTime, 8, "ShipDateETD")
            Update_Command.Parameters.Add("@ShipDateCustom", SqlDbType.DateTime, 8, "ShipDateCustom")
            Update_Command.Parameters.Add("@DeliveryInLand", SqlDbType.VarChar, 60, "DeliveryInLand")
            Update_Command.Parameters.Add("@TotalContainer", SqlDbType.Int, 4, "TotalContainer")
            Update_Command.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100, "InvoiceNo")
            Update_Command.Parameters.Add("@BLNo", SqlDbType.VarChar, 100, "BLNo")
            Update_Command.Parameters.Add("@AJUNo", SqlDbType.VarChar, 100, "AJUNo")
            Update_Command.Parameters.Add("@ContainerNo", SqlDbType.VarChar, 100, "ContainerNo")
            Update_Command.Parameters.Add("@Respon", SqlDbType.VarChar, 100, "Respon")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@ItemN", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PRCPOImportStatusDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCPOImportStatusDt")

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
            If cekHd() = False Then
                Exit Sub
            End If

            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbInvoiceNo.Text
            ddlField.SelectedValue = "TransNmbr"
            BtnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveAll_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
