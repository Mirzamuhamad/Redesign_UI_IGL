Imports System.Data
Partial Class MsJobPlant_MsJobPlant
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlLevelPlant, "SELECT LevelPlantCode, LevelPlantName FROM V_MsLevelPlant", True, "LevelPlantCode", "LevelPlantName", ViewState("DBConnection"))
            FillCombo(ddlJobGroupPlant, "SELECT Job_Group, Job_Group_Name FROM V_MsJobPlantGroup", True, "Job_Group", "Job_Group_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitAreal, "SELECT UnitCode, UnitName FROM V_MsUnitAreal", True, "UnitCode", "UnitName", ViewState("DBConnection"))
            FillCombo(ddlUnit, "SELECT UnitCode, UnitName FROM MsUnit", True, "UnitCode", "UnitName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

            'tbKepadatan.Attributes.Add("OnKeyDown", "return PressNumeric();")

        End If
        If Not Session("Result") Is Nothing Then

            If ViewState("Sender") = "btnAccExpense" Then
                tbAccExpense.Text = Session("Result")(0).ToString
                tbAccExpenseName.Text = Session("Result")(1).ToString
            End If
            If ViewState("Sender") = "btnAccAsset" Then
                tbAccAsset.Text = Session("Result")(0).ToString
                tbAccAssetName.Text = Session("Result")(1).ToString
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing

        End If
        lstatus.Text = ""
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function


    Private Sub ClearInput()
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If

            tbName.Text = ""
            ddlLevelPlant.SelectedIndex = 0
            ddlJobGroupPlant.SelectedIndex = 0
            tbJobNo.Text = ""
            ddlUnitAreal.SelectedIndex = 0
            ddlUnitConvert.SelectedIndex = 0
            ddlUnit.SelectedIndex = 0
            tbAccExpense.Text = ""
            tbAccExpenseName.Text = ""
            tbAccAsset.Text = ""
            tbAccAssetName.Text = ""
            ddlFgUsedMaterial.SelectedIndex = 1
            ddlFgUsedMachine.SelectedIndex = 1
            ddlActive.SelectedIndex = 0
            ddlFgCIP.SelectedIndex = 1
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal JobCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM V_MsJobPlantView  WHERE JobCode = " + QuotedStr(JobCode)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbCode, DT.Rows(0)("JobCode").ToString)
            BindToText(tbName, DT.Rows(0)("JobName").ToString)
            ddlLevelPlant.SelectedIndex = 0
            ddlJobGroupPlant.SelectedIndex = 0
            ddlUnitAreal.SelectedIndex = 0
            ddlUnit.SelectedIndex = 0
            BindToDropList(ddlLevelPlant, DT.Rows(0)("LevelPlant").ToString)
            BindToDropList(ddlJobGroupPlant, DT.Rows(0)("JobGroupPlant").ToString)
            BindToText(tbJobNo, DT.Rows(0)("JobNo").ToString)
            BindToDropList(ddlUnitAreal, DT.Rows(0)("UnitAreal").ToString)
            BindToDropList(ddlUnit, DT.Rows(0)("Unit").ToString)
            FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            BindToDropList(ddlUnitConvert, DT.Rows(0)("UnitConvert").ToString)
            BindToText(tbAccExpense, DT.Rows(0)("AccExpense").ToString)
            BindToText(tbAccExpenseName, DT.Rows(0)("AccExpenseName").ToString)
            BindToText(tbAccAsset, DT.Rows(0)("AccAsset").ToString)
            BindToText(tbAccAssetName, DT.Rows(0)("AccAssetName").ToString)
            BindToDropList(ddlFgCIP, DT.Rows(0)("FgCIP").ToString)
            BindToDropList(ddlFgUsedMaterial, DT.Rows(0)("FgUsedMaterial").ToString)
            BindToDropList(ddlFgUsedMachine, DT.Rows(0)("FgUsedMachine").ToString)
            BindToDropList(ddlActive, DT.Rows(0)("FgActive").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * From V_MsJobPlantView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "JobCode ASC"
                ViewState("SortOrder") = "ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim dt As DataTable
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count = 0 Then
                lstatus.Text = "No Data"
                DataGrid.Visible = False
                btnAdd2.Visible = False
            Else
                DataGrid.Visible = True
                btnAdd2.Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            ddlField2.SelectedValue.Replace("JobCode", "Job_Code")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsJobPlantView','JobCode','JobName','JobGroupPlantName','Unit','AccExpense','AccAsset','Job Plantation File','Code','Description','Job Group','Unit Norma','Acc. Exp','Acc. FA'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(pnlHd, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    ViewState("State") = "View"
                    ModifyInput(False, pnlInput)
                    tbCode.Enabled = False
                    btnHome.Visible = True
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    btnCancel.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    MovePanel(pnlHd, pnlInput)
                    FillTextBox(GVR.Cells(1).Text)
                    ViewState("State") = "Edit"
                    ModifyInput(True, pnlInput)
                    tbCode.Enabled = False
                    btnHome.Visible = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                    btnCancel.Visible = True
                    tbName.Focus()
                ElseIf DDL.SelectedValue = "Non Active" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        If GVR.Cells(8).Text = "N" Then
                            lstatus.Text = "<script language='javascript'> {alert('Job Plantation closed already')}</script>"
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("UPDATE MsJobPlant SET Fgactive = 'N' WHERE JobCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("DELETE MsJobPlant WHERE JobCode = '" & GVR.Cells(1).Text & "' ", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_RowCommand Delete Error = " + ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbCode.Enabled = True
            FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ClearInput()
            ModifyInput(True, pnlInput)
            BtnSave.Visible = True
            btnReset.Visible = True
            btnCancel.Visible = True
            ddlActive.Enabled = False
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Code must be filled.")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job Name must be filled.")
                tbName.Focus()
                Return False
            End If
            If ddlLevelPlant.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Level Plantation must be filled.")
                ddlLevelPlant.Focus()
                Return False
            End If
            If ddlJobGroupPlant.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg(" Job Group must be filled.")
                ddlJobGroupPlant.Focus()
                Return False
            End If
            If tbJobNo.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Job No must be filled.")
                tbJobNo.Focus()
                Return False
            End If
            If ddlUnitAreal.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Unit Areal must be filled.")
                ddlUnitAreal.Focus()
                Return False
            End If
            If ddlUnit.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Unit Norma must be filled.")
                ddlUnit.Focus()
                Return False
            End If
            If ddlUnitConvert.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Unit Dosis must be filled.")
                ddlUnitConvert.Focus()
                Return False
            End If
            If tbAccExpense.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Acc Expense must be filled.")
                tbAccExpense.Focus()
                Return False
            End If
            If tbAccAsset.Text.Trim.Length < 0 Then
                lstatus.Text = MessageDlg("Acc Asset must be filled.")
                tbAccAsset.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT JobCode FROM V_MsJobPlantView WHERE JobCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Job Code " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsJobPlant (JobCode, JobName, LevelPlant, JobGroupPlant, JobNo,UnitAreal, Unit, UnitConvert, AccExpense, AccAsset, FgCIP, FgUsedMaterial, FgUsedMachine, FgActive, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlLevelPlant.Text) + ", " + QuotedStr(ddlJobGroupPlant.Text) + ", " & _
                QuotedStr(tbJobNo.Text) + ", " & _
                QuotedStr(ddlUnitAreal.Text) + ", " + _
                QuotedStr(ddlUnit.Text) + ", " + _
                QuotedStr(ddlUnitConvert.Text) + ", " + _
                QuotedStr(tbAccExpense.Text) + ", " + _
                QuotedStr(tbAccAsset.Text) + ", " + _
                QuotedStr(ddlFgCIP.SelectedValue) + ", " + _
                QuotedStr(ddlFgUsedMaterial.SelectedValue) + ", " + _
                QuotedStr(ddlFgUsedMachine.SelectedValue) + ", " + _
                QuotedStr(ddlActive.SelectedValue) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                SqlString = "UPDATE MsJobPlant SET JobName= " + QuotedStr(tbName.Text) & _
                            ", LevelPlant = " + QuotedStr(ddlLevelPlant.Text) & _
                            ", JobGroupPlant = " + QuotedStr(ddlJobGroupPlant.Text) & _
                            ", JobNo = " + QuotedStr(tbJobNo.Text) & _
                            ", UnitAreal = " + QuotedStr(ddlUnitAreal.Text) & _
                            ", Unit = " + QuotedStr(ddlUnit.Text) & _
                            ", UnitConvert = " + QuotedStr(ddlUnitConvert.Text) & _
                            ", AccExpense = " + QuotedStr(tbAccExpense.Text) & _
                            ", AccAsset = " + QuotedStr(tbAccAsset.Text) & _
                            ", FgCIP = " + QuotedStr(ddlFgCIP.SelectedValue) & _
                            ", FgUsedMaterial = " + QuotedStr(ddlFgUsedMaterial.SelectedValue) & _
                            ", FgUsedMachine = " + QuotedStr(ddlFgUsedMachine.SelectedValue) & _
                            ", FgActive = " + QuotedStr(ddlActive.SelectedValue) & _
                            " WHERE JobCode = " + QuotedStr(tbCode.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccExpense_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccExpense.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Account, Description from V_MsAccount WHERE FgActive = 'Y' "
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccExpense"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc Expense Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAccAsset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAsset.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select Account, Description from V_MsAccount WHERE FgActive = 'Y' "
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccAsset"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc Asset Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccExpense_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccExpense.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Account, Description from V_MsAccount WHERE FgActive = 'Y' AND Account = " + QuotedStr(tbAccExpense.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccExpense.Text = ""
                tbAccExpenseName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccExpense.Text = dr("Account").ToString
                tbAccExpenseName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccExpense_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccAsset_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccAsset.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select Account, Description from V_MsAccount WHERE FgActive = 'Y' AND Account = " + QuotedStr(tbAccAsset.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccAsset.Text = ""
                tbAccAssetName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccAsset.Text = dr("Account").ToString
                tbAccAssetName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccAsset_TextChanged Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnit.SelectedIndexChanged
        Try
            FillCombo(ddlUnitConvert, "SELECT UnitCode, UnitName from MsUnit WHERE unitcode in ( Select X.UnitCode from V_MsUnitAreal X ) OR unitcode = " + QuotedStr(ddlUnit.SelectedValue), False, "UnitCode", "UnitName", ViewState("DBConnection"))
            ddlUnitConvert.SelectedValue = ddlUnit.SelectedValue
        Catch ex As Exception
            lstatus.Text = "tbAccAsset_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Home Error : " + ex.ToString
        End Try
    End Sub
End Class
