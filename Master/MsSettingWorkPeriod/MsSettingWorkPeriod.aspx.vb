Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsSettingWorkPeriod_MsSettingWorkPeriod
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
        'Dim obj As GridViewRow

        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
            DataGrid.Visible = True
            'txt = obj.FindControl("TunjanganTypeAdd")

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
        Dim txt As Label
        Dim GVR As GridViewRow
        Dim StartYear, EndYear, Formula As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter = "" Then
                SqlString = "Select * from VMsSettingWorkPeriod WHERE TunjanganType = " + QuotedStr(ddlType.Text)
            Else
                StrFilter = StrFilter.Replace("Where", "")
                SqlString = "Select * from VMsSettingWorkPeriod WHERE TunjanganType = " + QuotedStr(ddlType.Text) + " AND (" + StrFilter + ")"
            End If

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "StartYear ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            txt = DataGrid.FooterRow.FindControl("TunjanganTypeAdd")
            txt.Text = ddlType.Text

            GVR = DataGrid.FooterRow
            StartYear = GVR.FindControl("StartYearAdd")
            StartYear.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndYear = GVR.FindControl("EndYearAdd")
            EndYear.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Formula = GVR.FindControl("FormulaAdd")
            Formula.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'lstatus.Text = ddlType.Text
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter <> "" Then
                StrFilter = QuotedStr(StrFilter)
                StrFilter = StrFilter.Remove(StrFilter.Length - 1)
                StrFilter = StrFilter.Replace("Where", "AND(")
            End If
            Session("PrintType") = "Print"
            If StrFilter <> "" Then
                Session("SelectCommand") = "S_FormPrintMaster5 'VMsSettingWorkPeriod','TunjanganType','StartYear','EndYear','BaseOn','Formula','Setting Work Period File','Tunjangan','Start Year','End Year','Base On','Formula','Where TunjanganType = '" + QuotedStr(ddlType.Text) + StrFilter + ")'" + "," + QuotedStr(ViewState("UserId"))
            Else
                Session("SelectCommand") = "S_FormPrintMaster5 'VMsSettingWorkPeriod','TunjanganType','StartYear','EndYear','BaseOn','Formula','Setting Work Period File','Tunjangan','Start Year','End Year','Base On','Formula','Where TunjanganType = '" + QuotedStr(ddlType.Text) + "''" + "," + QuotedStr(ViewState("UserId"))
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub

            'AttachScriptAJAX("openprintdlg();", Page, Me.GetType)
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
        Dim dbCode As Label
        Dim dbStartYear, dbEndYear, dbFormula As TextBox
        Dim ddlBaseOn As DropDownList
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("TunjanganTypeAdd")
                dbStartYear = DataGrid.FooterRow.FindControl("StartYearAdd")
                dbEndYear = DataGrid.FooterRow.FindControl("EndYearAdd")
                ddlBaseOn = DataGrid.FooterRow.FindControl("BaseOnAdd")
                dbFormula = DataGrid.FooterRow.FindControl("FormulaAdd")

                If dbStartYear.Text.Trim.Length = 0 Then
                    lstatus.Text = "Start Year must be filled."
                    dbStartYear.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbStartYear.Text) Then
                    lstatus.Text = "Start Year must be numeric type."
                    dbStartYear.Focus()
                    Exit Sub
                End If

                If dbEndYear.Text.Trim.Length = 0 Then
                    lstatus.Text = "End Year must be filled."
                    dbEndYear.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbEndYear.Text) Then
                    lstatus.Text = "End Year must be numeric type."
                    dbEndYear.Focus()
                    Exit Sub
                End If

                If ddlBaseOn.Text.Trim.Length = 0 Then
                    lstatus.Text = "Base On must be filled."
                    ddlBaseOn.Focus()
                    Exit Sub
                End If

                If dbFormula.Text.Trim.Length = 0 Then
                    lstatus.Text = "Formula Must be filled."
                    dbFormula.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbFormula.Text) Then
                    lstatus.Text = "Formula must be numeric type."
                    dbFormula.Focus()
                    Exit Sub
                End If

                If CInt(dbStartYear.Text) > CInt(dbEndYear.Text) Then
                    lstatus.Text = "Start Year Must be Less Than Or Equal To End Year"
                    dbStartYear.Focus()
                    Exit Sub
                End If

                If (CInt(dbEndYear.Text) - CInt(dbStartYear.Text)) > 4 Then
                    lstatus.Text = "Start Year And End Year Range Must be Less Than Or Equal To 4"
                    dbStartYear.Focus()
                    Exit Sub
                End If

                'lstatus.Text = "SELECT TunjanganType,StartYear From VMsSettingWorkPeriod WHERE TunjanganType  = " + QuotedStr(ddlType.Text) + _
                '    " AND StartYear = " + dbStartYear.Text
                'Exit Sub

                If SQLExecuteScalar("SELECT TunjanganType,StartYear From VMsSettingWorkPeriod WHERE TunjanganType  = " + QuotedStr(ddlType.Text) + _
                    " AND StartYear = " + dbStartYear.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Allowance " + QuotedStr(ddlType.Text) + " with Start Year = " + dbStartYear.Text + " has already been exist"
                    Exit Sub
                End If

                'lstatus.Text = "SELECT TunjanganType,StartYear From VMsSettingWorkPeriod WHERE TunjanganType  = " + QuotedStr(ddlType.Text) + _
                '    " AND " + dbStartYear.Text + "  BETWEEN StartYear AND EndYear"
                'Exit Sub

                If SQLExecuteScalar("SELECT TunjanganType,StartYear From VMsSettingWorkPeriod WHERE TunjanganType  = " + QuotedStr(ddlType.Text) + _
                    " AND (" + dbStartYear.Text + "  BETWEEN StartYear AND EndYear)", ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Allowance " + QuotedStr(ddlType.Text) + " with Start Year = " + QuotedStr(dbStartYear.Text) + " Must be Greater Than With Another End Year"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsTunjangan(TunjanganType, StartYear,EndYear,BaseOn,Formula,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(ddlType.Text) + ", " + dbStartYear.Text + ", " + dbEndYear.Text + "," + QuotedStr(ddlBaseOn.Text) + "," + dbFormula.Text + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID, txtYear As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("TunjanganType")
            txtYear = DataGrid.Rows(e.RowIndex).FindControl("StartYear")

            SQLExecuteNonQuery("Delete from MsTunjangan where TunjanganType = '" & txtID.Text & "' AND StartYear = '" & txtYear.Text & "'  ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, EndYear, Formula As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("EndYearEdit")
            txt.Focus()

            EndYear = DataGrid.Rows(e.NewEditIndex).FindControl("EndYearEdit")
            EndYear.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Formula = DataGrid.Rows(e.NewEditIndex).FindControl("FormulaEdit")
            Formula.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbEndYear, dbFormula As TextBox
        Dim ddlBaseOn As DropDownList
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("StartYearEdit")
            dbEndYear = DataGrid.Rows(e.RowIndex).FindControl("EndYearEdit")
            ddlBaseOn = DataGrid.Rows(e.RowIndex).FindControl("BaseOnEdit")
            dbFormula = DataGrid.Rows(e.RowIndex).FindControl("FormulaEdit")

            If dbEndYear.Text.Trim.Length = 0 Then
                lstatus.Text = "End Year must be filled."
                dbEndYear.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbEndYear.Text) Then
                lstatus.Text = "End Year must be numeric type."
                dbEndYear.Focus()
                Exit Sub
            End If


            If ddlBaseOn.Text.Trim.Length = 0 Then
                lstatus.Text = "Base On must be filled."
                ddlBaseOn.Focus()
                Exit Sub
            End If

            If dbFormula.Text.Trim.Length = 0 Then
                lstatus.Text = "Formula Must be filled."
                dbFormula.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbFormula.Text) Then
                lstatus.Text = "Formula must be numeric type."
                dbFormula.Focus()
                Exit Sub
            End If


            If CInt(lbCode.Text) > CInt(dbEndYear.Text) Then
                lstatus.Text = "Start Year Must be Less Than Or Equal To End Year"
                dbEndYear.Focus()
                Exit Sub
            End If

            If (CInt(dbEndYear.Text) - CInt(lbCode.Text)) > 4 Then
                lstatus.Text = "Start Year And End Year Range Must be Less Thane Or Equal To 4"
                dbEndYear.Focus()
                Exit Sub
            End If

            dbFormula.Text = dbFormula.Text.Replace(",", "")


            SQLString = "Update MsTunjangan set EndYear = " + dbEndYear.Text + ",BaseOn = " + QuotedStr(ddlBaseOn.Text) + _
            ",Formula = " + dbFormula.Text + " where TunjanganType = " + QuotedStr(ddlType.Text) + " AND StartYear = " + lbCode.Text

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

    
    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        DataGrid.Visible = False


    End Sub
End Class
