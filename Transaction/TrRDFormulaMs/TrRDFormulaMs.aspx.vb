Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization

Partial Class Transaction_TrRDFormulaMs_TrRDFormulaMs
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_RDFormulaHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbSpecification, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustomer, Session("Result")(0).ToString)
                    BindToText(tbCustomerName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterialDt" Then
                    BindToText(tbMaterialDt, Session("Result")(0).ToString)
                    BindToText(tbMaterialDtName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterialDt3" Then
                    BindToText(tbMaterialDt3, Session("Result")(0).ToString)
                    BindToText(tbMaterialDt3Name, Session("Result")(1).ToString)
                    BindToText(tbSpecificationDt3, Session("Result")(2).ToString)
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                pnlDt4.Visible = True
                pnlEditDt4.Visible = False
            Else
                PnlDt.Visible = True
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
            'FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbRatio.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbLengthDt3.Attributes.Add("OnBlur", "setformatdt();")            
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
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_RDFormulaDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " and Revisi = " + Revisi
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_RDFormulaProcedure WHERE TransNmbr = " + QuotedStr(Nmbr) + " and Revisi = " + Revisi
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_RDFormulaSpec WHERE TransNmbr = " + QuotedStr(Nmbr) + " and Revisi = " + Revisi
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_RDFormulaPack WHERE TransNmbr = " + QuotedStr(Nmbr) + " and Revisi = " + Revisi
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
            GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_RDFormula", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")

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
            'tbDate.Enabled = State
            'tbSpecification.Enabled = State
            tbCustomer.Enabled = State
            'btnCustomer.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            BindGridDt(dt, GridDt4)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
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
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt4.Click
        Try
            MovePanel(pnlEditDt4, pnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbRevisi.Text = "0"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            tbRatio.Text = "0"
            ddlUserType.SelectedIndex = 0
            tbCustomer.Text = ""
            tbCustomerName.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbMaterialDt.Text = ""
            tbMaterialDtName.Text = ""
            tbPercentDt.Text = "0"
            'tbQtyDt.Text = "0"
            'tbWeightDt.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbProcedure1.Text = ""
            tbProcedure2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt3()
        Try
            tbMaterialDt3.Text = ""
            tbMaterialDt3Name.Text = ""
            tbTebal.Text = ""
            tbUkuran.Text = ""
            tbSpecificationDt3.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt4()
        Try
            tbStandardPack.Text = ""
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
            If tbProductCode.Text = "" Then
                lbStatus.Text = MessageDlg("Product must have value")
                tbProductCode.Focus()
                Return False
            End If
            If tbCustomerName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustomer.Focus()
                Return False
            End If
            If CFloat(tbRatio.Text.Trim) <= 0 Then
                lbStatus.Text = MessageDlg("Ratio Premix must have value")
                tbRatio.Focus()
                Return False
            End If
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
                If Dr("MaterialName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                'If Dr("StartDate").Then Then
                '    lbStatus.Text = MessageDlg("Start Date Must Have Value")
                '    Return False
                'End If
                'If Dr("EndDate").Then Then
                '    lbStatus.Text = MessageDlg("End Date Must Have Value")
                '    Return False
                'End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Formulation Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Percentage").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Percentage Formulation Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Weight").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Weight Formulation Must Have Value")
                    Return False
                End If
            Else
                If tbMaterialDtName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    tbMaterialDt.Focus()
                    Return False
                End If
                If CFloat(tbPercentDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Percentage Formulation Must Have Value")
                    tbPercentDt.Focus()
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
                If Dr("ProcedureDesc1").ToString = "" Then
                    lbStatus.Text = MessageDlg("Procedure #1 Must Have Value")
                    Return False
                End If

            Else
                If tbProcedure1.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Procedure #1 Must Have Value")
                    tbProcedure1.Focus()
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
                If Dr("Parameter").ToString = "" Then
                    lbStatus.Text = MessageDlg("Parameter Must Have Value")
                    Return False
                End If

            Else
                If tbStandardPack.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Standard Packages Must Have Value")
                    tbStandardPack.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            tbRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbProductCode, Dt.Rows(0)("Product").ToString)
            BindToText(tbProductName, Dt.Rows(0)("ProductName").ToString)
            BindToText(tbSpecification, Dt.Rows(0)("Specification").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbCustomer, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustomerName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(tbRatio, FormatNumber(Dt.Rows(0)("RatioPremix").ToString, 2))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Material = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbMaterialDt, Dr(0)("Material").ToString)
                BindToText(tbMaterialDtName, Dr(0)("MaterialName").ToString)
                BindToText(tbPercentDt, Dr(0)("Percentage").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemDt2.Text = Dr(0)("ItemNo").ToString
                BindToText(tbProcedure1, Dr(0)("ProcedureDesc1").ToString)
                BindToText(tbProcedure2, Dr(0)("ProcedureDesc2").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("Material = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbMaterialDt3, Dr(0)("Material").ToString)
                BindToText(tbMaterialDt3Name, Dr(0)("MaterialName").ToString)
                BindToText(tbUkuran, Dr(0)("Ukuran").ToString)
                BindToText(tbTebal, Dr(0)("Tebal").ToString)
                BindToText(tbSpecificationDt3, Dr(0)("Specification").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt4(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemDt4.Text = Dr(0)("ItemNo").ToString
                BindToText(tbStandardPack, Dr(0)("StandardPack").ToString)
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
                If ViewState("DtValue") <> tbMaterialDt.Text Then
                    If CekExistData(ViewState("Dt"), "Material", tbMaterialDt.Text) Then
                        lbStatus.Text = "Material " + tbMaterialDtName.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("Material = " + QuotedStr(tbMaterialDt.Text))(0)
                Row.BeginEdit()
                Row("Material") = tbMaterialDt.Text
                Row("MaterialName") = tbMaterialDtName.Text
                Row("Percentage") = FormatFloat(tbPercentDt.Text, 3) 'Row("Percentage") = FormatFloat(tbPercentDt.Text, ViewState("DigitQty"))
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt"), "Material", tbMaterialDt.Text) Then
                    lbStatus.Text = "Material " + tbMaterialDtName.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Material") = tbMaterialDt.Text
                dr("MaterialName") = tbMaterialDtName.Text
                dr("Percentage") = FormatFloat(tbPercentDt.Text, 3) 'dr("Percentage") = FormatFloat(tbPercentDt.Text, ViewState("DigitQty"))
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
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
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemDt2.Text)(0)
                Row.BeginEdit()
                Row("ProcedureDesc1") = tbProcedure1.Text
                Row("ProcedureDesc2") = tbProcedure2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = lbItemDt2.Text
                dr("ProcedureDesc1") = tbProcedure1.Text
                dr("ProcedureDesc2") = tbProcedure2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt4.Click
        Try

            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If
            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt4").Select("ItemNo = " + lbItemDt4.Text)(0)
                Row.BeginEdit()
                Row("StandardPack") = tbStandardPack.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("ItemNo") = lbItemDt4.Text
                dr("StandardPack") = tbStandardPack.Text
                ViewState("Dt4").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt4, pnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt4"), GridDt4)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt4 Error : " + ex.ToString
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
                tbCode.Text = GetAutoNmbr("RDFM", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO RDFormulaHd (TransNmbr, Revisi, TransDate, STATUS, Product, Specification, RatioPremix, Customer, Remark, UserPrep, DatePrep, UserType ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + tbRevisi.Text + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(tbSpecification.Text) + ", " + tbRatio.Text.Replace(",", "") + ", " + QuotedStr(tbCustomer.Text) + ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr(ddlUserType.SelectedValue)
                ViewState("TransNmbr") = tbCode.Text
                ViewState("Revisi") = tbRevisi.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM RDFormulaHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + tbRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE RDFormulaHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                            ", Product = " + QuotedStr(tbProductCode.Text) + _
                            ", Specification = " + QuotedStr(tbSpecification.Text) + _
                            ", RatioPremix = " + tbRatio.Text.Replace(",", "") + _
                            ", UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                            ", Customer = " + QuotedStr(tbCustomer.Text) + _
                            ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
                            " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " and Revisi = " + tbRevisi.Text
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
                Row(I)("Revisi") = tbRevisi.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = tbRevisi.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = tbRevisi.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I)("Revisi") = tbRevisi.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT  TransNmbr, Revisi, Material, Percentage, Remark FROM RDFormulaDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr") + " And Revisi = " + ViewState("Revisi")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("RDFormulaDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT  TransNmbr, Revisi, ItemNo, ProcedureDesc1, ProcedureDesc2 FROM RDFormulaProcedure WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr") + " And Revisi = " + ViewState("Revisi")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("RDFormulaProcedure")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT  TransNmbr, Revisi, Material, Ukuran, Tebal, Specification FROM RDFormulaSpec WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr") + " And Revisi = " + ViewState("Revisi")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("RDFormulaSpec")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            cmdSql = New SqlCommand("SELECT  TransNmbr, Revisi, ItemNo, StandardPack FROM RDFormulaPack WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr") + " And Revisi = " + ViewState("Revisi")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt4 As New DataTable("RDFormulaPack")

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
            If ViewState("Percentage") <> 100 Then
                lbStatus.Text = MessageDlg("Total Percentage must be 100")
                Menu1.Items.Item(0).Selected = True
                MultiView1.ActiveViewIndex = 0
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Formulation must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Procedure must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Specification must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Packing must have at least 1 record")
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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, pnlDt4, GridDt4)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbMaterialDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            lbItemDt2.Text = GetNewItemNo(ViewState("Dt2"))
            MovePanel(pnlDt2, pnlEditDt2)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbProcedure1.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt4.Click, btnAddDt4Ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            lbItemDt4.Text = GetNewItemNo(ViewState("Dt4"))
            MovePanel(pnlDt4, pnlEditDt4)
            ModifyInput2(True, pnlInput, pnlDt4, GridDt4)
            EnableHd(False)
            StatusButtonSave(False)
            tbStandardPack.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()
            Cleardt2()
            Cleardt3()
            Cleardt4()
            PnlDt.Visible = True
            BindDataDt("", "0")
            BindDataDt2("", "0")
            BindDataDt3("", "0")
            BindDataDt4("", "0")
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
            FDateName = "Formula Date"
            FDateValue = "TransDate"
            FilterName = "Formula No, Formula Date, Product, Customer, Specification, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), ProductName, CustomerName, Specification, Remark"
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
                    ViewState("Revisi") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt3(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt4(ViewState("TransNmbr"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, pnlDt4, GridDt4)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDt3(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDt4(ViewState("TransNmbr"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, pnlDt4, GridDt4)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Create Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_RDFormulaCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = tbCode.Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",0"
                        Session("SelectCommand2") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",1"
                        Session("SelectCommand3") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",2"
                        Session("SelectCommand4") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",3"

                        Session("ReportFile") = ".../../../Rpt/FormTrRDFormula.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg4ds();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Spec" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",0"
                        Session("SelectCommand2") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",1"
                        Session("SelectCommand3") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",2"
                        Session("SelectCommand4") = "EXEC S_RDFormulaForm " + QuotedStr(GVR.Cells(2).Text) + "," + GVR.Cells(3).Text + ",3"

                        Session("ReportFile") = ".../../../Rpt/FormTrRDFormulaSpec.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg4ds();", Page, Me.GetType)
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

    Dim Percentage As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Material")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
                    ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    Percentage = GetTotalSum(ViewState("Dt"), "Percentage")
                    e.Row.Cells(2).Text = "Total :"
                    'for the Footer, display the running totals
                    'e.Row.Cells(3).Text = FormatNumber(Percentage, 2)
                    e.Row.Cells(3).Text = FormatNumber(Percentage, 4) + " %"
                    e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    'e.Row.Cells(10).Text = FormatNumber(CrHome, ViewState("DigitHome"))
                    ViewState("Percentage") = CInt(Percentage)
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Material = " + QuotedStr(GVR.Cells(1).Text))
            For Each r In dr
                r.Delete()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            ViewState("DtValue") = GVR.Cells(1).Text
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            FillTextBoxDt4(GVR.Cells(1).Text)
            MovePanel(pnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            btnSaveDt4.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Formulation must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Procedure must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt3")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Packing must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt4")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Specification must have at least 1 record")
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

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click, btnAddDt3ke2.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            'lbItemDt3.Text = GetNewItemNo(ViewState("Dt3"))
            MovePanel(pnlDt3, pnlEditDt3)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            EnableHd(False)
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                If ViewState("DtValue3") <> tbMaterialDt3.Text Then
                    If CekExistData(ViewState("Dt3"), "Material", tbMaterialDt3.Text) Then
                        lbStatus.Text = "Material " + tbMaterialDt3Name.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("Material = " + QuotedStr(tbMaterialDt3.Text))(0)
                Row.BeginEdit()
                Row("Material") = tbMaterialDt3.Text
                Row("MaterialName") = tbMaterialDt3Name.Text
                Row("Ukuran") = tbUkuran.Text
                Row("Tebal") = tbTebal.Text
                Row("Specification") = tbSpecificationDt3.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt3"), "Material", tbMaterialDt3.Text) Then
                    lbStatus.Text = "Material " + tbMaterialDt3Name.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("Material") = tbMaterialDt3.Text
                dr("MaterialName") = tbMaterialDt3Name.Text
                dr("Ukuran") = tbUkuran.Text
                dr("Tebal") = tbTebal.Text
                dr("Specification") = tbSpecificationDt3.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
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
                If Dr("Material").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Supplier").ToString = "" Then
                    lbStatus.Text = MessageDlg("Supplier Must Have Value")
                    Return False
                End If
            Else
                If tbMaterialDt3Name.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    tbMaterialDt3.Focus()
                    Return False
                End If
                If tbSpecificationDt3.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Specification Must Have Value")
                    tbSpecificationDt3.Focus()
                    Return False
                End If
                If tbUkuran.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Ukuran Must Have Value")
                    tbUkuran.Focus()
                    Return False
                End If
                If tbTebal.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Tebal Must Have Value")
                    tbTebal.Focus()
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
            dr = ViewState("Dt3").Select("Material = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            ViewState("DtValue3") = GVR.Cells(1).Text
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomer.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT * FROM VMsCustomer Where FgActive = 'Y' "
            'ResultField = "Customer_Code, Customer_Name"
            Session("filter") = "SELECT User_Code, User_Name, Contact_Person FROM VMsUserType WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCustomer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustomer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustomer.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT Customer_Code, Customer_Name FROM VMsCustomer WHERE FgActive = 'Y' AND Customer_Code = " + QuotedStr(tbCustomer.Text), ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count > 0 Then
            Dr = FindMaster("UserType", tbCustomer.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbCustomer.Text = Dr("User_Code")
                tbCustomerName.Text = Dr("User_Name")
            Else
                tbCustomer.Text = ""
                tbCustomerName.Text = ""
            End If
            tbCustomer.Focus()
        Catch ex As Exception
            Throw New Exception("tbCustomer change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaterialDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialDt.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT * FROM VMsProduct WHERE Fg_Active = 'Y' "
            Session("filter") = "SELECT * FROM VMsProductMaterial WHERE Fg_Active = 'Y' "
            ResultField = "Product_Code, Product_Name"
            ViewState("Sender") = "btnMaterialDt"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterialDt Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMaterialDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterialDt3.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT * FROM VMsProduct WHERE Fg_Active = 'Y' "
            Session("filter") = "SELECT * FROM VMsProductMaterial WHERE Fg_Active = 'Y' "
            ResultField = "Product_Code, Product_Name, Specification"
            ViewState("Sender") = "btnMaterialDt3"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterial Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbMaterialDt3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterialDt3.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification FROM VMsProductMaterial WHERE Fg_Active = 'Y' AND Product_Code = " + QuotedStr(tbMaterialDt3.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterialDt3.Text = Dr("Product_Code")
                tbMaterialDt3Name.Text = Dr("Product_Name")
                tbSpecificationDt3.Text = Dr("Specification")
            Else
                tbMaterialDt3.Text = ""
                tbMaterialDt3Name.Text = ""
                tbSpecificationDt3.Text = ""
            End If
            tbMaterialDt3.Focus()
        Catch ex As Exception
            Throw New Exception("tbMaterialDt3_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "Select * from VMsProduct WHERE Fg_Active = 'Y' "
            Session("filter") = "Select * from VMsProductProduce WHERE Fg_Active = 'Y' "
            ResultField = "Product_Code, Product_Name, Specification2"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnProduct_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Product_Code, Product_Name, Specification2 from VMsProductProduce WHERE Fg_Active = 'Y' AND Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = Dr("Specification2")
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMaterialDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterialDt.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Product_Code, Product_Name from VMsProductMaterial WHERE Fg_Active = 'Y' AND Product_Code = " + QuotedStr(tbMaterialDt.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterialDt.Text = Dr("Product_Code")
                tbMaterialDtName.Text = Dr("Product_Name")
            Else
                tbMaterialDt.Text = ""
                tbMaterialDtName.Text = ""
            End If
            tbMaterialDt.Focus()
        Catch ex As Exception
            Throw New Exception("tbMaterialDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbCustomer.Text = ""
        tbCustomerName.Text = ""
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData(Session("AdvanceFilter"))
    End Sub
End Class
