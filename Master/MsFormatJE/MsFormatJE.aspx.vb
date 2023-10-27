Imports System.Data

Partial Class Master_MsFormatJE_MsFormatJE
    Inherits System.Web.UI.Page

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAccountAdd" Or ViewState("Sender") = "SearchAccountEdit" Then
                Dim Acc, Subled As New TextBox
                Dim AccName, SubledName As New Label
                Dim FgSubled, FgType As New Label
                Dim btnSubled As New Button
                Dim ddlCostCtr As DropDownList
                If ViewState("Sender") = "SearchAccountAdd" Then
                    Acc = DataGridDt.FooterRow.FindControl("AccountAdd")
                    AccName = DataGridDt.FooterRow.FindControl("AccountNameAdd")
                    FgSubled = DataGridDt.FooterRow.FindControl("FgSubledAdd")
                    FgType = DataGridDt.FooterRow.FindControl("FgTypeAdd")
                    Subled = DataGridDt.FooterRow.FindControl("SubledAdd")
                    SubledName = DataGridDt.FooterRow.FindControl("SubLedNameAdd")
                    btnSubled = DataGridDt.FooterRow.FindControl("btnSubledAdd")
                    ddlCostCtr = DataGridDt.FooterRow.FindControl("CostCtrAdd")
                Else
                    Acc = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccountEdit")
                    AccName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccountNameEdit")
                    FgSubled = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("FgSubledEdit")
                    FgType = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("FgTypeEdit")
                    Subled = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("SubledEdit")
                    SubledName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("SubLedNameEdit")
                    btnSubled = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("btnSubledEdit")
                    ddlCostCtr = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("CostCtrEdit")
                End If
                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                FgSubled.Text = Session("Result")(2).ToString
                FgType.Text = Session("Result")(3).ToString
                Subled.Enabled = FgSubled.Text <> "N"
                ddlCostCtr.Enabled = (FgType.Text = "PL")
                btnSubled.Visible = Subled.Enabled
                Subled.Text = ""
                SubledName.Text = ""
                ddlCostCtr.SelectedIndex = 0
                Acc.Focus()
            ElseIf ViewState("Sender") = "SearchSubledAdd" Or ViewState("Sender") = "SearchSubledEdit" Then
                Dim Subled As New TextBox
                Dim SubledName As New Label
                If ViewState("Sender") = "SearchSubledAdd" Then
                    Subled = DataGridDt.FooterRow.FindControl("SubledAdd")
                    SubledName = DataGridDt.FooterRow.FindControl("SubledNameAdd")
                Else
                    Subled = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("SubledEdit")
                    SubledName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("SubledNameEdit")
                End If
                Subled.Text = Session("Result")(0).ToString
                SubledName.Text = Session("Result")(1).ToString
                Subled.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        dsCostCtr.ConnectionString = ViewState("DBConnection")
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

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsFormatJE " + StrFilter + " Order By FormatJECode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FormatJECode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb, SubLed As TextBox
        Dim FgSubled, AccName, SubLedName, FgType As Label
        Dim btnSubled As Button
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim ddlCostCtr As DropDownList
        Try
            tb = sender
            If tb.ID = "AccountAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdd")
                AccName = dgi.FindControl("AccountNameAdd")
                FgSubled = dgi.FindControl("FgSubledAdd")
                SubLed = dgi.FindControl("SubledAdd")
                SubLedName = dgi.FindControl("SubledNameAdd")
                btnSubled = dgi.FindControl("btnSubledAdd")
                FgType = dgi.FindControl("FgTypeAdd")
                ddlCostCtr = dgi.FindControl("CostCtrAdd")
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccountEdit")
                AccName = dgi.FindControl("AccountNameEdit")
                FgSubled = dgi.FindControl("FgSubledEdit")
                SubLed = dgi.FindControl("SubledEdit")
                SubLedName = dgi.FindControl("SubledNameEdit")
                btnSubled = dgi.FindControl("btnSubledEdit")
                FgType = dgi.FindControl("FgTypeEdit")
                ddlCostCtr = dgi.FindControl("CostCtrEdit")
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            End If
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
                FgSubled.Text = "N"
                FgType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
                FgSubled.Text = dr("FgSubLed").ToString
                FgType.Text = dr("FgType").ToString
            End If
            SubLed.Enabled = (FgSubled.Text <> "N")
            btnSubled.Visible = SubLed.Enabled
            'ddlCostCtr.Enabled = (FgSubled.Text = "PL")
            ddlCostCtr.Enabled = (FgType.Text = "PL")
            ddlCostCtr.SelectedIndex = 0
            SubLed.Text = ""
            SubLedName.Text = ""
        Catch ex As Exception
            lstatus.Text = "tb Account Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim tb, SubLed As TextBox
        Dim FgSubled, SubLedName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "SubLedAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                FgSubled = dgi.FindControl("FgSubledAdd")
                SubLed = dgi.FindControl("SubledAdd")
                SubLedName = dgi.FindControl("SubledNameAdd")

                ds = SQLExecuteQuery("SELECT SubLed_No, SubLed_Name From VMsSubLed Where FgSubLed = " + QuotedStr(FgSubled.Text) + " AND SubLed_No = " + QuotedStr(SubLed.Text), ViewState("DBConnection").ToString)
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                FgSubled = dgi.FindControl("FgSubledEdit")
                SubLed = dgi.FindControl("SubledEdit")
                SubLedName = dgi.FindControl("SubledNameEdit")
                ds = SQLExecuteQuery("SELECT SubLed_No, SubLed_Name From VMsSubLed Where FgSubLed = " + QuotedStr(FgSubled.Text) + " AND SubLed_No = " + QuotedStr(SubLed.Text), ViewState("DBConnection").ToString)
            End If
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                SubLed.Text = ""
                SubLedName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                SubLed.Text = dr("SubLed_No").ToString
                SubLedName.Text = dr("SubLed_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Subled Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'vmsFormatJE','Format_Code','Format_Name','Type','Format Jurnal Entry File','Format Jurnal Code','Format Jurnal Name','Type'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try


        'Dim ReportGw As New ReportDocument
        'Dim Ds As DataSet
        'Dim crParameterFieldDefinitnions As ParameterFieldDefinitions
        'Dim crparameter1 As ParameterFieldDefinition
        'Dim crparameter1values As ParameterValues
        'Dim crDiscrete1Value As New ParameterDiscreteValue
        'Try
        'Ds = SQLExecuteQuery("Select FormatJECode AS Code, FormatJEName AS Name from MsFormatJE")
        'ReportGw.Load(Server.MapPath("~\Rpt\PrintMaster.Rpt"))

        'ReportGw.SetDataSource(Ds.Tables(0))
        'crParameterFieldDefinitnions = ReportGw.DataDefinition.ParameterFields
        'crparameter1 = crParameterFieldDefinitnions.Item("Title")
        'crparameter1values = crparameter1.CurrentValues
        'crDiscrete1Value.Value = "Format Journal Entry File"
        'crparameter1values.Add(crDiscrete1Value)
        'crparameter1.ApplyCurrentValues(crparameter1values)
        'Session("Report") = ReportGw
        'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")
        'lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        'Catch ex As Exception
        'lstatus.Text = "btn print Error = " + ex.ToString
        'End Try

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
        Dim cbxType As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("FormatJECodeAdd")
                dbName = DataGrid.FooterRow.FindControl("FormatJENameAdd")
                cbxType = DataGrid.FooterRow.FindControl("TypeAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Format Journal Entry Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Format Journal Entry Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("SELECT Format_Code From VMsFormatJE WHERE Format_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Format JE " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsFormatJE (FormatJECode, FormatJEName,Type,UserId, UserDate)" + _
                "Select " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + "," + QuotedStr(cbxType.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"


                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName, lbTypex As Label
                Dim gvr As GridViewRow
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("FormatJECode")
                lbName = gvr.FindControl("FormatJEName")
                lbTypex = gvr.FindControl("Type")
                '  lbProductCategory = GVR.FindControl("ProductCategoryAdd")
                '  lbFgStock = GVR.FindControl("FgStockAdd")
                '  lbFgDirectExpence = GVR.FindControl("FgDirectExpenceAdd")
                lbFormatJECode.Text = lbCode.Text
                lbFormatJEName.Text = lbName.Text
                lbType.Text = lbTypex.Text
                ViewState("Nmbr") = lbCode.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
                'Dim lbCode As Label
                'Dim lbName As Label
                'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                'lbCode = GVR.FindControl("ProductTypeCode")
                'lbName = GVR.FindControl("ProductTypeName")
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Product Type', '" + lbCode.Text + "|" + lbName.Text + "','AssProductt');", True)
                'End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SqlString As String
        Dim GVR As GridViewRow
        Dim debit, credit, SubLed As TextBox
        Dim btnSubled As Button
        Dim ddlCostCtr As DropDownList
        
        Try
            SqlString = "SELECT A.FormatJE, A.ItemNo, B.FgType, A.Account, A.FgSubLed, A.SubLed, dbo.FormatFloat(A.Debit,2) AS Debit, dbo.FormatFloat(A.Credit,2) AS Credit, A.UserId, A.UserDate, B.Description AS AccountName, C.Subled_Name AS SubledName, A.CostCtr, D.Cost_Ctr_Name " + _
            " FROM MsFormatJEdt A INNER JOIN VMsAccount B ON A.Account = B.Account " + _
            " LEFT OUTER JOIN VMsSubled C ON A.FgSubled = C.FgSubLed AND A.Subled = C.SubLed_No " + _
            " LEFT OUTER JOIN VMsCostCtr D ON A.CostCtr = D.Cost_Ctr_Code " + _
            " WHERE A.FormatJE =" + QuotedStr(ViewState("Nmbr").ToString) + " Order By ItemNo"

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                If ViewState("SortExpressionDt") = Nothing Then
                    ViewState("SortExpressionDt") = "ItemNo ASC "
                    ViewState("SortOrder") = "ASC"
                End If
                'DV.Sort = Session("SortExpressionDt")
                DV.Sort = (ViewState("SortExpressionDt"))
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If

            GVR = DataGridDt.FooterRow
            debit = GVR.FindControl("DebitAdd")
            debit.Attributes.Add("OnKeyDown", "return PressNumeric();")

            credit = GVR.FindControl("CreditAdd")
            credit.Attributes.Add("OnKeyDown", "return PressNumeric();")

            SubLed = GVR.FindControl("SubledAdd")
            SubLed.Enabled = False

            ddlCostCtr = GVR.FindControl("CostCtrAdd")
            ddlCostCtr.Enabled = False

            btnSubled = GVR.FindControl("btnSubledAdd")
            btnSubled.Visible = False
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("FormatJECode")

            SQLExecuteNonQuery("Delete from MsFormatJEDt where FormatJE = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsFormatJE where FormatJECode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("FormatJENameEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim cbxType As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("FormatJECodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("FormatJENameEdit")
            cbxType = DataGrid.Rows(e.RowIndex).FindControl("TypeEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "FA Location Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            'SQLString = "Update MsFormatJE set FormatJEName= '" + dbName.Text.Replace("'", "''") + "," & _
            '"Type = " + QuotedStr(cbxType.Text) + "' where FormatJECode = '" & lbCode.Text + "'"
            'lstatus.Text = SQLString

            SQLString = "Update MsFormatJE set FormatJEName= " + QuotedStr(dbName.Text) + _
            ",Type = " + QuotedStr(cbxType.Text) + " where FormatJECode = '" & lbCode.Text + "'"
            'lstatus.Text = SQLString

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
    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbAccount, tbSubled As TextBox
        Dim SQLString As String
        Dim tbFgSubled, tbFgType As Label
        Dim GVR As GridViewRow
        Dim max As Integer
        Dim dbDebit, dbCredit As TextBox
        Dim ddlCostCtr As DropDownList
        Try
            If e.CommandName = "Insert" Then
                max = CInt(SQLExecuteScalar("SELECT COALESCE(Max(ItemNo),0) FROM MsFormatJEdt WHERE FormatJE = " + QuotedStr(lbFormatJECode.Text), ViewState("DBConnection")))
                max = max + 1
                ViewState("StateDt") = "Insert"

                GVR = DataGridDt.FooterRow
                tbFgSubled = GVR.FindControl("FgSubledAdd")
                tbAccount = GVR.FindControl("AccountAdd")
                tbSubled = GVR.FindControl("SubledAdd")
                dbDebit = GVR.FindControl("DebitAdd")
                dbCredit = GVR.FindControl("CreditAdd")
                ddlCostCtr = GVR.FindControl("CostCtrAdd")
                dbDebit.Text = dbDebit.Text.Replace(",", "")
                dbCredit.Text = dbCredit.Text.Replace(",", "")
                tbFgType = GVR.FindControl("FgTypeAdd")


                If dbDebit.Text.Trim = "" Then
                    dbDebit.Text = "0"
                End If
                If dbCredit.Text.Trim = "" Then
                    dbCredit.Text = "0"
                End If
                If tbAccount.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Account must be filled")
                    tbAccount.Focus()
                    Exit Sub
                End If
                If tbFgType.Text.Trim = "PL" Then
                    If ddlCostCtr.SelectedValue.Trim = "" Then
                        lstatus.Text = MessageDlg("Cost Center must be filled")
                        ddlCostCtr.Focus()
                        Exit Sub
                    End If
                End If
                If tbFgSubled.Text.Trim <> "N" And TrimStr(tbSubled.Text) = "" Then
                    lstatus.Text = MessageDlg("Subled must be filled")
                    tbSubled.Focus()
                    Exit Sub
                End If
                If CFloat(dbDebit.Text) = 0 And CFloat(dbCredit.Text) = 0 Then
                    lstatus.Text = MessageDlg("Debit or Credit must have value")
                    dbDebit.Focus()
                    Exit Sub
                End If
                SQLString = "Insert Into MsFormatJEDt (FormatJE, ItemNo, Account, FgSubled,  Subled, debit, credit, UserId, UserDate, CostCtr) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + max.ToString + "," + _
                QuotedStr(tbAccount.Text) + "," + QuotedStr(tbFgSubled.Text) + "," + QuotedStr(tbSubled.Text) + "," + _
                dbDebit.Text + "," + dbCredit.Text + "," + QuotedStr(ViewState("UserId")) + ", GetDate(), " + QuotedStr(ddlCostCtr.SelectedValue)

                'SQLString = Replace(SQLString, "''", "NULL")
                'lstatus.Text = SQLString
                'Exit Sub
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
                'lstatus.Text = "SELECT COALESCE(Max(ItemNo),0) FROM MsFormatJEdt WHERE FormatJE = " + QuotedStr(Left(lbFormatJECode.Text, 4))



            ElseIf e.CommandName = "SearchAccountEdit" Or e.CommandName = "SearchAccountAdd" Then
                Dim FieldResult As String
                Session("filter") = "SELECT * From VMsAccount "
                FieldResult = "Account, Description, FgSubled, FgType"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAccountAdd" Then
                    ViewState("Sender") = "SearchAccountAdd"
                Else
                    ViewState("Sender") = "SearchAccountEdit"
                End If
                AttachScript("OpenSearchGrid();", Page, Me.GetType())
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                'End If
            ElseIf e.CommandName = "SearchSubledEdit" Or e.CommandName = "SearchSubledAdd" Then
                Dim FieldResult As String
                FieldResult = "Subled_No, Subled_Name"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchSubledAdd" Then
                    GVR = DataGridDt.FooterRow
                    tbFgSubled = GVR.FindControl("FgSubledAdd")
                    Session("filter") = "SELECT * FROM VMsSubled Where FgSubled = " + QuotedStr(tbFgSubled.Text) + ""
                    ViewState("Sender") = "SearchSubledAdd"
                Else
                    GVR = DataGridDt.Rows(DataGridDt.EditIndex)
                    tbFgSubled = GVR.FindControl("FgSubledEdit")
                    Session("filter") = "SELECT * FROM VMsSubled Where FgSubled = " + QuotedStr(tbFgSubled.Text) + ""
                    ViewState("Sender") = "SearchSubledEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenSearchGrid();", True)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try


    End Sub

    Dim Dr, Cr As Double
    Protected Sub DataGridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    Cr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Credit"))
                    Dr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Debit"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'Dr = GetTotalSum(ViewState("Dt"), "Debit")
                    'Cr = GetTotalSum(ViewState("Dt"), "Credit")
                    lbTotalDebit.Text = FormatNumber(Dr, 2)
                    lbTotalCredit.Text = FormatNumber(Cr, 2)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("ItemNo")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("Delete from MsFormatJEDt where FormatJE = " + QuotedStr(ViewState("Nmbr").ToString) + " AND ItemNo =" + txtID.Text,ViewState("DBConnection").ToString)
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub


    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim TxtID, tbFgSubled, tbFgType As Label
        Dim tbSubled, debit, credit As TextBox
        Dim btnSubled As Button
        Dim GVR As GridViewRow
        Dim ddlCostCtr As DropDownList

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            TxtID = DataGridDt.Rows(e.NewEditIndex).FindControl("ItemNo")
            tbFgSubled = DataGridDt.Rows(e.NewEditIndex).FindControl("FgSubled")
            tbFgType = DataGridDt.Rows(e.NewEditIndex).FindControl("FgType")

            If TxtID.Text.ToString = "" Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()

            GVR = DataGridDt.Rows(DataGridDt.EditIndex)
            tbSubled = GVR.FindControl("SubledEdit")
            btnSubled = GVR.FindControl("btnSubledEdit")
            ddlCostCtr = GVR.FindControl("CostCtrEdit")
            tbSubled.Enabled = tbFgSubled.Text <> "N"
            btnSubled.Visible = tbSubled.Enabled
            ddlCostCtr.Enabled = (tbFgType.Text = "PL")

            debit = DataGridDt.Rows(e.NewEditIndex).FindControl("DebitEdit")
            debit.Attributes.Add("OnKeyDown", "return PressNumeric();")

            credit = DataGridDt.Rows(e.NewEditIndex).FindControl("CreditEdit")
            credit.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccount, tbSubled, tbdebit, tbCredit As TextBox
        Dim lbItemNo, tbFgSubled, tbFgType As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Dim ddlCostCtr As DropDownList
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbItemNo = GVR.FindControl("ItemNoEdit")
            tbAccount = GVR.FindControl("AccountEdit")
            tbFgSubled = GVR.FindControl("FgSubledEdit")
            tbSubled = GVR.FindControl("SubledEdit")
            tbdebit = GVR.FindControl("DebitEdit")
            tbCredit = GVR.FindControl("CreditEdit")
            ddlCostCtr = GVR.FindControl("CostCtrEdit")
            tbdebit.Text = tbdebit.Text.Replace(",", "")
            tbCredit.Text = tbCredit.Text.Replace(",", "")
            tbFgType = GVR.FindControl("FgTypeEdit")
            
            If tbdebit.Text.Trim = "" Then
                tbdebit.Text = "0"
            End If
            If tbCredit.Text.Trim = "" Then
                tbCredit.Text = "0"
            End If
            If tbAccount.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Account must be filled")
                tbAccount.Focus()
                Exit Sub
            End If

            If tbFgType.Text.Trim = "PL" Then
                If ddlCostCtr.SelectedValue.Trim = "" Then
                    lstatus.Text = MessageDlg("Cost Center must be filled")
                    ddlCostCtr.Focus()
                    Exit Sub
                End If
            End If

            If tbFgSubled.Text.Trim <> "N" And TrimStr(tbSubled.Text) = "" Then
                lstatus.Text = MessageDlg("Subled must be filled")
                tbSubled.Focus()
                Exit Sub
            End If
            If CFloat(tbdebit.Text) = 0 And CFloat(tbCredit.Text) = 0 Then
                lstatus.Text = MessageDlg("Debit or Credit must have value")
                tbdebit.Focus()
                Exit Sub
            End If
            SQLString = "UPDATE MsFormatJEDt SET Account = " + QuotedStr(tbAccount.Text) + _
            ", FgSubled= " + QuotedStr(tbFgSubled.Text) + ", Subled= " + QuotedStr(tbSubled.Text) + _
            ", Debit = " + tbdebit.Text + _
            ", Credit = " + tbCredit.Text + _
            ", CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
            " WHERE FormatJE = " + QuotedStr(lbFormatJECode.Text) + " AND ItemNo =" + lbItemNo.Text
            
            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            'lstatus.Text = SQLString

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class

