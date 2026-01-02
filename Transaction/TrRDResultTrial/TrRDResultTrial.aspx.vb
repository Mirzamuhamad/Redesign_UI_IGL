Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization

Partial Class Transaction_TrRDResultTrial_TrRDResultTrial
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_RDResultTrialHd"

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
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProduct" Then
                    'Product_Code, ProductName, Length, Width, Net_Weight"
                    tbProductCode.Text = Session("Result")(0).ToString
                    BindToText(tbProductName, Session("Result")(1).ToString)
                    BindToText(tbSpecification, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnMachine" Then
                    BindToText(tbMachine, Session("Result")(0).ToString)
                    BindToText(tbMachineName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustomer, Session("Result")(0).ToString)
                    BindToText(tbCustomerName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnSupplier" Then
                    BindToText(tbSuppCode, Session("Result")(0).ToString)
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterialDt" Then
                    BindToText(tbMaterialDt, Session("Result")(0).ToString)
                    BindToText(tbMaterialDtName, Session("Result")(1).ToString)
                End If
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
        Try
            FillRange(ddlRange)
            'FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            'tbLength.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbLength.Attributes.Add("OnBlur", "setformat();")
            'Me.tbQtyFormulasi.Attributes.Add("ReadOnly", "True")
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
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_RDResultTrialDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_RDResultTrialForm " + Result
                Session("ReportFile") = ".../../../Rpt/FormRDResultTrial.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_RDResultTrial", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            tbMachine.Text = ""
            tbMachineName.Text = ""
            tbSpecification.Text = ""
            ddlUserType.SelectedIndex = 0
            tbCustomer.Text = ""
            tbCustomerName.Text = ""
            tbNote.Text = ""
            tbCommentME.Text = ""
            tbCommentProd.Text = ""
            ddlTrialType.SelectedIndex = 0
            tbConclusion.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbMaterialDt.Text = ""
            tbMaterialDtName.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbobservationQC.Text = ""
            tbobservationRD.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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
            If tbMachine.Text = "" Then
                lbStatus.Text = MessageDlg("Machine must have value")
                tbMachine.Focus()
                Return False
            End If
            If tbCustomerName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustomer.Focus()
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
                If Dr("Supplier").ToString = "" Then
                    lbStatus.Text = MessageDlg("Supplier Must Have Value")
                    Return False
                End If
            Else
                If tbMaterialDtName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    tbMaterialDt.Focus()
                    Return False
                End If
                If tbSuppCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Supplier Must Have Value")
                    tbSuppCode.Focus()
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
            BindToText(tbProductCode, Dt.Rows(0)("Product").ToString)
            BindToText(tbProductName, Dt.Rows(0)("ProductName").ToString)
            BindToText(tbMachine, Dt.Rows(0)("Machine").ToString)
            BindToText(tbMachineName, Dt.Rows(0)("MachineName").ToString)
            BindToText(tbSpecification, Dt.Rows(0)("Specification").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbCustomer, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustomerName, Dt.Rows(0)("CustomerName").ToString)
            BindToDropList(ddlTrialType, Dt.Rows(0)("TrialType").ToString)
            BindToText(tbCommentME, Dt.Rows(0)("CommentME").ToString)
            BindToText(tbCommentProd, Dt.Rows(0)("CommentProduction").ToString)
            BindToText(tbNote, Dt.Rows(0)("Note").ToString)
            BindToText(tbConclusion, Dt.Rows(0)("Conclusion").ToString)
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
                BindToText(tbobservationQC, Dr(0)("ResultObservationQC").ToString)
                BindToText(tbobservationRD, Dr(0)("ResultObservationRD").ToString)
                BindToText(tbSuppCode, Dr(0)("Supplier").ToString)
                BindToText(tbSuppName, Dr(0)("SupplierName").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
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
                Row("Supplier") = tbSuppCode.Text
                Row("SupplierName") = tbSuppName.Text
                Row("ResultObservationQC") = tbobservationQC.Text
                Row("ResultObservationRD") = tbobservationRD.Text
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
                dr("Supplier") = tbSuppCode.Text
                dr("SupplierName") = tbSuppName.Text
                dr("ResultObservationQC") = tbobservationQC.Text
                dr("ResultObservationRD") = tbobservationRD.Text
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("RDLRT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO RDResultTrialHd (TransNmbr, TransDate, STATUS, Product, Machine, Specification, Customer, TrialType, CommentProduction, CommentME, Note, Conclusion, UserPrep, DatePrep, UserType ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbProductCode.Text) + ", " + QuotedStr(tbMachine.Text) + "," + QuotedStr(tbSpecification.Text) + ", " + QuotedStr(tbCustomer.Text) + ", " + QuotedStr(ddlTrialType.SelectedValue) + "," + _
                QuotedStr(tbCommentProd.Text) + "," + QuotedStr(tbCommentME.Text) + "," + QuotedStr(tbNote.Text) + ", " + QuotedStr(tbConclusion.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr(ddlUserType.SelectedValue)
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM RDResultTrialHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE RDResultTrialHd SET TransDate = " + _
                QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Product = " + QuotedStr(tbProductCode.Text) + _
                ", Machine = " + QuotedStr(tbMachine.Text) + _
                ", CommentME = " + QuotedStr(tbCommentME.Text) + _
                ", CommentProduction = " + QuotedStr(tbCommentProd.Text) + _
                ", Specification = " + QuotedStr(tbSpecification.Text) + _
                ", Note = " + QuotedStr(tbNote.Text) + _
                ", UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                ", Customer = " + QuotedStr(tbCustomer.Text) + _
                ", TrialType = " + QuotedStr(ddlTrialType.SelectedValue) + _
                ", Conclusion = " + QuotedStr(tbConclusion.Text) + _
                ", DatePrep = GetDate()" + _
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT  TransNmbr, Material, Supplier, ResultObservationRD, ResultObservationQC FROM RDResultTrialDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("RDResultTrialDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
                lbStatus.Text = MessageDlg("Detail Material must have at least 1 record")
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
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbMaterialDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
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
            FDateName = "Formula PP Date"
            FDateValue = "TransDate"
            FilterName = "Formula PP No, Formula PP Date, Sample, Product, Customer, ConfirmNo, Sample Request, Sample Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), SampleName, ProductName, CustomerName, ConfirmNo, SampleReqNo, dbo.FormatDate(SampleDate), Remark"
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
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("SelectCommand") = "EXEC S_RDResultTrialForm '''" + GVR.Cells(2).Text + "'''"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormRDResultTrial.frx"
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
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
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Material must have at least 1 record")
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

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "Select * from VMsProduct WHERE Fg_Active = 'Y' "
            Session("filter") = "Select * from VMsProductProduce WHERE Fg_Active = 'Y' "
            ResultField = "Product_Code, Product_Name, Specification"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnProduct_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachine.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select MachineCode, MachineName from MsMachine "
            ResultField = "MachineCode, MachineName"
            ViewState("Sender") = "btnMachine"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnMachine_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMachine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachine.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT MachineCode, MachineName from MsMachine WHERE MachineCode = " + QuotedStr(tbMachine.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMachine.Text = Dr("MachineCode")
                tbMachineName.Text = Dr("MachineName")
            Else
                tbMachine.Text = ""
                tbMachineName.Text = ""
            End If
            tbMachine.Focus()
        Catch ex As Exception
            Throw New Exception("tbMachine_TextChanged change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Product_Code, Product_Name, Specification from VMsProductProduce WHERE Fg_Active = 'Y' AND Product_Code = " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = Dr("Specification")
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

    Protected Sub btnSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupplier.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select * from VMsSupplier WHERE FgActive = 'Y' "
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnSupplier_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Supplier_Code, Supplier_Name FROM VMsSupplier WHERE FgActive = 'Y' AND Supplier_Code = " + QuotedStr(tbSuppCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tbSuppCode_TextChanged Error : " + ex.ToString)
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
