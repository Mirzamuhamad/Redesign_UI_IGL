Imports System.Data

Partial Class Master_MsFaGroupSub_MsFAGroupSub
    Inherits System.Web.UI.Page
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
            Dim Acc As New TextBox
            Dim AccName As New Label
            Dim Count As Integer
            Dim dgi As GridViewRow

            If Session("Sender") = "SearchFAAdd" Then
                Acc = DataGridDt.FooterRow.FindControl("AccFAAdd")
                AccName = DataGridDt.FooterRow.FindControl("AccFANameAdd")
            ElseIf Session("Sender") = "SearchFAEdit" Then
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccFAEdit")
                AccName = dgi.FindControl("AccFANameEdit")
            ElseIf Session("Sender") = "SearchDeprAdd" Then
                Acc = DataGridDt.FooterRow.FindControl("AccDeprAdd")
                AccName = DataGridDt.FooterRow.FindControl("AccDeprNameAdd")
            ElseIf Session("Sender") = "SearchDeprEdit" Then
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccDeprEdit")
                AccName = dgi.FindControl("AccDeprNameEdit")
            ElseIf Session("Sender") = "SearchAkumDeprAdd" Then
                Acc = DataGridDt.FooterRow.FindControl("AccAkumDeprAdd")
                AccName = DataGridDt.FooterRow.FindControl("AccAkumDeprNameAdd")
            ElseIf Session("Sender") = "SearchAkumDeprEdit" Then
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccAkumDeprEdit")
                AccName = dgi.FindControl("AccAkumDeprNameEdit")
            ElseIf Session("Sender") = "SearchSalesAdd" Then
                Acc = DataGridDt.FooterRow.FindControl("AccSalesAdd")
                AccName = DataGridDt.FooterRow.FindControl("AccSalesNameAdd")
            ElseIf Session("Sender") = "SearchSalesEdit" Then
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccSalesEdit")
                AccName = dgi.FindControl("AccSalesNameEdit")
            End If

            Acc.Text = Session("Result")(0).ToString
            AccName.Text = Session("Result")(1).ToString
            Acc.Focus()

            Session("Result") = Nothing
            Session("Sender") = Nothing
            Session("filter") = Nothing
            Session("Criteria") = Nothing
            Session("Column") = Nothing
        End If
        dsCurrency.ConnectionString = ViewState("DBConnection")
        dsFAGroup.ConnectionString = ViewState("DBConnection")
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
        ViewState("FACurrency") = ViewState("Currency")
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
            SqlString = "Select A.FASubGrpCode, A.FASubGrpName, A.FAGroup, A.FGMoving, A.FgProcess, dbo.FormatFloat(A.PercentNDA,2) AS PercentNDA, A.LifeMonth, A.FgExpendable, A.MethodDepr, B.FAGroupNAme from MsFAGroupSub A INNER JOIN MsFAGroup B ON A.FAGroup = B.FAGroupCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FASubGrpName ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("Select A.*, FA.AccountName AS AccFAName, " + _
            "De.AccountName AS AccDeprName, Ak.AccountName AS AccAkumDeprName, " + _
            "SA.AccountName AS AccSalesName FROM MsFAGroupSubAcc A LEFT OUTER JOIN " + _
            "MsAccount FA on A.AccFA = FA.Account LEFT OUTER JOIN " + _
            "MsAccount De on A.AccDepr = De.Account LEFT OUTER JOIN " + _
            "MsAccount Ak on A.AccAkumDepr = Ak.Account LEFT OUTER JOIN " + _
            "MsAccount SA on A.AccSales = SA.Account WHERE A.FASubGroup =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = Session("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If

            Dim ddlCurrency As DropDownList
            ddlCurrency = DataGridDt.FooterRow.FindControl("CurrCodeAdd")

            ddlCurrency.SelectedValue = ViewState("Currency")

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("FASubGrpNameEdit")
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim lbCurr As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
            lbCurr = DataGridDt.Rows(e.NewEditIndex).FindControl("CurrCodeEdit")
            ViewState("FACurrency") = lbCurr.Text
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Edit exception : " + ex.ToString
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
        Dim dbCode, dbName, dbNDA, dbLife As TextBox
        Dim ddlFAGroup, ddlFgMoving, ddlFgProcess, ddlFgExp, ddlMethod As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("FASubGrpCodeAdd")
                dbName = GVR.FindControl("FASubGrpNameAdd")
                ddlFAGroup = GVR.FindControl("FAGroupAdd")
                ddlFgMoving = GVR.FindControl("FGMovingAdd")
                ddlFgProcess = GVR.FindControl("FGProcessAdd")
                ddlFgExp = GVR.FindControl("FgExpendableAdd")
                ddlMethod = GVR.FindControl("MethodDeprAdd")
                dbNDA = GVR.FindControl("PercentNDAAdd")
                dbLife = GVR.FindControl("LifeMonthAdd")
                dbNDA.Text = dbNDA.Text.Replace(",", "")
                dbLife.Text = dbLife.Text.Replace(",", "")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('FA Sub Group Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('FA Sub Group Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbNDA.Text) Then
                    lstatus.Text = "NDA must be numeric values."
                    dbNDA.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbLife.Text) Then
                    lstatus.Text = "Life(Month) must be numeric values."
                    dbLife.Focus()
                    Exit Sub
                End If


                If CFloat(dbNDA.Text.Trim.Length) < 0 Then
                    dbNDA.Text = "0"
                End If
                If dbLife.Text.Trim.Length = 0 Then
                    dbLife.Text = "0"
                End If

                If CFloat(dbNDA.Text) > 100 Then
                    dbNDA.Text = "100"
                End If
                If SQLExecuteScalar("SELECT FA_SubGrp_Code From VMsFAGroupSub WHERE FA_SubGrp_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "FA Sub Group " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsFAGroupSub (FASubGrpCode, FASubGrpName, FAGroup, FgMoving, FgProcess, PercentNDA, LifeMonth, FgExpendable, MethodDepr, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlFAGroup.SelectedValue) + "," + _
                QuotedStr(ddlFgMoving.SelectedValue) + "," + QuotedStr(ddlFgProcess.SelectedValue) + "," + dbNDA.Text.Replace(",", "") + "," + _
                dbLife.Text.Replace(",", "") + "," + QuotedStr(ddlFgExp.SelectedValue) + "," + QuotedStr(ddlMethod.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("FASubGrpCode")
                lbName = GVR.FindControl("FASubGrpName")
                ViewState("Nmbr") = lbCode.Text
                lbFASubGroupCode.Text = lbCode.Text + " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbAccFA, tbAccDepr, tbAccAkumDepr, tbAccSales As TextBox
        Dim ddlCurr As DropDownList
        Dim lbCurr As Label
        Dim SQLString, CurrCode As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.FooterRow
            If e.CommandName = "Insert" Then
                ddlCurr = GVR.FindControl("CurrCodeAdd")
                tbAccFA = GVR.FindControl("AccFAAdd")
                tbAccDepr = GVR.FindControl("AccDeprAdd")
                tbAccAkumDepr = GVR.FindControl("AccAkumDeprAdd")
                tbAccSales = GVR.FindControl("AccSalesAdd")

                If tbAccFA.Text.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Account FA must be filled');</script>"
                    tbAccFA.Focus()
                    Exit Sub
                End If
                If SQLExecuteScalar("SELECT Currency From VMsFAGroupSubAcc WHERE FASubGroup = " + QuotedStr(ViewState("Nmbr")) + " AND Currency = " + QuotedStr(ddlCurr.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Currency " + QuotedStr(ddlCurr.Text) + " has already been exist"
                    Exit Sub
                End If


                SQLString = "Insert Into MsFAGroupSubAcc (FASubGroup, CurrCode, AccFA, AccDepr, AccAkumDepr, AccSales, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                QuotedStr(tbAccFA.Text) + "," + QuotedStr(tbAccDepr.Text) + "," + QuotedStr(tbAccAkumDepr.Text) + "," + _
                QuotedStr(tbAccSales.Text) + "," + QuotedStr(ViewState("UserId")) + ", getDate()"

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                bindDataGridDt()
            ElseIf e.CommandName = "SearchFAEdit" Or e.CommandName = "SearchFAAdd" Then
                Dim FieldResult As String
                If e.CommandName = "SearchFAAdd" Then
                    ddlCurr = GVR.FindControl("CurrCodeAdd")
                    CurrCode = ddlCurr.Text
                Else
                    CurrCode = ViewState("FACurrency")
                End If

                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")

                If e.CommandName = "SearchFAAdd" Then
                    Session("Sender") = "SearchFAAdd"
                Else
                    Session("Sender") = "SearchFAEdit"
                End If

                Session("filter") = "Select Account, Description FROM VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','F')"

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If

            ElseIf e.CommandName = "SearchDeprEdit" Or e.CommandName = "SearchDeprAdd" Then
                Dim FieldResult As String

                If e.CommandName = "SearchDeprAdd" Then
                    ddlCurr = GVR.FindControl("CurrCodeAdd")
                    CurrCode = ddlCurr.Text
                Else
                    'lbCurr = GVR.FindControl("CurrCodeEdit")
                    CurrCode = ViewState("FACurrency")
                End If

                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")

                If e.CommandName = "SearchDeprAdd" Then
                    Session("Sender") = "SearchDeprAdd"
                Else
                    Session("Sender") = "SearchDeprEdit"
                End If
                Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgActive ='Y' And FgType = 'PL' AND FgSubled IN ('N', 'F') AND FgNormal = 'D'"

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            ElseIf e.CommandName = "SearchAkumDeprEdit" Or e.CommandName = "SearchAkumDeprAdd" Then
                Dim FieldResult As String
                FieldResult = "Account, Description"

                If e.CommandName = "SearchAkumDeprAdd" Then
                    ddlCurr = GVR.FindControl("CurrCodeAdd")
                    CurrCode = ddlCurr.Text
                Else
                    'lbCurr = GVR.FindControl("CurrCodeEdit")
                    CurrCode = ViewState("FACurrency")
                End If

                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")

                If e.CommandName = "SearchAkumDeprAdd" Then
                    Session("Sender") = "SearchAkumDeprAdd"
                Else
                    Session("Sender") = "SearchAkumDeprEdit"
                End If

                Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) and FgType = 'BS' And FgNormal = 'C' AND FgSubled IN ('N', 'F') "

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            ElseIf e.CommandName = "SearchSalesEdit" Or e.CommandName = "SearchSalesAdd" Then
                Dim FieldResult As String
                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")

                If e.CommandName = "SearchSalesAdd" Then
                    ddlCurr = GVR.FindControl("CurrCodeAdd")
                    CurrCode = ddlCurr.Text
                Else
                    'lbCurr = GVR.FindControl("CurrCodeEdit")
                    CurrCode = ViewState("FACurrency")
                    'GVR = DataGridDt.Rows(CInt(e.CommandArgument))
                    'lbCurr = GVR.FindControl("CurrCodeEdit")
                End If

                If e.CommandName = "SearchSalesAdd" Then
                    Session("Sender") = "SearchSalesAdd"
                Else
                    Session("Sender") = "SearchSalesEdit"
                End If
                Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F')"

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbNDA, dbLife As TextBox
        Dim lbCode As Label
        Dim ddlFAGroup, ddlFgMoving, ddlFgProcess, ddlFgExp, ddlMethod As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("FASubGrpCodeEdit")
            dbName = GVR.FindControl("FASubGrpNameEdit")
            ddlFAGroup = GVR.FindControl("FAGroupEdit")
            ddlFgMoving = GVR.FindControl("FGMovingEdit")
            ddlFgProcess = GVR.FindControl("FGProcessEdit")
            dbNDA = GVR.FindControl("PercentNDAEdit")
            ddlFgExp = GVR.FindControl("FgExpendableEdit")
            ddlMethod = GVR.FindControl("MethodDeprEdit")
            dbLife = GVR.FindControl("LifeMonthEdit")
            dbNDA.Text = dbNDA.Text.Replace(",", "")
            dbLife.Text = dbLife.Text.Replace(",", "")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('FA Sub Group Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If


            If Not IsNumeric(dbNDA.Text) Then
                lstatus.Text = "NDA must be numeric values."
                dbNDA.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbLife.Text) Then
                lstatus.Text = "Life(Month) must be numeric values."
                dbLife.Focus()
                Exit Sub
            End If


            If CFloat(dbNDA.Text) > 100 Then
                lstatus.Text = "<script language='javascript'>alert('NDA (%) must less than 100.');</script>"
                dbNDA.Focus()
                Exit Sub
            End If

            If CFloat(dbNDA.Text) < 0 Then
                lstatus.Text = "<script language='javascript'>alert('NDA (%) must more than or equal to 0.');</script>"
                dbNDA.Focus()
                Exit Sub
            End If

            SQLString = "Update MSFAGroupSub set FASubGrpName = " + QuotedStr(dbName.Text) + _
            ", FAGroup =" + QuotedStr(ddlFAGroup.SelectedValue) + ", FgMoving =" + QuotedStr(ddlFgMoving.SelectedValue) + _
            ", fgProcess =" + QuotedStr(ddlFgProcess.SelectedValue) + ", PercentNDA =" + dbNDA.Text.Replace(",", "") + _
            ", FgExpendable =" + QuotedStr(ddlFgExp.SelectedValue) + ", LifeMonth =" + dbLife.Text.Replace(",", "") + _
            ", MethodDepr =" + QuotedStr(ddlMethod.SelectedValue) + _
            " where FASubGrpCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccFA, tbAccDepr, tbAccAkumDepr, tbAccSales As TextBox
        Dim lbCurr As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbCurr = GVR.FindControl("CurrCodeEdit")
            tbAccFA = GVR.FindControl("AccFAEdit")
            tbAccDepr = GVR.FindControl("AccDeprEdit")
            tbAccAkumDepr = GVR.FindControl("AccAkumDeprEdit")
            tbAccSales = GVR.FindControl("AccSalesEdit")

            SQLString = "UPDATE MsFAGroupSubAcc SET ACCFA = " + QuotedStr(tbAccFA.Text) + _
            ", AccDepr= " + QuotedStr(tbAccDepr.Text) + ", AccAkumDepr= " + QuotedStr(tbAccAkumDepr.Text) + _
            ", AccSales= " + QuotedStr(tbAccSales.Text) + " WHERE Currcode = " + QuotedStr(lbCurr.Text) + _
            " AND FASubGroup =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("FASubGrpCode")
            SQLExecuteNonQuery("Delete from MSFAGroupSubAcc where FASubGroup = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MSFAGroupSub where FASubGrpCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrCode")

            SQLExecuteNonQuery("Delete from MSFAGroupSubAcc where FASubGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
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
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            Session("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccFA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim DDLCurrency As DropDownList
        Dim LBCurrency As Label
        Dim CurrCode As String
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccFAAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccFAAdd")
                AccName = dgi.FindControl("AccFANameAdd")
                DDLCurrency = dgi.FindControl("CurrCodeAdd")
                CurrCode = DDLCurrency.Text
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccFAEdit")
                AccName = dgi.FindControl("AccFANameEdit")
                LBCurrency = dgi.FindControl("CurrCodeEdit")
                CurrCode = LBCurrency.Text
            End If
            ds = SQLExecuteQuery("Select Account, Description FROM VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','F') AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc FA Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccDepr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim DDLCurrency As DropDownList
        Dim LBCurrency As Label
        Dim CurrCode As String
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccDeprAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccDeprAdd")
                AccName = dgi.FindControl("AccDeprNameAdd")
                DDLCurrency = dgi.FindControl("CurrCodeAdd")
                CurrCode = DDLCurrency.Text
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccDeprEdit")
                AccName = dgi.FindControl("AccDeprNameEdit")
                LBCurrency = dgi.FindControl("CurrCodeEdit")
                CurrCode = LBCurrency.Text
            End If


            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgActive ='Y' And FgType = 'PL' AND FgSubled IN ('N', 'F') AND FgNormal = 'D' AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc depr Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccAkumDepr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim DDLCurrency As DropDownList
        Dim LBCurrency As Label
        Dim CurrCode As String
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccAkumDeprAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccAkumDeprAdd")
                AccName = dgi.FindControl("AccAkumDeprNameAdd")
                DDLCurrency = dgi.FindControl("CurrCodeAdd")
                CurrCode = DDLCurrency.Text
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccAkumDeprEdit")
                AccName = dgi.FindControl("AccAkumDeprNameEdit")
                LBCurrency = dgi.FindControl("CurrCodeEdit")
                CurrCode = LBCurrency.Text
            End If

            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgType = 'BS' AND FgSubled IN ('N', 'F') And FgNormal = 'C' AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc depr Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccSales_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName, lbCurr As Label
        Dim Count As Integer
        Dim ddlCurr As DropDownList
        'Dim DDLCurrency As DropDownList
        'Dim LBCurrency As Label
        Dim CurrCode As String
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccSalesAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccSalesAdd")
                AccName = dgi.FindControl("AccSalesNameAdd")
                ddlCurr = dgi.FindControl("CurrCodeAdd")
                CurrCode = ddlCurr.SelectedValue
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccSalesEdit")
                AccName = dgi.FindControl("AccSalesNameEdit")
                lbCurr = dgi.FindControl("CurrCodeEdit")
                CurrCode = lbCurr.Text
            End If
            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(CurrCode) + ") OR (Fixed_Currency = 'N')) AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc depr Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub AccFAEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Dim Acc As TextBox
    '    Dim AccName As Label
    '    Dim Count As Integer
    '    Dim dgi As DataGridItem
    '    Try
    '        Count = DataGrid.EditItemIndex
    '        dgi = DataGrid.Items(Count)
    '        Acc = dgi.FindControl("AccFAEdit")
    '        AccName = dgi.FindControl("AccFANameEdit")

    '        ds = SQLExecuteQuery("Select Account, Description FROM V_MsAccount Where Currency =" + QuotedStr(Session("Currency")) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','F') AND Account = " + QuotedStr(Acc.Text))
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            Acc.Text = ""
    '            AccName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            Acc.Text = dr("Account").ToString
    '            AccName.Text = dr("Description").ToString
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "tb acc FA Edit Changed Error : " + ex.ToString
    '    End Try
    'End Sub
    'Protected Sub AccDeprEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Dim Acc As TextBox
    '    Dim AccName As Label
    '    Dim Count As Integer
    '    Dim dgi As DataGridItem
    '    Try
    '        Count = DataGrid.EditItemIndex
    '        dgi = DataGrid.Items(Count)
    '        Acc = dgi.FindControl("AccDeprEdit")
    '        AccName = dgi.FindControl("AccDeprNameEdit")

    '        ds = SQLExecuteQuery("SELECT Account, Description From V_MsAccount Where FgActive ='Y' AND  Currency = " + QuotedStr(Session("Currency")) + " And FgType = 'PL' AND FgSubled IN ('N', 'F') AND FgNormal = 'D' AND Account = " + QuotedStr(Acc.Text))
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            Acc.Text = ""
    '            AccName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            Acc.Text = dr("Account").ToString
    '            AccName.Text = dr("Description").ToString
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "tb acc Depr Edit Changed Error : " + ex.ToString
    '    End Try
    'End Sub
    'Protected Sub AccSalesEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Dim Acc As TextBox
    '    Dim AccName, lbSales As Label
    '    Dim Count As Integer
    '    Dim dgi As DataGridItem
    '    Try
    '        Count = DataGrid.EditItemIndex
    '        dgi = DataGrid.Items(Count)
    '        Acc = dgi.FindControl("AccSalesEdit")
    '        AccName = dgi.FindControl("AccSalesNameEdit")
    '        lbSales = dgi.FindControl("CurrCodeEdit")

    '        ds = SQLExecuteQuery("SELECT Account, Description From V_MsAccount Where Currency In (" + QuotedStr(lbSales.Text) + "," + QuotedStr(Session("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(Acc.Text))
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            Acc.Text = ""
    '            AccName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            Acc.Text = dr("Account").ToString
    '            AccName.Text = dr("Description").ToString
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "tb acc Sales Edit Changed Error : " + ex.ToString
    '    End Try
    'End Sub
    'Protected Sub AccAkumDeprEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Dim Acc As TextBox
    '    Dim AccName, lbCurr As Label
    '    Dim Count As Integer
    '    Dim dgi As DataGridItem
    '    Try
    '        Count = DataGrid.EditItemIndex
    '        dgi = DataGrid.Items(Count)
    '        Acc = dgi.FindControl("AccAkumDeprEdit")
    '        AccName = dgi.FindControl("AccAkumDeprNameEdit")
    '        lbCurr = dgi.FindControl("CurrCodeEdit")

    '        ds = SQLExecuteQuery("SELECT Account, Description From V_MsAccount Where Currency In (" + QuotedStr(lbCurr.Text) + "," + QuotedStr(Session("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(Acc.Text))
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            Acc.Text = ""
    '            AccName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            Acc.Text = dr("Account").ToString
    '            AccName.Text = dr("Description").ToString
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "tb acc Akum Depr Edit Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub CurrCodeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccFA, tbAccDepr, tbAccAkumDepr, tbAccSales As TextBox
        Dim lbAccFAName, lbAccDeprName, lbAccAkumDeprNAme, lbAccSalesName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

            tbAccFA = dgi.FindControl("AccFAAdd")
            lbAccFAName = dgi.FindControl("AccFANameAdd")
            tbAccDepr = dgi.FindControl("AccDeprAdd")
            lbAccDeprName = dgi.FindControl("AccDeprNameAdd")
            tbAccAkumDepr = dgi.FindControl("AccAkumDeprAdd")
            lbAccAkumDeprNAme = dgi.FindControl("AccAkumDeprNameAdd")
            tbAccSales = dgi.FindControl("AccSalesAdd")
            lbAccSalesName = dgi.FindControl("AccSalesNameAdd")

            tbAccFA.Text = ""
            lbAccFAName.Text = ""
            tbAccDepr.Text = ""
            lbAccDeprName.Text = ""
            tbAccAkumDepr.Text = ""
            lbAccAkumDeprNAme.Text = ""
            tbAccSales.Text = ""
            lbAccSalesName.Text = ""
        Catch ex As Exception
            lstatus.Text = "Curr Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsFAGroupSub A','FA_SubGrp_Code','FA_SubGrp_Name','FAGroup_Name','Fg_Moving','Fg_Process','Fixed Asset Sub Group File','FA Sub Group Code','FA Sub Group Name','FA Group Name','Moving','Process'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            'lstatus.Text = Session("SelectCommand")
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class
