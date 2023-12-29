Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class MsFixedAsset
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_MsFixedAssetList "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlFASubGroup, "EXEC S_GetFAGroupSub", False, "FA_SubGrp_Code", "FA_SubGrp_Name", ViewState("DBConnection"))
                FillCombo(ddlFAGroup, "EXEC S_GetFAGroup", False, "FAGroupCode", "FAGroupName", ViewState("DBConnection"))
                FillCombo(ddlFAStatus, "EXEC S_GetFAStatus", False, "FAStatusCode", "FAStatusName", ViewState("DBConnection"))
                FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", False, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
                SetInit()
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            End If
            lbStatus.Text = ""
            If Not Session("AdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("AdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

    Private Sub SetInit()
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        If Request.QueryString("ContainerId").ToString = "MsEAID" Then
            ViewState("FgExpendable") = "Y"
            lbTitle.Text = "Expendable Asset File"
        Else
            ViewState("FgExpendable") = "N"
            lbTitle.Text = "Fixed Asset File"
        End If
    End Sub

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            End If
            DT = BindDataTransaction(GetStringHd + " WHERE FgExpendable = " + QuotedStr(ViewState("FgExpendable").ToString), StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FA_Code DESC"
                ViewState("SortOrder") = "ASC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From VMsFixedAssetDt WHERE FACode = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData()
            pnlNav.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Dim DV As DataView
        Try
            Dim dt As DataTable
            ViewState("Dt") = Nothing
            dt = BindDataTransaction(GetStringDt(Referens), "", ViewState("DBConnection").ToString)
            'ViewState("Dt") = dt
            'BindGridDt(dt, GridDt)

            If dt.Rows.Count = 0 Then
                'lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If

            DV = dt.DefaultView
            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "FALocationCode DESC"
                ViewState("SortOrder") = "ASC"
            End If
            DV.Sort = ViewState("SortExpressionDt")
            GridDt.DataSource = DV
            GridDt.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FilterName, FilterValue, FDateName, FDateValue As String
        Try
            FDateName = "BuyingDate"
            FDateValue = "BuyingDate"
            FilterName = "Asset Code, Asset Name, Asset Sub Group, Asset Group, Moving, Active"
            FilterValue = "FA_Code, FA_Name, FA_Sub_Group_Name, FAGroup_Name, Fg_Moving, FgActive"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim paramgo As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(1).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True                    
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                      
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Photo" Then
                    Try
                        paramgo = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|Fixed Asset"
                        'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenTransaction('TrUploadImage', '" + paramgo + "' );", True)
                        'End If
                        AttachScript("OpenTransaction('TrUploadImage', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                    Catch ex As Exception
                        lbStatus.Text = "DDL.SelectedValue = Menu Error : " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindData()
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, " FA_Code = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToText(tbName, Dt.Rows(0)("FA_Name").ToString)
            BindToDropList(ddlFASubGroup, Dt.Rows(0)("FA_Sub_Group").ToString)
            BindToDropList(ddlFAGroup, Dt.Rows(0)("FA_Group").ToString)
            BindToDropList(ddlFAStatus, Dt.Rows(0)("FA_Status").ToString)
            BindToDate(tbBuyingDate, Dt.Rows(0)("BuyingDate").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            BindToText(tbQtyBuying, FormatFloat((Dt.Rows(0)("QtyTotal").ToString), 2))
            BindToText(tbQtyOpname, FormatFloat((Dt.Rows(0)("QtyOpname").ToString), 2))
            BindToText(tbQtySold, FormatFloat((Dt.Rows(0)("QtySold").ToString), 2))
            BindToText(tbQtyBalance, FormatFloat((Dt.Rows(0)("QtyBalance").ToString), 2))

            BindToText(tbLifeBuying, FormatFloat((Dt.Rows(0)("LifeTotal").ToString), 2))
            'BindToText(tbBeginDepr, FormatFloat((Dt.Rows(0)("LifeProcess").ToString), 2))
            BindToText(tbBeginDepr, FormatFloat((Dt.Rows(0)("LifeProcessDepr").ToString), 2)) '--Modify by Chris

            BindToText(tbProcessedDepr, FormatFloat((Dt.Rows(0)("ProcessedDepr").ToString), 2))
            'BindToText(tbTotalLiveDepr, FormatFloat((Dt.Rows(0)("TotalLiveDepr").ToString), 2))
            'BindToText(tbTotalLiveDepr, FormatFloat((Dt.Rows(0)("LifeProcess").ToString) + (Dt.Rows(0)("ProcessedDepr").ToString), 2))
            BindToText(tbTotalLiveDepr, FormatFloat(Val(tbBeginDepr.Text) + Val(tbProcessedDepr.Text), 2))

            'BindToText(tbLifeBalance, FormatFloat((Dt.Rows(0)("LifeCurrent").ToString), 2))
            'BindToText(tbLifeBalance, FormatFloat((Dt.Rows(0)("LifeTotal").ToString) - (Dt.Rows(0)("LifeProcess").ToString + tbProcessedDepr.Text), 2)) 'Dt.Rows(0)("ProcessedDepr").ToString)
            BindToText(tbLifeBalance, FormatFloat(Val(tbLifeBuying.Text) - (Val(tbBeginDepr.Text) + Val(tbProcessedDepr.Text)), 2)) 'Dt.Rows(0)("ProcessedDepr").ToString)
            BindToText(tbTotalBuying, FormatFloat((Dt.Rows(0)("Total_FA").ToString), 2))
            'BindToText(tbBeginTotalDepr, FormatFloat((Dt.Rows(0)("Total_Process").ToString), 2)) '--Modify by Chris 
            BindToText(tbBeginTotalDepr, FormatFloat((Dt.Rows(0)("LifeProcessDepr").ToString) * Dt.Rows(0)("Total_FA").ToString / Dt.Rows(0)("LifeTotal").ToString, 2)) '--Modify by Chris 

            BindToText(tbTotalProcessDepr, FormatFloat(Dt.Rows(0)("ProcessedDepr").ToString * Dt.Rows(0)("Total_FA").ToString / Dt.Rows(0)("LifeTotal").ToString, 2))  '--Modify by Chris 
            'BindToText(tbTotalDepr, FormatFloat((Dt.Rows(0)("TotalDepr").ToString), 2))
            'BindToText(tbTotalDepr, FormatFloat((Dt.Rows(0)("Total_Process").ToString) + (Dt.Rows(0)("TotalProcessDepr").ToString), 2))
            BindToText(tbTotalDepr, FormatFloat(Val(tbTotalLiveDepr.Text) * Dt.Rows(0)("Total_FA").ToString / Dt.Rows(0)("LifeTotal").ToString, 2))
            'BindToText(tbTotalBalance, FormatFloat((Dt.Rows(0)("Total_Current").ToString), 2))
            'BindToText(tbTotalBalance, FormatFloat((Dt.Rows(0)("Total_Current").ToString) - (Dt.Rows(0)("Total_Process").ToString + Dt.Rows(0)("TotalProcessDepr").ToString), 2))
            BindToText(tbTotalBalance, FormatFloat(Val(tbLifeBalance.Text) * Dt.Rows(0)("Total_FA").ToString / Dt.Rows(0)("LifeTotal").ToString, 2))
            'BindToText(tbTotalNDA, FormatFloat((Dt.Rows(0)("Total_NDA").ToString), 2))
            BindToText(tbFgProcess, Dt.Rows(0)("FgProcess").ToString)
            BindToText(tbFgActive, Dt.Rows(0)("FgActive").ToString)
            BindToText(tbFgMoving, Dt.Rows(0)("Fg_Moving").ToString)
            BindToText(tbFgExpendable, Dt.Rows(0)("FgExpendable").ToString)
            BindToText(tbSpecification, Dt.Rows(0)("Specification").ToString)
            BindToDropList(ddlUnit, Dt.Rows(0)("Unit").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 0 Then
                BindDataDt(ViewState("TransNmbr"))
                'ElseIf MultiView1.ActiveViewIndex = 1 Then
                '    bindDataGridWaste()
                'ElseIf MultiView1.ActiveViewIndex = 2 Then
                '    bindDataGridMoveSales()
                'ElseIf MultiView1.ActiveViewIndex = 3 Then
                '    bindDataGridMovingExp()
                'ElseIf MultiView1.ActiveViewIndex = 4 Then
                '    bindDataGridMovingProcess()
                'ElseIf MultiView1.ActiveViewIndex = 5 Then
                '    bindDataGridProduct()
                'ElseIf MultiView1.ActiveViewIndex = 6 Then
                '    bindDataGridOnExp()
                'Else
                '    bindDataGridOnHPP()
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDt.Sorting
        If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
            ViewState("SortOrder") = "ASC"
        Else
            ViewState("SortOrder") = "DESC"
        End If
        ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
        BindDataDt(ViewState("TransNmbr"))
    End Sub
End Class

