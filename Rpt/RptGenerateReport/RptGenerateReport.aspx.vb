Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Rpt_RptGenerateReport_RptGenerateReport
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                BindDataDt("", "")

                'ddlAOperator1_SelectedIndexChanged(Nothing, Nothing)
            End If
            lbStatus.Text = ""
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnReport" Then
                    tbReport.Text = Session("Result")(0).ToString
                    tbReportName.Text = Session("Result")(1).ToString
                    tbTable.Text = Session("Result")(2).ToString
                    tbTemp.Text = ""
                    tbTempName.Text = ""
                    FillComboColumn()
                    ddlAColumn1.Enabled = True
                    ddlType_SelectedIndexChanged(Nothing, Nothing)
                    tbReport_TextChanged(Nothing, Nothing)
                    'ClearCriteria()
                    pnlPrint.Visible = False
                End If
                If ViewState("Sender") = "btnTemp" Then
                    tbTemp.Text = Session("Result")(0).ToString
                    tbTempName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlType, Session("Result")(2).ToString.Trim)
                    ddlType_SelectedIndexChanged(Nothing, Nothing)
                    'EnableCriteriaReset()
                    tbTempName.Enabled = False
                    'FillCriteria()
                    tbTemp_TextChanged(Nothing, Nothing)
                    BindToDropList(ddlTabColumn, Session("Result")(3).ToString.Trim)
                    BindToDropList(ddlTabResult, Session("Result")(4).ToString.Trim)
                    pnlPrint.Visible = False
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    For Each drResult In Session("Result").Rows
                        MaxItem = GetNewItemNo(ViewState("Dt"))
                        If CekExistData(ViewState("Dt"), "ColumnField", drResult("ColumnField")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = MaxItem
                            dr("RptCode") = tbReport.Text
                            dr("Template") = tbTemp.Text
                            dr("ColumnField") = drResult("ColumnField")
                            dr("ColumnAlias") = drResult("ColumnField").ToString.Replace("_", " ")
                            dr("SortOrder") = "0"
                            dr("FgDisplay") = "Y"
                            dr("SortType") = ""
                            dr("ColumnFunct") = ""
                            dr("Type") = drResult("ColumnType")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
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


    Private Sub FillComboColumn()
        Try
            FillCombo(ddlColumn, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection").ToString)
            FillCombo(ddlAColumn1, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlAColumn2, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlAColumn3, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlBColumn1, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlBColumn2, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlBColumn3, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlCColumn1, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlCColumn2, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlCColumn3, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlTabColumn, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlTabResult, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
            FillCombo(ddlColumnType, "EXEC S_GetColumnType " + QuotedStr(tbTable.Text), True, "ColumnField", "ColumnType", ViewState("DBConnection"))
        Catch ex As Exception
            lbStatus.Text = "Fill Combo Column Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillCriteria()
        Dim Dt As DataTable
        Dim dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select * from VMsReportTemplateCriteria Where RptCode = " + QuotedStr(tbReport.Text.Trim) + " and Template = " + QuotedStr(tbTemp.Text.Trim), ViewState("DBConnection")).Tables(0)
            ClearCriteria()
            EnableCriteriaReset()
            If Dt.Rows.Count > 0 Then
                dr = Dt.Rows(0)
                ddlAColumn1.SelectedValue = TrimStr(dr("AColumn1").ToString)
                ddlAColumn2.SelectedValue = TrimStr(dr("AColumn2").ToString)
                ddlAColumn3.SelectedValue = TrimStr(dr("AColumn3").ToString)
                ddlBColumn1.SelectedValue = TrimStr(dr("BColumn1").ToString)
                ddlBColumn2.SelectedValue = TrimStr(dr("BColumn2").ToString)
                ddlBColumn3.SelectedValue = TrimStr(dr("BColumn3").ToString)
                ddlCColumn1.SelectedValue = TrimStr(dr("CColumn1").ToString)
                ddlCColumn2.SelectedValue = TrimStr(dr("CColumn2").ToString)
                ddlCColumn3.SelectedValue = TrimStr(dr("CColumn3").ToString)
                ddlAOperator1.SelectedValue = TrimStr(dr("AOperator1").ToString)
                ddlAOperator2.SelectedValue = TrimStr(dr("AOperator2").ToString)
                ddlAOperator3.SelectedValue = TrimStr(dr("AOperator3").ToString)
                ddlBOperator1.SelectedValue = TrimStr(dr("BOperator1").ToString)
                ddlBOperator2.SelectedValue = TrimStr(dr("BOperator2").ToString)
                ddlBOperator3.SelectedValue = TrimStr(dr("BOperator3").ToString)
                ddlCOperator1.SelectedValue = TrimStr(dr("COperator1").ToString)
                ddlCOperator2.SelectedValue = TrimStr(dr("COperator2").ToString)
                ddlCOperator3.SelectedValue = TrimStr(dr("COperator3").ToString)
                ddlALogic2.SelectedValue = TrimStr(dr("ALogic2").ToString)
                ddlALogic3.SelectedValue = TrimStr(dr("ALogic3").ToString)
                ddlBLogic1.SelectedValue = TrimStr(dr("BLogic1").ToString)
                ddlBLogic2.SelectedValue = TrimStr(dr("BLogic2").ToString)
                ddlBLogic3.SelectedValue = TrimStr(dr("BLogic3").ToString)
                ddlCLogic1.SelectedValue = TrimStr(dr("CLogic1").ToString)
                ddlCLogic2.SelectedValue = TrimStr(dr("CLogic2").ToString)
                ddlCLogic3.SelectedValue = TrimStr(dr("CLogic3").ToString)
                tbAValue1.Text = TrimStr(dr("AValue1").ToString)
                tbAValue2.Text = TrimStr(dr("AValue2").ToString)
                tbAValue3.Text = TrimStr(dr("AValue3").ToString)
                tbBValue1.Text = TrimStr(dr("BValue1").ToString)
                tbBValue2.Text = TrimStr(dr("BValue2").ToString)
                tbBValue3.Text = TrimStr(dr("BValue3").ToString)
                tbCValue1.Text = TrimStr(dr("CValue1").ToString)
                tbCValue2.Text = TrimStr(dr("CValue2").ToString)
                tbCValue3.Text = TrimStr(dr("CValue3").ToString)
            End If
            EnableCriteria()
            BindDataDt(tbReport.Text, tbTemp.Text)
        Catch ex As Exception
            lbStatus.Text = "Fill Template Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub RefereshButton()
        If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
            lbJudul.Text = "Report Creator"
            btnGetDt.Visible = tbReportName.Text <> "" And tbTempName.Text <> ""
            btnUser.Visible = tbReportName.Text <> "" And tbTempName.Text <> ""
            btnAddDt.Visible = btnUser.Visible
            btnAddDt2.Visible = btnUser.Visible
            GridDt.Columns(0).Visible = True
            ddlType.Enabled = True
        Else
            lbJudul.Text = "Report Generator"
            btnGetDt.Visible = False
            btnUser.Visible = False
            'DisableCriteriaUser()
            btnAddDt.Visible = False
            btnAddDt2.Visible = False
            GridDt.Columns(0).Visible = False
            ddlType.Enabled = False
        End If
    End Sub

    Private Sub SetInit()
        ddlTabColumn.Enabled = False
        ddlTabResult.Enabled = False
        GridResult.PageSize = CInt(ViewState("PageSizeGrid"))
        RefereshButton()
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
    End Sub

    Private Sub BindDataDt(ByVal Rpt As String, ByVal Template As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery("SELECT * From VMsReportTemplateDt WHERE RptCode = " + QuotedStr(Rpt) + " And Template = " + QuotedStr(Template), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("RptCode") = tbReport.Text
                Row("Template") = tbTemp.Text
                Row("ColumnField") = ddlColumn.SelectedValue
                Row("ColumnAlias") = tbAlias.Text
                Row("FgDisplay") = ddlDisplay.SelectedValue
                Row("SortType") = ddlSortType.SelectedValue
                Row("SortOrder") = ddlSortOrder.SelectedValue
                Row("Type") = tbType.Text
                Row("ColumnFunct") = ddlFunction.SelectedValue
                Row("CompareOperator") = ddlCompare.SelectedValue
                Row("CompareField") = ddlColumnCompare.SelectedValue
                Row("Compare") = ddlCompare.SelectedValue + " " + ddlColumnCompare.SelectedValue
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("ColumnField") = ddlColumn.SelectedValue
                dr("ColumnAlias") = tbAlias.Text
                dr("SortOrder") = ddlSortOrder.SelectedValue
                dr("SortType") = ddlSortType.SelectedValue
                dr("FgDisplay") = ddlDisplay.SelectedValue
                dr("Type") = tbType.Text
                dr("ColumnFunct") = ddlFunction.SelectedValue
                dr("CompareOperator") = ddlCompare.SelectedValue
                dr("CompareField") = ddlColumnCompare.SelectedValue
                dr("Compare") = ddlCompare.SelectedValue + " " + ddlColumnCompare.SelectedValue
                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
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
            'Save Template
            SQLString = "EXEC S_MsReportTemplateUpdate " + QuotedStr(tbReport.Text) + ", " + QuotedStr(tbTemp.Text) + " , " + QuotedStr(tbTempName.Text) + "," + _
                QuotedStr(ddlType.SelectedValue) + ", " + QuotedStr(ddlTabColumn.SelectedValue) + ", " + QuotedStr(ddlTabResult.SelectedValue) + ", " + QuotedStr(ViewState("UserId"))
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))

            'Save Template Criteria
            SQLString = "EXEC S_MsReportTemplateCriteriaUpdate " + QuotedStr(tbReport.Text) + ", " + QuotedStr(tbTemp.Text) + ", " + QuotedStr(ddlAColumn1.SelectedValue) + ", " + _
                QuotedStr(ddlAOperator1.SelectedValue) + ", " + QuotedStr(tbAValue1.Text) + ", " + QuotedStr(ddlALogic2.SelectedValue) + ", " + _
                QuotedStr(ddlAColumn2.SelectedValue) + ", " + QuotedStr(ddlAOperator2.SelectedValue) + ", " + QuotedStr(tbAValue2.Text) + ", " + QuotedStr(ddlALogic3.SelectedValue) + ", " + _
                QuotedStr(ddlAColumn3.SelectedValue) + ", " + QuotedStr(ddlAOperator3.SelectedValue) + ", " + QuotedStr(tbAValue3.Text) + ", " + QuotedStr(ddlBLogic1.SelectedValue) + ", " + _
                QuotedStr(ddlBColumn1.SelectedValue) + ", " + QuotedStr(ddlBOperator1.SelectedValue) + ", " + QuotedStr(tbBValue1.Text) + ", " + QuotedStr(ddlBLogic2.SelectedValue) + ", " + _
                QuotedStr(ddlBColumn2.SelectedValue) + ", " + QuotedStr(ddlBOperator2.SelectedValue) + ", " + QuotedStr(tbBValue2.Text) + ", " + QuotedStr(ddlBLogic3.SelectedValue) + ", " + _
                QuotedStr(ddlBColumn3.SelectedValue) + ", " + QuotedStr(ddlBOperator3.SelectedValue) + ", " + QuotedStr(tbBValue3.Text) + ", " + QuotedStr(ddlCLogic1.SelectedValue) + ", " + _
                QuotedStr(ddlCColumn1.SelectedValue) + ", " + QuotedStr(ddlCOperator1.SelectedValue) + ", " + QuotedStr(tbCValue1.Text) + ", " + QuotedStr(ddlCLogic2.SelectedValue) + ", " + _
                QuotedStr(ddlCColumn2.SelectedValue) + ", " + QuotedStr(ddlCOperator2.SelectedValue) + ", " + QuotedStr(tbCValue2.Text) + ", " + QuotedStr(ddlCLogic3.SelectedValue) + ", " + _
                QuotedStr(ddlCColumn3.SelectedValue) + ", " + QuotedStr(ddlCOperator3.SelectedValue) + ", " + QuotedStr(tbCValue3.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))

            'Save Template Dt
            Dim Row As DataRow()
            Row = ViewState("Dt").Select("RptCode IS NULL OR Template IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("RptCode") = tbReport.Text
                Row(I)("Template") = tbTemp.Text
                Row(I).EndEdit()
            Next
            'save dt
            SaveDt()
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Try
            If CekCriteria() = False Then
                Exit Sub
            End If
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = "Detail column must be fill"
                Exit Sub
            End If
            SaveAll()
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearCriteria()
        Try
            ddlAColumn1.SelectedIndex = 0
            ddlAColumn2.SelectedIndex = 0
            ddlAColumn3.SelectedIndex = 0
            ddlBColumn1.SelectedIndex = 0
            ddlBColumn2.SelectedIndex = 0
            ddlBColumn3.SelectedIndex = 0
            ddlCColumn1.SelectedIndex = 0
            ddlCColumn2.SelectedIndex = 0
            ddlCColumn3.SelectedIndex = 0
            ddlAOperator1.SelectedIndex = 0
            ddlAOperator2.SelectedIndex = 0
            ddlAOperator3.SelectedIndex = 0
            ddlBOperator1.SelectedIndex = 0
            ddlBOperator2.SelectedIndex = 0
            ddlBOperator3.SelectedIndex = 0
            ddlCOperator1.SelectedIndex = 0
            ddlCOperator2.SelectedIndex = 0
            ddlCOperator3.SelectedIndex = 0
            ddlALogic2.SelectedIndex = 0
            ddlALogic3.SelectedIndex = 0
            ddlBLogic1.SelectedIndex = 0
            ddlBLogic2.SelectedIndex = 0
            ddlBLogic3.SelectedIndex = 0
            ddlCLogic1.SelectedIndex = 0
            ddlCLogic2.SelectedIndex = 0
            ddlCLogic3.SelectedIndex = 0
            tbAValue1.Text = ""
            tbAValue2.Text = ""
            tbAValue3.Text = ""
            tbBValue1.Text = ""
            tbBValue2.Text = ""
            tbBValue3.Text = ""
            tbCValue1.Text = ""
            tbCValue2.Text = ""
            tbCValue3.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            lbItemNo.Text = ""
            ddlColumn.SelectedIndex = 0
            tbAlias.Text = ""
            ddlDisplay.SelectedIndex = 0
            ddlSortOrder.SelectedIndex = 0
            ddlSortType.Items.Clear()
            tbType.Text = ""
            ddlFunction.SelectedValue = ""
            ddlDisplay.SelectedValue = "Y"
            ddlCompare.SelectedValue = ""
            FillComboBlank(ddlColumnCompare)
            'ddlColumnCompare.Items.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnTemp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemp.Click
        Dim ResultField As String
        Try
            'CriteriaField = "TemplateCode, TemplateName, RptType, ColumnTab, ResultTab"
            If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
                Session("Filter") = "Select TemplateCode, TemplateName, RptType, ColumnTab, ResultTab FROM VMsReportTemplate where RptCode = " + QuotedStr(tbReport.Text)
            Else
                Session("Filter") = "Select A.* from VMsReportTemplate A INNER JOIN VMsReportTemplateUser B ON A.RptCode = B.RptCode AND A.TemplateCode = B.template AND B.userId = " + QuotedStr(ViewState("UserId").ToString) + " And A.RptCode = " + QuotedStr(tbReport.Text)
            End If
            ResultField = "TemplateCode, TemplateName, RptType, ColumnTab, ResultTab"
            'Session("CriteriaField") = CriteriaField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnTemp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search TemplateCode Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbTemp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTemp.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try

            If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
                SQLString = "Select TemplateCode, TemplateName, RptType, ColumnTab, ResultTab FROM VMsReportTemplate where RptCode = '" + tbReport.Text.Trim + "' AND TemplateCode = '" + tbTemp.Text.Trim + "'"
            Else
                SQLString = "Select A.TemplateCode, A.TemplateName, A.RptType, A.ColumnTab, A.ResultTab from VMsReportTemplate A INNER JOIN VMsReportTemplateUser B ON A.RptCode = B.RptCode AND A.TemplateCode = B.template AND B.UserId = " + QuotedStr(ViewState("UserId").ToString) + " And A.RptCode = '" + tbReport.Text.Trim + "' AND A.TemplateCode = '" + tbTemp.Text.Trim + "'"
            End If
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbTemp.Text = Dr("TemplateCode").ToString
                tbTempName.Text = Dr("TemplateName").ToString
                tbTempName.Enabled = False
                ddlType.SelectedValue = TrimStr(Dr("RptType").ToString)
                'EnableCriteriaReset()
                FillCriteria()
                'EnableCriteria()
                ddlType_SelectedIndexChanged(Nothing, Nothing)
                ddlTabColumn.SelectedValue = TrimStr(Dr("ColumnTab").ToString)
                ddlTabResult.SelectedValue = TrimStr(Dr("ResultTab").ToString)
            Else
                tbTempName.Text = ""
                ddlType_SelectedIndexChanged(Nothing, Nothing)
                BindDataDt("", "")
                If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
                    tbTempName.Enabled = True
                Else
                    tbTemp.Text = ""
                    tbTempName.Enabled = False
                End If
                ClearCriteria()
                EnableCriteria()
            End If
            pnlPrint.Visible = False
            RefereshButton()
            DisplayLabelDate()
            'labelDateA1()
            'labelDateA2()
            'labelDateA3()
            'labelDateB1()
            'labelDateB2()
            'labelDateB3()
            'labelDateC1()
            'labelDateC2()
            'labelDateC3()
            'AttachScript("setformatdt();", Page, Me.GetType())
            tbTemp.Focus()
        Catch ex As Exception
            Throw New Exception("tb Temp Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click

        If CekHd() = False Then
            Exit Sub
        End If
        Cleardt()
        ViewState("StateDt") = "Insert"
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        'EnableHd(False)
        StatusButtonSave(False)
        tbTemp.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If tbReport.Text.Trim = "" Then
                lbStatus.Text = "Report Must Have Value"
                tbReport.Focus()
                Return False
            End If
            If tbTemp.Text.Trim = "" Then
                lbStatus.Text = "Template Must Have Value"
                tbTemp.Focus()
                Return False
            End If
            If tbTempName.Text.Trim = "" Then
                lbStatus.Text = "Template Name Must Have Value"
                tbTempName.Focus()
                Return False
            End If
            If ddlType.SelectedValue <> "Common" Then
                If ddlTabColumn.SelectedValue = "" Then
                    lbStatus.Text = "CrossTab Column Must Have Value"
                    ddlTabColumn.Focus()
                    Return False
                End If
                If ddlTabResult.SelectedValue = "" Then
                    lbStatus.Text = "CrossTab Result Must Have Value"
                    ddlTabResult.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function


    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("RptCode").ToString.Trim = "" Then
                    lbStatus.Text = "Report Must Have Value"
                    Return False
                End If
                If Dr("Template").ToString.Trim = "" Then
                    lbStatus.Text = "Template Must Have Value"
                    Return False
                End If

            Else
                If tbReport.Text.Trim = "" Then
                    lbStatus.Text = "Report Must Have Value"
                    tbReport.Focus()
                    Return False
                End If
                If tbTemp.Text.Trim = "" Then
                    lbStatus.Text = "Template Must Have Value"
                    tbTemp.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function


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
        'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        ' Dim dt As DataTable
        'Dim dr As DataRow
        'Dim drResult As DataRow
        'Dim MaxItem As String
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Up" Then
                Try
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridDt.Rows(index)
                    MoveRecord(GVR.Cells(1).Text, "Up")
                    'BindGridDt(ViewState("Dt"), GridDt)
                Catch ex As Exception
                End Try
            ElseIf e.CommandName = "Down" Then
                Try
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridDt.Rows(index)
                    MoveRecord(GVR.Cells(1).Text, "Down")
                    'BindGridDt(ViewState("Dt"), GridDt)
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lb = GVR.FindControl("lbLocation")
            ViewState("DtValue") = GVR.Cells(1).Text
            'If ViewState("SetLocation") Then
            '    FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name")
            '    ViewState("SetLocation") = False
            'End If
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            'EnableHd(False)
            ViewState("StateDt") = "Edit"
            ddlColumn.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToDropList(ddlColumn, Dr(0)("ColumnField").ToString)
                BindToText(tbAlias, Dr(0)("ColumnAlias").ToString)
                BindToDropList(ddlDisplay, Dr(0)("FgDisplay").ToString)
                BindToDropList(ddlSortOrder, Dr(0)("SortOrder").ToString)
                ddlSortOrder_SelectedIndexChanged(Nothing, Nothing)
                BindToDropList(ddlSortType, Dr(0)("SortType").ToString)
                BindToText(tbType, Dr(0)("Type").ToString)
                GetListFunction(tbType.Text)
                BindToDropList(ddlFunction, TrimStr(Dr(0)("ColumnFunct").ToString))
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        '   btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool

        If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
            btnGetDt.Visible = Bool And tbReportName.Text <> "" And tbTempName.Text <> ""
            btnUser.Visible = Bool And tbReportName.Text <> "" And tbTempName.Text <> ""
        Else
            btnGetDt.Visible = False
            btnUser.Visible = False
        End If
        '  btnBack.Visible = Bool
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReport.Click
        Dim ResultField As String
        Try
            If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
                Session("filter") = "select Rpt_Code, Rpt_Name, Rpt_Table from VMsReport "
            Else
                Session("Filter") = "Select DISTINCT A.Rpt_Code, A.Rpt_Name, A.Rpt_Table from VMsReport A INNER JOIN VMsReportTemplateUser B ON A.Rpt_Code = B.RptCode AND B.userId = " + QuotedStr(ViewState("UserId").ToString)
            End If

            ResultField = "Rpt_Code, Rpt_Name, Rpt_Table"
            ViewState("Sender") = "btnReport"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbReport_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReport.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
                Dt = SQLExecuteQuery("SELECT Rpt_Code, Rpt_Name, Rpt_Table FROM VMsReport WHERE Rpt_Code = '" + tbReport.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            Else
                Dt = SQLExecuteQuery("SELECT DISTINCT A.Rpt_Code, A.Rpt_Name, A.Rpt_Table FROM VMsReport A INNER JOIN VMsReportTemplateUser B ON A.Rpt_Code = B.RptCode AND B.UserId = " + QuotedStr(ViewState("UserId").ToString) + " And A.Rpt_Code = '" + tbReport.Text.Trim + "'", ViewState("DBConnection").ToString).Tables(0)
            End If
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbReport.Text = Dr("Rpt_Code").ToString
                tbReportName.Text = Dr("Rpt_Name").ToString
                tbTable.Text = Dr("Rpt_Table").ToString
                FillComboColumn()
                ClearCriteria()
            Else
                tbReport.Text = ""
                tbReportName.Text = ""
            End If

            ddlAColumn1.Enabled = True
            tbTemp.Text = ""
            tbTempName.Text = ""
            EnableCriteria()
            BindDataDt("", "")
            ddlType_SelectedIndexChanged(Nothing, Nothing)
            pnlPrint.Visible = False
            RefereshButton()
            DisplayLabelDate()
            'labelDateA1()
            'labelDateA2()
            'labelDateA3()
            'labelDateB1()
            'labelDateB2()
            'labelDateB3()
            'labelDateC1()
            'labelDateC2()
            'labelDateC3()
            tbTemp.Focus()
        Catch ex As Exception
            Throw New Exception("tb Report Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            'If CekHd() = False Then
            '    Exit Sub
            'End If
            CriteriaField = "ColumnField, ColumnType"
            Session("Filter") = "EXEC S_GetColumnType '" + tbTable.Text.Trim + "' "
            ResultField = "ColumnField, ColumnType"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub GetListFunction(ByVal Type As String)
        ddlFunction.Items.Clear()
        Dim L, LAvg, LCount, LMin, LMax, LSum As ListItem
        L = New ListItem(" - ", "")
        LAvg = New ListItem("AVG", "AVG")
        LCount = New ListItem("COUNT", "COUNT")
        LMin = New ListItem("MIN", "MIN")
        LMax = New ListItem("MAX", "MAX")
        LSum = New ListItem("SUM", "SUM")
        ddlFunction.Items.Add(L)
        If (LCase(Type.Trim) = "float") Or (LCase(Type.Trim) = "int") Then
            ddlFunction.Items.Add(LAvg)
            ddlFunction.Items.Add(LMin)
            ddlFunction.Items.Add(LMax)
            ddlFunction.Items.Add(LSum)
        Else
            ddlFunction.Items.Add(LCount)
        End If
    End Sub

    Protected Sub ddlColumn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlColumn.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindColumnType '" + tbTable.Text + "' , '" + ddlColumn.SelectedValue + "' ", ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAlias.Text = Dr("ColumnField").ToString.Replace("_", " ")
                tbType.Text = Dr("ColumnType")
                GetListFunction(tbType.Text)
            Else
                tbAlias.Text = ""
                tbType.Text = ""
            End If
            ddlCompare.SelectedValue = ""
            ddlCompare_SelectedIndexChanged(Nothing, Nothing)
            'tbTemp.Focus()
        Catch ex As Exception
            Throw New Exception("tb Report Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSortOrder.SelectedIndexChanged
        If ddlSortOrder.SelectedIndex = 0 Then
            ddlSortType.Items.Clear()
        Else
            ddlSortType.Items.Add("ASC")
            ddlSortType.Items.Add("DESC")
        End If
        ddlFunction.Focus()
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPreview.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If CekDt() = False Then
                Exit Sub
            End If
            If CekCriteria() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = "Detail column must be fill"
                Exit Sub
            End If

            ViewState("SortExpression") = Nothing
            GridResult.PageIndex = 0
            'If Request.QueryString("ContainerId").ToString <> "RptReportCreatorId" Then
            'Save Template Criteria
            SaveAll()
            'End If
            BindDataResult()

            pnlPrint.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Preview Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindDataResult()
        Dim dt As DataTable
        Dim DV As DataView
        Try
            dt = SQLExecuteQuery("EXEC S_SAGenerateReport " + QuotedStr(tbReport.Text.Trim) + ", " + QuotedStr(tbTemp.Text.Trim) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection")).Tables(0)
            DV = dt.DefaultView
            If Not ViewState("SortExpression") = Nothing Then
                DV.Sort = ViewState("SortExpression")
            End If
            GridResult.DataSource = DV
            GridResult.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Result Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridResult.PageIndexChanging
        GridResult.PageIndex = e.NewPageIndex
        BindDataResult()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'Dim PrintDetails As String
        Dim dt As DataTable
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If CekDt() = False Then
                Exit Sub
            End If
            If CekCriteria() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = "Detail column must be fill"
                Exit Sub
            End If
            'If Request.QueryString("ContainerId").ToString <> "RptReportCreatorId" Then
            'Save Template Criteria
            SaveAll()
            'End If

            'ExportFromHtmlForm(GridResult)

            dt = SQLExecuteQuery("EXEC S_SAGenerateReport " + QuotedStr(tbReport.Text.Trim) + ", " + QuotedStr(tbTemp.Text.Trim) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection")).Tables(0)

            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel()
            'ExportDbToExel(dt, tbReportName.Text + " " + tbTempName.Text)
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub ExportDbToExel(ByVal data As DataTable, ByVal fileName As String)
        Dim context As HttpContext = HttpContext.Current
        Dim datarow As DataRow
        Dim i, Column As Integer

        context.Response.Clear()

        'test bole pakai dan tidak 2 baris ini
        context.Response.Charset = System.Text.UTF8Encoding.UTF8.EncodingName.ToString()
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8")

        context.Response.ContentType = "text/csv"
        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv")
        'Write column header names
        Column = data.Columns.Count
        i = 0
        While i < Column
            If (i > 0) Then
                context.Response.Write(",")
                'context.Response.Write("\t")
            End If
            context.Response.Write(data.Columns(i).ColumnName)
            i = i + 1
        End While
        context.Response.Write(Environment.NewLine)

        'Write data
        For Each datarow In data.Rows()
            i = 0
            While i < Column
                If (i > 0) Then
                    context.Response.Write(",")
                End If
                context.Response.Write(datarow(i).ToString())
                i = i + 1
            End While
            context.Response.Write(Environment.NewLine)

        Next
        context.Response.End()
    End Sub

    Public Sub ExportGridToExcel()
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname, StrWorkname As String

        StrWorkname = Trim(tbReportName.Text + " " + tbTempName.Text)
        worksheetname = Left(StrWorkname, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + worksheetname + ".xls"
        If CekDt() = False Then
            Exit Sub
        End If
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Public Sub ExportFromHtmlForm(ByVal gv As GridView)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname, StrWorkname As String
        StrWorkname = Trim(tbReportName.Text + " " + tbTempName.Text)
        worksheetname = Left(StrWorkname, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + worksheetname + ".xls"
        If CekDt() = False Then
            Exit Sub
        End If
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        gv.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(gv)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Private Sub ClearControls(ByVal Ctrl As Control)
        Dim i As Integer

        For i = Ctrl.Controls.Count - 1 To 0 Step i - 1
            ClearControls(Ctrl.Controls(i))
        Next

        If Not TypeOf Ctrl Is TableCell Then
            If Not Ctrl.GetType().GetProperty("SelectedItem") Is Nothing Then
                Dim Lt As LiteralControl = New LiteralControl()
                Ctrl.Parent.Controls.Add(Lt)
                Try
                    Lt.Text = CType(Ctrl.GetType().GetProperty("SelectedItem").GetValue(Ctrl, Nothing), String)
                Catch

                End Try
                Ctrl.Parent.Controls.Remove(Ctrl)
            ElseIf (Ctrl.GetType().GetProperty("Text") IsNot Nothing) Then
                Dim Lt As LiteralControl = New LiteralControl()
                Ctrl.Parent.Controls.Add(Lt)
                Lt.Text = CType(Ctrl.GetType().GetProperty("Text").GetValue(Ctrl, Nothing), String)
                Ctrl.Parent.Controls.Remove(Ctrl)
            End If
        End If
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        ddlTabColumn.Enabled = (ddlType.SelectedValue <> "Common")
        ddlTabResult.Enabled = (ddlType.SelectedValue <> "Common")
        ddlTabColumn.SelectedValue = ""
        ddlTabResult.SelectedValue = ""
    End Sub

    Protected Sub ddlCompare_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompare.SelectedIndexChanged
        If ddlCompare.SelectedValue <> "" Then
            ddlColumnCompare.Enabled = True
            FillCombo(ddlColumnCompare, "EXEC S_GetColumnSameType " + QuotedStr(tbTable.Text) + ", " + QuotedStr(tbType.Text), True, "ColumnField", "ColumnField", ViewState("DBConnection"))
        Else
            ddlColumnCompare.Enabled = False
            ddlColumnCompare.SelectedIndex = 0
        End If
    End Sub

    Protected Sub ddlDisplay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDisplay.SelectedIndexChanged
        If ddlDisplay.SelectedValue = "N" Then
            ddlFunction.SelectedIndex = 0
            ddlFunction.Enabled = False
        Else
            ddlFunction.Enabled = True
        End If
    End Sub

    Sub MoveRecord(ByVal thisid As String, ByVal type As String)
        Dim drSrc, drDest As DataRow
        Dim movetoId As String
        Dim DV As DataView
        Dim tempDS As New DataSet()
        Try
            If thisid = 1 And type = "Up" Then
                Exit Sub
            End If
            If type = "Down" Then
                movetoId = CStr(CInt(thisid) + 1).Trim
            Else
                movetoId = CStr(CInt(thisid) - 1).Trim
            End If
            drSrc = ViewState("Dt").Select("ItemNo = " + thisid)(0)
            drDest = ViewState("Dt").Select("ItemNo = " + movetoId)(0)
            If Not (drSrc Is Nothing) And Not (drDest Is Nothing) Then
                'Edit this Id jadi 999
                drSrc.BeginEdit()
                drSrc("ItemNo") = 999
                drSrc.EndEdit()

                drDest.BeginEdit()
                drDest("ItemNo") = CInt(thisid)
                drDest.EndEdit()

                drSrc.BeginEdit()
                drSrc("ItemNo") = CInt(movetoId)
                drSrc.EndEdit()
                'Edit move id jadi this
                tempDS.Tables.Add(ViewState("Dt"))
                DV = tempDS.Tables(0).DefaultView
                DV.Sort = "ItemNo"
                GridDt.DataSource = DV
                GridDt.DataBind()
                'BindGridDt(ViewState("Dt"), GridDt)
            End If


        Catch ex As Exception
            lbStatus.Text = "MoveRecord Error : " + ex.ToString
        End Try

    End Sub

    Private Sub EnableCriteriaReset()
        ddlAColumn1.Enabled = True
        ddlAColumn2.Enabled = True
        ddlAColumn3.Enabled = True
        ddlBColumn1.Enabled = True
        ddlBColumn2.Enabled = True
        ddlBColumn3.Enabled = True
        ddlCColumn1.Enabled = True
        ddlCColumn2.Enabled = True
        ddlCColumn3.Enabled = True
        ddlAOperator1.Enabled = True
        ddlAOperator2.Enabled = True
        ddlAOperator3.Enabled = True
        ddlBOperator1.Enabled = True
        ddlBOperator2.Enabled = True
        ddlBOperator3.Enabled = True
        ddlCOperator1.Enabled = True
        ddlCOperator2.Enabled = True
        ddlCOperator3.Enabled = True
        ddlALogic2.Enabled = True
        ddlALogic3.Enabled = True
        ddlBLogic1.Enabled = True
        ddlBLogic2.Enabled = True
        ddlBLogic3.Enabled = True
        ddlCLogic1.Enabled = True
        ddlCLogic2.Enabled = True
        ddlCLogic3.Enabled = True
    End Sub

    Private Sub EnableCriteria()
        tbAValue1.Enabled = ddlAColumn1.SelectedValue <> ""
        If tbAValue1.Enabled = False Then
            tbAValue1.Text = ""
        End If
        ddlALogic2.Enabled = ddlAColumn1.SelectedValue <> "" And tbAValue1.Text <> ""
        If ddlALogic2.Enabled = False Then
            ddlALogic2.SelectedValue = ""
        End If
        ddlAColumn2.Enabled = ddlALogic2.SelectedValue <> ""
        If ddlAColumn2.Enabled = False Then
            ddlAColumn2.SelectedValue = ""
        End If
        tbAValue2.Enabled = ddlAColumn2.SelectedValue <> ""
        If tbAValue2.Enabled = False Then
            tbAValue2.Text = ""
        End If
        ddlALogic3.Enabled = ddlAColumn2.SelectedValue <> "" And tbAValue2.Text <> ""
        If ddlALogic3.Enabled = False Then
            ddlALogic3.SelectedValue = ""
        End If
        ddlAColumn3.Enabled = ddlALogic3.SelectedValue <> ""
        If ddlAColumn3.Enabled = False Then
            ddlAColumn3.SelectedValue = ""
        End If
        tbAValue3.Enabled = ddlAColumn3.SelectedValue <> ""
        If tbAValue3.Enabled = False Then
            tbAValue3.Text = ""
        End If
        ddlBLogic1.Enabled = ddlAColumn1.SelectedValue <> "" And tbAValue1.Text <> ""
        If ddlBLogic1.Enabled = False Then
            ddlBLogic1.SelectedValue = ""
        End If
        ddlBColumn1.Enabled = ddlBLogic1.SelectedValue <> ""
        If ddlBColumn1.Enabled = False Then
            ddlBColumn1.SelectedValue = ""
        End If

        tbBValue1.Enabled = ddlBColumn1.SelectedValue <> ""
        If tbBValue1.Enabled = False Then
            tbBValue1.Text = ""
        End If
        ddlBLogic2.Enabled = ddlBColumn1.SelectedValue <> "" And tbBValue1.Text <> ""
        If ddlBLogic2.Enabled = False Then
            ddlBLogic2.SelectedValue = ""
        End If
        ddlBColumn2.Enabled = ddlBLogic2.SelectedValue <> ""
        If ddlBColumn2.Enabled = False Then
            ddlBColumn2.SelectedValue = ""
        End If
        tbBValue2.Enabled = ddlBColumn2.SelectedValue <> ""
        If tbBValue2.Enabled = False Then
            tbBValue2.Text = ""
        End If
        ddlBLogic3.Enabled = ddlBColumn2.SelectedValue <> "" And tbBValue2.Text <> ""
        If ddlBLogic3.Enabled = False Then
            ddlBLogic3.SelectedValue = ""
        End If
        ddlBColumn3.Enabled = ddlBLogic3.SelectedValue <> ""
        If ddlBColumn3.Enabled = False Then
            ddlBColumn3.SelectedValue = ""
        End If
        tbBValue3.Enabled = ddlBColumn3.SelectedValue <> ""
        If tbBValue3.Enabled = False Then
            tbBValue3.Text = ""
        End If
        ddlCLogic1.Enabled = ddlBColumn1.SelectedValue <> "" And tbBValue1.Text <> ""
        If ddlCLogic1.Enabled = False Then
            ddlCLogic1.SelectedValue = ""
        End If
        ddlCColumn1.Enabled = ddlCLogic1.SelectedValue <> ""
        If ddlCColumn1.Enabled = False Then
            ddlCColumn1.SelectedValue = ""
        End If
        tbCValue1.Enabled = ddlCColumn1.SelectedValue <> ""
        If tbCValue1.Enabled = False Then
            tbCValue1.Text = ""
        End If
        ddlCLogic2.Enabled = ddlCColumn1.SelectedValue <> "" 'And tbCValue1.Text <> ""
        If ddlCLogic2.Enabled = False Then
            ddlCLogic2.SelectedValue = ""
        End If
        ddlCColumn2.Enabled = ddlCLogic2.SelectedValue <> ""
        If ddlCColumn2.Enabled = False Then
            ddlCColumn2.SelectedValue = ""
        End If
        tbCValue2.Enabled = ddlCColumn2.SelectedValue <> ""
        If tbCValue2.Enabled = False Then
            tbCValue2.Text = ""
        End If
        ddlCLogic3.Enabled = ddlCColumn2.SelectedValue <> "" 'And tbCValue2.Text <> ""
        If ddlCLogic3.Enabled = False Then
            ddlCLogic3.SelectedValue = ""
        End If
        ddlCColumn3.Enabled = ddlCLogic3.SelectedValue <> ""
        If ddlCColumn3.Enabled = False Then
            ddlCColumn3.SelectedValue = ""
        End If
        tbCValue3.Enabled = ddlCColumn3.SelectedValue <> ""
        If tbCValue3.Enabled = False Then
            tbCValue3.Text = ""
        End If
        ddlAOperator1.Enabled = tbAValue1.Enabled
        ddlAOperator2.Enabled = tbAValue2.Enabled
        ddlAOperator3.Enabled = tbAValue3.Enabled

        ddlBOperator1.Enabled = tbBValue1.Enabled
        ddlBOperator2.Enabled = tbBValue2.Enabled
        ddlBOperator3.Enabled = tbBValue3.Enabled

        ddlCOperator1.Enabled = tbCValue1.Enabled
        ddlCOperator2.Enabled = tbCValue2.Enabled
        ddlCOperator3.Enabled = tbCValue3.Enabled
    End Sub

    Protected Sub ChangeCombo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAColumn1.SelectedIndexChanged, ddlAColumn2.SelectedIndexChanged, ddlAColumn3.SelectedIndexChanged, ddlBColumn1.SelectedIndexChanged, ddlBColumn2.SelectedIndexChanged, ddlBColumn3.SelectedIndexChanged, ddlCColumn1.SelectedIndexChanged, ddlCColumn2.SelectedIndexChanged, ddlCColumn3.SelectedIndexChanged, ddlALogic2.SelectedIndexChanged, ddlALogic3.SelectedIndexChanged, ddlBLogic1.SelectedIndexChanged, ddlBLogic2.SelectedIndexChanged, ddlBLogic3.SelectedIndexChanged, ddlCLogic1.SelectedIndexChanged, ddlCLogic2.SelectedIndexChanged, ddlCLogic3.SelectedIndexChanged
        '''If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
        EnableCriteria()
        '''End If
        DisplayLabelDate()
        'labelDateA1()
        'labelDateA2()
        'labelDateA3()
        'labelDateB1()
        'labelDateB2()
        'labelDateB3()
        'labelDateC1()
        'labelDateC2()
        'labelDateC3()
    End Sub

    Protected Sub ChangeText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAValue1.TextChanged, tbAValue2.TextChanged, tbAValue3.TextChanged, tbBValue1.TextChanged, tbBValue2.TextChanged, tbBValue3.TextChanged, tbCValue1.TextChanged, tbCValue2.TextChanged, tbCValue3.TextChanged
        '''If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
        EnableCriteria()
        '''End If
    End Sub

    Private Function CekCriteria() As Boolean
        If ddlAColumn1.SelectedValue <> "" And tbAValue1.Text.Trim = "" Then
            lbStatus.Text = "Field Value 1 must have value"
            tbAValue1.Focus()
            Return False
        End If
        If ddlAColumn2.SelectedValue <> "" And tbAValue2.Text.Trim = "" Then
            lbStatus.Text = "Field Value 2 must have value"
            tbAValue2.Focus()
            Return False
        End If
        If ddlAColumn3.SelectedValue <> "" And tbAValue3.Text.Trim = "" Then
            lbStatus.Text = "Field Value 3 must have value"
            tbAValue3.Focus()
            Return False
        End If
        If ddlBColumn1.SelectedValue <> "" And tbBValue1.Text.Trim = "" Then
            lbStatus.Text = "Field Value 1 must have value"
            tbBValue1.Focus()
            Return False
        End If
        If ddlBColumn2.SelectedValue <> "" And tbBValue2.Text.Trim = "" Then
            lbStatus.Text = "Field Value 2 must have value"
            tbBValue2.Focus()
            Return False
        End If
        If ddlBColumn3.SelectedValue <> "" And tbBValue3.Text.Trim = "" Then
            lbStatus.Text = "Field Value 3 must have value"
            tbBValue3.Focus()
            Return False
        End If
        If ddlCColumn1.SelectedValue <> "" And tbCValue1.Text.Trim = "" Then
            lbStatus.Text = "Field Value 1 must have value"
            tbCValue1.Focus()
            Return False
        End If
        If ddlCColumn2.SelectedValue <> "" And tbCValue2.Text.Trim = "" Then
            lbStatus.Text = "Field Value 2 must have value"
            tbCValue2.Focus()
            Return False
        End If
        If ddlCColumn3.SelectedValue <> "" And tbCValue3.Text.Trim = "" Then
            lbStatus.Text = "Field Value 3 must have value"
            tbCValue3.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub SaveDt()
        Dim SQLString As String
        Try
            'execute sp hapus data Dt
            SQLString = "EXEC S_MsReportTemplateDtDelete " + QuotedStr(tbReport.Text) + "," + QuotedStr(tbTemp.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))

            Dim Row As DataRow()
            Row = ViewState("Dt").Select("RptCode IS NOT NULL")
            'Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
            Dim GVR As GridViewRow = Nothing
            For I = 0 To Row.Length - 1
                SQLString = "EXEC S_MsReportTemplateDtUpdate " + QuotedStr(TrimStr(Row(I)("RptCode").ToString)) + "," + QuotedStr(TrimStr(Row(I)("Template").ToString)) + ", " + Row(I)("ItemNo").ToString + ", " + QuotedStr(TrimStr(Row(I)("ColumnField").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("ColumnAlias").ToString)) + ", " + _
                QuotedStr(TrimStr(Row(I)("FgDisplay").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("SortOrder").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("SortType").ToString)) + ", " + _
                QuotedStr(TrimStr(Row(I)("Type").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("ColumnFunct").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("CompareOperator").ToString)) + ", " + QuotedStr(TrimStr(Row(I)("CompareField").ToString))
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            Next
            BindGridDt(ViewState("Dt"), GridDt)

            BindDataDt(tbReport.Text, tbTemp.Text)
        Catch ex As Exception
            lbStatus.Text = "SaveDt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub DisableCriteriaUser()
        ddlAColumn1.Enabled = False
        ddlAColumn2.Enabled = False
        ddlAColumn3.Enabled = False 'And tbAValue3.Text = ""
        ddlBColumn1.Enabled = False
        ddlBColumn2.Enabled = False
        ddlBColumn3.Enabled = False
        ddlCColumn1.Enabled = False
        ddlCColumn2.Enabled = False
        ddlCColumn3.Enabled = False
        ddlALogic2.Enabled = False
        ddlALogic3.Enabled = False
        ddlBLogic1.Enabled = False
        ddlBLogic2.Enabled = False
        ddlBLogic3.Enabled = False
        ddlCLogic1.Enabled = False
        ddlCLogic2.Enabled = False
        ddlCLogic3.Enabled = False
    End Sub

    Private Sub DisplayLabelDate()
        Try
            If ddlAColumn1.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlAColumn1.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbAColumn1T.Text = "yyyymmdd"
                Else
                    lbAColumn1T.Text = ""
                End If
            End If
            If ddlAColumn2.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlAColumn2.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbAColumn2T.Text = "yyyymmdd"
                Else
                    lbAColumn2T.Text = ""
                End If
            End If
            If ddlAColumn3.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlAColumn3.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbAColumn3T.Text = "yyyymmdd"
                Else
                    lbAColumn3T.Text = ""
                End If
            End If

            If ddlBColumn1.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlBColumn1.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbBColumn1T.Text = "yyyymmdd"
                Else
                    lbBColumn1T.Text = ""
                End If
            End If
            If ddlBColumn2.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlBColumn2.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbBColumn2T.Text = "yyyymmdd"
                Else
                    lbBColumn2T.Text = ""
                End If
            End If
            If ddlBColumn3.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlBColumn3.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbBColumn3T.Text = "yyyymmdd"
                Else
                    lbBColumn3T.Text = ""
                End If
            End If

            If ddlCColumn1.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlCColumn1.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbCColumn1T.Text = "yyyymmdd"
                Else
                    lbCColumn1T.Text = ""
                End If
            End If
            If ddlCColumn2.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlCColumn2.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbCColumn2T.Text = "yyyymmdd"
                Else
                    lbCColumn2T.Text = ""
                End If
            End If
            If ddlCColumn3.SelectedValue <> "" Then
                ddlColumnType.SelectedValue = ddlCColumn3.SelectedValue
                If UCase(TrimStr(ddlColumnType.SelectedItem.Text)) = "DATETIME " Then
                    lbCColumn3T.Text = "yyyymmdd"
                Else
                    lbCColumn3T.Text = ""
                End If
            End If
            lbAColumn1T.Visible = ddlAColumn1.SelectedValue <> ""
            lbAColumn2T.Visible = ddlAColumn2.SelectedValue <> ""
            lbAColumn3T.Visible = ddlAColumn3.SelectedValue <> ""
            lbBColumn1T.Visible = ddlBColumn1.SelectedValue <> ""
            lbBColumn2T.Visible = ddlBColumn2.SelectedValue <> ""
            lbBColumn3T.Visible = ddlBColumn3.SelectedValue <> ""
            lbCColumn1T.Visible = ddlCColumn1.SelectedValue <> ""
            lbCColumn2T.Visible = ddlCColumn2.SelectedValue <> ""
            lbCColumn3T.Visible = ddlCColumn3.SelectedValue <> ""
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUser.Click
        Dim paramgo As String
        Dim GVR As GridViewRow = Nothing
        Try
            'If Request.QueryString("ContainerId").ToString = "RptReportCreatorId" Then
            If CekDt() = False Then
                Exit Sub
            End If

            'End If

            ' GVR = GridView1.Rows(CInt(e.CommandArgument))
            paramgo = tbReport.Text + "-" + tbTemp.Text + "|" + tbReportName.Text + " " + tbTempName.Text
            'paramgo = Session("UserId").ToString + "|" + Session("UserName").ToString
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Report', '" + Request.QueryString("KeyId") + "', '" + paramgo + "','AssMsReportUser');", True)
            End If
        Catch ex As Exception
            lbStatus.Text = "btnUser Clik " + ex.ToString
        End Try
    End Sub

    Protected Sub GridResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridResult.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindDataResult()
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTempName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTempName.TextChanged
        RefereshButton()
    End Sub
End Class
