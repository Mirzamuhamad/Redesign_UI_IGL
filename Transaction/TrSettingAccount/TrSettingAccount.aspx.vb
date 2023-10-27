Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrSettingAccount_TrSettingAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()

                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                MV1.Visible = True
                MV1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True

                'Product Type ==========================================================================================
                ViewState("SortExpression") = Nothing
                FillCombo(ddlWrhsType, "EXEC S_GetWrhsType", False, "WrhsType", "WrhsType", ViewState("DBConnection"))
                FillCombo(ddlProductBentuk, "select DISTINCT MateriCode, MateriName from MsProductMateri", False, "MateriCode", "MateriName", ViewState("DBConnection"))
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

                'Customer Group ========================================================================================
                ViewState("SortExpressionCustGroup") = Nothing
                FillCombo(ddlCustGroup, "select CustGroupCode, CustGroupName from MsCustGroup", False, "CustGroupCode", "CustGroupName", ViewState("DBConnection"))
                'DataGridCustGroup.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGridDtCustGroup.PageSize = CInt(ViewState("PageSizeGrid"))
                'DataGridCustGroup.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGridDtCustGroup.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnPrintCustGroup.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

                'Supplier Type =========================================================================================
                ViewState("SortExpressionSuppType") = Nothing
                FillCombo(ddlSuppType, "select SuppTypeCode, SuppTypeName from MsSuppType", False, "SuppTypeCode", "SuppTypeName", ViewState("DBConnection"))
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGridDtSuppType.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGridDtSuppType.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                FillCombo(ddlCurrCodeDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

                'FA Sub Group ==========================================================================================
                ViewState("SortExpressionFASubGroup") = Nothing
                FillCombo(ddlFASubGroup, "select FASubGrpCode, FASubGrpName from MsFAGroupSub", False, "FASubGrpCode", "FASubGrpName", ViewState("DBConnection"))
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGridDtFASubGroup.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGridDtFASubGroup.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                FillCombo(ddlCurrDtFASubGroup, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

                'Setup Account==========================================================================================
                ViewState("SortExpressionSetupAcc") = Nothing
                'bindDataGridSetupAcc()
            End If

            dsProductCategory.ConnectionString = ViewState("DBConnection")

            dsCurrency.ConnectionString = ViewState("DBConnection")
            dsPClass.ConnectionString = ViewState("DBConnection")

            If Not Session("Result") Is Nothing Then
                'Product Type==============================================================================
                If ViewState("Sender") = "btnAccInvent" Then
                    tbAccInvent.Text = Session("Result")(0).ToString
                    tbAccInventName.Text = Session("Result")(1).ToString
                    tbAccInvent.Focus()
                End If
                If ViewState("Sender") = "btnAccCOGS" Then
                    tbAccCOGS.Text = Session("Result")(0).ToString
                    tbAccCOGSName.Text = Session("Result")(1).ToString
                    tbAccCOGS.Focus()
                End If
                If ViewState("Sender") = "btnAccTransitSJ" Then
                    tbAccTransitSJ.Text = Session("Result")(0).ToString
                    tbAccTransitSJName.Text = Session("Result")(1).ToString
                    tbAccTransitSJ.Focus()
                End If
                If ViewState("Sender") = "btnAccSales" Then
                    tbAccSales.Text = Session("Result")(0).ToString
                    tbAccSalesName.Text = Session("Result")(1).ToString
                    tbAccSales.Focus()
                End If
                If ViewState("Sender") = "btnAccTransitWrhs" Then
                    tbAccTransitWrhs.Text = Session("Result")(0).ToString
                    tbAccTransitWrhsName.Text = Session("Result")(1).ToString
                    tbAccTransitWrhs.Focus()
                End If
                If ViewState("Sender") = "btnAccSRetur" Then
                    tbAccSRetur.Text = Session("Result")(0).ToString
                    tbAccSReturName.Text = Session("Result")(1).ToString
                    tbAccTransitRetur.Focus()
                End If

                If ViewState("Sender") = "btnAccTransitReject" Then
                    tbAccTransitReject.Text = Session("Result")(0).ToString
                    tbAccTransitRejectName.Text = Session("Result")(1).ToString
                    btnAccTransitReject.Focus()
                End If

                If ViewState("Sender") = "btnAccTransitRetur" Then
                    tbAccTransitRetur.Text = Session("Result")(0).ToString
                    tbAccTransitReturName.Text = Session("Result")(1).ToString
                    tbAccTransitRetur.Focus()
                End If

                If ViewState("Sender") = "AccSRetur" Then
                    tbAccSRetur.Text = Session("Result")(0).ToString
                    tbAccSReturName.Text = Session("Result")(1).ToString
                    tbAccSRetur.Focus()
                End If

                If ViewState("Sender") = "btnAccExpLoss" Then
                    tbAccExpLoss.Text = Session("Result")(0).ToString
                    tbAccExpLossName.Text = Session("Result")(1).ToString
                    tbAccExpLoss.Focus()
                End If

                'Customer Group============================================================================
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
                If ViewState("Sender") = "btnAccDiscCustGroup" Then
                    tbAccDiscCustGroup.Text = Session("Result")(0).ToString
                    tbAccDiscNameCustGroup.Text = Session("Result")(1).ToString
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

                'Supplier Type=============================================================================
                Dim AccSuppType As New TextBox
                Dim AccNameSuppType As New Label

                If ViewState("Sender") = "btnAccAP" Then
                    tbAccAP.Text = Session("Result")(0).ToString
                    tbAccAPName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccAPPending" Then
                    tbAccAPPending.Text = Session("Result")(0).ToString
                    tbAccAPPendingName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccDebitAP" Then
                    tbAccDebitAP.Text = Session("Result")(0).ToString
                    tbAccDebitAPName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccDPSuppType" Then
                    tbAccDPSuppType.Text = Session("Result")(0).ToString
                    tbAccDPNameSuppType.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccDepositSuppType" Then
                    tbAccDepositSuppType.Text = Session("Result")(0).ToString
                    tbAccDepositNameSuppType.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccVariantPO" Then
                    tbAccVariantPO.Text = Session("Result")(0).ToString
                    tbAccVariantPOName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccPPNSuppType" Then
                    tbAccPPNSuppType.Text = Session("Result")(0).ToString
                    tbAccPPNNameSuppType.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccFreight" Then
                    tbAccFreight.Text = Session("Result")(0).ToString
                    tbAccFreightName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccOtherSuppType" Then
                    tbAccOtherSuppType.Text = Session("Result")(0).ToString
                    tbAccOtherNameSuppType.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccPPH" Then
                    tbAccPPH.Text = Session("Result")(0).ToString
                    tbAccPPHName.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccDiscSuppType" Then
                    tbAccDiscSuppType.Text = Session("Result")(0).ToString
                    tbAccDiscNameSuppType.Text = Session("Result")(1).ToString
                End If

                'FA Sub Group==============================================================================
                If ViewState("Sender") = "btnAccFASubGroup" Then
                    tbAccFASubGroup.Text = Session("Result")(0).ToString
                    tbAccFANameFASubGroup.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccDeprFASubGroup" Then
                    tbAccDeprFASubGroup.Text = Session("Result")(0).ToString
                    tbAccDeprNameFASubGroup.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccAkumDeprFASubGroup" Then
                    tbAccAkumDeprFASubGroup.Text = Session("Result")(0).ToString
                    tbAccAkumDeprNameFASubGroup.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnAccSalesFASubGroup" Then
                    tbAccSalesFASubGroup.Text = Session("Result")(0).ToString
                    tbAccSalesNameFASubGroup.Text = Session("Result")(1).ToString
                End If

                'Setup Account=============================================================================
                If ViewState("Sender") = "btnSearchAcc" Then
                    tbSetupAcc.Text = Session("Result")(0).ToString
                    tbSetupAccName.Text = Session("Result")(1).ToString
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Criteria") = Nothing
                Session("Column") = Nothing
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "Form Load Error : " + ex.ToString
        End Try
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MV1.ActiveViewIndex = Int32.Parse(e.Item.Value)

        Dim i As Integer
        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then

            ElseIf Menu1.Items(1).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                Catch ex As Exception
                    lbstatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try
            ElseIf Menu1.Items(4).Selected = True Then
                bindDataGridSetupAcc()
            End If
        Next
    End Sub

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Insert" Then
                If ViewState("FgInsert") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

#Region "Region Product Type============================================================================================================"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGridDt.PageIndex = 0
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "btnSearch_Click Error : " + vbCrLf + ex.ToString
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
            lbstatus.Text = "btnExpand_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrintPT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintPT.Click
        Dim SQLString As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "exec S_MsProductMateriAccRpt " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsProductMateriAcc.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "btnPrintPT_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim StrFilter, SqlString As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'SqlString = "Select * from MsProductMateri " + StrFilter
            SqlString = "SELECT A.*, B.MateriCode As MateriCode, B.MateriName As MateriNAme, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitReturName, I.AccountName AS AccTransitRejectName, " + _
            "L.AccountName AS AccSReturName, K.AccountName As AccSalesName, " + _
            "R.AccountName AS AccExpLossName FROM MsProductMateriDt A LEFT OUTER JOIN " + _
            " MsProductMateri B ON A.ProductMateri = B.MateriCode LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitReject = I.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSRetur = L.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccExpLoss = R.Account  " + StrFilter
            '"WHERE A.MateriCode =" + QuotedStr(ViewState("Nmbr"))', ViewState("DBConnection").ToString)

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
        Catch ex As Exception
            lbstatus.Text = "bindDataGridDt Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridDt.PreRender

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
            lbstatus.Text = "DataGridDt_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Private Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_RowCancelingEdit Error : " + vbCrLf + ex.ToString
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
                    ViewState("Nmbr") = GVR.Cells(1).Text
                    FillTextBoxDt(GVR.Cells(1).Text, GVR.Cells(3).Text) ', GVR.Cells(4).Text)
                    ModifyInputDt(True)
                    ddlProductBentuk.Enabled = False
                    ddlWrhsType.Enabled = False
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    'PanelInfo.Visible = True
                    pnlInputDt.Visible = True
                    pnlCari.Visible = False
                    'FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                End If
                If DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If

                        'SQLExecuteNonQuery("Delete from MsProductMateriDt where MateriCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("Delete from MsProductMateriDt where ProductMateri = " + QuotedStr(GVR.Cells(1).Text) + " AND WrhsType =" + QuotedStr(GVR.Cells(3).Text), ViewState("DBConnection").ToString)
                        bindDataGridDt()
                        lbstatus.Text = MessageDlg("Data Deleted")
                    Catch ex As Exception
                        lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccInvent, tbAccCogs, tbAccTransitSJ, tbAccSales, tbAccTransitWrhs, tbAccTransitReject, tbAccSRetur, tbAccTransitRetur, tbAccExpLoss As TextBox
        Dim lbWrhsType As Label
        Dim SQLString As String
        Dim GVR As GridViewRow

        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbWrhsType = GVR.FindControl("WrhsTypeEdit")
            tbAccInvent = GVR.FindControl("AccInventEdit")
            tbAccCogs = GVR.FindControl("AccCogsEdit")
            tbAccSales = GVR.FindControl("AccSalesEdit")
            tbAccTransitWrhs = GVR.FindControl("AccTransitWrhsEdit")
            tbAccTransitRetur = GVR.FindControl("AccTransitReturEdit")
            tbAccTransitSJ = GVR.FindControl("AccTransitSJEdit")
            tbAccCogs = GVR.FindControl("AccCOGSEdit")
            tbAccExpLoss = GVR.FindControl("AccExpLossEdit")
            tbAccSRetur = GVR.FindControl("AccSReturEdit")
            tbAccTransitReject = GVR.FindControl("AccTransitRejectEdit")

            SQLString = "UPDATE MsProductMateriDt SET ACCInvent = " + QuotedStr(tbAccInvent.Text) + _
            ", AccTransitSj= " + QuotedStr(tbAccTransitSJ.Text) + _
            ", AccTransitWrhs= " + QuotedStr(tbAccTransitWrhs.Text) + _
            ", AccSales= " + QuotedStr(tbAccSales.Text) + _
            ", AccTransitRetur = " + QuotedStr(tbAccTransitRetur.Text) + _
            ", AccCOGS = " + QuotedStr(tbAccCogs.Text) + _
            ", AccExpLoss = " + QuotedStr(tbAccExpLoss.Text) + _
            ", AccSRetur = " + QuotedStr(tbAccSRetur.Text) + _
            ", AccTransitReject = " + QuotedStr(tbAccTransitReject.Text) + _
            " WHERE WrhsType = " + QuotedStr(lbWrhsType.Text) + _
            " AND ProductMateri =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_RowUpdating Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        'Dim txtID As Label
        'Try
        '    If CheckMenuLevel("Delete") = False Then
        '        Exit Sub
        '    End If
        '    txtID = DataGridDt.Rows(e.RowIndex).FindControl("WrhsType")

        '    SQLExecuteNonQuery("Delete from MsProductMateriDt where MateriCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(txtID.Text))
        '    bindDataGridDt()

        'Catch ex As Exception
        '    lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        'End Try
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
            lbstatus.Text = "DataGridDt_Sorting Error =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub WrhsTypeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccInvent, tbAccCogs, tbAccsj, tbAccSales, tbAccDisc, tbAccWrhs, tbAccTransitPRetur, tbAccTransitSRetur, tbAccPReturn, tbAccSReturn, tbAccSTCAdjust, tbAccSTCLost, tbAccSampleExps As TextBox
        Dim lbAccInventName, lbAccCogsName, lbAccsjName, lbAccSalesName, lbAccDiscName, lbAccWrhsName, lbAccTransitPReturName, lbAccTransitSReturName, lbAccPReturnName, lbAccSReturnName, lbAccSTCAdjustName, lbAccSTCLostName, lbAccSampleExpsName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

            tbAccInvent = dgi.FindControl("AccInventAdd")
            lbAccInventName = dgi.FindControl("AccInventNameAdd")
            tbAccCogs = dgi.FindControl("AccCogsAdd")
            lbAccCogsName = dgi.FindControl("AccCogsNameAdd")
            tbAccsj = dgi.FindControl("AccsjAdd")
            lbAccsjName = dgi.FindControl("AccsjNameAdd")
            tbAccSales = dgi.FindControl("AccSalesAdd")
            lbAccSalesName = dgi.FindControl("AccSalesNameAdd")
            tbAccDisc = dgi.FindControl("AccDiscAdd")
            lbAccDiscName = dgi.FindControl("AccDiscNameAdd")
            tbAccWrhs = dgi.FindControl("AccWrhsAdd")
            lbAccWrhsName = dgi.FindControl("AccWrhsNameAdd")
            tbAccTransitPRetur = dgi.FindControl("AccTransitPReturAdd")
            lbAccTransitPReturName = dgi.FindControl("AccTransitPReturNameAdd")
            tbAccTransitSRetur = dgi.FindControl("AccTransitSReturAdd")
            lbAccTransitSReturName = dgi.FindControl("AccTransitSReturNameAdd")
            tbAccPReturn = dgi.FindControl("AccPReturnAdd")
            lbAccPReturnName = dgi.FindControl("AccPReturnNameAdd")
            tbAccSReturn = dgi.FindControl("AccSReturnAdd")
            lbAccSReturnName = dgi.FindControl("AccSReturnNameAdd")
            tbAccSTCAdjust = dgi.FindControl("AccSTCAdjustAdd")
            lbAccSTCAdjustName = dgi.FindControl("AccSTCAdjustNameAdd")
            tbAccSTCLost = dgi.FindControl("AccSTCLostAdd")
            lbAccSTCLostName = dgi.FindControl("AccSTCLostNameAdd")
            tbAccSampleExps = dgi.FindControl("AccSampleExpsAdd")
            lbAccSampleExpsName = dgi.FindControl("AccSampleExpsNameAdd")

            tbAccInvent.Text = ""
            lbAccInventName.Text = ""
            tbAccCogs.Text = ""
            lbAccCogsName.Text = ""
            tbAccsj.Text = ""
            lbAccsjName.Text = ""
            tbAccSales.Text = ""
            lbAccSalesName.Text = ""
            tbAccDisc.Text = ""
            lbAccDiscName.Text = ""
            tbAccWrhs.Text = ""
            lbAccWrhsName.Text = ""
            tbAccTransitPRetur.Text = ""
            lbAccTransitPReturName.Text = ""
            tbAccTransitSRetur.Text = ""
            lbAccTransitSReturName.Text = ""
            tbAccPReturn.Text = ""
            lbAccPReturnName.Text = ""
            tbAccSReturn.Text = ""
            lbAccSReturnName.Text = ""
            tbAccSTCAdjust.Text = ""
            lbAccSTCAdjustName.Text = ""
            tbAccSTCLost.Text = ""
            lbAccSTCLostName.Text = ""
            tbAccSampleExps.Text = ""
            lbAccSampleExpsName.Text = ""

        Catch ex As Exception
            lbstatus.Text = "WrhsTypeAdd_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Nmbr As String, ByVal Code As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery("SELECT A.*, B.MateriCode As MateriCode, B.MateriName As MateriNAme, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitReturName, I.AccountName AS AccTransitRejectName, " + _
            "L.AccountName AS AccSReturName, K.AccountName As AccSalesName, " + _
            "R.AccountName AS AccExpLossName FROM MsProductMateriDt A LEFT OUTER JOIN " + _
            " MsProductMateri B ON A.ProductMateri = B.MateriCode LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitReject = I.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSRetur = L.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccExpLoss = R.Account WHERE A.ProductMateri =" + QuotedStr(Nmbr) + " AND WrhsType = " + QuotedStr(Code), ViewState("DBConnection").ToString).Tables(0).Rows(0)
            ClearDt()

            BindToText(tbAccInvent, dr("AccInvent").ToString)
            BindToText(tbAccInventName, dr("AccInventName").ToString)
            BindToText(tbAccCOGS, dr("acccogs").ToString)
            BindToText(tbAccCOGSName, dr("acccogsName").ToString)
            BindToText(tbAccTransitSJ, dr("acctransitsj").ToString)
            BindToText(tbAccTransitSJName, dr("acctransitsjName").ToString)
            BindToText(tbAccTransitWrhs, dr("acctransitwrhs").ToString)
            BindToText(tbAccTransitWrhsName, dr("acctransitwrhsName").ToString)
            BindToText(tbAccSales, dr("AccSales").ToString)
            BindToText(tbAccSalesName, dr("AccSalesName").ToString)
            BindToText(tbAccTransitSJ, dr("acctransitsj").ToString)
            BindToText(tbAccTransitSJName, dr("acctransitsjName").ToString)
            BindToText(tbAccTransitWrhs, dr("acctransitwrhs").ToString)
            BindToText(tbAccTransitWrhsName, dr("acctransitwrhsName").ToString)
            BindToText(tbAccSales, dr("AccSales").ToString)
            BindToText(tbAccSalesName, dr("AccSalesName").ToString)
            BindToText(tbAccTransitRetur, dr("AccTransitRetur").ToString)
            BindToText(tbAccTransitReturName, dr("AccTransitReturName").ToString)
            BindToText(tbAccTransitReject, dr("AccTransitReject").ToString)
            BindToText(tbAccTransitRejectName, dr("AccTransitRejectName").ToString)
            BindToText(tbAccSRetur, dr("AccSRetur").ToString)
            BindToText(tbAccSReturName, dr("AccSReturName").ToString)
            BindToText(tbAccExpLoss, dr("AccExpLoss").ToString)
            BindToText(tbAccExpLossName, dr("AccExpLossName").ToString)
            BindToDropList(ddlWrhsType, dr("WrhsType").ToString)
            BindToDropList(ddlProductBentuk, dr("MateriCode").ToString)

            ddlWrhsType.Focus()
        Catch ex As Exception
            lbstatus.Text = "FillTextBoxDt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        ViewState("State") = "Insert"
        ClearDt()
        ModifyInputDt(True)
        pnlDt.Visible = False
        'PanelInfo.Visible = True
        pnlInputDt.Visible = True
        btnSave.Visible = True
        btnReset.Visible = True
        ddlProductBentuk.Focus()
        'ddlWrhsType.Focus()
        pnlCari.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ClearDt()
        pnlDt.Visible = True
        'PanelInfo.Visible = True
        pnlInputDt.Visible = False
        pnlCari.Visible = True
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ClearDt()
    End Sub

    Private Sub ClearDt()
        Try
            ddlProductBentuk.SelectedIndex = 0
            ddlWrhsType.SelectedIndex = 0
            tbAccInvent.Text = ""
            tbAccInventName.Text = ""
            tbAccCOGS.Text = ""
            tbAccCOGSName.Text = ""
            tbAccTransitSJ.Text = ""
            tbAccTransitSJName.Text = ""
            tbAccSales.Text = ""
            tbAccSalesName.Text = ""
            tbAccTransitWrhs.Text = ""
            tbAccTransitWrhsName.Text = ""
            tbAccTransitSJ.Text = ""
            tbAccTransitSJName.Text = ""

            tbAccSales.Text = ""
            tbAccSalesName.Text = ""

            tbAccTransitWrhs.Text = ""
            tbAccTransitWrhsName.Text = ""

            tbAccTransitRetur.Text = ""
            tbAccTransitReturName.Text = ""

            tbAccTransitReject.Text = ""
            tbAccTransitRejectName.Text = ""

            tbAccSRetur.Text = ""
            tbAccSReturName.Text = ""

            tbAccExpLoss.Text = ""
            tbAccExpLossName.Text = ""
        Catch ex As Exception
            lbstatus.Text = "ClearDt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub _Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccInvent.Click
        Dim FieldResult As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            If ddlWrhsType.SelectedValue = "Reject" Then
                Session("filter") = "SELECT * From VMsAccount Where FgType ='PL' And FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            Else
                Session("filter") = "SELECT * From VMsAccount Where FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            End If

            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccInvent"
        Catch ex As Exception
            lbstatus.Text = "btnAccInvent_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccCOGS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccCOGS.Click
        Dim FieldResult As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccCOGS"
        Catch ex As Exception
            lbstatus.Text = "btnAccCOGS_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitSJ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitSJ.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * From V_MsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' And Currency = (" + QuotedStr(ViewState("Currency")) + ")  "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitSJ"
        Catch ex As Exception
            lbstatus.Text = "btnAccTransitSJ Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSales_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSales.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D'  AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSales"
        Catch ex As Exception
            lbstatus.Text = "btnAccSales Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitReject.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitReject"
        Catch ex As Exception
            lbstatus.Text = "btnAccTransitReject Error : " + ex.ToString
        End Try
        'FgType = 'PL' and FgNormal = 'C'
    End Sub

    Protected Sub btnAccTransitWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitWrhs.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N','P') And FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitWrhs"
        Catch ex As Exception
            lbstatus.Text = "btnAccTransitWrhs Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitRetur"
        Catch ex As Exception
            lbstatus.Text = "btnAccTransitPRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSRetur"
        Catch ex As Exception
            lbstatus.Text = "btnAccTransitSRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccExpLoss_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccExpLoss.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccExpLoss"
        Catch ex As Exception
            lbstatus.Text = "btnAccSReturn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccInvent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccInvent.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            If ddlWrhsType.SelectedValue = "Reject" Then
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where FgType = 'PL' AND FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccInvent.Text), ViewState("DBConnection").ToString)
            Else
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccInvent.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccInvent.Text = ""
                tbAccInventName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccInvent, dr("Account").ToString)
                BindToText(tbAccInventName, dr("Description").ToString)
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccInvent_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccCOGS_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCOGS.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccCOGS.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccCOGS.Text = ""
                tbAccCOGSName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccCOGS, dr("Account").ToString)
                BindToText(tbAccCOGSName, dr("Description").ToString)
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccCOGS_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitSJ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitSJ.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccTransitSJ.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitSJ.Text = ""
                tbAccTransitSJName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitSJ, dr("Account").ToString)
                BindToText(tbAccTransitSJName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccTransitSJ Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSales_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSales.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D'  AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccSales.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSales.Text = ""
                tbAccSalesName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSales, dr("Account").ToString)
                BindToText(tbAccSalesName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccSales Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitWrhs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitWrhs.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N','P') And FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitWrhs.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitWrhs.Text = ""
                tbAccTransitWrhsName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitWrhs, dr("Account").ToString)
                BindToText(tbAccTransitWrhsName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccTransitWrhs Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitReject.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'C' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitRetur.Text = ""
                tbAccTransitReturName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitRetur, dr("Account").ToString)
                BindToText(tbAccTransitReturName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccTransitRetur Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccTransitReject_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitReject.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND Account = " + QuotedStr(tbAccTransitReject.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitReject.Text = ""
                tbAccTransitRejectName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitReject, dr("Account").ToString)
                BindToText(tbAccTransitRejectName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccPReturn Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbAccSRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSRetur.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccSRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSRetur.Text = ""
                tbAccSReturName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSRetur, dr("Account").ToString)
                BindToText(tbAccSReturName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccPReturn Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccExpLoss_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccExpLoss.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y' AND Account = " + QuotedStr(tbAccExpLoss.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccExpLoss.Text = ""
                tbAccExpLossName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccExpLoss, dr("Account").ToString)
                BindToText(tbAccExpLossName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lbstatus.Text = "tbAccSTCAdjust Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""

        Try
            If tbAccInvent.Text.Trim = "" Then
                lbstatus.Text = "<script language='javascript'>alert('Account Invent must be filled');</script>"
                tbAccInvent.Focus()
                Exit Sub
            End If

            'lbstatus.Text = QuotedStr(ViewState("Nmbr"))
            'Exit Sub

            If ViewState("State") = "Insert" Then
                'If SQLExecuteScalar("SELECT WrhsType From MsProductMateriDt WHERE MateriCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(ddlWrhsType.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                '    lbstatus.Text = "Product Type" + QuotedStr(ViewState("Nmbr")) + " Warehouse Type " + QuotedStr(ddlWrhsType.Text) + " has already been exist"
                '    Exit Sub
                'End If
                If SQLExecuteScalar("SELECT WrhsType From MsProductMateriDt WHERE ProductMateri = " + QuotedStr(ddlProductBentuk.SelectedValue) + " AND WrhsType =" + QuotedStr(ddlWrhsType.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Product Type " + QuotedStr(ddlProductBentuk.SelectedItem.ToString) + " And Warehouse Type " + QuotedStr(ddlWrhsType.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert Into MsProductMateriDt (ProductMateri, WrhsType, AccInvent, AccSales, AccCOGS, AccTransitSJ, AccTransitWrhs, AccTransitReject, AccTransitRetur, AccSRetur, AccExpLoss, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ddlProductBentuk.SelectedValue) + ", " + QuotedStr(ddlWrhsType.SelectedValue) + "," + _
                QuotedStr(tbAccInvent.Text) + "," + QuotedStr(tbAccSales.Text) + "," + _
                QuotedStr(tbAccCOGS.Text) + "," + QuotedStr(tbAccTransitSJ.Text) + "," + _
                QuotedStr(tbAccTransitWrhs.Text) + "," + QuotedStr(tbAccTransitReject.Text) + "," + _
                QuotedStr(tbAccTransitRetur.Text) + "," + QuotedStr(tbAccSRetur.Text) + "," + _
                QuotedStr(tbAccExpLoss.Text) + "," + _
                QuotedStr(ViewState("UserId")) + ", GetDate()"
            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsProductMateriDt SET AccInvent = " + QuotedStr(tbAccInvent.Text) + _
                ", AccSales= " + QuotedStr(tbAccSales.Text) + _
                ", AccCogs= " + QuotedStr(tbAccCOGS.Text) + _
                ", AccTransitSj= " + QuotedStr(tbAccTransitSJ.Text) + _
                ", AccTransitWrhs= " + QuotedStr(tbAccTransitWrhs.Text) + _
                ", AccTransitReject= " + QuotedStr(tbAccTransitReject.Text) + _
                ", AccTransitRetur= " + QuotedStr(tbAccTransitRetur.Text) + _
                ", AccSRetur= " + QuotedStr(tbAccSRetur.Text) + _
                ", AccExpLoss = " + QuotedStr(tbAccExpLoss.Text) + _
                " WHERE WrhsType = " + QuotedStr(ddlWrhsType.SelectedValue) + _
                " AND ProductMateri =" + QuotedStr(ViewState("Nmbr"))
            End If

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridDt()
            pnlInputDt.Visible = False
            pnlDt.Visible = True
            pnlCari.Visible = True
            'PanelInfo.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnSave_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ModifyInputDt(ByVal State As Boolean)
        ddlProductBentuk.Enabled = State
        ddlWrhsType.Enabled = State
        tbAccInvent.Enabled = State
        tbAccCOGS.Enabled = State
        tbAccTransitSJ.Enabled = State
        tbAccSales.Enabled = State
        tbAccTransitWrhs.Enabled = State
        tbAccTransitSJ.Enabled = State
        tbAccSales.Enabled = State
        tbAccTransitWrhs.Enabled = State
        tbAccTransitRetur.Enabled = State
        tbAccSRetur.Enabled = State
        tbAccExpLoss.Enabled = State
        tbAccSRetur.Enabled = State
        tbAccExpLoss.Enabled = State
    End Sub

    'Protected Sub btnAccWIPFOH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPFOH.Click
    '    Dim FieldResult As String

    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
    '        FieldResult = "Account, Description"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnAccWIPFOH"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnAccWIPFOH_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnAccWIPLabor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPLabor.Click
    '    Dim FieldResult As String

    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
    '        FieldResult = "Account, Description"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnAccWIPLabor"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnAccWIPLabor_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnAccWIPLabor2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPLabor2.Click
    '    Dim FieldResult As String

    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
    '        FieldResult = "Account, Description"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnAccWIPLabor2"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnAccWIPLabor2_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbAccWIPFOH_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPFOH.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPFOH.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            tbAccWIPFOH.Text = ""
    '            tbAccWIPFOHName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(tbAccWIPFOH, dr("Account").ToString)
    '            BindToText(tbAccWIPFOHName, dr("Description").ToString)
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "tbAccWIPFOH_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbAccWIPLabor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPLabor.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPLabor.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            tbAccWIPLabor.Text = ""
    '            tbAccWIPLaborName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(tbAccWIPLabor, dr("Account").ToString)
    '            BindToText(tbAccWIPLaborName, dr("Description").ToString)
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "tbAccWIPLabor_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbAccWIPLabor2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPLabor2.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPLabor2.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            tbAccWIPLabor2.Text = ""
    '            tbAccWIPLabor2Name.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(tbAccWIPLabor2, dr("Account").ToString)
    '            BindToText(tbAccWIPLabor2Name, dr("Description").ToString)
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "tbAccWIPLabor2_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub
#End Region

#Region "Region Customer Group=========================================================================================================="
    Protected Sub btnSearchCustGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchCustGroup.Click
        Try
            DataGridDtCustGroup.PageIndex = 0
            DataGridDtCustGroup.EditIndex = -1
            bindDataGridDtCustGroup()
        Catch ex As Exception
            lbstatus.Text = "btnSearchCustGroup_Click Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpandCustGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpandCustGroup.Click
        Try
            tbfilter2CustGroup.Text = ""
            If pnlSearchCustGroup.Visible Then
                pnlSearchCustGroup.Visible = False
            Else
                pnlSearchCustGroup.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btnExpandCustGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrintCG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCG.Click
        Dim SQLString As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "exec S_MsCustGroupAccRpt " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsCustGroupAcc.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "btnPrintCG_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDtCustGroup()
        Dim tempDS As New DataSet()
        Dim SQLString As String
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlFieldCustGroup.SelectedValue, ddlField2CustGroup.SelectedValue, tbFilterCustGroup.Text, tbfilter2CustGroup.Text, ddlNotasiCustGroup.SelectedValue)
            'SqlString = "SELECT * FROM MsCustGroup  " + StrFilter + " ORDER BY CustGroupName ASC "
            SQLString = "SELECT A.*, B.CustGroupName, C.AccountName AS AccARName, E.AccountName AS AccCreditARName, " + _
            "F.AccountName AS AccDPName, G.AccountName AS AccDepositName, M.AccountName AS AccOtherName, " + _
            "I.AccountName AS AccPPNName, M.AccountName AS AccOtherName,P.AccountName AS AccSJUninvoiceName,K.AccountName AS AccPotonganName,  " + _
            "L.AccountName AS accdiscName FROM MsCustGroupAcc A LEFT OUTER JOIN " + _
            "MsCustGroup B ON A.CustGroup = B.CustGroupCode LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccAR = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.AccCreditAR = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.AccDP = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.AccDeposit = G.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.Accppn = I.Account LEFT OUTER JOIN " + _
            "MsAccount M ON A.AccOther = M.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccDisc = L.Account LEFT OUTER JOIN " + _
             "MsAccount K ON A.AccPotongan = K.Account LEFT OUTER JOIN " + _
            "MsAccount P ON A.AccSJUninvoice = P.Account " + StrFilter
            '"WHERE A.CustGroup = " + QuotedStr(ViewState("Nmbr"))

            'DV = tempDS.Tables(0).DefaultView

            Dim dt As New DataTable
            ViewState("DtCustGroup") = Nothing
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtCustGroup") = dt
            DataGridDtCustGroup.DataSource = dt
            DataGridDtCustGroup.DataBind()
            BindGridDt(dt, DataGridDtCustGroup)
        Catch ex As Exception
            lbstatus.Text = "bindDataGridDtCustGroup Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGridDtCustGroup_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDtCustGroup.RowEditing
        Dim GVR As GridViewRow

        Try
            GVR = DataGridDtCustGroup.Rows(e.NewEditIndex)
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            ViewState("NmbrCustGroup") = GVR.Cells(1).Text
            ddlCustGroup.SelectedValue = GVR.Cells(1).Text
            ddlCurrency.SelectedValue = GVR.Cells(3).Text
            tbAccAR.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbAccARName.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbAccSJUninvoice.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbAccSJUninvoiceName.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbAccDiscCustGroup.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbAccDiscNameCustGroup.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbAccOther.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            tbAccOtherName.Text = GVR.Cells(11).Text.Replace("&nbsp;", "")
            tbAccCreditAR.Text = GVR.Cells(12).Text.Replace("&nbsp;", "")
            tbAccCreditARName.Text = GVR.Cells(13).Text.Replace("&nbsp;", "")
            tbAccDP.Text = GVR.Cells(14).Text.Replace("&nbsp;", "")
            tbAccDPName.Text = GVR.Cells(15).Text.Replace("&nbsp;", "")
            tbAccDeposit.Text = GVR.Cells(16).Text.Replace("&nbsp;", "")
            tbAccDepositName.Text = GVR.Cells(17).Text.Replace("&nbsp;", "")
            tbAccPPN.Text = GVR.Cells(18).Text.Replace("&nbsp;", "")
            tbAccPPNName.Text = GVR.Cells(19).Text.Replace("&nbsp;", "")
            tbAccPotongan.Text = GVR.Cells(20).Text.Replace("&nbsp;", "")
            tbAccPotonganName.Text = GVR.Cells(21).Text.Replace("&nbsp;", "")

            PnlEditDetailCustGroup.Visible = True
            pnlDtCustGroup.Visible = False
            'PanelInfoCustGroup.Visible = False
            ViewState("StateDt") = "Edit"
            ddlCustGroup.Enabled = False
            ddlCurrency.Focus()
            pnlCariCustGroup.Visible = False
            'PanelInfoCustGroup.Visible = True
        Catch ex As Exception
            lbstatus.Text = "DataGridDtCustGroup_RowEditing Error : " + ex.ToString
        End Try
    End Sub

    Private Sub DataGridDtCustGroup_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDtCustGroup.RowCancelingEdit
        Try
            DataGridDtCustGroup.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDtCustGroup.EditIndex = -1
            bindDataGridDtCustGroup()
            Dim GVR As GridViewRow
            Dim ddlPClass As DropDownList
            GVR = DataGridDtCustGroup.FooterRow
            ddlPClass = GVR.FindControl("PClassCodeAdd")
            'If lbCustType.Text = "Export" Or lbCustType.Text = "Affiliasi" Then
            '    ddlPClass.Visible = False
            '    DataGridDtCustGroup.Columns(0).Visible = False
            'Else
            '    ddlPClass.Visible = True
            '    DataGridDtCustGroup.Columns(0).Visible = True
            'End If
        Catch ex As Exception
            lbstatus.Text = "DataGridDtCustGroup_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Private Function GetStringDtCustGroup(ByVal Nmbr As String) As String
        Return "SELECT CustGroup, CurrCode, AccAR, AccSJUninvoice, AccDisc, AccOther, AccCreditAR, AccDP, AccDeposit, AccPPN UserID, UserDate FROM mscustgroupacc WHERE CustGroup = '" + Nmbr + "' AND CurrCode = " + QuotedStr(ddlCurrency.SelectedValue)
    End Function

    Private Sub BindDataDtCustGroup(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtCustGroup") = Nothing
            dt = SQLExecuteQuery(GetStringDtCustGroup(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtCustGroup") = dt
            DataGridDtCustGroup.DataSource = dt
            DataGridDtCustGroup.DataBind()
            BindGridDt(dt, DataGridDtCustGroup)
        Catch ex As Exception
            Throw New Exception("BindDataDtCustGroup Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub DataGridDtCustGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDtCustGroup.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbstatus.Text = "DataGridDtCustGroup_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtCustGroup_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDtCustGroup.RowDeleting
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            Dim GVR As GridViewRow = DataGridDtCustGroup.Rows(e.RowIndex)
            'SQLExecuteNonQuery("Delete from MsCustGroupAcc where CustGroup = " + QuotedStr(ViewState("Nmbr")) + " AND CurrCode =" + GVR.Cells(1).Text)
            SQLExecuteNonQuery("Delete from MsCustGroupAcc where CustGroup = " + QuotedStr(GVR.Cells(1).Text) + " AND CurrCode =" + QuotedStr(GVR.Cells(3).Text), ViewState("DBConnection").ToString)
            bindDataGridDtCustGroup()
            lbstatus.Text = MessageDlg("Data Deleted")
        Catch ex As Exception
            lbstatus.Text = "DataGridDtCustGroup_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtCustGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDtCustGroup.PageIndexChanging
        DataGridDtCustGroup.PageIndex = e.NewPageIndex
        If DataGridDtCustGroup.EditIndex <> -1 Then
            DataGridDtCustGroup_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDtCustGroup()
    End Sub

    Protected Sub DataGridDtCustGroup_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDtCustGroup.Sorting
        Try
            ViewState("SortExpressionDt") = e.SortExpression
            bindDataGridDtCustGroup()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtCustGroup_Sorting error =" + vbCrLf + ex.ToString
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
            Throw New Exception("tbAccAR_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccCreditAR_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccDP_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccDeposit_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccppn_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccOther_TextChanged Error : " + ex.ToString)
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
            Throw New Exception("tbAccSJUninvoice_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAccDiscCustGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDiscCustGroup.TextChanged
        Dim DsAccDisc As DataSet
        Dim DrAccDisc As DataRow

        Try
            'DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccDiscCustGroup.Text), ViewState("DBConnection").ToString)
            If DsAccDisc.Tables(0).Rows.Count = 1 Then
                DrAccDisc = DsAccDisc.Tables(0).Rows(0)
                tbAccDiscCustGroup.Text = DrAccDisc("Account")
                tbAccDiscNameCustGroup.Text = DrAccDisc("Description")
            Else
                tbAccDiscCustGroup.Text = ""
                tbAccDiscNameCustGroup.Text = ""
            End If
            tbAccDiscCustGroup.Focus()
        Catch ex As Exception
            Throw New Exception("tbAccDiscCustGroup_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCanceldtCustGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCanceldtCustGroup.Click
        Try
            pnlDtCustGroup.Visible = True
            'PanelInfoCustGroup.Visible = True
            PnlEditDetailCustGroup.Visible = False
            pnlCariCustGroup.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnCanceldtCustGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CurrCodeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccAR, tbAccCreditAR, tbAccDP, tbAccDeposit, tbAccppn, tbAccOther, tbAccDisc As TextBox
        Dim lbAccFAName, lbAccCreditARNAme, lbAccDpName, lbAccDepositName, lbAccppnName, lbAccOtherName, lbAccDiscName As Label

        Try
            Count = DataGridDtCustGroup.Controls(0).Controls.Count
            dgi = DataGridDtCustGroup.FooterRow

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
            lbstatus.Text = "CurrCodeAdd_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAddCustGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAddCustGroup.Click, BtnAdd2CustGroup.Click
        Try
            ViewState("StateDt") = "Insert"
            ClearHdCustGroup()
            pnlDtCustGroup.Visible = False
            PnlEditDetailCustGroup.Visible = True
            ModifyInputCustGroup(True)
            pnlCariCustGroup.Visible = False
            ddlCustGroup.Focus()
        Catch ex As Exception
            lbstatus.Text = "BtnAddCustGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHdCustGroup()
        Try
            ddlCustGroup.SelectedIndex = 0
            ddlCurrency.SelectedValue = ViewState("Currency")

            tbAccAR.Text = ""
            tbAccCreditAR.Text = ""
            tbAccDiscCustGroup.Text = ""
            tbAccOther.Text = ""
            tbAccDeposit.Text = ""
            tbAccDP.Text = ""
            tbAccPPN.Text = ""
            tbAccSJUninvoice.Text = ""

            tbAccARName.Text = ""
            tbAccCreditARName.Text = ""
            tbAccDiscNameCustGroup.Text = ""
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
            Throw New Exception("ClearHdCustGroup Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyInputCustGroup(ByVal State As Boolean)
        Try
            ddlCustGroup.Enabled = State
            ddlCurrency.Enabled = State
            'tbName.Enabled = State
            tbAccAR.Enabled = State
            tbAccPPN.Enabled = State

            tbAccSJUninvoice.Enabled = State
            tbAccCreditAR.Enabled = State
            tbAccDiscCustGroup.Enabled = State
            tbAccOther.Enabled = State
            tbAccDeposit.Enabled = State
            tbAccDP.Enabled = State
            tbAccPPN.Enabled = State
            'btnSaveHd.Enabled = State
            'btnCancelHd.Enabled = State
            'tbCEmail.Enabled = State
        Catch ex As Exception
            Throw New Exception("ModifyInputCustGroup Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAccDiscCustGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDiscCustGroup.Click
        Dim FieldResult As String

        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F')"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccDiscCustGroup"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccDiscCustGroup_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccAR_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccCreditAR_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccDP_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccOther_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccDeposit_Click Error : " + ex.ToString
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
                lbstatus.Text = "btn Search Product Error : " + ex.ToString
            End Try
        Catch ex As Exception
            lbstatus.Text = "btn Acc. Deposit Error : " + ex.ToString
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
            lbstatus.Text = "btnAccPPN_Click Error : " + ex.ToString
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
            lbstatus.Text = "btnAccSJUninvoice_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDtCustGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDtCustGroup.Click
        Dim SQLString As String

        Try
            If tbAccAR.Text.Trim = "" Then
                lbstatus.Text = "<script language='javascript'>alert('Account AR must be filled');</script>"
                tbAccAR.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Insert" Then
                If SQLExecuteScalar("select CustGroup,CurrCode From VMsCustGroupAcc WHERE CustGroup = " + QuotedStr(ddlCustGroup.SelectedValue) + " AND CurrCode = " + QuotedStr(ddlCurrency.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Customer Group Detail " + QuotedStr(ddlCustGroup.SelectedItem.ToString) + " - " + QuotedStr(ddlCurrency.Text) + " has already been exist"
                    Exit Sub
                End If

                'If lbCustType.Text = "Export" Then
                SQLString = "Insert Into MsCustGroupAcc (CustGroup, CurrCode, AccAR, AccCreditAR, AccSJUninvoice, AccDP, AccDeposit, AccPPn, AccOther, AccDisc,AccPotongan, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ddlCustGroup.SelectedValue) + ", " + QuotedStr(ddlCurrency.SelectedValue) + "," + _
                QuotedStr(tbAccAR.Text) + "," + QuotedStr(tbAccCreditAR.Text) + "," + QuotedStr(tbAccSJUninvoice.Text) + "," + _
                QuotedStr(tbAccDP.Text) + "," + QuotedStr(tbAccDeposit.Text) + "," + _
                QuotedStr(tbAccPPN.Text) + "," + QuotedStr(tbAccOther.Text) + "," + _
                QuotedStr(tbAccDiscCustGroup.Text) + "," + QuotedStr(tbAccPotongan.Text) + "," + QuotedStr(ViewState("UserId")) + ", getDate()"
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
                ", AccDisc= " + QuotedStr(tbAccDiscCustGroup.Text) + " WHERE Currcode = " + QuotedStr(ddlCurrency.SelectedValue) + _
                " AND CustGroup =" + QuotedStr(ViewState("NmbrCustGroup"))
            End If

            'lstatus.Text = ViewState("StateDt") + " " + lbCustType.Text
            'Exit Sub
            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridDtCustGroup()
            btnCanceldtCustGroup_Click(btnCanceldtCustGroup, Nothing)
            pnlCariCustGroup.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnSavedDtCustGroup_Click Error" + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        Try
            tbAccAR.Text = ""
            tbAccCreditAR.Text = ""
            tbAccDiscCustGroup.Text = ""
            tbAccOther.Text = ""
            tbAccDeposit.Text = ""
            tbAccDP.Text = ""
            tbAccPPN.Text = ""
            tbAccSJUninvoice.Text = ""

            tbAccARName.Text = ""
            tbAccCreditARName.Text = ""
            tbAccDiscNameCustGroup.Text = ""
            tbAccOtherName.Text = ""
            tbAccDepositName.Text = ""
            tbAccDPName.Text = ""
            tbAccPPNName.Text = ""
            tbAccSJUninvoiceName.Text = ""
        Catch ex As Exception
            lbstatus.Text = "ddlCurrency_SelectedIndexChanged Error" + ex.ToString
        End Try
    End Sub

    Protected Sub btnResetCustGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetCustGroup.Click
        ClearHdCustGroup()
    End Sub
#End Region

#Region "Supplier Type=================================================================================================================="
    Protected Sub btnSearchSuppType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchSuppType.Click
        Try
            DataGridDtSuppType.PageIndex = 0
            DataGridDtSuppType.EditIndex = -1
            bindDataGridDtSuppType()
        Catch ex As Exception
            lbstatus.Text = "btnSearchSuppType_Click Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpandSuppType_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpandSuppType.Click
        Try
            tbfilter2SuppType.Text = ""
            If pnlSearchSuppType.Visible Then
                pnlSearchSuppType.Visible = False
            Else
                pnlSearchSuppType.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btnExpandSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrintST_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintST.Click
        Dim SQLString As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "exec S_MsSuppTypeAccRpt " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsSuppTypeAcc.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "btnPrintST_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDtSuppType()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim StrFilter, SqlString As String

        Try
            StrFilter = GenerateFilterMs(ddlFieldSuppType.SelectedValue, ddlField2SuppType.SelectedValue, tbFilterSuppType.Text, tbfilter2SuppType.Text, ddlNotasiSuppType.SelectedValue)
            'SqlString = "SELECT * FROM MsSuppType " + StrFilter
            SqlString = "SELECT A.*, B.SuppTypeName, C.AccountName AS AccAPName, D.AccountName AS AccAPPendingName, E.AccountName AS AccDebitAPName, " + _
            "F.AccountName AS AccDPName, G.AccountName AS AccDepositName, H.AccountName AS AccVariantPOName, M.AccountName AS AccOtherName, " + _
            "I.AccountName AS AccPPNName, J.AccountName AS AccFreightName, K.AccountName AS AccPPHName, M.AccountName AS AccOtherName, " + _
            "L.AccountName AS accdiscName FROM MsSuppTypeAcc A LEFT OUTER JOIN " + _
            "MsSuppType B ON A.SuppType = B.SuppTypeCode LEFT OUTER JOIN " + _
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
            "MsAccount L ON A.accdisc = L.Account " + StrFilter
            '"WHERE A.SuppType =" + QuotedStr(ViewState("Nmbr"))

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                lbstatus.Text = "No Data"
                DataGridDtSuppType.Visible = False
                btnAdd2SuppType.Visible = False
                'Button2.Visible = False
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDtSuppType)
                DV = DT.DefaultView
            Else
                DataGridDtSuppType.Visible = True
                btnAdd2SuppType.Visible = True
                'Button2.Visible = True

                If ViewState("SortExpressionDtSuppType") = Nothing Then
                    ViewState("SortExpressionDtSuppType") = "CurrCode DESC"
                    ViewState("SortOrderSuppType") = "ASC"
                End If

                DV.Sort = ViewState("SortExpressionDtSuppType")
                DataGridDtSuppType.DataSource = DV
                DataGridDtSuppType.DataBind()
            End If
        Catch ex As Exception
            lbstatus.Text = "bindDataGridDtSuppType Error: " & ex.ToString
        End Try
    End Sub

    Public Sub DataGridDtSuppType_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDtSuppType.RowEditing
        Dim obj As GridViewRow

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGridDtSuppType.Rows(e.NewEditIndex)

            pnlDtSuppType.Visible = False
            'PanelInfoSuppType.Visible = True
            pnlInputDtSuppType.Visible = True
            ddlSuppType.Enabled = False
            ddlCurrCodeDt.Enabled = False

            ViewState("NmbrSuppType") = obj.Cells(1).Text

            ddlSuppType.SelectedValue = obj.Cells(1).Text
            ddlCurrCodeDt.Text = obj.Cells(3).Text
            BindToText(tbAccAP, obj.Cells(4).Text)
            BindToText(tbAccAPName, obj.Cells(5).Text)
            BindToText(tbAccAPPending, obj.Cells(6).Text)
            BindToText(tbAccAPPendingName, obj.Cells(7).Text)
            BindToText(tbAccDebitAP, obj.Cells(8).Text)
            BindToText(tbAccDebitAPName, obj.Cells(9).Text)
            BindToText(tbAccDPSuppType, obj.Cells(10).Text)
            BindToText(tbAccDPNameSuppType, obj.Cells(11).Text)
            BindToText(tbAccDepositSuppType, obj.Cells(12).Text)
            BindToText(tbAccDepositNameSuppType, obj.Cells(13).Text)
            BindToText(tbAccVariantPO, obj.Cells(14).Text)
            BindToText(tbAccVariantPOName, obj.Cells(15).Text)
            BindToText(tbAccPPNSuppType, obj.Cells(16).Text)
            BindToText(tbAccPPNNameSuppType, obj.Cells(17).Text)
            BindToText(tbAccFreight, obj.Cells(18).Text)
            BindToText(tbAccFreightName, obj.Cells(19).Text)
            BindToText(tbAccOtherSuppType, obj.Cells(20).Text)
            BindToText(tbAccOtherNameSuppType, obj.Cells(21).Text)
            BindToText(tbAccPPH, obj.Cells(22).Text)
            BindToText(tbAccPPHName, obj.Cells(23).Text)
            BindToText(tbAccDiscSuppType, obj.Cells(24).Text)
            BindToText(tbAccDiscNameSuppType, obj.Cells(25).Text)
            ViewState("StateSuppType") = "Edit"

            pnlCariSuppType.Visible = False
        Catch ex As Exception
            lbstatus.Text = "DataGridDtSuppType_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Private Sub DataGridDtSuppType_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDtSuppType.RowCancelingEdit
        Try
            DataGridDtSuppType.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDtSuppType.EditIndex = -1
            bindDataGridDtSuppType()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtSuppType_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub DataGridDtSuppType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDtSuppType.RowCommand
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
            lbstatus.Text = "DataGridDtSuppType_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtSuppType_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDtSuppType.RowUpdating
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
            lbstatus.Text = "DataGridDtSuppType_RowUpdating Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtSuppType_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDtSuppType.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGridDtSuppType.Rows(e.RowIndex)

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrCode")

            SQLExecuteNonQuery("Delete from MsSuppTypeAcc where SuppType = " + QuotedStr(ViewState("NmbrSuppType")) + " AND CurrCode = '" & GVR.Cells(3).Text & "'", ViewState("DBConnection").ToString)
            bindDataGridDtSuppType()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtSuppType_RowDeleting Error : " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtSuppType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDtSuppType.PageIndexChanging
        DataGridDtSuppType.PageIndex = e.NewPageIndex
        If DataGridDtSuppType.EditIndex <> -1 Then
            DataGridDtSuppType_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDtSuppType()
    End Sub

    Protected Sub DataGridDtSuppType_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDtSuppType.Sorting
        Try
            If ViewState("SortOrderSuppType") = Nothing Or ViewState("SortOrderSuppType") = "DESC" Then
                ViewState("SortOrderSuppType") = "ASC"
            Else
                ViewState("SortOrderSuppType") = "DESC"
            End If
            ViewState("SortExpressionDtSuppType") = e.SortExpression + " " + ViewState("SortOrderSuppType")
            bindDataGridDtSuppType()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtSuppType_Sorting Error =" + vbCrLf + ex.ToString
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
            lbstatus.Text = "btnAccAP_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccAP_TextChanged Error : " + ex.ToString
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
            tbAccDPSuppType.Text = ""
            tbAccDPNameSuppType.Text = ""
            tbAccDepositSuppType.Text = ""
            tbAccDepositNameSuppType.Text = ""
            tbAccVariantPO.Text = ""
            tbAccVariantPOName.Text = ""
            tbAccPPNSuppType.Text = ""
            tbAccPPNNameSuppType.Text = ""
            tbAccFreight.Text = ""
            tbAccFreightName.Text = ""
            tbAccOtherSuppType.Text = ""
            tbAccOtherNameSuppType.Text = ""
            tbAccPPH.Text = ""
            tbAccPPHName.Text = ""
            tbAccDiscSuppType.Text = ""
            tbAccDiscNameSuppType.Text = ""
        Catch ex As Exception
            lbstatus.Text = "ddlCurrCodeDt_SelectedIndexChanged Error : " + ex.ToString
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
            lbstatus.Text = "btnAccAPPending_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccAPPending_TextChanged Error : " + ex.ToString
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
            lbstatus.Text = "btnAccDebitAP_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccDebitAP_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDepositSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDepositSuppType.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccDepositSuppType"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccDepositSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDepositSuppType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDepositSuppType.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDepositSuppType.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDepositSuppType.Text = ""
                tbAccDepositNameSuppType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDepositSuppType.Text = dr("Account").ToString
                tbAccDepositNameSuppType.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccDepositSuppType_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDiscSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDiscSuppType.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'S') 
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccDiscSuppType"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccDiscSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDiscSuppType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDiscSuppType.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDiscSuppType.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDiscSuppType.Text = ""
                tbAccDiscNameSuppType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDiscSuppType.Text = dr("Account").ToString
                tbAccDiscNameSuppType.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccDiscSuppType_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDPSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDPSuppType.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgSubLed in ('N','S') AND FgType = 'BS' And FgNormal = 'D'
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccDPSuppType"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccDPSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDPSuppType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDPSuppType.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * From VMsAccount Where (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccDPSuppType.Text), ViewState("DBConnection").ToString)
            'Currency in (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgSubLed in ('N','S') AND FgType = 'BS' And FgNormal = 'D'
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDPSuppType.Text = ""
                tbAccDPNameSuppType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDPSuppType.Text = dr("Account").ToString
                tbAccDPNameSuppType.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccDPSuppType_TextChanged Error : " + ex.ToString
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
            lbstatus.Text = "btnAccFreight_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccFreight_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccOtherSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccOtherSuppType.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAccOtherSuppType"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccOtherSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccOtherSuppType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccOtherSuppType.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccOtherSuppType.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'PL' and FgNormal = 'D' AND FgSubled IN ('N', 'S') 
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccOtherSuppType.Text = ""
                tbAccOtherNameSuppType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccOtherSuppType.Text = dr("Account").ToString
                tbAccOtherNameSuppType.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccOtherSuppType_TextChanged Error : " + ex.ToString
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
            lbstatus.Text = "btnAccPPH_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccPPH_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccPPNSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPPNSuppType.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y'"
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccPPNSuppType"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccPPNSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccPPNSuppType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccPPNSuppType.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount WHERE (( Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " AND FgType = 'BS') OR (FgType = 'PL')) AND FgActive='Y' AND Account = " + QuotedStr(tbAccPPNSuppType.Text), ViewState("DBConnection").ToString)
            'Currency IN (" + QuotedStr(ViewState("Currency")) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + ") AND FgType = 'BS' and FgNormal = 'D' AND FgSubled IN ('N', 'S')
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccPPNSuppType.Text = ""
                tbAccPPNNameSuppType.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccPPNSuppType.Text = dr("Account").ToString
                tbAccPPNNameSuppType.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccPPNSuppType_TextChanged Error : " + ex.ToString
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
            lbstatus.Text = "btnAccVariantPO_Click Error : " + ex.ToString
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
            lbstatus.Text = "tbAccVariantPO_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSuppType.Click, btnAdd2SuppType.Click
        Try
            pnlDtSuppType.Visible = False
            'PanelInfoSuppType.Visible = True
            pnlInputDtSuppType.Visible = True
            ViewState("StateSuppType") = "Insert"
            ddlSuppType.Enabled = True
            ddlCurrCodeDt.Enabled = True
            ClearInputSuppType()
            pnlCariSuppType.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btnAddSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearInputSuppType()
        Try
            ddlSuppType.SelectedIndex = 0
            ddlCurrCodeDt.SelectedValue = ViewState("Currency")

            tbAccAP.Text = ""
            tbAccAPName.Text = ""
            tbAccAPPending.Text = ""
            tbAccAPPendingName.Text = ""
            tbAccDebitAP.Text = ""
            tbAccDebitAPName.Text = ""
            tbAccDPSuppType.Text = ""
            tbAccDPNameSuppType.Text = ""
            tbAccDepositSuppType.Text = ""
            tbAccDepositNameSuppType.Text = ""
            tbAccVariantPO.Text = ""
            tbAccVariantPOName.Text = ""
            tbAccPPNSuppType.Text = ""
            tbAccPPNNameSuppType.Text = ""
            tbAccFreight.Text = ""
            tbAccFreightName.Text = ""
            tbAccOtherSuppType.Text = ""
            tbAccOtherNameSuppType.Text = ""
            tbAccPPH.Text = ""
            tbAccPPHName.Text = ""
            tbAccDiscSuppType.Text = ""
            tbAccDiscNameSuppType.Text = ""
        Catch ex As Exception
            Throw New Exception("ClearInputSuppType Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSuppType.Click
        Try
            pnlInputDtSuppType.Visible = False
            pnlDtSuppType.Visible = True
            pnlCariSuppType.Visible = True
            'PanelInfoSuppType.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnCancelSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnResetSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSuppType.Click
        Try
            ClearInputSuppType()
            tbAccAP.Focus()
        Catch ex As Exception
            lbstatus.Text = "btnResetSuppType_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSaveSuppType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveSuppType.Click
        Dim SqlString As String

        Try
            If tbAccAP.Text.Trim = "" Then
                lbstatus.Text = "<script language='javascript'>alert('Account AP must be filled');</script>"
                tbAccAP.Focus()
                Exit Sub
            End If

            If ViewState("StateSuppType") = "Insert" Then
                If SQLExecuteScalar("SELECT SuppType, Currency From VMsSuppTypeAcc WHERE SuppType = " + QuotedStr(ddlSuppType.SelectedValue) + _
                                    " AND Currency = " + QuotedStr(ddlCurrCodeDt.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Supplier Type " + QuotedStr(ddlSuppType.SelectedItem.ToString) + " And Currency " + QuotedStr(ddlCurrCodeDt.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert Into MsSuppTypeAcc (SuppType, CurrCode, AccAP, AccAPPending, AccDebitAP, AccDP, AccDeposit, AccVariantPO, AccPPN, AccFreight, AccOther, AccPPh, AccDisc, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ddlSuppType.SelectedValue) + "," + QuotedStr(ddlCurrCodeDt.SelectedValue) + "," + _
                QuotedStr(tbAccAP.Text) + "," + QuotedStr(tbAccAPPending.Text) + "," + QuotedStr(tbAccDebitAP.Text) + "," + _
                QuotedStr(tbAccDPSuppType.Text) + "," + QuotedStr(tbAccDepositSuppType.Text) + "," + QuotedStr(tbAccVariantPO.Text) + "," + _
                QuotedStr(tbAccOtherSuppType.Text) + "," + QuotedStr(tbAccOtherSuppType.Text) + "," + QuotedStr(tbAccFreight.Text) + "," + _
                QuotedStr(tbAccDiscSuppType.Text) + "," + QuotedStr(tbAccPPH.Text) + "," + QuotedStr(ViewState("UserId")) + ", GETDATE()"
            Else
                SqlString = "UPDATE MsSuppTypeAcc SET ACCAP = " + QuotedStr(tbAccAP.Text) + _
                ", AccAPPending= " + QuotedStr(tbAccAPPending.Text) + ", AccDebitAP= " + QuotedStr(tbAccDebitAP.Text) + _
                ", AccDP= " + QuotedStr(tbAccDPSuppType.Text) + ", AccDeposit= " + QuotedStr(tbAccDepositSuppType.Text) + _
                ", AccVariantPO= " + QuotedStr(tbAccVariantPO.Text) + ", AccPPN= " + QuotedStr(tbAccPPNSuppType.Text) + _
                ", AccFreight= " + QuotedStr(tbAccFreight.Text) + ", AccOther= " + QuotedStr(tbAccOtherSuppType.Text) + _
                ", Accpph= " + QuotedStr(tbAccPPH.Text) + _
                ", AccDisc= " + QuotedStr(tbAccDiscSuppType.Text) + " WHERE Currcode = " + QuotedStr(ddlCurrCodeDt.SelectedValue) + _
                " AND SuppType =" + QuotedStr(ViewState("NmbrSuppType"))
            End If

            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGridDtSuppType()
            pnlInputDtSuppType.Visible = False
            pnlDtSuppType.Visible = True
            pnlCariSuppType.Visible = True
            'PanelInfoSuppType.Visible = True
        Catch ex As Exception
            lbstatus.Text = "BtnSaveSuppType_Click Error : " + ex.ToString
        End Try
    End Sub
#End Region

#Region "Fixed Asset===================================================================================================================="
    Protected Sub btnSearchFASubGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchFASubGroup.Click
        Try
            DataGridDtFASubGroup.PageIndex = 0
            DataGridDtFASubGroup.EditIndex = -1
            bindDataGridDtFASubGroup()
        Catch ex As Exception
            lbstatus.Text = "btnSearchFASubGroup_Click Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpandFASubGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpandFASubGroup.Click
        Try
            tbfilter2FASubGroup.Text = ""
            If pnlSearchFASubGroup.Visible Then
                pnlSearchFASubGroup.Visible = False
            Else
                pnlSearchFASubGroup.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btnExpandFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrintFSG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintFSG.Click
        Dim SQLString As String

        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "exec S_MsFAGroupSubAccRpt " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsFAGroupSubAcc.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "btnPrintFSG_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDtFASubGroup()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim StrFilter, SqlString As String

        Try
            StrFilter = GenerateFilterMs(ddlFieldFASubGroup.SelectedValue, ddlField2FASubGroup.SelectedValue, tbFilterFASubGroup.Text, tbfilter2FASubGroup.Text, ddlNotasiFASubGroup.SelectedValue)
            SqlString = "Select A.*, B.FASubGrpName, FA.AccountName AS AccFAName, " + _
            "De.AccountName AS AccDeprName, Ak.AccountName AS AccAkumDeprName, " + _
            "SA.AccountName AS AccSalesName FROM MsFAGroupSubAcc A LEFT OUTER JOIN " + _
            "MsFAGroupSub B on A.FASubGroup = B.FASubGrpCode LEFT OUTER JOIN " + _
            "MsAccount FA on A.AccFA = FA.Account LEFT OUTER JOIN " + _
            "MsAccount De on A.AccDepr = De.Account LEFT OUTER JOIN " + _
            "MsAccount Ak on A.AccAkumDepr = Ak.Account LEFT OUTER JOIN " + _
            "MsAccount SA on A.AccSales = SA.Account " + StrFilter
            '"WHERE A.FASubGroup = " + QuotedStr(ViewState("Nmbr"))

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDtFASubGroup)
                DV = DT.DefaultView
            Else
                DV.Sort = Session("SortExpressionDtFASubGroup")
                DataGridDtFASubGroup.DataSource = DV
                DataGridDtFASubGroup.DataBind()
            End If

            'Dim ddlCurrency As DropDownList
            'ddlCurrency = DataGridDtFASubGroup.FooterRow.FindControl("CurrCodeAdd")
            'ddlCurrency.SelectedValue = ViewState("Currency")
        Catch ex As Exception
            lbstatus.Text = "bindDataGridDtFASubGroup Error : " & ex.ToString
        End Try
    End Sub

    Public Sub DataGridDtFASubGroup_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDtFASubGroup.RowEditing
        Dim obj As GridViewRow

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGridDtFASubGroup.Rows(e.NewEditIndex)

            pnlDtFASubGroup.Visible = False
            'PanelInfoSuppType.Visible = True
            pnlInputDtFASubGroup.Visible = True
            ddlFASubGroup.Enabled = False
            ddlCurrDtFASubGroup.Enabled = False

            ViewState("NmbrFASubGroup") = obj.Cells(1).Text
            ddlFASubGroup.SelectedValue = obj.Cells(1).Text
            ddlCurrDtFASubGroup.Text = obj.Cells(3).Text
            BindToText(tbAccFASubGroup, obj.Cells(4).Text)
            BindToText(tbAccFANameFASubGroup, obj.Cells(5).Text)
            BindToText(tbAccDeprFASubGroup, obj.Cells(6).Text)
            BindToText(tbAccDeprNameFASubGroup, obj.Cells(7).Text)
            BindToText(tbAccAkumDeprFASubGroup, obj.Cells(8).Text)
            BindToText(tbAccAkumDeprNameFASubGroup, obj.Cells(9).Text)
            BindToText(tbAccSalesFASubGroup, obj.Cells(10).Text)
            BindToText(tbAccSalesNameFASubGroup, obj.Cells(11).Text)

            ViewState("StateFASubGroup") = "Edit"
            pnlCariFASubGroup.Visible = False
        Catch ex As Exception
            lbstatus.Text = "DataGridDtFASubGroup_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Private Sub DataGridDtFASubGroup_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDtFASubGroup.RowCancelingEdit
        Try
            DataGridDtFASubGroup.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDtFASubGroup.EditIndex = -1
            bindDataGridDtFASubGroup()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtFASubGroup_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtFASubGroup_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDtFASubGroup.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGridDtFASubGroup.Rows(e.RowIndex)

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrCode")

            SQLExecuteNonQuery("Delete from MsFAGroupSubAcc where FASubGroup = " + QuotedStr(ViewState("NmbrFASubGroup")) + " AND CurrCode = '" & GVR.Cells(3).Text & "'", ViewState("DBConnection").ToString)
            bindDataGridDtFASubGroup()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtFASubGroup_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDtFASubGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDtFASubGroup.PageIndexChanging
        DataGridDtFASubGroup.PageIndex = e.NewPageIndex
        If DataGridDtFASubGroup.EditIndex <> -1 Then
            DataGridDtFASubGroup_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDtFASubGroup()
    End Sub

    Protected Sub DataGridDtFASubGroup_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDtFASubGroup.Sorting
        Try
            If ViewState("SortOrderFASubGroup") = Nothing Or ViewState("SortOrderFASubGroup") = "DESC" Then
                ViewState("SortOrderFASubGroup") = "ASC"
            Else
                ViewState("SortOrderFASubGroup") = "DESC"
            End If
            ViewState("SortExpressionDtFASubGroup") = e.SortExpression + " " + ViewState("SortOrderFASubGroup")
            bindDataGridDtFASubGroup()
        Catch ex As Exception
            lbstatus.Text = "DataGridDtFASubGroup_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrDtFASubGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDtFASubGroup.SelectedIndexChanged
        Try
            tbAccFASubGroup.Text = ""
            tbAccFANameFASubGroup.Text = ""
            tbAccDeprFASubGroup.Text = ""
            tbAccDeprNameFASubGroup.Text = ""
            tbAccAkumDeprFASubGroup.Text = ""
            tbAccAkumDeprNameFASubGroup.Text = ""
            tbAccSalesFASubGroup.Text = ""
            tbAccSalesNameFASubGroup.Text = ""
        Catch ex As Exception
            lbstatus.Text = "ddlCurrDtFASubGroup_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccFASubGroup.Click
        Dim ResultField As String

        Try
            Session("filter") = "Select Account, Description FROM VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','F')"
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccFASubGroup"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccFASubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccFASubGroup.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("Select Account, Description FROM VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgActive = 'Y' AND FgType = 'BS' and FgNormal = 'D' and FgSubled IN ('N','F') AND Account = " + QuotedStr(tbAccFASubGroup.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccFASubGroup.Text = ""
                tbAccFANameFASubGroup.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccFASubGroup.Text = dr("Account").ToString
                tbAccFANameFASubGroup.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccFASubGroup_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDeprFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDeprFASubGroup.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgActive ='Y' And FgType = 'PL' AND FgSubled IN ('N', 'F') AND FgNormal = 'D'"
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccDeprFASubGroup"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccDeprFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccDeprFASubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDeprFASubGroup.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgActive ='Y' And FgType = 'PL' AND FgSubled IN ('N', 'F') AND FgNormal = 'D' AND Account = " + QuotedStr(tbAccDeprFASubGroup.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccDeprFASubGroup.Text = ""
                tbAccDeprNameFASubGroup.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccDeprFASubGroup.Text = dr("Account").ToString
                tbAccDeprNameFASubGroup.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccDeprFASubGroup_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccAkumDeprFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAkumDeprFASubGroup.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) and FgType = 'BS' And FgNormal = 'C' AND FgSubled IN ('N', 'F') "
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccAkumDeprFASubGroup"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccAkumDeprFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccAkumDeprFASubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccAkumDeprFASubGroup.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgType = 'BS' AND FgSubled IN ('N', 'F') And FgNormal = 'C' AND Account = " + QuotedStr(tbAccAkumDeprFASubGroup.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccAkumDeprFASubGroup.Text = ""
                tbAccAkumDeprNameFASubGroup.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccAkumDeprFASubGroup.Text = dr("Account").ToString
                tbAccAkumDeprNameFASubGroup.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccAkumDeprFASubGroup_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSalesFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSalesFASubGroup.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F')"
            ResultField = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnAccSalesFASubGroup"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnAccSalesFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSalesFASubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSalesFASubGroup.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet

        Try
            ds = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where ((Fixed_Currency = 'Y' AND Currency =" + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + ") OR (Fixed_Currency = 'N')) AND FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'F') AND Account = " + QuotedStr(tbAccSalesFASubGroup.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSalesFASubGroup.Text = ""
                tbAccSalesNameFASubGroup.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccSalesFASubGroup.Text = dr("Account").ToString
                tbAccSalesNameFASubGroup.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbAccSalesFASubGroup_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelFASubGroup.Click
        Try
            pnlInputDtFASubGroup.Visible = False
            pnlDtFASubGroup.Visible = True
            pnlCariFASubGroup.Visible = True
            'PanelInfoSuppType.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnCancelFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnResetFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetFASubGroup.Click
        Try
            ClearInputFASubGroup()
            tbAccFASubGroup.Focus()
        Catch ex As Exception
            lbstatus.Text = "btnResetFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddFASubGroup.Click, btnAdd2FASubGroup.Click
        Try
            pnlDtFASubGroup.Visible = False
            'PanelInfoSuppType.Visible = True
            pnlInputDtFASubGroup.Visible = True
            ViewState("StateFASubGroup") = "Insert"
            ddlFASubGroup.Enabled = True
            ddlCurrDtFASubGroup.Enabled = True
            ClearInputFASubGroup()
            pnlCariFASubGroup.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btnAddFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearInputFASubGroup()
        Try
            ddlFASubGroup.SelectedIndex = 0
            ddlCurrDtFASubGroup.SelectedValue = ViewState("Currency")
            tbAccFASubGroup.Text = ""
            tbAccFANameFASubGroup.Text = ""
            tbAccDeprFASubGroup.Text = ""
            tbAccDeprNameFASubGroup.Text = ""
            tbAccAkumDeprFASubGroup.Text = ""
            tbAccAkumDeprNameFASubGroup.Text = ""
            tbAccSalesFASubGroup.Text = ""
            tbAccSalesNameFASubGroup.Text = ""
        Catch ex As Exception
            Throw New Exception("ClearInputFASubGroup Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnSaveFASubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveFASubGroup.Click
        Dim SqlString As String

        Try
            If tbAccFASubGroup.Text.Trim = "" Then
                lbstatus.Text = "<script language='javascript'>alert('Account FA must be filled');</script>"
                tbAccFASubGroup.Focus()
                Exit Sub
            End If

            If ViewState("StateFASubGroup") = "Insert" Then
                If SQLExecuteScalar("SELECT FASubGroup, CurrCode From MsFAGroupSubAcc WHERE FASubGroup = " + QuotedStr(ddlFASubGroup.SelectedValue) + _
                                    " AND CurrCode = " + QuotedStr(ddlCurrDtFASubGroup.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "FA Sub Group " + QuotedStr(ddlFASubGroup.SelectedItem.ToString) + " Currency " + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert Into MsFAGroupSubAcc (FASubGroup, CurrCode, AccFA, AccDepr, AccAkumDepr, AccSales, UserId, UserDate) " + _
                            "SELECT " + QuotedStr(ddlFASubGroup.SelectedValue) + "," + _
                            QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + "," + _
                            QuotedStr(tbAccFASubGroup.Text) + "," + _
                            QuotedStr(tbAccDeprFASubGroup.Text) + "," + _
                            QuotedStr(tbAccAkumDeprFASubGroup.Text) + "," + _
                            QuotedStr(tbAccSalesFASubGroup.Text) + "," + _
                            QuotedStr(ViewState("UserId")) + ", getDate()"

                SqlString = Replace(SqlString, "''", "NULL")
            Else
                SqlString = "UPDATE MsFAGroupSubAcc SET ACCFA = " + QuotedStr(tbAccFASubGroup.Text) + _
                            ", AccDepr= " + QuotedStr(tbAccDeprFASubGroup.Text) + _
                            ", AccAkumDepr= " + QuotedStr(tbAccAkumDeprFASubGroup.Text) + _
                            ", AccSales= " + QuotedStr(tbAccSalesFASubGroup.Text) + _
                            " WHERE Currcode = " + QuotedStr(ddlCurrDtFASubGroup.SelectedValue) + _
                            " AND FASubGroup =" + QuotedStr(ViewState("NmbrFASubGroup"))
            End If

            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGridDtFASubGroup()
            pnlInputDtFASubGroup.Visible = False
            pnlDtFASubGroup.Visible = True
            pnlCariFASubGroup.Visible = True
            'PanelInfoSuppType.Visible = True
        Catch ex As Exception
            lbstatus.Text = "BtnSaveFASubGroup_Click Error : " + ex.ToString
        End Try
    End Sub
#End Region

#Region "Setup Account=================================================================================================================="
    Private Sub bindDataGridSetupAcc()
        Dim tempDS As New DataSet()
        Dim DV As DataView

        Try
            tempDS = SQLExecuteQuery("SELECT A.SetCode, A.SetValue, A.SetDescription, COALESCE(A.SetRemark, '') AS SetRemark, B.AccountName FROM MsSetup A LEFT OUTER JOIN MsAccount B ON A.SetValue = B.Account WHERE SetGroup = 'Account' ORDER BY A.SetCode", ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            DV.Sort = ViewState("SortExpression")
            DGSetupAcc.DataSource = DV
            DGSetupAcc.DataBind()
        Catch ex As Exception
            lbstatus.Text = "bindDataGridSetupAcc Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            If e.CommandName = "Edit" Then
                PnlMainSetupAcc.Visible = False
                pnlInputSetupAcc.Visible = True
                Dim lbDesc, lbCode, lbValue, lbRemark, lbName As Label
                lbDesc = e.Item.FindControl("SetDescription")
                lbCode = e.Item.FindControl("SetCode")
                lbName = e.Item.FindControl("AccountName")
                lbValue = e.Item.FindControl("SetValue")
                lbRemark = e.Item.FindControl("SetRemark")

                lbSetDescription.Text = lbDesc.Text
                tbSetupAcc.Text = lbValue.Text
                tbSetupAccName.Text = lbName.Text
                ViewState("SetRemark") = lbRemark.Text.Replace("***", ViewState("Currency"))
                ViewState("SetCode") = lbCode.Text
            ElseIf e.CommandName = "Delete" Then
                Dim lbCode As Label
                lbCode = e.Item.FindControl("SetCode")
                SQLExecuteNonQuery("UPDATE MsSetup SET SetValue=NULL WHERE SetGroup = 'Account' AND SetCode =" + QuotedStr(lbCode.Text), ViewState("DBConnection").ToString)
                bindDataGridSetupAcc()
            End If
        Catch ex As Exception
            lbstatus.Text = "ItemCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelSetupAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSetupAcc.Click
        Try
            pnlInputSetupAcc.Visible = False
            PnlMainSetupAcc.Visible = True
            bindDataGridSetupAcc()
        Catch ex As Exception
            lbstatus.Text = "btnCancelSetupAcc_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnResetSetupAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSetupAcc.Click
        Try
            tbSetupAcc.Text = ""
            tbSetupAccName.Text = ""
        Catch ex As Exception
            lbstatus.Text = "btnResetSetupAcc_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveSetupAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSetupAcc.Click
        Dim SQLString As String = ""

        Try
            If tbSetupAccName.Text.Trim = "" Then
                lbstatus.Text = "Account Must Have Value."
                tbSetupAcc.Focus()
                Exit Sub
            End If

            SQLString = "Update MsSetup set SetValue= " + QuotedStr(tbSetupAcc.Text) + " WHERE SetGroup = 'Account' AND SetCode =" + QuotedStr(ViewState("SetCode"))

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridSetupAcc()
            pnlInputSetupAcc.Visible = False
            PnlMainSetupAcc.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btnSaveSetupAcc_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearchSetupAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchSetupAcc.Click
        Dim Filter(10) As String
        Dim Remark, ResultField As String

        Try
            If ViewState("SetRemark").length > 0 Then
                Remark = "Where " + ViewState("SetRemark")
            Else
                Remark = ""
            End If
            'lbstatus.Text = Remark
            'Exit Sub
            Session("filter") = "select Account, Description From V_MsAccount " '+ Remark
            ResultField = "Account, Description"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearchAcc"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btnSearchSetupAcc_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSetupAcc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSetupAcc.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Remark As String

        Try
            If ViewState("SetRemark").length > 0 Then
                Remark = "WHERE " + ViewState("SetRemark") + " AND Account = " + QuotedStr(tbSetupAcc.Text)
            Else
                Remark = "WHERE Account = " + QuotedStr(tbSetupAcc.Text)
            End If

            ds = SQLExecuteQuery("select Account, Description From V_MsAccount " + Remark, ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbSetupAcc.Text = ""
                tbSetupAccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbSetupAcc.Text = dr("Account").ToString
                tbSetupAccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbSetupAcc_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DGSetupAcc_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles DGSetupAcc.BeforeColumnSortingGrouping
        bindDataGridSetupAcc()
    End Sub

    Protected Sub DGSetupAcc_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGSetupAcc.PageIndexChanged
        bindDataGridSetupAcc()
    End Sub

    Protected Sub DGSetupAcc_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles DGSetupAcc.ProcessColumnAutoFilter
        bindDataGridSetupAcc()
    End Sub

    Protected Sub DGSetupAcc_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs) Handles DGSetupAcc.CustomCallback
        Dim SelectValue As Object
        Dim val(4) As String

        Try
            val(0) = "SetDescription"
            val(1) = "SetValue"
            val(2) = "AccountName"
            val(3) = "SetRemark"
            val(4) = "SetCode"
            SelectValue = DGSetupAcc.GetRowValues(Convert.ToInt32(e.Parameters), val)

            PnlMainSetupAcc.Visible = False
            pnlInputSetupAcc.Visible = True

            lbSetDescription.Text = SelectValue(0).ToString
            tbSetupAcc.Text = SelectValue(1).ToString
            tbSetupAccName.Text = SelectValue(2).ToString
            ViewState("SetRemark") = SelectValue(3).ToString.Replace("***", ViewState("Currency"))
            ViewState("SetCode") = SelectValue(4).ToString
        Catch ex As Exception
            lbstatus.Text = "DGSetupAcc_CustomCallback Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DGSetupAcc_CustomButtonCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs) Handles DGSetupAcc.CustomButtonCallback
        Dim SelectValue As Object
        Dim i As Integer
        Dim kod As String
        Try
            SelectValue = DGSetupAcc.GetRowValues(DGSetupAcc.FocusedRowIndex, "SetCode")
            kod = ""
            For i = 0 To SelectValue.length - 1
                kod = kod + SelectValue(i).ToString
            Next
            SQLExecuteNonQuery("UPDATE MsSetup SET SetValue=NULL WHERE SetGroup = 'Account' AND SetCode =" + QuotedStr(kod), ViewState("DBConnection").ToString)
            bindDataGridSetupAcc()
        Catch ex As Exception
            lbstatus.Text = "DGSetupAcc_CustomButtonCallback Error : " + ex.ToString
        End Try
    End Sub
#End Region

    'Protected Sub btnAccAdjustCOGS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccAdjustCOGS.Click
    '    Dim FieldResult As String

    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
    '        FieldResult = "Account, Description"
    '        Session("Column") = FieldResult.Split(",")
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '        ViewState("Sender") = "btnAccAdjustCOGS"
    '    Catch ex As Exception
    '        lbstatus.Text = "btnAccCOGS_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbAccAdjustCOGS_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccAdjustCOGS.TextChanged
    '    Dim dr As DataRow
    '    Dim ds As DataSet

    '    Try
    '        ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccAdjustCOGS.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            tbAccAdjustCOGS.Text = ""
    '            tbAccAdjustCOGSName.Text = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            BindToText(tbAccAdjustCOGS, dr("Account").ToString)
    '            BindToText(tbAccAdjustCOGSName, dr("Description").ToString)
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "tbAccAdjustCOGS_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub
End Class
