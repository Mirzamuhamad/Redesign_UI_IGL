Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Execute_Master_MsSampling_MsSampling
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            Session("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
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
        Dim tbRangeStart, tbRangeEnd, tbQty, tbPercent As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsSampling " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "SamplingCode ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            Dim ddlType As DropDownList
            
            Try
                GVR = DataGrid.FooterRow
                ddlType = GVR.FindControl("SamplingTypeAdd")
                tbRangeStart = GVR.FindControl("RangeStartAdd")
                tbRangeEnd = GVR.FindControl("RangeEndAdd")
                tbQty = GVR.FindControl("QtyAdd")
                tbPercent = GVR.FindControl("PercentageAdd")

                tbRangeStart.Enabled = ddlType.SelectedValue = "Range"
                tbRangeEnd.Enabled = ddlType.SelectedValue = "Range"
                tbQty.Enabled = ddlType.SelectedValue = "Range"
                tbPercent.Enabled = ddlType.SelectedValue = "Percentage"

                tbRangeStart.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbRangeEnd.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Catch ex As Exception
                lstatus.Text = "BindDataGrid Error : " + vbCrLf + ex.ToString
            End Try

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster4 'MsSampling','SamplingCode','SamplingType','Qty', 'Percentage','Sampling File','Sampling Size','Type','Qty', 'Percentage'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
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
        Dim dbCode, dbRangeStart, dbRangeEnd, dbQty, dbPercent As TextBox
        Dim cbxType As DropDownList
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("SamplingCodeAdd")
                cbxType = DataGrid.FooterRow.FindControl("SamplingTypeAdd")
                dbRangeStart = DataGrid.FooterRow.FindControl("RangeStartAdd")
                dbRangeEnd = DataGrid.FooterRow.FindControl("RangeEndAdd")
                dbQty = DataGrid.FooterRow.FindControl("QtyAdd")
                dbPercent = DataGrid.FooterRow.FindControl("PercentageAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Sampling Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If cbxType.SelectedValue = "Range" And dbQty.Text.Trim = "" Then
                    lstatus.Text = "Qty must be filled."
                    dbQty.Focus()
                    Exit Sub
                End If
                If cbxType.SelectedValue = "Range" And CFloat(dbQty.Text.Trim) = 0 Then
                    lstatus.Text = "Qty must be greater than 0."
                    dbQty.Focus()
                    Exit Sub
                End If
                If cbxType.SelectedValue = "Percentage" And dbPercent.Text.Trim = "" Then
                    lstatus.Text = "Percentage must be filled."
                    dbPercent.Focus()
                    Exit Sub
                End If
                If cbxType.SelectedValue = "Percentage" And Not (CFloat(dbPercent.Text.Trim) >= 0 And CFloat(dbPercent.Text.Trim) <= 100) Then
                    lstatus.Text = "Percentage must be between 0 and 100."
                    dbPercent.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT SamplingCode From MsSampling WHERE SamplingCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Sampling Size " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If
                'insert the new entry
                SQLString = "Insert into MsSampling (SamplingCode, SamplingType, RangeStart, RangeEnd, Qty, Percentage, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(cbxType.SelectedValue) + "," + dbRangeStart.Text.Replace(",", "") + "," + dbRangeEnd.Text.Replace(",", "") + "," + dbQty.Text.Replace(",", "") + "," + dbPercent.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("SamplingCode")

            SQLExecuteNonQuery("Delete from MsSampling where SamplingCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        'Dim txt As TextBox
        Dim cbxType As DropDownList
        Dim dbRangeStart, dbRangeEnd, dbQty, dbPercent As TextBox
        Dim txtCode As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)

            txtCode = obj.FindControl("SamplingCodeEdit")
            cbxType = obj.FindControl("SamplingTypeEdit")

            dbRangeStart = obj.FindControl("RangeStartEdit")
            dbRangeEnd = obj.FindControl("RangeEndEdit")
            dbQty = obj.FindControl("QtyEdit")
            dbPercent = obj.FindControl("PercentageEdit")

            dbRangeStart.Enabled = cbxType.SelectedValue = "Range"
            dbRangeEnd.Enabled = cbxType.SelectedValue = "Range"
            dbQty.Enabled = cbxType.SelectedValue = "Range"
            dbPercent.Enabled = cbxType.SelectedValue = "Percentage"

            dbRangeStart.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbRangeEnd.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Dim PClass As String = DirectCast(SQLExecuteScalar("SELECT Position FROM VMsSampling WHERE SamplingCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)
            'Dim FgPLSumClass As String = DirectCast(SQLExecuteScalar("SELECT FgPLSummary FROM VMsSampling WHERE SamplingCode = " + QuotedStr(txtCode.Text), ViewState("DBConnection").ToString), String)

            'txt = obj.FindControl("SamplingNameEdit")
            'txt.Focus()


            'Dim ddlFgType, ddlPClass, ddlFgPLSumClass As DropDownList
            'Dim GVR As GridViewRow

            'GVR = DataGrid.Rows(DataGrid.EditIndex)
            'ddlFgType = GVR.FindControl("FgTypeEdit")
            'ddlPClass = GVR.FindControl("PositionEdit")
            'ddlFgPLSumClass = GVR.FindControl("FgPLSummaryEdit")


            'If ddlFgType.SelectedIndex = 1 Then
            '    ddlPClass.Visible = False
            '    ddlPClass.Items.Clear()

            '    ddlFgPLSumClass.Visible = True
            '    ddlFgPLSumClass.Items.Add("Y")
            '    ddlFgPLSumClass.Items.Add("N")

            '    ddlFgPLSumClass.SelectedValue = FgPLSumClass

            'Else
            '    ddlFgPLSumClass.Visible = False
            '    ddlFgPLSumClass.Items.Clear()

            '    ddlPClass.Visible = True
            '    ddlPClass.Items.Add("ACTIVA")
            '    ddlPClass.Items.Add("PASIVA")

            '    ddlPClass.SelectedValue = PClass
            'End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        'Dim dbName As TextBox
        Dim lbCode As Label
        Dim CbxType As DropDownList
        Dim dbRangeStart, dbRangeEnd, dbQty, dbPercent As TextBox

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("SamplingCodeEdit")
            cbxType = DataGrid.Rows(e.RowIndex).FindControl("SamplingTypeEdit")
            dbRangeStart = DataGrid.Rows(e.RowIndex).FindControl("RangeStartEdit")
            dbRangeEnd = DataGrid.Rows(e.RowIndex).FindControl("RangeEndEdit")
            dbQty = DataGrid.Rows(e.RowIndex).FindControl("QtyEdit")
            dbPercent = DataGrid.Rows(e.RowIndex).FindControl("PercentageEdit")

            If CbxType.SelectedValue = "Range" And dbQty.Text.Trim = "" Then
                lstatus.Text = "Qty must be filled."
                dbQty.Focus()
                Exit Sub
            End If
            If CbxType.SelectedValue = "Range" And CFloat(dbQty.Text.Trim) = 0 Then
                lstatus.Text = "Qty must be greater than 0."
                dbQty.Focus()
                Exit Sub
            End If
            If CbxType.SelectedValue = "Percentage" And dbPercent.Text.Trim = "" Then
                lstatus.Text = "Percentage must be filled."
                dbPercent.Focus()
                Exit Sub
            End If
            If CbxType.SelectedValue = "Percentage" And Not (CFloat(dbPercent.Text.Trim) >= 0 And CFloat(dbPercent.Text.Trim) <= 100) Then
                lstatus.Text = "Percentage must be between 0 and 100."
                dbPercent.Focus()
                Exit Sub
            End If

            SQLString = "Update MsSampling set SamplingType = " + QuotedStr(CbxType.Text) + ", RangeStart = " + dbRangeStart.Text.Replace(",", "") + ", RangeEnd = " + dbRangeEnd.Text.Replace(",", "") + ", Qty = " + dbQty.Text.Replace(",", "") + ", Percentage = " + dbPercent.Text.Replace(",", "") + " where SamplingCode = " & QuotedStr(lbCode.Text)

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

    Protected Sub SamplingTypeEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlType As DropDownList
        Dim tbRangeStart, tbRangeEnd, tbQty, tbPercent As TextBox
        Dim GVR As GridViewRow
        Dim txtCode As Label
        Try
            GVR = DataGrid.Rows(DataGrid.EditIndex)
            ddlType = GVR.FindControl("SamplingTypeEdit")
            tbRangeStart = GVR.FindControl("RangeStartEdit")
            tbRangeEnd = GVR.FindControl("RangeEndEdit")
            tbQty = GVR.FindControl("QtyEdit")
            tbPercent = GVR.FindControl("PercentageEdit")

            txtCode = GVR.FindControl("SamplingCodeEdit")

            tbRangeStart.Enabled = ddlType.SelectedValue = "Range"
            tbRangeEnd.Enabled = ddlType.SelectedValue = "Range"
            tbQty.Enabled = ddlType.SelectedValue = "Range"
            tbPercent.Enabled = ddlType.SelectedValue = "Percentage"
            tbRangeStart.Text = "0"
            tbRangeEnd.Text = "0"
            tbQty.Text = "0"
            tbPercent.Text = "0"
        Catch ex As Exception
            lstatus.Text = "SamplingTypeEdit_SelectedIndexChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub SamplingTypeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlType As DropDownList
        Dim tbRangeStart, tbRangeEnd, tbQty, tbPercent As TextBox
        Dim GVR As GridViewRow

        Try
            GVR = DataGrid.FooterRow
            ddlType = GVR.FindControl("SamplingTypeAdd")
            tbRangeStart = GVR.FindControl("RangeStartAdd")
            tbRangeEnd = GVR.FindControl("RangeEndAdd")
            tbQty = GVR.FindControl("QtyAdd")
            tbPercent = GVR.FindControl("PercentageAdd")

            tbRangeStart.Enabled = ddlType.SelectedValue = "Range"
            tbRangeEnd.Enabled = ddlType.SelectedValue = "Range"
            tbQty.Enabled = ddlType.SelectedValue = "Range"
            tbPercent.Enabled = ddlType.SelectedValue = "Percentage"
            tbRangeStart.Text = "0"
            tbRangeEnd.Text = "0"
            tbQty.Text = "0"
            tbPercent.Text = "0"
        Catch ex As Exception
            lstatus.Text = "SamplingTypeAdd_SelectedIndexChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Public Sub New()

    End Sub
End Class
