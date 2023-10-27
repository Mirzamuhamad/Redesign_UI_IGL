Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsMasterPlan_MsMasterPlan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlCopyFrom, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", True, "TransNmbr", "MasterPlanName", ViewState("DBConnection").ToString)
                FillCombo(ddlVarietas, "select VarietasCode, VarietasName from MsVarietas", True, "VarietasCode", "VarietasName", ViewState("DBConnection"))
                FillCombo(ddlLandScape, "SELECT LandScapeCode, LandScapeName FROM MsLandScape", True, "LandScapeCode", "LandScapeName", ViewState("DBConnection"))
                FillCombo(ddlLandType, "SELECT LandTypeCode, LandTypeName FROM MsLandType", True, "LandTypeCode", "LandTypeName", ViewState("DBConnection"))
                FillCombo(ddlMaster, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", True, "TransNmbr", "MasterPlanName", ViewState("DBConnection").ToString)
                FillCombo(ddlJobGroup, "Select JobPlantGrpCode, JobPlantGrpName from MsJobPlantGroup", False, "JobPlantGrpCode", "JobPlantGrpName", ViewState("DBConnection").ToString)
                BindData()
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                DataGridMaterial.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            End If
            btnGetJob.Visible = False
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

                Dim Acc, AccName As TextBox
                Dim UnitCap, UnitCon, UnitAr As Label
                If ViewState("Sender") = "SearchJobCodeAdd" Then
                    ViewState("Sender") = "SearchJobCodeAdd"
                    Acc = DataGrid.FooterRow.FindControl("JobCodeAdd")
                    AccName = DataGrid.FooterRow.FindControl("JobNameAdd")
                    UnitCap = DataGrid.FooterRow.FindControl("lbUnitCapAdd")
                    UnitCon = DataGrid.FooterRow.FindControl("lbUnitCAdd")
                    UnitAr = DataGrid.FooterRow.FindControl("lbUnitArAdd")

                    Acc.Text = Session("Result")(0).ToString
                    AccName.Text = Session("Result")(1).ToString
                    UnitCap.Text = Session("Result")(3).ToString
                    UnitCon.Text = Session("Result")(4).ToString
                    UnitAr.Text = Session("Result")(2).ToString
                    Acc.Focus()

                End If

                If ViewState("Sender") = "btnGetMaterial" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    Dim SQLUpdate As String
                    For Each drResult In Session("Result").Rows
                        'Row = ViewState("DtMT").Select("ProductCode = " + QuotedStr(drResult("ProductCode")))
                        'If Row.Count = 0 Then
                        'dr = ViewState("DtMT").NewRow
                        'dr("ProductCode") = drResult("ProductCode")
                        'dr("ProductName") = drResult("ProductName")
                        'dr("Qty") = drResult("Qty")
                        'dr("Unit") = drResult("Unit")
                        'dr("QtyCap") = drResult("QtyCap")
                        'ViewState("DtMT").Rows.Add(dr)
                        'End If

                        SQLUpdate = "EXEC S_MsMasterPlanDtViewRMUpdate " + QuotedStr(ddlMaster.SelectedValue) + "," + QuotedStr(ViewState("JobCode")) + "," + QuotedStr(ViewState("StatusTanam")) + _
                        "," + QuotedStr(drResult("ProductCode").ToString) + "," + drResult("Qty").ToString.Replace(",", "") + "," + _
                        QuotedStr(drResult("Unit").ToString) + "," + drResult("QtyCap").ToString.Replace(",", "") + "," + QuotedStr(ViewState("UserId"))

                        SQLExecuteNonQuery(SQLUpdate, ViewState("DBConnection").ToString)
                    Next
                    bindDataGridMaterial(ViewState("JobCode"), ViewState("StatusTanam"))
                    'BindGridDt(ViewState("DtMT"), DataGridMaterial)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If

                If ViewState("Sender") = "btnGetJob" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    Dim SQLUpdateDt As String
                    For Each drResult In Session("Result").Rows
                        SQLUpdateDt = "EXEC S_MsMasterPlanDtAdd " + QuotedStr(ddlMaster.SelectedValue) + _
                        ", " + drResult("JobCode") + _
                        ", " + drResult("StatusTanam") + _
                        ", " + drResult("Capacity").ToString.Replace(",", "") + _
                        ", " + drResult("Unit") + _
                        ", " + drResult("Volume").ToString.Replace(",", "") + _
                        ", " + drResult("QtyAreal").ToString.Replace(",", "") + _
                        ", " + drResult("UnitConvert") + _
                        ", " + drResult("QtyConvert").ToString.Replace(",", "") + _
                        ", " + drResult("Unit") + _
                        ", '" + drResult("Rotasi") + "', " + drResult("UsedMaterial") + _
                        ", " + QuotedStr(ViewState("UserId").ToString)

                        SQLExecuteNonQuery(SQLUpdateDt, ViewState("DBConnection").ToString)
                    Next
                    BindData()
                    'BindGridDt(ViewState("DtMT"), DataGridMaterial)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If

                If ViewState("Sender") = "btnFind" Then
                    'TransNmbr, JobPlant, JobPlant_Name, Rotasi, Team, Person
                    BindToDropList(ddlMaster, Session("Result")(0).ToString)
                    BindData()
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

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr, MasterPlanName FROM PLMasterPlanHd where FgActive = 'Y' "
            ResultField = "TransNmbr, MasterPlanName"
            ViewState("Sender") = "btnFind"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btnReff_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
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
        Dim Sqlstring As String
        Dim lblUnitConvert, lblUnitAreal, LbCode, Label2, Label1 As Label
        Dim QtyConvert, QtyAreal, QtyCap, Volume As TextBox
        Dim GVR As GridViewRow
        Dim UsedMaterial, Rotasi As DropDownList
        Dim btnPercentage, btnView, btnDeleteHd, btnHkfaktor As Button
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            dsGetStatusTanam.ConnectionString = ViewState("DBConnection")
            dsGetRotasi.ConnectionString = ViewState("DBConnection")
            Sqlstring = "EXEC S_MsMasterPlanDtView " + QuotedStr(ddlMaster.SelectedValue) + " , " + QuotedStr(ddlJobGroup.SelectedValue) + " , " + QuotedStr(StrFilter) + " , " + QuotedStr(ViewState("UserId").ToString)
            tempDS = SQLExecuteQuery(Sqlstring, ViewState("DBConnection"))
            BindDataMaster(Sqlstring, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            GVR = DataGrid.FooterRow
            DataGrid.DataBind()
            DataGrid.DataSource = tempDS.Tables(0)

            For Each GVR In DataGrid.Rows
                LbCode = GVR.FindControl("JobCode")
                lblUnitAreal = GVR.FindControl("lbUnitA")
                UsedMaterial = GVR.FindControl("UsedMaterial")
                Rotasi = GVR.FindControl("Rotasi")
                lblUnitConvert = GVR.FindControl("lbUnitC")
                QtyConvert = GVR.FindControl("QtyConvert")
                QtyAreal = GVR.FindControl("QtyAreal")
                QtyCap = GVR.FindControl("Capacity")
                Label1 = GVR.FindControl("Label1")
                Label2 = GVR.FindControl("Label2")
                Volume = GVR.FindControl("Volume")
                btnPercentage = GVR.FindControl("btnPercentage")
                btnDeleteHd = GVR.FindControl("btnDeleteHd")
                btnHkfaktor = GVR.FindControl("btnHKFactor")
                btnView = GVR.FindControl("btnView")
                QtyConvert.Attributes.Add("OnKeyDown", "return PressNumeric();")
                QtyAreal.Attributes.Add("OnKeyDown", "return PressNumeric();")
                QtyCap.Attributes.Add("OnKeyDown", "return PressNumeric();")

                If lblUnitAreal.Text = lblUnitConvert.Text Then
                    QtyConvert.Enabled = False
                    QtyAreal.Enabled = False
                    QtyConvert.Text = "1"
                    QtyAreal.Text = "1"
                Else
                    QtyConvert.Enabled = True
                    QtyAreal.Enabled = True
                End If

                If UsedMaterial.SelectedValue = "N" Then
                    btnPercentage.Enabled = False
                    btnView.Enabled = False
                End If

                If LbCode.Text = "" Then
                    btnPercentage.Visible = False
                    btnView.Visible = False
                    btnHkfaktor.Visible = False
                    btnDeleteHd.Visible = False
                    QtyConvert.Visible = False
                    QtyAreal.Visible = False
                    QtyConvert.Visible = False
                    QtyAreal.Visible = False
                    QtyCap.Visible = False
                    Label1.Visible = False
                    Label2.Visible = False
                    Volume.Visible = False
                    Rotasi.Visible = False
                    UsedMaterial.Visible = False
                End If

            Next

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

                    SQLString = "EXEC S_MsMasterPlanDtViewRMPercentUpdate " + _
                    QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(ViewState("JobCode")) + ", " + QuotedStr(ViewState("StatusTanam")) + ", " + RotasiNo.Text + ", " + RMPercentage.Text + ", " + QuotedStr(ViewState("UserId").ToString)
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

    Protected Sub ddlMaster_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaster.SelectedIndexChanged
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
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsMasterPlanDtViewRMPercent  " + QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam) + "," + CStr(XRotasi) + "," + QuotedStr(ViewState("UserId").ToString)
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DataGridDt.DataSource = tempDS.Tables(0)
            DataGridDt.DataBind()
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridHK(ByVal JobCode As String, ByVal StatusTanam As String, ByVal XRotasi As Integer)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsMasterPlanDtViewHK  " + QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam) + "," + CStr(XRotasi) + "," + QuotedStr(ViewState("UserId").ToString)


            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            DataGridHK.DataSource = tempDS.Tables(0)
            DataGridHK.DataBind()
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnGetMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetMaterial.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame As String
        Try
            sqlstring = "SELECT A.ProductCode,B.ProductName, A.Qty, A.Unit, A.QtyCap FROM MsJobPlantDtRM A inner join MsProduct B ON A.ProductCode = B.ProductCode  Where JobCode = " + QuotedStr(lblJobMaterial2.Text) + " and StatusTanam=" + QuotedStr(lblStatusTanamMaterial2.Text)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "ProductCode, ProductName, Qty, Unit, QtyCap"
            CriteriaField = "ProductCode, ProductName, Qty, Unit, QtyCap"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = "ProductCode"
            'Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetMaterial"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetJob.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame, StrFilter As String

        Try

            If ddlMaster.SelectedValue = "" Then
                lbstatus.Text = ("Master Must be Filled")
                Exit Sub
            End If

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If


            sqlstring = "EXEC S_MsJobPlantCapacityView " + QuotedStr(ddlJobGroup.SelectedValue) + " , " + QuotedStr(StrFilter)
            'sqlstring = "S_MsJobPlantCapacityViewMP " + QuotedStr(StrFilter)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "JobCode, JobName, StatusTanam, statusTanamName, Capacity, Unit, QtyConvert, UnitConvert, QtyAreal, Volume, Rotasi, UsedMaterial, JobGroupCode "
            CriteriaField = "JobCode, JobName, StatusTanam, statusTanamName, Capacity, Unit, QtyConvert, UnitConvert, QtyAreal, Volume, Rotasi, UsedMaterial, JobGroupCode "
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = "ProductCode"
            'Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetJob"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbstatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click
        Dim ResultField, sqlstring, StrFilter As String
        Dim tempDS As New DataSet()

        Try
            If ddlMaster.SelectedValue = "" Then
                lbstatus.Text = ("Master Must be Filled")
                Exit Sub
            End If

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            sqlstring = "Declare @A VarChar(255) EXEC S_MsJobPlantDT " + QuotedStr(ddlMaster.SelectedValue) + "," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId").ToString) + ", @A Out SELECT @A"
            ResultField = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            'If Trim(ResultField) <> "" Then
            '    lbstatus.Text = lbstatus.Text + ResultField

            'End If
            'SQLExecuteNonQuery(sqlstring, ViewState("DBConnection"))
            BindData()
        Catch ex As Exception
            lbstatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub


    Private Sub bindDataGridMaterial(ByVal JobCode As String, ByVal StatusTanam As String)
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim GVR As GridViewRow
        Dim SQLString As String
        Dim Qty, QtyCap As TextBox
        Try
            'ViewState("DtMT") = Nothing

            SQLString = "EXEC S_MsMasterPlanDtViewRM  " + QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam)
            'SQLString = "EXEC S_MsJobPlantMaterialViewDt  " + QuotedStr(JobCode) + "," + QuotedStr(StatusTanam)
            'SQLString = "SELECT A.ProductCode,B.ProductName, A.Qty, A.Unit, A.QtyCap FROM MsJobPlantDtRM A inner join MsProduct B ON A.ProductCode = B.ProductCode  Where JobCode = " + QuotedStr(JobCode) + " and StatusTanam=" + QuotedStr(StatusTanam)

            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection"))
            'ViewState("DtMT") = tempDS.Tables(0)
	    
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridMaterial)
                DV = DT.DefaultView
            Else
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
        Dim SQLString As String
        Dim GVR As GridViewRow = Nothing
        'Dim index As Integer
        Try
            'If Not (e.CommandName = "SearchJobCodeAdd") Then
            '    index = Convert.ToInt32(e.CommandArgument)
            '    GVR = DataGrid.Rows(index)
            'End If

            If e.CommandName = "Percentage" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("JobCode")
                lbName = GVR.FindControl("JobName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                lbXRotasi = GVR.FindControl("XRotation")

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
                lblJobMaterial2.Text = lbCode.Text
                lblStatusTanamMaterial2.Text = lbStatusTanam.Text
                pnlHd.Visible = False
                pnlDt.Visible = False
                pnlViewDt.Visible = True
                bindDataGridMaterial(lbCode.Text, lbStatusTanam.Text)

            ElseIf e.CommandName = "HKFactor" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("JobCode")
                lbName = GVR.FindControl("JobName")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                lbStatusTanamName = GVR.FindControl("StatusTanamName")
                lbXRotasi = GVR.FindControl("XRotation")

                lblJobHK.Text = "Job : " + lbCode.Text + " - " + lbName.Text
                lblStatusTanamHK.Text = "Status : " + lbStatusTanamName.Text
                pnlHd.Visible = False
                pnlHKFactor.Visible = True
                ViewState("JobCode") = lbCode.Text
                ViewState("StatusTanam") = lbStatusTanam.Text
                bindDataGridHK(lbCode.Text, lbStatusTanam.Text, CInt(lbXRotasi.Text))

            ElseIf e.CommandName = "DeleteHd" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("JobCode")
                lbStatusTanam = GVR.FindControl("StatusTanam")
                ViewState("JobCode") = lbCode.Text
                ViewState("StatusTanam") = lbStatusTanam.Text
                SQLString = "EXEC S_MsMasterPlanDtDelete  " + QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(lbCode.Text) + "," + QuotedStr(lbStatusTanam.Text)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                BindData()

            ElseIf e.CommandName = "InsertHd" Then
                Dim JobCode, JobName, Capacity, Qty, QtyConvert, Volume As TextBox
                Dim UnitAreal, Unit, UnitDosis As Label
                Dim StatusTanam, Rotasi, FgUsedMaterial As DropDownList
                JobCode = DataGrid.FooterRow.FindControl("JobCodeAdd")
                JobName = DataGrid.FooterRow.FindControl("JobNameAdd")
                Capacity = DataGrid.FooterRow.FindControl("CapacityAdd")
                QtyConvert = DataGrid.FooterRow.FindControl("QtyConvertAdd")
                Volume = DataGrid.FooterRow.FindControl("VolumeAdd")
                FgUsedMaterial = DataGrid.FooterRow.FindControl("UsedMaterialAdd")
                StatusTanam = DataGrid.FooterRow.FindControl("StatusTanamAdd")
                Rotasi = DataGrid.FooterRow.FindControl("RotasiAdd")
                UnitAreal = DataGrid.FooterRow.FindControl("lbUnitArAdd")
                UnitDosis = DataGrid.FooterRow.FindControl("lbUnitCAdd")
                Qty = DataGrid.FooterRow.FindControl("QtyArealAdd")
                Unit = DataGrid.FooterRow.FindControl("lbUnitCapAdd")

                If JobCode.Text.Trim.Length = 0 Then
                    lbstatus.Text = "Job Code must be filled."
                    JobCode.Focus()
                    Exit Sub
                End If
                If StatusTanam.SelectedValue.Trim.Length = 0 Then
                    lbstatus.Text = "Status Tanam must be filled."
                    StatusTanam.Focus()
                    Exit Sub
                End If
                If Capacity.Text.Trim = "" Or Capacity.Text.Trim = "0" Then
                    lbstatus.Text = "Capacity must be filled."
                    Rotasi.Focus()
                    Exit Sub
                End If

                If QtyConvert.Text.Trim = "" Or Capacity.Text.Trim = "0" Then
                    lbstatus.Text = "Qty Convert must be filled."
                    Rotasi.Focus()
                    Exit Sub
                End If

                If Rotasi.SelectedValue.Trim.Length = 0 Then
                    lbstatus.Text = "Rotasi must be filled."
                    Rotasi.Focus()
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "EXEC S_MsMasterPlanDtAdd " + QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(JobCode.Text) + ", " + QuotedStr(StatusTanam.SelectedValue) + _
                   ", " + Capacity.Text.Replace(",", "") + ", " + QuotedStr(UnitAreal.Text) + ", " + Volume.Text.Replace(",", "") + ", " + Qty.Text.Replace(",", "") + ", " + QuotedStr(Unit.Text) + _
                   ", " + QtyConvert.Text.Replace(",", "") + ", " + QuotedStr(UnitDosis.Text) + ", " + QuotedStr(Rotasi.SelectedValue) + ", " + QuotedStr(FgUsedMaterial.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                'lbstatus.Text = SQLString
                'Exit Sub
                BindData()

                'If DataGrid.Rows.Count > 0 Then
                '    DataGrid.Rows(0).Visible = True

                'End If

            ElseIf e.CommandName = "SearchJobCodeEdit" Or e.CommandName = "SearchJobCodeAdd" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select JobCode, JobName, UnitAreal, Unit, UnitConvert from V_MsJobPlantView WHERE FgActive = 'Y' AND JobGroupPlant = " + QuotedStr(ddlJobGroup.SelectedValue)
                FieldResult = "JobCode, JobName, UnitAreal, Unit, UnitConvert"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchJobCodeAdd" Then
                    ViewState("Sender") = "SearchJobCodeAdd"
                Else
                    ViewState("Sender") = "SearchJobCodeEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
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

                SQLString = "EXEC S_MsMasterPlanDtViewRMUpdate " + QuotedStr(ddlMaster.SelectedValue) + "," + QuotedStr(ViewState("JobCode")) + "," + QuotedStr(ViewState("StatusTanam")) + _
                "," + QuotedStr(tbProductCode.Text) + "," + dbQty.Text.Replace(",", "") + "," + _
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
            SQLExecuteNonQuery("Delete from PLMasterPlanDtRM where JobPlant = " + QuotedStr(ViewState("JobCode")) + " AND StatusTanam = " + QuotedStr(ViewState("StatusTanam")) + " AND MaterialCode =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
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
        Dim tbProductCode, dbUnit, lbCode As Label
        Dim dbQty, dbQtyCap As TextBox
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridMaterial.Rows(e.RowIndex)
            lbCode = GVR.FindControl("ProductCodeEdit")
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
            SQLString = "UPDATE PLMasterPlanDtRM SET Qty = " + QuotedStr(dbQty.Text) + _
            ", Unit = " + QuotedStr(dbUnit.Text) + _
            ", QtyCap = " + dbQtyCap.Text + _
            ", UserId = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = Getdate() " + _
            " WHERE MaterialCode = " + QuotedStr(lbCode.Text) + " "
            '" WHERE JobPlant = " + QuotedStr(ViewState("JobCode")) + " AND StatusTanam = " + QuotedStr(ViewState("StatusTanam"))
            SQLString = Replace(SQLString, "''", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

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
            pnlHKFactor.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnApply1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply1.Click
        Dim SQLString As String
        Dim lbCode, StatusTanam, lbUnitC As Label
        Dim QtyConvert, QtyAreal, Capacity, Volume As TextBox
        Dim Rotasi, UsedMaterial As DropDownList
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                'ddlMasterPlan = GVR.FindControl("MasterNo")
                lbCode = GVR.FindControl("JobCode")
                StatusTanam = GVR.FindControl("StatusTanam")
                QtyConvert = GVR.FindControl("QtyConvert")
                lbUnitC = GVR.FindControl("lbUnitC")
                QtyAreal = GVR.FindControl("QtyAreal")
                Capacity = GVR.FindControl("Capacity")
                Volume = GVR.FindControl("Volume")
                UsedMaterial = GVR.FindControl("UsedMaterial")
                Rotasi = GVR.FindControl("Rotasi")
                QtyConvert.Text = QtyConvert.Text.Replace(",", "")
                QtyAreal.Text = QtyAreal.Text.Replace(",", "")
                Capacity.Text = Capacity.Text.Replace(",", "")
                Volume.Text = Volume.Text.Replace(",", "")

                If QtyConvert.Text.Trim = "" Then
                    QtyConvert.Text = "0"
                End If
                If QtyAreal.Text.Trim = "" Then
                    QtyAreal.Text = "0"
                End If
                If Capacity.Text.Trim = "" Then
                    Capacity.Text = "0"
                End If
                If Volume.Text.Trim = "" Then
                    Volume.Text = "0"
                End If
                If Not IsNumeric(QtyConvert.Text) Then
                    lbstatus.Text = "Qty Convert for " + lbCode.Text + " must in numeric format"
                    exe = False
                    QtyConvert.Focus()
                    Exit For
                End If
                If Not IsNumeric(QtyAreal.Text) Then
                    lbstatus.Text = "Qty Areal for " + lbCode.Text + " must in numeric format"
                    exe = False
                    QtyAreal.Focus()
                    Exit For
                End If
                If Not IsNumeric(Capacity.Text) Then
                    lbstatus.Text = "Capacity for " + lbCode.Text + " must in numeric format"
                    exe = False
                    Capacity.Focus()
                    Exit For
                End If
                If Not IsNumeric(Volume.Text) Then
                    lbstatus.Text = "Volume for " + lbCode.Text + " must in numeric format"
                    exe = False
                    Volume.Focus()
                    Exit For
                End If
            Next

            If exe Then
                For i = 0 To DataGrid.Rows.Count - 1
                    GVR = DataGrid.Rows(i)
                    'ddlMasterPlan = GVR.FindControl("MasterNo")
                    lbCode = GVR.FindControl("JobCode")
                    StatusTanam = GVR.FindControl("StatusTanam")
                    Capacity = GVR.FindControl("Capacity")
                    QtyConvert = GVR.FindControl("QtyConvert")
                    lbUnitC = GVR.FindControl("lbUnitC")
                    QtyAreal = GVR.FindControl("QtyAreal")
                    Volume = GVR.FindControl("Volume")
                    UsedMaterial = GVR.FindControl("UsedMaterial")
                    Rotasi = GVR.FindControl("Rotasi")
                    Capacity.Text = Capacity.Text.Replace(",", "")
                    QtyConvert.Text = QtyConvert.Text.Replace(",", "")
                    QtyAreal.Text = QtyAreal.Text.Replace(",", "")
                    Volume.Text = Volume.Text.Replace(",", "")
                    

                    If StatusTanam.Text = "3040" Or StatusTanam.Text = "3050" Then
                        If Not ((QtyConvert.Text = "" Or QtyConvert.Text = "0")) And Not (Capacity.Text = "" Or Capacity.Text = "0") And Not (QtyAreal.Text = "" Or QtyAreal.Text = "0") And Not (Rotasi.SelectedValue = "") Then
                            SQLString = "EXEC S_MsMasterPlanDtUpdate " + _
                                QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(StatusTanam.Text) + ", " + QtyConvert.Text + ", " + QuotedStr(lbUnitC.Text) + ", " + QtyAreal.Text + ", " + Capacity.Text + ", " + Volume.Text + ", " + QuotedStr(UsedMaterial.SelectedValue) + ", " + QuotedStr(Rotasi.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                            SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

                            ' Else
                            ' lbstatus.Text = MessageDlg("Test")
                        End If
                    Else

                        SQLString = "EXEC S_MsMasterPlanDtUpdate " + _
                            QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(lbCode.Text) + ", " + QuotedStr(StatusTanam.Text) + ", " + QtyConvert.Text + ", " + QuotedStr(lbUnitC.Text) + ", " + QtyAreal.Text + ", " + Capacity.Text + ", " + Volume.Text + ", " + QuotedStr(UsedMaterial.SelectedValue) + ", " + QuotedStr(Rotasi.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)

                        SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

                    End If
                Next
                BindData()
            End If

        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            pnlHd.Visible = False
            pnlView.Visible = True
            ViewState("State") = "Insert"
            tbMasterName.Enabled = True
            ModifyInput(True, pnlView)
            BtnSave.Visible = True
            btnCancel.Visible = True
            tbMasterName.Text = ""
            ddlVarietas.SelectedValue = ""
            ddlLandScape.SelectedValue = ""
            ddlLandType.SelectedValue = ""
            tbDate.SelectedDate = Now.Date
            tbMasterName.Focus()
        Catch ex As Exception
            lbstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlView.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lbstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal TransNmbr As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM PLMasterPlanHd  WHERE TransNmbr = " + QuotedStr(TransNmbr)
            DT = BindDataTransaction(SqlString, "", ViewState("DBConnection").ToString)
            BindToText(tbMasterCode, DT.Rows(0)("TransNmbr").ToString)
            BindToText(tbMasterName, DT.Rows(0)("MasterPlanName").ToString)
            BindToDropList(ddlVarietas, DT.Rows(0)("Varietas").ToString)
            BindToDate(tbDate, DT.Rows(0)("EffectiveDate").ToString)
            BindToDropList(ddlLandType, DT.Rows(0)("LandType").ToString)
            BindToDropList(ddlLandScape, DT.Rows(0)("LandScape").ToString)
            BindToDropList(ddlPlantType, DT.Rows(0)("PlanType").ToString)
            BindToText(QtyPokok, DT.Rows(0)("QtyPokok").ToString)
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub


    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Try
            ' GVR = DataGrid.Rows(index)
            MovePanel(pnlHd, pnlView)
            FillTextBox(ddlMaster.SelectedValue)
            ViewState("State") = "Edit"
            ModifyInput(True, pnlView)
            'tbMasterName.Enabled = False
            BtnSave.Visible = True
            btnCancel.Visible = True
            tbMasterName.Focus()
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "btnEdit error: " & ex.ToString
        End Try
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            SQLExecuteNonQuery("EXEC S_MsMasterPlanDelete " + QuotedStr(ddlMaster.SelectedValue) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            FillCombo(ddlMaster, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", False, "TransNmbr", "MasterPlanName", ViewState("DBConnection").ToString)
            FillCombo(ddlCopyFrom, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", False, "TransNmbr", "MasterPlanName", ViewState("DBConnection").ToString)
            btnEdit.Visible = ddlMaster.Items.Count > 0
            btnDelete.Visible = ddlMaster.Items.Count > 0
            BindData()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Year, Period As Integer
        Dim LastMP, Nmbr, SqlString As String
        Try
            If tbMasterName.Text.Trim = "" Then
                lbstatus.Text = "Master Plan Name must have value"
                tbMasterName.Focus()
                Exit Sub
            End If
            If ViewState("State") = "Insert" Then
                Year = Now.Year
                Period = Now.Month
                Nmbr = GetAutoNmbr("MP", "Y", Year, Period, "", ViewState("DBConnection").ToString)
                SqlString = "INSERT INTO PLMasterPlanHd ( TransNmbr, MasterPlanName, EffectiveDate, Varietas, LandType, LandScape, PlanType, QtyPokok, FgActive, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(Nmbr) + ", " + QuotedStr(tbMasterName.Text) + ", '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlVarietas.SelectedValue) + _
                 ", " + QuotedStr(ddlLandType.SelectedValue) + ", " + QuotedStr(ddlLandScape.SelectedValue) + ", " + QuotedStr(ddlPlantType.SelectedValue) + ", " + QuotedStr(QtyPokok.Text) + _
                ", 'Y', " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

            Else
                SqlString = "UPDATE PLMasterPlanHd SET MasterPlanName= " + QuotedStr(tbMasterName.Text) & _
                        ", EffectiveDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " & _
                        ", Varietas = " + QuotedStr(ddlVarietas.SelectedValue) & _
                        ", LandType = " + QuotedStr(ddlLandType.SelectedValue) & _
                        ", LandScape = " + QuotedStr(ddlLandScape.SelectedValue) & _
                        ", PlanType = " + QuotedStr(ddlPlantType.SelectedValue) & _
                        ", QtyPokok = " + QuotedStr(QtyPokok.Text) & _
                        ", UserId = " + QuotedStr(ViewState("UserId").ToString) & _
                        ", UserDate = GetDate() " & _
                        " WHERE TransNmbr = " + QuotedStr(ddlMaster.SelectedValue)

            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            LastMP = ddlMaster.SelectedValue
            FillCombo(ddlMaster, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", False, "TransNmbr", "MasterPlanName", ViewState("DBConnection").ToString)
            FillCombo(ddlCopyFrom, "Select TransNmbr, MasterPlanName  from PLMasterPlanHd where FgActive = 'Y' ", False, "TransNmbr", "TransNmbr", ViewState("DBConnection").ToString)
            'ddlMaster.SelectedValue = LastMP
            btnEdit.Visible = ddlMaster.Items.Count > 0
            btnDelete.Visible = ddlMaster.Items.Count > 0
            'BindData()
            pnlView.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lbstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopyFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyFrom.Click
        Try
            If ddlMaster.SelectedValue = ddlCopyFrom.SelectedValue Then
                lbstatus.Text = "Master Plan cannot same with Copy From"
                Exit Sub
            End If
            SQLExecuteNonQuery("EXEC S_MsMasterPlanCopy " + QuotedStr(ddlMaster.SelectedValue) + "," + QuotedStr(ddlCopyFrom.SelectedValue) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            BindData()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnApplyHK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyHK.Click
        Dim RotasiNo As Label
        Dim HKFactor As TextBox
        Dim SQLString As String
        'Dim Percent As Double
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            'Percent = 0
            For i = 0 To DataGridHK.Rows.Count - 1
                GVR = DataGridHK.Rows(i)
                RotasiNo = GVR.FindControl("RotasiNo")
                HKFactor = GVR.FindControl("HKFactor")

                HKFactor.Text = HKFactor.Text.Replace(",", "")

                If HKFactor.Text.Trim = "" Then
                    HKFactor.Text = "0"
                End If

                If Not IsNumeric(HKFactor.Text) Then
                    lbstatus.Text = "Percentage for must in numeric format"
                    exe = False
                    HKFactor.Focus()
                    Exit For
                End If
                ' Percent = Percent + CFloat(HKFactor.Text)
            Next

            'If exe = True Then
            '    If Percent <> 100 Then
            '        lbstatus.Text = "Percentage (%) must equal to 100 "
            '        Exit Sub
            '    End If
            ' simpan ke database

            For i = 0 To DataGridDt.Rows.Count - 1
                GVR = DataGridDt.Rows(i)
                RotasiNo = GVR.FindControl("RotasiNo")
                HKFactor = GVR.FindControl("HKFactor")

                HKFactor.Text = HKFactor.Text.Replace(",", "")
                If HKFactor.Text.Trim = "" Then
                    HKFactor.Text = "0"
                End If

                SQLString = "EXEC S_MsMasterPlanDtViewHKUpdate " + _
                QuotedStr(ddlMaster.SelectedValue) + ", " + QuotedStr(ViewState("JobCode")) + ", " + QuotedStr(ViewState("StatusTanam")) + ", " + RotasiNo.Text + ", " + HKFactor.Text + ", " + QuotedStr(ViewState("UserId").ToString)
                SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            Next

        Catch ex As Exception
            lbstatus.Text = " btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackHK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackHK.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlViewDt.Visible = False
            pnlHKFactor.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnbackHK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbackHK2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            pnlViewDt.Visible = False
            pnlHKFactor.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbJobCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim dr As DataRow
        'Dim ds As DataSet
        'Dim Acc, tb, AccName As TextBox
        ''Dim AccName As Label
        'Dim Count As Integer
        'Dim dgi As GridViewRow
        'Try
        '    tb = sender
        '    If tb.ID = "JobCodeAdd" Then
        '        Count = DataGrid.Controls(0).Controls.Count
        '        dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
        '        Acc = dgi.FindControl("JobCodeAdd")
        '        AccName = dgi.FindControl("JobNameAdd")
        '        ds = SQLExecuteQuery("Select JobCode, JobName FROM V_MsJobPlantView WHERE FgActive = 'Y' AND JobCode = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)

        '    Else
        '        Count = DataGrid.EditIndex
        '        dgi = DataGrid.Rows(Count)
        '        Acc = dgi.FindControl("JobCodeAdd")
        '        AccName = dgi.FindControl("JobNameAdd")
        '        ds = SQLExecuteQuery("Select JobCode, JobName FROM V_MsJobPlantView WHERE FgActive = 'Y' AND JobCode = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
        '    End If
        '    If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '        Acc.Text = ""
        '        AccName.Text = ""
        '    Else
        '        dr = ds.Tables(0).Rows(0)
        '        Acc.Text = dr("JobCode").ToString
        '        AccName.Text = dr("JobName").ToString
        '    End If
        'Catch ex As Exception
        '    lbstatus.Text = "tb Job Code Changed Error : " + ex.ToString
        'End Try
    End Sub


    
End Class
