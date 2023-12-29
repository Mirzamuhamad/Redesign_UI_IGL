Imports System.Data
Imports BasicFrame.WebControls

Partial Class Transaction_TrRDSampleConfirm_TrRDSampleConfirm
    Inherits System.Web.UI.Page

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

            'Hasil dari Advance Filter
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            'Hasil dari Search Dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    BindToText(tbCustName, Session("Result")(1).ToString)
                ElseIf ViewState("Sender") = "btnSample" Then
                    tbSampleCode.Text = Session("Result")(0).ToString
                    BindToText(tbSampleName, Session("Result")(1).ToString)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If

            lbStatus.Text = ""
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
            FillRange(ddlRange)
            ViewState("SortExpression") = Nothing
            'GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlUnit2, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            Me.tbQtySample.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbQtySheet.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Me.tbPPNForex.Attributes.Add("OnChange", "setformat();")
            'Me.tbTotalForex.Attributes.Add("OnChange", "setformat();")
            'Me.ddlUnit.Attributes.Add("OnChange", "setformat();")            
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
            DT = BindDataTransaction("Select * From V_RDSampleConfirm", StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "Nmbr DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()

            'If ddlShowRecord.SelectedIndex = 0 Then
            '    GridView1.PageSize = 15
            'Else
            '    GridView1.PageSize = ddlShowRecord.SelectedValue
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            GridView1.PageSize = ddlShowRecord.SelectedValue
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput(True, pnlInput)
            newTrans()
            MovePanel(PnlHd, pnlInput)
            btnHome.Visible = False
            tbTransDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbTransNmbr.Text = ""
            ddlUserType.SelectedIndex = 0
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSampleDate.SelectedDate = ViewState("ServerDate") 'Today
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbReqNo.Text = ""
            tbSampleCode.Text = ""
            tbSampleName.Text = ""
            tbStartPlan.SelectedDate = ViewState("ServerDate") 'Today
            tbEndPlan.SelectedDate = ViewState("ServerDate") 'Today
            tbDeliveryDate.SelectedDate = ViewState("ServerDate") 'Today
            tbQtySample.Text = "0"
            tbQtySheet.Text = "0"
            ddlMaterialAvailable.SelectedIndex = 0
            ddlUnit.SelectedValue = ""
            ddlUnit2.SelectedValue = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
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
                Session("SelectCommand") = "EXEC S_RDSampleConfirmForm " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/RDSampleConfirmForm.frx"
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

                        Result = ExecSPCommandGo(ActionValue, "S_RDSampleConfirm", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
                    btnHome.Visible = True
                    MovePanel(PnlHd, pnlInput)
                    FillTextBox(GVR)
                    ' change report harus diatas modifyinput dan dibawah filTextBox
                    ModifyInput(False, pnlInput)
                    ViewState("StateHd") = "View"
                ElseIf DDL.SelectedValue = "Edit" Then
                    Dim lbStatusTemp As String
                    lbStatusTemp = GVR.Cells(3).Text
                    If lbStatusTemp = "H" Or lbStatusTemp = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("StateHd") = "Edit"
                        ModifyInput(True, pnlInput)
                        FillTextBox(GVR)
                        btnHome.Visible = False
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Print
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_RDSampleConfirmForm ''" + QuotedStr(GVR.Cells(2).Text) + "'' "
                    Session("ReportFile") = ".../../../Rpt/RDSampleConfirmForm.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)

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

    Private Sub FillTextBox(ByVal e As GridViewRow)
        Dim Dt As DataTable
        Dim Nmbr As String
        Try
            Nmbr = e.Cells(2).Text
            Dt = BindDataTransaction("Select * From V_RDSampleConfirm", "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbTransNmbr.Text = Dt.Rows(0)("TransNmbr").ToString
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbSampleDate, Dt.Rows(0)("SampleDate").ToString)
            BindToDate(tbStartPlan, Dt.Rows(0)("StartPlan").ToString)
            BindToDate(tbEndPlan, Dt.Rows(0)("EndPlan").ToString)
            BindToDate(tbDeliveryDate, Dt.Rows(0)("DeliveryDate").ToString)
            BindToText(tbSampleCode, Dt.Rows(0)("Sample").ToString)
            BindToText(tbSampleName, Dt.Rows(0)("SampleName").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbReqNo, Dt.Rows(0)("SampleReqNo").ToString)
            BindToText(tbQtySample, Dt.Rows(0)("QtySample").ToString)
            BindToText(tbQtySheet, Dt.Rows(0)("QtySheet").ToString)
            BindToDropList(ddlUnit, Dt.Rows(0)("UnitPack").ToString)
            BindToDropList(ddlUnit2, Dt.Rows(0)("UnitPack2").ToString)
            BindToDropList(ddlMaterialAvailable, Dt.Rows(0)("MaterialAvailable").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
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
            tbFilter.Text = tbTransNmbr.Text
            ddlField.SelectedValue = "TransNmbr"
            BtnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save New Error : " + ex.ToString
        End Try
    End Sub

    Function cekhd() As Boolean
        Try
            If tbCustName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Customer Cannot Empty.")
                tbCustCode.Focus()
                Return False
            End If
            If tbSampleName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Sample Cannot Empty.")
                tbSampleName.Focus()
                Return False
            End If
            If tbReqNo.Text.Length = 0 Then
                lbStatus.Text = MessageDlg("Sample Request No Cannot Empty.")
                tbReqNo.Focus()
                Return False
            End If
            If tbSampleDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Sample Date must have value")
                tbSampleDate.Focus()
                Return False
            End If
            If CFloat(tbQtySample.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Qty Sample must be input values")
                tbQtySample.Focus()
                Return False
            End If
            If CFloat(tbQtySheet.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Qty Sheet must be input values")
                tbQtySheet.Focus()
                Return False
            End If
            If ddlUnit.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Unit Packing must have value")
                ddlUnit.Focus()
                Return False
            End If
            If ddlUnit2.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Unit Qty must have value")
                ddlUnit2.Focus()
                Return False
            End If
            If tbStartPlan.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Start Planning must have value")
                tbStartPlan.Focus()
                Return False
            End If
            If tbEndPlan.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("End Planning must have value")
                tbEndPlan.Focus()
                Return False
            End If
            If tbDeliveryDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Delivery Date must have value")
                tbDeliveryDate.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub SaveData()
        Dim SQLString As String
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbTransNmbr.Text = GetAutoNmbr("RDSC", "N", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'insert                
                SQLString = "Insert INTO RDSampleConfirm (TransNmbr, Status, TransDate, SampleDate, Sample, Customer, SampleReqNo, QtySheet, UnitPack, QtySample, MaterialAvailable, StartPlan, EndPlan, DeliveryDate, Remark, UserPrep, DatePrep, UserType, UnitPack2) " + _
                "SELECT " + QuotedStr(tbTransNmbr.Text) + ",'H'," + _
                QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbSampleDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbSampleCode.Text) + ", " + _
                QuotedStr(tbCustCode.Text) + "," + _
                QuotedStr(tbReqNo.Text) + "," + _
                tbQtySheet.Text.Replace(",", "") + "," + _
                QuotedStr(ddlUnit.SelectedValue) + "," + _
                tbQtySample.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlMaterialAvailable.SelectedValue) + ", " + _
                QuotedStr(Format(tbStartPlan.SelectedDate, "yyyy-MM-dd")) + "," + _
                QuotedStr(Format(tbEndPlan.SelectedDate, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbDeliveryDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()," + _
                QuotedStr(ddlUserType.SelectedValue) + "," + _
                QuotedStr(ddlUnit2.SelectedValue)
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM RDSampleConfirm WHERE TransNmbr = " + QuotedStr(tbTransNmbr.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE RDSampleConfirm SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", SampleDate = " + QuotedStr(Format(tbSampleDate.SelectedDate, "yyyy-MM-dd")) + _
                ", StartPlan = " + QuotedStr(Format(tbStartPlan.SelectedDate, "yyyy-MM-dd")) + _
                ", EndPlan = " + QuotedStr(Format(tbEndPlan.SelectedDate, "yyyy-MM-dd")) + _
                ", DeliveryDate = " + QuotedStr(Format(tbDeliveryDate.SelectedDate, "yyyy-MM-dd")) + _
                ", Sample = " + QuotedStr(tbSampleCode.Text) + _
                ", Customer= " + QuotedStr(tbCustCode.Text) + _
                ", SampleReqNo =" + QuotedStr(tbReqNo.Text) + _
                ", QtySheet = " + tbQtySheet.Text.Replace(",", "") + _
                ", UnitPack = " + QuotedStr(ddlUnit.SelectedValue) + _
                ", UnitPack2 = " + QuotedStr(ddlUnit2.SelectedValue) + _
                ", QtySample = " + tbQtySample.Text.Replace(",", "") + _
                ", MaterialAvailable = " + QuotedStr(ddlMaterialAvailable.SelectedValue) + _
                ", Remark=" + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate(), UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
                ", UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNmbr.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Save Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            'dt = SQLExecuteQuery("Select CustCode, CustName FROM MsCustomer WHERE CustCode = " + QuotedStr(tbCustCode.Text), ViewState("DBConnection")).Tables(0)
            Dr = FindMaster("UserType", tbCustCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("User_Code")
                tbCustName.Text = Dr("User_Name")
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb CustCode Code ERror : " + ex.ToString
        End Try
    End Sub

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
            FDateName = "Date, Sample Date, DeliveryDate"
            FDateValue = "TransDate, SampleDate, DeliveryDate"
            FilterName = "Reference, Status, Date, Sample Date, Sample, Customer, Sample Request, Start Planning, End Planning, Delivery Date, Remark"
            FilterValue = "TransNmbr, Status, dbo.FormatDate(TransDate), dbo.FormatDate(SampleDate), Customer_Name, SampleName, SampleReqNo, dbo.FormatDate(StartPlan), dbo.FormatDate(EndPlan), dbo.FormatDate(DeliveryDate), Remark"
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

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub tbSampleCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSampleCode.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            dt = SQLExecuteQuery("Select SampleCode, SampleName FROM MsSample WHERE SampleCode = " + QuotedStr(tbSampleCode.Text), ViewState("DBConnection")).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbSampleCode.Text = Dr("SampleCode")
                tbSampleName.Text = Dr("SampleName")
            Else
                tbSampleCode.Text = ""
                tbSampleName.Text = ""
            End If
            tbSampleCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbSampleCode_TextChangedERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSample_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSample.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SampleCode, SampleName FROM MsSample "
            ResultField = "SampleCode, SampleName"
            ViewState("Sender") = "btnSample"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Bill To Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            'Session("filter") = "select * FROM VMsCustomer "
            'ResultField = "Customer_Code, Customer_Name"
            Session("filter") = "SELECT User_Code, User_Name, Contact_Person FROM VMsUserType WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Transaction_TrRDSampleConfirm_TrRDSampleConfirm_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    End Sub

    Protected Sub Transaction_TrRDSampleConfirm_TrRDSampleConfirm_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbCustCode.Text = ""
        tbCustName.Text = ""
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData(Session("AdvanceFilter"))
    End Sub
End Class
