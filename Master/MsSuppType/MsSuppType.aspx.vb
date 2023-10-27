Imports System.Data

Partial Class Master_MsSuppType_MsSuppType
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
            FillCombo(ddlCurrCodeDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        End If
        dsCurrency.ConnectionString = ViewState("DBConnection")
        If Not Session("Result") Is Nothing Then
            Dim Acc As New TextBox
            Dim AccName As New Label

            If ViewState("Sender") = "btnAccAP" Then
                tbAccAP.Text = Session("Result")(0).ToString
                tbAccAPName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccAPPending" Then
                tbAccAPPending.Text = Session("Result")(0).ToString
                tbAccAPPendingName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccDebitAP" Then
                tbAccDebitAP.Text = Session("Result")(0).ToString
                tbAccDebitAPName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccDP" Then
                tbAccDP.Text = Session("Result")(0).ToString
                tbAccDPName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccDeposit" Then
                tbAccDeposit.Text = Session("Result")(0).ToString
                tbAccDepositName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccVariantPO" Then
                tbAccVariantPO.Text = Session("Result")(0).ToString
                tbAccVariantPOName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccPPN" Then
                tbAccPPN.Text = Session("Result")(0).ToString
                tbAccPPNName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccFreight" Then
                tbAccFreight.Text = Session("Result")(0).ToString
                tbAccFreightName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccOther" Then
                tbAccOther.Text = Session("Result")(0).ToString
                tbAccOtherName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccPPH" Then
                tbAccPPH.Text = Session("Result")(0).ToString
                tbAccPPHName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "btnAccDisc" Then
                tbAccDisc.Text = Session("Result")(0).ToString
                tbAccDiscName.Text = Session("Result")(1).ToString
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
            SqlString = "SELECT * FROM MsSuppType " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "SuppTypeCode ASC"
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
            tempDS = SQLExecuteQuery("SELECT A.*, C.AccountName AS AccAPName, D.AccountName AS AccAPPendingName, E.AccountName AS AccDebitAPName, " + _
            "F.AccountName AS AccDPName, G.AccountName AS AccDepositName, H.AccountName AS AccVariantPOName, M.AccountName AS AccOtherName, " + _
            "I.AccountName AS AccPPNName, J.AccountName AS AccFreightName, K.AccountName AS AccPPHName, M.AccountName AS AccOtherName, " + _
            "L.AccountName AS accdiscName FROM MsSuppTypeAcc A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccAP = C.Account LEFT OUTER JOIN " + _
            "MsAccount D ON A.AccAPPending = D.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.AccDebitAP = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.AccDP = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.AccDeposit = G.Account LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccVariantPO = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccPPN = I.Account LEFT OUTER JOIN " + _
            "MsAccount M ON A.AccOther = M.Account LEFT OUTER JOIN " + _
            "MsAccount J ON A.accFreight = J.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.accpph = K.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.accdisc = L.Account WHERE A.SuppType =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                lstatus.Text = "No Data"
                DataGridDt.Visible = False
                btnAdd2.Visible = False
                Button2.Visible = False
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DataGridDt.Visible = True
                btnAdd2.Visible = True
                Button2.Visible = True

                If ViewState("SortExpressionDt") = Nothing Then
                    ViewState("SortExpressionDt") = "CurrCode DESC"
                    ViewState("SortOrder") = "ASC"
                End If

                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
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
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("SuppTypeNameEdit")
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGridDt.Rows(e.NewEditIndex)

            pnlDt.Visible = False
            PanelInfo.Visible = True
            pnlInputDt.Visible = True
            ddlCurrCodeDt.Enabled = False

            ddlCurrCodeDt.Text = obj.Cells(0).Text
            BindToText(tbAccAP, obj.Cells(1).Text)
            BindToText(tbAccAPName, obj.Cells(2).Text)
            BindToText(tbAccAPPending, obj.Cells(3).Text)
            BindToText(tbAccAPPendingName, obj.Cells(4).Text)
            BindToText(tbAccDebitAP, obj.Cells(5).Text)
            BindToText(tbAccDebitAPName, obj.Cells(6).Text)
            BindToText(tbAccDP, obj.Cells(7).Text)
            BindToText(tbAccDPName, obj.Cells(8).Text)
            BindToText(tbAccDeposit, obj.Cells(9).Text)
            BindToText(tbAccDepositName, obj.Cells(10).Text)
            BindToText(tbAccVariantPO, obj.Cells(11).Text)
            BindToText(tbAccVariantPOName, obj.Cells(12).Text)
            BindToText(tbAccPPN, obj.Cells(13).Text)
            BindToText(tbAccPPNName, obj.Cells(14).Text)
            BindToText(tbAccFreight, obj.Cells(15).Text)
            BindToText(tbAccFreightName, obj.Cells(16).Text)
            BindToText(tbAccOther, obj.Cells(17).Text)
            BindToText(tbAccOtherName, obj.Cells(18).Text)
            BindToText(tbAccPPH, obj.Cells(19).Text)
            BindToText(tbAccPPHName, obj.Cells(20).Text)
            BindToText(tbAccDisc, obj.Cells(21).Text)
            BindToText(tbAccDiscName, obj.Cells(22).Text)
            ViewState("State") = "Edit"
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
        Dim ddlGroupType As DropDownList
        Dim GVR As GridViewRow
        
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("SuppTypeCodeAdd")
                dbName = GVR.FindControl("SuppTypeNameAdd")
                ddlGroupType = GVR.FindControl("GroupTypeAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Supplier Type Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Supplier Type Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Supp_Type_Code From VMsSuppType WHERE Supp_Type_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Supplier Type " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                ''insert the new entry
                SQLString = "Insert into MsSuppType (SuppTypeCode, SuppTypeName, GroupType, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlGroupType.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("SuppTypeCode")
                lbName = GVR.FindControl("SuppTypeName")
                ViewState("Nmbr") = lbCode.Text
                lbGroupTypeCode.Text = lbCode.Text + " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                PanelInfo.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        'Dim tbAccAP, tbAccAPPending, tbAccDebitAP, tbAccDP, tbAccDeposit, tbAccVariantPO, tbAccPPN, tbAccFreight, tbAccOther, tbAccpph, tbAccDisc As TextBox
        Dim ddlCurr As DropDownList = New DropDownList
        'Dim SQLString As String
        'Dim lbcurr As Label
        'Dim GVR As GridViewRow
        Try            
                        
            If e.CommandName = "Insert" Then
                'GVR = DataGridDt.FooterRow
                'ddlCurr = GVR.FindControl("CurrCodeAdd")
                'tbAccAP = GVR.FindControl("AccAPAdd")
                'tbAccAPPending = GVR.FindControl("AccAPPendingAdd")
                'tbAccDebitAP = GVR.FindControl("AccDebitAPAdd")
                'tbAccDP = GVR.FindControl("AccDPAdd")
                'tbAccDeposit = GVR.FindControl("AccDepositAdd")
                'tbAccVariantPO = GVR.FindControl("AccVariantPOAdd")
                'tbAccPPN = GVR.FindControl("AccPPNAdd")
                'tbAccOther = GVR.FindControl("AccOtherAdd")
                'tbAccFreight = GVR.FindControl("AccFreightAdd")
                'tbAccpph = GVR.FindControl("AccpphAdd")
                'tbAccDisc = GVR.FindControl("AccDiscAdd")

                'If tbAccAP.Text.Trim = "" Then
                '    lstatus.Text = "<script language='javascript'>alert('Account AP must be filled');</script>"
                '    tbAccAP.Focus()
                '    Exit Sub
                'End If

                'If SQLExecuteScalar("SELECT SuppType, Currency, ProductType From VMsSuppTypeAcc WHERE SuppType = " + QuotedStr(ViewState("Nmbr").ToString) + _
                '" AND Currency = " + QuotedStr(ddlCurr.SelectedValue), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Supplier Type " + QuotedStr(ViewState("Nmbr").ToString) + " Currency " + QuotedStr(ddlCurr.SelectedValue) + " has already been exist"
                '    Exit Sub
                'End If

                'SQLString = "Insert Into MsSuppTypeAcc (SuppType, CurrCode, AccAP, AccAPPending, AccDebitAP, AccDP, AccDeposit, AccVariantPO, AccPPN, AccFreight, AccOther, AccPPh, AccDisc, UserId, UserDate) " + _
                '"SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                'QuotedStr(tbAccAP.Text) + "," + QuotedStr(tbAccAPPending.Text) + "," + QuotedStr(tbAccDebitAP.Text) + "," + _
                'QuotedStr(tbAccDP.Text) + "," + QuotedStr(tbAccDeposit.Text) + "," + QuotedStr(tbAccVariantPO.Text) + "," + _
                'QuotedStr(tbAccOther.Text) + "," + QuotedStr(tbAccOther.Text) + "," + QuotedStr(tbAccFreight.Text) + "," + _
                'QuotedStr(tbAccDisc.Text) + "," + QuotedStr(tbAccpph.Text) + "," + QuotedStr(Session("UserId")) + ", GETDATE()"

                'SQLString = Replace(SQLString, "''", "NULL")
                'SQLExecuteNonQuery(SQLString)
                'bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlGroupType As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("SuppTypeCodeEdit")
            dbName = GVR.FindControl("SuppTypeNameEdit")
            ddlGroupType = GVR.FindControl("GroupTypeEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Supplier Type Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsSuppType set SuppTypeName = " + QuotedStr(dbName.Text) + _
            ", GroupType =" + QuotedStr(ddlGroupType.SelectedValue) + " where SuppTypeCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        'Dim tbAccAP, tbAccAPPending, tbAccDebitAP, tbAccDP, tbAccDeposit, tbAccVariantPO, tbAccPPN, tbAccFreight, tbAccOther, tbAccpph, tbAccDisc As TextBox
        'Dim lbCurr As Label
        'Dim SQLString As String
        'Dim GVR As GridViewRow
        Try
            'GVR = DataGridDt.Rows(e.RowIndex)
            'lbCurr = GVR.FindControl("CurrCodeEdit")
            'tbAccAP = GVR.FindControl("AccAPEdit")
            'tbAccAPPending = GVR.FindControl("AccAPPendingEdit")
            'tbAccDebitAP = GVR.FindControl("AccDebitAPEdit")
            'tbAccDP = GVR.FindControl("AccDPEdit")
            'tbAccDeposit = GVR.FindControl("AccDepositEdit")
            'tbAccVariantPO = GVR.FindControl("AccVariantPOEdit")
            'tbAccPPN = GVR.FindControl("AccPPNEdit")
            'tbAccFreight = GVR.FindControl("AccFreightEdit")
            'tbAccOther = GVR.FindControl("AccOtherEdit")
            'tbAccpph = GVR.FindControl("AccPphEdit")
            'tbAccDisc = GVR.FindControl("AccDiscEdit")

            'SQLString = "UPDATE MsSuppTypeAcc SET ACCAP = " + QuotedStr(tbAccAP.Text) + _
            '", AccAPPending= " + QuotedStr(tbAccAPPending.Text) + ", AccDebitAP= " + QuotedStr(tbAccDebitAP.Text) + _
            '", AccDP= " + QuotedStr(tbAccDP.Text) + ", AccDeposit= " + QuotedStr(tbAccDeposit.Text) + _
            '", AccVariantPO= " + QuotedStr(tbAccVariantPO.Text) + ", AccPPN= " + QuotedStr(tbAccPPN.Text) + _
            '", AccFreight= " + QuotedStr(tbAccFreight.Text) + ", AccOther= " + QuotedStr(tbAccOther.Text) + _
            '", Accpph= " + QuotedStr(tbAccpph.Text) + _
            '", AccDisc= " + QuotedStr(tbAccDisc.Text) + " WHERE Currcode = " + QuotedStr(lbCurr.Text) + _
            '" AND SuppType =" + QuotedStr(ViewState("Nmbr"))

            'SQLString = Replace(SQLString, "''", "NULL")
            'SQLExecuteNonQuery(SQLString)

            'DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'DataGridDt.EditIndex = -1
            'bindDataGridDt()
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("SuppTypeCode")

            SQLExecuteNonQuery("Delete from MsSuppTypeAcc where SuppType = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsSuppType where SuppTypeCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGridDt.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrCode")
            
            SQLExecuteNonQuery("Delete from MsSuppTypeAcc where SuppType = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode = '" & GVR.Cells(0).Text & "'", ViewState("DBConnection").ToString)
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

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            PanelInfo.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            PanelInfo.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'MsSupptype','SuppTypeCode','SuppTypeName','grouptype','Supplier Type File','Supplier Type Code','Supplier Type Name','Group Type'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnAccAP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAP.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE ((Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','S') 
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccAP"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccAP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccAP.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE ((Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccAP.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccAP.Text = ""
                tbAccAPName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccAP.Text = dr("Account").ToString
                tbAccAPName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccAP_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrCodeDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrCodeDt.SelectedIndexChanged
        Try
            tbAccAP.Text = ""
            tbAccAPName.Text = ""
            tbAccAPPending.Text = ""
            tbAccAPPendingName.Text = ""
            tbAccDebitAP.Text = ""
            tbAccDebitAPName.Text = ""
            tbAccDP.Text = ""
            tbAccDPName.Text = ""
            tbAccDeposit.Text = ""
            tbAccDepositName.Text = ""
            tbAccVariantPO.Text = ""
            tbAccVariantPOName.Text = ""
            tbAccPPN.Text = ""
            tbAccPPNName.Text = ""
            tbAccFreight.Text = ""
            tbAccFreightName.Text = ""
            tbAccOther.Text = ""
            tbAccOtherName.Text = ""
            tbAccPPH.Text = ""
            tbAccPPHName.Text = ""
            tbAccDisc.Text = ""
            tbAccDiscName.Text = ""

        Catch ex As Exception
            lstatus.Text = "ddlCurrCodeDt_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccAPPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAPPending.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN  (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") And FgType = 'BS' AND FgSubled IN ('N', 'S') AND FgNormal = 'C'
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccAPPending"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccAPPending_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccAPPending_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccAPPending.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccAPPending.Text), ViewState("DBConnection").ToString)
            'Currency IN  (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") And FgType = 'BS' AND FgSubled IN ('N', 'S') AND FgNormal = 'C' 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccAPPending.Text = ""
                tbAccAPPendingName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccAPPending.Text = dr("Account").ToString
                tbAccAPPendingName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccAPPending_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDebitAP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDebitAP.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' And FgNormal = 'D' AND FgSubled in ('N', 'S')
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccDebitAP"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccDebitAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDebitAP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDebitAP.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDebitAP.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' And FgNormal = 'D' AND FgSubled in ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDebitAP.Text = ""
                tbAccDebitAPName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDebitAP.Text = dr("Account").ToString
                tbAccDebitAPName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccDebitAP_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDeposit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDeposit.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccDeposit"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccDeposit_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDeposit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDeposit.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDeposit.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDeposit.Text = ""
                tbAccDepositName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDeposit.Text = dr("Account").ToString
                tbAccDepositName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccDeposit_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDisc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDisc.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'S') 
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccDisc"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccDisc_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDisc.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDisc.Text = ""
                tbAccDiscName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDisc.Text = dr("Account").ToString
                tbAccDiscName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccDisc_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDP.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgSubLed in ('N','S') AND FgType = 'BS' And FgNormal = 'D'
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccDP"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccDP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDP.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDP.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgSubLed in ('N','S') AND FgType = 'BS' And FgNormal = 'D'
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDP.Text = ""
                tbAccDPName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDP.Text = dr("Account").ToString
                tbAccDPName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccDP_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccFreight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccFreight.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'C' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccFreight"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccFreight_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccFreight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccFreight.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccFreight.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'C' AND FgSubled IN ('N', 'S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccFreight.Text = ""
                tbAccFreightName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccFreight.Text = dr("Account").ToString
                tbAccFreightName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccFreight_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccOther_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccOther.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccOther"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccOther_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccOther_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccOther.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccOther.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'D' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccOther.Text = ""
                tbAccOtherName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccOther.Text = dr("Account").ToString
                tbAccOtherName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccOther_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccPPH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPPH.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccPPH"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccPPH_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccPPH_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccPPH.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccPPH.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccPPH.Text = ""
                tbAccPPHName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccPPH.Text = dr("Account").ToString
                tbAccPPHName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccPPH_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccPPN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPPN.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccPPN"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccPPN_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccPPN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccPPN.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccPPN.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccPPN.Text = ""
                tbAccPPNName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccPPN.Text = dr("Account").ToString
                tbAccPPNName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccPPN_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccVariantPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccVariantPO.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccVariantPO"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnAccVariantPO_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccVariantPO_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccVariantPO.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccVariantPO.Text), ViewState("DBConnection").ToString)
            'QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' AND FgSubled IN ('N', 'S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccVariantPO.Text = ""
                tbAccVariantPOName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccVariantPO.Text = dr("Account").ToString
                tbAccVariantPOName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccVariantPO_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlDt.Visible = False
            PanelInfo.Visible = True
            pnlInputDt.Visible = True
            ViewState("State") = "Insert"
            ddlCurrCodeDt.Enabled = True
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearInput()
        Try
            tbAccAP.Text = ""
            tbAccAPName.Text = ""
            tbAccAPPending.Text = ""
            tbAccAPPendingName.Text = ""
            tbAccDebitAP.Text = ""
            tbAccDebitAPName.Text = ""
            tbAccDP.Text = ""
            tbAccDPName.Text = ""
            tbAccDeposit.Text = ""
            tbAccDepositName.Text = ""
            tbAccVariantPO.Text = ""
            tbAccVariantPOName.Text = ""
            tbAccPPN.Text = ""
            tbAccPPNName.Text = ""
            tbAccFreight.Text = ""
            tbAccFreightName.Text = ""
            tbAccOther.Text = ""
            tbAccOtherName.Text = ""
            tbAccPPH.Text = ""
            tbAccPPHName.Text = ""
            tbAccDisc.Text = ""
            tbAccDiscName.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInputDt.Visible = False
            pnlDt.Visible = True
            PanelInfo.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbAccAP.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If tbAccAP.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Account AP must be filled');</script>"
                tbAccAP.Focus()
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT SuppType, Currency From VMsSuppTypeAcc WHERE SuppType = " + QuotedStr(ViewState("Nmbr").ToString) + _
                " AND Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Supplier Type " + QuotedStr(ViewState("Nmbr").ToString) + " Currency " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert Into MsSuppTypeAcc (SuppType, CurrCode, AccAP, AccAPPending, AccDebitAP, AccDP, AccDeposit, AccVariantPO, AccPPN, AccFreight, AccOther, AccPPh, AccDisc, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + "," + _
                QuotedStr(tbAccAP.Text) + "," + QuotedStr(tbAccAPPending.Text) + "," + QuotedStr(tbAccDebitAP.Text) + "," + _
                QuotedStr(tbAccDP.Text) + "," + QuotedStr(tbAccDeposit.Text) + "," + QuotedStr(tbAccVariantPO.Text) + "," + _
                QuotedStr(tbAccOther.Text) + "," + QuotedStr(tbAccOther.Text) + "," + QuotedStr(tbAccFreight.Text) + "," + _
                QuotedStr(tbAccDisc.Text) + "," + QuotedStr(tbAccPPH.Text) + "," + QuotedStr(ViewState("UserId")) + ", GETDATE()"
            Else
                SqlString = "UPDATE MsSuppTypeAcc SET ACCAP = " + QuotedStr(tbAccAP.Text) + _
                ", AccAPPending= " + QuotedStr(tbAccAPPending.Text) + ", AccDebitAP= " + QuotedStr(tbAccDebitAP.Text) + _
                ", AccDP= " + QuotedStr(tbAccDP.Text) + ", AccDeposit= " + QuotedStr(tbAccDeposit.Text) + _
                ", AccVariantPO= " + QuotedStr(tbAccVariantPO.Text) + ", AccPPN= " + QuotedStr(tbAccPPN.Text) + _
                ", AccFreight= " + QuotedStr(tbAccFreight.Text) + ", AccOther= " + QuotedStr(tbAccOther.Text) + _
                ", Accpph= " + QuotedStr(tbAccPPH.Text) + _
                ", AccDisc= " + QuotedStr(tbAccDisc.Text) + " WHERE Currcode = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + _
                " AND SuppType =" + QuotedStr(ViewState("Nmbr"))
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGridDt()
            pnlInputDt.Visible = False
            pnlDt.Visible = True
            PanelInfo.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

End Class
