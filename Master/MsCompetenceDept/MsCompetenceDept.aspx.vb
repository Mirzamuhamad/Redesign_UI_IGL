Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_MsCompetenceDept_MsCompetenceDept
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            FillCombo(ddlComptenceType, "SELECT * FROM VMsBobotPA", True, "CompetenceType", "CompetenceType", ViewState("DBConnection"))
            FillCombo(ddlJobLevelStart, "SELECT * FROM VMsJobLevel", True, "Job_Level", "Job_Level_Name", ViewState("DBConnection"))
            FillCombo(ddlJobLevelEnd, "SELECT * FROM VMsJobLevel ", True, "Job_Level", "Job_Level_Name", ViewState("DBConnection"))
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            bindDataGrid()
            If MultiView1.ActiveViewIndex = 0 Then
                bindDataGrid()
            End If
            Session("AdvanceFilter") = ""

        End If
        If Not Session("BackSchedule") Is Nothing Then
            dsAccClass.ConnectionString = ViewState("DBConnection")
            MultiView1.ActiveViewIndex = 0
            Session("BackSchedule") = Nothing
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnDept" Then
                tbDeptCode.Text = Session("Result")(0).ToString
                tbDeptName.Text = Session("Result")(1).ToString
            End If
            
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If

        dsAccClass.ConnectionString = ViewState("DBConnection")
        tbDeptName.Attributes.Add("Readonly", "True")
        tbCode.Attributes.Add("Readonly", "True")
        tbName.Attributes.Add("Readonly", "True")
        tbNo.Attributes.Add("ReadOnly", "True")
        tbPriority.Attributes.Add("OnKeyDown", "return PressNumeric();")
        VisibleGrid()

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
            'If ViewState("FgInsert") = "N" Then
            'lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            'Return False
            'Exit Function
            'End If
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

    Private Sub bindDataGrid()
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            SqlString = "EXEC S_MsCompetenceDeptGetData " + QuotedStr(tbDeptCode.Text) + ", " + QuotedStr(ddlComptenceType.SelectedValue)
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))
            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
            Else
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex

        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim index As Integer
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing

        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                Dim lbCompetenceCode, lbItemNo As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCompetenceCode = GVR.FindControl("CompetenceCode")
                lbItemNo = GVR.FindControl("ItemNo")
                If DDL.SelectedValue = "View" Then
                    Panel5.Visible = False
                    pnlView.Visible = True
                    PanelHd.Enabled = False
                    Menu1.Enabled = False
                    ViewState("StateHd") = "View"
                    'FillTextBox(lbCompetenceCode.Text, lbItemNo.Text)

                    Dim Code, Name, No, Desc1, Desc2, Type, AllJob, StartJob, EndJob, Priority, Input As Label

                    Code = GVR.FindControl("CompetenceCode")
                    Name = GVR.FindControl("CompetenceName")
                    No = GVR.FindControl("ItemNo")
                    Desc1 = GVR.FindControl("Description1")
                    Desc2 = GVR.FindControl("Description2")
                    Type = GVR.FindControl("Type")
                    AllJob = GVR.FindControl("AllJobLevel")
                    StartJob = GVR.FindControl("StartJobLevel")
                    EndJob = GVR.FindControl("EndJobLevel")
                    Priority = GVR.FindControl("Priority")
                    Input = GVR.FindControl("Input")

                    tbCode.Text = Code.Text
                    tbName.Text = Name.Text
                    tbNo.Text = No.Text
                    tbDescription1.Text = Desc1.Text
                    tbDescription2.Text = Desc2.Text
                    ddlType.SelectedValue = Type.Text
                    ddlAllJobLevel.Text = AllJob.Text
                    ddlJobLevelStart.Text = StartJob.Text
                    ddlJobLevelEnd.Text = EndJob.Text
                    tbPriority.Text = Priority.Text
                    ddlInput.SelectedValue = Input.Text

                    ModifyInput(False)
                    btnSaveHd.Visible = False
                    btnCancelHd.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    Panel5.Visible = False
                    pnlView.Visible = True
                    PanelHd.Enabled = False
                    Menu1.Enabled = False
                    'FillTextBox(lbCompetenceCode.Text, lbItemNo.Text)

                    Dim Code, Name, No, Desc1, Desc2, Type, AllJob, StartJob, EndJob, Priority, Input As Label

                    Code = GVR.FindControl("CompetenceCode")
                    Name = GVR.FindControl("CompetenceName")
                    No = GVR.FindControl("ItemNo")
                    Desc1 = GVR.FindControl("Description1")
                    Desc2 = GVR.FindControl("Description2")
                    Type = GVR.FindControl("Type")
                    AllJob = GVR.FindControl("AllJobLevel")
                    StartJob = GVR.FindControl("StartJobLevel")
                    EndJob = GVR.FindControl("EndJobLevel")
                    Priority = GVR.FindControl("Priority")
                    Input = GVR.FindControl("Input")
                    
                    tbCode.Text = Code.Text
                    tbName.Text = Name.Text
                    tbNo.Text = No.Text
                    tbDescription1.Text = Desc1.Text
                    tbDescription2.Text = Desc2.Text
                    ddlType.SelectedValue = Type.Text
                    ddlAllJobLevel.Text = AllJob.Text
                    ddlJobLevelStart.Text = StartJob.Text
                    ddlJobLevelEnd.Text = EndJob.Text
                    tbPriority.Text = Priority.Text
                    ddlInput.SelectedValue = Input.Text

                    ViewState("StateHd") = "Edit"
                    ModifyInput(True)
                    JobLevelChange()
                    bindDataGrid()
                    btnSaveHd.Visible = True
                    btnCancelHd.Visible = True
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        'Dim txtID As Label
        'Try
        'If CheckMenuLevel("Delete") = False Then
        'Exit Sub
        'End If
        'txtID = DataGridOnExp.Rows(e.RowIndex).FindControl("OnExpAccClass")

        'Dim dr() As DataRow
        'dr = ViewState("DtOnExp").Select("AccClass = " + txtID.Text + " AND [Year] = " + ddlYear.SelectedValue)
        'dr(0).Delete()
        'SQLExecuteNonQuery("DELETE FROM MsAsumsiBaseOnExpense where AccClass = " + txtID.Text + " AND Year = " + ddlYear.SelectedValue, Session("DBConnection").ToString)

        'bindDataGridOnExp()
        'Catch ex As Exception
        'lstatus.Text = "DataGridOnExp_RowDeleting Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        'Dim obj As GridViewRow
        'Dim txt As TextBox
        'Try
        '    If CheckMenuLevel("Edit") = False Then
        '        Exit Sub
        '    End If
        '    DataGrid.EditIndex = e.NewEditIndex
        '    DataGrid.ShowFooter = False
        '    bindDataGrid()
        '    obj = DataGrid.Rows(e.NewEditIndex)
        '    txt = obj.FindControl("AllJobLevelEdit")
        '    txt.Focus()

        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_RowEditing exception : " + ex.ToString
        'End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbStartJobLevel, dbEndJobLevel, dbPriority As TextBox
        'Dim ddlJobLevel, ddlInput As DropDownList
        'Dim lbCompetence, lbItemNo As Label
        'Try
        '    dbStartJobLevel = DataGrid.Rows(e.RowIndex).FindControl("StartJobLevelEdit")
        '    dbEndJobLevel = DataGrid.Rows(e.RowIndex).FindControl("EndJobLevelEdit")
        '    dbPriority = DataGrid.Rows(e.RowIndex).FindControl("PriorityEdit")
        '    ddlJobLevel = DataGrid.Rows(e.RowIndex).FindControl("AllJobLevelEdit")
        '    ddlInput = DataGrid.Rows(e.RowIndex).FindControl("InputEdit")
        '    lbCompetence = DataGrid.Rows(e.RowIndex).FindControl("CompetenceCodeEdit")
        '    lbItemNo = DataGrid.Rows(e.RowIndex).FindControl("ItemNoEdit")

        '    If IsNumeric(dbPriority.Text.Replace(",", "")) = 0 Then
        '        lstatus.Text = MessageDlg("Priority must be in numeric.")
        '        dbPriority.Focus()
        '        Exit Sub
        '    End If

        '    Dim dt As New DataTable
        '    Dim Cek As String
        '    Cek = "SELECT ItemNo, Dept_Code, Dept_Name, Dept_Level FROM VMsDepartment Order By ItemNo"
        '    dt = SQLExecuteQuery(Cek, ViewState("DBConnection")).Tables(0)

        '    If dt.Rows.Count > 0 Then
        '        SQLString = "UPDATE MsCompetenceDept SET AllJobLevel = " + QuotedStr(ddlJobLevel.SelectedValue) + _
        '        ", StartJobLevel = " + QuotedStr(dbStartJobLevel.Text) + _
        '        ", EndJobLevel = " + QuotedStr(dbEndJobLevel.Text) + _
        '        ", Priority = " + dbPriority.Text.Replace(",", "") + _
        '        " WHERE Department = " + QuotedStr(tbDeptCode.Text) + " AND CompetenceType =  " + QuotedStr(ddlComptenceType.SelectedValue) + " AND CompetenceCode = " + QuotedStr(lbCompetence.Text) + " AND ItemNo = " + lbItemNo.Text
        '        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        '    Else
        '        SQLString = "INSERT INTO MsCompetenceDept (Department, CompetenceType, CompetenceCode, ItemNo, Priority, FgAllJobLevel, StartobLevel, EndJobLevel, UserId, UserDate) " + _
        '        "SELECT " + QuotedStr(tbDeptCode.Text) + ", " + QuotedStr(ddlComptenceType.SelectedValue) + ", " + QuotedStr(lbCompetence.Text) + ", " + lbItemNo.Text + ", " + dbPriority.Text + ", " + QuotedStr(ddlJobLevel.SelectedValue) + " , " + QuotedStr(dbStartJobLevel.Text) + ", " + QuotedStr(dbEndJobLevel.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
        '        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        '    End If


        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()
        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGridOnExp_RowUpdating Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGridOnExp_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 0 Then
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BindGridDetail(ByVal source As DataTable, ByVal gv As GridView)
        'Dim IsEmpty As Boolean
        Dim DV As DataView
        'Dim DtTemp As DataTable
        'Dim dr As DataRow()
        Try
            DV = source.DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = source
                ShowGridViewIfEmpty(DT, gv)
            Else
                'DV.Sort = SortExpression
                gv.DataSource = DV
                gv.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
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
            lstatus.Text = "DataGridOnExp_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDept.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Dept_Code, Dept_Name FROM VMsDepartment "
            ResultField = "Dept_Code, Dept_Name"
            ViewState("Sender") = "btnDept"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnDept_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDeptCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDeptCode.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Dim Cek As String
        Try
            
            Cek = "SELECT Dept_Code, Dept_Name FROM VMsDepartment WHERE Dept_Code = " + QuotedStr(tbDeptCode.Text)
            Ds = SQLExecuteQuery(Cek, ViewState("DBConnection").ToString)


            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbDeptCode.Text = Dr("Dept_Code")
                tbDeptName.Text = Dr("Dept_Name")
            Else
                tbDeptCode.Text = ""
                tbDeptName.Text = ""
            End If
            VisibleGrid()
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("tb Product Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub VisibleGrid()
        Try
            If (tbDeptCode.Text.Trim = "") Or (ddlComptenceType.SelectedValue.Trim = "") Then
                Panel5.Visible = False
            Else
                Panel5.Visible = True
            End If
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlComptenceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlComptenceType.SelectedIndexChanged
        Try

            VisibleGrid()
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            pnlView.Visible = False
            Panel5.Visible = True
            PanelHd.Enabled = True
            Menu1.Enabled = True
        Catch ex As Exception
            lstatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelHd.Click
        Try
            pnlView.Visible = False
            Panel5.Visible = True
            PanelHd.Enabled = True
            Menu1.Enabled = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal CompetenceCode As String, ByVal ItemNo As String)
        Dim Ds As DataSet
        Dim Dr As DataRow
        Dim SQL As String
        Try
            SQL = "EXEC S_MsCompetenceDeptGetData " + QuotedStr(tbDeptCode.Text) + ", " + QuotedStr(ddlComptenceType.SelectedValue)
            Ds = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString)

            If Ds.Tables(0).Rows.Count <> 0 Then
                Dr = Ds.Tables(0).Rows(0)
                'lstatus.Text = Dr("CompetenceCode") + " - " + Dr("ItemNo")
                'Exit Sub

                If (Dr("CompetenceCode") = CompetenceCode) And (Dr("ItemNo") = ItemNo) Then
                    tbCode.Text = CompetenceCode
                    BindToText(tbName, Dr("CompetenceName").ToString)
                    BindToText(tbNo, Dr("ItemNo").ToString)
                    BindToText(tbDescription1, Dr("Description1").ToString)
                    BindToText(tbDescription2, Dr("Description2").ToString)
                    BindToDropList(ddlType, Dr("Type").ToString)
                    BindToDropList(ddlAllJobLevel, Dr("AllJobLevel").ToString)
                    BindToDropList(ddlJobLevelStart, Dr("StartJobLevel").ToString)
                    BindToDropList(ddlJobLevelEnd, Dr("EndJobLevel").ToString)
                    BindToText(tbPriority, Dr("Priority").ToString)
                    BindToDropList(ddlInput, Dr("FgInput").ToString)
                Else
                    tbCode.Text = ""
                    tbName.Text = ""
                    tbNo.Text = ""
                    tbDescription1.Text = ""
                    tbDescription2.Text = ""
                    ddlType.SelectedIndex = 0
                    ddlAllJobLevel.SelectedIndex = 0
                    ddlJobLevelStart.SelectedIndex = 0
                    ddlJobLevelEnd.SelectedIndex = 0
                    tbPriority.Text = "0"
                    ddlInput.SelectedIndex = 1
                End If
            Else
                tbCode.Text = ""
                tbName.Text = ""
                tbNo.Text = ""
                tbDescription1.Text = ""
                tbDescription2.Text = ""
                ddlType.SelectedIndex = 0
                ddlAllJobLevel.SelectedIndex = 0
                ddlJobLevelStart.SelectedIndex = 0
                ddlJobLevelEnd.SelectedIndex = 0
                tbPriority.Text = "0"
                ddlInput.SelectedIndex = 1
            End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        Try
            ddlAllJobLevel.Enabled = State
            tbPriority.Enabled = State
            ddlInput.Enabled = State
            btnSaveHd.Enabled = State
            btnCancelHd.Enabled = State
            ddlJobLevelStart.Enabled = State
            ddlJobLevelEnd.Enabled = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlAllJobLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAllJobLevel.SelectedIndexChanged
        Try
            JobLevelChange()
            Panel5.Visible = False
            PanelHd.Enabled = False
        Catch ex As Exception
            Throw New Exception("ddlAllJobLevel_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub JobLevelChange()
        Try
            If ddlAllJobLevel.SelectedValue = "Y" Then
                ddlJobLevelStart.Enabled = False
                ddlJobLevelEnd.Enabled = False
                ddlJobLevelStart.SelectedIndex = 0
                ddlJobLevelEnd.SelectedIndex = 0
            Else
                ddlJobLevelStart.Enabled = True
                ddlJobLevelEnd.Enabled = True
            End If
        Catch ex As Exception
            Throw New Exception("JobLevelChange : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveHd()
        Dim SQLString As String
        Try

            If cekInput() = False Then
                Panel5.Visible = False
                Exit Sub
            End If

            Dim Ds As DataSet
            Dim SQL As String

            SQL = "SELECT * FROM MsCompetenceDept WHERE Department = " + QuotedStr(tbDeptCode.Text) + " AND CompetenceType = " + QuotedStr(ddlComptenceType.SelectedValue) + " AND CompetenceCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + tbNo.Text
            Ds = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString)

            If Ds.Tables(0).Rows.Count <> 0 Then
                If ddlInput.SelectedValue = "Y" Then

                    SQLString = "DELETE FROM MsCompetenceDept WHERE Department = " + QuotedStr(tbDeptCode.Text) + " AND CompetenceType = " + QuotedStr(ddlComptenceType.SelectedValue) + " AND CompetenceCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + tbNo.Text
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                    SQLString = "INSERT INTO MsCompetenceDept (Department, CompetenceType, CompetenceCode, ItemNo, Priority, FgAllJobLevel, StartJobLevel, EndJobLevel, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(tbDeptCode.Text) + ", " + QuotedStr(ddlComptenceType.SelectedValue) + ", " + QuotedStr(tbCode.Text) + ", " + tbNo.Text + ", " + tbPriority.Text.Replace(",", "") + ", " + QuotedStr(ddlAllJobLevel.SelectedValue) + " , " + QuotedStr(ddlJobLevelStart.SelectedValue) + ", " + QuotedStr(ddlJobLevelEnd.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                
                Else
                    SQLString = "DELETE FROM MsCompetenceDept WHERE Department = " + QuotedStr(tbDeptCode.Text) + " AND CompetenceType = " + QuotedStr(ddlComptenceType.SelectedValue) + " AND CompetenceCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + tbNo.Text
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                End If
            Else
                If ddlInput.SelectedValue = "Y" Then
                    SQLString = "INSERT INTO MsCompetenceDept (Department, CompetenceType, CompetenceCode, ItemNo, Priority, FgAllJobLevel, StartJobLevel, EndJobLevel, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(tbDeptCode.Text) + ", " + QuotedStr(ddlComptenceType.SelectedValue) + ", " + QuotedStr(tbCode.Text) + ", " + tbNo.Text + ", " + tbPriority.Text.Replace(",", "") + ", " + QuotedStr(ddlAllJobLevel.SelectedValue) + " , " + QuotedStr(ddlJobLevelStart.SelectedValue) + ", " + QuotedStr(ddlJobLevelEnd.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If

            End If
            bindDataGrid()
            pnlView.Visible = False
            Panel5.Visible = True
            PanelHd.Enabled = True
            Menu1.Enabled = True
        Catch ex As Exception
            Throw New Exception("Save Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveHd.Click
        Try
            SaveHd()
        Catch ex As Exception
            lstatus.Text = "Save Hd Error : " + ex.ToString
        End Try
    End Sub

    Private Function cekInput() As Boolean
        Try
            If tbPriority.Text.Trim = "" Then
                tbPriority.Text = 0
            End If
            If CInt(tbPriority.Text) <= 0 Then
                lstatus.Text = MessageDlg("Priority must have value")
                tbPriority.Focus()
                Exit Function
            End If

            If ddlAllJobLevel.SelectedValue = "N" Then
                If (ddlJobLevelStart.SelectedValue.Trim = "") Or (ddlJobLevelEnd.SelectedValue.Trim = "") Then
                    lstatus.Text = MessageDlg("Range Job Level must have value")
                    ddlJobLevelStart.Focus()
                    Exit Function
                End If
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function
End Class
