Imports System.Data

Partial Class Master_MsRotation_MsRotation
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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
        Dim XPeriod, RangePeriod As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "select RotationCode, RotationName, Rotation From MsRotasi" + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "RotationCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            'GVR = DataGrid.FooterRow
            'XPeriod = GVR.FindControl("XPeriodAdd")
            'XPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'RangePeriod = GVR.FindControl("XRangeAdd")
            'RangePeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT Month, Percentage  FROM V_MsRotasiDtView WHERE RotationCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DataGridDt.Columns(0).Visible = False
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
                DataGridDt.Columns(0).Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim txtRotation As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("RotationNameEdit")

                        txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub

    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
    Private Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        Dim dbRotation As Label
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("RotationCodeAdd")
                dbName = GVR.FindControl("RotationNameAdd")
                dbRotation = GVR.FindControl("RotationAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Rotation Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Rotation Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT RotationCode From V_MsRotasi WHERE RotationCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Rotation " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsRotasi (RotationCode, RotationName, Rotation,UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", 0 ," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("RotationCode")
                lbName = GVR.FindControl("RotationName")
                ViewState("Nmbr") = lbCode.Text
                lbRotationCode.Text = lbCode.Text + " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Dim Total As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub DataGridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Month")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Percentage"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    ' DbHome = GetTotalSum(ViewState("Dt"), "Percentage")
			lbTotalPercentage.Text = FormatNumber(Total, ViewState("DigitHome"))
                End If
'                lbTotalPercentage.Text = FormatNumber(DbHome, ViewState("DigitHome"))

            End If
        Catch ex As Exception
            lstatus.Text = "DataGridDt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGridDt.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGridDt.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                    ModifyInputDt(True)
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    pnlInputDt.Visible = True
                ElseIf DDL.SelectedValue = "Copy New" Then
                    ViewState("State") = "Insert"
                    FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                    ModifyInputDt(True)
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    pnlInputDt.Visible = True
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If

                        SQLExecuteNonQuery("Delete from MsRotasiDt where RotationCode = " + QuotedStr(ViewState("Nmbr")) + " AND Month =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
                        bindDataGridDt()

                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If

            End If


        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim dbRotation As Label
        Dim lbCode As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("RotationCodeEdit")
            dbName = GVR.FindControl("RotationNameEdit")
            dbRotation = GVR.FindControl("RotationEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Rotation Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            'If IsNumeric(dbRotation.Text.Trim.Length) = 0 Then
            '    lstatus.Text = "<script language='javascript'>alert('Rotation must be filled.');</script>"
            '    dbName.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update MsRotasi set RotationName = " + QuotedStr(dbName.Text) + _
            "  where RotationCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("RotationCode")

            SQLExecuteNonQuery("Delete from MsRotasiDt where RotationCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsRotasi where RotationCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        'Dim txtID As Label
        'Try
        '    If CheckMenuLevel("Delete") = False Then
        '        Exit Sub
        '    End If
        '    txtID = DataGridDt.Rows(e.RowIndex).FindControl("WrhsType")

        '    SQLExecuteNonQuery("Delete from MsRotationDt where RotationCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(txtID.Text))
        '    bindDataGridDt()

        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub
    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
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
            lstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            ViewState("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'lstatus.Text = StrFilter
            'Exit Sub
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'MsRotasi','RotationCode', 'RotationName','Rotation','Rotation File','Rotation Code','Rotation Name','Rotation '," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = Server.MapPath("~\Rpt\RptPrintMaster2.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub FillTextBoxDt(ByVal Nmbr As String, ByVal Code As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery("SELECT A.*, C.RotationName FROM MsRotasiDt A " + _
            "LEFT OUTER JOIN MsRotasi C ON A.RotationCode = C.RotationCode " + _
            "WHERE A.RotationCode =" + QuotedStr(Nmbr) + " AND Month = " + QuotedStr(Code), ViewState("DBConnection").ToString).Tables(0).Rows(0)

            ClearDt()

            BindToText(tbMonth, dr("Month").ToString)
            BindToText(tbPercentage, FormatFloat(dr("Percentage").ToString,2))
            tbMonth.Focus()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        ViewState("State") = "Insert"
        ClearDt()
        ModifyInputDt(True)
	tbMonth.Enabled = True
        pnlDt.Visible = False
        pnlInputDt.Visible = True
        btnSave.Visible = True
        btnReset.Visible = True
        tbMonth.Focus()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ClearDt()
        pnlDt.Visible = True
        pnlInputDt.Visible = False
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ClearDt()
    End Sub
    Private Sub ClearDt()
        Try
            tbMonth.Text = 0
            tbPercentage.Text = "0"
        Catch ex As Exception
            lstatus.Text = "ClearDt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Dim SQLString2 As String = ""
        Try
            If tbMonth.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('month must be filled');</script>"
                tbMonth.Focus()
                Exit Sub
            End If

            If tbPercentage.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Percentage must be filled');</script>"
                tbPercentage.Focus()
                Exit Sub
            End If


            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT Percentage From V_MsRotasiDtView WHERE RotationCode = " + QuotedStr(ViewState("Nmbr")) + " AND Month =" + QuotedStr(tbMonth.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Rotation " + QuotedStr(ViewState("Nmbr")) + " Month " + QuotedStr(tbMonth.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert Into MsRotasiDt (RotationCode, Month, Percentage )" + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(tbMonth.Text) + "," + _
                QuotedStr(tbPercentage.Text)

            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsRotasiDt SET Percentage= " + QuotedStr(tbPercentage.Text) + _
                " WHERE month = " + QuotedStr(tbMonth.Text) + _
                " AND RotationCode =" + QuotedStr(ViewState("Nmbr"))

               
            End If
            SQLString2 = "UPDATE MsRotasi SET Rotation= " + lbTotalPercentage.Text + _
               " WHERE RotationCode =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            SQLString2 = Replace(SQLString2, "''", "NULL")
            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)

            bindDataGridDt()

            bindDataGrid()
            pnlInputDt.Visible = False
            pnlDt.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ModifyInputDt(ByVal State As Boolean)
        tbMonth.Enabled = False
        tbPercentage.Enabled = State
    End Sub

End Class

