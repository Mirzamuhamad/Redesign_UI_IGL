Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsJobMaterial_MsJobMaterial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlJobGroup, "Select JobPlantGrpCode, JobPlantGrpName from MsJobPlantGroup", False, "JobPlantGrpCode", "JobPlantGrpName", ViewState("DBConnection").ToString)
                BindData()
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                DataGridMaterial.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"

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
        Dim StrFilter As String
        ' Dim FactorHK As TextBox
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            'dsGetSection.ConnectionString = ViewState("DBConnection")
            tempDS = SQLExecuteQuery("EXEC S_MsJobPlantMaterialView " + QuotedStr(ddlJobGroup.SelectedValue) + " , " + QuotedStr(StrFilter) + " , " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection"))
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            'bindDataGridDt()
            'For Each GVR In DataGrid.Rows
            'FactorHK = GVR.FindControl("FactorHK")
            'FactorHK.Attributes.Add("OnKeyDown", "return PressNumeric();")
            ' Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim RotasiNo As Label
        Dim RMPercentage As TextBox
        Dim SQLString As String
        Dim Percent As Double
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            Percent = 0
            For i = 0 To DataGridDt.Rows.Count - 1
                GVR = DataGridDt.Rows(i)
                RotasiNo = GVR.FindControl("RotasiNo")
                RMPercentage = GVR.FindControl("RMPercentage")

                RMPercentage.Text = RMPercentage.Text.Replace(",", "")

                If RMPercentage.Text.Trim = "" Then
                    RMPercentage.Text = "0"
                End If

                If Not IsNumeric(RMPercentage.Text) Then
                    lbstatus.Text = "Percentage for must in numeric format"
                    exe = False
                    RMPercentage.Focus()
                    Exit For
                End If
                Percent = Percent + CFloat(RMPercentage.Text)
            Next

            If exe = True Then
                If Percent <> 100 Then
                    lbstatus.Text = "Percentage (%) must equal to 100 "
                    Exit Sub
                End If

                ' simpan ke database
                For i = 0 To DataGridDt.Rows.Count - 1
                    GVR = DataGridDt.Rows(i)
                    RotasiNo = GVR.FindControl("RotasiNo")
                    RMPercentage = GVR.FindControl("RMPercentage")

                    RMPercentage.Text = RMPercentage.Text.Replace(",", "")
                    If RMPercentage.Text.Trim = "" Then
                        RMPercentage.Text = "0"
                    End If

                    SQLString = "EXEC S_MsJobPlantMaterialPercentageUpdate " + _
                    QuotedStr(ViewState("JobCode")) + ", " + QuotedStr(ViewState("StatusTanam")) + ", " + RotasiNo.Text + ", " + RMPercentage.Text + ", " + QuotedStr(ViewState("UserId").ToString)
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
            End If
            
        Catch ex As Exception
            lbstatus.Text = " btn apply error : " + ex.ToString
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
                tempDS = SQLExecuteQuery("EXEC S_MsJobPlantMaterialView " + QuotedStr(ddlJobGroup.SelectedValue) + " , " + QuotedStr(StrFilter) + " , " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection"))
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try
            ExportGridToExcel("Job Material File")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlJobGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJobGroup.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlViewDt.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlViewDt.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt(ByVal JobCode As String, ByVal StatusTanam As String, ByVal XRotasi As Integer)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsJobPlantMaterialViewPercentage  " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam) + "," + CStr(XRotasi) + "," + QuotedStr(ViewState("UserId").ToString)
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DataGridDt.DataSource = tempDS.Tables(0)
            DataGridDt.DataBind()
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridMaterial(ByVal JobCode As String, ByVal StatusTanam As String)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim GVR As GridViewRow
        Dim SQLString As String
        Dim Qty, QtyCap As TextBox
        Try
            SQLString = "EXEC S_MsJobPlantMaterialViewRM  " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam)

            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridMaterial)
                DV = DT.DefaultView
            Else
                'If ViewState("SortExpressionDt") = Nothing Then
                '    ViewState("SortExpressionDt") = "ItemNo ASC "
                '    ViewState("SortOrder") = "ASC"
                'End If
                ''DV.Sort = Session("SortExpressionDt")
                'DV.Sort = (ViewState("SortExpressionDt"))
                DataGridMaterial.DataSource = DV
                DataGridMaterial.DataBind()
            End If
            'footer format
            GVR = DataGridMaterial.FooterRow
            Qty = GVR.FindControl("QtyAdd")
            Qty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            QtyCap = GVR.FindControl("QtyCapAdd")
            QtyCap.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGridViewDt Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim lbCode, lbName, lbStatusTanam, lbStatusTanamName, lbXRotasi As Label
        Dim RMPercentage As TextBox
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Percentage" Then
                lbCode = GVR.FindControl("JobCode")
                lbName = GVR.FindControl("JobName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                lbXRotasi = GVR.FindControl("XRotation")
                RMPercentage = GVR.FindControl("RMPercentage")
                lblJobDt.Text = "Job : " + lbCode.Text + " - " + lbName.Text
                lblStatusTanamDt.Text = "Status : " + lbStatusTanamName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                ViewState("JobCode") = lbCode.Text
                ViewState("StatusTanam") = lbStatusTanam.Text
                bindDataGridDt(lbCode.Text, lbStatusTanam.Text, CInt(lbXRotasi.Text))


            ElseIf e.CommandName = "Detail" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("JobCode")
                lbName = GVR.FindControl("JobName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                ViewState("JobCode") = lbCode.Text
                ViewState("StatusTanam") = lbStatusTanam.Text
                lblJobMaterial.Text = "Job : " + lbCode.Text + " - " + lbName.Text
                lblStatusTanamMaterial.Text = "Status : " + lbStatusTanamName.Text
                pnlHd.Visible = False
                pnlDt.Visible = False
                pnlViewDt.Visible = True
                bindDataGridMaterial(lbCode.Text, lbStatusTanam.Text)
            End If

        Catch ex As Exception
            lbstatus.Text = "Row Command Error : " + ex.ToString
        End Try
       
    End Sub
    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim tb, ProdCode As TextBox
        Dim ProdName, Unit As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "ProductCodeAdd" Then
                Count = DataGridMaterial.Controls(0).Controls.Count
                dgi = DataGridMaterial.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                ProdCode = dgi.FindControl("ProductCodeAdd")
                ProdName = dgi.FindControl("ProductNameAdd")
                Unit = dgi.FindControl("UnitAdd")
                ds = SQLExecuteQuery("SELECT ProductCode, ProductName, Unit From MsProduct Where ProductCode = " + QuotedStr(ProdCode.Text) + " AND FgActive = 'Y'", ViewState("DBConnection").ToString)
            Else
                Count = DataGridMaterial.EditIndex
                dgi = DataGridMaterial.Rows(Count)
                ProdCode = dgi.FindControl("ProductCodeAdd")
                ProdName = dgi.FindControl("ProductNameAdd")
                Unit = dgi.FindControl("UnitAdd")
                ds = SQLExecuteQuery("SELECT ProductCode, ProductName, Unit From MsProduct Where ProductCode = " + QuotedStr(ProdCode.Text), ViewState("DBConnection").ToString)
            End If
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                ProdCode.Text = ""
                ProdName.Text = ""
                Unit.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                ProdCode.Text = dr("ProductCode").ToString
                ProdName.Text = dr("ProductName").ToString
                Unit.Text = dr("Unit").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tb Product Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridMaterial_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridMaterial.RowCancelingEdit
        Try
            DataGridMaterial.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridMaterial.EditIndex = -1
            bindDataGridMaterial(ViewState("JobCode").ToString, ViewState("StatusTanam").ToString)
        Catch ex As Exception
            lbstatus.Text = "DataGridMaterial_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub DataGridMaterial_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridMaterial.RowCommand
        Dim tbProductCode, dbQty, dbQtyCap As TextBox
        Dim SQLString As String
        Dim tbProductName, dbUnit As Label
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridMaterial.FooterRow
                tbProductCode = GVR.FindControl("ProductCodeAdd")
                tbProductName = GVR.FindControl("ProductNameAdd")
                dbQty = GVR.FindControl("QtyAdd")
                dbUnit = GVR.FindControl("UnitAdd")
                dbQtyCap = GVR.FindControl("QtyCapAdd")
                dbQty.Text = dbQty.Text.Replace(",", "")
                dbUnit.Text = dbUnit.Text
                dbQtyCap.Text = dbQtyCap.Text.Replace(",", "")

                If dbQty.Text.Trim = "" Then
                    dbQty.Text = "0"
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbstatus.Text = MessageDlg("Product must be filled")
                    tbProductCode.Focus()
                    Exit Sub
                End If
                If tbProductName.Text.Trim = "" Then
                    If tbProductName.Text.Trim = "" Then
                        lbstatus.Text = MessageDlg("Name must be filled")
                        tbProductName.Focus()
                        Exit Sub
                    End If
                End If
                If CFloat(dbQty.Text) = 0 Then
                    lbstatus.Text = MessageDlg("Qty must have value")
                    dbQty.Focus()
                    Exit Sub
                End If
                If CFloat(dbQtyCap.Text) = 0 Then
                    lbstatus.Text = MessageDlg("Qty Cap must have value")
                    dbQtyCap.Focus()
                    Exit Sub
                End If

                SQLString = "EXEC S_MsJobPlantMaterialRMUpdate " + QuotedStr(ViewState("JobCode")) + "," + QuotedStr(ViewState("StatusTanam")) + ", 'Material'," + _
                QuotedStr(tbProductCode.Text) + "," + dbQty.Text.Replace(",", "") + "," + _
                QuotedStr(dbUnit.Text) + "," + dbQtyCap.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId"))

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridMaterial(ViewState("JobCode"), ViewState("StatusTanam"))
            ElseIf e.CommandName = "SearchProductCodeEdit" Or e.CommandName = "SearchProductCodeAdd" Then
                Dim FieldResult As String
                Session("filter") = "SELECT Product_Code, Product_Name, ProductType_Name, ProductSubGrp_Name, ProductGrp_Name, Specification, Unit From VMsProduct WHERE Fg_Active = 'Y'"
                FieldResult = "Product_Code, Product_Name, Unit "
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchProductCodeAdd" Then
                    ViewState("Sender") = "SearchProductCodeAdd"
                Else
                    ViewState("Sender") = "SearchProductCodeEdit"
                End If
                AttachScript("OpenSearchDlg();", Page, Me.GetType())


                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
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

    Protected Sub DataGridMaterial_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridMaterial.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridMaterial.Rows(e.RowIndex).FindControl("ProductCode")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("Delete from MsJobPlantDtRM where JobCode = " + QuotedStr(ViewState("JobCode")) + " AND StatusTanam = " + QuotedStr(ViewState("StatusTanam")) + " AND ProductCode =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridMaterial(ViewState("JobCode"), ViewState("StatusTanam"))
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub


    Public Sub DataGridMaterial_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles DataGridMaterial.RowEditing
        Dim dbQty, dbQtyCap As TextBox

        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridMaterial.EditIndex = e.NewEditIndex
            DataGridMaterial.ShowFooter = False
            bindDataGridMaterial(ViewState("JobCode"), ViewState("StatusTanam"))

            GVR = DataGridMaterial.Rows(DataGridMaterial.EditIndex)
            dbQty = GVR.FindControl("QtyEdit")
            dbQtyCap = GVR.FindControl("QtyCapEdit")
            dbQty.Text = dbQty.Text.Replace(",", "")
            dbQtyCap.Text = dbQtyCap.Text.Replace(",", "")
            dbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbQtyCap.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbQty.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub

    Protected Sub DataGridMaterial_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridMaterial.RowUpdating
        Dim tbProductCode, dbUnit As Label
        Dim dbQty, dbQtyCap As TextBox
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridMaterial.Rows(e.RowIndex)
            tbProductCode = GVR.FindControl("ProductCodeEdit")
            dbQty = GVR.FindControl("QtyEdit")
            dbUnit = GVR.FindControl("UnitEdit")
            dbQtyCap = GVR.FindControl("QtyCapEdit")
            dbQty.Text = dbQty.Text.Replace(",", "")
            dbQtyCap.Text = dbQtyCap.Text.Replace(",", "")
            
            If dbQty.Text.Trim = "" Then
                dbQty.Text = "0"
            End If
            If dbQtyCap.Text.Trim = "" Then
                dbQtyCap.Text = "0"
            End If
            If tbProductCode.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Product Code must be filled")
                tbProductCode.Focus()
                Exit Sub
            End If
            If CFloat(dbQty.Text) = 0 Then
                lbstatus.Text = MessageDlg("Qty must have value")
                dbQty.Focus()
                Exit Sub
            End If
            SQLString = "UPDATE MsJobPlantDtRM SET Qty= " + dbQty.Text + _
            ", Unit = " + QuotedStr(dbUnit.Text) + _
            ", QtyCap = " + dbQtyCap.Text + _
            ", UserId = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = Getdate() " + _
            " WHERE JobCode = " + QuotedStr(ViewState("JobCode")) + " AND StatusTanam = " + QuotedStr(ViewState("StatusTanam")) + " AND ProductCode = " + QuotedStr(tbProductCode.Text)

            SQLString = Replace(SQLString, "''", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            'lstatus.Text = SQLString

            DataGridMaterial.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridMaterial.EditIndex = -1
            bindDataGridMaterial(ViewState("JobCode"), ViewState("StatusTanam"))
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnBack2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack2.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlViewDt.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
End Class
