Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrWOPlanStatus_TrWOPlanStatus
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "SELECT * FROM V_MsOPRJobReportHdListing"
    'And StatusWrhs IN ('Y','N') 
    Private Function GetStringHd(ByVal Statuslevel As String) As String
        'If ViewState("StatusLevel") = "Warehouse" Then
        Return "EXEC S_PLWOStatusSearch " + QuotedStr(Statuslevel + " And S.UserId = " + QuotedStr(ViewState("UserId")))
        'End If
    End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        'Return "SELECT * FROM V_MsOPRJobReportDtListing WHERE JobNo = " + QuotedStr(Nmbr) + " "
        Return "SELECT * FROM  V_PLWOStatusDt Where TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Private Function GetStringDtMI(ByVal Nmbr As String) As String
        Return "EXEC S_PLWOStatusDtInfo " + QuotedStr(Nmbr) + " "
    End Function

    Private Function GetStringDtSPE(ByVal Nmbr As String) As String
        Return "EXEC S_PLWOStatusDtMaterial " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim getValuePost As String = Request.Params.Get("__EVENTTARGET")

            If Not IsPostBack Then
                InitProperty()
                '  tbChangeDate.SelectedDate = ViewState("ServerDate") 'Date.Today
                FillCombo(ddlwrhsSrc, "select * From MsWarehouse where WrhsCondition  = 'GOOD'", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "select * From MsWarehouse where WrhsCondition  = 'WO'", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
                FillCombo(ddlwrhs, "select * From MsWarehouse where WrhsCondition  = 'GOOD'", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
                FillCombo(ddlwrhsdest2, "select * From MsWarehouse where WrhsCondition  = 'WO'", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                ViewState("FromPage") = False
                If Request.QueryString("ContainerId").ToString = "TrWOPlanStatusWrhsID" Then
                    ViewState("StatusLevel") = "Warehouse"
                    ViewState("Level") = "1"
                ElseIf Request.QueryString("ContainerId").ToString = "TrWOPlanStatusAsistenID" Then
                    ViewState("StatusLevel") = "Asisten"
                    ViewState("Level") = "2"
                Else
                    ViewState("StatusLevel") = "Manager"
                    ViewState("Level") = "3"
                End If
                lblstatuslevel.Text = ViewState("StatusLevel")



            End If
            If Not Request.QueryString("Code") Is Nothing Then
                FromTransactionPage()
                ViewState("FromPage") = True

            End If

            If getValuePost <> "send" Then
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            If Not Session("FgAdvanceFilter") Is Nothing Then
                GridViewHd.PageSize = ddlShowRecord.SelectedValue
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            lbStatus.Text = ""
            getValuePost = ""


            If Left(btnWOLevel.Text, 2) = "Un" Or btnWOLevel.Text = "" Then
                lbremarkApprov.Visible = False
                TbremarkApprov.Visible = False
                lbremarkApprovDt.Visible = False
                TbremarkApprovDt.Visible = False

            Else
                lbremarkApprovDt.Visible = True
                TbremarkApprovDt.Visible = True
            
                lbremarkApprov.Visible = True
                TbremarkApprov.Visible = True
            End If

            'lbStatus.Text = MessageDlg(HiddenRemarkClose)
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FromTransactionPage()
        Dim param() As String

        Try
            param = Request.QueryString("Code").ToString.Split("|")

            If Request.QueryString("ContainerId").ToString = "TrWOPlanStatusWrhsID" Then
                lblstatuslevel.Text = "Warehouse"
            ElseIf Request.QueryString("ContainerId").ToString = "TrWOPlanStatusAsistenID" Then
                lblstatuslevel.Text = "Asisten"
            Else
                lblstatuslevel.Text = "Manager"
            End If
            BindDataDt(param(0))
            BindDataDtMI(param(0))
            BindDataDtSPE(param(0))

            lblWono.Text = param(1)
            lblWODate.Text = param(2)
            lblWorkBy.Text = param(3)
            lblJobPlant.Text = param(4)
            lblReff.Text = param(5)
            lblQty.Text = param(6)
            lblUnit.Text = param(7)

            PnlHd.Visible = False
            pnlDetail.Visible = True
        Catch ex As Exception
            lbStatus.Text = "FromTransactionPage Error : " + ex.ToString
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
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        'End If

        tbFilter.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('btnSearch').click();return false;}} else {return true}; ")
    End Sub

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'If CommandName = "Edit" Then
            '    If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
            '        lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If
            'If CommandName = "Delete" Then
            '    If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
            '        lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
            '        Return False
            '    End If
            'End If
            If CommandName = "Cancel" Then
                If ViewState("MenuLevel").Rows(0)("FgCancel") = "N" Or ViewState("MenuLevel").Rows(0)("FgCancel") = "X" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to cancel record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            If CommandName = "UnPostBA" Then
                If ViewState("MenuLevel").Rows(0)("FgUnPost") = "N" Or ViewState("MenuLevel").Rows(0)("FgUnPost") = "X" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to UnPost BA record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            If CommandName = "UnPostSPE" Then
                If ViewState("MenuLevel").Rows(0)("FgUnPost") = "N" Or ViewState("MenuLevel").Rows(0)("FgUnPost") = "X" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to UnPost SPE record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            If CommandName = "UnPostIssue" Then
                If ViewState("MenuLevel").Rows(0)("FgUnPost") = "N" Or ViewState("MenuLevel").Rows(0)("FgUnPost") = "X" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to UnPost Issue record. Please contact administrator')}</script>"
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
        Dim StrFilter, Strparam As String

        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            Else
                'StrFilter = StrFilter.Remove(1, 5)
                'StrFilter = " And " + StrFilter
                'StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue, "TransDate")
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue, "TransDate")
                'StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            End If

            If StrFilter.Length > 5 Then
                If ViewState("StatusLevel") = "Warehouse" Then
                    Strparam = " And StatusWrhs IN ('Y','N') AND " + Replace(StrFilter, " TransNmbr", " A.TransNmbr")
                ElseIf ViewState("StatusLevel") = "Asisten" Then
                    Strparam = " And StatusWrhs IN ('Y','X') AND " + Replace(StrFilter, " TransNmbr", " A.TransNmbr")
                Else
                    Strparam = " And StatusWrhs IN ('Y','X')AND  " + Replace(StrFilter, " TransNmbr", " A.TransNmbr")
                End If
            Else
                If ViewState("StatusLevel") = "Warehouse" Then
                    Strparam = " And StatusWrhs IN ('Y','N') "
                ElseIf ViewState("StatusLevel") = "Asisten" Then
                    Strparam = " And StatusWrhs IN ('Y','X') "
                Else
                    Strparam = " And StatusWrhs IN ('Y','X') "
                End If
            End If


            DT = BindDataTransaction(GetStringHd(Strparam), "", ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data" 
                pnlChangeTeam.Visible = False
                pnlWrhs.Visible = False
            Else
                pnlChangeTeam.Visible = True
                pnlWrhs.Visible = True
            End If

            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransNmbr DESC"
            End If

            If lblstatuslevel.Text = "Warehouse" Then

                pnlChangeTeam.Visible = True
            Else
                pnlChangeTeam.Visible = False
            End If

            DV.Sort = ViewState("SortExpression")
            GridViewHd.DataSource = DV
            GridViewHd.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Dim Dt As New DataTable
        Dim DV As DataView

        Try
            ViewState("Dt") = Nothing
            Dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = Dt

            DV = Dt.DefaultView
            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "TransNmbr DESC"
            End If

            DV.Sort = ViewState("SortExpressionDt")
            GridDt.DataSource = DV
            GridDt.DataBind()

            BindGridDt(Dt, GridDt)
            GridDt.Columns(0).Visible = False
        Catch ex As Exception
            Throw New Exception("BindDataDt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtMI(ByVal Referens As String)
        Dim Dr As DataRow
        Dim Dt As New DataTable
        Dim DV As DataView
        Dim row As DataRow
        Dim tempDS As New DataSet()
        Dim qtyConv, qtyConvView As Label
        Dim rental, materialType, qtyDone As String
        Dim qty As Single

        Try
            ViewState("DtMI") = Nothing
            Dt = SQLExecuteQuery(GetStringDtMI(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtMI") = Dt

            'lbStatus.Text = ViewState("DtMI")
            'Exit Sub

            DV = Dt.DefaultView
            If ViewState("SortExpressionDtMI") = Nothing Then
                ViewState("SortExpressionDtMI") = "TransNmbr DESC"
            End If

            If Dt.Rows.Count <> 0 Then
                DV.Sort = ViewState("SortExpressionDtMI")
                GridDtMI.DataSource = DV
                GridDtMI.DataBind()
                GridDtMI.Enabled = True

                For Each GVR In GridDtMI.Rows
                    rental = GVR.Cells(2).Text
                    materialType = GVR.Cells(4).Text
                    'qtyDone = GVR.Cells(12).Text
                    'qtyConvView = GVR.FindControl("lblQtyConvertView")
                    'qtyConv = GVR.FindControl("lblQtyConvert")

                    'Dt = SQLExecuteQuery("Select Qty From MsRentalBom Where ProductRental = " + QuotedStr(rental) + " And MaterialType = " + QuotedStr(materialType), ViewState("DBConnection").ToString).Tables(0)
                    'If Dt.Rows.Count > 0 Then
                    '    Dr = Dt.Rows(0)
                    '    qty = Dr("Qty")
                    'Else
                    '    Dr = Nothing
                    '    qty = 1
                    'End If

                    ' qtyConvView.Text = nilaiKoma((CFloat(qtyDone) / CFloat(qtyConv.Text)), ViewState("DigitPercent"))
                Next
            Else
                tempDS = SQLExecuteQuery(GetStringDtMI(Referens), ViewState("DBConnection").ToString)
                row = tempDS.Tables(0).NewRow
                tempDS.Tables(0).Rows.Add(row)
                GridDtMI.DataSource = tempDS.Tables(0)
                GridDtMI.DataBind()
                GridDtMI.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("BindDataDtMI Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtSPE(ByVal Referens As String)
        Dim Dr As DataRow
        Dim Dt As New DataTable
        Dim DV As DataView
        Dim row As DataRow
        Dim tempDS As New DataSet()
        Dim qtyConv, qtyConvView As Label
        Dim rental, materialType, qtyIssue, qtySubstitute As String
        Dim qty As Single

        Try
            ViewState("DtSPE") = Nothing
            Dt = SQLExecuteQuery(GetStringDtSPE(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtSPE") = Dt

            DV = Dt.DefaultView
            If ViewState("SortExpressionDtSPE") = Nothing Then
                ViewState("SortExpressionDtSPE") = "DivisiBlok DESC"
            End If

            If Dt.Rows.Count <> 0 Then
                DV.Sort = ViewState("SortExpressionDtSPE")
                GridDtSPE.DataSource = DV
                GridDtSPE.DataBind()
                GridDtSPE.Enabled = True

                For Each GVR In GridDtSPE.Rows
                    rental = GVR.Cells(2).Text
                    materialType = GVR.Cells(4).Text
                    'qtyIssue = GVR.Cells(11).Text
                    'qtySubstitute = GVR.Cells(12).Text
                    'qtyConvView = GVR.FindControl("lblQtyConvertView")
                    'qtyConv = GVR.FindControl("lblQtyConvert")

                    'Dt = SQLExecuteQuery("Select Qty From MsRentalBom Where ProductRental = " + QuotedStr(rental) + " And MaterialType = " + QuotedStr(materialType), ViewState("DBConnection").ToString).Tables(0)
                    'If Dt.Rows.Count > 0 Then
                    '    Dr = Dt.Rows(0)
                    '    qty = Dr("Qty")
                    'Else
                    '    Dr = Nothing
                    '    qty = 1
                    'End If

                    'If qtyIssue <> 0 And qtySubstitute = 0 Then
                    '    'qtyConvView.Text = nilaiKoma((CFloat(qty) / CFloat(qtyConv.Text)) * CFloat(qtyIssue), ViewState("DigitPercent"))
                    '    ' qtyConvView.Text = nilaiKoma((CFloat(qtyIssue) / CFloat(qtyConv.Text)), ViewState("DigitPercent")).ToString
                    'End If
                    'If qtyIssue = 0 And qtySubstitute <> 0 Then
                    '    'qtyConvView.Text = nilaiKoma((CFloat(qty) / CFloat(qtyConv.Text)) * CFloat(qtySubstitute), ViewState("DigitPercent"))
                    '    ' qtyConvView.Text = nilaiKoma((CFloat(qtySubstitute) / CFloat(qtyConv.Text)), ViewState("DigitPercent")).ToString
                    'End If
                    'If qtyIssue = 0 And qtySubstitute = 0 Then
                    '    qtyConvView.Text = "0"
                    'End If
                Next
            Else
                tempDS = SQLExecuteQuery(GetStringDtSPE(Referens), ViewState("DBConnection").ToString)
                row = tempDS.Tables(0).NewRow
                tempDS.Tables(0).Rows.Add(row)
                GridDtSPE.DataSource = tempDS.Tables(0)
                GridDtSPE.DataBind()
                GridDtSPE.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("BindDataDtSPE Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    Try
    '        GridViewHd.PageIndex = 0
    '        Session("AdvanceFilter") = ""
    '        BindData(Session("AdvanceFilter"))
    '    Catch ex As Exception
    '        lbStatus.Text = "Btn Search Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSearch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.ServerClick
        Try
            GridViewHd.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindData(Session("AdvanceFilter"))


           

        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
    '    Try
    '        tbfilter2.Text = ""
    '        If pnlSearch.Visible Then
    '            pnlSearch.Visible = False
    '        Else
    '            pnlSearch.Visible = True
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Expand Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnExpand_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.ServerClick
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
        CheckAll(GridViewHd, sender)

    End Sub

    Protected Sub cbSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridViewHd.Rows

                If cb.Checked = True Then
                    Statuslevel(GRW.Cells(16).Text, GRW.Cells(17).Text, GRW.Cells(18).Text)
                End If

            Next

            If Left(btnWOLevel.Text, 2) = "Un" Or btnWOLevel.Text = "" Then
                lbremarkApprov.Visible = False
                TbremarkApprov.Visible = False
            Else
                lbremarkApprov.Visible = True
                TbremarkApprov.Visible = True
            End If


        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "dbo.FormatDate(AssignDate)"
            FilterName = "Job No, Assign Date, Team, Type, Building Code, Building Name, Customer, Customer Name, Contract No, Active, Done BA, BAP No., BAP Date, Done Material, Issue No., Issue Date, Done SPE, SPE No., SPE Date"
            FilterValue = "JobNo, dbo.FormatDate(AssignDate), TeamName, Type, Building, BuildingName, Customer, CustomerName, ContractNo, FgActive, DoneBAP, BAPNmbr, BAPDate, DoneTT, TTNmbr, TTDate, DoneIssue, IssueNmbr, IssueDate"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            Session("DateTime") = ViewState("ServerDate")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewHd_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewHd.PageIndexChanging
        GridViewHd.PageIndex = e.NewPageIndex
        GridViewHd.PageSize = ddlShowRecord.SelectedValue
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub GridViewHd_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridViewHd.RowCancelingEdit

    End Sub

    Protected Sub GridViewHd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewHd.RowCommand
        Dim GVR As GridViewRow = Nothing
        Dim jobNo As String
        Dim Result As String
        Dim index As Integer
        Dim DDL As DropDownList

        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridViewHd.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridViewHd.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "Detail" Then
                    'GVR = GridViewHd.Rows(CInt(e.CommandArgument))


                    If Left(btnpost.Text, 2) <> "Un" Then
                        TbremarkApprovDt.Visible = True
                        lbremarkApprovDt.Visible = True
                    End If


                    jobNo = GVR.Cells(2).Text
                    ViewState("jobNo") = GVR.Cells(2).Text
                    ViewState("Warehouse") = GVR.Cells(16).Text
                    ViewState("Asisten") = GVR.Cells(17).Text
                    ViewState("Manager") = GVR.Cells(18).Text

                    'S.UserId = '''+SecurityRec.UserId+''' And StatusWrhs IN (''Y'',''N'')
                    BindDataDt(jobNo)
                    BindDataDtMI(jobNo)
                    BindDataDtSPE(jobNo)

                    lblWono.Text = GVR.Cells(2).Text
                    lblWODate.Text = GVR.Cells(3).Text
                    lblWorkBy.Text = GVR.Cells(15).Text
                    lblJobPlant.Text = GVR.Cells(5).Text
                    lblReff.Text = GVR.Cells(4).Text
                    lblQty.Text = GVR.Cells(12).Text
                    lblUnit.Text = GVR.Cells(13).Text
                    '  lblStatus.Text = GVR.Cells(5).Text
                    lblStatusW.Text = GVR.Cells(16).Text
                    lblStatusA.Text = GVR.Cells(17).Text
                    lblStatusN.Text = GVR.Cells(18).Text
                    Statuslevel(GVR.Cells(16).Text, GVR.Cells(17).Text, GVR.Cells(18).Text)
                    PnlHd.Visible = False
                    pnlDetail.Visible = True
                    GridDt.Columns(0).Visible = False

                    If lblstatuslevel.Text = "Warehouse" Then

                        pnlwrhs1.Visible = True
                        GridDt.Columns(7).Visible = True
                        GridDt.Columns(8).Visible = True
                        GridDt.Columns(9).Visible = True

                    Else
                        pnlwrhs1.Visible = False
                        GridDt.Columns(7).Visible = False
                        GridDt.Columns(8).Visible = False
                        GridDt.Columns(9).Visible = False

                    End If


                    'If lblStatusW.Text = "X" Then
                    '    pnlwrhs1.Visible = False
                    '    GridDt.Columns(7).Visible = False
                    '    GridDt.Columns(8).Visible = False
                    'Else
                    '    pnlwrhs1.Visible = True
                    '    GridDt.Columns(7).Visible = True
                    '    GridDt.Columns(8).Visible = True
                    'End If


                ElseIf DDL.SelectedValue = "Cancel" Then
                    If CheckMenuLevel("Cancel") = False Then
                        Exit Sub
                    End If

                    'GVR = GridViewHd.Rows(CInt(e.CommandArgument))

                    jobNo = GVR.Cells(2).Text
                    Result = ExecChangeActive(jobNo)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(lbStatus.Text + Result)
                    End If

                    GridViewHd.PageSize = ddlShowRecord.SelectedValue
                    BindData()
                ElseIf DDL.SelectedValue = "UnPost BA" Then
                    If CheckMenuLevel("UnPostBA") = False Then
                        Exit Sub
                    End If

                    jobNo = GVR.Cells(2).Text

                    Result = ExecUnPostBA(jobNo)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(lbStatus.Text + Result)
                    End If

                    GridViewHd.PageSize = ddlShowRecord.SelectedValue
                    BindData()
                ElseIf DDL.SelectedValue = "UnPost SPE" Then
                    If CheckMenuLevel("UnPostSPE") = False Then
                        Exit Sub
                    End If

                    jobNo = GVR.Cells(2).Text

                    Result = ExecUnPostSPE(jobNo)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(lbStatus.Text + Result)
                    End If

                    GridViewHd.PageSize = ddlShowRecord.SelectedValue
                    BindData()
                ElseIf DDL.SelectedValue = "UnPost Issue" Then
                    If CheckMenuLevel("UnPostIssue") = False Then
                        Exit Sub
                    End If

                    jobNo = GVR.Cells(2).Text

                    Result = ExecUnPostIssue(jobNo)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(lbStatus.Text + Result)
                    End If

                    GridViewHd.PageSize = ddlShowRecord.SelectedValue
                    BindData()
                End If
            End If


            'If e.CommandName = "Detail" Then
            '    GVR = GridViewHd.Rows(CInt(e.CommandArgument))

            '    jobNo = GVR.Cells(2).Text
            '    BindDataDt(jobNo)

            '    PnlHd.Visible = False
            '    pnlDetail.Visible = True
            'ElseIf e.CommandName = "Cancel" Then
            '    If CheckMenuLevel("Cancel") = False Then
            '        Exit Sub
            '    End If

            '    GVR = GridViewHd.Rows(CInt(e.CommandArgument))

            '    jobNo = GVR.Cells(2).Text

            '    Result = ExecChangeActive(jobNo)
            '    If Trim(Result) <> "" Then
            '        lbStatus.Text = MessageDlg(lbStatus.Text + Result)
            '    End If

            '    GridViewHd.PageSize = ddlShowRecord.SelectedValue
            '    BindData()
            'End If

            'If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
            '    index = Convert.ToInt32(e.CommandArgument)
            '    GVR = GridViewHd.Rows(index)
            'End If

            'If e.CommandName = "Go" Then
            '    DDL = GridViewHd.Rows(index).FindControl("ddl")
            '    If DDL.SelectedValue = "Detail" Then
            '        GVR = GridViewHd.Rows(CInt(e.CommandArgument))

            '        jobNo = GVR.Cells(2).Text
            '        BindDataDt(jobNo)

            '        PnlHd.Visible = False
            '        pnlDetail.Visible = True
            '    ElseIf DDL.SelectedValue = "Cancel" Then
            '        GVR = GridViewHd.Rows(CInt(e.CommandArgument))

            '        jobNo = GVR.Cells(2).Text

            '        Result = ExecChangeActive(jobNo)
            '        If Trim(Result) <> "" Then
            '            lbStatus.Text = lbStatus.Text + Result + " <br/>"
            '        End If

            '        BindData()
            '    End If
            'End If
        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewHd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridViewHd.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.ServerClick
        PnlHd.Visible = True
        pnlDetail.Visible = False
        BindData()
    End Sub

    Protected Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnpost.Click
        Dim GVR As GridViewRow
        Dim HaveCheck As Boolean
        Dim CB As CheckBox
        Dim JobNo, Nmbr, WOlevel, team, userapp, Remark As String
        Dim Result As String

        Try


            If lblstatuslevel.Text = "Warehouse" Then
                If ddlwrhs.SelectedIndex = 0 Then
                    lbStatus.Text = MessageDlg("Wrhs Src cannot empty")
                    Exit Sub
                End If
                If ddlwrhsdest2.SelectedIndex = 0 Then
                    lbStatus.Text = MessageDlg("Wrhs Dest cannot empty")
                    Exit Sub
                End If
            End If

            'For Each GVR In GridDt.Rows

            WOlevel = ViewState("Level")
            userapp = ViewState("UserId")
            Remark = ""
            AttachScript("closing();", Page, Me.GetType)
            Nmbr = lblWono.Text
            If Left(btnpost.Text, 2) = "Un" Then
                Result = ExecUnPostlevel(Nmbr, WOlevel)

                If lblstatuslevel.Text = "Warehouse" Then
                    btnpost.Text = "Gudang Approval"
                    lblStatusW.Text = "N"
                ElseIf lblstatuslevel.Text = "Asisten" Then
                    btnpost.Text = "Asisten"
                    lblStatusA.Text = "N"
                ElseIf lblstatuslevel.Text = "Manager" Then
                    btnpost.Text = "Manager"
                    lblStatusN.Text = "N"
                End If

                lbremarkApprovDt.Visible = True
                TbremarkApprovDt.Visible = True
                'TbremarkApprovDt.Text = ""
            Else
                Result = ExecPostlevel(Nmbr, WOlevel, userapp, TbremarkApprovDt.Text, ddlwrhs.SelectedValue, ddlwrhsdest2.SelectedValue)

                If lblstatuslevel.Text = "Warehouse" Then
                    btnpost.Text = "Un Gudang Approval"
                    lblStatusW.Text = "Y"
                ElseIf lblstatuslevel.Text = "Asisten" Then
                    btnpost.Text = "Un Asisten"
                    lblStatusA.Text = "Y"
                ElseIf lblstatuslevel.Text = "Manager" Then
                    btnpost.Text = "Un Manager"
                    lblStatusN.Text = "Y"
                End If


                lbremarkApprovDt.Visible = False
                TbremarkApprovDt.Visible = False
                TbremarkApprovDt.Text = ""
            End If
            If Trim(Result) <> "" Then
                lbStatus.Text = MessageDlg(lbStatus.Text + Result)
                btnpost.Text = "Gudang Approval"
                lblStatusW.Text = "N"
                lbremarkApprovDt.Visible = True
                TbremarkApprovDt.Visible = True
                TbremarkApprovDt.Text = ""
                Exit Sub
            End If

           

            'Next
            GridViewHd.PageSize = ddlShowRecord.SelectedValue
            'BindData()
            BindDataDt(Nmbr)
            BindDataDtMI(Nmbr)
            BindDataDtSPE(Nmbr)

        Catch ex As Exception
            lbStatus.Text = "btnPost_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnWOLevel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOLevel.Click
        Dim GVR As GridViewRow
        Dim HaveCheck As Boolean
        Dim CB As CheckBox
        Dim JobNo, Nmbr, WOlevel, team, userapp, Remark As String
        Dim Result As String

        Try

            'AttachScript("closing();", Page, Me.GetType)
            'If HiddenRemarkClose.Value <> "False Value" Then
            '    tbFilter.Text = HiddenRemarkClose.Value
            'Else
            '    lbStatus.Text = "Zonk"
            '    Exit Sub
            'End If
            'Exit Sub

            'lbStatus.Text = MessageDlg(StrComp(btnWOLevel.Text, 1, 2))
            'Exit Sub

            If lblstatuslevel.Text = "Warehouse" Then
                If ddlwrhsSrc.SelectedIndex = 0 Then
                    lbStatus.Text = MessageDlg("Wrhs Src cannot empty")
                    Exit Sub
                End If
                If ddlWrhsDest.SelectedIndex = 0 Then
                    lbStatus.Text = MessageDlg("Wrhs Dest cannot empty")
                    Exit Sub
                End If
            End If

            HaveCheck = False
            For Each GVR In GridViewHd.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked = True Then
                    HaveCheck = True
                End If
            Next

            If HaveCheck = True Then
                For Each GVR In GridViewHd.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked = True Then
                        JobNo = GVR.Cells(2).Text
                        WOlevel = ViewState("Level")
                        team = GVR.Cells(2).Text
                        userapp = ViewState("UserId")
                        Remark = TbremarkApprov.Text
                        Nmbr = GVR.Cells(2).Text

                        If Left(btnWOLevel.Text, 2) = "Un" Then
                            Result = ExecUnPostlevel(Nmbr, WOlevel)
                        Else
                            Result = ExecPostlevel(Nmbr, WOlevel, userapp, Remark, ddlwrhsSrc.SelectedValue, ddlWrhsDest.SelectedValue)
                        End If

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                            Exit Sub
                        End If
                        lbStatus.Text = "sukses"
                        TbremarkApprov.Text = ""
                        TbremarkApprov.Visible = False
                        lbremarkApprov.Visible = False
                        ''USERLOG
                        'SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + ddlwrhsSrc.SelectedValue + "," + _
                        'QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("ChangeTeam") + ",'Test'", ViewState("DBConnection").ToString)
                    End If
                Next
            Else
                lbStatus.Text = MessageDlg("Data must have at least 1 selected")
                Exit Sub
            End If

            GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindData()
        Catch ex As Exception
            lbStatus.Text = "btnWOLevel_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnapply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnapply.Click
    '    Dim GVR As GridViewRow
    '    Dim HaveCheck As Boolean
    '    Dim CB As CheckBox
    '    Dim JobNo As String
    '    Dim Result As String

    '    Try
    '        HaveCheck = False
    '        For Each GVR In GridViewHd.Rows
    '            CB = GVR.FindControl("cbSelect")
    '            If CB.Checked = True Then
    '                HaveCheck = True
    '            End If
    '        Next

    '        If HaveCheck = True Then
    '            For Each GVR In GridViewHd.Rows
    '                CB = GVR.FindControl("cbSelect")
    '                If CB.Checked = True Then
    '                    JobNo = GVR.Cells(2).Text

    '                    'Result = ExecChangeDate(JobNo, tbChangeDate.SelectedValue)
    '                    'If Trim(Result) <> "" Then
    '                    '    lbStatus.Text = lbStatus.Text + Result + " <br/>"
    '                    '    Exit Sub
    '                    'End If

    '                    'USERLOG
    '                    'SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo + "," + tbChangeDate.SelectedValue.ToString) + "," + _
    '                    '                   QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("ChangeTeam") + ",''", ViewState("DBConnection").ToString)
    '                End If
    '            Next
    '        Else
    '            lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '            Exit Sub
    '        End If

    '        GridViewHd.PageSize = ddlShowRecord.SelectedValue
    '        BindData()
    '    Catch ex As Exception
    '        lbStatus.Text = "btnapply_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDt.Sorting
        Try
            If ViewState("SortOrderDt") = Nothing Or ViewState("SortOrderDt") = "DESC" Then
                ViewState("SortOrderDt") = "ASC"
            Else
                ViewState("SortOrderDt") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrderDt")
            'GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindDataDt(ViewState("jobNo"))
        Catch ex As Exception
            lbStatus.Text = ("GridViewDt_Sorting Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDtMI_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDtMI.PageIndexChanging
        Try
            GridDtMI.PageIndex = e.NewPageIndex
            GridDtMI.DataSource = ViewState("DtMI")
            GridDtMI.DataBind()
        Catch ex As Exception
            lbStatus.Text = "GridDtMI_PageIndexChanging Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDtMI_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDtMI.Sorting
        Try
            If ViewState("SortOrderDtMI") = Nothing Or ViewState("SortOrderDtMI") = "DESC" Then
                ViewState("SortOrderDtMI") = "ASC"
            Else
                ViewState("SortOrderDtMI") = "DESC"
            End If
            ViewState("SortExpressionDtMI") = e.SortExpression + " " + ViewState("SortOrderDtMI")
            'GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindDataDtMI(ViewState("jobNo"))
        Catch ex As Exception
            lbStatus.Text = ("GridDtMI_Sorting Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDtSPE_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDtSPE.PageIndexChanging
        Try
            GridDtSPE.PageIndex = e.NewPageIndex
            GridDtSPE.DataSource = ViewState("DtSPE")
            GridDtSPE.DataBind()
        Catch ex As Exception
            lbStatus.Text = "GridDtSPE_PageIndexChanging Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDtSPE_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDtSPE.Sorting
        Try
            If ViewState("SortOrderDtSPE") = Nothing Or ViewState("SortOrderDtSPE") = "DESC" Then
                ViewState("SortOrderDtSPE") = "ASC"
            Else
                ViewState("SortOrderDtSPE") = "DESC"
            End If
            ViewState("SortExpressionDtSPE") = e.SortExpression + " " + ViewState("SortOrderDtSPE")
            'GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindDataDtSPE(ViewState("jobNo"))
        Catch ex As Exception
            lbStatus.Text = ("GridDtSPE_Sorting Error : " + ex.ToString)
        End Try
    End Sub

    Private Function ExecPostlevel(ByVal Nmbr As String, ByVal WOlevel As String, ByVal Userappr As String, ByVal Remark As String, ByVal WrhsSrc As String, ByVal WrhsDest As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                                "EXEC S_PLWOStatusPost " + QuotedStr(Nmbr) + ", " + QuotedStr(WOlevel) + ", " + QuotedStr(ViewState("ServerDate")) + ", " + QuotedStr(Userappr) + "," + QuotedStr(WrhsSrc) + "," + QuotedStr(WrhsDest) + "," + QuotedStr(Remark) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                                "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecChangeTeam Error : " + ex.ToString)
        End Try
    End Function

    Private Function ExecUnPostlevel(ByVal Nmbr As String, ByVal WOlevel As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                                "EXEC S_PLWOStatusUnPost " + QuotedStr(Nmbr) + ", " + QuotedStr(WOlevel) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                                "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function
            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecChangeTeam Error : " + ex.ToString)
        End Try
    End Function

    Private Function ExecChangeDate(ByVal jobNo As String, ByVal tgl As Date) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_OPRJobReportListChangeDate " + QuotedStr(jobNo) + ", " + QuotedStr(Format(tgl, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecChangeDate Error : " + ex.ToString)
        End Try
    End Function

    Private Function ExecChangeActive(ByVal jobNo As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_OPRJobReportListNonActive " + QuotedStr(jobNo) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecChangeActive Error : " + ex.ToString)
        End Try
    End Function

    Private Function ExecUnPostBA(ByVal jobNo As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_OPRJobReportListUnPostBA " + QuotedStr(jobNo) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecUnPostBA Error : " + ex.ToString)
        End Try
    End Function

    Private Function ExecUnPostSPE(ByVal jobNo As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_OPRJobReportListUnPostSPE " + QuotedStr(jobNo) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecUnPostSPE Error : " + ex.ToString)
        End Try
    End Function

    Public Function ExecUnPostIssue(ByVal jobNo As String) As String
        Try
            Dim sqlstring, Result As String
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_OPRJobReportListUnPostIssue " + QuotedStr(jobNo) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                            "SELECT @A "

            'lbStatus.Text = sqlstring
            'Exit Function

            Result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Result.Length > 1 Then
                Return Result
            Else
                Return Result.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("ExecUnPostIssue Error : " + ex.ToString)
        End Try
    End Function

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        'Dim DGI As GridViewRow
    '        'Dim Cb As CheckBox

    '        Dim HaveSelect As Boolean
    '        Dim GVR As GridViewRow
    '        Dim CB As CheckBox
    '        Dim HaveCheck As Boolean

    '        Dim team As Object
    '        Dim SqlString, j As String
    '        Dim JobNo As String = ""
    '        Dim schedule As Date
    '        Dim assign As Date

    '        'Dim Result As String
    '        Dim Fields() As String
    '        Dim building, ref, Type As String

    '        Dim DoneExec As Boolean


    '        'Dim list As Collections.Generic.List(Of Object) = GridViewHd.get

    '        'If list.Count = 0 Then
    '        '    lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '        '    GridViewHd.Focus()
    '        '    Exit Sub
    '        'End If

    '        'For i As Integer = 0 To list.Count - 1
    '        '    'currID = list.Item(0) 'Index 0 means first fields passed to the function GetSelectedFieldValues
    '        '    'Now you can do what you want to do to the current returned ID

    '        '    Fields = list.Item(i).ToString.Split("|")
    '        '    building = Fields(1)
    '        '    ref = Fields(8)
    '        '    schedule = Fields(12)
    '        '    Type = Fields(11)

    '        '    If Fields(13).ToString <> "~Xtra#Base64AAEAAAD/////AQAAAAAAAAAEAQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEICgIAAAAGAgAAAAAL" Then 'cell kosong
    '        '        assign = Fields(13)
    '        '        SqlString = "EXEC S_OPJobSchedulePrintUpdate " + QuotedStr(building) + "," + QuotedStr(ref) + "," + QuotedStr(Type) + "," + QuotedStr(Format(schedule, "yyyy-MM-dd")) + ", " + QuotedStr(Format(assign, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("UserId"))

    '        '        'lbStatus.Text = SqlString
    '        '        'Exit Sub

    '        '        SQLExecuteNonQuery(SqlString, ViewState("DBConnection"))
    '        '    End If

    '        '    DoneExec = True
    '        'Next

    '        'If ViewState("JobTypeCode") = "01" Then         'Install

    '        'sche = DataGrid.GetRowValues(DataGrid.FocusedRowIndex, "ScheduleDate")

    '        HaveSelect = False
    '        HaveCheck = False
    '        For Each GVR In GridViewHd.Rows
    '            CB = GVR.FindControl("cbSelect")
    '            If CB.Checked = True Then
    '                HaveCheck = True
    '            End If
    '        Next

    '        If HaveCheck = True Then
    '            For Each GVR In GridViewHd.Rows
    '                CB = GVR.FindControl("cbSelect")
    '                If CB.Checked = True Then
    '                    JobNo += QuotedStr(GVR.Cells(2).Text)
    '                End If
    '            Next
    '        Else
    '            lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '            Exit Sub
    '        End If

    '        JobNo = JobNo.Replace("''", "','")
    '        'lbStatus.Text = "EXEC S_OPRJobReportListingFormTransferMaterial " + QuotedStr(JobNo)
    '        'Exit Sub

    '        If ddlTipe.SelectedIndex = 0 Then
    '            Session("DBConnection") = ViewState("DBConnection")
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormService " + QuotedStr(JobNo)
    '            Session("ReportFile") = ".../../../Rpt/FormJobScheduleServiceV2.frx"
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 1 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormInstallUnit " + QuotedStr(JobNo)
    '            Session("ReportFile") = ".../../../Rpt/FormJobScheduleInstallV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 2 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormRemoveUnit " + QuotedStr(JobNo)
    '            Session("ReportFile") = ".../../../Rpt/FormJobScheduleRemoveV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 3 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormJobReportBA " + QuotedStr(JobNo) + ",'BAPL'"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintBAPV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 4 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormJobReportBA " + QuotedStr(JobNo) + ",'Service'"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintBASV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 5 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormTransferMaterial " + QuotedStr(JobNo)
    '            Session("ReportFile") = ".../../../Rpt/RptPrintTransferMaterialV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 6 Then
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormSPE " + QuotedStr(JobNo)
    '            Session("ReportFile") = ".../../../Rpt/RptPrintSPEV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)
    '        End If

    '        'AttachScript("openprintdlg();", Page, Me.GetType)
    '        'If DoneExec Then
    '        '    BindDataGrid(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '        '    'BindDataDt(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '        'End If

    '        'GridViewHd.Selection.UnselectAll()
    '        'End If
    '        'BindDataDt(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '    Catch ex As Exception
    '        lbStatus.Text = "btnPrint_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnPrint_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.ServerClick
    '    Try
    '        'Dim DGI As GridViewRow
    '        'Dim Cb As CheckBox

    '        Dim HaveSelect As Boolean
    '        Dim GVR As GridViewRow
    '        Dim CB As CheckBox
    '        Dim Active As String
    '        Dim HaveCheck As Boolean

    '        'Dim team As Object
    '        'Dim SqlString, j As String
    '        Dim JobNo As String = ""
    '        Dim team2 As Label
    '        Dim tgl As String = ""
    '        Dim team As String = ""
    '        Dim type As String = ""

    '        'Dim schedule As Date
    '        'Dim assign As Date

    '        'Dim Result As String
    '        'Dim Fields() As String
    '        'Dim building, ref, Type As String

    '        'Dim DoneExec As Boolean


    '        'Dim list As Collections.Generic.List(Of Object) = GridViewHd.get

    '        'If list.Count = 0 Then
    '        '    lbStatus.Text = MessageDlg("Data must have at least 1 selected")
    '        '    GridViewHd.Focus()
    '        '    Exit Sub
    '        'End If

    '        'For i As Integer = 0 To list.Count - 1
    '        '    'currID = list.Item(0) 'Index 0 means first fields passed to the function GetSelectedFieldValues
    '        '    'Now you can do what you want to do to the current returned ID

    '        '    Fields = list.Item(i).ToString.Split("|")
    '        '    building = Fields(1)
    '        '    ref = Fields(8)
    '        '    schedule = Fields(12)
    '        '    Type = Fields(11)

    '        '    If Fields(13).ToString <> "~Xtra#Base64AAEAAAD/////AQAAAAAAAAAEAQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEICgIAAAAGAgAAAAAL" Then 'cell kosong
    '        '        assign = Fields(13)
    '        '        SqlString = "EXEC S_OPJobSchedulePrintUpdate " + QuotedStr(building) + "," + QuotedStr(ref) + "," + QuotedStr(Type) + "," + QuotedStr(Format(schedule, "yyyy-MM-dd")) + ", " + QuotedStr(Format(assign, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("UserId"))

    '        '        'lbStatus.Text = SqlString
    '        '        'Exit Sub

    '        '        SQLExecuteNonQuery(SqlString, ViewState("DBConnection"))
    '        '    End If

    '        '    DoneExec = True
    '        'Next

    '        'If ViewState("JobTypeCode") = "01" Then         'Install

    '        'sche = DataGrid.GetRowValues(DataGrid.FocusedRowIndex, "ScheduleDate")

    '        HaveSelect = False
    '        HaveCheck = False
    '        For Each GVR In GridViewHd.Rows
    '            CB = GVR.FindControl("cbSelect")
    '            Active = GVR.Cells(9).Text
    '            If CB.Checked = True And Active = "Y" Then
    '                HaveCheck = True
    '            End If
    '        Next

    '        If HaveCheck = True Then
    '            For Each GVR In GridViewHd.Rows
    '                CB = GVR.FindControl("cbSelect")
    '                team2 = GVR.FindControl("team")
    '                Active = GVR.Cells(9).Text

    '                If (CB.Checked = True) And (Active = "Y") Then
    '                    JobNo += QuotedStr(GVR.Cells(2).Text)
    '                    team += QuotedStr(team2.Text)
    '                    type = GVR.Cells(5).Text
    '                    If ddlTipe.SelectedIndex = 8 Then '5
    '                        If GVR.Cells(13).Text <> "&nbsp;" Then
    '                            tgl += QuotedStr(Format(CDate(GVR.Cells(13).Text), "yyyy-MM-dd"))
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        Else
    '            lbStatus.Text = MessageDlg("Data must have at least 1 selected And Active ='Y'")
    '            Exit Sub
    '        End If

    '        JobNo = JobNo.Replace("''", "','")
    '        team = team.Replace("''", "','")
    '        tgl = tgl.Replace("''", "','")

    '        'lbStatus.Text = "EXEC S_OPRJobReportListingFormTransferMaterial " + QuotedStr(JobNo)
    '        'Exit Sub

    '        'If ddlTipe.SelectedIndex = 0 Then
    '        '    Session("PrintType") = "Print"
    '        '    Session("DBConnection") = ViewState("DBConnection")
    '        '    Session("SelectCommand") = "EXEC S_OPRJobReportListingFormService " + QuotedStr(JobNo)
    '        '    Session("ReportFile") = ".../../../Rpt/FormJobScheduleServiceV2.frx"
    '        '    AttachScript("openprintdlg();", Page, Me.GetType)
    '        If ddlTipe.SelectedIndex = 0 Or ddlTipe.SelectedIndex = 1 Then 'CSR
    '            'Session("SelectCommand") = "EXEC S_OPRFormService " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(Type) + "," + QuotedStr(Format(CDate(sche), "yyyy-MM-dd"))
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRFormService " + QuotedStr(JobNo) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(ViewState("UserName"))
    '            If ddlTipe.SelectedIndex = 1 Then
    '                Session("ReportFile") = ".../../../Rpt/FormJobScheduleServiceTCalmic.frx"
    '            Else
    '                Session("ReportFile") = ".../../../Rpt/FormJobScheduleServiceT.frx"
    '            End If

    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintCSR") + ",''", ViewState("DBConnection").ToString)
    '        ElseIf ddlTipe.SelectedIndex = 2 Or ddlTipe.SelectedIndex = 3 Then 'Install Report '1
    '            If type = "Install" Then
    '                Session("PrintType") = "Print"
    '                Session("SelectCommand") = "EXEC S_OPRJobReportListingFormInstallUnit " + QuotedStr(JobNo)
    '                If ddlTipe.SelectedIndex = 3 Then
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleInstallV2Calmic.frx"
    '                Else
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleInstallV2.frx"
    '                End If

    '                Session("DBConnection") = ViewState("DBConnection")
    '                AttachScript("openprintdlg();", Page, Me.GetType)

    '                'USERLOG
    '                SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                                   QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintInstall") + ",''", ViewState("DBConnection").ToString)
    '            ElseIf type = "Install Free" Then
    '                Session("PrintType") = "Print"
    '                Session("SelectCommand") = "EXEC S_OPRJobReportListingFormFreeInstallUnit " + QuotedStr(JobNo)
    '                If ddlTipe.SelectedIndex = 3 Then
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleFreeInstallV2Calmic.frx"
    '                Else
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleFreeInstallV2.frx"
    '                End If
    '                Session("DBConnection") = ViewState("DBConnection")
    '                AttachScript("openprintdlg();", Page, Me.GetType)

    '                'USERLOG
    '                SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                                   QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintInstallFree") + ",''", ViewState("DBConnection").ToString)
    '            End If
    '        ElseIf ddlTipe.SelectedIndex = 4 Or ddlTipe.SelectedIndex = 5 Then 'Remove Report '2
    '            If type = "Remove" Then
    '                Session("PrintType") = "Print"
    '                Session("SelectCommand") = "EXEC S_OPRJobReportListingFormRemoveUnit " + QuotedStr(JobNo)
    '                If ddlTipe.SelectedIndex = 5 Then
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleRemoveV2Calmic.frx"
    '                Else
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleRemoveV2.frx"
    '                End If

    '                Session("DBConnection") = ViewState("DBConnection")
    '                AttachScript("openprintdlg();", Page, Me.GetType)

    '                'USERLOG
    '                SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                                   QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintRemove") + ",''", ViewState("DBConnection").ToString)
    '            ElseIf type = "Remove Free" Then
    '                Session("PrintType") = "Print"
    '                Session("SelectCommand") = "EXEC S_OPRJobReportListingFormFreeRemoveUnit " + QuotedStr(JobNo)
    '                If ddlTipe.SelectedIndex = 5 Then
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleFreeRemoveV2Calmic.frx"
    '                Else
    '                    Session("ReportFile") = ".../../../Rpt/FormJobScheduleFreeRemoveV2.frx"
    '                End If

    '                Session("DBConnection") = ViewState("DBConnection")
    '                AttachScript("openprintdlg();", Page, Me.GetType)

    '                'USERLOG
    '                SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                                   QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintRemoveFree") + ",''", ViewState("DBConnection").ToString)
    '            End If
    '        ElseIf ddlTipe.SelectedIndex = 6 Then 'BAP '3
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormJobReportBA " + QuotedStr(JobNo) + ",'Install'"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintBAPV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintBAP") + ",''", ViewState("DBConnection").ToString)
    '        ElseIf ddlTipe.SelectedIndex = 7 Then 'BAS '4
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormJobReportBA " + QuotedStr(JobNo) + ",'Service'"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintBASV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintBAS") + ",''", ViewState("DBConnection").ToString)
    '        ElseIf ddlTipe.SelectedIndex = 8 Then 'Tanda Terima '5
    '            Session("PrintType") = "Print"

    '            If tgl = "" Then
    '                lbStatus.Text = MessageDlg("No Data!")
    '                Exit Sub
    '            End If

    '            Session("SelectCommand1") = "EXEC S_OPRFormTransferMaterialForListing " + QuotedStr(JobNo) + "," + QuotedStr(team) + "," + QuotedStr(tgl) + "," + QuotedStr(ViewState("UserId"))
    '            Session("SelectCommand2") = "EXEC S_OPRFormTransferMaterialSumForListing " + QuotedStr(JobNo) + "," + QuotedStr(team) + "," + QuotedStr(tgl) + "," + QuotedStr(ViewState("UserId"))

    '            Session("ReportFile") = ".../../../Rpt/RptPrintTransferMaterial2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg2ds();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintTandaTerima") + ",''", ViewState("DBConnection").ToString)

    '            'Session("SelectCommand") = "EXEC S_OPRJobReportListingFormTransferMaterial " + QuotedStr(JobNo)
    '            'Session("ReportFile") = ".../../../Rpt/RptPrintTransferMaterialV2.frx"
    '            'AttachScript("openprintdlg();", Page, Me.GetType)
    '        ElseIf ddlTipe.SelectedIndex = 9 Then 'SPE '6
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingFormSPE " + QuotedStr(JobNo) + "," + QuotedStr(ViewState("UserId"))
    '            'Session("SelectCommand") = "Exec S_OPRFormSPE " + QuotedStr(team) + "," + QuotedStr(Format(tbDateTeam.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(Format(tbDateTeam2.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(ViewState("UserId"))

    '            'Session("ReportFile") = ".../../../Rpt/RptPrintSPEV2.frx"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintSPE2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintSPE") + ",''", ViewState("DBConnection").ToString)
    '        ElseIf ddlTipe.SelectedIndex = 10 Then 'DPF '7
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingSuspClose " + QuotedStr(JobNo) + ",'''DPF'''"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintDPFV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintDPF") + ",''", ViewState("DBConnection").ToString)
    '        ElseIf ddlTipe.SelectedIndex = 11 Then 'Suspend '10
    '            Session("PrintType") = "Print"
    '            Session("SelectCommand") = "EXEC S_OPRJobReportListingSuspClose " + QuotedStr(JobNo) + ",'''Suspend'''"
    '            Session("ReportFile") = ".../../../Rpt/RptPrintSuspendV2.frx"
    '            Session("DBConnection") = ViewState("DBConnection")
    '            AttachScript("openprintdlg();", Page, Me.GetType)

    '            'USERLOG
    '            SQLExecuteNonQuery("EXEC S_SAUserLog " + QuotedStr(Request.QueryString("ContainerId").ToString) + ",''," + QuotedStr(JobNo) + "," + _
    '                               QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr("PrintSuspend") + ",''", ViewState("DBConnection").ToString)
    '        End If

    '        'AttachScript("openprintdlg();", Page, Me.GetType)
    '        'If DoneExec Then
    '        '    BindDataGrid(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), Format(tbEndDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '        '    'BindDataDt(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '        'End If

    '        'GridViewHd.Selection.UnselectAll()
    '        'End If
    '        'BindDataDt(Format(tbStartDate.SelectedValue, "yyyy-MM-dd"), ddlArea.SelectedValue)
    '    Catch ex As Exception
    '        lbStatus.Text = "btnPrint_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        Try
            GridViewHd.PageSize = ddlShowRecord.SelectedValue
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Show Record Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Statuslevel(ByVal Wrhs As String, ByVal Asisten As String, ByVal Manager As String)
        If lblstatuslevel.Text = "Warehouse" Then
            If Wrhs = "Y" Then
                btnpost.Text = "Un Gudang Approval"
            Else : btnpost.Text = "Gudang Approval"
            End If
        ElseIf lblstatuslevel.Text = "Asisten" Then

            If Asisten = "Y" Then
                btnpost.Text = "Un Asisten"
            Else : btnpost.Text = "Asisten"
            End If

        ElseIf lblstatuslevel.Text = "Manager" Then
            If Manager = "Y" Then
                btnpost.Text = "Un Manager"
            Else : btnpost.Text = "Manager"
            End If
        End If

        If lblstatuslevel.Text = "Warehouse" Then
            If Wrhs = "Y" Then
                btnWOLevel.Text = "Un Gudang Approval"
            Else : btnWOLevel.Text = "Gudang Approval"
            End If
        ElseIf lblstatuslevel.Text = "Asisten" Then
            If Asisten = "Y" Then
                btnWOLevel.Text = "Un Asisten"
            Else : btnWOLevel.Text = "Asisten"
            End If
        ElseIf lblstatuslevel.Text = "Manager" Then
            If Manager = "Y" Then
                btnWOLevel.Text = "Un Manager"
            Else : btnWOLevel.Text = "Manager"
            End If
        End If
    End Sub

End Class
