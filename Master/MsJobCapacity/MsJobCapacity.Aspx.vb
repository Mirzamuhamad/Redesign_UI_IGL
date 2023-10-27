Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsJobCapacity_MsJobCapacity
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                FillCombo(ddlJobGroup, "Select JobPlantGrpCode, JobPlantGrpName from MsJobPlantGroup", False, "JobPlantGrpCode", "JobPlantGrpName", ViewState("DBConnection").ToString)
                'tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today                
                BindData()
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "Page Load Error : " + ex.ToString
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

    Sub BindData()
        Dim tempDS As New DataSet()
        Dim StrFilter As String
        Dim lblUnitConvert, lblUnitAreal As Label
        Dim QtyConvert, QtyAreal, QtyCap As TextBox
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            dsGetSection.ConnectionString = ViewState("DBConnection")
            tempDS = SQLExecuteQuery("EXEC S_MsJobPlantCapacityView " + QuotedStr(ddlJobGroup.SelectedValue) + " , " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            For Each GVR In DataGrid.Rows
                lblUnitAreal = GVR.FindControl("lbUnitA")
                lblUnitConvert = GVR.FindControl("lbUnitC")
                QtyConvert = GVR.FindControl("QtyConvert")
                QtyAreal = GVR.FindControl("QtyAreal")
                QtyCap = GVR.FindControl("Capacity")
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
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim SQLString As String
        Dim lbCode, StatusTanam As Label
        Dim QtyConvert, QtyAreal, Capacity, Volume As TextBox
        Dim Rotasi, UsedMaterial As DropDownList
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True

            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                lbCode = GVR.FindControl("JobCode")
                StatusTanam = GVR.FindControl("StatusTanam")
                QtyConvert = GVR.FindControl("QtyConvert")
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

                    lbCode = GVR.FindControl("JobCode")
                    StatusTanam = GVR.FindControl("StatusTanam")
                    QtyConvert = GVR.FindControl("QtyConvert")
                    QtyAreal = GVR.FindControl("QtyAreal")
                    Capacity = GVR.FindControl("Capacity")
                    Volume = GVR.FindControl("Volume")
                    UsedMaterial = GVR.FindControl("UsedMaterial")
                    Rotasi = GVR.FindControl("Rotasi")
                    QtyConvert.Text = QtyConvert.Text.Replace(",", "")
                    QtyAreal.Text = QtyAreal.Text.Replace(",", "")
                    Capacity.Text = Capacity.Text.Replace(",", "")
                    Volume.Text = Volume.Text.Replace(",", "")
                    'lbstatus.Text = StatusTanam.Text
                    'Exit Sub


                    If StatusTanam.Text = "3040" Or StatusTanam.Text = "3050" Then
                        If Not ((QtyConvert.Text = "" Or QtyConvert.Text = "0")) And Not (Capacity.Text = "" Or Capacity.Text = "0") And Not (QtyAreal.Text = "" Or QtyAreal.Text = "0") And Not (Rotasi.SelectedValue = "") Then
                            SQLString = "EXEC S_MsJobPlantCapacityUpdate " + _
                                QuotedStr(lbCode.Text) + ", " + QuotedStr(StatusTanam.Text) + ", " + QtyConvert.Text + ", " + QtyAreal.Text + ", " + Capacity.Text + ", " + Volume.Text + ", " + QuotedStr(UsedMaterial.SelectedValue) + ", " + QuotedStr(Rotasi.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                            'lbstatus.Text = SQLString
                            'Exit Sub
                            SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                        Else
                            lbstatus.Text = MessageDlg("QtyConvert Or Capacity Or QtyAreal Or Rotasi Must Have Value")
                        End If
                    Else
  
                        'If Not ((QtyConvert.Text = "" Or QtyConvert.Text = "0")) And Not (Capacity.Text = "" Or Capacity.Text = "0") And Not (QtyAreal.Text = "" Or QtyAreal.Text = "0") And Not (Rotasi.SelectedValue = "") Then
                        SQLString = "EXEC S_MsJobPlantCapacityUpdate " + _
                            QuotedStr(lbCode.Text) + ", " + QuotedStr(StatusTanam.Text) + ", " + QtyConvert.Text + ", " + QtyAreal.Text + ", " + Capacity.Text + ", " + Volume.Text + ", " + QuotedStr(UsedMaterial.SelectedValue) + ", " + QuotedStr(Rotasi.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)
                        SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                        'Else
                        '    lbstatus.Text = MessageDlg("QtyConvert Or Capacity Or QtyAreal Or Rotasi Must Have Value")
                        'End If
                    End If
                Next
                BindData()
            End If

        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
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
        'namespace (using system.IO)      
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
                tempDS = SQLExecuteQuery("EXEC S_MsJobPlantCapacityView " + QuotedStr(ddlJobGroup.SelectedValue) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try
            ExportGridToExcel("Job Capacity")
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
            'lstatus.Text = "ddlCategory_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
