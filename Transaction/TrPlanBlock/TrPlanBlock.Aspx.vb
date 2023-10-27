Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Transcation_TrPlanBlock_TrPlanBlock
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlYear, "select year from glyear ", True, "year", "year", ViewState("DBConnection").ToString)
                FillCombo(ddlDivisi, "EXEC S_GetPlantDivision", True, "DivisionCode", "DivisionName", ViewState("DBConnection").ToString)
                FillCombo(ddlmasterplan, " SELECT Master_Plan_No,Effective_Date FROM V_PLPlanBlockGetMasterPlan ", True, "Master_Plan_No", "Master_Plan_No", ViewState("DBConnection").ToString)
                BindData()
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                DataGridMaterial.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                tbDate.SelectedDate = ViewState("ServerDate")
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "SearchProductCodeAdd" Or ViewState("Sender") = "SearchProductCodeEdit" Then
                    Dim Code, Qty, QtyCap As New TextBox
                    Dim ProName, Unit As New Label
                    If ViewState("Sender") = "SearchProductCodeAdd" Then
                        Code = DataGridMaterial.FooterRow.FindControl("ProductCodeAdd")
                        ProName = DataGridMaterial.FooterRow.FindControl("ProductNameAdd")
                        Qty = DataGridMaterial.FooterRow.FindControl("QtyAdd")
                        Unit = DataGridMaterial.FooterRow.FindControl("UnitAdd")
                        QtyCap = DataGridMaterial.FooterRow.FindControl("QtyCapAdd")

                    Else
                        Code = DataGridMaterial.Rows(DataGridMaterial.EditIndex).FindControl("ProductCodeEdit")
                        ProName = DataGridMaterial.Rows(DataGridMaterial.EditIndex).FindControl("ProductNameEdit")
                        Qty = DataGridMaterial.Rows(DataGridMaterial.EditIndex).FindControl("QtyEdit")
                        Unit = DataGridMaterial.Rows(DataGridMaterial.EditIndex).FindControl("UnitEdit")
                        QtyCap = DataGridMaterial.Rows(DataGridMaterial.EditIndex).FindControl("QtyCapEdit")
                    End If
                    Code.Text = Session("Result")(0).ToString
                    ProName.Text = Session("Result")(1).ToString
                    Qty.Text = "1"
                    Unit.Text = Session("Result")(2).ToString
                    QtyCap.Text = "1"
                    Code.Focus()

                End If

                Dim Kode As TextBox
                Dim Name, StatusTanamName As Label
                If ViewState("Sender") = "SearchBlockCodeAdd" Then
                    ViewState("Sender") = "SearchBlockCodeAdd"
                    Kode = DataGrid.FooterRow.FindControl("BlockCodeAdd")
                    Name = DataGrid.FooterRow.FindControl("BlockNameAdd")
                    StatusTanamName = DataGrid.FooterRow.FindControl("StatusTanamNameAdd")

                    Kode.Text = Session("Result")(0).ToString
                    Name.Text = Session("Result")(1).ToString
                    StatusTanamName.Text = Session("Result")(2).ToString
                    Kode.Focus()
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "Page Load Error : " + ex.ToString
        End Try
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


    Sub BindData()
        Dim tempDS As New DataSet()
        Dim LblStatus, LbBlok, LbStatusTanam As Label
        Dim BtnPost, BtnUnPost, BtnGetAppr, btnDeleteHd, BtnGenerate, BtnSchedule As Button
        Dim StrFilter As String
        Dim Sqlstring As String
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            
            
            dsGetStatusTanam.ConnectionString = ViewState("DBConnection")
            dsGetMasterPlan.ConnectionString = ViewState("DBConnection")
            Sqlstring = "EXEC S_PLPlanBlockDtView " + QuotedStr(ddlYear.SelectedValue) + " , " + QuotedStr(ddlDivisi.SelectedValue) + " , " + QuotedStr(StrFilter) + " ," + QuotedStr(tbCode.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            tempDS = SQLExecuteQuery(Sqlstring, ViewState("DBConnection"))
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            For Each GVR In DataGrid.Rows
                LblStatus = GVR.FindControl("Status")
                LbBlok = GVR.FindControl("BlockCode")
                LbStatusTanam = GVR.FindControl("StatusTanam")
                BtnPost = GVR.FindControl("btnPost")
                BtnGetAppr = GVR.FindControl("btnGetAppr")
                BtnUnPost = GVR.FindControl("btnUnpost")
                btnDeleteHd = GVR.FindControl("btnDeleteHd")
                BtnGenerate = GVR.FindControl("btnGenerate")
                BtnSchedule = GVR.FindControl("btnSchedule")
                BtnGetAppr.Enabled = False
                BtnPost.Enabled = False
                BtnUnPost.Enabled = False
                BtnSchedule.Enabled = False
                BtnGenerate.Enabled = False
                btnDeleteHd.Enabled = False
                If LblStatus.Text = "H" Then
                    BtnGetAppr.Enabled = True
                    BtnGenerate.Enabled = True
                    BtnSchedule.Enabled = True
                    btnDeleteHd.Enabled = True
                ElseIf LblStatus.Text = "G" Then
                    BtnPost.Enabled = True
                    BtnGenerate.Enabled = True
                    btnDeleteHd.Enabled = True
                    BtnSchedule.Enabled = True
                ElseIf LblStatus.Text = "P" Then
                    BtnUnPost.Enabled = True
                    BtnSchedule.Enabled = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim tempDS As New DataSet()
            Dim StrFilter As String
            Try
                StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
                If StrFilter.Length > 5 Then
                    StrFilter = StrFilter.Remove(1, 5)
                    StrFilter = " And " + StrFilter
                End If
                tempDS = SQLExecuteQuery("EXEC S_MsBlockPlantMaterialView " + QuotedStr(ddlDivisi.SelectedValue) + " , " + QuotedStr(StrFilter) + " , " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection"))
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try
            ExportGridToExcel("Block Material File")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub ddlBlockGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivisi.SelectedIndexChanged
        Dim Sqlstring As String
        Dim tempDS As String
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1

            Sqlstring = "SELECT TransNmbr FROM PlplanBlokhd Where Year  = " + QuotedStr(ddlYear.SelectedValue) + " And Division = " + QuotedStr(ddlDivisi.SelectedValue)
            tempDS = SQLExecuteScalar(Sqlstring, ViewState("DBConnection").ToString)
            tbCode.Text = tempDS

            BindData()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Dim Sqlstring As String
        Dim tempDS As String
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1

            Sqlstring = "SELECT TransNmbr FROM PlplanBlokhd Where Year  = " + QuotedStr(ddlYear.SelectedValue) + " And Division = " + QuotedStr(ddlDivisi.SelectedValue)
            tempDS = SQLExecuteScalar(Sqlstring, ViewState("DBConnection").ToString)
            tbCode.Text = tempDS
            
            BindData()


        Catch ex As Exception
        End Try
    End Sub

    Private Sub bindDataGridHK(ByVal Year As String, ByVal StatusTanam As String, ByVal Job As String)
        Dim tempDS As New DataSet()
        Dim SQLString As String
        Try
            SQLString = "EXEC S_PLPlanBlockDtSchedule    " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlDivisi.SelectedValue) + "," + QuotedStr(Job) + "," + QuotedStr(ViewState("UserId").ToString)
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            'lbstatus.Text = SQLString
            'Exit Sub
            DataGridHK.DataSource = tempDS.Tables(0)
            DataGridHK.DataBind()
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        End Try
    End Sub


    Private Sub bindDataGridMaterial(ByVal BlockCode As String, ByVal JobCode As String)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SQLString As String
        Try
            'SQLString = "SELECT TransNmbr, Block, JobPlant, MonthNo, Material, Qty, Unit, MaterialName, Dosis FROM V_PLPlanBlokJobDtRM Where TransNmbr =" + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + " AND Block =" + QuotedStr(BlockCode) + " AND JobPlant =" + QuotedStr(JobCode)
            SQLString = "SELECT TransNmbr, Block, JobPlant, MonthNo, Material, Qty, Unit, MaterialName, Dosis FROM V_PLPlanBlokJobDtRM Where TransNmbr =" + QuotedStr(tbCode.Text) + " AND Block =" + QuotedStr(BlockCode) + " AND JobPlant =" + QuotedStr(JobCode) '+ " AND Block =" + QuotedStr(BlockCode) ' + " AND MonthNo ='1'"
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            'lbstatus.Text = SQLString
            'Exit Sub
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                DataGridMaterial.DataBind()
                'ShowGridViewIfEmpty(DT, DataGridMaterial)
                'DV = DT.DefaultView

            Else
                DataGridMaterial.DataSource = DV
                DataGridMaterial.DataBind()
            End If

        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGridViewDt Error: " & ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim lbCode, lbName, lbStatusTanam, lbStatusTanamName, lbXRotasi As Label
        Dim SQLString As String
        Dim sqlstring1, result As String
        Dim GVR As GridViewRow = Nothing
        'Dim index As Integer
        Try

            'If Not (e.CommandName = "SearchBlockCodeAdd") Then
            '    index = Convert.ToInt32(e.CommandArgument)
            '    GVR = DataGrid.Rows(index)
            'End If

            If e.CommandName = "Generate" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                lbName = GVR.FindControl("BlockName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                lbXRotasi = GVR.FindControl("Month")

                'sqlstring1 = "Declare @A VarChar(255) EXEC S_PLPlanBlokGetSchedule " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + "," + QuotedStr(lbCode.Text) + ", @A OUT SELECT @A"
                sqlstring1 = "Declare @A VarChar(255) EXEC S_PLPlanBlokGetSchedule " + QuotedStr(tbCode.Text) + "," + QuotedStr(lbCode.Text) + ", @A OUT SELECT @A"
                result = SQLExecuteScalar(sqlstring1, ViewState("DBConnection"))

                If result.Length > 2 Then
                    lbstatus.Text = MessageDlg(result)
                Else
                    lbstatus.Text = MessageDlg("generate success")
                End If


            ElseIf e.CommandName = "Schedule" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                lbName = GVR.FindControl("BlockName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                lbXRotasi = GVR.FindControl("XRotation")

                lblBlockHK.Text = "Block : " + lbCode.Text + " - " + lbName.Text
                lblStatusTanamHK.Text = "Status : " + lbStatusTanamName.Text
                pnlHd.Visible = False
                pnlHKFactor.Visible = True
                pnlViewDt.Visible = True
                ViewState("BlockCode") = lbCode.Text
                ViewState("StatusTanam") = lbStatusTanam.Text
                bindDataGridHK(ddlYear.SelectedValue, ddlDivisi.SelectedValue, lbCode.Text)

            ElseIf e.CommandName = "DeleteHd" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                'SQLString = "EXEC S_PLPlanBlokDelete  " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + ", " + QuotedStr(lbCode.Text)
                SQLString = "EXEC S_PLPlanBlokDelete  " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                BindData()
            ElseIf e.CommandName = "GetAppr" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                'SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokGetAppr " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokGetAppr " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
                If result.Length > 2 Then
                    lbstatus.Text = MessageDlg(result)
                End If
                BindData()

            ElseIf e.CommandName = "Post" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                'SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokPost " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokPost " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"

                result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
                If result.Length > 2 Then
                    lbstatus.Text = MessageDlg(result)
                End If
                BindData()

            ElseIf e.CommandName = "UnPost" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("BlockCode")
                'SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokUnPost " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"

                SQLString = "Declare @A VarChar(255) EXEC S_PLPlanBlokUnPost " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbCode.Text) + "," + ViewState("GLYear").ToString + ", " + QuotedStr(ViewState("GLPeriod").ToString) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
                If result.Length > 2 Then
                    lbstatus.Text = MessageDlg(result)
                End If
                BindData()

            ElseIf e.CommandName = "InsertHd" Then
                Dim BlockCode, Capacity, Qty, QtyConvert, Volume As TextBox
                Dim UnitAreal, BlockName, Unit, UnitDosis, StatusTanam As Label
                Dim MasterPlan As DropDownList
                BlockCode = DataGrid.FooterRow.FindControl("BlockCodeAdd")
                BlockName = DataGrid.FooterRow.FindControl("BlockNameAdd")
                MasterPlan = DataGrid.FooterRow.FindControl("MasterPlanAdd")
                StatusTanam = DataGrid.FooterRow.FindControl("StatusTanamAdd")

                If BlockCode.Text.Trim.Length = 0 Then
                    lbstatus.Text = "Block Code must be filled."
                    BlockCode.Focus()
                    Exit Sub
                End If
                'If StatusTanam.Text.Trim.Length = 0 Then
                '    lbstatus.Text = "Status Tanam must be filled."
                '    StatusTanam.Focus()
                '    Exit Sub
                'End If


                'insert the new entry
                'SQLString = "EXEC S_PLPlanBlokDtAdd" + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlDivisi.Text) + ", " + QuotedStr(BlockCode.Text) + _
                '   ", " + QuotedStr(MasterPlan.Text) + ", " + QuotedStr(ViewState("UserId").ToString)

                SQLString = "EXEC S_PLPlanBlokDtAdd" + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlDivisi.Text) + ", " + QuotedStr(BlockCode.Text) + _
                   ", " + QuotedStr(MasterPlan.Text) + "," + QuotedStr(tbCode.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                'lbstatus.Text = SQLString
                'Exit Sub
                BindData()

            ElseIf e.CommandName = "SearchBlockCodeEdit" Or e.CommandName = "SearchBlockCodeAdd" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select Block_Code, Block_Name, StatusTanamCode, StatusTanam  from V_MsBlock WHERE Division = " + QuotedStr(ddlDivisi.SelectedValue)
                FieldResult = "Block_Code, Block_Name, StatusTanamCode, StatusTanam"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchBlockCodeAdd" Then
                    ViewState("Sender") = "SearchBlockCodeAdd"
                Else
                    ViewState("Sender") = "SearchBlockCodeEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If

            End If

        Catch ex As Exception
            lbstatus.Text = "Row Command Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub DataGridHK_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridHK.PageIndexChanging
        DataGridHK.PageIndex = e.NewPageIndex
        'If DataGrid.EditIndex <> -1 Then
        '    DataGrid_RowCancelingEdit(Nothing, Nothing)
        'End If
        bindDataGridHK(ddlYear.SelectedValue, ddlDivisi.SelectedValue, "")
    End Sub

    Protected Sub DataGridMaterial_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridMaterial.Sorting
        'Try
        '    If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
        '        ViewState("SortOrder") = "ASC"
        '    Else
        '        ViewState("SortOrder") = "DESC"
        '    End If
        '    ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
        '    'bindDataGridViewDt()
        'Catch ex As Exception
        '    lbstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        'End Try
    End Sub

    Protected Sub btnBack2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack2.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlViewDt.Visible = False
            pnlHKFactor.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnApply1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply1.Click
        Dim SQLString As String
        Try
            tbCode.Text = GetAutoNmbr("PLT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
            SQLString = "EXEC S_PLPlanBlokViewBlock " + ddlYear.SelectedValue + ", " + QuotedStr(ddlDivisi.SelectedValue) + " , " + QuotedStr(tbCode.Text) + "," + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            BindData()

        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub


    Protected Sub BtnApply2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply2.Click
        Dim GVR As GridViewRow
        Dim CB As CheckBox
        Dim lbCode As Label
        Dim HaveSelect As Boolean
        'Dim ddlMaster As DropDownList
        Dim SQLString, Hasil As String
        Try
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbCode = GVR.FindControl("BlockCode")
                'ddlMaster = GVR.FindControl("MasterPlan")


                If CB.Checked Then
                    HaveSelect = True
                    'SQLString = "EXEC S_PLPlanBlokUpdate  " + QuotedStr(ddlYear.SelectedValue + ddlDivisi.SelectedValue) + "," + QuotedStr(lbCode.Text) + ", " + QuotedStr(ddlmasterplan.SelectedValue)
                    SQLString = "EXEC S_PLPlanBlokUpdate  " + QuotedStr(tbCode.Text) + "," + QuotedStr(lbCode.Text) + ", " + QuotedStr(ddlmasterplan.SelectedValue)
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If

                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If


        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(DataGrid, sender)
    End Sub
    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub btnBackHK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackHK.Click
        Try
            pnlHd.Visible = True
            pnlViewDt.Visible = False
            pnlHKFactor.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbBlockCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim dr As DataRow
        'Dim ds As DataSet
        'Dim Acc As TextBox
        'Dim AccName As Label
        'Dim Count As Integer
        'Dim dgi As GridViewRow
        'Try
        '    Count = DataGrid.EditIndex
        '    dgi = DataGrid.Rows(Count)
        '    Acc = dgi.FindControl("AccountExpEdit")
        '    AccName = dgi.FindControl("AccountExpNameEdit")


        '    ds = SQLExecuteQuery("Select Account, Description From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
        '    If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '        Acc.Text = ""
        '        AccName.Text = ""
        '    Else
        '        dr = ds.Tables(0).Rows(0)
        '        Acc.Text = dr("Account").ToString
        '        AccName.Text = dr("Description").ToString
        '    End If
        'Catch ex As Exception
        '    lbstatus.Text = "tb Acc Changed Error : " + ex.ToString
        'End Try
        'Dim dr As DataRow
        'Dim ds As DataSet
        'Dim Kode, tb As TextBox
        'Dim Name, StatusTanam As Label
        'Dim Count As Integer
        'Dim dgi As GridViewRow
        'Try
        '    tb = sender

        '    Count = DataGrid.EditIndex
        '    dgi = DataGrid.Rows(Count)
        '    Kode = dgi.FindControl("BlockCodeAdd")
        '    Name = dgi.FindControl("BlockNameAdd")
        '    StatusTanam = dgi.FindControl("StatusTanamAdd")

        '    ds = SQLExecuteQuery("Select Block_Code, Block_Name, StatusTanam FROM V_MsBlock WHERE BlockCode = " + QuotedStr(Kode.Text), ViewState("DBConnection").ToString)
        '    If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '        Kode.Text = ""
        '        Name.Text = ""
        '        StatusTanam.Text = ""
        '    Else
        '        dr = ds.Tables(0).Rows(0)
        '        Kode.Text = dr("Block_Code").ToString
        '        Name.Text = dr("Block_Name").ToString
        '        StatusTanam.Text = dr("StatusTanam").ToString
        '    End If
        'Catch ex As Exception
        '    lbstatus.Text = "tb Block Code Changed Error : " + ex.ToString
        'End Try
    End Sub


    Protected Sub DataGridHK_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridHK.RowCommand
        Dim lbJobCode, lbJobName As Label
        Dim GVR As GridViewRow = Nothing
        Try
            GVR = DataGridHK.Rows(CInt(e.CommandArgument))
            lbJobCode = GVR.FindControl("JobPlant")
            lbJobName = GVR.FindControl("JobPlantName")
            lblJob.Text = "Job : " + lbJobCode.Text + " - " + lbJobName.Text
            pnlHd.Visible = False
            pnlViewDt.Visible = True

            bindDataGridMaterial(ViewState("BlockCode"), lbJobCode.Text)
        Catch ex As Exception
            lbstatus.Text = "DataGridHK_RowCommand Changed Error : " + ex.ToString
        End Try
    End Sub


End Class
