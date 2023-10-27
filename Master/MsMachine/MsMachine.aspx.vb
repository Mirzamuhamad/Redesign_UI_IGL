Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsMachine_MsMachine
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
        End If

        dsUnit.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnMachineGroupAdd" Or ViewState("Sender") = "btnMachineGroupEdit" Then
                Dim MachineGroup As TextBox
                Dim MachineGroupName As Label
                If ViewState("Sender") = "btnMachineGroupAdd" Then
                    MachineGroup = DataGrid.FooterRow.FindControl("MachineGroupAdd")
                    MachineGroupName = DataGrid.FooterRow.FindControl("MachineGroupNameAdd")
                Else
                    MachineGroup = DataGrid.Rows(DataGrid.EditIndex).FindControl("MachineGroupEdit")
                    MachineGroupName = DataGrid.Rows(DataGrid.EditIndex).FindControl("MachineGroupNameEdit")
                End If
                MachineGroup.Text = Session("Result")(0).ToString
                MachineGroupName.Text = Session("Result")(1).ToString
                MachineGroup.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
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
            bindDataGrid()
            'tbFilter.Text = ""
            'tbfilter2.Text = ""
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
            SqlString = "Select * from VMsMachine " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Machine_Code Asc "
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster6 'VMsMachine','Machine_Code','Machine_Name','MachineGroup_Name','Specification','Unit','LoadFactor','Machine File','Machine Code','Machine Name','Machine Group Name','Specification','Unit','Load Factor'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
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
        Dim dbCode, dbName, dbSpecification, dbMachineGroup As TextBox
        Dim lbLoadFactor As Label
        Dim ddlUnit As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("MachineCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("MachineNameAdd")
                dbSpecification = DataGrid.FooterRow.FindControl("SpecificationAdd")
                dbMachineGroup = DataGrid.FooterRow.FindControl("MachineGroupAdd")
                lbLoadFactor = DataGrid.FooterRow.FindControl("LoadFactorAdd")
                ddlUnit = DataGrid.FooterRow.FindControl("UnitAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Machine Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Machine Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If dbMachineGroup.Text.Trim.Length = 0 Then
                    lstatus.Text = "Machine Group must be filled."
                    dbSpecification.Focus()
                    Exit Sub
                End If


                If dbSpecification.Text.Trim.Length = 0 Then
                    lstatus.Text = "Specification must be filled."
                    dbSpecification.Focus()
                    Exit Sub
                End If

                If ddlUnit.Text.Trim.Length = 0 Then
                    lstatus.Text = "Unit must be filled."
                    ddlUnit.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("Select Machine_Code From VMsMachine WHERE Machine_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Machine " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsMachine(MachineCode, MachineName, MachineGroup, Specification, Unit, LoadFactor, UserId, UserDate, LoadFactorLB, ManPower)" + _
                " SELECT " + QuotedStr(dbCode.Text) + _
                "," + QuotedStr(dbName.Text) + ", " + QuotedStr(dbMachineGroup.Text.Trim) + _
                "," + QuotedStr(dbSpecification.Text) + ", " + QuotedStr(ddlUnit.SelectedValue) + _
                "," + QuotedStr(lbLoadFactor.Text.Replace(",", "")) + _
                "," + QuotedStr(ViewState("UserId").ToString) + " , getDate(), 0, 0"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "btnMachineGroupAdd" Or e.CommandName = "btnMachineGroupEdit" Then
                Dim FieldResult As String

                If e.CommandName = "btnMachineGroupAdd" Then
                    Session("filter") = "Select * FROM VMsMachineGroup "
                    ViewState("Sender") = "btnMachineGroupAdd"
                Else
                    Session("filter") = "Select * FROM VMsMachineGroup "
                    ViewState("Sender") = "btnMachineGroupEdit"
                End If
                FieldResult = "MachineGrpCode, MachineGrpName"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MachineCode")

            SQLExecuteNonQuery("Delete from MsMachine where MachineCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("MachineNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbMachineGroup, dbSpecification As TextBox
        Dim ddlUnit As DropDownList
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("MachineCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("MachineNameEdit")
            dbMachineGroup = DataGrid.Rows(e.RowIndex).FindControl("MachineGroupEdit")
            dbSpecification = DataGrid.Rows(e.RowIndex).FindControl("SpecificationEdit")
            ddlUnit = DataGrid.Rows(e.RowIndex).FindControl("UnitEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Machine Group Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsMachine set MachineName = " + QuotedStr(dbName.Text) + _
            ",MachineGroup = " + QuotedStr(dbMachineGroup.Text) + _
            ",Specification = " + QuotedStr(dbSpecification.Text) + _
            ",Unit = " + QuotedStr(ddlUnit.Text) + _
            " where MachineCode =  " + QuotedStr(lbCode.Text) + " "

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

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

    Protected Sub MachineGroupAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim MachineGroup, tb As TextBox
        Dim MachineGroupName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try

            tb = sender
            If tb.ID = "MachineGroupAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                MachineGroup = dgi.FindControl("MachineGroupAdd")
                MachineGroupName = dgi.FindControl("MachineGroupNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                MachineGroup = dgi.FindControl("MachineGroupEdit")
                MachineGroupName = dgi.FindControl("MachineGroupNameEdit")
            End If

            ds = SQLExecuteQuery("Select MachineGrpCode, MachineGrpName From VMsMachineGroup WHERE MachineGrpCode = " + QuotedStr(MachineGroup.Text), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                MachineGroup.Text = ""
                MachineGroupName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                MachineGroup.Text = dr("MachineGrpCode").ToString
                MachineGroupName.Text = dr("MachineGrpName").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tb Machine Group Changed Error : " + ex.ToString
        End Try
    End Sub


End Class
