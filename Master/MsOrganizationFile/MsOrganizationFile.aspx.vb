Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsOrganizationFile_MsOrganizationFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        If Not Session("Result") Is Nothing Then
            Dim Acc As TextBox
            Dim AccName As TextBox
            If ViewState("Sender") = "SearchParentAdd" Or ViewState("Sender") = "SearchParentEdit" Then
                If ViewState("Sender") = "SearchParentAdd" Then
                    Acc = DataGrid.FooterRow.FindControl("DeptGroupAdd")
                    AccName = DataGrid.FooterRow.FindControl("DeptGroupAddDesc")
                Else
                    Acc = DataGrid.Rows(DataGrid.EditIndex).FindControl("DeptGroupEdit")
                    AccName = DataGrid.Rows(DataGrid.EditIndex).FindControl("DeptGroupEditDesc")
                End If

                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                Acc.Focus()
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Criteria") = Nothing
            Session("Column") = Nothing
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
        Dim GVR As GridViewRow
        'Dim Parent As TextBox
        'StrucCode, Level

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsDepartment " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "DeptCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            'StrucCode = GVR.FindControl("StructureCodeAdd")
            'StrucCode.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Parent = GVR.FindControl("ParentAdd")
            'Parent.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Level = GVR.FindControl("LevelAdd")
            'Level.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster4 'VMsDepartment2','DeptCode','DeptName','DeptLevel','DeptGroup','Department','Department Code','Department Name','Department Level','Department Group'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
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
        Dim SQLString As String
        Dim dbCode, dbDesc, dbParent, dblevel As TextBox
        Dim StrucCode, Level As String
        Dim Dt As DataTable
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)

        Try
            If e.CommandName = "Insert" Then
                'dbCode = DataGrid.FooterRow.FindControl("StructureCodeAdd")
                'dbDesc = DataGrid.FooterRow.FindControl("DescriptionAdd")
                'dbParent = DataGrid.FooterRow.FindControl("ParentAdd")
                'dblevel = DataGrid.FooterRow.FindControl("LevelAdd")

                dbCode = DataGrid.FooterRow.FindControl("DeptCodeAdd")
                dbDesc = DataGrid.FooterRow.FindControl("DeptNameAdd")
                'dblevel = DataGrid.FooterRow.FindControl("LevelAdd")
                dbParent = DataGrid.FooterRow.FindControl("DeptGroupAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Department Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If dbDesc.Text.Trim.Length = 0 Then
                    lstatus.Text = "Description must be filled."
                    dbDesc.Focus()
                    Exit Sub
                End If

                'If dbParent.Text.Trim.Length = 0 Then
                '    lstatus.Text = "Parent must be filled."
                '    dbParent.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(dbCode.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters"
                '    dbCode.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(dbName.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters"
                '    dbName.Focus()
                '    Exit Sub
                'End If

                If dbParent.Text = "" Then
                    'SQLString = "Select * From MsStructureReport Where Parent Is Null"
                    'SQLString = "Select * From MsDepartment Where DeptGroup Is Null"
                    'Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                    'If Dt.Rows.Count > 0 Then
                    'SQLString = "Select Max(StructureCode) As MaxCode From MsStructureReport Where Parent Is Null"
                    'Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                    Level = "0"

                    'Dim tempCode, newCode As String
                    'Dim tempCode1 As Integer

                    'tempCode = Dt.Rows(0).Item("MaxCode").ToString
                    'tempCode1 = CInt(tempCode) + 1
                    'newCode = "0" + tempCode1.ToString

                    SQLString = "Insert Into MsDepartment (DeptCode, DeptName, DeptLevel, DeptGroup) " + _
                    "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbDesc.Text) + _
                    "," + QuotedStr(Level) + _
                    ", null"

                    'SQLString = "Insert Into MsStructureReport (StructureCode, Description, Parent, Level) " + _
                    '"SELECT " + QuotedStr(newCode) + ", " + QuotedStr(dbDesc.Text) + _
                    '", null  " + _
                    '"," + QuotedStr(Level)
                    'Else
                    'StrucCode = "01"
                    'Level = "0"

                    'SQLString = "Insert Into MsDepartment (DeptCode, DeptName, DeptLevel, DeptGroup) " + _
                    '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbDesc.Text) + _
                    '"," + QuotedStr(Level) + _
                    '", null"

                    'SQLString = "Insert Into MsStructureReport (StructureCode, Description, Parent, Level) " + _
                    '"SELECT " + QuotedStr(StrucCode) + ", " + QuotedStr(dbDesc.Text) + _
                    '", null  " + _
                    '"," + QuotedStr(Level)
                    'End If
                Else
                    'SQLString = "Select Max(StructureCode) As MaxCode From MsStructureReport Where Parent='" + dbParent.Text.Trim + "'"
                    SQLString = "Select DeptLevel From MsDepartment Where DeptCode='" + dbParent.Text.Trim + "'"
                    Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                    If IsDBNull(Dt.Rows(0).Item("DeptLevel").ToString) Or Dt.Rows(0).Item("DeptLevel").ToString = "" Then
                        'Dim lvl As Integer

                        'lvl = CInt(dbParent.Text.Length) / 2
                        'Level = lvl.ToString
                        'StrucCode = dbParent.Text + "01"

                        'SQLString = "Insert Into MsStructureReport (StructureCode, Description, Parent, Level) " + _
                        '"SELECT " + QuotedStr(StrucCode) + ", " + QuotedStr(dbDesc.Text) + _
                        '"," + QuotedStr(dbParent.Text) + _
                        '"," + QuotedStr(Level)
                        Exit Sub
                    Else
                        Dim tempLevel As String ', newLevel As String
                        Dim tempLevel1 As Integer
                        'Dim lvl As Integer

                        'tempCode = Right(Dt.Rows(0).Item("MaxCode").ToString, 2)
                        tempLevel = Dt.Rows(0).Item("DeptLevel").ToString
                        tempLevel1 = CInt(tempLevel) + 1
                        'newLevel = dbParent.Text + "0" + tempLevel1.ToString

                        'lvl = CInt(dbParent.Text.Length) / 2
                        'Level = lvl.ToString

                        SQLString = "Insert Into MsDepartment (DeptCode, DeptName, DeptLevel, DeptGroup) " + _
                        "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbDesc.Text) + _
                        "," + tempLevel1.ToString() + _
                        "," + QuotedStr(dbParent.Text)

                        'SQLString = "Insert Into MsStructureReport (StructureCode, Description, Parent, Level) " + _
                        '"SELECT " + QuotedStr(newCode) + ", " + QuotedStr(dbDesc.Text) + _
                        '"," + QuotedStr(dbParent.Text) + _
                        '"," + QuotedStr(Level)
                    End If
                End If

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()

            ElseIf e.CommandName = "SearchParentEdit" Or e.CommandName = "SearchParentAdd" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select DeptCode, DeptName, DeptLevel FROM MsDepartment"
                FieldResult = "DeptCode, DeptName"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchParentAdd" Then
                    ViewState("Sender") = "SearchParentAdd"
                Else
                    ViewState("Sender") = "SearchParentEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If

                'If SQLExecuteScalar("SELECT AreaServiceCode From VMsAreaService WHERE AreaServiceCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "AreaService " + QuotedStr(dbDesc.Text) + " has already been exist"
                '    Exit Sub
                'End If

                'insert the new entry
                'SQLString = "Insert into MsAreaService (AreaServiceCode, AreaServiceName, UserId, UserDate) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbDesc.Text) + _
                '"," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("DeptCode")

            SQLExecuteNonQuery("Delete From MsDepartment Where DeptCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        'Dim Parent, Level As TextBox
        'StrucCode, Level

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("DeptCodeEdit")
            txt.Focus()

            'StrucCode = DataGrid.Rows(e.NewEditIndex).FindControl("StructureCodeEdit")
            'StrucCode.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Parent = DataGrid.Rows(e.NewEditIndex).FindControl("ParentEdit")
            'Parent.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'Level = DataGrid.Rows(e.NewEditIndex).FindControl("LevelEdit")
            'Level.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Dim tb As New TextBox
            tb.ID = "DeptGroupEdit"

            tbAccount_TextChanged(tb, Nothing)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim Dt As DataTable
        'Dim dbName As TextBox
        Dim dbCode, dbDesc, DbParent As TextBox
        'dbCode, dbParent, dblevel
        Dim lbCode As Label
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("StructureCodeEdit")
            dbCode = DataGrid.Rows(e.RowIndex).FindControl("DeptCodeEdit")
            dbDesc = DataGrid.Rows(e.RowIndex).FindControl("DeptNameEdit")
            DbParent = DataGrid.Rows(e.RowIndex).FindControl("DeptGroupEdit")

            If dbDesc.Text.Trim.Length = 0 Then
                lstatus.Text = "Description must be filled."
                dbDesc.Focus()
                Exit Sub
            End If


            'If r.IsMatch(dbName.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters"
            '    dbName.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update MsDepartment set DeptName='" + dbDesc.Text.Replace("'", "''") + "', DeptCode='" + dbCode.Text.Replace("'", "''") + "', DeptGroup='" + DbParent.Text.Replace("'", "''") + "'  where DeptCode = '" + dbCode.Text + "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            SQLString = "Select DeptLevel From MsDepartment Where DeptCode='" + DbParent.Text.Trim + "'"
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

            If IsDBNull(Dt.Rows(0).Item("DeptLevel").ToString) Or Dt.Rows(0).Item("DeptLevel").ToString = "" Then
                Exit Sub
            Else
                Dim tempLevel As String ', newLevel As String
                Dim tempLevel1 As Integer

                tempLevel = Dt.Rows(0).Item("DeptLevel").ToString
                tempLevel1 = CInt(tempLevel) + 1

                SQLString = "Update MsDepartment set DeptLevel=" + tempLevel1.ToString + " where DeptCode = '" + dbCode.Text + "'"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Parent, tb As TextBox
        Dim ParentName As TextBox
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "DeptGroupAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Parent = dgi.FindControl("DeptGroupAdd")
                ParentName = dgi.FindControl("DeptGroupAddDesc")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Parent = dgi.FindControl("DeptGroupEdit")
                ParentName = dgi.FindControl("DeptGroupEditDesc")
            End If

            ds = SQLExecuteQuery("Select DeptCode, DeptName FROM MsDepartment where DeptCode=" + QuotedStr(Parent.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Parent.Text = ""
                ParentName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Parent.Text = dr("DeptCode").ToString
                ParentName.Text = dr("DeptName").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Parent Changed Error : " + ex.ToString
        End Try
    End Sub
End Class
