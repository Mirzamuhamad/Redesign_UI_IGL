Imports System.Data

Partial Class Master_MsCustGroup_MsCustGroup
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        dsCurrency.ConnectionString = ViewState("DBConnection")
        dsPClass.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then
            Dim Acc As New TextBox
            Dim AccName As New Label

            If ViewState("Sender") = "btnAccAR" Then
                tbAccAR.Text = Session("Result")(0).ToString
                tbAccARName.Text = Session("Result")(1).ToString
            End If

            If ViewState("Sender") = "btnAccSJUninvoice" Then
                tbAccSJUninvoice.Text = Session("Result")(0).ToString
                tbAccSJUninvoiceName.Text = Session("Result")(1).ToString

            End If


            If ViewState("Sender") = "btnAccDisc" Then
                tbAccDisc.Text = Session("Result")(0).ToString
                tbAccDiscName.Text = Session("Result")(1).ToString

            End If

            If ViewState("Sender") = "btnAccOther" Then
                tbAccOther.Text = Session("Result")(0).ToString
                tbAccOtherName.Text = Session("Result")(1).ToString

            End If

            If ViewState("Sender") = "btnAccCreditAR" Then
                tbAccCreditAR.Text = Session("Result")(0).ToString
                tbAccCreditARName.Text = Session("Result")(1).ToString
            End If

            If ViewState("Sender") = "btnAccDP" Then
                tbAccDP.Text = Session("Result")(0).ToString
                tbAccDPName.Text = Session("Result")(1).ToString

            End If

            If ViewState("Sender") = "btnAccPPN" Then
                tbAccPPN.Text = Session("Result")(0).ToString
                tbAccPPNName.Text = Session("Result")(1).ToString

            End If

            If ViewState("Sender") = "btnAccDeposit" Then
                tbAccDeposit.Text = Session("Result")(0).ToString
                tbAccDepositName.Text = Session("Result")(1).ToString
            End If

            If ViewState("Sender") = "btnAccPotongan" Then
                tbAccPotongan.Text = Session("Result")(0).ToString
                tbAccPotonganName.Text = Session("Result")(1).ToString
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
        Dim tempDS As New DataSet()


        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM MsCustGroup  " + StrFilter + " ORDER BY CustGroupName ASC "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CustGroupNAme ASC"
                ViewState("SortOrder") = "ASC"
            End If


            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub



    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim SQLString As String


        Try
            'tempDS = SQLExecuteQuery(

            SQLString = "SELECT A.*, C.AccountName AS AccARName, E.AccountName AS AccCreditARName, " + _
            "F.AccountName AS AccDPName, G.AccountName AS AccDepositName, M.AccountName AS AccOtherName, " + _
            "I.AccountName AS AccPPNName, M.AccountName AS AccOtherName,P.AccountName AS AccSJUninvoiceName, K.AccountName AS AccPotonganName,  " + _
            "L.AccountName AS accdiscName FROM MsCustGroupAcc A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccAR = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.AccCreditAR = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.AccDP = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.AccDeposit = G.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.Accppn = I.Account LEFT OUTER JOIN " + _
            "MsAccount M ON A.AccOther = M.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccDisc = L.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccPotongan = K.Account LEFT OUTER JOIN " + _
            "MsAccount P ON A.AccSJUninvoice = P.Account WHERE A.CustGroup =" + QuotedStr(ViewState("Nmbr"))

            'DV = tempDS.Tables(0).DefaultView


            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            DataGridDt.DataSource = dt
            DataGridDt.DataBind()
            BindGridDt(dt, DataGridDt)
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
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("CustGroupNameEdit")
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        'Try
        'If CheckMenuLevel("Edit") = False Then
        'Exit Sub
        'End If

        Dim GVR As GridViewRow
        Try

            If SQLExecuteScalar("select CustGroup,CurrCode From VMsCustGroupAcc WHERE CustGroup = " + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString).Length = 0 Then
                lstatus.Text = "select CustGroup,CurrCode From VMsCustGroupAcc WHERE CustGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode = " + QuotedStr(ddlCurrency.Text)

                Exit Sub

            End If

            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = DataGridDt.Rows(e.NewEditIndex)



            ddlCurrency.SelectedValue = GVR.Cells(1).Text
            tbAccAR.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbAccARName.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbAccSJUninvoice.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbAccSJUninvoiceName.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbAccDisc.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbAccDiscName.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbAccOther.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbAccOtherName.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbAccCreditAR.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            tbAccCreditARName.Text = GVR.Cells(11).Text.Replace("&nbsp;", "")
            tbAccDP.Text = GVR.Cells(12).Text.Replace("&nbsp;", "")
            tbAccDPName.Text = GVR.Cells(13).Text.Replace("&nbsp;", "")
            tbAccDeposit.Text = GVR.Cells(14).Text.Replace("&nbsp;", "")
            tbAccDepositName.Text = GVR.Cells(15).Text.Replace("&nbsp;", "")
            tbAccPPN.Text = GVR.Cells(16).Text.Replace("&nbsp;", "")
            tbAccPPNName.Text = GVR.Cells(17).Text.Replace("&nbsp;", "")
            tbAccPotongan.Text = GVR.Cells(18).Text.Replace("&nbsp;", "")
            tbAccPotonganName.Text = GVR.Cells(19).Text.Replace("&nbsp;", "")

            PnlEditDetail.Visible = True
            pnlDt.Visible = False
            PanelInfo.Visible = False
            ViewState("StateDt") = "Edit"
            ddlCurrency.Focus()
            PanelInfo.Visible = True
        Catch ex As Exception
            lstatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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
            Dim GVR As GridViewRow
            Dim ddlPClass As DropDownList
            GVR = DataGridDt.FooterRow
            ddlPClass = GVR.FindControl("PClassCodeAdd")
            If lbCustType.Text = "Export" Or lbCustType.Text = "Affiliasi" Then
                ddlPClass.Visible = False
                DataGridDt.Columns(0).Visible = False
            Else
                ddlPClass.Visible = True
                DataGridDt.Columns(0).Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT CustGroup, CurrCode, AccAR, AccSJUninvoice, AccDisc, AccOther, AccCreditAR, AccDP, AccDeposit, AccPPN UserID, UserDate FROM mscustgroupacc WHERE CustGroup = '" + Nmbr + "' AND CurrCode = " + QuotedStr(ddlCurrency.SelectedValue)
    End Function

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        Dim ddlCustGroupType, ddlPKP As DropDownList

        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("CustGroupCodeAdd")
                dbName = GVR.FindControl("CustGroupNameAdd")
                ddlCustGroupType = GVR.FindControl("CustGroupTypeAdd")
                ddlPKP = GVR.FindControl("PKPAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Customer Group Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Customer Group Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("select CustGroupCode From VMsCustGroup WHERE CustGroupCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Customer Group " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsCustGroup (CustGroupCode, CustGroupName, CustGroupType, FgPKP, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlCustGroupType.SelectedValue) + "," + _
                QuotedStr(ddlPKP.SelectedValue) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName, lbCustGroupType As Label
                Dim tbCustGroupType As TextBox
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("CustGroupCode")
                lbName = GVR.FindControl("CustGroupName")
                lbCustGroupType = GVR.FindControl("CustGroupType")
                tbCustGroupType = GVR.FindControl("CustGroupTypeEdit")
                ViewState("Nmbr") = lbCode.Text
                lbCustGrouType.Text = lbCode.Text + " - " + lbName.Text
                lbCustType.Text = lbCustGroupType.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                PanelInfo.Visible = True
                bindDataGridDt()
                DataGridDt.PageIndex = 0

                'GVR = DataGridDt.FooterRow
                'If lbCustGroupType.Text = "Export" Or lbCustType.Text = "Affiliasi" Then
                ' ddlPClass.Visible = False
                'DataGridDt.Columns(0).Visible = False
                'Else
                '   ddlPClass.Visible = True
                '  DataGridDt.Columns(0).Visible = True
                ' End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            DataGridDt.DataSource = dt
            DataGridDt.DataBind()
            BindGridDt(dt, DataGridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lstatus.Text = "Detail Cust. Group Command Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlCustGroupType, ddlPKP As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("CustGroupCodeEdit")
            dbName = GVR.FindControl("CustGroupNameEdit")
            ddlCustGroupType = GVR.FindControl("CustGroupTypeEdit")
            ddlPKP = GVR.FindControl("PKPEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Customer Group Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsCustGroup set CustGroupName = " + QuotedStr(dbName.Text) + _
            ", CustGroupType =" + QuotedStr(ddlCustGroupType.SelectedValue) + ", FgPKP =" + QuotedStr(ddlPKP.SelectedValue) + " where CustGroupCode = '" & lbCode.Text + "'"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CustGroupCode")

            SQLExecuteNonQuery("Delete from MsCustGroup    where CustGroupCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsCustGroupAcc where CustGroup = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            Dim GVR As GridViewRow = DataGridDt.Rows(e.RowIndex)
            'SQLExecuteNonQuery("Delete from MsCustGroupAcc where CustGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode =" + GVR.Cells(1).Text)
            SQLExecuteNonQuery("Delete from MsCustGroupAcc where CustGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
            bindDataGridDt()
            lstatus.Text = "Data Deleted"
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


    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            ViewState("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccAR_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbAccAR.TextChanged
        Dim DsAccAR As DataSet
        Dim DrAccAR As DataRow
        Dim SQLString As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                SQLString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgNormal = 'D' and FgSubled IN ('N','C') AND Account = " + QuotedStr(tbAccAR.Text)
            Else
                SQLString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FGType = 'BS' AND FgNormal = 'D' and FgSubled IN ('N','C') AND Account = " + QuotedStr(tbAccAR.Text)
            End If
            'DsAccAR = SQLExecuteQuery("Select * FROM VMsAccount Where Currency =" + QuotedStr(ViewState("Currency")) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','C') AND Account = " + QuotedStr(tbAccAR.Text), ViewState("DBConnection").ToString)
            DsAccAR = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)
            If DsAccAR.Tables(0).Rows.Count = 1 Then
                DrAccAR = DsAccAR.Tables(0).Rows(0)
                tbAccAR.Text = DrAccAR("Account")
                tbAccARName.Text = DrAccAR("Description")
            Else
                tbAccAR.Text = ""
                tbAccARName.Text = ""
            End If
            tbAccAR.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc AR To Error : " + ex.ToString)
        End Try
    End Sub




    Protected Sub tbAccCreditAR_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCreditAR.TextChanged
        Dim DsAccCreditAR As DataSet
        Dim DrAccCreditAR As DataRow
        Dim SqlString As String
        Try

            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                SqlString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                SqlString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If
            'DsAccCreditAR = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency = " + QuotedStr(ViewState("Currency")) + " and FgType = 'BS' And FgNormal = 'C' AND Account = " + QuotedStr(tbAccCreditAR.Text), ViewState("DBConnection").ToString)

            DsAccCreditAR = SQLExecuteQuery(SqlString + " AND Account = " + QuotedStr(tbAccCreditAR.Text), ViewState("DBConnection").ToString)

            If DsAccCreditAR.Tables(0).Rows.Count = 1 Then
                DrAccCreditAR = DsAccCreditAR.Tables(0).Rows(0)
                tbAccCreditAR.Text = DrAccCreditAR("Account")
                tbAccCreditARName.Text = DrAccCreditAR("Description")
            Else
                tbAccCreditAR.Text = ""
                tbAccCreditARName.Text = ""
            End If
            tbAccCreditAR.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc Disc To Error : " + ex.ToString)
        End Try


    End Sub

    Protected Sub tbAccDP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDP.TextChanged
        Dim DsAccDP As DataSet
        Dim DrAccDP As DataRow
        Dim SQLString As String
        Try
            'DsAccDP = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency = " + QuotedStr(ViewState("Currency")) + " and FgType = 'BS' And FgNormal = 'C' AND Account = " + QuotedStr(tbAccDP.Text), ViewState("DBConnection").ToString)

            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                SQLString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                SQLString = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If

            DsAccDP = SQLExecuteQuery(SQLString + " AND Account = " + QuotedStr(tbAccDP.Text), ViewState("DBConnection").ToString)
            If DsAccDP.Tables(0).Rows.Count = 1 Then
                DrAccDP = DsAccDP.Tables(0).Rows(0)
                tbAccDP.Text = DrAccDP("Account")
                tbAccDPName.Text = DrAccDP("Description")
            Else
                tbAccDP.Text = ""
                tbAccDPName.Text = ""
            End If
            tbAccDP.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc DP To Error : " + ex.ToString)
        End Try



    End Sub

    Protected Sub tbAccDeposit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDeposit.TextChanged
        Dim DsAccDeposit As DataSet
        Dim DrAccDeposit As DataRow
        Dim SQlString As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                SQlString = "Select * FROM VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                SQlString = "Select * FROM VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If

            'DsAccDeposit = SQLExecuteQuery("SELECT * From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'BS'  And FgActive= 'Y' and FgNormal = 'C' AND FgSubled IN ('N', 'C') AND Account = " + QuotedStr(tbAccDeposit.Text), ViewState("DBConnection").ToString)
            DsAccDeposit = SQLExecuteQuery(SQlString + " AND Account = " + QuotedStr(tbAccDeposit.Text), ViewState("DBConnection").ToString)
            If DsAccDeposit.Tables(0).Rows.Count = 1 Then
                DrAccDeposit = DsAccDeposit.Tables(0).Rows(0)
                tbAccDeposit.Text = DrAccDeposit("Account")
                tbAccDepositName.Text = DrAccDeposit("Description")
            Else
                tbAccDeposit.Text = ""
                tbAccDepositName.Text = ""
            End If
            tbAccDeposit.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc Deposit To Error : " + ex.ToString)
        End Try


    End Sub

    Protected Sub tbAccppn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccPPN.TextChanged
        Dim DsAccPPN As DataSet
        Dim DrAccPPN As DataRow
        Try
            'DsAccPPN = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccPPN.Text), ViewState("DBConnection").ToString)
            DsAccPPN = SQLExecuteQuery("SELECT * From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'BS' and FgNormal = 'C' AND FgSubled IN ('N', 'C') AND FgActive='Y' AND Account = " + QuotedStr(tbAccPPN.Text), ViewState("DBConnection").ToString)
            If DsAccPPN.Tables(0).Rows.Count = 1 Then
                DrAccPPN = DsAccPPN.Tables(0).Rows(0)
                tbAccPPN.Text = DrAccPPN("Account")
                tbAccPPNName.Text = DrAccPPN("Description")
            Else
                tbAccPPN.Text = ""
                tbAccPPNName.Text = ""
            End If
            tbAccPPN.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc PPN To Error : " + ex.ToString)
        End Try



    End Sub

    Protected Sub tbAccOther_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccOther.TextChanged
        Dim DsAccOther As DataSet
        Dim DrAccOther As DataRow
        Try
            'DsAccOther = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccOther.Text), ViewState("DBConnection").ToString)
            DsAccOther = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccOther.Text), ViewState("DBConnection").ToString)
            If DsAccOther.Tables(0).Rows.Count = 1 Then
                DrAccOther = DsAccOther.Tables(0).Rows(0)
                tbAccOther.Text = DrAccOther("Account")
                tbAccOtherName.Text = DrAccOther("Description")
            Else
                tbAccOther.Text = ""
                tbAccOtherName.Text = ""
            End If
            tbAccOther.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc Disc To Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAccSJUninvoice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSJUninvoice.TextChanged
        Dim DsAccSJUninvoice As DataSet
        Dim DrAccSJUninvoice As DataRow
        Try
            'DsAccSJUninvoice = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'BS' AND Account = " + QuotedStr(tbAccSJUninvoice.Text), ViewState("DBConnection").ToString)
            DsAccSJUninvoice = SQLExecuteQuery("SELECT * From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") And FgType = 'BS' AND Account = " + QuotedStr(tbAccSJUninvoice.Text), ViewState("DBConnection").ToString)

            If DsAccSJUninvoice.Tables(0).Rows.Count = 1 Then
                DrAccSJUninvoice = DsAccSJUninvoice.Tables(0).Rows(0)
                tbAccSJUninvoice.Text = DrAccSJUninvoice("Account")
                tbAccSJUninvoiceName.Text = DrAccSJUninvoice("Description")
            Else
                tbAccSJUninvoice.Text = ""
                tbAccSJUninvoiceName.Text = ""
            End If
            tbAccSJUninvoice.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc SJ Uninvoice To Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAccDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDisc.TextChanged

        Dim DsAccDisc As DataSet
        Dim DrAccDisc As DataRow
        Try
            'DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            If DsAccDisc.Tables(0).Rows.Count = 1 Then
                DrAccDisc = DsAccDisc.Tables(0).Rows(0)
                tbAccDisc.Text = DrAccDisc("Account")
                tbAccDiscName.Text = DrAccDisc("Description")
            Else
                tbAccDisc.Text = ""
                tbAccDiscName.Text = ""
            End If
            tbAccDisc.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc Disc To Error : " + ex.ToString)
        End Try


    End Sub
    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles DataGrid.Sorting
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

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, Button2.Click
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
    Protected Sub BtnCanceldt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCanceldt.Click
        Try
            pnlDt.Visible = True
            PanelInfo.Visible = True
            PnlEditDetail.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
    '    'Dim Count As Integer
    '    'Dim dgi As GridViewRow
    '    Dim tbAccAR, tbAccSales, tbAccSRetur, tbAccCOGS, tbAccCreditAR, tbAccDP, tbAccDeposit, tbAccppn, tbAccOther, tbAccDisc As TextBox
    '    Dim lbAccFAName, lbAccSalesName, lbAccSReturName, lbAccCOGSName, lbAccCreditARNAme, lbAccDpName, lbAccDepositName, lbAccppnName, lbAccOtherName, lbAccDiscName As Label
    '    Try
    '        'Count = DataGridDt.Controls(0).Controls.Count
    '        'dgi = DataGridDt.FooterRow

    '        'tbAccAR = dgi.FindControl("AccARAdd")
    '        'lbAccFAName = dgi.FindControl("AccARNameAdd")
    '        'tbAccSales = dgi.FindControl("AccSalesAdd")
    '        'tbAccSRetur = dgi.FindControl("AccSReturAdd")
    '        'tbAccCOGS = dgi.FindControl("AccCOGSAdd")
    '        'lbAccSalesName = dgi.FindControl("AccSalesNameAdd")
    '        'lbAccSReturName = dgi.FindControl("AccSReturNameAdd")
    '        'lbAccCOGSName = dgi.FindControl("AccCOGSNameAdd")
    '        'tbAccCreditAR = dgi.FindControl("AccCreditARAdd")
    '        'lbAccCreditARNAme = dgi.FindControl("AccCreditARNameAdd")
    '        'tbAccDP = dgi.FindControl("AccDPAdd")
    '        'lbAccDpName = dgi.FindControl("AccDPNameAdd")
    '        'tbAccDeposit = dgi.FindControl("AccDepositAdd")
    '        'lbAccDepositName = dgi.FindControl("AccDepositNameAdd")
    '        'tbAccppn = dgi.FindControl("AccppnAdd")
    '        'lbAccppnName = dgi.FindControl("AccppnNameAdd")
    '        'tbAccOther = dgi.FindControl("AccOtherAdd")
    '        'lbAccOtherName = dgi.FindControl("AccOtherNameAdd")
    '        'tbAccDisc = dgi.FindControl("AccDiscAdd")
    '        'lbAccDiscName = dgi.FindControl("AccDiscNameAdd")

    '        tbAccAR.Text = ""
    '        lbAccFAName.Text = ""
    '        tbAccSales.Text = ""
    '        tbAccCOGS.Text = ""
    '        tbAccSRetur.Text = ""
    '        lbAccSalesName.Text = ""
    '        lbAccSReturName.Text = ""
    '        lbAccCOGSName.Text = ""
    '        tbAccCreditAR.Text = ""
    '        lbAccCreditARNAme.Text = ""
    '        tbAccDP.Text = ""
    '        lbAccDpName.Text = ""
    '        tbAccDeposit.Text = ""
    '        lbAccDepositName.Text = ""
    '        tbAccppn.Text = ""
    '        lbAccppnName.Text = ""
    '        tbAccOther.Text = ""
    '        lbAccOtherName.Text = ""
    '        tbAccDisc.Text = ""
    '        lbAccDiscName.Text = ""

    '    Catch ex As Exception
    '        lstatus.Text = "P Class Code Add Index Changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub CurrCodeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccAR, tbAccCreditAR, tbAccDP, tbAccDeposit, tbAccppn, tbAccOther, tbAccDisc As TextBox
        Dim lbAccFAName, lbAccCreditARNAme, lbAccDpName, lbAccDepositName, lbAccppnName, lbAccOtherName, lbAccDiscName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            dgi = DataGridDt.FooterRow

            tbAccAR = dgi.FindControl("AccARAdd")
            lbAccFAName = dgi.FindControl("AccARNameAdd")
            tbAccCreditAR = dgi.FindControl("AccCreditARAdd")
            lbAccCreditARNAme = dgi.FindControl("AccCreditARNameAdd")
            tbAccDP = dgi.FindControl("AccDPAdd")
            lbAccDpName = dgi.FindControl("AccDPNameAdd")
            tbAccDeposit = dgi.FindControl("AccDepositAdd")
            lbAccDepositName = dgi.FindControl("AccDepositNameAdd")
            tbAccppn = dgi.FindControl("AccppnAdd")
            lbAccppnName = dgi.FindControl("AccppnNameAdd")
            tbAccOther = dgi.FindControl("AccOtherAdd")
            lbAccOtherName = dgi.FindControl("AccOtherNameAdd")
            tbAccDisc = dgi.FindControl("AccDiscAdd")
            lbAccDiscName = dgi.FindControl("AccDiscNameAdd")

            tbAccAR.Text = ""
            lbAccFAName.Text = ""
            tbAccCreditAR.Text = ""
            lbAccCreditARNAme.Text = ""
            tbAccDP.Text = ""
            lbAccDpName.Text = ""
            tbAccDeposit.Text = ""
            lbAccDepositName.Text = ""
            tbAccppn.Text = ""
            lbAccppnName.Text = ""
            tbAccOther.Text = ""
            lbAccOtherName.Text = ""
            tbAccDisc.Text = ""
            lbAccDiscName.Text = ""

        Catch ex As Exception
            lstatus.Text = "Curr Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FormPrintMaster4 'MsCustGroup','CustGroupCode','CustGroupName','CustGroupType','fgPKP','Customer Group File','Customer Group Code','Customer Group Name','Customer Group Type Name','PKP'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try

    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, BtnAdd2.Click
        Try
            ViewState("StateDt") = "Insert"
            ClearHd()
            pnlDt.Visible = False
            
            PnlEditDetail.Visible = True
            ModifyInput(True)

        Catch ex As Exception
            lstatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            ddlCurrency.SelectedValue = ViewState("Currency")


            tbAccAR.Text = ""
            tbAccCreditAR.Text = ""
            tbAccDisc.Text = ""
            tbAccOther.Text = ""
            tbAccDeposit.Text = ""
            tbAccDP.Text = ""
            tbAccPPN.Text = ""
            tbAccSJUninvoice.Text = ""


            tbAccARName.Text = ""
            tbAccCreditARName.Text = ""
            tbAccDiscName.Text = ""
            tbAccOtherName.Text = ""
            tbAccDepositName.Text = ""
            tbAccDPName.Text = ""
            tbAccPPNName.Text = ""
            tbAccSJUninvoiceName.Text = ""
            tbAccPotongan.Text = ""
            tbAccPotonganName.Text = ""

            'btnSaveHd.Visible = True
            'btnCancelHd.Visible = True
            'btnReset.Visible = True

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        Try
            ddlCurrency.Enabled = State
            'tbName.Enabled = State
            tbAccAR.Enabled = State
            tbAccPPN.Enabled = State

            tbAccSJUninvoice.Enabled = State
            tbAccCreditAR.Enabled = State
            tbAccDisc.Enabled = State
            tbAccOther.Enabled = State
            tbAccDeposit.Enabled = State
            tbAccDP.Enabled = State
            tbAccPPN.Enabled = State
            'btnSaveHd.Enabled = State
            'btnCancelHd.Enabled = State
            'tbCEmail.Enabled = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAccDisc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDisc.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F')"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccDisc"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAccAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAR.Click
        Dim FieldResult As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgNormal = 'D' and FgSubled IN ('N','C')"
            Else
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FGType = 'BS' AND FgNormal = 'D' and FgSubled IN ('N','C')"
            End If
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccAR"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. AR Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAccCreditAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccCreditAR.Click
        Dim FieldResult As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccCreditAR"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. Credit AR Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAccDP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDP.Click
        Dim FieldResult As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                Session("filter") = "Select * FROM VMsAccount Where Currency =" + QuotedStr(ddlCurrency.SelectedValue) + " AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccDP"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. DP Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccOther_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccOther.Click
        Dim FieldResult As String
        Try

            Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ")  and FgNormal = 'C' AND FgSubled IN ('N', 'F')"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccOther"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. DP Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAccDeposit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDeposit.Click
        Dim FieldResult As String
        Try
            If ddlCurrency.SelectedValue = ViewState("Currency") Then
                Session("filter") = "Select * FROM VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") AND FgActive = 'Y' and FgNormal = 'C' and FgSubled IN ('N','C')"
            Else
                Session("filter") = "Select * FROM VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'C' and FgSubled IN ('N','C')"
            End If
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccDeposit"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. Deposit Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAccPotongan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPotongan.Click
        Dim FieldResult As String
        Try
            Try
                Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N', 'F')"
                FieldResult = "Account, Description"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                ViewState("Sender") = "btnAccPotongan"
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            Catch ex As Exception
                lstatus.Text = "btn Search Product Error : " + ex.ToString
            End Try
        Catch ex As Exception
            lstatus.Text = "btn Acc. Deposit Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccPPN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPPN.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'BS' and FgNormal = 'C' AND FgSubled IN ('N', 'C') AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccPPN"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. Deposit Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSJUninvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSJUninvoice.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT * From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + ") And FgType = 'BS' "
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccSJUninvoice"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Acc. SJ Uninvoice Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSavedt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavedt.Click
        Dim SQLString As String
        Try

            If tbAccAR.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Account AR must be filled');</script>"
                tbAccAR.Focus()
                Exit Sub
            End If



            If ViewState("StateDt") = "Insert" Then


                If SQLExecuteScalar("select CustGroup,CurrCode From VMsCustGroupAcc WHERE CustGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode = " + QuotedStr(ddlCurrency.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Customer Group Detail " + QuotedStr(lbCustGrouType.Text) + " - " + QuotedStr(ddlCurrency.Text) + " has already been exist"
                    Exit Sub
                End If





                'If lbCustType.Text = "Export" Then
                SQLString = "Insert Into MsCustGroupAcc (CustGroup, CurrCode, AccAR, AccCreditAR, AccSJUninvoice, AccDP, AccDeposit, AccPPn, AccOther, AccDisc,AccPotongan, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + ", " + QuotedStr(ddlCurrency.SelectedValue) + "," + _
                QuotedStr(tbAccAR.Text) + "," + QuotedStr(tbAccCreditAR.Text) + "," + QuotedStr(tbAccSJUninvoice.Text) + "," + _
                QuotedStr(tbAccDP.Text) + "," + QuotedStr(tbAccDeposit.Text) + "," + _
                QuotedStr(tbAccPPN.Text) + "," + QuotedStr(tbAccOther.Text) + "," + _
                QuotedStr(tbAccDisc.Text) + "," + QuotedStr(tbAccPotongan.Text) + "," + QuotedStr(ViewState("UserId")) + ", getDate()"
                'Else
                '    SQLString = "Insert Into MsCustGroupAcc (CustGroup, CurrCode, AccAR, AccCreditAR, AccDP, AccDeposit, AccPPn, AccOther, AccDisc, UserId, UserDate) " + _
                '    "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlCurrency.SelectedValue) + "," + _
                '    QuotedStr(tbAccAR.Text) + "," + QuotedStr(tbAccCreditAR.Text) + "," + _
                '    QuotedStr(tbAccDP.Text) + "," + QuotedStr(tbAccDeposit.Text) + "," + _
                '    QuotedStr(tbAccPPN.Text) + "," + QuotedStr(tbAccOther.Text) + "," + _
                '    QuotedStr(tbAccDisc.Text) + "," + QuotedStr(ViewState("UserId")) + ", getDate()"

                'End If



            Else
                SQLString = "UPDATE MsCustGroupAcc SET AccAR = " + QuotedStr(tbAccAR.Text) + _
                ", AccCreditAR= " + QuotedStr(tbAccCreditAR.Text) + ", AccSJUninvoice= " + QuotedStr(tbAccSJUninvoice.Text) + _
                ", AccDP= " + QuotedStr(tbAccDP.Text) + ", AccDeposit= " + QuotedStr(tbAccDeposit.Text) + _
                ", AccPpn= " + QuotedStr(tbAccPPN.Text) + ", AccOther= " + QuotedStr(tbAccOther.Text) + _
                ", AccPotongan= " + QuotedStr(tbAccPotongan.Text) + _
                ", AccDisc= " + QuotedStr(tbAccDisc.Text) + " WHERE Currcode = " + QuotedStr(ddlCurrency.SelectedValue) + _
                " AND CustGroup =" + QuotedStr(ViewState("Nmbr"))

            End If



            'lstatus.Text = ViewState("StateDt") + " " + lbCustType.Text
            'Exit Sub

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridDt()
            BtnCanceldt_Click(btnCanceldt, Nothing)





        Catch ex As Exception
            lstatus.Text = "Insert Detail Error" + ex.ToString
        End Try


    End Sub


    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        Try
            tbAccAR.Text = ""
            tbAccCreditAR.Text = ""
            tbAccDisc.Text = ""
            tbAccOther.Text = ""
            tbAccDeposit.Text = ""
            tbAccDP.Text = ""
            tbAccPPN.Text = ""
            tbAccSJUninvoice.Text = ""


            tbAccARName.Text = ""
            tbAccCreditARName.Text = ""
            tbAccDiscName.Text = ""
            tbAccOtherName.Text = ""
            tbAccDepositName.Text = ""
            tbAccDPName.Text = ""
            tbAccPPNName.Text = ""
            tbAccSJUninvoiceName.Text = ""

        Catch ex As Exception

        End Try
    End Sub
End Class
