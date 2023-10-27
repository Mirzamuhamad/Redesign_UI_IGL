Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrPlanland_TrPlanland
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, TransDate, Status, TransName, MasterPlanNo, Divisi, StartDate, Areal, QtyTarget, QtyBibitPN, TargetType, TargetRange, " + _
                                    " LevelPlant, Regenerate, DivisiName, Area UserPrep, DatePrep, UserAppr, DateAppr, Remark FROM V_PLPlanLandHd Where LevelPlant ='01'"
    Protected GetStringHdMN As String = "SELECT DISTINCT TransNmbr, TransDate, Status, TransName, MasterPlanNo, Divisi, StartDate, Areal, QtyTarget, QtyBibitPN, TargetType, TargetRange, " + _
                                    " LevelPlant, Regenerate, DivisiName, Area UserPrep, DatePrep, UserAppr, DateAppr, Remark FROM V_PLPlanLandHd Where LevelPlant ='02'"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbQtyTarget.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyBibitPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbArealB.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbqtyTargetB.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbQtyT.Attributes.Add("Onkeydown", "return PressNumeric();")
                tbperson.Attributes.Add("OnKeyDown", "return PressNumeric();")


                Session("AdvanceFilter") = ""
                If Request.QueryString("ContainerId").ToString = "TrPlanLandID" Then
                    ViewState("Param") = "PN"
                    lblJml.Text = "Jumlah Bibt di PN (pokok)"
                    ViewState("StrType") = "PLP"
                    ViewState("LevelPlant") = "01"
                    lbltitle.Text = "Pengembangan - Lahan Planning"
                Else
                    ViewState("Param") = "MN"
                    lblJml.Text = "Jumlah Bibt di MN (pokok)"
                    ViewState("StrType") = "PLTB"
                    ViewState("LevelPlant") = "02"
                    lbltitle.Text = "Pengembangan - Blok Tanam Planning"
                End If



            End If
            
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnMaterial" Then
                    BindToText(tbMaterial, Session("Result")(0).ToString)
                    BindToText(tbMaterialName, Session("Result")(1).ToString)
                    'BindToText(tbQtyM, Session("Result")(2).ToString)
                    BindToText(tbUnit, Session("Result")(2).ToString)

                End If
                If ViewState("Sender") = "btnBlock" Then
                    BindToText(tbBlock, Session("Result")(0).ToString)
                    BindToText(tbBlockName, Session("Result")(1).ToString)
                    BindToText(tbArealB, Session("Result")(2).ToString)
                    BindToText(tbqtyTargetB, Session("Result")(3).ToString)

                    'tbArealB.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitQty"))
                    'tbqtyTargetB.Text = Session("Result")(3).ToString
                End If
                If ViewState("Sender") = "btnMasterPlan" Then
                    BindToText(tbMasterPlan, Session("Result")(0).ToString)
                    'BindToText(tbBlockName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnTeam" Then
                    BindToText(tbTeam, Session("Result")(0).ToString)
                    BindToText(tbTeamName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnJobPlant" Then
                    BindToText(tbJobPlant, Session("Result")(0).ToString)
                    BindToText(tbJobPlantName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnEquipment" Then
                    BindToText(tbEquip, Session("Result")(0).ToString)
                    BindToText(tbEquipName, Session("Result")(1).ToString)
                    BindToText(tbQty, Session("Result")(2).ToString)

                    lblUnit.Text = Session("Result")(3).ToString
                End If
                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing





            End If
            'dsUseRollNo.ConnectionString = ViewState("DBConnection")




            lbStatus.Text = ""
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
                GridDt2.Columns(0).Visible = GetCountRecord(ViewState("Dt2")) > 0

            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                GridDt3.Columns(0).Visible = GetCountRecord(ViewState("Dt3")) > 0
                GridDt3.Columns(1).Visible = True

                'BindDataDt3(ViewState("TransNmbr"))
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                PnlDt4.Visible = True
                pnlEditDt4.Visible = False
                GridDt4.Columns(0).Visible = GetCountRecord(ViewState("Dt4")) > 0
                'BindDataDt4(ViewState("TransNmbr"))
            Else
                pnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlDivisi, "EXEC S_GetPlantDivision", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
            ' FillCombo(ddlVarietas, "EXEC S_GetVarietas", True, "VarietasCode", "VarietasName", ViewState("DBConnection"))


            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            ' tbQtyMaterial.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyMaterial.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyTotal.Attributes.Add("ReadOnly", "True")
            'tbQtyTotal.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnBlur", "setformatdt()")



        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            If Request.QueryString("ContainerId").ToString = "TrPlanLandID" Then
                SQLString = GetStringHd
            Else
                SQLString = GetStringHdMN
            End If
            DT = BindDataTransaction(SQLString, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                'BtnGo.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLPlanLandBlock WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLPlanLandDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLPlanLandJob WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLPlanLandJobDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PLPlanLand", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")

        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlDivisi.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try
            'V_PLPlanLandPN
            'tbJobPlant.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
            ' tbMaterial.Enabled = State
            'diremark tbStartTime.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt4 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
            ' Drow = ViewState("Dt2").Select("Block=" + QuotedStr(GVR.cell().text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt2)
            ' GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            ' Else
            ' Dim DtTemp As DataTable
            ' DtTemp = ViewState("Dt2").Clone
            ' DtTemp.Rows.Add(DtTemp.NewRow())
            ' GridDt2.DataSource = DtTemp
            ' GridDt2.DataBind()
            'GridDt2.Columns(0).Visible = False
            ' End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
            ' Drow = ViewState("Dt3").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblMaterialE.Text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt3)
            ' GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "ViewE"
            'Else
            'Dim DtTemp As DataTable
            'DtTemp = ViewState("Dt3").Clone
            'DtTemp.Rows.Add(DtTemp.NewRow())
            'GridDt3.DataSource = DtTemp
            'GridDt3.DataBind()
            'GridDt3.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            BindGridDt(dt, GridDt4)
            'Drow = ViewState("Dt4").Select("BlockPlant=" + QuotedStr(LblTypeMT.Text))
            'If Drow.Length > 0 Then
            '    BindGridDt(Drow.CopyToDataTable, GridDt4)
            '    GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "ViewM"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt4").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt4.DataSource = DtTemp
            '    GridDt4.DataBind()
            '    GridDt4.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDt2(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            If IsEmpty = True Then
                gv.Columns(0).Visible = False
            Else
                gv.Columns(0).Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Dim Jml As Double

        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbMasterPlan.Text = ""
            tdpStartDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tdpEndDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlTargetBy.SelectedIndex = 0
            tbTransName.Text = ""
            tdpStart.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbAreal.Text = "0"
            lblRegenerate.Text = "Re Generate Schedule"
            tbTargetRange.Text = "0"
            Jml = SQLExecuteScalar("Select * From V_GetBatchPNMN Where Type = " + QuotedStr(ViewState("Param")), ViewState("DBConnection"))
            tbQtyBibitPN.Text = FormatFloat(Jml, ViewState("DigitQty"))
            tbQtyTarget.Text = "0"
            tbQtyBibitPN.Text ="0"
            tbRemarkHD.Text = ""
            Dim Division As String
            Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))
            ddlDivisi.SelectedValue = Division

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbBlock.Text = ""
            tbBlockName.Text = ""
            tbArealB.Text = "0"
            tbqtyTargetB.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbEquip.Text = ""
            tbEquipName.Text = ""
            tbQty.Text = "0"
            lblUnit.Text = ""
            tbremarkEquip.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbJobPlant.Text = ""
            tbJobPlantName.Text = ""
            tbTeam.Text = ""
            tbTeamName.Text = ""
            tbQtyT.Text = "0"
            tdpEndDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tdpStartDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            lblUnitT.Text = ""
            tbWorkDay.Text = "0"
            tbcapacity.Text = "0"
            'tdpStartDate.SelectedDate = Nothing
            'tdpEndDate.SelectedDate = Nothing
            tbperson.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbMaterial.Text = ""
            tbMaterialName.Text = ""
            tbQtyM.Text = "0"
            tbUnit.Text = "0"
            tbDosis.Text = "0"

        Catch ex As Exception
            Throw New Exception("Clear Dt 4 Error " + ex.ToString)
        End Try
    End Sub



    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbTransName.Text = "" Then
                lbStatus.Text = MessageDlg("Planning Name must have value")
                tbTransName.Focus()
                Return False
            End If
            If ddlDivisi.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Divisi must have value")
                ddlDivisi.Focus()
                Return False
            End If

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If CFloat(tbWorkHour.Text) <= 0 Then
            '    tbWorkHour.Text = MessageDlg("Work Hour must have value")
            '    tbWorkHour.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Block").ToString = "" Then
                    lbStatus.Text = MessageDlg("Block Must Have Value")
                    Return False
                End If
                If LTrim(Dr("Area").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Equipment").ToString = "" Then
                    lbStatus.Text = MessageDlg("Equipment Must Have Value")
                    Return False
                End If
                If Len(Dr("Qty").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Qty Equipment Must Have Value")
                    Return False
                End If

            Else
                If tbEquip.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Equipment must have value")
                    tbEquip.Focus()
                    Return False
                End If
                If tbQty.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty must have value")
                    tbQty.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt4(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Material").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                ''If Dr("StartTime").ToString = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                ''    Return False
                ''End If
                'If CFloat(Dr("Duration").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    Return False
                'End If


            Else
                If tbMaterial.Text = "" Then
                    lbStatus.Text = MessageDlg("Material must have value")
                    tbMaterial.Focus()
                    Return False
                End If
                ''If tbStartTime.Text.Trim = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time must have value")
                ''    tbStartTime.Focus()
                ''    Return False
                ''End If
                'If CFloat(tbDuration.Text.Trim) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    tbDuration.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Dim GetStringAll As String
        Try
            If Request.QueryString("ContainerId").ToString = "TrPlanLandID" Then
                GetStringAll = GetStringHd
            Else
                GetStringAll = GetStringHdMN
            End If

            Dt = BindDataTransaction(GetStringAll, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tdpStart, Dt.Rows(0)("StartDate").ToString)
            BindToDropList(ddlDivisi, Dt.Rows(0)("Divisi").ToString)
            BindToDropList(ddlTargetBy, Dt.Rows(0)("TargetType").ToString)
            BindToText(tbMasterPlan, Dt.Rows(0)("MasterPlanNo").ToString)
            BindToText(tbTransName, Dt.Rows(0)("TransName").ToString)
            BindToText(tbAreal, Dt.Rows(0)("Areal").ToString)
            BindToText(tbTargetRange, Dt.Rows(0)("TargetRange").ToString)
            BindToText(tbQtyBibitPN, Dt.Rows(0)("QtyBibitPN").ToString)
            BindToText(tbQtyTarget, Dt.Rows(0)("QtyTarget").ToString)
            BindToText(tbRemarkHD, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Block = " + QuotedStr(RRNo))
            If Dr.Length > 0 Then
                BindToText(tbBlock, Dr(0)("Block").ToString)
                BindToText(tbBlockName, Dr(0)("BlockName").ToString)
                BindToText(tbArealB, Dr(0)("Area").ToString)
                BindToText(tbqtyTargetB, Dr(0)("QtyTarget").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Equipment = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbEquip, Dr(0)("Equipment").ToString)
                BindToText(tbEquipName, Dr(0)("EquipmentName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                lblUnit.Text = Dr(0)("Unit").ToString
                BindToText(tbremarkEquip, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("JobPlant = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbJobPlant, Dr(0)("JobPlant").ToString)
                BindToText(tbJobPlantName, Dr(0)("JobPlantName").ToString)
                BindToText(tbTeam, Dr(0)("Team").ToString)
                BindToText(tbTeamName, Dr(0)("TeamName").ToString)
                BindToText(tbQtyT, CFloat(Dr(0)("Qty").ToString))
                lblUnitT.Text = Dr(0)("Unit").ToString
                BindToText(tbWorkDay, Dr(0)("WorkDay").ToString)
                BindToText(tbcapacity, CFloat(Dr(0)("Capacity").ToString).ToString)
                BindToText(tbperson, Dr(0)("Person").ToString)
                BindToDate(tdpStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tdpEndDate, Dr(0)("EndDate").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt4(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("Material = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbMaterial, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("MaterialName").ToString)
                BindToText(tbQtyM, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbDosis, Dr(0)("Dosis").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 4 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("Block = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("Block") = tbBlock.Text
                Row("BlockName") = tbBlockName.Text
                Row("Area") = tbArealB.Text.Replace(",", "")
                Row("QtyTarget") = CInt(tbqtyTargetB.Text)
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "Block", tbBlock.Text) = True Then
                    lbStatus.Text = "Block " + tbBlock.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Block") = tbBlock.Text
                dr("BlockName") = tbBlockName.Text
                dr("Area") = tbArealB.Text.Replace(",", "")
                dr("QtyTarget") = CInt(tbqtyTargetB.Text)
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            'GridDt.Columns(1).Visible = True

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If


            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("Equipment = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("Equipment") = tbEquip.Text
                Row("EquipmentName") = tbEquipName.Text
                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                Row("Unit") = lblUnit.Text
                Row("Remark") = tbremarkEquip.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("Equipment") = tbEquip.Text
                dr("EquipmentName") = tbEquipName.Text
                dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                dr("Unit") = lblUnit.Text
                dr("Remark") = tbremarkEquip.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub





    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd

            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr(ViewState("StrType"), "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLPlanLandHd (TransNmbr, TransDate, Status, MasterPlanNo, TransName, Divisi, " + _
                "StartDate, TargetType, Areal, TargetRange, QtyBibitPN, QtyTarget, LevelPlant, Regenerate, Remark,  " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbMasterPlan.Text) + "," + QuotedStr(tbTransName.Text) + "," + _
                QuotedStr(ddlDivisi.SelectedValue) + "," + QuotedStr(Format(tdpStart.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(ddlTargetBy.SelectedValue) + ", " + tbAreal.Text.Replace(", ", "") + "," + tbTargetRange.Text.Replace(", ", "") + "," + _
                tbQtyBibitPN.Text.Replace(", ", "") + "," + tbQtyTarget.Text.Replace(", ", "") + "," + QuotedStr(ViewState("LevelPlant")) + ", 'Re Generate Schedule' ," + _
                QuotedStr(tbRemarkHD.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLPlanLandHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLPlanLandHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", MasterPlanNo = " + QuotedStr(tbMasterPlan.Text) + ", TransName = " + QuotedStr(tbTransName.Text) + _
                ", Divisi = " + QuotedStr(ddlDivisi.SelectedValue) + ", StartDate = " + QuotedStr(Format(tdpStart.SelectedValue, "yyyy-MM-dd")) + _
                ", TargetType = " + QuotedStr(ddlTargetBy.SelectedValue) + ", Areal = " + tbAreal.Text.Replace(", ", "") + ", TargetRange =" + tbTargetRange.Text.Replace(", ", "") + _
                ", Remark = " + QuotedStr(tbRemarkHD.Text) + ", QtyTarget = " + tbQtyTarget.Text.Replace(", ", "") + _
                ", Regenerate = 'Re Generate Schedule' " + _
                ", QtyBibitPN = " + tbQtyBibitPN.Text.Replace(", ", "") + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Block, Area, QtyTarget FROM PLPlanLandBlock WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLPlanLandBlock SET Block = @Block, Area = @Area, QtyTarget = @QtyTarget " + _
                    " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Block = @OldBlock  ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Block", SqlDbType.VarChar, 5, "Block")
            Update_Command.Parameters.Add("@Area", SqlDbType.Float, 18, "Area")
            Update_Command.Parameters.Add("@QtyTarget", SqlDbType.Int, 4, "QtyTarget")


            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldBlock", SqlDbType.VarChar, 5, "Block")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLPlanLandBlock WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Block = @Block", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Block", SqlDbType.VarChar, 5, "Block")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLPlanLandBlock")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, Equipment, Qty, Unit, Remark FROM PLPlanLandDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
                    "UPDATE PLPlanLandDt SET Equipment = @Equipment, Qty = @Qty, Unit = @Unit, Remark = @Remark  " + _
                    " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Equipment = @OldEquipment ", con)
            ' Define output parameters.
            Update_Command2.Parameters.Add("@Equipment", SqlDbType.VarChar, 20, "Equipment")
            Update_Command2.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command2.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")

            ' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldEquipment", SqlDbType.VarChar, 20, "Equipment")
            param2.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM PLPlanLandDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Equipment = @Equipment", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@Equipment", SqlDbType.VarChar, 20, "Equipment")
            param2.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("PLPlanLandDt")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, JobPlant, Team, StartDate,EndDate, Qty, Unit, WorkDay, Capacity, Person FROM PLPlanLandJob WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param3 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command3 = New SqlCommand( _
                    "UPDATE PLPlanLandJob SET JobPlant = @JobPlant, Team = @Team, @Qty = @Qty, StartDate = @StartDate, EndDate = @EndDate, Unit = @Unit, WorkDay = @WorkDay, " + _
                    "Capacity = @Capacity, Person = @Person " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @OldJobPlant ", con)

            ' Define output parameters.
            Update_Command3.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
            Update_Command3.Parameters.Add("@Team", SqlDbType.VarChar, 5, "Team")
            Update_Command3.Parameters.Add("@StartDate", SqlDbType.DateTime, 8, "StartDate")
            Update_Command3.Parameters.Add("@EndDate", SqlDbType.DateTime, 8, "EndDate")
            Update_Command3.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command3.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command3.Parameters.Add("@WorkDay", SqlDbType.Float, 18, "WorkDay")
            Update_Command3.Parameters.Add("@Capacity", SqlDbType.Float, 18, "Capacity")
            Update_Command3.Parameters.Add("@Person", SqlDbType.Float, 18, "Person")

            ' Define intput (WHERE) parameters.
            param3 = Update_Command3.Parameters.Add("@OldJobPlant", SqlDbType.VarChar, 5, "JobPlant")
            param3.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command3

            ' Create the DeleteCommand.
            Dim Delete_Command3 = New SqlCommand( _
                "DELETE FROM PLPlanLandJob WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @JobPlant", con)
            ' Add the parameters for the DeleteCommand.
            param3 = Delete_Command3.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
            param3.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command3

            Dim Dt3 As New DataTable("PLPlanLandJob")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            Dim SQLStringProcess As String
            If Not ViewState("JobPlant") Is Nothing Then
                SQLStringProcess = "SELECT TransNmbr, JobPlant, Material, Qty, Unit FROM PLPlanLandJobDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr"))
                cmdSql = New SqlCommand(SQLStringProcess, con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand

                'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim param4 As SqlParameter
                ' Create the UpdateCommand.
                Dim Update_Command4 = New SqlCommand( _
                        "UPDATE PLPlanLandJobDt SET JobPlant = @JobPlant, Material = @Material, @Qty = @Qty, Unit = @Unit " + _
                        " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @OldJobPlant AND Material = @OldMaterial ", con)
                ' Define output parameters.
                Update_Command4.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
                Update_Command4.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
                Update_Command4.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
                Update_Command4.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")

                ' Define intput (WHERE) parameters.
                param4 = Update_Command4.Parameters.Add("@OldJobPlant", SqlDbType.VarChar, 5, "JobPlant")
                param4 = Update_Command4.Parameters.Add("@OldMaterial", SqlDbType.VarChar, 20, "Material")
                param4.SourceVersion = DataRowVersion.Original
                ' Attach the update command to the DataAdapter.
                da.UpdateCommand = Update_Command4

                ' Create the DeleteCommand.
                Dim Delete_Command4 = New SqlCommand( _
                    "DELETE FROM PLPlanLandJobDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND JobPlant = @JobPlant AND Material = @Material ", con)
                ' Add the parameters for the DeleteCommand.
                param4 = Delete_Command4.Parameters.Add("@JobPlant", SqlDbType.VarChar, 5, "JobPlant")
                param4 = Delete_Command4.Parameters.Add("@Material", SqlDbType.VarChar, 20, "Material")
                param4.SourceVersion = DataRowVersion.Original
                da.DeleteCommand = Delete_Command4

                Dim Dt4 As New DataTable("PLPlanLandJobDt")

                Dt4 = ViewState("Dt4")
                da.Update(Dt4)
                Dt4.AcceptChanges()
                ViewState("Dt4") = Dt4
            End If

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Item' must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Material Waste' must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Machine Down Time' must have at least 1 record")
            '    Exit Sub
            'End If
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            GridDt.Columns(1).Visible = True
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            tbTransName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbBlock.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbEquip.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            BindDataDt4("")

            EnableHd(True)

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "Trasn No, Trasb Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub


    Protected Sub GridDt3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt3.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt3_Click(Nothing, Nothing)
            ElseIf e.CommandName = "DetailMaterial" Then

                Dim GVR As GridViewRow
                GVR = GridDt3.Rows(Convert.ToInt32(e.CommandArgument))
                lblJobplanCode.Text = GVR.Cells(2).Text
                lblJobPlanNameDt.Text = " - " + GVR.Cells(3).Text
                ViewState("FormulaNo") = GVR.Cells(6).Text
                ViewState("JobPlant") = GVR.Cells(2).Text
                lblJobplanCode.Text = ViewState("JobPlant")


                If GVR.Cells(2).Text = "&nbsp;" Then
                    lbStatus.Text = "JobPlant " + lblJobplanCode.Text + " - " + lblJobPlanNameDt.Text + " not exists have Detail"
                    Exit Sub
                End If

                If ViewState("StateHd") = "View" Then
                    btnAddDt.Visible = False
                    btnAddDt2.Visible = False
                    'btnGetDt.Visible = False
                Else
                    btnAddDt.Visible = True
                    btnAddDt2.Visible = True
                    'btnGetDt3.Visible = True
                End If

                MultiView1.ActiveViewIndex = 3

                Dim drow As DataRow()
                If ViewState("Dt") Is Nothing Then
                    BindDataDt(ViewState("Reference"))
                    'Else
                    '    drow = ViewState("Dt3").Select("JobPlan" + QuotedStr(lblJobplanCode.Text.Trim))
                    '    If drow.Length > 0 Then
                    '        BindGridDt(drow.CopyToDataTable, GridDt3)
                    '        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    '    Else
                    '        Dim DtTemp As DataTable
                    '        DtTemp = ViewState("Dt3").Clone
                    '        DtTemp.Rows.Add(DtTemp.NewRow())
                    '        GridDt4.DataSource = DtTemp
                    '        GridDt4.DataBind()
                    '        GridDt4.Columns(0).Visible = False
                    '    End If
                End If
                btnSaveAll.Visible = False
                btnSaveTrans.Visible = False
                btnBack.Visible = False

                btnHome.Visible = False
                'totalingDt()
            End If
        Catch ex As Exception
            lbStatus.Text = "GridProcess_RowCommand Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub btnBackDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt.Click, btnBackDt2.Click
        MultiView1.ActiveViewIndex = 2
        btnSaveAll.Visible = Not (ViewState("StateHd") = "View")
        btnSaveTrans.Visible = Not (ViewState("StateHd") = "View")
        btnBack.Visible = Not (ViewState("StateHd") = "View")
        btnHome.Visible = (ViewState("StateHd") = "View")
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt.Click, btnBackDt2.Click
        MultiView1.ActiveViewIndex = 2
        btnSaveAll.Visible = Not (ViewState("StateHd") = "View")
        btnSaveTrans.Visible = Not (ViewState("StateHd") = "View")
        btnBack.Visible = Not (ViewState("StateHd") = "View")
        btnHome.Visible = (ViewState("StateHd") = "View")
    End Sub


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim CekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    BindDataDt4(ViewState("TransNmbr"))

                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, PnlDt4, GridDt4)

                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    GridDt.Columns(1).Visible = True
                    btnSchedule.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        Dim Division As String
                        Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))

                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        BindDataDt4(ViewState("TransNmbr"))

                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        'GridDt.Columns(1).Visible = True
                        btnSchedule.Visible = True
                        btnSchedule.Enabled = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PLFormPlanLand " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPLPlanLand.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
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
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            Dim GVR As GridViewRow
            If e.CommandName = "View" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                '  lbWODt2.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Block = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewA" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewE" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 2

                Dim drow As DataRow()
                If ViewState("Dt3") Is Nothing Then
                    BindDataDt3(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt3").Select("JobPlant = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        GridDt3.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewM" Then

                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 3

                Dim drow As DataRow()
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("Material = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt4)
                        GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt4").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt4.DataSource = DtTemp
                        GridDt4.DataBind()
                        GridDt4.Columns(0).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Dim TQtyWO As Double = 0
    Dim TQLuas As Double = 0
    Dim MaxItem As Integer = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    MaxItem = MaxItem + 1
                    TQtyWO = GetTotalSum(ViewState("Dt"), "QtyTarget")
                    TQLuas = GetTotalSum(ViewState("Dt"), "Area")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbQtyTarget.Text = TQtyWO
            tbAreal.Text = TQLuas

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try

    '    Catch ex As Exception
    '        lbStatus.Text = "GridDt_RowDataBound Error : " & ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r, drt As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("Block = " + QuotedStr(GVR.Cells(1).Text))
            For Each r In dr
                r.Delete()
            Next

            'For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
            '    drt = ViewState("Dt2").Rows(i)
            '    If Not drt.RowState = DataRowState.Deleted Then
            '        drt.Delete()
            '    End If
            'Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TQty As Double = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Equipment")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQty = GetTotalSum(ViewState("Dt2"), "Qty")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbQty.Text = TQty

        Catch ex As Exception
            lbStatus.Text = "GridDt2_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            'dr = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(1).Text))
            'If dr.Length > 0 Then
            '    BindGridDt(dr.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Block' must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            'tbEquip.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If
            'JobPlant, Team, StartDate,EndDate, Qty, Unit, WorkDay, Capacity, Person
            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("JobPlant = " + QuotedStr(ViewState("Dt3Value")))(0)
                Row.BeginEdit()
                Row("JobPlant") = tbJobPlant.Text
                Row("JobPlantName") = tbJobPlantName.Text
                Row("Team") = tbTeam.Text
                Row("TeamName") = tbTeamName.Text
                Row("Qty") = tbQtyT.Text
                Row("Unit") = lblUnitT.Text
                Row("WorkDay") = tbWorkDay.Text
                Row("Capacity") = tbcapacity.Text
                Row("Person") = tbperson.Text
                Row("StartDate") = tdpStartDate.SelectedDate
                Row("EndDate") = tdpEndDate.SelectedDate
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("JobPlant") = tbJobPlant.Text
                dr("JobPlantName") = tbJobPlantName.Text
                dr("Team") = tbTeam.Text
                dr("TeamName") = tbTeamName.Text
                dr("Qty") = tbQtyT.Text
                dr("Unit") = lblUnitT.Text
                dr("WorkDay") = tbWorkDay.Text
                dr("Capacity") = tbcapacity.Text
                dr("Person") = tbperson.Text
                dr("StartDate") = tdpStartDate.SelectedDate
                dr("EndDate") = tdpEndDate.SelectedDate

                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt3"), GridDt3)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("JobPlant").ToString = "" Then
                    lbStatus.Text = MessageDlg("Job Plant Must Have Value")
                    Return False
                End If
                If Dr("JobPlantName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Job Plant Name Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty JobPlant Must Have Value")
                    Return False
                End If

            Else
                If tbJobPlant.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("JobPlant Must Have Value")
                    tbJobPlantName.Focus()
                    Return False
                End If
                If CFloat(tbQtyT.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQtyT.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Dim TQtyB As Double = 0
    Protected Sub GridDt3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt3.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "JobPlant")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQtyB = GetTotalSum(ViewState("Dt3"), "Qty")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbQtyJobPlant.Text = TQtyB


        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("JobPlant = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(2).Text)
            ViewState("Dt3Value") = GVR.Cells(2).Text
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBlock.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT BlockCode, BlockName, Area, MaxCap, QtyUse, Division From V_PlanLandGetBlock WHERE Division = " + QuotedStr(ddlDivisi.SelectedValue)
            ResultField = "BlockCode, BlockName, Area, MaxCap"
            ViewState("Sender") = "btnBlock"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnBlock_Click Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt4.Click
        Try
            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If
            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt4").Select("Material = " + QuotedStr(ViewState("Dt4Value")))(0)
                Row.BeginEdit()
                Row("JobPlant") = lblJobplanCode.Text
                Row("Material") = tbMaterial.Text
                Row("MaterialName") = tbMaterialName.Text
                Row("Qty") = FormatFloat(tbQtyM.Text, ViewState("DigitQty"))
                Row("Unit") = tbUnit.Text
                Row("Dosis") = FormatFloat(tbDosis.Text, ViewState("DigitQty"))
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("JobPlant") = lblJobplanCode.Text
                dr("Material") = tbMaterial.Text
                dr("MaterialName") = tbMaterialName.Text
                dr("Qty") = tbQtyM.Text
                dr("Unit") = tbUnit.Text
                dr("Dosis") = FormatFloat(tbDosis.Text, ViewState("DigitQty"))
                ViewState("Dt4").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt4"), GridDt4)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt4 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Dim TQtyA As Double = 0
    Protected Sub GridDt4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt4.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Material")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQtyA = GetTotalSum(ViewState("Dt4"), "QtyUse")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbBooking.Text = TQtyA

        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("Material = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            FillTextBoxDt4(GVR.Cells(1).Text)
            ViewState("Dt4Value") = GVR.Cells(1).Text
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            EnableDt4(False)
        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt4.Click, btnAddDt4ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            StatusButtonSave(False)
            EnableDt4(True)
            tbMaterial.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt4.Click
        Try
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text, GVR.Cells(1).Text)
            ViewState("DtValue") = GVR.Cells(1).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
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
                    '   btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        '  CheckAllDt(GridRollOutput, sender)
    End Sub

    Protected Sub tbBlock_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBlock.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select BlockCode, BlockName, Area, MaxCap, QtyUse, Division From V_PlanLandGetBlock Where BlockCode= " + QuotedStr(tbBlock.Text) + " And Division = " + QuotedStr(ddlDivisi.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbBlock.Text = Dr("BlockCode")
                tbBlockName.Text = Dr("BlockName")
                tbArealB.Text = Dr("Area")
                tbqtyTargetB.Text = Dr("MaxCap")
                tbqtyTargetB.Focus()
            Else
                tbBlock.Text = ""
                tbBlockName.Text = ""
                tbArealB.Text = "0"
                tbqtyTargetB.Text = "0"
                btnBlock.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbBlock_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTeam.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select TeamCode, TeamName From V_MsTeamJob Where Division = " + QuotedStr(ddlDivisi.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbTeam.Text = Dr("TeamCode")
                tbTeamName.Text = Dr("TeamName")
                btnTeam.Focus()
            Else
                tbTeam.Text = ""
                tbTeamName.Text = ""
                tbTeam.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbJobPlant_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    

    Protected Sub tbEquip_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEquip.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Equipment, Equipment_Name FROM V_MsEquipment Where Equipment= " + QuotedStr(tbEquip.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbEquip.Text = Dr("Equipment")
                tbEquipName.Text = Dr("Equipment_Name")
                btnEquip.Focus()
            Else
                tbEquip.Text = ""
                tbEquipName.Text = ""
                btnEquip.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbEquip_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMasterPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMasterPlan.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr,EffectiveDate,MasterPlanName, Varietas, LandScape,LandType,PlanType From V_PLMasterPlanHd "
            ResultField = " TransNmbr,EffectiveDate,MasterPlanName,"
            ViewState("Sender") = "btnMasterPlan"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMasterPlan_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnJobPlant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJobPlant.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Job_Code, Job_Name, Level_Plant, Job_No From V_MsJobPlant "
            ResultField = " Job_Code, Job_Name"
            ViewState("Sender") = "btnJobPlant"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMasterPlan_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnTeam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTeam.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Team_Code, Team_Name, Team_Type, Total_Member, Division From V_MsTeam "
            ResultField = " Team_Code, Team_Name"
            ViewState("Sender") = "btnTeam"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMasterPlan_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEquip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEquip.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Equipment, Equipment_Name, Qty, Unit From V_MsEquipment "
            ResultField = "Equipment, Equipment_Name, Qty, Unit"
            ViewState("Sender") = "btnEquipment"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEquipment_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Material_Code,Material_Name, Unit From V_MsMaterial "
            ResultField = "Material_Code,Material_Name, Unit"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEquipment_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSchedule.Click
        Dim SQLString, Hasil As String
        Try
            'S_PLPlanLandGetSchedule.

            SQLString = "DECLARE @A VARCHAR(5000) EXEC S_PLPlanLandGetSchedule  " + QuotedStr(tbCode.Text) + ", @A Out Select @A"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            Hasil = Replace(Hasil, "0", "")

            If Trim(Hasil) <> "" Then
                lbStatus.Text = MessageDlg(Hasil)
                btnSchedule.Enabled = True
                Exit Sub
            Else
                BindDataDt2(tbCode.Text)
                BindDataDt3(tbCode.Text)
                btnSchedule.Enabled = True
            End If
            btnSchedule.Enabled = False
        Catch ex As Exception
            lbStatus.Text = "btnSchedule_Click Error : " + ex.ToString
        End Try
    End Sub
End Class


