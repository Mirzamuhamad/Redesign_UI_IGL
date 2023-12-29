Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data



Partial Class MsVehicle_MsVehicle
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        dsVehicleType.ConnectionString = ViewState("DBConnection")
        dsSupplier.ConnectionString = ViewState("DBConnection")
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
        Dim ddlBelongsTo As DropDownList
        Dim ddlSupplier As DropDownList
        Dim GVR As GridViewRow
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select A.*,B.VehicleTypeName,C.SuppName from MsVehicle A INNER JOIN MsVehicleType B ON A.VehicleType = B.VehicleTypeCode LEFT OUTER JOIN MsSupplier C ON A.Supplier = C.SuppCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "VehicleCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            ddlBelongsTo = GVR.FindControl("BelongsToAdd")
            ddlSupplier = GVR.FindControl("ddlSupplierAdd")

            If ddlBelongsTo.SelectedIndex = 0 Then
                ddlSupplier.Visible = False
            Else
                ddlSupplier.Visible = True
            End If

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
            Session("SelectCommand") = "EXEC S_FormMsVehicle " + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMsVehicle.frx"
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
        Dim dbCode, dbName As TextBox
        Dim ddlVehicleType, ddlSupplier, ddlBelongsTo, ddlFgActive, ddlSize As DropDownList
        Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("VehicleCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("VehicleNameAdd")
                ddlVehicleType = DataGrid.FooterRow.FindControl("ddlVehicleTypeAdd")
                ddlBelongsTo = DataGrid.FooterRow.FindControl("BelongsToAdd")
                ddlFgActive = DataGrid.FooterRow.FindControl("ActiveAdd")
                ddlSize = DataGrid.FooterRow.FindControl("SizeAdd")
                tbExpireDate = DataGrid.FooterRow.FindControl("tbDateAdd")
                ddlSupplier = DataGrid.FooterRow.FindControl("ddlSupplierAdd")


                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Vehicle Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Vehicle Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If




                If ddlVehicleType.Text.Trim.Length = 0 Then
                    lstatus.Text = "Vehicle Type must be filled."
                    ddlVehicleType.Focus()
                    Exit Sub
                End If



                If ddlBelongsTo.Text.Trim.Length = 0 Then
                    lstatus.Text = "Belongs To must be filled."
                    ddlBelongsTo.Focus()
                    Exit Sub
                End If



                If ddlBelongsTo.Text = "Supplier" Then
                    If ddlSupplier.Text.Trim.Length = 0 Then
                        lstatus.Text = "Supplier must be filled."
                        ddlSupplier.Focus()
                        Exit Sub
                    End If

                End If


                If ddlFgActive.Text.Trim.Length = 0 Then
                    lstatus.Text = "Status must be filled."
                    ddlFgActive.Focus()
                    Exit Sub
                End If



                If ddlSize.Text.Trim.Length = 0 Then
                    lstatus.Text = "Size must be filled."
                    ddlSize.Focus()
                    Exit Sub
                End If




                If tbExpireDate.Text.Trim.Length = 0 Then
                    lstatus.Text = "Expire Date Must Be filled."
                    tbExpireDate.Focus()
                    Exit Sub
                End If

                'If r.IsMatch(dbCode.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Vehicle Code"
                '    dbCode.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(dbName.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Vehicle Name"
                '    dbName.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(ddlVehicleType.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Vehicle Type"
                '    ddlVehicleType.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(ddlBelongsTo.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Belongs To"
                '    ddlBelongsTo.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(ddlFgActive.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Active"
                '    ddlFgActive.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(ddlSize.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Size"
                '    ddlSize.Focus()
                '    Exit Sub
                'End If

                'If r.IsMatch(tbExpireDate.Text) = False Then
                '    lstatus.Text = "Please enter valid Characters in to Expire Date"
                '    tbExpireDate.Focus()
                '    Exit Sub
                'End If


                If SQLExecuteScalar("SELECT Vehicle_Code From VMsVehicle WHERE Vehicle_Code  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Vehicle " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If



                'insert the new entry
                If ddlBelongsTo.Text = "Supplier" Then

                    SQLString = "Insert into MsVehicle (VehicleCode, VehicleName,VehicleType,BelongsTo,VehicleSize," + _
                                "Supplier, ExpireDate, FgActive, UserId, UserDate)" + _
                                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + _
                                "," + QuotedStr(ddlVehicleType.SelectedValue) + ", " + QuotedStr(ddlBelongsTo.SelectedValue) + _
                                "," + QuotedStr(ddlSize.SelectedValue) + ", " + QuotedStr(ddlSupplier.SelectedValue) + _
                                "," + QuotedStr(tbExpireDate.SelectedValue) + ", " + QuotedStr(ddlFgActive.SelectedValue) + _
                                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                Else
                    SQLString = "Insert into MsVehicle (VehicleCode, VehicleName,VehicleType,BelongsTo,VehicleSize," + _
                            "Supplier, ExpireDate, FgActive, UserId, UserDate)" + _
                            "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + _
                            "," + QuotedStr(ddlVehicleType.SelectedValue) + ", " + QuotedStr(ddlBelongsTo.SelectedValue) + _
                            "," + QuotedStr(ddlSize.SelectedValue) + _
                            ",NULL," + QuotedStr(tbExpireDate.SelectedValue) + ", " + QuotedStr(ddlFgActive.SelectedValue) + _
                            "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                End If

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("VehicleCode")

            SQLExecuteNonQuery("Delete from MsVehicle where VehicleCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("VehicleNameEdit")
            txt.Focus()

            Dim GVR As GridViewRow
            Dim ddlBelongsTo As DropDownList
            Dim ddlSupplier As DropDownList

            GVR = DataGrid.Rows(DataGrid.EditIndex)
            ddlBelongsTo = GVR.FindControl("BelongsToEdit")
            ddlSupplier = GVR.FindControl("ddlSupplierEdit")

            If ddlBelongsTo.SelectedIndex = 0 Then
                ddlSupplier.Visible = False
            Else
                ddlSupplier.Visible = True
            End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlVehicleType, ddlSupplier, ddlBelongsTo, ddlFgActive, ddlSize As DropDownList
        Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)


        Try

            lbCode = DataGrid.Rows(e.RowIndex).FindControl("VehicleCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("VehicleNameEdit")
            ddlVehicleType = DataGrid.Rows(e.RowIndex).FindControl("ddlVehicleTypeEdit")
            ddlSupplier = DataGrid.Rows(e.RowIndex).FindControl("ddlSupplierEdit")
            ddlBelongsTo = DataGrid.Rows(e.RowIndex).FindControl("BelongsToEdit")
            ddlFgActive = DataGrid.Rows(e.RowIndex).FindControl("ActiveEdit")
            ddlSize = DataGrid.Rows(e.RowIndex).FindControl("SizeEdit")
            tbExpireDate = DataGrid.Rows(e.RowIndex).FindControl("tbDateEdit")





            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Vehicle Name must be filled."
                dbName.Focus()
                Exit Sub
            End If




            If ddlVehicleType.Text.Trim.Length = 0 Then
                lstatus.Text = "Vehicle Type must be filled."
                ddlVehicleType.Focus()
                Exit Sub
            End If





            If ddlBelongsTo.Text.Trim.Length = 0 Then
                lstatus.Text = "Belongs To must be filled."
                ddlBelongsTo.Focus()
                Exit Sub
            End If

            If ddlBelongsTo.Text = "Supplier" Then
                'ddlSupplier = DataGrid.FooterRow.FindControl("ddlSupplierAdd")
                If ddlSupplier.Text.Trim.Length = 0 Then
                    lstatus.Text = "Supplier must be filled."
                    ddlSupplier.Focus()
                    Exit Sub
                End If

            End If



            If ddlFgActive.Text.Trim.Length = 0 Then
                lstatus.Text = "Status must be filled."
                ddlFgActive.Focus()
                Exit Sub
            End If



            If ddlSize.Text.Trim.Length = 0 Then
                lstatus.Text = "Size must be filled."
                ddlSize.Focus()
                Exit Sub
            End If


            If tbExpireDate.Text.Trim.Length = 0 Then
                lstatus.Text = "Expire Date Must Be filled."
                tbExpireDate.Focus()
                Exit Sub
            End If


            'If r.IsMatch(dbName.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Vehicle Name"
            '    dbName.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(ddlVehicleType.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Vehicle Type"
            '    ddlVehicleType.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(ddlBelongsTo.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Belongs To"
            '    ddlBelongsTo.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(ddlFgActive.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Active"
            '    ddlFgActive.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(ddlSize.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Size"
            '    ddlSize.Focus()
            '    Exit Sub
            'End If

            'If r.IsMatch(tbExpireDate.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Expire Date"
            '    tbExpireDate.Focus()
            '    Exit Sub
            'End If


            If ddlBelongsTo.Text = "Supplier" Then
                SQLString = "UPDATE MsVehicle SET VehicleName = " + QuotedStr(dbName.Text) + ",VehicleType = " + QuotedStr(ddlVehicleType.SelectedValue) + _
                            ",BelongsTo = " + QuotedStr(ddlBelongsTo.SelectedValue) + ", VehicleSize = " + QuotedStr(ddlSize.SelectedValue) + _
                            ",Supplier = " + QuotedStr(ddlSupplier.SelectedValue) + _
                            ",ExpireDate = " + QuotedStr(tbExpireDate.SelectedValue) + ", FgActive = " + QuotedStr(ddlFgActive.SelectedValue) + _
                            ",UserId = " + QuotedStr(ViewState("UserId").ToString) + _
                            ",UserDate = getDate() WHERE VehicleCode = " + QuotedStr(lbCode.Text)
            Else

                SQLString = "UPDATE MsVehicle SET VehicleName = " + QuotedStr(dbName.Text) + ",VehicleType = " + QuotedStr(ddlVehicleType.SelectedValue) + _
                            ",BelongsTo = " + QuotedStr(ddlBelongsTo.SelectedValue) + ", VehicleSize = " + QuotedStr(ddlSize.SelectedValue) + _
                            ",Supplier = NULL " + _
                            ",ExpireDate = " + QuotedStr(tbExpireDate.SelectedValue) + ", FgActive = " + QuotedStr(ddlFgActive.SelectedValue) + _
                            ",UserId = " + QuotedStr(ViewState("UserId").ToString) + _
                            ",UserDate = getDate() WHERE VehicleCode = " + QuotedStr(lbCode.Text)
            End If

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


    Protected Sub ddlBelongstoadd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddlBelongsTo As DropDownList
        Dim ddlSupplier As DropDownList

        Try

            dgi = DataGrid.FooterRow

            ddlBelongsTo = dgi.FindControl("BelongsToAdd")
            ddlSupplier = dgi.FindControl("ddlSupplierAdd")

            If ddlBelongsTo.SelectedIndex = 0 Then
                ddlSupplier.Visible = False
            Else
                ddlSupplier.Visible = True
            End If

        Catch ex As Exception
            lstatus.Text = "Belongs To Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlBelongstoedit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddlBelongsTo As DropDownList
        Dim ddlSupplier As DropDownList

        Try

            dgi = DataGrid.Rows(DataGrid.EditIndex)

            ddlBelongsTo = dgi.FindControl("BelongsToEdit")
            ddlSupplier = dgi.FindControl("ddlSupplierEdit")

            If ddlBelongsTo.SelectedIndex = 0 Then
                ddlSupplier.Visible = False
            Else
                ddlSupplier.Visible = True
            End If

        Catch ex As Exception
            lstatus.Text = "Belongs To Index Changed Error : " + ex.ToString
        End Try
    End Sub



End Class
